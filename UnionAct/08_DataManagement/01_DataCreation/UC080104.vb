#Region "UC080104"
'===========================================================================================================
'   クラスＩＤ　　：UC080104
'   クラス名称　　：労金データ作成 － 新規登録画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDFile
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common
Imports System.Text
Imports System.Text.RegularExpressions

Public Class UC080104

#Region " 列挙 "
    Private Enum CsvCol
        '-------------------------------------------------------------------------------------------------------------------
        '                          1：ヘッダーレコード     | 2：データレコード       | 8：集計レコード | 9：終了レコード
        '-------------------------------------------------------------------------------------------------------------------
        Col_A = 0       ' 01. A列：レコード種別：1         | レコード種別：2         | レコード種別：8 | レコード種別：9
        Col_B = 1       ' 02. B列：21                      | 銀行番号                | 振込件数        | 無し
        Col_C = 2       ' 03. C列：0                       | 銀行名                  | 振込総額        | 無し
        Col_D = 3       ' 04. D列：労金契約番号            | 支店番号                | 無し            | 無し
        Col_E = 4       ' 05. E列：口座名義カナ            | 支店名                  | 無し            | 無し
        Col_F = 5       ' 06. F列：振込日                  | 無し                    | 無し            | 無し
        Col_G = 6       ' 07. G列：金融機関コード          | 預金種目                | 無し            | 無し
        Col_H = 7       ' 08. H列：銀行名カナ              | 口座番号                | 無し            | 無し
        Col_I = 8       ' 09. I列：支店番号                | 受取人名                | 無し            | 無し
        Col_J = 9       ' 10. J列：支店名カナ              | 振込金額(※1)           | 無し            | 無し
        Col_K = 10      ' 11. K列：預金種目                | 無し                    | 無し            | 無し
        Col_L = 11      ' 12. L列：口座番号                | 社員番号                | 無し            | 無し
        Col_M = 12      ' 13. M列：無し                    | 無し                    | 無し            | 無し
        Col_N = 13      ' 14. N列：無し                    | 無し                    | 無し            | 無し
        Col_O = 14      ' 15. O列：無し                    | 無し                    | 無し            | 無し
        Col_P = 15      ' 16. P列：無し                    | 無し                    | 無し            | 無し
        Col_Q = 16      ' 17. Q列：無し                    | 手数料区分              | 無し            | 無し
        Col_R = 17      ' 18. R列：所属委員会（役職）(※2) | 所属委員会（役職）(※3) | 無し            | 無し

        ' ※1 ① 振込先金融機関コードが、中央労金(2963)、近畿労金(2978)の場合
        '        ・振込手数料は、差し引かない金額を振込金額として出力する。
        '     ② 振込先金融機関コードが、中央労金(2963)、近畿労金(2978)以外の場合
        '        ②-① 且つ振込金額が1万未満の場合
        '              ・振込金額から220円を差し引いて出力する。
        '        ②-② 且つ振込金額が1万以上5万未満の場合
        '              ・振込金額から330円を差し引いて出力する。
        '        ②-③ 且つ振込金額が5万以上の場合
        '              ・振込金額から550円を差し引いて出力する。
        ' ※2 "所属委員会（役職）"の文字列
        ' ※3 所属委員会（役職）データ

    End Enum
#End Region

#Region " 定数・メンバ変数 "
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' 名称
    Private Const DAILY_PAY_KIND_COMMITTEE As String = "部／委員会日当"
    Private Const DAILY_PAY_KIND_BRANCH As String = "支部委員会（三役）日当"
    Private Const DAILY_PAY_KIND_EXECUTIVE As String = "中央執行日当"
    Private Const DAILY_PAY_KIND_DGM As String = "ＤＧＭ日当"
    Private Const DAILY_PAY_KIND_CUT As String = "賃金カット／役員手当"
    Private Const DAILY_PAY_KIND_ONCE_CUT As String = "一時金カット"

    ' 日当計算区分
    Private Const DAILY_PAY_KIND_COMMITTEE_CODE As String = "01"
    Private Const DAILY_PAY_KIND_BRANCH_CODE As String = "02"
    Private Const DAILY_PAY_KIND_EXECUTIVE_CODE As String = "03"
    Private Const DAILY_PAY_KIND_DGM_CODE As String = "04"
    Private Const DAILY_PAY_KIND_CUT_CODE As String = "05"
    Private Const DAILY_PAY_KIND_ONCE_CUT_CODE As String = "06"

    Private Const MAX_DATE As String = "99999999"

    ' 金融機関コード
    Private Const FINANCIAL_CODE_CENTRAL_LABOR_BANK As String = "2963"  ' 中央労働金庫
    Private Const FINANCIAL_CODE_KINKI_LABOR_BANK As String = "2978"    ' 近畿労働金庫

    ' 権限関連
    Private _strGrantReference As String = String.Empty                 ' 参照権限
    Private _strGrantInsert As String = String.Empty                    ' 登録権限
    Private _strGrantPrint As String = String.Empty                     ' 印刷権限
    Private _strGrantFileOutput As String = String.Empty                ' ファイル出力権限

    ' 手数料区分
    Private _strFeeDivision As String = String.Empty
    Private _strFeeDivisionCsv As String = String.Empty
    Dim _dtMakeUser As DataTable = Nothing

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC080104              ' UC080104
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC080104          ' 労金データ作成 － 新規登録画面

    ' データ作成対象○○件
    Private Const DATA_COUNT As String = "データ作成対象({0}件)"

    ' CSVファイルヘッダー
    Private _strCsvHeaderArray As String() = {"振込銀行番号", "振込銀行名", "振込支店番号", "振込支店名", "手形交換所番号", "預金種目", _
                                              "口座番号", "受取人名", "振込金額", "新規コード", "顧客コード１", "顧客コード２", "振込区分", _
                                              "ＥＤＩ識別表示", "ＥＤＩ情報", "手数料区分", "所属委員会（役職）"}

    ' ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    ''' <summary>労金データ検索画面で選択された締め日</summary>
    Private _dtSelectCloseDay As DataTable = Nothing

    ''' <summary>締め日未選択の労金データ作成フラグ</summary>
    Private _blnIsFreeEntry As Boolean = False

    ''' <summary>編集可能フラグ</summary>
    Private _blnIsEdit As Boolean = True

    ''' <summary>クリックボタン判別用</summary>
    Private _intSelectBtn As Integer = -1
    '0 = 新規作成 
    '1 = 振込日検索-詳細
    '2 = 個人別振込検索-詳細

    ''' <summary>画面終了前に押下したボタン判別用</summary>
    Private _intReturnBtn As Integer = -1
    ' 0 = 登録確認ボタン
    ' 1 = キャンセルボタン
    ' 2 = 戻るボタン

    ''' <summary>選択振込データID</summary>
    ''' <remarks></remarks>
    Private _strStafBankSendId As String = String.Empty

    Private _strPayStatus As String = String.Empty
    Private _strPayStatusCd As String = String.Empty
    Private _strTitle As String = String.Empty
    Private _datePayDay As DateTime = Nothing

    ' 労金データ
    Private _dtStafBankSend As DataTable = Nothing

    ' 労金メンバーデータ
    Private _dtStafBankSendMember As DataTable = Nothing

    ' 締め日データ
    Private _dtStafBankClose As DataTable = Nothing

    ' 締め日メンバーデータ
    Private _dtStafBankCloseMember As DataTable = Nothing

    ' 対象締め日・締め日種別ごとのユーザーリスト
    Dim _AllUserList As List(Of List(Of String)) = New List(Of List(Of String))

    ' 各ユーザーの役員手当支給委員会（源泉の支払いがある方のみ）
    Dim _dicUserCommittee As Dictionary(Of String, String) = New Dictionary(Of String, String)

    ' 現在日付
    Private _NowDate As DateTime = Now.Date

    ' STAF_BANK_SENDの構造体
    Private Structure StafSendData
        Public strStafBankSendId As String
        Public dateBankSend As String
        Public strBankSendMargin As String
        Public strBankSendItem As String
        Public strPayTimeKind As String
        Public strPayTimeCut As String
        Public strDailyPaykind As String
        Public strDailyPayClose As String
        Public strDateIns As String
        Public strUserIdIns As String
        Public strDateUpdate As String
        Public strUserIdUpdate As String
        Public intUp As Integer
    End Structure

    ' STAF_BANK_SEND_MEMBERの構造体
    Private Structure StafSendDataMember
        Public strStafBankSendId As String
        Public strDateBankSend As String
        Public strUserId As String
        Public intBankPay As Long
        Public intAdjust As Integer
        Public intPayCutTotal As Integer
        Public intDailyPayTotal As Integer
        Public strDateFromAccount As String
        Public strDateIns As String
        Public strUserIdIns As String
    End Structure

    ' STAF_BANK_CLOSEの構造体
    Private Structure StafBankClose
        Public strDatePayClose As String
        Public strDailyPayKind As String
        Public strBankSendStatus As String
        Public strDateIns As String
        Public strUserIdIns As String
        Public strDateUpdate As String
        Public strUserIdUpdate As String
        Public intUpCount As Integer
    End Structure

    ' STAF_BANK_CLOSE_MEMBERの構造体
    Private Structure StafBankCloseMember
        Public strDatePayClose As String
        Public strDailyPayKind As String
        Public strUserId As String
        Public strStafBankSendId As String
        Public strDateBankSend As String
        Public intPay As Integer
        Public strDateIns As String
        Public strUserIdIns As String
        Public strDateUpdate As String
        Public strUserIdUpdate As String
        Public intUpCount As Integer
    End Structure
#End Region

