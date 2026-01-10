#Region "UC020501"
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo

Public Class UC020501

#Region "ログ出力オブジェクト"
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "定数"

    Private Const C_STAF_ID = "社員番号"
    Private Const L_NAME = "名前"
    Private Const L_NAME_KNA = "名前カナ"
    Private Const C_TELL_1 = "電話番号1"
    Private Const C_TELL_2 = "電話番号2"
    Private Const L_ADD_NUMBER = "郵便番号"
    Private Const C_PREFECTURES = "都道府県"
    Private Const C_CITIES = "市区町村"
    Private Const C_ADD_ATHER = "番地等"
    Private Const C_BUILDING = "建物名等"
    Private Const K_LOCAL = "会社支部"
    Private Const C_KSH = "所属会社"
    Private Const D_FROM = "住所適用日付"
    Private Const K_LOCAL_CODE = "会社支部コード"
    Private Const K_WORK_PLACE = "職場"
    Private Const L_FOREIGN_ADDRESS_1 = "海外アドレス1"
    Private Const L_FOREIGN_ADDRESS_2 = "海外アドレス2"
    Private Const L_FOREIGN_ADDRESS_3 = "海外アドレス3"
    Private Const L_FOREIGN_ADDRESS_4 = "海外アドレス4"
    Private Const L_FOREIGN_ADDRESS_5 = "海外アドレス5"
    Private Const K_FOREIGN = "住所区分"


    Private Const ALIAS_C_STAF_ID = "C_STAF_ID"
    Private Const ALIAS_L_NAME = "L_NAME"
    Private Const ALIAS_L_NAME_KNA = "L_NAME_KNA"
    Private Const ALIAS_C_TELL_1 = "C_TELL_1"
    Private Const ALIAS_C_TELL_2 = "C_TELL_2"
    Private Const ALIAS_L_ADD_NUMBER = "L_ADD_NUMBER"
    Private Const ALIAS_C_PREFECTURES = "C_PREFECTURES"
    Private Const ALIAS_C_CITIES = "C_CITIES"
    Private Const ALIAS_C_ADD_ATHER = "C_ADD_ATHER"
    Private Const ALIAS_C_BUILDING = "C_BUILDING"
    Private Const ALIAS_K_LOCAL = "K_LOCAL"
    Private Const ALIAS_C_KSH = "C_KSH"
    Private Const ALIAS_D_FROM = "D_FROM"
    Private Const ALIAS_K_LOCALCODE = "K_LOCAL_CODE"
    Private Const ALIAS_K_WORKPLACE = "K_WORKPLACE"
    Private Const ALIAS_FOREIGN_ADDRESS_1 = "L_FOREIGN_ADDRESS_1"
    Private Const ALIAS_FOREIGN_ADDRESS_2 = "L_FOREIGN_ADDRESS_2"
    Private Const ALIAS_FOREIGN_ADDRESS_3 = "L_FOREIGN_ADDRESS_3"
    Private Const ALIAS_FOREIGN_ADDRESS_4 = "L_FOREIGN_ADDRESS_4"
    Private Const ALIAS_FOREIGN_ADDRESS_5 = "L_FOREIGN_ADDRESS_5"
    Private Const ALIAS_K_FOREIGN = "K_FOREIGN"

    Private Const FIXED_FORMAT_VIEW = "FixedFormat_Common_View"

    'グループ化は不要と思われるが念のため（アクセスの作成済ビューで単一データ出力されるはずなので）
    Private Const SQL_SELECT_FROM = "SELECT " + _
                                          "社員番号 " + "AS " + ALIAS_C_STAF_ID + ", " + _
                                          "名前 AS " + ALIAS_L_NAME + ", " + _
                                          "名前カナ AS " + ALIAS_L_NAME_KNA + ", " + _
                                          "電話番号1 AS " + ALIAS_C_TELL_1 + ", " + _
                                          "電話番号2 AS " + ALIAS_C_TELL_2 + ", " + _
                                          "郵便番号 AS " + ALIAS_L_ADD_NUMBER + ", " + _
                                          "都道府県 AS " + ALIAS_C_PREFECTURES + ", " + _
                                          "市区町村 AS " + ALIAS_C_CITIES + ", " + _
                                          "番地等 AS " + ALIAS_C_ADD_ATHER + ", " + _
                                          "建物名等 AS " + ALIAS_C_BUILDING + ", " + _
                                          "会社支部 AS " + ALIAS_K_LOCAL + ", " + _
                                          "所属会社 AS " + ALIAS_C_KSH + ", " + _
                                          "会社支部コード AS " + ALIAS_K_LOCALCODE + ", " + _
                                          "職場 AS " + ALIAS_K_WORKPLACE + ", " + _
                                          "海外アドレス1 AS " + ALIAS_FOREIGN_ADDRESS_1 + ", " + _
                                          "海外アドレス2 AS " + ALIAS_FOREIGN_ADDRESS_2 + ", " + _
                                          "海外アドレス3 AS " + ALIAS_FOREIGN_ADDRESS_3 + ", " + _
                                          "海外アドレス4 AS " + ALIAS_FOREIGN_ADDRESS_4 + ", " + _
                                          "海外アドレス5 AS " + ALIAS_FOREIGN_ADDRESS_5 + ", " + _
                                          "住所区分 AS " + ALIAS_K_FOREIGN + " " + _
                                          "FROM " + FIXED_FORMAT_VIEW + " "


    '************************************************************************
    '* 抽出条件―組合員種別が正組合員かシニア組合員でステータスが加入のもの *
    '************************************************************************

    'Private Const SQL_GROUP = "GROUP BY " + C_STAF_ID + " "   'グループ句


    Private Const SQL_ORDER_JPN_SYLLABARY_TYPE = "ORDER BY " + L_NAME_KNA + ""    '50音順のAタイプ、Bタイプのオーダー句
    Private Const SQL_ORDER_STAFF_ID_TYPE = "ORDER BY CLng(" + C_STAF_ID + ")"    '社員番号順のオーダー句

    Private Const PRINT_ORDERTYPE_JPN_SYLLABARY As String = "PRINT_TYPE_JPN_SYLLABARY"
    Private Const PRINT_ORDERTYPE_STAFF_ID As String = "PRINT_TYPE_STAFF_ID"

    Private Const MESSAGE_ERR_NOT_CHECK = "GE0100"


