#Region "FM040207　クラス"

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Public Class FM040207

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
    Private STRIKE_KIND As String                                       '争議解除行為   03-05
    Private STRIKE_INFO As String                                       'Sequence
    Private TEXT_NO_KIND As String = "Ｂ（ＡＮＡ宛）"
#End Region

#Region "プロパティ"
    Public _bytStatus As Byte = 0                 ' ステータス（0：新規登録, 1:詳細 2：内容変更）
    Public _bytTabKind As Byte = 0                ' Tab Kind （1：本登録, 2：一時保管）
    Public _strStrikeId As String = ""            ' strikeID
    Public _strKind As String = ""                ' strike Kind 
    Public _strBasisStrikeID As String = ""       ' Basis strikeID
    Public _strPreScreenId As String = ""         ' 呼び元画面ID
    Public _SelectDataList As DataTable = Nothing                ' 別紙
    Public _strAnotherSubject As String = ""                      ' 別紙
    Private _IntQlickBtnFlag As Integer = -1      'クリックボタン判別用(OKボタン= 0 、キャンセルボタン = 1 、画面未表示(初期値) = -1)
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
    ' 呼び元画面ID
    Public Property strPreScreenId() As String
        Get
            Return _strPreScreenId
        End Get
        Set(ByVal value As String)
            _strPreScreenId = value
        End Set
    End Property
    Public Property SelectDataList() As DataTable
        Get
            Return _SelectDataList
        End Get
        Set(ByVal value As DataTable)
            _SelectDataList = value
        End Set
    End Property
    Public Property strAnotherSubject() As String
        Get
            Return _strAnotherSubject
        End Get
        Set(ByVal value As String)
            _strAnotherSubject = value
        End Set
    End Property
    Public Property IntQlickBtnFlag() As Integer
        Get
            Return _IntQlickBtnFlag
        End Get
        Set(ByVal value As Integer)
            _IntQlickBtnFlag = value
        End Set
    End Property
