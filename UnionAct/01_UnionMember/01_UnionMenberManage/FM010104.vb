#Region "FM010104"
'===========================================================================================================
'   クラスＩＤ　　：FM010104
'   クラス名称　　：適用日付選択画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDConst

Public Class FM010104

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM010104                                ' FM010104
    Private Const SCREEN_NAME = SCREEN_NAME_FM010104                            ' 適用日付選択画面
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Const MESSAGE_ERR_INSERT_FUTUREDAY As String = "GE0089"
    Private Const MESSAGE_CHECK_CONTINUE As String = "GQ0024"
#End Region

#Region "プロパティ"
    Private _StrSqlSentence As String = ""              ' SQL文
    Private _IntQlickBtnFlag As Integer = -1            ' クリックボタン判別用（0 = OKボタン, 1 = キャンセルボタン）
    Private _IsDirectInsert As Boolean = True           ' 直接指定が選択されているか（True = 選択, False = 未選択）
    Public _DirectInsertValue As Date = New Date        ' 直接入力時の設定値
    Public _SelectRowInf As DataTable = New DataTable   ' 選択行
    Private _SetCulumnsName As String()                 ' 列名
    Private _SetCulumnsWidth As Integer()               ' 列幅
    Private _SetCulumnsShow As Boolean()                ' 列表示
    Private _ShowInsertDataMessage As Boolean = False   ' メッセージ表示フラグ（加入日を入力してください）
    Private _EnableChkDirectSpecify = True               ' 直接入力の使用可能フラグ初期表示

    ' SQL格納用（グリッド初期値のSQLを呼出元から取得。）
    Public Property strSqlSentence() As String
        Get
            Return _StrSqlSentence
        End Get
        Set(ByVal value As String)
            _StrSqlSentence = value
        End Set
    End Property
    ' クリックボタン判別用
    Public Property IntQlickBtnFlag() As Integer
        Get
            Return _IntQlickBtnFlag
        End Get
        Set(ByVal value As Integer)
            _IntQlickBtnFlag = value
        End Set
    End Property
    ' 選択行取得用
    Public Property SelectRowInf() As DataTable
        Get
            Return _SelectRowInf
        End Get
        Set(ByVal value As DataTable)
            _SelectRowInf = value
        End Set
    End Property
    ' 直接入力を行うかの判別
    Public Property IsDirectInsert() As Boolean
        Get
            Return _IsDirectInsert
        End Get
        Set(ByVal value As Boolean)
            _IsDirectInsert = value
        End Set
    End Property
    ' 直接入力時の入力値
    Public Property DirectInsertValue() As Date
        Get
            Return _DirectInsertValue
        End Get
        Set(ByVal value As Date)
            _DirectInsertValue = value
        End Set
    End Property
    ' データグリッドのヘッダーに表示する列名
    Public Property SetCulumnsName() As String()
        Get
            Return _SetCulumnsName
        End Get
        Set(ByVal value As String())
            _SetCulumnsName = value
        End Set
    End Property
    ' データグリッドのヘッダーに表示する列幅
    Public Property SetCulumnsWidth() As Integer()
        Get
            Return _SetCulumnsWidth
        End Get
        Set(ByVal value As Integer())
            _SetCulumnsWidth = value
        End Set
    End Property
    ' データグリッドのヘッダーの列表示・非表示
    Public Property SetCulumnsShow() As Boolean()
        Get
            Return _SetCulumnsShow
        End Get
        Set(ByVal value As Boolean())
            _SetCulumnsShow = value
        End Set
    End Property
    'メッセージ表示フラグ
    Public Property ShowInsertDataMessage() As Boolean
        Get
            Return _ShowInsertDataMessage
        End Get
        Set(ByVal value As Boolean)
            _ShowInsertDataMessage = value
        End Set
    End Property
    '直接指定
    Public Property EnableChkDirectSpecify() As Boolean
        Get
            Return _EnableChkDirectSpecify
        End Get
        Set(ByVal value As Boolean)
            _EnableChkDirectSpecify = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM010104_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/16  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/16  somesaki  新規作成
    '***************************************************************************************************
    Private Sub FM010104_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim intRet As Integer = Nothing     ' グリッド行件数
        Try
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            'メッセージの表示切替
            If ShowInsertDataMessage Then
                lblInsertMessage.Visible = True
            End If
            If Not EnableChkDirectSpecify Then
                Me.chkDirectSpecify.Enabled = False
                Me.dtpSpecifyTime.Enabled = False
            End If
            ' SQLプロパティがない場合は履歴は表示しない
            If strSqlSentence.Length = 0 Then
                Me.dgdHistoryList.Visible = False       ' グリッドビュー非表示
                Me.chkDirectSpecify.Checked = True      ' 直接入力ラジオボタンチェック
                Me.chkDirectSpecify.Enabled = False     ' 直接入力ラジオボタン使用不可
            Else
                ' データ取得表示処理
                intRet = SqlQueryShowDataGrid()
            End If
            If Me.dgdHistoryList.Rows.Count = 0 Then
                Me.dgdHistoryList.Visible = False       ' グリッドビュー非表示
                Me.chkDirectSpecify.Checked = True      ' 直接入力ラジオボタンチェック
                Me.chkDirectSpecify.Enabled = False     ' 直接入力ラジオボタン使用不可
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
    '   ＩＤ　：btnOK_Click
    '   名称　：OKボタンクリック
    '   概要　：
    '   作成日：2011/11/07 m.somezaki
    '   更新日：
    '----------------------------------------------------------------------------
    '   履歴　：2011/11/07 m.somezaki 新規
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            Call executeOK()
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

    '************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2011/11/07(月) m.somezaki
    '   更新日：
    '----------------------------------------------------------------------------
    '   履歴　：2011/10/27(木) m.somezaki 新規
    '************************************************************************************ 
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            IntQlickBtnFlag = 1
            Me.Visible = False
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

    '************************************************************************************
    '   ＩＤ　：dgdHistoryList_KeyDown
    '   名称　：キーダウン
    '   概要　：
    '   作成日：2011/11/07(月) m.somezaki
    '   更新日：
    '----------------------------------------------------------------------------
    '   履歴　：2011/10/27(木) m.somezaki 新規
    '************************************************************************************
    Private Sub dgdHistoryList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgdHistoryList.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Call executeOK()
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

    '************************************************************************************
    '   ＩＤ　：chkDirectSpecify_CheckedChanged
    '   名称　：直接指定チェック
    '   概要　：
    '   作成日：2011/11/07(月) m.somezaki
    '   更新日：
    '   備考　：
    '----------------------------------------------------------------------------
    '   履歴　：2011/10/27(木) m.somezaki 新規
    '************************************************************************************ 
    Private Sub chkDirectSpecify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDirectSpecify.CheckedChanged
        Try
            If Me.chkDirectSpecify.Checked Then
                Me.dgdHistoryList.Visible = False
            Else
                Me.dgdHistoryList.Visible = True
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

    '************************************************************************************
    '   ＩＤ　：dgdHistoryList_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要　：
    '   作成日：2011/12/16  m.somezaki
    '   更新日：
    '   備考　：
    '----------------------------------------------------------------------------
    '   履歴　：2011/12/16  m.somezaki 新規
    '************************************************************************************ 
    Private Sub dgdHistoryList_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdHistoryList.CellDoubleClick
        Try
            If e.RowIndex <> -1 Then
                Call GetSelectHistory()
                IntQlickBtnFlag = 0
                Me.Visible = False
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