#End Region


#Region "イベント"

#Region "UC020501_Load"
    '***************************************************************************************************
    '   ＩＤ　：UC020501_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/22 somezaki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22 somezaki  新規作成
    '***************************************************************************************************
    Private Sub UC020501_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim dt As DataTable = Nothing
        Dim strPrint As String = ""
        Try
            dt = MDCommon.getGrant(MENU_ID_UC020501)
            strPrint = dt.Rows(0).Item(5).ToString

            If strPrint = GRANT_VOID Then
                Me.btnPrintJpnSyllabary.Enabled = False
                Me.btnPrintStaffId.Enabled = False
            End If

            ' データベース接続
            clsDb.Connect()
            ' 定数マスタ詳細（組合支部）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, Me.cboUnionBranch, MDConst.CONSTANT_ID_BELONGING) = False Then
                Exit Sub
            End If

            Me.cboUnionBranch.Focus()


        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
#End Region


#Region "印刷（50音順定型名簿出力）ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnPrintJpnSyllabary_Click
    '   名称　：印刷（50音順定型名簿出力）処理
    '   概要　：
    '   作成日：2011/11/22 somezaki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnPrintJpnSyllabary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintJpnSyllabary.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call PrintJpnSyllabary()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
#End Region


#Region "印刷（社員番号順定型名簿出力）ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnPrintStaffId_Click
    '   名称　：印刷（社員番号順定型名簿出力）処理
    '   概要　：
    '   作成日：2011/11/22 somezaki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnPrintStaffId_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintStaffId.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        
        Try
            Call PrintStafID()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
