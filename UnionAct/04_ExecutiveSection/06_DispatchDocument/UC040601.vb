#Region "UC040601"
'===========================================================================================================
'   クラスＩＤ　　：UC040601
'   クラス名称　　：発信文書検索画面
'   備考  　　　　：
'===========================================================================================================

Imports System.Data.OleDb
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDFile
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common

Public Class UC040601

#Region "定数・変数"
    '---------------------------------------------------------------
    '   定数
    '---------------------------------------------------------------
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040601          ' UC040601
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040601      ' 発信文書画面
    ' ステータス
    Private Const STATUS_INSERT As String = "0"                     ' 新規作成（発信済・一時保存）
    Private Const STATUS_SHOW As String = "1"                       ' 表示（発信済・一時保存）
    Private Const STATUS_EDIT As String = "2"                       ' 編集（発信済・一時保存）
    Private Const STATUS_COPY_EDIT As String = "3"                  ' コピーして編集（発信済・一時保存）
    Private Const STATUS_DELETE As String = "4"                     ' 削除（一時保存のみ）
    ' タブ
    Private Const TAB_NORMAL As Byte = 0                            ' 発信済タブ
    Private Const TAB_TMP As Byte = 1                               ' 一時保存タブ
    ' 任意入力関連
    Private Const MANUAL_INSERT As String = "999"                   ' 任意入力コード番号
    Private Const MANUAL_INSERT_SEARCHWORD As String = "その他"     ' 任意入力
    ' 文字列
    Private Const STR_INSERT As String = "新規作成"                 ' 新規作成（発信済・一時保存）
    Private Const STR_SHOW As String = "表示"                       ' 表示（発信済・一時保存）
    Private Const STR_EDIT As String = "編集"                       ' 編集（発信済・一時保存）
    Private Const STR_COPY_EDIT As String = "コピーして編集"        ' コピーして編集（発信済・一時保存）
    Private Const STR_DELETE As String = "削除"                     ' 削除（一時保存のみ）

    '---------------------------------------------------------------
    '   変数
    '---------------------------------------------------------------
    ' 権限
    Private strGrantReference As String = "0"                       ' 参照権限
    Private strGrantInsert As String = "0"                          ' 登録権限
    Private strGrantPrint As String = "0"                           ' 印刷権限
    Private strGrantFileOutput As String = "0"                      ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _strDocCode As String = ""                               ' 管理コード
    Public _strDocNumber As String = ""                             ' 文書№（文書番号）
    Public _strPeriodIdD As String = ""                             ' 期ID（文書番号Ｄ文書用）
    Public _strDocNumberD As String = ""                            ' 文書№（文書番号Ｄ文書用）
    Public _strFile As String = ""                                  ' ファイル名
    Public _strIssueDate As String = ""                             ' 発行日
    Public _strDocId As String = ""                                 ' 文書フルパス（文書ID）
    Public _intDocId As Integer = 0                                 ' 文書識別コード
    Public _strPeriodId As String = ""                              ' 期ID
    Public _intPeriod As Integer = 0                                ' 期（数値）
    Public _strPeriodName As String = ""                            ' 期（全角期（第ＸＸ期））
    Public _intSubjectSeq As Integer = 0                            ' 標題枝番
    Public _strSubject As String = ""                               ' 標題
    Public _strTemplate As String = ""                              ' テンプレート区分
    Public _strApplyDate As String = ""                             ' 適用日付（要件改訂で不要っぽい）

    ' 委員の追加・削除、長の交代用
    Public _strCommitteeUpdate As String() = Nothing                ' 委員会変更ID
    Public _strDocumentOut As String() = Nothing                    ' 帳票出力
    Public _strCommitteeDFrom As String() = Nothing                 ' 委員会適用日付

    Public _strDetails As String = ""                               ' 詳細設定分類
    Public _bytSaveKindFlg As Byte = 0                              ' 保存した文書種別（0：発信済, 1：一時保存）

    ' 新規作成のみ
    Public _strTemplateFile As String = ""                          ' テンプレートファイル名（ファイル名のみ）
    Public _strTemplateFileFull As String = ""                      ' テンプレートファイル名（フルパス）

    ' 管理コード
    Public Property strDocCode() As String
        Get
            Return _strDocCode
        End Get
        Set(ByVal value As String)
            _strDocCode = value
        End Set
    End Property

    ' 文書№（文書番号）
    Public Property strDocNumber() As String
        Get
            Return _strDocNumber
        End Get
        Set(ByVal value As String)
            _strDocNumber = value
        End Set
    End Property

    ' 期ID（期IDＤ文書用）
    Public Property strPeriodIdD() As String
        Get
            Return _strPeriodIdD
        End Get
        Set(ByVal value As String)
            _strPeriodIdD = value
        End Set
    End Property

    ' 文書№（文書番号Ｄ文書用）
    Public Property strDocNumberD() As String
        Get
            Return _strDocNumberD
        End Get
        Set(ByVal value As String)
            _strDocNumberD = value
        End Set
    End Property

    ' ファイル名
    Public Property strFile() As String
        Get
            Return _strFile
        End Get
        Set(ByVal value As String)
            _strFile = value
        End Set
    End Property

    ' 発行日
    Public Property strIssueDate() As String
        Get
            Return _strIssueDate
        End Get
        Set(ByVal value As String)
            _strIssueDate = value
        End Set
    End Property

    ' 文書フルパス（文書ID）
    Public Property strDocId() As String
        Get
            Return _strDocId
        End Get
        Set(ByVal value As String)
            _strDocId = value
        End Set
    End Property

    ' 文書識別コード
    Public Property intDocId() As Integer
        Get
            Return _intDocId
        End Get
        Set(ByVal value As Integer)
            _intDocId = value
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

    ' 期（数値）
    Public Property intPeriod() As Integer
        Get
            Return _intPeriod
        End Get
        Set(ByVal value As Integer)
            _intPeriod = value
        End Set
    End Property

    ' 期（全角期（第ＸＸ期））
    Public Property strPeriodName() As String
        Get
            Return _strPeriodName
        End Get
        Set(ByVal value As String)
            _strPeriodName = value
        End Set
    End Property

    ' 標題枝番
    Public Property intSubjectSeq() As Integer
        Get
            Return _intSubjectSeq
        End Get
        Set(ByVal value As Integer)
            _intSubjectSeq = value
        End Set
    End Property

    ' 標題
    Public Property strSubject() As String
        Get
            Return _strSubject
        End Get
        Set(ByVal value As String)
            _strSubject = value
        End Set
    End Property

    ' テンプレート区分
    Public Property strTemplate() As String
        Get
            Return _strTemplate
        End Get
        Set(ByVal value As String)
            _strTemplate = value
        End Set
    End Property

    ' 適用日付（要件改訂で不要っぽい）
    Public Property strApplyDate() As String
        Get
            Return _strApplyDate
        End Get
        Set(ByVal value As String)
            _strApplyDate = value
        End Set
    End Property

    ' 委員会変更ID
    Public Property strCommitteeUpdate() As String()
        Get
            Return _strCommitteeUpdate
        End Get
        Set(ByVal value As String())
            _strCommitteeUpdate = value
        End Set
    End Property

    ' 帳票出力
    Public Property strDocumentOut() As String()
        Get
            Return _strDocumentOut
        End Get
        Set(ByVal value As String())
            _strDocumentOut = value
        End Set
    End Property

    ' 委員会適用日付
    Public Property strCommitteeDFrom() As String()
        Get
            Return _strCommitteeDFrom
        End Get
        Set(ByVal value As String())
            _strCommitteeDFrom = value
        End Set
    End Property

    ' 詳細設定分類
    Public Property strDetails() As String
        Get
            Return _strDetails
        End Get
        Set(ByVal value As String)
            _strDetails = value
        End Set
    End Property

    ' 保存した文書種別（0：発信済, 1：一時保存）
    Public Property bytSaveKindFlg() As Byte
        Get
            Return _bytSaveKindFlg
        End Get
        Set(ByVal value As Byte)
            _bytSaveKindFlg = value
        End Set
    End Property

    ' テンプレートファイル名（ファイル名のみ）
    Public Property strTemplateFile() As String
        Get
            Return _strTemplateFile
        End Get
        Set(ByVal value As String)
            _strTemplateFile = value
        End Set
    End Property

    ' テンプレートファイル名（フルパス）
    Public Property strTemplateFileFull() As String
        Get
            Return _strTemplateFileFull
        End Get
        Set(ByVal value As String)
            _strTemplateFileFull = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM040604_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040601_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If Me.setGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            ' 一時保存
            If Me.DataGridViewIni(TAB_TMP, _
                               Me.dgvResultTmp) = False Then
                Exit Sub
            End If
            ' 発信済
            If Me.DataGridViewIni(TAB_NORMAL, _
                               Me.dgvResult) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If Me.GetData() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   検索データ取得処理
            '-------------------------------------------------------------------------------
            ' 一時保存
            If Me.GetSearchData(TAB_TMP, _
                                Me.dgvResultTmp) = False Then
                Exit Sub
            End If
            ' 発信済
            If Me.GetSearchData(TAB_NORMAL, _
                                Me.dgvResult) = False Then
                Exit Sub
            End If

            ' 発信済タブ表示
            Me.TabDoc.SelectedIndex = 0

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
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            '-------------------------------------------------------------------------------
            '   検索データ取得処理
            '-------------------------------------------------------------------------------
            If Me.GetSearchData(TAB_NORMAL, _
                                Me.dgvResult) = False Then
                Exit Sub
            End If

            ' タブ設定
            Me.TabDoc.SelectedIndex = TAB_NORMAL

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
    '   ＩＤ　：btnCreate_Click
    '   名称　：新規作成ボタンボタンクリック処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        Try
            '-------------------------------------------------------------------------------
            '   新規作成メイン処理
            '-------------------------------------------------------------------------------
            If Me.MainInsert() = False Then
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

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnShow_Click
    '   名称　：表示ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click

        Try
            '-------------------------------------------------------------------------------
            '   各データ格納
            '-------------------------------------------------------------------------------
            If Me.SetData(STR_SHOW) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   表示メイン処理
            '-------------------------------------------------------------------------------
            If Me.MainShow() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：btnEdit_Click
    '   名称　：編集ボタンクリック処理処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            '-------------------------------------------------------------------------------
            '   各データ格納
            '-------------------------------------------------------------------------------
            If Me.SetData(STR_EDIT) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   編集メイン処理
            '-------------------------------------------------------------------------------
            If Me.MainEdit() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：btnCopyEdit_Click
    '   名称　：コピーして編集ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCopyEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyEdit.Click

        Try
            '-------------------------------------------------------------------------------
            '   各データ格納
            '-------------------------------------------------------------------------------
            If Me.SetData(STR_COPY_EDIT) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コピーして編集メイン処理
            '-------------------------------------------------------------------------------
            If Me.MainCopyEdit() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：btnDelete_Click
    '   名称　：削除ボタンボタンクリック
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Dim daiRet As DialogResult = Nothing        ' 確認メッセージ結果

        Try
            '-------------------------------------------------------------------------------
            '   各データ格納
            '-------------------------------------------------------------------------------
            If Me.SetData(STR_DELETE) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   削除確認メッセージ表示
            '-------------------------------------------------------------------------------
            daiRet = CLMsg.Show("GQ0062", Me.strFile)

            ' 押下ボタン判定
            If daiRet = DialogResult.No Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   一時保存文書削除処理
            '-------------------------------------------------------------------------------
            If Me.MainDelete() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_CellDoubleClick()
    '   名称　：発信済グリッドセルダブルクリック処理
    '   概要  ：発信済グリッドセルをダブルクリックした行データの詳細画面を表示する。
    '   作成日：2012/03/18(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/18(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' ヘッダー行かチェック
            If e.RowIndex <> -1 Then
                '---------------------------------------------------
                '   各データ格納
                '---------------------------------------------------
                If Me.SetData(STR_SHOW) = False Then
                    Exit Sub
                End If
                '---------------------------------------------------
                '   表示メイン処理
                '---------------------------------------------------
                If Me.MainShow() = False Then
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResultTmp_CellDoubleClick()
    '   名称　：一時保存グリッドセルダブルクリック処理
    '   概要  ：一時保存グリッドセルをダブルクリックした行データの詳細画面を表示する。
    '   作成日：2012/03/26(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResultTmp_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResultTmp.CellDoubleClick

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' ヘッダー行かチェック
            If e.RowIndex <> -1 Then
                '---------------------------------------------------
                '   各データ格納
                '---------------------------------------------------
                If Me.SetData(STR_SHOW) = False Then
                    Exit Sub
                End If
                '---------------------------------------------------
                '   表示メイン処理
                '---------------------------------------------------
                If Me.MainShow() = False Then
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：optSpecify_CheckedChanged
    '   名称　：ラジオボタン変更処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub optSpecify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optSpecify.CheckedChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
                Exit Sub
            End If

            Utilities.SetEnabledProperty(Me.optSpecify.Checked, New Control() {Me.dtpIssueDate})

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
    '   ＩＤ　：cboManageCode_SelectedIndexChanged
    '   名称　：管理コードコンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboManageCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDocCode.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
                Exit Sub
            End If

            ' 標第分類コンボボックスリストクリア
            Me.cboSubjectSeq.DataSource = Nothing

            ' 管理コードが選択されている場合
            If Me.cboDocCode.SelectedIndex > 0 Then
                ' 標題分類コンボボックスリスト作成処理
                If Me.CreateSubjectKindList(Me.cboDocCode.SelectedValue.ToString()) = False Then
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
    '   ＩＤ　：cmbPeriod_SelectedIndexChanged
    '   名称　：期コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cmbPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：txtDocumentNumber_TextChanged
    '   名称　：文書番号テキストボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtDocumentNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDocNo.TextChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：cboSubjectKind_SelectedIndexChanged
    '   名称　：標題分類コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboSubjectKind_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubjectSeq.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：TabDoc_SelectedIndexChanged
    '   名称　：タブチェンジ処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub TabDoc_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabDoc.SelectedIndexChanged

        Dim blnFlg As Boolean = False   ' 表示・非表示フラグ（True：表示, False：非表示）

        Try
            ' 表示タブ判定
            If Me.TabDoc.SelectedIndex = 0 Then
                blnFlg = True
            End If

            ' タブチェンジ処理
            If Me.TabChange(blnFlg) = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：dtpIssueDate_ValueChanged
    '   名称　：発行年月チェンジ処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dtpIssueDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpIssueDate.ValueChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
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

    '***************************************************************************************************
    '   ＩＤ　：txtDocNo_GotFocus
    '   名称　：文書番号テキストボックスフォーカス取得処理
    '   概要　：
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtDocNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocNo.GotFocus

        Try
            Me.txtDocNo.SelectAll()
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
    '   ＩＤ　：cboDocCode_KeyDown
    '   名称　：管理コードコンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboDocCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboDocCode.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：cboPeriod_KeyDown
    '   名称　：期コンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboPeriod_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboPeriod.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：txtDocNo_KeyDown
    '   名称　：文書番号テキストボックスキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtDocNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDocNo.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：cboSubjectSeq_KeyDown
    '   名称　：標類分類コンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboSubjectSeq_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboSubjectSeq.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：optNotSpecify_KeyDown
    '   名称　：指定しないオプションボタンキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub optNotSpecify_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles optNotSpecify.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：optSpecify_KeyDown
    '   名称　：指定するオプションボタンキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub optSpecify_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles optSpecify.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：dtpIssueDate_KeyDown
    '   名称　：発行年月デートタイムピッカーキーダウン処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dtpIssueDate_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpIssueDate.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            '---------------------------------------------------
            '   検索条件
            '---------------------------------------------------
            ' ComboBox
            Me.cboDocCode.DataSource = Nothing                  ' 管理コード
            Me.cboDocCode.Text = ""
            Me.cboPeriod.DataSource = Nothing                   ' 期
            Me.cboPeriod.Text = ""
            Me.cboSubjectSeq.DataSource = Nothing              ' 標題分類
            Me.cboSubjectSeq.Text = ""
            ' DateTimePicker
            Me.dtpIssueDate.Text = ""                           ' 指定する
            ' TextBox
            Me.txtDocNo.Text = ""                               ' 文書番号
            ' Option
            Me.optNotSpecify.Checked = True                     ' 指定しない
            Me.optSpecify.Checked = False                       ' 指定する

            '---------------------------------------------------
            '   発信文書一覧
            '---------------------------------------------------
            ' Button
            Me.btnSearch.Visible = True                         ' 検索
            Me.btnShow.Visible = True                           ' 表示
            Me.btnCreate.Visible = True                         ' 新規作成
            Me.btnEdit.Visible = True                           ' 編集
            Me.btnCopyEdit.Visible = True                       ' コピーして編集
            Me.btnDelete.Visible = True                         ' 削除
            ' GroupBox
            Me.grpIssueDocList.Visible = True                   ' 検索結果
            ' Tab
            Me.tbpIssued.Focus()                                ' 発信済

            '---------------------------------------------------
            '   権限設定
            '---------------------------------------------------
            If strGrantInsert = GRANT_VALID Then

                ' 権限有り
                Me.btnCreate.Enabled = True                     ' 新規作成
                Me.btnEdit.Enabled = True                       ' 編集
                Me.btnCopyEdit.Enabled = True                   ' コピーして編集
                Me.btnDelete.Enabled = True                     ' 削除

            ElseIf strGrantInsert = GRANT_VOID Then

                ' 権限無し
                Me.btnCreate.Enabled = False                    ' 新規作成
                Me.btnEdit.Enabled = False                      ' 編集
                Me.btnCopyEdit.Enabled = False                  ' コピーして編集
                Me.btnDelete.Enabled = True                     ' 削除

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
    '   ＩＤ　：TabChange
    '   名称　：タブチェンジ処理
    '   概要　：検索条件情報表示の切り替えを行う。
    '   引数　：ByVal iBlnTab As Boolean = 発信文書一覧タブ（True：発信済, False：一時保存）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>タブチェンジ処理</summary>
    ''' <param name="iBlnTab">発信文書一覧タブ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TabChange(ByVal iBlnTab As Boolean) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim blnDisplayFlg As Boolean = False        ' ボタン表示・非表示フラグ
        Dim blnIssueDate As Boolean = False         ' 発行年月コンボボックス用表示・非表示フラグ

        Try
            '---------------------------------------------------
            '   ボタン表示・非表示判定
            '---------------------------------------------------
            ' タブ表示によって、データが1件以上あれば、ボタン表示。データが1件以上なければ、ボタン非表示。
            If iBlnTab Then
                '===================================
                '   発信済タブ表示
                '===================================
                ' 検索データ有無判定
                If Me.dgvResult.RowCount > 0 Then
                    blnDisplayFlg = True
                End If
                ' 指定するが選択されている場合
                If Me.optSpecify.Checked Then
                    blnIssueDate = True                         ' 発行年月コンボボックス用表示・非表示フラグ
                End If
            Else
                '===================================
                '   一時保存タブ表示
                '===================================
                ' 検索データ有無判定
                If Me.dgvResultTmp.RowCount > 0 Then
                    blnDisplayFlg = True
                End If
            End If

            '---------------------------------------------------
            '   検索条件
            '---------------------------------------------------
            ' GroupBox
            Me.grpSearch.Enabled = iBlnTab                      ' 検索条件
            Me.grpIssueDate.Enabled = iBlnTab                   ' 発行年月
            ' ComboBox
            Me.cboDocCode.Enabled = iBlnTab                     ' 管理コード
            Me.cboPeriod.Enabled = iBlnTab                      ' 期
            Me.cboSubjectSeq.Enabled = iBlnTab                  ' 標題分類
            ' DateTimePicker
            Me.dtpIssueDate.Enabled = blnIssueDate              ' 発行年月
            ' TextBox
            Me.txtDocNo.Enabled = iBlnTab                       ' 文書番号
            ' Option
            Me.optNotSpecify.Enabled = iBlnTab                  ' 指定しない
            Me.optSpecify.Enabled = iBlnTab                     ' 指定する
            ' Label
            Me.lblManageCode.Enabled = iBlnTab                  ' 管理コード
            Me.lblPeriod.Enabled = iBlnTab                      ' 期
            Me.lblSubjectKind.Enabled = iBlnTab                 ' 標題分類
            Me.lblDocNo.Enabled = iBlnTab                       ' 文書番号
            Me.lblHyphen.Enabled = iBlnTab                      ' ハイフン
            Me.lblGo.Enabled = iBlnTab                          ' 号
            ' Button
            Me.btnSearch.Enabled = iBlnTab                      ' 検索

            '---------------------------------------------------
            '   フッダー
            '---------------------------------------------------
            ' Button
            Me.btnDelete.Enabled = blnDisplayFlg                ' 削除（一時保存タブ）

            Me.btnShow.Enabled = blnDisplayFlg                  ' 表示
            Me.btnEdit.Enabled = blnDisplayFlg                  ' 編集
            Me.btnCopyEdit.Enabled = blnDisplayFlg              ' コピーして編集

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

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim dtGrant As DataTable = Nothing          ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC040601)
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
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス
        Dim strSql As String = ""           ' SQL

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   管理コードコンボボックス作成
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_name     AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.c_doc_code AS ValueName" & vbCrLf
            strSql = strSql & "       ,a.d_from" & vbCrLf
            strSql = strSql & "   FROM document_code_master AS a" & vbCrLf
            strSql = strSql & "  WHERE a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "    AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.c_doc_code" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, _
                                          Me.cboDocCode, _
                                          strSql, _
                                          "DisplayName", _
                                          "ValueName") = False Then
                Return False
            End If

            '-------------------------------------------------------------------------------
            '   期コンボボックス作成
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_name      AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.c_period_id AS ValueName" & vbCrLf
            strSql = strSql & "   FROM period AS a" & vbCrLf
            strSql = strSql & "  ORDER BY d_to DESC" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, _
                                          Me.cboPeriod, _
                                          strSql, _
                                          "DisplayName", _
                                          "ValueName") = False Then
                Return False
            End If
            ' ログイン期設定
            Me.cboPeriod.SelectedIndex = Me.cboPeriod.FindString(MDLoginInfo.PeriodName)

            ' 処理結果に正常を格納
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
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：ByVal iBytTab    As Byte                              = 表示タブ（0：発信済, 1：一時保存）,
    '           ByVal iDgvResult As System.Windows.Forms.DataGridView = データグリッドビュー
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：2015/03/04(水)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '         ：2015/03/04(水)  y.fujisaku  管理コードD対応、一覧に和暦側の値表示
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <param name="iBytTab">表示タブ（0：発信済, 1：一時保存）</param>
    ''' <param name="iDgvResult">データグリッドビュー</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData(ByVal iBytTab As Byte, _
                                   ByVal iDgvResult As System.Windows.Forms.DataGridView) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス
        Dim tbRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                       ' 検索結果件数

        ' 検索条件
        Dim strManageCode As String = ""                ' 管理コード
        Dim intSubjectSeq As Integer = 0                ' 標題分類（標題枝番）
        Dim strPeriod As String = ""                    ' 期
        Dim strDocNo As String = ""                     ' 文書番号
        Dim strIssueFrom As String = ""                 ' 発行年月日From
        Dim strIssueTo As String = ""                   ' 発行年月日To
        Dim blnFlg As Boolean = False                   ' ボタン表示・非表示フラグ

        Try
            ' 検索クリア
            iDgvResult.Rows.Clear()

            '-------------------------------------------------------------------
            '   検索条件取得（発信済のみ）
            '-------------------------------------------------------------------
            ' 発行済 or 一時保存判定
            If iBytTab = TAB_NORMAL Then
                ' 管理コード
                If Me.cboDocCode.SelectedIndex > 0 Then
                    strManageCode = Me.cboDocCode.SelectedValue.ToString()
                End If
                ' 標題分類
                If Me.cboSubjectSeq.SelectedIndex > 0 Then
                    intSubjectSeq = CInt(Me.cboSubjectSeq.SelectedValue)
                End If
                ' 期
                If Me.cboPeriod.SelectedIndex > 0 Then
                    strPeriod = Me.cboPeriod.SelectedValue.ToString()
                End If
                ' 文書番号
                If ChkNull(Me.txtDocNo.Text.Trim()) = False Then
                    strDocNo = Me.txtDocNo.Text.Trim()
                End If
                ' 発行年月
                If Me.optSpecify.Checked Then
                    ' 発行年月日From
                    strIssueFrom = Me.dtpIssueDate.Value.ToString("yyyyMM") & "01"
                    ' 発行年月日To
                    strIssueTo = Me.dtpIssueDate.Value.ToString("yyyyMM") & CStr(Date.DaysInMonth(CInt(Me.dtpIssueDate.Value.ToString("yyyy")), CInt(Me.dtpIssueDate.Value.ToString("MM"))))
                End If
            End If

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT dpd.c_doc_code      AS c_doc_code" & vbCrLf                ' 01. 管理コード
            ' 02. 期(表示用) (D以外：期略称、D：和暦値)
            strSql = strSql & "       ,IIF(dpd.c_doc_code='D', dpd.c_period_id_D, prd.l_omission_name) AS view_Period" & vbCrLf
            ' 03. 文書No.(表示用) (D以外：s_doc_number、D：s_doc_number_D)
            strSql = strSql & "       ,IIF(dpd.c_doc_code='D', dpd.s_doc_number_D, dpd.s_doc_number)   AS view_s_doc_number" & vbCrLf
            strSql = strSql & "       ,dpd.l_file          AS l_file" & vbCrLf                    ' 04. ファイル名
            strSql = strSql & "       ,dpd.d_issue         AS d_issue" & vbCrLf                   ' 05. 発行日
            strSql = strSql & "       ,FORMAT(dpd.d_ins, 'yyyy/MM/dd') AS create_date" & vbCrLf   ' 06. 作成日
            strSql = strSql & "       ,st1.l_name          AS create_person" & vbCrLf             ' 07. 作成者
            strSql = strSql & "       ,FORMAT(dpd.d_up, 'yyyy/MM/dd') AS update_date" & vbCrLf    ' 08. 更新日
            strSql = strSql & "       ,st2.l_name          AS update_person" & vbCrLf             ' 09. 更新者
            strSql = strSql & "       ,dpd.s_doc_id        AS s_doc_id" & vbCrLf                  ' 10. 文書フルパス
            strSql = strSql & "       ,dpd.c_doc_id        AS c_doc_id" & vbCrLf                  ' 11. 文書識別コード
            strSql = strSql & "       ,dcm.l_name          AS managemant_name" & vbCrLf           ' 12. 期コード
            strSql = strSql & "       ,dpd.c_period_id     AS c_period_id" & vbCrLf               ' 13. 期ID
            strSql = strSql & "       ,dpd.s_subject_seq   AS s_subject_seq" & vbCrLf             ' 14. 標題枝番
            strSql = strSql & "       ,dpd.l_subject       AS l_subject" & vbCrLf                 ' 15. 標題
            strSql = strSql & "       ,dpd.c_template      AS c_template" & vbCrLf                ' 16. テンプレート
            strSql = strSql & "       ,dpd.l_biko          AS l_biko" & vbCrLf                    ' 17. 備考
            strSql = strSql & "       ,dpd.c_user_id_ins   AS c_user_id_ins" & vbCrLf             ' 18. 作成者ID
            strSql = strSql & "       ,dpd.c_user_id_up    AS c_user_id_up" & vbCrLf              ' 19. 更新者ID
            strSql = strSql & "       ,prd.l_omission_name AS Period" & vbCrLf                    ' 02->20. 期
            strSql = strSql & "       ,dpd.s_doc_number    AS s_doc_number" & vbCrLf              ' 03->21. 文書№
            strSql = strSql & "   FROM (((dispatch_document AS dpd" & vbCrLf
            strSql = strSql & "        LEFT JOIN (" & vbCrLf
            strSql = strSql & "           SELECT dcm1.c_doc_code, dcm1.d_from, dcm1.l_name " & vbCrLf
            strSql = strSql & "             FROM document_code_master AS dcm1," & vbCrLf
            strSql = strSql & "                  (SELECT a.c_doc_code, MAX(a.d_from) AS max_d_from" & vbCrLf
            strSql = strSql & "                   FROM document_code_master AS a" & vbCrLf
            strSql = strSql & "                   GROUP BY a.c_doc_code) AS max_dcm" & vbCrLf
            strSql = strSql & "            WHERE dcm1.c_doc_code = max_dcm.c_doc_code " & vbCrLf
            strSql = strSql & "             AND  dcm1.d_from = max_dcm.max_d_from " & vbCrLf
            strSql = strSql & "         ) AS dcm" & vbCrLf
            strSql = strSql & "        ON dpd.c_doc_code = dcm.c_doc_code)" & vbCrLf
            strSql = strSql & "            LEFT JOIN staf_attribute_full_time_now_name_view AS st1" & vbCrLf
            strSql = strSql & "            ON dpd.c_user_id_ins = st1.user_id)" & vbCrLf
            strSql = strSql & "                LEFT JOIN staf_attribute_full_time_now_name_view AS st2" & vbCrLf
            strSql = strSql & "                ON dpd.c_user_id_up = st2.user_id)" & vbCrLf
            strSql = strSql & "                    LEFT JOIN period AS prd" & vbCrLf
            strSql = strSql & "                    ON dpd.c_period_id = prd.c_period_id" & vbCrLf

            ' 発行済 or 一時保存判定
            If iBytTab = TAB_NORMAL Then
                '===================================================
                '   発行済
                '===================================================
                strSql = strSql & "  WHERE (dpd.s_doc_number IS NOT NULL" & vbCrLf
                strSql = strSql & "        AND dpd.s_doc_number <> '')" & vbCrLf
                ' 管理コード
                If ChkNull(strManageCode) = False Then
                    strSql = strSql & "    AND dpd.c_doc_code = '" & strManageCode & "'" & vbCrLf
                End If
                ' 標題分類（標題枝番）
                If intSubjectSeq <> 0 Then
                    strSql = strSql & "    AND dpd.s_subject_seq = " & intSubjectSeq & vbCrLf
                End If
                ' 期
                If ChkNull(strPeriod) = False Then
                    strSql = strSql & "    AND dpd.c_period_id = '" & strPeriod & "'" & vbCrLf
                End If
                ' 文書番号
                If ChkNull(strDocNo) = False Then
                    strSql = strSql & "    AND dpd.s_doc_number LIKE '" & strDocNo & "%'" & vbCrLf
                End If
                ' 発行年月
                If ChkNull(strIssueFrom) = False Then
                    strSql = strSql & "    AND dpd.d_issue BETWEEN '" & strIssueFrom & "' AND '" & strIssueTo & "'" & vbCrLf
                End If
            ElseIf iBytTab = TAB_TMP Then
                '===================================================
                '   一時保存
                '===================================================
                strSql = strSql & "  WHERE (dpd.s_doc_number IS NULL" & vbCrLf
                strSql = strSql & "        OR dpd.s_doc_number = '')" & vbCrLf
            End If

            ' ORDER BY
            If iBytTab = TAB_NORMAL Then
                '===================================================
                '   発信済
                '===================================================
                ' 発信済の場合、期ID（降順）・管理コード（昇順）・文書番号（降順）
                strSql = strSql & "  ORDER BY dpd.c_period_id DESC" & vbCrLf
                strSql = strSql & "          ,dpd.c_doc_code" & vbCrLf
                strSql = strSql & "          ,dpd.s_doc_number DESC" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            ElseIf iBytTab = TAB_TMP Then
                '===================================================
                '   一時保存
                '===================================================
                ' 一時保存の場合、期ID（降順）・管理コード（昇順）
                strSql = strSql & "  ORDER BY dpd.c_period_id DESC" & vbCrLf
                strSql = strSql & "          ,dpd.c_doc_code" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            End If
            'strSql = strSql & "  ORDER BY dcm.l_name" & vbCrLf
            'strSql = strSql & "          ,dpd.d_issue DESC" & vbCrLf
            'strSql = strSql & "          ,dpd.d_ins DESC" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet > 0 Then
                '===============================================================
                '   1件以上の場合
                '===============================================================
                ' 縦総数設定
                iDgvResult.RowCount = intRet

                ' レコード数分ループ
                For i = 0 To intRet - 1
                    ' ヘッダー行番号表示
                    iDgvResult.Rows(i).HeaderCell.Value = (i + 1).ToString()

                    ' データ設定
                    With iDgvResult.Rows(i).Cells
                        ' 01. 管理コード
                        If IsDBNull(tbRet.Rows(i).Item(0)) Then
                            .Item(0).Value = ""
                        Else
                            .Item(0).Value = tbRet.Rows(i).Item(0)
                        End If
                        ' 02. 表示用期
                        If IsDBNull(tbRet.Rows(i).Item(1)) Then
                            .Item(1).Value = ""
                        Else
                            .Item(1).Value = tbRet.Rows(i).Item(1)
                        End If
                        ' 02. 表示用文書No.
                        If IsDBNull(tbRet.Rows(i).Item(2)) Then
                            .Item(2).Value = ""
                        Else
                            .Item(2).Value = tbRet.Rows(i).Item(2)
                        End If
                        ' 04. ファイル名
                        If IsDBNull(tbRet.Rows(i).Item(3)) Then
                            .Item(3).Value = ""
                        Else
                            .Item(3).Value = tbRet.Rows(i).Item(3)
                        End If
                        ' 05. 発行日
                        If IsDBNull(tbRet.Rows(i).Item(4)) Then
                            .Item(4).Value = ""
                        Else
                            .Item(4).Value = Date.Parse(Format(CInt(tbRet.Rows(i).Item(4).ToString()), "0000/00/00")).ToString("yyyy/MM/dd")
                        End If
                        ' 06. 作成日
                        If IsDBNull(tbRet.Rows(i).Item(5)) Then
                            .Item(5).Value = ""
                        Else
                            .Item(5).Value = tbRet.Rows(i).Item(5)
                        End If
                        ' 07. 作成者
                        If IsDBNull(tbRet.Rows(i).Item(6)) Then
                            .Item(6).Value = ""
                        Else
                            .Item(6).Value = tbRet.Rows(i).Item(6)
                        End If
                        ' 08. 更新日
                        If IsDBNull(tbRet.Rows(i).Item(7)) Then
                            .Item(7).Value = ""
                        Else
                            .Item(7).Value = tbRet.Rows(i).Item(7)
                        End If
                        ' 09. 更新者
                        If IsDBNull(tbRet.Rows(i).Item(8)) Then
                            .Item(8).Value = ""
                        Else
                            .Item(8).Value = tbRet.Rows(i).Item(8)
                        End If
                        ' 10. 文書フルパス
                        If IsDBNull(tbRet.Rows(i).Item(9)) Then
                            .Item(9).Value = ""
                        Else
                            .Item(9).Value = tbRet.Rows(i).Item(9)
                        End If
                        ' 11. 文書識別コード
                        If IsDBNull(tbRet.Rows(i).Item(10)) Then
                            .Item(10).Value = ""
                        Else
                            .Item(10).Value = tbRet.Rows(i).Item(10)
                        End If
                        ' 12. 期コード
                        If IsDBNull(tbRet.Rows(i).Item(11)) Then
                            .Item(11).Value = ""
                        Else
                            .Item(11).Value = tbRet.Rows(i).Item(11)
                        End If
                        ' 13. 期ID
                        If IsDBNull(tbRet.Rows(i).Item(12)) Then
                            .Item(12).Value = ""
                        Else
                            .Item(12).Value = tbRet.Rows(i).Item(12)
                        End If
                        ' 14. 標題枝番
                        If IsDBNull(tbRet.Rows(i).Item(13)) Then
                            .Item(13).Value = ""
                        Else
                            .Item(13).Value = tbRet.Rows(i).Item(13)
                        End If
                        ' 15. 標題
                        If IsDBNull(tbRet.Rows(i).Item(14)) Then
                            .Item(14).Value = ""
                        Else
                            .Item(14).Value = tbRet.Rows(i).Item(14)
                        End If
                        ' 16. テンプレート
                        If IsDBNull(tbRet.Rows(i).Item(15)) Then
                            .Item(15).Value = ""
                        Else
                            .Item(15).Value = tbRet.Rows(i).Item(15)
                        End If
                        ' 17. 備考
                        If IsDBNull(tbRet.Rows(i).Item(16)) Then
                            .Item(16).Value = ""
                        Else
                            .Item(16).Value = tbRet.Rows(i).Item(16)
                        End If
                        ' 18. 作成者ID
                        If IsDBNull(tbRet.Rows(i).Item(17)) Then
                            .Item(17).Value = ""
                        Else
                            .Item(17).Value = tbRet.Rows(i).Item(17)
                        End If
                        ' 19. 更新者ID
                        If IsDBNull(tbRet.Rows(i).Item(18)) Then
                            .Item(18).Value = ""
                        Else
                            .Item(18).Value = tbRet.Rows(i).Item(18)
                        End If
                        ' 02->20. 期
                        If IsDBNull(tbRet.Rows(i).Item(1)) Then
                            .Item(19).Value = ""
                        Else
                            .Item(19).Value = tbRet.Rows(i).Item(19)
                        End If
                        ' 03->21. 文書№
                        If IsDBNull(tbRet.Rows(i).Item(2)) Then
                            .Item(20).Value = ""
                        Else
                            .Item(20).Value = tbRet.Rows(i).Item(20)
                        End If
                    End With
                Next
            End If

            '-------------------------------------------------------------------
            '   ボタン表示・非表示
            '-------------------------------------------------------------------
            ' 0件以上の場合、フラグを True にする。
            If intRet > 0 Then
                blnFlg = True
            End If

            Me.btnCopyEdit.Enabled = blnFlg
            Me.btnEdit.Enabled = blnFlg
            Me.btnShow.Enabled = blnFlg
            Me.btnDelete.Enabled = blnFlg

            '' タブ設定
            'Me.TabDoc.SelectedIndex = iBytTab

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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateSubjectKindList
    '   名称　：標題分類コンボボックスリスト作成処理
    '   概要  ：
    '   引数　：ByVal iStrManageCode As String = 管理コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>標題分類コンボボックスリスト作成処理</summary>
    ''' <param name="iStrManageCode">管理コード</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateSubjectKindList(ByVal iStrManageCode As String) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb      ' データベースクラス
        Dim strSql As String = ""                       ' SQL

        Try
            ' 標題分類コンボボックスクリア
            Me.cboSubjectSeq.DataSource = Nothing

            ' データベース接続
            Call clsDb.Connect()

            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT IIF(a.s_subject_seq = 999, 'その他', a.l_subject) AS DisplayName"
            strSql = strSql & "       ,a.s_subject_seq AS ValueName"
            strSql = strSql & "   FROM document_subject AS a"
            strSql = strSql & "  WHERE a.c_doc_code = '" & iStrManageCode & "'"
            strSql = strSql & "    AND a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "    AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.s_subject_seq" & vbCrLf

            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, _
                                          Me.cboSubjectSeq, _
                                          strSql, _
                                          "DisplayName", _
                                          "ValueName") = False Then
                Return False
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ResultClear
    '   名称　：検索結果クリア処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索結果クリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ResultClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            '-----------------------------------------------
            '   発信済
            '-----------------------------------------------
            ' DataGridView
            With Me.dgvResult
                ' 発信文書一覧（発信済）クリア
                If .Rows.Count > 0 Then
                    .Rows.Clear()
                End If
            End With

            ' Button
            Me.btnShow.Enabled = False          ' 表示
            Me.btnEdit.Enabled = False          ' 編集
            Me.btnCopyEdit.Enabled = False      ' コピーして編集

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
    '   ＩＤ　：MainInsert
    '   名称　：新規作成メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>新規作成メイン処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function MainInsert() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   発信文書新規作成画面表示処理
            '-------------------------------------------------------------------------------
            If Me.ShowFM040602() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   文書操作ウィンドウ画面表示処理
            '-------------------------------------------------------------------------------
            If Me.ShowFM040604(STATUS_INSERT) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   再検索処理
            '-------------------------------------------------------------------------------
            ' タブ表示の為、検索処理の順番を入れ替える
            If Me.bytSaveKindFlg = TAB_NORMAL Then
                '===========================================================================
                '   発信済の登録更新処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_NORMAL

            ElseIf Me.bytSaveKindFlg = TAB_TMP Then
                '===========================================================================
                '   一時保存の登録更新削除処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_TMP
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
    '   ＩＤ　：MainShow
    '   名称　：表示メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>表示メイン処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function MainShow() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '-----------------------------------------------------------
            '   文書操作ウィンドウ画面表示処理
            '-----------------------------------------------------------
            If Me.ShowFM040604(STATUS_SHOW) = False Then
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：MainEdit
    '   名称　：編集メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function MainEdit() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   文書操作ウィンドウ画面表示処理
            '-------------------------------------------------------------------------------
            If Me.ShowFM040604(STATUS_EDIT) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   再検索処理
            '-------------------------------------------------------------------------------
            ' タブ表示の為、検索処理の順番を入れ替える
            If Me.bytSaveKindFlg = TAB_NORMAL Then
                '===========================================================================
                '   発信済の登録更新処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_NORMAL

            ElseIf Me.bytSaveKindFlg = TAB_TMP Then
                '===========================================================================
                '   一時保存の登録更新削除処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_TMP
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
    '   ＩＤ　：MainCopyEdit
    '   名称　：コピーして編集メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function MainCopyEdit() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   コピー編集期選択画面表示処理
            '-------------------------------------------------------------------------------
            If Me.ShowFM040605() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   文書操作ウィンドウ画面表示処理
            '-------------------------------------------------------------------------------
            If Me.ShowFM040604(STATUS_COPY_EDIT) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   再検索処理
            '-------------------------------------------------------------------------------
            ' タブ表示の為、検索処理の順番を入れ替える
            If Me.bytSaveKindFlg = TAB_NORMAL Then
                '===========================================================================
                '   発信済の登録更新処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_NORMAL

            ElseIf Me.bytSaveKindFlg = TAB_TMP Then
                '===========================================================================
                '   一時保存の登録更新削除処理からの帰り
                '===========================================================================
                ' 検索データ取得処理（発信済）
                If Me.GetSearchData(TAB_NORMAL, _
                                    Me.dgvResult) = False Then
                    Exit Function
                End If
                ' 検索データ取得処理（一時保存）
                If Me.GetSearchData(TAB_TMP, _
                                    Me.dgvResultTmp) = False Then
                    Exit Function
                End If
                ' タブ設定
                Me.TabDoc.SelectedIndex = TAB_TMP
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
    '   ＩＤ　：MainDelete
    '   名称　：一時保存文書削除メイン処理
    '   概要　：一時保存文書の削除を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>一時保存文書削除処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function MainDelete() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス

        Try
            '---------------------------------------------------
            '   削除処理
            '---------------------------------------------------
            ' データベース接続
            Call clsDb.Connect()

            ' トランザクション開始
            Call clsDb.BeginTran()

            ' 発信文書情報存在チェック処理
            If Me.ExistsDispatchDocument(clsDb, _
                                         Me.intDocId, _
                                         Me.strDocCode, _
                                         Me.strPeriodId) = False Then
                ' トランザクション取消
                Call clsDb.RollbackTran()
                ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                Call CLMsg.Show("GE0052")
                Return blnRet
            End If

            ' 発信文書削除処理
            If Me.DeleteDispatchDocument(clsDb, _
                                         Me.intDocId, _
                                         Me.strDocCode, _
                                         Me.strPeriodId) = False Then
                ' トランザクション取消
                Call clsDb.RollbackTran()
                ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                Call CLMsg.Show("FE0001")
                Return blnRet
            End If

            '---------------------------------------------------
            '   保存先ファイル削除
            '---------------------------------------------------
            ' ファイル削除処理
            If NSMDFile.FileDelete(Me.strDocId, _
                                   True) = False Then
                ' トランザクション取消
                Call clsDb.RollbackTran()
                Return blnRet
            End If

            ' トランザクション確定
            Call clsDb.CommitTran()

            '---------------------------------------------------
            '   再検索
            '---------------------------------------------------
            ' 検索データ取得処理（一時保存）
            If Me.GetSearchData(TAB_TMP, _
                                Me.dgvResultTmp) = False Then
                Exit Function
            End If
            ' タブ設定
            Me.TabDoc.SelectedIndex = TAB_TMP

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' トランザクション取消
            Call clsDb.RollbackTran()

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
    '   ＩＤ　：SetData
    '   名称　：各データ設定処理
    '   概要  ：
    '   引数　：ByVal iStrMsg As String = メッセージ文字列
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：2015/03/04(水)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '         ：2015/03/04(水)  y.fujisaku  管理コードD対応、子画面に値の引き継ぎ
    '***************************************************************************************************
    ''' <summary>各データ処理</summary>
    ''' <param name="iStrMsg">メッセージ文字列</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetData(ByVal iStrMsg As String) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim dgvRet As DataGridView = Nothing    ' データグリッドビュー

        Try
            '-----------------------------------------------------------
            '   対象データグリッドビュー設定
            '-----------------------------------------------------------
            ' タブ判定
            If Me.TabDoc.SelectedIndex = TAB_NORMAL Then
                ' 発信済
                dgvRet = Me.dgvResult
            ElseIf Me.TabDoc.SelectedIndex = TAB_TMP Then
                ' 一時保存
                dgvRet = Me.dgvResultTmp
            End If

            '-----------------------------------------------------------
            '   データ選択チェック
            '-----------------------------------------------------------
            If dgvRet.SelectedRows.Count = 0 Then
                Call CLMsg.Show("GE0010", "「" & iStrMsg & "」する文書")
                Exit Function
            End If

            '-----------------------------------------------------------
            '   各データ格納
            '-----------------------------------------------------------
            With dgvRet.SelectedRows(0).Cells

                Me.strDocCode = .Item(0).Value.ToString()                   ' 管理コード
                Me.intPeriod = .Item(19).Value.ToString()                    ' 期（数値）
                Me.strDocNumber = .Item(20).Value.ToString()                 ' 文書№（文書番号）
                Me.strFile = .Item(3).Value.ToString()                      ' ファイル名
                'Me.strIssueDate = .Item(4).Value.ToString()                 
                Me.strIssueDate = .Item(4).Value.ToString().Replace("/", "") ' 発行日
                Me.strDocId = .Item(9).Value.ToString()                      ' 文書フルパス（文書ID）
                Me.intDocId = .Item(10).Value.ToString()                     ' 文書識別コード
                Me.strPeriodName = .Item(11).Value.ToString()                ' 期コード
                Me.strPeriodId = .Item(12).Value.ToString()                  ' 期ID
                Me.intSubjectSeq = .Item(13).Value.ToString()                ' 標題枝番
                Me.strSubject = .Item(14).Value.ToString()                   ' 標題
                Me.strTemplate = .Item(15).Value.ToString()                  ' テンプレート区分
                ' 管理コード"D"の時
                If Me.strDocCode = "D" Then
                    Me.strPeriodIdD = .Item(1).Value.ToString()              ' 期（Ｄ文書用）
                    Me.strDocNumberD = .Item(2).Value.ToString()             ' 文書№（Ｄ文書用）
                End If

                ' 新規作成のみなのでクリア
                Me.strTemplateFile = ""                                      ' テンプレートファイル名（ファイル名のみ）
                Me.strTemplateFileFull = ""                                  ' テンプレートファイル名（フルパス）

            End With

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
    '   ＩＤ　：ShowFM040605
    '   名称　：コピー編集期選択画面表示処理
    '   概要  ：
    '   引数　：ByVal iClsFM040604 As FM040604 = 発信文書新規作成画面クラス,
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：2013/11/09(土)　Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    ' 　　　　：2013/11/09(土)　Fujisaku　期名称も取得更新するよう追加
    '***************************************************************************************************
    ''' <summary>コピー編集期選択画面表示処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ShowFM040605() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsFM040605 As FM040605 = Nothing   ' コピー編集期選択画面クラス
        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス
        Dim strSql As String = ""               ' SQL
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル

        Try
            '-----------------------------------------------------------
            '   コピー編集期選択画面
            '-----------------------------------------------------------
            ' コピー編集期選択画面クラス生成
            clsFM040605 = New FM040605

            ' プロパティ設定
            clsFM040605.strDocCode = Me.strDocCode          ' 管理コード
            clsFM040605.strSubject = Me.strSubject          ' 標題

            ' コピー編集期選択画面表示
            diaRet = clsFM040605.ShowDialog()

            '-----------------------------------------------------------
            '   押下ボタン判定
            '-----------------------------------------------------------
            If diaRet = DialogResult.Cancel Then

                ' プロパティクリア処理
                If Me.ClearProperty() = False Then
                    Return blnRet
                End If

                ' 処理を抜ける
                Return blnRet

            End If

            '-----------------------------------------------------------
            '   各情報取得
            '-----------------------------------------------------------
            Me.strPeriodId = clsFM040605.strPeriodId        ' 期ID
            Me.strDocCode = clsFM040605.strDocCode          ' 管理コード
            Me.strSubject = clsFM040605.strSubject          ' 標題

            ' 適用日付
            If MDLoginInfo.PeriodNewFlg = FLG_NEW_PERIOD Then
                ' 最新期
                Me.strApplyDate = Now.Date.ToString("yyyy/MM/dd")
            ElseIf MDLoginInfo.PeriodNewFlg = FLG_OLD_PERIOD Then
                ' 最新期以外
                Me.strApplyDate = MDLoginInfo.PeriodTo
            End If

            ' 期名称取得
            Call clsDb.Connect()

            strSql = "" & vbCrLf
            strSql = strSql & " SELECT l_omission_name" & vbCrLf
            strSql = strSql & "   FROM period" & vbCrLf
            strSql = strSql & "  WHERE c_period_id = '" & Me.strPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            dtRet = clsDb.ExecuteSql(strSql)

            Me.intPeriod = Integer.Parse(dtRet.Rows(0).Item(0))

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
            Call clsDb.Disconnect()
            ' コピー編集期選択画面クラス開放
            If Not clsFM040605 Is Nothing Then
                clsFM040605.Close()
                clsFM040605.Dispose()
            End If
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ShowFM040602
    '   名称　：発信文書新規作成画面表示処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function ShowFM040602() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsFM040602 As FM040602 = Nothing   ' 発信文書新規作成画面クラス
        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果

        Try
            '---------------------------------------------------------------
            '   発信文書新規作成画面
            '---------------------------------------------------------------
            ' 発信文書新規作成画面クラス生成
            clsFM040602 = New FM040602

            ' プロパティ設定
            ' なし

            ' 発信文書新規作成画面表示
            diaRet = clsFM040602.ShowDialog()

            '-----------------------------------------------------------
            '   押下ボタン判定
            '-----------------------------------------------------------
            If diaRet = DialogResult.Cancel Then

                ' プロパティクリア処理
                If Me.ClearProperty() = False Then
                    Return blnRet
                End If

                ' 処理を抜ける
                Return blnRet

            End If

            '-----------------------------------------------------------
            '   各情報取得
            '-----------------------------------------------------------
            Me.strTemplate = clsFM040602.strTemplate                    ' テンプレート区分
            Me.strTemplateFile = clsFM040602.strTemplateFile            ' テンプレートファイル名（ファイル名のみ）
            Me.strTemplateFileFull = clsFM040602.strTemplateFileFull    ' テンプレートファイルフルパス

            Me.intSubjectSeq = clsFM040602.intSubjectSeq                ' 標題枝番
            Me.strPeriodId = clsFM040602.strPeriodId                    ' 期ID
            Me.intPeriod = clsFM040602.intPeriod                        ' 期（数値）
            Me.strPeriodName = clsFM040602.strPeriodName                ' 期（全角期（第ＸＸ期））
            Me.strDocCode = clsFM040602.strDocCode                      ' 管理コード
            Me.strSubject = clsFM040602.strSubject                      ' 標題（もしくは任意入力欄）
            Me.strApplyDate = clsFM040602.strApplyDate                  ' 適用日付（要件改定で不要っぽい）

            Me.strCommitteeUpdate() = clsFM040602.strCommitteeUpdate()  ' 変更ID
            Me.strDocumentOut() = clsFM040602.strDocumentOut()          ' 帳票出力

            Me.strCommitteeDFrom() = clsFM040602.strCommitteeDFrom()    ' 委員会適用日付

            Me.strDetails = clsFM040602.strDetails                      ' 詳細設定分類

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
            ' 発信文書新規作成画面クラス開放
            If Not clsFM040602 Is Nothing Then
                clsFM040602.Close()
                clsFM040602.Dispose()
            End If
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ShowFM040604
    '   名称　：文書操作ウィンドウ画面表示処理
    '   概要  ：
    '   引数　：ByVal iStrStatus As String = ステータス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：2015/03/04(水)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '         ：2015/03/04(水)  y.fujisaku  管理コードD対応、子画面に値の引き継ぎ
    '***************************************************************************************************
    ''' <summary>文書操作ウィンドウ画面表示処理</summary>
    ''' <param name="iStrStatus">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ShowFM040604(ByVal iStrStatus As String) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsFM040604 As FM040604 = Nothing   ' 文書操作ウィンドウ画面クラス
        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果

        Try
            '-------------------------------------------------------
            '   文書操作ウィンドウ画面
            '-------------------------------------------------------
            ' 文書操作ウィンドウ画面クラス生成
            clsFM040604 = New FM040604

            With clsFM040604
                ' プロパティ設定
                .strStatus = iStrStatus                             ' ステータス

                .strDocCode = Me.strDocCode                         ' 管理コード
                .intPeriod = Me.intPeriod                           ' 期（数値）
                .strDocNumber = Me.strDocNumber                     ' 文書№（文書番号）
                .strFile = Me.strFile                               ' ファイル名
                .strPeriodIdD = Me.strPeriodIdD                     ' 期（Ｄ文書用）
                .strDocNumberD = Me.strDocNumberD                   ' 文書№（Ｄ文書用）
                .strIssueDate = Me.strIssueDate                     ' 発行日
                .strDocId = Me.strDocId                             ' 文書ID（保存先ファイル名フルパス）
                .intDocId = Me.intDocId                             ' 文書識別コード
                .strPeriodId = Me.strPeriodId                       ' 期ID
                .strPeriodName = Me.strPeriodName                   ' 期（全角期（第ＸＸ期））
                .intPeriod = Me.intPeriod                           ' 期（数値）
                .intSubjectSeq = Me.intSubjectSeq                   ' 標題枝番
                .strSubject = Me.strSubject                         ' 標題
                .strTemplate = Me.strTemplate                       ' テンプレート区分
                .strApplyDate = Me.strApplyDate                     ' 適用日付（要件改訂で不要っぽい）

                .strDetails = Me.strDetails                         ' 詳細設定分類

                .strCommitteeUpdate() = Me.strCommitteeUpdate()     ' 委員会変更ID
                .strDocumentOut() = Me.strDocumentOut()             ' 帳票出力
                .strCommitteeDFrom() = Me.strCommitteeDFrom()       ' 委員会適用日付

                ' 新規作成のみ
                .strTemplateFile = Me.strTemplateFile               ' テンプレートファイル名（ファイル名のみ）
                .strTemplateFileFull = Me.strTemplateFileFull       ' テンプレートファイル名（フルパス）

                ' 初期設定
                .StartPosition = FormStartPosition.Manual           ' 開始位置
                .Location = New Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - clsFM040604.Size.Width, 0)
                .TopMost = True                                     ' 常に最前面に表示

            End With

            ' 文書操作ウィンドウ画面表示
            Me.TopLevelControl.Visible = False
            diaRet = clsFM040604.ShowDialog()

            '-----------------------------------------------------------
            '   押下ボタン判定
            '-----------------------------------------------------------
            If diaRet = DialogResult.Cancel Then
                '===================================
                '   総合OAに戻るボタン押下
                '===================================
                ' プロパティクリア処理
                If Me.ClearProperty() = False Then
                    Return blnRet
                End If

                ' 処理を抜ける
                Return blnRet

            ElseIf diaRet = DialogResult.OK Then
                '===================================
                '   DB登録ボタン押下
                '===================================
                Me.bytSaveKindFlg = clsFM040604.bytSaveKindFlg

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
            ' 文書操作ウィンドウ画面クラス開放
            If Not clsFM040604 Is Nothing Then
                clsFM040604.Close()
                clsFM040604.Dispose()
            End If
            ' 元画面表示
            Me.TopLevelControl.Visible = True
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：ByVal iBytTab As Byte                       = 表示タブ（0：発信済, 1：一時保存）,
    '           ByVal iDgv    As Windows.Forms.DataGridView = データグリッドビュー
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：2015/03/04(水)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '         ：2015/03/04(水)  y.fujisaku  管理コードD対応、一覧に和暦側の値表示
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <param name="iBytTab"></param>
    ''' <param name="iDgv">データグリッドビュー</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni(ByVal iBytTab As Byte, _
                                     ByVal iDgv As Windows.Forms.DataGridView) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            With iDgv
                '-----------------------------------------------------------------------------------
                '   グリッド全体設定
                '-----------------------------------------------------------------------------------
                ' 総数
                .RowCount = 0                                                                       ' 縦
                .ColumnCount = 21                                                                   ' 横
                ' 固定行
                .ColumnHeadersVisible = True                                                        ' 固定列有り
                .RowHeadersVisible = True                                                           ' 固定行有り
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                       ' 縦横両方
                ' 1行選択モード
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect                            ' 1行選択
                .MultiSelect = False                                                                ' 複数選択なし
                ' サイズ変更
                .AllowUserToResizeColumns = True                                                    ' 列サイズ変更可
                .AllowUserToResizeRows = False                                                      ' 行サイズ変更不可
                .DefaultCellStyle.ForeColor = Color.Black                                           ' ヘッダー列黒
                ' 並び替え禁止
                For intCnt = 0 To .Columns.Count - 1
                    .Columns(intCnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next

                '-----------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-----------------------------------------------------------------------------------
                ' ヘッダー文字列
                .TopLeftHeaderCell.Value = "No."                                                    ' トップレフトセル内容
                .Columns(0).HeaderText = "管理CD"                                                   ' 01. 管理CD
                .Columns(1).HeaderText = "期"                                                       ' 02. 期(表示用)
                .Columns(2).HeaderText = "文書№"                                                   ' 03. 文書№(表示用)
                .Columns(3).HeaderText = "ファイル名"                                               ' 04. ファイル名
                .Columns(4).HeaderText = "発行日"                                                   ' 05. 発行日
                .Columns(5).HeaderText = "作成日"                                                   ' 06. 作成日
                .Columns(6).HeaderText = "作成者"                                                   ' 07. 作成者
                .Columns(7).HeaderText = "更新日"                                                   ' 08. 更新日
                .Columns(8).HeaderText = "更新者"                                                   ' 09. 更新者
                .Columns(9).HeaderText = "文書フルパス"                                             ' 10. 文書フルパス
                .Columns(10).HeaderText = "文書識別コード"                                          ' 11. 文書識別コード
                .Columns(11).HeaderText = "期コード"                                                ' 12. 期コード
                .Columns(12).HeaderText = "期ID"                                                    ' 13. 期ID
                .Columns(13).HeaderText = "標題枝番"                                                ' 14. 標題枝番
                .Columns(14).HeaderText = "標題"                                                    ' 15. 標題
                .Columns(15).HeaderText = "テンプレート"                                            ' 16. テンプレート
                .Columns(16).HeaderText = "備考"                                                    ' 17. 備考
                .Columns(17).HeaderText = "作成者ID"                                                ' 18. 作成者ID
                .Columns(18).HeaderText = "更新者ID"                                                ' 19. 更新者ID
                .Columns(19).HeaderText = "期（数値）"                                              ' 02->20. 期
                .Columns(20).HeaderText = "s_doc_number"                                            ' 03->21. 文書№
                ' ヘッダー文字位置
                .TopLeftHeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' トップレフトセル中央寄せ
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 01. 管理CD
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 02. 期(表示用)
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 03. 文書№(表示用)
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 04. ファイル名
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 05. 発行日
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 06. 作成日
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 07. 作成者
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 08. 更新日
                .Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 09. 更新者
                .Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 10. 文書フルパス
                .Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 11. 文書識別コード
                .Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 12. 期コード
                .Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 13. 期ID
                .Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 14. 標題枝番
                .Columns(14).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 15. 標題
                .Columns(15).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 16. テンプレート
                .Columns(16).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 17. 備考
                .Columns(17).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 18. 作成者ID
                .Columns(18).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 19. 更新者ID
                .Columns(19).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 02->20. 期
                .Columns(20).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 03->21. 文書№

                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight    ' ヘッダー列中央寄席
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 01. 管理CD
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 02. 期(表示用)
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 03. 文書№(表示用)
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 04. ファイル名
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 05. 発行日
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 06. 作成日
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 07. 作成者
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 08. 更新日
                .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 09. 更新者
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 10. 文書フルパス
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 11. 文書識別コード
                .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 12. 期コード
                .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 13. 期ID
                .Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 14. 標題枝番
                .Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 15. 標題
                .Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 16. テンプレート
                .Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 17. 備考
                .Columns(17).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 18. 作成者ID
                .Columns(18).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 19. 更新者ID
                .Columns(19).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 02->. 期
                .Columns(20).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 03->. 文書№
                ' カラム幅
                .RowHeadersWidth = 55                                                               ' ヘッダー列幅
                .Columns(0).Width = 70                                                              ' 01. 管理CD
                .Columns(1).Width = 40                                                              ' 02. 期(表示用)
                .Columns(2).Width = 70                                                              ' 03. 文書№(表示用)
                .Columns(3).Width = 310                                                             ' 04. ファイル名
                .Columns(4).Width = 100                                                             ' 05. 発行日
                .Columns(5).Width = 100                                                             ' 06. 作成日
                .Columns(6).Width = 120                                                             ' 07. 作成者
                .Columns(7).Width = 100                                                             ' 08. 更新日
                .Columns(8).Width = 120                                                             ' 09. 更新者
                .Columns(9).Width = 100                                                             ' 10. 文書フルパス
                .Columns(10).Width = 100                                                            ' 11. 文書識別コード
                .Columns(11).Width = 100                                                            ' 12. 期コード
                .Columns(12).Width = 100                                                            ' 13. 期ID
                .Columns(13).Width = 100                                                            ' 14. 標題枝番
                .Columns(14).Width = 100                                                            ' 15. 標題
                .Columns(15).Width = 100                                                            ' 16. テンプレート
                .Columns(16).Width = 100                                                            ' 17. 備考
                .Columns(17).Width = 100                                                            ' 18. 作成者ID
                .Columns(18).Width = 100                                                            ' 19. 更新者ID
                .Columns(19).Width = 40                                                             ' 02->20. 期
                .Columns(20).Width = 70                                                             ' 03->21. 文書№

                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 01. 管理CD
                .Columns(1).Visible = True                                                          ' 02. 期(表示用)

                ' 03. 文書№(表示用)
                If iBytTab = TAB_NORMAL Then
                    ' 発信済の場合、表示
                    .Columns(2).Visible = True
                ElseIf iBytTab = TAB_TMP Then
                    ' 一時保存の場合、非表示
                    .Columns(2).Visible = False
                End If

                .Columns(3).Visible = True                                                          ' 04. ファイル名
                .Columns(4).Visible = True                                                          ' 05. 発行日
                .Columns(5).Visible = True                                                          ' 06. 作成日
                .Columns(6).Visible = True                                                          ' 07. 作成者
                .Columns(7).Visible = True                                                          ' 08. 更新日
                .Columns(8).Visible = True                                                          ' 09. 更新者
                .Columns(9).Visible = False                                                         ' 10. 文書フルパス
                .Columns(10).Visible = False                                                        ' 11. 文書識別コード
                .Columns(11).Visible = False                                                        ' 12. 期コード
                .Columns(12).Visible = False                                                        ' 13. 期ID
                .Columns(13).Visible = False                                                        ' 14. 標題枝番
                .Columns(14).Visible = False                                                        ' 15. 標題
                .Columns(15).Visible = False                                                        ' 16. テンプレート
                .Columns(16).Visible = False                                                        ' 17. 備考
                .Columns(17).Visible = False                                                        ' 18. 作成者ID
                .Columns(18).Visible = False                                                        ' 19. 更新者ID
                .Columns(19).Visible = False                                                        ' 02->20. 期
                .Columns(20).Visible = False                                                        ' 03->21. 文書№

            End With

            ' 戻り値格納
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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsDispatchDocument
    '   名称　：発信文書情報存在チェック処理
    '   概要　：発信文書情報が存在しているかチェックを行う。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iIntDocId    As Integer     = 文書識別コード,
    '           ByVal iStrDocCode  As String      = 管理コード,
    '           ByVal iStrPeriodId As String      = 期ID
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書情報存在チェック処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iIntDocId">文書識別コード</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsDispatchDocument(ByVal iClsDb As CLAccessMdb, _
                                            ByVal iIntDocId As Integer, _
                                            ByVal iStrDocCode As String, _
                                            ByVal iStrPeriodId As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 処理結果件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_doc_id" & vbCrLf
            strSql = strSql & "       ,a.c_doc_code" & vbCrLf
            strSql = strSql & "       ,a.c_period_id" & vbCrLf
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_doc_id    =  " & iIntDocId & vbCrLf
            strSql = strSql & "    AND a.c_doc_code  = '" & iStrDocCode & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intRet = dtRet.Rows.Count

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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteDispatchDocument
    '   名称　：発信文書削除処理
    '   概要　：発信文書の削除を行う。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iIntDocId    As Integer     = 文書識別コード,
    '           ByVal iStrDocCode  As String      = 管理コード,
    '           ByVal iStrPeriodId As String      = 期ID
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書削除処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iIntDocId">文書識別コード</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteDispatchDocument(ByVal iClsDb As CLAccessMdb, _
                                            ByVal iIntDocId As Integer, _
                                            ByVal iStrDocCode As String, _
                                            ByVal iStrPeriodId As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False             ' 処理結果
        Dim strSql As String = ""                 ' SQL文
        Dim intRet As Integer = 0                 ' 処理件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " DELETE " & vbCrLf
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_doc_id    =  " & iIntDocId & vbCrLf
            strSql = strSql & "    AND a.c_doc_code  = '" & iStrDocCode & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ClearProperty
    '   名称　：プロパティクリア処理
    '   概要　：プロパティの値をクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>プロパティクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ClearProperty() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            Me.intDocId = 0                     ' 01. 文書識別コード
            Me.strDocCode = ""                  ' 02. 管理コード
            Me.strPeriodId = ""                 ' 03. 期ID
            Me.strDocNumber = ""                ' 04. 文書番号
            Me.intSubjectSeq = 0                ' 05. 標題枝番
            Me.strTemplate = ""                 ' 06. テンプレート区分
            Me.strDocId = ""                    ' 07. 文書ID
            Me.strFile = ""                     ' 08. ファイル名
            Me.strIssueDate = ""                ' 09. 発行日
            Me.strSubject = ""                  ' 10. 標題
            Me.strApplyDate = ""                ' 14. 適用日付（要件改訂で不要っぽい）
            Me.strDetails = ""                  ' 15. 詳細設定分類
            Me.bytSaveKindFlg = 0               ' 16. 保存した文書種別（0：発信済, 1：一時保存）
            Me.strPeriodName = ""               ' 17. 期名前
            Me.strCommitteeUpdate() = Nothing   ' 18. 選択された委員会変更ID
            Me.strTemplateFile = ""             ' 19. テンプレートファイル

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