#Region " プロパティ "
    Public Property SelectCloseDay() As DataTable
        Get
            Return _dtSelectCloseDay
        End Get
        Set(ByVal value As DataTable)
            _dtSelectCloseDay = value
        End Set
    End Property

    ' 支払方法の取得・返却
    Public Property strPayStatus() As String
        Get
            Return _strPayStatus
        End Get
        Set(ByVal value As String)
            _strPayStatus = value
        End Set
    End Property

    ' 支払方法コードの取得・返却
    Public Property strPayStatusCd() As String
        Get
            Return _strPayStatusCd
        End Get
        Set(ByVal value As String)
            _strPayStatusCd = value
        End Set
    End Property

    ' 題目の取得・返却
    Public Property strTitle() As String
        Get
            Return _strTitle
        End Get
        Set(ByVal value As String)
            _strTitle = value
        End Set
    End Property

    ' 振込日の取得・返却
    Public Property datePayDay() As DateTime
        Get
            Return _datePayDay
        End Get
        Set(ByVal value As DateTime)
            _datePayDay = value
        End Set
    End Property

    ' 前画面でクリックされたボタン
    Public Property intClickBtnFlg() As Integer
        Get
            Return _intSelectBtn
        End Get
        Set(ByVal value As Integer)
            _intSelectBtn = value
        End Set
    End Property

    '画面終了時に押下したボタン
    Public ReadOnly Property intReturnBtnFlg() As Integer
        Get
            Return _intReturnBtn
        End Get
    End Property

    ''' <summary>締日未選択の労金データ作成フラグの取得・返却</summary>
    Public Property IsFreeEntry() As Boolean
        Get
            Return _blnIsFreeEntry
        End Get
        Set(ByVal value As Boolean)
            _blnIsFreeEntry = value
        End Set
    End Property

    ' 選択した振込データの振込ID
    Public Property strStafBankSendId() As String
        Get
            Return _strStafBankSendId
        End Get
        Set(ByVal value As String)
            _strStafBankSendId = value
        End Set
    End Property
#End Region

#Region " イベント "
#Region " フォームロード "
    '***************************************************************************************************
    '   ＩＤ　：UC080104_Load
    '   名称　：フォームロード
    '   概要　：
    '   作成日：2012/02/06(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC080104_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            Dim dtGrant As DataTable = Nothing
            Me.Cursor = Cursors.WaitCursor

            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC080101)
            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString  ' 参照権限
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString     ' 登録権限
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString      ' 印刷権限
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString ' ファイル出力権限
            End If

            If _strGrantInsert <> GRANT_VALID Then
                ' 登録権限がない場合、内容変更ボタン使用不可
                ' (念のため登録確認ボタンも使用不可)
                Me.btnEntryConfirm.Enabled = False
                Me.btnChange.Enabled = False
            End If

            If _strGrantPrint <> GRANT_VALID Then
                ' 印刷権限がない場合、印刷ボタン使用不可
                Me.btnPrint.Enabled = False
            End If

            If _strGrantFileOutput <> GRANT_VALID Then
                ' ファイル出力権限がない場合、ファイル出力ボタン使用不可
                Me.btnOutputCsv.Enabled = False
            End If

            ' フレックスグリッドのセルスタイルを定義
            Call Me.DefinitionStyles()

            ' 手数料区分をメンバ変数へ格納する
            Call Me.SetFeeDivision()

            ' 支払方法
            Me.txtBankSendMargin.Text = _strPayStatus

            ' 題目
            Me.txtTitle.Text = _strTitle

            ' 振込日
            Me.txtPayDay.Text = _datePayDay

            If _intSelectBtn = 0 Then
                ' 新規作成ボタンから開かれた場合
                Call Me.InitializeNew()
            Else
                ' 振込日検索データ照会時
                Call Me.InitializeDetail()
                _blnIsEdit = False
            End If

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

        End Try
    End Sub
#End Region

#Region " 登録確認ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnEntryConfirm_Click
    '   名称　：登録確認ボタンクリック
    '   概要　：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnEntryConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEntryConfirm.Click

        Dim iSelect As Integer = -1
        Dim fmPreview As FM000205 = New FM000205

        Try
            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' チェック行が１行以上あるか
            If Me.CheckHasData() = False Then
                Exit Sub
            End If

            ' 振込額がマイナスのデータが含まれていないか
            If Me.CheckBankPayValue() = False Then
                Exit Sub
            End If

            ' 重複社員番号が存在するか
            If Me.CheckDuplexStafId() = False Then
                Exit Sub
            End If

            If _intSelectBtn = 0 AndAlso _blnIsFreeEntry = False Then
                If CLMsg.Show("GQ0042") = DialogResult.No Then
                    Exit Sub
                End If
            End If

            ' ボタンクリックフラグの更新
            Me._intReturnBtn = 0

            ' 印刷プレビュー画面表示
            iSelect = Me.ShowPrintPreview(False, fmPreview)
            If (iSelect <> 2) Then
                ' キャンセル以外が選択された場合は登録処理を行う
                If Me.EntryData() = False Then
                    Exit Sub
                End If

                If (iSelect = 0) Then
                    fmPreview.PrintOut()
                End If
                If Me._strPayStatusCd.Equals("03") = False Then
                    ' 現金振込以外の場合はCSVファイルを出力
                    Call Me.SaveCsvFile()
                End If

                ' 問題なく登録処理が完了した場合画面を閉じる
                Call Me.FormClose()
            End If

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

        End Try
    End Sub
#End Region

#Region " 振込ファイル出力ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnOutputCsv_Click
    '   名称　：振込ファイル出力ボタンクリック
    '   概要　：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnOutputCsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOutputCsv.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            ' CSV出力処理実行
            Call Me.SaveCsvFile()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region " 内容変更ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：内容変更ボタンクリック
    '   概要　：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChange.Click

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            ' 編集可能モードに変更
            Call Me.ChangeEditMode(True)

            If _dtStafBankCloseMember.Rows.Count > 0 Then
                ' 対象締め日の振込データ未作成組合員表示
                Call Me.ShowNotSendMember()
            End If

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

        End Try
    End Sub
#End Region

#Region " プレ印刷ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：プレ印刷ボタンクリック
    '   概要　：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim iSelect As Integer = -1
        Dim fmPreview As FM000205 = New FM000205

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            iSelect = Me.ShowPrintPreview(True, fmPreview)
            If iSelect = 3 Then
                ' 印刷処理実行
                fmPreview.PrintOut()
            End If

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

        End Try

    End Sub
#End Region

#Region " キャンセルボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            If CLMsg.Show("GQ0007") = DialogResult.No Then
                ' いいえが選択された場合は何も行わない
                Exit Sub
            End If

            ' ボタンクリックフラグの更新
            Me._intReturnBtn = 1
            If _intSelectBtn = 0 Then
                ' 新規作成の場合は検索画面に戻る
                Call Me.FormClose()
            Else
                ' データ照会の場合は画面を所期状態に戻す
                Call Me.CancelEdit()
                ' コントロールを編集不可モードにする
                Call Me.ChangeEditMode(False)
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        End Try

    End Sub
#End Region

#Region " 戻るボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnReturn_Click
    '   名称　：戻るボタンクリック
    '   概要　：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        ' ボタンクリックフラグの更新
        Me._intReturnBtn = 2

        ' 画面終了処理
        Call Me.FormClose()

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region " 組合員の追加ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnAddMember_Click
    '   名称　：組合員の追加ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnAddMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMember.Click

        ' 組合員抽出画面
        Dim fmUnion As FM000204 = New FM000204()
        Dim strUserIdList As List(Of String) = New List(Of String)

        ' 銀行口座情報
        Dim dtAccount As DataTable = Nothing

        ' DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            If Me.flxNetbank.Rows.Count > 1 Then
                Me.Cursor = Cursors.WaitCursor

                For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
                    ' 既に表示済みのユーザーIDを格納
                    strUserIdList.Add(Me.flxNetbank.GetData(iCnt, 1))
                Next
                ' 初期表示するメンバー削除不可
                fmUnion.AllowDeleteMember = False
                ' ユーザーIDリストを組合員抽出画面に渡す
                fmUnion.StafIDList = strUserIdList.ToArray()
            End If

            ' 組合員抽出画面の表示
            fmUnion.ShowDialog()
            Select Case fmUnion.IntQlickBtnFlag
                Case 0 ' OKボタン押下時
                    ' 選択された組合員のリスト
                    Dim dt As DataTable = fmUnion.SelectMemberList

                    ' DB接続開始
                    clsDb.Connect()

                    For Each dtRow As DataRow In dt.Rows
                        If strUserIdList.Contains(dtRow.Item("社員番号")) = False Then
                            Me.flxNetbank.Rows.Add()
                            Me.flxNetbank.SetCellStyle(Me.flxNetbank.Rows.Count - 1, 0, "bool")
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 1, dtRow.Item("社員番号"))   ' 01. 社員番号
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 2, dtRow.Item("ディジット")) ' 02. CD
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 3, dtRow.Item("名前"))       ' 03. 名前
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 4, dtRow.Item("組合支部"))   ' 04. 組合支部
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 5, 0)                        ' 05. 源泉
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 6, 0)                        ' 06. 日当
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 7, 0)                        ' 07. 調整金額
                            Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 8, 0)                        ' 08. 振込金額
                            If Me._strPayStatusCd <> "03" Then
                                ' 支払方法が現金以外の場合は口座情報を表示
                                dtAccount = GetAccountData(clsDb, dtRow.Item("社員番号"))
                                If dtAccount.Rows.Count > 0 Then
                                    For Each accRow As DataRow In dtAccount.Rows
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 10, accRow.Item("c_bank"))              ' 10. 金融機関コード
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 11, accRow.Item("bank_name"))           ' 11. 金融機関名
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 12, accRow.Item("c_bank_office"))       ' 12. 支店コード
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 13, accRow.Item("bank_office_name"))    ' 13. 支店名
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 14, accRow.Item("deposit_name"))        ' 14. 預金種目
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 15, accRow.Item("c_bank_account"))      ' 15. 口座番号
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 16, accRow.Item("l_account_name_kna"))  ' 16. 口座名義
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 17, accRow.Item("d_from"))              ' 17. 口座適用開始日
                                        Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 18, accRow.Item("deposit_name_om"))     ' 18. 預金種目略称
                                    Next
                                Else
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 10, String.Empty)   ' 10. 金融機関コード
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 11, String.Empty)   ' 11. 金融機関名
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 12, String.Empty)   ' 12. 支店コード
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 13, String.Empty)   ' 13. 支店名
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 14, String.Empty)   ' 14. 預金種目
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 15, String.Empty)   ' 15. 口座番号
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 16, String.Empty)   ' 16. 口座名義
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 17, String.Empty)   ' 17. 口座適用開始日
                                    Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 18, String.Empty)   ' 18. 預金種目略称
                                End If
                            End If

                            ' 各列のバックカラー変更
                            If CanMakeData(Me.flxNetbank.Rows.Count - 1) = False Then
                                ' 支払不可
                                SetNotMakeStyleForRow(Me.flxNetbank.Rows.Count - 1)
                            Else
                                ' 支払可能
                                Me.flxNetbank.SetData(Me.flxNetbank.Rows.Count - 1, 0, True)
                                SetNotMakeStyleForRow(Me.flxNetbank.Rows.Count - 1)
                            End If
                        End If
                    Next
                    ' データ件数の更新
                    Call Me.SetDataCount()
                Case 1
                    ' キャンセルの場合何も行わない
            End Select

            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default
            clsDb.Disconnect()

        End Try

    End Sub
#End Region

#Region " 全行チェックONボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnAllCheckOn_Click
    '   名称　：全行チェックONボタンクリック
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnAllCheckOn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheckOn.Click

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' 全行チェック
            Call Me.AllCheck(True)

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
    End Sub
#End Region

#Region " 全行チェックOFFボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnAllCheckOff_Click
    '   名称　：全行チェックOFFボタンクリック
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnAllCheckOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheckOff.Click

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' 全行チェックOFF
            Call Me.AllCheck(False)

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
    End Sub
#End Region

#Region " 振込データ作成対象グリッドクリック "
    '***************************************************************************************************
    '   ＩＤ　：flxNetbank_Click
    '   名称　：振込データ作成対象グリッドクリック
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub flxNetbank_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxNetbank.Click

        Try
            If Me._blnIsEdit Then
                Dim info As HitTestInfo = Me.flxNetbank.HitTest
                If info.Type = HitTestTypeEnum.Checkbox AndAlso Me.flxNetbank.Cols.Item(info.Column).Caption.Equals("*") Then
                    Dim row As Row
                    For Each row In DirectCast(Me.flxNetbank.Rows.Selected, IEnumerable)
                        If Me.CanMakeData(row.Index) = False Then
                            CLMsg.Show("GI0030", row.Item(3).ToString)
                            row.Item(0) = False
                        Else
                            row.Item(0) = Not CBool(row.Item(0))
                        End If
                    Next
                    Me.SetNotMakeStyle()
                    Me.CalcTotal()
                ElseIf info.Type = HitTestTypeEnum.Cell AndAlso (Me.flxNetbank.Cols.Item(info.Column).Caption.Equals("調整金額")) Then
                    Me.flxNetbank.StartEditing(info.Row, 7)
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
    End Sub
#End Region

#Region " 振込データ作成対象キーダウン "
    '***************************************************************************************************
    '   ＩＤ　：flxNetbank_KeyDown
    '   名称　：振込データ作成対象キーダウン
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub flxNetbank_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles flxNetbank.KeyDown

        If Me._blnIsEdit = True Then
            If e.KeyCode = Keys.V AndAlso e.Control = True Then
                Dim strTest As String = System.Windows.Forms.Clipboard.GetDataObject().GetData(System.Windows.Forms.DataFormats.Text).ToString()
                Dim intIndex As Integer = 0
                If ChkNumber(strTest) = False Then
                    ' クリップボードのテキストが数値でない場合処理を抜ける
                    Exit Sub
                Else
                    Dim selectedRow As Row
                    Dim lngCopy As Long = CLng(strTest)
                    For Each selectedRow In DirectCast(Me.flxNetbank.Rows.Selected, IEnumerable)
                        intIndex = selectedRow.Index
                        If selectedRow.Item(0) = True Then
                            ' チェックがされている列の場合、調整金額に値を入れる
                            selectedRow.Item(7) = lngCopy
                            Call Me.ReCallculation(intIndex)
                        End If
                    Next

                    ' 振込金額の更新
                    Call Me.CalcTotal()
                End If
            End If
        End If

    End Sub
#End Region

#Region " 振込データ作成対象編集開始 "
    '***************************************************************************************************
    '   ＩＤ　：flxNetbank_StartEdit
    '   名称　：振込データ作成対象編集開始
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub flxNetbank_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxNetbank.StartEdit
        Try
            ' 編集不可モードの場合イベントキャンセル
            If Me._blnIsEdit = False Then
                e.Cancel = True
            End If

            ' 調整金額以外の列の場合イベントキャンセル
            If Not Me.flxNetbank.Cols.Item(e.Col).Caption.Equals("調整金額") Then
                e.Cancel = True
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

#Region " 振込データ作成対象 "
    '***************************************************************************************************
    '   ＩＤ　：flxNetbank_ValidateEdit
    '   名称　：振込データ作成対象
    '   概要　：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub flxNetbank_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles flxNetbank.ValidateEdit

        Try
            If (Me.flxNetbank.Cols.Item(e.Col).Caption.Equals("調整金額") _
            AndAlso Not Me.flxNetbank.Editor.Text.Equals(Me.flxNetbank.Item(e.Row, e.Col).ToString)) Then
                If CBool(Me.flxNetbank.Item(e.Row, 0)) = False Then
                    CLMsg.Show("GI0029")
                    Me.flxNetbank.FinishEditing(True)
                    Exit Sub
                Else
                    Me.ReCallculation(e.Row)
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region
#End Region

#Region " 関数 "
#Region " 画面初期化 - 新規作成時 "
    '***************************************************************************************************
    '   ＩＤ　：InitializeNew
    '   名称　：画面初期化 - 新規作成時
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/06(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>画面初期化 - 新規作成時</summary>
    ''' <remarks></remarks>
    Private Sub InitializeNew()

        Try
            If _blnIsFreeEntry = False Then
                Call Me.ShowPayMember()
            Else
                ' 表示件数のみ更新する
                Call Me.SetDataCount()
            End If

            ' プレ印刷ボタン、ファイル出力ボタン非表示
            Me.btnPrint.Visible = False
            Me.btnOutputCsv.Visible = False

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " 画面初期化 - 詳細 "
    '***************************************************************************************************
    '   ＩＤ　：InitializeDetail
    '   名称　：画面初期化 - 詳細
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/06(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>画面初期化 - 詳細</summary>
    ''' <remarks></remarks>
    Private Sub InitializeDetail()

        Try
            ' 登録済みデータの表示
            Call Me.ShowBankSendData()

            ' 画面を編集不可モードにする
            Call Me.ChangeEditMode(False)

            ' 振込日が現在日以前の場合内容変更ボタンは使用不可
            If Me.datePayDay <= _NowDate Then
                Me.btnChange.Enabled = False
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " セルスタイル定義 "
    '***************************************************************************************************
    '   ＩＤ　：DefinitionStyles
    '   名称　：スタイル定義
    '   概要　：セルのスタイルを定義
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>スタイル定義</summary>
    ''' <remarks></remarks>
    Private Sub DefinitionStyles()

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' スタイルを作成
            Dim cs As CellStyle = flxNetbank.Styles.Add("DisableEdit")

            'cs.ForeColor = Color.Black
            cs.BackColor = Color.LightYellow
            cs = flxNetbank.Styles.Add("CanEdit")
            cs.BackColor = Color.White
            cs = flxNetbank.Styles.Add("NotCheck")
            cs.BackColor = Color.Silver
            cs = flxNetbank.Styles.Add("bool")
            cs.DataType = Type.GetType("System.Boolean")
            cs.ImageAlign = ImageAlignEnum.CenterCenter

        Catch ex As Exception
            ' ログ出力（致命的エラー）

            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region " 振込対象者の表示 "
    '***************************************************************************************************
    '   ＩＤ　：ShowPayMember
    '   名称　：振込対象者の表示
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/06(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込対象者の表示</summary>
    ''' <remarks></remarks>
    Private Sub ShowPayMember()

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        ' 源泉側の該当ユーザー取得用
        'Dim strCut As String = "SELECT tbl1.c_user_id, CONVERT(date, FORMAT(tbl1.d_years, 'yyyy/MM/dd')) AS close_day, tbl1.k_daily_pay_kind, " &
        Dim strCut As String = "SELECT tbl1.c_user_id, FORMAT(tbl1.d_years, 'yyyy/MM/dd') AS close_day, tbl1.k_daily_pay_kind, " &
                               "IIF(tbl1.k_daily_pay_kind = '05', SUM(tbl1.s_pay_time_cut_monthly + tbl1.s_pay_strike_cut_monthly + " &
                               "tbl1.s_officer_pay - tbl1.s_pay_time_cut_monthly_break - tbl1.s_pay_strike_cut_monthly_break - tbl1.s_cut_monthly_taxation)," &
                               "IIF(tbl1.k_daily_pay_kind = '06',SUM(tbl1.s_pay_time_cut_once + s_pay_strike_cut_once - " &
                               "s_pay_time_cut_once_break -s_pay_strike_cut_once_break - s_cut_once_taxation ),0)) AS total_pay " &
                               " FROM taxation_total AS tbl1 WHERE ({0}) AND tbl1.k_daily_pay_kind = '{1}' " &
                               " GROUP BY tbl1.c_user_id, FORMAT(tbl1.d_years, 'yyyy/MM/dd'), tbl1.k_daily_pay_kind "

        ' 日当側の該当ユーザー取得用
        'Dim strDaily As String = "SELECT tbl1.c_user_id, CONVERT(date, FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd')) AS close_day " &
        Dim strDaily As String = "SELECT tbl1.c_user_id, FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') AS close_day " &
                         ", tbl1.k_daily_pay_kind, SUM(tbl1.s_daily_pay_total + tbl1.s_food_expenses_total + tbl1.s_next_balance_daily_pay_total + tbl1.s_next_balance_food_expenses_total ) AS total_pay " &
                         " FROM call_roll_user AS tbl1 WHERE ({0}) AND tbl1.k_daily_pay_kind = '{1}' " &
                         " GROUP BY tbl1.c_user_id, FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd'), tbl1.k_daily_pay_kind "

        Dim strWhereCut As String = String.Empty        ' 賃金カット・役員手当の対象者取得用WHERE句
        Dim strWhereOnceCut As String = String.Empty    ' 一時金カットの対象者取得用WHERE句
        Dim strWhereCommittee As String = String.Empty  ' 部・委員会日当の対象者取得用WHERE句
        Dim strWhereBranch As String = String.Empty     ' 支部委員会（三役）日当の対象者取得用WHERE句
        Dim strWhereExecutive As String = String.Empty  ' 中央執行委員会日当の対象者取得用WHERE句
        Dim strWhereDgm As String = String.Empty        ' DGM日当の対象者取得用WHERE句
        Dim strSqlCut As String = String.Empty          ' 賃金カット・役員手当SQL
        Dim strSqlOnceCut As String = String.Empty      ' 一時金カットSQL
        Dim strSqlCommittee As String = String.Empty    ' 部・委員会日当SQL
        Dim strSqlBranch As String = String.Empty       ' 支部委員会（三役）日当SQL
        Dim strSqlExecutive As String = String.Empty    ' 中央執行委員会日当SQL
        Dim strSqlDgm As String = String.Empty          ' DGM日当SQL
        Dim strSql As String = String.Empty             ' 実行SQL
        Dim strSqlUnion As String = String.Empty        ' UNION部分

        ' 現在日付
        Dim strNow As String = Now.ToString("yyyyMMdd")

        ' 振り込み日
        Dim strPayDay As String = Format(Me._datePayDay, "yyyyMMdd")

        ' 振込総額
        Dim lngSendSum As Long = 0

        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        ' ユーザーごとの源泉合計額
        Dim intTotalCut As Integer = 0

        ' ユーザーごとの日当合計額
        Dim intTotalDailyPay As Integer = 0
        Dim dtShow As DataTable = New DataTable

        Try
            If _dtSelectCloseDay.Rows.Count > 0 Then
                ' DB接続開始
                Call clsDb.Connect()

                For Each dtRow As DataRow In _dtSelectCloseDay.Rows
                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_CUT_CODE Then ' 賃金カットの場合
                        ' 源泉側グリッドに締め日を表示
                        Me.dgdPayCutCloseDay.Rows.Add()
                        Me.dgdPayCutCloseDay(0, Me.dgdPayCutCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_CUT
                        Me.dgdPayCutCloseDay(0, Me.dgdPayCutCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdPayCutCloseDay(1, Me.dgdPayCutCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereCut = String.Empty Then
                            strWhereCut = "FORMAT(tbl1.d_years, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereCut = strWhereCut & "OR FORMAT(tbl1.d_years, 'yyyy/MM/dd') like '" & dtRow.Item("close_day") & "%' "
                        End If
                    End If

                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_ONCE_CUT_CODE Then ' 一時金カットの場合
                        ' 源泉側グリッドに締め日を表示
                        Me.dgdPayCutCloseDay.Rows.Add()
                        Me.dgdPayCutCloseDay(0, Me.dgdPayCutCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_ONCE_CUT
                        Me.dgdPayCutCloseDay(0, Me.dgdPayCutCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdPayCutCloseDay(1, Me.dgdPayCutCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereOnceCut = String.Empty Then
                            strWhereOnceCut = "FORMAT(tbl1.d_years, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereOnceCut = strWhereOnceCut & "OR FORMAT(tbl1.d_years, 'yyyy/MM/dd') like '" & dtRow.Item("close_day") & "%' "
                        End If
                    End If

                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_COMMITTEE_CODE Then ' 日当（部／委員会）の場合
                        ' 日当側グリッドに締め日を表示
                        Me.dgdDayPayCloseDay.Rows.Add()
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_COMMITTEE
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdDayPayCloseDay(1, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereCommittee = String.Empty Then
                            strWhereCommittee = "FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereCommittee = strWhereCommittee & "OR FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like '" & dtRow.Item("close_day") & "%' "
                        End If
                    End If

                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_BRANCH_CODE Then ' 日当（支部委員）の場合
                        ' 日当側グリッドに締め日を表示
                        Me.dgdDayPayCloseDay.Rows.Add()
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_BRANCH
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdDayPayCloseDay(1, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereBranch = String.Empty Then
                            strWhereBranch = "FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereBranch = strWhereBranch & "OR FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        End If
                    End If

                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_EXECUTIVE_CODE Then ' 日当（中央執行委員会）の場合
                        ' 日当側グリッドに締め日を表示
                        Me.dgdDayPayCloseDay.Rows.Add()
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_EXECUTIVE
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdDayPayCloseDay(1, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereExecutive = String.Empty Then
                            strWhereExecutive = "FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereExecutive = strWhereExecutive & "OR FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        End If
                    End If

                    If dtRow.Item("close_day_kind") = DAILY_PAY_KIND_DGM_CODE Then ' 日当（DGM）の場合
                        ' 日当側グリッドに締め日を表示
                        Me.dgdDayPayCloseDay.Rows.Add()
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = DAILY_PAY_KIND_DGM
                        Me.dgdDayPayCloseDay(0, Me.dgdDayPayCloseDay.Rows.Count - 1).Tag = dtRow.Item("close_day_kind")
                        Me.dgdDayPayCloseDay(1, Me.dgdDayPayCloseDay.Rows.Count - 1).Value = dtRow.Item("close_day")

                        If strWhereDgm = String.Empty Then
                            strWhereDgm = "FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        Else
                            strWhereDgm = strWhereDgm & "OR FORMAT(tbl1.d_daily_pay_close, 'yyyy/MM/dd') like'" & dtRow.Item("close_day") & "%' "
                        End If
                    End If
                Next

                ' 賃金カット／役員手当の対象者取得SQL作成
                If strWhereCut <> String.Empty Then
                    strSqlCut = String.Format(strCut, strWhereCut, DAILY_PAY_KIND_CUT_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlCut
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlCut
                    End If
                End If

                ' 一時金カットの対象者取得SQL作成
                If strWhereOnceCut <> String.Empty Then
                    strSqlOnceCut = String.Format(strCut, strWhereOnceCut, DAILY_PAY_KIND_ONCE_CUT_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlOnceCut
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlOnceCut
                    End If
                End If

                ' 部／委員会日当の対象者取得SQL作成
                If strWhereCommittee <> String.Empty Then
                    strSqlCommittee = String.Format(strDaily, strWhereCommittee, DAILY_PAY_KIND_COMMITTEE_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlCommittee
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlCommittee
                    End If
                End If

                ' 支部委員（三役）日当の対象者取得SQL作成
                If strWhereBranch <> String.Empty Then
                    strSqlBranch = String.Format(strDaily, strWhereBranch, DAILY_PAY_KIND_BRANCH_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlBranch
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlBranch
                    End If
                End If

                ' 中央執行委員会日当の対象者取得SQL作成
                If strWhereExecutive <> String.Empty Then
                    strSqlExecutive = String.Format(strDaily, strWhereExecutive, DAILY_PAY_KIND_EXECUTIVE_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlExecutive
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlExecutive
                    End If
                End If

                ' DGM日当の対象者取得SQL作成
                If strWhereDgm <> String.Empty Then
                    strSqlDgm = String.Format(strDaily, strWhereDgm, DAILY_PAY_KIND_DGM_CODE)
                    If strSqlUnion = String.Empty Then
                        strSqlUnion = strSqlDgm
                    Else
                        strSqlUnion = strSqlUnion & " UNION " & strSqlDgm
                    End If
                End If

                strSqlUnion = "(" & strSqlUnion & ") AS send_user "

                ' 選択した締め日種別の締め日に属するユーザー取得
                strSql = "SELECT staf.c_user_id,send_user.close_day, send_user.k_daily_pay_kind  " & _
                         "      ,IIF(send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_CUT_CODE & "' " & _
                         "        OR send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_ONCE_CUT_CODE & "', send_user.total_pay, 0) AS payCutTotal " & _
                         "      ,IIF(send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_COMMITTEE_CODE & "' " & _
                         "        OR send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_BRANCH_CODE & "' " & _
                         "        OR send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_EXECUTIVE_CODE & "' " & _
                         "        OR send_user.k_daily_pay_kind = '" & DAILY_PAY_KIND_DGM_CODE & "', send_user.total_pay, 0) AS payDailyTotal " & _
                         "      , 0 AS s_adjust, staf.c_dezit,staf.l_name, staf.belonging, account.c_bank, account.bank_name " & _
                         "      ,account.c_bank_office, account.bank_office_name, account.deposit_name, account.d_from " & _
                         "      ,account.c_bank_account, account.l_account_name_kna, account.deposit_name_om " & _
                         "FROM " & _
                         "     (" & strSqlUnion & "INNER JOIN " & _
                         "     (SELECT attr1.c_user_id AS c_user_id, attr1.l_name AS l_name " & _
                         "            ,attr1.c_dezit AS c_dezit, dtl1.l_name AS belonging " & _
                         "      FROM staf_attribute AS attr1, " & _
                         "           (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from  " & _
                         "            FROM staf_attribute  " & _
                         "            WHERE d_from <= '" & strPayDay & "' " & _
                         "            GROUP BY c_user_id, c_ksh, c_staf_id " & _
                         "           ) AS attr2, " & _
                         "           constant_dtl AS dtl1 " & _
                         "      WHERE attr1.c_user_id = attr2.c_user_id " & _
                         "      AND attr1.c_ksh = attr2.c_ksh " & _
                         "      AND attr1.c_staf_id = attr2.c_staf_id " & _
                         "      AND attr1.d_from = attr2.now_from " & _
                         "      AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr1.k_belonging " & _
                         "     ) AS staf ON send_user.c_user_id = staf.c_user_id) LEFT JOIN " & _
                         "     (SELECT acc1.*,bank1.l_bank_name_kna AS bank_name, bankdtl1.l_bank_office_name_kna AS bank_office_name " & _
                         "            ,dtl2.l_name AS deposit_name, dtl2.l_omission_name AS deposit_name_om " & _
                         "      FROM staf_account AS acc1, " & _
                         "           (SELECT c_user_id, MAX(d_from) AS now_from " & _
                         "            FROM staf_account " & _
                         "            WHERE d_from <= '" & strPayDay & "' " & _
                         "            GROUP BY c_user_id " & _
                         "           ) AS acc2, " & _
                         "           bank_info AS bank1," & _
                         "           bank_info_dtl AS bankdtl1," & _
                         "           constant_dtl AS dtl2 " & _
                         "      WHERE acc1.c_user_id = acc2.c_user_id " & _
                         "      AND acc1.d_from = acc2.now_from " & _
                         "      AND acc1.c_bank = bank1.c_bank " & _
                         "      AND acc1.c_bank_office = bankdtl1.c_bank_office " & _
                         "      AND bank1.c_bank = bankdtl1.c_bank " & _
                         "      AND bank1.d_from <= '" & strPayDay & "' AND bank1.d_to >= '" & strPayDay & "'" & _
                         "      AND bankdtl1.d_from <= '" & strPayDay & "' AND bankdtl1.d_to >= '" & strPayDay & "'" & _
                         "      AND dtl2.c_constant = 'DEPOSIT_ITEMS' AND dtl2.c_constant_seq = acc1.k_deposit_items " & _
                         "     ) AS account " & _
                         "     ON staf.c_user_id = account.c_user_id " & _
                         "ORDER BY CLng(staf.c_user_id) "

                dtRet = clsDb.ExecuteSql(strSql)
                ' 労金データ作成済みリスト
                _dtMakeUser = MakeSendUserList()

                Dim rowArray As DataRow() = Nothing
                'Dim rowShowArray As DataRow()
                Dim userList As List(Of String) = Nothing

                If dtRet.Rows.Count > 0 Then

                    For cCnt As Integer = 0 To dtRet.Columns.Count - 1
                        ' グリッド表示用データテーブルにカラムを追加
                        dtShow.Columns.Add(dtRet.Columns(cCnt).Caption)
                    Next

                    For Each drNew As DataRow In dtRet.Rows
                        If _dtMakeUser.Rows.Count > 0 Then '既に作成済みの労金データが存在する場合
                            ' 作成済みユーザーリストに同社員番号、締め日、締め日種別をもつデータが存在するか
                            rowArray = _dtMakeUser.Select("c_user_id = '" & drNew.Item("c_user_id") & _
                                                    "' AND d_pay_close ='" & CDate(drNew.Item("close_day")).Year & _
                                                    CDate(drNew.Item("close_day")).Month.ToString.PadLeft(2, "0") & _
                                                    "' AND k_daily_pay_kind ='" & drNew.Item("k_daily_pay_kind") & "' ")
                        End If

                        If _dtMakeUser.Rows.Count = 0 OrElse rowArray.Length = 0 Then '
                            userList = New List(Of String)
                            Dim dtTypeName As String = drNew("close_day").GetType().Name
                            Dim dtTypeName2 As String = drNew.Item("close_day").GetType().Name
                            If (TypeOf drNew.Item("close_day") Is DateTime) Then
                                Dim dtCloseDay As DateTime = drNew.Item("close_day")
                            Else
                                Dim dtCloseDay As Object = drNew.Item("close_day")
                            End If
                            userList.Add(drNew.Item("close_day"))
                            userList.Add(drNew.Item("k_daily_pay_kind"))
                            userList.Add(drNew.Item("c_user_id"))
                            If drNew.Item("k_daily_pay_kind") = DAILY_PAY_KIND_CUT_CODE _
                                OrElse drNew.Item("k_daily_pay_kind") = DAILY_PAY_KIND_ONCE_CUT_CODE Then
                                userList.Add(drNew.Item("payCutTotal"))
                            Else
                                userList.Add(drNew.Item("payDailyTotal"))
                            End If
                            userList.Add(String.Empty)
                            userList.Add(String.Empty)
                            _AllUserList.Add(userList)
                            If drNew.Item("k_daily_pay_kind") = DAILY_PAY_KIND_CUT_CODE Then
                                If _dicUserCommittee.ContainsKey(drNew.Item("c_user_id")) = False Then
                                    Call Me.GetCommitteeData(clsDb, drNew.Item("c_user_id"))
                                End If
                            End If

                            ' 既に表示リストに追加済みのユーザーかチェック
                            Dim rowShowArray As DataRow() = dtShow.Select("c_user_id = '" & drNew.Item("c_user_id") & "' ")
                            If rowShowArray.Length > 0 Then
                                ' 表示リストに追加済みの場合、源泉、日当をそれぞれ加算する
                                rowShowArray(0).Item("payCutTotal") = rowShowArray(0).Item("payCutTotal") + drNew.Item("payCutTotal")
                                rowShowArray(0).Item("payDailyTotal") = rowShowArray(0).Item("payDailyTotal") + drNew.Item("payDailyTotal")
                            Else
                                ' 表示リストに追加されていないユーザーの場合
                                dtShow.Rows.Add()
                                For cCnt As Integer = 0 To dtRet.Columns.Count - 1
                                    dtShow.Rows(dtShow.Rows.Count - 1).Item(dtRet.Columns(cCnt).Caption) = drNew.Item(dtRet.Columns(cCnt).Caption)
                                Next
                            End If
                        End If
                        intTotalCut = 0
                        intTotalDailyPay = 0
                    Next
                End If
                ' データをグリッドへセット
                Call Me.ShowNetBankGrid(dtShow)
            Else
                ' 表示件数の更新
                Call Me.SetDataCount()
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' DB接続終了
            Call clsDb.Disconnect()

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region " 労金データ表示処理 "
    '***************************************************************************************************
    '   ＩＤ　：ShowBankSendData
    '   名称　：労金データ表示処理
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>労金データ表示処理</summary>
    ''' <remarks></remarks>
    Private Sub ShowBankSendData()

        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty


        Dim strCutDayArray As String() = Nothing        ' 源泉側締日データ
        Dim strCutKindArray As String() = Nothing       ' 源泉側締め日種別データ
        Dim strDailyDayArray As String() = Nothing      ' 日当側締め日データ
        Dim strDailyKindArray As String() = Nothing     ' 日当側締め日種別データ

        Dim userList As List(Of String) = Nothing

        _dtStafBankSend = New DataTable
        _dtStafBankSendMember = New DataTable
        _dtStafBankClose = New DataTable
        _dtStafBankCloseMember = New DataTable

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' データベース接続処理
            clsDb.Connect()

            ' 労金データ取得
            _dtStafBankSend = GetStafBankSend(clsDb)

            ' 労金詳細取得
            _dtStafBankSendMember = GetStafBankSendMember(clsDb)

            If _dtStafBankSend.Rows.Count > 0 Then
                If (_dtStafBankSend.Rows(0).Item("c_daily_pay_kind").Equals(DBNull.Value) = False _
                AndAlso _dtStafBankSend.Rows(0).Item("c_daily_pay_kind") <> String.Empty) _
                OrElse (_dtStafBankSend.Rows(0).Item("c_pay_time_kind").Equals(DBNull.Value) = False _
                AndAlso _dtStafBankSend.Rows(0).Item("c_pay_time_kind") <> String.Empty) Then
                    ' 表示対象が締め日未選択の振込データでない場合、締め日関連のデータも取得する
                    ' 締め日・締め日種別の条件文字列取得
                    Dim strWherePhrase As String = Me.GetWhereStringForStafBankClose(_dtStafBankSend.Rows(0).Item("c_pay_time_kind").ToString, _
                                                                                     _dtStafBankSend.Rows(0).Item("c_pay_time_cut").ToString, _
                                                                                     _dtStafBankSend.Rows(0).Item("c_daily_pay_kind").ToString, _
                                                                                     _dtStafBankSend.Rows(0).Item("c_daily_pay_close").ToString)
                    ' 締め日データ取得
                    _dtStafBankClose = GetStafBankCloseData(clsDb, strWherePhrase)
                    ' 締め日メンバーデータ取得
                    _dtStafBankCloseMember = GetStafBankCloseMember(clsDb, strWherePhrase)
                Else
                    ' 締め日未選択データフラグの設定
                    Me._blnIsFreeEntry = True
                End If
                strCutDayArray = _dtStafBankSend.Rows(0).Item("c_pay_time_cut").ToString.Split(",")
                strCutKindArray = _dtStafBankSend.Rows(0).Item("c_pay_time_kind").ToString.Split(",")
                strDailyDayArray = _dtStafBankSend.Rows(0).Item("c_daily_pay_close").ToString.Split(",")
                strDailyKindArray = _dtStafBankSend.Rows(0).Item("c_daily_pay_kind").ToString.Split(",")

                ' 源泉側グリッドの表示
                For iCnt As Integer = 0 To strCutDayArray.Length - 1
                    If String.IsNullOrEmpty(strCutKindArray(iCnt)) = False Then
                        Me.dgdPayCutCloseDay.Rows.Add()
                        ' 締め日種別名称
                        If strCutKindArray(iCnt) = DAILY_PAY_KIND_CUT_CODE Then
                            Me.dgdPayCutCloseDay(0, iCnt).Value = DAILY_PAY_KIND_CUT

                            If _dtStafBankCloseMember.Rows.Count > 0 Then
                                Dim rowArray As DataRow() = _dtStafBankCloseMember.Select("k_daily_pay_kind = '" & strCutKindArray(iCnt) &
                                                                                          "' AND d_pay_close = '" & strCutDayArray(iCnt).Replace("/", "").Replace("-", "") & "' ")
                                For iRowCnt As Integer = 0 To rowArray.Length - 1
                                    If _dicUserCommittee.ContainsKey(rowArray(iRowCnt).Item("c_user_id")) = False Then
                                        Call Me.GetCommitteeData(clsDb, rowArray(iRowCnt).Item("c_user_id"))
                                    End If
                                Next
                            End If
                        ElseIf strCutKindArray(iCnt) = DAILY_PAY_KIND_ONCE_CUT_CODE Then
                            Me.dgdPayCutCloseDay(0, iCnt).Value = DAILY_PAY_KIND_ONCE_CUT
                        End If
                        ' 締め日種別
                        Me.dgdPayCutCloseDay(0, iCnt).Tag = strCutKindArray(iCnt)
                        ' 締め日
                        Me.dgdPayCutCloseDay(1, iCnt).Value = strCutDayArray(iCnt)
                    End If
                Next

                ' 日当側のグリッドの表示
                For iCnt As Integer = 0 To strDailyDayArray.Length - 1
                    If String.IsNullOrEmpty(strDailyKindArray(iCnt)) = False Then
                        Me.dgdDayPayCloseDay.Rows.Add()
                        ' 締め日種別名称
                        If strDailyKindArray(iCnt) = DAILY_PAY_KIND_COMMITTEE_CODE Then
                            Me.dgdDayPayCloseDay(0, iCnt).Value = DAILY_PAY_KIND_COMMITTEE
                        ElseIf strDailyKindArray(iCnt) = DAILY_PAY_KIND_BRANCH_CODE Then
                            Me.dgdDayPayCloseDay(0, iCnt).Value = DAILY_PAY_KIND_BRANCH
                        ElseIf strDailyKindArray(iCnt) = DAILY_PAY_KIND_EXECUTIVE_CODE Then
                            Me.dgdDayPayCloseDay(0, iCnt).Value = DAILY_PAY_KIND_EXECUTIVE
                        ElseIf strDailyKindArray(iCnt) = DAILY_PAY_KIND_DGM_CODE Then
                            Me.dgdDayPayCloseDay(0, iCnt).Value = DAILY_PAY_KIND_DGM
                        End If
                        ' 締め日種別
                        Me.dgdDayPayCloseDay(0, iCnt).Tag = strDailyKindArray(iCnt)
                        ' 締め日
                        Me.dgdDayPayCloseDay(1, iCnt).Value = strDailyDayArray(iCnt)
                    End If
                Next

                ' 振込済みユーザーをグリッドに表示
                If _dtStafBankSendMember.Rows.Count > 0 Then
                    Call Me.ShowNetBankGrid(_dtStafBankSendMember)
                End If

                'staf_bank_close_member用にリスト作成
                For Each memRow As DataRow In _dtStafBankCloseMember.Rows
                    userList = New List(Of String)
                    userList.Add(memRow.Item("d_pay_close"))
                    userList.Add(memRow.Item("k_daily_pay_kind"))
                    userList.Add(memRow.Item("c_user_id"))
                    userList.Add(memRow.Item("s_pay"))
                    ' 振込ID、振込日も格納
                    userList.Add(memRow.Item("c_staf_bank_send_id"))
                    userList.Add(memRow.Item("d_bank_send"))
                    _AllUserList.Add(userList)
                Next

                'If (_dtStafBankSend.Rows(0).Item("c_daily_pay_kind").Equals(DBNull.Value) OrElse _
                '    _dtStafBankSend.Rows(0).Item("c_daily_pay_kind") = String.Empty) = True AndAlso _
                '    (_dtStafBankSend.Rows(0).Item("c_pay_time_kind").Equals(DBNull.Value) OrElse _
                '    _dtStafBankSend.Rows(0).Item("c_pay_time_kind") = String.Empty) = True Then
                '    ' 締め日未選択データフラグの設定
                '    Me._blnIsFreeEntry = True
                'End If

                '' 労金データ作成済みリスト
                '_dtMakeUser = MakeSendUserList()
            End If

        Catch ex As Exception
        Finally
            ' DB接続終了
            clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub

#End Region

#Region " 振込データ未作成の組合員表示 "
    '***************************************************************************************************
    '   ＩＤ　：ShowNotSendMember
    '   名称　：振込データ未作成の組合員表示
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/22(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/22(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowNotSendMember()

        Dim clsDb As CLAccessMdb = New CLAccessMdb
        ' グリッドに表示済みのユーザーリスト
        Dim showUserList As List(Of String) = New List(Of String)
        Dim dtShow As DataTable = New DataTable
        Dim dtUserInfo As DataTable = Nothing
        Dim rowArray As DataRow() = Nothing
        Dim rowShowArray As DataRow() = Nothing
        ' ユーザーごとの源泉合計額
        Dim intTotalCut As Integer = 0
        ' ユーザーごとの日当合計額
        Dim intTotalDailyPay As Integer = 0

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' グリッドから表示済み社員番号を取得
            For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
                showUserList.Add(Me.flxNetbank.GetData(iCnt, 1))
            Next

            ' 表示データテーブルのカラム追加
            dtShow.Columns.Add("c_user_id")
            dtShow.Columns.Add("c_dezit")
            dtShow.Columns.Add("l_name")
            dtShow.Columns.Add("belonging")
            dtShow.Columns.Add("payCutTotal")
            dtShow.Columns.Add("payDailyTotal")
            dtShow.Columns.Add("s_adjust")
            dtShow.Columns.Add("c_bank")
            dtShow.Columns.Add("bank_name")
            dtShow.Columns.Add("c_bank_office")
            dtShow.Columns.Add("bank_office_name")
            dtShow.Columns.Add("deposit_name")
            dtShow.Columns.Add("c_bank_account")
            dtShow.Columns.Add("l_account_name_kna")
            dtShow.Columns.Add("d_from")
            dtShow.Columns.Add("deposit_name_om")
            ' StafBankCloseMemberから表示用データの作成
            rowArray = _dtStafBankCloseMember.Select("d_bank_send ='" & MAX_DATE & _
                                                     "' AND (c_staf_bank_send_id = '' OR c_staf_bank_send_id IS NULL) ")

            ' DB接続開始
            clsDb.Connect()
            For Each notSendRow In rowArray
                If notSendRow.Item("k_daily_pay_kind") = DAILY_PAY_KIND_CUT_CODE _
                        OrElse notSendRow.Item("k_daily_pay_kind") = DAILY_PAY_KIND_ONCE_CUT_CODE Then
                    intTotalCut = notSendRow.Item("s_pay")
                Else
                    intTotalDailyPay = notSendRow.Item("s_pay")
                End If

                If showUserList.Contains(notSendRow.Item("c_user_id")) = False Then
                    rowShowArray = dtShow.Select("c_user_id = '" & notSendRow.Item("c_user_id") & "' ")
                    If rowShowArray.Length > 0 Then
                        ' 表示リストに追加済みの場合、源泉、日当をそれぞれ加算する
                        If notSendRow.Item("k_daily_pay_kind") = DAILY_PAY_KIND_CUT_CODE _
                        OrElse notSendRow.Item("k_daily_pay_kind") = DAILY_PAY_KIND_ONCE_CUT_CODE Then
                            ' 源泉額の更新
                            rowShowArray(0).Item("payCutTotal") = rowShowArray(0).Item("payCutTotal") + intTotalCut
                        Else
                            ' 日当額の更新
                            rowShowArray(0).Item("payDailyTotal") = rowShowArray(0).Item("payDailyTotal") + intTotalDailyPay
                        End If
                    Else
                        ' 表示リストに追加されていないユーザーの場合
                        dtUserInfo = GetAttributeAccountData(clsDb, notSendRow.Item("c_user_id"))
                        dtShow.Rows.Add()
                        For cCnt As Integer = 0 To dtShow.Columns.Count - 1
                            If dtShow.Columns(cCnt).Caption = "payCutTotal" Then
                                ' 源泉額の追加
                                dtShow.Rows(dtShow.Rows.Count - 1).Item(dtShow.Columns(cCnt).Caption) = intTotalCut
                            ElseIf dtShow.Columns(cCnt).Caption = "payDailyTotal" Then
                                ' 日当額の追加
                                dtShow.Rows(dtShow.Rows.Count - 1).Item(dtShow.Columns(cCnt).Caption) = intTotalDailyPay
                            ElseIf dtShow.Columns(cCnt).Caption = "s_adjust" Then
                                '調整額の追加
                                dtShow.Rows(dtShow.Rows.Count - 1).Item(dtShow.Columns(cCnt).Caption) = 0
                            Else
                                '源泉、日当以外のデータ追加
                                dtShow.Rows(dtShow.Rows.Count - 1).Item(dtShow.Columns(cCnt).Caption) = dtUserInfo.Rows(0).Item(dtShow.Columns(cCnt).Caption)
                            End If
                        Next
                    End If
                End If
                intTotalCut = 0
                intTotalDailyPay = 0
            Next

            ' グリッドへ追加処理
            Call Me.ShowNetBankGrid(dtShow)
        Catch ex As Exception

        Finally
            clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub

#End Region

#Region " 労金データグリッド表示 "
    '***************************************************************************************************
    '   ＩＤ　：ShowBankSendData
    '   名称　：労金データグリッド表示
    '   概要　：渡されたデータをグリッドに表示します
    '   引数　：
    '   戻り値：なし
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowNetBankGrid(ByVal dtShow As DataTable)

        Dim intBankMoney As Integer = 0
        Dim iCnt As Integer = Me.flxNetbank.Rows.Count
        Dim lngSendSum As Long = 0

        If Me.lblSum.Text <> String.Empty AndAlso ChkNumber(Me.lblSum.Text) = True Then
            lngSendSum = CLng(Me.lblSum.Text)
        End If

        If dtShow.Rows.Count > 0 Then
            For Each userRow As DataRow In dtShow.Rows
                Me.flxNetbank.Rows.Add()
                Me.flxNetbank.SetCellStyle(iCnt, 0, "bool")                                         ' 01. チェックボックス
                Me.flxNetbank.SetData(iCnt, 1, userRow.Item("c_user_id"))                           ' 02. 社員番号
                Me.flxNetbank.SetData(iCnt, 2, userRow.Item("c_dezit"))                             ' 03. CD
                Me.flxNetbank.SetData(iCnt, 3, userRow.Item("l_name"))                              ' 04. 名前
                Me.flxNetbank.SetData(iCnt, 4, userRow.Item("belonging"))                           ' 05. 組合支部
                Me.flxNetbank.SetData(iCnt, 5, userRow.Item("payCutTotal"))                         ' 06. 源泉
                Me.flxNetbank.SetData(iCnt, 6, userRow.Item("payDailyTotal"))                       ' 07. 日当
                Me.flxNetbank.SetData(iCnt, 7, userRow.Item("s_adjust"))                            ' 08. 調整金額

                ' 09. 振込金額
                intBankMoney = Me.flxNetbank.GetData(iCnt, 5) _
                             + Me.flxNetbank.GetData(iCnt, 6) _
                             + Me.flxNetbank.GetData(iCnt, 7)
                Me.flxNetbank.SetData(iCnt, 8, intBankMoney)

                ' 所属委員会の表示
                If _dicUserCommittee.ContainsKey(userRow.Item("c_user_id")) = True Then
                    Me.flxNetbank.SetData(iCnt, 9, _dicUserCommittee(userRow.Item("c_user_id")))    ' 10.所属委員会
                End If
                If _strPayStatusCd <> "03" Then
                    ' 口座情報は現金支払い以外のときのみ表示
                    Me.flxNetbank.SetData(iCnt, 10, userRow.Item("c_bank"))                         ' 11. 金融機関コード
                    Me.flxNetbank.SetData(iCnt, 11, userRow.Item("bank_name"))                      ' 12. 金融機関名
                    Me.flxNetbank.SetData(iCnt, 12, userRow.Item("c_bank_office"))                  ' 13. 支店コード
                    Me.flxNetbank.SetData(iCnt, 13, userRow.Item("bank_office_name"))               ' 14. 支店名
                    Me.flxNetbank.SetData(iCnt, 14, userRow.Item("deposit_name"))                   ' 15. 預金種目
                    Me.flxNetbank.SetData(iCnt, 15, userRow.Item("c_bank_account"))                 ' 16. 口座番号
                    Me.flxNetbank.SetData(iCnt, 16, userRow.Item("l_account_name_kna"))             ' 17. 口座名義
                    Me.flxNetbank.SetData(iCnt, 17, userRow.Item("d_from"))                         ' 18. 口座適用開始日
                    Me.flxNetbank.SetData(iCnt, 18, userRow.Item("deposit_name_om"))                ' 19. 預金種目略称
                End If

                ' 各列のバックカラー変更
                If CanMakeData(iCnt) = False Then
                    ' 支払不可
                    SetNotMakeStyleForRow(iCnt)
                Else
                    If CInt(Me.flxNetbank.GetData(iCnt, 8)) <= 0 Then
                        ' 振込金額がマイナスの場合、デフォルトではチェックしない
                        SetNotMakeStyleForRow(iCnt)
                    Else
                        ' 支払可能
                        Me.flxNetbank.SetData(iCnt, 0, True)
                        SetNotMakeStyleForRow(iCnt)
                        lngSendSum = lngSendSum + Me.flxNetbank.GetData(iCnt, 8)
                    End If
                End If
                iCnt = iCnt + 1
            Next

            ' 振込総額を表示
            Me.lblSum.Text = String.Format("{0:N0}", lngSendSum)

            ' データ件数の更新
            Call Me.SetDataCount()

        End If
    End Sub
#End Region

#Region " 労金データ作成済みユーザーリストの作成 "
    '***************************************************************************************************
    '   ＩＤ　：MakeSendUserList
    '   名称　：労金データ作成済みユーザーリストの作成
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   戻り値：DataTable = 労金データ作成済みユーザーリスト
    '   作成日：2012/02/15(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>労金データ作成済みユーザーリストの作成</summary>
    ''' <returns>労金データ作成済みユーザーリスト</returns>
    ''' <remarks></remarks>
    Private Function MakeSendUserList() As DataTable

        Dim strWhereCut As String = String.Empty
        Dim strWhereOnceCut As String = String.Empty
        Dim strWhereCommittee As String = String.Empty
        Dim strWhereBranch As String = String.Empty
        Dim strWhereExecutive As String = String.Empty
        Dim strWhereDgm As String = String.Empty
        Dim strWhereConst As String = "(({0}) AND cl.k_daily_pay_kind = '{1}') "
        Dim strWhere As StringBuilder = New StringBuilder

        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' 源泉側の締め日からWHERE句作成
            For iCnt As Integer = 0 To Me.dgdPayCutCloseDay.Rows.Count - 1
                If Me.dgdPayCutCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_CUT_CODE Then
                    If strWhereCut = String.Empty Then
                        strWhereCut = "cl.d_pay_close = '" & Me.dgdPayCutCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereCut = strWhereCut & " OR cl.d_pay_close = '" &
                                      Me.dgdPayCutCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                ElseIf Me.dgdPayCutCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_ONCE_CUT_CODE Then
                    If strWhereOnceCut = String.Empty Then
                        strWhereOnceCut = "cl.d_pay_close = '" & Me.dgdPayCutCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereOnceCut = strWhereOnceCut & " OR cl.d_pay_close = '" &
                                      Me.dgdPayCutCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                End If
            Next
            If strWhereCut <> String.Empty Then
                strWhereCut = String.Format(strWhereConst, strWhereCut, DAILY_PAY_KIND_CUT_CODE)
            End If
            If strWhereOnceCut <> String.Empty Then
                strWhereOnceCut = String.Format(strWhereConst, strWhereOnceCut, DAILY_PAY_KIND_ONCE_CUT_CODE)
            End If


            ' 日当側の締め日からWHERE句作成
            For iCnt As Integer = 0 To Me.dgdDayPayCloseDay.Rows.Count - 1
                If Me.dgdDayPayCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_COMMITTEE_CODE Then
                    If strWhereCommittee = String.Empty Then
                        strWhereCommittee = "cl.d_pay_close = '" & Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereCommittee = strWhereCommittee & " OR cl.d_pay_close = '" &
                                      Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                ElseIf Me.dgdDayPayCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_BRANCH_CODE Then
                    If strWhereBranch = String.Empty Then
                        strWhereBranch = "cl.d_pay_close = '" & Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereBranch = strWhereBranch & " OR cl.d_pay_close = '" &
                                      Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                ElseIf Me.dgdDayPayCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_EXECUTIVE_CODE Then
                    If strWhereExecutive = String.Empty Then
                        strWhereExecutive = "cl.d_pay_close = '" & Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereExecutive = strWhereExecutive & " OR cl.d_pay_close = '" &
                                      Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                ElseIf Me.dgdDayPayCloseDay(0, iCnt).Tag = DAILY_PAY_KIND_DGM_CODE Then
                    If strWhereDgm = String.Empty Then
                        strWhereDgm = "cl.d_pay_close = '" & Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    Else
                        strWhereDgm = strWhereDgm & " OR cl.d_pay_close = '" &
                                      Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "") & "' "
                    End If
                End If
            Next
            If strWhereCommittee <> String.Empty Then
                strWhereCommittee = String.Format(strWhereConst, strWhereCommittee, DAILY_PAY_KIND_COMMITTEE_CODE)
            End If
            If strWhereBranch <> String.Empty Then
                strWhereBranch = String.Format(strWhereConst, strWhereBranch, DAILY_PAY_KIND_BRANCH_CODE)
            End If
            If strWhereExecutive <> String.Empty Then
                strWhereExecutive = String.Format(strWhereConst, strWhereExecutive, DAILY_PAY_KIND_EXECUTIVE_CODE)
            End If
            If strWhereDgm <> String.Empty Then
                strWhereDgm = String.Format(strWhereConst, strWhereDgm, DAILY_PAY_KIND_DGM_CODE)
            End If

            If strWhereCut <> String.Empty Then
                strWhere.Append(strWhereCut)
            End If
            If strWhereOnceCut <> String.Empty Then
                If strWhere.Length > 0 Then
                    strWhere.Append(" OR " & strWhereOnceCut)
                Else
                    strWhere.Append(strWhereOnceCut)
                End If
            End If
            If strWhereCommittee <> String.Empty Then
                If strWhere.Length > 0 Then
                    strWhere.Append(" OR " & strWhereCommittee)
                Else
                    strWhere.Append(strWhereCommittee)
                End If
            End If
            If strWhereBranch <> String.Empty Then
                If strWhere.Length > 0 Then
                    strWhere.Append(" OR " & strWhereBranch)
                Else
                    strWhere.Append(strWhereBranch)
                End If
            End If
            If strWhereExecutive <> String.Empty Then
                If strWhere.Length > 0 Then
                    strWhere.Append(" OR " & strWhereExecutive)
                Else
                    strWhere.Append(strWhereExecutive)
                End If
            End If
            If strWhereDgm <> String.Empty Then
                If strWhere.Length > 0 Then
                    strWhere.Append(" OR " & strWhereDgm)
                Else
                    strWhere.Append(strWhereDgm)
                End If
            End If

            Call clsDb.Connect()
            'strSql = "SELECT cl_mem.c_user_id, cl_mem.d_pay_close, cl_mem.k_daily_pay_kind " & _
            '         "FROM staf_bank_close_member AS cl_mem , staf_bank_close AS cl " & _
            '         "WHERE (cl_mem.c_staf_bank_send_id IS NOT Null AND cl_mem.c_staf_bank_send_id <> '') " & _
            '         "AND cl_mem.d_bank_send <> '" & MAX_DATE & "' " & _
            '         "AND cl_mem.d_pay_close = cl.d_pay_close " & _
            '         "AND cl_mem.k_daily_pay_kind = cl.k_daily_pay_kind " & _
            '         "AND cl.k_bank_send_status <> '01' "
            strSql = "SELECT cl_mem.c_user_id, cl_mem.d_pay_close, cl_mem.k_daily_pay_kind " & _
                     "FROM staf_bank_close_member AS cl_mem , staf_bank_close AS cl " & _
                     "WHERE (cl_mem.c_staf_bank_send_id IS NOT Null AND cl_mem.c_staf_bank_send_id <> '') " & _
                     "AND cl_mem.d_bank_send <> '" & MAX_DATE & "' " & _
                     "AND cl_mem.d_pay_close = cl.d_pay_close " & _
                     "AND cl_mem.k_daily_pay_kind = cl.k_daily_pay_kind " & _
                     "AND cl.k_bank_send_status <> '01' " & _
                     "AND (" & strWhere.ToString & ") "

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return dtRet

    End Function

#End Region

#Region " 所属委員会情報取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeData
    '   名称　：所属委員会情報取得
    '   概要　：渡されたユーザーIDが所属している委員会情報を取得します
    '   引数　：ByVal clsDb       As CLAccessMdb = データベースクラス,
    '           ByVal strUserId   As String      = 個人認証ID,
    '   戻り値：
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>所属委員会情報取得</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="strUserId">個人認証ID</param>
    ''' <remarks></remarks>
    Private Sub GetCommitteeData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strUserId As String _
    )

        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty
        Dim strCommittee As StringBuilder = New StringBuilder
        Dim strPayDay As String = Format(Me._datePayDay, "yyyyMMdd")
        Dim strFormat As String = "{0}（{1}）"
        Dim intMaxPay As Integer = 0
        strCommittee.Length = 0

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

            ' SQL
            strSql = "SELECT LIST.c_user_id, MASTER.officer_pay, com_info.com_name, DTL.l_name " & _
                     "FROM (( " & _
                     "       (SELECT t1.l_name AS com_name ,t1.c_committee_id " & _
                     "        FROM committee AS t1, " & _
                     "             (SELECT c_committee_id,MAX(d_from) AS MAX_FROM " & _
                     "              FROM committee " & _
                     "              WHERE d_from < '" & strPayDay & "' " & _
                     "              AND '" & strPayDay & "' <= d_to " & _
                     "              GROUP BY c_committee_id " & _
                     "             )t2 " & _
                     "        WHERE t1.c_committee_id = t2.c_committee_id " & _
                     "        AND t1.d_from = t2.MAX_FROM ) AS com_info INNER JOIN " & _
                     "       (SELECT MST.c_user_id, MST.c_committee_id, MST.s_committee_seq " & _
                     "        FROM committee_list_dtl AS MST,  " & _
                     "             committee_list AS COM, " & _
                     "             (SELECT M.c_committee_id AS max_id, MAX(M.d_from) AS max_d_from " & _
                     "              FROM committee_list_dtl AS M " & _
                     "              WHERE M.d_from <= '" & strPayDay & "' " & _
                     "              GROUP BY M.c_committee_id ) AS MT, " & _
                     "             (SELECT CDtl.c_committee_id, CDtl.s_committee_seq, PSdf.c_period_id " & _
                     "              FROM committee_dtl AS CDtl, period_service_diff AS PSdf " & _
                     "              WHERE CDtl.s_from_diff = PSdf.service_diff " & _
                     "               AND PSdf.service_from <= '" & strPayDay & "' " & _
                     "               AND PSdf.service_to >= '" & strPayDay & "' " & _
                     "               AND CDtl.d_from  <= '" & strPayDay & "' " & _
                     "               AND CDtl.d_to >= '" & strPayDay & "') AS PSD " & _
                     "        WHERE MT.max_id = MST.c_committee_id  " & _
                     "         AND MT.max_d_from = MST.d_from " & _
                     "         AND MST.c_committee_list = COM.c_committee_list " & _
                     "         AND MST.c_committee_id = PSD.c_committee_id " & _
                     "         AND MST.s_committee_seq = PSD.s_committee_seq " & _
                     "         AND COM.c_period_id = PSD.c_period_id " & _
                     "        GROUP BY  MST.c_user_id, MST.c_committee_id, MST.s_committee_seq  " & _
                     "       ) AS LIST  ON LIST.c_committee_id = com_info.c_committee_id ) INNER JOIN " & _
                     "      (SELECT MST.c_committee_id, MST.s_committee_seq, " & _
                     "              MST.c_officer_pay_id, MST.l_name, " & _
                     "              MST.d_service_from, MST.d_service_to " & _
                     "       FROM committee_dtl AS MST, " & _
                     "            (SELECT M.c_committee_id AS max_id, " & _
                     "                    MAX(M.d_from) AS max_d_from " & _
                     "             FROM committee_dtl AS M " & _
                     "             WHERE M.d_from < '" & strPayDay & "' " & _
                     "             AND '" & strPayDay & "' <= M.d_to " & _
                     "             GROUP BY M.c_committee_id " & _
                     "             )  AS MT " & _
                     "       WHERE MT.max_id=MST.c_committee_id " & _
                     "       AND MT.max_d_from=MST.d_from " & _
                     "      ) AS DTL " & _
                     "      ON (LIST.s_committee_seq = DTL.s_committee_seq) " & _
                     "      AND (LIST.c_committee_id = DTL.c_committee_id) " & _
                     "   ) INNER JOIN " & _
                     "(SELECT MST.c_officer_pay_id," & _
                     "        MAX(MST.s_officer_pay) AS officer_pay " & _
                     " FROM officer_pay_master AS MST, " & _
                     "      (SELECT M.c_officer_pay_id AS max_id, " & _
                     "              MAX(M.d_from) AS max_d_from " & _
                     "       FROM officer_pay_master AS M  " & _
                     "       WHERE M.d_from < '" & strPayDay & "' " & _
                     "       AND '" & strPayDay & "' <= M.d_to " & _
                     "       GROUP BY M.c_officer_pay_id " & _
                     "      ) AS MT " & _
                     " WHERE MT.max_id = MST.c_officer_pay_id " & _
                     " AND MT.max_d_from=MST.d_from " & _
                     " GROUP BY MST.c_officer_pay_id " & _
                     " ) AS MASTER ON DTL.c_officer_pay_id = MASTER.c_officer_pay_id " & _
                     "WHERE LIST.c_user_id = '" & strUserId & "' " & _
                     "ORDER BY LIST.c_committee_id "

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                If dtRet.Rows.Count = 1 Then
                    ' 所属委員会が1つのみの場合その委員会名、役職名を取得
                    strCommittee.Append(String.Format(strFormat, dtRet.Rows(0).Item("com_name"), dtRet.Rows(0).Item("l_name")))
                Else
                    ' 所属委員会が複数の場合、手当が最大の委員会・役職を取得
                    For Each dtRow As DataRow In dtRet.Rows
                        If intMaxPay < CInt(dtRow.Item("officer_pay")) Then
                            intMaxPay = CInt(dtRow.Item("officer_pay"))
                        End If
                    Next
                    Dim rowArray As DataRow() = dtRet.Select("officer_pay = " & intMaxPay)
                    strCommittee.Append(String.Format(strFormat, rowArray(0).Item("com_name"), rowArray(0).Item("l_name")))
                End If
                ' 委員会名をディクショナリに格納
                _dicUserCommittee.Add(strUserId, strCommittee.ToString)
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region " 銀行口座情報取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetAccountData
    '   名称　：銀行口座情報取得
    '   概要　：
    '   引数　：ByVal strUserId As String = 個人認証ID
    '   戻り値：DataTable = 銀行口座情報
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>銀行口座情報取得</summary>
    ''' <param name="strUserId">個人認証ID</param>
    ''' <returns>銀行口座情報</returns>
    ''' <remarks></remarks>
    Private Function GetAccountData(ByVal clsDb As CLAccessMdb, ByVal strUserId As String) As DataTable

        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty

        ' 振り込み日
        Dim strPayDay As String = Format(Me._datePayDay, "yyyyMMdd")

        Try
            ' SQL
            strSql = "SELECT acc1.*,bank1.l_bank_name_kna AS bank_name, bankdtl1.l_bank_office_name_kna AS bank_office_name " & _
                     "      ,dtl2.l_name AS deposit_name, dtl2.l_omission_name AS deposit_name_om " & _
                     "FROM staf_account AS acc1, " & _
                     "     (SELECT c_user_id, MAX(d_from) AS now_from " & _
                     "      FROM staf_account " & _
                     "      WHERE d_from <= '" & strPayDay & "' " & _
                     "      GROUP BY c_user_id " & _
                     "     ) AS acc2, " & _
                     "     bank_info AS bank1," & _
                     "     bank_info_dtl AS bankdtl1," & _
                     "     constant_dtl AS dtl2 " & _
                     "WHERE acc1.c_user_id = acc2.c_user_id " & _
                     "AND acc1.d_from = acc2.now_from " & _
                     "AND acc1.c_bank = bank1.c_bank " & _
                     "AND acc1.c_bank_office = bankdtl1.c_bank_office " & _
                     "AND bank1.c_bank = bankdtl1.c_bank " & _
                     "AND bank1.d_from <= '" & strPayDay & "' AND bank1.d_to >= '" & strPayDay & "' " & _
                     "AND bankdtl1.d_from <= '" & strPayDay & "' AND bankdtl1.d_to >= '" & strPayDay & "' " & _
                     "AND dtl2.c_constant = 'DEPOSIT_ITEMS' AND dtl2.c_constant_seq = acc1.k_deposit_items " & _
                     "AND acc1.c_user_id = '" & strUserId & "' "

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )
        End Try

        ' 戻り値設定
        Return dtRet

    End Function
#End Region

#Region " 基本情報・口座情報取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetAttributeAccountData
    '   名称　：基本情報・銀行口座情報取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetAttributeAccountData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strUserId As String _
    ) As DataTable

        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty

        ' 振り込み日
        Dim strPayDay As String = Format(Me._datePayDay, "yyyyMMdd")

        Try
            'ユーザーIDをキーに基本情報・口座情報を取得する
            strSql = "SELECT staf.c_user_id, staf.c_dezit,staf.l_name, staf.belonging, account.c_bank ,account.bank_name " & _
                     "      ,account.c_bank_office, account.bank_office_name, account.deposit_name, account.d_from " & _
                     "      ,account.c_bank_account, account.l_account_name_kna, account.deposit_name_om " & _
                     "FROM " & _
                     "     (SELECT attr1.c_user_id AS c_user_id, attr1.l_name AS l_name " & _
                     "            ,attr1.c_dezit AS c_dezit, dtl1.l_name AS belonging " & _
                     "      FROM staf_attribute AS attr1, " & _
                     "           (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from  " & _
                     "            FROM staf_attribute  " & _
                     "            WHERE d_from <= '" & strPayDay & "' " & _
                     "            GROUP BY c_user_id, c_ksh, c_staf_id " & _
                     "           ) AS attr2, " & _
                     "           constant_dtl AS dtl1 " & _
                     "      WHERE attr1.c_user_id = attr2.c_user_id " & _
                     "      AND attr1.c_ksh = attr2.c_ksh " & _
                     "      AND attr1.c_staf_id = attr2.c_staf_id " & _
                     "      AND attr1.d_from = attr2.now_from " & _
                     "      AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr1.k_belonging " & _
                     "     ) AS staf LEFT JOIN " & _
                     "     (SELECT acc1.*,bank1.l_bank_name_kna AS bank_name, bankdtl1.l_bank_office_name_kna AS bank_office_name " & _
                     "            ,dtl2.l_name AS deposit_name, dtl2.l_omission_name AS deposit_name_om " & _
                     "      FROM staf_account AS acc1, " & _
                     "           (SELECT c_user_id, MAX(d_from) AS now_from " & _
                     "            FROM staf_account " & _
                     "            WHERE d_from <= '" & strPayDay & "' " & _
                     "            GROUP BY c_user_id " & _
                     "           ) AS acc2, " & _
                     "           bank_info AS bank1," & _
                     "           bank_info_dtl AS bankdtl1," & _
                     "           constant_dtl AS dtl2 " & _
                     "      WHERE acc1.c_user_id = acc2.c_user_id " & _
                     "      AND acc1.d_from = acc2.now_from " & _
                     "      AND acc1.c_bank = bank1.c_bank " & _
                     "      AND acc1.c_bank_office = bankdtl1.c_bank_office " & _
                     "      AND bank1.c_bank = bankdtl1.c_bank " & _
                     "      AND bank1.d_from <= '" & strPayDay & "' AND bank1.d_to >= '" & strPayDay & "'" & _
                     "      AND bankdtl1.d_from <= '" & strPayDay & "' AND bankdtl1.d_to >= '" & strPayDay & "'" & _
                     "      AND dtl2.c_constant = 'DEPOSIT_ITEMS' AND dtl2.c_constant_seq = acc1.k_deposit_items " & _
                     "     ) AS account " & _
                     "     ON staf.c_user_id = account.c_user_id " & _
                     "WHERE staf.c_user_id = '" & strUserId & "' "

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dtRet

    End Function
#End Region

#Region " 労金データ取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankSend
    '   名称　：労金データ取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetStafBankSend(ByVal clsDb As CLAccessMdb) As DataTable

        Dim dtStafBankSend As DataTable = Nothing
        Dim strSql As String = String.Empty

        Try
            'STAF_BANK_SENDからデータ取得
            strSql = "SELECT c_staf_bank_send_id, d_ins, d_up, " & _
                     "       d_bank_send, k_bank_send_margin, l_bank_send_item, " & _
                     "       c_pay_time_kind, c_pay_time_cut, c_daily_pay_kind, c_daily_pay_close " & _
                     "FROM staf_bank_send " & _
                     "WHERE c_staf_bank_send_id = '" & _strStafBankSendId & "' " & _
                     "AND d_bank_send = '" & Format(Me._datePayDay, "yyyyMMdd") & "' "
            dtStafBankSend = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dtStafBankSend

    End Function
#End Region

#Region " 労金データメンバー取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankSendMember
    '   名称　：労金データメンバー取得取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '         ：2012/08/15(水) Fujisaku 参照時にはステータスチェックを行わない
    '***************************************************************************************************
    Private Function GetStafBankSendMember(ByVal clsDb As CLAccessMdb) As DataTable

        Dim dtStafBankSendMember As DataTable = Nothing
        Dim strSql As String = String.Empty

        ' 現在日付
        Dim strNow As String = Now.ToString("yyyyMMdd")

        ' 振り込み日
        Dim strPayDay As String = Format(Me._datePayDay, "yyyyMMdd")

        Try
            ' SQL
            strSql = "SELECT bank_mem.c_user_id, bank_mem.s_bank_pay, bank_mem.s_adjust " & _
                     "      ,bank_mem.s_pay_cut_total AS payCutTotal, bank_mem.s_daily_pay_total AS payDailyTotal " & _
                     "      ,bank_mem.d_from_account AS d_from,staf.c_dezit,staf.l_name, staf.belonging, account.c_bank ,account.bank_name " & _
                     "      ,account.c_bank_office, account.bank_office_name, account.deposit_name ,account.d_from " & _
                     "      ,account.c_bank_account,account.l_account_name_kna, account.deposit_name_om " & _
                     "  FROM ((staf_bank_send_member AS bank_mem INNER JOIN" & _
                     "     staf_bank_send AS bank ON bank_mem.c_staf_bank_send_id = bank.c_staf_bank_send_id " & _
                     "     AND  bank_mem.d_bank_send = bank.d_bank_send) INNER JOIN " & _
                     "     (SELECT attr1.c_user_id AS c_user_id, attr1.l_name AS l_name " & _
                     "            ,attr1.c_dezit AS c_dezit, dtl1.l_name AS belonging " & _
                     "      FROM staf_attribute AS attr1, " & _
                     "           (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from  " & _
                     "            FROM staf_attribute  " & _
                     "            WHERE d_from <= '" & strNow & "' " & _
                     "            GROUP BY c_user_id, c_ksh, c_staf_id " & _
                     "           ) AS attr2, " & _
                     "           constant_dtl AS dtl1 " & _
                     "      WHERE attr1.c_user_id = attr2.c_user_id " & _
                     "      AND attr1.c_ksh = attr2.c_ksh " & _
                     "      AND attr1.c_staf_id = attr2.c_staf_id " & _
                     "      AND attr1.d_from = attr2.now_from " & _
                     "      AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr1.k_belonging " & _
                     "     ) AS staf ON bank_mem.c_user_id = staf.c_user_id) LEFT JOIN " & _
                     "     (SELECT acc1.*,bank1.l_bank_name_kna AS bank_name, bankdtl1.l_bank_office_name_kna AS bank_office_name " & _
                     "            ,dtl2.l_name AS deposit_name, dtl2.l_omission_name AS deposit_name_om " & _
                     "      FROM staf_account AS acc1, " & _
                     "           (SELECT c_user_id, MAX(d_from) AS now_from " & _
                     "            FROM staf_account " & _
                     "            WHERE d_from <= '" & strNow & "' " & _
                     "            GROUP BY c_user_id " & _
                     "           ) AS acc2, " & _
                     "           bank_info AS bank1," & _
                     "           bank_info_dtl AS bankdtl1," & _
                     "           constant_dtl AS dtl2 " & _
                     "      WHERE acc1.c_user_id = acc2.c_user_id " & _
                     "      AND acc1.d_from = acc2.now_from " & _
                     "      AND acc1.c_bank = bank1.c_bank " & _
                     "      AND acc1.c_bank_office = bankdtl1.c_bank_office " & _
                     "      AND bank1.c_bank = bankdtl1.c_bank " & _
                     "      AND bank1.d_from <= '" & strPayDay & "' AND bank1.d_to >= '" & strPayDay & "'" & _
                     "      AND bankdtl1.d_from <= '" & strPayDay & "' AND bankdtl1.d_to >= '" & strPayDay & "'" & _
                     "      AND dtl2.c_constant = 'DEPOSIT_ITEMS' AND dtl2.c_constant_seq = acc1.k_deposit_items " & _
                     "     ) AS account " & _
                     "ON bank_mem.c_user_id = account.c_user_id " & _
                     "WHERE bank_mem.c_staf_bank_send_id = '" & _strStafBankSendId & "' " & _
                     "AND bank_mem.d_bank_send = '" & strPayDay & "' " & _
                     "ORDER BY CLng(bank_mem.c_user_id) "

            ' SQL実行
            dtStafBankSendMember = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dtStafBankSendMember

    End Function

#End Region

#Region " 締め日データ取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankCloseData
    '   名称　：締め日データ取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetStafBankCloseData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strWhere As String _
    ) As DataTable

        Dim dtStafBankClose As DataTable = Nothing
        Dim strSql As String = String.Empty

        Try
            ' SQL
            strSql = ""
            strSql += "SELECT k_bank_send_status"
            strSql += "      ,d_ins"
            strSql += "      ,d_up"
            strSql += "      ,d_pay_close"
            strSql += "      ,k_daily_pay_kind"
            strSql += "  FROM staf_bank_close "
            strSql += strWhere

            ' SQL実行
            dtStafBankClose = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dtStafBankClose

    End Function