#End Region

#Region "キープレス（各ラジオボタン、エンターで検索）"
    '***************************************************************************************************
    '   ＩＤ　：optJpnSyllabary_A_KeyPress
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************

    Private Sub optJpnSyllabary_A_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optJpnSyllabary_A.KeyPress
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintJpnSyllabary()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
    '***************************************************************************************************
    '   ＩＤ　：optJpnSyllabary_B_KeyPress
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************
    Private Sub optJpnSyllabary_B_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optJpnSyllabary_B.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintJpnSyllabary()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：optJpnSyllabary_C_KeyDown
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************
    Private Sub optJpnSyllabary_C_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optJpnSyllabary_C.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintJpnSyllabary()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
    'Private Sub optJpnSyllabary_C_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles optJpnSyllabary_C.KeyDown
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
    '    Try
    '        If e.KeyCode = Keys.Enter Then
    '            Call PrintJpnSyllabary()
    '        End If
    '    Catch ex As Exception
    '        Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
    '        log.Fatal(ex.Message)
    '    End Try
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    'End Sub

    '***************************************************************************************************
    '   ＩＤ　：optStaffId_A_KeyPress
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************
    Private Sub optStaffId_A_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optStaffId_A.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintStafID()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：optStaffId_B_KeyPress
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************
    Private Sub optStaffId_B_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optStaffId_B.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintStafID()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：optStaffId_C_KeyPress
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/12/15 
    '   更新日：
    '***************************************************************************************************
    Private Sub optStaffId_C_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles optStaffId_C.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call PrintStafID()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

#End Region


#End Region