#Region "関数"
    '************************************************************************************
    '   ＩＤ　：SqlQueryShowDataGrid
    '   名称　：データグリッド表示処理
    '   概要　：
    '   引数　：なし
    '   戻り値：SqlQueryShowDataGrid As Integer = 件数
    '   作成日：2011/11/07  m.somezaki
    '   更新日：
    '   備考　：
    '----------------------------------------------------------------------------
    '   履歴　：2011/12/16  m.somezaki 新規
    '************************************************************************************ 
    ''' <summary>データグリッド表示処理</summary>
    ''' <returns>件数</returns>
    ''' <remarks></remarks>
    Public Function SqlQueryShowDataGrid() As Integer
        Dim db_inf As CLAccessMdb = Nothing
        Dim dtResultSql As New DataTable
        Try
            ' DB接続
            db_inf = New CLAccessMdb
            dtResultSql = New DataTable
            Call db_inf.Connect()
            ' SQL実行
            dtResultSql = db_inf.ExecuteSql(strSqlSentence)
            Dim bsSource As New BindingSource
            bsSource.DataSource = dtResultSql
            dgdHistoryList.DataSource = bsSource
            ' 切断
            Call db_inf.Disconnect()
            '
            If Me.dgdHistoryList.Rows.Count > 0 Then
                ' データグリッドのヘッダー設定
                Dim intCnt As Integer = 0
                For Each cell As DataGridViewColumn In dgdHistoryList.Columns
                    cell.HeaderText = _SetCulumnsName(intCnt)
                    cell.Width = _SetCulumnsWidth(intCnt)
                    cell.Visible = _SetCulumnsShow(intCnt)
                    intCnt = intCnt + 1
                Next
            End If
            Return Me.dgdHistoryList.Rows.Count
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Function

    '************************************************************************************
    '   ＩＤ　：GetSelectHistory
    '   名称　：データグリッドビュー選択行を返す設定を行う
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/16 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16 somezaki  新規作成
    '************************************************************************************
    ''' <summary>データグリッドビュー選択行を返す設定を行う</summary>
    ''' <remarks></remarks>
    Private Sub GetSelectHistory()
        Try
            IsDirectInsert = False
            Dim dtReturn As DataTable = New DataTable
            Dim arrAddData() As String = Nothing
            Dim iCounter As Integer
            '列作成
            For intColCnt As Integer = 0 To dgdHistoryList.ColumnCount - 1
                dtReturn.Columns.Add(SetCulumnsName(intColCnt))
            Next
            'データ取得
            iCounter = 0
            For Each GetCell As DataGridViewCell In dgdHistoryList.SelectedRows.Item(0).Cells()
                ReDim Preserve arrAddData(iCounter)
                If Not (IsDBNull(GetCell.Value)) Then
                    arrAddData(iCounter) = GetCell.Value.ToString
                Else
                    arrAddData(iCounter) = ""
                End If
                iCounter = iCounter + 1
            Next
            dtReturn.Rows.Add(arrAddData)
            SelectRowInf = dtReturn
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

    '************************************************************************************
    '   ＩＤ　：executeOK
    '   名称　：データグリッドビュー選択行を返す設定を行う
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/16  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16  somezaki  新規作成
    '************************************************************************************ 
    ''' <summary>データグリッドビュー選択行を返す設定を行う</summary>
    ''' <remarks></remarks>
    Private Sub executeOK()
        Const STR_D_FROM_MAXVAL As String = "99999999" '適用開始日の最大値
        Try
            If Me.chkDirectSpecify.Checked Then '直接入力
                IsDirectInsert = True 'プロパティセット
                Dim dtMostNew As DateTime = New DateTime()
                '履歴の最新データ取得
                For Each rowCheckDgdRow As DataGridViewRow In dgdHistoryList.Rows
                    'ありえないが適用開始日が99999999だったら…
                    If rowCheckDgdRow.Cells.Item("d_from").Value.ToString.Trim() = STR_D_FROM_MAXVAL Then
                        CLMsg.Show(MESSAGE_ERR_INSERT_FUTUREDAY, "9999/99/99") '
                        Return
                    End If
                    If dtMostNew < DateTime.Parse(rowCheckDgdRow.Cells.Item("d_from").Value) Then
                        dtMostNew = DateTime.Parse(rowCheckDgdRow.Cells.Item("d_from").Value)
                    End If
                Next
                '入力値が履歴データより新しいかチェック
                Dim setDate As Date = dtpSpecifyTime.Value.Date
                '履歴情報のほうが新しいか同じならエラー
                If dtMostNew >= setDate Then
                    CLMsg.Show(MESSAGE_ERR_INSERT_FUTUREDAY, dtMostNew.ToString("yyyy/MM/dd")) '
                    Return
                Else
                    Dim insertVal As Date = dtpSpecifyTime.Value.Date
                    If DateTime.Today < insertVal Then  '現在時刻より未来かチェック
                        If CLMsg.Show(MESSAGE_CHECK_CONTINUE) = DialogResult.No Then '未来日付でも続行確認メッセージ
                            Return
                        End If
                    End If
                    DirectInsertValue = dtpSpecifyTime.Value.Date
                End If

            Else '履歴から選択
                Call GetSelectHistory()
            End If
            IntQlickBtnFlag = 0
            Me.Visible = False
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

End Class

#End Region