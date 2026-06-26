#Region "UC040102"
'===========================================================================================================
'   クラスＩＤ　　：UC040102
'   クラス名称　　：組合大会通知 - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDChk
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.Document

Public Class UC040102

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                             ' 開催登録（組合大会通知検索のみ）
    Private Const STATUS_DETAIL As Byte = 2                             ' 詳細（組合大会通知検索・一時保存文書検索）
    Private Const STATUS_UPDATE As Byte = 3                             ' 変更（組合大会通知検索のみ）
    Private Const STATUS_STOP As Byte = 4                               ' 中止（組合大会通知検索のみ）
    Private Const STATUS_DELETE As Byte = 5                             ' 削除（一時保存文書検索のみ）
    Private Const STATUS_SAME As Byte = 6                               ' 同番号開催登録（組合大会通知検索のみ）
    ' 印刷スタイル
    Private Const PRINT_STYLE_ETC As Integer = 1                        ' 1：プレ印刷以外（開催登録・変更・削除）
    Private Const PRINT_STYLE_PREPRINT As Integer = 3                   ' 3：プレ印刷
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040102              ' UC040102
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040102          ' 組合大会通知 - 詳細画面
    ' 権限
    Private strGrantReference As String = "0"                           ' 参権限照
    Private strGrantInsert As String = "0"                              ' 登録権限
    Private strGrantPrint As String = "0"                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                          ' ファイル出力権限
    ' 件名副題
    Private Const MEETING_SUBJECT_OPEN As String = "下記の通り組合大会を開催しますので、ご出席願います。"   ' 開催
    Private Const MEETING_SUBJECT_UPDATE As String = "下記の通り組合大会を変更しますので、ご出席願います。" ' 変更
#End Region

#Region "プロパティ"
    Public _bytStatus As Byte = 0                                       ' ステータス（1：開催登録, 2：詳細, 3：変更, 4：中止, 5：削除）
    Public _strKsh As String = ""                                       ' 会社コード
    Public _strPeriodId As String = ""                                  ' 期ID
    Public _strUnionMeeting As String = ""                              ' 組合大会会議番号
    Public _strApplyArea As String = ""                                 ' 申請地区区分
    Public _intUnionMeetingSeq As Integer = 0                           ' 組合大会会議SEQ
    Public _strKind As String = ""                                      ' 種別
    ' 開催登録のみ
    Public _strMeetingDate As String = ""                               ' 開催開始日付（開催登録のみ）

    ' ステータス
    Public Property bytStatus() As Byte
        Get
            Return _bytStatus
        End Get
        Set(ByVal value As Byte)
            _bytStatus = value
        End Set
    End Property

    ' 会社コード
    Public Property strKsh() As String
        Get
            Return _strKsh
        End Get
        Set(ByVal value As String)
            _strKsh = value
        End Set
    End Property

    ' 期ID
    Public Property strPeriodId() As String
        Get
            Return _strPeriodId
        End Get
        Set(ByVal value As String)
            _strPeriodId = value
        End Set
    End Property

    ' 組合大会会議番号
    Public Property strUnionMeeting() As String
        Get
            Return _strUnionMeeting
        End Get
        Set(ByVal value As String)
            _strUnionMeeting = value
        End Set
    End Property

    ' 申請地区区分
    Public Property strApplyArea() As String
        Get
            Return _strApplyArea
        End Get
        Set(ByVal value As String)
            _strApplyArea = value
        End Set
    End Property

    ' 組合大会会議SEQ
    Public Property intUnionMeetingSeq() As Integer
        Get
            Return _intUnionMeetingSeq
        End Get
        Set(ByVal value As Integer)
            _intUnionMeetingSeq = value
        End Set
    End Property

    ' 種別（開催登録のみ）
    Public Property strKind() As String
        Get
            Return _strKind
        End Get
        Set(ByVal value As String)
            _strKind = value
        End Set
    End Property

    ' 開催開始日付（開催登録のみ）
    Public Property strMeetingDate() As String
        Get
            Return _strMeetingDate
        End Get
        Set(ByVal value As String)
            _strMeetingDate = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC040102_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040102_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If Me.SetGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If Me.GetData() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールロック処理
            '-------------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT _
            Or Me.bytStatus = STATUS_UPDATE _
            Or Me.bytStatus = STATUS_STOP _
            Or Me.bytStatus = STATUS_SAME Then
                '===========================================================================
                '   開催登録・変更・中止
                '===========================================================================
                If Me.ControlRockUnLock(True) = False Then
                    Exit Sub
                End If
            ElseIf Me.bytStatus = STATUS_DETAIL Then
                '===========================================================================
                '   詳細
                '===========================================================================
                If Me.ControlRockUnLock(False) = False Then
                    Exit Sub
                End If
            End If

            ' 戻るボタンにフォーカスセット
            Me.btnBack.Focus()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertChk_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertChk.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim dtPrintMemberListInfo As DataTable = Nothing                                    ' 印刷メンバー情報
        Dim intRes As Integer = 0                                                           ' プレビュー画面押下ボタン
        Dim strNewUnionMeetingNo As String = ""                                             ' 採番した組合大会会議番号

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   入力チェック
            '-------------------------------------------------------------------------------
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   印刷メンバーリスト取得処理  
            '-------------------------------------------------------------------------------
            If Me.GetPrintMemberList(dtPrintMemberListInfo) = False OrElse dtPrintMemberListInfo Is Nothing Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   印刷プレビュー処理
            '-------------------------------------------------------------------------------
            If Me.PrintPreview(PRINT_STYLE_ETC, _
                               dtPrintMemberListInfo, _
                               intRes) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   登録完了メッセージ表示
            '-------------------------------------------------------------------------------
            If (intRes = 0) _
            Or (intRes = 1) Then
                Call CLMsg.Show("GI0015")
            End If

            '-------------------------------------------------------------------------------
            '   画面遷移処理
            '-------------------------------------------------------------------------------
            ' [登録＆印刷] or [登録のみ] 押下時、画面遷移を行う。
            If (intRes = 0) _
            Or (intRes = 1) Then
                If Me.TransitionScreen(True) = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim diaRet As DialogResult = Nothing            ' 戻りボタン判定

        Try
            ' 入力・変更内容破棄メッセージボックス表示
            diaRet = CLMsg.Show("GQ0007")

            ' 押下ボタンチェック
            If diaRet = DialogResult.No Then
                Exit Sub        ' 「いいえ」ボタン押下時、処理を抜ける
            End If

            ' 画面遷移処理
            If Me.TransitionScreen(False) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：戻るボタンクリック処理
    '   概要　：
    '   作成日：2012/02/20(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Try
            ' 画面遷移処理
            If Me.TransitionScreen(False) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：プレ印刷ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim dtPrintMemberListInfo As DataTable = Nothing
        Dim intRes As Integer = 0                            ' プレビュー画面押下ボタン
        Try
            '-------------------------------------------------------------------------------
            '   印刷メンバーリスト取得処理  
            '-------------------------------------------------------------------------------
            If Me.GetPrintMemberList(dtPrintMemberListInfo) = False OrElse dtPrintMemberListInfo Is Nothing Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   印刷プレビュー処理
            '-------------------------------------------------------------------------------
            If Me.PrintPreview(PRINT_STYLE_PREPRINT, _
                               dtPrintMemberListInfo, _
                               intRes) = False Then
                Exit Sub
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：chkMeeting2_CheckedChanged
    '   名称　：終了日時チェックボックスチェンジ処理
    '   概要　：
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub chkMeeting2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMeeting2.CheckedChanged

        Try
            '---------------------------------------------------
            '   終了日時各項目表示・非表示
            '---------------------------------------------------
            If Me.ChangeEndDateTime(Me.chkMeeting2.Checked) = False Then
                Exit Sub
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

    End Sub