#End Region

#Region " 締め日メンバーデータ取得 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankCloseMember
    '   名称　：締め日メンバーデータ取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetStafBankCloseMember( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strWhere As String _
    ) As DataTable

        Dim dtStafBankCloseMember As DataTable = Nothing
        Dim strSql As String = String.Empty

        Try
            ' SQL
            strSql = ""
            strSql += "SELECT d_pay_close"
            strSql += "      ,k_daily_pay_kind"
            strSql += "      ,c_user_id"
            strSql += "      ,d_bank_send"
            strSql += "      ,c_staf_bank_send_id"
            strSql += "      ,s_pay"
            strSql += "  FROM staf_bank_close_member "
            strSql += strWhere

            ' SQL実行
            dtStafBankCloseMember = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dtStafBankCloseMember

    End Function
#End Region

#Region " 印刷プレビュー画面表示 "
    '***************************************************************************************************
    '   ＩＤ　：ShowPrintPreview
    '   名称　：印刷プレビュー画面表示
    '   概要　：
    '   引数　：ByVal blnPrintOnly As Boolean  = 印刷フラグ,
    '           ByVal fmPreview    As FM000205 = 印刷プレビュー画面
    '   戻り値：Integer = 印刷プレビュー画面押下ボタン
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>印刷プレビュー画面表示</summary>
    ''' <param name="blnPrintOnly">印刷フラグ</param>
    ''' <param name="fmPreview">印刷プレビュー画面</param>
    ''' <returns>印刷プレビュー画面押下ボタン</returns>
    ''' <remarks></remarks>
    Private Function ShowPrintPreview( _
        ByVal blnPrintOnly As Boolean, _
        ByVal fmPreview As FM000205 _
    ) As Integer

        ' プレビュー画面でのクリックボタン判別用
        Dim intBtn As Integer = 0
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0801P1 = New DS0801P1

        Try
            ' 表示ボタンタイプを設定
            If blnPrintOnly = True Then
                ' 印刷、キャンセルボタン
                fmPreview.ButtonShowType = 2
            Else
                ' 印刷＆登録、登録のみ、キャンセルボタン
                fmPreview.ButtonShowType = 1
            End If

            ' 印刷範囲指定欄の表示
            fmPreview.PrintAreaVisible = True
            reportObj = New CR0801P1
            fmPreview.ObjResource = reportObj

            ' 詳細部分
            Dim drDetail As DS0801P1.dtDetailRow

            ' ヘッダー
            Dim drHeader As DS0801P1.dtHeaderRow = ds.dtHeader.NewRow

            drHeader.BeginEdit()
            ' 振込日
            drHeader.d_year = CDate(Me.txtPayDay.Text).Year
            drHeader.d_month = CDate(Me.txtPayDay.Text).Month.ToString.PadLeft(2, "0")
            drHeader.d_day = CDate(Me.txtPayDay.Text).Day.ToString.PadLeft(2, "0")
            ' 支払方法
            drHeader.l_paid_kind = Me.txtBankSendMargin.Text
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader) ' ヘッダー情報格納

            ' チェックされているユーザーを詳細部分に追加
            For intCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
                If Me.flxNetbank.GetData(intCnt, 0) = True Then
                    drDetail = ds.dtDetail.NewRow
                    drDetail.BeginEdit()
                    ' 金融機関CD
                    drDetail.c_bank = Me.flxNetbank.GetData(intCnt, 10)
                    ' 金融機関名
                    drDetail.l_bank_name_kna = Me.flxNetbank.GetData(intCnt, 11)
                    ' 支店番号
                    drDetail.c_bank_office = Me.flxNetbank.GetData(intCnt, 12)
                    ' 支店名
                    drDetail.l_name_office = Me.flxNetbank.GetData(intCnt, 13)
                    '預金種目
                    If Me._strPayStatusCd <> "03" AndAlso String.IsNullOrEmpty(Me.flxNetbank.GetData(intCnt, 14)) = False Then
                        drDetail.k_deposit_items = Me.flxNetbank.GetData(intCnt, 14) & "(" & Me.flxNetbank.GetData(intCnt, 18) & ")"
                    End If
                    ' 口座番号
                    drDetail.c_bank_account = Me.flxNetbank.GetData(intCnt, 15)
                    ' 口座名義
                    drDetail.l_account_name_kna = Me.flxNetbank.GetData(intCnt, 16)
                    ' 振込金額
                    drDetail.s_bank_pay = Me.flxNetbank.GetData(intCnt, 8)
                    ' 顧客コード（社員番号）
                    drDetail.l_customer_code_1 = Me.flxNetbank.GetData(intCnt, 1)
                    ' 手数料区分
                    If Me._strPayStatusCd <> "03" Then
                        drDetail.l_fee_division = Me._strFeeDivision
                    End If

                    drDetail.EndEdit()
                    ds.dtDetail.Rows.Add(drDetail) ' 詳細情報格納
                End If
            Next

            ' データソースセット
            reportObj.SetDataSource(ds)
            fmPreview.ShowDialog()

            intBtn = fmPreview.IntQlickBtnFlag

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' クリックボタンフラグの返却
        Return intBtn

    End Function
