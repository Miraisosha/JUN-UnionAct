#Region "UC010103"
'===========================================================================================================
'   クラスＩＤ　　：UC010103
'   クラス名称　　：組合員管理 - 住所情報
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDChk
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo

Public Class UC010103

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面種別
    Private Const SCREEN_HISTORY As Byte = 0                        ' 適用日付選択画面
    Private Const SCREEN_SEARCH As Byte = 1                         ' 組合員検索画面
    Private Const SCREEN_ADDRESS As Byte = 2                        ' 組合員管理 - 住所情報画面
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                         ' 新規登録
    Private Const STATUS_UPDATE As Byte = 2                         ' 内容変更
    ' 処理
    Private Const PROCESS_INSERT As Byte = 1                        ' 登録処理
    Private Const PROCESS_UPDATE As Byte = 2                        ' 更新処理
    Private Const PROCESS_DELETE As Byte = 3                        ' 削除処理
    ' 住所タブ
    Private Const ADDRESS_TAB1 As Byte = 1                          ' 住所1タブ
    Private Const ADDRESS_TAB2 As Byte = 2                          ' 住所2タブ
    Private Const ADDRESS_TAB3 As Byte = 3                          ' 住所3タブ
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC010103          ' UC010103
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC010103      ' 組合員管理 - 住所情報画面
    ' エラー文字
    Private Const ERR_ADDRESS1 As String = "住所１－"               ' 住所１タブ
    Private Const ERR_ADDRESS2 As String = "住所２－"               ' 住所２タブ
    Private Const ERR_ADDRESS3 As String = "住所３－"               ' 住所３タブ
    ' 子画面用パラメータ（住所検索ボタン押下時）
    ReadOnly AddressColName As String() = {"郵便番号", "都道府県半角カナ", "市区町村名半角カナ", "町域名半角", "都道府県名", "市区町村名", "町域名", "旧郵便番号", "作成日"}
    ReadOnly AddressColWidth As Integer() = {100, 100, 100, 100, 100, 100, 100, 100, 100}
    ReadOnly AddressColShow As Boolean() = {True, True, True, True, True, True, True, True, True}
    ' 子画面用プロパティ（履歴ボタン押下時）
    ReadOnly UseDateColName As String() = {"適用日付", "担当者"}    ' 適用日付選択画面カラム名
    ReadOnly UseDateColWidth As Integer() = {150, 200}              ' 適用日付選択画面カラム幅
    ReadOnly UseDateColShow As Boolean() = {True, True}             ' 適用日付選択画面カラム表示
    ReadOnly UseDateColShowNo As Boolean() = {False, False}         ' 適用日付選択画面カラム非表示
    ' 権限
    Private strGrantReference As String = "0"                       ' 参照権限
    Private strGrantInsert As String = "0"                          ' 登録権限
    Private strGrantPrint As String = "0"                           ' 印刷権限
    Private strGrantFileOutput As String = "0"                      ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _bytStatus As Byte = 0                                   ' ステータス（1：新規登録, 2：内容変更）
    Public _bytHistoryFlg As Byte = 0                               ' 履歴新規登録フラグ（0：通常新規登録1：履歴新規登録）
    Public _strUserId As String = ""                                ' 個人認証ID
    Public _strKsh As String = ""                                   ' 会社コード
    Public _strStafId As String = ""                                ' 社員番号
    Public _strUseDateAtt As String = ""                            ' 適用日付（基本情報）
    Public _strUseDateAdd As String = ""                            ' 適用日付（住所情報）
    Public _strPreScreenId As String = ""                           ' 呼び元画面ID
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
    ' 適用日付（基本情報）
    Public Property strUseDateAtt() As String
        Get
            Return _strUseDateAtt
        End Get
        Set(ByVal value As String)
            _strUseDateAtt = value
        End Set
    End Property
    ' 適用日付（住所情報）
    Public Property strUseDateAdd() As String
        Get
            Return _strUseDateAdd
        End Get
        Set(ByVal value As String)
            _strUseDateAdd = value
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
    '   ＩＤ　：UC010103_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC010103_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If setGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If ControlClear(bytStatus) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If GetData() = False Then
                Exit Sub
            End If
            ' 住所区分1チェンジ処理
            If ChangeAddress(ADDRESS_TAB1, Me.cboAddressKbn1.SelectedIndex) = False Then
                Exit Sub
            End If
            ' 住所区分2チェンジ処理
            If ChangeAddress(ADDRESS_TAB2, Me.cboAddressKbn2.SelectedIndex) = False Then
                Exit Sub
            End If
            ' 住所区分3チェンジ処理
            If ChangeAddress(ADDRESS_TAB3, Me.cboAddressKbn3.SelectedIndex) = False Then
                Exit Sub
            End If
            ' 国内海外区分1チェンジ処理
            If ChangeInternational(ADDRESS_TAB1, Me.cboInternational1.SelectedValue.ToString()) = False Then
                Exit Sub
            End If
            ' 国内海外区分2チェンジ処理
            If ChangeInternational(ADDRESS_TAB2, Me.cboInternational2.SelectedValue.ToString()) = False Then
                Exit Sub
            End If
            ' 国内海外区分3チェンジ処理
            If ChangeInternational(ADDRESS_TAB3, Me.cboInternational3.SelectedValue.ToString()) = False Then
                Exit Sub
            End If

            '---------------------------------------------------------------------------
            '   コントロールロック処理
            '---------------------------------------------------------------------------
            If Me.bytStatus = STATUS_INSERT Then
                '-----------------------------------------------------------------------
                '   新規登録
                '-----------------------------------------------------------------------
                If ControlRockUnLock(True) = False Then
                    Exit Sub
                End If
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '-----------------------------------------------------------------------
                '   内容変更
                '-----------------------------------------------------------------------
                If ControlRockUnLock(False) = False Then
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
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim clsFM010104 As FM010104 = Nothing                   ' 適用日付選択画面
        Dim strSql As String = ""                               ' SQL文
        Dim clsDb As New CLAccessMdb                            ' データベースクラス
        Dim bytRefStatus As Byte = 0                            ' ステータス（1：通常検索,2：最新検索）

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '---------------------------------------------------------------------------
            '   適用日付選択画面
            '---------------------------------------------------------------------------
            clsFM010104 = New FM010104                                  ' インスタンス作成
            clsFM010104.Text = "住所情報履歴 - 適用日付選択画面"        ' タイトル設定
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & "  SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf  ' 適用開始年月日
            strSql = strSql & "        ,staf_max.l_name" & vbCrLf                                       ' 最新の作成者の名前
            ' 住所1の住所情報取得
            strSql = strSql & "    FROM ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                 ,b.d_from" & vbCrLf
            strSql = strSql & "                 ,b.c_user_id_ins" & vbCrLf
            strSql = strSql & "             FROM staf_address AS b" & vbCrLf
            strSql = strSql & "                 ,( SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "                          ,a.s_seq" & vbCrLf
            strSql = strSql & "                          ,a.d_from" & vbCrLf
            strSql = strSql & "                      FROM staf_address AS a" & vbCrLf
            strSql = strSql & "                     WHERE a.s_seq = 1" & vbCrLf
            strSql = strSql & "                     GROUP BY a.c_user_id" & vbCrLf
            strSql = strSql & "                             ,a.s_seq" & vbCrLf
            strSql = strSql & "                             ,a.d_from ) AS c" & vbCrLf
            strSql = strSql & "            WHERE b.c_user_id = c.c_user_id" & vbCrLf
            strSql = strSql & "              AND b.s_seq = c.s_seq" & vbCrLf
            strSql = strSql & "              AND b.d_from = c.d_from ) AS staf" & vbCrLf
            ' 最新の作成者の名前取得
            strSql = strSql & "         LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                           ,b.l_name" & vbCrLf
            strSql = strSql & "                       FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                           ,( SELECT a.c_user_id" & vbCrLf
            'strSql = strSql & "                                    ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                    ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                    ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                                FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                               GROUP BY a.c_user_id" & vbCrLf
            'strSql = strSql & "                                       ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                       ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                      WHERE b.c_user_id = c.c_user_id" & vbCrLf
            'strSql = strSql & "                        AND b.c_ksh = c.c_ksh" & vbCrLf
            strSql = strSql & "                        AND b.c_staf_id = c.c_staf_id" & vbCrLf
            strSql = strSql & "                        AND b.d_from = c.d_from" & vbCrLf
            strSql = strSql & "                     UNION" & vbCrLf
            strSql = strSql & "                     SELECT e.c_staf_id" & vbCrLf
            strSql = strSql & "                           ,e.l_name" & vbCrLf
            strSql = strSql & "                       FROM full_time_staf AS e" & vbCrLf
            strSql = strSql & "                           ,( SELECT d.c_staf_id" & vbCrLf
            strSql = strSql & "                                    ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                                FROM full_time_staf AS d" & vbCrLf
            strSql = strSql & "                               GROUP BY d.c_staf_id ) AS f" & vbCrLf
            strSql = strSql & "                      WHERE e.c_staf_id = f.c_staf_id" & vbCrLf
            strSql = strSql & "                        AND e.d_from = f.d_from ) AS staf_max" & vbCrLf
            strSql = strSql & "         ON staf.c_user_id_ins = staf_max.c_user_id" & vbCrLf
            strSql = strSql & "   WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf
            strSql = strSql & "   ORDER BY staf.d_from DESC" & UtDb.DbOrderOffset & vbCrLf 'ok
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
                    Me.strUseDateAdd = clsFM010104.dtpSpecifyTime.Value.ToString("yyyyMMdd")
                    ' 履歴新規登録フラグを 1 にする
                    Me.bytHistoryFlg = 1
                    ' 2：最新検索
                    bytRefStatus = 2
                Else
                    ' 選択行から適用日付取得
                    Me.strUseDateAdd = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                    ' 履歴新規登録フラグを 0 にする
                    Me.bytHistoryFlg = 0
                    ' 1：通常検索
                    bytRefStatus = 1
                End If
                ' データベース接続
                Call clsDb.Connect()
                ' 住所情報取得処理
                If GetStafAddress(clsDb, bytRefStatus) = False Then
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
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理
            If TransitionScreen() = False Then
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
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertChk.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim daiRet As DialogResult = Nothing    ' 確認メッセージ結果

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
            '   住所情報更新処理（登録・更新・削除）
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
            ' 画面遷移処理（組合員検索）
            If TransitionScreen() = False Then
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
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
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
            If TransitionScreen() = False Then
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
    '   ＩＤ　：btnAttribute_Click
    '   名称　：基本情報照会ボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAttribute_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttribute.Click

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                               ' パネルオブジェクト
        Dim clsUC010102 As UC010102 = Nothing                                               ' 組合員管理 - 基本情報クラス

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   組合員管理 - 基本情報
            '-------------------------------------------------------------------------------
            clsUC010102 = pnl.Controls(SCREEN_ID_UC010102)
            ' 画面間パラメータ情報設定
            clsUC010102 = New UC010102                                                      ' 組合員管理 - 住所情報
            clsUC010102.bytStatus = STATUS_UPDATE                                           ' ステータス（内容変更）
            clsUC010102.strUserId = Me.strUserId                                            ' 個人認証ID
            clsUC010102.strKsh = Me.strKsh                                                  ' 会社コード
            clsUC010102.strStafId = Me.strStafId                                            ' 社員番号
            clsUC010102.strUseDate = Me.strUseDateAtt                                       ' 適用日付（基本情報）
            clsUC010102.strPreScreenId = SCREEN_ID                                          ' 呼び元画面ID
            Call pnl.Controls.Add(clsUC010102)                                              ' 組合員管理 - 住所情報画面表示

            ' パネル非表示
            Me.Visible = False

        Catch ex As Exception
            ' パネル非表示
            pnl.Visible = False
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
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click

        Dim clsFM010104 As FM010104 = Nothing           ' 適用日付選択画面
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '---------------------------------------------------------------------------
            '   適用日付選択画面
            '---------------------------------------------------------------------------
            ' インスタンス作成
            clsFM010104 = New FM010104
            ' タイトル設定
            clsFM010104.Text = "住所情報履歴 - 適用日付選択画面"
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & "  SELECT CONVERT(DATE,staf.d_from,112) AS d_from" & vbCrLf  ' 適用開始年月日
            strSql = strSql & "        ,staf_max.l_name" & vbCrLf                                       ' 最新の作成者の名前
            ' 住所1の住所情報取得
            strSql = strSql & "    FROM ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                 ,b.d_from" & vbCrLf
            strSql = strSql & "                 ,b.c_user_id_ins" & vbCrLf
            strSql = strSql & "             FROM staf_address AS b" & vbCrLf
            strSql = strSql & "                 ,( SELECT a.c_user_id" & vbCrLf
            strSql = strSql & "                          ,a.s_seq" & vbCrLf
            strSql = strSql & "                          ,a.d_from" & vbCrLf
            strSql = strSql & "                      FROM staf_address AS a" & vbCrLf
            strSql = strSql & "                     WHERE a.s_seq = 1" & vbCrLf
            strSql = strSql & "                     GROUP BY a.c_user_id" & vbCrLf
            strSql = strSql & "                             ,a.s_seq" & vbCrLf
            strSql = strSql & "                             ,a.d_from ) AS c" & vbCrLf
            strSql = strSql & "            WHERE b.c_user_id = c.c_user_id" & vbCrLf
            strSql = strSql & "              AND b.s_seq = c.s_seq" & vbCrLf
            strSql = strSql & "              AND b.d_from = c.d_from ) AS staf" & vbCrLf
            ' 最新の作成者の名前取得
            strSql = strSql & "         LEFT JOIN ( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                           ,b.l_name" & vbCrLf
            strSql = strSql & "                       FROM staf_attribute AS b" & vbCrLf
            strSql = strSql & "                           ,( SELECT a.c_user_id" & vbCrLf
            'strSql = strSql & "                                    ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                    ,a.c_staf_id" & vbCrLf
            strSql = strSql & "                                    ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                                FROM staf_attribute AS a" & vbCrLf
            strSql = strSql & "                               GROUP BY a.c_user_id" & vbCrLf
            'strSql = strSql & "                                       ,a.c_ksh" & vbCrLf
            strSql = strSql & "                                       ,a.c_staf_id ) AS c" & vbCrLf
            strSql = strSql & "                      WHERE b.c_user_id = c.c_user_id" & vbCrLf
            'strSql = strSql & "                        AND b.c_ksh = c.c_ksh" & vbCrLf
            strSql = strSql & "                        AND b.c_staf_id = c.c_staf_id" & vbCrLf
            strSql = strSql & "                        AND b.d_from = c.d_from" & vbCrLf
            strSql = strSql & "                     UNION" & vbCrLf
            strSql = strSql & "                     SELECT e.c_staf_id" & vbCrLf
            strSql = strSql & "                           ,e.l_name" & vbCrLf
            strSql = strSql & "                       FROM full_time_staf AS e" & vbCrLf
            strSql = strSql & "                           ,( SELECT d.c_staf_id" & vbCrLf
            strSql = strSql & "                                    ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                                FROM full_time_staf AS d" & vbCrLf
            strSql = strSql & "                               GROUP BY d.c_staf_id ) AS f" & vbCrLf
            strSql = strSql & "                      WHERE e.c_staf_id = f.c_staf_id" & vbCrLf
            strSql = strSql & "                        AND e.d_from = f.d_from ) AS staf_max" & vbCrLf
            strSql = strSql & "         ON staf.c_user_id_ins = staf_max.c_user_id" & vbCrLf
            strSql = strSql & "   WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf
            strSql = strSql & "   ORDER BY staf.d_from DESC" & UtDb.DbOrderOffset & vbCrLf
            'todo:
            ' プロパティ設定
            clsFM010104.strSqlSentence = strSql
            clsFM010104.SetCulumnsName = UseDateColName                 ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = UseDateColWidth               ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = UseDateColShow                 ' 適用日付選択画面カラム表示
            clsFM010104.EnableChkDirectSpecify = False                  ' 直接入力使用不可
            clsFM010104.chkDirectSpecify.Checked = False                ' 直接入力チェックなし

            ' 適用日付選択画面表示
            Call clsFM010104.ShowDialog()

            ' クリックされたボタンをチェック
            If clsFM010104.IntQlickBtnFlag = 0 Then
                ' OKボタン押下
                ' 選択行からのみ（直接指定からの遷移はない）
                ' 選択行から適用日付取得
                Me.strUseDateAdd = CDate(clsFM010104.dgdHistoryList.CurrentRow.Cells.Item(0).Value).ToString("yyyyMMdd")
                ' 履歴新規登録フラグを 0 にする
                Me.bytHistoryFlg = 0
                ' データベース接続
                Call clsDb.Connect()
                ' 住所情報取得処理
                If GetStafAddress(clsDb, 1) = False Then
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
    '   ＩＤ　：btnAddressSearch1_Click
    '   名称　：住所検索ボタン（タブ１）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAddressSearch1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddressSearch1.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-----------------------------------------------------------
            '   住所検索処理
            '-----------------------------------------------------------
            If SearchAddress(ADDRESS_TAB1, Me.txtPostalNo1_1.Text, Me.txtPostalNo1_2.Text) = False Then
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
    '   ＩＤ　：btnAddressSearch2_Click
    '   名称　：住所検索ボタン（タブ２）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAddressSearch2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddressSearch2.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-----------------------------------------------------------
            '   住所検索処理
            '-----------------------------------------------------------
            If SearchAddress(ADDRESS_TAB2, Me.txtPostalNo2_1.Text, Me.txtPostalNo2_2.Text) = False Then
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
    '   ＩＤ　：btnAddressSearch3_Click
    '   名称　：住所検索ボタン（タブ３）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAddressSearch3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddressSearch3.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-----------------------------------------------------------
            '   住所検索処理
            '-----------------------------------------------------------
            If SearchAddress(ADDRESS_TAB3, Me.txtPostalNo3_1.Text, Me.txtPostalNo3_2.Text) = False Then
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
    '   ＩＤ　：btnClear1_Click
    '   名称　：クリアボタン（タブ１）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnClear1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear1.Click

        Try
            '-----------------------------------------------------------
            '   アドレス1情報クリア処理
            '-----------------------------------------------------------
            If CLMsg.Show("GQ0023", "住所１") = DialogResult.Yes Then
                If ClearAddress(ADDRESS_TAB1) = False Then
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
    '   ＩＤ　：btnClear2_Click
    '   名称　：クリアボタン（タブ２）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnClear2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear2.Click

        Try
            '-----------------------------------------------------------
            '   アドレス2情報クリア処理
            '-----------------------------------------------------------
            If CLMsg.Show("GQ0023", "住所２") = DialogResult.Yes Then
                If ClearAddress(ADDRESS_TAB2) = False Then
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
    '   ＩＤ　：btnClear3_Click
    '   名称　：クリアボタン（タブ３）クリック処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnClear3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear3.Click

        Try
            '-----------------------------------------------------------
            '   アドレス3情報クリア処理
            '-----------------------------------------------------------
            If CLMsg.Show("GQ0023", "住所３") = DialogResult.Yes Then
                If ClearAddress(ADDRESS_TAB2) = False Then
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
    '   ＩＤ　：cboInternational1_SelectedIndexChanged
    '   名称　：国内海外区分（住所1タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/11/24(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboInternational1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInternational1.SelectedIndexChanged

        Try
            If Me.cboInternational1.SelectedIndex >= 0 Then
                ' 海外区分チェンジ処理
                If ChangeInternational(ADDRESS_TAB1, Me.cboInternational1.SelectedValue.ToString()) = False Then
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
    '   ＩＤ　：cboInternational2_SelectedIndexChanged
    '   名称　：国内海外区分（住所2タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/11/24(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboInternational2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInternational2.SelectedIndexChanged

        Try
            If Me.cboInternational2.SelectedIndex >= 0 Then
                ' 国内海外区分チェンジ処理
                If ChangeInternational(ADDRESS_TAB2, Me.cboInternational2.SelectedValue.ToString()) = False Then
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
    '   ＩＤ　：cboInternational3_SelectedIndexChanged
    '   名称　：国内海外区分（住所1タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/11/24(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboInternational3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboInternational3.SelectedIndexChanged

        Try
            If Me.cboInternational3.SelectedIndex >= 0 Then
                ' 国内海外区分チェンジ処理
                If ChangeInternational(ADDRESS_TAB3, Me.cboInternational3.SelectedValue.ToString()) = False Then
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
    '   ＩＤ　：cboAddressKbn1_SelectedIndexChanged
    '   名称　：住所区分（住所1タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboAddressKbn1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAddressKbn1.SelectedIndexChanged

        Try
            If ChangeAddress(ADDRESS_TAB1, Me.cboAddressKbn1.SelectedIndex) = False Then    ' 国内海外情報表示処理
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
    '   ＩＤ　：cboAddressKbn2_SelectedIndexChanged
    '   名称　：住所区分（住所2タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboAddressKbn2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAddressKbn2.SelectedIndexChanged

        Try
            If ChangeAddress(ADDRESS_TAB2, cboAddressKbn2.SelectedIndex) = False Then       ' 国内海外情報表示処理
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
    '   ＩＤ　：cboAddressKbn3_SelectionChangeCommitted
    '   名称　：住所区分（住所3タブ）コンボボックスチェンジ処理
    '   概要  ：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboAddressKbn3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAddressKbn3.SelectedIndexChanged

        Try
            If ChangeAddress(ADDRESS_TAB3, Me.cboAddressKbn3.SelectedIndex) = False Then    ' 国内海外情報表示処理
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
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
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
        Dim strChk2 As String = ""                  ' 住所2チェック
        Dim strChk3 As String = ""                  ' 住所3チェック
        Dim intMainAddress As Integer = 0           ' 現住所チェック

        Try
            '-------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Call CLMsg.Show("FE0001")
                Return blnRet
            End If

            '===================================================================
            '   共通
            '===================================================================
            ' 住所区分チェック
            If (Me.cboAddressKbn1.SelectedIndex = 0) _
            And (Me.cboAddressKbn2.SelectedIndex = 0) _
            And (Me.cboAddressKbn3.SelectedIndex = 0) Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0094"))
            End If
            ' 現住所チェック
            If Me.cboAddressKbn1.SelectedIndex > 0 Then
                If Me.chkMainAddress1.Checked Then
                    intMainAddress = intMainAddress + 1
                End If
            End If
            If Me.cboAddressKbn2.SelectedIndex > 0 Then
                If Me.chkMainAddress2.Checked Then
                    intMainAddress = intMainAddress + 1
                End If
            End If
            If Me.cboAddressKbn3.SelectedIndex > 0 Then
                If Me.chkMainAddress3.Checked Then
                    intMainAddress = intMainAddress + 1
                End If
            End If
            ' 1つのみチェックしているかチェック
            If intMainAddress = 0 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0095"))
            End If

            '===================================================================
            '   住所1
            '===================================================================
            ' 国内海外判定
            If Me.cboInternational1.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '---------------------------------------------------------------
                '   国内情報
                '---------------------------------------------------------------
                ' 郵便番号1
                If ChkNull(Me.txtPostalNo1_1.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "郵便番号1"))
                    SetErr(Me.txtPostalNo1_1)
                Else
                    ' 数値チェック
                    If ChkNumber(Me.txtPostalNo1_1.Text) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "郵便番号1"))
                        SetErr(Me.txtPostalNo1_1)
                    End If
                End If
                ' 郵便番号2
                If ChkNull(Me.txtPostalNo1_2.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "郵便番号2"))
                    SetErr(Me.txtPostalNo1_2)
                Else
                    ' 数値チェック
                    If ChkNumber(Me.txtPostalNo1_2.Text) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "郵便番号2"))
                        SetErr(Me.txtPostalNo1_2)
                    End If
                End If
                ' 都道府県
                If Me.cboPrefectures1.SelectedIndex < 0 Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "都道府県"))
                    SetErr(Me.cboPrefectures1)
                End If
                ' 市区町村
                If ChkNull(Me.txtCities1.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "市区町村"))
                    SetErr(Me.txtCities1)
                End If
                ' 番地等
                If ChkNull(Me.txtAddAther1.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "番地等"))
                    SetErr(Me.txtAddAther1)
                End If
                ' 電話番号1-1
                If ChkNull(Me.txtTel1_1_1.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号１(市外局番)"))
                    SetErr(Me.txtTel1_1_1)
                Else
                    ' 数値チェック
                    If ChkNumber(Me.txtTel1_1_1.Text) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号１(市外局番))"))
                        SetErr(Me.txtTel1_1_1)
                    End If
                End If
                ' 電話番号1-2
                If ChkNull(Me.txtTel1_1_2.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号１(加入者番号)"))
                    SetErr(Me.txtTel1_1_2)
                Else
                    ' 数値チェック
                    If ChkNumber(Me.txtTel1_1_2.Text) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号１(加入者番号)"))
                        SetErr(Me.txtTel1_1_2)
                    End If
                End If
                ' 電話番号1-3
                If ChkNull(Me.txtTel1_1_3.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号１(市内局番)"))
                    SetErr(Me.txtTel1_1_3)
                Else
                    ' 数値チェック
                    If ChkNumber(Me.txtTel1_1_3.Text) = False Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号１(市内局番)"))
                        SetErr(Me.txtTel1_1_3)
                    End If
                End If
                ' 電話番号2
                If (Me.txtTel1_2_1.Text & Me.txtTel1_2_2.Text & Me.txtTel1_2_3.Text).Length <> 0 Then
                    ' 電話番号2-1
                    If ChkNull(Me.txtTel1_2_1.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号２(市外局番)"))
                        SetErr(Me.txtTel1_2_1)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_2_1.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号２(市外局番)"))
                            SetErr(Me.txtTel1_2_1)
                        End If
                    End If
                    ' 電話番号2-2
                    If ChkNull(Me.txtTel1_2_2.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号２(加入者番号)"))
                        SetErr(Me.txtTel1_2_2)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_2_2.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号２(加入者番号)"))
                            SetErr(Me.txtTel1_2_2)
                        End If
                    End If
                    ' 電話番号2-3
                    If ChkNull(Me.txtTel1_2_3.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号２(市内局番)"))
                        SetErr(Me.txtTel1_2_3)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_2_3.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号２(市内局番)"))
                            SetErr(Me.txtTel1_2_3)
                        End If
                    End If
                End If
                ' 電話番号3
                If (Me.txtTel1_3_1.Text & Me.txtTel1_3_2.Text & Me.txtTel1_3_3.Text).Length <> 0 Then
                    ' 電話番号3-1
                    If ChkNull(Me.txtTel1_3_1.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号３(市外局番)"))
                        SetErr(Me.txtTel1_3_1)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_3_1.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号３(市外局番)"))
                            SetErr(Me.txtTel1_3_1)
                        End If
                    End If
                    ' 電話番号3-2
                    If ChkNull(Me.txtTel1_3_2.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号３(加入者番号)"))
                        SetErr(Me.txtTel1_3_2)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_3_2.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号３(加入者番号)"))
                            SetErr(Me.txtTel1_3_2)
                        End If
                    End If
                    ' 電話番号3-3
                    If ChkNull(Me.txtTel1_3_3.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号３(市内局番)"))
                        SetErr(Me.txtTel1_3_3)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtTel1_3_3.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "電話番号３(市内局番)"))
                            SetErr(Me.txtTel1_3_3)
                        End If
                    End If
                End If
                ' FAX
                If (Me.txtFax1_1.Text & Me.txtFax1_2.Text & Me.txtFax1_3.Text).Length <> 0 Then
                    ' FAX1-1
                    If ChkNull(Me.txtFax1_1.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "FAX(市外局番)"))
                        SetErr(Me.txtFax1_1)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtFax1_1.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "FAX(市外局番)"))
                            SetErr(Me.txtFax1_1)
                        End If
                    End If
                    ' FAX1-2
                    If ChkNull(Me.txtFax1_2.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "FAX(加入者番号)"))
                        SetErr(Me.txtTel1_2)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtFax1_2.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "FAX(加入者番号)"))
                            SetErr(Me.txtFax1_2)
                        End If
                    End If
                    ' FAX1-3
                    If ChkNull(Me.txtFax1_3.Text) Then
                        arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "FAX(市内局番)"))
                        SetErr(Me.txtTel1_3)
                    Else
                        ' 数値チェック
                        If ChkNumber(Me.txtFax1_3.Text) = False Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS1 & "FAX(市内局番)"))
                            SetErr(Me.txtFax1_3)
                        End If
                    End If
                End If

            ElseIf Me.cboInternational1.SelectedValue = INTERNATIONAL_KBN_ABROAD Then

                '---------------------------------------------------------------
                '   海外情報
                '---------------------------------------------------------------
                ' アドレス1
                If Me.txtForeignAdress1_1.Text.Length = 0 Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "アドレス1"))
                    SetErr(Me.txtForeignAdress1_1)
                End If
                ' 電話番号1
                If Me.txtTel1_1.Text.Length = 0 Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS1 & "電話番号1"))
                    SetErr(Me.txtTel1_1)
                End If
            End If

            '===================================================================
            '   住所2
            '===================================================================
            ' 国内海外判定
            If Me.cboInternational2.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '---------------------------------------------------------------
                '   国内情報
                '---------------------------------------------------------------
                If Me.cboAddressKbn2.SelectedIndex > 0 Then
                    strChk2 = Me.txtPostalNo2_1.Text & Me.txtPostalNo2_2.Text & _
                              Me.cboPrefectures2.Text & _
                              Me.txtCities2.Text & _
                              Me.txtAddAther1.Text & _
                              Me.txtTel2_1_1.Text & Me.txtTel2_1_2.Text & Me.txtTel2_1_3.Text & _
                              Me.txtTel2_2_1.Text & Me.txtTel2_2_2.Text & Me.txtTel2_2_3.Text & _
                              Me.txtTel2_3_1.Text & Me.txtTel2_3_2.Text & Me.txtTel2_3_3.Text & _
                              Me.txtFax2_1.Text & Me.txtFax2_2.Text & Me.txtFax2_3.Text
                    ' なんらかが入力されていればチェックを行う。
                    If strChk2.Length <> 0 Then
                        ' 郵便番号1
                        If ChkNull(Me.txtPostalNo2_1.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "郵便番号1"))
                            SetErr(Me.txtPostalNo2_1)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtPostalNo2_1.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "郵便番号1"))
                                SetErr(Me.txtPostalNo2_1)
                            End If
                        End If
                        ' 郵便番号2
                        If ChkNull(Me.txtPostalNo2_2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "郵便番号2"))
                            SetErr(Me.txtPostalNo2_2)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtPostalNo2_2.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "郵便番号2"))
                                SetErr(Me.txtPostalNo2_2)
                            End If
                        End If
                        ' 都道府県
                        If Me.cboPrefectures2.SelectedIndex < 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "都道府県"))
                            SetErr(Me.cboPrefectures2)
                        End If
                        ' 市区町村
                        If ChkNull(Me.txtCities2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "市区町村"))
                            SetErr(Me.txtCities2)
                        End If
                        ' 番地等
                        If ChkNull(Me.txtAddAther2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "番地等"))
                            SetErr(Me.txtAddAther2)
                        End If
                        ' 電話番号1-1
                        If ChkNull(Me.txtTel2_1_1.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号１(市外局番)"))
                            SetErr(Me.txtTel2_1_1)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel2_1_1.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号１(市外局番)"))
                                SetErr(Me.txtTel2_1_1)
                            End If
                        End If
                        ' 電話番号1-2
                        If ChkNull(Me.txtTel2_1_2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号１(加入者番号)"))
                            SetErr(Me.txtTel2_1_2)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel2_1_2.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号１(加入者番号)"))
                                SetErr(Me.txtTel2_1_2)
                            End If
                        End If
                        ' 電話番号1-3
                        If ChkNull(Me.txtTel2_1_3.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号１(市内局番)"))
                            SetErr(Me.txtTel2_1_3)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel2_1_3.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号１(市内局番)"))
                                SetErr(Me.txtTel2_1_3)
                            End If
                        End If
                        ' 電話番号2
                        If (Me.txtTel2_2_1.Text & Me.txtTel2_2_2.Text & Me.txtTel2_2_3.Text).Length <> 0 Then
                            ' 電話番号2-1
                            If ChkNull(Me.txtTel2_2_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号２(市外局番)"))
                                SetErr(Me.txtTel2_2_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_2_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号２(市外局番)"))
                                    SetErr(Me.txtTel2_2_1)
                                End If
                            End If
                            ' 電話番号2-2
                            If ChkNull(Me.txtTel2_2_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号２(加入者番号)"))
                                SetErr(Me.txtTel2_2_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_2_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号２(加入者番号)"))
                                    SetErr(Me.txtTel2_2_2)
                                End If
                            End If
                            ' 電話番号2-3
                            If ChkNull(Me.txtTel2_2_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号２(市内局番)"))
                                SetErr(Me.txtTel2_2_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_2_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号２(市内局番)"))
                                    SetErr(Me.txtTel2_2_3)
                                End If
                            End If
                        End If
                        ' 電話番号3
                        If (Me.txtTel2_3_1.Text & Me.txtTel2_3_2.Text & Me.txtTel2_3_3.Text).Length <> 0 Then
                            ' 電話番号3-1
                            If ChkNull(Me.txtTel2_3_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号３(市外局番)"))
                                SetErr(Me.txtTel2_3_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_3_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号３(市外局番)"))
                                    SetErr(Me.txtTel2_3_1)
                                End If
                            End If
                            ' 電話番号3-2
                            If ChkNull(Me.txtTel2_3_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号２(加入者番号)"))
                                SetErr(Me.txtTel2_3_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_3_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号２(加入者番号)"))
                                    SetErr(Me.txtTel2_3_2)
                                End If
                            End If
                            ' 電話番号3-3
                            If ChkNull(Me.txtTel2_3_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号３(市内局番)"))
                                SetErr(Me.txtTel2_3_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel2_3_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "電話番号３(市内局番)"))
                                    SetErr(Me.txtTel2_3_3)
                                End If
                            End If
                        End If
                        ' FAX
                        If (Me.txtFax2_1.Text & Me.txtFax2_2.Text & Me.txtFax2_3.Text).Length <> 0 Then
                            ' FAX2-1
                            If ChkNull(Me.txtFax2_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "FAX(市外局番)"))
                                SetErr(Me.txtFax2_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax2_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "FAX(市外局番)"))
                                    SetErr(Me.txtFax2_1)
                                End If
                            End If
                            ' FAX2-2
                            If ChkNull(Me.txtFax2_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "FAX(加入者番号)"))
                                SetErr(Me.txtTel2_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax2_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "FAX(加入者番号)"))
                                    SetErr(Me.txtFax2_2)
                                End If
                            End If
                            ' FAX2-3
                            If ChkNull(Me.txtFax2_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "FAX(市内局番)"))
                                SetErr(Me.txtTel2_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax2_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS2 & "FAX(市内局番)"))
                                    SetErr(Me.txtFax2_3)
                                End If
                            End If
                        End If
                    End If
                End If

            ElseIf Me.cboInternational2.SelectedValue = INTERNATIONAL_KBN_ABROAD Then
                '---------------------------------------------------------------
                '   海外情報
                '---------------------------------------------------------------
                If Me.cboAddressKbn2.SelectedIndex <> 0 Then
                    strChk2 = Me.txtForeignAdress2_1.Text & _
                              Me.txtForeignAdress2_2.Text & _
                              Me.txtForeignAdress2_3.Text & _
                              Me.txtForeignAdress2_4.Text & _
                              Me.txtForeignAdress2_5.Text & _
                              Me.txtTel2_1.Text & Me.txtTel2_2.Text & Me.txtTel2_3.Text & _
                              Me.txtFax2.Text
                    If strChk2.Length <> 0 Then
                        ' アドレス1
                        If Me.txtForeignAdress2_1.Text.Length = 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "アドレス1"))
                            SetErr(Me.txtForeignAdress2_1)
                        End If
                        ' 電話番号1
                        If Me.txtTel2_1.Text.Length = 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS2 & "電話番号1"))
                            SetErr(Me.txtTel2_1)
                        End If
                    End If
                End If
            End If

            '===================================================================
            '   住所3
            '===================================================================
            ' 国内海外判定
            If Me.cboInternational3.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '---------------------------------------------------------------
                '   国内
                '---------------------------------------------------------------
                If Me.cboAddressKbn3.SelectedIndex > 0 Then
                    strChk3 = Me.txtPostalNo3_1.Text & Me.txtPostalNo3_2.Text & _
                              Me.cboPrefectures3.Text & _
                              Me.txtCities3.Text & _
                              Me.txtAddAther3.Text & _
                              Me.txtTel3_1_1.Text & Me.txtTel3_1_2.Text & Me.txtTel3_1_3.Text & _
                              Me.txtTel3_2_1.Text & Me.txtTel3_2_2.Text & Me.txtTel3_2_3.Text & _
                              Me.txtTel3_3_1.Text & Me.txtTel3_3_2.Text & Me.txtTel3_3_3.Text & _
                              Me.txtFax3_1.Text & Me.txtFax3_2.Text & Me.txtFax3_3.Text
                    ' なんらかが入力されていればチェックを行う。
                    If strChk3.Length <> 0 Then
                        ' 郵便番号1
                        If ChkNull(Me.txtPostalNo3_1.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "郵便番号1"))
                            SetErr(Me.txtPostalNo3_1)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtPostalNo3_1.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "郵便番号1"))
                                SetErr(Me.txtPostalNo3_1)
                            End If
                        End If
                        ' 郵便番号2
                        If ChkNull(Me.txtPostalNo3_2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "郵便番号2"))
                            SetErr(Me.txtPostalNo3_2)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtPostalNo3_2.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "郵便番号2"))
                                SetErr(Me.txtPostalNo3_2)
                            End If
                        End If
                        ' 都道府県
                        If Me.cboPrefectures3.SelectedIndex < 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "都道府県"))
                            SetErr(Me.cboPrefectures3)
                        End If
                        ' 市区町村
                        If ChkNull(Me.txtCities3.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "市区町村"))
                            SetErr(Me.txtCities3)
                        End If
                        ' 番地等
                        If ChkNull(Me.txtAddAther3.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "番地等"))
                            SetErr(Me.txtAddAther3)
                        End If
                        ' 電話番号1-1
                        If ChkNull(Me.txtTel3_1_1.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号１(市外局番)"))
                            SetErr(Me.txtTel3_1_1)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel3_1_1.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号１(市外局番)"))
                                SetErr(Me.txtTel3_1_1)
                            End If
                        End If
                        ' 電話番号1-2
                        If ChkNull(Me.txtTel3_1_2.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号１(加入者番号)"))
                            SetErr(Me.txtTel3_1_2)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel3_1_2.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号１(加入者番号)"))
                                SetErr(Me.txtTel3_1_2)
                            End If
                        End If
                        ' 電話番号1-3
                        If ChkNull(Me.txtTel3_1_3.Text) Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号１(市内局番)"))
                            SetErr(Me.txtTel3_1_3)
                        Else
                            ' 数値チェック
                            If ChkNumber(Me.txtTel3_1_3.Text) = False Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号１(市内局番)"))
                                SetErr(Me.txtTel3_1_3)
                            End If
                        End If
                        ' 電話番号2
                        If (Me.txtTel3_2_1.Text & Me.txtTel3_2_2.Text & Me.txtTel3_2_3.Text).Length <> 0 Then
                            ' 電話番号2-1
                            If ChkNull(Me.txtTel3_2_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号２(市外局番)"))
                                SetErr(Me.txtTel3_2_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_2_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号２(市外局番)"))
                                    SetErr(Me.txtTel3_2_1)
                                End If
                            End If
                            ' 電話番号2-2
                            If ChkNull(Me.txtTel3_2_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号２(加入者番号)"))
                                SetErr(Me.txtTel3_2_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_2_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号２(加入者番号)"))
                                    SetErr(Me.txtTel3_2_2)
                                End If
                            End If
                            ' 電話番号2-3
                            If ChkNull(Me.txtTel3_2_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号２(市内局番)"))
                                SetErr(Me.txtTel3_2_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_2_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号２(市内局番)"))
                                    SetErr(Me.txtTel3_2_3)
                                End If
                            End If
                        End If
                        ' 電話番号3
                        If (Me.txtTel3_3_1.Text & Me.txtTel3_3_2.Text & Me.txtTel3_3_3.Text).Length <> 0 Then
                            ' 電話番号3-1
                            If ChkNull(Me.txtTel3_3_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号３(市外局番)"))
                                SetErr(Me.txtTel3_3_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_3_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号３(市外局番)"))
                                    SetErr(Me.txtTel3_3_1)
                                End If
                            End If
                            ' 電話番号3-2
                            If ChkNull(Me.txtTel3_3_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号３(加入者番号)"))
                                SetErr(Me.txtTel3_3_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_3_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号３(加入者番号)"))
                                    SetErr(Me.txtTel3_3_2)
                                End If
                            End If
                            ' 電話番号3-3
                            If ChkNull(Me.txtTel3_3_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号３(市内局番)"))
                                SetErr(Me.txtTel3_3_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtTel3_3_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "電話番号３(市内局番)"))
                                    SetErr(Me.txtTel3_3_3)
                                End If
                            End If
                        End If
                        ' FAX
                        If (Me.txtFax3_1.Text & Me.txtFax3_2.Text & Me.txtFax3_3.Text).Length <> 0 Then
                            ' FAX1-1
                            If ChkNull(Me.txtFax3_1.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "FAX(市外局番)"))
                                SetErr(Me.txtFax3_1)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax3_1.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "FAX(市外局番)"))
                                    SetErr(Me.txtFax3_1)
                                End If
                            End If
                            ' FAX1-2
                            If ChkNull(Me.txtFax3_2.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "FAX(加入者番号)"))
                                SetErr(Me.txtTel3_2)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax3_2.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "FAX(加入者番号)"))
                                    SetErr(Me.txtFax3_2)
                                End If
                            End If
                            ' FAX1-3
                            If ChkNull(Me.txtFax3_3.Text) Then
                                arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "FAX(市内局番)"))
                                SetErr(Me.txtTel3_3)
                            Else
                                ' 数値チェック
                                If ChkNumber(Me.txtFax3_3.Text) = False Then
                                    arlErrMsg.Add(CLMsg.GetMsg("GE0019", ERR_ADDRESS3 & "FAX(市内局番)"))
                                    SetErr(Me.txtFax3_3)
                                End If
                            End If
                        End If
                    End If
                End If

            ElseIf Me.cboInternational3.SelectedValue = INTERNATIONAL_KBN_ABROAD Then
                '---------------------------------------------------------------
                '   海外情報
                '---------------------------------------------------------------
                If Me.cboAddressKbn3.SelectedIndex <> 0 Then
                    strChk3 = Me.txtForeignAdress3_1.Text & _
                              Me.txtForeignAdress3_2.Text & _
                              Me.txtForeignAdress3_3.Text & _
                              Me.txtForeignAdress3_4.Text & _
                              Me.txtForeignAdress3_5.Text & _
                              Me.txtTel3_1.Text & Me.txtTel3_2.Text & Me.txtTel3_3.Text & _
                              Me.txtFax3.Text
                    If strChk3.Length <> 0 Then
                        ' アドレス1
                        If Me.txtForeignAdress3_1.Text.Length = 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "アドレス1"))
                            SetErr(Me.txtForeignAdress3_1)
                        End If
                        ' 電話番号1
                        If Me.txtTel3_1.Text.Length = 0 Then
                            arlErrMsg.Add(CLMsg.GetMsg("GE0006", ERR_ADDRESS3 & "電話番号1"))
                            SetErr(Me.txtTel3_1)
                        End If
                    End If
                End If
            End If

            '===============================================================================
            '   複数エラーメッセージ表示画面表示
            '===============================================================================
            ' エラーメッセージ存在チェック
            If Not arlErrMsg.Count = 0 Then
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
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As New CLAccessMdb                    ' データベースクラス生成
        Dim strSql As String = ""                       ' SQL

        Try
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            ' データベース接続
            Call clsDb.Connect()

            '===========================================================================
            '   コンボボックス作成
            '===========================================================================
            '---------------------------------------
            '   住所1
            '---------------------------------------
            ' 住所区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboAddressKbn1, _
                                    CONSTANT_ID_ADD_KIND, _
                                    True, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If
            ' 国内海外区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboInternational1, _
                                    CONSTANT_ID_INTERNATIONAL_KBN, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If

            '---------------------------------------
            '   住所2
            '---------------------------------------
            ' 住所区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboAddressKbn2, _
                                    CONSTANT_ID_ADD_KIND, _
                                    True, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If
            ' 国内海外区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboInternational2, _
                                    CONSTANT_ID_INTERNATIONAL_KBN, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If

            '---------------------------------------
            '   住所3
            '---------------------------------------
            ' 住所区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboAddressKbn3, _
                                    CONSTANT_ID_ADD_KIND, _
                                    True, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If
            ' 国内海外区分
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboInternational3, _
                                    CONSTANT_ID_INTERNATIONAL_KBN, _
                                    False, _
                                    MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                    0) = False Then
                Return blnRet
            End If

            '===============================================================================
            '   基本情報
            '===============================================================================
            ' 基本情報取得処理
            If GetStafAttribute(clsDb) = False Then
                Return blnRet
            End If

            '===============================================================================
            '   住所情報
            '===============================================================================
            If Me.bytStatus = STATUS_UPDATE Then
                ' 内容変更
                If GetStafAddress(clsDb, 2) = False Then
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
    '   ＩＤ　：InsertUpdate
    '   名称　：住所情報更新処理（登録・更新・削除）
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所登録処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdate() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス
        Dim strSql As String = ""               ' SQL文

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' トランザクション開始処理
            Call clsDb.BeginTran()

            If bytHistoryFlg = 0 Then
                ' 通常登録の場合
                ' ステータス判定
                If Me.bytStatus = STATUS_INSERT Then
                    '===================================================================
                    '   登録処理
                    '===================================================================
                    ' 住所区分判定
                    If Me.cboAddressKbn1.SelectedIndex > 0 Then
                        '---------------------------------------------------------------
                        '   住所1
                        '---------------------------------------------------------------
                        If InsertStafAddress1(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If
                    ' 住所区分判定
                    If Me.cboAddressKbn2.SelectedIndex > 0 Then
                        '---------------------------------------------------------------
                        '   住所2
                        '---------------------------------------------------------------
                        If InsertStafAddress2(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If
                    ' 住所区分判定
                    If Me.cboAddressKbn3.SelectedIndex > 0 Then
                        '---------------------------------------------------------------
                        '   住所3
                        '---------------------------------------------------------------
                        If InsertStafAddress3(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If

                ElseIf Me.bytStatus = STATUS_UPDATE Then
                    '===================================================================
                    '   更新処理
                    '===================================================================
                    '-------------------------------------------------------------------
                    '   住所1
                    '-------------------------------------------------------------------
                    If Me.cboAddressKbn1.SelectedIndex >= 0 Then
                        ' 住所区分判定
                        If Me.cboAddressKbn1.SelectedIndex = 0 Then
                            ' 住所1存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB1) Then
                                ' 住所1削除処理
                                If DeleteAddress(clsDb, ADDRESS_TAB1) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        Else
                            ' 住所1存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB1) Then
                                ' 更新処理
                                If UpdateAddress1(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            Else
                                ' 登録処理
                                If InsertStafAddress1(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        End If
                    End If

                    '-------------------------------------------------------------------
                    '   住所2
                    '-------------------------------------------------------------------
                    If Me.cboAddressKbn2.SelectedIndex >= 0 Then
                        ' 住所区分判定
                        If Me.cboAddressKbn2.SelectedIndex = 0 Then
                            ' 住所2存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB2) Then
                                ' 住所2削除処理
                                If DeleteAddress(clsDb, ADDRESS_TAB2) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        Else
                            ' 住所2存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB2) Then
                                ' 更新処理
                                If UpdateAddress2(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            Else
                                ' 登録処理
                                If InsertStafAddress2(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        End If
                    End If

                    '-------------------------------------------------------------------
                    '   住所3
                    '-------------------------------------------------------------------
                    If Me.cboAddressKbn3.SelectedIndex >= 0 Then
                        ' 住所区分判定
                        If Me.cboAddressKbn3.SelectedIndex = 0 Then
                            ' 住所1存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB3) Then
                                ' 住所1削除処理
                                If DeleteAddress(clsDb, ADDRESS_TAB3) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        Else
                            ' 住所1存在チェック処理
                            If ExistsStafAddress(clsDb, ADDRESS_TAB3) Then
                                ' 更新処理
                                If UpdateAddress3(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            Else
                                ' 登録処理
                                If InsertStafAddress3(clsDb) = False Then
                                    Call clsDb.RollbackTran()                           ' トランザクション取消処理
                                    Return blnRet
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                ' 履歴新規登録の場合
                '-----------------------------------------------------------------------
                '   住所１情報登録処理
                '-----------------------------------------------------------------------
                ' 住所区分判定
                If Me.cboAddressKbn1.SelectedIndex > 0 Then
                    ' 住所情報存在チェック処理
                    If ExistsStafAddress(clsDb, ADDRESS_TAB1) = False Then
                        ' 住所情報が存在しない場合、登録処理
                        If InsertStafAddress1(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If
                End If

                '-----------------------------------------------------------------------
                '   住所２情報登録処理
                '-----------------------------------------------------------------------
                ' 住所区分判定
                If Me.cboAddressKbn2.SelectedIndex > 0 Then
                    ' 住所情報存在チェック処理
                    If ExistsStafAddress(clsDb, ADDRESS_TAB2) = False Then
                        ' 住所情報が存在しない場合、登録処理
                        If InsertStafAddress2(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If
                End If

                '-----------------------------------------------------------------------
                '   住所３情報登録処理
                '-----------------------------------------------------------------------
                ' 住所区分判定
                If Me.cboAddressKbn3.SelectedIndex > 0 Then
                    ' 住所情報存在チェック処理
                    If ExistsStafAddress(clsDb, ADDRESS_TAB2) = False Then
                        ' 住所情報が存在しない場合、登録処理
                        If InsertStafAddress3(clsDb) = False Then
                            Call clsDb.RollbackTran()                                   ' トランザクション取消処理
                            Return blnRet
                        End If
                    End If
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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertStafAddress1
    '   名称　：住所1登録処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所1登録処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafAddress1(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim intRet As Integer = 0                       ' 処理件数
        Dim strInsUserId As String = ""                 ' 個人認証ID
        Dim intInsSeq As Integer = 0                    ' 住所SEQ
        Dim strInsUseDate As String = ""                ' 適用日付

        Try
            ' 各情報取得
            strInsUserId = Me.txtMemberNo.Text.Trim
            intInsSeq = 1
            strInsUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO staf_address ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                     ' 01. 個人認証ID
            strSql = strSql & "   ,s_seq" & vbCrLf                         ' 02. 住所SEQ
            strSql = strSql & "   ,d_from" & vbCrLf                        ' 03. 適用開始年月日
            strSql = strSql & "   ,k_add_kind" & vbCrLf                    ' 04. 住所種別
            strSql = strSql & "   ,k_main_add" & vbCrLf                    ' 05. 現住所FLG
            strSql = strSql & "   ,l_add_number" & vbCrLf                  ' 06. 郵便番号
            strSql = strSql & "   ,l_prefectures" & vbCrLf                 ' 07. 都道府県
            strSql = strSql & "   ,l_cities" & vbCrLf                      ' 08. 地区町村
            strSql = strSql & "   ,l_add_ather" & vbCrLf                   ' 09. 番地等
            strSql = strSql & "   ,l_building" & vbCrLf                    ' 10. 建物名等
            strSql = strSql & "   ,k_foreign" & vbCrLf                     ' 11. 海外区分
            strSql = strSql & "   ,l_foreign_adress_1" & vbCrLf            ' 12. アドレス1
            strSql = strSql & "   ,l_foreign_adress_2" & vbCrLf            ' 13. アドレス2
            strSql = strSql & "   ,l_foreign_adress_3" & vbCrLf            ' 14. アドレス3
            strSql = strSql & "   ,l_foreign_adress_4" & vbCrLf            ' 15. アドレス4
            strSql = strSql & "   ,l_foreign_adress_5" & vbCrLf            ' 16. アドレス5
            strSql = strSql & "   ,l_tell_1" & vbCrLf                      ' 17. 電話番号1
            strSql = strSql & "   ,l_tell_2" & vbCrLf                      ' 18. 電話番号2
            strSql = strSql & "   ,l_tell_3" & vbCrLf                      ' 19. 電話番号3
            strSql = strSql & "   ,l_fax" & vbCrLf                         ' 20. FAX
            strSql = strSql & "   ,l_mail_pc" & vbCrLf                     ' 21. メールアドレスPC
            strSql = strSql & "   ,l_mail_mobile" & vbCrLf                 ' 22. メールアドレス携帯
            strSql = strSql & "   ,k_del" & vbCrLf                         ' 23. 削除区分
            strSql = strSql & "   ,l_biko_1" & vbCrLf                      ' 24. 備考
            strSql = strSql & "   ,d_ins" & vbCrLf                         ' 25. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                 ' 26. 作成者個人ID
            strSql = strSql & "   ,d_up" & vbCrLf                          ' 27. 更新日
            strSql = strSql & "   ,c_user_id_up" & vbCrLf                  ' 28. 更新者個人ID
            strSql = strSql & "   ,s_up" & vbCrLf                          ' 29. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            strSql = strSql & "    '" & strInsUserId & "'" & vbCrLf                                                 ' 01. 個人認証ID
            strSql = strSql & "   ," & intInsSeq & vbCrLf                                                           ' 02. 住所SEQ
            strSql = strSql & "   ,'" & strInsUseDate & "'" & vbCrLf                                                ' 03. 適用開始年月日
            strSql = strSql & "   ,'" & Me.cboAddressKbn1.SelectedValue.ToString() & "'" & vbCrLf                   ' 04. 住所種別
            strSql = strSql & "   ,'" & Me.chkMainAddress1.Checked & "'"                                            ' 05. 現住所FLG

            ' 国内海外区分判定
            If Me.cboInternational1.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '=======================================================================
                '    国内
                '=======================================================================
                strSql = strSql & "   ,'" & Me.txtPostalNo1_1.Text & "-" & Me.txtPostalNo1_2.Text & "'" & vbCrLf    ' 06. 郵便番号
                strSql = strSql & "   ,'" & Me.cboPrefectures1.Text & "'" & vbCrLf                                  ' 07. 都道府県
                strSql = strSql & "   ,'" & Me.txtCities1.Text & "'" & vbCrLf                                       ' 08. 地区町村
                strSql = strSql & "   ,'" & Me.txtAddAther1.Text & "'" & vbCrLf                                     ' 09. 番地等
                strSql = strSql & "   ,'" & Me.txtBuilding1.Text & "'" & vbCrLf                                     ' 10. 建物名等
                strSql = strSql & "   ,'" & Me.cboInternational1.SelectedValue.ToString() & "'" & vbCrLf            ' 11. 海外区分
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 12. アドレス1
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 13. アドレス2
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 14. アドレス3
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 15. アドレス4
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel1_1_1.Text & "-" & Me.txtTel1_1_2.Text & "-" & Me.txtTel1_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel1_2_1.Text & Me.txtTel1_2_2.Text & Me.txtTel1_2_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel1_2_1.Text & "-" & Me.txtTel1_2_2.Text & "-" & Me.txtTel1_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel1_3_1.Text & Me.txtTel1_3_2.Text & Me.txtTel1_3_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel1_3_1.Text & "-" & Me.txtTel1_3_2.Text & "-" & Me.txtTel1_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax1_1.Text & Me.txtFax1_2.Text & Me.txtFax1_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax1_1.Text & "-" & Me.txtFax1_2.Text & "-" & Me.txtFax1_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational1.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '===========================================================================
                '   海外
                '===========================================================================
                strSql = strSql & "   ,''" & vbCrLf                                         ' 06. 郵便番号
                strSql = strSql & "   ,''" & vbCrLf                                         ' 07. 都道府県
                strSql = strSql & "   ,''" & vbCrLf                                         ' 08. 地区町村
                strSql = strSql & "   ,''" & vbCrLf                                         ' 09. 番地等
                strSql = strSql & "   ,''" & vbCrLf                                         ' 10. 建物名等
                ' 11. 海外区分
                strSql = strSql & "   ,'" & Me.cboInternational1.SelectedValue.ToString() & "'" & vbCrLf
                ' 12. アドレス1
                strSql = strSql & "   ,'" & Me.txtForeignAdress1_1.Text & "'" & vbCrLf
                ' 13. アドレス2
                If Me.txtForeignAdress1_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress1_2.Text & "'" & vbCrLf
                End If
                ' 14. アドレス3
                If Me.txtForeignAdress1_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress1_3.Text & "'" & vbCrLf
                End If
                ' 15. アドレス4
                If Me.txtForeignAdress1_4.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress1_4.Text & "'" & vbCrLf
                End If
                ' 16. アドレス5
                If Me.txtForeignAdress1_5.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress1_5.Text & "'" & vbCrLf
                End If
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel1_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel1_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel1_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel1_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel1_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax1.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax1.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc1.Text.Length = 0 Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailPc1.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile1.Text.Length = 0 Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailMobile1.Text & "'" & vbCrLf
            End If
            ' 23. 削除区分
            strSql = strSql & "    ,'0'" & vbCrLf
            ' 24. 備考
            If Me.txtNote1.Text.Length = 0 Then
                strSql = strSql & "   ,''" & vbCrLf
            Else
                strSql = strSql & "   ,'" & Me.txtNote1.Text & "'" & vbCrLf
            End If
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf                               ' 25. 作成日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf                  ' 26. 作成者個人ＩＤ
            strSql = strSql & "    ,Null" & vbCrLf                                          ' 27. 更新日
            strSql = strSql & "    ,''" & vbCrLf                                            ' 28. 更新者個人ＩＤ
            strSql = strSql & "    ,0" & vbCrLf                                             ' 29. 更新回数
            strSql = strSql & " );" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：InsertStafAddress2
    '   名称　：住所2登録処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所2情報登録処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafAddress2(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim intRet As Integer = 0                           ' 処理件数
        Dim strInsUserId As String = ""                     ' 個人認証ID
        Dim intInsSeq As Integer = 0                        ' 住所SEQ
        Dim strInsUseDate As String = ""                    ' 適用日付

        Try
            ' 各情報取得
            strInsUserId = Me.txtMemberNo.Text.Trim
            intInsSeq = 2
            strInsUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO staf_address ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                     ' 01. 個人認証ID
            strSql = strSql & "   ,s_seq" & vbCrLf                         ' 02. 住所SEQ
            strSql = strSql & "   ,d_from" & vbCrLf                        ' 03. 適用開始年月日
            strSql = strSql & "   ,k_add_kind" & vbCrLf                    ' 04. 住所種別
            strSql = strSql & "   ,k_main_add" & vbCrLf                    ' 05. 現住所FLG
            strSql = strSql & "   ,l_add_number" & vbCrLf                  ' 06. 郵便番号
            strSql = strSql & "   ,l_prefectures" & vbCrLf                 ' 07. 都道府県
            strSql = strSql & "   ,l_cities" & vbCrLf                      ' 08. 地区町村
            strSql = strSql & "   ,l_add_ather" & vbCrLf                   ' 09. 番地等
            strSql = strSql & "   ,l_building" & vbCrLf                    ' 10. 建物名等
            strSql = strSql & "   ,k_foreign" & vbCrLf                     ' 11. 海外区分
            strSql = strSql & "   ,l_foreign_adress_1" & vbCrLf            ' 12. アドレス1
            strSql = strSql & "   ,l_foreign_adress_2" & vbCrLf            ' 13. アドレス2
            strSql = strSql & "   ,l_foreign_adress_3" & vbCrLf            ' 14. アドレス3
            strSql = strSql & "   ,l_foreign_adress_4" & vbCrLf            ' 15. アドレス4
            strSql = strSql & "   ,l_foreign_adress_5" & vbCrLf            ' 16. アドレス5
            strSql = strSql & "   ,l_tell_1" & vbCrLf                      ' 17. 電話番号1
            strSql = strSql & "   ,l_tell_2" & vbCrLf                      ' 18. 電話番号2
            strSql = strSql & "   ,l_tell_3" & vbCrLf                      ' 19. 電話番号3
            strSql = strSql & "   ,l_fax" & vbCrLf                         ' 20. FAX
            strSql = strSql & "   ,l_mail_pc" & vbCrLf                     ' 21. メールアドレスPC
            strSql = strSql & "   ,l_mail_mobile" & vbCrLf                 ' 22. メールアドレス携帯
            strSql = strSql & "   ,k_del" & vbCrLf                         ' 23. 削除区分
            strSql = strSql & "   ,l_biko_1" & vbCrLf                      ' 24. 備考
            strSql = strSql & "   ,d_ins" & vbCrLf                         ' 25. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                 ' 26. 作成者個人ID
            strSql = strSql & "   ,d_up" & vbCrLf                          ' 27. 更新日
            strSql = strSql & "   ,c_user_id_up" & vbCrLf                  ' 28. 更新者個人ID
            strSql = strSql & "   ,s_up" & vbCrLf                          ' 29. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            strSql = strSql & "    '" & strInsUserId & "'" & vbCrLf                                                 ' 01. 個人認証ID
            strSql = strSql & "   ," & intInsSeq & vbCrLf                                                           ' 02. 住所SEQ
            strSql = strSql & "   ,'" & strInsUseDate & "'" & vbCrLf                                                ' 03. 適用開始年月日
            strSql = strSql & "   ,'" & Me.cboAddressKbn2.SelectedValue.ToString() & "'" & vbCrLf                   ' 04. 住所種別
            strSql = strSql & "   ,'" & Me.chkMainAddress2.Checked & "'"                                            ' 05. 現住所FLG

            ' 国内海外区分判定
            If Me.cboInternational2.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '=======================================================================
                '    国内
                '=======================================================================
                strSql = strSql & "   ,'" & Me.txtPostalNo2_1.Text & "-" & Me.txtPostalNo2_2.Text & "'" & vbCrLf    ' 06. 郵便番号
                strSql = strSql & "   ,'" & Me.cboPrefectures2.Text & "'" & vbCrLf                                  ' 07. 都道府県
                strSql = strSql & "   ,'" & Me.txtCities2.Text & "'" & vbCrLf                                       ' 08. 地区町村
                strSql = strSql & "   ,'" & Me.txtAddAther2.Text & "'" & vbCrLf                                     ' 09. 番地等
                strSql = strSql & "   ,'" & Me.txtBuilding2.Text & "'" & vbCrLf                                     ' 10. 建物名等
                strSql = strSql & "   ,'" & Me.cboInternational2.SelectedValue.ToString() & "'" & vbCrLf            ' 11. 海外区分
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 12. アドレス1
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 13. アドレス2
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 14. アドレス3
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 15. アドレス4
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel2_1_1.Text & "-" & Me.txtTel2_1_2.Text & "-" & Me.txtTel2_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel2_2_1.Text & Me.txtTel2_2_2.Text & Me.txtTel2_2_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel2_2_1.Text & "-" & Me.txtTel2_2_2.Text & "-" & Me.txtTel2_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel2_3_1.Text & Me.txtTel2_3_2.Text & Me.txtTel2_3_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel2_3_1.Text & "-" & Me.txtTel2_3_2.Text & "-" & Me.txtTel2_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax2_1.Text & Me.txtFax2_2.Text & Me.txtFax2_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax2_1.Text & "-" & Me.txtFax2_2.Text & "-" & Me.txtFax2_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational2.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '=======================================================================
                '   海外
                '=======================================================================
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 06. 郵便番号
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 07. 都道府県
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 08. 地区町村
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 09. 番地等
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 10. 建物名等
                strSql = strSql & "   ,'" & Me.cboInternational2.SelectedValue.ToString() & "'" & vbCrLf            ' 11. 海外区分
                ' 12. アドレス1
                strSql = strSql & "   ,'" & Me.txtForeignAdress2_1.Text & "'" & vbCrLf
                ' 13. アドレス2
                If Me.txtForeignAdress2_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress2_2.Text & "'" & vbCrLf
                End If
                ' 14. アドレス3
                If Me.txtForeignAdress2_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress2_3.Text & "'" & vbCrLf
                End If
                ' 15. アドレス4
                If Me.txtForeignAdress2_4.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress2_4.Text & "'" & vbCrLf
                End If
                ' 16. アドレス5
                If Me.txtForeignAdress2_5.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress2_5.Text & "'" & vbCrLf
                End If
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel2_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel2_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel2_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel2_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel2_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax2.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc2.Text.Length = 0 Then
                strSql = strSql & "    ,Null" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailPc2.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile2.Text.Length = 0 Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailMobile2.Text & "'" & vbCrLf
            End If
            ' 23. 削除区分
            strSql = strSql & "    ,'0'" & vbCrLf
            ' 24. 備考
            If Me.txtNote1.Text.Length = 0 Then
                strSql = strSql & "   ,''" & vbCrLf
            Else
                strSql = strSql & "   ,'" & Me.txtNote1.Text & "'" & vbCrLf
            End If
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf                                                       ' 25. 作成日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf                                          ' 26. 作成者個人ＩＤ
            strSql = strSql & "    ,Null" & vbCrLf                                                                  ' 27. 更新日
            strSql = strSql & "    ,''" & vbCrLf                                                                    ' 28. 更新者個人ＩＤ
            strSql = strSql & "    ,0" & vbCrLf                                                                     ' 29. 更新回数
            strSql = strSql & " );" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：InsertStafAddress3
    '   名称　：住所3登録処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所3登録処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafAddress3(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim intRet As Integer = 0                           ' 処理件数
        Dim strInsUserId As String = ""                     ' 個人認証ID
        Dim intInsSeq As Integer = 0                        ' 住所SEQ
        Dim strInsUseDate As String = ""                    ' 適用日付

        Try
            ' 各情報取得
            strInsUserId = Me.txtMemberNo.Text.Trim
            intInsSeq = 3
            strInsUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO staf_address ( " & vbCrLf
            strSql = strSql & "    c_user_id" & vbCrLf                     ' 01. 個人認証ID
            strSql = strSql & "   ,s_seq" & vbCrLf                         ' 02. 住所SEQ
            strSql = strSql & "   ,d_from" & vbCrLf                        ' 03. 適用開始年月日
            strSql = strSql & "   ,k_add_kind" & vbCrLf                    ' 04. 住所種別
            strSql = strSql & "   ,k_main_add" & vbCrLf                    ' 05. 現住所FLG
            strSql = strSql & "   ,l_add_number" & vbCrLf                  ' 06. 郵便番号
            strSql = strSql & "   ,l_prefectures" & vbCrLf                 ' 07. 都道府県
            strSql = strSql & "   ,l_cities" & vbCrLf                      ' 08. 地区町村
            strSql = strSql & "   ,l_add_ather" & vbCrLf                   ' 09. 番地等
            strSql = strSql & "   ,l_building" & vbCrLf                    ' 10. 建物名等
            strSql = strSql & "   ,k_foreign" & vbCrLf                     ' 11. 海外区分
            strSql = strSql & "   ,l_foreign_adress_1" & vbCrLf            ' 12. アドレス1
            strSql = strSql & "   ,l_foreign_adress_2" & vbCrLf            ' 13. アドレス2
            strSql = strSql & "   ,l_foreign_adress_3" & vbCrLf            ' 14. アドレス3
            strSql = strSql & "   ,l_foreign_adress_4" & vbCrLf            ' 15. アドレス4
            strSql = strSql & "   ,l_foreign_adress_5" & vbCrLf            ' 16. アドレス5
            strSql = strSql & "   ,l_tell_1" & vbCrLf                      ' 17. 電話番号1
            strSql = strSql & "   ,l_tell_2" & vbCrLf                      ' 18. 電話番号2
            strSql = strSql & "   ,l_tell_3" & vbCrLf                      ' 19. 電話番号3
            strSql = strSql & "   ,l_fax" & vbCrLf                         ' 20. FAX
            strSql = strSql & "   ,l_mail_pc" & vbCrLf                     ' 21. メールアドレスPC
            strSql = strSql & "   ,l_mail_mobile" & vbCrLf                 ' 22. メールアドレス携帯
            strSql = strSql & "   ,k_del" & vbCrLf                         ' 23. 削除区分
            strSql = strSql & "   ,l_biko_1" & vbCrLf                      ' 24. 備考
            strSql = strSql & "   ,d_ins" & vbCrLf                         ' 25. 作成日
            strSql = strSql & "   ,c_user_id_ins" & vbCrLf                 ' 26. 作成者個人ID
            strSql = strSql & "   ,d_up" & vbCrLf                          ' 27. 更新日
            strSql = strSql & "   ,c_user_id_up" & vbCrLf                  ' 28. 更新者個人ID
            strSql = strSql & "   ,s_up" & vbCrLf                          ' 29. 更新回数
            strSql = strSql & " ) VALUES ( " & vbCrLf
            strSql = strSql & "    '" & strInsUserId & "'" & vbCrLf                                                 ' 01. 個人認証ID
            strSql = strSql & "   ," & intInsSeq & vbCrLf                                                           ' 02. 住所SEQ
            strSql = strSql & "   ,'" & strInsUseDate & "'" & vbCrLf                                                ' 03. 適用開始年月日
            strSql = strSql & "   ,'" & Me.cboAddressKbn3.SelectedValue.ToString() & "'" & vbCrLf                   ' 04. 住所種別
            strSql = strSql & "   ,'" & Me.chkMainAddress3.Checked & "'"                                            ' 05. 現住所FLG

            ' 国内海外区分判定
            If Me.cboInternational3.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '=======================================================================
                '    国内
                '=======================================================================
                strSql = strSql & "   ,'" & Me.txtPostalNo3_1.Text & "-" & Me.txtPostalNo3_2.Text & "'" & vbCrLf    ' 06. 郵便番号
                strSql = strSql & "   ,'" & Me.cboPrefectures3.Text & "'" & vbCrLf                                  ' 07. 都道府県
                strSql = strSql & "   ,'" & Me.txtCities3.Text & "'" & vbCrLf                                       ' 08. 地区町村
                strSql = strSql & "   ,'" & Me.txtAddAther3.Text & "'" & vbCrLf                                     ' 09. 番地等
                strSql = strSql & "   ,'" & Me.txtBuilding3.Text & "'" & vbCrLf                                     ' 10. 建物名等
                strSql = strSql & "   ,'" & Me.cboInternational3.SelectedValue.ToString() & "'" & vbCrLf            ' 11. 海外区分
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 12. アドレス1
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 13. アドレス2
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 14. アドレス3
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 15. アドレス4
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel3_1_1.Text & "-" & Me.txtTel3_1_2.Text & "-" & Me.txtTel3_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel3_2_1.Text & Me.txtTel3_2_2.Text & Me.txtTel3_2_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel3_2_1.Text & "-" & Me.txtTel3_2_2.Text & "-" & Me.txtTel3_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel3_3_1.Text & Me.txtTel3_3_2.Text & Me.txtTel3_3_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel3_3_1.Text & "-" & Me.txtTel3_3_2.Text & "-" & Me.txtTel3_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax3_1.Text & Me.txtFax3_2.Text & Me.txtFax3_3.Text).Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax3_1.Text & "-" & Me.txtFax3_2.Text & "-" & Me.txtFax3_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational3.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '=======================================================================
                '   海外
                '=======================================================================
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 06. 郵便番号
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 07. 都道府県
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 08. 地区町村
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 09. 番地等
                strSql = strSql & "   ,''" & vbCrLf                                                                 ' 10. 建物名等
                strSql = strSql & "   ,'" & Me.cboInternational3.SelectedValue.ToString() & "'" & vbCrLf            ' 11. 海外区分
                ' 12. アドレス1
                strSql = strSql & "   ,'" & Me.txtForeignAdress3_1.Text & "'" & vbCrLf
                ' 13. アドレス2
                If Me.txtForeignAdress3_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress3_2.Text & "'" & vbCrLf
                End If
                ' 14. アドレス3
                If Me.txtForeignAdress3_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress3_3.Text & "'" & vbCrLf
                End If
                ' 15. アドレス4
                If Me.txtForeignAdress3_4.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress3_4.Text & "'" & vbCrLf
                End If
                ' 16. アドレス5
                If Me.txtForeignAdress3_5.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtForeignAdress3_5.Text & "'" & vbCrLf
                End If
                ' 17. 電話番号1
                strSql = strSql & "   ,'" & Me.txtTel3_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel3_2.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel3_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel3_3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtTel3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax3.Text.Length = 0 Then
                    strSql = strSql & "   ,''" & vbCrLf
                Else
                    strSql = strSql & "   ,'" & Me.txtFax3.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc3.Text.Length = 0 Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailPc3.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile3.Text.Length = 0 Then
                strSql = strSql & "    ,''" & vbCrLf
            Else
                strSql = strSql & "    ,'" & Me.txtMailMobile3.Text & "'" & vbCrLf
            End If
            ' 23. 削除区分
            strSql = strSql & "    ,'0'" & vbCrLf
            ' 24. 備考
            If Me.txtNote1.Text.Length = 0 Then
                strSql = strSql & "   ,Null" & vbCrLf
            Else
                strSql = strSql & "   ,'" & Me.txtNote1.Text & "'" & vbCrLf
            End If
            strSql = strSql & "    ,'" & Now() & "'" & vbCrLf                                                   ' 25. 作成日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'" & vbCrLf                                      ' 26. 作成者個人ＩＤ
            strSql = strSql & "    ,Null" & vbCrLf                                                              ' 27. 更新日
            strSql = strSql & "    ,''" & vbCrLf                                                                ' 28. 更新者個人ＩＤ
            strSql = strSql & "    ,0" & vbCrLf                                                                 ' 29. 更新回数
            strSql = strSql & " );" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：UpdateAddress1
    '   名称　：住所1更新処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所1更新処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateAddress1(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim intRet As Integer = 0                       ' 処理件数
        Dim strUpdUserId As String = ""                 ' 社員番号
        Dim intUpdSeq As Integer = 0                    ' 住所SEQ
        Dim strUpdUseDate As String = ""                ' 適用日付

        Try
            ' 各情報取得
            strUpdUserId = Me.txtMemberNo.Text                                                          ' 社員番号
            intUpdSeq = 1                                                                               ' 住所SEQ
            strUpdUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE staf_address" & vbCrLf
            strSql = strSql & "    SET c_user_id          = '" & strUpdUserId & "'" & vbCrLf            ' 01. 社員番号
            strSql = strSql & "       ,s_seq              = " & intUpdSeq & vbCrLf                      ' 02. 住所SEQ
            strSql = strSql & "       ,d_from             = '" & strUpdUseDate & "'" & vbCrLf           ' 03. 適用日付
            strSql = strSql & "       ,k_add_kind         = '" & Me.cboAddressKbn1.SelectedValue.ToString() & "'" & vbCrLf                  ' 04. 住所種別
            strSql = strSql & "       ,k_main_add         = '" & Me.chkMainAddress1.Checked.ToString() & "'" & vbCrLf                       ' 05. 現住所FLG

            If Me.cboInternational1.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '===========================================================
                '   国内
                '===========================================================
                strSql = strSql & "       ,l_add_number       = '" & Me.txtPostalNo1_1.Text & "-" & Me.txtPostalNo1_2.Text & "'" & vbCrLf   ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = '" & Me.cboPrefectures1.Text & "'" & vbCrLf                                 ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = '" & Me.txtCities1.Text & "'" & vbCrLf                                      ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = '" & Me.txtAddAther1.Text & "'" & vbCrLf                                    ' 09. 番地等
                strSql = strSql & "       ,l_building         = '" & Me.txtBuilding1.Text & "'" & vbCrLf                                    ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational1.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = Null" & vbCrLf                                                              ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = Null" & vbCrLf                                                              ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = Null" & vbCrLf                                                              ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = Null" & vbCrLf                                                              ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = Null" & vbCrLf                                                              ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel1_1_1.Text & "-" & Me.txtTel1_1_2.Text & "-" & Me.txtTel1_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel1_2_1.Text & Me.txtTel1_2_2.Text & Me.txtTel1_2_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel1_2_1.Text & "-" & Me.txtTel1_2_2.Text & "-" & Me.txtTel1_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel1_3_1.Text & Me.txtTel1_3_2.Text & Me.txtTel1_3_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel1_3_1.Text & "-" & Me.txtTel1_3_2.Text & "-" & Me.txtTel1_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax1_1.Text & Me.txtFax1_2.Text & Me.txtFax1_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax1_1.Text & "-" & Me.txtFax1_2.Text & "-" & Me.txtFax1_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational1.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '===========================================================
                '   海外
                '===========================================================
                strSql = strSql & "       ,l_add_number       = ''" & vbCrLf                                                                ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = ''" & vbCrLf                                                                ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = ''" & vbCrLf                                                                ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = ''" & vbCrLf                                                                ' 09. 番地等
                strSql = strSql & "       ,l_building         = ''" & vbCrLf                                                                ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational1.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = '" & Me.txtForeignAdress1_1.Text & "'" & vbCrLf                             ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = '" & Me.txtForeignAdress1_2.Text & "'" & vbCrLf                             ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = '" & Me.txtForeignAdress1_3.Text & " '" & vbCrLf                            ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = '" & Me.txtForeignAdress1_4.Text & " '" & vbCrLf                            ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = '" & Me.txtForeignAdress1_5.Text & " '" & vbCrLf                            ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel1_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel1_2.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel1_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel1_3.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel1_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax1.Text.Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax1.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc1.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_pc          = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_pc          = '" & Me.txtMailPc1.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile1.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_mobile      = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_mobile      = '" & Me.txtMailMobile1.Text & "'" & vbCrLf
            End If
            ' 24. 備考
            If Me.txtNote1.Text.Length = 0 Then
                strSql = strSql & "       ,l_biko_1           = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_biko_1           = '" & Me.txtNote1.Text & "'" & vbCrLf
            End If
            strSql = strSql & "       ,d_up               = '" & Now() & "'" & vbCrLf                   ' 25. 更新日
            strSql = strSql & "       ,c_user_id_up       = '" & MDLoginInfo.UserId & "'" & vbCrLf      ' 26. 更新者個人ＩＤ
            strSql = strSql & "       ,s_up               = s_up + 1" & vbCrLf                          ' 27. 更新回数
            strSql = strSql & "  WHERE c_user_id = '" & strUpdUserId & "'" & vbCrLf                     ' 個人認証IDと同じもの
            strSql = strSql & "    AND s_seq = " & intUpdSeq & vbCrLf                                   ' 住所SEQと同じもの
            strSql = strSql & "    AND d_from = '" & strUpdUseDate & "'" & vbCrLf                       ' 適用日付と同じもの
            strSql = strSql & ";"

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：UpdateAddress2
    '   名称　：住所2更新処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所2更新処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateAddress2(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim intRet As Integer = 0                           ' 処理件数
        Dim strUpdUserId As String = ""                     ' 社員番号
        Dim intUpdSeq As Integer = 0                        ' 住所SEQ
        Dim strUpdUseDate As String = ""                    ' 適用日付

        Try
            ' 各情報取得
            strUpdUserId = Me.txtMemberNo.Text                                                          ' 社員番号
            intUpdSeq = 2                                                                               ' 住所SEQ
            strUpdUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE staf_address" & vbCrLf
            strSql = strSql & "    SET c_user_id          = '" & strUpdUserId & "'" & vbCrLf            ' 01. 社員番号
            strSql = strSql & "       ,s_seq              = " & intUpdSeq & vbCrLf                      ' 02. 住所SEQ
            strSql = strSql & "       ,d_from             = '" & strUpdUseDate & "'" & vbCrLf           ' 03. 適用日付
            strSql = strSql & "      , k_add_kind         = '" & Me.cboAddressKbn2.SelectedValue.ToString() & "'" & vbCrLf                  ' 04. 住所種別
            strSql = strSql & "       ,k_main_add         = '" & Me.chkMainAddress2.Checked.ToString() & "'" & vbCrLf                       ' 05. 現住所FLG

            If Me.cboInternational2.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '===========================================================
                '   国内
                '===========================================================
                strSql = strSql & "       ,l_add_number       = '" & Me.txtPostalNo2_1.Text & "-" & Me.txtPostalNo2_2.Text & "'" & vbCrLf   ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = '" & Me.cboPrefectures2.Text & "'" & vbCrLf                                 ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = '" & Me.txtCities2.Text & "'" & vbCrLf                                      ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = '" & Me.txtAddAther2.Text & "'" & vbCrLf                                    ' 09. 番地等
                strSql = strSql & "       ,l_building         = '" & Me.txtBuilding2.Text & "'" & vbCrLf                                    ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational2.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = ''" & vbCrLf                                                                ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = ''" & vbCrLf                                                                ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = ''" & vbCrLf                                                                ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = ''" & vbCrLf                                                                ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = ''" & vbCrLf                                                                ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel2_1_1.Text & "-" & Me.txtTel2_1_2.Text & "-" & Me.txtTel2_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel2_2_1.Text & Me.txtTel2_2_2.Text & Me.txtTel2_2_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel2_2_1.Text & "-" & Me.txtTel2_2_2.Text & "-" & Me.txtTel2_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel2_3_1.Text & Me.txtTel2_3_2.Text & Me.txtTel2_3_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel2_3_1.Text & "-" & Me.txtTel2_3_2.Text & "-" & Me.txtTel2_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax2_1.Text & Me.txtFax2_2.Text & Me.txtFax2_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax2_1.Text & "-" & Me.txtFax2_2.Text & "-" & Me.txtFax2_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational2.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '===========================================================
                '   海外
                '===========================================================
                strSql = strSql & "       ,l_add_number       = ''" & vbCrLf                                                                ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = ''" & vbCrLf                                                                ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = ''" & vbCrLf                                                                ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = ''" & vbCrLf                                                                ' 09. 番地等
                strSql = strSql & "       ,l_building         = ''" & vbCrLf                                                                ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational2.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = '" & Me.txtForeignAdress2_1.Text & "'" & vbCrLf                             ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = '" & Me.txtForeignAdress2_2.Text & "'" & vbCrLf                             ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = '" & Me.txtForeignAdress2_3.Text & " '" & vbCrLf                            ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = '" & Me.txtForeignAdress2_4.Text & " '" & vbCrLf                            ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = '" & Me.txtForeignAdress2_5.Text & " '" & vbCrLf                            ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel2_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel2_2.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel2_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel2_3.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel2_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax2.Text.Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax2.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc2.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_pc          = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_pc          = '" & Me.txtMailPc2.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile2.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_mobile      = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_pc          = '" & Me.txtMailMobile2.Text & "'" & vbCrLf
            End If
            ' 24. 備考
            If Me.txtNote2.Text.Length = 0 Then
                strSql = strSql & "       ,l_biko_1           = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_biko_1           = '" & Me.txtNote2.Text & "'" & vbCrLf
            End If
            strSql = strSql & "       ,d_up               = '" & Now() & "'" & vbCrLf                       ' 25. 更新日
            strSql = strSql & "       ,c_user_id_up       = '" & MDLoginInfo.UserId & "'" & vbCrLf          ' 26. 更新者個人ＩＤ
            strSql = strSql & "       ,s_up               = s_up + 1" & vbCrLf                              ' 27. 更新回数
            strSql = strSql & "  WHERE c_user_id = '" & strUpdUserId & "'" & vbCrLf                         ' 個人認証IDと同じもの
            strSql = strSql & "    AND s_seq = " & intUpdSeq & vbCrLf                                       ' 住所SEQと同じもの
            strSql = strSql & "    AND d_from = '" & strUpdUseDate & "'" & vbCrLf                           ' 適用日付と同じもの
            strSql = strSql & ";"

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：UpdateAddress3
    '   名称　：住所3更新処理
    '   概要  ：
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所3更新処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateAddress3(ByVal pClsDb As CLAccessMdb) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim intRet As Integer = 0                           ' 処理件数
        Dim strUpdUserId As String = ""                     ' 社員番号
        Dim intUpdSeq As Integer = 0                        ' 住所SEQ
        Dim strUpdUseDate As String = ""                    ' 適用日付

        Try
            ' 各情報取得
            strUpdUserId = Me.txtMemberNo.Text                                                          ' 社員番号
            intUpdSeq = 3                                                                               ' 住所SEQ
            strUpdUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE staf_address" & vbCrLf
            strSql = strSql & "    SET c_user_id          = '" & strUpdUserId & "'" & vbCrLf            ' 01. 社員番号
            strSql = strSql & "       ,s_seq              = " & intUpdSeq & vbCrLf                      ' 02. 住所SEQ
            strSql = strSql & "       ,d_from             = '" & strUpdUseDate & "'" & vbCrLf           ' 03. 適用日付
            strSql = strSql & "      ,k_add_kind          = '" & Me.cboAddressKbn3.SelectedValue.ToString() & "'" & vbCrLf                  ' 04. 住所種別
            strSql = strSql & "       ,k_main_add         = '" & Me.chkMainAddress3.Checked.ToString() & "'" & vbCrLf                       ' 05. 現住所FLG

            If Me.cboInternational3.SelectedValue.ToString() = INTERNATIONAL_KBN_HOME Then
                '===========================================================
                '   国内
                '===========================================================
                strSql = strSql & "       ,l_add_number       = '" & Me.txtPostalNo3_1.Text & "-" & Me.txtPostalNo3_2.Text & "'" & vbCrLf   ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = '" & Me.cboPrefectures3.Text & "'" & vbCrLf                                 ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = '" & Me.txtCities3.Text & "'" & vbCrLf                                      ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = '" & Me.txtAddAther3.Text & "'" & vbCrLf                                    ' 09. 番地等
                strSql = strSql & "       ,l_building         = '" & Me.txtBuilding3.Text & "'" & vbCrLf                                    ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational3.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = ''" & vbCrLf                                                                ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = ''" & vbCrLf                                                                ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = ''" & vbCrLf                                                                ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = ''" & vbCrLf                                                                ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = ''" & vbCrLf                                                                ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel3_1_1.Text & "-" & Me.txtTel3_1_2.Text & "-" & Me.txtTel3_1_3.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If (Me.txtTel3_2_1.Text & Me.txtTel3_2_2.Text & Me.txtTel3_2_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel3_2_1.Text & "-" & Me.txtTel3_2_2.Text & "-" & Me.txtTel3_2_3.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If (Me.txtTel3_3_1.Text & Me.txtTel3_3_2.Text & Me.txtTel3_3_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel3_3_1.Text & "-" & Me.txtTel3_3_2.Text & "-" & Me.txtTel3_3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If (Me.txtFax3_1.Text & Me.txtFax3_2.Text & Me.txtFax3_3.Text).Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax3_1.Text & "-" & Me.txtFax3_2.Text & "-" & Me.txtFax3_3.Text & "'" & vbCrLf
                End If

            ElseIf Me.cboInternational3.SelectedValue.ToString() = INTERNATIONAL_KBN_ABROAD Then
                '===========================================================
                '   海外
                '===========================================================
                strSql = strSql & "       ,l_add_number       = ''" & vbCrLf                                                                ' 06. 郵便番号
                strSql = strSql & "       ,l_prefectures      = ''" & vbCrLf                                                                ' 07. 都道府県
                strSql = strSql & "       ,l_cities           = ''" & vbCrLf                                                                ' 08. 地区町村
                strSql = strSql & "       ,l_add_ather        = ''" & vbCrLf                                                                ' 09. 番地等
                strSql = strSql & "       ,l_building         = ''" & vbCrLf                                                                ' 10. 建物名等
                strSql = strSql & "       ,k_foreign          = '" & Me.cboInternational3.SelectedValue.ToString() & "'" & vbCrLf           ' 11. 海外区分
                strSql = strSql & "       ,l_foreign_adress_1 = '" & Me.txtForeignAdress3_1.Text & "'" & vbCrLf                             ' 12. アドレス1
                strSql = strSql & "       ,l_foreign_adress_2 = '" & Me.txtForeignAdress3_2.Text & "'" & vbCrLf                             ' 13. アドレス2
                strSql = strSql & "       ,l_foreign_adress_3 = '" & Me.txtForeignAdress3_3.Text & " '" & vbCrLf                            ' 14. アドレス3
                strSql = strSql & "       ,l_foreign_adress_4 = '" & Me.txtForeignAdress3_4.Text & " '" & vbCrLf                            ' 15. アドレス4
                strSql = strSql & "       ,l_foreign_adress_5 = '" & Me.txtForeignAdress3_5.Text & " '" & vbCrLf                            ' 16. アドレス5
                ' 17. 電話番号1
                strSql = strSql & "       ,l_tell_1           = '" & Me.txtTel3_1.Text & "'" & vbCrLf
                ' 18. 電話番号2
                If Me.txtTel3_2.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_2           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_2           = '" & Me.txtTel3_2.Text & "'" & vbCrLf
                End If
                ' 19. 電話番号3
                If Me.txtTel3_3.Text.Length = 0 Then
                    strSql = strSql & "       ,l_tell_3           = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_tell_3           = '" & Me.txtTel3_3.Text & "'" & vbCrLf
                End If
                ' 20. FAX
                If Me.txtFax3.Text.Length = 0 Then
                    strSql = strSql & "       ,l_fax              = ''" & vbCrLf
                Else
                    strSql = strSql & "       ,l_fax              = '" & Me.txtFax3.Text & "'" & vbCrLf
                End If
            End If
            ' 21. メールアドレスPC
            If Me.txtMailPc3.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_pc          = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_pc          = '" & Me.txtMailPc3.Text & "'" & vbCrLf
            End If
            ' 22. メールアドレス携帯
            If Me.txtMailMobile3.Text.Length = 0 Then
                strSql = strSql & "       ,l_mail_mobile      = '" & Me.txtMailMobile3.Text & "'" & vbCrLf
            Else
                strSql = strSql & "       ,l_mail_mobile      = ''" & vbCrLf
            End If
            ' 24. 備考
            If Me.txtNote3.Text.Length = 0 Then
                strSql = strSql & "       ,l_biko_1           = ''" & vbCrLf
            Else
                strSql = strSql & "       ,l_biko_1           = '" & Me.txtNote3.Text & "'" & vbCrLf
            End If
            strSql = strSql & "       ,d_up               = '" & Now() & "'" & vbCrLf                       ' 25. 更新日
            strSql = strSql & "       ,c_user_id_up       = '" & MDLoginInfo.UserId & "'" & vbCrLf          ' 26. 更新者個人ＩＤ
            strSql = strSql & "       ,s_up               = s_up + 1" & vbCrLf                              ' 27. 更新回数
            strSql = strSql & "  WHERE c_user_id = '" & strUpdUserId & "'" & vbCrLf                         ' 個人認証IDと同じもの
            strSql = strSql & "    AND s_seq = " & intUpdSeq & vbCrLf                                       ' 住所SEQと同じもの
            strSql = strSql & "    AND d_from = '" & strUpdUseDate & "'" & vbCrLf                           ' 適用日付と同じもの
            strSql = strSql & ";"

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

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
    '   ＩＤ　：DeleteAddress
    '   名称　：住所削除処理
    '   概要  ：
    '   引数　：ByVal pClsDb         As CLAccessMdb = データベースクラス
    '           ByVal pIntAddressSeq As Integer     = 住所SEQ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/23(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所削除処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <param name="pIntAddressSeq">住所SEQ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteAddress(ByVal pClsDb As CLAccessMdb, _
                                   ByVal pIntAddressSeq As Integer) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim intRet As Integer = 0                           ' 処理件数
        Dim strDelUserId As String = ""                     ' 社員番号
        Dim strDelUseDate As String = ""                    ' 適用日付

        Try
            ' 各情報取得
            strDelUserId = Me.txtMemberNo.Text                                                          ' 社員番号
            strDelUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " DELETE" & vbCrLf
            strSql = strSql & "   FROM staf_address AS staf" & vbCrLf                                   ' 組合員住所情報
            strSql = strSql & "  WHERE staf.c_user_id = '" & strDelUserId & "'" & vbCrLf                ' 個人認証IDと同じもの
            strSql = strSql & "    AND staf.s_seq = " & pIntAddressSeq & vbCrLf                         ' 住所SEQと同じもの
            strSql = strSql & "    AND staf.d_from = '" & strDelUseDate & "'" & vbCrLf                  ' 適用日付と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)

            ' 処理件数確認
            'If intRet <> 1 Then                                                            
            '    Return blnRet
            'End If

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
    '   引数　：ByVal pClsDb         As CLAccessMdb = データベースクラス
    '           ByVal pIntAddressSeq As Integer     = 住所SEQ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/07(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所存在チェック処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <param name="pIntAddressSeq">住所SEQ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAddress(ByVal pClsDb As CLAccessMdb, _
                                       ByVal pIntAddressSeq As Integer) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim intRet As Integer = 0                       ' 処理件数
        Dim dtRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim strSelUserId As String = ""                 ' 社員番号
        Dim strSelUseDate As String = ""                ' 適用日付

        Try
            ' 各情報取得
            strSelUserId = Me.txtMemberNo.Text                                                          ' 社員番号
            strSelUseDate = Me.txtUseDate.Text.Replace("年", "").Replace("月", "").Replace("日", "")    ' 適用日付

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf.c_user_id" & vbCrLf
            strSql = strSql & "   FROM staf_address AS staf" & vbCrLf                                   ' 組合員住所情報
            strSql = strSql & "  WHERE staf.c_user_id = '" & strSelUserId & "'" & vbCrLf                ' 個人認証IDと同じもの
            strSql = strSql & "    AND staf.s_seq = " & pIntAddressSeq & vbCrLf                         ' 住所SEQと同じもの
            strSql = strSql & "    AND staf.d_from = '" & strSelUseDate & "'" & vbCrLf                  ' 適用日付と同じもの
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
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/25(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen() As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                               ' パネルオブジェクト
        Dim clsUC010101 As UC010101 = Nothing                                               ' 組合員検索画面クラス
        Dim strSql As String = ""                                                           ' SQL文
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス

        Try
            Me.Visible = False
            '-------------------------------------------------------------------------------
            '   組合員検索画面
            '-------------------------------------------------------------------------------
            ' 画面間パラメータ情報設定
            clsUC010101 = New UC010101                                                      ' 組合員検索画面クラス生成
            Call pnl.Controls.Add(clsUC010101)                                              ' 組合員検索画面表示

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
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：ByVal pBlnEdit As Boolean = ロックフラグ（True：アンロック, False：ロック）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <param name="pBlnEdit">修正可能フラグ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock(ByVal pBlnEdit As Boolean) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim blnAdd1 As Boolean = False                  ' 住所1用
        Dim blnAdd2 As Boolean = False                  ' 住所2用
        Dim blnAdd3 As Boolean = False                  ' 住所3用
        Dim blnHistory As Boolean = False               ' 履歴ボタン用
        Dim blnAttribute As Boolean = False             ' 基本情報照会ボタン用
        Dim blnEntry As Boolean = False                 ' 登録ボタン用
        Dim blnUpdate As Boolean = False                ' 内容変更ボタン用
        Dim blnCancel As Boolean = False                ' キャンセルボタン用
        Dim blnBack As Boolean = False                  ' 戻るボタン用

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

            ElseIf Me.bytStatus = STATUS_UPDATE Then
                '-----------------------------------------------
                '   内容変更
                '-----------------------------------------------
                If pBlnEdit Then
                    ' 内容変更ボタン押下後
                    blnEntry = True             ' 登録確認ボタン用
                    blnCancel = True            ' キャンセルボタン用

                    ' 住所区分1用
                    If Me.cboAddressKbn1.SelectedIndex <> 0 Then
                        ' 住所区分1が空白以外の場合、使用可能
                        blnAdd1 = True
                    End If
                    ' 住所区分2用
                    If Me.cboAddressKbn2.SelectedIndex <> 0 Then
                        ' 住所区分2が空白以外の場合、使用可能
                        blnAdd2 = True
                    End If
                    ' 住所区分3用
                    If Me.cboAddressKbn3.SelectedIndex <> 0 Then
                        ' 住所区分3が空白以外の場合、使用可能
                        blnAdd3 = True
                    End If
                Else
                    ' 内容変更ボタン押下前
                    blnUpdate = True            ' 内容変更ボタン用
                    blnAttribute = True         ' 基本情報照会ボタン用
                    blnBack = True              ' 戻るボタン用
                    blnHistory = True           ' 履歴ボタン
                End If
            End If

            '-------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------
            ' ボタン
            Me.btnHistory.Enabled = blnHistory                      ' 履歴ボタン用
            Me.btnAttribute.Visible = blnAttribute                  ' 基本情報照会ボタン用
            Me.btnInsertChk.Visible = blnEntry                      ' 登録ボタン用
            Me.btnUpdate.Visible = blnUpdate                        ' 内容変更ボタン用
            If blnUpdate Then
                If Me.strGrantInsert = GRANT_VALID Then
                    Me.btnUpdate.Enabled = True                     ' 内容変更ボタン使用可
                ElseIf Me.strGrantInsert = GRANT_VOID Then
                    Me.btnUpdate.Enabled = False                    ' 内容変更ボタン使用不可
                End If
            End If
            Me.btnCancel.Visible = blnCancel                        ' キャンセルボタン用
            Me.btnBack.Visible = blnBack                            ' 戻るボタン用

            '=======================================================
            '   基本情報
            '=======================================================
            Me.txtMemberNo.Enabled = False                          ' 社員番号
            Me.txtMemberNoDezit.Enabled = False                     ' 社員番号ディジット
            Me.txtKana.Enabled = False                              ' フリガナ
            Me.txtOldMemberNo.Enabled = False                       ' 旧社員番号
            Me.txtOldMemberNoDezit.Enabled = False                  ' 旧社員番号ディジット
            Me.txtName.Enabled = False                              ' 名前
            Me.txtStatus.Enabled = False                            ' ステータス
            Me.txtUnionMember.Enabled = False                       ' 組合員種別
            Me.txtUseDate.Enabled = False                           ' 運用日付

            Me.txtMemberNo.BackColor = Color.LightYellow            ' 社員番号
            Me.txtMemberNoDezit.BackColor = Color.LightYellow       ' 社員番号ディジット
            Me.txtKana.BackColor = Color.LightYellow                ' フリガナ
            Me.txtOldMemberNo.BackColor = Color.LightYellow         ' 旧社員番号
            Me.txtOldMemberNoDezit.BackColor = Color.LightYellow    ' 旧社員番号ディジット
            Me.txtName.BackColor = Color.LightYellow                ' 名前
            Me.txtStatus.BackColor = Color.LightYellow              ' ステータス
            Me.txtUnionMember.BackColor = Color.LightYellow         ' 組合員種別
            Me.txtUseDate.BackColor = Color.LightYellow             ' 運用日付

            '=======================================================
            '   住所1情報
            '=======================================================
            Me.lblAddressKbn1.Enabled = pBlnEdit                    ' 住所区分1ラベル
            Me.cboAddressKbn1.Enabled = pBlnEdit                    ' 住所区分1コンボボックス

            Me.lblMandatoryInternational1.Enabled = blnAdd1         ' 国内海外区分1ラベル
            Me.cboInternational1.Enabled = blnAdd1                  ' 国内海外区分1コンボボックス
            Me.chkMainAddress1.Enabled = blnAdd1                    ' 現住所1チェックボックス

            ' 国内1情報
            Me.lblMandatoryPostalNo1.Enabled = blnAdd1              ' 必須　郵便番号1ラベル
            Me.lblMandatoryPrefectures1.Enabled = blnAdd1           ' 必須　都道府県1ラベル
            Me.lblMandatoryCities1.Enabled = blnAdd1                ' 必須　市区町村1ラベル
            Me.lblMandatoryAddAther1.Enabled = blnAdd1              ' 必須　番地等1ラベル
            Me.lblMandatoryTel1.Enabled = blnAdd1                   ' 必須　電話番号1ラベル

            Me.lblPostalNo1.Enabled = blnAdd1                       ' 郵便番号1ラベル
            Me.lblPrefectures1.Enabled = blnAdd1                    ' 都道府県1ラベル
            Me.lblCities1.Enabled = blnAdd1                         ' 市区町村1ラベル
            Me.lblAddAther1.Enabled = blnAdd1                       ' 番地等1ラベル
            Me.lblBuilding1.Enabled = blnAdd1                       ' 建物名等1ラベル
            Me.lblTel1_1.Enabled = blnAdd1                          ' 電話番号1_1ラベル
            Me.lblTel1_2.Enabled = blnAdd1                          ' 電話番号1_2ラベル
            Me.lblTel1_3.Enabled = blnAdd1                          ' 電話番号1_3ラベル
            Me.lblFax1.Enabled = blnAdd1                            ' FAX1ラベル
            Me.lblMailPc1.Enabled = blnAdd1                         ' メールアドレスPC1ラベル
            Me.lblMailMobile1.Enabled = blnAdd1                     ' メールアドレス携帯1ラベル
            Me.lblNote1.Enabled = blnAdd1                           ' 備考1ラベル

            Me.lblPostalNoHyphen1.Enabled = blnAdd1                 ' 郵便番号1ハイフンラベル
            Me.lblTelHyphen1_1_1.Enabled = blnAdd1                  ' 電話番号1_1_1ハイフンラベル
            Me.lblTelHyphen1_1_2.Enabled = blnAdd1                  ' 電話番号1_1_2ハイフンラベル
            Me.lblTelHyphen1_2_1.Enabled = blnAdd1                  ' 電話番号1_2_1ハイフンラベル
            Me.lblTelHyphen1_2_2.Enabled = blnAdd1                  ' 電話番号1_2_2ハイフンラベル
            Me.lblTelHyphen1_3_1.Enabled = blnAdd1                  ' 電話番号1_3_1ハイフンラベル
            Me.lblTelHyphen1_3_2.Enabled = blnAdd1                  ' 電話番号1_3_2ハイフンラベル
            Me.lblFaxHyphen1_1.Enabled = blnAdd1                    ' FAX1_1ハイフンラベルラベル
            Me.lblFaxHyphen1_2.Enabled = blnAdd1                    ' FAX1_2ハイフンラベルラベル

            Me.cboPrefectures1.Enabled = blnAdd1                    ' 都道府県1コンボボックス

            Me.txtPostalNo1_1.Enabled = blnAdd1                     ' 郵便番号1_1テキストボックス
            Me.txtPostalNo1_2.Enabled = blnAdd1                     ' 郵便番号1_2テキストボックス
            Me.txtCities1.Enabled = blnAdd1                         ' 市区町村1テキストボックス
            Me.txtAddAther1.Enabled = blnAdd1                       ' 番地等1テキストボックス
            Me.txtBuilding1.Enabled = blnAdd1                       ' 建物名等1テキストボックス
            Me.txtTel1_1_1.Enabled = blnAdd1                        ' 電話番号1_1_1テキストボックス
            Me.txtTel1_1_2.Enabled = blnAdd1                        ' 電話番号1_1_2テキストボックス
            Me.txtTel1_1_3.Enabled = blnAdd1                        ' 電話番号1_1_3テキストボックス
            Me.txtTel1_2_1.Enabled = blnAdd1                        ' 電話番号1_2_1テキストボックス
            Me.txtTel1_2_2.Enabled = blnAdd1                        ' 電話番号1_2_2テキストボックス
            Me.txtTel1_2_3.Enabled = blnAdd1                        ' 電話番号1_2_3テキストボックス
            Me.txtTel1_3_1.Enabled = blnAdd1                        ' 電話番号1_3_1テキストボックス
            Me.txtTel1_3_2.Enabled = blnAdd1                        ' 電話番号1_3_2テキストボックス
            Me.txtTel1_3_3.Enabled = blnAdd1                        ' 電話番号1_3_3テキストボックス
            Me.txtFax1_1.Enabled = blnAdd1                          ' FAX1_1テキストボックス
            Me.txtFax1_2.Enabled = blnAdd1                          ' FAX1_2テキストボックス
            Me.txtFax1_3.Enabled = blnAdd1                          ' FAX1_3テキストボックス
            Me.txtMailPc1.Enabled = blnAdd1                         ' メールアドレスPC1テキストボックス
            Me.txtMailMobile1.Enabled = blnAdd1                     ' メールアドレス携帯1テキストボックス
            Me.txtNote1.Enabled = blnAdd1                           ' 備考1テキストボックス

            Me.btnAddressSearch1.Enabled = blnAdd1                  ' 住所検索1ボタン
            Me.btnClear1.Enabled = blnAdd1                          ' クリア1ボタン

            ' 海外1情報
            Me.lblForeignAdress1_1.Enabled = blnAdd1                ' アドレス1_1ラベル
            Me.lblForeignAdress1_2.Enabled = blnAdd1                ' アドレス1_2ラベル
            Me.lblForeignAdress1_3.Enabled = blnAdd1                ' アドレス1_3ラベル
            Me.lblForeignAdress1_4.Enabled = blnAdd1                ' アドレス1_4ラベル
            Me.lblForeignAdress1_5.Enabled = blnAdd1                ' アドレス1_5ラベル

            Me.txtForeignAdress1_1.Enabled = blnAdd1                ' アドレス1_1テキストボックス
            Me.txtForeignAdress1_2.Enabled = blnAdd1                ' アドレス1_2テキストボックス
            Me.txtForeignAdress1_3.Enabled = blnAdd1                ' アドレス1_3テキストボックス
            Me.txtForeignAdress1_4.Enabled = blnAdd1                ' アドレス1_4テキストボックス
            Me.txtForeignAdress1_5.Enabled = blnAdd1                ' アドレス1_5テキストボックス
            Me.txtTel1_1.Enabled = blnAdd1                          ' 電話番号1_1テキストボックス
            Me.txtTel1_2.Enabled = blnAdd1                          ' 電話番号1_2テキストボックス
            Me.txtTel1_3.Enabled = blnAdd1                          ' 電話番号1_3テキストボックス
            Me.txtFax1.Enabled = blnAdd1                            ' FAX1テキストボックス

            '=======================================================
            '   住所2情報
            '=======================================================
            Me.lblAddressKbn2.Enabled = pBlnEdit                    ' 住所区分2ラベル
            Me.cboAddressKbn2.Enabled = pBlnEdit                    ' 住所区分2コンボボックス

            Me.lblMandatoryInternational2.Enabled = blnAdd2         ' 国内海外区分2ラベル
            Me.cboInternational2.Enabled = blnAdd2                  ' 国内海外区分2コンボボックス
            Me.chkMainAddress2.Enabled = blnAdd2                    ' 現住所2チェックボックス

            ' 国内2情報
            Me.lblMandatoryPostalNo2.Enabled = blnAdd2              ' 必須　郵便番号2ラベル
            Me.lblMandatoryPrefectures2.Enabled = blnAdd2           ' 必須　都道府県2ラベル
            Me.lblMandatoryCities2.Enabled = blnAdd2                ' 必須　市区町村2ラベル
            Me.lblMandatoryAddAther2.Enabled = blnAdd2              ' 必須　番地等2ラベル
            Me.lblMandatoryTel2.Enabled = blnAdd2                   ' 必須　電話番号2ラベル

            Me.lblPostalNo2.Enabled = blnAdd2                       ' 郵便番号2ラベル
            Me.lblPrefectures2.Enabled = blnAdd2                    ' 都道府県2ラベル
            Me.lblCities2.Enabled = blnAdd2                         ' 市区町村2ラベル
            Me.lblAddAther2.Enabled = blnAdd2                       ' 番地等2ラベル
            Me.lblBuilding2.Enabled = blnAdd2                       ' 建物名等2ラベル
            Me.lblTel2_1.Enabled = blnAdd2                          ' 電話番号2_1ラベル
            Me.lblTel2_2.Enabled = blnAdd2                          ' 電話番号2_2ラベル
            Me.lblTel2_3.Enabled = blnAdd2                          ' 電話番号2_3ラベル
            Me.lblFax2.Enabled = blnAdd2                            ' FAX2ラベル
            Me.lblMailPc2.Enabled = blnAdd2                         ' メールアドレスPC2ラベル
            Me.lblMailMobile2.Enabled = blnAdd2                     ' メールアドレス携帯2ラベル
            Me.lblNote2.Enabled = blnAdd2                           ' 備考2ラベル

            Me.lblPostalNoHyphen2.Enabled = blnAdd2                 ' 郵便番号2ハイフンラベル
            Me.lblTelHyphen2_1_1.Enabled = blnAdd2                  ' 電話番号2_1_1ハイフンラベル
            Me.lblTelHyphen2_1_2.Enabled = blnAdd2                  ' 電話番号2_1_2ハイフンラベル
            Me.lblTelHyphen2_2_1.Enabled = blnAdd2                  ' 電話番号2_2_1ハイフンラベル
            Me.lblTelHyphen2_2_2.Enabled = blnAdd2                  ' 電話番号2_2_2ハイフンラベル
            Me.lblTelHyphen2_3_1.Enabled = blnAdd2                  ' 電話番号2_3_1ハイフンラベル
            Me.lblTelHyphen2_3_2.Enabled = blnAdd2                  ' 電話番号2_3_2ハイフンラベル
            Me.lblFaxHyphen2_1.Enabled = blnAdd2                    ' FAX2_1ハイフンラベルラベル
            Me.lblFaxHyphen2_2.Enabled = blnAdd2                    ' FAX2_2ハイフンラベルラベル

            Me.cboPrefectures2.Enabled = blnAdd2                    ' 都道府県2コンボボックス

            Me.txtPostalNo2_1.Enabled = blnAdd2                     ' 郵便番号2_1テキストボックス
            Me.txtPostalNo2_2.Enabled = blnAdd2                     ' 郵便番号2_2テキストボックス
            Me.txtCities2.Enabled = blnAdd2                         ' 市区町村2テキストボックス
            Me.txtAddAther2.Enabled = blnAdd2                       ' 番地等2テキストボックス
            Me.txtBuilding2.Enabled = blnAdd2                       ' 建物名等2テキストボックス
            Me.txtTel2_1_1.Enabled = blnAdd2                        ' 電話番号2_1_1テキストボックス
            Me.txtTel2_1_2.Enabled = blnAdd2                        ' 電話番号2_1_2テキストボックス
            Me.txtTel2_1_3.Enabled = blnAdd2                        ' 電話番号2_1_3テキストボックス
            Me.txtTel2_2_1.Enabled = blnAdd2                        ' 電話番号2_2_1テキストボックス
            Me.txtTel2_2_2.Enabled = blnAdd2                        ' 電話番号2_2_2テキストボックス
            Me.txtTel2_2_3.Enabled = blnAdd2                        ' 電話番号2_2_3テキストボックス
            Me.txtTel2_3_1.Enabled = blnAdd2                        ' 電話番号2_3_1テキストボックス
            Me.txtTel2_3_2.Enabled = blnAdd2                        ' 電話番号2_3_2テキストボックス
            Me.txtTel2_3_3.Enabled = blnAdd2                        ' 電話番号2_3_3テキストボックス
            Me.txtFax2_1.Enabled = blnAdd2                          ' FAX2_1テキストボックス
            Me.txtFax2_2.Enabled = blnAdd2                          ' FAX2_2テキストボックス
            Me.txtFax2_3.Enabled = blnAdd2                          ' FAX2_3テキストボックス
            Me.txtMailPc2.Enabled = blnAdd2                         ' メールアドレスPC2テキストボックス
            Me.txtMailMobile2.Enabled = blnAdd2                     ' メールアドレス携帯2テキストボックス
            Me.txtNote2.Enabled = blnAdd2                           ' 備考2テキストボックス

            Me.btnAddressSearch2.Enabled = blnAdd2                  ' 住所検索2ボタン
            Me.btnClear2.Enabled = blnAdd2                          ' クリア2ボタン

            ' 海外2情報
            Me.lblForeignAdress2_1.Enabled = blnAdd2                ' アドレス2_1ラベル
            Me.lblForeignAdress2_2.Enabled = blnAdd2                ' アドレス2_2ラベル
            Me.lblForeignAdress2_3.Enabled = blnAdd2                ' アドレス2_3ラベル
            Me.lblForeignAdress2_4.Enabled = blnAdd2                ' アドレス2_4ラベル
            Me.lblForeignAdress2_5.Enabled = blnAdd2                ' アドレス2_5ラベル

            Me.txtForeignAdress2_1.Enabled = blnAdd2                ' アドレス2_1テキストボックス
            Me.txtForeignAdress2_2.Enabled = blnAdd2                ' アドレス2_2テキストボックス
            Me.txtForeignAdress2_3.Enabled = blnAdd2                ' アドレス2_3テキストボックス
            Me.txtForeignAdress2_4.Enabled = blnAdd2                ' アドレス2_4テキストボックス
            Me.txtForeignAdress2_5.Enabled = blnAdd2                ' アドレス2_5テキストボックス
            Me.txtTel2_1.Enabled = blnAdd2                          ' 電話番号2_1テキストボックス
            Me.txtTel2_2.Enabled = blnAdd2                          ' 電話番号2_2テキストボックス
            Me.txtTel2_3.Enabled = blnAdd2                          ' 電話番号2_3テキストボックス
            Me.txtFax2.Enabled = blnAdd2                            ' FAX2テキストボックス

            '=======================================================
            '   住所3情報
            '=======================================================
            Me.lblAddressKbn3.Enabled = pBlnEdit                    ' 住所区分3ラベル
            Me.cboAddressKbn3.Enabled = pBlnEdit                    ' 住所区分3コンボボックス

            Me.lblMandatoryInternational3.Enabled = blnAdd3         ' 必須　国内海外区分3ラベル
            Me.cboInternational3.Enabled = blnAdd3                  ' 国内海外区分3コンボボックス
            Me.chkMainAddress3.Enabled = blnAdd3                    ' 現住所3チェックボックス

            ' 国内3情報
            Me.lblMandatoryPostalNo3.Enabled = blnAdd3              ' 必須　郵便番号3ラベル
            Me.lblMandatoryPrefectures3.Enabled = blnAdd3           ' 必須　都道府県3ラベル
            Me.lblMandatoryCities3.Enabled = blnAdd3                ' 必須　市区町村3ラベル
            Me.lblMandatoryAddAther3.Enabled = blnAdd3              ' 必須　番地等3ラベル
            Me.lblMandatoryTel3.Enabled = blnAdd3                   ' 必須　電話番号3ラベル

            Me.lblPostalNo3.Enabled = blnAdd3                       ' 郵便番号3ラベル
            Me.lblPrefectures3.Enabled = blnAdd3                    ' 都道府県3ラベル
            Me.lblCities3.Enabled = blnAdd3                         ' 市区町村3ラベル
            Me.lblAddAther3.Enabled = blnAdd3                       ' 番地等3ラベル
            Me.lblBuilding3.Enabled = blnAdd3                       ' 建物名等3ラベル
            Me.lblTel3_1.Enabled = blnAdd3                          ' 電話番号3_1ラベル
            Me.lblTel3_2.Enabled = blnAdd3                          ' 電話番号3_3ラベル
            Me.lblTel3_3.Enabled = blnAdd3                          ' 電話番号3_3ラベル
            Me.lblFax3.Enabled = blnAdd3                            ' FAX3ラベル
            Me.lblMailPc3.Enabled = blnAdd3                         ' メールアドレスPC3ラベル
            Me.lblMailMobile3.Enabled = blnAdd3                     ' メールアドレス携帯3ラベル
            Me.lblNote3.Enabled = blnAdd3                           ' 備考3ラベル

            Me.lblPostalNoHyphen3.Enabled = blnAdd3                 ' 郵便番号3ハイフンラベル
            Me.lblTelHyphen3_1_1.Enabled = blnAdd3                  ' 電話番号3_1_1ハイフンラベル
            Me.lblTelHyphen3_1_2.Enabled = blnAdd3                  ' 電話番号3_1_2ハイフンラベル
            Me.lblTelHyphen3_2_1.Enabled = blnAdd3                  ' 電話番号3_2_1ハイフンラベル
            Me.lblTelHyphen3_2_2.Enabled = blnAdd3                  ' 電話番号3_2_2ハイフンラベル
            Me.lblTelHyphen3_3_1.Enabled = blnAdd3                  ' 電話番号3_3_1ハイフンラベル
            Me.lblTelHyphen3_3_2.Enabled = blnAdd3                  ' 電話番号3_3_2ハイフンラベル
            Me.lblFaxHyphen3_1.Enabled = blnAdd3                    ' FAX3_1ハイフンラベルラベル
            Me.lblFaxHyphen3_2.Enabled = blnAdd3                    ' FAX3_2ハイフンラベルラベル

            Me.cboPrefectures3.Enabled = blnAdd3                    ' 都道府県3コンボボックス

            Me.txtPostalNo3_1.Enabled = blnAdd3                     ' 郵便番号3_1テキストボックス
            Me.txtPostalNo3_2.Enabled = blnAdd3                     ' 郵便番号3_2テキストボックス
            Me.txtCities3.Enabled = blnAdd3                         ' 市区町村3テキストボックス
            Me.txtAddAther3.Enabled = blnAdd3                       ' 番地等3テキストボックス
            Me.txtBuilding3.Enabled = blnAdd3                       ' 建物名等3テキストボックス
            Me.txtTel3_1_1.Enabled = blnAdd3                        ' 電話番号3_1_1テキストボックス
            Me.txtTel3_1_2.Enabled = blnAdd3                        ' 電話番号3_1_2テキストボックス
            Me.txtTel3_1_3.Enabled = blnAdd3                        ' 電話番号3_1_3テキストボックス
            Me.txtTel3_2_1.Enabled = blnAdd3                        ' 電話番号3_2_1テキストボックス
            Me.txtTel3_2_2.Enabled = blnAdd3                        ' 電話番号3_2_3テキストボックス
            Me.txtTel3_2_3.Enabled = blnAdd3                        ' 電話番号3_2_3テキストボックス
            Me.txtTel3_3_1.Enabled = blnAdd3                        ' 電話番号3_3_1テキストボックス
            Me.txtTel3_3_2.Enabled = blnAdd3                        ' 電話番号3_3_2テキストボックス
            Me.txtTel3_3_3.Enabled = blnAdd3                        ' 電話番号3_3_3テキストボックス
            Me.txtFax3_1.Enabled = blnAdd3                          ' FAX3_1テキストボックス
            Me.txtFax3_2.Enabled = blnAdd3                          ' FAX3_2テキストボックス
            Me.txtFax3_3.Enabled = blnAdd3                          ' FAX3_3テキストボックス
            Me.txtMailPc3.Enabled = blnAdd3                         ' メールアドレスPC3テキストボックス
            Me.txtMailMobile3.Enabled = blnAdd3                     ' メールアドレス携帯3テキストボックス
            Me.txtNote3.Enabled = blnAdd3                           ' 備考3テキストボックス

            Me.btnAddressSearch3.Enabled = blnAdd3                  ' 住所検索3ボタン
            Me.btnClear3.Enabled = blnAdd3                          ' クリア3ボタン

            ' 海外3情報
            Me.lblForeignAdress3_1.Enabled = blnAdd3                ' アドレス3_1ラベル
            Me.lblForeignAdress3_2.Enabled = blnAdd3                ' アドレス3_2ラベル
            Me.lblForeignAdress3_3.Enabled = blnAdd3                ' アドレス3_3ラベル
            Me.lblForeignAdress3_4.Enabled = blnAdd3                ' アドレス3_4ラベル
            Me.lblForeignAdress3_5.Enabled = blnAdd3                ' アドレス3_5ラベル

            Me.txtForeignAdress3_1.Enabled = blnAdd3                ' アドレス3_1テキストボックス
            Me.txtForeignAdress3_2.Enabled = blnAdd3                ' アドレス3_2テキストボックス
            Me.txtForeignAdress3_3.Enabled = blnAdd3                ' アドレス3_3テキストボックス
            Me.txtForeignAdress3_4.Enabled = blnAdd3                ' アドレス3_4テキストボックス
            Me.txtForeignAdress3_5.Enabled = blnAdd3                ' アドレス3_5テキストボックス
            Me.txtTel3_1.Enabled = blnAdd3                          ' 電話番号3_1テキストボックス
            Me.txtTel3_2.Enabled = blnAdd3                          ' 電話番号3_2テキストボックス
            Me.txtTel3_3.Enabled = blnAdd3                          ' 電話番号3_3テキストボックス
            Me.txtFax3.Enabled = blnAdd3                            ' FAX3テキストボックス

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
    '   引数　：ByVal pBytStatus As Byte = ステータス（1：新規登録, 2：内容変更）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <param name="pBytStatus">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear(ByVal pBytStatus As Byte) As Boolean

        Dim blnRet As Boolean = False                               ' 処理結果

        Try
            '=======================================================================
            '   共通
            '=======================================================================
            ' Title
            If Me.bytStatus = STATUS_INSERT Then
                Me.lblTitle.Text = "住所情報 - 新規登録"
            ElseIf Me.bytStatus = STATUS_UPDATE Then
                Me.lblTitle.Text = "組合員管理 - 住所情報"
            End If

            ' TextBox
            Me.txtMemberNo.Text = ""                                ' 社員番号
            Me.txtMemberNoDezit.Text = ""                           ' 社員番号ディジット
            Me.txtKana.Text = ""                                    ' フリガナ
            Me.txtOldMemberNo.Text = ""                             ' 旧社員番号
            Me.txtOldMemberNoDezit.Text = ""                        ' 旧社員番号ディジット
            Me.txtName.Text = ""                                    ' 名前
            Me.txtStatus.Text = ""                                  ' ステータス
            Me.txtUnionMember.Text = ""                             ' 組合員種別
            Me.txtUseDate.Text = ""                                 ' 運用日付

            ' TextBox BackColor
            Me.txtMemberNo.BackColor = Color.LightYellow            ' 社員番号バックカラー薄黄色
            Me.txtMemberNoDezit.BackColor = Color.LightYellow       ' 社員番号ディジットバックカラー薄黄色
            Me.txtKana.BackColor = Color.LightYellow                ' フリガナバックカラー薄黄色
            Me.txtOldMemberNo.BackColor = Color.LightYellow         ' 旧社員番号バックカラー薄黄色
            Me.txtOldMemberNoDezit.BackColor = Color.LightYellow    ' 旧社員番号ディジットバックカラー薄黄色
            Me.txtName.BackColor = Color.LightYellow                ' 名前バックカラー薄黄色
            Me.txtStatus.BackColor = Color.LightYellow              ' ステータスバックカラー薄黄色
            Me.txtUnionMember.BackColor = Color.LightYellow         ' 組合員種別バックカラー薄黄色
            Me.txtUseDate.BackColor = Color.LightYellow             ' 運用日付バックカラー薄黄色

            ' Button
            Me.btnBack.Visible = True                               ' 戻るボタン
            Me.btnUpdate.Visible = True                             ' 内容変更ボタン
            Me.btnCancel.Visible = True                             ' キャンセルボタン
            Me.btnInsertChk.Visible = True                          ' 登録確認ボタン
            Me.btnHistory.Visible = True                            ' 履歴ボタン
            Me.btnAttribute.Visible = True                          ' 基本情報照会ボタン
            ' Tab
            Me.tabAddress.SelectedIndex = 0                         ' 住所1タブ選択

            '=======================================================================
            '   住所1
            '=======================================================================
            '---------------------------------------
            '   共通
            '---------------------------------------
            ' Tab
            Me.TabPage1.Text = "住所1"

            ' Label
            Me.lblAddressKbn1.Visible = True                ' 住所区分1
            Me.lblMandatoryInternational1.Visible = True    ' 必須　国内海外区分1
            Me.lblNote1.Visible = True                      ' 備考1

            ' ComboBox
            Me.cboAddressKbn1.Visible = True                ' 住所区分1
            Me.cboInternational1.Visible = True             ' 国内海外区分1

            ' CheckBox
            Me.chkMainAddress1.Visible = True               ' 現住所1

            ' TextBox
            Me.txtNote1.Visible = True                      ' 備考1

            ' Button
            Me.btnClear1.Visible = True                     ' クリアボタン1

            ' クリア
            Me.cboAddressKbn1.DataSource = Nothing          ' 住所区分1
            Me.cboAddressKbn1.Text = ""
            Me.cboInternational1.DataSource = Nothing       ' 国内海外区分1
            Me.cboInternational1.Text = ""
            Me.chkMainAddress1.Checked = False              ' 現住所1
            Me.txtNote1.Text = ""                           ' 備考1

            '---------------------------------------
            '   国内
            '---------------------------------------
            ' Label
            Me.lblMandatoryPostalNo1.Visible = True         ' 必須　郵便番号1
            Me.lblMandatoryPrefectures1.Visible = True      ' 必須　都道府県1
            Me.lblMandatoryCities1.Visible = True           ' 必須　市区町村1
            Me.lblMandatoryAddAther1.Visible = True         ' 必須　番地等1
            Me.lblMandatoryTel1.Visible = True              ' 必須　電話番号1

            Me.lblPostalNo1.Visible = True                  ' 郵便番号1
            Me.lblPrefectures1.Visible = True               ' 都道府県1
            Me.lblCities1.Visible = True                    ' 市区町村1
            Me.lblAddAther1.Visible = True                  ' 番地等1
            Me.lblBuilding1.Visible = True                  ' 建物名等1
            Me.lblTel1_1.Visible = True                     ' 電話番号1_1
            Me.lblTel1_2.Visible = True                     ' 電話番号1_2
            Me.lblTel1_2.Visible = True                     ' 電話番号1_3
            Me.lblFax1.Visible = True                       ' FAX1
            Me.lblMailPc1.Visible = True                    ' メールアドレスPC1
            Me.lblMailMobile1.Visible = True                ' メールアドレス携帯1

            Me.lblTelHyphen1_1_1.Visible = True             ' 電話番号1_1ハイフン1
            Me.lblTelHyphen1_1_2.Visible = True             ' 電話番号1_1ハイフン2
            Me.lblTelHyphen1_2_1.Visible = True             ' 電話番号1_2ハイフン1
            Me.lblTelHyphen1_2_2.Visible = True             ' 電話番号1_2ハイフン2
            Me.lblTelHyphen1_3_1.Visible = True             ' 電話番号1_3ハイフン1
            Me.lblTelHyphen1_3_2.Visible = True             ' 電話番号1_3ハイフン2
            Me.lblFaxHyphen1_1.Visible = True               ' FAX1ハイフン1
            Me.lblFaxHyphen1_2.Visible = True               ' FAX1ハイフン2

            ' ComboBox
            Me.cboPrefectures1.Visible = True               ' 都道府県1

            ' TextBox
            Me.txtPostalNo1_1.Visible = True                ' 郵便番号1_1
            Me.txtPostalNo1_2.Visible = True                ' 郵便番号1_2
            Me.txtCities1.Visible = True                    ' 市区町村1
            Me.txtBuilding1.Visible = True                  ' 建物名等1
            Me.txtTel1_1_1.Visible = True                   ' 電話番号1_1_1
            Me.txtTel1_1_2.Visible = True                   ' 電話番号1_1_2
            Me.txtTel1_1_3.Visible = True                   ' 電話番号1_1_3
            Me.txtTel1_2_1.Visible = True                   ' 電話番号1_2_1
            Me.txtTel1_2_2.Visible = True                   ' 電話番号1_2_2
            Me.txtTel1_2_3.Visible = True                   ' 電話番号1_2_3
            Me.txtTel1_3_1.Visible = True                   ' 電話番号1_3_1
            Me.txtTel1_3_2.Visible = True                   ' 電話番号1_3_2
            Me.txtTel1_3_3.Visible = True                   ' 電話番号1_3_3
            Me.txtFax1_1.Visible = True                     ' FAX1_1
            Me.txtFax1_2.Visible = True                     ' FAX1_2
            Me.txtFax1_3.Visible = True                     ' FAX1_3
            Me.txtMailPc1.Visible = True                    ' メールアドレスPC1
            Me.txtMailMobile1.Visible = True                ' メールアドレス携帯1

            ' Button
            Me.btnAddressSearch1.Visible = True             ' 検索ボタン1

            ' クリア
            Me.txtPostalNo1_1.Text = ""                     ' 郵便番号1_1
            Me.txtPostalNo1_2.Text = ""                     ' 郵便番号1_2
            Me.txtCities1.Text = ""                         ' 市区町村1
            Me.txtAddAther1.Text = ""                       ' 番地等1
            Me.txtBuilding1.Text = ""                       ' 建物名等1
            Me.txtTel1_1_1.Text = ""                        ' 電話番号1_1_1
            Me.txtTel1_1_2.Text = ""                        ' 電話番号1_1_2
            Me.txtTel1_1_3.Text = ""                        ' 電話番号1_1_3
            Me.txtTel1_2_1.Text = ""                        ' 電話番号1_2_1
            Me.txtTel1_2_2.Text = ""                        ' 電話番号1_2_2
            Me.txtTel1_2_3.Text = ""                        ' 電話番号1_2_3
            Me.txtTel1_3_1.Text = ""                        ' 電話番号1_3_1
            Me.txtTel1_3_2.Text = ""                        ' 電話番号1_3_2
            Me.txtTel1_3_3.Text = ""                        ' 電話番号1_3_3
            Me.txtMailPc1.Text = ""                         ' メールアドレスPC
            Me.txtMailMobile1.Text = ""                     ' メールアドレス携帯
            Me.txtFax1_1.Text = ""                          ' FAX1_1
            Me.txtFax1_2.Text = ""                          ' FAX1_2
            Me.txtFax1_3.Text = ""                          ' FAX1_3

            '---------------------------------------
            '   海外情報
            '---------------------------------------
            ' Label
            Me.lblForeignAdress1_1.Visible = True           ' アドレス1_1
            Me.lblForeignAdress1_2.Visible = True           ' アドレス1_2
            Me.lblForeignAdress1_3.Visible = True           ' アドレス1_3
            Me.lblForeignAdress1_4.Visible = True           ' アドレス1_4
            Me.lblForeignAdress1_5.Visible = True           ' アドレス1_5

            ' TextBox
            Me.txtForeignAdress1_1.Visible = True           ' アドレス1_1
            Me.txtForeignAdress1_2.Visible = True           ' アドレス1_2
            Me.txtForeignAdress1_3.Visible = True           ' アドレス1_3
            Me.txtForeignAdress1_4.Visible = True           ' アドレス1_4
            Me.txtForeignAdress1_5.Visible = True           ' アドレス1_5
            Me.txtTel1_1.Visible = True                     ' 電話番号1_1
            Me.txtTel1_2.Visible = True                     ' 電話番号1_2
            Me.txtTel1_3.Visible = True                     ' 電話番号1_3
            Me.txtFax1.Visible = True                       ' FAX1

            ' クリア
            Me.txtTel1_1.Text = ""                          ' 電話番号1_1
            Me.txtTel1_2.Text = ""                          ' 電話番号1_2
            Me.txtTel1_3.Text = ""                          ' 電話番号1_3
            Me.txtFax1.Text = ""                            ' FAX1

            '=======================================================================
            '   住所2
            '=======================================================================
            '---------------------------------------
            '   共通
            '---------------------------------------
            ' Tab
            Me.TabPage2.Text = "住所2"

            ' Label
            Me.lblAddressKbn2.Visible = True                ' 住所区分2
            Me.lblMandatoryInternational2.Visible = True    ' 必須　国内海外区分2
            Me.lblNote2.Visible = True                      ' 備考2

            ' ComboBox
            Me.cboAddressKbn2.Visible = True                ' 住所区分2
            Me.cboInternational2.Visible = True             ' 国内海外区分2

            ' CheckBox
            Me.chkMainAddress2.Visible = True               ' 現住所2

            ' TextBox
            Me.txtNote2.Visible = True                      ' 備考2

            ' Button
            Me.btnClear2.Visible = True                     ' クリアボタン2

            ' クリア
            Me.cboAddressKbn2.DataSource = Nothing          ' 住所区分2
            Me.cboAddressKbn2.Text = ""
            Me.cboInternational2.DataSource = Nothing       ' 国内海外区分2
            Me.cboInternational2.Text = ""
            Me.chkMainAddress2.Checked = False              ' 現住所2
            Me.txtNote2.Text = ""                           ' 備考2

            '---------------------------------------
            '   国内
            '---------------------------------------
            ' Label
            Me.lblMandatoryPostalNo2.Visible = True         ' 必須　郵便番号2
            Me.lblMandatoryPrefectures2.Visible = True      ' 必須　都道府県2
            Me.lblMandatoryCities2.Visible = True           ' 必須　市区町村2
            Me.lblMandatoryAddAther2.Visible = True         ' 必須　番地等2
            Me.lblMandatoryTel2.Visible = True              ' 必須　電話番号2

            Me.lblPostalNo2.Visible = True                  ' 郵便番号2
            Me.lblPrefectures2.Visible = True               ' 都道府県2
            Me.lblCities2.Visible = True                    ' 市区町村2
            Me.lblAddAther2.Visible = True                  ' 番地等2
            Me.lblBuilding2.Visible = True                  ' 建物名等2
            Me.lblTel2_1.Visible = True                     ' 電話番号2_1
            Me.lblTel2_2.Visible = True                     ' 電話番号2_2
            Me.lblTel2_2.Visible = True                     ' 電話番号2_3
            Me.lblFax2.Visible = True                       ' FAX2
            Me.lblMailPc2.Visible = True                    ' メールアドレスPC2
            Me.lblMailMobile2.Visible = True                ' メールアドレス携帯2

            Me.lblTelHyphen2_1_1.Visible = True             ' 電話番号2_1ハイフン1
            Me.lblTelHyphen2_1_2.Visible = True             ' 電話番号2_1ハイフン2
            Me.lblTelHyphen2_2_1.Visible = True             ' 電話番号2_2ハイフン1
            Me.lblTelHyphen2_2_2.Visible = True             ' 電話番号2_2ハイフン2
            Me.lblTelHyphen2_3_1.Visible = True             ' 電話番号2_3ハイフン1
            Me.lblTelHyphen2_3_2.Visible = True             ' 電話番号2_3ハイフン2
            Me.lblFaxHyphen2_1.Visible = True               ' FAX2ハイフン1
            Me.lblFaxHyphen2_2.Visible = True               ' FAX2ハイフン2

            ' ComboBox
            Me.cboPrefectures2.Visible = True               ' 都道府県2

            ' TextBox
            Me.txtPostalNo2_1.Visible = True                ' 郵便番号2_1
            Me.txtPostalNo2_2.Visible = True                ' 郵便番号2_2
            Me.txtCities2.Visible = True                    ' 市区町村2
            Me.txtBuilding2.Visible = True                  ' 建物名等2
            Me.txtTel2_1_1.Visible = True                   ' 電話番号2_1_1
            Me.txtTel2_1_2.Visible = True                   ' 電話番号2_1_2
            Me.txtTel2_1_3.Visible = True                   ' 電話番号2_1_3
            Me.txtTel2_2_1.Visible = True                   ' 電話番号2_2_1
            Me.txtTel2_2_2.Visible = True                   ' 電話番号2_2_2
            Me.txtTel2_2_3.Visible = True                   ' 電話番号2_2_3
            Me.txtTel2_3_1.Visible = True                   ' 電話番号2_3_1
            Me.txtTel2_3_2.Visible = True                   ' 電話番号2_3_2
            Me.txtTel2_3_3.Visible = True                   ' 電話番号2_3_3
            Me.txtFax2_1.Visible = True                     ' FAX2_1
            Me.txtFax2_2.Visible = True                     ' FAX2_2
            Me.txtFax2_3.Visible = True                     ' FAX2_3
            Me.txtMailPc2.Visible = True                    ' メールアドレスPC2
            Me.txtMailMobile2.Visible = True                ' メールアドレス携帯2

            ' Button
            Me.btnAddressSearch2.Visible = True             ' 検索ボタン2
            Me.btnAddressSearch2.Enabled = False

            ' クリア
            Me.txtPostalNo2_1.Text = ""                     ' 郵便番号2_1
            Me.txtPostalNo2_2.Text = ""                     ' 郵便番号2_2
            Me.txtCities2.Text = ""                         ' 市区町村2
            Me.txtAddAther2.Text = ""                       ' 番地等2
            Me.txtBuilding2.Text = ""                       ' 建物名等2
            Me.txtTel2_1_1.Text = ""                        ' 電話番号2_1_1
            Me.txtTel2_1_2.Text = ""                        ' 電話番号2_1_2
            Me.txtTel2_1_3.Text = ""                        ' 電話番号2_1_3
            Me.txtTel2_2_1.Text = ""                        ' 電話番号2_2_1
            Me.txtTel2_2_2.Text = ""                        ' 電話番号2_2_2
            Me.txtTel2_2_3.Text = ""                        ' 電話番号2_2_3
            Me.txtTel2_3_1.Text = ""                        ' 電話番号2_3_1
            Me.txtTel2_3_2.Text = ""                        ' 電話番号2_3_2
            Me.txtTel2_3_3.Text = ""                        ' 電話番号2_3_3
            Me.txtMailPc2.Text = ""                         ' メールアドレスPC2
            Me.txtMailMobile2.Text = ""                     ' メールアドレス携帯2
            Me.txtFax2_1.Text = ""                          ' FAX2_1
            Me.txtFax2_2.Text = ""                          ' FAX2_2
            Me.txtFax2_3.Text = ""                          ' FAX2_3

            '---------------------------------------
            '   海外情報
            '---------------------------------------
            ' Label
            Me.lblForeignAdress2_1.Visible = True           ' アドレス2_1
            Me.lblForeignAdress2_2.Visible = True           ' アドレス2_2
            Me.lblForeignAdress2_3.Visible = True           ' アドレス2_3
            Me.lblForeignAdress2_4.Visible = True           ' アドレス2_4
            Me.lblForeignAdress2_5.Visible = True           ' アドレス2_5
            ' TextBox
            Me.txtForeignAdress2_1.Visible = True           ' アドレス2_1
            Me.txtForeignAdress2_2.Visible = True           ' アドレス2_2
            Me.txtForeignAdress2_3.Visible = True           ' アドレス2_3
            Me.txtForeignAdress2_4.Visible = True           ' アドレス2_4
            Me.txtForeignAdress2_5.Visible = True           ' アドレス2_5
            Me.txtTel2_1.Visible = True                     ' 電話番号2_1
            Me.txtTel2_2.Visible = True                     ' 電話番号2_2
            Me.txtTel2_3.Visible = True                     ' 電話番号2_3
            Me.txtFax2.Visible = True                       ' FAX2
            ' クリア
            Me.txtTel2_1.Text = ""                          ' 電話番号2_1
            Me.txtTel2_2.Text = ""                          ' 電話番号2_2
            Me.txtTel2_3.Text = ""                          ' 電話番号2_3
            Me.txtFax2.Text = ""                            ' FAX2

            '=======================================================================
            '   住所3
            '=======================================================================
            '---------------------------------------
            '   共通
            '---------------------------------------
            ' Tab
            Me.TabPage3.Text = "住所3"

            ' Label
            Me.lblAddressKbn3.Visible = True                ' 住所区分3
            Me.lblMandatoryInternational3.Visible = True    ' 必須　国内海外区分3
            Me.lblNote3.Visible = True                      ' 備考3

            ' ComboBox
            Me.cboAddressKbn3.Visible = True                ' 住所区分3
            Me.cboInternational3.Visible = True             ' 国内海外区分3

            ' CheckBox
            Me.chkMainAddress3.Visible = True               ' 現住所3

            ' TextBox
            Me.txtNote3.Visible = True                      ' 備考3

            ' Button
            Me.btnClear3.Visible = True                     ' クリアボタン3

            ' クリア
            Me.cboAddressKbn3.DataSource = Nothing          ' 住所区分3
            Me.cboAddressKbn3.Text = ""
            Me.cboInternational3.DataSource = Nothing       ' 国内海外区分3
            Me.cboInternational3.Text = ""
            Me.chkMainAddress3.Checked = False              ' 現住所3
            Me.txtNote3.Text = ""                           ' 備考3

            '---------------------------------------
            '   国内
            '---------------------------------------
            ' Label
            Me.lblMandatoryPostalNo3.Visible = True         ' 必須　郵便番号3
            Me.lblMandatoryPrefectures3.Visible = True      ' 必須　都道府県3
            Me.lblMandatoryCities3.Visible = True           ' 必須　市区町村3
            Me.lblMandatoryAddAther3.Visible = True         ' 必須　番地等3
            Me.lblMandatoryTel3.Visible = True              ' 必須　電話番号3

            Me.lblPostalNo3.Visible = True                  ' 郵便番号3
            Me.lblPrefectures3.Visible = True               ' 都道府県3
            Me.lblCities3.Visible = True                    ' 市区町村3
            Me.lblAddAther3.Visible = True                  ' 番地等3
            Me.lblBuilding3.Visible = True                  ' 建物名等3
            Me.lblTel3_1.Visible = True                     ' 電話番号3_1
            Me.lblTel3_2.Visible = True                     ' 電話番号3_2
            Me.lblTel3_2.Visible = True                     ' 電話番号3_3
            Me.lblFax3.Visible = True                       ' FAX3
            Me.lblMailPc3.Visible = True                    ' メールアドレスPC3
            Me.lblMailMobile3.Visible = True                ' メールアドレス携帯3

            Me.lblTelHyphen3_1_1.Visible = True             ' 電話番号3_1ハイフン1
            Me.lblTelHyphen3_1_2.Visible = True             ' 電話番号3_1ハイフン2
            Me.lblTelHyphen3_2_1.Visible = True             ' 電話番号3_2ハイフン1
            Me.lblTelHyphen3_2_2.Visible = True             ' 電話番号3_2ハイフン2
            Me.lblTelHyphen3_3_1.Visible = True             ' 電話番号3_3ハイフン1
            Me.lblTelHyphen3_3_2.Visible = True             ' 電話番号3_3ハイフン2
            Me.lblFaxHyphen3_1.Visible = True               ' FAX3ハイフン1
            Me.lblFaxHyphen3_2.Visible = True               ' FAX3ハイフン2

            ' ComboBox
            Me.cboPrefectures3.Visible = True               ' 都道府県3

            ' TextBox
            Me.txtPostalNo3_1.Visible = True                ' 郵便番号3_1
            Me.txtPostalNo3_2.Visible = True                ' 郵便番号3_2
            Me.txtCities3.Visible = True                    ' 市区町村3
            Me.txtBuilding3.Visible = True                  ' 建物名等3
            Me.txtTel3_1_1.Visible = True                   ' 電話番号3_1_1
            Me.txtTel3_1_2.Visible = True                   ' 電話番号3_1_2
            Me.txtTel3_1_3.Visible = True                   ' 電話番号3_1_3
            Me.txtTel3_2_1.Visible = True                   ' 電話番号3_2_1
            Me.txtTel3_2_2.Visible = True                   ' 電話番号3_2_2
            Me.txtTel3_2_3.Visible = True                   ' 電話番号3_2_3
            Me.txtTel3_3_1.Visible = True                   ' 電話番号3_3_1
            Me.txtTel3_3_2.Visible = True                   ' 電話番号3_3_2
            Me.txtTel3_3_3.Visible = True                   ' 電話番号3_3_3
            Me.txtFax3_1.Visible = True                     ' FAX3_1
            Me.txtFax3_2.Visible = True                     ' FAX3_2
            Me.txtFax3_3.Visible = True                     ' FAX3_3
            Me.txtMailPc3.Visible = True                    ' メールアドレスPC3
            Me.txtMailMobile3.Visible = True                ' メールアドレス携帯3

            ' Button
            Me.btnAddressSearch3.Visible = True             ' 検索ボタン3
            Me.btnAddressSearch3.Enabled = False

            ' クリア
            Me.txtPostalNo3_1.Text = ""                     ' 郵便番号3_1
            Me.txtPostalNo3_2.Text = ""                     ' 郵便番号3_2
            Me.txtCities3.Text = ""                         ' 市区町村3
            Me.txtAddAther3.Text = ""                       ' 番地等3
            Me.txtBuilding3.Text = ""                       ' 建物名等3
            Me.txtTel3_1_1.Text = ""                        ' 電話番号3_1_1
            Me.txtTel3_1_2.Text = ""                        ' 電話番号3_1_2
            Me.txtTel3_1_3.Text = ""                        ' 電話番号3_1_3
            Me.txtTel3_2_1.Text = ""                        ' 電話番号3_2_1
            Me.txtTel3_2_2.Text = ""                        ' 電話番号3_2_2
            Me.txtTel3_2_3.Text = ""                        ' 電話番号3_2_3
            Me.txtTel3_3_1.Text = ""                        ' 電話番号3_3_1
            Me.txtTel3_3_2.Text = ""                        ' 電話番号3_3_2
            Me.txtTel3_3_3.Text = ""                        ' 電話番号3_3_3
            Me.txtMailPc3.Text = ""                         ' メールアドレスPC3
            Me.txtMailMobile3.Text = ""                     ' メールアドレス携帯3
            Me.txtFax3_1.Text = ""                          ' FAX3_1
            Me.txtFax3_2.Text = ""                          ' FAX3_2
            Me.txtFax3_3.Text = ""                          ' FAX3_3

            '---------------------------------------
            '   海外情報
            '---------------------------------------
            ' Label
            Me.lblForeignAdress3_1.Visible = True           ' アドレス3_1
            Me.lblForeignAdress3_2.Visible = True           ' アドレス3_2
            Me.lblForeignAdress3_3.Visible = True           ' アドレス3_3
            Me.lblForeignAdress3_4.Visible = True           ' アドレス3_4
            Me.lblForeignAdress3_5.Visible = True           ' アドレス3_5

            ' TextBox
            Me.txtForeignAdress3_1.Visible = True           ' アドレス3_1
            Me.txtForeignAdress3_2.Visible = True           ' アドレス3_2
            Me.txtForeignAdress3_3.Visible = True           ' アドレス3_3
            Me.txtForeignAdress3_4.Visible = True           ' アドレス3_4
            Me.txtForeignAdress3_5.Visible = True           ' アドレス3_5
            Me.txtTel3_1.Visible = True                     ' 電話番号3_1
            Me.txtTel3_2.Visible = True                     ' 電話番号3_2
            Me.txtTel3_3.Visible = True                     ' 電話番号3_3
            Me.txtFax3.Visible = True                       ' FAX3

            ' クリア
            Me.txtTel3_1.Text = ""                          ' 電話番号3_1
            Me.txtTel3_2.Text = ""                          ' 電話番号3_2
            Me.txtTel3_3.Text = ""                          ' 電話番号3_3
            Me.txtFax3.Text = ""                            ' FAX3

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
    '   ＩＤ　：GetStafAttribute
    '   名称　：基本情報取得処理
    '   概要  ：組合員管理 - 基本情報を取得する。
    '   引数　：ByVal pClsDb     As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    ''***************************************************************************************************
    ''' <summary>基本情報取得処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStafAttribute(ByVal pClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim tbRet As DataTable                  ' 処理結果データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf.c_staf_id" & vbCrLf                                     ' 01. 社員番号
            strSql = strSql & "       ,staf.c_dezit" & vbCrLf                                       ' 02. 社員番号デジット
            strSql = strSql & "       ,staf.l_name_kna" & vbCrLf                                    ' 03. フリガナ
            strSql = strSql & "       ,staf.c_staf_id_old" & vbCrLf                                 ' 04. 旧社員番号
            strSql = strSql & "       ,staf.c_dezit_old" & vbCrLf                                   ' 05. 旧社員番号デジット
            strSql = strSql & "       ,staf.l_name" & vbCrLf                                        ' 06. 名前
            ' 07. スタータス
            strSql = strSql & "       ,(SELECT constant_dtl.l_name" & vbCrLf
            strSql = strSql & "           FROM constant_dtl" & vbCrLf
            strSql = strSql & "          WHERE constant_dtl.c_constant = '" & CONSTANT_ID_USER_STATUS & "'" & vbCrLf
            strSql = strSql & "            AND constant_dtl.c_constant_seq = staf.k_user_status) AS k_user_status" & vbCrLf
            ' 08. 組合員種別（組合員種別コードを元に定数マスタから組合員種別名称取得）
            strSql = strSql & "       ,(SELECT constant_dtl.l_name" & vbCrLf
            strSql = strSql & "           FROM constant_dtl" & vbCrLf
            strSql = strSql & "          WHERE constant_dtl.c_constant = '" & CONSTANT_ID_STAF_KIND & "'" & vbCrLf
            strSql = strSql & "            AND constant_dtl.c_constant_seq = staf.k_staf_kind) AS k_staf_kind_name" & vbCrLf
            strSql = strSql & "   FROM staf_attribute AS staf" & vbCrLf                             ' 組合員属性テーブル
            strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf            ' 個人認証IDと同じもの
            'strSql = strSql & "    AND staf.c_ksh = '" & Me.strKsh & "'" & vbCrLf                   ' 会社コードと同じもの
            strSql = strSql & "    AND staf.c_staf_id = '" & Me.strStafId & "'" & vbCrLf            ' 社員番号と同じもの
            strSql = strSql & "    AND staf.d_from = '" & Me.strUseDateAtt & "'" & vbCrLf           ' 適用日付（基本情報）と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = pClsDb.ExecuteSql(strSql)
            ' 0件チェック
            If tbRet.Rows.Count = 1 Then
                With tbRet.Rows(0)
                    Me.txtMemberNo.Text = NVL(.Item(0).ToString())                                      ' 01. 社員番号
                    Me.txtMemberNoDezit.Text = NVL(.Item(1).ToString())                                 ' 02. 社員番号ディジット
                    Me.txtKana.Text = NVL(.Item(2).ToString())                                          ' 03. フリガナ
                    Me.txtOldMemberNo.Text = NVL(.Item(3).ToString())                                   ' 04. 旧社員番号
                    Me.txtOldMemberNoDezit.Text = NVL(.Item(4).ToString())                              ' 05. 旧社員番号ディジット
                    Me.txtName.Text = NVL(.Item(5).ToString())                                          ' 06. 名前
                    Me.txtStatus.Text = NVL(.Item(6).ToString())                                        ' 07. ステータス
                    Me.txtUnionMember.Text = NVL(.Item(7).ToString())                                   ' 08. 組合員種別
                    If Me.bytStatus = STATUS_INSERT Then
                        '---------------------------------------
                        '   新規登録
                        '---------------------------------------
                        ' 適用日付（基本情報）と同じものを適用日付（住所情報）に格納
                        Me.strUseDateAdd = Me.strUseDateAtt
                        ' 画面の適用日付に適用日付（基本情報）を格納
                        Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strUseDateAtt), "0000/00/00")).ToString("yyyy年MM月dd日")
                    End If
                End With
            Else
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
    '   ＩＤ　：GetStafAddress
    '   名称　：住所情報取得処理
    '   概要  ：組合員管理 - 住所情報を取得する。
    '   引数　：ByVal pClsDb        As CLAccessMdb = データベースクラス
    '           ByVal pBytReference As Byte        = ステータス（1：通常検索, 2：最新検索（履歴ボタン押下後の適用日付選択画面で直接入力からの検索）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所情報取得処理</summary>
    ''' <param name="pClsDb">データベースクラス</param>
    ''' <param name="pBytReference">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStafAddress(ByVal pClsDb As CLAccessMdb, _
                                    ByVal pBytReference As Byte) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim tbRet As DataTable                          ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing              ' 処理結果件数
        Dim strTel1() As String                         ' 電話番号1
        Dim strTel2() As String                         ' 電話番号2
        Dim strTel3() As String                         ' 電話番号3
        Dim strFax() As String                          ' FAX

        Try
            ' SQL文作成
            If pBytReference = 1 Then
                ' 通常検索
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT staf.c_user_id" & vbCrLf                             ' 01. 個人認証ID
                strSql = strSql & "       ,staf.s_seq" & vbCrLf                                 ' 02. 住所SEQ
                strSql = strSql & "       ,staf.d_from" & vbCrLf                                ' 03. 適用開始年月日
                strSql = strSql & "       ,staf.k_add_kind" & vbCrLf                            ' 04. 住所種別
                strSql = strSql & "       ,staf.k_main_add" & vbCrLf                            ' 05. 現住所FLG
                strSql = strSql & "       ,staf.l_add_number" & vbCrLf                          ' 06. 郵便番号
                strSql = strSql & "       ,staf.l_prefectures" & vbCrLf                         ' 07. 都道府県
                strSql = strSql & "       ,staf.l_cities" & vbCrLf                              ' 08. 市区町村
                strSql = strSql & "       ,staf.l_add_ather" & vbCrLf                           ' 09. 番地等
                strSql = strSql & "       ,staf.l_building" & vbCrLf                            ' 10. 建物名等
                strSql = strSql & "       ,staf.k_foreign" & vbCrLf                             ' 11. 海外区分
                strSql = strSql & "       ,staf.l_foreign_adress_1" & vbCrLf                    ' 12. アドレス1
                strSql = strSql & "       ,staf.l_foreign_adress_2" & vbCrLf                    ' 13. アドレス2
                strSql = strSql & "       ,staf.l_foreign_adress_3" & vbCrLf                    ' 14. アドレス3
                strSql = strSql & "       ,staf.l_foreign_adress_4" & vbCrLf                    ' 15. アドレス4
                strSql = strSql & "       ,staf.l_foreign_adress_5" & vbCrLf                    ' 16. アドレス5
                strSql = strSql & "       ,staf.l_tell_1" & vbCrLf                              ' 17. 電話番号1
                strSql = strSql & "       ,staf.l_tell_2" & vbCrLf                              ' 18. 電話番号2
                strSql = strSql & "       ,staf.l_tell_3" & vbCrLf                              ' 19. 電話番号3
                strSql = strSql & "       ,staf.l_fax" & vbCrLf                                 ' 20. FAX
                strSql = strSql & "       ,staf.l_mail_pc" & vbCrLf                             ' 21. メールアドレスPC  
                strSql = strSql & "       ,staf.l_mail_mobile" & vbCrLf                         ' 22. メールアドレス携帯
                strSql = strSql & "       ,staf.l_biko_1" & vbCrLf                              ' 23. 備考
                strSql = strSql & "   FROM staf_address AS staf" & vbCrLf                       ' 組合員住所テーブル
                strSql = strSql & "  WHERE staf.s_seq = 1" & vbCrLf                             ' 住所SEQが 1 のもの
                strSql = strSql & "    AND staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf    ' 個人認証IDと同じもの
                strSql = strSql & "    AND staf.d_from = '" & Me.strUseDateAdd & "'" & vbCrLf   ' 適用開始年月日と同じもの
                strSql = strSql & "  ORDER BY staf.s_seq" & vbCrLf                              ' 住所SEQで並び替え   'chk 
                strSql = strSql & ";" & vbCrLf

            ElseIf pBytReference = 2 Then
                ' 最新検索
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT staf.c_user_id" & vbCrLf                             ' 01. 個人認証ID
                strSql = strSql & "       ,staf.s_seq" & vbCrLf                                 ' 02. 住所SEQ
                strSql = strSql & "       ,staf.d_from" & vbCrLf                                ' 03. 適用開始年月日
                strSql = strSql & "       ,staf.k_add_kind" & vbCrLf                            ' 04. 住所種別
                strSql = strSql & "       ,staf.k_main_add" & vbCrLf                            ' 05. 現住所FLG
                strSql = strSql & "       ,staf.l_add_number" & vbCrLf                          ' 06. 郵便番号
                strSql = strSql & "       ,staf.l_prefectures" & vbCrLf                         ' 07. 都道府県
                strSql = strSql & "       ,staf.l_cities" & vbCrLf                              ' 08. 市区町村
                strSql = strSql & "       ,staf.l_add_ather" & vbCrLf                           ' 09. 番地等
                strSql = strSql & "       ,staf.l_building" & vbCrLf                            ' 10. 建物名等
                strSql = strSql & "       ,staf.k_foreign" & vbCrLf                             ' 11. 海外区分
                strSql = strSql & "       ,staf.l_foreign_adress_1" & vbCrLf                    ' 12. アドレス1
                strSql = strSql & "       ,staf.l_foreign_adress_2" & vbCrLf                    ' 13. アドレス2
                strSql = strSql & "       ,staf.l_foreign_adress_3" & vbCrLf                    ' 14. アドレス3
                strSql = strSql & "       ,staf.l_foreign_adress_4" & vbCrLf                    ' 15. アドレス4
                strSql = strSql & "       ,staf.l_foreign_adress_5" & vbCrLf                    ' 16. アドレス5
                strSql = strSql & "       ,staf.l_tell_1" & vbCrLf                              ' 17. 電話番号1
                strSql = strSql & "       ,staf.l_tell_2" & vbCrLf                              ' 18. 電話番号2
                strSql = strSql & "       ,staf.l_tell_3" & vbCrLf                              ' 19. 電話番号3
                strSql = strSql & "       ,staf.l_fax" & vbCrLf                                 ' 20. FAX
                strSql = strSql & "       ,staf.l_mail_pc" & vbCrLf                             ' 21. メールアドレスPC  
                strSql = strSql & "       ,staf.l_mail_mobile" & vbCrLf                         ' 22. メールアドレス携帯
                strSql = strSql & "       ,staf.l_biko_1" & vbCrLf                              ' 23. 備考
                strSql = strSql & "   FROM staf_address AS staf" & vbCrLf                       ' 組合員住所テーブル
                ' 最新の住所情報取得
                strSql = strSql & "       ,( SELECT a.c_user_id" & vbCrLf
                strSql = strSql & "                ,MAX(a.d_from) AS d_from" & vbCrLf
                strSql = strSql & "            FROM staf_address AS a" & vbCrLf
                strSql = strSql & "           WHERE a.s_seq = 1" & vbCrLf
                strSql = strSql & "           GROUP BY a.c_user_id" & vbCrLf
                strSql = strSql & "                   ,a.s_seq ) AS staf_max" & vbCrLf
                strSql = strSql & "  WHERE staf.c_user_id = '" & Me.strUserId & "'" & vbCrLf    ' 個人認証IDと同じもの
                strSql = strSql & "    AND staf.c_user_id = staf_max.c_user_id" & vbCrLf
                strSql = strSql & "    AND staf.d_from = staf_max.d_from" & vbCrLf
                strSql = strSql & "  ORDER BY staf.s_seq" & UtDb.DbOrderOffset & vbCrLf 'ok                              ' 住所SEQで並び替え
                strSql = strSql & ";" & vbCrLf

            End If
            tbRet = pClsDb.ExecuteSql(strSql)                                               ' SQL実行
            intCntRet = tbRet.Rows.Count                                                    ' 件数取得
            ' 0件チェック
            If intCntRet <> 0 Then
                If intCntRet <= 3 Then
                    ' 件数分ループ（最大3件）
                    For i = 0 To intCntRet - 1
                        With tbRet.Rows(i)
                            If CInt(tbRet.Rows(i).Item(1)) = 1 Then
                                Me.TabPage1.Text = "住所1*"
                                ' 04. 適用日付
                                If pBytReference = 1 Then
                                    Me.txtUseDate.Text = Date.Parse(Format(CInt(.Item(2).ToString()), "0000/00/00")).ToString("yyyy年MM月dd日")
                                ElseIf pBytReference = 2 Then
                                    If Me.strUseDateAdd.Length = 0 Then
                                        Me.txtUseDate.Text = Date.Parse(Format(CInt(.Item(2).ToString()), "0000/00/00")).ToString("yyyy年MM月dd日")
                                    Else
                                        Me.txtUseDate.Text = Date.Parse(Format(CInt(Me.strUseDateAdd), "0000/00/00")).ToString("yyyy年MM月dd日")
                                    End If
                                End If
                                '===================================================================
                                '   住所1
                                '===================================================================
                                ' 04. 住所種別
                                Me.cboAddressKbn1.SelectedValue = NVL(.Item(3)).ToString()
                                ' 05. 現住所FLG
                                If .Item(4).ToString().Length = 0 Then
                                    Me.chkMainAddress1.Checked = False
                                Else
                                    If .Item(4).ToString() = "1" Then
                                        Me.chkMainAddress1.Checked = True
                                    ElseIf .Item(4).ToString() = "0" Then
                                        Me.chkMainAddress1.Checked = False
                                    Else
                                        Me.chkMainAddress1.Checked = CBool(.Item(4).ToString())
                                    End If
                                End If
                                ' 国内海外区分判定
                                If .Item(10).ToString() = INTERNATIONAL_KBN_HOME Then
                                    '---------------------------------------------------------------
                                    '   国内
                                    '---------------------------------------------------------------
                                    ' 06. 郵便番号
                                    If ChkNull(.Item(5).ToString()) Then
                                        Me.txtPostalNo1_1.Text = ""
                                        Me.txtPostalNo1_2.Text = ""
                                    Else
                                        Me.txtPostalNo1_1.Text = .Item(5).ToString().Substring(0, 3)
                                        Me.txtPostalNo1_2.Text = .Item(5).ToString().Substring(.Item(5).ToString().IndexOf("-") + 1)
                                    End If
                                    Me.cboPrefectures1.Text = NVL(.Item(6)).ToString()              ' 07. 都道府県
                                    Me.txtCities1.Text = NVL(.Item(7)).ToString()                   ' 08. 市区町村
                                    Me.txtAddAther1.Text = NVL(.Item(8)).ToString()                 ' 09. 番地等
                                    Me.txtBuilding1.Text = NVL(.Item(9)).ToString()                 ' 10. 建物名等
                                    Me.cboInternational1.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    ' 17. 電話番号1
                                    If ChkNull(.Item(16).ToString()) Then
                                        Me.txtTel1_1_1.Text = ""
                                        Me.txtTel1_1_2.Text = ""
                                        Me.txtTel1_1_3.Text = ""
                                    Else
                                        strTel1 = Split(.Item(16).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel1.Length - 1
                                            If j = 0 Then
                                                Me.txtTel1_1_1.Text = strTel1(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel1_1_2.Text = strTel1(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel1_1_3.Text = strTel1(j)
                                            End If
                                        Next
                                    End If
                                    ' 18. 電話番号2
                                    If ChkNull(.Item(17).ToString()) Then
                                        Me.txtTel1_2_1.Text = ""
                                        Me.txtTel1_2_2.Text = ""
                                        Me.txtTel1_2_3.Text = ""
                                    Else
                                        strTel2 = Split(.Item(17).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel2.Length - 1
                                            If j = 0 Then
                                                Me.txtTel1_2_1.Text = strTel2(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel1_2_2.Text = strTel2(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel1_2_3.Text = strTel2(j)
                                            End If
                                        Next
                                    End If
                                    ' 19. 電話番号3
                                    If ChkNull(.Item(18).ToString()) Then
                                        Me.txtTel1_3_1.Text = ""
                                        Me.txtTel1_3_2.Text = ""
                                        Me.txtTel1_3_3.Text = ""
                                    Else
                                        strTel3 = Split(.Item(18).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel3.Length - 1
                                            If j = 0 Then
                                                Me.txtTel1_3_1.Text = strTel3(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel1_3_2.Text = strTel3(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel1_3_3.Text = strTel3(j)
                                            End If
                                        Next
                                    End If
                                    ' 20. FAX
                                    If ChkNull(.Item(19).ToString()) Then
                                        Me.txtFax1_1.Text = ""
                                        Me.txtFax1_2.Text = ""
                                        Me.txtFax1_3.Text = ""
                                    Else
                                        strFax = Split(.Item(19).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strFax.Length - 1
                                            If j = 0 Then
                                                Me.txtFax1_1.Text = strFax(j)
                                            ElseIf j = 1 Then
                                                Me.txtFax1_2.Text = strFax(j)
                                            ElseIf j = 2 Then
                                                Me.txtFax1_3.Text = strFax(j)
                                            End If
                                        Next
                                    End If
                                    Me.txtMailPc1.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile1.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote1.Text = NVL(.Item(22)).ToString                      ' 23. 備考1

                                    ' 海外情報（表示なし）
                                    Me.txtForeignAdress1_1.Text = ""                                ' 12. アドレス1
                                    Me.txtForeignAdress1_2.Text = ""                                ' 13. アドレス2
                                    Me.txtForeignAdress1_3.Text = ""                                ' 14. アドレス3
                                    Me.txtForeignAdress1_4.Text = ""                                ' 15. アドレス4
                                    Me.txtForeignAdress1_5.Text = ""                                ' 16. アドレス5
                                    Me.txtTel1_1.Text = ""                                          ' 17. 電話番号1
                                    Me.txtTel1_2.Text = ""                                          ' 17. 電話番号2
                                    Me.txtTel1_3.Text = ""                                          ' 17. 電話番号3
                                    Me.txtFax1.Text = ""                                            ' 20. FAX1
                                ElseIf .Item(10).ToString() = INTERNATIONAL_KBN_ABROAD Then
                                    '---------------------------------------------------------------
                                    '   海外
                                    '---------------------------------------------------------------
                                    Me.cboInternational1.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    Me.txtForeignAdress1_1.Text = NVL(.Item(11)).ToString()         ' 12. アドレス1
                                    Me.txtForeignAdress1_2.Text = NVL(.Item(12)).ToString()         ' 13. アドレス2
                                    Me.txtForeignAdress1_3.Text = NVL(.Item(13)).ToString()         ' 14. アドレス3
                                    Me.txtForeignAdress1_4.Text = NVL(.Item(14)).ToString()         ' 15. アドレス4
                                    Me.txtForeignAdress1_5.Text = NVL(.Item(15)).ToString()         ' 16. アドレス5
                                    Me.txtTel1_1.Text = NVL(.Item(16)).ToString()                   ' 17. 電話番号1
                                    Me.txtTel1_2.Text = NVL(.Item(17)).ToString()                   ' 18. 電話番号2
                                    Me.txtTel1_3.Text = NVL(.Item(18)).ToString()                   ' 19. 電話番号3
                                    Me.txtFax1.Text = NVL(.Item(19)).ToString()                     ' 20. FAX1
                                    Me.txtMailPc1.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile1.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote1.Text = NVL(.Item(22)).ToString()                    ' 23. 備考1
                                    ' 国内情報（表示なし）
                                    Me.txtPostalNo1_1.Text = ""                                     ' 06. 郵便番号1
                                    Me.txtPostalNo1_2.Text = ""                                     ' 06. 郵便番号2
                                    Me.cboPrefectures1.Text = ""                                    ' 07. 都道府県
                                    Me.txtCities1.Text = ""                                         ' 08. 市区町村
                                    Me.txtAddAther1.Text = ""                                       ' 09. 番地等
                                    Me.txtBuilding1.Text = ""                                       ' 10. 建物名等
                                    Me.txtTel1_1_1.Text = ""                                        ' 17. 電話番号1-1
                                    Me.txtTel1_1_2.Text = ""                                        ' 17. 電話番号1-2
                                    Me.txtTel1_1_3.Text = ""                                        ' 17. 電話番号1-3
                                    Me.txtTel1_2_1.Text = ""                                        ' 18. 電話番号2-1
                                    Me.txtTel1_2_2.Text = ""                                        ' 18. 電話番号2-2
                                    Me.txtTel1_2_3.Text = ""                                        ' 18. 電話番号2-3
                                    Me.txtTel1_3_1.Text = ""                                        ' 19. 電話番号3-1
                                    Me.txtTel1_3_2.Text = ""                                        ' 19. 電話番号3-2
                                    Me.txtTel1_3_3.Text = ""                                        ' 19. 電話番号3-3
                                    Me.txtFax1_1.Text = ""                                          ' 20. FAX1-1
                                    Me.txtFax1_2.Text = ""                                          ' 20. FAX1-2
                                    Me.txtFax1_3.Text = ""                                          ' 20. FAX1-3
                                End If
                            ElseIf CInt(tbRet.Rows(i).Item(1)) = 2 Then
                                '===================================================================
                                '   住所2
                                '===================================================================
                                Me.TabPage2.Text = "住所2*"                                         ' 住所2タブ
                                Me.cboAddressKbn2.SelectedValue = NVL(.Item(3)).ToString()          ' 04. 住所種別
                                ' 05. 現住所FLG
                                If .Item(4).ToString().Length = 0 Then
                                    Me.chkMainAddress2.Checked = False
                                Else
                                    If .Item(4).ToString() = "1" Then
                                        Me.chkMainAddress2.Checked = True
                                    ElseIf .Item(4).ToString() = "0" Then
                                        Me.chkMainAddress2.Checked = False
                                    Else
                                        Me.chkMainAddress2.Checked = CBool(.Item(4).ToString())
                                    End If
                                End If
                                ' 国内海外区分判定
                                If .Item(10).ToString() = INTERNATIONAL_KBN_HOME Then
                                    '---------------------------------------------------------------
                                    '   国内
                                    '---------------------------------------------------------------
                                    ' 06. 郵便番号
                                    If ChkNull(.Item(5).ToString()) Then
                                        Me.txtPostalNo2_1.Text = ""
                                        Me.txtPostalNo2_2.Text = ""
                                    Else
                                        Me.txtPostalNo2_1.Text = .Item(5).ToString().Substring(0, 3)
                                        Me.txtPostalNo2_2.Text = .Item(5).ToString().Substring(.Item(5).ToString().IndexOf("-") + 1)
                                    End If
                                    Me.cboPrefectures2.Text = NVL(.Item(6)).ToString()              ' 07. 都道府県
                                    Me.txtCities2.Text = NVL(.Item(7)).ToString()                   ' 08. 市区町村
                                    Me.txtAddAther2.Text = NVL(.Item(8)).ToString()                 ' 09. 番地等
                                    Me.txtBuilding2.Text = NVL(.Item(9)).ToString()                 ' 10. 建物名等
                                    Me.cboInternational2.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    ' 17. 電話番号1
                                    If ChkNull(.Item(16).ToString()) Then
                                        Me.txtTel2_1_1.Text = ""
                                        Me.txtTel2_1_2.Text = ""
                                        Me.txtTel2_1_3.Text = ""
                                    Else
                                        strTel1 = Split(.Item(16).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel1.Length - 1
                                            If j = 0 Then
                                                Me.txtTel2_1_1.Text = strTel1(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel2_1_2.Text = strTel1(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel2_1_3.Text = strTel1(j)
                                            End If
                                        Next
                                    End If
                                    ' 18. 電話番号2
                                    If ChkNull(.Item(17).ToString()) Then
                                        Me.txtTel2_2_1.Text = ""
                                        Me.txtTel2_2_2.Text = ""
                                        Me.txtTel2_2_3.Text = ""
                                    Else
                                        strTel2 = Split(.Item(17).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel2.Length - 1
                                            If j = 0 Then
                                                Me.txtTel2_2_1.Text = strTel2(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel2_2_2.Text = strTel2(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel2_2_3.Text = strTel2(j)
                                            End If
                                        Next
                                    End If
                                    ' 19. 電話番号3
                                    If ChkNull(.Item(18).ToString()) Then
                                        Me.txtTel2_3_1.Text = ""
                                        Me.txtTel2_3_2.Text = ""
                                        Me.txtTel2_3_3.Text = ""
                                    Else
                                        strTel3 = Split(.Item(18).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel3.Length - 1
                                            If j = 0 Then
                                                Me.txtTel2_3_1.Text = strTel3(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel2_3_2.Text = strTel3(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel2_3_3.Text = strTel3(j)
                                            End If
                                        Next
                                    End If
                                    ' 20. FAX
                                    If ChkNull(.Item(19).ToString()) Then
                                        Me.txtFax2_1.Text = ""
                                        Me.txtFax2_2.Text = ""
                                        Me.txtFax2_3.Text = ""
                                    Else
                                        strFax = Split(.Item(19).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strFax.Length - 1
                                            If j = 0 Then
                                                Me.txtFax2_1.Text = strFax(j)
                                            ElseIf j = 1 Then
                                                Me.txtFax2_2.Text = strFax(j)
                                            ElseIf j = 2 Then
                                                Me.txtFax2_3.Text = strFax(j)
                                            End If
                                        Next
                                    End If
                                    Me.txtMailPc2.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile2.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote2.Text = NVL(.Item(22)).ToString                      ' 23. 備考1
                                    ' 海外情報（表示なし）
                                    Me.txtForeignAdress2_1.Text = ""                                ' 12. アドレス1
                                    Me.txtForeignAdress2_2.Text = ""                                ' 13. アドレス2
                                    Me.txtForeignAdress2_3.Text = ""                                ' 14. アドレス3
                                    Me.txtForeignAdress2_4.Text = ""                                ' 15. アドレス4
                                    Me.txtForeignAdress2_5.Text = ""                                ' 16. アドレス5
                                    Me.txtTel2_1.Text = ""                                          ' 17. 電話番号1
                                    Me.txtTel2_2.Text = ""                                          ' 18. 電話番号2
                                    Me.txtTel2_3.Text = ""                                          ' 19. 電話番号3
                                    Me.txtFax2.Text = ""                                            ' 20. FAX
                                ElseIf .Item(10).ToString() = INTERNATIONAL_KBN_ABROAD Then
                                    '---------------------------------------------------------------
                                    '   海外
                                    '---------------------------------------------------------------
                                    Me.txtPostalNo2_1.Text = ""                                     ' 06. 郵便番号1
                                    Me.txtPostalNo2_2.Text = ""                                     ' 06. 郵便番号2
                                    Me.cboPrefectures2.Text = NVL(.Item(6)).ToString()              ' 07. 都道府県
                                    Me.txtCities2.Text = NVL(.Item(7)).ToString()                   ' 08. 市区町村
                                    Me.txtAddAther2.Text = NVL(.Item(8)).ToString()                 ' 09. 番地等
                                    Me.txtBuilding2.Text = NVL(.Item(9)).ToString()                 ' 10. 建物名等
                                    Me.cboInternational2.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    Me.txtForeignAdress2_1.Text = NVL(.Item(11)).ToString()         ' 12. アドレス1
                                    Me.txtForeignAdress2_2.Text = NVL(.Item(12)).ToString()         ' 13. アドレス2
                                    Me.txtForeignAdress2_3.Text = NVL(.Item(13)).ToString()         ' 14. アドレス3
                                    Me.txtForeignAdress2_4.Text = NVL(.Item(14)).ToString()         ' 15. アドレス4
                                    Me.txtForeignAdress2_5.Text = NVL(.Item(15)).ToString()         ' 16. アドレス5
                                    Me.txtTel2_1.Text = NVL(.Item(16)).ToString()                   ' 17. 電話番号1
                                    Me.txtTel2_2.Text = NVL(.Item(17)).ToString()                   ' 18. 電話番号2
                                    Me.txtTel2_3.Text = NVL(.Item(18)).ToString()                   ' 19. 電話番号3
                                    Me.txtFax2.Text = NVL(.Item(19)).ToString()                     ' 20. FAX1
                                    Me.txtMailPc2.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile2.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote2.Text = NVL(.Item(22)).ToString                      ' 23. 備考1
                                    ' 国内情報（表示なし）
                                    Me.txtTel2_1_1.Text = ""                                        ' 17. 電話番号1-1
                                    Me.txtTel2_1_2.Text = ""                                        ' 17. 電話番号1-2
                                    Me.txtTel2_1_3.Text = ""                                        ' 17. 電話番号1-3
                                    Me.txtTel2_2_1.Text = ""                                        ' 18. 電話番号2-1
                                    Me.txtTel2_2_2.Text = ""                                        ' 18. 電話番号2-2
                                    Me.txtTel2_2_3.Text = ""                                        ' 18. 電話番号2-3
                                    Me.txtTel2_3_1.Text = ""                                        ' 19. 電話番号3-1
                                    Me.txtTel2_3_2.Text = ""                                        ' 19. 電話番号3-2
                                    Me.txtTel2_3_3.Text = ""                                        ' 19. 電話番号3-3
                                    Me.txtFax2_1.Text = ""                                          ' 20. FAX1-1
                                    Me.txtFax2_2.Text = ""                                          ' 20. FAX2-2
                                    Me.txtFax2_3.Text = ""                                          ' 20. FAX3-3
                                End If
                            ElseIf CInt(tbRet.Rows(i).Item(1)) = 3 Then
                                '===================================================================
                                '   住所3
                                '===================================================================
                                Me.TabPage3.Text = "住所3*"                                         ' 住所3タブ
                                Me.cboAddressKbn3.SelectedValue = NVL(.Item(3)).ToString()          ' 04. 住所種別
                                ' 05. 現住所FLG
                                If .Item(4).ToString().Length = 0 Then
                                    Me.chkMainAddress3.Checked = False
                                Else
                                    If .Item(4).ToString() = "1" Then
                                        Me.chkMainAddress3.Checked = True
                                    ElseIf .Item(4).ToString() = "0" Then
                                        Me.chkMainAddress3.Checked = False
                                    Else
                                        Me.chkMainAddress3.Checked = CBool(.Item(4).ToString())
                                    End If
                                End If
                                ' 国内海外区分判定
                                If .Item(10).ToString() = INTERNATIONAL_KBN_HOME Then
                                    '---------------------------------------------------------------
                                    '   国内
                                    '---------------------------------------------------------------
                                    ' 06. 郵便番号
                                    If ChkNull(.Item(5).ToString()) Then
                                        Me.txtPostalNo3_1.Text = ""
                                        Me.txtPostalNo3_2.Text = ""
                                    Else
                                        Me.txtPostalNo3_1.Text = .Item(5).ToString().Substring(0, 3)
                                        Me.txtPostalNo3_2.Text = .Item(5).ToString().Substring(.Item(5).ToString().IndexOf("-") + 1)
                                    End If
                                    Me.cboPrefectures3.Text = NVL(.Item(6)).ToString()              ' 07. 都道府県
                                    Me.txtCities3.Text = NVL(.Item(7)).ToString()                   ' 08. 市区町村
                                    Me.txtAddAther3.Text = NVL(.Item(8)).ToString()                 ' 09. 番地等
                                    Me.txtBuilding3.Text = NVL(.Item(9)).ToString()                 ' 10. 建物名等
                                    Me.cboInternational3.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    ' 17. 電話番号1
                                    If ChkNull(.Item(16).ToString()) Then
                                        Me.txtTel3_1_1.Text = ""
                                        Me.txtTel3_1_2.Text = ""
                                        Me.txtTel3_1_3.Text = ""
                                    Else
                                        strTel1 = Split(.Item(16).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel1.Length - 1
                                            If j = 0 Then
                                                Me.txtTel3_1_1.Text = strTel1(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel3_1_2.Text = strTel1(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel3_1_3.Text = strTel1(j)
                                            End If
                                        Next
                                    End If
                                    ' 18. 電話番号2
                                    If ChkNull(.Item(17).ToString()) Then
                                        Me.txtTel3_2_1.Text = ""
                                        Me.txtTel3_2_2.Text = ""
                                        Me.txtTel3_2_3.Text = ""
                                    Else
                                        strTel2 = Split(.Item(17).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel2.Length - 1
                                            If j = 0 Then
                                                Me.txtTel3_2_1.Text = strTel2(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel3_2_2.Text = strTel2(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel3_2_3.Text = strTel2(j)
                                            End If
                                        Next
                                    End If
                                    ' 19. 電話番号3
                                    If ChkNull(.Item(18).ToString()) Then
                                        Me.txtTel3_3_1.Text = ""
                                        Me.txtTel3_3_2.Text = ""
                                        Me.txtTel3_3_3.Text = ""
                                    Else
                                        strTel3 = Split(.Item(18).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strTel3.Length - 1
                                            If j = 0 Then
                                                Me.txtTel3_3_1.Text = strTel3(j)
                                            ElseIf j = 1 Then
                                                Me.txtTel3_3_2.Text = strTel3(j)
                                            ElseIf j = 2 Then
                                                Me.txtTel3_3_3.Text = strTel3(j)
                                            End If
                                        Next
                                    End If
                                    ' 20. FAX
                                    If ChkNull(.Item(19).ToString()) Then
                                        Me.txtFax3_1.Text = ""
                                        Me.txtFax3_2.Text = ""
                                        Me.txtFax3_3.Text = ""
                                    Else
                                        strFax = Split(.Item(19).ToString(), "-", -1, CompareMethod.Binary)
                                        For j = 0 To strFax.Length - 1
                                            If j = 0 Then
                                                Me.txtFax3_1.Text = strFax(j)
                                            ElseIf j = 1 Then
                                                Me.txtFax3_2.Text = strFax(j)
                                            ElseIf j = 2 Then
                                                Me.txtFax3_3.Text = strFax(j)
                                            End If
                                        Next
                                    End If
                                    Me.txtMailPc3.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile3.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote3.Text = NVL(.Item(22)).ToString                      ' 23. 備考1
                                    ' 海外情報（表示なし）
                                    Me.txtForeignAdress3_1.Text = ""                                ' 12. アドレス1
                                    Me.txtForeignAdress3_2.Text = ""                                ' 13. アドレス2
                                    Me.txtForeignAdress3_3.Text = ""                                ' 14. アドレス3
                                    Me.txtForeignAdress3_4.Text = ""                                ' 15. アドレス4
                                    Me.txtForeignAdress3_5.Text = ""                                ' 16. アドレス5
                                    Me.txtTel3_1.Text = ""                                          ' 17. 電話番号1
                                    Me.txtTel3_2.Text = ""                                          ' 18. 電話番号2
                                    Me.txtTel3_3.Text = ""                                          ' 19. 電話番号3
                                    Me.txtFax3.Text = ""                                            ' 20. FAX
                                ElseIf .Item(10).ToString() = INTERNATIONAL_KBN_ABROAD Then
                                    '---------------------------------------------------------------
                                    '   海外
                                    '---------------------------------------------------------------
                                    Me.txtPostalNo3_1.Text = ""                                     ' 06. 郵便番号1
                                    Me.txtPostalNo3_2.Text = ""                                     ' 06. 郵便番号2
                                    Me.cboPrefectures3.Text = NVL(.Item(6)).ToString()              ' 07. 都道府県
                                    Me.txtCities3.Text = NVL(.Item(7)).ToString()                   ' 08. 市区町村
                                    Me.txtAddAther3.Text = NVL(.Item(8)).ToString()                 ' 09. 番地等
                                    Me.txtBuilding3.Text = NVL(.Item(9)).ToString()                 ' 10. 建物名等
                                    Me.cboInternational3.SelectedValue = NVL(.Item(10)).ToString()  ' 11. 海外区分
                                    Me.txtForeignAdress3_1.Text = NVL(.Item(11)).ToString()         ' 12. アドレス1
                                    Me.txtForeignAdress3_2.Text = NVL(.Item(12)).ToString()         ' 13. アドレス2
                                    Me.txtForeignAdress3_3.Text = NVL(.Item(13)).ToString()         ' 14. アドレス3
                                    Me.txtForeignAdress3_4.Text = NVL(.Item(14)).ToString()         ' 15. アドレス4
                                    Me.txtForeignAdress3_5.Text = NVL(.Item(15)).ToString()         ' 16. アドレス5
                                    Me.txtTel3_1.Text = NVL(.Item(16)).ToString()                   ' 17. 電話番号1
                                    Me.txtTel3_2.Text = NVL(.Item(17)).ToString()                   ' 18. 電話番号2
                                    Me.txtTel3_3.Text = NVL(.Item(18)).ToString()                   ' 19. 電話番号3
                                    Me.txtFax3.Text = NVL(.Item(19)).ToString()                     ' 20. FAX1
                                    Me.txtMailPc3.Text = NVL(.Item(20)).ToString()                  ' 21. メールアドレスPC
                                    Me.txtMailMobile3.Text = NVL(.Item(21)).ToString()              ' 22. メールアドレス携帯
                                    Me.txtNote3.Text = NVL(.Item(22)).ToString()                    ' 23. 備考1
                                    ' 国内情報（表示なし）
                                    Me.txtTel3_1_1.Text = ""                                        ' 17. 電話番号1-1
                                    Me.txtTel3_1_2.Text = ""                                        ' 17. 電話番号1-2
                                    Me.txtTel3_1_3.Text = ""                                        ' 17. 電話番号1-3
                                    Me.txtTel3_2_1.Text = ""                                        ' 18. 電話番号2-1
                                    Me.txtTel3_2_2.Text = ""                                        ' 18. 電話番号2-2
                                    Me.txtTel3_2_3.Text = ""                                        ' 18. 電話番号2-3
                                    Me.txtTel3_3_1.Text = ""                                        ' 19. 電話番号3-1
                                    Me.txtTel3_3_2.Text = ""                                        ' 19. 電話番号3-2
                                    Me.txtTel3_3_3.Text = ""                                        ' 19. 電話番号3-3
                                    Me.txtFax3_1.Text = ""                                          ' 20. FAX1-1
                                    Me.txtFax3_2.Text = ""                                          ' 20. FAX2-2
                                    Me.txtFax3_3.Text = ""                                          ' 20. FAX3-3
                                End If
                            End If
                        End With
                    Next
                Else
                    ' 3件以上なのでエラー
                    Call MessageBox.Show("住所が3件以上もあるなんて信じられない。", _
                                         "エラー", _
                                         MessageBoxButtons.OK, _
                                         MessageBoxIcon.Error, _
                                         MessageBoxDefaultButton.Button1)
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
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ClearAddress
    '   名称　：住所クリア処理
    '   概要  ：住所情報をクリアする。
    '   引数　：ByVal bytTab As Byte = クリア押下タブ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所クリア</summary>
    ''' <param name="bytTab">住所タブ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ClearAddress(ByVal bytTab As Byte) As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果

        Try
            ' タブ判定
            If bytTab = ADDRESS_TAB1 Then
                '---------------------------------------------------------------
                '   住所1
                '---------------------------------------------------------------
                ' 共通
                Me.cboAddressKbn1.SelectedIndex = 0         ' 住所区分1
                Me.cboInternational1.SelectedIndex = 0      ' 国内海外区分1
                Me.txtMailPc1.Text = ""                     ' メールアドレスPC1
                Me.txtMailMobile1.Text = ""                 ' メールアドレス携帯1
                Me.txtNote1.Text = ""                       ' 備考1
                ' 国内
                Me.txtPostalNo1_1.Text = ""                 ' 郵便番号1_1
                Me.txtPostalNo1_2.Text = ""                 ' 郵便番号1_2
                Me.cboPrefectures1.SelectedIndex = -1       ' 都道府県1
                Me.txtCities1.Text = ""                     ' 市区町村1
                Me.txtAddAther1.Text = ""                   ' 番地等1
                Me.txtBuilding1.Text = ""                   ' 建物名等1
                Me.txtTel1_1_1.Text = ""                    ' 電話番号1_1_1
                Me.txtTel1_1_2.Text = ""                    ' 電話番号1_1_2
                Me.txtTel1_1_3.Text = ""                    ' 電話番号1_3
                Me.txtTel1_2_1.Text = ""                    ' 電話番号1_2_1
                Me.txtTel1_2_2.Text = ""                    ' 電話番号1_2_2
                Me.txtTel1_2_3.Text = ""                    ' 電話番号1_2_3
                Me.txtTel1_3_1.Text = ""                    ' 電話番号1_3_1
                Me.txtTel1_3_2.Text = ""                    ' 電話番号1_3_2
                Me.txtTel1_3_3.Text = ""                    ' 電話番号1_3_3
                Me.txtFax1_1.Text = ""                      ' FAX1_1
                Me.txtFax1_2.Text = ""                      ' FAX1_2
                Me.txtFax1_3.Text = ""                      ' FAX1_3
                ' 海外
                Me.txtTel1_1.Text = ""                      ' 電話番号1_1
                Me.txtTel1_2.Text = ""                      ' 電話番号1_2
                Me.txtTel1_3.Text = ""                      ' 電話番号1_3
                Me.txtFax1.Text = ""                        ' FAX1
                Me.txtForeignAdress1_1.Text = ""            ' アドレス1_1
                Me.txtForeignAdress1_2.Text = ""            ' アドレス1_2
                Me.txtForeignAdress1_3.Text = ""            ' アドレス1_3
                Me.txtForeignAdress1_4.Text = ""            ' アドレス1_4
                Me.txtForeignAdress1_5.Text = ""            ' アドレス1_5

            ElseIf bytTab = ADDRESS_TAB2 Then

                '---------------------------------------------------------------
                '   住所2
                '---------------------------------------------------------------
                ' 共通
                Me.cboAddressKbn2.SelectedIndex = 0         ' 住所区分2
                Me.cboInternational2.SelectedIndex = 0      ' 国内海外区分2
                Me.txtMailPc2.Text = ""                     ' メールアドレスPC2
                Me.txtMailMobile2.Text = ""                 ' メールアドレス携帯2
                Me.txtNote2.Text = ""                       ' 備考2
                ' 国内
                Me.txtPostalNo2_1.Text = ""                 ' 郵便番号2_1
                Me.txtPostalNo2_2.Text = ""                 ' 郵便番号2_2
                Me.cboPrefectures2.SelectedIndex = -1       ' 都道府県2
                Me.txtCities2.Text = ""                     ' 市区町村2
                Me.txtAddAther2.Text = ""                   ' 番地等2
                Me.txtBuilding2.Text = ""                   ' 建物名等2
                Me.txtTel2_1_1.Text = ""                    ' 電話番号2_1_1
                Me.txtTel2_1_2.Text = ""                    ' 電話番号2_1_2
                Me.txtTel2_1_3.Text = ""                    ' 電話番号2_1_3
                Me.txtTel2_2_1.Text = ""                    ' 電話番号2_2_1
                Me.txtTel2_2_2.Text = ""                    ' 電話番号2_2_2
                Me.txtTel2_2_3.Text = ""                    ' 電話番号2_2_3
                Me.txtTel2_3_1.Text = ""                    ' 電話番号2_3_1
                Me.txtTel2_3_2.Text = ""                    ' 電話番号2_3_2
                Me.txtTel2_3_3.Text = ""                    ' 電話番号2_3_3
                Me.txtFax2_1.Text = ""                      ' FAX2_1
                Me.txtFax2_2.Text = ""                      ' FAX2_2
                Me.txtFax2_3.Text = ""                      ' FAX2_3
                ' 海外
                Me.txtTel2_1.Text = ""                      ' 電話番号2_1
                Me.txtTel2_2.Text = ""                      ' 電話番号2_2
                Me.txtTel2_3.Text = ""                      ' 電話番号2_3
                Me.txtFax2.Text = ""                        ' FAX2
                Me.txtForeignAdress2_1.Text = ""            ' アドレス2_1
                Me.txtForeignAdress2_2.Text = ""            ' アドレス2_2
                Me.txtForeignAdress2_3.Text = ""            ' アドレス2_3
                Me.txtForeignAdress2_4.Text = ""            ' アドレス2_4
                Me.txtForeignAdress2_5.Text = ""            ' アドレス2_5

            ElseIf bytTab = ADDRESS_TAB3 Then
                '---------------------------------------------------------------
                '   住所3
                '---------------------------------------------------------------
                ' 共通
                Me.cboAddressKbn3.SelectedIndex = 0         ' 住所区分3
                Me.cboInternational3.SelectedIndex = 0      ' 国内海外区分3
                Me.txtMailPc3.Text = ""                     ' メールアドレスPC3
                Me.txtMailMobile3.Text = ""                 ' メールアドレス携帯3
                Me.txtNote3.Text = ""                       ' 備考3
                Me.txtPostalNo3_1.Text = ""                 ' 郵便番号3_1
                Me.txtPostalNo3_2.Text = ""                 ' 郵便番号3_2
                Me.cboPrefectures3.SelectedIndex = -1       ' 都道府県3
                Me.txtCities3.Text = ""                     ' 市区町村3
                Me.txtAddAther3.Text = ""                   ' 番地等3
                Me.txtBuilding3.Text = ""                   ' 建物名等3
                Me.txtTel3_1_1.Text = ""                    ' 電話番号3_1_1
                Me.txtTel3_1_2.Text = ""                    ' 電話番号3_1_2
                Me.txtTel3_1_3.Text = ""                    ' 電話番号3_1_3
                Me.txtTel3_2_1.Text = ""                    ' 電話番号3_2_1
                Me.txtTel3_2_2.Text = ""                    ' 電話番号3_2_2
                Me.txtTel3_2_3.Text = ""                    ' 電話番号3_2_3
                Me.txtTel3_3_1.Text = ""                    ' 電話番号3_3_1
                Me.txtTel3_3_2.Text = ""                    ' 電話番号3_3_2
                Me.txtTel3_3_3.Text = ""                    ' 電話番号3_3_3
                Me.txtFax3_1.Text = ""                      ' FAX3_1
                Me.txtFax3_2.Text = ""                      ' FAX3_2
                Me.txtFax3_3.Text = ""                      ' FAX3_3
                ' 海外
                Me.txtTel3_1.Text = ""                      ' 電話番号3_1
                Me.txtTel3_2.Text = ""                      ' 電話番号3_2
                Me.txtTel3_3.Text = ""                      ' 電話番号3_3
                Me.txtFax3.Text = ""                        ' FAX3
                Me.txtForeignAdress3_1.Text = ""            ' アドレス3_1
                Me.txtForeignAdress3_2.Text = ""            ' アドレス3_2
                Me.txtForeignAdress3_3.Text = ""            ' アドレス3_3
                Me.txtForeignAdress3_4.Text = ""            ' アドレス3_4
                Me.txtForeignAdress3_5.Text = ""            ' アドレス3_5

            End If

            ' 処理結果に正常設定
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
    '   ＩＤ　：SearchAddress
    '   名称　：住所検索処理
    '   概要　：郵便番号1・郵便番号2を元にSQLを作成し、子画面を呼び出す。
    '   引数　：ByVal bytTab As Byte = 1：住所1, 2：住所2, 3：住所3
    '           ByVal strPostalNo1 As String = 郵便番号1
    '           ByVal strPostalNo1 As String = 郵便番号2
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所検索処理</summary>
    ''' <param name="bytTab">住所タブ</param>
    ''' <param name="strPostalNo1">郵便番号1</param>
    ''' <param name="strPostalNo2">郵便番号2</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SearchAddress(ByVal bytTab As Byte, _
                                   ByVal strPostalNo1 As String, _
                                   ByVal strPostalNo2 As String) As Boolean

        Dim blnRet As Boolean                           ' 処理結果
        Dim strSql As String = ""                       ' SQL
        Dim clsFM000202 As FM000202 = Nothing           ' 郵便番号検索画面クラス

        Try
            ' SQL作成
            strSql = ""
            strSql = strSql & " SELECT zip_code_full.c_zip_code" & vbCrLf           ' 郵便番号
            strSql = strSql & "       ,zip_code_full.l_prefectures" & vbCrLf        ' 都道府県名
            strSql = strSql & "       ,zip_code_full.l_cities" & vbCrLf             ' 市区町村名
            strSql = strSql & "       ,zip_code_full.l_cities_area" & vbCrLf        ' 町域名
            strSql = strSql & "   FROM zip_code_full" & vbCrLf                      ' 郵便番号マスタ
            If (strPostalNo1 & strPostalNo2).Length <> 0 Then                       ' 郵便番号桁数チェック
                ' 郵便番号（郵便番号1（上3桁） + 郵便番号2（下3桁））が1桁以上ある場合
                If (strPostalNo1 & strPostalNo2).Length = 7 Then
                    ' 7桁ある場合、固定検索
                    strSql = strSql & "  WHERE zip_code_full.c_zip_code = '" & strPostalNo1 & strPostalNo2 & "'" & vbCrLf
                Else
                    ' 7桁以下の場合、曖昧検索
                    If strPostalNo1.Length = 0 Then
                        ' 郵便番号1（上3桁）が0桁の場合、郵便番号2（下4桁）のいずれかはあるので、郵便番号2で曖昧検索
                        strSql = strSql & "  WHERE zip_code_full.c_zip_code LIKE '%" & strPostalNo2 & "%'" & vbCrLf
                    Else
                        ' 郵便番号1（上桁3）が1桁以上の場合
                        If strPostalNo2.Length = 0 Then
                            ' 郵便番号1（上桁3）が1桁以上、郵便番号2（下3桁）が0桁の場合、郵便番号1で曖昧検索
                            strSql = strSql & "  WHERE zip_code_full.c_zip_code LIKE '%" & strPostalNo1 & "%'" & vbCrLf
                        Else
                            ' 郵便番号1（上桁3）が1桁以上、郵便番号2（下3桁）が1桁の場合、郵便番号1 + 郵便番号2で曖昧検索
                            strSql = strSql & "  WHERE zip_code_full.c_zip_code LIKE '%" & strPostalNo1 & "%" & strPostalNo2 & "%'" & vbCrLf
                        End If
                    End If
                End If
            End If
            ' 郵便番号検索画面クラス生成
            clsFM000202 = New FM000202
            ' パラメータ設定
            clsFM000202.strSqlSentence = strSql
            ' 郵便番号検索画面表示
            'Call clsFM000202.ShowDialog()
            Call clsFM000202.SqlQueryShowDataGrid()
            If clsFM000202.DialogResult = DialogResult.OK Then
                If clsFM000202.SelectAddress.Rows.Count <> 0 Then
                    With clsFM000202.SelectAddress.Rows(0)
                        If bytTab = ADDRESS_TAB1 Then
                            Me.txtPostalNo1_1.Text = .Item(0).ToString().Substring(0, 3)    ' 郵便番号1
                            Me.txtPostalNo1_2.Text = .Item(0).ToString().Substring(3, 4)    ' 郵便番号2
                            Me.cboPrefectures1.Text = .Item(1).ToString()                   ' 都道府県名
                            Me.txtCities1.Text = .Item(2).ToString()                        ' 市区町村名
                            Me.txtAddAther1.Text = .Item(3).ToString()                      ' 町域名
                        ElseIf bytTab = ADDRESS_TAB2 Then
                            Me.txtPostalNo2_1.Text = .Item(0).ToString().Substring(0, 3)    ' 郵便番号1
                            Me.txtPostalNo2_2.Text = .Item(0).ToString().Substring(3, 4)    ' 郵便番号2
                            Me.cboPrefectures2.Text = .Item(1).ToString()                   ' 都道府県名
                            Me.txtCities2.Text = .Item(2).ToString()                        ' 市区町村名
                            Me.txtAddAther2.Text = .Item(3).ToString()                      ' 町域名
                        ElseIf bytTab = ADDRESS_TAB3 Then
                            Me.txtPostalNo3_1.Text = .Item(0).ToString().Substring(0, 3)    ' 郵便番号1
                            Me.txtPostalNo3_2.Text = .Item(0).ToString().Substring(3, 4)    ' 郵便番号2
                            Me.cboPrefectures3.Text = .Item(1).ToString()                   ' 都道府県名
                            Me.txtCities3.Text = .Item(2).ToString()                        ' 市区町村名
                            Me.txtAddAther3.Text = .Item(3).ToString()                      ' 町域名
                        End If
                    End With
                End If
            End If

            clsFM000202.Close()                     ' 画面閉じる
            clsFM000202.Dispose()                   ' クラス破棄

            '処理結果に正常を設定
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
    '   ＩＤ　：ChangeAddress
    '   名称　：住所区分チェンジ処理
    '   概要　：住所情報表示の切り替えを行う。
    '   引数　：ByVal bytTab  As Byte      = 住所タブ（1：住所1, 2：住所2, 3：住所3）
    '           ByVal pIntIndex As Integer = 住所区分インデックス（0：住所区分空白, 1以上：住所区分空白以外）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所区分チェンジ処理</summary>
    ''' <param name="pBytTab">住所タブ</param>
    ''' <param name="pIntIndex">住所区分インデックス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeAddress(ByVal pBytTab As Byte, _
                                   ByVal pIntIndex As Integer) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim blnFlg As Boolean = False               ' 表示・非表示フラグ（True：住所区分空白以外, False：住所区分空白）
        Dim clrBack As Color = Color.White          ' バックカラー

        Try
            ' 表示・非表示フラグ
            If pIntIndex = 0 Then
                blnFlg = False
                clrBack = Color.Gainsboro
            Else
                blnFlg = True
            End If

            ' 住所タブ判定
            If pBytTab = ADDRESS_TAB1 Then
                '=======================================================
                '   住所1
                '=======================================================
                Me.lblMandatoryPostalNo1.Enabled = blnFlg               ' 必須　郵便番号1 / アドレス1_1
                Me.lblMandatoryInternational1.Enabled = blnFlg          ' 必須　国内海外区分1
                Me.lblMandatoryPrefectures1.Enabled = blnFlg            ' 必須　都道府県1
                Me.lblMandatoryCities1.Enabled = blnFlg                 ' 必須　市区町村1
                Me.lblMandatoryAddAther1.Enabled = blnFlg               ' 必須　番地等1
                Me.lblMandatoryTel1.Enabled = blnFlg                    ' 必須　電話番号1

                '-----------------------------------
                '   国内
                '-----------------------------------
                ' Label
                Me.lblPostalNo1.Enabled = blnFlg                        ' 郵便番号1
                Me.lblPrefectures1.Enabled = blnFlg                     ' 都道府県1
                Me.lblCities1.Enabled = blnFlg                          ' 市区町村1
                Me.lblAddAther1.Enabled = blnFlg                        ' 番地等1
                Me.lblBuilding1.Enabled = blnFlg                        ' 建物名等1
                Me.lblTel1_1.Enabled = blnFlg                           ' 電話番号1_1
                Me.lblTel1_2.Enabled = blnFlg                           ' 電話番号1_2
                Me.lblTel1_3.Enabled = blnFlg                           ' 電話番号1_3
                Me.lblFax1.Enabled = blnFlg                             ' FAX1
                Me.lblNote1.Enabled = blnFlg                            ' 備考1
                Me.lblMailPc1.Enabled = blnFlg                          ' Email1_1
                Me.lblMailMobile1.Enabled = blnFlg                      ' Email1_2

                Me.lblPostalNoHyphen1.Enabled = blnFlg                  ' 郵便番号1ハイフン1
                Me.lblTelHyphen1_1_1.Enabled = blnFlg                   ' 電話番号1_1ハイフン1
                Me.lblTelHyphen1_1_2.Enabled = blnFlg                   ' 電話番号1_1ハイフン2
                Me.lblTelHyphen1_2_1.Enabled = blnFlg                   ' 電話番号1_2ハイフン1
                Me.lblTelHyphen1_2_2.Enabled = blnFlg                   ' 電話番号1_2ハイフン2
                Me.lblTelHyphen1_3_1.Enabled = blnFlg                   ' 電話番号1_3ハイフン1
                Me.lblTelHyphen1_3_2.Enabled = blnFlg                   ' 電話番号1_3ハイフン2
                Me.lblFaxHyphen1_1.Enabled = blnFlg                     ' FAX1ハイフン1
                Me.lblFaxHyphen1_2.Enabled = blnFlg                     ' FAX1ハイフン2

                ' Button
                Me.btnAddressSearch1.Enabled = blnFlg                   ' 住所検索1ボタン
                Me.btnClear1.Enabled = blnFlg                           ' クリア1タン

                ' TextBox
                Me.txtPostalNo1_1.Enabled = blnFlg                      ' 郵便番号1_1
                Me.txtPostalNo1_2.Enabled = blnFlg                      ' 郵便番号1_2
                Me.txtCities1.Enabled = blnFlg                          ' 市区町村1
                Me.txtAddAther1.Enabled = blnFlg                        ' 番地等1
                Me.txtBuilding1.Enabled = blnFlg                        ' 建物名等1
                Me.txtTel1_1_1.Enabled = blnFlg                         ' 電話番号1_1
                Me.txtTel1_1_2.Enabled = blnFlg                         ' 電話番号1_2
                Me.txtTel1_1_3.Enabled = blnFlg                         ' 電話番号1_3
                Me.txtTel1_2_1.Enabled = blnFlg                         ' 電話番号2_1
                Me.txtTel1_2_2.Enabled = blnFlg                         ' 電話番号2_2
                Me.txtTel1_2_3.Enabled = blnFlg                         ' 電話番号2_3
                Me.txtTel1_3_1.Enabled = blnFlg                         ' 電話番号3_1
                Me.txtTel1_3_2.Enabled = blnFlg                         ' 電話番号3_2
                Me.txtTel1_3_3.Enabled = blnFlg                         ' 電話番号3_3
                Me.txtFax1_1.Enabled = blnFlg                           ' FAX1-1
                Me.txtFax1_2.Enabled = blnFlg                           ' FAX1-2
                Me.txtFax1_3.Enabled = blnFlg                           ' FAX1-3
                Me.txtMailPc1.Enabled = blnFlg                          ' Email1
                Me.txtMailMobile1.Enabled = blnFlg                      ' Email2
                Me.txtNote1.Enabled = blnFlg                            ' 備考1

                ' ComboBox
                Me.cboPrefectures1.Enabled = blnFlg                     ' 都道府県1
                Me.cboInternational1.Enabled = blnFlg                   ' 国内海外区分1

                ' CheckBox
                Me.chkMainAddress1.Enabled = blnFlg                     ' 現住所1

                '-----------------------------------
                '   海外
                '-----------------------------------
                ' Label
                Me.lblForeignAdress1_1.Enabled = blnFlg                 ' アドレス1_1
                Me.lblForeignAdress1_2.Enabled = blnFlg                 ' アドレス1_2
                Me.lblForeignAdress1_3.Enabled = blnFlg                 ' アドレス1_3
                Me.lblForeignAdress1_4.Enabled = blnFlg                 ' アドレス1_4
                Me.lblForeignAdress1_5.Enabled = blnFlg                 ' アドレス1_5

                ' TextBox
                Me.txtForeignAdress1_1.Enabled = blnFlg                 ' アドレス1_1
                Me.txtForeignAdress1_2.Enabled = blnFlg                 ' アドレス1_2
                Me.txtForeignAdress1_3.Enabled = blnFlg                 ' アドレス1_3
                Me.txtForeignAdress1_4.Enabled = blnFlg                 ' アドレス1_4
                Me.txtForeignAdress1_5.Enabled = blnFlg                 ' アドレス1_5
                Me.txtTel1_1.Enabled = blnFlg                           ' 電話番号1_1
                Me.txtTel1_2.Enabled = blnFlg                           ' 電話番号1_2
                Me.txtTel1_3.Enabled = blnFlg                           ' 電話番号1_3
                Me.txtFax1.Enabled = blnFlg                             ' FAX1

                ' BackColor
                Me.txtPostalNo1_1.BackColor = clrBack                   ' 郵便番号1_1
                Me.txtPostalNo1_2.BackColor = clrBack                   ' 郵便番号1_2
                Me.txtCities1.BackColor = clrBack                       ' 市区町村1
                Me.txtAddAther1.BackColor = clrBack                     ' 番地等1
                Me.txtBuilding1.BackColor = clrBack                     ' 建物名等1
                Me.txtTel1_1_1.BackColor = clrBack                      ' 電話番号1_1
                Me.txtTel1_1_2.BackColor = clrBack                      ' 電話番号1_2
                Me.txtTel1_1_3.BackColor = clrBack                      ' 電話番号1_3
                Me.txtTel1_2_1.BackColor = clrBack                      ' 電話番号2_1
                Me.txtTel1_2_2.BackColor = clrBack                      ' 電話番号2_2
                Me.txtTel1_2_3.BackColor = clrBack                      ' 電話番号2_3
                Me.txtTel1_3_1.BackColor = clrBack                      ' 電話番号3_1
                Me.txtTel1_3_2.BackColor = clrBack                      ' 電話番号3_2
                Me.txtTel1_3_3.BackColor = clrBack                      ' 電話番号3_3
                Me.txtFax1_1.BackColor = clrBack                        ' FAX1-1
                Me.txtFax1_2.BackColor = clrBack                        ' FAX1-2
                Me.txtFax1_3.BackColor = clrBack                        ' FAX1-3
                Me.txtMailPc1.BackColor = clrBack                       ' EmailPc1
                Me.txtMailMobile1.BackColor = clrBack                   ' EmailMobile1
                Me.txtNote1.BackColor = clrBack                         ' 備考1
                Me.cboPrefectures1.BackColor = clrBack                  ' 都道府県1
                Me.cboInternational1.BackColor = clrBack                ' 国内海外区分1
                Me.txtForeignAdress1_1.BackColor = clrBack              ' アドレス1_1
                Me.txtForeignAdress1_2.BackColor = clrBack              ' アドレス1_2
                Me.txtForeignAdress1_3.BackColor = clrBack              ' アドレス1_3
                Me.txtForeignAdress1_4.BackColor = clrBack              ' アドレス1_4
                Me.txtForeignAdress1_5.BackColor = clrBack              ' アドレス1_5
                Me.txtTel1_1.BackColor = clrBack                        ' 電話番号1_1
                Me.txtTel1_2.BackColor = clrBack                        ' 電話番号1_2
                Me.txtTel1_3.BackColor = clrBack                        ' 電話番号1_3
                Me.txtFax1.BackColor = clrBack                          ' FAX1

            ElseIf pBytTab = ADDRESS_TAB2 Then
                '=======================================================
                '   住所2
                '=======================================================
                Me.lblMandatoryPostalNo2.Enabled = blnFlg               ' 必須　郵便番号2 / アドレス2_1
                Me.lblMandatoryInternational2.Enabled = blnFlg          ' 必須　国内海外区分2
                Me.lblMandatoryPrefectures2.Enabled = blnFlg            ' 必須　都道府県2
                Me.lblMandatoryCities2.Enabled = blnFlg                 ' 必須　市区町村2
                Me.lblMandatoryAddAther2.Enabled = blnFlg               ' 必須　番地等2
                Me.lblMandatoryTel2.Enabled = blnFlg                    ' 必須　電話番号2

                '-----------------------------------
                '   国内
                '-----------------------------------
                ' Label
                Me.lblPostalNo2.Enabled = blnFlg                        ' 郵便番号2
                Me.lblPrefectures2.Enabled = blnFlg                     ' 都道府県2
                Me.lblCities2.Enabled = blnFlg                          ' 市区町村2
                Me.lblAddAther2.Enabled = blnFlg                        ' 番地等2
                Me.lblBuilding2.Enabled = blnFlg                        ' 建物名等2
                Me.lblPostalNoHyphen2.Enabled = blnFlg                  ' 郵便番号2ハイフン1
                Me.lblTel2_1.Enabled = blnFlg                           ' 電話番号2_1
                Me.lblTel2_2.Enabled = blnFlg                           ' 電話番号2_2
                Me.lblTel2_3.Enabled = blnFlg                           ' 電話番号2_3
                Me.lblFax2.Enabled = blnFlg                             ' FAX2
                Me.lblNote2.Enabled = blnFlg                            ' 備考2
                Me.lblMailPc2.Enabled = blnFlg                          ' Email2_1
                Me.lblMailMobile2.Enabled = blnFlg                      ' Email2_2
                Me.lblTelHyphen2_1_1.Enabled = blnFlg                   ' 電話番号2_1ハイフン1
                Me.lblTelHyphen2_1_2.Enabled = blnFlg                   ' 電話番号2_1ハイフン2
                Me.lblTelHyphen2_2_1.Enabled = blnFlg                   ' 電話番号2_2ハイフン1
                Me.lblTelHyphen2_2_2.Enabled = blnFlg                   ' 電話番号2_2ハイフン2
                Me.lblTelHyphen2_3_1.Enabled = blnFlg                   ' 電話番号2_3ハイフン1
                Me.lblTelHyphen2_3_2.Enabled = blnFlg                   ' 電話番号2_3ハイフン2
                Me.lblFaxHyphen2_1.Enabled = blnFlg                     ' FAX2ハイフン1
                Me.lblFaxHyphen2_2.Enabled = blnFlg                     ' FAX2ハイフン2

                ' Button
                Me.btnAddressSearch2.Enabled = blnFlg                   ' 住所検索2ボタン
                Me.btnClear2.Enabled = blnFlg                           ' クリア2ボタン

                ' TextBox
                Me.txtPostalNo2_1.Enabled = blnFlg                      ' 郵便番号2_1
                Me.txtPostalNo2_2.Enabled = blnFlg                      ' 郵便番号2_2
                Me.txtCities2.Enabled = blnFlg                          ' 市区町村2
                Me.txtAddAther2.Enabled = blnFlg                        ' 番地等2
                Me.txtBuilding2.Enabled = blnFlg                        ' 建物名等2
                Me.txtTel2_1_1.Enabled = blnFlg                         ' 電話番号2_1_1
                Me.txtTel2_1_2.Enabled = blnFlg                         ' 電話番号2_1_2
                Me.txtTel2_1_3.Enabled = blnFlg                         ' 電話番号2_1_3
                Me.txtTel2_2_1.Enabled = blnFlg                         ' 電話番号2-2_1
                Me.txtTel2_2_2.Enabled = blnFlg                         ' 電話番号2-2_2
                Me.txtTel2_2_3.Enabled = blnFlg                         ' 電話番号2-2_3
                Me.txtTel2_3_1.Enabled = blnFlg                         ' 電話番号2-3_1
                Me.txtTel2_3_2.Enabled = blnFlg                         ' 電話番号2-3_2
                Me.txtTel2_3_3.Enabled = blnFlg                         ' 電話番号2-3_3
                Me.txtFax2_1.Enabled = blnFlg                           ' FAX2-1
                Me.txtFax2_2.Enabled = blnFlg                           ' FAX2-2
                Me.txtFax2_3.Enabled = blnFlg                           ' FAX2-3
                Me.txtMailPc2.Enabled = blnFlg                          ' EmailPc2
                Me.txtMailMobile2.Enabled = blnFlg                      ' EmailMobile2
                Me.txtNote2.Enabled = blnFlg                            ' 備考2

                ' ComboBox
                Me.cboPrefectures2.Enabled = blnFlg                     ' 都道府県2
                Me.cboInternational2.Enabled = blnFlg                   ' 国内海外区分2

                ' CheckBox
                Me.chkMainAddress2.Enabled = blnFlg                     ' 現住所2

                '-----------------------------------
                '   海外
                '-----------------------------------
                ' Label
                Me.lblForeignAdress2_1.Enabled = blnFlg                 ' アドレス2_1
                Me.lblForeignAdress2_2.Enabled = blnFlg                 ' アドレス2_2
                Me.lblForeignAdress2_3.Enabled = blnFlg                 ' アドレス2_3
                Me.lblForeignAdress2_4.Enabled = blnFlg                 ' アドレス2_4
                Me.lblForeignAdress2_5.Enabled = blnFlg                 ' アドレス2_5

                ' TextBox
                Me.txtForeignAdress2_1.Enabled = blnFlg                 ' アドレス2_1
                Me.txtForeignAdress2_2.Enabled = blnFlg                 ' アドレス2_2
                Me.txtForeignAdress2_3.Enabled = blnFlg                 ' アドレス2_3
                Me.txtForeignAdress2_4.Enabled = blnFlg                 ' アドレス2_4
                Me.txtForeignAdress2_5.Enabled = blnFlg                 ' アドレス2_5
                Me.txtTel2_1.Enabled = blnFlg                           ' 電話番号2_1
                Me.txtTel2_2.Enabled = blnFlg                           ' 電話番号2_2
                Me.txtTel2_3.Enabled = blnFlg                           ' 電話番号2_3
                Me.txtFax2.Enabled = blnFlg                             ' FAX2

                ' BackColor
                Me.txtPostalNo2_1.BackColor = clrBack                   ' 郵便番号2_1
                Me.txtPostalNo2_2.BackColor = clrBack                   ' 郵便番号2_2
                Me.txtCities2.BackColor = clrBack                       ' 市区町村2
                Me.txtAddAther2.BackColor = clrBack                     ' 番地等2
                Me.txtBuilding2.BackColor = clrBack                     ' 建物名等2
                Me.txtTel2_1_1.BackColor = clrBack                      ' 電話番号2_1_1
                Me.txtTel2_1_2.BackColor = clrBack                      ' 電話番号2_1_2
                Me.txtTel2_1_3.BackColor = clrBack                      ' 電話番号2_1_3
                Me.txtTel2_2_1.BackColor = clrBack                      ' 電話番号2-2_1
                Me.txtTel2_2_2.BackColor = clrBack                      ' 電話番号2-2_2
                Me.txtTel2_2_3.BackColor = clrBack                      ' 電話番号2-2_3
                Me.txtTel2_3_1.BackColor = clrBack                      ' 電話番号2-3_1
                Me.txtTel2_3_2.BackColor = clrBack                      ' 電話番号2-3_2
                Me.txtTel2_3_3.BackColor = clrBack                      ' 電話番号2-3_3
                Me.txtFax2_1.BackColor = clrBack                        ' FAX2-1
                Me.txtFax2_2.BackColor = clrBack                        ' FAX2-2
                Me.txtFax2_3.BackColor = clrBack                        ' FAX2-3
                Me.txtMailPc2.BackColor = clrBack                       ' EmailPc2
                Me.txtMailMobile2.BackColor = clrBack                   ' EmailMobile2

                Me.txtNote2.BackColor = clrBack                         ' 備考2
                Me.cboPrefectures2.BackColor = clrBack                  ' 都道府県2
                Me.cboInternational2.BackColor = clrBack                ' 国内海外区分2
                Me.txtForeignAdress2_1.BackColor = clrBack              ' アドレス2_1
                Me.txtForeignAdress2_2.BackColor = clrBack              ' アドレス2_2
                Me.txtForeignAdress2_3.BackColor = clrBack              ' アドレス2_3
                Me.txtForeignAdress2_4.BackColor = clrBack              ' アドレス2_4
                Me.txtForeignAdress2_5.BackColor = clrBack              ' アドレス2_5
                Me.txtTel2_1.BackColor = clrBack                        ' 電話番号2_1
                Me.txtTel2_2.BackColor = clrBack                        ' 電話番号2_2
                Me.txtTel2_3.BackColor = clrBack                        ' 電話番号2_3
                Me.txtFax2.BackColor = clrBack                          ' FAX2

            ElseIf pBytTab = ADDRESS_TAB3 Then
                '=======================================================
                '   住所3
                '=======================================================
                Me.lblMandatoryPostalNo3.Enabled = blnFlg               ' 必須　郵便番号1 / アドレス1_1
                Me.lblMandatoryInternational3.Enabled = blnFlg          ' 必須　国内海外区分1
                Me.lblMandatoryPrefectures3.Enabled = blnFlg            ' 必須　都道府県1
                Me.lblMandatoryCities3.Enabled = blnFlg                 ' 必須　市区町村1
                Me.lblMandatoryAddAther3.Enabled = blnFlg               ' 必須　番地等1
                Me.lblMandatoryTel3.Enabled = blnFlg                    ' 必須　電話番号1

                '-----------------------------------
                '   国内
                '-----------------------------------
                ' Label
                Me.lblPostalNo3.Enabled = blnFlg                        ' 郵便番号3
                Me.lblPrefectures3.Enabled = blnFlg                     ' 都道府県3
                Me.lblCities3.Enabled = blnFlg                          ' 市区町村3
                Me.lblAddAther3.Enabled = blnFlg                        ' 番地等3
                Me.lblBuilding3.Enabled = blnFlg                        ' 建物名等3
                Me.lblTel3_1.Enabled = blnFlg                           ' 電話番号3_1
                Me.lblTel3_2.Enabled = blnFlg                           ' 電話番号3_2
                Me.lblTel3_3.Enabled = blnFlg                           ' 電話番号3_3
                Me.lblFax3.Enabled = blnFlg                             ' FAX3
                Me.lblNote3.Enabled = blnFlg                            ' 備考3
                Me.lblMailPc3.Enabled = blnFlg                          ' Email3_1
                Me.lblMailMobile3.Enabled = blnFlg                      ' Email3_2

                Me.lblPostalNoHyphen3.Enabled = blnFlg                  ' 郵便番号3ハイフン1
                Me.lblTelHyphen3_1_1.Enabled = blnFlg                   ' 電話番号3_1ハイフン1
                Me.lblTelHyphen3_1_2.Enabled = blnFlg                   ' 電話番号3_1ハイフン2
                Me.lblTelHyphen3_2_1.Enabled = blnFlg                   ' 電話番号3_2ハイフン1
                Me.lblTelHyphen3_2_2.Enabled = blnFlg                   ' 電話番号3_2ハイフン2
                Me.lblTelHyphen3_3_1.Enabled = blnFlg                   ' 電話番号3_3ハイフン1
                Me.lblTelHyphen3_3_2.Enabled = blnFlg                   ' 電話番号3_3ハイフン2
                Me.lblFaxHyphen3_1.Enabled = blnFlg                     ' FAX3ハイフン1
                Me.lblFaxHyphen3_2.Enabled = blnFlg                     ' FAX3ハイフン2

                ' Button
                Me.btnAddressSearch3.Enabled = blnFlg                   ' 住所検索3ボタン
                Me.btnClear3.Enabled = blnFlg                           ' クリア3ボタン

                ' TextBox
                Me.txtPostalNo3_1.Enabled = blnFlg                      ' 郵便番号3_1
                Me.txtPostalNo3_2.Enabled = blnFlg                      ' 郵便番号3_2
                Me.txtCities3.Enabled = blnFlg                          ' 市区町村3
                Me.txtAddAther3.Enabled = blnFlg                        ' 番地等3
                Me.txtBuilding3.Enabled = blnFlg                        ' 建物名等3
                Me.txtTel3_1_1.Enabled = blnFlg                         ' 電話番号3_1
                Me.txtTel3_1_2.Enabled = blnFlg                         ' 電話番号3_2
                Me.txtTel3_1_3.Enabled = blnFlg                         ' 電話番号3_3
                Me.txtTel3_2_1.Enabled = blnFlg                         ' 電話番号3_1
                Me.txtTel3_2_2.Enabled = blnFlg                         ' 電話番号3_2
                Me.txtTel3_2_3.Enabled = blnFlg                         ' 電話番号3_3
                Me.txtTel3_3_1.Enabled = blnFlg                         ' 電話番号3_1
                Me.txtTel3_3_2.Enabled = blnFlg                         ' 電話番号3_2
                Me.txtTel3_3_3.Enabled = blnFlg                         ' 電話番号3_3
                Me.txtFax3_1.Enabled = blnFlg                           ' FAX3-1
                Me.txtFax3_2.Enabled = blnFlg                           ' FAX3-2
                Me.txtFax3_3.Enabled = blnFlg                           ' FAX3-3
                Me.txtMailPc3.Enabled = blnFlg                          ' EmailPc3
                Me.txtMailMobile3.Enabled = blnFlg                      ' EmailMobile3
                Me.txtNote3.Enabled = blnFlg                            ' 備考3

                ' ComboBox
                Me.cboPrefectures3.Enabled = blnFlg                     ' 都道府県3
                Me.cboInternational3.Enabled = blnFlg                   ' 国内海外区分3

                ' CheckBox
                Me.chkMainAddress3.Enabled = blnFlg                     ' 現住所3

                '-----------------------------------
                '   海外
                '-----------------------------------
                ' Label
                Me.lblForeignAdress3_1.Enabled = blnFlg                 ' アドレス3_1
                Me.lblForeignAdress3_2.Enabled = blnFlg                 ' アドレス3_2
                Me.lblForeignAdress3_3.Enabled = blnFlg                 ' アドレス3_3
                Me.lblForeignAdress3_4.Enabled = blnFlg                 ' アドレス3_4
                Me.lblForeignAdress3_5.Enabled = blnFlg                 ' アドレス3_5

                ' TextBox
                Me.txtForeignAdress3_1.Enabled = blnFlg                 ' アドレス3_1
                Me.txtForeignAdress3_2.Enabled = blnFlg                 ' アドレス3_2
                Me.txtForeignAdress3_3.Enabled = blnFlg                 ' アドレス3_3
                Me.txtForeignAdress3_4.Enabled = blnFlg                 ' アドレス3_4
                Me.txtForeignAdress3_5.Enabled = blnFlg                 ' アドレス3_5
                Me.txtTel3_1.Enabled = blnFlg                           ' 電話番号3_1
                Me.txtTel3_2.Enabled = blnFlg                           ' 電話番号3_2
                Me.txtTel3_3.Enabled = blnFlg                           ' 電話番号3_3
                Me.txtFax3.Enabled = blnFlg                             ' FAX3

                ' BackColor
                Me.txtPostalNo3_1.BackColor = clrBack                   ' 郵便番号3_1
                Me.txtPostalNo3_2.BackColor = clrBack                   ' 郵便番号3_2
                Me.txtCities3.BackColor = clrBack                       ' 市区町村3
                Me.txtAddAther3.BackColor = clrBack                     ' 番地等3
                Me.txtBuilding3.BackColor = clrBack                     ' 建物名等3
                Me.txtTel3_1_1.BackColor = clrBack                      ' 電話番号3_1
                Me.txtTel3_1_2.BackColor = clrBack                      ' 電話番号3_2
                Me.txtTel3_1_3.BackColor = clrBack                      ' 電話番号3_3
                Me.txtTel3_2_1.BackColor = clrBack                      ' 電話番号3_1
                Me.txtTel3_2_2.BackColor = clrBack                      ' 電話番号3_2
                Me.txtTel3_2_3.BackColor = clrBack                      ' 電話番号3_3
                Me.txtTel3_3_1.BackColor = clrBack                      ' 電話番号3_1
                Me.txtTel3_3_2.BackColor = clrBack                      ' 電話番号3_2
                Me.txtTel3_3_3.BackColor = clrBack                      ' 電話番号3_3
                Me.txtFax3_1.BackColor = clrBack                        ' FAX3-1
                Me.txtFax3_2.BackColor = clrBack                        ' FAX3-2
                Me.txtFax3_3.BackColor = clrBack                        ' FAX3-3
                Me.txtMailPc3.BackColor = clrBack                       ' EmailPc3
                Me.txtMailMobile3.BackColor = clrBack                   ' EmailMobile3
                Me.txtNote3.BackColor = clrBack                         ' 備考3
                Me.cboPrefectures3.BackColor = clrBack                  ' 都道府県3
                Me.cboInternational3.BackColor = clrBack                ' 国内海外区分3
                Me.txtForeignAdress3_1.BackColor = clrBack              ' アドレス3_1
                Me.txtForeignAdress3_2.BackColor = clrBack              ' アドレス3_2
                Me.txtForeignAdress3_3.BackColor = clrBack              ' アドレス3_3
                Me.txtForeignAdress3_4.BackColor = clrBack              ' アドレス3_4
                Me.txtForeignAdress3_5.BackColor = clrBack              ' アドレス3_5
                Me.txtTel3_1.BackColor = clrBack                        ' 電話番号3_1
                Me.txtTel3_2.BackColor = clrBack                        ' 電話番号3_2
                Me.txtTel3_3.BackColor = clrBack                        ' 電話番号3_3
                Me.txtFax3.BackColor = clrBack                          ' FAX3

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

        ' 戻り値に処理結果を設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChangeInternational
    '   名称　：国内海外区分チェンジ処理
    '   概要　：国内・海外情報表示の切り替えを行う。
    '   引数　：ByVal bytTab            As Byte   = 住所タブ（1：住所1, 2：住所2, 3：住所3）
    '           ByVal pBytInternational As String = 国内海外区分（"0"：国内, "1"：海外）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>国内海外区分チェンジ処理</summary>
    ''' <param name="pBytTab">住所タブ</param>
    ''' <param name="pBytInternational">住所国内海外区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeInternational(ByVal pBytTab As Byte, _
                                         ByVal pBytInternational As String) As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim blnHome As Boolean = False                      ' 国内表示フラグ
        Dim blnAbroad As Boolean = False                    ' 海外表示フラグ

        Try
            ' 国内海外フラグ判定
            If pBytInternational = INTERNATIONAL_KBN_HOME Then
                ' 国内
                blnHome = True          ' 国内表示
                blnAbroad = False       ' 海外非表示
            ElseIf pBytInternational = INTERNATIONAL_KBN_ABROAD Then
                ' 海外
                blnHome = False         ' 国内非表示
                blnAbroad = True        ' 海外表示
            End If

            ' 住所タブ判定
            If pBytTab = ADDRESS_TAB1 Then

                '-------------------------------------------------------
                '   国内1情報
                '-------------------------------------------------------
                Me.lblPostalNo1.Visible = blnHome                       ' 郵便番号1ラベル
                Me.lblPrefectures1.Visible = blnHome                    ' 都道府県1ラベル
                Me.lblCities1.Visible = blnHome                         ' 市区町村1ラベル
                Me.lblBuilding1.Visible = blnHome                       ' 建物名等1ラベル
                Me.lblAddAther1.Visible = blnHome                       ' 番地等1ラベル
                Me.lblTel1_1.Visible = True                             ' 電話番号1_1ラベル
                Me.lblTel1_2.Visible = True                             ' 電話番号1_2ラベル
                Me.lblTel1_3.Visible = True                             ' 電話番号1_3ラベル
                Me.lblFax1.Visible = True                               ' FAX1ラベル

                Me.lblPostalNoHyphen1.Visible = blnHome                 ' 郵便番号1ハイフンラベル
                Me.lblTelHyphen1_1_1.Visible = blnHome                  ' 電話番号1_1_1ハイフンラベル
                Me.lblTelHyphen1_1_2.Visible = blnHome                  ' 電話番号1_1_2ハイフンラベル
                Me.lblTelHyphen1_2_1.Visible = blnHome                  ' 電話番号1_2_1ハイフンラベル
                Me.lblTelHyphen1_2_2.Visible = blnHome                  ' 電話番号1_2_2ハイフンラベル
                Me.lblTelHyphen1_3_1.Visible = blnHome                  ' 電話番号1_3_1ハイフンラベル
                Me.lblTelHyphen1_3_2.Visible = blnHome                  ' 電話番号1_3_2ハイフンラベル
                Me.lblFaxHyphen1_1.Visible = blnHome                    ' FAX1_1ハイフンラベルラベル
                Me.lblFaxHyphen1_2.Visible = blnHome                    ' FAX1_2ハイフンラベルラベル

                Me.cboPrefectures1.Visible = blnHome                    ' 都道府県1コンボボックス

                Me.txtPostalNo1_1.Visible = blnHome                     ' 郵便番号1_1テキストボックス
                Me.txtPostalNo1_2.Visible = blnHome                     ' 郵便番号1_2テキストボックス
                Me.txtCities1.Visible = blnHome                         ' 市区町村1テキストボックス
                Me.txtAddAther1.Visible = blnHome                       ' 番地等1テキストボックス
                Me.txtBuilding1.Visible = blnHome                       ' 建物名等1テキストボックス
                Me.txtTel1_1_1.Visible = blnHome                        ' 電話番号1_1_1テキストボックス
                Me.txtTel1_1_2.Visible = blnHome                        ' 電話番号1_1_2テキストボックス
                Me.txtTel1_1_3.Visible = blnHome                        ' 電話番号1_1_3テキストボックス
                Me.txtTel1_2_1.Visible = blnHome                        ' 電話番号1_2_1テキストボックス
                Me.txtTel1_2_2.Visible = blnHome                        ' 電話番号1_2_2テキストボックス
                Me.txtTel1_2_3.Visible = blnHome                        ' 電話番号1_2_3テキストボックス
                Me.txtTel1_3_1.Visible = blnHome                        ' 電話番号1_3_1テキストボックス
                Me.txtTel1_3_2.Visible = blnHome                        ' 電話番号1_3_2テキストボックス
                Me.txtTel1_3_3.Visible = blnHome                        ' 電話番号1_3_3テキストボックス
                Me.txtFax1_1.Visible = blnHome                          ' FAX1_1テキストボックス
                Me.txtFax1_2.Visible = blnHome                          ' FAX1_2テキストボックス
                Me.txtFax1_3.Visible = blnHome                          ' FAX1_3テキストボックス

                Me.btnAddressSearch1.Visible = blnHome                  ' 住所検索1ボタン

                '-------------------------------------------------------
                '   海外1情報
                '-------------------------------------------------------
                Me.lblForeignAdress1_1.Visible = blnAbroad              ' アドレス1_1ラベル
                Me.lblForeignAdress1_2.Visible = blnAbroad              ' アドレス1_2ラベル
                Me.lblForeignAdress1_3.Visible = blnAbroad              ' アドレス1_3ラベル
                Me.lblForeignAdress1_4.Visible = blnAbroad              ' アドレス1_4ラベル
                Me.lblForeignAdress1_5.Visible = blnAbroad              ' アドレス1_5ラベル

                Me.txtForeignAdress1_1.Visible = blnAbroad              ' アドレス1_1テキストボックス
                Me.txtForeignAdress1_2.Visible = blnAbroad              ' アドレス1_2テキストボックス
                Me.txtForeignAdress1_3.Visible = blnAbroad              ' アドレス1_3テキストボックス
                Me.txtForeignAdress1_4.Visible = blnAbroad              ' アドレス1_4テキストボックス
                Me.txtForeignAdress1_5.Visible = blnAbroad              ' アドレス1_5テキストボックス
                Me.txtTel1_1.Visible = blnAbroad                        ' 電話番号1_1テキストボックス
                Me.txtTel1_2.Visible = blnAbroad                        ' 電話番号1_2テキストボックス
                Me.txtTel1_3.Visible = blnAbroad                        ' 電話番号1_3テキストボックス
                Me.txtFax1.Visible = blnAbroad                          ' FAX1テキストボックス

            ElseIf pBytTab = ADDRESS_TAB2 Then

                '-------------------------------------------------------
                '   国内2情報
                '-------------------------------------------------------
                Me.lblPostalNo2.Visible = blnHome                       ' 郵便番号2ラベル
                Me.lblPrefectures2.Visible = blnHome                    ' 都道府県2ラベル
                Me.lblCities2.Visible = blnHome                         ' 市区町村2ラベル
                Me.lblBuilding2.Visible = blnHome                       ' 建物名等2ラベル
                Me.lblAddAther2.Visible = blnHome                       ' 番地等2ラベル
                Me.lblTel2_1.Visible = True                             ' 電話番号2_1ラベル
                Me.lblTel2_2.Visible = True                             ' 電話番号2_2ラベル
                Me.lblTel2_3.Visible = True                             ' 電話番号2_3ラベル
                Me.lblFax2.Visible = True                               ' FAX2ラベル

                Me.cboPrefectures2.Visible = blnHome                    ' 都道府県2コンボボックス

                Me.txtPostalNo2_1.Visible = blnHome                     ' 郵便番号2_1テキストボックス
                Me.txtPostalNo2_2.Visible = blnHome                     ' 郵便番号2_2テキストボックス
                Me.txtCities2.Visible = blnHome                         ' 市区町村2テキストボックス
                Me.txtAddAther2.Visible = blnHome                       ' 番地等2テキストボックス
                Me.txtBuilding2.Visible = blnHome                       ' 建物名等2テキストボックス
                Me.txtTel2_1_1.Visible = blnHome                        ' 電話番号2_1_1テキストボックス
                Me.txtTel2_1_2.Visible = blnHome                        ' 電話番号2_1_2テキストボックス
                Me.txtTel2_1_3.Visible = blnHome                        ' 電話番号2_1_3テキストボックス
                Me.txtTel2_2_1.Visible = blnHome                        ' 電話番号2_2_1テキストボックス
                Me.txtTel2_2_2.Visible = blnHome                        ' 電話番号2_2_2テキストボックス
                Me.txtTel2_2_3.Visible = blnHome                        ' 電話番号2_2_3テキストボックス
                Me.txtTel2_3_1.Visible = blnHome                        ' 電話番号2_3_1テキストボックス
                Me.txtTel2_3_2.Visible = blnHome                        ' 電話番号2_3_2テキストボックス
                Me.txtTel2_3_3.Visible = blnHome                        ' 電話番号2_3_3テキストボックス
                Me.txtFax2_1.Visible = blnHome                          ' FAX2_1テキストボックス
                Me.txtFax2_2.Visible = blnHome                          ' FAX2_2テキストボックス
                Me.txtFax2_3.Visible = blnHome                          ' FAX2_3テキストボックス

                Me.btnAddressSearch2.Visible = blnHome                  ' 住所検索2ボタン

                Me.lblPostalNoHyphen2.Visible = blnHome                 ' 郵便番号2ハイフンラベル
                Me.lblTelHyphen2_1_1.Visible = blnHome                  ' 電話番号2_1_1ハイフンラベル
                Me.lblTelHyphen2_1_2.Visible = blnHome                  ' 電話番号2_1_2ハイフンラベル
                Me.lblTelHyphen2_2_1.Visible = blnHome                  ' 電話番号2_2_1ハイフンラベル
                Me.lblTelHyphen2_2_2.Visible = blnHome                  ' 電話番号2_2_2ハイフンラベル
                Me.lblTelHyphen2_3_1.Visible = blnHome                  ' 電話番号2_3_1ハイフンラベル
                Me.lblTelHyphen2_3_2.Visible = blnHome                  ' 電話番号2_3_2ハイフンラベル
                Me.lblFaxHyphen2_1.Visible = blnHome                    ' FAX2_1ハイフンラベルラベル
                Me.lblFaxHyphen2_2.Visible = blnHome                    ' FAX2_2ハイフンラベルラベル

                '-------------------------------------------------------
                '   海外2情報
                '-------------------------------------------------------
                Me.lblForeignAdress2_1.Visible = blnAbroad              ' アドレス2_1ラベル
                Me.lblForeignAdress2_2.Visible = blnAbroad              ' アドレス2_2ラベル
                Me.lblForeignAdress2_3.Visible = blnAbroad              ' アドレス2_3ラベル
                Me.lblForeignAdress2_4.Visible = blnAbroad              ' アドレス2_4ラベル
                Me.lblForeignAdress2_5.Visible = blnAbroad              ' アドレス2_5ラベル

                Me.txtForeignAdress2_1.Visible = blnAbroad              ' アドレス2_1テキストボックス
                Me.txtForeignAdress2_2.Visible = blnAbroad              ' アドレス2_2テキストボックス
                Me.txtForeignAdress2_3.Visible = blnAbroad              ' アドレス2_3テキストボックス
                Me.txtForeignAdress2_4.Visible = blnAbroad              ' アドレス2_4テキストボックス
                Me.txtForeignAdress2_5.Visible = blnAbroad              ' アドレス2_5テキストボックス

                Me.txtTel2_1.Visible = blnAbroad                        ' 電話番号2_1テキストボックス
                Me.txtTel2_2.Visible = blnAbroad                        ' 電話番号2_2テキストボックス
                Me.txtTel2_3.Visible = blnAbroad                        ' 電話番号2_3テキストボックス
                Me.txtFax2.Visible = blnAbroad                          ' FAX2テキストボックス

            ElseIf pBytTab = ADDRESS_TAB3 Then

                '-------------------------------------------------------
                '   国内3情報
                '-------------------------------------------------------
                Me.lblPostalNo3.Visible = blnHome                       ' 郵便番号3ラベル
                Me.lblPrefectures3.Visible = blnHome                    ' 都道府県3ラベル
                Me.lblCities3.Visible = blnHome                         ' 市区町村3ラベル
                Me.lblBuilding3.Visible = blnHome                       ' 建物名等3ラベル
                Me.lblAddAther3.Visible = blnHome                       ' 番地等3ラベル
                Me.lblTel3_1.Visible = True                             ' 電話番号3_1ラベル
                Me.lblTel3_2.Visible = True                             ' 電話番号3_2ラベル
                Me.lblFax3.Visible = True                               ' FAX3ラベル

                Me.lblPostalNoHyphen3.Visible = blnHome                 ' 郵便番号3ハイフンラベル
                Me.lblTelHyphen3_1_1.Visible = blnHome                  ' 電話番号3_1_1ハイフンラベル
                Me.lblTelHyphen3_1_2.Visible = blnHome                  ' 電話番号3_1_2ハイフンラベル
                Me.lblTelHyphen3_2_1.Visible = blnHome                  ' 電話番号3_2_1ハイフンラベル
                Me.lblTelHyphen3_2_2.Visible = blnHome                  ' 電話番号3_2_2ハイフンラベル
                Me.lblTelHyphen3_3_1.Visible = blnHome                  ' 電話番号3_3_1ハイフンラベル
                Me.lblTelHyphen3_3_2.Visible = blnHome                  ' 電話番号3_3_2ハイフンラベル
                Me.lblFaxHyphen3_1.Visible = blnHome                    ' FAX3_1ハイフンラベルラベル
                Me.lblFaxHyphen3_2.Visible = blnHome                    ' FAX3_2ハイフンラベルラベル

                Me.cboPrefectures3.Visible = blnHome                    ' 都道府県3コンボボックス

                Me.txtPostalNo3_1.Visible = blnHome                     ' 郵便番号3_1テキストボックス
                Me.txtPostalNo3_2.Visible = blnHome                     ' 郵便番号3_3テキストボックス
                Me.txtCities3.Visible = blnHome                         ' 市区町村3テキストボックス
                Me.txtAddAther3.Visible = blnHome                       ' 番地等3テキストボックス
                Me.txtBuilding3.Visible = blnHome                       ' 建物名等3テキストボックス
                Me.txtTel3_1_1.Visible = blnHome                        ' 電話番号3_1_1テキストボックス
                Me.txtTel3_1_2.Visible = blnHome                        ' 電話番号3_1_3テキストボックス
                Me.txtTel3_1_3.Visible = blnHome                        ' 電話番号3_1_3テキストボックス
                Me.txtTel3_2_1.Visible = blnHome                        ' 電話番号3_2_1テキストボックス
                Me.txtTel3_2_2.Visible = blnHome                        ' 電話番号3_2_2テキストボックス
                Me.txtTel3_2_3.Visible = blnHome                        ' 電話番号3_2_3テキストボックス
                Me.txtTel3_3_1.Visible = blnHome                        ' 電話番号3_3_1テキストボックス
                Me.txtTel3_3_2.Visible = blnHome                        ' 電話番号3_3_2テキストボックス
                Me.txtTel3_3_3.Visible = blnHome                        ' 電話番号3_3_3テキストボックス
                Me.txtFax3_1.Visible = blnHome                          ' FAX3_1テキストボックス
                Me.txtFax3_2.Visible = blnHome                          ' FAX3_2テキストボックス
                Me.txtFax3_3.Visible = blnHome                          ' FAX3_3テキストボックス

                Me.btnAddressSearch3.Visible = blnHome                  ' 住所検索3ボタン

                '-------------------------------------------------------
                '   海外3情報
                '-------------------------------------------------------
                Me.lblForeignAdress3_1.Visible = blnAbroad              ' アドレス3_1ラベル
                Me.lblForeignAdress3_2.Visible = blnAbroad              ' アドレス3_2ラベル
                Me.lblForeignAdress3_3.Visible = blnAbroad              ' アドレス3_3ラベル
                Me.lblForeignAdress3_4.Visible = blnAbroad              ' アドレス3_4ラベル
                Me.lblForeignAdress3_5.Visible = blnAbroad              ' アドレス3_5ラベル

                Me.txtForeignAdress3_1.Visible = blnAbroad              ' アドレス3_1テキストボックス
                Me.txtForeignAdress3_2.Visible = blnAbroad              ' アドレス3_2テキストボックス
                Me.txtForeignAdress3_3.Visible = blnAbroad              ' アドレス3_3テキストボックス
                Me.txtForeignAdress3_4.Visible = blnAbroad              ' アドレス3_4テキストボックス
                Me.txtForeignAdress3_5.Visible = blnAbroad              ' アドレス3_5テキストボックス
                Me.txtTel3_1.Visible = blnAbroad                        ' 電話番号3_1テキストボックス
                Me.txtTel3_2.Visible = blnAbroad                        ' 電話番号3_2テキストボックス
                Me.txtTel3_3.Visible = blnAbroad                        ' 電話番号3_3テキストボックス
                Me.txtFax3.Visible = blnAbroad                          ' FAX3テキストボックス

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

        ' 戻り値に処理結果を設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim dtGrant As DataTable = Nothing              ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC010101)
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
#End Region

End Class

#End Region
