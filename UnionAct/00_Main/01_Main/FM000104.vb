#Region "FM000104"
'===========================================================================================================
'   クラスＩＤ　　：FM000104
'   クラス名称　　：部／委員会選択画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon

Public Class FM000104

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM000104                     ' FM000104
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM000104                 ' 部／委員会選択画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "プロパティ"
    ' Input
    Public _strMemberNo As String = ""              ' 社員番号
    ' Oupput
    Public _dtSelData As DataTable = Nothing        ' 選択データ
    Public _intPeriod As Integer = Nothing          ' 01. 期（数値のみ）
    Public _strCommitteeName As String = ""         ' 02. 委員会名称
    Public _strPostName As String = ""              ' 03. 役職名称
    Public _strTerm As String = ""                  ' 04. 期間From～期間To
    Public _strCommitteeId As String = ""           ' 05. 委員会ID
    Public _strPostId As String = ""                ' 06. 役職ID
    Public _strPeriodFrom As String = ""            ' 07. 期間From（yyyy/MM/dd）
    Public _strPeriodTo As String = ""              ' 08. 期間To（yyyy/MM/dd）

    ' 社員番号
    Public Property strMemberNo() As String
        Get
            Return _strMemberNo
        End Get
        Set(ByVal value As String)
            _strMemberNo = value
        End Set
    End Property
    ' 選択データ
    Public Property dtSelData() As DataTable
        Get
            Return _dtSelData
        End Get
        Set(ByVal value As DataTable)
            _dtSelData = value
        End Set
    End Property
    ' 期（数値のみ）
    Public Property intPeriod() As Integer
        Get
            Return _intPeriod
        End Get
        Set(ByVal value As Integer)
            _intPeriod = value
        End Set
    End Property
    ' 委員会名称
    Public Property strCommitteeName() As String
        Get
            Return _strCommitteeName
        End Get
        Set(ByVal value As String)
            _strCommitteeName = value
        End Set
    End Property
    ' 役職名称
    Public Property strPostName() As String
        Get
            Return _strPostName
        End Get
        Set(ByVal value As String)
            _strPostName = value
        End Set
    End Property
    ' 期間
    Public Property strTerm() As String
        Get
            Return _strTerm
        End Get
        Set(ByVal value As String)
            _strTerm = value
        End Set
    End Property
    ' 委員会ID
    Public Property strCommitteeId() As String
        Get
            Return _strCommitteeId
        End Get
        Set(ByVal value As String)
            _strCommitteeId = value
        End Set
    End Property
    ' 役職ID
    Public Property strPostId() As String
        Get
            Return _strPostId
        End Get
        Set(ByVal value As String)
            _strPostId = value
        End Set
    End Property
    ' 期間From
    Public Property strPeriodFrom() As String
        Get
            Return _strPeriodFrom
        End Get
        Set(ByVal value As String)
            _strPeriodFrom = value
        End Set
    End Property
    ' 期間To
    Public Property strPeriodTo() As String
        Get
            Return _strPeriodTo
        End Get
        Set(ByVal value As String)
            _strPeriodTo = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000104_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM000104_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '---------------------------------------------------------------------------
            '   グリッド初期化
            '---------------------------------------------------------------------------
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            '---------------------------------------------------------------------------
            '   各データ取得
            '---------------------------------------------------------------------------
            If GetData() = False Then
                Me.Visible = False
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOk_Click
    '   名称　：OKボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            ' 選択情報取得処理
            If getSelData() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
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
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel                      ' キャンセルボタン押下結果格納
            Me.Close()                                                                      ' 画面閉じる
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_KeyDown()
    '   名称　：グリッドキーダウン処理
    '   概要  ：
    '   作成日：2011/11/21(月)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvResult.KeyDown
        Try
            ' Enterキーの場合
            If e.KeyCode = Keys.Enter Then
                If getSelData() = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_CellDoubleClick()
    '   名称　：グリッドセルダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/21(月)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick
        Try
            ' ヘッダー行ではない場合
            If e.RowIndex <> -1 Then
                If getSelData() = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
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
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni() As Boolean
        Dim blnRet As Boolean = False   ' 処理結果
        Try
            '-------------------------------------------------------------------------------
            '   グリッド全体設定
            '-------------------------------------------------------------------------------
            ' 総数
            Me.dgvResult.RowCount = 1                                                       ' 縦
            Me.dgvResult.ColumnCount = 6                                                    ' 横
            ' スクロールバー
            Me.dgvResult.ScrollBars = ScrollBars.Both
            ' 縦のみ
            ' 1行選択モード
            Me.dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect            ' 1行選択
            Me.dgvResult.MultiSelect = False                                                ' 複数選択なし
            ' サイズ変更
            Me.dgvResult.AllowUserToResizeColumns = True                                  ' 列サイズ変更禁止
            Me.dgvResult.AllowUserToResizeRows = False                                      ' 行サイズ変更禁止
            '-------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-------------------------------------------------------------------------------
            Me.dgvResult.Columns(0).HeaderText = "期"                                       ' 期
            Me.dgvResult.Columns(1).HeaderText = "委員会名称"                               ' 委員会名称
            Me.dgvResult.Columns(2).HeaderText = "役職"                                     ' 役職名称
            Me.dgvResult.Columns(3).HeaderText = "期間"                                     ' 期間
            Me.dgvResult.Columns(4).HeaderText = "委員会ID"                                 ' 委員会ID
            Me.dgvResult.Columns(5).HeaderText = "役職ID"                                   ' 役職ID
            ' ヘッダー文字位置
            Me.dgvResult.Columns("PeriodNo").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter         ' 期
            Me.dgvResult.Columns("CommitteeName").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter    ' 委員会名称
            Me.dgvResult.Columns("PostName").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter         ' 役職名称
            Me.dgvResult.Columns("Term").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter             ' 期間
            Me.dgvResult.Columns("CommitteeId").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 委員会ID
            Me.dgvResult.Columns("PostId").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter           ' 役職ID
            '-----------------------------------------------------------------------------------
            '   カラム部設定
            '-----------------------------------------------------------------------------------
            ' カラム文字位置
            Me.dgvResult.Columns("PeriodNo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter         ' 期
            Me.dgvResult.Columns("CommitteeName").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter    ' 委員会名称
            Me.dgvResult.Columns("PostName").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter         ' 役職名称
            Me.dgvResult.Columns("Term").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter             ' 期間
            Me.dgvResult.Columns("CommitteeId").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 委員会ID
            Me.dgvResult.Columns("PostId").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter           ' 役職ID
            ' カラム幅
            Me.dgvResult.Columns(0).Width = 30                                                                              ' 期
            Me.dgvResult.Columns(1).Width = 175                                                                             ' 委員会名称
            Me.dgvResult.Columns(2).Width = 125                                                                              ' 役職名称
            Me.dgvResult.Columns(3).Width = 185                                                                             ' 期間
            Me.dgvResult.Columns(4).Width = 100                                                                             ' 委員会ID
            Me.dgvResult.Columns(5).Width = 100                                                                             ' 役職ID
            ' カラム表示有無
            Me.dgvResult.Columns(0).Visible = True                                                                          ' 期
            Me.dgvResult.Columns(1).Visible = True                                                                          ' 委員会名称
            Me.dgvResult.Columns(2).Visible = True                                                                          ' 役職名称
            Me.dgvResult.Columns(3).Visible = True                                                                          ' 期間
            Me.dgvResult.Columns(4).Visible = False                                                                         ' 委員会ID
            Me.dgvResult.Columns(5).Visible = False                                                                         ' 役職ID
            blnRet = True                                                                                                   ' 戻り値格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス生成
        Dim strSql As String = ""                                                           ' SQL文
        Dim tbRet As DataTable = Nothing                                                    ' 処理結果データテーブル
        Dim intRetCnt As Integer = Nothing                                                  ' 件数
        Dim strTerm As String = ""                                                          ' 期間
        Dim strTargetDate As String = ""                                                    ' 対象日付け（yyyyMMdd）
        Dim strDateNow As String = ""                                                       ' 対象日付現在日（yyyyMMdd）
        Dim intRet As Integer = 0                                                           ' 件数
        Try
            Call clsDb.Connect()                                                            ' データベース接続
            '-------------------------------------------------------------------------------
            '   期マスタから各期の適用開始日取得
            '-------------------------------------------------------------------------------
            ' 現在日取得
            strDateNow = Now.ToString("yyyyMMdd")

            ' SQL作成
            strSql = strSql & "" & vbCrLf
            strSql = strSql & " SELECT c_period_id" & vbCrLf
            strSql = strSql & "   FROM period " & vbCrLf
            strSql = strSql & "  WHERE d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "    AND d_to   >= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & ";"
            tbRet = clsDb.ExecuteSql(strSql)                                                ' SQL実行
            intRet = tbRet.Rows.Count                                                       ' 件数取得
            If intRet = 1 Then
                If tbRet.Rows(0).Item(0).ToString() = MDLoginInfo.PeriodId Then
                    strTargetDate = strDateNow
                Else
                    strTargetDate = MDLoginInfo.PeriodTo
                End If
            Else
                strTargetDate = MDLoginInfo.PeriodTo
            End If
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & "   SELECT DISTINCT " & vbCrLf
            strSql = strSql & "          prod.l_omission_name AS period_no" & vbCrLf
            strSql = strSql & "         ,comt.l_name AS CommitteeName" & vbCrLf
            strSql = strSql & "         ,cmtd.l_name AS PostName" & vbCrLf
            strSql = strSql & "         ,comt.d_from" & vbCrLf
            strSql = strSql & "         ,comt.d_to" & vbCrLf
            strSql = strSql & "         ,comt.c_committee_id" & vbCrLf
            strSql = strSql & "         ,cmtd.s_committee_seq" & vbCrLf
            strSql = strSql & "     FROM period AS prod" & vbCrLf
            strSql = strSql & "         ,( SELECT a2.c_committee_list" & vbCrLf
            strSql = strSql & "                  ,a2.c_period_id" & vbCrLf
            strSql = strSql & "                  ,a2.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,a2.d_from" & vbCrLf
            strSql = strSql & "              FROM committee_list AS a2" & vbCrLf
            strSql = strSql & "                  ,( SELECT a.c_period_id" & vbCrLf
            strSql = strSql & "                           ,a.c_committee_id" & vbCrLf
            strSql = strSql & "                           ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                       FROM committee_list AS a" & vbCrLf
            strSql = strSql & "                      WHERE a.d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      GROUP BY a.c_period_id" & vbCrLf
            strSql = strSql & "                              ,a.c_committee_id" & vbCrLf
            strSql = strSql & "                      ORDER BY a.c_period_id" & vbCrLf   'ok
            strSql = strSql & "                              ,a.c_committee_id" & UtDb.DbOrderOffset & vbCrLf
            strSql = strSql & "                               ) AS a1" & vbCrLf
            strSql = strSql & "             WHERE a1.c_period_id = a2.c_period_id" & vbCrLf
            strSql = strSql & "               AND a1.c_committee_id = a2.c_committee_id" & vbCrLf
            strSql = strSql & "               AND a1.d_from = a2.d_from ) AS cmtl" & vbCrLf
            strSql = strSql & "         ,( SELECT b2.c_committee_list" & vbCrLf
            strSql = strSql & "                  ,b2.c_user_id" & vbCrLf
            strSql = strSql & "                  ,b2.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,b2.s_committee_seq" & vbCrLf
            strSql = strSql & "                  ,b2.d_from" & vbCrLf
            strSql = strSql & "              FROM committee_list_dtl AS b2" & vbCrLf
            strSql = strSql & "                  ,( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                           ,b.c_committee_id" & vbCrLf
            strSql = strSql & "                           ,b.s_committee_seq" & vbCrLf
            strSql = strSql & "                           ,b.d_from" & vbCrLf
            strSql = strSql & "                       FROM committee_list_dtl AS b" & vbCrLf
            strSql = strSql & "                      WHERE b.d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      ORDER BY b.c_user_id" & vbCrLf 'ok
            strSql = strSql & "                              ,b.c_committee_id" & vbCrLf
            strSql = strSql & "                              ,b.s_committee_seq" & UtDb.DbOrderOffset & vbCrLf
            strSql = strSql & "                               ) AS b1" & vbCrLf
            strSql = strSql & "             WHERE b1.c_user_id = b2.c_user_id" & vbCrLf
            strSql = strSql & "               AND b1.c_committee_id = b2.c_committee_id" & vbCrLf
            strSql = strSql & "               AND b1.s_committee_seq = b2.s_committee_seq" & vbCrLf
            strSql = strSql & "               AND b1.d_from = b2.d_from ) AS cmld" & vbCrLf
            strSql = strSql & "         ,( SELECT c.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,c.s_committee_seq" & vbCrLf
            strSql = strSql & "                  ,c.l_name" & vbCrLf
            strSql = strSql & "                  ,MAX(c.d_from) AS d_from_max" & vbCrLf
            strSql = strSql & "              FROM committee_dtl AS c" & vbCrLf
            strSql = strSql & "             WHERE c.d_from <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             AND   c.d_to   >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             GROUP BY c.c_committee_id" & vbCrLf
            strSql = strSql & "                     ,c.s_committee_seq" & vbCrLf
            strSql = strSql & "                     ,c.l_name" & vbCrLf
            strSql = strSql & "                     ,c.d_from" & vbCrLf
            strSql = strSql & "             ORDER BY c.c_committee_id" & vbCrLf 'ok
            strSql = strSql & "                     ,c.s_committee_seq" & UtDb.DbOrderOffset & vbCrLf
            strSql = strSql & "                     ) AS cmtd" & vbCrLf
            strSql = strSql & "         ,( SELECT d.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,d.l_name" & vbCrLf
            strSql = strSql & "                  ,d.d_to" & vbCrLf
            strSql = strSql & "                  ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "              FROM committee AS d" & vbCrLf
            strSql = strSql & "             WHERE d.d_from <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             AND   d.d_to   >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             GROUP BY d.c_committee_id" & vbCrLf
            strSql = strSql & "                     ,d.l_name" & vbCrLf
            strSql = strSql & "                     ,d.d_to" & vbCrLf
            strSql = strSql & "             ORDER BY d.c_committee_id" & UtDb.DbOrderOffset & vbCrLf    'ok
            strSql = strSql & "              ) AS comt" & vbCrLf
            strSql = strSql & "     WHERE cmld.c_user_id   = '" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & "       AND cmtl.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "       AND comt.d_from     <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND comt.d_to       >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND prod.c_period_id      = cmtl.c_period_id" & vbCrLf
            strSql = strSql & "       AND cmtl.c_committee_list = cmld.c_committee_list" & vbCrLf
            strSql = strSql & "       AND cmld.c_committee_id   = cmtd.c_committee_id" & vbCrLf
            strSql = strSql & "       AND cmld.s_committee_seq  = cmtd.s_committee_seq" & vbCrLf
            strSql = strSql & "       AND cmtl.c_committee_id   = comt.c_committee_id" & vbCrLf
            ' 管理部マスタ情報を連結
            strSql = strSql & "   UNION ALL" & vbCrLf
            strSql = strSql & "   SELECT prod.l_omission_name AS period_no" & vbCrLf
            strSql = strSql & "         ,dpmt.l_name AS CommitteeName" & vbCrLf
            strSql = strSql & "         ,dpmd.l_name AS PostName" & vbCrLf
            strSql = strSql & "         ,dpmt.d_from" & vbCrLf
            strSql = strSql & "         ,dpmt.d_to" & vbCrLf
            strSql = strSql & "         ,dpmt.c_department_id" & vbCrLf
            strSql = strSql & "         ,dpmd.s_department_seq" & vbCrLf
            strSql = strSql & "     FROM period AS prod" & vbCrLf
            strSql = strSql & "         ,( SELECT e2.c_department_list" & vbCrLf
            strSql = strSql & "                  ,e2.c_period_id" & vbCrLf
            strSql = strSql & "                  ,e2.c_department_id" & vbCrLf
            strSql = strSql & "            FROM department_list AS e2" & vbCrLf
            strSql = strSql & "                  ,( SELECT e.c_period_id" & vbCrLf
            strSql = strSql & "                           ,e.c_department_id" & vbCrLf
            strSql = strSql & "                           ,MAX(e.c_department_list) AS c_department_list" & vbCrLf
            strSql = strSql & "                       FROM department_list AS e" & vbCrLf
            strSql = strSql & "                      WHERE RIGHT(e.c_department_list,8) <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      GROUP BY e.c_period_id" & vbCrLf
            strSql = strSql & "                              ,e.c_department_id" & vbCrLf
            strSql = strSql & "                      ORDER BY e.c_period_id" & vbCrLf   'ok
            strSql = strSql & "                              ,e.c_department_id" & UtDb.DbOrderOffset & vbCrLf
            strSql = strSql & "                              ) AS e1" & vbCrLf
            strSql = strSql & "            WHERE e1.c_period_id = e2.c_period_id" & vbCrLf
            strSql = strSql & "              AND e1.c_department_id = e2.c_department_id" & vbCrLf
            strSql = strSql & "              AND e1.c_department_list = e2.c_department_list ) AS dpml" & vbCrLf
            strSql = strSql & "         ,department_list_dtl AS dpld" & vbCrLf
            strSql = strSql & "         ,department_dtl AS dpmd" & vbCrLf
            strSql = strSql & "         ,department AS dpmt" & vbCrLf
            strSql = strSql & "     WHERE dpld.c_user_id   = '" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & "       AND dpml.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "       AND dpmt.d_from     <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND dpmt.d_to       >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND prod.c_period_id       = dpml.c_period_id" & vbCrLf
            strSql = strSql & "       AND dpml.c_department_list = dpld.c_department_list" & vbCrLf
            strSql = strSql & "       AND dpld.c_department_id   = dpmd.c_department_id" & vbCrLf
            strSql = strSql & "       AND dpld.s_department_seq  = CStr(dpmd.s_department_seq)" & vbCrLf
            strSql = strSql & "       AND dpml.c_department_id   = dpmt.c_department_id" & vbCrLf
            strSql = strSql & " ;" & vbCrLf
            tbRet = clsDb.ExecuteSql(strSql)                                                ' SQL実行

            intRetCnt = tbRet.Rows.Count                                                    ' 処理件数取得
            Me.dgvResult.RowCount = intRetCnt
            If intRetCnt > 0 Then                                                           ' 件数チェック
                For i = 0 To intRetCnt - 1                                                  ' 1件以上の処理
                    ' データ投入
                    With Me.dgvResult.Rows(i).Cells
                        .Item(0).Value = tbRet.Rows(i).Item(0).ToString()                   ' 01. 期（数値のみ）
                        .Item(1).Value = tbRet.Rows(i).Item(1).ToString()                   ' 02. 委員会名称
                        .Item(2).Value = tbRet.Rows(i).Item(2).ToString()                   ' 03. 役職名称
                        ' 04. 期間From～期間To
                        strTerm = Date.Parse(Format(CInt(tbRet.Rows(i).Item(3).ToString), "0000/00/00")).ToString("yyyy/MM")
                        strTerm = strTerm & "～"
                        If tbRet.Rows(i).Item(4).ToString() = "99999999" Then
                            .Item(3).Value = strTerm & "2099/12"
                        Else
                            .Item(3).Value = strTerm & Date.Parse(Format(CInt(tbRet.Rows(i).Item(4).ToString), "0000/00/00")).ToString("yyyy/MM")
                        End If
                        .Item(4).Value = tbRet.Rows(i).Item(5).ToString()                   ' 05. 委員会ID
                        .Item(5).Value = tbRet.Rows(i).Item(6).ToString()                   ' 06. 役職ID
                    End With
                Next
            Else
                CLMsg.Show("DI0001")                                                        ' 0件の処理（対象データなしメッセージボックス表示）
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                         ' データベース切断
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetSelData
    '   名称　：選択情報設定処理
    '   概要  ：選択情報設定処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>選択情報設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetSelData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim drRet As DataRow                                                                ' 取得用データロー
        Try
            ' 選択されているかチェック
            If Me.dgvResult.SelectedRows.Count < 0 Then
                ' 未選択の場合、エラーメッセージ表示
                CLMsg.Show("GW0001", "データ")
                Return blnRet
            End If
            '---------------------------------------------------------------------------
            '   選択情報設定
            '---------------------------------------------------------------------------
            ' 選択データ
            dtSelData = New DataTable
            dtSelData.Columns.Add("期（数値のみ）", GetType(Integer))                       ' 01. 期（数値のみ）
            dtSelData.Columns.Add("委員会名称", GetType(String))                            ' 02. 委員会名称
            dtSelData.Columns.Add("役職名称", GetType(String))                              ' 03. 役職名称
            dtSelData.Columns.Add("期間From～期間To", GetType(String))                      ' 04. 期間From～期間To
            dtSelData.Columns.Add("委員会ID", GetType(String))                              ' 05. 委員会ID
            dtSelData.Columns.Add("役職ID", GetType(String))                                ' 06. 役職ID
            drRet = dtSelData.NewRow()
            drRet(0) = CInt(Me.dgvResult.SelectedRows.Item(0).Cells(0).Value)               ' 01. 期（数値のみ）
            drRet(1) = Me.dgvResult.SelectedRows.Item(0).Cells(1).Value                     ' 02. 委員会名称
            drRet(2) = Me.dgvResult.SelectedRows.Item(0).Cells(2).Value                     ' 03. 役職名称
            drRet(3) = Me.dgvResult.SelectedRows.Item(0).Cells(3).Value                     ' 04. 期間From～期間To
            drRet(4) = Me.dgvResult.SelectedRows.Item(0).Cells(4).Value                     ' 05. 委員会ID
            drRet(5) = Me.dgvResult.SelectedRows.Item(0).Cells(5).Value                     ' 06. 役職ID
            dtSelData.Rows.Add(drRet)

            MDLoginInfo.Period = CInt(Me.dgvResult.SelectedRows.Item(0).Cells(0).Value)     ' 01. 期（数値のみ）
            MDLoginInfo.CommitteeName = Me.dgvResult.SelectedRows.Item(0).Cells(1).Value    ' 02. 委員会名称
            MDLoginInfo.PostName = Me.dgvResult.SelectedRows.Item(0).Cells(2).Value         ' 03. 役職名称
            MDLoginInfo.CommitteeId = Me.dgvResult.SelectedRows.Item(0).Cells(4).Value      ' 05. 委員会ID
            MDLoginInfo.PostId = Me.dgvResult.SelectedRows.Item(0).Cells(5).Value           ' 06. 役職ID

            ' 委員会ステータスフラグ（0：専従, 1：委員会, 2：管理部）取得
            ' 管理部用委員会IDリスト取得
            Call clsDb.Connect()                                                            ' データベース接続
            If MDLoginInfo.CommitteeId.Substring(0, 1) = "M" Then
                MDLoginInfo.CommitteeStatusFlg = 2                                          ' 委員会ステータスフラグ（2：管理部）
                ' 管理部委員会IDリスト取得処理
                If getDepartmentCommitteeIdList(clsDb, MDLoginInfo.CommitteeId) = False Then
                    Exit Function
                End If
            Else
                MDLoginInfo.CommitteeStatusFlg = 1                                          ' 委員会ステータスフラグ（1：委員会）
                MDLoginInfo.CommitteeIdList.Clear()
            End If

            'Me.intPeriod = CInt(Me.dgvResult.SelectedRows.Item(0).Cells(0).Value)           ' 01. 期（数値のみ）
            'Me.strCommitteeName = Me.dgvResult.SelectedRows.Item(0).Cells(1).Value          ' 02. 委員会名称
            'Me.strPostName = Me.dgvResult.SelectedRows.Item(0).Cells(2).Value               ' 03. 役職名称
            'Me.strTerm = Me.dgvResult.SelectedRows.Item(0).Cells(3).Value                   ' 04. 期間From～期間To
            'Me.strCommitteeId = Me.dgvResult.SelectedRows.Item(0).Cells(4).Value            ' 05. 委員会ID
            'Me.strPostId = Me.dgvResult.SelectedRows.Item(0).Cells(5).Value                 ' 06. 役職ID

            Me.DialogResult = System.Windows.Forms.DialogResult.OK                          ' OKボタン押下結果格納
            Me.Visible = False                                                              ' 画面非表示
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                         ' データベース切断
        End Try
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：getSelData
    '   名称　：選択情報取得処理
    '   概要  ：選択情報取得処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/12(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/12(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>選択情報取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function getSelData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            If SetSelData() = False Then
                Exit Function
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK                                 ' OKボタン押下結果格納
            Me.Close()                                                                      ' 画面閉じる
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値設定
    End Function

#End Region

End Class

#End Region