#End Region

#Region " 表示件数更新 "
    '***************************************************************************************************
    '   ＩＤ　：SetDataCount
    '   名称　：表示件数更新
    '   概要　：データ作成対象の件数をセットします
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>表示件数更新</summary>
    ''' <remarks></remarks>
    Private Sub SetDataCount()

        Try
            Me.grpNetBank.Text = String.Format(DATA_COUNT, (Me.flxNetbank.Rows.Count - 1).ToString)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
    End Sub
#End Region

#Region " 手数料区分取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：SetFeeDivision
    '   名称　：手数料区分取得処理
    '   概要　：手数料区分をDBから取得しメンバ変数へ格納します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>手数料区分取得処理</summary>
    ''' <remarks></remarks>
    Private Sub SetFeeDivision()

        Dim dtRet As DataTable = Nothing                ' 処理結果データテーブル
        Dim strSql As String = String.Empty             ' SQL
        Dim clsDb As CLAccessMdb = New CLAccessMdb      ' データベースクラス

        Try
            'SQL
            strSql = ""
            strSql += "SELECT l_name"
            strSql += "      ,l_omission_name"
            strSql += "  FROM constant_dtl"
            strSql += " WHERE c_constant        = 'OA_BANK_CHARGES'"
            strSql += "   AND l_omission_name_2 = '○' "

            ' データベース接続処理
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 件数確認
            If dtRet.Rows.Count > 0 Then
                ' クリスタルレポート用
                Me._strFeeDivision = dtRet.Rows(0).Item("l_name") & "(" & dtRet.Rows(0).Item("l_omission_name") & ")"
                ' CSVファイル用
                Me._strFeeDivisionCsv = dtRet.Rows(0).Item("l_omission_name")
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' データベース切断処理
            Call clsDb.Disconnect()

        End Try
    End Sub

