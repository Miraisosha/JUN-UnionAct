#Region "UC080202"
'===========================================================================================================
'   クラスＩＤ　　：UC080202
'   クラス名称　　：組合員口座情報 - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common

Public Class UC080202

#Region "定数・変数"
    '---------------------------------------------------------------------------
    '   定数
    '---------------------------------------------------------------------------
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC080202                              ' UC080202
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC080202                          ' 組合員口座情報 - 詳細画面
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                                             ' 新規登録
    Private Const STATUS_UPDATE As Byte = 2                                             ' 内容変更（照会モード）
    '---------------------------------------------------------------------------
    '   子画面用プロパティ（履歴ボタン押下時）
    '---------------------------------------------------------------------------
    ' 適用日付選択画面カラム名
    ReadOnly USE_DATE_COL_NAME As String() = {"適用日付", "担当者", "金融機関コード", "支店番号"}
    ReadOnly USE_DATE_COLWIDTH As Integer() = {150, 200, 300, 300}                      ' 適用日付選択画面カラム幅
    ReadOnly USE_DATE_COLSHOW As Boolean() = {True, True, False, False}                 ' 適用日付選択画面カラム表示
    Private TITLE_SELECT_USE_DATE As String = "組合員口座情報履歴 - 適用日付選択画面"   ' タイトル
    '-----------------------------------------------------------------------------------
    '   変数
    '-----------------------------------------------------------------------------------
    Private bytHistoryFlg As Byte = 0                                                   ' 履歴新規登録フラグ（0：通常新規登録, 1：履歴新規登録）
    ' 権限
    Private strGrantReference As String = "0"                                           ' 参照権限
    Private strGrantInsert As String = "0"                                              ' 登録権限
    Private strGrantPrint As String = "0"                                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                                          ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _bytStatus As Byte = 0                                                       ' ステータス（0：新規登録, 1：内容変更（照会モード）, 2：内容変更（修正モード））
    Public _strAttrUserId As String = ""                                                ' 組合員属性情報の個人認証ID
    Public _strAttrKsh As String = ""                                                   ' 組合員属性情報の会社コード
    Public _strAttrStafId As String = ""                                                ' 組合員属性情報の社員番号
    Public _strAttrUseDate As String = ""                                               ' 組合員属性情報の適用日付
    Public _strAcntUserId As String = ""                                                ' 組合員口座情報の個人認証ID
    Public _strAcntUseDate As String = ""                                               ' 組合員口座情報の適用日付
    Public _strAcntBank As String = ""                                                  ' 組合員口座情報の金融機関コード
    Public _strAcntBankOffice As String = ""                                            ' 組合員口座情報の支店番号

    ' ステータス
    Public Property bytStatus() As Byte
        Get
            Return _bytStatus
        End Get
        Set(ByVal value As Byte)
            _bytStatus = value
        End Set
    End Property

    ' 組合員属性情報の個人認証ID
    Public Property strAttrUserId() As String
        Get
            Return _strAttrUserId
        End Get
        Set(ByVal value As String)
            _strAttrUserId = value
        End Set
    End Property

    ' 組合員属性情報の会社コード
    Public Property strAttrKsh() As String
        Get
            Return _strAttrKsh
        End Get
        Set(ByVal value As String)
            _strAttrKsh = value
        End Set
    End Property

    ' 組合員属性情報の社員番号
    Public Property strAttrStafId() As String
        Get
            Return _strAttrStafId
        End Get
        Set(ByVal value As String)
            _strAttrStafId = value
        End Set
    End Property

    ' 組合員属性情報の適用日付
    Public Property strAttrUseDate() As String
        Get
            Return _strAttrUseDate
        End Get
        Set(ByVal value As String)
            _strAttrUseDate = value
        End Set
    End Property

    ' 組合員口座情報の個人認証ID
    Public Property strAcntUserId() As String
        Get
            Return _strAcntUserId
        End Get
        Set(ByVal value As String)
            _strAcntUserId = value
        End Set
    End Property

    ' 組合員口座情報の適用日付
    Public Property strAcntUseDate() As String
        Get
            Return _strAcntUseDate
        End Get
        Set(ByVal value As String)
            _strAcntUseDate = value
        End Set
    End Property

    ' 組合員口座情報の金融機関コード
    Public Property strAcntBank() As String
        Get
            Return _strAcntBank
        End Get
        Set(ByVal value As String)
            _strAcntBank = value
        End Set
    End Property

    ' 組合員口座情報の支店番号
    Public Property strAcntBankOffice() As String
        Get
            Return _strAcntBankOffice
        End Get
        Set(ByVal value As String)
            _strAcntBankOffice = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC080202_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC080202_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If setGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If GetData() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールロック処理
            '-------------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT Then
                '-------------------------------------------
                '   新規登録
                '-------------------------------------------
                If ControlRockUnLock(True) = False Then
                    Exit Sub
                End If
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '-------------------------------------------
                '   内容変更
                '-------------------------------------------
                If ControlRockUnLock(False) = False Then
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
    '   ＩＤ　：btnHistory_Click
    '   名称　：履歴ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/07(月) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click

        Dim clsFM010104 As FM010104 = Nothing                                               ' 適用日付選択画面
        Dim strSql As String = ""                                                           ' SQL文
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス

        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            '-------------------------------------------------------------------------------
            '   適用日付選択画面
            '-------------------------------------------------------------------------------
            clsFM010104 = New FM010104                                                      ' インスタンス作成
            clsFM010104.Text = TITLE_SELECT_USE_DATE                                        ' タイトル設定
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf   ' 適用開始年月日
            strSql = strSql & "       ,staf_max.l_name    AS l_name" & vbCrLf                           ' 最新の作成者の名前
            strSql = strSql & "       ,staf.c_bank        AS c_bank" & vbCrLf                           ' 金融機関コード
            strSql = strSql & "       ,staf.c_bank_office AS c_bank_office" & vbCrLf                    ' 支店番号
            strSql = strSql & "   FROM staf_account AS staf" & vbCrLf
            ' 最新の作成者の口座情報取得
            strSql = strSql & "        LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                          ,b.l_name" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                          ,( SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "                                   ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                   ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                              GROUP BY a.c_user_id" & vbCrLf
            strSql = strSql & "                                      ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                      ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                     WHERE b.c_user_id = c.c_user_id" & vbCrLf
            strSql = strSql & "                       AND b.c_ksh = c.c_ksh" & vbCrLf
            strSql = strSql & "                       AND b.c_staf_id = c.c_staf_id" & vbCrLf
            strSql = strSql & "                       AND b.d_from = c.d_from" & vbCrLf
            strSql = strSql & "                     UNION" & vbCrLf
            strSql = strSql & "                    SELECT e.c_staf_id" & vbCrLf
            strSql = strSql & "                          ,e.l_name" & vbCrLf
            strSql = strSql & "                      FROM full_time_staf AS e" & vbCrLf
            strSql = strSql & "                          ,( SELECT d.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM full_time_staf AS d" & vbCrLf
            strSql = strSql & "                              GROUP BY d.c_staf_id ) AS f" & vbCrLf
            strSql = strSql & "                     WHERE e.c_staf_id = f.c_staf_id" & vbCrLf
            strSql = strSql & "                       AND e.d_from = f.d_from ) AS staf_max" & vbCrLf
            strSql = strSql & "        ON staf.c_user_id_ins = staf_max.c_user_id" & vbCrLf
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.txtStafId.Text.Trim & "'" & vbCrLf
            strSql = strSql & "  ORDER BY staf.d_from DESC" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' プロパティ設定
            clsFM010104.strSqlSentence = strSql
            clsFM010104.SetCulumnsName = USE_DATE_COL_NAME              ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = USE_DATE_COLWIDTH             ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = USE_DATE_COLSHOW               ' 適用日付選択画面カラム表示有無
            clsFM010104.EnableChkDirectSpecify = False                  ' 直接指定使用不可
            clsFM010104.chkDirectSpecify.Checked = False                ' 直接指定チェックなし

            ' 適用日付選択画面表示
            Call clsFM010104.ShowDialog()

            ' クリックされたボタンをチェック
            If clsFM010104.IntQlickBtnFlag = 0 Then
                ' 選択行からのみ（直接指定からの遷移はない）
                ' 選択行から適用日付取得
                Me.strAcntUseDate = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                Me.strAcntBank = clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(2).Value.ToString()
                Me.strAcntBankOffice = clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(3).Value.ToString()
                ' 履歴新規登録フラグを 0 にする
                Me.bytHistoryFlg = 0
                ' データベース接続
                Call clsDb.Connect()
                ' 組合員口座情報取得処理
                If GetStafAccount(clsDb, 1) = False Then
                    Exit Sub
                End If
                ' コントロールロック処理
                If ControlRockUnLock(False) = False Then
                    Exit Sub
                End If
            ElseIf clsFM010104.IntQlickBtnFlag = 1 Then
                ' キャンセルボタン押下
                Exit Sub
            End If

            ' 不要になった時点で破棄
            clsFM010104.Close()
            clsFM010104.Dispose()

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
            ' データベース切断
            Call clsDb.Disconnect()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要  ：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim clsFM010104 As FM010104 = Nothing                                               ' 適用日付選択画面
        Dim strSql As String = ""                                                           ' SQL文
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim bytRefStatus As Byte = 0                                                        ' ステータス（1：通常検索,2：最新検索）

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   適用日付選択画面
            '-------------------------------------------------------------------------------
            clsFM010104 = New FM010104                                                      ' インスタンス作成
            clsFM010104.Text = TITLE_SELECT_USE_DATE                                        ' タイトル設定
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf   ' 適用開始年月日
            strSql = strSql & "       ,staf_max.l_name    AS l_name" & vbCrLf                           ' 最新の作成者の名前
            strSql = strSql & "       ,staf.c_bank        AS c_bank" & vbCrLf                           ' 金融機関コード
            strSql = strSql & "       ,staf.c_bank_office AS c_bank_office" & vbCrLf                    ' 支店番号
            strSql = strSql & "   FROM staf_account AS staf" & vbCrLf
            ' 最新の作成者の口座情報取得
            strSql = strSql & "        LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                          ,b.l_name" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                          ,( SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "                                   ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                   ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                              GROUP BY a.c_user_id" & vbCrLf
            strSql = strSql & "                                      ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                      ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                     WHERE b.c_user_id = c.c_user_id" & vbCrLf
            strSql = strSql & "                       AND b.c_ksh = c.c_ksh" & vbCrLf
            strSql = strSql & "                       AND b.c_staf_id = c.c_staf_id" & vbCrLf
            strSql = strSql & "                       AND b.d_from = c.d_from" & vbCrLf
            strSql = strSql & "                     UNION" & vbCrLf
            strSql = strSql & "                    SELECT e.c_staf_id" & vbCrLf
            strSql = strSql & "                          ,e.l_name" & vbCrLf
            strSql = strSql & "                      FROM full_time_staf AS e" & vbCrLf
            strSql = strSql & "                          ,( SELECT d.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM full_time_staf AS d" & vbCrLf
            strSql = strSql & "                              GROUP BY d.c_staf_id ) AS f" & vbCrLf
            strSql = strSql & "                     WHERE e.c_staf_id = f.c_staf_id" & vbCrLf
            strSql = strSql & "                       AND e.d_from = f.d_from ) AS staf_max" & vbCrLf
            strSql = strSql & "        ON staf.c_user_id_ins = staf_max.c_user_id" & vbCrLf
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.txtStafId.Text.Trim & "'" & vbCrLf
            strSql = strSql & "  ORDER BY staf.d_from DESC" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' プロパティ設定
            clsFM010104.strSqlSentence = strSql
            clsFM010104.SetCulumnsName = USE_DATE_COL_NAME              ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = USE_DATE_COLWIDTH             ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = USE_DATE_COLSHOW               ' 適用日付選択画面カラム表示有無
            clsFM010104.EnableChkDirectSpecify = True                   ' 内容変更ボタン押下時、直接入力使用可能
            clsFM010104.chkDirectSpecify.Checked = True                 ' 直接指定チェック

            ' 適用日付選択画面表示
            Call clsFM010104.ShowDialog()

            ' クリックされたボタンをチェック
            If clsFM010104.IntQlickBtnFlag = 0 Then
                ' OKボタン押下
                If clsFM010104.chkDirectSpecify.Checked Then
                    ' 直接指定して選択から適用日付取得
                    Me.strAcntUseDate = clsFM010104.dtpSpecifyTime.Value.ToString("yyyyMMdd")
                    ' 履歴新規登録フラグを 1 にする
                    bytHistoryFlg = 1
                    ' 2：最新検索
                    bytRefStatus = 2
                Else
                    ' 選択行から適用日付取得
                    Me.strAcntUseDate = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                    ' 選択行から金融機関コード取得
                    Me.strAcntBank = clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(2).Value.ToString()
                    ' 選択行から支店番号取得
                    Me.strAcntBankOffice = clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(3).Value.ToString()
                    ' 履歴新規登録フラグを 0 にする
                    bytHistoryFlg = 0
                    ' 1：通常検索
                    bytRefStatus = 1
                End If
                ' データベース接続
                Call clsDb.Connect()
                ' 銀行名コンボボックス作成処理呼び出し
                If CreateComboBoxBank(clsDb) = False Then
                    Exit Sub
                End If
                ' 組合員口座情報取得処理
                If GetStafAccount(clsDb, bytRefStatus) = False Then
                    Exit Sub
                End If
                ' コントロールロック処理
                If ControlRockUnLock(True) = False Then
                    Exit Sub
                End If
            ElseIf clsFM010104.IntQlickBtnFlag = 1 Then
                ' キャンセルボタン押下
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
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default

            ' 不要になった時点で破棄
            clsFM010104.Close()
            clsFM010104.Dispose()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：戻るボタンクリック処理
    '   概要  ：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理
            If TransitionScreen(False) = False Then
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
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertChk_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要  ：
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertChk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsertChk.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim daiRet As DialogResult = Nothing                                                ' 確認メッセージ結果

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   入力チェック
            '-------------------------------------------------------------------------------
            If ChkInput() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   登録確認メッセージ表示
            '-------------------------------------------------------------------------------
            ' 登録確認メッセージ表示
            daiRet = CLMsg.Show("GQ0001")
            ' 確認メッセージ判定
            If daiRet = DialogResult.No Then
                Exit Sub                    ' 「いいえ」押下時、処理を抜ける
            End If

            '-------------------------------------------------------------------------------
            '   組合員口座情報更新処理（登録・更新）
            '-------------------------------------------------------------------------------
            If InsertUpdate() = False Then
                Exit Sub
            Else
                ' 登録完了メッセージ表示
                Call CLMsg.Show("GI0015")
            End If

            '-------------------------------------------------------------------------------
            '   画面遷移
            '-------------------------------------------------------------------------------
            If TransitionScreen(True) = False Then
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
    '   概要  ：
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 入力・変更内容破棄メッセージボックス表示
            If CLMsg.Show("GQ0007") = DialogResult.No Then
                Exit Sub                    ' 「いいえ」ボタン押下時、処理を抜ける
            End If

            ' 画面遷移処理
            If TransitionScreen(False) = False Then
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
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboBankName_SelectedIndexChanged
    '   名称　：銀行名コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboBankName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBankName.SelectedIndexChanged

        Dim clsDb As New CLAccessMdb            ' データベースクラス

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 銀行・支店関連クリア
            Me.lblBankNameKana.Text = ""                                                ' 銀行名カナ
            Me.lblBank.Text = ""                                                        ' 金融機関コード
            Me.cboBankOfficeName.DataSource = Nothing                                   ' 支店名
            Me.lblBankOfficeNameKana.Text = ""                                          ' 支店名カナ
            Me.lblBankOffice.Text = ""                                                  ' 支店番号

            If Me.cboBankName.SelectedIndex > 0 Then
                ' データベース接続
                Call clsDb.Connect()

                ' 銀行名カナ金融機関コード取得
                If getBankKanaBankCd(clsDb, _
                                     Me.cboBankName.SelectedValue.ToString()) = False Then
                    Exit Sub
                End If

                ' 支店名コンボボックス作成処理呼び出し
                If CreateComboBoxBankOffice(clsDb, _
                                            Me.cboBankName.SelectedValue.ToString()) Then
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
            ' データベース切断
            Call clsDb.Disconnect()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboBankOfficeName_SelectedIndexChanged
    '   名称　：支店名コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboBankOfficeName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBankOfficeName.SelectedIndexChanged

        Dim clsDb As New CLAccessMdb            ' データベースクラス

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 支店関連クリア
            Me.lblBankOfficeNameKana.Text = ""                                          ' 支店名カナ
            Me.lblBankOffice.Text = ""                                                  ' 支店番号

            If Me.cboBankOfficeName.SelectedIndex > 0 Then
                ' データベース接続
                Call clsDb.Connect()
                ' 支店名カナ支店番号取得
                If getBankOfficeKanaBankOfficeCd(clsDb, _
                                                 Me.cboBankName.SelectedValue.ToString(), _
                                                 Me.cboBankOfficeName.SelectedValue.ToString()) = False Then
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
            ' データベース切断
            Call clsDb.Disconnect()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtBankAccount_KeyPress
    '   名称　：口座番号テキストボックスキープレス取得処理
    '   概要  ：数値、BackSpace以外入力無効
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtBankAccount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBankAccount.KeyPress

        Try
            ' 0～9, BackSpace以外、入力禁止
            If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack Then
                e.Handled = True
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
    '   ＩＤ　：txtRokinCir_KeyPress
    '   名称　：労金CIRテキストボックスキープレス取得処理
    '   概要  ：数値、BackSpace以外入力無効
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtRokinCir_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRokinCir.KeyPress

        Try
            ' 0～9, BackSpace以外、入力禁止
            If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And e.KeyChar <> vbBack Then
                e.Handled = True
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
    '   ＩＤ　：txtBankAccount_GotFocus
    '   名称　：口座番号テキストボックスフォーカス取得処理
    '   概要  ：口座番号テキストボックスを全選択する。
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtBankAccount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBankAccount.GotFocus

        Try
            Me.txtBankAccount.SelectAll()

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
    '   ＩＤ　：txtAccountName_GotFocus
    '   名称　：口座名義テキストボックスフォーカス取得処理
    '   概要  ：口座名義テキストボックスを全選択する。
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtAccountName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccountName.GotFocus

        Try
            Me.txtAccountName.SelectAll()

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
    '   ＩＤ　：txtAccountNameKana_GotFocus
    '   名称　：口座名義カナテキストボックスフォーカス取得処理
    '   概要  ：口座名義カナテキストボックスを全選択する。
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtAccountNameKana_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccountNameKana.GotFocus

        Try
            Me.txtAccountNameKana.SelectAll()

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
    '   ＩＤ　：txtRokinCir_GotFocus
    '   名称　：労金CIRテキストボックスフォーカス取得処理
    '   概要  ：労金CIRテキストボックスを全選択する。
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtRokinCir_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRokinCir.GotFocus

        Try
            Me.txtRokinCir.SelectAll()

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
    '   ＩＤ　：CreateComboBoxBank
    '   名称　：銀行名コンボボックス作成処理
    '   概要  ：銀行名コンボボックスリストを作成する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/12(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/12(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>銀行名コンボボックス作成処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateComboBoxBank(ByVal clsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_bank_name AS l_bank_name" & vbCrLf                   ' 銀行名
            strSql = strSql & "       ,a.c_bank      AS c_bank" & vbCrLf                        ' 銀行コード
            strSql = strSql & "   FROM bank_info AS a" & vbCrLf                                 ' 銀行マスタ
            strSql = strSql & "  WHERE a.d_from <= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間From
            strSql = strSql & "    AND a.d_to   >= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間To
            strSql = strSql & "  ORDER BY a.l_bank_name_kna" & vbCrLf                           ' 銀行名カナで並び替え
            If CreateComboBoxNew(clsDb, _
                                 Me.cboBankName, _
                                 strSql, _
                                 "l_bank_name", _
                                 "c_bank") = False Then
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
    '   ＩＤ　：CreateComboBoxBankOffice
    '   名称　：支店名コンボボックス作成処理
    '   概要  ：支店名コンボボックスリストを作成する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス, 
    '           ByVal BankCd As String     = 金融機関コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/12(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/12(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>支店名コンボボックス作成処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="strBankCd">金融機関コード</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateComboBoxBankOffice(ByVal clsDb As CLAccessMdb, _
                                              ByVal strBankCd As String) As Boolean


        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_bank_office_name AS l_bank_office_name" & vbCrLf     ' 支店名
            strSql = strSql & "       ,a.c_bank_office      AS c_bank_office" & vbCrLf          ' 支店番号
            strSql = strSql & "   FROM bank_info_dtl AS a" & vbCrLf                             ' 銀行マスタ詳細
            strSql = strSql & "  WHERE a.c_bank = '" & strBankCd & "'" & vbCrLf                 ' 銀行コードと同じもの
            strSql = strSql & "    AND a.d_from <= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間From
            strSql = strSql & "    AND a.d_to   >= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間To
            strSql = strSql & "  ORDER BY a.l_bank_office_name_kna" & vbCrLf                    ' 支店名カナで並び替え

            ' 支店名コンボボックス作成処理呼び出し
            If CreateComboBoxNew(clsDb, _
                                 Me.cboBankOfficeName, _
                                 strSql, _
                                 "l_bank_office_name", _
                                 "c_bank_office") = False Then
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
    '   ＩＤ　：getBankKanaBankCd
    '   名称　：銀行名カナ金融機関コード取得処理
    '   概要  ：金融機関コードから銀行名カナと金融機関コードを取得する。
    '   引数　：ByVal clsDb     As CLAccessMdb = データベースクラス
    '           ByVal strBankCd As String      = 金融機関コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/17(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function getBankKanaBankCd(ByVal clsDb As CLAccessMdb, _
                                       ByVal strBankCd As String) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim intRet As Integer = 0               ' 処理件数

        Try
            ' 金融機関コード表示
            Me.lblBank.Text = strBankCd

            ' 銀行名カナ表示
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_bank_name_kna AS l_bank_name_kna" & vbCrLf           ' 銀行名
            strSql = strSql & "   FROM bank_info AS a" & vbCrLf                                 ' 銀行マスタ
            strSql = strSql & "  WHERE a.d_from <= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間From
            strSql = strSql & "    AND a.d_to   >= '" & Me.strAcntUseDate & "'" & vbCrLf        ' 期間To
            strSql = strSql & "    AND a.c_bank = '" & strBankCd & "'" & vbCrLf                 ' 金融機関コードと同じもの

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 1 Then
                Me.lblBankNameKana.Text = "( " & dtRet.Rows(0).Item(0).ToString() & " )"
            ElseIf intRet = 0 Then
                Call MessageBox.Show("銀行マスタにデータがありません！", _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Error, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            Else
                Call MessageBox.Show("銀行マスタにデータが複数あります！", _
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：getBankOfficeKanaBankOfficeCd
    '   名称　：支店名カナ支店番号取得処理
    '   概要  ：金融機関コードから銀行名カナと金融機関コードを取得する。
    '   引数　：ByVal clsDb           As CLAccessMdb = データベースクラス
    '           ByVal strBankCd       As String      = 金融機関コード
    '           ByVal strBankOfficeCd As String      = 支店番号
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/17(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function getBankOfficeKanaBankOfficeCd(ByVal clsDb As CLAccessMdb, _
                                                   ByVal strBankCd As String, _
                                                   ByVal strBankOfficeCd As String) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL
        Dim dtRet As DataTable = Nothing            ' データテーブル
        Dim intRet As Integer = 0                   ' 処理件数

        Try
            ' 支店番号表示
            Me.lblBankOffice.Text = strBankOfficeCd

            ' 支店名カナ表示
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_bank_office_name_kna AS l_bank_office_name_kna" & vbCrLf ' 支店名
            strSql = strSql & "   FROM bank_info_dtl AS a" & vbCrLf                                 ' 銀行マスタ詳細
            strSql = strSql & "  WHERE a.d_from <= '" & Me.strAcntUseDate & "'" & vbCrLf            ' 期間From
            strSql = strSql & "    AND a.d_to   >= '" & Me.strAcntUseDate & "'" & vbCrLf            ' 期間To
            strSql = strSql & "    AND a.c_bank = '" & strBankCd & "'" & vbCrLf                     ' 金融機関コードと同じもの
            strSql = strSql & "    AND a.c_bank_office = '" & strBankOfficeCd & "'" & vbCrLf        ' 支店番号と同じもの

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 1 Then
                Me.lblBankOfficeNameKana.Text = "( " & dtRet.Rows(0).Item(0).ToString() & " )"
            ElseIf intRet = 0 Then
                Call MessageBox.Show("銀行マスタ詳細にデータがありません！", _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Error, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            Else
                Call MessageBox.Show("銀行マスタ詳細にデータが複数あります！", _
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            ' Title
            If Me.bytStatus = STATUS_INSERT Then
                Me.lblTitle.Text = "組合員口座情報 - 新規登録"
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                Me.lblTitle.Text = "組合員口座情報 - 詳細"
            End If
            ' TextBox
            Me.txtStafId.Text = ""                                                          ' 社員番号
            Me.txtStafIdDezit.Text = ""                                                     ' 社員番号ディジット
            Me.txtNameKana.Text = ""                                                        ' フリガナ
            Me.txtOldStafId.Text = ""                                                       ' 旧社員番号
            Me.txtOldStafIdDezit.Text = ""                                                  ' 旧社員番号ディジット
            Me.txtStatus.Text = ""                                                          ' ステータス
            Me.txtStafKind.Text = ""                                                        ' 組合員種別
            Me.txtUseDate.Text = ""                                                         ' 適用日付
            Me.txtBankAccount.Text = ""                                                     ' 口座番号
            Me.txtAccountName.Text = ""                                                     ' 口座名義
            Me.txtAccountNameKana.Text = ""                                                 ' 口座名義（カナ）
            Me.txtRokinCir.Text = ""                                                        ' 労金CIR
            ' BackColor
            Me.txtStafId.BackColor = Color.LightYellow                                      ' 社員番号バックカラー薄黄色
            Me.txtStafIdDezit.BackColor = Color.LightYellow                                 ' 社員番号ディジットバックカラー薄黄色
            Me.txtNameKana.BackColor = Color.LightYellow                                    ' フリガナバックカラー薄黄色
            Me.txtOldStafId.BackColor = Color.LightYellow                                   ' 旧社員番号バックカラー薄黄色
            Me.txtOldStafIdDezit.BackColor = Color.LightYellow                              ' 旧社員番号ディジットバックカラー薄黄色
            Me.txtStatus.BackColor = Color.LightYellow                                      ' ステータスバックカラー薄黄色
            Me.txtStafKind.BackColor = Color.LightYellow                                    ' 組合員種別バックカラー薄黄色
            Me.txtUseDate.BackColor = Color.LightYellow                                     ' 適用日付バックカラー薄黄色
            ' ComboBoxList
            Me.cboBankName.DataSource = Nothing                                             ' 銀行名
            Me.cboBankOfficeName.DataSource = Nothing                                       ' 支店名
            Me.cboDepositItems.DataSource = Nothing                                         ' 預金種目
            ' ComboBox
            Me.cboBankName.Text = ""                                                        ' 銀行名
            Me.cboBankOfficeName.Text = ""                                                  ' 支店名
            Me.cboDepositItems.Text = ""                                                    ' 預金種目
            ' Button
            Me.btnHistory.Visible = True                                                    ' 履歴ボタン
            Me.btnBack.Visible = True                                                       ' 戻るボタン
            Me.btnUpdate.Visible = True                                                     ' 内容変更ボタン
            Me.btnCancel.Visible = True                                                     ' キャンセルボタン
            Me.btnInsertChk.Visible = True                                                  ' 登録確認ボタン
            Me.btnHistory.Enabled = True                                                    ' 履歴ボタン
            ' Label
            Me.lblBank.Text = ""                                                            ' 金融機関コード
            Me.lblBankOffice.Text = ""                                                      ' 支店番号

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
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock(ByVal pBlnEdit As Boolean) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim blnHistory As Boolean = False           ' 履歴ボタン用
        Dim blnEntry As Boolean = False             ' 新規登録ボタン用
        Dim blnUpdate As Boolean = False            ' 内容変更ボタン用
        Dim blnCancel As Boolean = False            ' キャンセルボタン用
        Dim blnBack As Boolean = False              ' 戻るボタン用
        Dim strTitle As String = ""                 ' タイトル
        Dim blnReadOnlyFlg As Boolean = False       ' ReadOnlyフラグ

        Try
            '-------------------------------------------------------------------------------
            '   表示・非表示フラグ設定
            '-------------------------------------------------------------------------------
            ' 新規登録の場合、デフォルトのFalseを設定済み
            If Me.bytStatus = STATUS_INSERT Then
                '---------------------------------------------------------------------------
                '   新規登録
                '---------------------------------------------------------------------------
                blnEntry = True                                                             ' 登録確認ボタン用
                blnCancel = True                                                            ' キャンセルボタン用
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '---------------------------------------------------------------------------
                '   内容変更
                '---------------------------------------------------------------------------
                If pBlnEdit Then
                    ' 内容変更ボタン押下後
                    blnEntry = True                                                         ' 登録確認ボタン用
                    blnCancel = True                                                        ' キャンセルボタン用
                Else
                    ' 内容変更ボタン押下前
                    blnUpdate = True                                                        ' 内容変更ボタン用
                    blnBack = True                                                          ' 戻るボタン用
                    blnHistory = True                                                       ' 履歴ボタン
                End If
            End If
            If pBlnEdit = False Then
                blnReadOnlyFlg = True
            End If

            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            ' Button
            Me.btnHistory.Enabled = blnHistory                                              ' 履歴ボタン
            Me.btnInsertChk.Visible = blnEntry                                              ' 登録確認ボタン
            Me.btnUpdate.Visible = blnUpdate                                                ' 内容変更ボタン
            If blnUpdate Then
                If Me.strGrantInsert = GRANT_VALID Then
                    Me.btnUpdate.Enabled = True                                             ' 内容変更ボタン使用可
                ElseIf Me.strGrantInsert = GRANT_VOID Then
                    Me.btnUpdate.Enabled = False                                            ' 内容変更ボタン使用不可
                End If
            End If
            Me.btnCancel.Visible = blnCancel                                                ' キャンセルボタン
            Me.btnBack.Visible = blnBack                                                    ' 戻るボタン
            ' TextBox
            Me.txtStafId.ReadOnly = True                                                    ' 社員番号
            Me.txtStafIdDezit.ReadOnly = True                                               ' 社員番号ディジット
            Me.txtNameKana.ReadOnly = True                                                  ' フリガナ
            Me.txtOldStafId.ReadOnly = True                                                 ' 旧社員番号
            Me.txtOldStafIdDezit.ReadOnly = True                                            ' 旧社員番号ディジット
            Me.txtStafName.ReadOnly = True                                                  ' 名前
            Me.txtStatus.ReadOnly = True                                                    ' ステータス
            Me.txtStafKind.ReadOnly = True                                                  ' 組合員種別
            Me.txtUseDate.ReadOnly = True                                                   ' 適用日付
            Me.txtBankAccount.ReadOnly = blnReadOnlyFlg                                     ' 口座番号
            Me.txtAccountName.ReadOnly = blnReadOnlyFlg                                     ' 口座名義
            Me.txtAccountNameKana.ReadOnly = blnReadOnlyFlg                                 ' 口座名義（カナ）
            Me.txtRokinCir.ReadOnly = blnReadOnlyFlg                                        ' 労金CIR
            ' BackColor
            Me.txtStafId.BackColor = Color.LightYellow                                      ' 社員番号
            Me.txtStafIdDezit.BackColor = Color.LightYellow                                 ' 社員番号ディジット
            Me.txtNameKana.BackColor = Color.LightYellow                                    ' フリガナ
            Me.txtOldStafId.BackColor = Color.LightYellow                                   ' 旧社員番号
            Me.txtOldStafIdDezit.BackColor = Color.LightYellow                              ' 旧社員番号ディジット
            Me.txtStafName.BackColor = Color.LightYellow                                    ' 名前
            Me.txtStatus.BackColor = Color.LightYellow                                      ' ステータス
            Me.txtStafKind.BackColor = Color.LightYellow                                    ' 組合員種別
            Me.txtUseDate.BackColor = Color.LightYellow                                     ' 適用日付
            ' ComboBox
            Call Utilities.SetCanEditToControl(pBlnEdit, Me.cboBankName)                    ' 銀行名
            Call Utilities.SetCanEditToControl(pBlnEdit, Me.cboBankOfficeName)              ' 支店名
            Call Utilities.SetCanEditToControl(pBlnEdit, Me.cboDepositItems)                ' 預金種目

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
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim arlErrMsg As New ArrayList              ' エラーメッセージリスト
        Dim clsUC999999 As UC999999 = Nothing       ' メッセージボックスクラス生成
        Dim strKanaChk As String = ""               ' カナチェック用文字列

        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   未入力・未選択・数値チェック
            '-------------------------------------------------------------------------------
            ' 銀行名
            If Me.cboBankName.SelectedIndex <= 0 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "銀行名"))
                Call SetErr(Me.cboBankName)
            End If
            ' 支店名
            If Me.cboBankOfficeName.SelectedIndex <= 0 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "支店名"))
                Call SetErr(Me.cboBankOfficeName)
            End If
            ' 預金種目
            If Me.cboDepositItems.SelectedIndex <= 0 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "銀行種目"))
                Call SetErr(Me.cboDepositItems)
            End If
            ' 口座番号
            If ChkNull(Me.txtBankAccount.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "口座番号"))
                Call SetErr(Me.txtBankAccount)
            Else
                ' 数値チェック
                If ChkNumber(Me.txtBankAccount.Text.Trim) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "口座番号"))
                    Call SetErr(Me.txtBankAccount)
                End If
            End If
            ' 口座名義
            If ChkNull(Me.txtAccountName.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "口座名義"))
                Call SetErr(Me.txtAccountName)
            End If
            ' 口座名義（カナ）
            If ChkNull(Me.txtAccountNameKana.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "口座名義（カナ）"))
                Call SetErr(Me.txtAccountNameKana)
            Else
                ' 半角カナチェック
                If ChkHankakuKana(Me.txtAccountNameKana.Text.Trim()) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "口座名義（カナ）"))
                    Call SetErr(Me.txtAccountNameKana)
                End If
            End If
            ' 労金CIR
            If Me.txtRokinCir.Text.Trim().Length <> 0 Then
                ' 数値チェック
                If ChkNumber(Me.txtRokinCir.Text.Trim()) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "労金CIR"))
                    Call SetErr(Me.txtRokinCir)
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
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成

        Try
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   コンボボックス作成
            '-------------------------------------------------------------------------------
            ' 銀行名コンボボックス作成処理呼び出し
            If CreateComboBoxBank(clsDb) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（預金種目）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboDepositItems, _
                                    CONSTANT_ID_DEPOSIT_ITEMS) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   組合員属性情報取得
            '-------------------------------------------------------------------------------
            If GetStafAttribute(clsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   組合員口座情報取得
            '-------------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT Then
                ' 新規登録
                Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strAcntUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")

            ElseIf Me.bytStatus = STATUS_UPDATE Then
                ' 内容変更
                If GetStafAccount(clsDb, 1) = False Then
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
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetStafAttribute
    '   名称　：組合員属性情報取得処理
    '   概要  ：組合員属性情報をを取得する。
    '   引数　：ByVal pClsDb        As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員属性情報取得処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStafAttribute(ByVal pClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable                      ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing          ' 処理件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_staf_id     AS c_staf_id" & vbCrLf               ' 01. 社員番号
            strSql = strSql & "       ,a.c_dezit       AS c_dezit" & vbCrLf                 ' 02. ディジット
            strSql = strSql & "       ,a.l_name_kna    AS l_name_kna" & vbCrLf              ' 05. 名前カナ
            strSql = strSql & "       ,a.c_staf_id_old AS c_staf_id_old" & vbCrLf           ' 03. 旧社員番号
            strSql = strSql & "       ,a.c_dezit_old   AS c_dezit_old" & vbCrLf             ' 04. 旧ディジット
            strSql = strSql & "       ,a.l_name        AS l_name" & vbCrLf                  ' 06. 名前
            strSql = strSql & "       ,ctd1.l_name     AS k_user_status" & vbCrLf           ' 07. 組合員ステータス区分
            strSql = strSql & "       ,ctd2.l_name     AS k_staf_kind" & vbCrLf             ' 08. 組合員種別コード
            strSql = strSql & "   FROM staf_attribute AS a" & vbCrLf                        ' 組合員情報テーブル
            strSql = strSql & "       ,constant_dtl AS ctd1" & vbCrLf                       ' 定数マスタ詳細1
            strSql = strSql & "       ,constant_dtl AS ctd2" & vbCrLf                       ' 定数マスタ詳細2
            strSql = strSql & "  WHERE a.c_user_id = '" & Me.strAttrUserId & "'" & vbCrLf   ' 個人認証IDと同じもの
            strSql = strSql & "    AND a.c_ksh     = '" & Me.strAttrKsh & "'" & vbCrLf      ' 会社コードと同じもの
            strSql = strSql & "    AND a.c_staf_id = '" & Me.strAttrStafId & "'" & vbCrLf   ' 社員番号と同じもの
            strSql = strSql & "    AND a.d_from    = '" & Me.strAttrUseDate & "'" & vbCrLf  ' 適用日付と同じもの
            strSql = strSql & "    AND ctd1.c_constant = 'USER_STATUS'" & vbCrLf
            strSql = strSql & "    AND ctd1.c_constant_seq = a.k_user_status" & vbCrLf
            strSql = strSql & "    AND ctd2.c_constant = 'STAF_KIND'" & vbCrLf
            strSql = strSql & "    AND ctd2.c_constant_seq = a.k_staf_kind" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = pClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intCntRet = tbRet.Rows.Count

            ' 各データ設定
            If intCntRet = 1 Then
                With tbRet.Rows(0)
                    Me.txtStafId.Text = NVL(.Item(0).ToString())                                    ' 01. 社員番号
                    Me.txtStafIdDezit.Text = NVL(.Item(1).ToString())                               ' 02. 社員番号ディジット
                    Me.txtNameKana.Text = NVL(.Item(2).ToString())                                  ' 03. フリガナ
                    Me.txtOldStafId.Text = NVL(.Item(3).ToString())                                 ' 04. 旧社員番号
                    Me.txtOldStafIdDezit.Text = NVL(.Item(4).ToString())                            ' 05. 旧社員番号ディジット
                    Me.txtStafName.Text = NVL(.Item(5).ToString())                                  ' 06. 名前
                    Me.txtStatus.Text = NVL(.Item(6).ToString())                                    ' 07. ステータス
                    Me.txtStafKind.Text = NVL(.Item(7).ToString())                                  ' 08. 組合員種別

                    ' 口座名義の初期値設定(内容変更時は口座情報の値で後に上書かれる)
                    Me.txtAccountNameKana.Text = NVL(.Item(2).ToString())                           ' 口座名義（カナ）
                    Me.txtAccountName.Text = NVL(.Item(5).ToString())                               ' 口座名義
                End With
            Else
                Call MessageBox.Show("データがありません！", _
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetStafAccount
    '   名称　：組合員口座情報取得処理
    '   概要  ：組合員口座情報をを取得する。
    '   引数　：ByVal pClsDb        As CLAccessMdb = データベースクラス
    '           ByVal pBytReference As Byte        = ステータス（1：通常検索, 2：最新検索（履歴ボタン押下後の適用日付選択画面で直接入力からの検索）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員口座情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="pBytReference">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStafAccount(ByVal iClsDb As CLAccessMdb, _
                                    ByVal pBytReference As Byte) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable                      ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing          ' 処理件数

        Try
            ' SQL文作成
            If pBytReference = 1 Then
                ' 通常検索
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT a.d_from             AS d_from" & vbCrLf                     ' 01. 適用日付
                strSql = strSql & "       ,a.c_bank             AS c_bank" & vbCrLf                     ' 02. 金融機関コード
                strSql = strSql & "       ,a.c_bank_office      AS c_bank_office" & vbCrLf              ' 03. 支店番号
                strSql = strSql & "       ,a.k_deposit_items    AS k_deposit_items" & vbCrLf            ' 04. 預金種目
                strSql = strSql & "       ,a.c_bank_account     AS c_bank_account" & vbCrLf             ' 05. 口座番号
                strSql = strSql & "       ,a.l_account_name     AS l_account_name" & vbCrLf             ' 06. 口座名義
                strSql = strSql & "       ,a.l_account_name_kna AS l_account_name_kna" & vbCrLf         ' 07. 口座名義カナ
                strSql = strSql & "       ,a.c_rokin_cir        AS c_rokin_cir" & vbCrLf                ' 08. 労金CIR
                strSql = strSql & "   FROM staf_account AS a" & vbCrLf                                  ' 組合員口座情報
                strSql = strSql & "  WHERE a.c_user_id = '" & Me.strAcntUserId & "'" & vbCrLf           ' 個人認証IDと同じもの
                strSql = strSql & "    AND a.d_from = '" & Me.strAcntUseDate & "'" & vbCrLf             ' 適用日付と同じもの
                strSql = strSql & "    AND a.c_bank = '" & Me.strAcntBank & "'" & vbCrLf                ' 金融機関コードと同じもの
                strSql = strSql & "    AND a.c_bank_office = '" & Me.strAcntBankOffice & "'" & vbCrLf   ' 支店番号と同じもの
                strSql = strSql & ";" & vbCrLf
            ElseIf pBytReference = 2 Then
                ' 最新の組合員口座情報取得
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT a.d_from             AS d_from" & vbCrLf                     ' 01. 適用日付
                strSql = strSql & "       ,a.c_bank             AS c_bank" & vbCrLf                     ' 02. 金融機関コード
                strSql = strSql & "       ,a.c_bank_office      AS c_bank_office" & vbCrLf              ' 03. 支店番号
                strSql = strSql & "       ,a.k_deposit_items    AS k_deposit_items" & vbCrLf            ' 04. 預金種目
                strSql = strSql & "       ,a.c_bank_account     AS c_bank_account" & vbCrLf             ' 05. 口座番号
                strSql = strSql & "       ,a.l_account_name     AS l_account_name" & vbCrLf             ' 06. 口座名義
                strSql = strSql & "       ,a.l_account_name_kna AS l_account_name_kna" & vbCrLf         ' 07. 口座名義カナ
                strSql = strSql & "       ,a.c_rokin_cir        AS c_rokin_cir" & vbCrLf                ' 08. 労金CIR
                strSql = strSql & "   FROM staf_account AS a" & vbCrLf                                  ' 組合員口座情報
                strSql = strSql & "       ,( SELECT MAX(b.d_from) AS d_from" & vbCrLf
                strSql = strSql & "            FROM staf_account AS b" & vbCrLf
                strSql = strSql & "           WHERE b.c_user_id = '" & Me.strAcntUserId & "'" & vbCrLf
                strSql = strSql & "        ) AS staf_max" & vbCrLf
                strSql = strSql & "  WHERE a.c_user_id = '" & Me.strAcntUserId & "'" & vbCrLf           ' 個人認証IDと同じもの
                strSql = strSql & "    AND a.d_from = staf_max.d_from" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            End If

            ' SQL実行
            tbRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intCntRet = tbRet.Rows.Count

            ' 各データ設定
            If intCntRet = 0 Then
                ' 01. 適用日付
                Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strAcntUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")
                Me.cboBankName.SelectedIndex = 0                                                    ' 02. 銀行名
                ' 03. 支店名
                If Me.cboBankOfficeName.SelectedIndex >= 0 Then
                    Me.cboBankOfficeName.SelectedIndex = 0
                End If
                Me.cboDepositItems.SelectedIndex = 0                                                ' 04. 預金種目
                Me.txtBankAccount.Text = ""                                                         ' 05. 口座番号
                Me.txtAccountName.Text = ""                                                         ' 06. 口座名義
                Me.txtAccountNameKana.Text = ""                                                     ' 07. 口座名義（カナ）
                Me.txtRokinCir.Text = ""                                                            ' 08. 労金CIR
            ElseIf intCntRet = 1 Then
                With tbRet.Rows(0)
                    ' 01. 適用日付
                    If pBytReference = 1 Then
                        Me.txtUseDate.Text = Date.Parse(Format(CInt(.Item(0).ToString()), "0000/00/00")).ToString("yyyy年MM月dd日")
                    ElseIf pBytReference = 2 Then
                        Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strAcntUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")
                    End If
                    Me.cboBankName.SelectedValue = NVL(.Item(1)).ToString()                         ' 02. 銀行名
                    Me.cboBankOfficeName.SelectedValue = NVL(.Item(2)).ToString()                   ' 03. 支店名
                    Me.cboDepositItems.SelectedValue = NVL(.Item(3)).ToString()                     ' 04. 預金種目
                    Me.txtBankAccount.Text = NVL(.Item(4)).ToString()                               ' 05. 口座番号
                    Me.txtAccountName.Text = NVL(.Item(5)).ToString()                               ' 06. 口座名義
                    Me.txtAccountNameKana.Text = NVL(.Item(6)).ToString()                           ' 07. 口座名義（カナ）
                    Me.txtRokinCir.Text = NVL(.Item(7)).ToString()                                  ' 08. 労金CIR
                End With
            Else
                Call MessageBox.Show("データがありません！", _
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUpdate
    '   名称　：基本情報更新処理（登録・更新）
    '   概要  ：基本更新処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/19(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>登録更新処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdate() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' トランザクション開始処理
            Call clsDb.BeginTran()

            If bytHistoryFlg = 0 Then
                ' 通常登録の場合
                If Me.bytStatus = STATUS_INSERT Then                                        ' ステータス判定
                    '-----------------------------------------------------------------------
                    '   組合員口座情報存在チェック処理
                    '-----------------------------------------------------------------------
                    If ExistsStafAccount(clsDb) = False Then
                        '-------------------------------------------------------------------
                        '   組合員口座情報登録処理
                        '-------------------------------------------------------------------
                        If InsertStafAccount(clsDb) = False Then
                            Call CLMsg.Show("FE0001")                                       ' 予期しないエラーメッセージ表示
                            Call clsDb.RollbackTran()                                       ' トランザクション取消処理
                            Return blnRet                                                   ' 処理を抜ける
                        End If
                    Else
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("BE0005")                                           ' 既に他の方によって変更されていますの旨のメッセージ表示
                        Return blnRet                                                       ' 処理を抜ける
                    End If

                ElseIf Me.bytStatus = STATUS_UPDATE Then
                    '-----------------------------------------------------------------------
                    '   組合員口座情報存在チェック処理
                    '-----------------------------------------------------------------------
                    If ExistsStafAccount(clsDb) Then
                        '-------------------------------------------------------------------
                        '   組合員口座情報更新処理
                        '-------------------------------------------------------------------
                        ' 組合員口座情報が存在していた場合、更新処理
                        If UpdateStafAccount(clsDb) = False Then
                            Call clsDb.RollbackTran()                                       ' トランザクション取消処理
                            Call CLMsg.Show("DE0005")                                       ' 正しくデータが更新できませんでしたの旨のメッセージ表示
                            Return blnRet                                                   ' 処理を抜ける
                        End If
                    Else
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("GE0052")                                           ' 他のユーザによって更新された可能性の旨のメッセージ表示
                        Return blnRet                                                       ' 処理を抜ける
                    End If
                End If
            Else
                ' 内容変更（直接入力）新規登録の場合
                '---------------------------------------------------------------------------
                '   基本情報存在チェック処理
                '---------------------------------------------------------------------------
                If ExistsStafAccount(clsDb) = False Then
                    '-----------------------------------------------------------------------
                    '   組合員口座情報登録処理
                    '-----------------------------------------------------------------------
                    ' 組合員口座情報が存在しない場合、登録処理
                    If InsertStafAccount(clsDb) = False Then
                        Call CLMsg.Show("FE0001")                                           ' 予期しないエラーメッセージ表示
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Return blnRet                                                       ' 処理を抜ける
                    End If
                Else
                    Call clsDb.RollbackTran()                                               ' トランザクション取消処理
                    Call CLMsg.Show("GE0128", "データ")                                     ' 既に同じデータが存在します。の旨のメッセージ表示
                    Return blnRet                                                           ' 処理を抜ける
                End If
            End If

            ' トランザクション確定処理
            Call clsDb.CommitTran()

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
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
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertStafAccount
    '   名称　：組合員口座情報登録処理
    '   概要  ：
    '   引数　：ByVal clsDb As CLAccessMdb
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員口座情報登録処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafAccount(ByVal clsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim intRet As Integer = 0                   ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合員口座情報登録
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO staf_account ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                      ' 01. 個人認証ID
            strSql = strSql & "   ,d_from" & vbCrLf                         ' 02. 適用開始年月日
            strSql = strSql & "   ,c_bank" & vbCrLf                         ' 03. 金融機関コード
            strSql = strSql & "   ,c_bank_office" & vbCrLf                  ' 04. 支店番号
            strSql = strSql & "   ,k_deposit_items" & vbCrLf                ' 05. 預金種目
            strSql = strSql & "   ,c_bank_account" & vbCrLf                 ' 06. 口座番号
            strSql = strSql & "   ,l_account_name" & vbCrLf                 ' 07. 口座名義
            strSql = strSql & "   ,l_account_name_kna" & vbCrLf             ' 08. 口座名義カナ
            strSql = strSql & "   ,c_rokin_cir" & vbCrLf                    ' 09. 労金CIR
            strSql = strSql & "   ,l_biko" & vbCrLf                         ' 10. 備考
            strSql = strSql & "   ,d_ins" & vbCrLf                          ' 11. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                  ' 12. 作成者個人ID
            strSql = strSql & " ) VALUES ( " & vbCrLf
            ' 01. 個人認証ID
            strSql = strSql & "     '" & Me.txtStafId.Text.Trim() & "'" & vbCrLf
            ' 02. 適用開始年月日
            strSql = strSql & "    ,'" & Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "") & "'" & vbCrLf
            ' 03. 金融機関コード
            strSql = strSql & "    ,'" & Me.cboBankName.SelectedValue.ToString() & "'" & vbCrLf
            ' 04. 支店番号
            strSql = strSql & "    ,'" & Me.cboBankOfficeName.SelectedValue.ToString() & "'" & vbCrLf
            ' 05. 預金種目
            strSql = strSql & "    ,'" & Me.cboDepositItems.SelectedValue.ToString() & "'" & vbCrLf
            ' 06. 口座番号
            strSql = strSql & "    ,'" & Me.txtBankAccount.Text.Trim() & "'" & vbCrLf
            ' 07. 口座名義
            strSql = strSql & "    ,'" & Me.txtAccountName.Text.Trim() & "'" & vbCrLf
            ' 08. 口座名義カナ
            strSql = strSql & "    ,'" & Me.txtAccountNameKana.Text.Trim() & "'" & vbCrLf
            ' 09. 労金CIR
            strSql = strSql & "    ,'" & Me.txtRokinCir.Text.Trim() & "'" & vbCrLf
            ' 10. 備考
            strSql = strSql & "    ,''" & vbCrLf
            ' 11. 作成日
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf
            ' 12. 作成者個人ID
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & " );" & vbCrLf

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            ' 処理件数判定
            If intRet <> 1 Then
                Call MessageBox.Show("組合員口座情報を登録できませんでした。", _
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
    '   ＩＤ　：UpdateStafAccoun
    '   名称　：組合員口座情報更新処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員口座情報更新処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateStafAccount(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim intRet As Integer = 0               ' 処理件数

        Try
            '-----------------------------------------------------------------------------------
            '   組合員口座情報更新
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE staf_account" & vbCrLf
            ' 03. 金融機関コード
            strSql = strSql & "    SET c_bank = '" & Me.cboBankName.SelectedValue.ToString() & "'" & vbCrLf
            ' 04. 支店番号
            strSql = strSql & "       ,c_bank_office = '" & Me.cboBankOfficeName.SelectedValue.ToString() & "'" & vbCrLf
            ' 05. 預金種目
            strSql = strSql & "       ,k_deposit_items = '" & Me.cboDepositItems.SelectedValue.ToString() & "'" & vbCrLf
            ' 06. 口座番号
            strSql = strSql & "       ,c_bank_account = '" & Me.txtBankAccount.Text.Trim() & "'" & vbCrLf
            ' 07. 口座名義
            strSql = strSql & "       ,l_account_name = '" & Me.txtAccountName.Text.Trim() & "'" & vbCrLf
            ' 08. 口座名義カナ
            strSql = strSql & "       ,l_account_name_kna = '" & Me.txtAccountNameKana.Text.Trim() & "'" & vbCrLf
            ' 09. 労金CIR
            strSql = strSql & "       ,c_rokin_cir = '" & Me.txtRokinCir.Text.Trim() & "'" & vbCrLf
            ' 10. 備考
            strSql = strSql & "       ,l_biko = ''" & vbCrLf
            ' 11. 作成日
            strSql = strSql & "       ,d_ins = '" & Now() & "'" & vbCrLf
            ' 12. 作成者個人ID
            strSql = strSql & "       ,c_user_id_ins = '" & MDLoginInfo.UserId & "'" & vbCrLf
            ' 個人認証IDと同じもの
            strSql = strSql & "  WHERE c_user_id = '" & Me.strAcntUserId & "'" & vbCrLf
            ' 適用年月日と同じもの
            strSql = strSql & "    AND d_from = '" & Me.strAcntUseDate & "'" & vbCrLf
            ' 金融機関コードと同じもの
            strSql = strSql & "    AND c_bank = '" & Me.strAcntBank & "'" & vbCrLf
            ' 支店番号と同じもの
            strSql = strSql & "    AND c_bank_office = '" & Me.strAcntBankOffice & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：ExistsStafAccount
    '   名称　：組合員口座情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal pClsDb         As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/19(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員口座情報存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAccount(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim intRet As Integer = 0                   ' 処理件数
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "       ,a.d_from" & vbCrLf
            strSql = strSql & "       ,a.c_bank" & vbCrLf
            strSql = strSql & "       ,a.c_bank_office" & vbCrLf
            strSql = strSql & "   FROM staf_account AS a" & vbCrLf                                  ' 組合員口座情報
            strSql = strSql & "  WHERE a.c_user_id = '" & Me.strAcntUserId & "'" & vbCrLf           ' 個人認証IDと同じもの
            strSql = strSql & "    AND a.d_from = '" & Me.strAcntUseDate & "'" & vbCrLf             ' 適用日付と同じもの
            strSql = strSql & "    AND a.c_bank = '" & Me.strAcntBank & "'" & vbCrLf                ' 金融機関コードと同じもの
            strSql = strSql & "    AND a.c_bank_office = '" & Me.strAcntBankOffice & "'" & vbCrLf   ' 支店番号と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)

            ' 処理件数取得
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
    '   ＩＤ　：setGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim dtGrant As DataTable = Nothing                  ' 権限取得データテーブル

        Try
            dtGrant = getGrant(MENU_ID_UC080201)                                        ' 権限取得
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString                    ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString                       ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                        ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString                   ' ファイル出力権限
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
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理（組合員口座情報 - 検索画面）
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal iBlnSearchFlg As Boolean = True：再検索有り, False：再検索無し
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/18(水) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <param name="iBlnSearchFlg">再検索フラグ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal iBlnSearchFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False                                   ' 処理結果
        Dim pn As Panel = ParentForm.Controls(MDConst.MAIN_PANEL_ID)    ' メインパネル
        Dim uc As Control = Nothing                                     ' ユーザコントロール
        Dim clsUC080201 As New UC080201                                 ' 組合員口座情報 - 検索画面

        Try
            Cursor.Current = Cursors.WaitCursor                         ' カーソルを砂時計に設定
            uc = pn.Controls(SCREEN_ID_UC080201)                        ' 組合員口座情報 - 検索画面
            If uc Is Nothing Then
                uc = New UC080201                                       ' 組合員口座情報 - 検索画面生成
                Call pn.Controls.Add(uc)                                ' メインパネルに組合員口座情報 - 検索画面追加
            Else
                clsUC080201 = pn.Controls(SCREEN_ID_UC080201)           ' 組合員口座情報 - 検索画面
                clsUC080201.blnSearchFlg = iBlnSearchFlg                ' 再検索フラグ設定
                uc.Visible = True                                       ' 組合員口座情報 - 検索画面表示
            End If
            Me.Dispose()                                                ' 組合員口座情報 - 詳細画面閉じる

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
#End Region

End Class

#End Region
