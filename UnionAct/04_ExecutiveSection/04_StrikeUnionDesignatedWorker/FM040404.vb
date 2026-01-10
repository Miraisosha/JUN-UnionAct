#Region "FM000204"

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDConst

Public Class FM040404
    Dim agoForm As System.Windows.Forms.UserControl

#Region "ログ出力オブジェクト"
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040404
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040404

    Private Const NOTICE_NUMBER As String = "通告番号"
    Private Const NOTICE_NUMBER_KIND As String = "通告番号種別"
    Private Const LABOR_DISPUTE_NOTICE_NUMBER As String = "争議行為通告番号"
    Private Const NOTICE_DATE As String = "日付"
    Private Const LABOR_DATE As String = "争議有効日付"
    Private Const STR_EVENT As String = "事件"

    Private ReadOnly ARR_SETVAL_COLUMNS_NAME As String() = {NOTICE_NUMBER_KIND, NOTICE_NUMBER, LABOR_DISPUTE_NOTICE_NUMBER, NOTICE_DATE, LABOR_DATE, STR_EVENT}
    Private ReadOnly ARR_SETVAL_COLUMNS_WIDTH As Integer() = {100, 100, 160, 100, 120, 400}
    Private ReadOnly ARR_SETVAL_COLUMNS_VISIBLE As Boolean() = {False, True, True, True, True, True}
#End Region

#Region "プロパティ"
    ' 画面関連
    Private _intQlickBtnFlag As Integer = -1                 ' 押下ボタンフラグ（0=OKボタン、1=キャンセルボタン）
    ' クリックボタン判別
    Public Property IntQlickBtnFlag() As Integer
        Get
            Return _intQlickBtnFlag
        End Get
        Set(ByVal value As Integer)
            _intQlickBtnFlag = value
        End Set
    End Property

    '選択行取得用
    Private _selectDataRow As DataRow = Nothing      ' 選択行格納
    'Private _selectDataRow As DataRow = Nothing
    Public Property SelectDataRow() As DataRow
        Get
            Return _selectDataRow
        End Get
        Set(ByVal value As DataRow)
            _selectDataRow = value
        End Set
    End Property

#End Region

    'Public Sub New()
    '    'ここに初期処理を書く
    '    InitializeComponent()
    'End Sub

    'Public Sub New(ByVal setForm As System.Windows.Forms.UserControl)
    '    'ここに初期処理を書く
    '    InitializeComponent()
    '    agoForm = setForm
    'End Sub

#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：FM040404_Load()
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2012/01/10 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub FM040404_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            ' 画面中央表示処理
            If SetFormCenter(Me) = False Then
                Me.Visible = False
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub

#End Region

#Region "OKボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnOK_Click()
    '   名称　：OKクリック
    '   概要  ：
    '   作成日：2012/01/10 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            IntQlickBtnFlag = 0
            '選択行が存在すれば（複数行指定は不可）プロパティにセット
            Call GetSelectRowInf()
            Me.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        
    End Sub

#End Region

#Region "ダブルクリック"
    Private Sub dgvResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.RowIndex <> -1 Then
                IntQlickBtnFlag = 0
                '選択行が存在すれば（複数行指定は不可）プロパティにセット
                Call GetSelectRowInf()
                Me.Visible = False
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region

#Region "キャンセルボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click()
    '   名称　：キャンセルクリック
    '   概要  ：
    '   作成日：2012/01/10 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            IntQlickBtnFlag = 1
            Me.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub

#End Region

#End Region