#End Region

#Region " データ登録処理 "
    '***************************************************************************************************
    '   ＩＤ　：EntryData
    '   名称　：データ登録処理
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>データ登録処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function EntryData() As Boolean

        Try
            If _intSelectBtn = 0 Then
                ' 新規振込データ登録処理
                Return Me.NewEntry()
            Else
                ' 振込データ更新処理
                Return Me.UpdateEntry()
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Function
#End Region

#Region " 振込データ新規登録 "
    '***************************************************************************************************
    '   ＩＤ　：NewEntry
    '   名称　：振込データ新規登録
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ新規登録</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function NewEntry() As Boolean

        Dim clsDb As CLAccessMdb = New CLAccessMdb      ' データベースクラス
        Dim blnNoError = False                          ' 登録処理結果
        Dim blnRet As Boolean = False                   ' 返却用

        Try
            ' STAF_BANK_SEND用データ取得
            Dim sendData As StafSendData = CreateStafSendData()
            Dim sendMemberList As List(Of StafSendDataMember) = CreateStafSendmemberData(sendData.strStafBankSendId)
            Dim closeData As List(Of StafBankClose) = Nothing
            Dim closeDataMemberList As List(Of StafBankCloseMember) = Nothing
            ' 締め日データが選択済みの場合はStaf_Bank_Close側のデータ登録も行う
            If Me._blnIsFreeEntry = False Then
                closeData = CreateStafBankClose()
                closeDataMemberList = ChkExistStafBankCloseMemberDataEntry(sendData.strStafBankSendId)
            End If

            Call clsDb.Connect()        ' DB接続開始
            Call clsDb.BeginTran()      ' トランザクション開始

            ' staf_bank_sendへデータ登録
            If InsertStafBankSendData(clsDb, sendData) = False Then
                clsDb.RollbackTran()
                Return False
            End If
            ' staf_bank_send_memberへデータ登録
            If InsertStafBankSendMemberData(clsDb, sendMemberList) = False Then
                clsDb.RollbackTran()
                Return False
            End If

            If Me._blnIsFreeEntry = False Then
                ' 締め日データが選択済みの場合はStaf_Bank_Close側のデータ登録も行う
                For Each targetData As StafBankClose In closeData
                    ' 同締め日・締め日種別のデータが既に登録済みかチェック
                    If ChkExistsStafBankClose(clsDb, targetData.strDatePayClose, targetData.strDailyPayKind) = True Then
                        ' 同締め日・締め日種別のデータがない場合Insert実行
                        If InsertStafBankCloseData(clsDb, targetData) = False Then
                            clsDb.RollbackTran()
                            Return False
                        End If
                    Else
                        ' 存在する場合はUpdateを実行
                        If UpdateStafBankCloseData(clsDb, targetData) = False Then
                            clsDb.RollbackTran()
                            Return False
                        End If
                        ' 締め日メンバーデータの削除
                        If DeleteStafBankCloseMemberData(clsDb, targetData.strDatePayClose, targetData.strDailyPayKind) = False Then
                            clsDb.RollbackTran()
                            Return False
                        End If
                    End If
                Next

                ' 締め日メンバーデータの登録
                If InsertStafBankCloseMemberData(clsDb, closeDataMemberList) = False Then
                    clsDb.RollbackTran()
                    Return False
                End If

                ' 締め日データの振込状況更新
                If UpdateStafBankCloseDataStatus(clsDb, closeData) = False Then
                    clsDb.RollbackTran()
                    Return False
                End If
            End If

            ' 問題なく終了した場合正常終了フラグをたてる
            blnNoError = True

            If blnNoError = True Then
                Call clsDb.CommitTran()
                blnRet = True
            Else
                Call clsDb.RollbackTran()
            End If

        Catch ex As Exception
            Call clsDb.RollbackTran()

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 参照振込データの更新 "
    '***************************************************************************************************
    '   ＩＤ　：UpdateEntry
    '   名称　：参照振込データの更新
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function UpdateEntry() As Boolean

        Dim blnRet As Boolean = False               ' 返却用正常フラグ
        Dim blnNoError As Boolean = False           ' 各SQL正常終了フラグ
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        ' STAF_BANK_SEND用データ取得
        Dim sendData As StafSendData = CreateStafSendData()
        Dim sendMemberList As List(Of StafSendDataMember) = CreateStafSendmemberData(sendData.strStafBankSendId)
        Dim closeData As List(Of StafBankClose) = Nothing
        Dim closeDataMemberList As List(Of StafBankCloseMember) = Nothing

        Try
            ' 締日未選択の振込データでない場合、締め日データ側も更新する
            If Me._blnIsFreeEntry = False Then
                closeData = CreateStafBankClose()
                closeDataMemberList = ChkExistStafBankCloseMemberDataEntry(sendData.strStafBankSendId)
            End If

            ' DB接続開始
            clsDb.Connect()

            ' トランザクション開始
            clsDb.BeginTran()

            ' staf_bank_send更新 TODO:タイムスタンプチェック
            If UpdateStafBankSend(clsDb, sendData) = False Then
                clsDb.RollbackTran()
                Return False
            End If

            ' staf_bank_send_memberからいったんデータを削除
            If DeleteStafBankSendMemberData(clsDb) = False Then
                clsDb.RollbackTran()
                Return False
            End If
            ' staf_bank_send_memberに再度データ登録
            If InsertStafBankSendMemberData(clsDb, sendMemberList) = False Then
                clsDb.RollbackTran()
                Return False
            End If

            If Me._blnIsFreeEntry = False Then
                ' staf_bank_closeの更新 TODO:タイムスタンプチェック
                For Each targetData As StafBankClose In closeData
                    If UpdateStafBankCloseData(clsDb, targetData) = False Then
                        clsDb.RollbackTran()
                        Return False
                    End If
                    ' 締め日メンバーデータの削除
                    If DeleteStafBankCloseMemberData(clsDb, targetData.strDatePayClose, targetData.strDailyPayKind) = False Then
                        clsDb.RollbackTran()
                        Return False
                    End If
                Next

                ' 締め日メンバーデータの登録
                If InsertStafBankCloseMemberData(clsDb, closeDataMemberList) = False Then
                    clsDb.RollbackTran()
                    Return False
                End If
            End If

            If closeData IsNot Nothing Then
                ' 締め日データの振込状況更新
                If UpdateStafBankCloseDataStatus(clsDb, closeData) = False Then
                    clsDb.RollbackTran()
                    Return False
                End If
            End If
            ' 問題なく終了した場合正常終了フラグをたてる
            blnNoError = True

            If blnNoError = True Then
                Call clsDb.CommitTran()
                blnRet = True
            Else
                Call clsDb.RollbackTran()
            End If

        Catch ex As Exception
            Call clsDb.RollbackTran()
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

#End Region

#Region " 振込データID返却 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankSendId
    '   名称　：振込データID返却
    '   概要　：
    '   引数　：なし
    '   戻り値：String = 振込データID
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データID返却</summary>
    ''' <returns>振込データID</returns>
    ''' <remarks></remarks>
    Private Function GetStafBankSendId() As String

        Dim intId As Integer = 0
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        Try
            ' SQL
            strSql = ""
            strSql += "SELECT MAX(CInt(c_staf_bank_send_id)) AS MAX_ID"
            strSql += "  FROM staf_bank_send"

            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows(0).Item("MAX_ID").Equals(DBNull.Value) = False Then
                intId = CInt(dtRet.Rows(0).Item("MAX_ID"))
            End If
            intId = intId + 1

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return intId.ToString

    End Function
#End Region

#Region " 振込データ登録処理 "
    '***************************************************************************************************
    '   ＩＤ　：InsertStafBankSendData
    '   名称　：振込データ登録処理
    '   概要　：
    '   引数　：ByVal clsDb      As CLAccessMdb  = データベース,
    '           ByVal insertData As StafSendData = 振込データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ登録処理</summary>
    ''' <param name="clsDb">データベース</param>
    ''' <param name="insertData">登録データ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafBankSendData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal insertData As StafSendData _
    ) As Boolean

        Dim blnRet As Boolean = False
        Dim intRet As Integer = -1
        Dim strSql As String = String.Empty

        Try
            ' SQL
            strSql = "INSERT INTO staf_bank_send (" & _
                     "c_staf_bank_send_id, d_bank_send, k_bank_send_margin " & _
                     ",l_bank_send_item, c_pay_time_kind, c_pay_time_cut " & _
                     ",c_daily_pay_kind, c_daily_pay_close, d_ins " & _
                     ",c_user_id_ins, d_up, c_user_id_up, s_up" & _
                     ") VALUES (" & _
                     "'" & insertData.strStafBankSendId & "' " & _
                     ",'" & insertData.dateBankSend & "' " & _
                     ",'" & insertData.strBankSendMargin & "' " & _
                     ",'" & insertData.strBankSendItem & "' " & _
                     ",'" & insertData.strPayTimeKind & "' " & _
                     ",'" & insertData.strPayTimeCut & "' " & _
                     ",'" & insertData.strDailyPaykind & "' " & _
                     ",'" & insertData.strDailyPayClose & "' " & _
                     "," & insertData.strDateIns & " " & _
                     ",'" & insertData.strUserIdIns & "' " & _
                     "," & insertData.strDateUpdate & " " & _
                     ",'" & insertData.strUserIdUpdate & "' " & _
                     "," & insertData.intUp & ") "

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)
            If intRet <> 1 Then
                'Call clsDb.RollbackTran()
            Else
                blnRet = True
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 振込データ更新処理 "
    '***************************************************************************************************
    '   ＩＤ　：UpdateStafBankSend
    '   名称　：振込データ更新処理
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/20(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function UpdateStafBankSend( _
        ByVal clsDb As CLAccessMdb, _
        ByVal upDateData As StafSendData _
    ) As Boolean

        Dim blnRet As Boolean = False
        Dim intRet As Integer = -1
        Dim strSql As String = String.Empty

        Try
            ' SQL
            strSql = "UPDATE staf_bank_send " & _
                     "   SET d_up         = " & upDateData.strDateUpdate & " " & _
                     "      ,c_user_id_up = '" & upDateData.strUserIdUpdate & "' " & _
                     "      ,s_up         = s_up + 1 " & _
                     " WHERE c_staf_bank_send_id = '" & Me._strStafBankSendId & "' " & _
                     "   AND d_bank_send         = '" & Format(Me._datePayDay, "yyyyMMdd") & "' "

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet <> 1 Then
                Return blnRet
            Else
                ' 正常に更新できた場合Trueを返却
                blnRet = True
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        Return blnRet

    End Function

#End Region

#Region " 振込データ対象者登録処理 "
    '***************************************************************************************************
    '   ＩＤ　：InsertStafBankSendMemberData
    '   名称　：振込データ対象者登録処理
    '   概要　：
    '   引数　：ByVal clsDb          As CLAccessMdb                 = データベースクラス,
    '           ByVal insertDataList As List(Of StafSendDataMember) = 振込登録データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ対象者登録処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="insertDataList">振込登録データ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafBankSendMemberData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal insertDataList As List(Of StafSendDataMember) _
    ) As Boolean

        Dim blnRet As Boolean = False
        Dim intRet As Integer = -1
        Dim strSql As String = String.Empty

        Try
            For Each iData As StafSendDataMember In insertDataList
                ' SQL
                strSql = "INSERT INTO staf_bank_send_member " & _
                         "(c_staf_bank_send_id, d_bank_send, c_user_id " & _
                         ",s_bank_pay, s_adjust, s_pay_cut_total " & _
                         ",s_daily_pay_total, d_from_account, d_ins " & _
                         ",c_user_id_ins) " & _
                         "VALUES " & _
                         "('" & iData.strStafBankSendId & "' " & _
                         ",'" & iData.strDateBankSend & "' " & _
                         ",'" & iData.strUserId & "' " & _
                         "," & iData.intBankPay & _
                         "," & iData.intAdjust & _
                         "," & iData.intPayCutTotal & _
                         "," & iData.intDailyPayTotal & _
                         ",'" & iData.strDateFromAccount & "' " & _
                         "," & iData.strDateIns & _
                         ",'" & iData.strUserIdIns & "') "
                ' SQL実行
                intRet = clsDb.ExecuteNonQuery(strSql)
                If intRet <> 1 Then
                    ' 処理結果が１件以外の場合処理終了
                    'clsDb.RollbackTran()
                    Return blnRet
                End If
            Next

            ' 全て問題なく登録できた場合Trueを返却
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        Return blnRet

    End Function
