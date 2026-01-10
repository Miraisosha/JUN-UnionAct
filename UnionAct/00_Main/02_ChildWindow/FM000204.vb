#Region "FM000204"
'===========================================================================================================
'   クラスＩＤ　　：FM000204
'   クラス名称　　：配布者選択画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDChk
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo

Public Class FM000204

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000204                                ' FM000204
    Private Const SCREEN_NAME = SCREEN_NAME_FM000204                            ' 配布者選択画面
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    '列名－これを増減する場合はConstで定義（STR_SELECT_BASE）しているSELECT文も変更すること
    Private ReadOnly STR_COLUMNNAME_USERID = "個人認証ID"
    Private ReadOnly STR_COLUMNNAME_STAFFID = "社員番号"
    Private ReadOnly STR_COLUMNNAME_NAME = "名前"
    Private ReadOnly STR_COLUMNNAME_NAME_KNA = "名前カナ"
    Private ReadOnly STR_COLUMNNAME_KIND = "組合員種別"
    Private ReadOnly STR_COLUMNNAME_STATUS = "ステータス"
    Private ReadOnly STR_COLUMNNAME_BELONGING = "組合支部"
    Private ReadOnly STR_COLUMNNAME_QUOLIFICATION = "資格"
    Private ReadOnly STR_COLUMNNAME_MODEL = "機種"
    Private ReadOnly STR_COLUMNNAME_LOCAL = "会社所属"
    Private ReadOnly STR_COLUMNNAME_WORK_PLACE = "職場"
    Private ReadOnly STR_COLUMNNAME_KSH = "所属会社"
    Private ReadOnly STR_COLUMNNAME_CAPTAIN = "機長年月日"
    Private ReadOnly STR_COLUMNNAME_JOIN = "加入年月日"
    Private ReadOnly STR_COLUMNNAME_BIRTH = "生年月日"
    Private ReadOnly STR_COLUMNNAME_ENTER = "入社年月日"
    Private ReadOnly STR_COLUMNNAME_RETIRE = "退職年月日"
    Private ReadOnly STR_COLUMNNAME_WORK_STATE = "勤務状態"
    Private ReadOnly STR_COLUMNNAME_SEX = "性別"
    Private ReadOnly STR_COLUMNNAME_FROM = "適用日付"
    Private ReadOnly STR_COLUMNNAME_DEZIT = "ディジット"
    Private ReadOnly STR_COLUMNNAME_INTERNATIONAL = "国際線"
    Private ReadOnly STR_COLUMNNAME_DROPOUT = "脱退年月日"
    Private ReadOnly STR_COLUMNNAME_TEACHER = "教官機長年月日"
    Private ReadOnly STR_COLUMNNAME_LOS_POSITION = "地位喪失年月日"
    Private ReadOnly STR_COLUMNNAME_LOS_REASON = "地位喪失理由区分"
    Private ReadOnly STR_COLUMNNAME_DIE = "死亡年月日"
    Private ReadOnly STR_COLUMNNAME_STAFFID_OLD = "旧社員番号"
    Private ReadOnly STR_COLUMNNAME_DEZIT_OLD = "旧ディジット"
    Private ReadOnly STR_COLUMNNAME_RETIRE_REASON = "脱退理由"
    Private ReadOnly STR_COLUMNNAME_BIKO = "備考１"
    Private ReadOnly STR_COLUMNNAME_DEL = "削除区分"
    Private ReadOnly STR_COLUMNNAME_MAKE_DATE = "作成日"
    Private ReadOnly STR_COLUMNNAME_MAKE_USER = "作成者個人ID"
    Private ReadOnly STR_COLUMNNAME_UP_DATE = "更新日"
    Private ReadOnly STR_COLUMNNAME_UP_USER = "更新者個人ID"
    Private ReadOnly STR_COLUMNNAME_UP_CNT = "更新回数"
    Private ReadOnly STR_COLUMNNAME_LOCAL_SHORT = "会社所属簡略"
    Private ReadOnly STR_COLUMNNAME_MODEL_SHORT = "機種簡略"
    '列を増やすときはデザイナ側も列増やすこと。またこの配列の名前順でカラム名を設定するので、SQLの取得順とこの配列の順序は同じにすること。
    Private ReadOnly ARR_CULUMNSNAME_SELECTLIST As String() = {STR_COLUMNNAME_NAME, _
                                                               STR_COLUMNNAME_STAFFID, _
                                                               STR_COLUMNNAME_MODEL, _
                                                               STR_COLUMNNAME_QUOLIFICATION, _
                                                               STR_COLUMNNAME_BELONGING, _
                                                               STR_COLUMNNAME_LOCAL, _
                                                               STR_COLUMNNAME_USERID, _
                                                               STR_COLUMNNAME_NAME_KNA, _
                                                               STR_COLUMNNAME_KIND, _
                                                               STR_COLUMNNAME_STATUS, _
                                                               STR_COLUMNNAME_WORK_PLACE, _
                                                               STR_COLUMNNAME_KSH, _
                                                               STR_COLUMNNAME_CAPTAIN, _
                                                               STR_COLUMNNAME_JOIN, _
                                                               STR_COLUMNNAME_BIRTH, _
                                                               STR_COLUMNNAME_ENTER, _
                                                               STR_COLUMNNAME_RETIRE, _
                                                               STR_COLUMNNAME_WORK_STATE, _
                                                               STR_COLUMNNAME_SEX, _
                                                               STR_COLUMNNAME_FROM, _
                                                               STR_COLUMNNAME_DEZIT, _
                                                               STR_COLUMNNAME_INTERNATIONAL, _
                                                               STR_COLUMNNAME_DROPOUT, _
                                                               STR_COLUMNNAME_TEACHER, _
                                                               STR_COLUMNNAME_LOS_POSITION, _
                                                               STR_COLUMNNAME_LOS_REASON, _
                                                               STR_COLUMNNAME_DIE, _
                                                               STR_COLUMNNAME_STAFFID_OLD, _
                                                               STR_COLUMNNAME_DEZIT_OLD, _
                                                               STR_COLUMNNAME_RETIRE_REASON, _
                                                               STR_COLUMNNAME_BIKO, _
                                                               STR_COLUMNNAME_DEL, _
                                                               STR_COLUMNNAME_MAKE_DATE, _
                                                               STR_COLUMNNAME_MAKE_USER, _
                                                               STR_COLUMNNAME_UP_DATE, _
                                                               STR_COLUMNNAME_UP_USER, _
                                                               STR_COLUMNNAME_UP_CNT, _
                                                               STR_COLUMNNAME_MODEL_SHORT, _
                                                               STR_COLUMNNAME_LOCAL_SHORT}
    Private ReadOnly ARR_CULUMNSHEADER_SELECTLIST As String() = ARR_CULUMNSNAME_SELECTLIST 'ヘッダー表示テキスト（現状、列名と同じ）
    Private ReadOnly ARR_CULUMNSWIDTH_SELECTLIST As Integer() = {150, _
                                                                 100, _
                                                                 100, _
                                                                 100, _
                                                                 100, _
                                                                 100, _
                                                                 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100, _
                                                                 100, 100} 'データグリッドの列幅
    '画面、右ブロックのデータグリッドビュー列表示設定
    Private ReadOnly ARR_CULUMNSSHOW_SELECTLIST As Boolean() = {True, _
                                                               True, _
                                                               True, _
                                                               True, _
                                                               True, _
                                                               True, _
                                                               False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False} 'データグリッドの表示・非表示
    '画面左のデータグリッドビュー
    Private ReadOnly ARR_CULUMNSSHOW_UNIONSELECTLIST As Boolean() = {True, _
                                                               True, _
                                                               True, _
                                                               True, _
                                                               True, _
                                                               False, _
                                                               False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False, _
                                                               False, False}
    '呼び出し画面でどの情報が必要か不明なのでとりあえずすべてのカラムを取得はしておく
    Public STR_SELECT_BASE As String = "SELECT " +
                                     "staf_attr.l_name AS " + STR_COLUMNNAME_NAME + ", " +
                                     "CLng(staf_attr.c_staf_id) AS " + STR_COLUMNNAME_STAFFID + ", " +
                                     "staf_attr.l_name3 AS " + STR_COLUMNNAME_MODEL + ", " +
                                     "staf_attr.l_name2 AS " + STR_COLUMNNAME_QUOLIFICATION + ", " +
                                     "staf_attr.l_name1 AS " + STR_COLUMNNAME_BELONGING + ", " +
                                     "staf_attr.l_name4 AS " + STR_COLUMNNAME_LOCAL + ", " +
                                     "staf_attr.c_user_id AS " + STR_COLUMNNAME_USERID + ", " +
                                     "staf_attr.l_name_kna AS " + STR_COLUMNNAME_NAME_KNA + ", " +
                                     "staf_attr.l_name5 AS " + STR_COLUMNNAME_KIND + ", " +
                                     "staf_attr.l_name6 AS " + STR_COLUMNNAME_STATUS + ", " +
                                     "staf_attr.l_name7 AS " + STR_COLUMNNAME_WORK_PLACE + ", " +
                                     "(SELECT ksh.n_ksh FROM  ksh WHERE staf_attr.c_ksh = ksh.c_ksh) AS " + STR_COLUMNNAME_KSH + ", " +
                                     "staf_attr.d_captain AS " + STR_COLUMNNAME_CAPTAIN + ", " +
                                     "staf_attr.d_join AS " + STR_COLUMNNAME_JOIN + ", " +
                                     "staf_attr.d_birth AS " + STR_COLUMNNAME_BIRTH + ", " +
                                     "staf_attr.d_enter AS " + STR_COLUMNNAME_ENTER + ", " +
                                     "staf_attr.d_retire AS " + STR_COLUMNNAME_RETIRE + ", " +
                                     "(SELECT constant_dtl.l_name FROM constant_dtl WHERE constant_dtl.c_constant = 'WORK_STATE' AND constant_dtl.c_constant_seq = staf_attr.k_work_state) AS " + STR_COLUMNNAME_WORK_STATE + ", " +
                                     "staf_attr.l_name8 AS " + STR_COLUMNNAME_SEX + ", " +
                                     "staf_attr.d_from AS " + STR_COLUMNNAME_FROM + ", " +
                                     "staf_attr.c_dezit AS " + STR_COLUMNNAME_DEZIT + ", " +
                                     "staf_attr.k_international AS " + STR_COLUMNNAME_INTERNATIONAL + ", " +
                                     "staf_attr.d_drop_out AS " + STR_COLUMNNAME_DROPOUT + ", " +
                                     "staf_attr.d_teacher_captain AS " + STR_COLUMNNAME_TEACHER + ", " +
                                     "staf_attr.d_los_position AS " + STR_COLUMNNAME_LOS_POSITION + ", " +
                                     "staf_attr.l_los_position AS " + STR_COLUMNNAME_LOS_REASON + ", " +
                                     "staf_attr.d_die AS " + STR_COLUMNNAME_DIE + ", " +
                                     "staf_attr.c_staf_id_old AS " + STR_COLUMNNAME_STAFFID_OLD + ", " +
                                     "staf_attr.c_dezit_old AS " + STR_COLUMNNAME_DEZIT_OLD + ", " +
                                     "staf_attr.l_reason AS " + STR_COLUMNNAME_RETIRE_REASON + ", " +
                                     "staf_attr.l_biko_1 AS " + STR_COLUMNNAME_BIKO + ", " +
                                     "staf_attr.k_del AS " + STR_COLUMNNAME_DEL + ", " +
                                     "staf_attr.d_ins AS " + STR_COLUMNNAME_MAKE_DATE + ", " +
                                     "staf_attr.c_user_id_ins AS " + STR_COLUMNNAME_MAKE_USER + ", " +
                                     "staf_attr.d_up AS " + STR_COLUMNNAME_UP_DATE + ", " +
                                     "staf_attr.c_user_id_up AS " + STR_COLUMNNAME_UP_USER + ", " +
                                     "staf_attr.s_up AS " + STR_COLUMNNAME_UP_CNT + ", " +
                                     "staf_attr.l_name_3_2 AS " + STR_COLUMNNAME_MODEL_SHORT + ", " +
                                     "staf_attr.l_name_4_2 AS " + STR_COLUMNNAME_LOCAL_SHORT + " " +
                                     "FROM "
    'あ行～わ行の結果設定時に使用
    Private ReadOnly ARR_DATAGRIDVIEW As DataGridView() = {New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView, New DataGridView}
    Private ReadOnly ARR_STR_SQLHAVING As String() = {"AND staf_attr.l_name_kna LIKE '[ｱ-ｵ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ｶ-ｺﾞ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ｻ-ｿﾞ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾀ-ﾄﾞ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾅ-ﾉ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾊ-ﾎﾞ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾏ-ﾓ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾔ-ﾖ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾗ-ﾛ]%'",
                                             "AND staf_attr.l_name_kna LIKE '[ﾜ-ﾝ]%'",
                                             "AND NOT staf_attr.l_name_kna LIKE '[ｱ-ﾝ]%'"}
    'データグリッドの設定
    Private Const DGD_HEADER_CENTER As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleCenter
    Private ReadOnly DGD_FORECOLOR_BLACK As System.Drawing.Color = System.Drawing.Color.Black
    Private ReadOnly DGD_ROWHEADER_NONVISIBLE As Boolean = False
    Private ReadOnly DGD_ADDROW_NON As Boolean = False
    Private ReadOnly DGD_DELETEROW_NON As Boolean = False
    Private ReadOnly DGD_SELECTIONMODE_FULLRAW As DataGridViewSelectionMode = DataGridViewSelectionMode.FullRowSelect
