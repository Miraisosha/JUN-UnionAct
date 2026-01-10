#Region "UC010102"
'===========================================================================================================
'   クラスＩＤ　　：FM000206
'   クラス名称　　：委員会名簿履歴一覧
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon

Public Class FM000206

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000206                                ' FM000206
    Private Const SCREEN_NAME = SCREEN_NAME_FM000206                            ' 委員会名簿履歴一覧画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "プロパティ"
    ' 検索項目
    Public _strSearchPeriodId As String = ""                ' 期
    Public _strSearchCommitteeId As String = ""             ' 委員会ID

    ' 選択項目
    Public _dtSelData As DataTable = Nothing                ' 選択データ
    Public _strYear As String = ""                          ' 選択対象年
    Public _strMonth As String = ""                         ' 選択月
    Public _strCreateUser As String = ""                    ' 作成者
    Public _datCreateDate As Date                           ' 作成日

    ' 検索項目　期
    Public Property strSearchPeriodId() As String
        Get
            Return _strSearchPeriodId
        End Get
        Set(ByVal value As String)
            _strSearchPeriodId = value
        End Set
    End Property

    ' 検索項目　委員会ID
    Public Property strSearchCommitteeId() As String
        Get
            Return _strSearchCommitteeId
        End Get
        Set(ByVal value As String)
            _strSearchCommitteeId = value
        End Set
    End Property

    ' 選択項目　選択データ
    Public Property dtSelData() As DataTable
        Get
            Return _dtSelData
        End Get
        Set(ByVal value As DataTable)
            _dtSelData = value
        End Set
    End Property

    ' 選択項目　対象年
    Public Property strYear() As String
        Get
            Return _strYear
        End Get
        Set(ByVal value As String)
            _strYear = value
        End Set
    End Property

    ' 選択項目　月
    Public Property strMonth() As String
        Get
            Return _strMonth
        End Get
        Set(ByVal value As String)
            _strMonth = value
        End Set
    End Property

    ' 選択項目　作成者
    Public Property strCreateUser() As String
        Get
            Return _strCreateUser
        End Get
        Set(ByVal value As String)
            _strCreateUser = value
        End Set
    End Property

    ' 選択項目　作成日
    Public Property datCreateDate() As Date
        Get
            Return _datCreateDate
        End Get
        Set(ByVal value As Date)
            _datCreateDate = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000206_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM000206_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' 画面中央表示処理
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            ' データグリッドビュー初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 各データ取得処理
            If GetData() = False Then
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
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSelect_Click
    '   名称　：選択ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Try
            ' 選択情報設定処理
            If SetSelData() = False Then
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK         ' OKボタン押下結果格納
            Me.Visible = False                                      ' 画面非表示
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

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.Cancel     ' キャンセルボタン押下格納
            Me.Visible = False                                      ' 画面非表示

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

    '***************************************************************************************************
    '   ＩＤ　：cfgResult_KeyDown()
    '   名称　：データグリッドビューキーダウン処理
    '   概要  ：データグリッドビュー内で、Enterキーを押されたら、ダブルクリック同様の処理を行う。
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cfgResult_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        Try
            ' Enterキーかチェック
            If e.KeyCode = Keys.Enter Then
                ' 選択情報設定処理
                If SetSelData() Then
                    Exit Sub
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_MouseDoubleClick()
    '   名称　：データグリッドビュー内マウスダブルクリック処理
    '   概要  ：ダブルクリック同様の処理を行う。
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvResult.MouseDoubleClick
        Dim dgv As DataGridView = Nothing
        Dim hti As System.Windows.Forms.DataGridView.HitTestInfo = Nothing
        Try
            dgv = CType(sender, DataGridView)
            hti = dgv.HitTest(e.X, e.Y)
            If hti.Type = DataGrid.HitTestType.ColumnHeader Then
                Exit Sub
            Else
                ' 選択情報設定処理
                If SetSelData() = False Then
                    Exit Sub
                End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK         ' OKボタン押下結果格納
            Me.Visible = False                                      ' 画面非表示
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

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：データグリッドビュー初期化処理
    '   概要　：データグリッドビューの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni() As Boolean
        Dim blnRet As Boolean = False                                                               ' 処理結果
        Try
            With Me.dgvResult
                '-----------------------------------------------------------------------------------
                '   グリッド全体設定
                '-----------------------------------------------------------------------------------
                ' 総数
                .RowCount = 0                                                                       ' 縦
                .ColumnCount = 4                                                                    ' 横
                ' 固定行
                .ColumnHeadersVisible = True                                                        ' 列固定行有り
                .RowHeadersVisible = False                                                          ' 行固定無し
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                       ' 縦横両方
                ' 1行選択モード
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect                            ' 一度に１つの行選択
                ' サイズ変更
                .AllowUserToResizeColumns = False                                                   ' 列サイズ変更禁止
                .AllowUserToResizeRows = False                                                      ' 行サイズ変更禁止
                '-----------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-----------------------------------------------------------------------------------
                ' ヘッダー文字列
                .Columns(0).HeaderText = "対象年"                                                   ' 対象年
                .Columns(1).HeaderText = "月"                                                       ' 月
                .Columns(2).HeaderText = "作成者"                                                   ' 作成者
                .Columns(3).HeaderText = "作成日"                                                   ' 作成日
                ' ヘッダー文字位置（上下左右中央）
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 対象年
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 月
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 作成者
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 作成日
                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 対象年
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 月
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 作成者
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 作成日
                ' カラム幅
                .Columns(0).Width = 100                                                             ' 対象年
                .Columns(1).Width = 70                                                              ' 月
                .Columns(2).Width = 105                                                             ' 作成者
                .Columns(3).Width = 100                                                             ' 作成日
                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 対象年
                .Columns(1).Visible = True                                                          ' 月
                .Columns(2).Visible = True                                                          ' 作成者
                .Columns(3).Visible = True                                                          ' 作成日
                blnRet = True                                                                       ' 処理結果に正常設定
            End With
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                               ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス生成
        Dim strSql As String = ""                                                           ' SQL
        Try
            Call clsDb.Connect()                                                            ' データベース接続
            Me.lblPeriod.Text = "第" & Me.GetPeriodOmission(clsDb, strSearchPeriodId) & "期"
            Me.lblCommittee.Text = Me.GetCommitteeName(clsDb, strSearchPeriodId, strSearchCommitteeId)
            ' 委員会名簿履歴情報取得
            If GetCommitteeHistoryInfo(clsDb) = False Then
                Return blnRet
            End If
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
            Call clsDb.Disconnect()                                                         ' データベース切断
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeHistoryInfo
    '   名称　：委員会名簿履歴情報取得処理
    '   概要  ：委員会名簿履歴情報を取得する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：2012/02/29(水)　Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    ' 　　　　：2012/02/29(水)　Fujisaku  作成者の取得を変更(専従業員名に対応)
    '***************************************************************************************************
    ''' <summary>登録状況情報取得処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetCommitteeHistoryInfo(ByVal clsDb As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strSql As String = ""                                                           ' SQL文
        Dim tbRet As DataTable                                                              ' 処理結果データテーブル
        Dim drNew As DataRow                                                                ' データロー
        Dim intRetCnt As Integer = Nothing                                                  ' 処理件数
        Try
            '' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT Mid(com_list.d_from, 1, 4) AS 対象年" & vbCrLf
            strSql = strSql & "       ,Mid(com_list.d_from, 5, 2) AS 月" & vbCrLf
            strSql = strSql & "       ,( SELECT t1.l_name" & vbCrLf
            strSql = strSql & "          FROM staf_attribute_full_time_now_name_view t1" & vbCrLf
            strSql = strSql & "          WHERE t1.user_id = com_list.c_user_id_up) AS 作成者" & vbCrLf
            strSql = strSql & "       ,FORMAT(com_list.d_ins, 'yyyy/MM/dd') AS 作成日" & vbCrLf
            strSql = strSql & "   FROM committee_list AS com_list" & vbCrLf
            strSql = strSql & "  WHERE com_list.c_period_id    = '" & strSearchPeriodId & "'" & vbCrLf
            strSql = strSql & "    AND com_list.c_committee_id = '" & strSearchCommitteeId & "'" & vbCrLf
            strSql = strSql & "  ORDER BY com_list.d_from"  'chk
            tbRet = clsDb.ExecuteSql(strSql)                                                    ' SQL実行
            intRetCnt = tbRet.Rows.Count                                                        ' 処理件数取得
            ' 件数チェック
            If intRetCnt > 0 Then
                For i = 0 To intRetCnt - 1                                                      ' 1件以上の処理
                    dgvResult.Rows.Add()                                                        ' 行作成
                    drNew = tbRet.Rows(i)                                                       ' 追加行データ取得
                    ' データ投入
                    Me.dgvResult.Rows(i).Cells.Item(0).Value = tbRet.Rows(i).Item(0).ToString() ' 01. 対象年
                    Me.dgvResult.Rows(i).Cells.Item(1).Value = tbRet.Rows(i).Item(1).ToString() ' 02. 月
                    Me.dgvResult.Rows(i).Cells.Item(2).Value = tbRet.Rows(i).Item(2).ToString() ' 03. 作成者
                    Me.dgvResult.Rows(i).Cells.Item(3).Value = tbRet.Rows(i).Item(3).ToString() ' 04. 作成日
                Next
            Else
                Call CLMsg.Show("DI0001")                                                       ' 0件の処理（対象データなしメッセージボックス表示）
            End If
            blnRet = True                                                                       ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                           ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetSelData
    '   名称　：選択情報設定処理
    '   概要  ：選択情報設定処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>選択情報設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetSelData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim drRet As DataRow
        Try
            ' 選択されているかチェック
            If Me.dgvResult.SelectedRows.Count < 0 Then
                CLMsg.Show("GW0001", "データ")                                              ' 未選択の場合、エラーメッセージ表示
                Return blnRet
            End If
            '---------------------------------------------------------------------------
            '   選択情報設定（データテーブル）
            '---------------------------------------------------------------------------
            ' 選択データ
            dtSelData = New DataTable
            dtSelData.Columns.Add("対象年", GetType(String))
            dtSelData.Columns.Add("月", GetType(String))
            dtSelData.Columns.Add("作成者", GetType(String))
            dtSelData.Columns.Add("作成日", GetType(Integer))
            drRet = dtSelData.NewRow()
            drRet(0) = Me.dgvResult.SelectedRows.Item(0).Cells(0).Value                                 ' 対象年
            drRet(1) = Me.dgvResult.SelectedRows.Item(0).Cells(1).Value                                 ' 月
            drRet(2) = Me.dgvResult.SelectedRows.Item(0).Cells(2).Value                                 ' 作成者
            drRet(3) = Replace(Me.dgvResult.SelectedRows.Item(0).Cells(3).Value, "/", "")               ' 作成日
            dtSelData.Rows.Add(drRet)                                                                   ' 選択されたデータをデータテーブルに格納
            '---------------------------------------------------------------------------
            '   選択情報設定（個別）
            '---------------------------------------------------------------------------
            Me.strYear = Me.dgvResult.SelectedRows.Item(0).Cells(0).Value.ToString()                    ' 対象年
            Me.strMonth = Me.dgvResult.SelectedRows.Item(0).Cells(1).Value.ToString()                   ' 月
            Me.strCreateUser = Me.dgvResult.SelectedRows.Item(0).Cells(2).Value.ToString()              ' 作成者
            Me.datCreateDate = Date.Parse(Me.dgvResult.SelectedRows.Item(0).Cells(3).Value.ToString())  ' 作成日
            blnRet = True                                                                               ' 戻り値設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeName
    '   名称　：委員会名取得
    '   概要  ：期ID、委員会IDから委員会名を取得する
    '   引数　：なし
    '   戻り値：委員会名
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function GetCommitteeName(ByVal clsDB As CLAccessMdb, ByVal pStrPeriodId As String, ByVal pStrCommitteeID As String) As String
        Dim strSql As String = ""                                               ' SQL文
        Dim tbRet As DataTable = Nothing                                        ' 処理結果格納データテーブル
        Dim strComName As String = ""                                           ' 委員会名
        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT l_name" & vbCrLf
            strSql = strSql & "   FROM committee AS t1" & vbCrLf
            strSql = strSql & "  WHERE EXISTS ( SELECT *" & vbCrLf
            strSql = strSql & "                   FROM period t2" & vbCrLf
            strSql = strSql & "                  WHERE c_period_id = '" & pStrPeriodId & "'" & vbCrLf
            strSql = strSql & "                    AND NOT (( t1.d_from < t2.d_from AND t1.d_to < t2.d_from )" & vbCrLf
            strSql = strSql & "                     OR ( t1.d_from > t2.d_to AND t1.d_to > t2.d_to )))" & vbCrLf
            strSql = strSql & "    AND t1.c_committee_id = '" & pStrCommitteeID & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            tbRet = clsDB.ExecuteSql(strSql)                                    ' SQL実行
            If tbRet.Rows.Count > 0 Then
                strComName = NSMDCommon.NVL(tbRet.Rows(0).Item(0))
            Else
                strComName = ""
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
        Return strComName
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetPeriodOmission
    '   名称　：期短縮名取得
    '   概要  ：期IDから期短縮名を取得する
    '   引数　：なし
    '   戻り値：期短縮名
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function GetPeriodOmission(ByVal clsDB As CLAccessMdb, ByVal PstrPeriodId As String) As String
        Dim strSql As String = ""                                               ' SQL文
        Dim tbRet As DataTable = Nothing                                        ' 処理結果格納データテーブル
        Dim strPeriodOmission As String = ""                                    ' 期短縮名
        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT l_omission_name" & vbCrLf
            strSql = strSql & "   FROM period" & vbCrLf
            strSql = strSql & "  WHERE c_period_id = '" & PstrPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            tbRet = clsDB.ExecuteSql(strSql)                                    ' SQL実行
            If tbRet.Rows.Count > 0 Then
                strPeriodOmission = NSMDCommon.NVL(tbRet.Rows(0).Item(0))
            Else
                strPeriodOmission = ""
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
        Return strPeriodOmission
    End Function
#End Region

End Class
#End Region