#End Region

    '***************************************************************************************************
    '   ＩＤ　：ChangeEndDateTime
    '   名称　：終了日時チェンジ処理
    '   概要　：
    '   引数　：ByVal iBlnFlg As Boolean = 表示・非表示フラグ（True = 表示, False = 非表示）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/27(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/27(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>終了日時チェンジ処理</summary>
    ''' <param name="iBlnFlg">表示・非表示フラグ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeEndDateTime(ByVal iBlnFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '---------------------------------------------------
            '   各項目表示・非表示
            '---------------------------------------------------
            ' 終了日
            If iBlnFlg Then
                Me.dtpMeeting2.Value = DateAdd(DateInterval.Day, 1, Me.dtpMeeting1.Value)
            End If
            ' DateTimePicker
            Me.dtpMeeting2.Enabled = iBlnFlg                    ' 終了日時
            ' MaskedTextBox
            Me.mtbMeetingTimeFrom2.Enabled = iBlnFlg            ' 終了日時（開始時刻）
            Me.mtbMeetingTimeTo2.Enabled = iBlnFlg              ' 終了日時（終了時刻）
            Me.mtbDFlightBack2.Enabled = iBlnFlg                ' 移動フライト時間（復路）2
            ' TextBox
            Me.txtLFlightBack2.Enabled = iBlnFlg                ' 移動フライト（復路）2
            Me.txtPlace2.Enabled = iBlnFlg                      ' 会議場2
            ' Label
            Me.lblLFlightBack2.Enabled = iBlnFlg                ' ？？？？？
            Me.lblFlightBack2.Enabled = iBlnFlg                 ' 移動便名ラベル
            Me.lblHatsu3.Enabled = iBlnFlg                      ' 発ラベル
            ' CheckBox
            Me.chkLunch2.Enabled = iBlnFlg                      ' 昼食可否2
            Me.chkExchangeMeeting2.Enabled = iBlnFlg            ' 夕食交流会可否2

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値に処理結果を設定
        Return blnRet

    End Function

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            ' Title
            If Me.bytStatus = STATUS_INSERT _ 
            Or Me.bytStatus = STATUS_SAME Then
                Me.lblTitle.Text = "組合大会通知 - 新規登録"
            ElseIf Me.bytStatus = STATUS_DETAIL Then
                Me.lblTitle.Text = "組合大会通知 - 詳細"
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                Me.lblTitle.Text = "組合大会通知 - 変更"
            ElseIf Me.bytStatus = STATUS_STOP Then
                Me.lblTitle.Text = "組合大会通知 - 中止"
            End If
            ' Label
            Me.lblUnionMeetingNo.Text = ""                          ' 会議通知番号
            Me.lblCreateDate.Text = ""                              ' 登録日
            'Me.lblLFlightBack1.Text = ""                            '
            'Me.lblLFlightBack2.Text = ""                            ' 
            ' TextBox
            Me.txtPlace1.Text = ""                                  ' 会議場1
            Me.txtPlace2.Text = ""                                  ' 会議場2
            Me.txtBiko1.Text = ""                                   ' 備考
            Me.txtSubject1.Text = ""                                ' 議題
            Me.txtBiko2.Text = ""                                   ' 議題備考
            Me.txtLFlight1.Text = ""                                ' 移動1（往路）
            Me.txtLFlightBack1.Text = ""                            ' 移動1（復路）
            Me.txtLFlightBack2.Text = ""                            ' 移動2（復路）
            ' DateTimePicker
            Me.dtpMeeting1.Text = ""                                ' 会議場1
            Me.dtpMeeting2.Text = ""                                ' 会議場2
            ' MaskedTextBoxt
            Me.mtbMeetingTimeFrom1.Text = ""                        ' 開催日時From
            Me.mtbMeetingTimeTo1.Text = ""                          ' 開催日時To
            Me.mtbMeetingTimeFrom2.Text = ""                        ' 終了日時From
            Me.mtbMeetingTimeTo2.Text = ""                          ' 終了日時To
            Me.mtbDFlight1.Text = ""                                ' 移動1（往路）
            Me.mtbDFlightBack1.Text = ""                            ' 移動1（復路）
            Me.mtbDFlightBack2.Text = ""                            ' 移動2（復路）
            ' ComboBoxList
            Me.cboApplyArea.DataSource = Nothing                    ' 支部
            Me.cboOpenBelonging.DataSource = Nothing                ' 開催場所
            ' ComboBox
            Me.cboApplyArea.Text = ""                               ' 支部
            Me.cboOpenBelonging.Text = ""                           ' 開催場所
            ' Button
            Me.btnPrint.Visible = True                              ' プレ印刷ボタン
            Me.btnInsertChk.Visible = True                          ' 登録確認ボタン
            Me.btnCancel.Visible = True                             ' キャンセルボタン
            Me.btnBack.Visible = True                               ' 戻るボタン

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：ByVal pBlnEdit As Boolean = True：アンロック, False：ロック
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <param name="iBlnEdit">True：アンロック, False：ロック</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock(ByVal iBlnEdit As Boolean) As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果

        Dim blnPrint As Boolean = False                     ' プレ印刷ボタン用
        Dim blnTempSaveChk As Boolean = False               ' 一時保存確認ボタン用
        Dim blnInsertChk As Boolean = False                 ' 登録確認ボタン用
        Dim blnUpdate As Boolean = False                    ' 内容変更ボタン用
        Dim blnCancel As Boolean = False                    ' キャンセルボタン用
        Dim blnBack As Boolean = False                      ' 戻るボタン用

        Dim blnReadOnlyFlg As Boolean = False               ' ReadOnly用フラグ
        Dim blnEnabledFlg As Boolean = False                ' Enabled用フラグ
        Dim blnEndDateTimeFlg As Boolean = False            ' 終了日時関連用フラグ

        Try
            '-------------------------------------------------------------------------------
            '   表示・非表示フラグ設定
            '-------------------------------------------------------------------------------
            ' 各フラグはデフォルトでFalseを設定済み
            If Me.bytStatus = STATUS_INSERT _
            Or Me.bytStatus = STATUS_UPDATE _
            Or Me.bytStatus = STATUS_STOP _
            Or Me.bytStatus = STATUS_SAME Then
                '===========================================================================
                '   開催登録・変更・中止
                '===========================================================================
                blnTempSaveChk = True                                                       ' 一時保存確認ボタン用
                blnInsertChk = True                                                         ' 登録確認ボタン用
                blnCancel = True                                                            ' キャンセルボタン用
            ElseIf Me.bytStatus = STATUS_DETAIL Then
                '===========================================================================
                '   詳細
                '===========================================================================
                blnPrint = True                                                             ' プレ印刷ボタン用
                blnBack = True                                                              ' 戻るボタン用
            End If

            ' ReadOnly・Enabled用フラグ設定
            If iBlnEdit = False Then
                blnReadOnlyFlg = True
            Else
                blnEnabledFlg = True
            End If

            ' 終了日時関連用フラグ設定
            If (Me.chkMeeting2.Checked = True) And (iBlnEdit = True) Then
                blnEndDateTimeFlg = True
            End If

            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            ' Button
            Me.btnPrint.Visible = blnPrint                                                  ' プレ印刷ボタン
            Me.btnInsertChk.Visible = blnInsertChk                                          ' 登録確認ボタン
            Me.btnCancel.Visible = blnCancel                                                ' キャンセルボタン
            Me.btnBack.Visible = blnBack                                                    ' 戻るボタン

            '===========================================
            '   開始日時
            '===========================================
            ' DateTimePicker
            Me.dtpMeeting1.Enabled = blnEnabledFlg                                          ' 開催日時
            ' MaskedTextBox
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.mtbMeetingTimeFrom1)            ' 会議時間From1
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.mtbMeetingTimeTo1)              ' 会議時間To1
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.mtbDFlight1)                    ' 移動フライト時間（往路）1
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.mtbDFlightBack1)                ' 移動フライト時間（復路）1
            ' TextBox
            Me.txtLFlight1.ReadOnly = blnReadOnlyFlg                                        ' 移動フライト（往路）1
            Me.txtLFlightBack1.ReadOnly = blnReadOnlyFlg                                    ' 移動フライト（復路）1
            Me.txtPlace1.ReadOnly = blnReadOnlyFlg                                          ' 会議場1
            ' CheckBox
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.chkLunch1)                      ' 昼食可否1
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.chkExchangeMeeting1)            ' 夕食交流会可否1

            '===========================================
            '   終了日時
            '===========================================
            ' DateTimePicker
            Me.dtpMeeting2.Value = DateAdd(DateInterval.Day, 1, Me.dtpMeeting1.Value)
            Me.dtpMeeting2.Enabled = blnEndDateTimeFlg                                      ' 終了日時
            ' MaskedTextBox
            Me.mtbMeetingTimeFrom2.Enabled = blnEndDateTimeFlg                              ' 終了日時（開始時刻）
            Me.mtbMeetingTimeTo2.Enabled = blnEndDateTimeFlg                                ' 終了日時（終了時刻）
            Me.mtbDFlightBack2.Enabled = blnEndDateTimeFlg                                  ' 移動フライト時間（復路）2
            ' TextBox
            Me.txtLFlightBack2.Enabled = blnEndDateTimeFlg                                  ' 移動フライト（復路）2
            Me.txtPlace2.Enabled = blnEndDateTimeFlg                                        ' 会議場2
            ' Label
            Me.lblLFlightBack2.Enabled = blnEndDateTimeFlg                                  ' ？？？？？
            Me.lblFlightBack2.Enabled = blnEndDateTimeFlg                                   ' 移動便名ラベル
            Me.lblHatsu3.Enabled = blnEndDateTimeFlg                                        ' 発ラベル
            ' CheckBox
            Me.chkMeeting2.Enabled = iBlnEdit                                               ' 会議場所2
            Me.chkLunch2.Enabled = blnEndDateTimeFlg                                        ' 昼食可否2
            Me.chkExchangeMeeting2.Enabled = blnEndDateTimeFlg                              ' 夕食交流会可否2

            '===========================================
            '   全体
            '===========================================
            ' ComboBox
            Call Utilities.SetCanEditToControl(False, Me.cboApplyArea)                      ' 支部
            Call Utilities.SetCanEditToControl(iBlnEdit, Me.cboOpenBelonging)               ' 開催場所
            ' OptionButton
            Dim blnUnionType As Boolean = False
            If Me.bytStatus = STATUS_UPDATE Then
                If Me.strKind = UI_CIR_KIND_JOIN _
                Or Me.strKind = UI_CIR_KIND_TV Then
                    ' 「変更」で種別が「合同」/「TV」の時だけ変更可能
                    blnUnionType = True
                End If
            End If
            Call Utilities.SetCanEditToControl(blnUnionType, Me.optUnionType1)              ' 種別（合同）
            Call Utilities.SetCanEditToControl(blnUnionType, Me.optUnionType2)              ' 種別（TV）
            Call Utilities.SetCanEditToControl(False, Me.optUnionType3)                     ' 種別（任意）
            Call Utilities.SetCanEditToControl(False, Me.optInfomationName1)                ' 種類（開催）
            Call Utilities.SetCanEditToControl(False, Me.optInfomationName2)                ' 種類（変更）
            Call Utilities.SetCanEditToControl(False, Me.optInfomationName3)                ' 種類（中止）
            ' TextBox
            Me.txtBiko1.ReadOnly = blnReadOnlyFlg                                           ' 備考
            Me.txtSubject1.ReadOnly = blnReadOnlyFlg                                        ' 議題
            Me.txtBiko2.ReadOnly = blnReadOnlyFlg                                           ' 議題備考

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As New CLAccessMdb                    ' データベースクラス生成
        Dim strSql As String = ""                       ' SQL

        Try
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            ' データベース接続
            Call clsDb.Connect()

            '---------------------------------------------------------------------------
            '   コンボボックス作成
            '---------------------------------------------------------------------------
            ' 定数マスタ詳細（支部）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboApplyArea, _
                                    CONSTANT_ID_APPLY_AREA, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（開催場所）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboOpenBelonging, _
                                    CONSTANT_ID_AREA_LOCAL, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWN, _
                                    -1) = False Then
                Return blnRet
            End If
            '---------------------------------------------------------------------------
            '   組合大会通知情報
            '---------------------------------------------------------------------------            
            ' 組合大会通知情報取得処理
            If GetUnionInformation(clsDb) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetUnionInformation
    '   名称　：組合大会通知情報取得処理
    '   概要  ：組合大会通知情報をを取得する。
    '   引数　：ByVal pClsDb        As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetUnionInformation(ByVal iClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim tbRet As DataTable                  ' 処理結果データテーブル
        Dim intRet As Integer = 0               ' 処理件数

        Try
            If Me.bytStatus = STATUS_INSERT Then
                '=======================================================================
                '   開催登録
                '=======================================================================
                ' 会議通知番号
                Me.lblUnionMeetingNo.Text = "*****"
                ' 登録日
                Me.lblCreateDate.Text = Now.ToString("yyyy/MM/dd")
                ' 地区（04. 申請地区区分）
                Me.cboApplyArea.SelectedValue = Me.strApplyArea
                ' 開催開始日付（08. 会議日付1）
                If Me.strMeetingDate.Length <> 0 Then
                    Me.dtpMeeting1.Value = Date.Parse(Format(CInt(Me.strMeetingDate), "0000/00/00"))
                Else
                    Me.dtpMeeting1.Value = Now
                End If
                ' 種別（06. 種類）
                If Me.strKind = UI_CIR_KIND_JOIN Then
                    Me.optUnionType1.Checked = True         ' 合同
                ElseIf Me.strKind = UI_CIR_KIND_TV Then
                    Me.optUnionType2.Checked = True         ' TV
                ElseIf Me.strKind = UI_CIR_KIND_ANY Then
                    Me.optUnionType3.Checked = True         ' 任意
                End If
                ' 種類
                Me.optInfomationName1.Checked = True        ' 開催
                ' 昼食可否
                Me.chkLunch1.Checked = True                 ' チェック有り
                ' 夕食交流会可否
                Me.chkExchangeMeeting1.Checked = True       ' チェック有り
                ' 議題
                If Me.strKind = UI_CIR_KIND_JOIN _
                Or Me.strKind = UI_CIR_KIND_TV Then
                    ' 合同・TV
                    Me.txtSubject1.Text = "(1)　一般経過報告" & vbCrLf & _
                                          "(2)　年末闘争方針" & vbCrLf & _
                                          "(3)　ストライキ開始の決定" & vbCrLf & _
                                          "(4)　その他"
                ElseIf Me.strKind = UI_CIR_KIND_ANY Then
                    ' 任意
                    Me.txtSubject1.Text = "(1)　一般経過報告" & vbCrLf & _
                                          "(2)　春闘、重点課題等に関する中執方針" & vbCrLf & _
                                          "(3)　要求の決定" & vbCrLf & _
                                          "(4)　労働協約の締結　　　　(航空機関士の職場安定関連等)" & vbCrLf & _
                                          "(5)　争議の終結　　　　　　(航空機関士の職場安定要求に関する争議)" & vbCrLf & _
                                          "(6)　労働協約の締結　　　　(貨物深夜便関連)" & vbCrLf & _
                                          "(7)　労働協約の締結　　　　(神戸空港関湾出退関連)" & vbCrLf & _
                                          "(8)　その他"
                End If

                ' 議題備考
                Dim biko2Date As DateTime = DateAdd(DateInterval.Day, -5, Me.dtpMeeting1.Value)
                Me.txtBiko2.Text = "◎出欠に関わらず、早めの連絡をお願いします。(提出先：組合事務所大会出欠ＢＯＸ)" & vbCrLf & _
                                   "◎返事を書面でできない場合には組合事務所までＴＥＬ、ＦＡＸで必ずご連絡ください。" & vbCrLf & _
                                   "◎指名ストライキ受付期限は、" & biko2Date.ToString("MM") & " 月 " & biko2Date.ToString("dd") & " 日 17時とします。" & vbCrLf & _
                                   "◎大阪居住の代議員等、宿泊の希望もご連絡下さい。"

            ElseIf Me.bytStatus = STATUS_DETAIL _
            Or Me.bytStatus = STATUS_UPDATE _
            Or Me.bytStatus = STATUS_STOP _
            Or Me.bytStatus = STATUS_SAME Then
                '=======================================================================
                '   詳細・変更・中止
                '=======================================================================
                ' SQL文作成
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT a.c_ksh                 AS c_ksh"                    ' 01. 会社コード
                strSql = strSql & "       ,a.c_period_id           AS c_period_id"              ' 02. 期 ID
                strSql = strSql & "       ,a.c_union_meeting       AS c_union_meeting"          ' 03. 組合大会会議番号
                strSql = strSql & "       ,a.k_apply_area          AS k_apply_area"             ' 04. 申請地区区分
                strSql = strSql & "       ,a.s_union_meeting_seq   AS s_union_meeting_seq"      ' 05. 組合大会会議SEQ
                strSql = strSql & "       ,a.k_information_type    AS k_information_type"       ' 06. 種類
                strSql = strSql & "       ,a.l_information_name    AS l_information_name"       ' 07. 会議名（目的）
                strSql = strSql & "       ,a.d_meeting_1           AS d_meeting_1"              ' 08. 会議日付1
                strSql = strSql & "       ,a.d_meeting_time_from_1 AS d_meeting_time_from_1"    ' 09. 会議時間From1
                strSql = strSql & "       ,a.d_meeting_time_to_1   AS d_meeting_time_to_1"      ' 10. 会議時間To1
                strSql = strSql & "       ,a.l_place_1             AS l_place_1"                ' 11. 会議場所1
                strSql = strSql & "       ,a.l_flight_1            AS l_flight_1"               ' 12. 移動フライト（往路）1
                strSql = strSql & "       ,a.d_flight_1            AS d_flight_1"               ' 13. 移動フライト時間（往路）1
                strSql = strSql & "       ,a.l_flight_back_1       AS l_flight_back_1"          ' 14. 移動フライト（復路）1
                strSql = strSql & "       ,a.d_flight_back_1       AS d_flight_back_1"          ' 15. 移動フライト時間（復路）1
                strSql = strSql & "       ,a.k_lunch_1             AS k_lunch_1"                ' 16. 昼食可否1
                strSql = strSql & "       ,a.k_exchange_meeting_1  AS k_exchange_meeting_1"     ' 17. 夕食交流会可否1
                strSql = strSql & "       ,a.d_meeting_2           AS d_meeting_2"              ' 18. 会議日付2
                strSql = strSql & "       ,a.d_meeting_time_from_2 AS d_meeting_time_from_2"    ' 19. 会議時間From2
                strSql = strSql & "       ,a.d_meeting_time_to_2   AS d_meeting_time_to_2"      ' 20. 会議時間To2
                strSql = strSql & "       ,a.l_place_2             AS l_place_2"                ' 21. 会議場所2
                strSql = strSql & "       ,a.l_flight_2            AS l_flight_2"               ' 22. 移動フライト（往路）2
                strSql = strSql & "       ,a.d_flight_2            AS d_flight_2"               ' 23. 移動フライト時間（往路）2
                strSql = strSql & "       ,a.l_flight_back_2       AS l_flight_back_2"          ' 24. 移動フライト（復路）2
                strSql = strSql & "       ,a.d_flight_back_2       AS d_flight_back_2"          ' 25. 移動フライト時間（復路）2
                strSql = strSql & "       ,a.k_lunch_2             AS k_lunch_2"                ' 26. 昼食可否2
                strSql = strSql & "       ,a.k_exchange_meeting_2  AS k_exchange_meeting_2"     ' 27. 夕食交流会可否2
                strSql = strSql & "       ,a.l_open_bebiginting    AS l_open_bebiginting"       ' 28. 開催場所（所属支部区分）
                strSql = strSql & "       ,a.l_subject_1           AS l_subject_1"              ' 29. 議題1
                strSql = strSql & "       ,a.l_subject_2           AS l_subject_2"              ' 30. 議題2
                strSql = strSql & "       ,a.l_biko_1              AS l_biko_1"                 ' 31. 備考1
                strSql = strSql & "       ,a.l_biko_2              AS l_biko_2"                 ' 32. 備考2
                strSql = strSql & "       ,a.l_biko_3              AS l_biko_3"                 ' 33. 備考3
                strSql = strSql & "       ,a.d_ins                 AS d_ins"                    ' 34. 作成日
                strSql = strSql & "       ,a.c_user_id_ins         AS c_user_id_ins"            ' 35. 作成者個人ID
                strSql = strSql & "       ,a.d_up                  AS d_up"                     ' 36. 更新日
                strSql = strSql & "       ,a.c_user_id_up          AS c_user_id_up"             ' 37. 更新者個人ID
                strSql = strSql & "       ,a.s_up                  AS s_up"                     ' 38. 更新回数
                strSql = strSql & "   FROM union_information AS a" & vbCrLf                     ' 組合大会通知情報
                strSql = strSql & "  WHERE a.c_ksh           = '" & Me.strKsh & "'" & vbCrLf            ' 会社コードと同じもの
                strSql = strSql & "    AND a.c_period_id     = '" & Me.strPeriodId & "'" & vbCrLf       ' 期IDと同じもの
                strSql = strSql & "    AND a.c_union_meeting = '" & Me.strUnionMeeting & "'" & vbCrLf   ' 組合大会会議番号と同じもの
                If Me.bytStatus = STATUS_SAME Then
                    strSql = strSql & "    AND a.k_apply_area   <> '" & Me.strApplyArea & "'" & vbCrLf  ' 申請地区区分と異なるもの
                Else
                    strSql = strSql & "    AND a.k_apply_area    = '" & Me.strApplyArea & "'" & vbCrLf  ' 申請地区区分と同じもの
                End If
                strSql = strSql & ";" & vbCrLf
                '-----------------------------------------------------------------------
                '   各データ設定
                '-----------------------------------------------------------------------
                ' SQL実行
                tbRet = iClsDb.ExecuteSql(strSql)
                ' 処理件数取得
                intRet = tbRet.Rows.Count
                If intRet = 1 Then
                    With tbRet.Rows(0)
                        ' 会議通知番号
                        Me.lblUnionMeetingNo.Text = NVL(.Item(2).ToString())
                        ' 登録日（36. 更新日が有れば更新日、無ければ34. 作成日を表示）
                        If MDChk.ChkNull(.Item(35).ToString()) Then
                            If NSMDChk.ChkNull(.Item(33).ToString()) Then
                                Me.lblCreateDate.Text = ""
                            Else
                                Me.lblCreateDate.Text = CDate(.Item(33)).ToString("yyyy/MM/dd")
                            End If
                        Else
                            Me.lblCreateDate.Text = CDate(.Item(35)).ToString("yyyy/MM/dd")
                        End If
                        If Me.bytStatus = STATUS_SAME Then
                            ' 支部（04. 申請地区区分）
                            Me.cboApplyArea.SelectedValue = Me.strApplyArea
                        Else
                            ' 支部（04. 申請地区区分）
                            Me.cboApplyArea.SelectedValue = NVL(.Item(3).ToString())
                        End If
                        ' 05. 組合大会会議SEQ
                        Me.intUnionMeetingSeq = Integer.Parse(NVL(.Item(4).ToString()))
                        ' 種別（種別）
                        If .Item(5).ToString() = UI_CIR_KIND_JOIN Then
                            Me.optUnionType1.Checked = True
                            Me.strKind = UI_CIR_KIND_JOIN
                        ElseIf .Item(5).ToString() = UI_CIR_KIND_TV Then
                            Me.optUnionType2.Checked = True
                            Me.strKind = UI_CIR_KIND_TV
                        ElseIf .Item(5).ToString() = UI_CIR_KIND_ANY Then
                            Me.optUnionType3.Checked = True
                            Me.strKind = UI_CIR_KIND_ANY
                        End If
                        ' 種類（06. 種類）
                        If Me.bytStatus = STATUS_INSERT Or Me.bytStatus = STATUS_SAME Then
                            ' 開催登録ボタン押下時、開催ラジオボタンチェック
                            Me.optInfomationName1.Checked = True
                        ElseIf Me.bytStatus = STATUS_DETAIL Then
                            ' 詳細ボタン押下時、各情報表示
                            If .Item(6).ToString() = STR_INFORMATION_NAME_OPEN Then
                                ' 開催の場合、開催ラジオボタンチェック
                                Me.optInfomationName1.Checked = True
                            ElseIf .Item(6).ToString() = STR_INFORMATION_NAME_UPDATE Then
                                ' 変更の場合、変更ラジオボタンチェック
                                Me.optInfomationName2.Checked = True
                            ElseIf .Item(6).ToString() = STR_INFORMATION_NAME_STOP Then
                                ' 中止の場合、中止ラジオボタンチェック
                                Me.optInfomationName3.Checked = True
                            End If
                        ElseIf Me.bytStatus = STATUS_UPDATE Then
                            ' 変更ボタン押下時、変更ラジオボタンチェック
                            Me.optInfomationName2.Checked = True
                        ElseIf Me.bytStatus = STATUS_STOP Then
                            ' 中止ボタン押下時、中止ラジオボタンチェック
                            Me.optInfomationName3.Checked = True
                        End If
                        ' 開催場所（28. 開催場所（所属支部区分））
                        'Me.cboOpenBelonging.SelectedIndex = Me.cboOpenBelonging.FindString(NVL(.Item(27)).ToString())
                        Me.cboOpenBelonging.Text = NVL(.Item(27)).ToString()
                        '===========================================
                        '   開始日時
                        '===========================================
                        ' 開催日時（08. 会議日付1）
                        Me.dtpMeeting1.Value = .Item(7)
                        ' 開催日時From（09. 会議時間From1）
                        Me.mtbMeetingTimeFrom1.Text = .Item(8).ToString()
                        ' 開催日時To（10. 会議時間To1）
                        If MDChk.ChkNull(.Item(9).ToString()) Then
                            Me.mtbMeetingTimeTo1.Text = ""
                        Else
                            Me.mtbMeetingTimeTo1.Text = .Item(9).ToString()
                        End If
                        ' 会議場1（11. 会議場所1）
                        Me.txtPlace1.Text = NVL(.Item(10).ToString())

                        '===========================================
                        '   終了日時
                        '===========================================
                        ' 会議日付2が有れば終了日時チェックボックスにチェック
                        If MDChk.ChkNull(.Item(17).ToString()) Then
                            Me.chkMeeting2.Checked = False          ' 終了日時チェックボックス
                            'Me.dtpMeeting2.Value = Nothing          ' 終了日時（18. 会議日付2）
                            Me.mtbMeetingTimeFrom2.Text = ""        ' 終了日時From（19. 会議時間From2）
                            Me.mtbMeetingTimeTo2.Text = ""          ' 終了日時To（20. 会議時間To2）
                            Me.txtPlace2.Text = ""                  ' 会議場2（21. 会議場所2）
                        Else
                            Me.chkMeeting2.Checked = True
                            ' 終了日時（18. 会議日付2）
                            If MDChk.ChkNull(Me.dtpMeeting2.Value.ToString()) = False Then
                                Me.dtpMeeting2.Value = .Item(17)
                            End If
                            ' 終了日時From（19. 会議時間From2）
                            If MDChk.ChkNull(.Item(18).ToString()) Then
                                Me.mtbMeetingTimeFrom2.Text = ""
                            Else
                                Me.mtbMeetingTimeFrom2.Text = .Item(18).ToString()
                            End If
                            ' 終了日時To（20. 会議時間To2）
                            If MDChk.ChkNull(.Item(19).ToString()) Then
                                Me.mtbMeetingTimeTo2.Text = ""
                            Else
                                Me.mtbMeetingTimeTo2.Text = .Item(19).ToString()
                            End If
                            ' 会議場2（21. 会議場所2）
                            If MDChk.ChkNull(.Item(20).ToString()) Then
                                Me.txtPlace2.Text = ""
                            Else
                                Me.txtPlace2.Text = NVL(.Item(20).ToString())
                            End If
                        End If
                        ' 備考（31. 備考1）
                        Me.txtBiko1.Text = NVL(.Item(30).ToString())
                        ' 議題（29. 議題1）
                        Me.txtSubject1.Text = NVL(.Item(28).ToString())
                        ' 議題備考（32. 備考2）
                        Me.txtBiko2.Text = NVL(.Item(31).ToString())
                        ' 昼食可否1（16. 昼食可否1）
                        Me.chkLunch1.Checked = CBool(NVL(.Item(15).ToString()))
                        ' 夕食交流会可否1（17. 夕食交流会可否1）
                        Me.chkExchangeMeeting1.Checked = CBool(NVL(.Item(16).ToString()))
                        ' 昼食可否2（26. 昼食可否2）
                        Me.chkLunch2.Checked = CBool(NVL(.Item(25).ToString()))
                        ' 夕食交流会可否2（27. 夕食交流会可否2）
                        Me.chkExchangeMeeting2.Checked = CBool(NVL(.Item(26).ToString()))
                        ' 移動1（往路）（12. 移動フライト（往路）1）
                        Me.txtLFlight1.Text = NVL(.Item(11).ToString())
                        ' 移動1（往路）時間（13. 移動フライト時間（往路）1）
                        Me.mtbDFlight1.Text = NVL(.Item(12).ToString())
                        ' 移動1（復路）（14. 移動フライト（復路）1）
                        Me.txtLFlightBack1.Text = NVL(.Item(13).ToString())
                        ' 移動1（復路）時間（15. 移動フライト時間（復路）1）
                        Me.mtbDFlightBack1.Text = NVL(.Item(14).ToString())
                        ' 移動2（復路）（24. 移動フライト（復路）2）
                        Me.txtLFlightBack2.Text = NVL(.Item(23).ToString())
                        ' 移動2（復路）時間（25. 移動フライト時間（復路）2）
                        Me.mtbDFlightBack2.Text = NVL(.Item(24).ToString())
                    End With
                Else
                    Call MessageBox.Show("データがありません！", _
                                         "エラー", _
                                         MessageBoxButtons.OK, _
                                         MessageBoxIcon.Error, _
                                         MessageBoxDefaultButton.Button1)
                    Return blnRet
                End If
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetUnionInformationMember
    '   名称　：組合大会通知メンバー情報取得処理
    '   概要  ：組合大会通知メンバー情報をを取得する。
    '   引数　：ByVal oStrStafId As String()    = 社員番号リスト
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/20(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知メンバー情報取得処理</summary>
    ''' <param name="oStrStafId">社員番号リスト</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetUnionInformationMember(ByRef oStrStafId As String()) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim intRet As Integer = 0                   ' 処理件数
        Dim dtRet As DataTable                      ' 処理結果データテーブル
        Dim strSql As String = ""                   ' SQL文
        Dim strStafId As String() = Nothing         ' 社員番号リスト
        Dim clsDb As New CLAccessMdb                ' データベースクラス

        Try
            '-------------------------------------------------------------------------------
            '   社員番号リスト取得
            '   会社コードと期IDと組合大会会議番号と同じ社員番号取得
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_user_id AS c_user_id" & vbCrLf
            strSql = strSql & "   FROM union_information_member AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_ksh           = '" & Me.strKsh & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id     = '" & Me.strPeriodId & "'" & vbCrLf
            strSql = strSql & "    AND a.c_union_meeting = '" & Me.strUnionMeeting & "'" & vbCrLf
            strSql = strSql & "    AND a.k_apply_area    = '" & Me.strApplyArea & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.c_user_id" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            '-------------------------------------------------------------------------------
            '   各データ設定
            '-------------------------------------------------------------------------------
            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet <> 0 Then
                ReDim strStafId(intRet - 1)                                                 ' 社員番号リスト再生成（レコード数分）
                For i = 0 To intRet - 1                                                     ' データ件数分ループ
                    strStafId(i) = dtRet.Rows(i).Item(0).ToString()                         ' 社員番号設定
                Next
            End If
            oStrStafId = strStafId                                                          ' アウトプット引数に社員番号リストを設定
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim arlErrMsg As New ArrayList                  ' エラーメッセージリスト
        Dim clsUC999999 As UC999999 = Nothing           ' メッセージボックスクラス生成
        Dim strChk As String = ""                       ' チェック対象文字列
        Dim daiRet As DialogResult = Nothing            ' 確認メッセージ結果
        Dim strNow As String = ""                       ' 比較用現在日付（yyyyMMdd）
        Dim strStartDate As String = ""                 ' 比較用開催日付（yyyyMMdd）

        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   未入力・未選択・日付チェック
            '-------------------------------------------------------------------------------
            ' 支部
            strChk = Me.cboApplyArea.Text.Trim()
            If MDChk.ChkNull(strChk) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "支部"))
                Call SetErr(Me.cboApplyArea)
            End If
            ' 開催場所
            strChk = Me.cboOpenBelonging.Text.Trim()
            If MDChk.ChkNull(strChk) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開催場所"))
                Call SetErr(Me.cboOpenBelonging)
            Else
                ' 全角チェック
                If MDChk.ChkZenkaku(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0112", "開催場所", "0", "5"))
                    Call SetErr(Me.cboOpenBelonging)
                Else
                    ' 全角5文字以内チェック
                    If MDChk.ChkLength(strChk, 5) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0112", "開催場所", "0", "5"))
                        Call SetErr(Me.cboOpenBelonging)
                    End If
                End If
            End If
            '---------------------------------------
            '   開始日時
            '---------------------------------------
            ' 開始日付
            strChk = Me.dtpMeeting1.Value.ToString("yyyyMMdd")
            If MDChk.ChkNull(strChk) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 年月日"))
                Call SetErr(Me.dtpMeeting1)
            Else
                If MDChk.ChkDate(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "開始日時 - 年月日"))
                    Call SetErr(Me.dtpMeeting1)
                End If
            End If
            ' 開始時刻
            strChk = MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom1.Text)
            If MDChk.ChkNull(strChk) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 開始時刻"))
                Call SetErr(Me.mtbMeetingTimeFrom1)
            Else
                If MDChk.ChkTime(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0021", "開始日時 - 開始時刻"))
                    Call SetErr(Me.mtbMeetingTimeFrom1)
                End If
            End If
            ' 終了時刻
            strChk = MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text)
            If Not MDChk.ChkNull(strChk) Then
                If MDChk.ChkTime(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0021", "開始日時 - 終了時刻"))
                    Call SetErr(Me.mtbMeetingTimeTo1)
                End If
            End If
            ' 会議場
            strChk = Me.txtPlace1.Text.Trim()
            If MDChk.ChkLength(strChk, 22) = False Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0112", "開始日時 - 会議場", "22", "22"))
                Call SetErr(Me.txtPlace1)
            End If
            ' 移動便名（往路）
            strChk = Me.txtLFlight1.Text.Trim()
            If MDChk.ChkNull(strChk) = False Then
                If MDChk.ChkHankaku(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "開始日時 - 移動便名（往路）"))
                    Call SetErr(Me.txtLFlight1)
                End If
                ' 移動便発時刻（往路）
                strChk = MDCommon.ReplaceTime(Me.mtbDFlight1.Text)
                If MDChk.ChkNull(strChk) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 移動便発時刻（往路）"))
                    Call SetErr(Me.mtbDFlight1)
                End If
            End If
            ' 移動便発時刻（往路）
            strChk = MDCommon.ReplaceTime(Me.mtbDFlight1.Text)
            If MDChk.ChkNull(strChk) = False Then
                If MDChk.ChkTime(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0021", "開始日時 - 移動便発時刻（往路）"))
                    Call SetErr(Me.mtbDFlight1)
                End If
                ' 移動便名（往路）
                strChk = Me.txtLFlight1.Text.Trim()
                If MDChk.ChkNull(strChk) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 移動便名（往路）"))
                    Call SetErr(Me.txtLFlight1)
                End If
            End If
            ' 移動便名（復路）
            strChk = Me.txtLFlightBack1.Text.Trim()
            If MDChk.ChkNull(strChk) = False Then
                If MDChk.ChkHankaku(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "開始日時 - 移動便名（復路）"))
                    Call SetErr(Me.txtLFlightBack1)
                End If
                ' 移動便発時刻（復路）
                strChk = MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text)
                If MDChk.ChkNull(strChk) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 移動便発時刻（復路）"))
                    Call SetErr(Me.mtbDFlightBack1)
                End If
            End If
            ' 移動便発時刻（復路）
            strChk = MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text)
            If MDChk.ChkNull(strChk) = False Then
                If MDChk.ChkTime(strChk) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0021", "開始日時 - 移動便発時刻（復路）"))
                    Call SetErr(Me.mtbDFlightBack1)
                End If
                ' 移動便名（復路）
                strChk = Me.txtLFlightBack1.Text.Trim()
                If MDChk.ChkNull(strChk) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "開始日時 - 移動便名（復路）"))
                    Call SetErr(Me.txtLFlightBack1)
                End If
            End If

            '---------------------------------------
            '   終了日時
            '---------------------------------------
            ' チェック有の場合
            If Me.chkMeeting2.Checked Then
                ' 開始日付
                strChk = Me.dtpMeeting2.Value.ToString("yyyyMMdd")
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkDate(strChk) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", "終了日時 - 年月日"))
                        Call SetErr(Me.dtpMeeting2)
                    End If
                End If
                ' 開始時刻
                strChk = MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text)
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkTime(strChk) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", "終了日時 - 開始時刻"))
                        Call SetErr(Me.mtbMeetingTimeFrom2)
                    End If
                Else
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "終了日時 - 開始時刻"))
                    Call SetErr(Me.mtbMeetingTimeFrom2)
                End If
                ' 終了時刻
                strChk = MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text)
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkTime(strChk) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", "終了日時 - 終了時刻"))
                        Call SetErr(Me.mtbMeetingTimeTo2)
                    End If
                Else
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "終了日時 - 終了時刻"))
                    Call SetErr(Me.mtbMeetingTimeTo2)
                End If
                ' 会議場
                strChk = Me.txtPlace2.Text.Trim()
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkLength(strChk, 22) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0112", "終了日時 - 会議場", "22", "22"))
                        Call SetErr(Me.txtPlace2)
                    End If
                End If
                ' 移動便名（復路）
                strChk = Me.txtLFlightBack2.Text.Trim()
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkHankaku(strChk) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", "終了日時 - 移動便名（復路）"))
                        Call SetErr(Me.txtLFlightBack2)
                    End If
                    ' 移動便発時刻（復路）
                    strChk = MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text)
                    If MDChk.ChkNull(strChk) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "終了日時 - 移動便発時刻（復路）"))
                        Call SetErr(Me.mtbDFlightBack2)
                    End If
                End If
                ' 移動便発時刻（復路）
                strChk = MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text)
                If MDChk.ChkNull(strChk) = False Then
                    If MDChk.ChkTime(strChk) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", "終了日時 - 移動便発時刻（復路）"))
                        Call SetErr(Me.mtbDFlightBack2)
                    End If
                    ' 移動便名（復路）
                    strChk = Me.txtLFlightBack2.Text.Trim()
                    If MDChk.ChkNull(strChk) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "終了日時 - 移動便名（復路）"))
                        Call SetErr(Me.txtLFlightBack2)
                    End If
                End If
            End If
            '-------------------------------------------------------------------------------
            '   複数エラーメッセージ表示画面表示
            '-------------------------------------------------------------------------------
            If Not arlErrMsg.Count = 0 Then                                                 ' エラー存在チェック
                clsUC999999 = New UC999999                                                  ' メッセージボックスクラス生成
                clsUC999999.errMsgList = arlErrMsg                                          ' プロパティ設定エラーメッセージリスト
                Call clsUC999999.ShowDialog()                                               ' エラーメッセージ表示画面表示
                Return blnRet                                                               ' 処理を抜ける
            End If
            '-------------------------------------------------------------------------------
            '   単数エラーチェック
            '-------------------------------------------------------------------------------
            '===================================================
            '   備考
            '===================================================
            strChk = Me.txtBiko1.Text.Trim()
            If MDChk.ChkNull(strChk) = False Then
                ' 3行以内
                If MDChk.ChkLineCnt(strChk, 3) = False Then
                    Call CLMsg.Show("GE0073", "備考", "40", "3")
                    Call SetErr(Me.txtBiko1)
                    Return blnRet
                End If
                ' 1行40文字以内
                If MDChk.ChkLineLengh(strChk, 40) = False Then
                    Call CLMsg.Show("GE0073", "備考", "40", "3")
                    Call SetErr(Me.txtBiko1)
                    Return blnRet
                End If
            End If
            '===================================================
            '   議題
            '===================================================
            strChk = Me.txtSubject1.Text.Trim()
            If MDChk.ChkNull(strChk) = False Then
                ' 9行以内
                If MDChk.ChkLineCnt(strChk, 9) = False Then
                    Call CLMsg.Show("GE0073", "議題", "40", "9")
                    Call SetErr(Me.txtSubject1)
                    Return blnRet
                End If
                ' 1行40文字以内
                If MDChk.ChkLineLengh(strChk, 40) = False Then
                    Call CLMsg.Show("GE0073", "議題", "40", "9")
                    Call SetErr(Me.txtSubject1)
                    Return blnRet
                End If
            End If
            '===================================================
            '   議題備考
            '===================================================
            strChk = Me.txtBiko2.Text.Trim()
            If MDChk.ChkNull(strChk) = False Then
                ' 5行以内
                If MDChk.ChkLineCnt(strChk, 5) = False Then
                    Call CLMsg.Show("GE0073", "議題備考", "43", "5")
                    Call SetErr(Me.txtBiko2)
                    Return blnRet
                End If
                ' 1行43文字以内
                If MDChk.ChkLineLengh(strChk, 43) = False Then
                    Call CLMsg.Show("GE0073", "議題備考", "43", "5")
                    Call SetErr(Me.txtBiko2)
                    Return blnRet
                End If
            End If
            '-------------------------------------------------------------------------------
            '   開催日付が過去日付（今日含む）の場合、登録確認メッセージ表示
            '-------------------------------------------------------------------------------
            strNow = Now.ToString("yyyyMMdd")                           ' 現在日付取得
            strStartDate = Me.dtpMeeting1.Value.ToString("yyyyMMdd")    ' 開催日付取得
            If strStartDate <= strNow Then
                daiRet = CLMsg.Show("GW0018")                           ' 登録確認メッセージ表示
                If daiRet = DialogResult.No Then                        ' 確認メッセージ判定
                    Return blnRet                                       ' 「いいえ」押下時、処理を抜ける
                End If
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUpdateDelete
    '   名称　：組合大会通知情報更新処理（登録・更新）
    '   概要  ：組合大会通い情報の登録更新処理を行う。
    '   引数　：ByVal iDtStafId                       As DataTable = 社員情報リスト
    '           Optional ByVal iStrNewUnionMeetingNo  As String    = 採番した組合大会会議番号,
    '           Optional ByVal iIntNewUnionMeetingSeq As Integer   = 採番した組合大会会議SEQ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知情報更新処理（登録・更新</summary>
    ''' <param name="iDtStafId"></param>
    ''' <param name="iStrNewUnionMeetingNo"></param>
    ''' <param name="iIntNewUnionMeetingSeq"></param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdateDelete(ByVal iDtStafId As DataTable, _
                                        Optional ByVal iStrNewUnionMeetingNo As String = "", _
                                        Optional ByVal iIntNewUnionMeetingSeq As Integer = 0) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As New CLAccessMdb                    ' データベースクラス

        Try
            '-------------------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------------------
            Cursor.Current = Cursors.WaitCursor         ' カーソルを砂時計に設定
            Call FrmWaitInfo.ShowWaitForm(Nothing)      ' しばらくお待ちくださいフォーム表示
            Call clsDb.Connect()                        ' データベース接続
            Call clsDb.BeginTran()                      ' トランザクション開始処理

            '-------------------------------------------------------------------------------
            '   登録・更新処理
            '-------------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT _
            Or Me.bytStatus = STATUS_SAME Then
                '---------------------------------------------------------------------------
                '   開催登録（登録処理）
                '---------------------------------------------------------------------------
                '===========================================
                '   組合大会通知情報
                '===========================================
                ' 組合大会通知情報存在確認処理
                If ExistsUnionInformation(clsDb, _
                                          MDLoginInfo.Ksh, _
                                          MDLoginInfo.Period.ToString(), _
                                          iStrNewUnionMeetingNo, _
                                          Me.cboApplyArea.SelectedValue.ToString()) Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知情報登録処理
                If InsertUnionInformation(clsDb, _
                                          iStrNewUnionMeetingNo, _
                                          iIntNewUnionMeetingSeq) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    'Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If

                '===========================================
                '   組合大会通知詳細情報
                '===========================================
                ' 組合大会通知詳細情報存在確認処理
                If ExistsUnionInformationDtl(clsDb, _
                                             MDLoginInfo.Ksh, _
                                             MDLoginInfo.Period.ToString(), _
                                             iStrNewUnionMeetingNo, _
                                             Me.cboApplyArea.SelectedValue.ToString()) Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知詳細情報登録処理
                If InsertUnionInformationDtl(clsDb, _
                                             iStrNewUnionMeetingNo) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If

                '===========================================
                '   組合大会通知メンバー情報
                '===========================================
                ' 組合大会通知メンバー情報存在確認処理
                If ExistsUnionInformationMember(clsDb, _
                                                MDLoginInfo.Ksh, _
                                                MDLoginInfo.Period.ToString(), _
                                                iStrNewUnionMeetingNo, _
                                                Me.cboApplyArea.SelectedValue.ToString()) Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知メンバー情報登録処理
                If InsertUnionInformationMember(clsDb, _
                                                iStrNewUnionMeetingNo, _
                                                iIntNewUnionMeetingSeq, _
                                                iDtStafId) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If

            ElseIf Me.bytStatus = STATUS_UPDATE _
            Or Me._bytStatus = STATUS_STOP Then
                '---------------------------------------------------------------------------
                '   変更・中止（更新処理）
                '---------------------------------------------------------------------------
                '===========================================
                '   組合大会通知情報
                '===========================================
                ' 組合大会通知情報存在確認処理
                If ExistsUnionInformation(clsDb, _
                                          Me.strKsh, _
                                          Me.strPeriodId, _
                                          Me.strUnionMeeting, _
                                          Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知情報更新処理
                If UpdateUnionInformation(clsDb, _
                                          Me.strKsh, _
                                          Me.strPeriodId, _
                                          Me.strUnionMeeting, _
                                          Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("DE0005")           ' 正しくデータが更新できませんでした。の旨のメッセージ表示
                    Return blnRet
                End If

                '===========================================
                '   組合大会通知詳細情報
                '===========================================
                If ExistsUnionInformationDtl(clsDb, _
                                             Me.strKsh, _
                                             Me.strPeriodId, _
                                             Me.strUnionMeeting, _
                                             Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知詳細情報更新処理
                If UpdateUnionInformationDtl(clsDb, _
                                             Me.strKsh, _
                                             Me.strPeriodId, _
                                             Me.strUnionMeeting, _
                                             Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("DE0005")           ' 正しくデータが更新できませんでした。の旨のメッセージ表示
                    Return blnRet
                End If

                '===========================================
                '   組合大会通知メンバー情報
                '===========================================
                If ExistsUnionInformationMember(clsDb, _
                                                Me.strKsh, _
                                                Me.strPeriodId, _
                                                Me.strUnionMeeting, _
                                                Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知メンバー情報削除処理
                If DeleteUnionInformationMember(clsDb, _
                                                Me.strKsh, _
                                                Me.strPeriodId, _
                                                Me.strUnionMeeting, _
                                                Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知メンバー情報登録処理
                If InsertUnionInformationMember(clsDb, _
                                                iStrNewUnionMeetingNo, _
                                                iIntNewUnionMeetingSeq, _
                                                iDtStafId) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If

            ElseIf Me.bytStatus = STATUS_DETAIL Then
                '---------------------------------------------------------------------------
                '   プレ印刷（更新処理）
                '---------------------------------------------------------------------------
                '===========================================
                '   組合大会通知メンバー情報
                '===========================================
                If ExistsUnionInformationMember(clsDb, _
                                                Me.strKsh, _
                                                Me.strPeriodId, _
                                                Me.strUnionMeeting, _
                                                Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知メンバー情報削除処理
                If DeleteUnionInformationMember(clsDb, _
                                                Me.strKsh, _
                                                Me.strPeriodId, _
                                                Me.strUnionMeeting, _
                                                Me.strApplyArea) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If
                ' 組合大会通知メンバー情報登録処理
                If InsertUnionInformationMember(clsDb, _
                                                iStrNewUnionMeetingNo, _
                                                iIntNewUnionMeetingSeq, _
                                                iDtStafId) = False Then
                    Call clsDb.RollbackTran()           ' トランザクション取消処理
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If
            End If

            ' トランザクション確定処理
            Call clsDb.CommitTran()

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            '-------------------------------------------------------------------------------
            '   エラー処理
            '-------------------------------------------------------------------------------
            ' トランザクション取消処理
            Call clsDb.RollbackTran()
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            '-------------------------------------------------------------------------------
            '   終了処理
            '-------------------------------------------------------------------------------
            Call clsDb.Disconnect()                 ' データベース切断
            Call FrmWaitInfo.CloseWaitForm()        ' しばらくお待ちくださいフォームクローズ
            Cursor.Current = Cursors.Default        ' カーソルを矢印に戻す
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsUnionInformation
    '   名称　：組合大会通知情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス
    '           ByVal iStrKsh             As String      = 会社コード,
    '           ByVal iStrPeriodId        As String      = 期ID,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea       As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知情報存在チェック処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsUnionInformation(ByVal iClsDb As CLAccessMdb, _
                                            ByVal iStrKsh As String, _
                                            ByVal iStrPeriodId As String, _
                                            ByVal iStrUnionMeetingNo As String, _
                                            ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim intRet As Integer = 0               ' 処理件数
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_ksh" & vbCrLf
            strSql = strSql & "       ,a.c_period_id" & vbCrLf
            strSql = strSql & "       ,a.c_union_meeting" & vbCrLf
            strSql = strSql & "       ,a.k_apply_area" & vbCrLf
            strSql = strSql & "   FROM union_information AS a" & vbCrLf                             ' 組合大会通知情報
            strSql = strSql & "  WHERE a.c_ksh           = '" & iStrKsh & "'" & vbCrLf              ' 会社コードと同じもの
            strSql = strSql & "    AND a.c_period_id     = '" & iStrPeriodId & "'" & vbCrLf         ' 期IDと同じもの
            strSql = strSql & "    AND a.c_union_meeting = '" & iStrUnionMeetingNo & "'" & vbCrLf   ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND a.k_apply_area    = '" & iStrApplyArea & "'" & vbCrLf        ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数確認
            If intRet <> 1 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsUnionInformationDtl
    '   名称　：組合大会通知詳細情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス
    '           ByVal iStrKsh             As String      = 会社コード,
    '           ByVal iStrPeriodId        As String      = 期ID,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea       As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知詳細情報存在チェック処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsUnionInformationDtl(ByVal iClsDb As CLAccessMdb, _
                                               ByVal iStrKsh As String, _
                                               ByVal iStrPeriodId As String, _
                                               ByVal iStrUnionMeetingNo As String, _
                                               ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim intRet As Integer = 0               ' 処理件数
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_ksh" & vbCrLf
            strSql = strSql & "       ,a.c_period_id" & vbCrLf
            strSql = strSql & "       ,a.c_union_meeting" & vbCrLf
            strSql = strSql & "       ,a.k_apply_area" & vbCrLf
            strSql = strSql & "   FROM union_information_dtl AS a" & vbCrLf                         ' 組合大会通知詳細情報
            strSql = strSql & "  WHERE a.c_ksh           = '" & iStrKsh & "'" & vbCrLf              ' 会社コードと同じもの
            strSql = strSql & "    AND a.c_period_id     = '" & iStrPeriodId & "'" & vbCrLf         ' 期IDと同じもの
            strSql = strSql & "    AND a.c_union_meeting = '" & iStrUnionMeetingNo & "'" & vbCrLf   ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND a.k_apply_area    = '" & iStrApplyArea & "'" & vbCrLf        ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数確認
            If intRet <> 1 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsUnionInformationMember
    '   名称　：組合大会通知メンバー情報存在確認処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス
    '           ByVal iStrKsh             As String      = 会社コード,
    '           ByVal iStrPeriodId        As String      = 期ID,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea       As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/20(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知メンバー情報存在チェック処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsUnionInformationMember(ByVal iClsDb As CLAccessMdb, _
                                                  ByVal iStrKsh As String, _
                                                  ByVal iStrPeriodId As String, _
                                                  ByVal iStrUnionMeetingNo As String, _
                                                  ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim intRet As Integer = 0                   ' 処理件数
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_ksh" & vbCrLf
            strSql = strSql & "       ,a.c_period_id" & vbCrLf
            strSql = strSql & "       ,a.c_union_meeting" & vbCrLf
            strSql = strSql & "   FROM union_information_member AS a" & vbCrLf                      ' 組合大会通知メンバー情報
            strSql = strSql & "  WHERE a.c_ksh           = '" & iStrKsh & "'" & vbCrLf              ' 会社コードと同じもの
            strSql = strSql & "    AND a.c_period_id     = '" & iStrPeriodId & "'" & vbCrLf         ' 期IDと同じもの
            strSql = strSql & "    AND a.c_union_meeting = '" & iStrUnionMeetingNo & "'" & vbCrLf   ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND a.k_apply_area    = '" & iStrApplyArea & "'" & vbCrLf        ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数確認
            If intRet = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUnionInformation
    '   名称　：組合大会通知情報登録処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByVal iIntUnionMeetingSeq As Integer     = 組合大会会議SEQ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知情報登録処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iIntUnionMeetingSeq">組合大会会議SEQ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUnionInformation(ByVal iClsDb As CLAccessMdb, _
                                            ByVal iStrUnionMeetingNo As String, _
                                            ByVal iIntUnionMeetingSeq As Integer) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合大会通知情報登録
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = ""
            strSql = strSql & " INSERT INTO union_information ( " & vbCrLf
            strSql = strSql & "     c_ksh                " & vbCrLf     ' 01. 会社コード
            strSql = strSql & "    ,c_period_id          " & vbCrLf     ' 02. 期ID
            strSql = strSql & "    ,c_union_meeting      " & vbCrLf     ' 03. 組合大会会議番号
            strSql = strSql & "    ,k_apply_area         " & vbCrLf     ' 04. 申請地区区分
            strSql = strSql & "    ,s_union_meeting_seq  " & vbCrLf     ' 05. 組合大会会議SEQ
            strSql = strSql & "    ,k_information_type   " & vbCrLf     ' 06. 種類（種別）
            strSql = strSql & "    ,l_information_name   " & vbCrLf     ' 07. 会議名（目的）（種類）
            strSql = strSql & "    ,d_meeting_1          " & vbCrLf     ' 08. 会議日付1
            strSql = strSql & "    ,d_meeting_time_from_1" & vbCrLf     ' 09. 会議時間From1
            strSql = strSql & "    ,d_meeting_time_to_1  " & vbCrLf     ' 10. 会議時間To1
            strSql = strSql & "    ,l_place_1            " & vbCrLf     ' 11. 会議場所1
            strSql = strSql & "    ,l_flight_1           " & vbCrLf     ' 12. 移動フライト（往路）1
            strSql = strSql & "    ,d_flight_1           " & vbCrLf     ' 13. 移動フライト時間（往路）1
            strSql = strSql & "    ,l_flight_back_1      " & vbCrLf     ' 14. 移動フライト（復路）1
            strSql = strSql & "    ,d_flight_back_1      " & vbCrLf     ' 15. 移動フライト時間（復路）1
            strSql = strSql & "    ,k_lunch_1            " & vbCrLf     ' 16. 昼食可否1
            strSql = strSql & "    ,k_exchange_meeting_1 " & vbCrLf     ' 17. 夕食交流会可否1
            strSql = strSql & "    ,d_meeting_2          " & vbCrLf     ' 18. 会議日付2
            strSql = strSql & "    ,d_meeting_time_from_2" & vbCrLf     ' 19. 会議時間From2
            strSql = strSql & "    ,d_meeting_time_to_2  " & vbCrLf     ' 20. 会議時間To2
            strSql = strSql & "    ,l_place_2            " & vbCrLf     ' 21. 会議場所2
            strSql = strSql & "    ,l_flight_2           " & vbCrLf     ' 22. 移動フライト（往路）2
            strSql = strSql & "    ,d_flight_2           " & vbCrLf     ' 23. 移動フライト時間（往路）2
            strSql = strSql & "    ,l_flight_back_2      " & vbCrLf     ' 24. 移動フライト（復路）2
            strSql = strSql & "    ,d_flight_back_2      " & vbCrLf     ' 25. 移動フライト時間（復路）2
            strSql = strSql & "    ,k_lunch_2            " & vbCrLf     ' 26. 昼食可否2
            strSql = strSql & "    ,k_exchange_meeting_2 " & vbCrLf     ' 27. 夕食交流会可否2
            strSql = strSql & "    ,l_open_bebiginting   " & vbCrLf     ' 28. 開催場所（所属支部区分）
            strSql = strSql & "    ,l_subject_1          " & vbCrLf     ' 29. 議題1
            strSql = strSql & "    ,l_subject_2          " & vbCrLf     ' 30. 議題2
            strSql = strSql & "    ,l_biko_1             " & vbCrLf     ' 31. 備考1
            strSql = strSql & "    ,l_biko_2             " & vbCrLf     ' 32. 備考2
            strSql = strSql & "    ,l_biko_3             " & vbCrLf     ' 33. 備考3
            strSql = strSql & "    ,d_ins                " & vbCrLf     ' 34. 作成日
            strSql = strSql & "    ,c_user_id_ins        " & vbCrLf     ' 35. 作成者個人ID
            strSql = strSql & "    ,d_up                 " & vbCrLf     ' 36. 更新日
            strSql = strSql & "    ,c_user_id_up         " & vbCrLf     ' 37. 更新者個人ID
            strSql = strSql & "    ,s_up                 " & vbCrLf     ' 38. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            ' 01. 会社コード
            strSql = strSql & "     '" & MDLoginInfo.Ksh & "'" & vbCrLf
            ' 02. 期ID
            strSql = strSql & "    ,'" & MDLoginInfo.PeriodId & "'" & vbCrLf
            ' 03. 組合大会会議番号
            strSql = strSql & "    ,'" & iStrUnionMeetingNo & "'" & vbCrLf
            ' 04. 申請地区区分
            strSql = strSql & "    ,'" & Me.cboApplyArea.SelectedValue.ToString() & "'" & vbCrLf
            ' 05. 組合大会会議SEQ
            strSql = strSql & "    ," & iIntUnionMeetingSeq.ToString() & vbCrLf
            ' 06. 種類（種別）
            If Me.optUnionType1.Checked Then
                ' 合同
                strSql = strSql & "    ,'" & UI_CIR_KIND_JOIN & "'" & vbCrLf
            ElseIf Me.optUnionType2.Checked Then
                ' TV
                strSql = strSql & "    ,'" & UI_CIR_KIND_TV & "'" & vbCrLf
            ElseIf Me.optUnionType3.Checked Then
                ' 任意
                strSql = strSql & "    ,'" & UI_CIR_KIND_ANY & "'" & vbCrLf
            End If
            ' 07. 会議名（目的）（種類）
            If Me.optInfomationName1.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName1.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName2.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName2.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName3.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName3.Text.Trim() & "'" & vbCrLf
            End If
            '=======================================
            '   開始日時
            '=======================================
            ' 08. 会議日付1
            strSql = strSql & "    ,'" & Me.dtpMeeting1.Value.Date.ToString() & "'" & vbCrLf
            ' 09. 会議時間From1
            strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom1.Text) & "'" & vbCrLf
            ' 10. 会議時間To1
            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text)) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text) & "'" & vbCrLf
            End If
            ' 11. 会議場所1
            If MDChk.ChkNull(Me.txtPlace1.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtPlace1.Text.Trim() & "'" & vbCrLf
            End If
            ' 12. 移動フライト（往路）1
            If MDChk.ChkNull(Me.txtLFlight1.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtLFlight1.Text.Trim() & "'" & vbCrLf
            End If
            ' 13. 移動フライト時間（往路）1
            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlight1.Text)) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbDFlight1.Text) & "'" & vbCrLf
            End If
            ' 14. 移動フライト（復路）1
            If MDChk.ChkNull(Me.txtLFlightBack1.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtLFlightBack1.Text.Trim() & "'" & vbCrLf
            End If
            ' 15. 移動フライト時間（復路）1
            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text)) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text) & "'" & vbCrLf
            End If
            ' 16. 昼食可否1
            If Me.chkLunch1.Checked Then
                ' チェック有
                strSql = strSql & "    ,'1'" & vbCrLf
            Else
                ' チェック無
                strSql = strSql & "    ,'0'" & vbCrLf
            End If
            ' 17. 夕食交流会可否1
            If Me.chkExchangeMeeting1.Checked Then
                ' チェック有
                strSql = strSql & "    ,'1'" & vbCrLf
            Else
                ' チェック無
                strSql = strSql & "    ,'0'" & vbCrLf
            End If
            '=======================================
            '   終了日時
            '=======================================
            ' 終了日時チェックボックスにチェックが付いている場合
            If Me.chkMeeting2.Checked Then
                ' 18. 会議日付2
                strSql = strSql & "    ,'" & Me.dtpMeeting2.Value.Date.ToString() & "'" & vbCrLf
                ' 19. 会議時間From2
                If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text)) Then
                    strSql = strSql & "    ,''" & vbCrLf
                Else
                    strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text) & "'" & vbCrLf
                End If
                ' 20. 会議時間To2
                If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text)) Then
                    strSql = strSql & "    ,''" & vbCrLf
                Else
                    strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text) & "'" & vbCrLf
                End If
                ' 21. 会議場所2
                If MDChk.ChkNull(Me.txtPlace2.Text.Trim()) Then
                    strSql = strSql & "    ,''" & vbCrLf
                Else
                    strSql = strSql & "    ,'" & Me.txtPlace2.Text.Trim() & "'" & vbCrLf
                End If
                ' 22. 移動フライト（往路）2
                strSql = strSql & "    ,''" & vbCrLf
                ' 23. 移動フライト時間（往路）2
                strSql = strSql & "    ,''" & vbCrLf
                ' 24. 移動フライト（復路）2
                If MDChk.ChkNull(Me.txtLFlightBack2.Text.Trim()) Then
                    strSql = strSql & "    ,''" & vbCrLf
                Else
                    strSql = strSql & "    ,'" & Me.txtLFlightBack2.Text.Trim() & "'" & vbCrLf
                End If
                ' 25. 移動フライト時間（復路）2
                If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text)) Then
                    strSql = strSql & "    ,''" & vbCrLf
                Else
                    strSql = strSql & "    ,'" & MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text) & "'" & vbCrLf
                End If
                ' 26. 昼食可否2
                If Me.chkLunch2.Checked Then
                    strSql = strSql & "    ,'1'" & vbCrLf   ' チェック有
                Else
                    strSql = strSql & "    ,'0'" & vbCrLf   ' チェック無
                End If
                ' 27. 夕食交流会可否2
                If Me.chkExchangeMeeting2.Checked Then
                    strSql = strSql & "    ,'1'" & vbCrLf   ' チェック有
                Else
                    strSql = strSql & "    ,'0'" & vbCrLf   ' チェック無
                End If
            Else
                strSql = strSql & "    ,Null" & vbCrLf      ' 18. 会議日付2
                strSql = strSql & "    ,''" & vbCrLf        ' 19. 会議時間From2
                strSql = strSql & "    ,''" & vbCrLf        ' 20. 会議時間To2
                strSql = strSql & "    ,''" & vbCrLf        ' 21. 会議場所2
                strSql = strSql & "    ,''" & vbCrLf        ' 22. 移動フライト（往路）2
                strSql = strSql & "    ,''" & vbCrLf        ' 23. 移動フライト時間（往路）2
                strSql = strSql & "    ,''" & vbCrLf        ' 24. 移動フライト（復路）2
                strSql = strSql & "    ,''" & vbCrLf        ' 25. 移動フライト時間（復路）2
                strSql = strSql & "    ,'0'" & vbCrLf       ' 26. 昼食可否2
                strSql = strSql & "    ,'0'" & vbCrLf       ' 27. 夕食交流会可否2
            End If
            ' 28. 開催場所（所属支部区分）
            strSql = strSql & "    ,'" & Me.cboOpenBelonging.Text.Trim() & "'" & vbCrLf
            ' 29. 議題1
            If MDChk.ChkNull(Me.txtSubject1.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtSubject1.Text.Trim() & "'" & vbCrLf
            End If
            ' 30. 議題2
            strSql = strSql & "    ,''" & vbCrLf
            ' 31. 備考1
            If MDChk.ChkNull(Me.txtBiko1.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtBiko1.Text.Trim() & "'" & vbCrLf
            End If
            ' 32. 備考2
            If MDChk.ChkNull(Me.txtBiko2.Text.Trim()) Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtBiko2.Text.Trim() & "'" & vbCrLf
            End If
            ' 33. 備考3
            strSql = strSql & "    ,''" & vbCrLf
            ' 34. 作成日
            strSql = strSql & "    ,'" & System.DateTime.Now().Date.ToString() & "'" & vbCrLf
            ' 35. 作成者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 36. 更新日
            strSql = strSql & "    ,Null" & vbCrLf
            ' 37. 更新者個人ID
            strSql = strSql & "    ,''" & vbCrLf
            ' 38. 更新回数
            strSql = strSql & "    ,0" & vbCrLf
            strSql = strSql & ");" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQueryKeyErr(strSql)

            ' 処理件数判定
            If intRet = -2 Then
                CLMsg.Show("DE0015")
                Return blnRet
            ElseIf intRet <> 1 Then
                Call MessageBox.Show("組合大会通知情報を登録できませんでした。", _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Error, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUnionInformationDtl
    '   名称　：組合大会通知詳細情報登録処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知詳細情報登録処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUnionInformationDtl(ByVal iClsDb As CLAccessMdb, _
                                               ByVal iStrUnionMeetingNo As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合大会通知情報登録
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = ""
            strSql = strSql & " INSERT INTO union_information_dtl ( " & vbCrLf
            strSql = strSql & "     c_ksh                " & vbCrLf     ' 01. 会社コード
            strSql = strSql & "    ,c_period_id          " & vbCrLf     ' 02. 期ID
            strSql = strSql & "    ,c_union_meeting      " & vbCrLf     ' 03. 組合大会会議番号
            strSql = strSql & "    ,k_apply_area         " & vbCrLf     ' 04. 申請地区区分
            strSql = strSql & "    ,k_union_meeting      " & vbCrLf     ' 05. 
            strSql = strSql & "    ,d_ins                " & vbCrLf     ' 06. 作成日
            strSql = strSql & "    ,c_user_id_ins        " & vbCrLf     ' 07. 作成者個人ID
            strSql = strSql & "    ,d_up                 " & vbCrLf     ' 08. 更新日
            strSql = strSql & "    ,c_user_id_up         " & vbCrLf     ' 09. 更新者個人ID
            strSql = strSql & "    ,s_up                 " & vbCrLf     ' 10. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            ' 01. 会社コード
            strSql = strSql & "     '" & MDLoginInfo.Ksh & "'" & vbCrLf
            ' 02. 期ID
            strSql = strSql & "    ,'" & MDLoginInfo.PeriodId & "'" & vbCrLf
            ' 03. 組合大会会議番号
            strSql = strSql & "    ,'" & iStrUnionMeetingNo & "'" & vbCrLf
            ' 04. 申請地区区分
            strSql = strSql & "    ,'" & Me.cboApplyArea.SelectedValue.ToString() & "'" & vbCrLf
            ' 05. 種類
            If Me.optInfomationName1.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName1.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName2.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName2.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName3.Checked Then
                strSql = strSql & "    ,'" & Me.optInfomationName3.Text.Trim() & "'" & vbCrLf
            End If
            ' 06. 作成日
            strSql = strSql & "    ,'" & System.DateTime.Now().Date.ToString() & "'" & vbCrLf
            ' 07. 作成者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 08. 更新日
            strSql = strSql & "    ,Null" & vbCrLf
            ' 09. 更新者個人ID
            strSql = strSql & "    ,''" & vbCrLf
            ' 10. 更新回数
            strSql = strSql & "    ,0" & vbCrLf
            strSql = strSql & ");" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理件数判定
            If intRet <> 1 Then
                Call MessageBox.Show("組合大会通知詳細情報を登録できませんでした。", _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Error, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUnionInformationMember
    '   名称　：組合大会通知メンバー情報登録処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス,
    '           ByVal iStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByVal iIntUnionMeetingSeq As Integer     = 組合大会会議SEQ,
    '           ByVal iDtStafId           As DataTable   = 社員情報
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/17(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知メンバー情報登録処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iIntUnionMeetingSeq">組合大会会議SEQ</param>
    ''' <param name="iDtStafId">社員情報</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUnionInformationMember(ByVal iClsDb As CLAccessMdb, _
                                                  ByVal iStrUnionMeetingNo As String, _
                                                  ByVal iIntUnionMeetingSeq As Integer, _
                                                  ByVal iDtStafId As DataTable) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル
        Dim strSql As String = ""               ' SQL文
        Dim intRet As Integer = 0               ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合大会通知メンバー情報登録
            '-----------------------------------------------------------------------------------
            For i = 0 To iDtStafId.Rows.Count - 1
                ' SQL文作成
                strSql = ""
                strSql = strSql & " INSERT INTO union_information_member ( " & vbCrLf
                strSql = strSql & "     c_ksh              " & vbCrLf               ' 01. 会社コード
                strSql = strSql & "    ,c_period_id        " & vbCrLf               ' 02. 期ID
                strSql = strSql & "    ,c_union_meeting    " & vbCrLf               ' 03. 組合大会会議番号
                strSql = strSql & "    ,k_apply_area       " & vbCrLf               ' 04. 申請地区区分
                strSql = strSql & "    ,s_union_meeting_seq" & vbCrLf               ' 05. 組合大会会議SEQ
                strSql = strSql & "    ,c_user_id          " & vbCrLf               ' 06. 個人認証ID
                strSql = strSql & "    ,d_ins              " & vbCrLf               ' 07. 作成日
                strSql = strSql & "    ,c_user_id_ins      " & vbCrLf               ' 08. 作成者個人ID
                strSql = strSql & " ) VALUES ( " & vbCrLf
                ' 01. 会社コード
                strSql = strSql & "     '" & MDLoginInfo.Ksh & "'" & vbCrLf
                ' 02. 期ID
                strSql = strSql & "    ,'" & MDLoginInfo.PeriodId & "'" & vbCrLf
                ' 03. 組合大会会議番号
                strSql = strSql & "    ,'" & iStrUnionMeetingNo & "'" & vbCrLf
                ' 04. 申請地区区分
                strSql = strSql & "    ,'" & Me.cboApplyArea.SelectedValue.ToString() & "'" & vbCrLf
                ' 05. 組合大会会議SEQ
                strSql = strSql & "    ," & iIntUnionMeetingSeq.ToString() & vbCrLf
                ' 06. 個人認証ID
                strSql = strSql & "    ,'" & iDtStafId.Rows(i).Item("社員番号").ToString() & "'" & vbCrLf
                ' 07. 作成日
                strSql = strSql & "    ,'" & System.DateTime.Now().Date.ToString() & "'" & vbCrLf
                ' 08. 作成者個人ID
                strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
                strSql = strSql & ");" & vbCrLf

                ' SQL実行
                intRet = iClsDb.ExecuteNonQuery(strSql)

                ' 処理件数判定
                If intRet = 0 Then
                    Call MessageBox.Show("組合大会通知メンバー情報を登録できませんでした。", _
                                         "エラー", _
                                         MessageBoxButtons.OK, _
                                         MessageBoxIcon.Error, _
                                         MessageBoxDefaultButton.Button1)
                    Return blnRet
                End If
            Next

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateUnionInformation
    '   名称　：組合大会通知情報更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb                 As CLAccessMdb = データベースクラス,
    '           ByVal iStrKsh                As String      = 会社コード,
    '           ByVal iStrPeriodId           As String      = 期ID,
    '           ByVal iStrUnionMeeting       As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea          As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知情報更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeeting">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateUnionInformation(ByVal iClsDb As CLAccessMdb, _
                                            ByVal iStrKsh As String, _
                                            ByVal iStrPeriodId As String, _
                                            ByVal iStrUnionMeeting As String, _
                                            ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            ' SQL文作成
            strSql = ""
            strSql = strSql & " UPDATE union_information" & vbCrLf


            ' 07. 会議名（目的）（種類）
            If Me.optInfomationName1.Checked Then
                strSql = strSql & "    SET l_information_name    = '" & Me.optInfomationName1.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName2.Checked Then
                strSql = strSql & "    SET l_information_name    = '" & Me.optInfomationName2.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName3.Checked Then
                strSql = strSql & "    SET l_information_name    = '" & Me.optInfomationName3.Text.Trim() & "'" & vbCrLf
            End If
            ' 06. 種類（種別）
            If Me.optUnionType1.Checked Then
                ' 合同
                strSql = strSql & "       ,k_information_type  ='" & UI_CIR_KIND_JOIN & "'" & vbCrLf
            ElseIf Me.optUnionType2.Checked Then
                ' TV
                strSql = strSql & "       ,k_information_type  ='" & UI_CIR_KIND_TV & "'" & vbCrLf
            ElseIf Me.optUnionType3.Checked Then
                ' 任意
                strSql = strSql & "       ,k_information_type  ='" & UI_CIR_KIND_ANY & "'" & vbCrLf
            End If

            '=======================================
            '   開始日時
            '=======================================
            ' 08. 会議日付1
            strSql = strSql & "       ,d_meeting_1           = '" & Me.dtpMeeting1.Value.Date.ToString() & "'" & vbCrLf
            ' 09. 会議時間From1
            strSql = strSql & "       ,d_meeting_time_from_1 = '" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom1.Text) & "'" & vbCrLf
            ' 10. 会議時間To1
            strSql = strSql & "       ,d_meeting_time_to_1   = '" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text) & "'" & vbCrLf
            ' 11. 会議場所1
            strSql = strSql & "       ,l_place_1             = '" & Me.txtPlace1.Text.Trim() & "'" & vbCrLf
            ' 12. 移動フライト（往路）1
            strSql = strSql & "       ,l_flight_1            = '" & Me.txtLFlight1.Text.Trim() & "'" & vbCrLf
            ' 13. 移動フライト時間（往路）1
            strSql = strSql & "       ,d_flight_1            = '" & MDCommon.ReplaceTime(Me.mtbDFlight1.Text) & "'" & vbCrLf
            ' 14. 移動フライト（復路）1
            strSql = strSql & "       ,l_flight_back_1       = '" & Me.txtLFlightBack1.Text.Trim() & "'" & vbCrLf
            ' 15. 移動フライト時間（復路）1
            strSql = strSql & "       ,d_flight_back_1       = '" & MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text) & "'" & vbCrLf
            ' 16. 昼食可否1
            If Me.chkLunch1.Checked Then
                ' チェック有
                strSql = strSql & "       ,k_lunch_1             = '1'" & vbCrLf
            Else
                ' チェック無
                strSql = strSql & "       ,k_lunch_1             = '0'" & vbCrLf
            End If
            ' 17. 夕食交流会可否1
            If Me.chkExchangeMeeting1.Checked Then
                ' チェック有
                strSql = strSql & "       ,k_exchange_meeting_1  = '1' " & vbCrLf
            Else
                ' チェック無
                strSql = strSql & "       ,k_exchange_meeting_1  = '0' " & vbCrLf
            End If
            '=======================================
            '   終了日時
            '=======================================
            ' 終了日時チェックボックスにチェックが付いている場合
            If Me.chkMeeting2.Checked Then
                ' 18. 会議日付2
                strSql = strSql & "       ,d_meeting_2           = '" & Me.dtpMeeting2.Value.Date.ToString() & "'" & vbCrLf
                ' 19. 会議時間From2
                strSql = strSql & "       ,d_meeting_time_from_2 = '" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text) & "'" & vbCrLf
                ' 20. 会議時間To2
                strSql = strSql & "       ,d_meeting_time_to_2   = '" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text) & "'" & vbCrLf
                ' 21. 会議場所2
                strSql = strSql & "       ,l_place_2             = '" & Me.txtPlace2.Text.Trim() & "'" & vbCrLf
                ' 22. 移動フライト（往路）2
                strSql = strSql & "       ,l_flight_2            = ''" & vbCrLf
                ' 23. 移動フライト時間（往路）2
                strSql = strSql & "       ,d_flight_2            = ''" & vbCrLf
                ' 24. 移動フライト（復路）2
                strSql = strSql & "       ,l_flight_back_2       = '" & Me.txtLFlightBack2.Text.Trim() & "'" & vbCrLf
                ' 25. 移動フライト時間（復路）2
                strSql = strSql & "       ,d_flight_back_2       = '" & MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text) & "'" & vbCrLf
                ' 26. 昼食可否2
                If Me.chkLunch2.Checked Then
                    ' チェック有
                    strSql = strSql & "       ,k_lunch_2             = '1'" & vbCrLf
                Else
                    ' チェック無
                    strSql = strSql & "       ,k_lunch_2             = '0'" & vbCrLf
                End If
                ' 27. 夕食交流会可否2
                If Me.chkExchangeMeeting2.Checked Then
                    ' チェック有
                    strSql = strSql & "       ,k_exchange_meeting_2  = '1'" & vbCrLf
                Else
                    ' チェック無
                    strSql = strSql & "       ,k_exchange_meeting_2  = '0'" & vbCrLf
                End If
            Else
                ' 18. 会議日付2
                strSql = strSql & "       ,d_meeting_2           = Null" & vbCrLf
                ' 19. 会議時間From2
                strSql = strSql & "       ,d_meeting_time_from_2 = ''" & vbCrLf
                ' 20. 会議時間To2
                strSql = strSql & "       ,d_meeting_time_to_2   = ''" & vbCrLf
                ' 21. 会議場所2
                strSql = strSql & "       ,l_place_2             = ''" & vbCrLf
                ' 22. 移動フライト（往路）2
                strSql = strSql & "       ,l_flight_2            = ''" & vbCrLf
                ' 23. 移動フライト時間（往路）2
                strSql = strSql & "       ,d_flight_2            = ''" & vbCrLf
                ' 24. 移動フライト（復路）2
                strSql = strSql & "       ,l_flight_back_2       = ''" & vbCrLf
                ' 25. 移動フライト時間（復路）2
                strSql = strSql & "       ,d_flight_back_2       = ''" & vbCrLf
                ' 26. 昼食可否2
                strSql = strSql & "       ,k_lunch_2             = '0'" & vbCrLf
                ' 27. 夕食交流会可否2
                strSql = strSql & "       ,k_exchange_meeting_2  = '0'" & vbCrLf
            End If
            ' 28. 開催場所（所属支部区分）
            strSql = strSql & "       ,l_open_bebiginting    = '" & Me.cboOpenBelonging.Text.Trim() & "'" & vbCrLf
            ' 29. 議題1
            strSql = strSql & "       ,l_subject_1           = '" & Me.txtSubject1.Text.Trim() & "'" & vbCrLf
            ' 31. 備考1
            strSql = strSql & "       ,l_biko_1              = '" & Me.txtBiko1.Text.Trim() & "'" & vbCrLf
            ' 32. 備考2
            strSql = strSql & "       ,l_biko_2              = '" & Me.txtBiko2.Text.Trim() & "'" & vbCrLf
            ' 36. 更新日
            strSql = strSql & "       ,d_up                  = '" & System.DateTime.Now().Date.ToString() & "'" & vbCrLf
            ' 37. 更新者個人ID
            strSql = strSql & "       ,c_user_id_up          = '" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 38. 更新回数
            strSql = strSql & "       ,s_up                  = s_up + 1" & vbCrLf
            strSql = strSql & "  WHERE c_ksh           = '" & iStrKsh & "'         " & vbCrLf   ' 会社コードと同じもの
            strSql = strSql & "    AND c_period_id     = '" & iStrPeriodId & "'    " & vbCrLf   ' 期IDと同じもの
            strSql = strSql & "    AND c_union_meeting = '" & iStrUnionMeeting & "'" & vbCrLf   ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND k_apply_area    = '" & iStrApplyArea & "'   " & vbCrLf   ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理判定
            If intRet <> 1 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateUnionInformationDtl
    '   名称　：組合大会通知詳細情報更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb                 As CLAccessMdb = データベースクラス,
    '           ByVal iStrKsh                As String      = 会社コード,
    '           ByVal iStrPeriodId           As String      = 期ID,
    '           ByVal iStrUnionMeeting       As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea          As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知詳細情報更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeeting">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateUnionInformationDtl(ByVal iClsDb As CLAccessMdb, _
                                               ByVal iStrKsh As String, _
                                               ByVal iStrPeriodId As String, _
                                               ByVal iStrUnionMeeting As String, _
                                               ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            ' SQL文作成
            strSql = ""
            strSql = strSql & " UPDATE union_information_dtl" & vbCrLf
            ' 05. 種類
            If Me.optInfomationName1.Checked Then
                strSql = strSql & "    SET k_union_meeting = '" & Me.optInfomationName1.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName2.Checked Then
                strSql = strSql & "    SET k_union_meeting = '" & Me.optInfomationName2.Text.Trim() & "'" & vbCrLf
            ElseIf Me.optInfomationName3.Checked Then
                strSql = strSql & "    SET k_union_meeting = '" & Me.optInfomationName3.Text.Trim() & "'" & vbCrLf
            End If
            ' 08. 更新日
            strSql = strSql & "       ,d_up            = '" & System.DateTime.Now().Date.ToString() & "'" & vbCrLf
            ' 09. 更新者個人ID
            strSql = strSql & "       ,c_user_id_up    = '" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 10. 更新回数
            strSql = strSql & "       ,s_up                  = s_up + 1" & vbCrLf
            strSql = strSql & "  WHERE c_ksh           = '" & iStrKsh & "'" & vbCrLf            ' 会社コードと同じもの
            strSql = strSql & "    AND c_period_id     = '" & iStrPeriodId & "'" & vbCrLf       ' 期IDと同じもの
            strSql = strSql & "    AND c_union_meeting = '" & iStrUnionMeeting & "'" & vbCrLf   ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND k_apply_area    = '" & iStrApplyArea & "'" & vbCrLf      ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理判定
            If intRet <> 1 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteUnionInformationMember
    '   名称　：組合大会通知メンバー情報削除処理
    '   概要  ：
    '   引数　：ByVal iClsDb             As CLAccessMdb = データベースクラス,
    '           ByVal iStrKsh            As String      = 会社コード,
    '           ByVal iStrPeriodId       As String      = 期ID,
    '           ByVal iStrUnionMeetingNo As String      = 組合大会会議番号,
    '           ByVal iStrApplyArea      As String      = 申請地区区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/17(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会通知メンバー情報削除処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrUnionMeetingNo">組合大会会議番号</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteUnionInformationMember(ByVal iClsDb As CLAccessMdb, _
                                                  ByVal iStrKsh As String, _
                                                  ByVal iStrPeriodId As String, _
                                                  ByVal iStrUnionMeetingNo As String, _
                                                  ByVal iStrApplyArea As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False             ' 処理結果
        Dim strSql As String = ""                 ' SQL文
        Dim intRet As Integer = 0                 ' 処理件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " DELETE" & vbCrLf
            strSql = strSql & "   FROM union_information_member" & vbCrLf                       ' 組合大会通知メンバー
            strSql = strSql & "  WHERE c_ksh           = '" & iStrKsh & "'" & vbCrLf            ' 会社コードと同じもの
            strSql = strSql & "    AND c_period_id     = '" & iStrPeriodId & "'" & vbCrLf       ' 期IDと同じもの
            strSql = strSql & "    AND c_union_meeting = '" & iStrUnionMeetingNo & "'" & vbCrLf ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND k_apply_area    = '" & iStrApplyArea & "'" & vbCrLf      ' 申請地区区分と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理判定
            If intRet = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetUnionMeetingNoSeq
    '   名称　：組合大会会議番号・組合大会会議SEQ取得処理
    '   概要  ：組合大会会議番号・組合大会会議SEQ取得処理を行う。
    '   引数　：ByVal iClsDb               As CLAccessMdb = データベースクラス,
    '           ByVal iStrKsh              As String      = 会社コード,
    '           ByVal iStrPeriodId         As String      = 期ID,
    '           ByVal iStrApplyArea        As String      = 申請地区区分,
    '           ByRef ioStrUnionMeetingNo  As String      = 組合大会会議番号,
    '           ByRef ioIntUnionMeetingSeq As Integer     = 組合大会会議SEQ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合大会会議番号・組合大会会議SEQ取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrApplyArea">申請地区区分</param>
    ''' <param name="ioStrUnionMeeteinNo">組合大会会議番号</param>
    ''' <param name="ioIntUnionMeetingSeq">組合大会会議SEQ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetUnionMeetingNoSeq(ByVal iClsDb As CLAccessMdb, _
                                          ByVal iStrKsh As String, _
                                          ByVal iStrPeriodId As String, _
                                          ByVal iStrApplyArea As String, _
                                          ByRef ioStrUnionMeeteinNo As String, _
                                          ByRef ioIntUnionMeetingSeq As Integer) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル
        Dim intRet As Integer = 0               ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合大会会議SEQ取得
            '-----------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT (MAX(a.s_union_meeting_seq) + 1) AS MaxNo" & vbCrLf
            strSql = strSql & "   FROM union_information AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_ksh        = '" & iStrKsh & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id  = '" & iStrPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' データ存在チェック
            If IsDBNull(dtRet.Rows(0).Item(0)) Then
                ' 0件の場合、0
                ioIntUnionMeetingSeq = 0
            Else
                ' 1件
                ioIntUnionMeetingSeq = CInt(dtRet.Rows(0).Item(0))
            End If

            ' 組合大会会議SEQが 0 の場合、期（数値のみ）+ " - 定期" を組合大会会議番号として取得
            ' 組合大会会議SEQが 0 以外の場合、期（数値のみ）+ " - " + 組合大会会議SEQ を組合大会会議番号として取得
            ' ※ 期（数値のみ）は、期マスタ（period） の 略名称（l_omission_name）
            If ioIntUnionMeetingSeq = 0 Then
                ioStrUnionMeeteinNo = MDLoginInfo.Period.ToString() & " - 定期"
            Else
                ioStrUnionMeeteinNo = MDLoginInfo.Period.ToString() & " - " & ioIntUnionMeetingSeq.ToString()
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal iBlnSearchFlg As Boolean = True：再検索有り, False：再検索無し
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/27(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/27(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <param name="iBlnSearchFlg">再検索フラグ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal iBlnSearchFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False                                   ' 処理結果
        Dim pn As Panel = ParentForm.Controls(MDConst.MAIN_PANEL_ID)    ' メインパネル
        Dim uc As Control = Nothing                                     ' ユーザコントロール
        Dim clsUC040101 As New UC040101                                 ' 組合大会通知 - 検索画面

        Try
            Cursor.Current = Cursors.WaitCursor                         ' カーソルを砂時計に設定
            uc = pn.Controls(SCREEN_ID_UC040101)                        ' 組合大会通知 - 検索画面
            If uc Is Nothing Then
                uc = New UC040101                                       ' 組合大会通知 - 検索画面生成
                Call pn.Controls.Add(uc)                                ' メインパネルに組合大会通知 - 検索画面追加
            Else
                clsUC040101 = pn.Controls(SCREEN_ID_UC040101)           ' 組合大会通知 - 検索画面
                clsUC040101.blnSearchFlg = iBlnSearchFlg                ' 再検索フラグ設定
                uc.Visible = True                                       ' 組合大会通知 - 検索画面表示
            End If
            Me.Dispose()                                                ' 組合大会通知 - 詳細画面閉じる

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetGrant
    '   名称　：権限取得処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetGrant() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim dtGrant As DataTable = Nothing      ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC040101)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString        ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString           ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString            ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString       ' ファイル出力権限
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：PrintPreview
    '   名称　：印刷プレビュー処理
    '   概要  ：
    '   引数　：ByVal iIntStyle As Integer     = スタイル（1：プレ印刷,
    '                                                      2：プレ印刷以外（登録・変更・中止））,
    '           ByVal iDtStafInfo As DataTable = 印刷社員リスト情報,
    '           ByVal iIntResPre  As Integer   = 印刷プレビュー押下ボタン
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/15(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>印刷プレビュー処理</summary>
    ''' <param name="iIntStyle">スタイル（1：プレ印刷,2：プレ印刷以外（登録・変更・中止））</param>
    ''' <param name="iDtStafInfo">社員情報</param>
    ''' <param name="iIntResPre">プレビュー画面押下ボタン</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function PrintPreview(ByVal iIntStyle As Integer, _
                                  ByVal iDtStafInfo As DataTable, _
                                  ByRef iIntResPre As Integer) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim clsFM000203 As FM000203 = Nothing                                               ' プレビュークラス
        Dim ds As DS0401P1 = Nothing                                                        ' 帳票用データセット
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing    ' レポートドキュメントオブジェクト
        Dim drDetail As DS0401P1.dtDetailRow = Nothing                                      ' ヘッダーロー
        Dim dtStafId As DataTable = Nothing                                                 ' 印刷メンバー（社員番号）

        Dim strTelTokyo As String = ""                                                      ' 電話番号（東京）
        Dim strFaxTokyo As String = ""                                                      ' FAX番号（東京）
        Dim strTelOosaka As String = ""                                                     ' 電話番号（大阪）
        Dim strFaxOosaka As String = ""                                                     ' FAX番号（大阪）

        Dim strNewUnionMeetingNo As String = ""                                             ' 取得したMAX組合大会会議番号 + 1
        Dim intNewUnionMeetingSeq As Integer = 0                                            ' 取得したMAX組合大会会議SEQ + 1

        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソル砂時計
            ds = New DS0401P1                                                               ' データセットクラス生成
            Call clsDb.Connect()                                                            ' データベース接続
            '-------------------------------------------------------------------------------
            '   電話番号（東京・大阪）・FAX番号（東京・大阪）取得処理
            '-------------------------------------------------------------------------------
            If GetTelFax(clsDb, _
                         strTelTokyo, _
                         strFaxTokyo, _
                         strTelOosaka, _
                         strFaxOosaka) = False Then
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   印刷情報設定
            '-------------------------------------------------------------------------------
            If iDtStafInfo IsNot Nothing Then

                For i = 0 To iDtStafInfo.Rows.Count - 1
                    ' 詳細ロー作成
                    drDetail = ds.dtDetail.NewRow
                    ' 詳細ロー編集開始
                    drDetail.BeginEdit()

                    With drDetail
                        ' 01. 組合大会会議番号
                        If Me.bytStatus = STATUS_INSERT Then
                            ' 開催登録
                            .c_union_meeting = "***"
                        Else
                            ' 変更・中止
                            .c_union_meeting = Me.intUnionMeetingSeq.ToString()
                        End If
                        ' 02. 申請地区区分
                        .apply_area = Me.cboApplyArea.Text.Trim()
                        ' 03. 社員番号
                        .c_staf_id = iDtStafInfo.Rows(i).Item("社員番号").ToString()
                        ' 04. 名前
                        .l_name = iDtStafInfo.Rows(i).Item("名前").ToString()
                        ' 05. 機種
                        .k_model = iDtStafInfo.Rows(i).Item("機種").ToString()
                        ' 06. 期略名称
                        .l_omission_name = MDLoginInfo.Period
                        ' 07. 種類
                        If Me.optInfomationName1.Checked Then
                            .information_type = Me.optInfomationName1.Text.Trim()   ' 開催
                        ElseIf Me.optInfomationName2.Checked Then
                            .information_type = Me.optInfomationName1.Text.Trim()   ' 変更の時も"開催"
                        ElseIf Me.optInfomationName3.Checked Then
                            .information_type = Me.optInfomationName3.Text.Trim()   ' 中止
                        End If
                        '=======================================
                        '   開始日時
                        '=======================================
                        ' 08. 会議日付1
                        .d_meeting_1 = Me.dtpMeeting1.Value
                        ' 09. 会議時間1From
                        .d_meeting_time_from_1 = MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom1.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom1.Text).Substring(2, 2)
                        ' 10. 会議時間1To
                        If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text)) = False Then
                            .d_meeting_time_to_1 = MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo1.Text).Substring(2, 2)
                        End If
                        ' 11. 会議場所1
                        .l_place_1 = Me.txtPlace1.Text.Trim()
                        ' 12. 移動フライト（往路）1
                        If MDChk.ChkNull(Me.txtLFlight1.Text.Trim()) = False Then
                            .l_flight_1 = Me.txtLFlight1.Text.Trim()
                        End If
                        ' 13. 移動フライト時間（往路）1
                        If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlight1.Text)) = False Then
                            .d_flight_1 = MDCommon.ReplaceTime(Me.mtbDFlight1.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbDFlight1.Text).Substring(2, 2)
                        End If
                        ' 14. 昼食可否1
                        If Me.chkLunch1.Checked Then
                            .k_lunch_1 = "1"        ' チェック有り
                        End If
                        '=======================================
                        '   終了日時
                        '=======================================
                        If Me.chkMeeting2.Checked Then
                            '-----------------------------------
                            '   終了日時チェック有り
                            '-----------------------------------
                            ' 15. 会議日付2
                            .d_meeting_2 = Me.dtpMeeting2.Value.Date
                            ' 16. 会議時間From2
                            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text)) = False Then
                                .d_meeting_time_from_2 = MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbMeetingTimeFrom2.Text).Substring(2, 2)
                            End If
                            ' 17. 会議時間To2
                            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text)) = False Then
                                .d_meeting_time_to_2 = MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbMeetingTimeTo2.Text).Substring(2, 2)
                            End If
                            ' 18. 会議場所2
                            If MDChk.ChkNull(Me.txtPlace2.Text.Trim()) = False Then
                                .l_place_2 = Me.txtPlace2.Text.Trim()
                            End If
                            ' 19. 移動フライト（往路）2
                            If MDChk.ChkNull(Me.txtLFlightBack2.Text.Trim()) = False Then
                                .l_flight_2 = Me.txtLFlightBack2.Text.Trim()
                            End If
                            ' 20. 移動フライト時間（往路）2
                            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text)) = False Then
                                .d_flight_2 = MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbDFlightBack2.Text).Substring(2, 2)
                            End If
                            ' 21. 昼食可否
                            If Me.chkLunch2.Checked Then
                                .k_lunch_2 = "1"
                            End If
                        Else
                            '-----------------------------------
                            '   終了日時チェック無し
                            '-----------------------------------
                            ' 19. 移動フライト（往路）2
                            If MDChk.ChkNull(Me.txtLFlightBack1.Text.Trim()) = False Then
                                .l_flight_2 = Me.txtLFlightBack1.Text.Trim()
                            End If
                            ' 20. 移動フライト時間（往路）2
                            If MDChk.ChkNull(MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text)) = False Then
                                .d_flight_2 = MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text).Substring(0, 2) & ":" & MDCommon.ReplaceTime(Me.mtbDFlightBack1.Text).Substring(2, 2)
                            End If
                        End If
                        ' 22. 開催場所
                        .open_belonging = Me.cboOpenBelonging.Text.Trim()
                        ' 23. 議題備考
                        If MDChk.ChkNull(Me.txtSubject1.Text.Trim()) = False Then
                            .l_subject_1 = Me.txtSubject1.Text.Trim()
                        End If
                        ' 24. 備考1
                        If MDChk.ChkNull(Me.txtBiko1.Text.Trim()) = False Then
                            .l_biko_1 = Me.txtBiko1.Text.Trim()
                        End If
                        ' 25. 備考2
                        If MDChk.ChkNull(Me.txtBiko2.Text.Trim()) = False Then
                            .l_biko_2 = Me.txtBiko2.Text.Trim()
                        End If
                        ' 26. 電話番号（東京）
                        .t_tel = strTelTokyo
                        ' 27. FAX番号（東京）
                        .t_fax = strFaxTokyo
                        ' 28. 電話番号（大阪）
                        .o_tel = strTelOosaka
                        ' 29. FAX番号（大阪）
                        .o_fax = strFaxOosaka
                        ' 30. 登録日
                        .d_up = Me.lblCreateDate.Text.Trim().Replace("/", "").Replace("-", "").Substring(0, 4) & "年" & Me.lblCreateDate.Text.Trim().Replace("/", "").Replace("-", "").Substring(4, 2) & "月" & Me.lblCreateDate.Text.Trim().Replace("/", "").Replace("-", "").Substring(6, 2) & "日"
                        ' 31. 日付1
                        .meeting_days_1 = DateAdd(DateInterval.Day, -4, Me.dtpMeeting1.Value)
                        ' 32. 日付2
                        .meeting_days_2 = DateAdd(DateInterval.Day, -3, Me.dtpMeeting1.Value)
                        ' 33. 日付3
                        .meeting_days_3 = DateAdd(DateInterval.Day, -2, Me.dtpMeeting1.Value)
                        ' 34. 日付4
                        .meeting_days_4 = DateAdd(DateInterval.Day, -1, Me.dtpMeeting1.Value)
                        ' 35. 組合大会種別
                        If Me.optUnionType1.Checked Then
                            .l_union_type = Me.optUnionType1.Text.Trim()    ' 合同
                        ElseIf Me.optUnionType2.Checked Then
                            .l_union_type = Me.optUnionType2.Text.Trim()    ' TV
                        ElseIf Me.optUnionType3.Checked Then
                            .l_union_type = Me.optUnionType3.Text.Trim()    ' 任意
                        End If
                        ' 36. 件名副題
                        If Me.optInfomationName1.Checked Then
                            .l_meeting_subject = MEETING_SUBJECT_OPEN       ' 開催
                        ElseIf Me.optInfomationName2.Checked Then
                            .l_meeting_subject = MEETING_SUBJECT_UPDATE     ' 変更
                        ElseIf Me.optInfomationName3.Checked Then
                            .l_meeting_subject = ""                         ' 中止
                        End If
                        ' 37. 夕食交流会1
                        If Me.chkExchangeMeeting1.Checked Then
                            .k_dinner_1 = "1"
                        End If
                        ' 38. 夕食交流会2
                        If Me.chkExchangeMeeting2.Checked Then
                            .k_dinner_2 = "1"
                        End If
                    End With

                    ' 詳細ロー編集終了
                    drDetail.EndEdit()
                    ' データセットに作成データロー設定
                    ds.dtDetail.Rows.Add(drDetail)

                Next
            End If

            '-------------------------------------------------------------------------------
            '   印刷処理準備
            '-------------------------------------------------------------------------------
            ' クラス生成
            clsFM000203 = New FM000203                          ' 印刷プレビュー画面
            reportObj = New CR0401P1                            ' レポートドキュメント生成
            ' プロパティ設定
            clsFM000203.ButtonShowType = iIntStyle              ' ボタン形式設定（3：プレ印刷時（[印刷] [キャンセル]）・1：プレ印刷以外（[登録&印刷] [登録のみ] [キャンセル]））
            clsFM000203.PrintCntVisible = False                 ' 印刷部数項目表示可否
            clsFM000203.ObjResource = reportObj                 ' レポート形式設定
            reportObj.SetDataSource(ds)                         ' データセット設定

            ' 印刷プレビュー画面表示
            Call clsFM000203.ShowDialog()

            ' 印刷プレビュー画面処理結果取得
            iIntResPre = clsFM000203.IntQlickBtnFlag

            '-------------------------------------------------------------------------------
            '   登録処理
            '-------------------------------------------------------------------------------
            If (iIntResPre = 0) _
            Or (iIntResPre = 1) _
            Or (iIntResPre = 3) Then

                ' [登録＆印刷] or [登録のみ] or [印刷]押下時、登録処理を行う。
                '---------------------------------------------------------------------------
                '   組合大会会議番号・組合大会会議SEQ番号取得
                '---------------------------------------------------------------------------
                If Me.bytStatus = STATUS_INSERT Then
                    ' 開催登録のみ組合大会会議番号・組合大会会議SEQ番号を採番
                    If GetUnionMeetingNoSeq(clsDb, _
                                            MDLoginInfo.Ksh, _
                                            MDLoginInfo.PeriodId, _
                                            Me.cboApplyArea.SelectedValue.ToString(), _
                                            strNewUnionMeetingNo, _
                                            intNewUnionMeetingSeq) = False Then
                        Return blnRet
                    End If

                ElseIf Me.bytStatus = STATUS_UPDATE _
                Or Me.bytStatus = STATUS_STOP _
                Or Me.bytStatus = STATUS_DETAIL _
                Or Me.bytStatus = STATUS_SAME Then
                    ' 変更・中止・プレ印刷・同番号申請の場合、そのまま使用
                    strNewUnionMeetingNo = Me.strUnionMeeting
                    intNewUnionMeetingSeq = Me.intUnionMeetingSeq
                End If

                '---------------------------------------------------------------------------
                '   登録・更新・削除処理
                '---------------------------------------------------------------------------
                ' 開催登録の場合、採番した組合大会会議番号・組合大会会議SEQ番号を使用
                ' 変更・中止の場合、そのままの組合大会会議番号・組合大会会議SEQ番号を使用
                If Me.InsertUpdateDelete(iDtStafInfo, _
                                         strNewUnionMeetingNo, _
                                         intNewUnionMeetingSeq) = False Then
                    Exit Function
                End If
            End If

            '-------------------------------------------------------------------------------
            '   印刷処理
            '-------------------------------------------------------------------------------
            If (iIntResPre = 0) _
            Or (iIntResPre = 3) Then
                ' [登録＆印刷] or [印刷] 押下時、印刷処理を行う。
                If Me.bytStatus = STATUS_INSERT _ 
                Or Me.bytStatus = STATUS_SAME Then
                    ' 取得した組合大会会議番号（組合大会会議SEQ）を帳票に表示
                    For i = 0 To ds.dtDetail.Rows.Count - 1
                        ds.dtDetail.Rows(i).Item(0) = intNewUnionMeetingSeq
                    Next
                    reportObj.SetDataSource(ds)
                End If
                clsFM000203.PrintOut()
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' データベース切断
            Call clsDb.Disconnect()
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default        ' カーソル初期
            Call clsDb.Disconnect()                 ' データベース切断
            ds.Dispose()                            ' データセット破棄
            reportObj.Dispose()                     ' レポートドキュメントオブジェクト破棄
            clsFM000203.Dispose()                   ' プレビュー画面クラス破棄
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetPrintMemberList
    '   名称　：印刷メンバーリスト取得処理
    '   概要  ：
    '   引数　：ByRef ioDtStafId As DataTable = 社員番号リスト
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/16(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>印刷メンバー取得</summary>
    ''' <param name="ioDtStafId">社員番号リスト</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetPrintMemberList(ByRef ioDtStafId As DataTable) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsFM000204 As FM000204 = Nothing       ' 配布者選択画面クラス
        Dim strStafId As String() = Nothing         ' 社員番号リスト

        Try
            '-------------------------------------------------------------------------------
            '   組合大会通知メンバー情報取得処理
            '-------------------------------------------------------------------------------
            If GetUnionInformationMember(strStafId) = False Then
                Exit Function
            End If
            '-------------------------------------------------------------------------------
            '   印刷メンバー取得
            '-------------------------------------------------------------------------------
            ' プロパティ設定
            clsFM000204 = New FM000204()                    ' インスタンス作成
            clsFM000204.StafIDList = strStafId              ' 社員番号リスト
            clsFM000204.AllowDeleteMember = True            ' 初期表示メンバー削除可能
            ' 配布者選択画面表示
            Call clsFM000204.ShowDialog()
            ' クリックされたボタンをチェック
            If clsFM000204.IntQlickBtnFlag = 0 Then
                ' OKボタン押下
                ioDtStafId = clsFM000204.SelectMemberList   ' 社員番号リスト取得
            ElseIf clsFM000204.IntQlickBtnFlag = 1 Then
                ' キャンセルボタン押下
                Return blnRet
            End If
            ' 不要になった時点で破棄
            clsFM000204.Close()                             ' 配布者選択画面閉じる
            clsFM000204.Dispose()                           ' 配布者選択画面破棄
            blnRet = True                                   ' 処理結果に正常設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetTelFax
    '   名称　：電話番号・FAX番号取得処理
    '   概要  ：
    '   引数　：ByVal iClsDb           As CLAccessMdb = データベースクラス,
    '           ByRef ioStrTokyoTelNo  As String      = 東京電話番号,
    '           ByRef ioStrTokyoFaxNo  As String      = 東京FAX番号,
    '           ByRef ioStrOosakaTelNo As String      = 大阪電話番号,
    '           ByRef ioStrOosakaFaxNo As String      = 大阪FAX番号
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/21(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/21(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>メンバー取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="ioStrTokyoTelNo">東京電話番号</param>
    ''' <param name="ioStrTokyoFaxNo">東京FAX番号</param>
    ''' <param name="ioStrOosakaTelNo">大阪電話番号</param>
    ''' <param name="ioStrOosakaFaxNo">大阪FAX番号</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetTelFax(ByVal iClsDb As CLAccessMdb, _
                               ByRef ioStrTokyoTelNo As String, _
                               ByRef ioStrTokyoFaxNo As String, _
                               ByRef ioStrOosakaTelNo As String, _
                               ByRef ioStrOosakaFaxNo As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL
        Dim dtRet As DataTable = Nothing        ' 処理結果データテーブル
        Dim intRet As Integer = 0               ' 処理結果件数

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_constant_seq" & vbCrLf               ' 定数ID枝番
            strSql = strSql & "       ,a.l_name" & vbCrLf                       ' 名称
            strSql = strSql & "       ,a.l_omission_name" & vbCrLf              ' 略名称（TelNo）
            strSql = strSql & "       ,a.l_omission_name_2" & vbCrLf            ' 略名称2（FaxNo）
            strSql = strSql & "   FROM constant_dtl AS a" & vbCrLf              ' 定数マスタ詳細
            strSql = strSql & "  WHERE a.c_constant = 'TEL_INFO'" & vbCrLf      ' 定数IDが 'TEL_INFO' のもの
            strSql = strSql & "  ORDER BY a.s_order" & vbCrLf                   ' 表示順で並び替え
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet <> 0 Then
                For i = 0 To intRet - 1                                         ' 件数分ループ
                    If dtRet.Rows(i).Item(0).ToString() = CONSTANT_DTL_TEL_INFO_TOKYO Then
                        ' 東京
                        ioStrTokyoTelNo = dtRet.Rows(i).Item(2).ToString()      ' 電話番号
                        ioStrTokyoFaxNo = dtRet.Rows(i).Item(3).ToString()      ' FAX番号
                    ElseIf dtRet.Rows(i).Item(0).ToString() = CONSTANT_DTL_TEL_INFO_OOSAKA Then
                        ' 大阪
                        ioStrOosakaTelNo = dtRet.Rows(i).Item(2).ToString()     ' 電話番号
                        ioStrOosakaFaxNo = dtRet.Rows(i).Item(3).ToString()     ' FAX番号
                    End If
                Next
            Else
                Call MessageBox.Show("データがありません！", _
                     "エラー", _
                     MessageBoxButtons.OK, _
                     MessageBoxIcon.Error, _
                     MessageBoxDefaultButton.Button1)
                Return blnRet
            End If

            ' 処理結果に正常設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SelectMemberPrint
    '   名称　：組合員選択後の印刷処理
    '   概要  ：
    '   引数　：ByVal iDtMemberList As DataTable = メンバー情報
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/16(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SelectMemberPrint(ByVal iDtMemberList As DataTable) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument          ' レポートオブジェクト
        Dim ds As DS0401P1 = New DS0401P1()                                             ' レポート用データセット
        Dim drDetail As DS0401P1.dtDetailRow = Nothing                                  ' 詳細データロー
        Dim fmPrint As FM000203 = Nothing                                               ' プレビュークラス

        Try
            ' 
            For i = 0 To iDtMemberList.Rows.Count - 1
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()


                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail)

            Next

            reportObj = New CR0401P1                ' レポートドキュメント生成
            fmPrint.ObjResource = reportObj         ' レポート形式設定
            reportObj.SetDataSource(ds)             ' データセット設定
            fmPrint.PrintOut()                      ' 印刷

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

#End Region

End Class

#End Region