#End Region

#Region "プロパティ"
    '外部公開不要
    Private showDataGridView As DataGridView = Me.dgdALine '（右グリッドから左グリッドへデータ移行時に使用）
    Private selectWhereStandardDay As String = ""
    Private standardDay As String = ""
    Private isNewPeriod = False

    Private _StafIDList As String() = Nothing   '別画面から社員番号（選択組合員リスト（左側グリッド）に初期表示する組合員の社員番号）
    Private _StrSqlSentence As String = ""      '別画面からSQL文（選択組合員リスト（左側グリッド）に初期表示する組合員の抽出SQL。ただし右グリッドとカラムの順番など同じにする必要があるので基本社員番号を渡すようにする）

    Private _IntQlickBtnFlag As Integer = -1    'クリックボタン判別用(OKボタン= 0 、キャンセルボタン = 1 、画面未表示(初期値) = -1)
    Private _SelectMemberList As DataTable = New DataTable  '呼び出し元への結果
    Private _AllowDeleteMember As Boolean = True '初期表示メンバーを削除可能かどうか
    Private NotDeleteStafIDList As DataGridViewRow() = Nothing

    Public Property StrSqlSentence() As String
        Get
            Return _StrSqlSentence
        End Get
        Set(ByVal value As String)
            _StrSqlSentence = value
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

    Public Property SelectMemberList() As DataTable
        Get
            Return _SelectMemberList
        End Get
        Set(ByVal value As DataTable)
            _SelectMemberList = value
        End Set
    End Property

    Public Property StafIDList() As String()
        Get
            Return _StafIDList
        End Get
        Set(ByVal value As String())
            _StafIDList = value
        End Set
    End Property

    Public Property AllowDeleteMember() As Boolean
        Get
            Return _AllowDeleteMember
        End Get
        Set(ByVal value As Boolean)
            _AllowDeleteMember = value
        End Set
    End Property

