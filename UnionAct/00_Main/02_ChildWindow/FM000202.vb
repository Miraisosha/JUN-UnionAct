#Region "FM000202"
'===========================================================================================================
'   クラスＩＤ　　：FM000202
'   クラス名称　　：
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDChk

Public Class FM000202

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000202                                ' FM000202
    Private Const SCREEN_NAME = SCREEN_NAME_FM000202                            ' 住所検索結果画面
    ReadOnly ARR_STR_ADDRESS_COLUMNSNAME As String() = {"郵便番号", "都道府県名", "市区町村名", "町域名"}
    ReadOnly ARR_INT_ADDRESS_COLUMNSWIDTH As Integer() = {100, 140, 140, 140}
    ReadOnly ARR_BLN_ADDRESS_COLUMNSSHOW As Boolean() = {True, True, True, True}
    Const RESULT_CNT As String = "検索結果"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "プロパティ"
    Public _strSqlSentence As String = ""                                       ' SQL文
    Public _intQlickBtnFlag As Integer = -1                                     ' 押下ボタンフラグ（0=OKボタン、1=キャンセルボタン）
    Public _blnExist = False                                                    ' 検索結果が存在するかどうかのフラグ
    Public _SelectAddress As DataTable = New DataTable                          ' 選択行格納
    Public _intSearchHitCnt As Integer = 0                                      ' 検索結果

    ' SQL格納
    Public Property strSqlSentence() As String
        Get
            Return _strSqlSentence
        End Get
        Set(ByVal value As String)
            _strSqlSentence = value
        End Set
    End Property

    ' クリックボタン判別
    Public Property IntQlickBtnFlag() As Integer
        Get
            Return _intQlickBtnFlag
        End Get
        Set(ByVal value As Integer)
            _intQlickBtnFlag = value
        End Set
    End Property

    ' データ存在の有無確認用
    Public Property BlnExist() As Integer
        Get
            Return _blnExist
        End Get
        Set(ByVal value As Integer)
            _blnExist = value
        End Set
    End Property

    ' 選択行のデータ格納用
    Public Property SelectAddress() As DataTable
        Get
            Return _SelectAddress
        End Get
        Set(ByVal value As DataTable)
            _SelectAddress = value
        End Set
    End Property

    ' 検索結果数
    Public Property intSearchHitCnt() As Integer
        Get
            Return _intSearchHitCnt
        End Get
        Set(ByVal value As Integer)
            _intSearchHitCnt = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000202_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/16  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/16  somesaki  新規作成
    '***************************************************************************************************
    Private Sub FM000202_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            ' 画面中央表示処理
            If SetFormCenter(Me) = False Then
                Me.Visible = False
            End If
            '' データ表示処理（件数チェック）
            'If SqlQueryShowDataGrid() Then
            '    Me.Visible = False
            'End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：BtnDecide_Click
    '   名称　：決定ボタン処理
    '   概要  ：
    '   作成日：2011/11/08 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08 somesaki  新規作成
    '***************************************************************************************************
    Private Sub BtnDecide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDecide.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            '選択行なし―　GE0010
            IntQlickBtnFlag = 0 '戻り
            ' 処理結果にOK格納(依頼対応）
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Call SetSelData()

            'Dim dtReturn As DataTable = New DataTable   '戻り値格納用
            'Dim arrAddData() As String = Nothing        '選択行データ取得配列
            'Dim iCounter As Integer                     'カウンタ

            ''列を作成しておく
            'For intColCnt As Integer = 0 To dgdResultSQL.ColumnCount - 1
            '    dtReturn.Columns.Add(ARR_STR_ADDRESS_COLUMNSNAME(intColCnt))
            'Next

            ''データ取得
            'iCounter = 0
            'For Each GetCell As DataGridViewCell In dgdResultSQL.SelectedRows.Item(0).Cells()
            '    ReDim Preserve arrAddData(iCounter)
            '    If Not (IsDBNull(GetCell.Value)) Then
            '        'arrAddData.Add(GetCell.Value.ToString)
            '        arrAddData(iCounter) = GetCell.Value.ToString
            '    Else
            '        arrAddData(iCounter) = ""
            '    End If
            '    iCounter = iCounter + 1
            'Next

            ''データ追加
            'dtReturn.Rows.Add(arrAddData)
            'SelectAddress = dtReturn
            Me.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Me.Visible = False
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタン処理
    '   概要  ：
    '   作成日：2011/11/08  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08  somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            IntQlickBtnFlag = 1
            ' 処理結果にキャンセル格納
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            ' 画面閉じる
            Me.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdResultSQL_CellDoubleClick
    '   名称　：データグリッドダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/08  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08  somesaki  新規作成
    '***************************************************************************************************
    Private Sub dgdResultSQL_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdResultSQL.CellDoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        IntQlickBtnFlag = 0 '戻り
        ' 処理結果にOK格納(依頼対応）
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Try
            If e.RowIndex <> -1 Then
                If SetSelData() = False Then
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
        Me.Visible = False
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdResultSQL_KeyDown
    '   名称　：データグリッドキープレス処理
    '   概要  ：
    '   作成日：2011/11/08  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08  somesaki  新規作成
    '***************************************************************************************************
    Private Sub dgdResultSQL_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgdResultSQL.KeyDown
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        IntQlickBtnFlag = 0 '戻り
        ' 処理結果にOK格納(依頼対応）
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Try
            If e.KeyCode = Keys.Enter Then
                If SetSelData() = False Then
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
        Me.Visible = False
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：SqlQueryShowDataGrid
    '   名称　：SQL結果データグリッド表示
    '   概要　：SQLを発行し結果をデータグリッドに表示する
    '   引数　：なし
    '   戻り値：検索結果数
    '   作成日：2011/11/09  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/09  somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>SQL結果グリッド表示処理</summary>
    ''' <returns>検索結果件数</returns>
    ''' <remarks></remarks>
    Public Function SqlQueryShowDataGrid() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        'DB接続
        Dim db_inf As New CLAccessMdb
        Dim dtSqlResult As New DataTable    'Sql結果格納用
        Dim blnRet As Boolean = False
        Try
            If Not ChkNull(strSqlSentence) Then
                ' SQLあり
                Call db_inf.Connect()                               ' データベース接続
                dtSqlResult = db_inf.ExecuteSql(strSqlSentence)     ' SQL実行
                If dtSqlResult.Rows.Count <= 0 Then
                    CLMsg.Show("DI0001")                            ' メッセージ表示
                    Return blnRet                                   ' 処理抜ける
                End If
                ' データグリッドビューに表示
                gbxResult.Text = RESULT_CNT + "(" + dtSqlResult.Rows.Count.ToString + "件)"     ' 件数表示
                Dim bsSource As New BindingSource
                bsSource.DataSource = dtSqlResult
                dgdResultSQL.DataSource = bsSource
                Dim intCnt As Integer = 0
                For Each cell As DataGridViewColumn In dgdResultSQL.Columns
                    cell.HeaderText = ARR_STR_ADDRESS_COLUMNSNAME(intCnt)
                    cell.Width = ARR_INT_ADDRESS_COLUMNSWIDTH(intCnt)
                    cell.Visible = ARR_BLN_ADDRESS_COLUMNSSHOW(intCnt)
                    intCnt = intCnt + 1
                Next
                Me.ShowDialog()                                     ' 自分自身を表示
            Else
                ' SQLなし
                Call CLMsg.Show("GI0042")                           ' メッセージ表示
                Return blnRet                                       ' 処理抜ける
            End If
            blnRet = True                                           ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call db_inf.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return blnRet                                               ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetSelData
    '   名称　：選択されている行の情報格納
    '   概要　：選択行情報をデータテーブルに格納市プロパティにセットする
    '   引数　：なし
    '   戻り値：検索結果数
    '   作成日：2011/11/09  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/09  somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>選択されている行の情報格納</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetSelData() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim dtReturn As DataTable = New DataTable   '戻り値格納用
            Dim arrAddData() As String = Nothing        '選択行データ取得配列
            Dim iCounter As Integer                     'カウンタ

            '列を作成しておく
            For intColCnt As Integer = 0 To dgdResultSQL.ColumnCount - 1
                dtReturn.Columns.Add(ARR_STR_ADDRESS_COLUMNSNAME(intColCnt))
            Next
            'データ取得
            iCounter = 0
            For Each GetCell As DataGridViewCell In dgdResultSQL.SelectedRows.Item(0).Cells()
                ReDim Preserve arrAddData(iCounter)
                If Not (IsDBNull(GetCell.Value)) Then
                    'arrAddData.Add(GetCell.Value.ToString)
                    arrAddData(iCounter) = GetCell.Value.ToString
                Else
                    arrAddData(iCounter) = ""
                End If
                iCounter = iCounter + 1
            Next
            'データ追加
            dtReturn.Rows.Add(arrAddData)
            SelectAddress = dtReturn
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Return False
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return True
    End Function
#End Region

End Class

#End Region