#End Region

#Region " 振込データ対象者削除処理 "
    '***************************************************************************************************
    '   ＩＤ　：DeleteStafBankSendMember
    '   名称　：振込データ対象者削除処理
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/20(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function DeleteStafBankSendMemberData(ByVal clsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False
        Dim intRet As Integer = -1
        Dim strSql As String = String.Empty

        Try
            ' 振込ID、振込日が紐づく振込対象データを削除
            strSql = "DELETE " & _
                     "  FROM staf_bank_send_member " & _
                     " WHERE c_staf_bank_send_id = '" & Me._strStafBankSendId & "' " & _
                     "   AND d_bank_send         = '" & Format(Me._datePayDay, "yyyyMMdd") & "' "

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet < 1 Then
                Return blnRet
            Else
                '１件以上削除した場合はTrueを返却
                blnRet = True
            End If

        Catch ex As Exception

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

#End Region

#Region " 同締め日データ存在チェック "
    '***************************************************************************************************
    '   ＩＤ　：ChkExistsStafBankClose
    '   名称　：同締め日データ存在チェック
    '   概要　：
    '   引数　：
    '   戻り値：True = 同締め日データなし、False = 同締め日データあり
    '   作成日：2012/02/20(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkExistsStafBankClose( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strDayPayClose As String, _
        ByVal strDailyPayKind As String _
    ) As Boolean

        Dim blnRet As Boolean = False
        Dim dtRet As DataTable = Nothing

        Try
            ' SQL
            Dim strSql As String = "SELECT d_ins" & _
                                   "      ,d_up" & _
                                   "  FROM staf_bank_close" & _
                                   " WHERE d_pay_close      = '" & strDayPayClose & "' " & _
                                   "   AND k_daily_pay_kind = '" & strDailyPayKind & "' "
            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows.Count = 0 Then
                blnRet = True
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日データ登録処理 "
    '***************************************************************************************************
    '   ＩＤ　：InsertStafBankCloseData
    '   名称　：締め日データ登録処理
    '   概要　：
    '   引数　：ByVal clsDb     As CLAccessMdb            = データベースクラス,
    '           ByVal iDataList As List(Of StafBankClose) = 締め日データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>締め日データ登録処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="iData">締め日データ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafBankCloseData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal iData As StafBankClose _
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim intRet As Integer = -1              ' SQL実行結果
        Dim strSql As String = String.Empty     ' SQL

        Try
            ' SQL
            strSql = "INSERT INTO staf_bank_close (" & _
                         "d_pay_close, k_daily_pay_kind, k_bank_send_status " & _
                         ",d_ins, c_user_id_ins, d_up, c_user_id_up, s_up" & _
                         ") VALUES (" & _
                         "'" & iData.strDatePayClose & "' " & _
                         ",'" & iData.strDailyPayKind & "' " & _
                         ",'" & iData.strBankSendStatus & "' " & _
                         "," & iData.strDateIns & _
                         ",'" & iData.strUserIdIns & "' " & _
                         "," & iData.strDateUpdate & _
                         ",'" & iData.strUserIdUpdate & "' " & _
                         "," & iData.intUpCount & ") "

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet <> 1 Then
                ' Call clsDb.RollbackTran()
                Return blnRet
            End If

            ' 全て問題なく処理できた場合、Trueを返却
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日データ更新処理 "
    '***************************************************************************************************
    '   ＩＤ　：UpdateStafBankCloseData
    '   名称　：締め日データ更新処理
    '   概要　：
    '   引数　：ByVal clsDb As CLAccessMdb   = データベースクラス,
    '           ByVal uData As StafBankClose = 締め日データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function UpdateStafBankCloseData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal uData As StafBankClose _
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim intRet As Integer = -1              ' SQL実行結果
        Dim strSql As String = String.Empty     ' SQL

        Try
            ' SQL
            strSql = "UPDATE staf_bank_close " & _
                     "   SET d_up         = " & uData.strDateUpdate & " " & _
                     "      ,c_user_id_up = '" & uData.strUserIdUpdate & "' " & _
                     "      ,s_up         = s_up + 1 " & _
                     " WHERE d_pay_close      = '" & uData.strDatePayClose & "' " & _
                     "   AND k_daily_pay_kind = '" & uData.strDailyPayKind & "' "

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet <> 1 Then
                'Call clsDb.RollbackTran()
                Return blnRet
            End If

            ' 問題なく処理できた場合、Trueを返却
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日データ更新振込状況処理 "
    '***************************************************************************************************
    '   ＩＤ　：UpdateStafBankCloseDataStatus
    '   名称　：締め日データ更新振込状況処理
    '   概要　：
    '   引数　：ByVal clsDb As CLAccessMdb   = データベースクラス,
    '           ByVal uData As StafBankClose = 締め日データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/20(月)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function UpdateStafBankCloseDataStatus( _
        ByVal clsDb As CLAccessMdb, _
        ByVal uDataList As List(Of StafBankClose) _
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim intRet As Integer = -1              ' SQL実行結果
        Dim strSql As String = String.Empty     ' SQL
        Dim strSelectSql As String = String.Empty
        Dim dtNoSend As DataTable = Nothing
        Dim dtHasSend As DataTable = Nothing
        Dim intNoSend As Integer = -1
        Dim intHasSend As Integer = -1
        Dim strNewStatus As String = String.Empty

        Try
            For Each uData As StafBankClose In uDataList
                ' 振込データ未作成のデータ取得
                strSelectSql = "SELECT COUNT(c_user_id)" & _
                               "  FROM staf_bank_close_member" & _
                               " WHERE d_pay_close      = '" & uData.strDatePayClose & "'" & _
                               "   AND k_daily_pay_kind = '" & uData.strDailyPayKind & "'" & _
                               "   AND d_bank_send      = '" & MAX_DATE & "'"

                ' SQL実行
                dtNoSend = clsDb.ExecuteSql(strSelectSql)

                If dtNoSend.Rows(0).Item(0).Equals(DBNull.Value) = False Then
                    intNoSend = dtNoSend.Rows(0).Item(0)
                End If

                If intNoSend <> 0 Then
                    '振込データ作成済みのデータ取得
                    strSelectSql = "SELECT COUNT(c_user_id)" & _
                                   "  FROM staf_bank_close_member" & _
                                   " WHERE d_pay_close      = '" & uData.strDatePayClose & "'" & _
                                   "   AND k_daily_pay_kind = '" & uData.strDailyPayKind & "'"

                    ' SQL実行
                    dtHasSend = clsDb.ExecuteSql(strSelectSql)

                    If dtHasSend.Rows(0).Item(0).Equals(DBNull.Value) = False Then
                        intHasSend = dtHasSend.Rows(0).Item(0)
                    End If

                    If intNoSend = intHasSend Then
                        '全ユーザー振込データ未作成
                        strNewStatus = "03"
                    Else
                        '一部ユーザー振込済み
                        strNewStatus = "02"
                    End If
                Else
                    '全ユーザー振込済み
                    strNewStatus = "01"
                End If

                ' SQL
                strSql = "UPDATE staf_bank_close" & _
                         "   SET k_bank_send_status = '" & strNewStatus & "'" & _
                         " WHERE d_pay_close      = '" & uData.strDatePayClose & "'" & _
                         "   AND k_daily_pay_kind = '" & uData.strDailyPayKind & "'"

                ' SQL実行
                intRet = clsDb.ExecuteNonQuery(strSql)

                If intRet <> 1 Then
                    'Call clsDb.RollbackTran()
                    Return blnRet
                End If
            Next

            ' 問題なく処理できた場合、Trueを返却
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）

            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日メンバーデータ登録処理 "
    '***************************************************************************************************
    '   ＩＤ　：InsertStafBankCloseMemberData
    '   名称　：締め日メンバーデータ登録処理
    '   概要　：
    '   引数　：ByVal clsDb     As CLAccessMdb                  = データベースクラス,
    '           ByVal iDataList As List(Of StafBankCloseMember) = 締め日メンバーデータ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>締め日メンバーデータ登録処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="iDataList">締め日メンバーデータ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertStafBankCloseMemberData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal iDataList As List(Of StafBankCloseMember) _
    ) As Boolean

        Dim blnRet As Boolean = False
        Dim intRet As Integer = -1
        Dim strSql As String = String.Empty

        Try
            For Each iData As StafBankCloseMember In iDataList
                ' SQL
                strSql = "INSERT INTO staf_bank_close_member " & _
                         "(d_pay_close, k_daily_pay_kind, c_user_id " & _
                         ",c_staf_bank_send_id, d_bank_send, s_pay " & _
                         ",d_ins, c_user_id_ins, d_up " & _
                         ",c_user_id_up, s_up" & _
                         ") VALUES (" & _
                         "'" & iData.strDatePayClose & "'" & _
                         ",'" & iData.strDailyPayKind & "'" & _
                         ",'" & iData.strUserId & "'" & _
                         ",'" & iData.strStafBankSendId & "'" & _
                         ",'" & iData.strDateBankSend & "'" & _
                         ",'" & iData.intPay & "'" & _
                         "," & iData.strDateIns & _
                         ",'" & iData.strUserIdIns & "'" & _
                         "," & iData.strDateUpdate & _
                         ",'" & iData.strUserIdUpdate & "'" & _
                         "," & iData.intUpCount & ")"

                ' SQL実行
                intRet = clsDb.ExecuteNonQuery(strSql)
                If intRet <> 1 Then
                    'Call clsDb.RollbackTran()
                    Return blnRet
                End If
            Next
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日メンバーデータ削除 "
    '***************************************************************************************************
    '   ＩＤ　：DeleteStafBankCloseMemberData
    '   名称　：締め日メンバーデータ削除
    '   概要　：
    '   引数　：ByVal clsDb           As CLAccessMdb = データベースクラス, 
    '           ByVal strDayPayClose  As String      = 締め日,
    '           ByVal strDailyPayKind As String      = 日当計算区分
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/10(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/10(金)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>締め日メンバーデータ削除</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <param name="strDayPayClose">締め日</param>
    ''' <param name="strDailyPayKind">日当計算区分</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteStafBankCloseMemberData( _
        ByVal clsDb As CLAccessMdb, _
        ByVal strDayPayClose As String, _
        ByVal strDailyPayKind As String _
    ) As Boolean

        Dim intRet As Integer = -1
        Dim blnRet As Boolean = False
        Dim strSql As String = String.Empty
        Try
            ' 対象締め日・締め日種別のデータを削除
            strSql = "DELETE " & _
                     "  FROM staf_bank_close_member" & _
                     " WHERE d_pay_close      = '" & strDayPayClose & "'" & _
                     "   AND k_daily_pay_kind = '" & strDailyPayKind & "'"

            If Me._intSelectBtn = 0 Then
                ' 新規作成の場合、振込日付が未設定になっているもののみ削除
                strSql = strSql & "   AND d_bank_send = '" & MAX_DATE & "'"
            End If

            ' SQL実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet > -1 Then
                blnRet = True
            End If

        Catch ex As Exception
            ' 例外が起きた場合はロールバック
            Call clsDb.RollbackTran()

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 振込データ作成 "
    '***************************************************************************************************
    '   ＩＤ　：CreateStafSendData
    '   名称　：振込データ作成
    '   概要　：
    '   引数　：なし
    '   戻り値：StafSendData = 振込データ対象者
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ作成</summary>
    ''' <returns>振込データ対象者</returns>
    ''' <remarks></remarks>
    Private Function CreateStafSendData() As StafSendData

        Dim sendData As StafSendData = New StafSendData
        Dim strId As String = String.Empty

        Try
            If _intSelectBtn = 1 OrElse _intSelectBtn = 2 Then
                sendData.strStafBankSendId = Me._strStafBankSendId  ' 振込ID
                sendData.strDateUpdate = "'" & _NowDate & "'"       ' 更新日
                sendData.strUserIdUpdate = MDLoginInfo.UserId       ' 更新者
            Else
                ' 新規振込データIDの取得
                strId = Me.GetStafBankSendId()

                Dim sbCutPayKindList As StringBuilder = New StringBuilder       ' 源泉側の締日種別リスト用
                Dim sbCutPayCloseDayList As StringBuilder = New StringBuilder   ' 源泉側の締め日リスト用
                Dim sbDailyPayKindList As StringBuilder = New StringBuilder     ' 日当側の締日種別リスト用
                Dim sbDailyPayCloseDayList As StringBuilder = New StringBuilder ' 日当側の締め日リスト用

                ' 賃金カット役員手当対象締め日種別登録用、賃金カット役員手当対象締め日登録用の文字列作成
                If Me.dgdPayCutCloseDay.Rows.Count > 0 Then
                    For iCnt As Integer = 0 To Me.dgdPayCutCloseDay.Rows.Count - 1
                        ' 賃金カット役員手当対象締め日種別登録用
                        If sbCutPayKindList.Length = 0 Then
                            sbCutPayKindList.Append(Me.dgdPayCutCloseDay(0, iCnt).Tag)
                        Else
                            sbCutPayKindList.Append("," & Me.dgdPayCutCloseDay(0, iCnt).Tag)
                        End If
                        ' 賃金カット役員手当対象締め日登録用
                        If sbCutPayCloseDayList.Length = 0 Then
                            sbCutPayCloseDayList.Append(Me.dgdPayCutCloseDay(1, iCnt).Value)
                        Else
                            sbCutPayCloseDayList.Append("," & Me.dgdPayCutCloseDay(1, iCnt).Value)
                        End If
                    Next
                End If

                ' 日当対象締め日種別登録用、日当対象締め日登録用の文字列作成
                If Me.dgdDayPayCloseDay.Rows.Count > 0 Then
                    For iCnt As Integer = 0 To Me.dgdDayPayCloseDay.Rows.Count - 1
                        ' 日当対象締め日種別登録用
                        If sbDailyPayKindList.Length = 0 Then
                            sbDailyPayKindList.Append(Me.dgdDayPayCloseDay(0, iCnt).Tag)
                        Else
                            sbDailyPayKindList.Append("," & Me.dgdDayPayCloseDay(0, iCnt).Tag)
                        End If
                        ' 日当対象締め日登録用
                        If sbDailyPayCloseDayList.Length = 0 Then
                            sbDailyPayCloseDayList.Append(Me.dgdDayPayCloseDay(1, iCnt).Value)
                        Else
                            sbDailyPayCloseDayList.Append("," & Me.dgdDayPayCloseDay(1, iCnt).Value)
                        End If
                    Next
                End If

                sendData.strStafBankSendId = strId                              ' 振込番号ID
                sendData.dateBankSend = Me.txtPayDay.Text.Replace("/", "").Replace("-", "")      ' 振込日付
                sendData.strBankSendMargin = Me._strPayStatusCd                 ' 支払方法種別
                sendData.strBankSendItem = Me.txtTitle.Text                     ' 振込題目

                If _blnIsFreeEntry = False Then
                    sendData.strPayTimeKind = sbCutPayKindList.ToString         ' 賃金カット役員手当対象締め日種別
                    sendData.strPayTimeCut = sbCutPayCloseDayList.ToString      ' 賃金カット役員手当対象締め日
                    sendData.strDailyPaykind = sbDailyPayKindList.ToString      ' 日当対象締め日種別
                    sendData.strDailyPayClose = sbDailyPayCloseDayList.ToString ' 日当対象締め日
                Else
                    ' 対象締め日を選択していない場合、空文字を設定
                    sendData.strPayTimeKind = String.Empty
                    sendData.strPayTimeCut = String.Empty
                    sendData.strDailyPaykind = String.Empty
                    sendData.strDailyPayClose = String.Empty
                End If

                sendData.strDateIns = "'" & _NowDate & "'"                      ' 作成日
                sendData.strUserIdIns = MDLoginInfo.UserId                      ' 作成者
                sendData.strDateUpdate = "Null"                                 ' 更新日
                sendData.strUserIdUpdate = String.Empty                         ' 更新者
                sendData.intUp = 0                                              ' 更新回数
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        Return sendData

    End Function
#End Region

#Region " 振込データ対象者作成 "
    '***************************************************************************************************
    '   ＩＤ　：CreateStafSendmemberData
    '   名称　：振込データ対象者作成
    '   概要　：
    '   引数　：ByVal strSendId As String = 振込ID,
    '   戻り値：List(Of StafSendDataMember) = 振込データ対象者
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ対象者作成</summary>
    ''' <param name="strSendId">振込ID</param>
    ''' <returns>振込データ対象者</returns>
    ''' <remarks></remarks>
    Private Function CreateStafSendmemberData(ByVal strSendId As String) As List(Of StafSendDataMember)

        Dim dataList As List(Of StafSendDataMember) = New List(Of StafSendDataMember)
        Dim sendMemberData As StafSendDataMember = Nothing

        Try
            For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
                If Me.flxNetbank.GetData(iCnt, 0) = True Then

                    sendMemberData = New StafSendDataMember
                    sendMemberData.strStafBankSendId = strSendId                            ' 振込ID
                    sendMemberData.strDateBankSend = Me.txtPayDay.Text.Replace("/", "").Replace("-", "")     ' 振込日付
                    sendMemberData.strUserId = Me.flxNetbank.GetData(iCnt, 1)               ' 社員番号
                    sendMemberData.intBankPay = Me.flxNetbank.GetData(iCnt, 8)              ' 振込金額
                    sendMemberData.intAdjust = Me.flxNetbank.GetData(iCnt, 7)               ' 調整金額
                    sendMemberData.intPayCutTotal = Me.flxNetbank.GetData(iCnt, 5)          ' 源泉合計額
                    sendMemberData.intDailyPayTotal = Me.flxNetbank.GetData(iCnt, 6)        ' 日当合計額

                    ' 口座適用開始年月日
                    If Me._strPayStatusCd = "03" Then
                        sendMemberData.strDateFromAccount = String.Empty
                    Else
                        sendMemberData.strDateFromAccount = Me.flxNetbank.GetData(iCnt, 17)
                    End If


                    sendMemberData.strDateIns = "'" & _NowDate & "'"                        ' 作成日
                    sendMemberData.strUserIdIns = MDLoginInfo.UserId                        ' 作成者
                    dataList.Add(sendMemberData)
                End If
            Next

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dataList

    End Function
#End Region

#Region " 締め日データ作成 "
    '***************************************************************************************************
    '   ＩＤ　：CreateStafBankClose
    '   名称　：締め日データ作成
    '   概要　：
    '   引数　：なし
    '   戻り値：List(Of StafBankClose) = 
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>締め日データ作成</summary>
    ''' <returns>締め日データ</returns>
    ''' <remarks></remarks>
    Private Function CreateStafBankClose() As List(Of StafBankClose)

        Dim closeDataList As List(Of StafBankClose) = New List(Of StafBankClose)
        Dim closeData As StafBankClose = Nothing

        Try
            For iCnt As Integer = 0 To Me.dgdPayCutCloseDay.Rows.Count - 1
                closeData = New StafBankClose
                closeData.strDatePayClose = Me.dgdPayCutCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "")   ' 締め日
                closeData.strDailyPayKind = Me.dgdPayCutCloseDay(0, iCnt).Tag                               ' 締め日種別
                closeData.strBankSendStatus = "01"                                                          ' 振込完了区分
                closeData.strDateIns = "'" & _NowDate & "'"                                                 ' 作成日
                closeData.strUserIdIns = MDLoginInfo.UserId                                                 ' 作成者
                closeData.strDateUpdate = "Null"                                                            ' 更新日
                closeData.strUserIdUpdate = String.Empty                                                    ' 更新者
                closeData.intUpCount = 0                                                                    ' 更新回数
                closeDataList.Add(closeData)
            Next

            For iCnt As Integer = 0 To Me.dgdDayPayCloseDay.Rows.Count - 1
                closeData = New StafBankClose
                closeData.strDatePayClose = Me.dgdDayPayCloseDay(1, iCnt).Value.ToString.Replace("/", "").Replace("-", "")   ' 締め日
                closeData.strDailyPayKind = Me.dgdDayPayCloseDay(0, iCnt).Tag                               ' 締め日種別
                closeData.strBankSendStatus = "01"                                                          ' 振込完了区分
                closeData.strDateIns = "'" & _NowDate & "'"                                                 ' 作成日
                closeData.strUserIdIns = MDLoginInfo.UserId                                                 ' 作成者
                closeData.strDateUpdate = "'" & _NowDate & "'"                                              ' 更新日
                closeData.strUserIdUpdate = MDLoginInfo.UserId                                              ' 更新者
                closeData.intUpCount = 0                                                                    ' 更新回数
                closeDataList.Add(closeData)

            Next
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return closeDataList

    End Function
#End Region