#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000204_Load
    '   名称　：フォームロード
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub FM000204_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim db_inf As CLAccessMdb = Nothing 'DB接続用
        Dim tbResultSql As New DataTable    'SQL結果取得用
        Try
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            '画面初期（あ行～データグリッド）*後でループで処理するため配列
            ARR_DATAGRIDVIEW.SetValue(dgdALine, 0)
            ARR_DATAGRIDVIEW.SetValue(dgdKaLine, 1)
            ARR_DATAGRIDVIEW.SetValue(dgdSaLine, 2)
            ARR_DATAGRIDVIEW.SetValue(dgdTaLine, 3)
            ARR_DATAGRIDVIEW.SetValue(dgdNaLine, 4)
            ARR_DATAGRIDVIEW.SetValue(dgdHaLine, 5)
            ARR_DATAGRIDVIEW.SetValue(dgdMaLine, 6)
            ARR_DATAGRIDVIEW.SetValue(dgdYaLine, 7)
            ARR_DATAGRIDVIEW.SetValue(dgdRaLine, 8)
            ARR_DATAGRIDVIEW.SetValue(dgdWaLine, 9)
            ARR_DATAGRIDVIEW.SetValue(dgdOther, 10)
            'DB接続
            db_inf = New CLAccessMdb
            Call db_inf.Connect()
            'Where句の一部作成
            Call CheckNewPeriod(db_inf)
            Call MakeSqlWhereStandardDay()
            '▼*****SQL実行（あ～その他タブ結果表示）*****
            Call SetJpnSyllabaryEachPage(db_inf)
            '▲
            Call InitializeSearchGridView()
            'コンボボックスの表示データ取得
            If GetCboData(db_inf) = False Then
                Exit Sub
            End If
            '先頭のタブを初期選択する
            tclUnionMemberExtraction.SelectedIndex = 2
            tclJpnSyllabary.SelectedIndex = 0
            showDataGridView = dgdCommitteeResult
            '▼***画面左部組合員リストグリッド初期表示
            Call MakeSelectedUnionMemberList(db_inf)
            '▲***画面左部組合員リストグリッド初期表示()
            '削除不可メンバー取得
            If Not _AllowDeleteMember Then
                Dim strStaffnumber As List(Of DataGridViewRow) = New List(Of DataGridViewRow)
                For Each dtRow As DataGridViewRow In dgdUnionMemberList.Rows
                    strStaffnumber.Add(dtRow)
                Next
                NotDeleteStafIDList = strStaffnumber.ToArray()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call db_inf.Disconnect()
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOK_Click
    '   名称　：OKボタンクリック
    '   概要  ：OKボタンが押されたら_IntQlickBtnFlagのプロパティを0にする。
    '           また抽出リストに記載されたメンバーの情報を_SelectMemberListプロパティに格納する
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If dgdUnionMemberList.Rows.Count > 0 Then
                SelectMemberList = GetDgdUnionSelectListDataTable()
            Else
                CLMsg.Show("GE0072")
                Exit Sub
            End If
            IntQlickBtnFlag = 0
            Me.Visible = False
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Me.Visible = False
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 1
            Me.Visible = False
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnLeft_Click
    '   名称　：＜ボタンクリック
    '   概要  ：画面左の抽出リストに、選択されたデータを追加する
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Call AddUnionMemberList()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearchStafID_Click
    '   名称　：社員番号タブの検索ボタンクリック
    '   概要  ：社員番号タブのデータグリッドビューに検索結果を表示する
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearchStafID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchStafID.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Call SearchStafID()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtStafID_KeyPress
    '   名称　：テキストボックスエンターキー
    '   概要  ：テキストボックスエンターキー
    '   作成日：2011/12/22 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22 somezaki  新規作成
    '***************************************************************************************************
    Private Sub txtStafID_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStafID.KeyPress
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchStafID()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtStafID_TextChanged
    '   名称　：社員番号テキストボックス変更
    '   概要  ：社員番号タブのデータグリッドビューを初期化する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub txtStafID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStafID.TextChanged
        Try
            Call MakeInitialColumns(Me.dgdStafIDResult)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearchKana_Click
    '   名称　：50音タブの検索ボタンクリック
    '   概要  ：検索タブ内のデータグリッドビューに検索結果を表示する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearchKana_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchKana.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Call SearchKana()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtKanaSearchWord_KeyPress
    '   名称　：50音タブのカナ検索テキストボックスエンター
    '   概要  ：カナ検索テキストボックスでエンターボタン押下時に検索処理を行う
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub txtKanaSearchWord_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtKanaSearchWord.KeyPress
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchKana()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtKanaSearchWord_TextChanged
    '   名称　：カナ検索項目変更
    '   概要  ：カナ検索の条件が変更されたら検索結果を初期化する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub txtKanaSearchWord_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtKanaSearchWord.TextChanged
        Try
            Call MakeInitialColumns(Me.dgdKanaSearchResult)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearchCommittee_Click
    '   名称　：委員会タブの検索ボタンクリック
    '   概要  ：検索タブ内のデータグリッドビューに検索結果を表示する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearchCommittee_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchCommittee.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Call SearchCommiteeMember()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearchCommittee_Click
    '   名称　：委員会タブの検索ボタンクリック
    '   概要  ：検索タブ内のデータグリッドビューに検索結果を表示する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub cboSectionCommittee_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboSectionCommittee.KeyPress
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchCommiteeMember()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearchCommittee_Click
    '   名称　：委員会タブの検索ボタンクリック
    '   概要  ：検索タブ内のデータグリッドビューに検索結果を表示する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub cboUnionBranch_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboUnionBranch.KeyPress
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchCommiteeMember()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboSectionCommittee_SelectedIndexChanged
    '   名称　：検索条件の委員会変更時
    '   概要  ：検索条件変更時はデータグリッドビューをクリアする
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub cboSectionCommittee_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSectionCommittee.SelectedIndexChanged
        Try
            Call MakeInitialColumns(Me.dgdCommitteeResult)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboUnionBranch_SelectedIndexChanged
    '   名称　：検索条件の委員会変更時
    '   概要  ：検索条件変更時はデータグリッドビューをクリアする
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub cboUnionBranch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUnionBranch.SelectedIndexChanged
        Try
            Call MakeInitialColumns(Me.dgdCommitteeResult)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnRight_Click
    '   名称　：＞ボタンクリック
    '   概要  ：画面左の抽出リストから選択されたデータを追加する
    '   作成日：2011/11/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim aryNameList As ArrayList = New ArrayList()
            Dim errorCnt As Integer = 0
            If _AllowDeleteMember Then
                For Each delRow In dgdUnionMemberList.SelectedRows
                    dgdUnionMemberList.Rows.Remove(delRow)
                Next
                Call SetRowCnt()
            Else
                'エラーメッセージ
                Dim strDelStaffID As List(Of String) = New List(Of String)

                For Each delRow In dgdUnionMemberList.SelectedRows
                    'strDelStaffID.Add(dtRow.Cells.Item(STR_COLUMNNAME_STAFFID).Value)
                    Dim blnIsExist = False
                    For Each CanNotDel In NotDeleteStafIDList
                        If delRow.Cells.Item(STR_COLUMNNAME_STAFFID).Value = CanNotDel.Cells.Item(STR_COLUMNNAME_STAFFID).Value Then
                            aryNameList.Add(CanNotDel.Cells.Item(STR_COLUMNNAME_NAME).Value.ToString.Trim())
                            errorCnt = errorCnt + 1
                            blnIsExist = True
                            Exit For
                        End If
                    Next
                    If Not blnIsExist Then
                        dgdUnionMemberList.Rows.Remove(delRow)
                    End If
                Next
                '削除不可時の初期メンバーはエラー表示する
                If aryNameList.Count > 0 Then
                    If aryNameList.Count = 1 Then
                        CLMsg.Show("GE0043", aryNameList(0))
                    Else
                        Dim iCnt As Integer = 0
                        Dim aryErrList As ArrayList = New ArrayList()
                        Dim clsUC999999 As New UC999999

                        For Each strName As String In aryNameList
                            aryErrList.Add(CLMsg.GetMsg("GE0043", aryNameList(iCnt)))
                            iCnt = iCnt + 1
                        Next
                        clsUC999999.errMsgList = aryErrList
                        'エラーメッセージリストの表示
                        clsUC999999.ShowDialog()

                    End If
                End If
                Call SetRowCnt()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOrderStafID_Click
    '   名称　：社員番号順ボタンクリック
    '   概要  ：画面左の抽出リストを社員番号順で並び替え
    '   作成日：2011/11/15 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnOrderStafID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOrderStafID.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            'cfgUnionMemberList.Cols.Grid.Sort(C1.Win.C1FlexGrid.SortFlags.Ascending, 1)
            If Me.dgdUnionMemberList.RowCount < 1 Then
                Exit Sub
            End If
            Dim dt As DataTable = GetDgdUnionSelectListDataTable()
            '並び替え指定
            Dim Rows() As DataRow = dt.Select("1=1", STR_COLUMNNAME_STAFFID)
            'いったんクリア
            Call Me.dgdUnionMemberList.Rows.Clear()
            'データ追加
            For i = 0 To Rows.Count - 1
                Call dgdUnionMemberList.Rows.Add(Rows(i).ItemArray)
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSeparateModel_Click
    '   名称　：機種別ボタンクリック
    '   概要  ：画面左の抽出リストから選択されたデータを機種順に並び替える
    '   作成日：2011/11/15 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSeparateModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeparateModel.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Me.dgdUnionMemberList.RowCount < 1 Then
                Exit Sub
            End If
            Dim dt As DataTable = GetDgdUnionSelectListDataTable()
            '並び替え指定
            Dim Rows() As DataRow = dt.Select("1=1", STR_COLUMNNAME_MODEL + "," + STR_COLUMNNAME_STAFFID)
            'いったんクリア
            Call dgdUnionMemberList.Rows.Clear()
            'データ追加
            For i = 0 To Rows.Count - 1
                Call dgdUnionMemberList.Rows.Add(Rows(i).ItemArray)
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：tclUnionMemberExtraction_Selected
    '   名称　：画面右部のタブ選択
    '   概要  ：＜ボタンクリック時にどのグリッドビューの選択列を
    '           抽出リストに追加するか判別するために表示されているグリッド情報を格納する。
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub tclUnionMemberExtraction_Selected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tclUnionMemberExtraction.Selected
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            'どのタブが選択されたかチェック
            If tclUnionMemberExtraction.SelectedTab.Equals(tbpJpnSyllabaryPage) Then '50音タブ
                'さらにこのページに含まれるタブで選択されているページを取得する。
                For Each cControl As Control In tclJpnSyllabary.SelectedTab.Controls()
                    ' 列挙したコントロールのグリッドを取得
                    If TypeOf cControl Is DataGridView Then
                        showDataGridView = cControl
                    End If
                Next cControl
            ElseIf tclUnionMemberExtraction.SelectedTab.Equals(tbpStafIdPage) Then
                showDataGridView = dgdStafIDResult '社員番号タブ
            ElseIf tclUnionMemberExtraction.SelectedTab.Equals(tbpCommitteePage) Then
                'MessageBox.Show("委員会タブ")
                showDataGridView = dgdCommitteeResult '"委員会タブ
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：tclJpnSyllabary_SelectedIndexChanged
    '   名称　：画面右部の50音検索タブの各（あ行～）タブ選択時
    '   概要  ：＜ボタンクリック時にどのグリッドビューの選択列を
    '           抽出リストに追加するか判別するために表示されているグリッド情報を格納する。
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub tclJpnSyllabary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tclJpnSyllabary.SelectedIndexChanged
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            For Each cControl As Control In tclJpnSyllabary.SelectedTab.Controls()
                If TypeOf cControl Is DataGridView Then ' グリッドコントロールを取得
                    showDataGridView = cControl
                End If
            Next cControl
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdALine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdALine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdALine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdKaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdKaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdKaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdSaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdSaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdSaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdTaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdTaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdTaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdNaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdNaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdNaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdHaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdHaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdHaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdMaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdMaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdMaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdYaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdYaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdYaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdRaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdRaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdRaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdWaLine_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdWaLine_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdWaLine.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdOther_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdOther_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdOther.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdKanaSearchResult_CellDoubleClick
    '   名称　：ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdKanaSearchResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdKanaSearchResult.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdStafIDResult_CellDoubleClick
    '   名称　：社員番号検索ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdStafIDResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdStafIDResult.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdCommitteeResult_CellDoubleClick
    '   名称　：委員会ダブルクリック処理
    '   概要  ：
    '   作成日：2011/11/14 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub dgdCommitteeResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdCommitteeResult.CellDoubleClick
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If e.RowIndex <> -1 Then
                Call AddUnionMemberList()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：SetDatagridDefault
    '   名称　：データグリッドビューのヘッダー設定
    '   概要　：データグリッドビューの設定を行う
    '   引数　：ByVal setDatagridview As DataGridView = データグリッドビュー
    '   戻り値：なし
    '   作成日：2011/11/15 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>データグリッドビューのヘッダー設定</summary>
    ''' <param name="setDatagridview">データグリッドビュー</param>
    ''' <remarks></remarks>
    Private Sub SetDatagridDefault(ByVal setDatagridview As DataGridView)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            setDatagridview.Visible = True
            setDatagridview.ColumnHeadersDefaultCellStyle.Alignment = DGD_HEADER_CENTER 'ヘッダーセンター
            setDatagridview.DefaultCellStyle.ForeColor = DGD_FORECOLOR_BLACK            '文字黒
            setDatagridview.RowHeadersVisible = DGD_ROWHEADER_NONVISIBLE                'ヘッダー非表示
            setDatagridview.AllowUserToAddRows = DGD_ADDROW_NON                         '追加不可
            setDatagridview.AllowUserToDeleteRows = DGD_DELETEROW_NON                   '削除不可
            setDatagridview.SelectionMode = DGD_SELECTIONMODE_FULLRAW                   '行選択
            setDatagridview.ReadOnly = True                                             '編集不可
            setDatagridview.AllowUserToResizeRows = False                               '行幅変更不可
            For intColCnt As Integer = 0 To setDatagridview.Columns.Count - 1
                setDatagridview.Columns(intColCnt).Name = ARR_CULUMNSNAME_SELECTLIST(intColCnt)
                setDatagridview.Columns(intColCnt).HeaderText = ARR_CULUMNSHEADER_SELECTLIST(intColCnt)
                setDatagridview.Columns(intColCnt).Width = ARR_CULUMNSWIDTH_SELECTLIST(intColCnt)
                setDatagridview.Columns(intColCnt).Visible = ARR_CULUMNSSHOW_SELECTLIST(intColCnt)
                If (ARR_CULUMNSHEADER_SELECTLIST(intColCnt) = STR_COLUMNNAME_STAFFID) Then
                    setDatagridview.Columns(intColCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf (ARR_CULUMNSHEADER_SELECTLIST(intColCnt) = STR_COLUMNNAME_NAME) Then
                    setDatagridview.Columns(intColCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Else
                    setDatagridview.Columns(intColCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：GetCmbData
    '   名称　：コンボボックスのデータ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/15 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 m.somezaki  新規作成
    ' 　　　　：2013/08/31 Fujisaku 更新　当期は現在日
    '***************************************************************************************************
    ''' <summary>各コンボボックスデータ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetCboData(ByVal clsDb As CLAccessMdb) As Boolean
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim blnRet As Boolean = False       ' 処理結果
        Try
            Dim strSql As String = "SELECT l_name as DisplayName, c_committee_id as ValueName from committee " & vbCrLf
            If isNewPeriod Then
                'strSql = strSql & "WHERE NOT ( ((d_from < '" + MDLoginInfo.PeriodFrom + "' AND d_to < '" + MDLoginInfo.PeriodFrom + "')) OR (d_from > FORMAT(GETDATE(),'yyyyMMdd') AND d_to > FORMAT(GETDATE(),'yyyyMMdd')) ) " & vbCrLf
                strSql = strSql & "WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd') AND d_to >= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            Else
                strSql = strSql & "WHERE NOT ( ((d_from < '" + MDLoginInfo.PeriodFrom + "' AND d_to < '" + MDLoginInfo.PeriodFrom + "') OR (d_from > '" + MDLoginInfo.PeriodTo + "' AND d_to > '" + MDLoginInfo.PeriodTo + "')) ) " & vbCrLf
            End If
            strSql = strSql & "GROUP BY l_name, c_committee_id" & vbCrLf
            strSql = strSql & "ORDER BY c_committee_id" 'chk
            ' 委員会コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cboSectionCommittee, strSql, "DisplayName", "ValueName") = False Then
                Return False
            End If
            ' 定数マスタ詳細（組合支部）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, Me.cboUnionBranch, NSMDConst.CONSTANT_ID_BELONGING) = False Then
                Return blnRet
            End If
            ' 処理結果に正常を設定
            blnRet = True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        ' 戻り値格納
        Return blnRet
    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetRowCnt
    '   名称　：組合員一覧件数表示
    '   概要  ：組合員一覧件数表示（フレックスグリッド不使用対応）
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員一覧件数表示</summary>
    ''' <remarks></remarks>
    Private Sub SetRowCnt()
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Me.gbxUnionMemberList.Text = "組合員一覧(" + (dgdUnionMemberList.Rows.Count).ToString + "件)"
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SetJpnSyllabaryEachPage
    '   名称　：50音順タブページの初期表示
    '   概要  ：50音順タブページのデータグリッドビューを初期表示する
    '   引数　：ByVal db_inf As CLAccessMdb = データベースクラス
    '   戻り値：なし
    '   作成日：2011/11/15 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>50音順タブページの初期表示</summary>
    ''' <param name="db_inf">データベースクラス</param>
    ''' <remarks></remarks>
    Private Sub SetJpnSyllabaryEachPage(ByVal db_inf As CLAccessMdb)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim tbResultSql As New DataTable    'SQL結果取得用
            Dim strSql As String = "" 'SQL文格納用
            Dim intCnt As Integer = 0 '配列用ループカウンタ
            intCnt = 0
            For Each setDataGrid As DataGridView In ARR_DATAGRIDVIEW
                strSql = selectWhereStandardDay + ARR_STR_SQLHAVING(intCnt) + " ORDER BY staf_attr.l_name_kna ASC" & UtDb.DbOrderOffset 'ok
                strSql = STR_SELECT_BASE + strSql
                Dim bsSource As New BindingSource

                tbResultSql = db_inf.ExecuteSql(strSql)
                bsSource.DataSource = tbResultSql
                setDataGrid.DataSource = bsSource
                intCnt = intCnt + 1
                Call SetDatagridDefault(setDataGrid)
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：InitializeSearchGridView
    '   名称　：初期表示でデータなしのグリッドの初期化
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/21 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>初期表示でデータなしのグリッドの初期化</summary>
    ''' <remarks></remarks>
    Private Sub InitializeSearchGridView()
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Me.dgdKanaSearchResult.Visible = True
            Me.dgdStafIDResult.Visible = True
            Me.dgdCommitteeResult.Visible = True
            Call MakeInitialColumns(Me.dgdKanaSearchResult)
            Call MakeInitialColumns(Me.dgdStafIDResult)
            Call MakeInitialColumns(Me.dgdCommitteeResult)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：MakeSelectedUnionMemberList
    '   名称　：組合員リスト初期値設定処理
    '   概要  ：フォームロード時に組合員のリスト(画面左のグリッド)に社員番号の配列で指定された組合員、
    '           もしくは指定されたSQL結果（抽出カラムは当画面に合わせることが条件となる）を初期表示する。
    '           （フレックスグリッド不使用対応）
    '   引数　：ByVal db_inf As CLAccessMdb = データベースクラス
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員リスト初期値設定処理</summary>
    ''' <param name="db_inf">データベースクラス</param>
    ''' <remarks></remarks>
    Private Sub MakeSelectedUnionMemberList(ByVal db_inf As CLAccessMdb)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim tbResultSql As New DataTable    'SQL結果取得用
            Dim intCnt As Integer = 0           'カウンター
            If Not ChkNull(StrSqlSentence) Then 'SQLパターン
                tbResultSql = db_inf.ExecuteSql(StrSqlSentence)
                If tbResultSql.Rows.Count > 0 Then
                    ' 1件以上の処理  
                    Call SetUnionMemberDataTable(tbResultSql)
                End If
            ElseIf Not StafIDList Is Nothing Then '社員番号リストパターン
                For Each strStafId As String In StafIDList
                    Dim strSqlStafId = " (SELECT x.*, cd1.l_name AS l_name1 , cd2.l_omission_name AS l_name2 , cd3.l_name AS l_name3 ,  cd4.l_name AS l_name4 , " & vbCrLf
                    '追加下1行
                    strSqlStafId = strSqlStafId & " cd3.l_omission_name AS l_name_3_2 ,cd4.l_omission_name AS l_name_4_2 , " & vbCrLf
                    strSqlStafId = strSqlStafId & " cd5.l_name AS l_name5 , cd6.l_name AS l_name6 , cd7.l_name AS l_name7 ,  cd8.l_name AS l_name8   FROM staf_attribute AS x, constant_dtl cd1,constant_dtl "
                    strSqlStafId = strSqlStafId & " cd2,constant_dtl cd3,constant_dtl cd4, constant_dtl cd5,constant_dtl cd6,constant_dtl cd7,constant_dtl cd8,  (SELECT c_user_id , max(d_from) AS new_from FROM "
                    strSqlStafId = strSqlStafId & "staf_attribute  WHERE d_from <= '" & standardDay & "' GROUP BY c_user_id ) AS y "
                    strSqlStafId = strSqlStafId & " WHERE x.c_user_id=y.c_user_id  AND x.d_from=y.new_from "
                    strSqlStafId = strSqlStafId & "  AND cd1.c_constant = 'BELONGING' AND cd1.c_constant_seq = x.k_belonging"
                    strSqlStafId = strSqlStafId & "  AND cd2.c_constant = 'QUALIFICATION'  AND cd2.c_constant_seq = x.k_qualification"
                    strSqlStafId = strSqlStafId & "  AND cd3.c_constant = 'MODEL'  AND cd3.c_constant_seq = x.k_model"
                    strSqlStafId = strSqlStafId & "  AND cd4.c_constant = 'AREA_LOCAL'  AND cd4.c_constant_seq = x.k_local"
                    strSqlStafId = strSqlStafId & "  AND cd5.c_constant = 'STAF_KIND'  AND cd5.c_constant_seq = x.k_staf_kind "
                    strSqlStafId = strSqlStafId & "  AND cd6.c_constant = 'USER_STATUS'  AND cd6.c_constant_seq = x.k_user_status "
                    strSqlStafId = strSqlStafId & "  AND cd7.c_constant = 'WORK_PLACE'  AND cd7.c_constant_seq = x.k_work_place"
                    strSqlStafId = strSqlStafId & "  AND cd8.c_constant = 'SEX'  AND cd8.c_constant_seq = x.k_sex "
                    'strSqlStafId = strSqlStafId & "  AND x.k_staf_kind IN ('01','02') "
                    strSqlStafId = strSqlStafId & "  AND x.c_staf_id = '" + strStafId + "'"
                    strSqlStafId = strSqlStafId & ") AS staf_attr"
                    strSqlStafId = strSqlStafId & ""
                    'strSqlStafId = strSqlStafId & "   (SELECT * FROM " & vbCrLf
                    'strSqlStafId = strSqlStafId & "    (SELECT c_user_id , max(d_from) AS new_from FROM staf_attribute WHERE d_from <= '" & standardDay & "' GROUP BY c_user_id) AS max_attr" & vbCrLf
                    'strSqlStafId = strSqlStafId & "    WHERE staf_attr.c_user_id = max_attr.c_user_id AND staf_attr.d_from = max_attr.new_from )) "
                    'strSqlStafId = strSqlStafId & "AND staf_attr.c_staf_id = '" + strStafId + "'"
                    strSqlStafId = STR_SELECT_BASE & strSqlStafId
                    'STR_SELECT_BASE(+"WHERE staf_attr.c_staf_id = '" + strStafId + "'")
                    tbResultSql = db_inf.ExecuteSql(strSqlStafId)
                    If tbResultSql.Rows.Count > 0 Then
                        ' 1件以上の処理                             
                        Call SetUnionMemberDataTable(tbResultSql)
                    End If
                Next
            End If
            'ヘッダー作成
            intCnt = 0
            For Each col As DataGridViewColumn In Me.dgdUnionMemberList.Columns
                col.Name = ARR_CULUMNSNAME_SELECTLIST(intCnt)
                col.HeaderText = ARR_CULUMNSHEADER_SELECTLIST(intCnt)
                col.Width = ARR_CULUMNSWIDTH_SELECTLIST(intCnt)
                col.Visible = ARR_CULUMNSSHOW_UNIONSELECTLIST(intCnt)
                col.DefaultCellStyle.ForeColor = Color.Black
                'col.SortMode = DataGridViewColumnSortMode.NotSortable
                If (ARR_CULUMNSHEADER_SELECTLIST(intCnt) = STR_COLUMNNAME_STAFFID) Then
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf (ARR_CULUMNSHEADER_SELECTLIST(intCnt) = STR_COLUMNNAME_NAME) Then
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                Else
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
                intCnt = intCnt + 1
            Next
            Me.dgdUnionMemberList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgdUnionMemberList.AllowUserToResizeRows = False                               '行幅変更不可
            Call SetRowCnt()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SetUnionMember
    '   名称　：組合員選択データグリッド表示処理
    '   概要  ：取得したデータテーブルを組合員抽出リストに表示する。（フレックスグリッド不使用対応）
    '   引数　：ByVal tbResultSql As DataTable = 組合員抽出リスト
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員選択データグリッド表示処理</summary>
    ''' <param name="tbResultSql">組合員抽出リスト</param>
    ''' <remarks></remarks>
    Private Sub SetUnionMemberDataTable(ByVal tbResultSql As DataTable)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim row As DataRow                                                                  ' データ行
        Dim nowRowCnt = Me.dgdUnionMemberList.Rows.Count
        Dim intInsertRow As Integer = 0
        Try
            For i = 0 To tbResultSql.Rows.Count - 1
                ' 行作成
                dgdUnionMemberList.Rows.Add()
                ' 追加行データ取得
                row = tbResultSql.Rows(i)
                ' データ投入
                intInsertRow = i + nowRowCnt
                For intColCnt = 0 To Me.dgdUnionMemberList.ColumnCount - 1
                    If ARR_CULUMNSNAME_SELECTLIST(intColCnt) = STR_COLUMNNAME_STAFFID Then
                        Me.dgdUnionMemberList.Rows(intInsertRow).Cells.Item(intColCnt).Value = tbResultSql.Rows(i).Item(ARR_CULUMNSNAME_SELECTLIST(intColCnt))
                    Else
                        Me.dgdUnionMemberList.Rows(intInsertRow).Cells.Item(intColCnt).Value = NVL(tbResultSql.Rows(i).Item(ARR_CULUMNSNAME_SELECTLIST(intColCnt)))
                    End If
                Next
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SetUnionMemberRow
    '   名称　：組合員抽出リスト（画面左グリッド）データグリッド表示
    '   概要  ：取得したデータを組合員抽出リストに表示する。（フレックスグリッド不使用対応）
    '   引数　：ByVal addRow As DataGridViewRow = 組合員抽出リスト
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員抽出リスト（画面左グリッド）データグリッド表示</summary>
    ''' <param name="addRow">組合員抽出リスト</param>
    ''' <remarks></remarks>
    Private Sub SetUnionMemberRow(ByVal addRow As DataGridViewRow)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            ' データ行
            Dim nowRowCnt = Me.dgdUnionMemberList.Rows.Count
            Dim intInsertRow As Integer = nowRowCnt
            dgdUnionMemberList.Rows.Add()
            For intColCnt = 0 To Me.dgdUnionMemberList.ColumnCount - 1
                Dim insertValue As Object
                If ARR_CULUMNSNAME_SELECTLIST(intColCnt) = STR_COLUMNNAME_STAFFID Then
                    insertValue = addRow.Cells((ARR_CULUMNSNAME_SELECTLIST(intColCnt))).Value
                Else
                    If Not IsDBNull(addRow.Cells(ARR_CULUMNSNAME_SELECTLIST(intColCnt))) Then
                        insertValue = addRow.Cells((ARR_CULUMNSNAME_SELECTLIST(intColCnt))).Value.ToString
                    Else
                        insertValue = ""
                    End If
                End If
                Me.dgdUnionMemberList.Rows(intInsertRow).Cells.Item(intColCnt).Value = insertValue
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：GetDgdUnionSelectListDataTable
    '   名称　：組合員抽出リスト作成処理
    '   概要  ：組合員抽出リスト（画面左グリッド）に表示されている組合員の情報をデータテーブルに格納し返す（フレックスグリッド不使用対応）
    '   引数　：なし
    '   戻り値：GetDgdUnionSelectListDataTable As DataTable = 組合員抽出リスト
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員抽出リスト作成処理</summary>
    ''' <returns>組合員抽出リスト</returns>
    ''' <remarks></remarks>
    Private Function GetDgdUnionSelectListDataTable() As DataTable
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim dtReturn As New DataTable
        Try
            If dgdUnionMemberList.Rows.Count > 0 Then
                '列追加
                For i = 0 To dgdUnionMemberList.ColumnCount - 1
                    dtReturn.Columns.Add(dgdUnionMemberList.Columns.Item(i).Name)
                Next
                '値投入
                For rowAddCnt = 0 To dgdUnionMemberList.Rows.Count - 1
                    Call dtReturn.Rows.Add()    '行追加
                    For colAddCnt = 0 To dgdUnionMemberList.ColumnCount - 1
                        Dim strInsertData As String = ""
                        If Not (IsDBNull(dgdUnionMemberList.Rows.Item(rowAddCnt).Cells(ARR_CULUMNSNAME_SELECTLIST(colAddCnt)))) Then
                            strInsertData = dgdUnionMemberList.Rows.Item(rowAddCnt).Cells(ARR_CULUMNSNAME_SELECTLIST(colAddCnt)).Value.ToString
                        End If
                        dtReturn.Rows.Item(rowAddCnt).Item(ARR_CULUMNSNAME_SELECTLIST(colAddCnt)) = strInsertData
                    Next
                Next
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return dtReturn
    End Function

    '***************************************************************************************************
    '   ＩＤ　：MakeSqlWhereStandardDay
    '   名称　：SQL作成処理
    '   概要  ：SQLの共通的に使われるWhere句を作成
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>SQL作成処理</summary>
    ''' <remarks></remarks>
    Private Sub MakeSqlWhereStandardDay()
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            selectWhereStandardDay = "                          (SELECT x.*, cd1.l_name AS l_name1 , cd2.l_omission_name AS l_name2 , cd3.l_name AS l_name3 ,cd4.l_name AS l_name4 ," & vbCrLf
            '追加下1行
            selectWhereStandardDay = selectWhereStandardDay + "              cd3.l_omission_name AS l_name_3_2 ,cd4.l_omission_name AS l_name_4_2 , " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "              cd5.l_name AS l_name5 , cd6.l_name AS l_name6 , cd7.l_name AS l_name7 ,  cd8.l_name AS l_name8 "
            selectWhereStandardDay = selectWhereStandardDay + "  FROM staf_attribute AS x, constant_dtl cd1,constant_dtl cd2,constant_dtl cd3,constant_dtl cd4, constant_dtl cd5,constant_dtl cd6,constant_dtl cd7,constant_dtl cd8,"
            selectWhereStandardDay = selectWhereStandardDay + "  (SELECT c_user_id , max(d_from) AS new_from FROM staf_attribute  WHERE d_from <= '" + standardDay + "' GROUP BY c_user_id ) AS y " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "WHERE x.c_user_id=y.c_user_id  AND x.d_from=y.new_from " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd1.c_constant = 'BELONGING' AND cd1.c_constant_seq = x.k_belonging" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd2.c_constant = 'QUALIFICATION'  AND cd2.c_constant_seq = x.k_qualification" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd3.c_constant = 'MODEL'  AND cd3.c_constant_seq = x.k_model" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd4.c_constant = 'AREA_LOCAL'  AND cd4.c_constant_seq = x.k_local" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd5.c_constant = 'STAF_KIND'  AND cd5.c_constant_seq = x.k_staf_kind " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd6.c_constant = 'USER_STATUS'  AND cd6.c_constant_seq = x.k_user_status " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd7.c_constant = 'WORK_PLACE'  AND cd7.c_constant_seq = x.k_work_place" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND cd8.c_constant = 'SEX'  AND cd8.c_constant_seq = x.k_sex " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND x.k_staf_kind IN ('01','02') " & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "  AND x.k_user_status = '01'" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + ") AS staf_attr" & vbCrLf
            selectWhereStandardDay = selectWhereStandardDay + "WHERE 1=1 " & vbCrLf
            'selectWhereStandardDay = selectWhereStandardDay + "  WHERE staf_attr.c_user_id=new_attr.c_user_id AND staf_attr.d_from=new_attr.new_from " & vbCrLf
            'selectWhereStandardDay = selectWhereStandardDay + "  )       )  " & vbCrLf
            'selectWhereStandardDay = selectWhereStandardDay + "AND (SELECT constant_dtl.l_name FROM constant_dtl WHERE constant_dtl.c_constant = 'STAF_KIND' AND constant_dtl.c_constant_seq = staf_attr.k_staf_kind) IN ('正組合員' , 'シニア組合員') " & vbCrLf
            'selectWhereStandardDay = selectWhereStandardDay + "AND (SELECT constant_dtl.l_name FROM constant_dtl WHERE constant_dtl.c_constant = 'USER_STATUS' AND constant_dtl.c_constant_seq = staf_attr.k_user_status) = '加入' " & vbCrLf
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：CheckNewPeriod
    '   名称　：最新期設定処理
    '   概要  ：ログイン期が最新であれば内部プロパティに最新期であることをセットする
    '   引数　：ByVal db_inf As CLAccessMdb = データベースクラス
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>最新期設定処理</summary>
    ''' <param name="db_inf">データベースクラス</param>
    ''' <remarks></remarks>
    Private Sub CheckNewPeriod(ByVal db_inf As CLAccessMdb)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim dtResultSql As New DataTable
            Dim periodSql As String = "SELECT c_period_id FROM period ORDER BY d_from DESC" 'chk
            dtResultSql = db_inf.ExecuteSql(periodSql)
            If dtResultSql.Rows(0).Item(0).ToString() = MDLoginInfo.PeriodId Then
                isNewPeriod = True
                standardDay = Now.ToString("yyyyMMdd")
            Else
                isNewPeriod = False
                standardDay = MDLoginInfo.PeriodTo
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：AddUnionMemberList
    '   名称　：組合員リストデータ追加処理
    '   概要  ：選択されている右データグリッドの行を左データグリッドに追加する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員リストデータ追加処理</summary>
    ''' <remarks></remarks>
    Private Sub AddUnionMemberList()
        Dim aryNameList As ArrayList = New ArrayList()
        Dim errorCnt As Integer = 0
        Try
            If showDataGridView.SelectedRows.Count > 0 Then
                '選択行の数だけループ
                For Each selectRaw As DataGridViewRow In showDataGridView.SelectedRows
                    'ヘッダーの下行からすでに存在するデータではないかチェック
                    Dim blnIsExist = False
                    For intRow As Integer = 0 To dgdUnionMemberList.Rows.Count - 1
                        'すでに選択されている組合員リストの社員番号のセルと比較
                        Dim strCheckVal As String = dgdUnionMemberList.Rows.Item(intRow).Cells(STR_COLUMNNAME_STAFFID).value.ToString.Trim()
                        If strCheckVal = (selectRaw.Cells.Item(STR_COLUMNNAME_STAFFID).Value.ToString.Trim()) Then
                            'メッセージ
                            aryNameList.Add(selectRaw.Cells.Item(STR_COLUMNNAME_NAME).Value.ToString.Trim())
                            errorCnt = errorCnt + 1
                            blnIsExist = True
                            Exit For
                        End If
                    Next
                    '最下行にデータ挿入
                    If Not blnIsExist Then
                        '列カウンター
                        Call SetUnionMemberRow(selectRaw)
                        Call SetRowCnt()
                    End If
                Next
                '重複データはエラー表示する
                If aryNameList.Count > 0 Then
                    If aryNameList.Count = 1 Then
                        CLMsg.Show("GE0009", aryNameList(0))
                    Else
                        Dim iCnt As Integer = 0
                        Dim aryErrList As ArrayList = New ArrayList()
                        Dim clsUC999999 As New UC999999
                        For Each strName As String In aryNameList
                            aryErrList.Add(CLMsg.GetMsg("GE0009", aryNameList(iCnt)))
                            iCnt = iCnt + 1
                        Next
                        clsUC999999.errMsgList = aryErrList
                        'エラーメッセージリストの表示
                        clsUC999999.ShowDialog()
                    End If
                End If
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：MakeInitialColumns
    '   名称　：初期表示処理
    '   概要  ：検索ボタン押下時に結果が表示されるグリッドビューの初期表示状態の作成
    '           データなしの状態で列名のみ表示
    '   引数　：ByVal makeColDgd As DataGridView = 初期表示状態グリッドビュー
    '   戻り値：なし
    '   作成日：2011/12/21 somezaki
    '   更新日：
    '***************************************************************************************************
    ''' <summary>初期表示処理</summary>
    ''' <param name="makeColDgd">初期表示状態グリッドビュー</param>
    ''' <remarks></remarks>
    Private Sub MakeInitialColumns(ByVal makeColDgd As DataGridView)
        Try
            Call DgdClearPreSearch(makeColDgd)
            For intColCnt As Integer = 0 To ARR_CULUMNSNAME_SELECTLIST.Length - 1
                'カナ検索
                makeColDgd.Columns.Add(ARR_CULUMNSNAME_SELECTLIST(intColCnt), ARR_CULUMNSHEADER_SELECTLIST(intColCnt))
                Call SetDatagridDefault(makeColDgd)
            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：DgdClearPreSearch
    '   名称　：検索前処理
    '   概要  ：右のデータグリッドビューの検索結果表示グリッド（ｶﾅ検索、社員番号、委員会）の
    '           検索前の初期化を行う。未検索で検索されると現在表示されている列はそのままとなるので、
    '           列削除をおこなう。
    '   引数　：ByVal clearDgd As DataGridView = 検索結果表示グリッドビュー
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>検索前処理</summary>
    ''' <param name="clearDgd">検索結果表示グリッドビュー</param>
    ''' <remarks></remarks>
    Private Sub DgdClearPreSearch(ByVal clearDgd As DataGridView)
        Try
            If clearDgd.DataSource IsNot Nothing Then
                clearDgd.DataSource = Nothing
            Else
                clearDgd.Columns.Clear()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SearchStafID
    '   名称　：社員番号検索処理
    '   概要  ：社員番号で検索を行い対象のデータグリッドビューに結果を表示する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>社員番号検索処理</summary>
    ''' <remarks></remarks>
    Private Sub SearchStafID()
        Dim db_inf As CLAccessMdb = New CLAccessMdb
        Dim dtResultSql As New DataTable
        Try
            'db_inf = New CLAccessMdb
            dtResultSql = New DataTable
            Call db_inf.Connect()
            '条件の入力チェック
            If ChkNull(Me.txtStafID.Text.Trim()) Then
                Call CLMsg.Show("GE0011")
                Call SetErr(Me.txtStafID)
                Exit Sub
            End If
            Me.txtStafID.BackColor = Color.White
            '検索前処理（データグリッドクリア）
            Call DgdClearPreSearch(Me.dgdStafIDResult)
            'SQL文生成
            Dim strSqlSentence As String = ""
            Dim strSqlWhere = "AND staf_attr.c_staf_id LIKE '" + Me.txtStafID.Text.Trim() +
                                "%' ORDER BY CLng(staf_attr.c_staf_id) ASC" & UtDb.DbOrderOffset
            strSqlSentence = STR_SELECT_BASE + selectWhereStandardDay + strSqlWhere
            'データグリッドに連結
            Dim bsSource As New BindingSource
            'todo:
            dtResultSql = db_inf.ExecuteSql(strSqlSentence)
            bsSource.DataSource = dtResultSql
            dgdStafIDResult.DataSource = bsSource
            Call SetDatagridDefault(dgdStafIDResult)
            If dtResultSql.Rows.Count = 0 Then
                Call CLMsg.Show("GE0007")
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
            Call db_inf.Disconnect()
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SearchStafID
    '   名称　：カナ検索検索処理
    '   概要  ：カナで検索を行い対象のデータグリッドビューに結果を表示する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>カナ検索検索処理</summary>
    ''' <remarks></remarks>
    Private Sub SearchKana()
        Dim db_inf As CLAccessMdb = Nothing
        Dim dtResultSql As New DataTable
        Try
            db_inf = New CLAccessMdb
            dtResultSql = New DataTable
            Call db_inf.Connect()
            '条件の入力チェック
            If ChkNull(Me.txtKanaSearchWord.Text.Trim()) Then
                Call CLMsg.Show("GE0011")
                Call SetErr(Me.txtKanaSearchWord)
                Exit Sub
            End If
            'カナチェック
            'If ChkKana(Me.txtKanaSearchWord.Text.Trim()) = False Then
            '    Call CLMsg.Show("GE0019", "半角カナ")
            '    Exit Sub
            'End If
            Me.txtKanaSearchWord.BackColor = Color.White
            '検索前処理（データグリッドクリア）
            Call DgdClearPreSearch(Me.dgdKanaSearchResult)
            'SQL文生成
            Dim strSqlSentence As String = ""
            Dim strSqlWhere = "AND staf_attr.l_name_kna LIKE '%" + Me.txtKanaSearchWord.Text.Trim() + "%' ORDER BY staf_attr.l_name_kna ASC" & UtDb.DbOrderOffset
            strSqlSentence = STR_SELECT_BASE + selectWhereStandardDay + strSqlWhere
            '結果をデータグリッドに連結
            Dim bsSource As New BindingSource
            'todo:
            dtResultSql = db_inf.ExecuteSql(strSqlSentence)
            bsSource.DataSource = dtResultSql
            Me.dgdKanaSearchResult.DataSource = bsSource
            'データグリッドデフォルト設定
            Call SetDatagridDefault(Me.dgdKanaSearchResult)
            '検索タブを選択する
            tclUnionMemberExtraction.SelectedIndex = 0
            tclJpnSyllabary.SelectedIndex = 11
            If dtResultSql.Rows.Count = 0 Then
                Call CLMsg.Show("GE0007")
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
            ' データベース切断
            Call db_inf.Disconnect()
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SearchCommiteeMember
    '   名称　：委員会・組合支部検索処理
    '   概要  ：委員会、組合支部で検索を行い対象のデータグリッドビューに結果を表示する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 somezaki
    '   更新日：2011/03/01 Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 somezaki  新規作成
    ' 　　　　：2011/03/01 Fujisaku  検索条件修正
    '***************************************************************************************************
    ''' <summary>委員会・組合支部検索処理</summary>
    ''' <remarks></remarks>
    Private Sub SearchCommiteeMember()
        Dim db_inf As CLAccessMdb = Nothing
        Dim dtResultSql As New DataTable
        Try
            db_inf = New CLAccessMdb
            dtResultSql = New DataTable
            Call db_inf.Connect()
            '条件の入力チェック
            If Not ChkNull(Me.cboSectionCommittee.Text) Then
                Me.cboSectionCommittee.BackColor = Color.White
                '検索前処理（データグリッドクリア）
                Call DgdClearPreSearch(Me.dgdCommitteeResult)
                'MessageBox.Show(Me.cboSectionCommittee.SelectedValue)
                Dim strWhereSql As String = "" 'Where句
                Dim strSql As String = ""
                strWhereSql = "AND staf_attr.c_staf_id IN ( " & vbCrLf
                strWhereSql = strWhereSql & "SELECT DISTINCT committee_list_dtl.c_user_id FROM committee_list_dtl " & vbCrLf
                strWhereSql = strWhereSql & " WHERE " & vbCrLf
                strWhereSql = strWhereSql & "   committee_list_dtl.d_from = " & vbCrLf
                strWhereSql = strWhereSql & "     (SELECT Max(committee_list.d_from) AS d_fromの最大 " & vbCrLf
                strWhereSql = strWhereSql & "        FROM committee_list INNER JOIN committee_list_dtl ON committee_list.c_committee_list = committee_list_dtl.c_committee_list " & vbCrLf
                strWhereSql = strWhereSql & "        WHERE committee_list.d_from <= '" + standardDay + "' " & vbCrLf
                strWhereSql = strWhereSql & "         AND  committee_list.c_committee_id = '" & Me.cboSectionCommittee.SelectedValue.ToString.Trim() & "'" & vbCrLf
                strWhereSql = strWhereSql & "        GROUP BY committee_list.c_committee_id )" & vbCrLf
                strWhereSql = strWhereSql & "   AND (committee_list_dtl.c_committee_id) = '" & Me.cboSectionCommittee.SelectedValue.ToString.Trim() & "')"
                If Not ChkNull(Me.cboUnionBranch.Text) Then
                    strWhereSql = strWhereSql + " AND (SELECT constant_dtl.l_name FROM constant_dtl WHERE constant_dtl.c_constant = 'BELONGING' AND constant_dtl.c_constant_seq = staf_attr.k_belonging) = '" + Me.cboUnionBranch.Text.Trim + "'"
                End If
                strSql = STR_SELECT_BASE + selectWhereStandardDay + strWhereSql + " ORDER BY staf_attr.l_name_kna ASC " & UtDb.DbOrderOffset
                'データグリッドに連結
                Dim bsSource As New BindingSource
                'todo:
                dtResultSql = db_inf.ExecuteSql(strSql)
                bsSource.DataSource = dtResultSql
                dgdCommitteeResult.DataSource = bsSource
                Call SetDatagridDefault(dgdCommitteeResult)
                If dtResultSql.Rows.Count = 0 Then
                    Call CLMsg.Show("GE0007")
                End If
            Else
                '必須項目なしメッセージ
                Call CLMsg.Show("GE0006", "項目『部/委員会』")
                Call SetErr(Me.cboSectionCommittee)
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
            ' データベース切断
            Call db_inf.Disconnect()
        End Try
    End Sub
#End Region
End Class

#End Region