#Region "UC040306"
'===========================================================================================================
'   クラスＩＤ　　：UC040306
'   クラス名称　　：時間内組合活動 - 取消画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLAccessMdbMst
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDFile
Imports UnionAct.GUI.Document
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common

Public Class UC040306

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040306                              ' UC010101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040306                          ' 組合員検索画面
    ' 権限
    Private strGrantReference As String = "0"                                           ' 参照権限
    Private strGrantInsert As String = "0"                                              ' 登録権限
    Private strGrantPrint As String = "0"                                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                                          ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _strStrikeId As String = ""                                                  ' 時間内ストID
    Public _strKsh As String = ""                                                       ' 会社区分
    Public _strPeriodId As String = ""                                                  ' 期ID
    Public _strApplyArea As String = ""                                                 ' 申請地区区分　　　　　　（取消データ作成時必要）
    Public _strApplyClassify As String = ""                                             ' 種類区分　　　　　　　　（取消データ作成時必要）
    Public _strUnionInfoPeriodId As String = ""                                         ' 組合大会会議通知期ID　　（取消データ作成時必要）
    Public _strUnionInfoUnionMeeting As String = ""                                     ' 組合大会会議通知会議番号（取消データ作成時必要）
    Public _numUseCol As Integer = 5                                                    ' 表示日付件数(初期値5)

    ' 時間内ストID
    Public Property strStrikeId() As String
        Get
            Return _strStrikeId
        End Get
        Set(ByVal value As String)
            _strStrikeId = value
        End Set
    End Property
    ' 会社区分
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
    ' 申請地区区分
    Public Property strApplyArea() As String
        Get
            Return _strApplyArea
        End Get
        Set(ByVal value As String)
            _strApplyArea = value
        End Set
    End Property
    ' 種類区分
    Public Property strApplyClassify() As String
        Get
            Return _strApplyClassify
        End Get
        Set(ByVal value As String)
            _strApplyClassify = value
        End Set
    End Property
    ' 組合大会会議通知期ID
    Public Property strUnionInfoPeriodId() As String
        Get
            Return _strUnionInfoPeriodId
        End Get
        Set(ByVal value As String)
            _strUnionInfoPeriodId = value
        End Set
    End Property
    ' 組合大会会議通知会議番号
    Public Property strUnionInfoUnionMeeting() As String
        Get
            Return _strUnionInfoUnionMeeting
        End Get
        Set(ByVal value As String)
            _strUnionInfoUnionMeeting = value
        End Set
    End Property
    ' 表示日付件数
    Public Property numUseCol() As Integer
        Get
            Return _numUseCol
        End Get
        Set(ByVal value As Integer)
            _numUseCol = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC040306_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040306_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If SetGrant() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If ControlClear() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If GetData() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   画面表示処理
            '-------------------------------------------------------------------------------
            If ChangeDisplay(Me.strApplyClassify) = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   コントロールロックアンロック処理
            '-------------------------------------------------------------------------------
            If ControlRockUnLock() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancelChk_Click
    '   名称　：取消確認ボタンクリック処理
    '   概要　：
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancelChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelChk.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim daiRet As DialogResult = Nothing                                                ' 確認メッセージ結果
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            '-------------------------------------------------------------------------------
            '   入力チェック
            '-------------------------------------------------------------------------------
            If ChkInput() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   取消確認メッセージ表示
            '-------------------------------------------------------------------------------
            daiRet = CLMsg.Show("GQ0001")                                                   ' 登録確認メッセージ表示
            If daiRet = DialogResult.No Then                                                ' 確認メッセージ判定
                Exit Sub                                                                    ' 「いいえ」押下時、処理を抜ける
            End If
            '-------------------------------------------------------------------------------
            '   印刷プレビュー処理
            '-------------------------------------------------------------------------------
            If PrintPreview() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   画面遷移処理（時間内組合活動画面）
            '-------------------------------------------------------------------------------
            If TransitionScreen() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default                                                ' カーソルを矢印に戻す
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim daiRet As DialogResult = Nothing                                                ' 確認メッセージ結果
        Try
            '-------------------------------------------------------------------------------
            '   確認メッセージ表示
            '-------------------------------------------------------------------------------
            daiRet = CLMsg.Show("GQ0007")
            If daiRet = DialogResult.No Then                                                ' 入力・変更内容破棄メッセージボックス表示
                Exit Sub                                                                    ' 「いいえ」ボタン押下時、処理を抜ける
            End If
            '-------------------------------------------------------------------------------
            '   画面遷移処理（時間内組合活動画面）
            '-------------------------------------------------------------------------------
            If TransitionScreen() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：likMemo_Click
    '   名称　：覚書を表示リンクラベルクリック処理
    '   概要　：覚書のPDFを表示する。
    '   作成日：2012/01/24(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/24(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub likMemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles likMemo.Click
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            ' 覚書を表示処理
            If ShowOboegaki() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default                                                ' カーソルを矢印に戻す
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cfgResult_Click
    '   名称　：フレックスグリッドクリック処理
    '   概要　：日付1～日付5列をチェックして、対象データがあればセルに取消画像を表示する。
    '   作成日：2012/01/23(月) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cfgResult_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgResult.Click
        Dim row As Integer = 0                                                          ' 行
        Dim col As Integer = 0                                                          ' 列
        Try
            ' 取消画像があるかチェック
            If FileExists(MDSystemInfo.AppPath & FILE_CANCEL) = False Then
                Call MessageBox.Show("取消画像がありません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Exit Sub
            End If
            With Me.cfgResult
                If .HitTest.Type.Equals(HitTestTypeEnum.ColumnHeader) = False Then      ' ヘッダー行以外がクリックされたかチェック
                    If .HitTest.Type.Equals(HitTestTypeEnum.Cell) Then                  ' セルがクリックされたかチェック
                        row = .HitTest.Row                                              ' 行位置取得
                        col = .HitTest.Column                                           ' 列位置取得
                        If row > 0 And col > 4 Then                                     ' 日付列がクリックされたいるかチェック
                            If .GetData(row, col).ToString().Length <> 0 Then           ' 日付列があるかチェック
                                If (.GetCellImage(row, col) Is Nothing) Then
                                    .SetCellImage(row, col, Image.FromFile(MDSystemInfo.AppPath & FILE_CANCEL))
                                Else
                                    .SetCellImage(row, col, Nothing)                    ' 日付が表示されていない（=既に解除済み）の場合は画像を表示しない
                                End If
                            End If
                        End If
                    End If
                End If
            End With
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Try
            Me.txtApplyArea.Text = ""                                                   ' 支部テキストボックス
            Me.txtApplyNo.Text = ""                                                     ' 申請番号テキストボックス
            Me.txtApplyDate.Text = ""                                                   ' 申請日テキストボックス
            Me.txtApplyClassify.Text = ""                                               ' 種類テキストボックス
            Me.lblOmissionName.Text = ""                                                ' 略名称ラベル
            Me.cboMeeting.DataSource = Nothing                                          ' 会議名テキストボックス
            Me.txtTerm.Text = ""                                                        ' 開催期間テキストボックス
            Me.txtMeetingNo.Text = ""                                                   ' 組合大会番号テキストボックス
            Me.txtStandName.Text = ""                                                   ' 申請者テキストボックス
            ' 差替え
            Me.chkReplace.Checked = False                                               ' 差替えチェックボックス
            Me.btnReplace.Visible = True                                                ' 差替内容表示ボタン
            Me.txtReplaceNo.Text = ""                                                   ' 差戻申請番号テキストボックス

            Me.grpNameAndDate.Visible = True                                            ' グループボックス
            Me.cfgResult.Visible = True                                                 ' C1FlexGrid表示

            Me.btnPrint.Visible = False                                                 ' 印刷ボタン
            Me.btnCancelChk.Visible = True                                              ' 取消確認ボタン
            Me.btnCancel.Visible = True                                                 ' キャンセルボタン

            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：C1FlexGridIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 日付15対応
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function C1FlexGridIni() As Boolean
        Dim blnRet As Boolean = False                                                           ' 処理結果
        Try
            With Me.cfgResult
                '-------------------------------------------------------------------------------
                '   グリッド全体設定
                '-------------------------------------------------------------------------------
                ' 総数
                .Rows.Count = 1                                                                 ' 縦
                .Cols.Count = 20                                                                ' 横
                ' 固定行
                .Rows.Fixed = 1                                                                 ' 縦
                .Cols.Fixed = 0                                                                 ' 横
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                   ' 縦横両方
                ' 1行選択モード
                .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell                       ' セル選択
                '.MultiSelect = False                                                            ' 複数選択なし
                ' サイズ変更
                '.AllowUserToResizeColumns = True                                                ' 列サイズ変更可
                '.AllowUserToResizeRows = False                                                  ' 行サイズ変更不可
                ' バックカラー
                '.RowsDefaultCellStyle.BackColor = Color.White                                   ' 全ての列の背景色を白色
                .AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
                .AutoResize = False                                                             ' 列幅自動調節なし
                '-------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-------------------------------------------------------------------------------
                ' ヘッダー文字列
                .Cols(0).Caption = "社員番号"                                                   ' 社員番号
                .Cols(1).Caption = "氏名"                                                       ' 氏名
                .Cols(2).Caption = "所属"                                                       ' 所属（略名称）
                .Cols(3).Caption = "機種"                                                       ' 機種（略名称）
                .Cols(4).Caption = "資格"                                                       ' 資格（略名称）
                .Cols(5).Caption = "日付1"                                                      ' 日付1
                .Cols(6).Caption = "日付2"                                                      ' 日付2
                .Cols(7).Caption = "日付3"                                                      ' 日付3
                .Cols(8).Caption = "日付4"                                                      ' 日付4
                .Cols(9).Caption = "日付5"                                                      ' 日付5
                .Cols(10).Caption = "日付6"                                                     ' 日付6
                .Cols(11).Caption = "日付7"                                                     ' 日付7
                .Cols(12).Caption = "日付8"                                                     ' 日付8
                .Cols(13).Caption = "日付9"                                                     ' 日付9
                .Cols(14).Caption = "日付10"                                                    ' 日付10
                .Cols(15).Caption = "日付11"                                                    ' 日付11
                .Cols(16).Caption = "日付12"                                                    ' 日付12
                .Cols(17).Caption = "日付13"                                                    ' 日付13
                .Cols(18).Caption = "日付14"                                                    ' 日付14
                .Cols(19).Caption = "日付15"                                                    ' 日付15
                ' ヘッダー文字位置
                .Cols(0).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 社員番号
                .Cols(1).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 氏名
                .Cols(2).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 所属（略名称）
                .Cols(3).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 機種（略名称）
                .Cols(4).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 資格（略名称）
                .Cols(5).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 日付1
                .Cols(6).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 日付2
                .Cols(7).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 日付3
                .Cols(8).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 日付4
                .Cols(9).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter          ' 日付5
                .Cols(10).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付6
                .Cols(11).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付7
                .Cols(12).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付8
                .Cols(13).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付9
                .Cols(14).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付10
                .Cols(15).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付11
                .Cols(16).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付12
                .Cols(17).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付13
                .Cols(18).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付14
                .Cols(19).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter         ' 日付15
                '-------------------------------------------------------------------------------
                '   カラム部設定
                '-------------------------------------------------------------------------------
                ' カラム文字位置
                .Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter                ' 社員番号
                .Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                 ' 名前
                .Cols(2).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 所属（略名称）
                .Cols(3).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 機種（略名称）
                .Cols(4).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 資格（略名称）
                .Cols(5).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 日付1
                .Cols(6).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 日付2
                .Cols(7).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 日付3
                .Cols(8).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 日付4
                .Cols(9).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter               ' 日付5
                .Cols(10).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付6
                .Cols(11).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付7
                .Cols(12).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付8
                .Cols(13).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付9
                .Cols(14).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付10
                .Cols(15).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付11
                .Cols(16).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付12
                .Cols(17).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付13
                .Cols(18).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付14
                .Cols(19).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 日付15
                ' カラム幅
                .AutoResize = True
                ' カラムバックカラー
                .Cols(0).StyleNew.BackColor = Drawing.Color.LightYellow                         ' 社員番号
                .Cols(1).StyleNew.BackColor = Drawing.Color.LightYellow                         ' 名前
                .Cols(2).StyleNew.BackColor = Drawing.Color.LightYellow                         ' 所属（略名称）
                .Cols(3).StyleNew.BackColor = Drawing.Color.LightYellow                         ' 機種（略名称）
                .Cols(4).StyleNew.BackColor = Drawing.Color.LightYellow                         ' 資格（略名称）
                .Cols(5).StyleNew.BackColor = Drawing.Color.White                               ' 日付1
                .Cols(6).StyleNew.BackColor = Drawing.Color.White                               ' 日付2
                .Cols(7).StyleNew.BackColor = Drawing.Color.White                               ' 日付3
                .Cols(8).StyleNew.BackColor = Drawing.Color.White                               ' 日付4
                .Cols(9).StyleNew.BackColor = Drawing.Color.White                               ' 日付5
                .Cols(10).StyleNew.BackColor = Drawing.Color.White                              ' 日付6
                .Cols(11).StyleNew.BackColor = Drawing.Color.White                              ' 日付7
                .Cols(12).StyleNew.BackColor = Drawing.Color.White                              ' 日付8
                .Cols(13).StyleNew.BackColor = Drawing.Color.White                              ' 日付9
                .Cols(14).StyleNew.BackColor = Drawing.Color.White                              ' 日付10
                .Cols(15).StyleNew.BackColor = Drawing.Color.White                              ' 日付11
                .Cols(16).StyleNew.BackColor = Drawing.Color.White                              ' 日付12
                .Cols(17).StyleNew.BackColor = Drawing.Color.White                              ' 日付13
                .Cols(18).StyleNew.BackColor = Drawing.Color.White                              ' 日付14
                .Cols(19).StyleNew.BackColor = Drawing.Color.White                              ' 日付15
                ' カラム表示有無
                .Cols(0).Visible = True                                                         ' 社員番号
                .Cols(1).Visible = True                                                         ' 名前
                .Cols(2).Visible = True                                                         ' 所属（略名称）
                .Cols(3).Visible = True                                                         ' 機種（略名称）
                .Cols(4).Visible = True                                                         ' 資格（略名称）
                .Cols(5).Visible = True                                                         ' 日付1
                .Cols(6).Visible = True                                                         ' 日付2
                .Cols(7).Visible = True                                                         ' 日付3
                .Cols(8).Visible = True                                                         ' 日付4
                .Cols(9).Visible = True                                                         ' 日付5
                .Cols(10).Visible = False                                                       ' 日付6
                .Cols(11).Visible = False                                                       ' 日付7
                .Cols(12).Visible = False                                                       ' 日付8
                .Cols(13).Visible = False                                                       ' 日付9
                .Cols(14).Visible = False                                                       ' 日付10
                .Cols(15).Visible = False                                                       ' 日付11
                .Cols(16).Visible = False                                                       ' 日付12
                .Cols(17).Visible = False                                                       ' 日付13
                .Cols(18).Visible = False                                                       ' 日付14
                .Cols(19).Visible = False                                                       ' 日付15
                ' カラム編集可否
                .Cols(0).AllowEditing = False                                                   ' 社員番号
                .Cols(1).AllowEditing = False                                                   ' 名前
                .Cols(2).AllowEditing = False                                                   ' 所属（略名称）
                .Cols(3).AllowEditing = False                                                   ' （略名称）
                .Cols(4).AllowEditing = False                                                   ' （略名称）
                .Cols(5).AllowEditing = False                                                   ' 日付1
                .Cols(6).AllowEditing = False                                                   ' 日付2
                .Cols(7).AllowEditing = False                                                   ' 日付3
                .Cols(8).AllowEditing = False                                                   ' 日付4
                .Cols(9).AllowEditing = False                                                   ' 日付5
                .Cols(10).AllowEditing = False                                                  ' 日付6
                .Cols(11).AllowEditing = False                                                  ' 日付7
                .Cols(12).AllowEditing = False                                                  ' 日付8
                .Cols(13).AllowEditing = False                                                  ' 日付9
                .Cols(14).AllowEditing = False                                                  ' 日付10
                .Cols(15).AllowEditing = False                                                  ' 日付11
                .Cols(16).AllowEditing = False                                                  ' 日付12
                .Cols(17).AllowEditing = False                                                  ' 日付13
                .Cols(18).AllowEditing = False                                                  ' 日付14
                .Cols(19).AllowEditing = False                                                  ' 日付15
            End With
            blnRet = True                                                                       ' 戻り値格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                           ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：組合員種別コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As CLAccessMdb = Nothing                                                  ' データベースクラス
        Try
            clsDb = New CLAccessMdb                                                         ' データベースクラス生成
            clsDb.Connect()                                                                 ' データベース接続
            ' 時間内組合活動情報取得処理
            If GetApplyStrike(clsDb) = False Then
                Return blnRet
            End If
            ' 時間内組合活動メンバー日程情報処理
            If GetApplyStrikeMemberDate(clsDb) = False Then
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()                                                              ' データベース切断
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：ByVal pBlnEdit As Boolean = True：アンロック, False：ロック
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/27(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/27(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            ' TextBox
            Me.txtApplyArea.ReadOnly = True                                                 ' 支部テキストボックス
            Me.txtApplyNo.ReadOnly = True                                                   ' 申請番号テキストボックス
            Me.txtApplyDate.ReadOnly = True                                                 ' 申請日テキストボックス
            Me.txtApplyClassify.ReadOnly = True                                             ' 種類テキストボックス
            Me.txtMeetingNo.ReadOnly = True                                                 ' 組合大会番号テキストボックス
            Me.txtTerm.ReadOnly = True                                                      ' 開催期間テキストボックス
            Me.txtReplaceNo.ReadOnly = True                                                 ' 差替え申請番号テキストボックス
            Me.txtStandName.ReadOnly = True                                                 ' 申請者テキストボックス
            ' ComboBox
            Call Utilities.SetCanEditToControl(False, Me.cboMeeting)                        ' 会議名コンボボックス
            ' CheckBox
            Call Utilities.SetCanEditToControl(False, Me.chkReplace)                        ' 差替えチェックボックス
            ' Button
            Me.btnPrint.Visible = False                                                     ' 印刷ボタン
            Me.btnReplace.Visible = False                                                   ' 差替え内容表示ボタン
            Me.btnCancel.Visible = True                                                     ' キャンセルボタン
            Me.btnCancelChk.Visible = True                                                  ' 取消確認ボタン
            ' バックカラー
            Me.txtApplyArea.BackColor = Color.LightYellow                                   ' 支部テキストボックス
            Me.txtApplyNo.BackColor = Color.LightYellow                                     ' 申請番号テキストボックス
            Me.txtApplyDate.BackColor = Color.LightYellow                                   ' 申請日テキストボックス
            Me.txtApplyClassify.BackColor = Color.LightYellow                               ' 種類テキストボックス
            Me.txtMeetingNo.BackColor = Color.LightYellow                                   ' 組合大会番号テキストボックス
            Me.txtTerm.BackColor = Color.LightYellow                                        ' 開催期間テキストボックス
            Me.txtReplaceNo.BackColor = Color.LightYellow                                   ' 差替え申請番号テキストボックス
            Me.txtStandName.BackColor = Color.LightYellow                                   ' 申請者テキストボックス
            Me.cboMeeting.BackColor = Color.LightYellow                                     ' 会議名コンボボックス
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim daiRet As DialogResult = Nothing                                                ' 確認メッセージ結果
        Try
            '-------------------------------------------------------------------------------
            '   フレックスグリッド内画像有無チェック処理
            '-------------------------------------------------------------------------------
            If ChkCellImage(Me.cfgResult, 1, 5, (Me.numUseCol + 4)) = False Then
                Call CLMsg.Show("GE0042")
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   フレックスグリッド内過去日付有無チェック処理
            '-------------------------------------------------------------------------------
            If ChkCellPastDate(Me.cfgResult, 1, 5, (Me.numUseCol + 4)) Then
                daiRet = CLMsg.Show("GW0017")                                               ' メッセージボックス表示（取消日付が過去日付（今日含む））
                If daiRet = DialogResult.No Then                                            ' メッセージ判定
                    Return blnRet                                                           ' 「いいえ」押下時、処理を抜ける
                End If
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUpdate
    '   名称　：時間内組合活動情報更新処理（登録・更新）
    '   概要  ：時間内組合活動情報の登録更新処理を行う。
    '   引数　：ByRef oStrApplyNo As String = 取得したMAX申請番号 + 1
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '         ：2012/08/07(火) Fujisaku  変更 発番ルール修正
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動情報更新処理（登録・更新）</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdate( _
        ByRef oStrApplyNo As String _
    ) As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス（ローカルレプリカ）
        Dim strMaxApplyNo As String = ""                                                    ' 申請書番号
        Dim strNewStrikeId As String = ""                                                   ' 新時間内ストID

        Try
            '-------------------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------------------
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            Call FrmWaitInfo.ShowWaitForm(Nothing)                                          ' しばらくお待ちくださいフォーム表示
            ' 同期処理による最新データの取得 SEQ対応によって前同期を省略 2013/04/19
            'Call syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)

            '===============================================================================================================
            '   データベース接続
            '===============================================================================================================
            Call clsDb.Connect()                ' ローカルレプリカ

            '---------------------------------------------------------------------------------------------------------------
            '   トランザクション開始
            '---------------------------------------------------------------------------------------------------------------
            Call clsDb.BeginTran()              ' ローカルレプリカ

            '===============================================================================================================
            '
            '   登録処理
            '
            '===============================================================================================================
            ' MAX申請書番号取得処理
            strMaxApplyNo = GetNewApplyNo(clsDb)
            If strMaxApplyNo.Length = 0 Then
                Return blnRet
            End If
            ' Output引数に格納
            oStrApplyNo = strMaxApplyNo
            ' 時間内ストID（元申請期 + "-" + MAX申請番号 + 1）
            strNewStrikeId = Split(Me.txtApplyNo.Text, "-")(0) & "-" & strMaxApplyNo

            '===============================================================================================================
            '   時間内組合活動情報（取消データ作成）存在確認処理
            '===============================================================================================================
            If ExistsApplyStrike( _
                clsDb, _
                strNewStrikeId, _
                Me.strKsh, _
                Me.strPeriodId, _
                Me.strApplyArea _
            ) = False Then

                '===========================================================================================================
                '   時間内組合活動登録処理（取消データ作成）
                '===========================================================================================================
                If InsertApplyStrike(
                    clsDb,
                    strNewStrikeId,
                    Me.strKsh,
                    Me.strPeriodId,
                    strMaxApplyNo
                ) = False Then
                    ' トランザクション取消
                    Call clsDb.RollbackTran()               ' ローカルレプリカ
                    'Call CLMsg.Show("FE0001")              ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If
            Else
                ' トランザクション取消
                Call clsDb.RollbackTran()                   ' ローカルレプリカ
                Call CLMsg.Show("BE0005")                   ' 予期しないエラーメッセージ表示
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   更新・登録処理
            '-------------------------------------------------------------------------------
            For i = 1 To Me.cfgResult.Rows.Count - 1                                        ' レコード件数分ループ
                For j = 5 To (Me.numUseCol + 4)                                             ' 最大で日付1～日付15までループ
                    If Not (Me.cfgResult.GetCellImage(i, j) Is Nothing) Then                ' 画像があるかチェック
                        '===================================================================
                        '   登録
                        '===================================================================
                        ' 時間内組合活動メンバー日程情報存在確認処理
                        If ExistsApplyStrikeMemberDate( _
                            clsDb, _
                            strNewStrikeId, _
                            Me.strApplyArea, _
                            Me.cfgResult.GetData(i, 0).ToString(), _
                            CDate(Me.cfgResult.GetData(i, j)), _
                            "0" _
                        ) = False Then

                            '===============================================================================================
                            '   時間内組合活動メンバー日程登録処理
                            '===============================================================================================
                            If InsertApplyStrikeMemberDate(
                                clsDb,
                                strNewStrikeId,
                                Me.cfgResult.GetData(i, 0).ToString(),
                                CDate(Me.cfgResult.GetData(i, j))
                            ) = False Then
                                ' トランザクション取消
                                Call clsDb.RollbackTran()                                   ' ローカルレプリカ
                                Call CLMsg.Show("DE0005")                                   ' 正しくデータが更新できませんの旨のメッセージ表示
                                Return blnRet
                            End If
                        Else
                            ' トランザクション取消
                            Call clsDb.RollbackTran()                                       ' ローカルレプリカ
                            Call CLMsg.Show("GE0052")                                       ' 他のユーザによって更新された可能性の旨のメッセージ表示
                            Return blnRet
                        End If

                        '===================================================================================================
                        '
                        '   更新
                        '
                        '===================================================================================================
                        '===================================================================================================
                        '   時間内組合活動メンバー日程情報存在確認処理
                        '===================================================================================================
                        If ExistsApplyStrikeMemberDate( _
                            clsDb, _
                            Me.strStrikeId, _
                            Me.strApplyArea, _
                            Me.cfgResult.GetData(i, 0).ToString(), _
                            CDate(Me.cfgResult.GetData(i, j)), _
                            "0" _
                        ) Then
                            '===============================================================================================
                            '   時間内組合活動メンバー日程更新処理
                            '===============================================================================================
                            If UpdateApplyStrikeMemberDate(
                                clsDb,
                                strNewStrikeId,
                                Me.strStrikeId,
                                Me.strApplyArea,
                                Me.cfgResult.GetData(i, 0).ToString(),
                                CDate(Me.cfgResult.GetData(i, j)),
                                "0"
                            ) = False Then
                                ' トランザクション取消
                                Call clsDb.RollbackTran()                                   ' ローカルレプリカ
                                Call CLMsg.Show("DE0005")                                   ' 正しくデータが更新できませんの旨のメッセージ表示
                                Return blnRet
                            End If
                        Else
                            ' トランザクション取消
                            Call clsDb.RollbackTran()                                       ' ローカルレプリカ
                            Call CLMsg.Show("GE0052")                                       ' 他のユーザによって更新された可能性の旨のメッセージ表示
                            Return blnRet
                        End If
                    End If
                Next
            Next

            ' トランザクション確定
            Call clsDb.CommitTran()                                                         ' ローカルレプリカ

            ' SEQUENCファイルに値を反映
            Dim strSeqName As String = "seq_apl_err_306.txt"
            If Me.strApplyArea = "01" Then
                strSeqName = "seq_apl_tyo_" + Me.strPeriodId + ".txt"
            ElseIf Me.strApplyArea = "02" Then
                strSeqName = "seq_apl_osa_" + Me.strPeriodId + ".txt"
            End If
            Dim sw As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName, False)
            sw.Write(strMaxApplyNo)
            sw.Close()
            'サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
            'Call syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)                          ' 同期処理による最新データの反映

            blnRet = True                                                                   ' 処理結果に正常を設定

        Catch ex As Exception
            '---------------------------------------------------------------------------------------------------------------
            '   エラー処理
            '---------------------------------------------------------------------------------------------------------------
            Call clsDb.RollbackTran()                                                       ' ローカルレプリカ
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            '===============================================================================================================
            '   データベース切断
            '===============================================================================================================
            Call clsDb.Disconnect()                                                         ' データベース切断（ローカルレプリカ）

            '---------------------------------------------------------------------------------------------------------------
            '   データベースオブジェクト開放
            '---------------------------------------------------------------------------------------------------------------
            ' ローカルレプリカ
            If Not clsDb Is Nothing Then
                clsDb = Nothing
            End If

            Call FrmWaitInfo.CloseWaitForm()                                                ' しばらくお待ちくださいフォームクローズ
            Cursor.Current = Cursors.Default                                                ' カーソルを矢印に戻す
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetApplyStrike
    '   名称　：時間内組合活動情報取得処理
    '   概要  ：時間内組合活動情報を取得する。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/21(土) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/21(土) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetApplyStrike(ByVal iClsDb As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim strSql As String = ""                                                       ' SQL文
        Dim tbRet As DataTable                                                          ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing                                              ' 処理件数
        Dim strVersion As String = String.Empty                                         ' 更新回数
        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT strike.c_strike_id   AS StrikeId" & vbCrLf                           ' 01. 時間内ストID
            strSql = strSql & "       ,ctd1.l_name          AS ApplyAreaName" & vbCrLf                      ' 02. 申請地区区分名称
            strSql = strSql & "       ,strike.k_apply_area  AS ApplyArea" & vbCrLf                          ' 03. 申請地区区分
            strSql = strSql & "       ,strike.c_application AS ApplyNumber" & vbCrLf                        ' 04. 申請番号
            strSql = strSql & "       ,FORMAT(strike.d_application, 'yyyy/MM/dd' ) AS ApplyDate" & vbCrLf   ' 05. 申請日
            strSql = strSql & "       ,ctd2.l_name          AS ApplyClassifyName" & vbCrLf                  ' 06. 種類名称
            strSql = strSql & "       ,strike.k_apply_classify AS ApplyClassify" & vbCrLf                   ' 07. 種類コード
            strSql = strSql & "       ,ctd2.l_omission_name AS OmissionName" & vbCrLf                       ' 08. 略名称
            strSql = strSql & "       ,strike.l_meeting     AS Meeting" & vbCrLf                            ' 09. 会議名
            strSql = strSql & "       ,strike.union_info_c_period_id AS UnionInfoPeriodId" & vbCrLf         ' 10. 組合大会会議通知期ID
            strSql = strSql & "       ,strike.union_info_c_union_meeting AS UnionInfoUnionMeeting" & vbCrLf ' 11. 組合大会会議通知会議番号
            strSql = strSql & "       ," & UtDb.DbStrYYYYMMDDtoDateText("prd.d_from") & " AS d_from" & vbCrLf               ' 12. 開催期間（開始）
            strSql = strSql & "       ," & UtDb.DbStrYYYYMMDDtoDateText("prd.d_to") & " AS d_to" & vbCrLf                   ' 13. 開催期間（終了）
            strSql = strSql & "       ,strike.l_stand_name  AS StandName" & vbCrLf                          ' 14. 代表者名内容（申請者）
            strSql = strSql & "       ,strike.s_up  AS s_up" & vbCrLf                                       ' 15. 更新回数
            strSql = strSql & "   FROM apply_strike AS strike" & vbCrLf                     ' 時間内組合活動
            strSql = strSql & "       ,constant_dtl AS ctd1" & vbCrLf                       ' 定数マスタ詳細（地区）
            strSql = strSql & "       ,constant_dtl AS ctd2" & vbCrLf                       ' 定数マスタ詳細（種類）
            strSql = strSql & "       ,period AS prd" & vbCrLf                              ' 期マスタ
            strSql = strSql & "  WHERE strike.k_apply_area     = ctd1.c_constant_seq" & vbCrLf
            strSql = strSql & "    AND ctd1.c_constant         = 'APPLY_AREA'" & vbCrLf
            strSql = strSql & "    AND strike.k_apply_classify = ctd2.c_constant_seq" & vbCrLf
            strSql = strSql & "    AND ctd2.c_constant         = 'APPLY_CLASSIFY'" & vbCrLf
            strSql = strSql & "    AND strike.c_period_id      = prd.c_period_id" & vbCrLf
            strSql = strSql & "    AND strike.c_strike_id      = '" & Me.strStrikeId & "'" & vbCrLf
            strSql = strSql & "    AND strike.c_ksh            = '" & Me.strKsh & "'" & vbCrLf
            strSql = strSql & "    AND strike.c_period_id      = '" & Me.strPeriodId & "'" & vbCrLf
            strSql = strSql & "    AND strike.k_apply_area      = '" & Me.strApplyArea & "'" & vbCrLf
            strSql = strSql & "    AND strike.k_cancel         = '0'" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            tbRet = iClsDb.ExecuteSql(strSql)                                               ' SQL実行
            intCntRet = tbRet.Rows.Count                                                    ' 処理件数取得
            If intCntRet = 1 Then
                With tbRet.Rows(0)
                    Me.txtApplyArea.Text = .Item(1).ToString()                              ' 申請地区区分名称
                    Me.strApplyArea = .Item(2).ToString()                                   ' 申請地区区分
                    Me.txtApplyNo.Text = .Item(0).ToString()                                ' 申請番号（時間内ストID）
                    Me.txtApplyDate.Text = .Item(4).ToString()                              ' 申請日
                    Me.txtApplyClassify.Text = .Item(5).ToString()                          ' 種類名称
                    Me.strApplyClassify = .Item(6).ToString()                               ' 種類コード
                    Me.lblOmissionName.Text = .Item(7).ToString()                           ' 略名称
                    Me.cboMeeting.Text = .Item(8).ToString()                                ' 会議名
                    '-----------------------------------------------------------------------
                    '   開催期間 or 組合大会番号
                    '-----------------------------------------------------------------------
                    If (.Item(6).ToString() = APPLY_CLASSIFY_CENTRAL_EXECUTIVE) _
                    Or (.Item(6).ToString() = APPLY_CLASSIFY_CENTRAL) Then
                        ' 中央執行委員会か中央委員会
                        If GetApplyStrikeExecutiveTerm(iClsDb, CInt(.Item(10).ToString())) = False Then
                            Return blnRet
                        End If
                        ' プロパティに確保
                        Me.strApplyClassify = .Item(6).ToString()                           ' 種類コード
                        Me.strUnionInfoUnionMeeting = .Item(10).ToString()                  ' 組合大会会議通知会議番号
                    ElseIf .Item(6).ToString() = APPLY_CLASSIFY_GENERAL_MEETING Then
                        ' 組合大会
                        Me.txtMeetingNo.Text = .Item(10).ToString()                         ' 組合大会会議通知会議番号
                        ' プロパティに確保
                        Me.strUnionInfoUnionMeeting = .Item(10).ToString()                  ' 組合大会会議通知会議番号
                    End If
                    ' プロパティに確保
                    Me.strUnionInfoPeriodId = .Item(9).ToString()                           ' 組合大会会議通知期ID
                    Me.txtStandName.Text = .Item(13).ToString()                             ' 代表者名内容（申請者）
                    ' 更新回数アルファベット取得
                    strVersion = GetRevision(CInt(.Item(14)))
                    If Not strVersion Is String.Empty Then
                        Me.lblVersion.Text = strVersion
                    Else
                        Call MessageBox.Show("アルファベットに変換できませんでした！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        Return blnRet
                    End If
                End With
            ElseIf intCntRet = 0 Then
                Call MessageBox.Show("データがありません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            Else
                Call MessageBox.Show("データが複数あります！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetApplyStrike
    '   名称　：時間内組合活動メンバー日程情報取得処理
    '   概要  ：時間内組合活動メンバー日程情報を取得する。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/21(土) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/21(土) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動メンバー日程情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetApplyStrikeMemberDate(ByVal iClsDb As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strSql As String = ""                                                           ' SQL文
        Dim tbRet As DataTable                                                              ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing                                                  ' 処理件数
        Dim intRow As Integer = 1                                                           ' 日付出力行
        Dim intCol As Integer = 5                                                           ' 日付出力列
        Dim strKey As String = ""                                                           ' キー（社員番号）
        Dim intStafCnt As Integer = 0                                                       ' 社員件数
        Dim cntDateMax As Integer = 5                                                       ' 日付の最大表示件数(初期値5)
        Try
            '-------------------------------------------------------------------------------
            '   社員件数取得
            '-------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT DISTINCT(a.c_staf_id) AS StafCnt" & vbCrLf           ' 社員件数
            strSql = strSql & "   FROM apply_strike_member_date AS a" & vbCrLf              ' 時間内組合活動メンバー日程情報
            strSql = strSql & "  WHERE a.c_strike_id = '" & Me.strStrikeId & "'" & vbCrLf   ' 時間内ストIDと同じもの
            strSql = strSql & "   AND  a.k_apply_area = '" & Me.strApplyArea & "'" & vbCrLf   ' 申請地区と同じもの
            strSql = strSql & ";" & vbCrLf
            tbRet = iClsDb.ExecuteSql(strSql)                                               ' SQL実行
            intStafCnt = tbRet.Rows.Count + 1                                               ' 件数取得
            Me.cfgResult.Rows.Count = intStafCnt                                            ' グリッド縦総数設定
            '-------------------------------------------------------------------------------
            '   社員情報取得
            '-------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT asmd.c_staf_id                      AS c_staf_id" & vbCrLf       ' 01. 社員番号
            strSql = strSql & "       ,staf.l_name                         AS l_name" & vbCrLf          ' 02. 名前
            strSql = strSql & "       ,staf.k_belonging                    AS k_belonging" & vbCrLf     ' 03. 所属（略名称）
            strSql = strSql & "       ,staf.k_model                        AS l_omission_name" & vbCrLf ' 04. 機種（略名称）
            strSql = strSql & "       ,staf.k_model_name                   AS k_model_name" & vbCrLf    ' 05. 機種（名称）
            strSql = strSql & "       ,staf.k_qualification                AS k_qualification" & vbCrLf ' 06. 資格（略名称）
            strSql = strSql & "       ,FORMAT(asmd.d_strike, 'yyyy/MM/dd') AS d_strike" & vbCrLf        ' 07. 日付
            strSql = strSql & "       ,asmd.k_cancel                       AS k_cancel" & vbCrLf        ' 08. 取消区分
            strSql = strSql & "   FROM apply_strike_member_date AS asmd" & vbCrLf
            strSql = strSql & "       ,( SELECT b.l_name             AS l_name" & vbCrLf
            strSql = strSql & "                ,b.c_staf_id          AS c_staf_id" & vbCrLf
            strSql = strSql & "                ,cdt1.l_omission_name AS k_belonging" & vbCrLf
            strSql = strSql & "                ,cdt2.l_omission_name AS k_model" & vbCrLf
            strSql = strSql & "                ,cdt2.l_name          AS k_model_name" & vbCrLf
            strSql = strSql & "                ,cdt3.l_omission_name AS k_qualification" & vbCrLf
            strSql = strSql & "            FROM staf_attribute AS b" & vbCrLf
            ' 最新組合員属性情報
            strSql = strSql & "                ,( SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "                         ,a.c_ksh" & vbCrLf
            strSql = strSql & "                         ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                         ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                     FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                    GROUP BY a.c_user_id" & vbCrLf
            strSql = strSql & "                            ,a.c_ksh" & vbCrLf
            strSql = strSql & "                            ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS cdt1" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS cdt2" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS cdt3" & vbCrLf
            strSql = strSql & "           WHERE b.c_user_id       = c.c_user_id" & vbCrLf
            strSql = strSql & "             AND b.c_ksh           = c.c_ksh" & vbCrLf
            strSql = strSql & "             AND b.c_staf_id       = c.c_staf_id" & vbCrLf
            strSql = strSql & "             AND b.d_from          = c.d_from" & vbCrLf
            strSql = strSql & "             AND b.k_belonging     = cdt1.c_constant_seq" & vbCrLf
            strSql = strSql & "             AND cdt1.c_constant   = 'BELONGING'" & vbCrLf
            strSql = strSql & "             AND b.k_model         = cdt2.c_constant_seq" & vbCrLf
            strSql = strSql & "             AND cdt2.c_constant   = 'MODEL'" & vbCrLf
            strSql = strSql & "             AND b.k_qualification = cdt3.c_constant_seq" & vbCrLf
            strSql = strSql & "             AND cdt3.c_constant   = 'QUALIFICATION' ) AS staf" & vbCrLf
            strSql = strSql & "  WHERE asmd.c_strike_id = '" & Me.strStrikeId & "'" & vbCrLf    ' 時間内ストIDと同じもの
            strSql = strSql & "    AND asmd.k_apply_area =  '" & Me.strApplyArea & "'" & vbCrLf ' 申請地区が同じもの
            strSql = strSql & "    AND asmd.c_staf_id   = staf.c_staf_id" & vbCrLf              ' 社員番号が同じもの
            strSql = strSql & "  ORDER BY CLng(asmd.c_staf_id)" & vbCrLf                        ' 社員番号, 日付で並替
            strSql = strSql & "          ,asmd.d_strike" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            tbRet = iClsDb.ExecuteSql(strSql)                                               ' SQL実行
            intCntRet = tbRet.Rows.Count                                                    ' 処理件数取得
            If intCntRet <> 0 Then
                For i = 0 To intCntRet - 1                                                  ' レコード数分ループ
                    With tbRet.Rows(i)
                        ' キー（社員番号）判定
                        If strKey.Length <> 0 Then
                            If strKey <> .Item(0).ToString() Then
                                intRow = intRow + 1
                                intCol = 5
                            End If
                        End If
                        ' キー（社員番号）が変わっていたら出力
                        If strKey <> .Item(0).ToString() Then
                            Me.cfgResult.SetData(intRow, 0, .Item(0).ToString())            ' 01. 社員番号
                            Me.cfgResult.SetData(intRow, 1, .Item(1).ToString())            ' 02. 名前
                            Me.cfgResult.SetData(intRow, 2, .Item(2).ToString())            ' 03. 所属（略名称）
                            Me.cfgResult.SetData(intRow, 3, .Item(3).ToString())            ' 04. 機種（略名称）
                            Me.cfgResult.SetData(intRow, 4, .Item(5).ToString())            ' 06. 資格（略名称）
                            Me.cfgResult.SetData(intRow, 5, "")                             ' 07. 日付1
                            Me.cfgResult.SetData(intRow, 6, "")                             ' 08. 日付2
                            Me.cfgResult.SetData(intRow, 7, "")                             ' 09. 日付3
                            Me.cfgResult.SetData(intRow, 8, "")                             ' 10. 日付4
                            Me.cfgResult.SetData(intRow, 9, "")                             ' 11. 日付5
                            Me.cfgResult.SetData(intRow, 10, "")                            ' 12. 日付6
                            Me.cfgResult.SetData(intRow, 11, "")                            ' 13. 日付7
                            Me.cfgResult.SetData(intRow, 12, "")                            ' 14. 日付8
                            Me.cfgResult.SetData(intRow, 13, "")                            ' 15. 日付9
                            Me.cfgResult.SetData(intRow, 14, "")                            ' 16. 日付10
                            Me.cfgResult.SetData(intRow, 15, "")                            ' 17. 日付11
                            Me.cfgResult.SetData(intRow, 16, "")                            ' 18. 日付12
                            Me.cfgResult.SetData(intRow, 17, "")                            ' 19. 日付13
                            Me.cfgResult.SetData(intRow, 18, "")                            ' 20. 日付14
                            Me.cfgResult.SetData(intRow, 19, "")                            ' 21. 日付15
                        End If
                        ' 07. 日付1 ～ 21. 日付15
                        If .Item(7).ToString() = "0" Then
                            Me.cfgResult.SetData(intRow, intCol, .Item(6).ToString())
                            intCol = intCol + 1

                            If intCol > (cntDateMax + 5) Then                               ' 組合員毎の日付出力数を記録
                                cntDateMax = (intCol - 5)
                            End If
                        End If
                        strKey = .Item(0).ToString()                                        ' キー（社員番号）取得
                    End With
                Next

                Me.numUseCol = cntDateMax
            Else
                Call MessageBox.Show("データがありません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetNewApplyNo
    '   名称　：申請書番号取得処理
    '   概要  ：時間内組合活動情報から新しい申請書番号を取得する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/24(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/24(火) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '         ：2012/08/07(火) Fujisaku  変更 発番ルール修正
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    ''' <summary>申請書番号取得処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetNewApplyNo(ByVal clsDb As CLAccessMdb) As String
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)          ' ログ出力（処理開始）
        Dim dtRet As DataTable = Nothing                                                        ' 処理結果データテーブル
        Dim strRet As String = ""                                                               ' 処理結果
        Dim strSql As String = ""                                                               ' SQL文
        Dim intRet As Integer = 0                                                               ' 処理件数
        Dim intSeqDb As Integer
        Dim intSeqText As Integer
        Try
            '-----------------------------------------------------------------------------------
            '   MAX申請書番号取得
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT (MAX(CLng(a.c_application)) + 1) AS c_application" & vbCrLf  ' 申請書番号
            strSql = strSql & "   FROM apply_strike AS a" & vbCrLf                                  ' 時間内組合活動
            strSql = strSql & "  WHERE a.c_ksh       = '" & Me.strKsh & "'" & vbCrLf                ' 会社区分と同じもの
            strSql = strSql & "    AND a.c_period_id = '" & Me.strPeriodId & "'" & vbCrLf           ' 取消対象と同じもの
            strSql = strSql & "    AND a.k_apply_area = '" & Me.strApplyArea & "'" & vbCrLf         ' 申請地区と同じもの
            strSql = strSql & ";" & vbCrLf
            dtRet = clsDb.ExecuteSql(strSql)                                                        ' SQL実行
            intRet = dtRet.Rows.Count                                                               ' 処理件数取得
            If intRet = 1 Then                                                                      ' 処理件数判定
                intSeqDb = dtRet.Rows(0).Item(0)
            ElseIf intRet = 0 Then
                intSeqDb = 1
            End If

            ' TEXTから最新番号取得
            Dim strSeqName As String = "seq_apl.txt"
            If Me.strApplyArea = "01" Then
                strSeqName = "seq_apl_tyo_" + Me.strPeriodId + ".txt"
            ElseIf Me.strApplyArea = "02" Then
                strSeqName = "seq_apl_osa_" + Me.strPeriodId + ".txt"
            End If
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intSeqText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intSeqText = 1
            Catch ex As System.InvalidCastException
                intSeqText = 1
            End Try

            ' 値の大きいほうを採用
            If intSeqText >= intSeqDb Then
                Return intSeqText
            Else
                Return intSeqDb
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)            ' ログ出力（処理終了）
        Return strRet                                                                           ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsApplyStrike
    '   名称　：時間内組合活動情報（取消データ作成）存在確認処理
    '   概要  ：時間内組合活動情報（取消データ作成）の存在確認を行う。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iStrStrikeId As String      = 時間内ストID（期ID + "-" + 取得したMAX申請書番号）,
    '           ByVal iStrKsh      As String      = 会社区分,
    '           ByVal iStrPeriodId As String      = 期ID,
    '           ByVal iStrApplyArea As String     = 申請地区
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>時間内組合活動情報（取消データ作成）存在確認処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrStrikeId">時間内ストID</param>
    ''' <param name="iStrKsh">会社区分</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrApplyArea">申請地区</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsApplyStrike(ByVal iClsDb As CLAccessMdb, _
                                       ByVal iStrStrikeId As String, _
                                       ByVal iStrKsh As String, _
                                       ByVal iStrPeriodId As String, _
                                       ByVal iStrApplyArea As String) As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strSql As String = ""                                                           ' SQL文
        Dim intRet As Integer = 0                                                           ' 処理件数
        Dim dtRet As DataTable = Nothing                                                    ' 処理結果格納データテーブル
        Try
            '-------------------------------------------------------------------------------
            '   時間内組合活動情報（取消データ作成）存在確認処理
            '-------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_strike_id" & vbCrLf
            strSql = strSql & "   FROM apply_strike AS a" & vbCrLf                          ' 時間内組合活動情報
            ' 時間内ストID（期ID + "-" + 取得したMAX申請書番号）
            strSql = strSql & "  WHERE a.c_strike_id = '" & iStrStrikeId & "'" & vbCrLf     ' 時間内ストIDと同じもの
            strSql = strSql & "    AND a.c_ksh       = '" & iStrKsh & "'" & vbCrLf          ' 会社区分と同じもの
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf     ' 期IDと同じもの
            strSql = strSql & "    AND a.k_apply_area = '" & iStrApplyArea & "'" & vbCrLf   ' 申請地区と同じもの
            strSql = strSql & ";" & vbCrLf
            dtRet = iClsDb.ExecuteSql(strSql)                                               ' SQL実行
            intRet = dtRet.Rows.Count                                                       ' 件数取得
            If intRet <> 1 Then                                                             ' 処理件数確認
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertApplyStrike
    '   名称　：時間内組合活動登録処理（取消データ作成）
    '   概要  ：時間内組合活動の取消データの作成処理を行う。
    '   引数　：ByVal clsDb        As CLAccessMdb    = データベースクラス（ローカル）,
    '           ByVal clsDbMst     As CLAccessMdbMst = データベースクラス（サーバデザインマスタ）,
    '           ByVal iStrStrikeId As String         = 時間内ストID,
    '           ByVal iStrKsh      As String         = 会社区分,
    '           ByVal iStrPeriodId As String         = 期ID,
    '           ByVal iStrMaxNo    As String         = 登録申請書番号
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>時間内組合活動登録処理（取消データ作成）</summary>
    ''' <param name="iClsDb">データベースクラス（ローカルレプリカ）</param>
    ''' <param name="iClsDbMst">データベースクラス（サーバデザインマスタ）</param>
    ''' <param name="iStrStrikeId">時間内ストID</param>
    ''' <param name="iStrKsh">会社区分</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrMaxNo">登録申請書番号</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertApplyStrike(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrStrikeId As String,
        ByVal iStrKsh As String,
        ByVal iStrPeriodId As String,
        ByVal iStrMaxNo As String
    ) As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim strSql As String = ""                                                       ' SQL文
        Dim intRet As Integer = 0                                                       ' 処理件数（ローカルレプリカ）

        Try
            '---------------------------------------------------------------------------
            '   時間内組合活動情報登録（取消データ作成）
            '---------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO apply_strike ( " & vbCrLf
            strSql = strSql & "    c_strike_id" & vbCrLf                                ' 01. 時間内ストID
            strSql = strSql & "   ,c_ksh" & vbCrLf                                      ' 02. 会社区分
            strSql = strSql & "   ,c_period_id" & vbCrLf                                ' 03. 期ID
            strSql = strSql & "   ,k_apply_area" & vbCrLf                               ' 04. 申請地区区分
            strSql = strSql & "   ,c_application" & vbCrLf                              ' 05. 申請書番号
            strSql = strSql & "   ,d_application" & vbCrLf                              ' 06. 申請年月日
            strSql = strSql & "   ,l_stand_name" & vbCrLf                               ' 07. 代表者名内容
            strSql = strSql & "   ,k_cancel" & vbCrLf                                   ' 08. 取消区分
            strSql = strSql & "   ,k_replace" & vbCrLf                                  ' 09. 差替え区分
            strSql = strSql & "   ,c_replace_strike_id" & vbCrLf                        ' 10. 差替え元時間内ストID
            strSql = strSql & "   ,k_apply_classify" & vbCrLf                           ' 11. 種類区分
            strSql = strSql & "   ,l_meeting" & vbCrLf                                  ' 12. 会議名
            strSql = strSql & "   ,union_info_c_period_id" & vbCrLf                     ' 13. 組合大会会議通知期ID
            strSql = strSql & "   ,union_info_c_union_meeting" & vbCrLf                 ' 14. 組合大会会議通知会議番号
            strSql = strSql & "   ,d_ins" & vbCrLf                                      ' 15. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                              ' 16. 作成者個人ID
            strSql = strSql & "   ,d_up" & vbCrLf                                       ' 17. 更新日
            strSql = strSql & "   ,c_user_id_up" & vbCrLf                               ' 18. 更新者個人ID
            strSql = strSql & "   ,s_up" & vbCrLf                                       ' 19. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            ' 01. 時間内ストID（期ID + "-" + MAX申請書番号 + 1）
            strSql = strSql & "     '" & iStrStrikeId & "'" & vbCrLf
            ' 02. 会社区分
            strSql = strSql & "    ,'" & iStrKsh & "'" & vbCrLf
            ' 03. 期ID
            strSql = strSql & "    ,'" & iStrPeriodId & "'" & vbCrLf
            ' 04. 申請地区区分
            strSql = strSql & "    ,'" & Me.strApplyArea & "'" & vbCrLf
            ' 05. 申請書番号
            strSql = strSql & "    ,'" & iStrMaxNo & "'" & vbCrLf
            ' 06. 申請年月日
            strSql = strSql & "    ,'" & Now.Date & "'" & vbCrLf
            ' 07. 代表者名内容
            strSql = strSql & "    ,'" & Me.txtStandName.Text & "'" & vbCrLf
            ' 08. 取消区分
            strSql = strSql & "    ,'1'" & vbCrLf
            ' 09. 差替え区分
            strSql = strSql & "    ,'0'" & vbCrLf
            ' 10. 差替え元時間内ストID
            strSql = strSql & "    ,''" & vbCrLf
            ' 11. 種類区分
            strSql = strSql & "    ,'" & Me.strApplyClassify & "'" & vbCrLf
            ' 12. 会議名
            strSql = strSql & "    ,'" & Me.cboMeeting.Text & "'" & vbCrLf
            ' 13. 組合大会会議通知期ID
            strSql = strSql & "    ,'" & Me.strUnionInfoPeriodId & "'" & vbCrLf
            ' 14. 組合大会会議通知会議番号
            strSql = strSql & "    ,'" & Me.strUnionInfoUnionMeeting & "'" & vbCrLf
            ' 15. 作成日
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf
            ' 16. 作成者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 17. 更新日
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf
            ' 18. 更新者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 19. 更新回数
            strSql = strSql & "    ,0" & vbCrLf
            strSql = strSql & " );" & vbCrLf

            '-------------------------------------------------------------------
            '   SQL実行
            '-------------------------------------------------------------------
            intRet = iClsDb.ExecuteNonQueryKeyErr(strSql)           ' ローカルレプリカ

            ' 結果判定
            If intRet = -2 Then
                ' キー重複エラーの場合
                CLMsg.Show("DE0015")
                Return blnRet
            ElseIf Not intRet = 1 Then
                ' その他のエラー場合
                CLMsg.Show("DE0005")
                Return blnRet
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' ログ出力（処理終了）
        Return blnRet                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsApplyStrikeMemberDate
    '   名称　：時間内組合活動メンバー日程情報存在確認処理
    '   概要  ：時間内組合活動のメンバー日程情報の存在確認を行う。
    '   引数　：ByVal clsDb        As CLAccessMdb = データベースクラス,
    '           ByVal iStrStrikeId As String  = 時間内ストID,
    '           ByVal iStrApplyArea As String = 申請地区,
    '           ByVal iStrStafId   As String  = 社員番号,
    '           ByVal iStrPeriodId As String  = 日付
    '           ByVal iStrCancel   As String  = 取消区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動メンバー日程更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrStrikeId">時間内ストID</param>
    ''' <param name="iStrApplyArea">申請地区</param>
    ''' <param name="iStrStafId">社員番号</param>
    ''' <param name="iDatStrike">日付</param>
    ''' <param name="iStrCancel">取消区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsApplyStrikeMemberDate(ByVal iClsDb As CLAccessMdb, _
                                                 ByVal iStrStrikeId As String, _
                                                 ByVal iStrApplyArea As String, _
                                                 ByVal iStrStafId As String, _
                                                 ByVal iDatStrike As Date, _
                                                 ByVal iStrCancel As String) As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)          ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                           ' 処理結果
        Dim strSql As String = ""                                                               ' SQL文
        Dim intRet As Integer = 0                                                               ' 処理件数
        Dim dtRet As DataTable = Nothing                                                        ' 処理結果格納データテーブル
        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_strike_id" & vbCrLf
            strSql = strSql & "   FROM apply_strike_member_date AS a" & vbCrLf                  ' 時間内組合活動メンバー日程
            strSql = strSql & "  WHERE a.c_strike_id  = '" & iStrStrikeId & "'" & vbCrLf        ' 時間内ストIDと同じもの
            strSql = strSql & "    AND a.k_apply_area = '" & iStrApplyArea & "'" & vbCrLf       ' 申請地区と同じもの
            strSql = strSql & "    AND a.c_staf_id    = '" & iStrStafId & "'" & vbCrLf          ' 社員番号と同じもの
            strSql = strSql & "    AND a.d_strike     = '" & iDatStrike & "'" & vbCrLf          ' 日付と同じもの
            strSql = strSql & "    AND k_cancel       = '" & iStrCancel & "'" & vbCrLf          ' 削除区分と同じもの
            strSql = strSql & ";" & vbCrLf
            dtRet = iClsDb.ExecuteSql(strSql)                                                   ' SQL実行
            intRet = dtRet.Rows.Count                                                           ' 件数取得
            If intRet <> 1 Then                                                                 ' 処理件数確認
                Return blnRet
            End If
            blnRet = True                                                                       ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)            ' ログ出力（処理終了）
        Return blnRet                                                                           ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertApplyStrikeMemberDate
    '   名称　：時間内組合活動メンバー日程登録処理
    '   概要  ：時間内組合活動のメンバー日程の登録処理を行う。
    '   引数　：ByVal clsDb        As CLAccessMdb    = データベースクラス（ローカルレプリカ）,
    '           ByVal clsDbMst     As CLAccessMdbMst = データベースクラス（サーバデザインマスタ）,
    '           ByVal iStrStrikeId As String         = 旧時間内ストID,
    '           ByVal iStrKsh      As String         = 社員番号,
    '           ByVal iStrPeriodId As String         = 日付
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/26(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/26(木) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動メンバー日程更新処理</summary>
    ''' <param name="iClsDb">データベースクラス（ローカルレプリカ）</param>
    ''' <param name="iClsDbMst">データベースクラス（サーバデザインマスタ）</param>
    ''' <param name="iStrStrikeId">時間内ストID</param>
    ''' <param name="iStrStafId">社員番号</param>
    ''' <param name="iDatStrike">日付</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertApplyStrikeMemberDate(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrStrikeId As String,
        ByVal iStrStafId As String,
        ByVal iDatStrike As Date
    ) As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)              ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                               ' 処理結果
        Dim strSql As String = ""                                                                   ' SQL文
        Dim intRet As Integer = 0                                                                   ' 処理件数（ローカルレプリカ）

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO apply_strike_member_date (" & vbCrLf
            strSql = strSql & "     c_strike_id" & vbCrLf                           ' 01. 時間内ストID
            strSql = strSql & "    ,k_apply_area" & vbCrLf                          ' 02. 申請地区
            strSql = strSql & "    ,c_staf_id" & vbCrLf                             ' 03. 社員番号
            strSql = strSql & "    ,d_strike" & vbCrLf                              ' 04. 日付
            strSql = strSql & "    ,c_cancel_strike_id" & vbCrLf                    ' 05. 取消ストID
            strSql = strSql & "    ,k_cancel" & vbCrLf                              ' 06. 取消区分
            strSql = strSql & "    ,d_ins" & vbCrLf                                 ' 07. 作成日
            strSql = strSql & "    ,c_user_id_ins" & vbCrLf                         ' 08. 作成者個人ID
            strSql = strSql & "    ,d_up" & vbCrLf                                  ' 09. 更新日
            strSql = strSql & "    ,c_user_id_up" & vbCrLf                          ' 10. 更新者個人ID
            strSql = strSql & "    ,s_up" & vbCrLf                                  ' 11. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            ' 01. 時間内ストID
            strSql = strSql & "     '" & iStrStrikeId & "'" & vbCrLf
            ' 02. 申請地区
            strSql = strSql & "    ,'" & Me.strApplyArea & "'" & vbCrLf
            ' 03. 社員番号
            strSql = strSql & "    ,'" & iStrStafId & "'" & vbCrLf
            ' 04. 日付
            strSql = strSql & "    ,'" & iDatStrike & "'" & vbCrLf
            ' 05. 取消ストID
            strSql = strSql & "    ,''" & vbCrLf
            ' 06. 取消区分
            strSql = strSql & "    ,'0'" & vbCrLf
            ' 07. 作成日
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf
            ' 08. 作成者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 09. 更新日
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf
            ' 10. 更新者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 11. 更新回数
            strSql = strSql & "    ,0" & vbCrLf
            strSql = strSql & ");" & vbCrLf

            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = iClsDb.ExecuteNonQuery(strSql)             ' ローカルレプリカ

            ' 処理判定
            If Not intRet = 1 Then
                Call MessageBox.Show("時間内組合活動メンバー日程を登録できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            blnRet = True                                                                           ' 処理結果に正常を格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                                   ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)                ' ログ出力（処理終了）
        Return blnRet                                                                               ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateApplyStrikeMemberDate
    '   名称　：時間内組合活動メンバー日程更新処理
    '   概要  ：時間内組合活動のメンバー日程の取消処理を行う。
    '   引数　：ByVal clsDb           As CLAccessMdb = データベースクラス,
    '           ByVal iStrStrikeIdNew As String      = 新時間内ストID,
    '           ByVal iStrStrikeIdOld As String      = 旧時間内ストID,
    '           ByVal iStrApplyArea As String        = 申請地区,
    '           ByVal iStrKsh         As String      = 社員番号,
    '           ByVal iStrPeriodId    As String      = 日付
    '           ByVal iStrCancel      As String      = 取消区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動メンバー日程更新処理</summary>
    ''' <param name="iClsDb">データベースクラス（ローカルレプリカ）</param>
    ''' <param name="iClsDbMst">データベースクラス（サーバデザインマスタ）</param>
    ''' <param name="iStrStrikeIdNew">新時間内ストID</param>
    ''' <param name="iStrStrikeIdOld">旧時間内ストID</param>
    ''' <param name="iStrApplyArea">申請地区</param>
    ''' <param name="iStrStafId">社員番号</param>
    ''' <param name="iDatStrike">日付</param>
    ''' <param name="iStrCancel">取消区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateApplyStrikeMemberDate(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrStrikeIdNew As String,
        ByVal iStrStrikeIdOld As String,
        ByVal iStrApplyArea As String,
        ByVal iStrStafId As String,
        ByVal iDatStrike As Date,
        ByVal iStrCancel As String
    ) As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)              ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                               ' 処理結果
        Dim strSql As String = ""                                                                   ' SQL文
        Dim intRet As Integer = 0                                                                   ' 処理件数（ローカルレプリカ）

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE apply_strike_member_date" & vbCrLf                           ' 時間内組合活動メンバー日程
            strSql = strSql & "    SET k_cancel           = '1'" & vbCrLf                           ' 取消区分
            strSql = strSql & "       ,c_cancel_strike_id = '" & iStrStrikeIdNew & "'" & vbCrLf     ' 取消ストID
            strSql = strSql & "       ,d_up               = '" & Now() & "'" & vbCrLf               ' 更新日
            strSql = strSql & "       ,c_user_id_up       = '" & MDLoginInfo.UserId & "'" & vbCrLf  ' 更新者個人ID
            strSql = strSql & "       ,s_up               = s_up + 1" & vbCrLf                      ' 更新回数
            strSql = strSql & "  WHERE c_strike_id  = '" & iStrStrikeIdOld & "'" & vbCrLf           ' 時間内ストIDと同じもの
            strSql = strSql & "    AND k_apply_area = '" & iStrApplyArea & "'" & vbCrLf             ' 申請区分と同じもの
            strSql = strSql & "    AND c_staf_id    = '" & iStrStafId & "'" & vbCrLf                ' 社員番号と同じもの
            strSql = strSql & "    AND d_strike     = '" & iDatStrike & "'" & vbCrLf                ' 日付と同じもの
            strSql = strSql & "    AND k_cancel     = '" & iStrCancel & "'" & vbCrLf                ' 削除区分と同じもの

            '-----------------------------------------------------------------------------------
            '   SQL作成
            '-----------------------------------------------------------------------------------
            intRet = iClsDb.ExecuteNonQuery(strSql)         ' ローカルレプリカ

            ' 処理判定
            If Not intRet = 1 Then
                Call MessageBox.Show("時間内組合活動メンバー日程を更新できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            blnRet = True                                                                           ' 処理結果に正常を格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                                   ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)                ' ログ出力（処理終了）
        Return blnRet                                                                               ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen() As Boolean
        Dim pn As Panel = Nothing                                                       ' パネルコントロール
        Dim uc As UC040301 = Nothing                                                    ' ユーザーコントロール
        Dim clsUC040301 As UC040301 = Nothing                                           ' 時間内組合活動画面クラス
        Try
            Me.Visible = False                                                          ' 時間内組合活動取消画面非表示
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)                             ' パネル設定
            uc = pn.Controls(SCREEN_ID_UC040301)                                        ' 時間内組合活動画面設定
            If uc Is Nothing Then                                                       ' 時間内組合活動画面がパネルにあるかチェック
                ' パネルにない場合
                uc = New UC040301                                                       ' 時間内組合活動画面設定
                ' プロパティ設定
                clsUC040301.blnRefFlg = 2                                               ' 検索フラグ（2：申請番号検索）
                Call pn.Controls.Add(uc)                                                ' パネルに時間内組合活動画面追加
            Else
                ' パネルにある場合
                clsUC040301 = pn.Controls(SCREEN_ID_UC040301)                           ' 時間内組合活動画面設定
                ' プロパティ設定
                clsUC040301.blnRefFlg = 2                                               ' 検索フラグ（2：申請番号検索）
                uc.Visible = True                                                       ' 時間内組合活動画面表示
            End If
            Me.Dispose()                                                                ' 時間内組合活動取消画面解放
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetGrant() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim dtGrant As DataTable = Nothing                                              ' 権限取得データテーブル
        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC040301)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString                    ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString                       ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                        ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString                   ' ファイル出力権限
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkCfgCellImage
    '   名称　：フレックスグリッド内画像有無チェック処理
    '   概要  ：C1FlexGrid内の指定の列に画像があるかチェックを行う。
    '   引数　：ByVal iCfg     As C1FlexGrid = チェック対象C1FlexGrid
    '           ByVal iBytRow As Integer     = チェック対象行, 
    '           ByVal iBytFrom As Integer    = チェック対象列（開始）, 
    '           ByVal iBytTo   As Integer    = チェック対象列（終了）
    '   戻り値：True = 正常（画像有り）, False = 異常（画像無し）
    '   作成日：2012/01/23(月) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>フレックスグリッド内画像有無チェック処理</summary>
    ''' <param name="iCfg">チェック対象C1FlexGrid</param>
    ''' <param name="iBytRow">チェック対象行</param>
    ''' <param name="iBytColFrom">チェック対象列（開始）</param>
    ''' <param name="iBytColTo">チェック対象列（終了）</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkCellImage(ByVal iCfg As C1FlexGrid, _
                                  ByVal iBytRow As Byte, _
                                  ByVal iBytColFrom As Byte, _
                                  ByVal iBytColTo As Byte) As Boolean
        Dim blnRet As Boolean = False                                   ' 処理結果
        Try
            If iBytRow <= 0 Then                                        ' チェック対象行が0以上かチェック
                Return blnRet                                           ' 処理を抜ける
            End If
            If iBytColFrom > iBytColTo Then                             ' チェック対象列（終了）よりもチェック対象列（開始）が小さいかチェック
                Return blnRet                                           ' 処理を抜ける
            End If
            For i = iBytRow To iCfg.Rows.Count - 1                      ' 指定行からレコード件数までループ
                For j = iBytColFrom To iBytColTo                        ' 指定チェック対象列（開始）から指定チェック対象列（終了）までループ
                    If Not (iCfg.GetCellImage(i, j) Is Nothing) Then    ' 画像があるかチェック
                        Return True                                     ' 戻り値に正常（画像有り）を設定
                    End If
                Next
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                   ' 戻り値に異常（画像無し）を設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkCellPastDate
    '   名称　：フレックスグリッド内過去日付有無チェック処理
    '   概要  ：C1FlexGrid内の指定の列に過去日付があるかチェックを行う。
    '   引数　：ByVal iCfg     As C1FlexGrid = チェック対象C1FlexGrid
    '           ByVal iBytRow As Integer     = チェック対象行, 
    '           ByVal iBytFrom As Integer    = チェック対象列（開始）, 
    '           ByVal iBytTo   As Integer    = チェック対象列（終了）
    '   戻り値：True = 正常（過去日付有り）, False = 異常（過去日付無し）
    '   作成日：2012/01/25(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/25(水) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>フレックスグリッド内過去日付有無チェック処理</summary>
    ''' <param name="iCfg">チェック対象C1FlexGrid</param>
    ''' <param name="iBytRow">チェック対象行</param>
    ''' <param name="iBytColFrom">チェック対象列（開始）</param>
    ''' <param name="iBytColTo">チェック対象列（終了）</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkCellPastDate(ByVal iCfg As C1FlexGrid, _
                                     ByVal iBytRow As Byte, _
                                     ByVal iBytColFrom As Byte, _
                                     ByVal iBytColTo As Byte) As Boolean
        Dim blnRet As Boolean = False                                               ' 処理結果
        Dim strNow As String = ""                                                   ' 現在日付取得（yyyyMMdd）
        Dim strTarget As String = ""                                                ' チェック対象日付（yyyyMMdd）
        Try
            If iBytRow <= 0 Then                                                    ' チェック対象行が0以上かチェック
                Return blnRet                                                       ' 処理を抜ける
            End If
            If iBytColFrom > iBytColTo Then                                         ' チェック対象列（終了）よりもチェック対象列（開始）が小さいかチェック
                Return blnRet                                                       ' 処理を抜ける
            End If
            strNow = Now.ToString("yyyyMMdd")                                       ' 現在日付取得
            For i = iBytRow To iCfg.Rows.Count - 1                                  ' 指定行からレコード件数までループ
                For j = iBytColFrom To iBytColTo                                    ' 指定チェック対象列（開始）から指定チェック対象列（終了）までループ
                    If Not (iCfg.GetCellImage(i, j) Is Nothing) Then                ' 画像があるかチェック
                        strTarget = iCfg.GetData(i, j).ToString().Replace("/", "").Replace("-", "")  ' 対象日付取得（yyyyMMdd）
                        If strNow >= strTarget Then                                 ' 対象日付が過去日付（現在日付含）かチェック
                            Return True                                             ' 戻り値に正常（画像有り）を設定
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)                                                   ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                               ' 戻り値に異常（画像無し）を設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：PrintPreview
    '   名称　：印刷プレビュー処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/23(月) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月) m.suzuki  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 日付15対応
    '         ：2012/08/07(火) Fujisaku  変更 発番ルール修正
    '***************************************************************************************************
    ''' <summary>印刷プレビュー処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function PrintPreview() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim fmPrint As FM000203 = Nothing                                                   ' プレビュークラス
        Dim ds As DS0403P2 = Nothing                                                        ' 帳票用データセット
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing    ' レポートドキュメントオブジェクト
        Dim intRet As Integer = Nothing                                                     ' プレビュー画面処理結果
        Dim drHeader As DS0403P2.dtHeaderRow = Nothing                                      ' ヘッダーロー
        Dim drFooter As DS0403P2.dtFooterRow = Nothing                                      ' フッダーロー
        Dim drDetail As DS0403P2.dtDetailRow = Nothing                                      ' 詳細ロー

        Dim intColNo As Integer = 1
        Dim strNewApplyNo As String = ""                                                    ' 取得したMAX申請番号 + 1
        Dim outPurposeMsg As String                                                         '「１．目的」に表示する文字列
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソル砂時計
            ds = New DS0403P2                                                               ' データセットクラス生成
            '-------------------------------------------------------------------------------
            '   ヘッダー情報
            '-------------------------------------------------------------------------------
            drHeader = ds.dtHeader.NewRow                                                   ' ヘッダーロー作成
            drHeader.BeginEdit()                                                            ' ヘッダーロー編集開始
            drHeader.apply_area = Me.txtApplyArea.Text                                      ' 支部
            drHeader.l_omission_name = Split(Me.txtApplyNo.Text, "-")(0)                    ' 略名称
            drHeader.c_application = "*****"                                                ' 申請書番号
            drHeader.president_name = MDCommon.GetPresidentName()                           ' 取締役社長名
            drHeader.leader_name = Me.txtStandName.Text                                     ' 申請者
            '「目的」取得
            '2012/05/30「上記の会議以外」選択時は、固定文言に変更
            If Me._strApplyClassify = "99" Then
                outPurposeMsg = "覚書(イ～ニ)で定めてある事項"
            Else
                outPurposeMsg = Me.txtApplyClassify.Text
            End If
            ' 会議名空欄でないときのみ括弧を付けて追加
            If Trim(Me.cboMeeting.Text) <> "" Then                                          ' 種類 + "（" + 会議名 + "）"
                drHeader.purpose = outPurposeMsg & "（" & Me.cboMeeting.Text & "）"
            Else
                drHeader.purpose = outPurposeMsg
            End If
            drHeader.application_date = Now.Date                                            ' 申請年月日
            drHeader.EndEdit()                                                              ' ヘッダーロー修正終了
            ds.dtHeader.Rows.Add(drHeader)                                                  ' データセットに作成データロー設定
            '-------------------------------------------------------------------------------
            '   詳細情報
            '-------------------------------------------------------------------------------
            ' グリッド表示分ループ
            With Me.cfgResult
                For i = 1 To Me.cfgResult.Rows.Count - 1
                    ' 日付カラムに取消画像があるかチェック
                    If (Not .GetCellImage(i, 5) Is Nothing) _
                    Or (Not .GetCellImage(i, 6) Is Nothing) _
                    Or (Not .GetCellImage(i, 7) Is Nothing) _
                    Or (Not .GetCellImage(i, 8) Is Nothing) _
                    Or (Not .GetCellImage(i, 9) Is Nothing) _
                    Or (Not .GetCellImage(i, 10) Is Nothing) _
                    Or (Not .GetCellImage(i, 11) Is Nothing) _
                    Or (Not .GetCellImage(i, 12) Is Nothing) _
                    Or (Not .GetCellImage(i, 13) Is Nothing) _
                    Or (Not .GetCellImage(i, 14) Is Nothing) _
                    Or (Not .GetCellImage(i, 15) Is Nothing) _
                    Or (Not .GetCellImage(i, 16) Is Nothing) _
                    Or (Not .GetCellImage(i, 17) Is Nothing) _
                    Or (Not .GetCellImage(i, 18) Is Nothing) _
                    Or (Not .GetCellImage(i, 19) Is Nothing) Then
                        Dim numNw As Integer = 5
LBL_nextRow:
                        drDetail = ds.dtDetail.NewRow                                       ' 詳細ロー作成
                        drDetail.BeginEdit()                                                ' 詳細ロー編集開始
                        drDetail.c_staf_id = .GetData(i, 0).ToString()                      ' 社員番号
                        drDetail.l_name = .GetData(i, 1).ToString()                         ' 氏名
                        drDetail.belonging = .GetData(i, 2).ToString()                      ' 所属
                        drDetail.model = .GetData(i, 3).ToString()                          ' 機種
                        drDetail.qualification = .GetData(i, 4).ToString()                  ' 資格
                        ' 日付1
                        intColNo = 1
                        For iCol As Integer = numNw To 19
                            If Not .GetCellImage(i, iCol) Is Nothing Then
                                drDetail("d_strike" + CStr(intColNo)) = Format(Date.Parse(.GetData(i, iCol)), "yy/MM/dd")
                                intColNo = intColNo + 1
                            End If

                            ' 5件処理毎に次の行へ移動
                            If intColNo = 6 Then
                                drDetail.EndEdit()
                                ds.dtDetail.Rows.Add(drDetail)
                                numNw = numNw + 1 '現在カーソル位置+1
                                GoTo LBL_nextRow
                            End If
                            numNw = numNw + 1
                        Next

                        ' データセット登録
                        If drDetail.Isd_strike1Null() Then
                            drDetail.CancelEdit()                                            ' 日付の無い空行であれば削除
                        Else
                            drDetail.EndEdit()                                               ' 詳細ロー編集終了
                            ds.dtDetail.Rows.Add(drDetail)                                   ' データセットに作成データロー設定
                        End If
                    End If
                Next
                ' 空行を埋める（1頁20行になるまで空データを作成）
                Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
                Dim intRest As Integer = (intRowCnt Mod 20)
                If intRest > 0 Then
                    Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 20)
                    Do While (intRowCnt < 20 * (intQuotient + 1))
                        drDetail = ds.dtDetail.NewRow                                       ' 詳細ロー作成
                        drDetail.BeginEdit()                                                ' 詳細ロー編集開始
                        drDetail.c_staf_id = ""
                        drDetail.l_name = ""
                        drDetail.belonging = ""
                        drDetail.model = ""
                        drDetail.qualification = ""
                        For iCol As Integer = 5 To 9
                            drDetail("d_strike" + CStr(intColNo)) = ""
                        Next
                        drDetail.EndEdit()                                                  ' 詳細ロー編集終了
                        ds.dtDetail.Rows.Add(drDetail)                                      ' データセットに作成データロー設定
                        intRowCnt = intRowCnt + 1
                    Loop
                End If
            End With
            '-------------------------------------------------------------------------------
            '   フッダー情報
            '-------------------------------------------------------------------------------
            drFooter = ds.dtFooter.NewRow                                                   ' フッダーロー作成
            drFooter.BeginEdit()                                                            ' フッダーロー編集開始
            drFooter.s_up = "・・A・・"                                                     ' 更新回数
            drFooter.EndEdit()                                                              ' フッダーロー編集終了
            ds.dtFooter.Rows.Add(drFooter)
            '-------------------------------------------------------------------------------
            '   印刷処理準備
            '-------------------------------------------------------------------------------
            ' クラス生成
            fmPrint = New FM000203                                                          ' 印刷プレビュー画面
            reportObj = New CR0403P2                                                        ' レポートドキュメント生成
            ' プロパティ設定
            fmPrint.ButtonShowType = 2                                                      ' ボタン形式設定（登録（印刷）、キャンセル）
            fmPrint.PrintCntVisible = False                                                 ' 印刷部数項目表示可否
            fmPrint.ObjResource = reportObj                                                 ' レポート形式設定
            reportObj.SetDataSource(ds)                                                     ' データセット設定
            ' 印刷プレビュー
            Call fmPrint.ShowDialog()                                                       ' 印刷プレビュー画面表示
            intRet = fmPrint.IntQlickBtnFlag                                                ' 印刷プレビュー画面処理結果取得
            ' 印刷プレビュー画面ボタン押下判定
            If intRet = 4 Then
                ' 登録（印刷）ボタン押下時、登録・印刷処理を行う。
                '---------------------------------------------------------------------------
                '   登録処理
                '---------------------------------------------------------------------------
                If InsertUpdate(strNewApplyNo) = False Then
                    Return blnRet
                Else
                    Call CLMsg.Show("GI0015")                                               ' 登録完了メッセージ表示
                End If
                '---------------------------------------------------------------------------
                '   印刷処理
                '---------------------------------------------------------------------------
                ds.dtHeader.Rows.Item(0).Item("c_application") = strNewApplyNo              ' 取得したMAX申請番号 + 1 を帳票に表示
                reportObj.SetDataSource(ds)
                fmPrint.PrintOut()
            Else
                ' キャンセル
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default                                                ' カーソル初期
            fmPrint.Dispose()                                                               ' プレビュー画面クラス破棄
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetApplyStrike
    '   名称　：時間内組合活動中執委員会活動情報取得処理
    '   概要  ：時間内組合活動情報を取得する。
    '   引数　：ByVal iClsDb                As CLAccessMdb = データベースクラス,
    '           ByVal iIntApplyStrikeTermId As String      = 中央執行委員会期間ID
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/27(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/27(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>時間内組合活動中執委員会活動情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iIntApplyStrikeTermId">中央執行委員会期間ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetApplyStrikeExecutiveTerm(ByVal iClsDb As CLAccessMdb, _
                                                 ByVal iIntApplyStrikeTermId As Integer) As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim strSql As String = ""                                                       ' SQL文
        Dim tbRet As DataTable                                                          ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing                                              ' 処理件数
        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.d_from AS d_from" & vbCrLf                     ' 01. 開始日
            strSql = strSql & "       ,a.d_to   AS d_to" & vbCrLf                       ' 02. 終了日
            strSql = strSql & "   FROM apply_strike_executive_term AS a" & vbCrLf       ' 時間内組合活動 中執委員会活動
            strSql = strSql & "  WHERE a.c_apply_strike_term_id = " & iIntApplyStrikeTermId & vbCrLf
            strSql = strSql & ";" & vbCrLf
            tbRet = iClsDb.ExecuteSql(strSql)                                           ' SQL実行
            intCntRet = tbRet.Rows.Count                                                ' 処理件数取得
            If intCntRet = 1 Then
                ' 開催期間（開始日 + "～" + 終了日）
                Me.txtTerm.Text = tbRet.Rows(0).Item(0).ToString() & "～" & tbRet.Rows(0).Item(1).ToString()
            Else
                Call MessageBox.Show("データがありません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChangeDisplay
    '   名称　：画面表示処理
    '   概要  ：種類によって画面表示を切り替える。
    '   引数　：ByVal pStrApplyClassify As String = "01"：中央執行委員会,
    '                                               "02"：中央委員会,
    '                                               "03"：組合大会,
    '                                               それ以外：上記以外
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/27(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/27(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面表示処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeDisplay(ByVal pStrApplyClassify As String) As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim blnMeeting As Boolean = False                                                   ' 組合大会関連
        Dim blnTerm As Boolean = False                                                      ' 開催期間関連
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            '-------------------------------------------------------------------------------
            '   日付項目表示・非表示設定
            '-------------------------------------------------------------------------------
            ' numUseColに指定された日付まで表示
            For i As Integer = 1 To 15
                If i <= Me.numUseCol Then
                    Me.cfgResult.Cols(i + 4).Visible = True
                Else
                    Me.cfgResult.Cols(i + 4).Visible = False
                End If
            Next
            '-------------------------------------------------------------------------------
            '   表示・非表示フラグ設定
            '-------------------------------------------------------------------------------
            If (pStrApplyClassify = APPLY_CLASSIFY_CENTRAL_EXECUTIVE) _
            Or (pStrApplyClassify = APPLY_CLASSIFY_CENTRAL) Then
                ' 中央執行委員会か中央委員会
                blnTerm = True
            ElseIf pStrApplyClassify = APPLY_CLASSIFY_GENERAL_MEETING Then
                ' 組合大会
                blnMeeting = True
            End If
            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            ' 開催期間関連
            Me.lblTerm.Visible = blnTerm                                                    ' 開催期間ラベル
            Me.txtTerm.Visible = blnTerm                                                    ' 開催期間テキストボックス
            ' 組合大会関連
            Me.lblMeetingNo.Visible = blnMeeting                                            ' 組合大会番号ラベル
            Me.txtMeetingNo.Visible = blnMeeting                                            ' 組合大会番号テキストボックス
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function
#End Region

End Class

#End Region