#Region " 締め日ユーザーデータチェック "
    '***************************************************************************************************
    '   ＩＤ　：ChkExistStafBankCloseMemberDataEntry
    '   名称　：締め日ユーザーデータチェック
    '   概要　：
    '   引数　：ByRef CloseDataList As List(Of StafBankClose) = 
    '   戻り値：List(Of StafBankCloseMember) = 
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>締め日ユーザーデータチェック</summary>
    ''' <param name="strSendId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ChkExistStafBankCloseMemberDataEntry(ByVal strSendId As String) As List(Of StafBankCloseMember)

        Dim dataList As List(Of StafBankCloseMember) = New List(Of StafBankCloseMember)
        Dim closeMemberData As StafBankCloseMember = Nothing
        Dim strNoCheckList As List(Of String) = New List(Of String)

        ' チェックなしユーザーリストの作成
        For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
            If Me.flxNetbank.GetData(iCnt, 0) = False Then
                If strNoCheckList.Contains(Me.flxNetbank.GetData(iCnt, 1)) = False Then
                    strNoCheckList.Add(Me.flxNetbank.GetData(iCnt, 1))
                End If
            End If
        Next

        For Each userList As List(Of String) In Me._AllUserList
            If strNoCheckList.Contains(userList(2)) = True Then
                ' チェックがない場合、振込IDを空欄、振込日付を"99999999"とする
                closeMemberData = CreateStafBankCloseMemberData(String.Empty, MAX_DATE, userList)
            Else
                ' チェックされている場合は振込ID、振込日を設定する
                If userList(4) <> String.Empty Then
                    closeMemberData = CreateStafBankCloseMemberData(userList(4), userList(5), userList)
                Else
                    closeMemberData = CreateStafBankCloseMemberData(strSendId, Format(_datePayDay, "yyyyMMdd"), userList)
                End If
            End If
            dataList.Add(closeMemberData)
        Next

        ' 戻り値設定
        Return dataList

    End Function
#End Region

#Region " 締め日ユーザーデータ作成 "
    '***************************************************************************************************
    '   ＩＤ　：CreateStafBankCloseMemberData
    '   名称　：
    '   概要　：
    '   引数　：ByVal strDayPayClose  As String  = ,
    '           ByVal strDailyPayKind As String  = 日当計算区分,
    '           ByVal strUserId       As String  = 個人認証ID,
    '           ByVal intKindPay      As Integer = ,
    '   戻り値：StafBankCloseMember = 
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="strSendId">振込ID</param>
    ''' <param name="strSendDay">振込日</param>
    ''' <param name="strList">更新データ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateStafBankCloseMemberData( _
        ByVal strSendId As String, _
        ByVal strSendDay As String, _
        ByVal strList As List(Of String) _
    ) As StafBankCloseMember

        Dim closeMemberData As StafBankCloseMember = New StafBankCloseMember

        Try
            closeMemberData.strDatePayClose = strList(0).Replace("/", "").Replace("-", "").Substring(0, 6)   ' 締め日
            closeMemberData.strDailyPayKind = strList(1)                                    ' 締め日種別
            closeMemberData.strUserId = strList(2)                                          ' ユーザーID
            closeMemberData.strStafBankSendId = strSendId                                   ' 振込ID
            closeMemberData.strDateBankSend = strSendDay                                    ' 振込日付
            closeMemberData.intPay = CInt(strList(3))                                       ' 対象種別金額
            closeMemberData.strDateIns = "'" & _NowDate & "'"                               ' 作成日
            closeMemberData.strUserIdIns = MDLoginInfo.UserId                               ' 作成者
            closeMemberData.strDateUpdate = "Null"                                          ' 更新日
            closeMemberData.strUserIdUpdate = String.Empty                                  ' 更新者
            closeMemberData.intUpCount = 0                                                  ' 更新回数

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return closeMemberData

    End Function
#End Region

#Region " 全行チェック "
    '***************************************************************************************************
    '   ＩＤ　：AllCheck
    '   名称　：全行チェック
    '   概要　：
    '   引数　：ByVal isCheck As Boolean = 
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>全行チェック    ''' </summary>
    ''' <param name="isCheck"></param>
    ''' <remarks></remarks>
    Private Sub AllCheck(ByVal isCheck As Boolean)

        Dim i As Integer

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            For i = 1 To Me.flxNetbank.Rows.Count - 1
                If Me.CanMakeData(i) Then
                    Me.flxNetbank.SetData(i, 0, isCheck)
                    Me.SetNotMakeStyleForRow(i)
                End If
            Next i
            ' 振込合計金額を再計算
            Call Me.CalcTotal()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' カーソルを矢印に戻す
            Me.Cursor = Cursors.Default

        End Try

    End Sub
#End Region

#Region " 全行バックカラー変更 "
    '***************************************************************************************************
    '   ＩＤ　：SetNotMakeStyle
    '   名称　：全行バックカラー変更
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>全行バックカラー変更</summary>
    ''' <remarks></remarks>
    Private Sub SetNotMakeStyle()

        Dim i As Integer

        Try
            For i = 1 To Me.flxNetbank.Rows.Count - 1
                Me.SetNotMakeStyleForRow(i)
            Next i

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " セルのバックカラー変更 "
    '***************************************************************************************************
    '   ＩＤ　：SetNotMakeStyleForRow
    '   名称　：セルのバックカラー変更
    '   概要　：
    '   引数　：ByVal iRowIndex As Integer = ローインデックス
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>セルのバックカラー変更</summary>
    ''' <param name="iRowIndex">ローインデックス</param>
    ''' <remarks></remarks>
    Private Sub SetNotMakeStyleForRow(ByVal iRowIndex As Integer)

        Try
            If CBool(Me.flxNetbank.Item(iRowIndex, 0)) Then
                Me.flxNetbank.SetCellStyle(iRowIndex, 5, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 6, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 7, "CanEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 8, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 9, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 10, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 11, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 12, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 13, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 14, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 15, "DisableEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 16, "DisableEdit")
            Else
                Me.flxNetbank.SetCellStyle(iRowIndex, 5, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 6, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 7, "CanEdit")
                Me.flxNetbank.SetCellStyle(iRowIndex, 8, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 9, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 10, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 11, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 12, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 13, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 14, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 15, "NotCheck")
                Me.flxNetbank.SetCellStyle(iRowIndex, 16, "NotCheck")
                'range.Style = Me.flxNetbank.Styles.Item("NotCheck")
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " チェック可能列か返却する "
    '***************************************************************************************************
    '   ＩＤ　：CanMakeData
    '   名称　：チェック可能列か返却する
    '   概要　：
    '   引数　：ByVal iRowIndex As Integer = ローインデックス
    '   戻り値：True = チェック可能, False = チェック不可能
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>チェック可能列か返却する</summary>
    ''' <param name="iRowIndex">ローインデックス</param>
    ''' <returns>True = チェック可能, False = チェック不可能</returns>
    ''' <remarks></remarks>
    Private Function CanMakeData(ByVal iRowIndex As Integer) As Boolean

        Dim flag As Boolean = True

        Try
            If Me._strPayStatusCd.Equals("03") Then
                Return True
            End If
            If ChkNull(Me.flxNetbank.GetData(iRowIndex, 10).ToString) = True _
            AndAlso ChkNull(Me.flxNetbank.GetData(iRowIndex, 11).ToString) = True _
            AndAlso ChkNull(Me.flxNetbank.GetData(iRowIndex, 12).ToString) = True _
            AndAlso ChkNull(Me.flxNetbank.GetData(iRowIndex, 13).ToString) = True Then
                flag = False
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return flag

    End Function
#End Region

#Region " マイナスの振込額チェック "
    '***************************************************************************************************
    '   ＩＤ　：CheckBankPayValue
    '   名称　：マイナスの振込額チェック
    '   概要　：チェックされた振込データのうち、振込金額がマイナスのものが存在するかチェックする
    '   引数　：なし
    '   戻り値：True = 存在する, False = 存在しない
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>マイナスの振込額チェック</summary>
    ''' <returns>True = 存在する, False = 存在しない</returns>
    ''' <remarks></remarks>
    Private Function CheckBankPayValue() As Boolean

        Dim blnRet As Boolean = True

        Try
            Dim i As Integer
            For i = 1 To Me.flxNetbank.Rows.Count - 1
                If CBool(Me.flxNetbank.Item(i, 0)) Then
                    If (CInt(Me.flxNetbank.Item(i, 8)) <= 0) Then
                        CLMsg.Show("GE0152", Me.flxNetbank.Item(i, 3).ToString)
                        blnRet = False
                        Return blnRet
                    End If
                End If
            Next i

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 締め日データ取得条件文字列作成 "
    '***************************************************************************************************
    '   ＩＤ　：GetStafBankCloseData
    '   名称　：締め日データ取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetWhereStringForStafBankClose( _
        ByVal strPayTimeKind As String, _
        ByVal strPayTimeCut As String, _
        ByVal strDailyPayKind As String, _
        ByVal strDailyPayClose As String _
    ) As String

        Dim str As String = String.Empty

        Try
            Dim builder As New StringBuilder(" where (")
            builder.Append(Me.GetWhereStringFromCloseDayAndKind(strPayTimeCut, strPayTimeKind))
            builder.Append(Me.GetWhereStringFromCloseDayAndKind(strDailyPayClose, strDailyPayKind))
            builder.Remove(builder.ToString.LastIndexOf("or"), 2)
            builder.Append(" ) ")
            str = builder.ToString

        Catch ex As Exception

        End Try

        ' 戻り値設定
        Return str

    End Function
#End Region

#Region " 振込データ締め日・締め日種別をSQL用に生成 "
    '***************************************************************************************************
    '   ＩＤ　：GetWhereStringFromCloseDayAndKind
    '   名称　：締め日データ取得
    '   概要　：
    '   引数　：
    '   戻り値：
    '   作成日：2012/02/17(金)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetWhereStringFromCloseDayAndKind( _
        ByVal strCloseDay As String, _
        ByVal strCloseKind As String _
        ) As String

        Dim str As String = String.Empty

        Try
            If String.IsNullOrEmpty(strCloseDay) Then
                Return ""
            End If
            Dim builder As New StringBuilder
            Dim strArray As String() = strCloseDay.Replace("/", "").Replace("-", "").Split(New Char() {","c})
            Dim strArray2 As String() = Nothing
            If Not String.IsNullOrEmpty(strCloseKind) Then
                strArray2 = strCloseKind.Replace("/", "").Replace("-", "").Split(New Char() {","c})
            End If
            Dim i As Integer
            For i = 0 To strArray.Length - 1
                builder.Append(String.Concat(New String() {" (d_pay_close = '", strArray(i), "' and  k_daily_pay_kind = '", If(String.IsNullOrEmpty(strCloseKind), "04", strArray2(i)), "' ) or "}))
            Next i
            str = builder.ToString

        Catch ex As Exception

        End Try

        ' 戻り値設定
        Return str

    End Function
#End Region

#Region " 振込合計金額計算処理 "
    '***************************************************************************************************
    '   ＩＤ　：CalcTotal
    '   名称　：振込合計金額計算処理
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込合計金額計算処理</summary>
    ''' <remarks></remarks>
    Private Sub CalcTotal()

        Dim num As Long = 0
        Dim i As Integer

        Try
            For i = 1 To Me.flxNetbank.Rows.Count - 1
                If (CBool(Me.flxNetbank.Item(i, 0)) _
                AndAlso ChkNull(Me.flxNetbank.GetData(i, 8)) = False) Then
                    num = (num + Convert.ToInt64(Me.flxNetbank.Item(i, 8)))
                End If
            Next i
            Me.lblSum.Text = String.Format("{0:N0}", num)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " 調整金額チェック "
    '***************************************************************************************************
    '   ＩＤ　：ReCallculation
    '   名称　：調整金額チェック
    '   概要　：調整金額に不正な値が含まれていないかチェックします
    '   引数　：ByVal iRowIndex As Integer = ローインデックス
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>調整金額チェック</summary>
    ''' <param name="iRowIndex">ローインデックス</param>
    ''' <remarks></remarks>
    Private Sub ReCallculation(ByVal iRowIndex As Integer)

        Dim num As Long = 0

        Try
            If (Not Me.flxNetbank.Editor Is Nothing) Then
                If Not Integer.TryParse(Me.flxNetbank.Editor.Text.Replace(",", ""), num) Then
                    If String.IsNullOrEmpty(Me.flxNetbank.Editor.Text) Then
                        CLMsg.Show("GE0022", "調整金額")
                        Exit Sub
                    Else
                        CLMsg.Show("GE0178")
                    End If
                    Me.flxNetbank.FinishEditing(True)
                    Return
                End If
            Else
                num = Convert.ToInt64(Me.flxNetbank.Item(iRowIndex, 7))
            End If
            Dim num2 As Long = CLng(((Convert.ToInt32(Me.flxNetbank.Item(iRowIndex, 5)) + Convert.ToInt32(Me.flxNetbank.Item(iRowIndex, 6))) + num))
            Me.flxNetbank.Item(iRowIndex, 8) = num2
            Me.CalcTotal()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " 振込データ選択チェック "
    '***************************************************************************************************
    '   ＩＤ　：CheckHasData
    '   名称　：振込データ選択チェック
    '   概要　：作成対象データが１件以上選択されているかチェックします
    '   引数　：なし
    '   戻り値：True = 選択有り, False = 選択無し
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>振込データ選択チェック</summary>
    ''' <returns>True = 選択有り, False = 選択無し</returns>
    ''' <remarks></remarks>
    Private Function CheckHasData() As Boolean

        Dim blnRet = True

        Try
            If (Me.flxNetbank.FindRow(True, 1, 0, False) < 0) Then
                CLMsg.Show("GE0151")
                blnRet = False
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 社員番号重複チェック "
    '***************************************************************************************************
    '   ＩＤ　：CheckDuplexStafId
    '   名称　：社員番号重複チェック
    '   概要　：表示されている振込データ中に重複した社員番号が存在するかチェックします
    '   引数　：なし
    '   戻り値：True = 存在する, False = 存在しない
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>社員番号重複チェック</summary>
    ''' <returns>True = 存在する, False = 存在しない</returns>
    ''' <remarks></remarks>
    Private Function CheckDuplexStafId() As Boolean

        Dim list As New List(Of String)
        Dim builder As New StringBuilder
        Dim blnRet As Boolean = True

        Try
            For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
                If list.Contains(Me.flxNetbank.GetData(iCnt, 1)) = False Then
                    list.Add(Me.flxNetbank.GetData(iCnt, 1))
                Else
                    ' 重複社員番号がある場合、異常フラグをたてループを抜ける
                    blnRet = False
                    Exit For
                End If
            Next

            If blnRet = False Then
                If CLMsg.Show("GW0034") = DialogResult.No Then
                    Return blnRet
                Else
                    ' はいが選ばれたら処理続行のため正常終了フラグを返却
                    blnRet = True
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 更新対象データのタイムスタンプチェック "
    '***************************************************************************************************
    '   ＩＤ　：ChkTimeStamp
    '   名称　：更新対象データのタイムスタンプチェック
    '   概要　：
    '   引数　：
    '   戻り値：True=更新可能、False = 更新日不一致のため更新不可
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkTimeStamp( _
        ByVal dtTarget As DataTable, _
        ByVal strUpdate As String _
    ) As Boolean

        Dim blnRet As Boolean = False
        If dtTarget.Rows(0).Item("d_up").ToString.Equals(strUpdate) = True Then
            blnRet = True
        End If

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " CSVファイル出力処理 "
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：CSVファイル出力処理
    '   概要　：労金送信用のCSVファイルを出力します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>CSVファイル出力処理</summary>
    ''' <remarks></remarks>
    Private Sub SaveCsvFile()


        Dim dtOutputData As DataTable = Nothing             ' CSV出力データ
        Dim sfd As New System.Windows.Forms.SaveFileDialog  ' ファイル保存ダイアログボックス
        Dim strFileName As String = ""                      ' 初期表示のファイル名

        Try
            ' 初期表示のファイル名取得（振込日（yyyy-MM-dd形式）+ "_" + 題目 + ".csv"）
            ' ※ 題目に禁止文字があった場合、削除
            strFileName = Format(Me._datePayDay, "yyyy-MM-dd") & "_" & Regex.Replace(Me._strTitle, "[\\/:*?\""<>|]", "") & ".csv"

            ' ファイル保存ダイアログボックス設定
            sfd.Title = SAVE_DIALOG_TITLE                   ' タイトル
            sfd.Filter = "CSVファイル(*.csv)|*.csv"         ' フィルタ
            sfd.FileName = strFileName                      ' 初期表示のファイル名

            ' ダイアログボックス表示
            If sfd.ShowDialog = DialogResult.OK Then
                '---------------------------------------------------
                '   OKボタン押下時
                '---------------------------------------------------
                ' CSV出力データ作成処理
                dtOutputData = Me.SetCsvData()

                ' CSVファイル出力処理
                If CsvPut(dtOutputData, sfd.FileName, , , False) = True Then
                    ' 正常時
                    CLMsg.Show("GI0028")
                Else
                    ' 異常時
                    CLMsg.Show("BE0022", sfd.FileName)
                End If
            Else
                '---------------------------------------------------
                '   キャンセルボタン押下時
                '---------------------------------------------------
                ' TODO:メッセージ出す？
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
    End Sub
#End Region