#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ShowLaborDisputeData()
    '   名称　：争議行為の情報を表示する。
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function ShowLaborDisputeData() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")

        Dim clsDb As New CLAccessMdb

        Try
            'SQL生成
            Dim strSql = "SELECT "
            strSql = strSql & "  stli.k_strike_info AS " & ARR_SETVAL_COLUMNS_NAME(0) & " , "
            strSql = strSql & "  Mid(stli.k_strike_info , 1 , 1) AS " & ARR_SETVAL_COLUMNS_NAME(1) & " , "
            strSql = strSql & "  stli.c_strike_id AS " & ARR_SETVAL_COLUMNS_NAME(2) & " , "
            strSql = strSql & "  stli.d_strike AS " & ARR_SETVAL_COLUMNS_NAME(3) & " , "
            strSql = strSql & "  stli.d_strike_start AS " & ARR_SETVAL_COLUMNS_NAME(4) & " , "
            strSql = strSql & "  stli.l_event AS " & ARR_SETVAL_COLUMNS_NAME(5) & " "
            strSql = strSql & " FROM  strike_list stli "
            strSql = strSql & " WHERE stli.k_strike_kind = '01' "
            strSql = strSql & " AND not exists(SELECT stli1.c_basis_strike_id "
            strSql = strSql & "                FROM   strike_list AS stli1 "
            strSql = strSql & "                WHERE  stli1.k_strike_kind = '06' "
            strSql = strSql & "                AND    stli.c_strike_id = stli1.c_basis_strike_id "
            strSql = strSql & "               )  "
            strSql = strSql & " ORDER BY stli.c_strike_id "

            'Dim strSql = "SELECT " + vbCrLf
            'strSql = strSql & " strike_list.k_strike_info AS " & ARR_SETVAL_COLUMNS_NAME(0) & " , " & vbCrLf
            'strSql = strSql & " Mid(strike_list.k_strike_info , 1 , 1) AS " & ARR_SETVAL_COLUMNS_NAME(1) & " , " & vbCrLf
            'strSql = strSql & " strike_list.c_strike_info AS " & ARR_SETVAL_COLUMNS_NAME(2) & " , " & vbCrLf
            'strSql = strSql & " strike_list.d_strike AS " & ARR_SETVAL_COLUMNS_NAME(3) & " , " & vbCrLf
            'strSql = strSql & " strike_list.d_strike_start AS " & ARR_SETVAL_COLUMNS_NAME(4) & " , " & vbCrLf
            'strSql = strSql & " strike_list.l_event AS " & ARR_SETVAL_COLUMNS_NAME(5) & " " & vbCrLf
            'strSql = strSql & "FROM strike_list WHERE k_strike_kind <> '終結' "
            'strSql = strSql & "ORDER BY len(strike_list.c_strike_info) , strike_list.c_strike_info "

            'DB接続+SQL結果取得
            Call clsDb.Connect()
            Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strSql)

            '結果判定
            If dtSqlResult.Rows.Count = 0 Then
                'エラーメッセージ
                CLMsg.Show("GI0042")
                Return False
            Else
                'Dim bsSource As New BindingSource
                'bsSource.DataSource = dtSqlResult
                'dgvResult.DataSource = bsSource
                '日付データはyyyy/MM/ddにする必要があるのでバインドせずに取得データを整形

                ' ***  列作成  ***
                If Not MakeDataGridViewColumns() Then
                    Return False
                End If

                ' ***  行データ投入  ***
                For i = 0 To dtSqlResult.Rows.Count - 1
                    '行作成
                    dgvResult.Rows.Add()

                    '列データ投入(取得列数分ループ)
                    For intColCnt = 0 To dtSqlResult.Columns.Count - 1

                        Dim strColData As String = ""
                        strColData = NVL(dtSqlResult.Rows(i).Item(ARR_SETVAL_COLUMNS_NAME(intColCnt)))
                        '日付箇所はyyyy/MM/ddにする
                        If dtSqlResult.Columns(intColCnt).ColumnName = NOTICE_DATE OrElse dtSqlResult.Columns(intColCnt).ColumnName = LABOR_DATE Then
                            Dim intDate As Integer
                            If strColData.Length > 0 AndAlso Integer.TryParse(strColData, intDate) Then
                                strColData = Date.Parse(Format(intDate, "0000/00/00")).ToString("yyyy/MM/dd")
                            End If
                        End If
                        Me.dgvResult.Rows(i).Cells.Item(intColCnt).Value = strColData
                    Next
                Next

                'Dim intCnt As Integer = 0
                'For Each cell As DataGridViewColumn In dgvResult.Columns
                '    cell.HeaderText = ARR_SETVAL_COLUMNS_NAME(intCnt)
                '    cell.Width = ARR_SETVAL_COLUMNS_WIDTH(intCnt)
                '    cell.Visible = ARR_SETVAL_COLUMNS_VISIBLE(intCnt)
                '    intCnt = intCnt + 1
                'Next

                ' 自分自身を表示
                Me.ShowDialog()
                Return True
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSelectRowInf()
    '   名称　：選択行を取得する（プロパティに設定する）
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function GetSelectRowInf() As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim dtReturn As DataTable = New DataTable   '戻り値格納用
        Dim arrAddData() As String = Nothing        '選択行データ取得配列
        Dim iCounter As Integer
        Dim returnVal As Boolean = False
        Try
            If Me.dgvResult.SelectedRows.Count >= 1 Then

                '列を作成しておく
                For intColCnt As Integer = 0 To dgvResult.ColumnCount - 1
                    dtReturn.Columns.Add(ARR_SETVAL_COLUMNS_NAME(intColCnt))
                Next

                'データ取得
                iCounter = 0
                For Each GetCell As DataGridViewCell In dgvResult.SelectedRows.Item(0).Cells()
                    ReDim Preserve arrAddData(iCounter)
                    If Not (IsDBNull(GetCell.Value)) Then
                        arrAddData(iCounter) = GetCell.Value.ToString
                    Else
                        arrAddData(iCounter) = ""
                    End If
                    iCounter = iCounter + 1
                Next

                dtReturn.Rows.Add(arrAddData)
                SelectDataRow = dtReturn.Rows(0)
                '戻り値
                returnVal = True

            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Return False
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return returnVal

    End Function

    '***************************************************************************************************
    '   ＩＤ　：MakeDataGridViewColumns()
    '   名称　：データグリッドビューの列作成を行う
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function MakeDataGridViewColumns() As Boolean

        Try
            For intCnt As Integer = 0 To ARR_SETVAL_COLUMNS_NAME.Length - 1
                '列名、ヘッダー
                dgvResult.Columns.Add(ARR_SETVAL_COLUMNS_NAME(intCnt), ARR_SETVAL_COLUMNS_NAME(intCnt))
                '幅
                dgvResult.Columns(intCnt).Width = ARR_SETVAL_COLUMNS_WIDTH(intCnt)
                '可視
                dgvResult.Columns(intCnt).Visible = ARR_SETVAL_COLUMNS_VISIBLE(intCnt)
                '並び替え
                dgvResult.Columns(intCnt).SortMode = DataGridViewColumnSortMode.NotSortable

                '改行表示対応
                dgvResult.Columns(intCnt).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            Next

            dgvResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Return False
        End Try
        Return True

    End Function

#End Region


    
End Class

#End Region