#End Region
#Region "イベント"
    Private Sub FM040207_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
    '************************************************************************************
    '   ＩＤ　：btnPickup_Click
    '   名称　：組合員選択
    '   概要　：組合員選択画面へ遷移
    '   作成日：2012/02/06 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/02/06 Kim  新規作成
    '************************************************************************************
    Private Sub btnPickup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPickup.Click
        Dim cFormFM000204 As New FM000204()
        Dim strUserIdList As List(Of String) = New List(Of String)

        If Me.dgvResult.Rows.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            For Each dgRow As DataGridViewRow In Me.dgvResult.Rows
                '既に表示済みのユーザーIDを格納
                strUserIdList.Add(dgRow.Cells(1).Value)
            Next
            '初期表示するメンバー削除不可
            cFormFM000204.AllowDeleteMember = True
            'ユーザーIDリストを組合員抽出画面に渡す
            cFormFM000204.StafIDList = strUserIdList.ToArray()
        End If

        '組合員抽出画面の表示
        cFormFM000204.ShowDialog()

        Select Case cFormFM000204.IntQlickBtnFlag
            Case 0 'OKボタン押下時
                '選択された組合員のリスト
                Dim dt As DataTable = cFormFM000204.SelectMemberList
                ' グリッド総数設定
                Me.dgvResult.RowCount = dt.Rows.Count
                For i = 0 To dt.Rows.Count - 1
                    With Me.dgvResult
                        .Rows(i).Cells.Item(0).Value = dt.Rows(i).Item("名前").ToString()
                        .Rows(i).Cells.Item(1).Value = dt.Rows(i).Item("社員番号").ToString()
                        .Rows(i).Cells.Item(2).Value = dt.Rows(i).Item("機種").ToString()
                        .Rows(i).Cells.Item(3).Value = dt.Rows(i).Item("組合支部").ToString()
                    End With
                Next
            Case 1
                'キャンセルの場合何も行わない
        End Select
        Me.Cursor = Cursors.Default
    End Sub

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

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ' 選択チェック
        If Not dataCheck() Then
            Exit Sub
        End If

        ' データテーブル格納
        ' データテーブルクリア
        Me.SelectDataList = Nothing
        Me.SelectDataList = New DataTable
        '列追加
        SelectDataList.Columns.Add("Column1")
        SelectDataList.Columns.Add("Column2")
        SelectDataList.Columns.Add("Column3")
        SelectDataList.Columns.Add("Column4")
        '値投入
        For i = 0 To Me.dgvResult.Rows.Count - 1
            '行追加
            Call Me.SelectDataList.Rows.Add()
            Me.SelectDataList.Rows(i).Item(0) = Me.dgvResult.Rows(i).Cells.Item(0).Value
            Me.SelectDataList.Rows(i).Item(1) = Me.dgvResult.Rows(i).Cells.Item(1).Value
            Me.SelectDataList.Rows(i).Item(2) = Me.dgvResult.Rows(i).Cells.Item(2).Value
            Me.SelectDataList.Rows(i).Item(3) = Me.dgvResult.Rows(i).Cells.Item(3).Value
        Next
        Me.strAnotherSubject = Me.txtAnotherSubject.Text
        ' 処理結果設定
        Me.DialogResult = Windows.Forms.DialogResult.OK

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
            Me.txtAnotherSubject.Text = "第1項、但し書きの「24時間ストライキ」を解除しない者。"
            Select Case Me.bytStatus
                Case STATUS_DETAIL
                    Me.txtAnotherSubject.Visible = True
                    Me.txtAnotherSubject.ReadOnly = True
                    Me.txtAnotherSubject.BackColor = Color.White
                    Me.btnPickup.Enabled = False
                    Me.btnOK.Enabled = False
                Case STATUS_EDIT, STATUS_INSERT
                    Me.txtAnotherSubject.Visible = True
                    Me.txtAnotherSubject.ReadOnly = False
                    Me.txtAnotherSubject.BackColor = Color.White
                    Me.btnPickup.Enabled = True
                    Me.btnOK.Enabled = True
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
        Dim intRetCnt As Integer
        Try
            Select Case Me.bytStatus
                Case STATUS_DETAIL, STATUS_EDIT
                    Select Case Me.bytTabKind
                        Case STATUS_TAB_HONB
                            ' データベース接続
                            Call clsDb.Connect()
                            '申請者 Check Null
                            If ChkNull(Me.strStrikeId) Then
                                CLMsg.Show("GE0001")
                                Return False
                            End If
                            '-------------------------------------------------------------------------------
                            '   SQL作成
                            '-------------------------------------------------------------------------------
                            strsql = ""
                            strsql = strsql & " select stli.k_strike_info,stli.l_another_subject"
                            strsql = strsql & " from   strike_list stli "
                            strsql = strsql & " where  stli.c_strike_id = '" & strStrikeId & "'"
                            strsql = strsql & ";"
                            ' SQL実行
                            dt = clsDb.ExecuteSql(strsql)
                            Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                            Me.txtNo.Text = strStrikeId
                            Me.txtAnotherSubject.Text = Me.strAnotherSubject
                            Call clsDb.Disconnect()
                        Case STATUS_TAB_TEMP
                            ' データベース接続
                            Call clsDb.Connect()
                            '申請者 Check Null
                            If ChkNull(Me.strStrikeId) Then
                                CLMsg.Show("GE0001")
                                Return False
                            End If
                            '-------------------------------------------------------------------------------
                            '   SQL作成
                            '-------------------------------------------------------------------------------
                            strsql = ""
                            strsql = strsql & " select stli.k_strike_info,stli.l_another_subject"
                            strsql = strsql & " from   strike_work_list stli "
                            strsql = strsql & " where  stli.c_strike_work_id = '" & strStrikeId & "'"
                            strsql = strsql & ";"
                            ' SQL実行
                            dt = clsDb.ExecuteSql(strsql)
                            Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                            Me.txtNo.Text = "*****"
                            Me.txtAnotherSubject.Text = Me.strAnotherSubject
                            'grid情報表示
                            '-------------------------------------------------------------------------------
                            '   SQL作成
                            '-------------------------------------------------------------------------------
                            strsql = " select stli.c_strike_work_id,stli.c_user_id,"
                            strsql = strsql & "        staf.l_name as c_user_name,"
                            strsql = strsql & "        cd1.l_name as belong_name,"
                            strsql = strsql & "        cd2.l_name as model_name "
                            strsql = strsql & " from   strike_work_member_list stli,"
                            strsql = strsql & "        staf_attribute staf, "
                            strsql = strsql & "        constant_dtl cd1, "
                            strsql = strsql & "        constant_dtl cd2, "
                            strsql = strsql & "        (SELECT staf1.c_user_id,MAX(staf1.d_from ) AS c_d_from"
                            strsql = strsql & "         FROM   staf_attribute AS staf1"
                            strsql = strsql & "         WHERE  staf1.k_del = '0'"
                            strsql = strsql & "         GROUP BY staf1.c_user_id"
                            strsql = strsql & "         ORDER BY staf1.c_user_id ) AS staf2"
                            strsql = strsql & " where  stli.c_strike_work_id = '" & strStrikeId & "'"
                            strsql = strsql & " and    staf2.c_user_id = staf.c_user_id"
                            strsql = strsql & " and    staf2.c_d_from =   staf.d_from"
                            strsql = strsql & " and    stli.c_user_id = staf.c_user_id"
                            strsql = strsql & " and    staf.k_belonging  = cd1.c_constant_seq"
                            strsql = strsql & " and    cd1.c_constant  = 'BELONGING'"
                            strsql = strsql & " and    staf.k_model  = cd2.c_constant_seq"
                            strsql = strsql & " and    cd2.c_constant  = 'MODEL'"
                            strsql = strsql & " order by stli.c_user_id;"
                            ' SQL実行
                            dt = clsDb.ExecuteSql(strsql)

                            intRetCnt = dt.Rows.Count                                                                ' 件数取得
                            ' 件数チェック
                            If intRetCnt > 0 Then
                                ' 1件以上の処理
                                dgvResult.RowCount = intRetCnt                                                        ' 縦総数設定
                                For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                                    ' データ設定
                                    With Me.dgvResult.Rows(i).Cells
                                        If (IsDBNull(dt.Rows(i).Item("c_user_name"))) Then
                                            .Item(0).Value = "*****"
                                        Else
                                            .Item(0).Value = dt.Rows(i).Item("c_user_name")
                                        End If
                                        .Item(1).Value = dt.Rows(i).Item("c_user_id")
                                        .Item(2).Value = dt.Rows(i).Item("belong_name")
                                        .Item(3).Value = dt.Rows(i).Item("model_name")
                                        Me.dgvResult.Rows(i).Visible = True
                                    End With
                                Next
                            End If
                            Call clsDb.Disconnect()
                        Case Else
                            Call CLMsg.Show("GE0004", "Source controlScreen")
                            Exit Function
                    End Select
                Case STATUS_INSERT
                    Me.txtNoKind.Text = TEXT_NO_KIND
                    Me.txtNo.Text = "*****"
                    If Me.strAnotherSubject = "" Then
                        Me.txtAnotherSubject.Text = "第1項、但し書きの「24時間ストライキ」を解除しない者。"
                    Else
                        Me.txtAnotherSubject.Text = Me.strAnotherSubject
                    End If
                Case Else
                    Call CLMsg.Show("GE0004", "Source controlScreen")
                    Exit Function
            End Select

            'grid情報表示
            If Not Me.SelectDataList Is Nothing Then
                intRetCnt = Me.SelectDataList.Rows().Count
                ' 件数チェック
                If intRetCnt > 0 Then
                    ' 1件以上の処理
                    Me.dgvResult.RowCount = intRetCnt
                    For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                        ' データ設定
                        With Me.dgvResult.Rows(i).Cells
                            .Item(0).Value = Me.SelectDataList.Rows(i).Item(0)
                            .Item(1).Value = Me.SelectDataList.Rows(i).Item(1)
                            .Item(2).Value = Me.SelectDataList.Rows(i).Item(2)
                            .Item(3).Value = Me.SelectDataList.Rows(i).Item(3)
                            'Me.dgvResult.Rows(i).Visible = True
                        End With
                    Next
                End If
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
    Private Function dataCheck() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If
            ' 選択チェック
            If Me.dgvResult.RowCount < 1 Then
                CLMsg.Show("GE0006", "組合員選択")
                Me.dgvResult.BackgroundColor = Color.LightPink
                Me.dgvResult.Focus()
                Return False
            End If
            '申請者 Check Null
            If ChkNull(Me.txtAnotherSubject.Text) Then
                CLMsg.Show("GE0006", "件名")
                Me.txtAnotherSubject.BackColor = Color.LightPink
                Me.txtAnotherSubject.Focus()
                Return False
            End If

            If (Me.txtAnotherSubject.Text.Length > 42) Then
                CLMsg.Show("GE0093", "件名")
                txtAnotherSubject.BackColor = Color.LightPink
                Me.txtAnotherSubject.Focus()
                Return False
            End If

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
#End Region

End Class
#End Region
