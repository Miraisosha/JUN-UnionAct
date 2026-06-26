#Region "UC010102"
'===========================================================================================================
'   クラスＩＤ　　：UC010102
'   クラス名称　　：組合員管理 - 基本情報
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDChk
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLEncrypt

Public Class UC010102

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面種別
    Private Const SCREEN_HISTORY As Byte = 0                            ' 適用日付選択画面
    Private Const SCREEN_SEARCH As Byte = 1                             ' 組合員検索画面
    Private Const SCREEN_ADDRESS As Byte = 2                            ' 組合員管理 - 住所情報画面
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                             ' 新規登録
    Private Const STATUS_UPDATE As Byte = 2                             ' 内容変更
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC010102              ' UC010102
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC010102          ' 組合員管理 - 基本情報画面
    ' 子画面用プロパティ（履歴ボタン押下時）
    ReadOnly UseDateColName As String() = {"適用日付", "担当者"}        ' 適用日付選択画面カラム名
    ReadOnly UseDateColWidth As Integer() = {150, 200}                  ' 適用日付選択画面カラム幅
    ReadOnly UseDateColShow As Boolean() = {True, True}                 ' 適用日付選択画面カラム表示
    ' 権限
    Private strGrantReference As String = "0"                           ' 参権限照
    Private strGrantInsert As String = "0"                              ' 登録権限
    Private strGrantPrint As String = "0"                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                          ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _bytStatus As Byte = 0                                       ' ステータス（1：新規登録, 2：内容変更）
    Public _bytHistoryFlg As Byte = 0                                   ' 履歴新規登録フラグ（0：通常新規登録1：履歴新規登録）
    Public _strUserId As String = ""                                    ' 個人認証ID
    Public _strKsh As String = ""                                       ' 会社コード
    Public _strStafId As String = ""                                    ' 社員番号
    Public _strUseDate As String = ""                                   ' 適用日付
    Public _strPreScreenId As String = ""                               ' 呼び元画面ID
    ' ステータス
    Public Property bytStatus() As Byte
        Get
            Return _bytStatus
        End Get
        Set(ByVal value As Byte)
            _bytStatus = value
        End Set
    End Property
    ' 履歴新規登録フラグ（0：履歴新規登録以外, 1：履歴新規登録）
    Public Property bytHistoryFlg() As Byte
        Get
            Return _bytHistoryFlg
        End Get
        Set(ByVal value As Byte)
            _bytHistoryFlg = value
        End Set
    End Property
    ' 個人認証ID
    Public Property strUserId() As String
        Get
            Return _strUserId
        End Get
        Set(ByVal value As String)
            _strUserId = value
        End Set
    End Property
    ' 会社コード
    Public Property strKsh() As String
        Get
            Return _strKsh
        End Get
        Set(ByVal value As String)
            _strKsh = value
        End Set
    End Property
    ' 社員番号
    Public Property strStafId() As String
        Get
            Return _strStafId
        End Get
        Set(ByVal value As String)
            _strStafId = value
        End Set
    End Property
    ' 適用日付
    Public Property strUseDate() As String
        Get
            Return _strUseDate
        End Get
        Set(ByVal value As String)
            _strUseDate = value
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
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC010102_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC010102_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
                '===========================================================================
                '   新規登録
                '===========================================================================
                If ControlRockUnLock(True) = False Then
                    Exit Sub
                End If
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '===========================================================================
                '   内容変更
                '===========================================================================
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
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim clsFM010104 As FM010104 = Nothing               ' 適用日付選択画面
        Dim strSql As String = ""                           ' SQL文
        Dim clsDb As New CLAccessMdb                        ' データベースクラス
        Dim bytRefStatus As Byte = 0                        ' ステータス（1：通常検索,2：最新検索）

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '-------------------------------------------------------------------------------
            '   適用日付選択画面
            '-------------------------------------------------------------------------------
            clsFM010104 = New FM010104                                                      ' インスタンス作成
            clsFM010104.Text = "基本情報履歴 - 適用日付選択画面"                            ' タイトル設定
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf   ' 適用開始年月日
            strSql = strSql & "       ,staf_max.l_name AS l_name" & vbCrLf                              ' 最新の作成者の名前
            strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf
            ' 最新の作成者の基本情報取得
            strSql = strSql & "        LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                          ,b.l_name" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                          ,( SELECT a.c_user_id" & vbCrLf
            'strSql = strSql & "                                   ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                   ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                              GROUP BY a.c_user_id" & vbCrLf
            'strSql = strSql & "                                      ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                      ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                     WHERE b.c_user_id = c.c_user_id" & vbCrLf
            'strSql = strSql & "                       AND b.c_ksh = c.c_ksh" & vbCrLf
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
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf
            'strSql = strSql & "    AND staf.c_ksh = '" & Me.strKsh & "'" & vbCrLf
            strSql = strSql & "    AND staf.c_staf_id = '" & Me.strStafId & "'" & vbCrLf
            strSql = strSql & "  ORDER BY staf.d_from DESC" & UtDb.DbOrderOffset & vbCrLf   'ok
            strSql = strSql & ";" & vbCrLf


            ' プロパティ設定
            clsFM010104.strSqlSentence = strSql
            clsFM010104.SetCulumnsName = UseDateColName             ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = UseDateColWidth           ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = UseDateColShow             ' 適用日付選択画面カラム表示有無
            clsFM010104.EnableChkDirectSpecify = True               ' 内容変更ボタン押下時、直接入力使用可能
            clsFM010104.chkDirectSpecify.Checked = True             ' 直接指定チェック

            ' 適用日付選択画面表示
            Call clsFM010104.ShowDialog()

            ' クリックされたボタンをチェック
            If clsFM010104.IntQlickBtnFlag = 0 Then
                ' OKボタン押下
                If clsFM010104.chkDirectSpecify.Checked Then
                    ' 直接指定して選択から適用日付取得
                    Me.strUseDate = clsFM010104.dtpSpecifyTime.Value.ToString("yyyyMMdd")
                    ' 履歴新規登録フラグを 1 にする
                    Me.bytHistoryFlg = 1
                    ' 2：最新検索
                    bytRefStatus = 2
                Else
                    ' 選択行から適用日付取得
                    Me.strUseDate = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                    ' 履歴新規登録フラグを 0 にする
                    Me.bytHistoryFlg = 0
                    ' 1：通常検索
                    bytRefStatus = 1
                End If
                ' データベース接続
                Call clsDb.Connect()
                ' 基本情報取得処理
                If GetStafAttribute(clsDb, bytRefStatus) = False Then
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：戻るボタンクリック処理
    '   概要　：
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理
            If TransitionScreen(SCREEN_SEARCH) = False Then
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
    '   ＩＤ　：btnInsertChk_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要  ：登録確認処理を行う。
    '   作成日：2011/11/14(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertChk.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim daiRet As DialogResult = Nothing            ' 確認メッセージ結果

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
                Exit Sub                ' 「いいえ」押下時、処理を抜ける
            End If

            '-------------------------------------------------------------------------------
            '   基本情報更新処理（登録・更新）
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
            ' ステータス判定
            If Me.bytStatus = STATUS_INSERT Then
                ' 画面遷移処理（組合員管理 - 住所情報（新規登録））
                If TransitionScreenAddress(STATUS_INSERT) = False Then
                    Exit Sub
                End If
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                ' 画面遷移処理（組合員検索）
                If TransitionScreen(True) = False Then
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要  ：キャンセル処理を行う。
    '   作成日：2011/11/14(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 入力・変更内容破棄メッセージボックス表示
            If CLMsg.Show("GQ0007") = DialogResult.No Then
                ' 「いいえ」ボタン押下時、処理を抜ける
                Exit Sub
            End If

            ' 検索画面遷移
            If TransitionScreen(False) Then
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
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnAddress_Click
    '   名称　：住所情報照会ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddress.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理（組合員管理 - 住所情報（内容変更））
            If TransitionScreenAddress(STATUS_UPDATE) = False Then
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
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnHistory_Click
    '   名称　：履歴ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click

        Dim clsFM010104 As FM010104 = Nothing           ' 適用日付選択画面
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '-------------------------------------------------------------------------------
            '   適用日付選択画面
            '-------------------------------------------------------------------------------
            clsFM010104 = New FM010104                                                      ' インスタンス作成
            clsFM010104.Text = "基本情報履歴 - 適用日付選択画面"                            ' タイトル設定
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf   ' 適用開始年月日
            strSql = strSql & "       ,staf_max.l_name AS l_name" & vbCrLf                              ' 最新の作成者の名前
            strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf
            ' 最新の作成者の基本情報取得
            strSql = strSql & "        LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                          ,b.l_name" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                          ,( SELECT a.c_user_id" & vbCrLf
            'strSql = strSql & "                                   ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                   ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                   ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                               FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                              GROUP BY a.c_user_id" & vbCrLf
            'strSql = strSql & "                                      ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                      ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                     WHERE b.c_user_id = c.c_user_id" & vbCrLf
            'strSql = strSql & "                       AND b.c_ksh = c.c_ksh" & vbCrLf
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
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf
            'strSql = strSql & "    AND staf.c_ksh = '" & Me.strKsh & "'" & vbCrLf
            strSql = strSql & "    AND staf.c_staf_id = '" & Me.strStafId & "'" & vbCrLf
            strSql = strSql & "  ORDER BY staf.d_from DESC" & UtDb.DbOrderOffset & vbCrLf
            strSql = strSql & ";" & vbCrLf
            'todo:
            ' プロパティ設定
            clsFM010104.strSqlSentence = strSql
            clsFM010104.SetCulumnsName = UseDateColName             ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = UseDateColWidth           ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = UseDateColShow             ' 適用日付選択画面カラム表示有無
            clsFM010104.EnableChkDirectSpecify = False              ' 直接指定使用不可
            clsFM010104.chkDirectSpecify.Checked = False            ' 直接指定チェックなし

            ' 適用日付選択画面表示
            Call clsFM010104.ShowDialog()
            ' クリックされたボタンをチェック
            If clsFM010104.IntQlickBtnFlag = 0 Then
                ' 選択行からのみ（直接指定からの遷移はない）
                ' 選択行から適用日付取得
                Me.strUseDate = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                ' 履歴新規登録フラグを 0 にする
                Me.bytHistoryFlg = 0
                ' データベース接続
                Call clsDb.Connect()
                ' 基本情報取得処理
                If GetStafAttribute(clsDb, 1) = False Then
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
    '   ＩＤ　：mtbPilotDate_TextChanged
    '   名称　：加入年月日テキストチェンジ処理
    '   概要  ：
    '   作成日：2011/12/22(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub mtbEntryDay_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbEntryDay.TextChanged

        Try
            ' 加入年月日経過年数取得
            If Me.mtbEntryDay.ValidateText() Is Nothing Then
                Me.lblEntryDayAge.Text = ""
            Else
                Me.lblEntryDayAge.Text = CalPassedYears(Me.mtbEntryDay)
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
    '   ＩＤ　：mtbBirthDay_TextChanged
    '   名称　：生年月日テキストチェンジ処理
    '   概要  ：
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub mtbBirthDay_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbBirthDay.TextChanged

        Dim intAge As Integer = 0           ' 年齢

        Try
            ' 年齢取得
            If Me.mtbBirthDay.ValidateText() Is Nothing Then
                Me.lblBirthDayAge.Text = ""
            Else
                ' 年齢取得処理
                If GetAge(Me.mtbBirthDay, intAge) = False Then
                    Exit Sub
                End If
                Me.lblBirthDayAge.Text = "（満" & intAge.ToString() & "歳）"
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
    '   ＩＤ　：mtbEntryCoDate_TextChanged
    '   名称　：入社年月日テキストチェンジ処理
    '   概要  ：
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub mtbEntryCoDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbEntryCoDate.TextChanged

        Try
            ' 入社年月日経過年数取得
            If Me.mtbEntryCoDate.ValidateText() Is Nothing Then
                Me.lblEntryCoDateAge.Text = ""
            Else
                Me.lblEntryCoDateAge.Text = CalPassedYears(Me.mtbEntryCoDate)
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
    '   ＩＤ　：mtbPilotDate_TextChanged
    '   名称　：機長年月日テキストチェンジ処理
    '   概要　：
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub mtbPilotDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbPilotDate.TextChanged

        Try
            ' 機長年月日経過年数取得
            If Me.mtbPilotDate.ValidateText() Is Nothing Then
                Me.lblPilotDateAge.Text = ""
            Else
                Me.lblPilotDateAge.Text = CalPassedYears(Me.mtbPilotDate)
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
    '   ＩＤ　：mtbTeacherPilotDate_TextChanged
    '   名称　：教官機長年月日テキストチェンジ処理
    '   概要　：
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub mtbTeacherPilotDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbTeacherPilotDate.TextChanged

        Try
            ' 教官機長年月日経過年数取得
            If Me.mtbTeacherPilotDate.ValidateText() Is Nothing Then
                Me.lblTeacherPilotDateAge.Text = ""
            Else
                Me.lblTeacherPilotDateAge.Text = CalPassedYears(Me.mtbTeacherPilotDate)
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
    '   ＩＤ　：cboStatus_SelectedIndexChanged
    '   名称　：ステータスチェンジ処理
    '   概要　：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboStatus.SelectedIndexChanged

        Try
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_POSITION_LOSS Then
                    ' 地位喪失
                    Me.mtbPositionLossDate.Enabled = True
                    Me.cboLossReason.Enabled = True
                    Me.mtbWithdrawDate.Enabled = False
                    Me.txtWithdrawReason.Enabled = False
                ElseIf Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' 脱退
                    Me.mtbPositionLossDate.Enabled = False
                    Me.cboLossReason.Enabled = False
                    Me.mtbWithdrawDate.Enabled = True
                    Me.txtWithdrawReason.Enabled = True
                Else
                    ' その他
                    Me.mtbPositionLossDate.Enabled = False
                    Me.cboLossReason.Enabled = False
                    Me.mtbWithdrawDate.Enabled = False
                    Me.txtWithdrawReason.Enabled = False
                End If
            Else
                Me.mtbPositionLossDate.Enabled = False
                Me.cboLossReason.Enabled = False
                Me.mtbWithdrawDate.Enabled = False
                Me.txtWithdrawReason.Enabled = False
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
    '   ＩＤ　：cboUserKind_SelectedIndexChanged
    '   名称　：組合員種別チェンジ処理
    '   概要　：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboUserKind_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUserKind.SelectedIndexChanged

        Try
            If Me.cboUserKind.SelectedIndex >= 0 Then

                ' 旧社員番号、旧社員番号ディジット表示・非表示
                If Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR Then
                    ' 組合員種別がシニア組合員の場合、旧社員番号と旧社員番号ディジット入力可能
                    Me.txtOldMemberNo.Enabled = True
                    Me.txtOldMemberNoDezit.Enabled = True
                    Me.txtOldMemberNo.BackColor = Color.White
                    Me.txtOldMemberNoDezit.BackColor = Color.White
                Else
                    ' 組合員種別がシニア組合員の場合、旧社員番号と旧社員番号ディジット入力不可
                    Me.txtOldMemberNo.Enabled = False
                    Me.txtOldMemberNoDezit.Enabled = False
                    Me.txtOldMemberNo.BackColor = Color.LightYellow
                    Me.txtOldMemberNoDezit.BackColor = Color.LightYellow
                End If

                ' 教官機長年月日表示・非表示
                If Me.cboQualification.SelectedIndex >= 0 Then
                    If (Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR) _
                    And (Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT) Then
                        ' 組合員種別がシニアで乗務資格が教官機長の場合、入力可能
                        Me.mtbTeacherPilotDate.Enabled = True
                    Else
                        ' それ以外の場合、入力不可
                        Me.mtbTeacherPilotDate.Enabled = False
                    End If
                Else
                    ' それ以外の場合、入力不可
                    Me.mtbTeacherPilotDate.Enabled = False
                End If
            Else
                ' 組合員種別が未選択の場合、旧社員番号と旧社員番号ディジット入力不可
                Me.txtOldMemberNo.Enabled = False
                Me.txtOldMemberNoDezit.Enabled = False
                Me.txtOldMemberNo.BackColor = Color.LightYellow
                Me.txtOldMemberNoDezit.BackColor = Color.LightYellow
                ' 組合員種別が未選択の場合、教官機長年月日非表示
                Me.mtbTeacherPilotDate.Enabled = False
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
    '   ＩＤ　：cboQualification_SelectedIndexChanged
    '   名称　：乗務資格チェンジ処理
    '   概要  ：
    '   作成日：2011/12/07(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboQualification_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboQualification.SelectedIndexChanged

        Try
            If (Me.cboQualification.SelectedIndex >= 0) _
            And (Me.cboUserKind.SelectedIndex >= 0) Then
                If (Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR) _
                And (Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT) Then
                    ' 組合員種別がシニアで乗務資格が教官機長の場合、入力可能
                    Me.mtbTeacherPilotDate.Enabled = True
                Else
                    ' それ以外の場合、入力不可
                    Me.mtbTeacherPilotDate.Enabled = False
                End If
            Else
                ' それ以外の場合、入力不可
                Me.mtbTeacherPilotDate.Enabled = False
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
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim arlErrMsg As New ArrayList                                                      ' エラーメッセージリスト
        Dim clsUC999999 As UC999999 = Nothing                                               ' メッセージボックスクラス生成
        Dim strKanaChk As String = ""                                                       ' カナチェック用文字列

        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If
            '-------------------------------------------------------------------------------
            '   社員番号存在確認
            '-------------------------------------------------------------------------------
            If Me.bytHistoryFlg = 0 Then
                If ExistsMemberNo() = False Then
                    Return blnRet
                End If
            End If
            '-------------------------------------------------------------------------------
            '   未入力・未選択・日付チェック
            '-------------------------------------------------------------------------------
            ' 社員番号
            If ChkNull(Me.txtMemberNo.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "社員番号"))
                Call SetErr(Me.txtMemberNo)
            Else
                ' 数値チェック
                If ChkNumber(Me.txtMemberNo.Text.Trim) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "社員番号"))
                    Call SetErr(Me.txtMemberNo)
                End If
            End If
            ' 社員番号ディジット
            If ChkNull(Me.txtMemberNoDezit.Text) = False Then
                ' 半角アルファベットチェック
                If ChkHankakuBigAlpha(Me.txtMemberNoDezit.Text) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "社員番号ディジット"))
                    Call SetErr(Me.txtMemberNoDezit)
                End If
            End If
            ' フリガナ
            If ChkNull(Me.txtKana.Text.Trim) Then                       ' 未入力チェック
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "フリガナ"))
                Call SetErr(Me.txtKana)
            Else
                If ChkHankakuKana(Me.txtKana.Text) = False Then         ' 半角カナチェック
                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", "フリガナ"))
                    Call SetErr(Me.txtKana)
                End If
            End If
            ' 名前
            If ChkNull(Me.txtName.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "名前"))
                Call SetErr(Me.txtName)
            End If
            ' 旧社員番号（組合員種別がシニア組合員の場合、必須項目）
            If Me.cboUserKind.SelectedIndex > 0 Then
                If Me.cboUserKind.SelectedValue = STAF_KIND_SENIOR Then
                    ' 旧社員番号
                    If ChkNull(Me.txtOldMemberNo.Text.Trim) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "旧社員番号"))
                        Call SetErr(Me.txtOldMemberNo)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtOldMemberNo.Text.Trim) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", "旧社員番号"))
                            Call SetErr(Me.txtOldMemberNo)
                        End If
                    End If
                    ' 旧社員番号ディジット
                    If ChkNull(Me.txtOldMemberNoDezit.Text) = False Then
                        ' 半角大文字アルファベットチェック
                        If ChkHankakuBigAlpha(Me.txtOldMemberNoDezit.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", "旧社員番号ディジット"))
                            Call SetErr(Me.txtOldMemberNoDezit)
                        End If
                    End If
                End If
            End If
            ' ステータス
            If ChkNull(Me.cboStatus.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "ステータス"))
                Call SetErr(Me.cboStatus)
            End If
            ' 組合員種別
            If ChkNull(Me.cboUserKind.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "組合員種別"))
                Call SetErr(Me.cboUserKind)
            End If
            ' 運用日付
            If ChkNull(Me.txtUseDate.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "運用日付"))
                Call SetErr(Me.txtUseDate)
            End If
            ' 所属会社
            If ChkNull(Me.cboTransOffice.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "所属会社"))
                Call SetErr(Me.cboTransOffice)
            End If
            ' 会社所属
            If ChkNull(Me.cboLocal.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "会社所属"))
                Call SetErr(Me.cboLocal)
            End If
            ' 職場
            If ChkNull(Me.cboWorkPlace.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "職場"))
                Call SetErr(Me.cboWorkPlace)
            End If
            ' 組合支部
            If ChkNull(Me.cboBelonging.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "組合支部"))
                Call SetErr(Me.cboBelonging)
            End If
            ' 性別
            If ChkNull(Me.cboSex.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "性別"))
                Call SetErr(Me.cboSex)
            End If
            ' 乗務資格
            If ChkNull(Me.cboQualification.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "乗務資格"))
                Call SetErr(Me.cboQualification)
            End If
            ' 機種
            If ChkNull(Me.cboModel.Text.Trim) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "機種"))
                Call SetErr(Me.cboModel)
            End If
            ' 加入年月日
            If Me.mtbEntryDay.ValidateText() Is Nothing Then            ' 未入力チェック
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "加入年月日"))
                Call SetErr(Me.mtbEntryDay)
            Else
                If ChkDate(Me.mtbEntryDay.Text) = False Then            ' 日付チェック
                    arlErrMsg.Add(CLMsg.GetMsg("GE0020", "加入年月日"))
                    Call SetErr(Me.mtbEntryDay)
                End If
            End If
            ' 生年月日
            If Me.mtbBirthDay.ValidateText() Is Nothing Then            ' 未入力チェック
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "生年月日"))
                Call SetErr(Me.mtbBirthDay)
            Else
                If ChkDate(Me.mtbBirthDay.Text) = False Then            ' 日付チェック
                    arlErrMsg.Add(CLMsg.GetMsg("GE0020", "生年月日"))
                    Call SetErr(Me.mtbBirthDay)
                End If
            End If
            ' 機長年月日
            If Me.mtbPilotDate.ValidateText() IsNot Nothing Then
                If ChkDate(Me.mtbPilotDate.Text) = False Then           ' 日付チェック
                    arlErrMsg.Add(CLMsg.GetMsg("GE0020", "機長年月日"))
                    Call SetErr(Me.mtbPilotDate)
                End If
            End If
            ' 入社年月日
            If Me.mtbEntryCoDate.ValidateText() IsNot Nothing Then
                If ChkDate(Me.mtbEntryCoDate.Text) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0020", "入社年月日"))
                    Call SetErr(Me.mtbEntryCoDate)
                End If
            End If
            ' 教官機長年月日（組合員種別が「シニア組合員」で乗務資格が「教官機長」の場合、必須項目）
            If (Me.cboUserKind.SelectedIndex >= 0) And (Me.cboQualification.SelectedIndex >= 0) Then
                If Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR Then
                    If Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT Then
                        If Me.mtbTeacherPilotDate.ValidateText() Is Nothing Then    ' 未入力チェック
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", "教官機長年月日"))
                            Call SetErr(Me.mtbTeacherPilotDate)
                        Else
                            If ChkDate(Me.mtbTeacherPilotDate.Text) = False Then    ' 日付チェック
                                arlErrMsg.Add(CLMsg.GetMsg("GE0020", "教官機長年月日"))
                                Call SetErr(Me.mtbTeacherPilotDate)
                            End If
                        End If
                    End If
                End If
            End If
            ' 退職年月日
            If Me.mtbLeaveDate.ValidateText() IsNot Nothing Then
                If ChkDate(Me.mtbLeaveDate.Text) = False Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0020", "退職年月日"))
                    Call SetErr(Me.mtbLeaveDate)
                End If
            End If
            ' 地位喪失年月日 / 地位喪失理由（ステータスが地位喪失の場合、必須項目）
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_POSITION_LOSS Then
                    ' 地位喪失年月日
                    If Me.mtbPositionLossDate.ValidateText() Is Nothing Then        ' 未入力チェック
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "地位喪失年月日"))
                        Call SetErr(Me.mtbPositionLossDate)
                    Else
                        If ChkDate(Me.mtbPositionLossDate.Text) = False Then        ' 日付チェック
                            arlErrMsg.Add(CLMsg.GetMsg("GE0020", "地位喪失年月日"))
                            Call SetErr(Me.mtbPositionLossDate)
                        End If
                    End If
                    ' 地位喪失理由
                    If ChkNull(Me.cboLossReason.Text.Trim) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "地位喪失理由"))
                        Call SetErr(Me.cboLossReason)
                    End If
                End If
            End If
            ' 脱退年月日 / 脱退理由（ステータスが「脱退」の場合、必須項目）
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' 脱退年月日
                    If Me.mtbWithdrawDate.ValidateText() Is Nothing Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "脱退年月日"))
                        Call SetErr(Me.mtbWithdrawDate)
                    Else
                        If ChkDate(Me.mtbWithdrawDate.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0020", "脱退年月日"))
                            Call SetErr(Me.mtbWithdrawDate)
                        End If
                    End If
                    ' 脱退理由
                    If ChkNull(Me.txtWithdrawReason.Text.Trim) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", "脱退理由"))
                        Call SetErr(Me.txtWithdrawReason)
                    End If
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
    '   ＩＤ　：ExistsStafAttribute
    '   名称　：基本情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>基本情報存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAttribute(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim intRet As Integer = 0               ' 処理件数
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf.c_user_id" & vbCrLf
            strSql = strSql & "       ,staf.c_ksh" & vbCrLf
            strSql = strSql & "       ,staf.c_staf_id" & vbCrLf
            strSql = strSql & "       ,staf.d_from" & vbCrLf
            strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf                                     ' 組合員基本情報
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf                    ' 個人認証IDと同じもの
            strSql = strSql & "    AND staf.c_ksh = '" & Me.strKsh & "'" & vbCrLf                           ' 会社コードと同じもの
            strSql = strSql & "    AND staf.c_staf_id = '" & Me.strStafId & "'" & vbCrLf                    ' 社員番号と同じもの
            strSql = strSql & "    AND staf.d_from = '" & Me.strUseDate & "'" & vbCrLf                      ' 適用日付と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)

            ' 件数取得
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
    '   ＩＤ　：ExistsStafAddress
    '   名称　：住所情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal pStrUserId  As String = 個人認証ID,
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所情報存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAddress(ByVal pStrUserId As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                               ' 処理結果
        Dim clsDb As New CLAccessMdb                                                                ' データベースクラス
        Dim strSql As String = ""                                                                   ' SQL文
        Dim dtRet As DataTable = Nothing                                                            ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf_address.c_user_id" & vbCrLf                             ' 01. 個人認証ID
            strSql = strSql & "       ,staf_address.s_seq" & vbCrLf                                 ' 02. 住所SEQ
            strSql = strSql & "   FROM staf_address" & vbCrLf                                       ' 組合員住所
            strSql = strSql & "  WHERE staf_address.c_user_id = '" & pStrUserId & "'" & vbCrLf      ' 個人認証IDが同じもの
            strSql = strSql & "    AND staf_address.s_seq = 1" & vbCrLf                             ' 住所SEQが同じもの
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' データ存在チェック
            If dtRet.Rows.Count = 0 Then
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
        Finally

            Call clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsMemberNo
    '   名称　：社員番号存在チェック処理
    '   概要　：社員番号が存在しているかチェックを行う。
    '   引数　：
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>社員番号存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsMemberNo() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim strSql As String = ""                                                           ' SQL
        Dim dtRet As DataTable = Nothing                                                    ' 処理結果格納データテーブル
        Dim strUserDateAttribute As String = ""                                             ' 適用日付（基本情報）（年月日なし）

        Try
            ' 適用日付（基本情報）（年月日なし）
            strUserDateAttribute = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf_attribute.c_user_id" & vbCrLf
            strSql = strSql & "   FROM staf_attribute" & vbCrLf
            strSql = strSql & "  WHERE staf_attribute.c_user_id = '" & Me.txtMemberNo.Text & "'" & vbCrLf   ' 個人認証ID
            strSql = strSql & "    AND staf_attribute.c_ksh = '" & Me.strKsh & "'" & vbCrLf                 ' 会社コード
            strSql = strSql & "    AND staf_attribute.c_staf_id = '" & Me.txtMemberNo.Text & "'" & vbCrLf   ' 社員番号
            strSql = strSql & "    AND staf_attribute.d_from = '" & strUserDateAttribute & "'" & vbCrLf     ' 適用日付
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' ステータス判別
            If Me.bytStatus = STATUS_INSERT Then
                '---------------------------------------------------------------------------
                '   新規登録
                '---------------------------------------------------------------------------
                ' 存在チェック
                If dtRet.Rows.Count <> 0 Then
                    Call CLMsg.Show("BE0004")
                    Return blnRet
                End If
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '---------------------------------------------------------------------------
                '   内容変更
                '---------------------------------------------------------------------------
                ' 存在チェック
                If dtRet.Rows.Count = 0 Then
                    Call CLMsg.Show("GE0052")
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
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim clsDb As New CLAccessMdb                                                    ' データベースクラス生成
        Dim strSql As String = ""                                                       ' SQL

        Try
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If
            Call clsDb.Connect()                                                        ' データベース接続
            '-------------------------------------------------------------------------------
            '   コンボボックス作成
            '-------------------------------------------------------------------------------
            ' 定数マスタ詳細（ステータス）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboStatus, _
                                    CONSTANT_ID_USER_STATUS, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（組合員種別）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboUserKind, _
                                    CONSTANT_ID_STAF_KIND, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' （所属会社）コンボボックス作成処理呼び出し
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT ksh.n_ksh AS n_ksh" & vbCrLf     ' 会社名
            strSql = strSql & "       ,ksh.c_ksh AS c_ksh" & vbCrLf     ' 会社コード
            strSql = strSql & "   FROM ksh" & vbCrLf                    ' 会社マスタ
            strSql = strSql & "  ORDER BY ksh.c_ksh" & vbCrLf           ' 会社コードで並び替え    'chk
            If CreateComboBoxNew(clsDb, _
                                 Me.cboTransOffice, _
                                 strSql, _
                                 "n_ksh", _
                                 "c_ksh", _
                                 False, _
                                 MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                 -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（会社所属）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboLocal, _
                                    CONSTANT_ID_AREA_LOCAL, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（職場）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboWorkPlace, _
                                    CONSTANT_ID_WORK_PLACE, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（組合支部）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboBelonging, _
                                    CONSTANT_ID_BELONGING, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            Else
            End If
            ' 定数マスタ詳細（勤務形態）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboWorkForm, _
                                    CONSTANT_ID_WORK_STATE, _
                                    True, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（性別）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboSex, _
                                    CONSTANT_ID_SEX, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（乗務資格）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboQualification, _
                                    CONSTANT_ID_QUALIFICATION, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（機種）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboModel, _
                                    CONSTANT_ID_MODEL, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            ' 定数マスタ詳細（喪失理由）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboLossReason, _
                                    CONSTANT_ID_LOS_POSITION, _
                                    True, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    -1) = False Then
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   基本情報
            '-------------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT Then
                '===================================
                '   新規登録
                '===================================
                Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")   ' 適用日付テキストボックスをプロパティから取得
                Me.txtUseDate.BackColor = Color.LightYellow                                                             ' 適用日付テキストボックスバックカラー薄黄色
                Me.mtbEntryDay.Text = Date.Parse(Format(CInt(Me.strUseDate), "0000/00/00")).ToString("yyyy/MM/dd")      ' 加入年月日をプロパティから取得
                Me.mtbEntryDay.BackColor = Color.LightYellow                                                            ' 加入年月日マスクドテキストボックスバックカラー薄黄色
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '===================================
                '   内容変更
                '===================================
                ' 基本情報取得処理
                If GetStafAttribute(clsDb, 1) = False Then
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertCertify
    '   名称　：パスワードマスタ登録処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/03(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/03(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>パスワードマスタ登録処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertCertify(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim strSql As String = ""                                                       ' SQL文
        Dim intRet As Integer = 0                                                       ' 処理件数
        Dim dtRet As DataTable = Nothing                                                ' 処理結果格納データテーブル
        Dim strPass As String = ""                                                      ' 暗号化前パスワード
        Dim strPassEncrypt As String = ""                                               ' 暗号化後パスワード
        Dim strInputUserDate As String = ""                                             ' 適用日付（年月日なし）

        Try
            '-------------------------------------------------------------------------------
            '   パスワードマスタ存在チェック
            '-------------------------------------------------------------------------------
            ' 適用日付（年月日なし）
            strInputUserDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")
            ' SQL文作成
            strSql = ""
            strSql = strSql & " SELECT certify.c_user_id" & vbCrLf                                  ' 個人認証ID
            strSql = strSql & "   FROM certify" & vbCrLf                                            ' パスワードマスタ
            strSql = strSql & "  WHERE certify.c_user_id = '" & Me.txtMemberNo.Text & "'" & vbCrLf  ' 個人認証IDが入力した社員番号と同じもの
            'strSql = strSql & "    AND certify.d_from = '" & Me.txtUseDate.Text & "'" & vbCrLf      ' 適用開始年月日が入力した適用日付と同じもの
            'strSql = strSql & "    AND certify.d_to = '99999999'" & vbCrLf                          ' 適用終了年月日が99999999と同じもの
            strSql = strSql & ";" & vbCrLf
            ' パスワード暗号化（新規登録の場合、社員番号をパスワードとして登録）
            strPass = Me.txtMemberNo.Text
            strPassEncrypt = CLEncrypt.Encrypt(strPass, EncryptKey)
            If strPassEncrypt.Length = 0 Then
                Call MessageBox.Show("パスワードを暗号化できませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                Call MessageBox.Show("その社員番号は、既にパスワードマスタに登録されています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   パスワードマスタ登録
            '-------------------------------------------------------------------------------
            ' SQL文作成
            strSql = ""
            strSql = strSql & " INSERT INTO certify ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                                      ' 01. 個人認証ID
            strSql = strSql & "   ,d_from" & vbCrLf                                         ' 02. 適用開始年月日
            strSql = strSql & "   ,d_to" & vbCrLf                                           ' 03. 適用終了年月日
            strSql = strSql & "   ,c_pwd" & vbCrLf                                          ' 04. パスワード
            strSql = strSql & "   ,d_ins" & vbCrLf                                          ' 05. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                                  ' 06. 作成者個人ID
            strSql = strSql & " ) VALUES ( " & vbCrLf
            strSql = strSql & "    '" & Me.txtMemberNo.Text.Trim & "'" & vbCrLf             ' 入力した社員番号
            strSql = strSql & "   ,'" & strInputUserDate & "'" & vbCrLf                     ' 選択した適用日付
            strSql = strSql & "   ,'99999999'" & vbCrLf                                     ' 無期限
            strSql = strSql & "   ,'" & strPassEncrypt & "'" & vbCrLf                       ' 暗号化したパスワード（社員番号と同じ）
            strSql = strSql & "   ,'" & Now & "'" & vbCrLf                                  ' クライアント現在日付
            strSql = strSql & "   ,'" & MDLoginInfo.UserId & "'" & vbCrLf                   ' ログインユーザ
            strSql = strSql & ");" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

            ' 処理件数確認
            If intRet <> 1 Then
                Call MessageBox.Show("パスワードマスタに登録できませんでした。", _
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
    '   ＩＤ　：InsertStafAttribute
    '   名称　：基本情報登録処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>基本情報登録処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafAttribute(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim dtRet As DataTable = Nothing                                                ' 処理結果格納データテーブル
        Dim strSql As String = ""                                                       ' SQL文
        Dim intRet As Integer = 0                                                       ' 処理件数
        Dim strInsUserId As String = ""                                                 ' 個人認証ID
        Dim strInsKsh As String = ""                                                    ' 会社コード
        Dim strInsStafId As String = ""                                                 ' 社員番号
        Dim strInsUseDate As String = ""                                                ' 適用日付（年月日なし）

        Try
            '-----------------------------------------------------------------------------------
            '   各データ取得
            '-----------------------------------------------------------------------------------
            strInsUserId = Me.txtMemberNo.Text                                                          ' 個人認証ID
            strInsKsh = Me.cboTransOffice.SelectedValue.ToString()                                      ' 会社コード
            strInsStafId = Me.txtMemberNo.Text                                                          ' 社員番号
            strInsUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付（年月日なし）

            '-----------------------------------------------------------------------------------
            '   基本情報登録
            '-----------------------------------------------------------------------------------
            ' SQL文作成
            strSql = ""
            strSql = strSql & " INSERT INTO staf_attribute ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                  ' 01. 個人認証ＩＤ
            strSql = strSql & "   ,c_ksh" & vbCrLf                      ' 02. 会社コード
            strSql = strSql & "   ,c_staf_id" & vbCrLf                  ' 03. 社員番号
            strSql = strSql & "   ,d_from" & vbCrLf                     ' 04. 適用日付
            strSql = strSql & "   ,c_dezit" & vbCrLf                    ' 05. ディジット
            strSql = strSql & "   ,l_name" & vbCrLf                     ' 06. 名前
            strSql = strSql & "   ,l_name_kna" & vbCrLf                 ' 07. 名前カナ
            strSql = strSql & "   ,k_user_status" & vbCrLf              ' 08. 組合員ステータス区分
            strSql = strSql & "   ,c_trans_ksh" & vbCrLf                ' 09. 所属会社コード
            strSql = strSql & "   ,k_staf_kind" & vbCrLf                ' 10. 組合員種別コード
            strSql = strSql & "   ,k_belonging" & vbCrLf                ' 11. 所属支部
            strSql = strSql & "   ,k_qualification" & vbCrLf            ' 12. 乗務資格
            strSql = strSql & "   ,k_model" & vbCrLf                    ' 13. 機種
            strSql = strSql & "   ,k_international" & vbCrLf            ' 14. 国際線
            strSql = strSql & "   ,k_work_place" & vbCrLf               ' 15. 職場
            strSql = strSql & "   ,k_local" & vbCrLf                    ' 16. 会社支部
            strSql = strSql & "   ,k_work_state" & vbCrLf               ' 17. 勤務形態
            strSql = strSql & "   ,c_area" & vbCrLf                     ' 18. 地区
            strSql = strSql & "   ,k_sex" & vbCrLf                      ' 19. 性別
            strSql = strSql & "   ,d_birth" & vbCrLf                    ' 20. 生年月日
            strSql = strSql & "   ,d_enter" & vbCrLf                    ' 21. 入社年月日
            strSql = strSql & "   ,d_retire" & vbCrLf                   ' 22. 退職年月日
            strSql = strSql & "   ,d_join" & vbCrLf                     ' 23. 加入年月日
            strSql = strSql & "   ,d_drop_out" & vbCrLf                 ' 24. 脱退年月日
            strSql = strSql & "   ,d_captain" & vbCrLf                  ' 25. 機長年月日
            strSql = strSql & "   ,d_teacher_captain" & vbCrLf          ' 26. 教官機長年月日
            strSql = strSql & "   ,d_los_position" & vbCrLf             ' 27. 地位喪失年月日
            strSql = strSql & "   ,l_los_position" & vbCrLf             ' 28. 地位喪失理由区分
            strSql = strSql & "   ,d_die" & vbCrLf                      ' 29. 死亡年月日
            strSql = strSql & "   ,c_staf_id_old" & vbCrLf              ' 30. 旧社員番号
            strSql = strSql & "   ,c_dezit_old" & vbCrLf                ' 31. 旧社員番号ディジット
            strSql = strSql & "   ,l_reason" & vbCrLf                   ' 32. 脱退理由
            strSql = strSql & "   ,l_biko_1" & vbCrLf                   ' 33. 備考１
            strSql = strSql & "   ,k_del" & vbCrLf                      ' 34. 削除区分
            strSql = strSql & "   ,d_ins" & vbCrLf                      ' 35. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf              ' 36. 作成者個人ＩＤ
            strSql = strSql & "   ,d_up" & vbCrLf                       ' 37. 更新日
            strSql = strSql & "   ,c_user_id_up" & vbCrLf               ' 38. 更新者個人ＩＤ
            strSql = strSql & "   ,s_up" & vbCrLf                       ' 39. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            strSql = strSql & "     '" & strInsUserId & "'" & vbCrLf                                    ' 01. 個人認証ＩＤ
            strSql = strSql & "    ,'" & strInsKsh & "'" & vbCrLf                                       ' 02. 会社コード
            strSql = strSql & "    ,'" & strInsStafId & "'" & vbCrLf                                    ' 03. 社員番号
            strSql = strSql & "    ,'" & strInsUseDate & "'" & vbCrLf                                   ' 04. 適用日付
            strSql = strSql & "    ,'" & Me.txtMemberNoDezit.Text.Trim & "'" & vbCrLf                   ' 05. 社員番号ディジット
            strSql = strSql & "    ,'" & Me.txtName.Text.Trim & "'" & vbCrLf                            ' 06. 名前
            strSql = strSql & "    ,'" & Me.txtKana.Text.Trim & "'" & vbCrLf                            ' 07. 名前カナ
            strSql = strSql & "    ,'" & Me.cboStatus.SelectedValue.ToString() & "'" & vbCrLf           ' 08. 組合員ステータス区分
            strSql = strSql & "    ,'" & strInsKsh & "'" & vbCrLf                                       ' 09. 所属会社コード
            strSql = strSql & "    ,'" & Me.cboUserKind.SelectedValue.ToString() & "'" & vbCrLf         ' 10. 組合員種別コード
            strSql = strSql & "    ,'" & Me.cboBelonging.SelectedValue.ToString() & "'" & vbCrLf        ' 11. 所属支部
            strSql = strSql & "    ,'" & Me.cboQualification.SelectedValue.ToString() & "'" & vbCrLf    ' 12. 乗務資格
            strSql = strSql & "    ,'" & Me.cboModel.SelectedValue.ToString() & "'" & vbCrLf            ' 13. 機種
            strSql = strSql & "    ,'" & Me.chkInternational.Checked & "'" & vbCrLf                     ' 14. 国際線
            strSql = strSql & "    ,'" & Me.cboWorkPlace.SelectedValue.ToString() & "'" & vbCrLf        ' 15. 職場
            strSql = strSql & "    ,'" & Me.cboLocal.SelectedValue.ToString() & "'" & vbCrLf            ' 16. 会社支部
            ' 17. 勤務形態
            If Me.cboWorkForm.SelectedIndex >= 0 Then
                strSql = strSql & "    ,'" & Me.cboWorkForm.SelectedValue.ToString() & "'" & vbCrLf
            Else
                strSql = strSql & "    ,''" & vbCrLf
            End If
            strSql = strSql & "    ,''" & vbCrLf                                                        ' 18. 地区
            strSql = strSql & "    ,'" & Me.cboSex.SelectedValue.ToString() & "'" & vbCrLf              ' 19. 性別
            strSql = strSql & "    ,'" & CDate(Me.mtbBirthDay.Text) & "'" & vbCrLf                      ' 20. 生年月日
            ' 21. 入社年月日（チェック有の場合）
            If Me.mtbEntryCoDate.ValidateText() Is Nothing Then
                strSql = strSql & "    ,Null" & vbCrLf
            Else
                strSql = strSql & "    ,'" & CDate(Me.mtbEntryCoDate.Text) & "'" & vbCrLf
            End If
            ' 22. 退職年月日（チェック有の場合）
            If Me.mtbLeaveDate.ValidateText() Is Nothing Then
                strSql = strSql & "    ,Null" & vbCrLf
            Else
                strSql = strSql & "    ,'" & CDate(Me.mtbLeaveDate.Text) & "'" & vbCrLf
            End If
            ' 23. 加入年月日
            strSql = strSql & "    ,'" & CDate(Me.mtbEntryDay.Text) & "'" & vbCrLf
            ' 24. 脱退年月日
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' ステータスが脱退の場合、値設定
                    strSql = strSql & "    ,'" & CDate(Me.mtbWithdrawDate.Text) & "'" & vbCrLf
                Else
                    ' ステータスが脱退以外の場合、空文字設定
                    strSql = strSql & "    ,Null" & vbCrLf
                End If
            Else
                ' ステータスが脱退以外の場合、空文字設定
                strSql = strSql & "    ,Null" & vbCrLf
            End If
            ' 25. 機長年月日
            If Me.mtbPilotDate.ValidateText() Is Nothing Then
                strSql = strSql & "    ,Null" & vbCrLf
            Else
                strSql = strSql & "    ,'" & CDate(Me.mtbPilotDate.Text) & "'" & vbCrLf
            End If
            ' 26. 教官機長年月日
            If (Me.cboUserKind.SelectedIndex >= 0) _
            And (Me.cboQualification.SelectedIndex >= 0) Then
                If (Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR) _
                And (Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT) Then
                    ' 組合員種別がシニア組合員で乗務資格が教官機長の場合、値設定
                    strSql = strSql & "    ,'" & CDate(Me.mtbPilotDate.Text) & "'" & vbCrLf
                Else
                    ' 組合員種別がシニア組合員以外で、乗務資格が教官機長以外の場合、空文字設定
                    strSql = strSql & "    ,Null" & vbCrLf
                End If
            Else
                ' 組合員種別がシニア組合員以外で、乗務資格が教官機長以外の場合、空文字設定
                strSql = strSql & "    ,Null" & vbCrLf
            End If
            ' 27. 地位喪失年月日、28. 地位喪失理由区分
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_POSITION_LOSS Then
                    ' ステータスが地位喪失の場合、値設定
                    strSql = strSql & "    ,'" & CDate(Me.mtbPositionLossDate.Text) & "'" & vbCrLf
                    strSql = strSql & "    ,'" & Me.cboLossReason.SelectedValue.ToString() & "'" & vbCrLf
                Else
                    ' ステータスが地位喪失以外の場合、空文字設定
                    strSql = strSql & "    ,Null" & vbCrLf
                    strSql = strSql & "    ,''" & vbCrLf
                End If
            Else
                ' ステータスが地位喪失以外の場合、空文字設定
                strSql = strSql & "    ,Null" & vbCrLf
                strSql = strSql & "    ,''" & vbCrLf
            End If
            ' 29. 死亡年月日
            strSql = strSql & "    ,''" & vbCrLf
            ' 30. 旧社員番号、31. 旧社員番号ディジット
            If Me.cboUserKind.SelectedIndex >= 0 Then
                If Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR Then
                    strSql = strSql & "    ,'" & Me.txtOldMemberNo.Text.Trim & "'" & vbCrLf
                    strSql = strSql & "    ,'" & Me.txtOldMemberNoDezit.Text.Trim & "'" & vbCrLf
                Else
                    strSql = strSql & "    ,''" & vbCrLf
                    strSql = strSql & "    ,''" & vbCrLf
                End If
            Else
                strSql = strSql & "    ,''" & vbCrLf
                strSql = strSql & "    ,''" & vbCrLf
            End If
            ' 32. 脱退理由
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' ステータスが脱退の場合、値設定
                    strSql = strSql & "    ,'" & Me.txtWithdrawReason.Text.Trim & "'" & vbCrLf
                Else
                    ' ステータスが脱退以外の場合、空文字設定
                    strSql = strSql & "    ,''" & vbCrLf
                End If
            Else
                ' ステータスが脱退以外の場合、空文字設定
                strSql = strSql & "    ,''" & vbCrLf
            End If
            strSql = strSql & "    ,'" & Me.txtNote.Text.Trim & "'" & vbCrLf                            ' 33. 備考１
            strSql = strSql & "    ,'0'" & vbCrLf                                                       ' 34. 削除区分
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf                                           ' 35. 作成日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf                              ' 36. 作成者個人ＩＤ
            strSql = strSql & "    ,Null" & vbCrLf                                                      ' 37. 更新日
            strSql = strSql & "    ,''" & vbCrLf                                                        ' 38. 更新者個人ＩＤ
            strSql = strSql & "    ,0" & vbCrLf                                                         ' 39. 更新回数
            strSql = strSql & " );" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

            ' 処理件数判定
            If intRet <> 1 Then
                Call MessageBox.Show("基本情報を登録できませんでした。", _
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
    '   ＩＤ　：UpdateStafAttribute
    '   名称　：基本情報更新処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>基本情報更新処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateStafAttribute(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim strSql As String = ""                                                       ' SQL文
        Dim intRet As Integer = 0                                                       ' 処理件数
        Dim strUpdUserId As String = ""                                                 ' 個人認証ID
        Dim strUpdKsh As String = ""                                                    ' 会社コード
        Dim strUpdStafId As String = ""                                                 ' 社員番号
        Dim strUpdUseDate As String = ""                                                ' 適用日付（年月日なし）

        Try
            ' 各情報
            strUpdUserId = Me.txtMemberNo.Text.Trim                                                     ' 個人認証ID
            strUpdKsh = Me.cboTransOffice.SelectedValue.ToString()                                      ' 会社コード
            strUpdStafId = Me.txtMemberNo.Text.Trim                                                     ' 社員番号
            strUpdUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付（年月日なし）

            ' SQL文作成
            strSql = ""
            strSql = strSql & " UPDATE staf_attribute" & vbCrLf
            strSql = strSql & "    SET c_user_id         = '" & strUpdUserId & "'" & vbCrLf                                 ' 01. 個人認証ID
            strSql = strSql & "       ,c_ksh             = '" & strUpdKsh & "'" & vbCrLf                                    ' 02. 会社コード（所属会社）
            strSql = strSql & "       ,c_staf_id         = '" & strUpdStafId & "'" & vbCrLf                                 ' 03. 社員番号
            strSql = strSql & "       ,d_from            = '" & strUpdUseDate & "'" & vbCrLf                                ' 04. 適用日付
            strSql = strSql & "       ,c_dezit           = '" & Me.txtMemberNoDezit.Text.Trim & "'" & vbCrLf                ' 05. 社員番号ディジット
            strSql = strSql & "       ,l_name            = '" & Me.txtName.Text.Trim & "'" & vbCrLf                         ' 06. 名前
            strSql = strSql & "       ,l_name_kna        = '" & Me.txtKana.Text.Trim & "'" & vbCrLf                         ' 07. 名前カナ
            strSql = strSql & "       ,k_user_status     = '" & Me.cboStatus.SelectedValue.ToString() & "'" & vbCrLf        ' 08. 組合員ステータス区分
            strSql = strSql & "       ,c_trans_ksh       = '" & Me.cboTransOffice.SelectedValue.ToString() & "'" & vbCrLf   ' 09. 所属会社コード
            strSql = strSql & "       ,k_staf_kind       = '" & Me.cboUserKind.SelectedValue.ToString() & "'" & vbCrLf      ' 10. 組合員種別コード
            strSql = strSql & "       ,k_belonging       = '" & Me.cboBelonging.SelectedValue.ToString() & "'" & vbCrLf     ' 11. 所属支部
            ' 12. 乗務資格
            If Me.cboQualification.SelectedIndex >= 0 Then
                strSql = strSql & "       ,k_qualification   = '" & Me.cboQualification.SelectedValue.ToString() & "'" & vbCrLf
            Else
                strSql = strSql & "       ,k_qualification   = ''" & vbCrLf
            End If
            strSql = strSql & "       ,k_model           = '" & Me.cboModel.SelectedValue.ToString() & "'" & vbCrLf         ' 13. 機種
            strSql = strSql & "       ,k_international   = '" & Me.chkInternational.Checked.ToString() & "'"                ' 14. 国際線
            strSql = strSql & "       ,k_work_place      = '" & Me.cboWorkPlace.SelectedValue.ToString() & "'" & vbCrLf     ' 15. 職場
            strSql = strSql & "       ,k_local           = '" & Me.cboLocal.SelectedValue.ToString() & "'" & vbCrLf         ' 16. 会社支部
            ' 17. 勤務形態
            If Me.cboWorkForm.SelectedIndex >= 0 Then
                strSql = strSql & "       ,k_work_state      = '" & Me.cboWorkForm.SelectedValue.ToString() & "'" & vbCrLf
            Else
                strSql = strSql & "       ,k_work_state      = ''" & vbCrLf
            End If
            strSql = strSql & "       ,c_area            = ''" & vbCrLf                                                     ' 18. 地区
            strSql = strSql & "       ,k_sex             = '" & Me.cboSex.SelectedValue.ToString() & "'" & vbCrLf           ' 19. 性別
            strSql = strSql & "       ,d_birth           = '" & CDate(Me.mtbBirthDay.Text) & "'" & vbCrLf                   ' 20. 生年月日
            ' 21. 入社年月日
            If Me.mtbEntryCoDate.ValidateText() Is Nothing Then
                strSql = strSql & "       ,d_enter           = Null" & vbCrLf
            Else
                strSql = strSql & "       ,d_enter           = '" & CDate(Me.mtbEntryCoDate.Text) & "'" & vbCrLf
            End If
            ' 22. 退職年月日
            If Me.mtbLeaveDate.ValidateText() Is Nothing Then
                strSql = strSql & "       ,d_retire          = Null" & vbCrLf
            Else
                strSql = strSql & "       ,d_retire          = '" & CDate(Me.mtbLeaveDate.Text) & "'" & vbCrLf
            End If
            ' 23. 加入年月日
            strSql = strSql & "           ,d_join            = '" & CDate(Me.mtbEntryDay.Text) & "'" & vbCrLf
            ' 24. 脱退年月日
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' ステータスが脱退の場合、値設定
                    strSql = strSql & "       ,d_drop_out        = '" & CDate(Me.mtbWithdrawDate.Text) & "'" & vbCrLf
                Else
                    ' ステータスが脱退以外の場合、Null設定
                    strSql = strSql & "       ,d_drop_out        = Null" & vbCrLf
                End If
            Else
                ' ステータスが脱退以外の場合、Null設定
                strSql = strSql & "       ,d_drop_out        = Null" & vbCrLf
            End If
            ' 25. 機長年月日
            If Me.mtbPilotDate.ValidateText() Is Nothing Then
                strSql = strSql & "       ,d_captain         = Null" & vbCrLf
            Else
                strSql = strSql & "       ,d_captain         = '" & CDate(Me.mtbPilotDate.Text) & "'" & vbCrLf
            End If
            ' 26. 教官機長年月日
            If (Me.cboUserKind.SelectedIndex >= 0) _
            And (Me.cboQualification.SelectedIndex >= 0) Then
                If (Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR) _
                And (Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT) Then
                    ' 組合員種別がシニア組合員で乗務資格が教官機長の場合、値設定
                    strSql = strSql & "       ,d_teacher_captain = '" & CDate(Me.mtbTeacherPilotDate.Text) & "'" & vbCrLf
                Else
                    ' 組合員種別がシニア組合員以外で、乗務資格が教官機長以外の場合、Null設定
                    strSql = strSql & "       ,d_teacher_captain = Null" & vbCrLf
                End If
            Else
                ' 組合員種別がシニア組合員以外で、乗務資格が教官機長以外の場合、Null設定
                strSql = strSql & "       ,d_teacher_captain = Null" & vbCrLf
            End If
            ' 27. 地位喪失年月日、28. 地位喪失理由区分
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_POSITION_LOSS Then
                    ' ステータスが地位喪失の場合、値設定
                    strSql = strSql & "       ,d_los_position    = '" & CDate(Me.mtbPositionLossDate.Text) & "'" & vbCrLf
                    strSql = strSql & "       ,l_los_position    = '" & Me.cboLossReason.SelectedValue.ToString() & "'" & vbCrLf
                Else
                    ' ステータスが地位喪失以外の場合、Null設定
                    strSql = strSql & "       ,d_los_position    = Null" & vbCrLf
                    strSql = strSql & "       ,l_los_position    = ''" & vbCrLf
                End If
            Else
                ' ステータスが地位喪失以外の場合、Null設定
                strSql = strSql & "       ,d_los_position    = Null" & vbCrLf
                strSql = strSql & "       ,l_los_position    = ''" & vbCrLf
            End If
            ' 29. 死亡年月日
            ' 30. 旧社員番号、31. 旧社員番号ディジット
            If Me.cboUserKind.SelectedIndex >= 0 Then
                If Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR Then
                    ' 組合員種別がシニア組合員の場合、値設定
                    strSql = strSql & "       ,c_staf_id_old     = '" & Me.txtOldMemberNo.Text.Trim & "'" & vbCrLf
                    strSql = strSql & "       ,c_dezit_old       = '" & Me.txtOldMemberNoDezit.Text.Trim & "'" & vbCrLf
                Else
                    ' 組合員種別がシニア組合員以外の場合、空文字設定
                    strSql = strSql & "       ,c_staf_id_old     = ''" & vbCrLf
                    strSql = strSql & "       ,c_dezit_old       = ''" & vbCrLf
                End If
            Else
                ' 組合員種別がシニア組合員以外の場合、空文字設定
                strSql = strSql & "       ,c_staf_id_old     = ''" & vbCrLf
                strSql = strSql & "       ,c_dezit_old       = ''" & vbCrLf
            End If

            ' 32. 脱退理由
            If Me.cboStatus.SelectedIndex >= 0 Then
                If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                    ' ステータスが脱退の場合、値設定
                    strSql = strSql & "       ,l_reason          = '" & Me.txtWithdrawReason.Text.Trim & "'" & vbCrLf
                Else
                    ' ステータスが脱退以外の場合、空文字設定
                    strSql = strSql & "       ,l_reason          = ''" & vbCrLf
                End If
            Else
                ' ステータスが脱退以外の場合、空文字設定
                strSql = strSql & "       ,l_reason          = ''" & vbCrLf
            End If
            strSql = strSql & "       ,l_biko_1          = '" & Me.txtNote.Text.Trim & "'" & vbCrLf                         ' 33. 備考１
            strSql = strSql & "       ,d_up              = '" & Now() & "'" & vbCrLf                                        ' 37. 更新日
            strSql = strSql & "       ,c_user_id_up      = '" & MDLoginInfo.UserId & "'" & vbCrLf                           ' 38. 更新者個人ＩＤ
            strSql = strSql & "       ,s_up              = s_up + 1" & vbCrLf                                               ' 39. 更新回数
            strSql = strSql & "  WHERE c_user_id = '" & Me.strUserId & "'" & vbCrLf                                         ' 個人認証IDと同じもの
            strSql = strSql & "    AND c_ksh     = '" & Me.strKsh & "'" & vbCrLf                                            ' 会社コードと同じもの
            strSql = strSql & "    AND c_staf_id = '" & Me.strUserId & "'" & vbCrLf                                         ' 社員番号と同じもの
            strSql = strSql & "    AND d_from    = '" & Me.strUseDate & "'" & vbCrLf                                        ' 適用日付と同じもの
            strSql = strSql & ";"

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
    '   ＩＤ　：GetStafAttribute
    '   名称　：基本情報取得処理
    '   概要  ：組合員管理 - 基本情報を取得する。
    '   引数　：ByVal pClsDb        As CLAccessMdb = データベースクラス
    '           ByVal pBytReference As Byte        = ステータス（1：通常検索, 2：最新検索（履歴ボタン押下後の適用日付選択画面で直接入力からの検索）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>基本情報取得処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <param name="pBytReference">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStafAttribute(ByVal pClsDb As CLAccessMdb, _
                                      ByVal pBytReference As Byte) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim tbRet As DataTable              ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing  ' 処理件数

        Try
            ' SQL文作成
            If pBytReference = 1 Then
                ' 通常検索
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT staf.c_user_id         AS c_user_id" & vbCrLf            ' 01. 個人認証ＩＤ
                strSql = strSql & "       ,staf.c_ksh             AS c_ksh" & vbCrLf                ' 02. 会社コード
                strSql = strSql & "       ,staf.c_staf_id         AS c_staf_id" & vbCrLf            ' 03. 社員番号
                strSql = strSql & "       ,staf.d_from            AS d_from" & vbCrLf               ' 04. 適用日付
                strSql = strSql & "       ,staf.c_dezit           AS c_dezit" & vbCrLf              ' 05. ディジット
                strSql = strSql & "       ,staf.l_name            AS l_name" & vbCrLf               ' 06. 名前
                strSql = strSql & "       ,staf.l_name_kna        AS l_name_kna" & vbCrLf           ' 07. 名前カナ
                strSql = strSql & "       ,staf.k_user_status     AS k_user_status" & vbCrLf        ' 08. 組合員ステータス区分
                strSql = strSql & "       ,staf.c_trans_ksh       AS c_trans_ksh" & vbCrLf          ' 09. 所属会社コード
                strSql = strSql & "       ,staf.k_staf_kind       AS k_staf_kind" & vbCrLf          ' 10. 組合員種別コード
                strSql = strSql & "       ,staf.k_belonging       AS k_belonging" & vbCrLf          ' 11. 所属支部
                strSql = strSql & "       ,staf.k_qualification   AS k_qualification" & vbCrLf      ' 12. 乗務資格
                strSql = strSql & "       ,staf.k_model           AS k_model" & vbCrLf              ' 13. 機種
                strSql = strSql & "       ,staf.k_international   AS k_international" & vbCrLf      ' 14. 国際線
                strSql = strSql & "       ,staf.k_work_place      AS k_work_place" & vbCrLf         ' 15. 職場
                strSql = strSql & "       ,staf.k_local           AS k_local" & vbCrLf              ' 16. 会社支部
                strSql = strSql & "       ,staf.k_work_state      AS k_work_state" & vbCrLf         ' 17. 勤務形態
                strSql = strSql & "       ,staf.c_area            AS c_area" & vbCrLf               ' 18. 地区
                strSql = strSql & "       ,staf.k_sex             AS k_sex" & vbCrLf                ' 19. 性別
                strSql = strSql & "       ,staf.d_birth           AS d_birth" & vbCrLf              ' 20. 生年月日
                strSql = strSql & "       ,staf.d_enter           AS d_enter" & vbCrLf              ' 21. 入社年月日
                strSql = strSql & "       ,staf.d_retire          AS d_retire" & vbCrLf             ' 22. 退職年月日
                strSql = strSql & "       ,staf.d_join            AS d_join" & vbCrLf               ' 23. 加入年月日
                strSql = strSql & "       ,staf.d_drop_out        AS d_drop_out" & vbCrLf           ' 24. 脱退年月日
                strSql = strSql & "       ,staf.d_captain         AS d_captain" & vbCrLf            ' 25. 機長年月日
                strSql = strSql & "       ,staf.d_teacher_captain AS d_teacher_captain" & vbCrLf    ' 26. 教官機長年月日
                strSql = strSql & "       ,staf.d_los_position    AS d_los_position" & vbCrLf       ' 27. 地位喪失年月日
                strSql = strSql & "       ,staf.l_los_position    AS l_los_position" & vbCrLf       ' 28. 地位喪失理由区分
                strSql = strSql & "       ,staf.c_staf_id_old     AS c_staf_id_old" & vbCrLf        ' 29. 旧社員番号
                strSql = strSql & "       ,staf.c_dezit_old       AS c_dezit_old" & vbCrLf          ' 30. 旧ディジット
                strSql = strSql & "       ,staf.l_reason          AS l_reason" & vbCrLf             ' 31. 脱退理由
                strSql = strSql & "       ,staf.l_biko_1          AS l_biko_1" & vbCrLf             ' 32. 備考１
                strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf                         ' 組合員属性テーブル
                strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf        ' 個人認証IDと同じもの
                'strSql = strSql & "    AND staf.c_ksh = '" & Me.strKsh & "'" & vbCrLf               ' 会社コードと同じもの
                strSql = strSql & "    AND staf.c_staf_id = '" & Me.strStafId & "'" & vbCrLf        ' 社員番号と同じもの
                strSql = strSql & "    AND staf.d_from = '" & Me.strUseDate & "'" & vbCrLf          ' 適用日付と同じもの
                strSql = strSql & ";" & vbCrLf
            ElseIf pBytReference = 2 Then
                ' 最新検索
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT staf.c_user_id         AS c_user_id" & vbCrLf            ' 01. 個人認証ＩＤ
                strSql = strSql & "       ,staf.c_ksh             AS c_ksh" & vbCrLf                ' 02. 会社コード
                strSql = strSql & "       ,staf.c_staf_id         AS c_staf_id" & vbCrLf            ' 03. 社員番号
                strSql = strSql & "       ,staf.d_from            AS d_from" & vbCrLf               ' 04. 適用日付
                strSql = strSql & "       ,staf.c_dezit           AS c_dezit" & vbCrLf              ' 05. ディジット
                strSql = strSql & "       ,staf.l_name            AS l_name" & vbCrLf               ' 06. 名前
                strSql = strSql & "       ,staf.l_name_kna        AS l_name_kna" & vbCrLf           ' 07. 名前カナ
                strSql = strSql & "       ,staf.k_user_status     AS k_user_status" & vbCrLf        ' 08. 組合員ステータス区分
                strSql = strSql & "       ,staf.c_trans_ksh       AS c_trans_ksh" & vbCrLf          ' 09. 所属会社コード
                strSql = strSql & "       ,staf.k_staf_kind       AS k_staf_kind" & vbCrLf          ' 10. 組合員種別コード
                strSql = strSql & "       ,staf.k_belonging       AS k_belonging" & vbCrLf          ' 11. 所属支部
                strSql = strSql & "       ,staf.k_qualification   AS k_qualification" & vbCrLf      ' 12. 乗務資格
                strSql = strSql & "       ,staf.k_model           AS k_model" & vbCrLf              ' 13. 機種
                strSql = strSql & "       ,staf.k_international   AS k_international" & vbCrLf      ' 14. 国際線
                strSql = strSql & "       ,staf.k_work_place      AS k_work_place" & vbCrLf         ' 15. 職場
                strSql = strSql & "       ,staf.k_local           AS k_local" & vbCrLf              ' 16. 会社支部
                strSql = strSql & "       ,staf.k_work_state      AS k_work_state" & vbCrLf         ' 17. 勤務形態
                strSql = strSql & "       ,staf.c_area            AS c_area" & vbCrLf               ' 18. 地区
                strSql = strSql & "       ,staf.k_sex             AS k_sex" & vbCrLf                ' 19. 性別
                strSql = strSql & "       ,staf.d_birth           AS d_birth" & vbCrLf              ' 20. 生年月日
                strSql = strSql & "       ,staf.d_enter           AS d_enter" & vbCrLf              ' 21. 入社年月日
                strSql = strSql & "       ,staf.d_retire          AS d_retire" & vbCrLf             ' 22. 退職年月日
                strSql = strSql & "       ,staf.d_join            AS d_join" & vbCrLf               ' 23. 加入年月日
                strSql = strSql & "       ,staf.d_drop_out        AS d_drop_out" & vbCrLf           ' 24. 脱退年月日
                strSql = strSql & "       ,staf.d_captain         AS d_captain" & vbCrLf            ' 25. 機長年月日
                strSql = strSql & "       ,staf.d_teacher_captain AS d_teacher_captain" & vbCrLf    ' 26. 教官機長年月日
                strSql = strSql & "       ,staf.d_los_position    AS d_los_position" & vbCrLf       ' 27. 地位喪失年月日
                strSql = strSql & "       ,staf.l_los_position    AS l_los_position" & vbCrLf       ' 28. 地位喪失理由区分
                strSql = strSql & "       ,staf.c_staf_id_old     AS c_staf_id_old" & vbCrLf        ' 29. 旧社員番号
                strSql = strSql & "       ,staf.c_dezit_old       AS c_dezit_old" & vbCrLf          ' 30. 旧ディジット
                strSql = strSql & "       ,staf.l_reason          AS l_reason" & vbCrLf             ' 31. 脱退理由
                strSql = strSql & "       ,staf.l_biko_1          AS l_biko_1" & vbCrLf             ' 32. 備考１
                strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf                         ' 組合員属性テーブル
                ' 最新の基本情報取得
                strSql = strSql & "       ,( SELECT a.c_user_id" & vbCrLf
                'strSql = strSql & "                ,a.c_ksh" & vbCrLf
                strSql = strSql & "                ,a.c_staf_id" & vbCrLf
                strSql = strSql & "                ,MAX(a.d_from) AS d_from" & vbCrLf
                strSql = strSql & "            FROM staf_attribute AS a" & vbCrLf
                strSql = strSql & "           GROUP BY a.c_user_id" & vbCrLf
                'strSql = strSql & "                   ,a.c_ksh" & vbCrLf
                strSql = strSql & "                   ,a.c_staf_id ) AS staf_max" & vbCrLf
                strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf        ' 個人認証IDと同じもの
                strSql = strSql & "    AND staf.c_user_id = staf_max.c_user_id" & vbCrLf
                'strSql = strSql & "    AND staf.c_ksh = staf_max.c_ksh" & vbCrLf
                strSql = strSql & "    AND staf.c_staf_id = staf_max.c_staf_id" & vbCrLf
                strSql = strSql & "    AND staf.d_from = staf_max.d_from" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            End If
            tbRet = pClsDb.ExecuteSql(strSql)                                                       ' SQL実行
            intCntRet = tbRet.Rows.Count                                                            ' 処理件数取得
            If intCntRet = 0 Then
                Me.txtMemberNo.Text = Me.strUserId                                                                      ' 01. 個人認証ID
                ' 02. 会社コード
                'Me.txtMemberNo.Text = Me.strStafId                                                                      ' 03. 社員番号
                Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")   ' 04. 適用日付
                Me.txtMemberNoDezit.Text = ""                                                                           ' 05. 社員番号ディジット
                Me.txtName.Text = ""                                                                                    ' 06. 名前
                Me.txtKana.Text = ""                                                                                    ' 07. 名前カナ
                Me.cboStatus.SelectedIndex = 0                                                                          ' 08. 組合員ステータス区分
                Me.cboTransOffice.SelectedIndex = 0                                                                     ' 09. 所属会社
                Me.cboUserKind.SelectedIndex = 0                                                                        ' 10. 組合員種別コード
                Me.cboBelonging.SelectedValue = -1                                                                      ' 11. 所属支部
                Me.cboQualification.SelectedValue = -1                                                                  ' 12. 乗務資格
                Me.cboModel.SelectedValue = -1                                                                          ' 13. 機種
                Me.chkInternational.Checked = False                                                                     ' 14. 国際線
                Me.cboWorkPlace.SelectedValue = -1                                                                      ' 15. 職場
                Me.cboLocal.SelectedValue = -1                                                                          ' 16. 会社支部
                Me.cboWorkForm.SelectedValue = -1                                                                       ' 17. 勤務形態
                ' 18. 地区
                Me.cboSex.SelectedValue = -1                                                                            ' 19. 性別
                Me.mtbBirthDay.Text = ""                                                                                ' 20. 生年月日
                Me.mtbEntryCoDate.Text = ""                                                                             ' 21. 入社年月日
                Me.mtbLeaveDate.Text = ""                                                                               ' 22. 退職年月日
                Me.mtbEntryDay.Text = Date.Parse(Format(CInt(Me.strUseDate), "0000/00/00")).ToString("yyyy/MM/dd")      ' 23. 加入年月日
                Me.mtbWithdrawDate.Text = ""                                                                            ' 24. 脱退年月日
                Me.mtbPilotDate.Text = ""                                                                               ' 25. 機長年月日
                Me.mtbTeacherPilotDate.Text = ""                                                                        ' 26. 教官機長年月日
                Me.mtbPositionLossDate.Text = ""                                                                        ' 27. 地位喪失年月日
                Me.cboLossReason.SelectedIndex = -1                                                                     ' 28. 地位喪失理由区分
                ' 29. 死亡年月日
                Me.txtOldMemberNo.Text = ""                                                                             ' 29. 旧社員番号
                Me.txtOldMemberNo.BackColor = Color.LightYellow                                                         ' 旧社員番号バックカラー薄黄色
                Me.txtOldMemberNoDezit.Text = ""                                                                        ' 30. 旧ディジット
                Me.txtOldMemberNoDezit.BackColor = Color.LightYellow                                                    ' 旧ディジットバックカラー薄黄色
                Me.txtWithdrawReason.Text = ""                                                                          ' 31. 脱退理由
                Me.txtNote.Text = ""                                                                                    ' 32. 備考
            ElseIf intCntRet = 1 Then
                '-----------------------------------------------------------------------------------
                '   各データ設定
                '-----------------------------------------------------------------------------------
                With tbRet.Rows(0)
                    Me.txtMemberNo.Text = NVL(.Item(0).ToString())                                  ' 01. 個人認証ID
                    ' 02. 会社コード
                    Me.txtMemberNo.Text = NVL(.Item(2).ToString())                                  ' 03. 社員番号
                    ' 04. 適用日付
                    If pBytReference = 1 Then
                        Me.txtUseDate.Text = Date.Parse(Format(CInt(.Item(3).ToString()), "0000/00/00")).ToString("yyyy年MM月dd日")
                    ElseIf pBytReference = 2 Then
                        Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strUseDate), "0000/00/00")).ToString("yyyy年MM月dd日")
                    End If
                    Me.txtUseDate.BackColor = Color.LightYellow                                     ' 適用日付テキストボックスバックカラー薄黄色
                    Me.txtMemberNoDezit.Text = NVL(.Item(4).ToString())                             ' 05. 社員番号ディジット
                    Me.txtName.Text = NVL(.Item(5)).ToString()                                      ' 06. 名前
                    Me.txtKana.Text = NVL(.Item(6)).ToString()                                      ' 07. 名前カナ
                    Me.cboStatus.SelectedValue = NVL(.Item(7)).ToString()                           ' 08. 組合員ステータス区分
                    Me.cboTransOffice.SelectedValue = NVL(.Item(8)).ToString()                      ' 09. 所属会社
                    Me.cboUserKind.SelectedValue = NVL(.Item(9)).ToString()                         ' 10. 組合員種別コード
                    Me.cboBelonging.SelectedValue = NVL(.Item(10)).ToString()                       ' 11. 所属支部
                    Me.cboQualification.SelectedValue = NVL(.Item(11)).ToString()                   ' 12. 乗務資格
                    Me.cboModel.SelectedValue = NVL(.Item(12)).ToString()                           ' 13. 機種
                    ' 14. 国際線
                    If .Item(13).ToString().Length = 0 Then
                        Me.chkInternational.Checked = False
                    Else
                        Me.chkInternational.Checked = CBool(.Item(13).ToString())
                    End If
                    Me.cboWorkPlace.SelectedValue = NVL(.Item(14).ToString())                       ' 15. 職場
                    Me.cboLocal.SelectedValue = NVL(.Item(15).ToString())                           ' 16. 会社支部
                    Me.cboWorkForm.SelectedValue = NVL(.Item(16).ToString())                        ' 17. 勤務形態
                    ' 18. 地区
                    Me.cboSex.SelectedValue = NVL(.Item(18).ToString())                             ' 19. 性別
                    ' 20. 生年月日
                    If .Item(19).ToString().Length = 0 Then
                        Me.mtbBirthDay.Text = ""
                    Else
                        Me.mtbBirthDay.Text = CDate(.Item(19)).ToString("yyyy/MM/dd")
                    End If
                    ' 21. 入社年月日
                    If .Item(20).ToString().Length = 0 Then
                        Me.mtbEntryCoDate.Text = ""
                    Else
                        Me.mtbEntryCoDate.Text = CDate(.Item(20)).ToString("yyyy/MM/dd")
                    End If
                    ' 22. 退職年月日
                    If .Item(21).ToString().Length = 0 Then
                        Me.mtbLeaveDate.Text = ""
                    Else
                        Me.mtbLeaveDate.Text = CDate(.Item(21)).ToString("yyyy/MM/dd")
                    End If
                    ' 23. 加入年月日
                    If .Item(22).ToString().Length = 0 Then
                        Me.mtbEntryDay.Text = ""
                    Else
                        Me.mtbEntryDay.Text = CDate(.Item(22)).ToString("yyyy/MM/dd")
                    End If
                    ' 24. 脱退年月日
                    If .Item(23).ToString().Length = 0 Then
                        Me.mtbWithdrawDate.Text = ""
                    Else
                        Me.mtbWithdrawDate.Text = CDate(.Item(23)).ToString("yyyy/MM/dd")
                    End If
                    ' 25. 機長年月日
                    If .Item(24).ToString().Length = 0 Then
                        Me.mtbPilotDate.Text = ""
                    Else
                        Me.mtbPilotDate.Text = CDate(.Item(24)).ToString("yyyy/MM/dd")
                    End If
                    ' 26. 教官機長年月日
                    If .Item(25).ToString().Length = 0 Then
                        Me.mtbTeacherPilotDate.Text = ""
                    Else
                        Me.mtbTeacherPilotDate.Text = CDate(.Item(25)).ToString("yyyy/MM/dd")
                    End If
                    ' 27. 地位喪失年月日
                    If .Item(26).ToString().Length = 0 Then
                        Me.mtbPositionLossDate.Text = ""
                    Else
                        Me.mtbPositionLossDate.Text = CDate(.Item(26)).ToString("yyyy/MM/dd")
                    End If
                    Me.cboLossReason.SelectedValue = NVL(.Item(27).ToString())                      ' 28. 地位喪失理由区分
                    ' 29. 死亡年月日
                    Me.txtOldMemberNo.Text = NVL(.Item(28).ToString())                              ' 29. 旧社員番号
                    Me.txtOldMemberNo.BackColor = Color.White                                       ' 旧社員番号バックカラー薄黄色
                    Me.txtOldMemberNoDezit.Text = NVL(.Item(29).ToString())                         ' 30. 旧ディジット
                    Me.txtOldMemberNoDezit.BackColor = Color.White                                  ' 旧ディジットバックカラー薄黄色
                    Me.txtWithdrawReason.Text = NVL(.Item(30).ToString())                           ' 31. 脱退理由
                    Me.txtNote.Text = NVL(.Item(31).ToString())                                     ' 32. 備考
                End With
            Else
                Call MessageBox.Show("データがありません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
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
    '   作成日：2011/11/25(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>登録更新処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdate() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス

        Try
            Call clsDb.Connect()                                                            ' データベース接続
            Call clsDb.BeginTran()                                                          ' トランザクション開始処理

            If Me.bytHistoryFlg = 0 Then
                '---------------------------------------------------------------------------
                '   通常新規登録
                '---------------------------------------------------------------------------
                If Me.bytStatus = STATUS_INSERT Then                                        ' ステータス判定
                    '---------------------------------------
                    '   パスワードマスタ登録処理
                    '---------------------------------------
                    If InsertCertify(clsDb) = False Then
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("GE0023")                                           ' 入力された社員番号は既に存在します。の旨のメッセージ表示
                        Return blnRet                                                       ' 処理を抜ける
                    End If
                    '---------------------------------------
                    '   基本情報存在チェック処理
                    '---------------------------------------
                    If ExistsStafAttribute(clsDb) = False Then
                        '-----------------------------------
                        '   基本情報登録処理
                        '-----------------------------------
                        If InsertStafAttribute(clsDb) = False Then
                            Call CLMsg.Show("FE0001")                                       ' 予期しないエラーメッセージ表示
                            Call clsDb.RollbackTran()                                       ' トランザクション取消処理
                            Return blnRet                                                   ' 処理を抜ける
                        End If
                    Else
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("GE0023")                                           ' 入力された社員番号は既に存在します。の旨のメッセージ表示
                        Return blnRet                                                       ' 処理を抜ける
                    End If
                ElseIf Me.bytStatus = STATUS_UPDATE Then
                    '---------------------------------------
                    '   基本情報存在チェック処理
                    '---------------------------------------
                    If ExistsStafAttribute(clsDb) Then
                        '-----------------------------------
                        '   基本情報更新処理
                        '-----------------------------------
                        ' 基本情報が存在していた場合、更新処理
                        If UpdateStafAttribute(clsDb) = False Then
                            Call clsDb.RollbackTran()                                       ' トランザクション取消処理
                            Call CLMsg.Show("DE0005")                                       ' 正しくデータが更新できませんでした。の旨のメッセージ表示
                            Return blnRet                                                   ' 処理を抜ける
                        End If
                    Else
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("GE0052")                                           ' 他のユーザによって更新された可能性の旨のメッセージ表示
                        Return blnRet                                                       ' 処理を抜ける
                    End If
                End If
            Else
                '---------------------------------------------------------------------------
                '   内容変更（直接入力）新規登録の場合
                '---------------------------------------------------------------------------
                '-------------------------------------------
                '   基本情報存在チェック処理
                '-------------------------------------------
                If ExistsStafAttribute(clsDb) = False Then
                    '---------------------------------------
                    '   基本情報登録処理
                    '---------------------------------------
                    ' 基本情報が存在しない場合、登録処理
                    If InsertStafAttribute(clsDb) = False Then
                        Call clsDb.RollbackTran()                                           ' トランザクション取消処理
                        Call CLMsg.Show("BE0005")                                           ' 予期しないエラーメッセージ表示
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
    '   ＩＤ　：TransitionScreenAddress
    '   名称　：画面遷移処理（組合員管理 - 住所情報）
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal bytStaus  As Byte = 1：新規登録, 2：内容変更
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/25(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理（組合員管理 - 住所情報）</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreenAddress(ByVal bytStaus As Byte) As Boolean

        Dim blnRet As Boolean = False                                                           ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                                   ' パネルオブジェクト
        Dim clsUC010103 As UC010103 = Nothing                                                   ' 組合員管理 - 住所情報画面クラス
        Dim strUseDateAttribute As String = ""                                                  ' 適用日付（基本情報）

        Try
            ' 適用日付（基本情報）取得
            strUseDateAttribute = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")
            '-----------------------------------------------------------------------------------
            '   組合員管理 - 住所情報
            '-----------------------------------------------------------------------------------
            If bytStaus = STATUS_INSERT Then
                ' 新規登録
                Me.Visible = False                                                              ' 組合員基本情報画面非表示
                clsUC010103 = New UC010103                                                      ' 組合員管理 - 住所情報
                ' 画面間パラメータ情報設定
                clsUC010103.bytStatus = STATUS_INSERT                                           ' ステータス（新規登録）
                clsUC010103.strUserId = Me.txtMemberNo.Text                                     ' 個人認証ID
                clsUC010103.strKsh = Me.cboTransOffice.SelectedValue.ToString()                 ' 会社コード
                clsUC010103.strStafId = Me.txtMemberNo.Text                                     ' 社員番号
                clsUC010103.strUseDateAtt = strUseDateAttribute                                 ' 適用日付（基本情報）
                clsUC010103.strPreScreenId = SCREEN_ID                                          ' 呼び元画面ID
                Call pnl.Controls.Add(clsUC010103)                                              ' 組合員管理 - 基本情報画面表示

            ElseIf bytStaus = STATUS_UPDATE Then
                '-------------------------------------------------------------------------------
                '   住所情報存在チェック
                '-------------------------------------------------------------------------------
                If ExistsStafAddress(strUserId) = False Then
                    ' 新規登録
                    Call CLMsg.Show("GI0002")                                                   ' 存在しない場合、エラーメッセージ表示
                    clsUC010103 = New UC010103                                                  ' 組合員管理 - 住所情報
                    ' 画面間パラメータ情報設定
                    clsUC010103.bytStatus = STATUS_INSERT                                       ' ステータス（新規登録）
                    clsUC010103.strUserId = Me.txtMemberNo.Text                                 ' 個人認証ID
                    clsUC010103.strKsh = Me.cboTransOffice.SelectedValue.ToString()             ' 会社コード
                    clsUC010103.strStafId = Me.txtMemberNo.Text                                 ' 社員番号
                    clsUC010103.strUseDateAtt = strUseDateAttribute                             ' 適用日付（基本情報）
                    clsUC010103.strPreScreenId = SCREEN_ID                                      ' 呼び元画面ID
                    Call pnl.Controls.Add(clsUC010103)                                          ' 組合員管理 - 基本情報画面表示
                Else
                    ' 内容変更
                    clsUC010103 = New UC010103                                                  ' 組合員管理 - 住所情報
                    ' 画面間パラメータ情報設定
                    clsUC010103.bytStatus = STATUS_UPDATE                                       ' ステータス（内容変更）
                    clsUC010103.strUserId = Me.strUserId                                        ' 個人認証ID
                    clsUC010103.strKsh = Me.strKsh                                              ' 会社コード
                    clsUC010103.strStafId = Me.strStafId                                        ' 社員番号
                    clsUC010103.strUseDateAtt = Me.strUseDate                                   ' 適用日付（基本情報）
                    clsUC010103.strPreScreenId = SCREEN_ID                                      ' 呼び元画面ID
                    Call pnl.Controls.Add(clsUC010103)                                          ' 組合員管理 - 基本情報画面表示
                End If
            End If

            ' 組合員基本情報画面非表示
            Me.Visible = False

            ' 戻り値設定
            blnRet = True

        Catch ex As Exception
            ' パネル非表示
            pnl.Visible = False
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
    '   名称　：画面遷移処理（組合員検索画面）
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal iBlnSearchFlg As Boolean = True：再検索有り, False：再検索無し
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/27(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/27(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <param name="iBlnSearchFlg">再検索フラグ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal iBlnSearchFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False                                   ' 処理結果
        Dim pn As Panel = ParentForm.Controls(MDConst.MAIN_PANEL_ID)    ' メインパネル
        Dim uc As Control = Nothing                                     ' ユーザコントロール
        Dim clsUC010101 As New UC010101                                 ' 組合員検索画面

        Try
            Cursor.Current = Cursors.WaitCursor                         ' カーソルを砂時計に設定
            uc = pn.Controls(SCREEN_ID_UC010101)                        ' 組合員検索画面
            If uc Is Nothing Then
                uc = New UC010101                                       ' 組合員検索画面生成
                Call pn.Controls.Add(uc)                                ' メインパネルに組合員検索画面追加
            Else
                clsUC010101 = pn.Controls(SCREEN_ID_UC010101)           ' 組合員検索画面
                clsUC010101.blnSearchFlg = iBlnSearchFlg                ' 再検索フラグ設定
                uc.Visible = True                                       ' 組合検索画面表示
            End If
            Me.Dispose()                                                ' 組合員管理 - 基本情報画面閉じる

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

    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：ByVal pBlnEdit As Boolean = True：アンロック, False：ロック
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock(ByVal pBlnEdit As Boolean) As Boolean

        Dim blnRet As Boolean = False                                   ' 処理結果
        Dim blnOldMemberNo As Boolean = False                           ' 旧社員番号・旧社員番号ディジット用
        Dim clrOldMemberNo As Color = Color.LightYellow                 ' 旧社員番号・旧社員番号ディジットバックカラー用
        Dim blnTeacherCaptain As Boolean = False                        ' 教官機長年月日用
        Dim blnLosPosition As Boolean = False                           ' 地位喪失年月日、地位喪失理由用
        Dim blnDropOut As Boolean = False                               ' 脱退年月日、脱退理由用
        Dim blnHistory As Boolean = False                               ' 履歴ボタン用
        Dim blnAddress As Boolean = False                               ' 住所情報照会ボタン用
        Dim blnEntry As Boolean = False                                 ' 新規登録ボタン用
        Dim blnUpdate As Boolean = False                                ' 内容変更ボタン用
        Dim blnCancel As Boolean = False                                ' キャンセルボタン用
        Dim blnBack As Boolean = False                                  ' 戻るボタン用
        Dim strTitle As String = ""                                     ' タイトル

        Try
            '-------------------------------------------------------------------------------
            '   表示・非表示フラグ設定
            '-------------------------------------------------------------------------------
            ' 新規登録の場合、デフォルトのFalseを設定済み
            If Me.bytStatus = STATUS_INSERT Then
                '-----------------------------------------------
                '   新規登録
                '-----------------------------------------------
                blnEntry = True         ' 登録確認ボタン用
                blnCancel = True        ' キャンセルボタン用

            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '-----------------------------------------------
                '   内容変更
                '-----------------------------------------------
                If pBlnEdit Then
                    ' 内容変更ボタン押下後
                    blnEntry = True             ' 登録確認ボタン用
                    blnCancel = True            ' キャンセルボタン用

                    ' 旧社員番号、旧社員番号ディジット
                    If Me.cboUserKind.SelectedIndex >= 0 Then
                        If Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR Then
                            ' 組合員種別がシニア組合員の場合、旧社員番号と旧社員番号ディジット入力可能
                            blnOldMemberNo = True
                            clrOldMemberNo = Color.White
                        End If
                    End If

                    ' 教官機長年月日
                    If (Me.cboUserKind.SelectedIndex >= 0) _
                    And (Me.cboQualification.SelectedIndex >= 0) Then
                        If (Me.cboUserKind.SelectedValue.ToString() = STAF_KIND_SENIOR) _
                        And (Me.cboQualification.SelectedValue.ToString() = QUALIFICATION_TEACHER_PILOT) Then
                            ' 組合員種別がシニア組合員で乗務資格が教官機長の場合、使用可能
                            blnTeacherCaptain = True
                        End If
                    End If

                    ' 地位喪失年月日、地位喪失理由
                    If Me.cboStatus.SelectedIndex >= 0 Then
                        If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_POSITION_LOSS Then
                            ' ステータスが地位喪失の場合、使用可能
                            blnLosPosition = True
                        End If
                    End If

                    ' 脱退年月日、脱退理由
                    If Me.cboStatus.SelectedIndex >= 0 Then
                        If Me.cboStatus.SelectedValue.ToString() = USER_STATUS_LEAVE Then
                            ' ステータスが脱退の場合、使用可能
                            blnDropOut = True
                        End If
                    End If

                Else
                    ' 内容変更ボタン押下前
                    blnUpdate = True            ' 内容変更ボタン用
                    blnAddress = True           ' 住所情報照会ボタン用
                    blnBack = True              ' 戻るボタン用
                    blnHistory = True           ' 履歴ボタン
                End If
            End If

            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            ' ボタン
            Me.btnHistory.Enabled = blnHistory                                              ' 履歴ボタン
            Me.btnAddress.Visible = blnAddress                                              ' 住所情報照会ボタン
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


            Me.txtMemberNo.Enabled = pBlnEdit                                               ' 社員番号
            Me.txtMemberNoDezit.Enabled = pBlnEdit                                          ' 社員番号ディジット
            Me.txtKana.Enabled = pBlnEdit                                                   ' フリガナ

            Me.txtOldMemberNo.Enabled = blnOldMemberNo                                      ' 旧社員番号
            Me.txtOldMemberNoDezit.Enabled = blnOldMemberNo                                 ' 旧社員番号ディジット

            Me.txtOldMemberNo.BackColor = clrOldMemberNo                                    ' 旧社員番号バックカラー
            Me.txtOldMemberNoDezit.BackColor = clrOldMemberNo                               ' 旧社員番号ディジットバックカラー

            Me.txtName.Enabled = pBlnEdit                                                   ' 名前
            Me.cboStatus.Enabled = pBlnEdit                                                 ' ステータス
            Me.cboUserKind.Enabled = pBlnEdit                                               ' 組合員種別

            Me.txtUseDate.Enabled = False                                                   ' 適用日付

            Me.cboTransOffice.Enabled = pBlnEdit                                            ' 所属会社
            Me.cboLocal.Enabled = pBlnEdit                                                  ' 会社所属
            Me.cboWorkPlace.Enabled = pBlnEdit                                              ' 職場
            Me.cboBelonging.Enabled = pBlnEdit                                              ' 組合支部
            Me.cboWorkForm.Enabled = pBlnEdit                                               ' 勤務形態
            Me.cboSex.Enabled = pBlnEdit                                                    ' 性別
            Me.cboQualification.Enabled = pBlnEdit                                          ' 乗務資格
            Me.cboModel.Enabled = pBlnEdit                                                  ' 機種
            Me.chkInternational.Enabled = pBlnEdit                                          ' 国際線
            Me.mtbEntryDay.Enabled = pBlnEdit                                               ' 加入年月日
            Me.mtbBirthDay.Enabled = pBlnEdit                                               ' 生年月日
            Me.mtbPilotDate.Enabled = pBlnEdit                                              ' 機長年月日
            Me.mtbEntryCoDate.Enabled = pBlnEdit                                            ' 入社年月日

            Me.mtbTeacherPilotDate.Enabled = blnTeacherCaptain                              ' 教官機長年月日

            Me.mtbLeaveDate.Enabled = pBlnEdit                                              ' 退職年月日

            Me.mtbPositionLossDate.Enabled = blnLosPosition                                 ' 地位喪失年月日
            Me.cboLossReason.Enabled = blnLosPosition                                       ' 地位喪失理由

            Me.mtbWithdrawDate.Enabled = blnDropOut                                         ' 脱退年月日
            Me.txtWithdrawReason.Enabled = blnDropOut                                       ' 脱退理由

            Me.txtNote.Enabled = pBlnEdit


            ' バックカラー
            Me.txtMemberNo.ForeColor = Color.Black                                          ' 社員番号
            Me.txtMemberNoDezit.ForeColor = Color.Black                                     ' 社員番号ディジット
            Me.txtKana.ForeColor = Color.Black                                              ' フリガナ

            Me.txtName.ForeColor = Color.Black                                              ' 名前
            Me.cboStatus.ForeColor = Color.Black                                            ' ステータス
            Me.cboUserKind.ForeColor = Color.Black                                          ' 組合員種別

            Me.txtUseDate.ForeColor = Color.Black                                           ' 適用日付

            Me.cboTransOffice.ForeColor = Color.Black                                       ' 所属会社
            Me.cboLocal.ForeColor = Color.Black                                             ' 会社所属
            Me.cboWorkPlace.ForeColor = Color.Black                                         ' 職場
            Me.cboBelonging.ForeColor = Color.Black                                         ' 組合支部
            Me.cboWorkForm.ForeColor = Color.Black                                          ' 勤務形態
            Me.cboSex.ForeColor = Color.Black                                               ' 性別
            Me.cboQualification.ForeColor = Color.Black                                     ' 乗務資格
            Me.cboModel.ForeColor = Color.Black                                             ' 機種
            Me.chkInternational.ForeColor = Color.Black                                     ' 国際線
            Me.mtbEntryDay.ForeColor = Color.Black                                          ' 加入年月日
            Me.mtbBirthDay.ForeColor = Color.Black                                          ' 生年月日
            Me.mtbPilotDate.ForeColor = Color.Black                                         ' 機長年月日
            Me.mtbEntryCoDate.ForeColor = Color.Black                                       ' 入社年月日
            Me.txtNote.ForeColor = Color.Black                                              ' 備考

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
    '   作成日：2011/11/07(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果

        Try
            ' Title
            If Me.bytStatus = STATUS_INSERT Then
                Me.lblTitle.Text = "基本情報 - 新規登録"
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                Me.lblTitle.Text = "組合員管理 - 基本情報"
            End If
            ' TextBox
            Me.txtMemberNo.Text = ""                                                        ' 社員番号
            Me.txtMemberNoDezit.Text = ""                                                   ' 社員番号ディジット
            Me.txtKana.Text = ""                                                            ' フリガナ
            Me.txtOldMemberNo.Text = ""                                                     ' 旧社員番号
            Me.txtOldMemberNo.BackColor = Color.LightYellow                                 ' 旧社員番号バックカラー薄黄色
            Me.txtOldMemberNoDezit.Text = ""                                                ' 旧社員番号ディジット
            Me.txtOldMemberNoDezit.BackColor = Color.LightYellow                            ' 旧社員番号ディジットバックカラー薄黄色
            Me.txtName.Text = ""                                                            ' 名前
            Me.txtUseDate.Text = ""                                                         ' 運用日付
            Me.txtUseDate.BackColor = Color.LightYellow                                     ' 運用日付テキストボックスバックカラー薄黄色
            Me.txtWithdrawReason.Text = ""                                                  ' 脱退理由
            Me.txtNote.Text = ""                                                            ' 備考
            ' MaskedTextBoxt
            Me.mtbEntryDay.Text = ""                                                        ' 加入年月日
            Me.mtbBirthDay.Text = ""                                                        ' 生年月日
            Me.mtbPilotDate.Text = ""                                                       ' 機長年月日
            Me.mtbEntryCoDate.Text = ""                                                     ' 入社年月日
            Me.mtbTeacherPilotDate.Text = ""                                                ' 教官機長年月日
            Me.mtbLeaveDate.Text = ""                                                       ' 退職年月日
            Me.mtbPositionLossDate.Text = ""                                                ' 地位喪失年月日
            Me.mtbWithdrawDate.Text = ""                                                    ' 脱退年月日
            ' ComboBoxList
            Me.cboStatus.DataSource = Nothing                                               ' ステータス
            Me.cboUserKind.DataSource = Nothing                                             ' 組合員種別
            Me.cboTransOffice.DataSource = Nothing                                          ' 所属会社
            Me.cboLocal.DataSource = Nothing                                                ' 会社所属
            Me.cboWorkPlace.DataSource = Nothing                                            ' 職場
            Me.cboBelonging.DataSource = Nothing                                            ' 組合支部
            Me.cboWorkForm.DataSource = Nothing                                             ' 勤務形態
            Me.cboSex.DataSource = Nothing                                                  ' 性別
            Me.cboQualification.DataSource = Nothing                                        ' 乗務資格
            Me.cboModel.DataSource = Nothing                                                ' 機種
            Me.cboLossReason.DataSource = Nothing                                           ' 喪失理由
            ' ComboBox
            Me.cboStatus.Text = ""                                                          ' ステータス
            Me.cboUserKind.Text = ""                                                        ' 組合員種別
            Me.cboTransOffice.Text = ""                                                     ' 所属会社
            Me.cboLocal.Text = ""                                                           ' 会社所属
            Me.cboWorkPlace.Text = ""                                                       ' 職場
            Me.cboWorkPlace.Text = ""                                                       ' 組合支部
            Me.cboWorkForm.Text = ""                                                        ' 勤務形態
            Me.cboSex.Text = ""                                                             ' 性別
            Me.cboQualification.Text = ""                                                   ' 乗務資格
            Me.cboModel.Text = ""                                                           ' 機種
            Me.cboLossReason.Text = ""                                                      ' 喪失理由
            ' Button
            Me.btnHistory.Visible = True                                                    ' 履歴ボタン
            Me.btnBack.Visible = True                                                       ' 戻るボタン
            Me.btnUpdate.Visible = True                                                     ' 内容変更ボタン
            Me.btnCancel.Visible = True                                                     ' キャンセルボタン
            Me.btnInsertChk.Visible = True                                                  ' 登録確認ボタン
            Me.btnHistory.Enabled = True                                                    ' 履歴ボタン
            Me.btnAddress.Visible = True                                                    ' 住所情報照会ボタン
            ' Label
            Me.lblEntryDayAge.Text = ""                                                     ' 加入年月日経過年数
            Me.lblBirthDayAge.Text = ""                                                     ' 年齢
            Me.lblPilotDateAge.Text = ""                                                    ' 機長年月日経過年数
            Me.lblEntryCoDateAge.Text = ""                                                  ' 入社年月日経過年数
            Me.lblTeacherPilotDateAge.Text = ""                                             ' 教官機長年月日経過年数

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
    '   ＩＤ　：CalPassedYears
    '   名称　：経過年数計算処理
    '   概要  ：
    '   引数　：ByVal iMtbCtl As Windows.Forms.MaskedTextBox = 経過年数表示コントロール
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>経過年数計算処理</summary>
    ''' <param name="iMtbCtl">経過年数表示コントロール</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CalPassedYears(ByVal iMtbCtl As Windows.Forms.MaskedTextBox) As String

        Dim strRet As String = ""                   ' 処理結果
        Dim strTarget As String = ""                ' 対象年月日（"/"なしのyyyyMMdd）
        Dim strNow As String = ""                   ' 現在日付（"/"なしのyyyyMMdd）
        Dim intYear As Integer = 0                  ' 経過年
        Dim intMonth As Integer = 0                 ' 経過月（現在日付から年を引いたもの）
        Dim intMonthAll As Integer = 0              ' 経過月

        Try
            If iMtbCtl.ValidateText() IsNot Nothing Then                                    ' 対象年月日に値があるかチェック
                ' 対象年月日と現在日付取得
                strTarget = iMtbCtl.Text.Replace("/", "").Replace("-", "")                                   ' 対象年月日（"/"なしのyyyyMMD）
                strNow = Now.ToString("yyyyMMdd")                                           ' 現在日付（"/"なしのyyyyMMdd）
                If ChkDate(strTarget) Then                                                  ' 対象年月日が正しい日付かチェック
                    If strTarget <= strNow Then                                             ' 対象年月日が未来日でなないかチェック
                        ' 経過年計算
                        intYear = DateDiff(DateInterval.Year, Date.Parse(Format(CInt(strTarget), "0000/00/00")), Date.Parse(Format(CInt(strNow), "0000/00/00")))
                        ' 経過月計算
                        intMonthAll = DateDiff(DateInterval.Month, Date.Parse(Format(CInt(strTarget), "0000/00/00")), Date.Parse(Format(CInt(strNow), "0000/00/00")))
                        intMonth = intMonthAll - (intYear * 12)
                        ' 経過年数取得
                        strRet = "（" & intYear.ToString() & "年" & intMonth.ToString() & "ヶ月）"
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

        ' 戻り値設定
        Return strRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetAge
    '   名称　：年齢取得処理
    '   概要  ：
    '   引数　：ByVal iMtbCtl  As Windows.Forms.MaskedTextBox = 年齢表示コントロール,
    '           ByRef ioIntAge As Integer                     = 年齢
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>年齢取得処理</summary>
    ''' <param name="iMtbCtl">年齢表示コントロール</param>
    ''' <param name="ioIntAge">年齢</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetAge(ByVal iMtbCtl As Windows.Forms.MaskedTextBox,
                            ByRef ioIntAge As Integer) As Integer

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strTarget As String = ""                    ' 対象年月日（"/"なしのyyyyMMdd）
        Dim strNow As String = ""                       ' 現在日付（"/"なしのyyyyMMdd）
        Dim intAge As Integer = 0                       ' 年齢

        Try
            If iMtbCtl.ValidateText() IsNot Nothing Then                                    ' 対象年月日に値があるかチェック
                ' 対象年月日と現在日付取得
                strTarget = iMtbCtl.Text.Replace("/", "").Replace("-", "")                                   ' 対象年月日（"/"なしのyyyyMMD）
                strNow = Now.ToString("yyyyMMdd")                                           ' 現在日付（"/"なしのyyyyMMdd）
                If ChkDate(strTarget) Then                                                  ' 対象年月日が正しい日付かチェック
                    If strTarget <= strNow Then                                             ' 対象年月日が未来日でなないかチェック
                        ' (現在日 – 対象年月日) / 10000 の小数点以下を切り捨てたものが年数
                        intAge = System.Math.Floor((Integer.Parse(strNow) - Integer.Parse(strTarget)) / 10000)
                    End If
                End If
            End If

            ' 掲載した年齢を設定
            ioIntAge = intAge

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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限取得処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim dtGrant As DataTable = Nothing          ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC010101)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString            ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString               ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString           ' ファイル出力権限
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
#End Region

End Class

#End Region