#Region " CSV出力データ作成処理 "
    '***************************************************************************************************
    '   ＩＤ　：SetCsvData
    '   名称　：CSV出力データ作成処理
    '   概要　：CSV出力データを作成し返却します
    '   引数　：なし
    '   戻り値：
    '   作成日：2021/09/30(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/09/30(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetCsvData() As DataTable

        Dim clsDb As CLAccessMdb = New CLAccessMdb                  ' データベースクラス
        Dim tbl As DataTable = Nothing                              ' CSV出力用データテーブル
        Dim row As DataRow = Nothing                                ' CSV出力用データロー

        Dim tblFee As DataTable = Nothing                           ' 手数料データテーブル
        Dim tblAccount As DataTable = Nothing                       ' 振込元データテーブル

        Dim intBankcnt As Integer = 0                               ' 振込件数
        Dim intBankSum As Integer = 0                               ' 振込総額
        Dim intBankMoney As Integer = 0                             ' 振込金額
        Dim strBankNo As String = ""                                ' 振込銀行番号
        Dim strBelongCommittee As String = ""                       ' 所属委員会（役職）
        Dim intFee As Integer = 0                                   ' 手数料
        Dim intPay As Integer = 0                                   ' 差引額（振込金額 - 手数料）

        Dim intDepositType As Integer = 0                           ' 預金種別

        Try
            ' データベース接続処理
            clsDb.Connect()

            ' CSV出力用データテーブル作成
            tbl = New DataTable("csvData")
            tbl.Columns.Add(CsvCol.Col_A)                                       ' 01. A列
            tbl.Columns.Add(CsvCol.Col_B)                                       ' 02. B列
            tbl.Columns.Add(CsvCol.Col_C)                                       ' 03. C列
            tbl.Columns.Add(CsvCol.Col_D)                                       ' 04. D列
            tbl.Columns.Add(CsvCol.Col_E)                                       ' 05. E列
            tbl.Columns.Add(CsvCol.Col_F)                                       ' 06. F列
            tbl.Columns.Add(CsvCol.Col_G)                                       ' 07. G列
            tbl.Columns.Add(CsvCol.Col_H)                                       ' 08. H列
            tbl.Columns.Add(CsvCol.Col_I)                                       ' 09. I列
            tbl.Columns.Add(CsvCol.Col_J)                                       ' 10. J列
            tbl.Columns.Add(CsvCol.Col_K)                                       ' 11. K列
            tbl.Columns.Add(CsvCol.Col_L)                                       ' 12. L列
            tbl.Columns.Add(CsvCol.Col_M)                                       ' 13. M列
            tbl.Columns.Add(CsvCol.Col_N)                                       ' 14. N列
            tbl.Columns.Add(CsvCol.Col_O)                                       ' 15. O列
            tbl.Columns.Add(CsvCol.Col_P)                                       ' 16. P列
            tbl.Columns.Add(CsvCol.Col_Q)                                       ' 17. Q列
            tbl.Columns.Add(CsvCol.Col_R)                                       ' 18. R列

            '-------------------------------------------------------------------------------------------
            '   ヘッダーレコード
            '-------------------------------------------------------------------------------------------
            ' 振込元データ取得処理
            tblAccount = SetBankTransferAccount(clsDb)

            ' データロー作成
            row = tbl.NewRow

            row(CsvCol.Col_A) = "1"                                             ' 01. レコード種別（"1"：固定）
            row(CsvCol.Col_B) = "21"                                            ' 02. "21"：固定
            row(CsvCol.Col_C) = "0"                                             ' 03. "0"：固定
            row(CsvCol.Col_D) = tblAccount.Rows(0).Item(0).ToString()           ' 04. 労金契約番号
            row(CsvCol.Col_E) = tblAccount.Rows(0).Item(6).ToString()           ' 05. 口座名義カナ
            ' 2023/06/17 労金データ締め日の日2桁対応 m.suzuki Update Start
            row(CsvCol.Col_F) = CDate(Me.txtPayDay.Text).ToString("Mdd")        ' 06. 振込日：例）06月01日 ⇒ 601
            'row(CsvCol.Col_F) = CDate(Me.txtPayDay.Text).ToString("Md")         ' 06. 振込日：例）05月28日 ⇒ 528
            ' 2023/06/17 労金データ締め日の日2桁対応 m.suzuki Update End
            row(CsvCol.Col_G) = tblAccount.Rows(0).Item(1).ToString()           ' 07. 金融機関コード
            row(CsvCol.Col_H) = tblAccount.Rows(0).Item(8).ToString()           ' 08. 銀行名カナ
            row(CsvCol.Col_I) = tblAccount.Rows(0).Item(2).ToString()           ' 09. 支店番号
            row(CsvCol.Col_J) = tblAccount.Rows(0).Item(10).ToString()          ' 10. 支店名カナ

            ' 11. 預金種目（前0を数値型に変換して削除）
            intDepositType = CInt(tblAccount.Rows(0).Item(3))
            row(CsvCol.Col_K) = intDepositType.ToString()                       ' 11. 預金種目
            row(CsvCol.Col_L) = tblAccount.Rows(0).Item(4).ToString()           ' 12. 口座番号
            row(CsvCol.Col_M) = DBNull.Value                                    ' 13. 無し
            row(CsvCol.Col_N) = DBNull.Value                                    ' 14. 無し
            row(CsvCol.Col_O) = DBNull.Value                                    ' 15. 無し
            row(CsvCol.Col_P) = DBNull.Value                                    ' 16. 無し
            row(CsvCol.Col_Q) = DBNull.Value                                    ' 17. 無し
            row(CsvCol.Col_R) = Me.flxNetbank.Rows(0).Item(9)                   ' 18. 所属委員会（役職）の文字列（ヘッダー名）

            ' データロー追加
            tbl.Rows.Add(row)

            '---------------------------------------------------------------------------------------
            '   データレコード
            '---------------------------------------------------------------------------------------
            ' 振込手数料データ取得処理
            tblFee = SetBankTransferFee(clsDb)

            ' データ件数分ループ
            For i As Integer = 1 To Me.flxNetbank.Rows.Count - 1 Step 1

                ' チェックされている行のみ出力する
                If Me.flxNetbank.GetData(i, 0) = True Then

                    ' データロー作成
                    row = tbl.NewRow

                    ' データ設定
                    row(CsvCol.Col_A) = "2"                                     ' 01. レコード種別（"2"：固定）
                    row(CsvCol.Col_B) = Me.flxNetbank.GetData(i, 10)            ' 02. 振込銀行番号
                    row(CsvCol.Col_C) = Me.flxNetbank.GetData(i, 11)            ' 03. 振込銀行名
                    row(CsvCol.Col_D) = Me.flxNetbank.GetData(i, 12)            ' 04. 振込支店番号
                    row(CsvCol.Col_E) = Me.flxNetbank.GetData(i, 13)            ' 05. 振込支店名
                    row(CsvCol.Col_F) = DBNull.Value                            ' 06. 無し
                    row(CsvCol.Col_G) = Me.flxNetbank.GetData(i, 18)            ' 07. 預金種目
                    row(CsvCol.Col_H) = Me.flxNetbank.GetData(i, 15)            ' 08. 口座番号
                    row(CsvCol.Col_I) = Me.flxNetbank.GetData(i, 16)            ' 09. 受取人名

                    '-----------------------------------------------------------
                    '   振込手数料データ、振込金額を元に振込手数料を取得
                    '-----------------------------------------------------------
                    intBankMoney = CInt(Me.flxNetbank.GetData(i, 8))            ' 振込金額取得
                    strBankNo = Me.flxNetbank.GetData(i, 10)                    ' 振込銀行番号取得
                    strBelongCommittee = Me.flxNetbank.GetData(i, 9)            ' 所属委員会（役職）

                    ' 振込銀行番号チェック
                    If strBankNo = FINANCIAL_CODE_CENTRAL_LABOR_BANK _
                    Or strBankNo = FINANCIAL_CODE_KINKI_LABOR_BANK Then
                        '-------------------------------------------------------
                        '   中央労金、近畿労金の場合
                        '-------------------------------------------------------
                        ' 振込金額から手数料は差し引かない
                        intPay = intBankMoney
                    Else
                        '-------------------------------------------------------
                        '   上記以外の場合、振込金額から手数料を差し引く
                        '-------------------------------------------------------
                        '===========================
                        '   振込手数料取得処理
                        '===========================
                        ' 振込手数料データ、振込金額を元に振込手数料を取得
                        intFee = Me.GetFee(tblFee, intBankMoney)

                        ' 差引額取得（振込金額 - 手数料）
                        intPay = intBankMoney - intFee
                    End If

                    row(CsvCol.Col_J) = intPay                                  ' 10. 振込金額
                    row(CsvCol.Col_K) = DBNull.Value                            ' 11. 無し
                    row(CsvCol.Col_L) = Me.flxNetbank.GetData(i, 1)             ' 12. 社員番号
                    row(CsvCol.Col_M) = DBNull.Value                            ' 13. 無し
                    row(CsvCol.Col_N) = DBNull.Value                            ' 14. 無し
                    row(CsvCol.Col_O) = DBNull.Value                            ' 15. 無し
                    row(CsvCol.Col_P) = DBNull.Value                            ' 16. 無し
                    row(CsvCol.Col_Q) = Me._strFeeDivisionCsv                   ' 17. 手数料区分
                    row(CsvCol.Col_R) = strBelongCommittee                      ' 18. 所属委員会（役職）

                    ' データ追加
                    tbl.Rows.Add(row)

                    ' インクリメント
                    intBankcnt += 1         ' 振込件数
                    intBankSum += intPay    ' 振込総額合算
                End If
            Next i

            '---------------------------------------------------------------------------------------
            '   集計レコード
            '---------------------------------------------------------------------------------------
            ' データロー作成
            row = tbl.NewRow

            ' データ設定
            row(CsvCol.Col_A) = "8"                                             ' 01. レコード種別（"8"：固定）
            row(CsvCol.Col_B) = intBankcnt                                      ' 02. 振込件数
            row(CsvCol.Col_C) = intBankSum                                      ' 03. 振込総額
            row(CsvCol.Col_D) = DBNull.Value                                    ' 04. 無し
            row(CsvCol.Col_E) = DBNull.Value                                    ' 05. 無し
            row(CsvCol.Col_F) = DBNull.Value                                    ' 06. 無し
            row(CsvCol.Col_G) = DBNull.Value                                    ' 07. 無し
            row(CsvCol.Col_H) = DBNull.Value                                    ' 08. 無し
            row(CsvCol.Col_I) = DBNull.Value                                    ' 09. 無し
            row(CsvCol.Col_J) = DBNull.Value                                    ' 10. 無し
            row(CsvCol.Col_K) = DBNull.Value                                    ' 11. 無し
            row(CsvCol.Col_L) = DBNull.Value                                    ' 12. 無し
            row(CsvCol.Col_M) = DBNull.Value                                    ' 13. 無し
            row(CsvCol.Col_N) = DBNull.Value                                    ' 14. 無し
            row(CsvCol.Col_O) = DBNull.Value                                    ' 15. 無し
            row(CsvCol.Col_P) = DBNull.Value                                    ' 16. 無し
            row(CsvCol.Col_Q) = DBNull.Value                                    ' 17. 無し
            row(CsvCol.Col_R) = DBNull.Value                                    ' 18. 無し

            ' データロー追加
            tbl.Rows.Add(row)

            '---------------------------------------------------------------------------------------
            '   終了レコード
            '---------------------------------------------------------------------------------------
            ' データロー作成
            row = tbl.NewRow

            ' データ設定
            row(CsvCol.Col_A) = "9"                                             ' 01. レコード種別（"9"：固定）
            row(CsvCol.Col_B) = DBNull.Value                                    ' 02. 無し
            row(CsvCol.Col_C) = DBNull.Value                                    ' 03. 無し
            row(CsvCol.Col_D) = DBNull.Value                                    ' 04. 無し
            row(CsvCol.Col_E) = DBNull.Value                                    ' 05. 無し
            row(CsvCol.Col_F) = DBNull.Value                                    ' 06. 無し
            row(CsvCol.Col_G) = DBNull.Value                                    ' 07. 無し
            row(CsvCol.Col_H) = DBNull.Value                                    ' 08. 無し
            row(CsvCol.Col_I) = DBNull.Value                                    ' 09. 無し
            row(CsvCol.Col_J) = DBNull.Value                                    ' 10. 無し
            row(CsvCol.Col_K) = DBNull.Value                                    ' 11. 無し
            row(CsvCol.Col_L) = DBNull.Value                                    ' 12. 無し
            row(CsvCol.Col_M) = DBNull.Value                                    ' 13. 無し
            row(CsvCol.Col_N) = DBNull.Value                                    ' 14. 無し
            row(CsvCol.Col_O) = DBNull.Value                                    ' 15. 無し
            row(CsvCol.Col_P) = DBNull.Value                                    ' 16. 無し
            row(CsvCol.Col_Q) = DBNull.Value                                    ' 17. 無し
            row(CsvCol.Col_R) = DBNull.Value                                    ' 18. 無し

            ' データロー追加
            tbl.Rows.Add(row)

        Catch ex As Exception

        Finally
            ' データベース切断処理
            clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return tbl

    End Function

    ' 元のCSV出力ロジック
    ''***************************************************************************************************
    ''   ＩＤ　：SaveCsvFile
    ''   名称　：CSV出力データ作成
    ''   概要　：CSV出力データを作成し返却します
    ''   引数　：なし
    ''   戻り値：
    ''   作成日：2012/02/08(水)  a.onuma
    ''   更新日：
    ''---------------------------------------------------------------------------------------------------
    ''   履歴　：2012/02/08(水)  a.onuma  新規作成
    ''***************************************************************************************************
    'Private Function SetCsvData() As DataTable
    '    Dim dtOutput As DataTable = New DataTable
    '    Dim iCurrentRow As Integer = 0

    '    'カラムの設定
    '    For iCnt As Integer = 0 To Me._strCsvHeaderArray.Length - 1
    '        dtOutput.Columns.Add(Me._strCsvHeaderArray(iCnt))
    '    Next

    '    'CSV出力データの設定
    '    For iCnt As Integer = 1 To Me.flxNetbank.Rows.Count - 1
    '        If Me.flxNetbank.GetData(iCnt, 0) = True Then 'チェックされている行のみ出力する
    '            dtOutput.Rows.Add()
    '            dtOutput.Rows(iCurrentRow).Item("振込銀行番号") = Me.flxNetbank.GetData(iCnt, 10)
    '            dtOutput.Rows(iCurrentRow).Item("振込銀行名") = Me.flxNetbank.GetData(iCnt, 11)
    '            dtOutput.Rows(iCurrentRow).Item("振込支店番号") = Me.flxNetbank.GetData(iCnt, 12)
    '            dtOutput.Rows(iCurrentRow).Item("振込支店名") = Me.flxNetbank.GetData(iCnt, 13)
    '            dtOutput.Rows(iCurrentRow).Item("手形交換所番号") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("預金種目") = Me.flxNetbank.GetData(iCnt, 18)
    '            dtOutput.Rows(iCurrentRow).Item("口座番号") = Me.flxNetbank.GetData(iCnt, 15)
    '            dtOutput.Rows(iCurrentRow).Item("受取人名") = Me.flxNetbank.GetData(iCnt, 16)
    '            dtOutput.Rows(iCurrentRow).Item("振込金額") = Me.flxNetbank.GetData(iCnt, 8)
    '            dtOutput.Rows(iCurrentRow).Item("新規コード") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("顧客コード１") = Me.flxNetbank.GetData(iCnt, 1)
    '            dtOutput.Rows(iCurrentRow).Item("顧客コード２") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("振込区分") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("ＥＤＩ識別表示") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("ＥＤＩ情報") = DBNull.Value
    '            dtOutput.Rows(iCurrentRow).Item("手数料区分") = Me._strFeeDivisionCsv
    '            dtOutput.Rows(iCurrentRow).Item("所属委員会（役職）") = Me.flxNetbank.GetData(iCnt, 9)

    '            iCurrentRow = iCurrentRow + 1
    '        End If
    '    Next
    '    Return dtOutput
    'End Function
#End Region

#Region " 振込元データ取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：SetBankTransferAccount
    '   名称　：振込元データ取得処理
    '   概要　：振込元データを取得する。
    '   引数　：ByVal iClsDb As CLAccessMdb = データベース関連クラス
    '   戻り値：振込元データ情報
    '   作成日：2021/10/04(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/04(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetBankTransferAccount(ByVal iClsDb As CLAccessMdb) As DataTable

        Dim strNow As String = Nothing                                  ' 現在日付（yyyyMMdd）
        Dim sql As String = ""                                          ' SQL
        Dim dtRet As DataTable = Nothing                                ' データテーブル

        ' 現在日付（yyyyMMdd）
        strNow = Now.ToString("yyyMMdd")

        ' SQL文作成
        sql += ""
        sql += "SELECT a.c_rokin_contract_number" & vbCrLf              ' 01. 労金契約番号
        sql += "      ,a.c_bank" & vbCrLf                               ' 02. 金融機関コード
        sql += "      ,a.c_bank_office" & vbCrLf                        ' 03. 支店番号
        sql += "      ,a.k_deposit_items" & vbCrLf                      ' 04. 預金種目
        sql += "      ,a.c_bank_account" & vbCrLf                       ' 05. 口座番号
        sql += "      ,a.l_account_name" & vbCrLf                       ' 06. 口座名義
        sql += "      ,a.l_account_name_kna" & vbCrLf                   ' 07. 口座名義カナ
        sql += "      ,b.l_bank_name" & vbCrLf                          ' 08. 銀行名
        sql += "      ,b.l_bank_name_kna" & vbCrLf                      ' 09. 銀行名カナ
        sql += "      ,c.l_bank_office_name" & vbCrLf                   ' 10. 支店名
        sql += "      ,c.l_bank_office_name_kna" & vbCrLf               ' 11. 支店名カナ
        sql += "  FROM ((bank_transfer_account a" & vbCrLf              ' /* 振込元データ */
        sql += "       INNER JOIN bank_info b" & vbCrLf                 ' /* 銀行マスタ結合 */
        sql += "       ON  a.c_bank        = b.c_bank)" & vbCrLf        ' 振込元データの金融機関コードと銀行マスタの銀行コードが同じもの
        sql += "       INNER JOIN bank_info_dtl c" & vbCrLf             ' /* 銀行マスタ詳細結合 */
        sql += "       ON  a.c_bank        = c.c_bank" & vbCrLf         ' 銀行マスタ詳細の銀行コードが同じもの
        sql += "       AND a.c_bank_office = c.c_bank_office)" & vbCrLf ' 銀行マスタ詳細の支店番号が同じもの
        sql += " WHERE a.d_from <= '" & strNow & "'" & vbCrLf           ' 振込元データの適用開始日が現在日付よりも小さいもの
        sql += "   AND a.d_to   >= '" & strNow & "'" & vbCrLf           ' 振込元データの適用終了日が現在日付よりも大きいもの
        sql += "   AND b.d_from <= '" & strNow & "'" & vbCrLf           ' 銀行マスタの適用開始日が現在日付よりも小さいもの
        sql += "   AND b.d_to   >= '" & strNow & "'" & vbCrLf           ' 銀行マスタの適用終了日が現在日付よりも大きいもの
        sql += "   AND c.d_from <= '" & strNow & "'" & vbCrLf           ' 銀行マスタ詳細の適用開始日が現在日付よりも小さいもの
        sql += "   AND c.d_to   >= '" & strNow & "'" & vbCrLf           ' 銀行マスタ詳細の適用終了日が現在日付よりも大きいもの

        ' SQL実行
        dtRet = iClsDb.ExecuteSql(sql)

        ' 件数チェック
        If dtRet.Rows.Count = 0 Then
            Return Nothing
        End If

        ' 戻り値設定
        Return dtRet

    End Function
#End Region

#Region " 振込元データ取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：SetBankTransferFee
    '   名称　：振込手数料データ取得処理
    '   概要　：振込手数料データを取得する。
    '   引数　：ByVal iClsDb As CLAccessMdb = データベース関連クラス
    '   戻り値：振込手数料データ情報
    '   作成日：2021/10/04(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/04(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetBankTransferFee(ByVal iClsDb As CLAccessMdb) As DataTable

        Dim strNow As String = Nothing                  ' 現在日付（yyyyMMdd）
        Dim sql As String = ""                          ' SQL
        Dim dtRet As DataTable = Nothing                ' データテーブル

        ' 現在日付（yyyyMMdd）
        strNow = Now.ToString("yyyMMdd")

        ' SQL文作成
        sql += ""
        sql += "SELECT a.s_lower" & vbCrLf                      ' 01. 下限金額
        sql += "      ,a.s_upper" & vbCrLf                      ' 02. 上限金額
        sql += "      ,a.s_transfer_fee" & vbCrLf               ' 03. 振込手数料
        sql += "  FROM bank_transfer_fee a" & vbCrLf            ' /* 振込手数料 */
        sql += " WHERE a.d_from <= '" & strNow & "'" & vbCrLf   ' 振込手数料の適用開始日が現在日付よりも小さいもの
        sql += "   AND a.d_to   >= '" & strNow & "'" & vbCrLf   ' 振込手数料の適用終了日が現在日付よりも大きいもの
        sql += " ORDER BY a.s_lower" & vbCrLf                   ' 下限金額で並び替え

        ' SQL実行
        dtRet = iClsDb.ExecuteSql(sql)

        ' 件数チェック
        If dtRet.Rows.Count = 0 Then
            Return Nothing
        End If

        ' 戻り値設定
        Return dtRet

    End Function
#End Region

#Region " 振込手数料取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：SetBankTransferFee
    '   名称　：振込手数料取得処理
    '   概要　：振込手数料を取得する。
    '   引数　：ByVal iTbl As DataTable     = 振込手数料情報,
    '           ByVal iBankMoney As Integer = 振込手数料
    '   戻り値：振込手数料
    '   作成日：2021/10/05(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/05(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function GetFee( _
        ByVal iTbl As DataTable, _
        ByVal iBankMoney As Integer _
    ) As Integer

        ' 振込手数料
        Dim intFee As Integer = 0

        ' 振込手数料データ数分ループ
        For i As Integer = 0 To iTbl.Rows.Count - 1 Step 1
            ' 振込金額が、下限金額～上限金額範囲かチェック
            If CInt(iTbl.Rows(i).Item(0)) <= iBankMoney _
            And CInt(iTbl.Rows(i).Item(1)) >= iBankMoney Then
                ' 振込手数料
                intFee = CInt(iTbl.Rows(i).Item(2))
                Exit For
            End If
        Next i

        ' 戻り値設定
        Return intFee

    End Function
#End Region

#Region " 変更取り消し処理 "
    '***************************************************************************************************
    '   ＩＤ　：CancelEdit
    '   名称　：変更取り消し処理
    '   概要　：画面の編集を取り消し、データ照会時の値に戻します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>変更取り消し処理</summary>
    ''' <remarks></remarks>
    Private Sub CancelEdit()
        Try
            Me.flxNetbank.Redraw = False

            ' 一度グリッドに表示したデータを削除
            Me.flxNetbank.Rows.RemoveRange(1, Me.flxNetbank.Rows.Count - 1)

            ' 読み込み済みデータより再表示
            Call Me.ShowNetBankGrid(_dtStafBankSendMember)
            Me.flxNetbank.Redraw = True

            ' 振込総額を再計算
            Call Me.CalcTotal()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region

#Region " 編集モード切替 "
    '***************************************************************************************************
    '   ＩＤ　：ChangeEditMode
    '   名称　：編集モード切替
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ChangeEditMode(ByVal blnEdit As Boolean)

        If blnEdit = True Then
            ' データ編集可能時
            Me.btnAllCheckOn.Enabled = True         ' チェックオンボタン
            Me.btnAllCheckOff.Enabled = True        ' チェックオフボタン
            Me.btnAddMember.Enabled = True          ' 組合員追加ボタン
            Me.btnPrint.Visible = False             ' 印刷ボタン非表示
            Me.btnOutputCsv.Visible = False         ' 振込ファイル出力ボタン非表示
            Me.btnChange.Visible = False            ' 内容変更ボタン非表示
            Me.btnReturn.Visible = False            ' 戻るボタン非表示
            Me.btnEntryConfirm.Visible = True       ' 登録確認ボタン表示
            Me.btnCancel.Visible = True             ' キャンセルボタン表示
            Me.flxNetbank.Cols(0).Visible = True    ' チェック列を表示する
            Me._blnIsEdit = True                    ' 編集可能フラグをたてる
        Else
            ' データ編集不可時
            Me.btnAllCheckOn.Enabled = False        ' チェックオンボタン
            Me.btnAllCheckOff.Enabled = False       ' チェックオフボタン
            Me.btnAddMember.Enabled = False         ' 組合員追加ボタン
            Me.btnPrint.Visible = True              ' 印刷ボタン表示
            Me.btnOutputCsv.Visible = True          ' 振込ファイル出力ボタン表示
            Me.btnChange.Visible = True             ' 内容変更ボタン表示
            Me.btnReturn.Visible = True             ' 戻るボタン表示
            Me.btnEntryConfirm.Visible = False      ' 登録確認ボタン非表示
            Me.btnCancel.Visible = False            ' キャンセルボタン非表示
            Me.flxNetbank.Cols(0).Visible = False   ' チェック列を隠す
            Me._blnIsEdit = False                   ' 編集不可フラグを立てる
        End If

    End Sub

#End Region

#Region " フォームクローズ "
    '***************************************************************************************************
    '   ＩＤ　：FormClose
    '   名称　：フォームクローズ
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/09(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>フォームクローズ</summary>
    ''' <remarks></remarks>
    Private Sub FormClose()

        Dim pn As Panel
        Dim uc As Control
        Dim clsUC080101 As UC080101

        Try
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC080101)
            If uc Is Nothing Then
                uc = New UC080101

                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If

            If _intReturnBtn = 0 Then
                ' 振込データ登録が行われた場合、検索画面の各情報を更新する
                clsUC080101 = pn.Controls(SCREEN_ID_UC080101)
                Call clsUC080101.RenewScreenInfo()
            End If
            Me.Dispose()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region
#End Region

End Class

#End Region