#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ExistCheckTrue
    '   名称　：ラジオボタンチェック済判定
    '   概要  ：引数のラジオボタンのいずれかがチェックされているか確認する
    '   引数　：RadioButton()
    '   戻り値：True = チェックあり, False = チェックなし
    '   作成日：2011/11/27 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/27 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistCheckTrue(ByVal checkRadiobtns As RadioButton()) As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim blnResult As Boolean = False
        Try

            For Each chkRadioBtn As RadioButton In checkRadiobtns
                If chkRadioBtn.Checked Then
                    blnResult = True
                End If
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return blnResult

    End Function


    '***************************************************************************************************
    '   ＩＤ　：GetDatasetForReport
    '   名称　：データテーブルの情報をデータセットに格納する
    '   概要  ：データテーブルの情報をデータセットに格納する
    '   引数　：DataTable
    '   作成日：2011/11/26 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <remarks></remarks>
    Private Function GetDatasetForReport(ByVal dtResource As DataTable) As DS0205P1Main

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim dtsReturn As DS0205P1Main = New DS0205P1Main()
        Try

            Dim drDetail As DS0205P1Main.dtDetailRow
            For Each row As DataRow In dtResource.Rows
                drDetail = dtsReturn.dtDetail.NewRow
                drDetail.BeginEdit()
                drDetail.c_staf_id = NVL(row(ALIAS_C_STAF_ID))
                drDetail.l_name = NVL(row(ALIAS_L_NAME))
                drDetail.l_name_kana = NVL(row(ALIAS_L_NAME_KNA))
                drDetail.c_tel_1 = NVL(row(ALIAS_C_TELL_1))
                drDetail.c_tel_2 = NVL(row(ALIAS_C_TELL_2))
                If NVL(row(ALIAS_K_FOREIGN)) = "1" Then
                    drDetail.c_prefectures = ""
                    drDetail.c_cities = NVL(row(ALIAS_FOREIGN_ADDRESS_1)) + " " + NVL(row(ALIAS_FOREIGN_ADDRESS_2)) + " " + NVL(row(ALIAS_FOREIGN_ADDRESS_3)) + " " + NVL(row(ALIAS_FOREIGN_ADDRESS_4)) + " " + NVL(row(ALIAS_FOREIGN_ADDRESS_5))
                    drDetail.c_add_ather = ""
                    drDetail.c_building = ""
                Else
                    drDetail.c_prefectures = NVL(row(ALIAS_C_PREFECTURES))
                    drDetail.c_cities = NVL(row(ALIAS_C_CITIES))
                    drDetail.c_add_ather = NVL(row(ALIAS_C_ADD_ATHER))
                    drDetail.c_building = NVL(row(ALIAS_C_BUILDING))
                End If
                drDetail.k_local = NVL(row(ALIAS_K_LOCALCODE))
                'drDetail.c_trance_ksh = NVL(row(ALIAS_C_KSH))
                'drDetail.c_trance_kshは*の表示に利用する。ただし会社コードではなくとりあえず職場コードを入れる。
                'カラム名はc_trance_ksh（会社コード）だが、現行の動作を確認するに会社コードではなく職場を見ているようで、
                '現行の仕様をお客様に確認しても不明。データを見るに職場がその他のデータに*が表示されているよう。
                'お客様も*について調査して頂けるが、現状は職場で判定で良いということで会社コードの値に職場の値に代替え。
                '（データセットに職場の新規追加はrptファイルの変更などもあり規模が大きいため、会社コードの項目を使用する）
                drDetail.c_trance_ksh = NVL(row(ALIAS_K_WORKPLACE)) '会社コードだが上記コメント理由により職場を設定
                drDetail.l_add_number = NVL(row(ALIAS_L_ADD_NUMBER))
                drDetail.EndEdit()
                dtsReturn.dtDetail.Rows.Add(drDetail)
            Next

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return dtsReturn

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetReportType
    '   名称　：
    '   概要  ：レポート形式を取得する
    '   引数　：
    '   戻り値：CrystalDecisions.CrystalReports.Engine.ReportDocument（レポート形式）
    '   作成日：2011/11/26 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <remarks></remarks>
    Private Function GetReportType(ByVal strReportType As String) As CrystalDecisions.CrystalReports.Engine.ReportDocument

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim rptReturn As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing
        Try
            '印刷形式指定
            If strReportType = PRINT_ORDERTYPE_JPN_SYLLABARY Then
                If Me.optJpnSyllabary_A.Checked Then        '50音順Aタイプ
                    rptReturn = New CR0205P4
                ElseIf Me.optJpnSyllabary_B.Checked Then    '50音順Bタイプ
                    rptReturn = New CR0205P5
                Else                                        '50音順Cタイプ
                    rptReturn = New CR0205P6
                End If
            Else
                If Me.optStaffId_A.Checked Then         '社員番号順Aタイプ
                    rptReturn = New CR0205P1
                ElseIf Me.optStaffId_B.Checked Then     '社員番号順Bタイプ
                    rptReturn = New CR0205P2
                Else                                    '社員番号順Cタイプ
                    rptReturn = New CR0205P3
                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return rptReturn

    End Function

    '***************************************************************************************************
    '   ＩＤ　：PrintJpnSyllabary
    '   名称　：
    '   概要  ：印刷実行（50音順）
    '   引数　：
    '   作成日：2011/11/26 somezaki
    '   更新日：
    '***************************************************************************************************
    Private Sub PrintJpnSyllabary()
        Dim clsDb As New CLAccessMdb
        Dim fmPrint As New FM000203     'レポート画面
        Try
            Cursor.Current = Cursors.WaitCursor
            '対象のボタンが選択されているかチェックする
            Dim checkRadioBtn As RadioButton() = New RadioButton() {Me.optJpnSyllabary_A, Me.optJpnSyllabary_B, Me.optJpnSyllabary_C}

            If ExistCheckTrue(checkRadioBtn) Then

                Dim strSqlSentence = ""         'Sql文格納用
                Dim strWhere As String = ""    'Where句
                Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument  'レポート

                'Sql条件取得()
                If ChkNull(Me.cboUnionBranch.SelectedValue.ToString) = False Then
                    strWhere = "Where 組合支部 = '" + Me.cboUnionBranch.Text + "' "
                End If

                reportObj = GetReportType(PRINT_ORDERTYPE_JPN_SYLLABARY)

                strSqlSentence = SQL_SELECT_FROM + strWhere + SQL_ORDER_JPN_SYLLABARY_TYPE

                'DB接続+SQL結果取得
                Call clsDb.Connect()
                Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strSqlSentence)

                If dtSqlResult.Rows.Count = 0 Then
                    'エラーメッセージ
                    CLMsg.Show("GI0042")
                    Exit Sub
                End If

                'データセット作成
                Dim dsReportData As DS0205P1Main = GetDatasetForReport(dtSqlResult)

                fmPrint.ButtonShowType = 3              '「印刷」「キャンセル」ボタン表示
                fmPrint.ObjResource = reportObj         'レポートファイル
                reportObj.SetDataSource(dsReportData)   'データセット
                fmPrint.ShowDialog()                    '印刷画面表示

                If fmPrint.IntQlickBtnFlag = 3 Then
                    fmPrint.PrintOut()
                End If

            Else
                CLMsg.Show(MESSAGE_ERR_NOT_CHECK)  '未選択メッセージ
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Throw ex
        Finally
            Cursor.Current = Cursors.Default
            Call clsDb.Disconnect()
            fmPrint.Close()
            fmPrint.Dispose()
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：PrintStafID
    '   名称　：
    '   概要  ：印刷実行（社員番号順）
    '   引数　：
    '   作成日：2011/11/26 somezaki
    '   更新日：
    '***************************************************************************************************
    Private Sub PrintStafID()
        Dim clsDb As New CLAccessMdb
        Dim fmPrint As New FM000203     'レポート画面
        Try
            Cursor.Current = Cursors.WaitCursor
            '対象のボタンが選択されているかチェックする
            Dim checkRadioBtn As RadioButton() = New RadioButton() {Me.optStaffId_A, Me.optStaffId_B, Me.optStaffId_C}
            Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument

            If ExistCheckTrue(checkRadioBtn) Then

                Dim strSqlSentence = ""
                Dim strWhere As String = ""

                If ChkNull(Me.cboUnionBranch.SelectedValue.ToString) = False Then
                    strWhere = "Where 組合支部 = '" + Me.cboUnionBranch.Text + "' "
                End If

                'レポート形式取得
                reportObj = GetReportType(PRINT_ORDERTYPE_STAFF_ID)
                'SQL
                strSqlSentence = SQL_SELECT_FROM + strWhere + SQL_ORDER_STAFF_ID_TYPE

                'DB接続+SQL結果取得
                Call clsDb.Connect()
                Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strSqlSentence)

                If dtSqlResult.Rows.Count = 0 Then
                    'エラーメッセージ
                    CLMsg.Show("GI0042")
                    Exit Sub
                End If

                'データセット作成
                Dim dsReportData As DS0205P1Main = GetDatasetForReport(dtSqlResult)

                fmPrint.ButtonShowType = 3              '「印刷」「キャンセル」ボタン表示
                fmPrint.ObjResource = reportObj         'レポートファイル
                reportObj.SetDataSource(dsReportData)   'データセット

                fmPrint.ShowDialog()                    '印刷画面表示

                If fmPrint.IntQlickBtnFlag = 3 Then
                    fmPrint.PrintOut()
                End If

            Else
                CLMsg.Show(MESSAGE_ERR_NOT_CHECK)  '未選択メッセージ
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020501, SCREEN_NAME_UC020501, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Throw ex
        Finally
            Cursor.Current = Cursors.Default
            Call clsDb.Disconnect()
            fmPrint.Close()
            fmPrint.Dispose()
        End Try
    End Sub

#End Region
    

End Class

#End Region






