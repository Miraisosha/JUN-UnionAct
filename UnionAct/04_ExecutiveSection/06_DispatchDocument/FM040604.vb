#Region "FM040604"
'===========================================================================================================
'   クラスＩＤ　　：FM040604
'   クラス名称　　：文書操作ウィンドウ画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.Framework.Command
Imports UnionAct.GUI.Document
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDFile
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDChk
Imports UnionAct.NSCLAccessMdb

Imports System.Text
Imports System.Runtime.InteropServices

Imports Microsoft.Office.Interop
Imports System.IO

Public Class FM040604

#Region "定数・変数"
    '---------------------------------------------------------------
    '   定数
    '---------------------------------------------------------------
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040604          ' FM040604
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040604      ' 文書操作ウィンドウ画面
    ' ステータス
    Private Const STATUS_INSERT As String = "0"                     ' 新規作成
    Private Const STATUS_SHOW As String = "1"                       ' 表示
    Private Const STATUS_EDIT As String = "2"                       ' 編集
    Private Const STATUS_COPY_EDIT As String = "3"                  ' コピーして編集
    ' Excel用
    Private Const XL_EXCEL9795 As Integer = 43                      ' Excel 97-2002/および5.0/95ブック
    ' テンプレート区分
    Private Const TEMPLATE_KBN_ADD As String = "ADD"                ' アドインファイル（UnionDoc.xla）
    Private Const TEMPLATE_KBN_001 As String = "001"                ' Template001.xls
    Private Const TEMPLATE_KBN_002 As String = "002"                ' Template002.xls
    Private Const TEMPLATE_KBN_003 As String = "003"                ' Template003.xls
    Private Const TEMPLATE_KBN_004 As String = "004"                ' Template004.xls
    Private Const TEMPLATE_KBN_005 As String = "005"                ' Template005.xls
    Private Const TEMPLATE_KBN_006 As String = "006"                ' Template006.xls
    Private Const TEMPLATE_KBN_007 As String = "007"                ' Template007.xls
    Private Const TEMPLATE_KBN_008 As String = "008"                ' Template008.xls
    Private Const TEMPLATE_KBN_010 As String = "010"                ' Template010.xls
    Private Const TEMPLATE_KBN_011 As String = "011"                ' Template011.xls
    Private Const TEMPLATE_KBN_013 As String = "013"                ' Template013.xls
    Private Const TEMPLATE_KBN_A00 As String = "A00"                ' TemplateA00.xls
    Private Const TEMPLATE_KBN_C00 As String = "C00"                ' TemplateC00.xls
    Private Const TEMPLATE_KBN_D00 As String = "D00"                ' TemplateD00.xls
    Private Const TEMPLATE_KBN_J00 As String = "J00"                ' TemplateJ00.xls
    '---------------------------------------------------------------
    '   変数
    '---------------------------------------------------------------
    ' Excel関連
    ' 参照設定のヴァージョンに依存されるもの ----------------- START 
    Private mXlsAp As Microsoft.Office.Interop.Excel.Application            ' Excel アプリケーションオブジェクト
    Private mXlsWbs As Microsoft.Office.Interop.Excel.Workbooks             ' Excel Workbooksオブジェクト
    Private WithEvents mXlsWb As Microsoft.Office.Interop.Excel.Workbook    ' Excel Workbookオブジェクト
    Private mXlsSs As Microsoft.Office.Interop.Excel.Sheets                 ' Excel Sheetsオブジェクト
    Private mXlsWs As Microsoft.Office.Interop.Excel.Worksheet              ' Excel Worksheetオブジェクト
    ' 参照設定のヴァージョンに依存されるもの ------------------- END
    ' 参照設定のヴァージョンに依存されないもの --------------- START
    'Private mXlsAp As Object                                        ' Excelアプリケーションオブジェクト
    'Private mXlsWbs As Object                                       ' Excelワークブックオブジェクト
    'Private mXlsWs As Object                                        ' Excelワークシートオブジェクト
    'Private WithEvents mXlsWb As Object
    ' 参照設定のヴァージョンに依存されないもの ----------------- END
    ' ワーク関連
    Private mStrWorkDirFrom As String = ""                          ' ワークフォルダFrom名（新規作成ならテンプレート、その他は保存先）
    Private mStrWorkDirTo As String = ""                            ' ワークフォルダTo名
    Private mStrWorkFileFrom As String = ""                         ' ワークファイルFrom名（新規作成ならテンプレート、その他は保存先）
    Private mStrWorkFileTo As String = ""                           ' ワークファイルTo名
    Private mStrWorkDir As String = ""                              ' ワークフォルダ名
    ' 保存先関連
    Private mStrSaveDir As String = ""                              ' 保存先フォルダ
    Private mStrSaveFile As String = ""                             ' 保存先ファイル名
    '総合OAに戻るボタン押下フラグ
    Private mBlnIsClose As Boolean = False
#End Region

#Region "プロパティ"
    Public _strStatus As String = ""                                ' ステータス（"0"：新規作成, "1"：表示, "2"：編集, "3"：コピーして編集）

    Public _strDocCode As String = ""                               ' 管理コード
    Public _strDocNumber As String = ""                             ' 文書№（文書番号）
    Public _strPeriodIdD As String = ""                             ' 期ID（文書番号Ｄ文書用）    2012/06/27 追加
    Public _strDocNumberD As String = ""                            ' 文書№（文書番号Ｄ文書用）    2012/06/27 追加
    Public _strFile As String = ""                                  ' ファイル名
    Public _strIssueDate As String = ""                             ' 発行日
    Public _strDocId As String = ""                                 ' 文書ID（保存先ファイル名フルパス）
    Public _intDocId As Integer = 0                                 ' 文書識別コード
    Public _strPeriodId As String = ""                              ' 期ID
    Public _intPeriod As Integer = 0                                ' 期（数値）
    Public _strPeriodName As String = ""                            ' 期（全角期（第ＸＸ期））
    Public _intSubjectSeq As Integer = 0                            ' 標題枝番
    Public _strSubject As String = ""                               ' 標題
    Public _strTemplate As String = ""                              ' テンプレート区分
    Public _strApplyDate As String = ""                             ' 適用日付（要件改訂で不要っぽい）

    Public _strDetails As String = ""                               ' 詳細設定分類
    Public _bytSaveKindFlg As Byte = 0                              ' 保存した文書種別（0：発信済, 1：一時保存）

    ' 詳細設定分類が"1"：追加・削除、"2"：長の交代に使用
    Public _strCommitteeUpdate As String() = Nothing                ' 委員会変更ID
    Public _strDocumentOut As String() = Nothing                    ' 帳票出力
    Public _strCommitteeDFrom As String() = Nothing                 ' 委員会適用日付

    ' 新規作成のみ
    Public _strTemplateFile As String = ""                          ' テンプレートファイル名（ファイル名のみ）
    Public _strTemplateFileFull As String = ""                      ' テンプレートファイル名（フルパス）

    ' ステータス
    Public Property strStatus() As String
        Get
            Return _strStatus
        End Get
        Set(ByVal value As String)
            _strStatus = value
        End Set
    End Property

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

    ' 文書№（文書番号Ｄ文書用）　2012/06/27 追加
    Public Property strPeriodIdD() As String
        Get
            Return _strPeriodIdD
        End Get
        Set(ByVal value As String)
            _strPeriodIdD = value
        End Set
    End Property

    ' 文書№（文書番号Ｄ文書用）　2012/06/27 追加
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

    ' 文書ID（保存先ファイル名フルパス）
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

    ' 期全角期（第ＸＸ期））
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
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM040604_Load( _
        ByVal sender As Object, _
        ByVal e As System.EventArgs _
    ) Handles Me.Load

        Try
            '-------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   各データ取得
            '-------------------------------------------------------
            If Me.GetData() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   コントロールロックアンロック処理
            '-------------------------------------------------------
            If Me.ControlRockUnLock() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   Excelオープン処理
            '-------------------------------------------------------
            If Me.ExcelOpen(mStrWorkDirTo & mStrWorkFileTo) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   Excelデータ出力処理
            '-------------------------------------------------------
            ' 新規作成・コピーして編集時にテンプレート区分毎に初期表示データを出力
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                If Me.ExcelOutput() = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：FM040604_FormClosing
    '   名称　：フォームクローズ処理
    '   概要　：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM040604_FormClosing( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.FormClosingEventArgs _
    ) Handles Me.FormClosing

        ' Excelクローズ処理でエラーになっても一時保存フォルダ削除処理は行うので、エラー処理は無しにしておくよ。

        'Excel終了フラグをたてる
        Me.mBlnIsClose = True

        '---------------------------------------------------
        '   エクセルクローズ処理
        '---------------------------------------------------
        If Me.ExcelClose() = False Then
            Exit Sub
        End If

        '---------------------------------------------------
        '   ワークファイル削除処理
        '---------------------------------------------------
        ' フォルダ存在チェック処理
        If MDFile.DirExists(mStrWorkDirTo) = False Then
            Exit Sub
        End If

        ' フォルダ削除処理
        If MDFile.DirDelete( _
            mStrWorkDirTo, _
            True _
        ) = False Then
            Exit Sub
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnApply
    '   名称　：適用ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnApply_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnApply.Click

        Try
            ' Excelセル値発行日設定処理
            If Me.ExcelSetIssueDate() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnDocNo_Click
    '   名称　：文書番号の採番ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDocNo_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnDocNumber.Click

        Try
            '-------------------------------------------------------
            '   Excelファイル保存処理
            '-------------------------------------------------------
            If Me.ExcelSave(Me.mStrWorkDirTo & Me.mStrWorkFileTo) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   登録メイン処理
            '-------------------------------------------------------
            If Me.MainInsert(False) = False Then
                Exit Sub
            End If

            ' 文書番号採番後、文書番号の採番ボタン使用不可
            Me.btnDocNumber.Enabled = False

            ' 保存した文書種別（0：発信済, 1：一時保存）を発信済にする
            Me.bytSaveKindFlg = 1

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：BtnInsertDb_Click
    '   名称　：DB登録ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub BtnInsertDb_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnInsertDb.Click

        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果

        Try
            '-------------------------------------------------------
            '   登録確認メッセージ表示
            '-------------------------------------------------------
            ' 文書登録メッセージ表示
            diaRet = CLMsg.Show("GQ0058")

            ' 押下ボタン判定
            If diaRet = Windows.Forms.DialogResult.No Then
                ' 「いいえ」押下時、処理を抜ける
                Exit Sub
            End If

            '-------------------------------------------------------
            '   一時保存扱いメッセージ表示
            '-------------------------------------------------------
            ' 文書番号の採番ボタンが押下可能なら文書番号未採番と判断
            If Me.btnDocNumber.Enabled Then
                diaRet = CLMsg.Show("GQ0060")                       ' 一時保存扱いになる旨のメッセージを表示
                If diaRet = Windows.Forms.DialogResult.No Then      ' 押下ボタン判定
                    Exit Sub                                        ' 「いいえ」押下時、処理を抜ける
                End If
            End If

            '-------------------------------------------------------
            '   Excelファイル保存処理
            '-------------------------------------------------------
            If Me.ExcelSave(Me.mStrWorkDirTo & Me.mStrWorkFileTo) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   登録メイン処理
            '-------------------------------------------------------
            If Me.MainInsert(True) = False Then
                Exit Sub
            End If

            ' 保存した文書種別（0：発信済, 1：一時保存）
            If MDChk.ChkNull(Me.strDocNumber) Then
                Me.bytSaveKindFlg = 1               ' 文書番号が無い場合、一時保存
            Else
                Me.bytSaveKindFlg = 0               ' 文書番号が有る場合、発信済
            End If

            ' ダイアログ結果（DB登録）
            Me.DialogResult = Windows.Forms.DialogResult.OK

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：総合OAに戻るボタンクリック処理
    '   概要　：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBack_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnBack.Click

        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果

        Try
            ' 表示モード以外は保存確認
            If strStatus <> "1" Then
                If ConfirmAndSaveData() = False Then
                    Exit Sub
                End If
            Else
                ' 終了メッセージ表示（Excelも終了する旨のメッセージ）
                diaRet = CLMsg.Show("GQ0050")

                ' ダイアログ結果判定
                If diaRet = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

                ' ダイアログ結果格納（キャンセル）
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
            End If

            ' 文書操作ウィンドウ画面閉じる
            Me.Close()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：ConfirmAndSaveData
    '   名称　：データ確認登録処理
    '   概要　：
    '   作成日：2012/04/16(月)  
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/04/16(月)  新規作成
    '***************************************************************************************************
    Private Function ConfirmAndSaveData() As Boolean

        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果
        Dim strMsg As String = ""               ' 表示メッセージ

        Try
            '-------------------------------------------------------
            '   登録確認メッセージ表示
            '-------------------------------------------------------
            ' 文書登録メッセージ表示
            CLMsg.GetMsgInfo("GQ0059", False, False, strMsg)
            diaRet = MessageBox.Show( _
                strMsg, _
                "問合せ", _
                MessageBoxButtons.YesNoCancel, _
                MessageBoxIcon.Question, _
                MessageBoxDefaultButton.Button1 _
            )

            ' 押下ボタン判定
            If diaRet = Windows.Forms.DialogResult.No Then
                ' 「いいえ」押下時、処理を抜けて文書操作窓終了
                Return True
            ElseIf diaRet = Windows.Forms.DialogResult.Cancel Then
                ' 「キャンセル」押下時、処理を抜けて文書操作窓に戻る
                Return False
            End If

            '-------------------------------------------------------
            '   一時保存扱いメッセージ表示
            '-------------------------------------------------------
            ' 文書番号の採番ボタンが押下可能なら文書番号未採番と判断
            If Me.btnDocNumber.Enabled Then
                diaRet = CLMsg.Show("GQ0060")                       ' 一時保存扱いになる旨のメッセージを表示
                If diaRet = Windows.Forms.DialogResult.No Then      ' 押下ボタン判定
                    Return False                                    ' 「いいえ」押下時、処理を抜ける
                End If
            End If

            '-------------------------------------------------------
            '   Excelファイル保存処理
            '-------------------------------------------------------
            If Me.ExcelSave(Me.mStrWorkDirTo & Me.mStrWorkFileTo) = False Then
                Return False
            End If

            '-------------------------------------------------------
            '   登録メイン処理
            '-------------------------------------------------------
            If Me.MainInsert(True) = False Then
                Return False
            End If

            ' 保存した文書種別（0：発信済, 1：一時保存）
            If MDChk.ChkNull(Me.strDocNumber) Then
                Me.bytSaveKindFlg = 1               ' 文書番号が無い場合、一時保存
            Else
                Me.bytSaveKindFlg = 0               ' 文書番号が有る場合、発信済
            End If

            ' ダイアログ結果（DB登録）
            Me.DialogResult = Windows.Forms.DialogResult.OK

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try
        Return True
    End Function

    '***************************************************************************************************
    '   ＩＤ　：xlsWB_BeforeClose
    '   名称　：エクセルワークブックが閉じられる直前
    '   概要  ：
    '   引数　：ByRef Cancel As Boolean = 
    '   戻り値：なし
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub xlsWB_BeforeClose( _
        ByRef Cancel As Boolean _
    ) Handles mXlsWb.BeforeClose

        Try
            Cancel = True               ' ワークブックを閉じさせない
            Me.mXlsWb.Saved = True      ' 保存済みに設定
            If Me.mBlnIsClose = False Then
                CLMsg.Show("GI0041")
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )
        Finally
            mXlsWb = Nothing
        End Try

    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------
            '   ワークフォルダ・ファイル名取得処理
            '-------------------------------------------------------
            If Me.GetWorkDirFile(clsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   アドインファイルコピー処理
            '-------------------------------------------------------
            If Me.CopyAddinFile() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   発行日設定
            '-------------------------------------------------------
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                Me.dtpIssueDate.Value = Now()
            ElseIf Me.strStatus = STATUS_EDIT _
            Or Me.strStatus = STATUS_SHOW Then
                Me.dtpIssueDate.Value = Date.Parse(Format(CInt(Me.strIssueDate), "0000/00/00")).ToString("yyyy/MM/dd")
                'Me.dtpIssueDate.Value = Me.strIssueDate
            End If

            '-------------------------------------------------------
            '   各情報クリア
            '-------------------------------------------------------
            ' 新規作成・コピーして編集は、新規作成扱いなので各情報を消しておく
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                Me.intDocId = 0         ' 文書識別コード
                Me.strDocId = ""        ' 管理コード
                Me.strDocId = ""        ' 文書ID（文書フルパス）
                Me.strDocNumber = ""    ' 文書番号
                Me.strPeriodIdD = ""    ' 期ID（Ｄ文書用） 　     2012/06/27 追加
                Me.strDocNumberD = ""   ' 文書番号（Ｄ文書用）　  2012/06/27 追加
                Me.strFile = ""         ' ファイル名
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID, SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeUpdateInfo
    '   名称　：追加・削除委員会情報取得処理
    '   概要  ：追加・削除データの発信文書作成時、選択委員会の追加・削除情報を取得する
    '   引数　：
    '   戻り値：
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>
    ''' 追加・削除委員会情報取得処理
    ''' </summary>
    ''' <returns>SQL実行結果</returns>
    ''' <remarks></remarks>
    Private Function GetCommitteeUpdateInfo() As DataTable

        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb()
        Dim strWhereUpdateCommitteeId As String = String.Empty
        Dim sbSql As StringBuilder = New StringBuilder
        Dim strSqlUnion As String = ""

        ' 追加・削除情報取得SQL
        strSqlUnion = ""
        strSqlUnion += "SELECT culd.c_user_id" & vbCrLf
        strSqlUnion += "      ,culd.k_committee_insert" & vbCrLf
        strSqlUnion += "      ,staf.l_name AS user_name" & vbCrLf
        strSqlUnion += "      ,staf.belonging " & vbCrLf
        strSqlUnion += "      ,cul.c_committee_id" & vbCrLf
        strSqlUnion += "      ,comt.l_name" & vbCrLf
        strSqlUnion += "  FROM committee_update_list AS cul" & vbCrLf
        strSqlUnion += "      ,committee_update_list_dtl AS culd" & vbCrLf
        strSqlUnion += "      ,(" & vbCrLf
        strSqlUnion += "        SELECT attr1.c_user_id" & vbCrLf
        strSqlUnion += "              ,attr1.l_name" & vbCrLf
        strSqlUnion += "              ,dtl1.l_name AS belonging " & vbCrLf
        strSqlUnion += "          FROM staf_attribute AS attr1" & vbCrLf
        strSqlUnion += "              ,(" & vbCrLf
        strSqlUnion += "                SELECT c_user_id" & vbCrLf
        strSqlUnion += "                      ,c_ksh" & vbCrLf
        strSqlUnion += "                      ,c_staf_id" & vbCrLf
        strSqlUnion += "                      ,MAX(d_from) AS now_from" & vbCrLf
        strSqlUnion += "                  FROM staf_attribute" & vbCrLf
        strSqlUnion += "                 WHERE d_from <= '{0}'" & vbCrLf
        strSqlUnion += "                 GROUP BY c_user_id" & vbCrLf
        strSqlUnion += "                         ,c_ksh" & vbCrLf
        strSqlUnion += "                         ,c_staf_id" & vbCrLf
        strSqlUnion += "               ) AS attr2" & vbCrLf
        strSqlUnion += "              ,constant_dtl AS dtl1" & vbCrLf
        strSqlUnion += "         WHERE attr1.c_user_id = attr2.c_user_id" & vbCrLf
        strSqlUnion += "           AND attr1.c_ksh = attr2.c_ksh" & vbCrLf
        strSqlUnion += "           AND attr1.d_from = attr2.now_from " & vbCrLf
        strSqlUnion += "           AND dtl1.c_constant = 'BELONGING'" & vbCrLf
        strSqlUnion += "           AND dtl1.c_constant_seq = attr1.k_belonging" & vbCrLf
        strSqlUnion += "       ) AS staf" & vbCrLf
        strSqlUnion += "      ,(" & vbCrLf
        strSqlUnion += "        SELECT d.c_committee_id" & vbCrLf
        strSqlUnion += "              ,d.l_name" & vbCrLf
        strSqlUnion += "              ,d.d_to" & vbCrLf
        strSqlUnion += "              ,MAX(d.d_from) AS d_from" & vbCrLf
        strSqlUnion += "          FROM committee AS d" & vbCrLf
        strSqlUnion += "         WHERE d.d_from <= '{0}'" & vbCrLf
        strSqlUnion += "           AND d.d_to >= '{0}'" & vbCrLf
        strSqlUnion += "         GROUP BY d.c_committee_id" & vbCrLf
        strSqlUnion += "                 ,d.l_name" & vbCrLf
        strSqlUnion += "                 ,d.d_to" & vbCrLf
        strSqlUnion += "         ORDER BY d.c_committee_id" & vbCrLf
        strSqlUnion += "       ) AS comt" & vbCrLf
        strSqlUnion += " WHERE cul.c_committee_update = culd.c_committee_update" & vbCrLf
        strSqlUnion += "   AND culd.c_user_id = staf.c_user_id" & vbCrLf
        strSqlUnion += "   AND cul.c_committee_update = '{1}'" & vbCrLf
        strSqlUnion += "   AND cul.c_committee_id = comt.c_committee_id" & vbCrLf
        strSqlUnion += "   AND culd.k_committee_insert <> '2'" & vbCrLf

        Try
            If Me.strCommitteeUpdate.Length > 1 Then
                sbSql.Append(String.Format(strSqlUnion, strCommitteeDFrom(0), strCommitteeUpdate(0)))
                ' 選択された委員会IDからWhere句の条件作成
                For iCnt As Integer = 1 To Me.strCommitteeUpdate.Length - 1
                    sbSql.Append(" UNION " & String.Format(strSqlUnion, strCommitteeDFrom(iCnt), strCommitteeUpdate(iCnt)))
                Next
            ElseIf Me.strCommitteeUpdate.Length = 1 Then
                sbSql.Append(String.Format(strSqlUnion, strCommitteeDFrom(0), strCommitteeUpdate(0)))
            End If
            ' ORDER BY句を追加
            sbSql.Append("ORDER BY culd.k_committee_insert ,cul.c_committee_id, culd.c_user_id")

            ' DB接続開始
            clsDb.Connect()
            ' 委員会変更IDをキーに選択データの追加・削除情報を取得
            dtRet = clsDb.ExecuteSql(sbSql.ToString)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeUpdateInfo
    '   名称　：追加・削除委員会情報取得処理
    '   概要  ：追加・削除データの発信文書作成時、選択委員会の追加・削除情報を取得する
    '   引数　：
    '   戻り値：
    '   作成日：2012/03/29(木)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/29(木)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetCommitteeTopChangeInfo() As DataTable

        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = ""

        ' SQL文作成
        strSql = ""
        strSql += "SELECT culd.c_user_id, culd.k_committee_insert, culd.s_committee_seq, staf.l_name AS user_name " &
                               "     , staf.belonging, comt.l_name, com_dtl.l_name AS post_name, cul.c_committee_id " &
                               "FROM committee_update_list AS cul , " &
                               "     committee_update_list_dtl culd , " &
                               "     (SELECT attr1.c_user_id , attr1.l_name , dtl1.l_name AS belonging " &
                               "      FROM staf_attribute AS attr1," &
                               "           (SELECT  c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " &
                               "            FROM staf_attribute " &
                               "            WHERE d_from <= '{0}' " &
                               "            GROUP BY c_user_id, c_ksh , c_staf_id ) AS attr2, " &
                               "           constant_dtl AS dtl1 " &
                               "      WHERE attr1.c_user_id = attr2.c_user_id " &
                               "      AND attr1.c_ksh = attr2.c_ksh " &
                               "      AND attr1.d_from = attr2.now_from " &
                               "      AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr1.k_belonging " &
                               "     ) AS staf, " &
                               "     (SELECT d.c_committee_id " &
                               "             ,d.l_name " &
                               "             ,d.d_to " &
                               "             ,MAX(d.d_from) AS d_from " &
                               "        FROM committee AS d " &
                               "        WHERE d.d_from <= '{0}' " &
                               "        AND   d.d_to   >= '{0}' " &
                               "        GROUP BY d.c_committee_id " &
                               "                ,d.l_name " &
                               "                ,d.d_to " &
                               "        ORDER BY d.c_committee_id ) AS comt, " &
                               "     (SELECT comd1.c_committee_id, comd1.s_committee_seq, " &
                               "             comd1.c_officer_pay_id, comd1.l_name, " &
                               "             comd1.d_service_from, comd1.d_service_to " &
                               "      FROM committee_dtl AS comd1, " &
                               "           (SELECT M.c_committee_id AS max_id, " &
                               "                   MAX(M.d_from) AS max_d_from " &
                               "            FROM committee_dtl AS M " &
                               "            WHERE M.d_from < '{0}' " &
                               "            AND M.d_to >= '{0}' " &
                               "            GROUP BY M.c_committee_id " &
                               "           )  AS comd2 " &
                               "       WHERE comd2.max_id = comd1.c_committee_id " &
                               "       AND comd2.max_d_from = comd1.d_from " &
                               "       ) AS com_dtl " &
                               "WHERE cul.c_committee_update = culd.c_committee_update " &
                               "AND culd.c_user_id = staf.c_user_id " &
                               "AND cul.c_committee_update = '{1}' " &
                               "AND cul.c_committee_id = comt.c_committee_id " &
                               "AND culd.c_committee_id = com_dtl.c_committee_id " &
                               "AND culd.s_committee_seq = com_dtl.s_committee_seq " &
                               "AND culd.k_related_head = '1' " &
                               "AND culd.k_committee_insert <> '1' " &
                               "ORDER BY culd.s_committee_seq , culd.c_user_id "

        Try
            ' DB接続開始
            clsDb.Connect()

            ' SQL実行結果を取得
            dtRet = clsDb.ExecuteSql(String.Format(strSql, strCommitteeDFrom(0), strCommitteeUpdate(0)))
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            ' DB接続終了
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果

        Try
            '---------------------------------------------------
            '   文書番号欄
            '---------------------------------------------------
            ' GroupBox
            Me.grpDocumentNo.Visible = True                     ' 文書番号
            Me.grpDocumentNo.Enabled = True
            Me.grpDocumentNo.Text = "文書番号"
            ' Button
            Me.btnDocNumber.Visible = True                      ' 文書番号の採番
            Me.btnDocNumber.Enabled = True

            '---------------------------------------------------
            '   発行日欄
            '---------------------------------------------------
            ' GroupBox
            Me.grpIssueDate.Visible = True                      ' 発行日
            Me.grpIssueDate.Enabled = True
            Me.grpIssueDate.Text = "発行日"
            ' DateTimePicker
            Me.dtpIssueDate.Visible = True                      ' 発行日
            Me.dtpIssueDate.Enabled = True
            ' Button
            Me.btnApply.Visible = True                          ' 適用
            Me.btnApply.Enabled = True
            ' CheckBox
            Me.chkWareki.Visible = True                         ' 和暦
            Me.chkWareki.Enabled = True
            Me.chkWareki.Checked = False


            ' Button
            Me.btnBack.Visible = True                           ' 総合OAに戻る
            Me.btnBack.Enabled = True
            Me.btnInsertDb.Visible = True                       ' DB登録
            Me.btnInsertDb.Enabled = True

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID, SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    ''***************************************************************************************************
    ''   ＩＤ　：DocumentIniMain
    ''   名称　：ドキュメント初期表示
    ''   概要  ：
    ''   引数　：なし
    ''   戻り値：True = 正常, False = 異常
    ''   作成日：2012/02/28(火)  m.suzuki
    ''   更新日：
    ''---------------------------------------------------------------------------------------------------
    ''   履歴　：2012/02/28(火)  m.suzuki  新規作成
    ''***************************************************************************************************
    '''' <summary>ドキュメント初期表示</summary>
    '''' <returns>True = 正常, False = 異常</returns>
    '''' <remarks></remarks>
    'Private Function DocumentIniMain() As Boolean

    '    Dim blnRet As Boolean = False   ' 処理結果
    '    Dim type As Type
    '    Dim dDocOperate As DocTemplateBase = Nothing

    '    Try
    '        type = type.GetType("UnionAct." & CallExcelMacro("GetClassName").ToString())
    '        If Not type Is Nothing Then
    '            dDocOperate = DirectCast(Activator.CreateInstance(type), DocTemplateBase)
    '            dDocOperate.SetIniInfo(_docInf, m_xlsAp, m_xlsWbs, m_xlsWb, m_xlsWs)
    '            dDocOperate.InitNewDocument()
    '        End If

    '        ' 処理結果に正常を設定
    '        blnRet = True

    '    Catch ex As Exception
    '        ' ログ出力（致命的エラー）
    '        log.Fatal(ex.Message)
    '        ' 致命的エラーメッセージボックス表示
    '        Call CLMsg.ShowEtarnal( _
    '            Err.Number, _
    '            Err.Description, _
    '            SCREEN_ID, _
    '            SCREEN_NAME, _
    '            System.Reflection.MethodInfo.GetCurrentMethod.Name() _
    '        )
    '    End Try

    '    ' 戻り値設定
    '    Return blnRet

    'End Function

    '***************************************************************************************************
    '   ＩＤ　：MainInsert
    '   名称　：登録メイン処理
    '   概要  ：
    '   引数　：ByVal iBlnBtn As Boolean = 処理フラグ（True：DB登録, False：文書番号の採番）
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：2015/03/04(水)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '         ：2015/03/04(水)  y.fujisaku　一時保存時にもc_period_id_Dを採番
    '***************************************************************************************************
    ''' <summary>登録メイン処理</summary>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function MainInsert(ByVal iBlnFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス

        Dim strReplace As String = ""           ' 全角英字変換後文字列

        Try
            '-------------------------------------------------------
            '   ファイル名取得
            '-------------------------------------------------------
            ' ファイル名がない場合
            ' 　１．新規登録
            '   ２．コピーして編集
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                ' 文書番号の採番ボタンが押下可ならファイルなしとみなす
                If Me.btnDocNumber.Enabled Then
                    ' 発信文書ファイル名入力画面表示処理
                    If Me.ShowFM040603(Me.strFile) = False Then
                        Return blnRet
                    End If
                End If
            End If

            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------
            '   保存先フォルダ取得
            '-------------------------------------------------------
            ' 定数マスタ詳細から取得
            If Me.GetSaveDir(clsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   保存先ファイル取得
            '-------------------------------------------------------
            ' 保存先ファイル名は、文書識別コードと文書番号（未採番の場合は空文字）が
            ' 採番されないと決められないので、後で取得

            '-------------------------------------------------------
            '   文書番号採番
            '-------------------------------------------------------
            ' 文書番号が無い場合
            ' 　１．新規作成で文書番号が採番されてない場合
            ' 　２．文書番号が採番されていない編集の場合
            ' 　３．文書番号が採番されていないコピーして編集の場合
            ' 　※表示の場合、DB登録ボタン押下不可なので、ここにはこない。

            ' 文書番号の採番ボタン押下処理は、文書番号の採番を行ってから登録
            ' DB登録ボタン押下処理の場合、文書番号の有無チェックを行って
            ' 文書番号が無い場合、一時保存扱いになる旨のメッセージを事前に表示してからここにくる

            ' 処理フラグが False（文書番号の採番）の場合
            If iBlnFlg = False Then
                ' 文書番号の採番ボタンが押下可なら未採番とみなし文書番号採番処理へ
                If Me.btnDocNumber.Enabled Then
                    ' 文書番号採番処理
                    If Me.GetNewDocNumber(
                        clsDb,
                        Me.strPeriodIdD,
                        Me.strDocNumber,
                        Me.strDocNumberD
                    ) = False Then    '2012/06/27 追加
                        'Me.strDocNumber) = False Then
                        Return blnRet
                    End If
                End If
            Else
                ' 和暦から「c_period_id_D」を算出
                Dim strRet As String
                Dim intPos As Integer
                Dim pvr As Globalization.CultureInfo = New Globalization.CultureInfo("ja-JP")
                pvr.DateTimeFormat.Calendar = New System.Globalization.JapaneseCalendar
                strRet = System.DateTime.Now().Date.ToString("gyy年", pvr)    '"平成27年"の数字部分だけ切り出す
                intPos = strRet.IndexOf("年"c)
                Me.strPeriodIdD = strRet.Substring(2, strRet.Length - intPos + 1).PadLeft(2, "0")
            End If

            '-------------------------------------------------------
            '   文書識別コード採番
            '-------------------------------------------------------
            ' 　１．新規登録
            ' 　２．コピーして編集
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                ' 文書番号の採番ボタンが押下可なら文書識別コードが未採番とみなす
                If Me.btnDocNumber.Enabled Then
                    ' 文書識別コード採番処理
                    If Me.GetNewDocId(
                        clsDb,
                        Me.strDocCode,
                        Me.strPeriodId,
                        Me.intDocId
                    ) = False Then
                        Return blnRet
                    End If
                End If
            End If

            ' 採番した文書コード反映
            ' 処理フラグが False（文書番号の採番）の場合
            If iBlnFlg = False Then
                Me.ExcelSetDocNumber(
                    StrConv(Me.strDocCode, VbStrConv.Wide),
                    PublicCommand.ConvertHanToZen(Me.intPeriod.ToString),
                    PublicCommand.ConvertHanToZen(Me.strPeriodIdD.ToString),
                    PublicCommand.ConvertHanToZen(Me.strDocNumber),
                    PublicCommand.ConvertHanToZen(Me.strDocNumberD)
                )
                'PublicCommand.ConvertHanToZen(Me.strDocNumber)) '2012/06/27
            End If

            '-------------------------------------------------------
            '   保存先ファイル取得
            '-------------------------------------------------------
            ' 文書識別コード + "_" + 管理コード + "_" + 期ID + "_" + 文書番号（採番済みの場合） + ".xls"（Excel拡張子）
            mStrSaveFile =
                Me.intDocId.ToString() _
                & "_" _
                & Me.strDocCode _
                & "_" _
                & Me.strPeriodId _
                & "_" _
                & Me.strDocNumber & STR_EXTENSION_EXCEL

            ''-------------------------------------------------------
            ''   エクセルファイル別名保存処理
            ''-------------------------------------------------------
            'If Me.ExcelFileNameChangeLocal(mStrWorkDirTo & Me.strFile) = False Then
            '    Return blnRet
            'End If
            'mStrWorkFileTo = Me.strFile

            '-------------------------------------------------------
            '   文書ID（ファイルフルパス）取得
            '-------------------------------------------------------
            ' 文書識別コード + "_" + 管理コード + "_" + 期ID + "_" + 文書番号（採番済みの場合） + ".xls"（Excel拡張子）
            Me.strDocId = mStrSaveDir & mStrSaveFile

            ''-------------------------------------------------------
            ''   エクセル制御前チェック処理
            ''-------------------------------------------------------
            'If Me.CheckBeforeExecute() = False Then
            '    Return blnRet
            'End If

            '-------------------------------------------------------
            '   登録更新処理
            '-------------------------------------------------------
            If Me.InsertUpdate(clsDb,
                               iBlnFlg) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   ワーク（ローカル）から保存先（サーバー）へファイルコピー
            '-------------------------------------------------------
            ' ファイルコピー処理
            If MDFile.FileCopy(
                mStrWorkDirTo & mStrWorkFileTo,
                mStrSaveDir & mStrSaveFile,
                True
            ) = False Then
                Call MessageBox.Show("ワークファイルをサーバーへコピーできませんでした。",
                     "エラー",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning,
                     MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertUpdate
    '   名称　：登録更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb  As CLAccessMdb = データベースクラス,
    '           ByVal iBlnBtn As Boolean     = 処理フラグ（True：DB登録, False：文書番号の採番）
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>登録更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iBlnBtn">処理フラグ（True：DB登録, False：文書番号の採番）</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function InsertUpdate(
        ByVal iClsDb As CLAccessMdb,
        ByVal iBlnBtn As Boolean
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim data As DataTable = Nothing

        Try
            ' トランザクション開始
            Call iClsDb.BeginTran()

            ' ステータス判定
            If Me.strStatus = STATUS_INSERT Then
                '---------------------------------------------------------------
                '   新規登録
                '---------------------------------------------------------------
                If Me.btnDocNumber.Enabled Then
                    '===================================
                    '   文書番号採番前
                    '===================================
                    ' 発信文書情報存在確認処理
                    If Me.ExistsDispatchDocument(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    ) Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("GE0052")       ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                        Return blnRet
                    End If

                    ' 発信文書情報登録処理
                    If Me.InsertDispatchDocument(
                        iClsDb
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If

                    ' 委員会更新一覧登録処理
                    If Me.InsertCoommitteeUpdateListOut(
                        iClsDb,
                        Me.strCommitteeUpdate,
                        Me.strDocumentOut
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If
                Else
                    '===================================
                    '   文書番号採番後
                    '===================================
                    ' 発信文書情報存在確認処理
                    If Me.ExistsDispatchDocument(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("GE0052")       ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                        Return blnRet
                    End If

                    ' 発信文書情報更新処理
                    If Me.UpdateDispatchDocument(
                        iClsDb
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If

                    ' 委員会更新一覧帳票出力済フラグ更新処理
                    data = Me.GetCoommitteeUpdateListOut(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    )
                    If Me.UpdateCommitteeUpdateListOutputFlag(
                        iClsDb,
                        data
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If
                End If

            ElseIf Me.strStatus = STATUS_COPY_EDIT Then
                '---------------------------------------------------------------
                '   コピーして編集
                '---------------------------------------------------------------
                If Me.btnDocNumber.Enabled Then
                    '===================================
                    '   文書番号採番前
                    '===================================
                    ' 発信文書情報存在確認処理
                    If Me.ExistsDispatchDocument(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    ) Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("GE0052")       ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                        Return blnRet
                    End If

                    ' 発信文書情報登録処理
                    If Me.InsertDispatchDocument(
                        iClsDb
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If

                    ' 委員会更新一覧登録処理
                    If Me.InsertCoommitteeUpdateListOut(
                        iClsDb,
                        Me.strCommitteeUpdate,
                        Me.strDocumentOut
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If

                Else
                    '===================================
                    '   文書番号採番後
                    '===================================
                    ' 発信文書情報存在確認処理
                    If Me.ExistsDispatchDocument(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("GE0052")       ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                        Return blnRet
                    End If

                    ' 発信文書情報更新処理
                    If Me.UpdateDispatchDocument(
                        iClsDb
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If

                    ' 委員会更新一覧帳票出力済フラグ更新処理
                    data = Me.GetCoommitteeUpdateListOut(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    )
                    If Me.UpdateCommitteeUpdateListOutputFlag(
                        iClsDb,
                        data
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If
                End If

            ElseIf Me.strStatus = STATUS_EDIT Then
                '---------------------------------------------------------------
                '   編集
                '---------------------------------------------------------------
                '===================================
                '   文書番号採番前
                '===================================
                ' 発信文書情報存在確認処理
                If Me.ExistsDispatchDocument(
                    iClsDb,
                    Me.intDocId,
                    Me.strDocCode,
                    Me.strPeriodId
                ) = False Then
                    Call iClsDb.RollbackTran()          ' トランザクション取消
                    Call CLMsg.Show("GE0052")           ' 他のユーザによって更新された可能性があります。の旨のメッセージ表示
                    Return blnRet
                End If

                ' 発信文書情報更新処理
                If Me.UpdateDispatchDocument(
                    iClsDb
                ) = False Then
                    Call iClsDb.RollbackTran()          ' トランザクション取消
                    Call CLMsg.Show("FE0001")           ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                    Return blnRet
                End If

                ' 委員会更新一覧帳票出力済フラグ更新処理
                If Not Me.btnDocNumber.Enabled Then
                    data = Me.GetCoommitteeUpdateListOut(
                        iClsDb,
                        Me.intDocId,
                        Me.strDocCode,
                        Me.strPeriodId
                    )
                    If Me.UpdateCommitteeUpdateListOutputFlag(
                        iClsDb,
                        data
                    ) = False Then
                        Call iClsDb.RollbackTran()      ' トランザクション取消
                        Call CLMsg.Show("FE0001")       ' 予期しないエラーが発生しましたの旨のエラーメッセージ表示
                        Return blnRet
                    End If
                End If

            ElseIf Me.strStatus = STATUS_SHOW Then
                '===============================================================
                '   表示
                '===============================================================
                Call MessageBox.Show(
                    "表示処理の際、ここの処理はこないハズだよ、ワトソン君！",
                    "大エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' トランザクション確定
            Call iClsDb.CommitTran()

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' トランザクション取り消し
            Call iClsDb.RollbackTran()

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetNewDocNumber
    '   名称　：文書番号採番処理
    '   概要  ：
    '   引数　：ByVal clsDb       As CLAccessMdb = データベースクラス,
    '           ByRef ioDocNumber As String      = 新規文書番号
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>文書番号採番処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="ioDocNumber">新規文書番号</param>
    ''' <param name="ioDocNumberD">新規文書番号Ｄ文書用</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function GetNewDocNumber(
        ByVal iClsDb As CLAccessMdb,
        ByRef ioPeriodIdD As String,
        ByRef ioDocNumber As String,
        ByRef ioDocNumberD As String
    ) As Boolean
        'ByRef ioDocNumber As String) As Boolean '2012/06/27 

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String                ' SQL文
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim intRet As Integer = 0           ' 処理件数
        Dim strRet As String = ""           ' ワーク変数
        Dim strRetD As String = ""          ' ワーク変数 2012/06/27 Ｄ文書番号追加

        Try
            ' SQL文作成 '2012/06/27 Ｄ文書番号追加
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT (IIF(MAX(CLng(a.s_doc_number)) IS NULL, 0, MAX(CLng(a.s_doc_number))) + 1 ) AS s_doc_number" & vbCrLf
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf
            strSql = strSql & "  WHERE a.s_doc_number IS NOT NULL" & vbCrLf
            strSql = strSql & "    AND a.s_doc_number <> ''" & vbCrLf
            strSql = strSql & "    AND a.c_doc_code  = '" & Me.strDocCode & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id = '" & Me.strPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 1 Then
                ' 1件の場合
                If IsDBNull(dtRet.Rows(0).Item(0)) Then
                    ' "1" を文書番号として設定
                    strRet = "1"
                Else
                    ' 採番した文書番号設定
                    strRet = dtRet.Rows(0).Item(0).ToString()
                End If
            Else
                ' その他：ありえないけど、とりあえず
                Call MessageBox.Show(
                    "文書番号が採番できませんでした。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 文書番号設定（2桁左0埋め）
            ioDocNumber = strRet.PadLeft(2, "0")

            ' 種別Dの時、期を跨いで01-12月を平成XX年としてc_period_id_Dとs_doc_number_Dを採番
            If Me.strDocCode = "D" Then
                If Me.GetNewDocNumberD(
                    iClsDb,
                    ioPeriodIdD,
                    ioDocNumber,
                    ioDocNumberD) _
                = False Then
                    Return blnRet
                End If
            Else
                ioDocNumberD = "00"        ' 2012/06/27
            End If
            '            strRetD = dtRet.Rows(0).Item(1).ToString()    ' 2012/06/27
            '            ioDocNumberD = strRetD.PadLeft(2, "0")        ' 2012/06/27

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID, SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetNewDocNumberD
    '   名称　：文書番号Ｄ文書期変更処理
    '   概要  ：
    '   引数　：ByVal clsDb       As CLAccessMdb = データベースクラス,
    '           ByRef ioDocNumber As String      = 新規文書番号
    '   戻り値：True：正常, False：異常
    '   作成日：2012/06/29(金)  k.shouji
    '   更新日：2015/03/02(月)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/06/29(金)  k.shouji  新規作成
    '       　：2015/03/02(月)  y.Fujisaku  和暦の独立採番として、仕様を満たすように作り直し
    '***************************************************************************************************
    ''' <summary>文書番号採番処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="ioDocNumberD">新規文書番号Ｄ文書用</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function GetNewDocNumberD(
        ByVal iClsDb As CLAccessMdb,
        ByRef ioPeriodIdD As String,
        ByRef ioDocNumber As String,
        ByRef ioDocNumberD As String
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String                ' SQL文
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim intRet As Integer = 0           ' 処理件数
        Dim strRet1 As String = ""          ' ワーク変数
        Dim strRet2 As String = ""          ' ワーク変数
        Dim intPos As Integer

        Try
            ' 和暦から「c_period_id_D」を算出
            Dim pvr As Globalization.CultureInfo = New Globalization.CultureInfo("ja-JP")
            pvr.DateTimeFormat.Calendar = New System.Globalization.JapaneseCalendar
            strRet1 = System.DateTime.Now().Date.ToString("gyy年", pvr)    '"平成27年"の数字部分だけ切り出す
            intPos = strRet1.IndexOf("年"c)
            ioPeriodIdD = strRet1.Substring(2, strRet1.Length - intPos + 1).PadLeft(2, "0")

            ' c_period_id_Dを元に、同年内のs_doc_number_D最大値+1取得
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT (IIF(MAX(a.s_doc_number_D) IS NULL, 0, MAX(a.s_doc_number_D)) + 1 ) AS s_doc_number_D_next" & vbCrLf
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_period_id_D = '" & ioPeriodIdD & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 1 Then
                ' 1件の場合
                If IsDBNull(dtRet.Rows(0).Item(0)) Then
                    ' "1" を文書番号として設定
                    strRet2 = "1"
                Else
                    ' 採番した文書番号設定
                    strRet2 = dtRet.Rows(0).Item(0).ToString()
                End If
            Else
                ' その他：ありえないけど、とりあえず
                Call MessageBox.Show(
                    "文書番号が採番できませんでした。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 2桁左0埋め
            ioDocNumberD = strRet2.PadLeft(2, "0")

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetNewDocId
    '   名称　：文書識別コード採番処理
    '   概要  ：
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iStrDocCodde As String      = 管理コード,
    '           ByVal iStrPeriodId As String      = 期ID,
    '           ByRef ioIntNewDocId As Integer    = 採番した文書識別コード
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>文書識別コード採番処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="ioIntNewDocId">採番した文書識別コード</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function GetNewDocId(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrDocCode As String,
        ByVal iStrPeriodId As String,
        ByRef ioIntNewDocId As Integer
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果
        Dim strSql As String            ' SQL文作成
        Dim dtRet As New DataTable      ' 処理結果格納データテーブル
        Dim intRet As Integer = 0       ' 処理件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT (IIF(MAX(a.c_doc_id) IS NULL, 0, MAX(a.c_doc_id)) + 1) AS c_doc_id" & vbCrLf
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_doc_code  = '" & iStrDocCode & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 1 Then
                ' 1件の場合
                If IsDBNull(dtRet.Rows(0).Item(0)) Then
                    ' 1 を文書識別コードとして設定
                    ioIntNewDocId = 1
                Else
                    ' 採番した文書識別コード設定
                    ioIntNewDocId = CInt(dtRet.Rows(0).Item(0))
                End If
            Else
                ' その他：ありえないけど、とりあえず
                Call MessageBox.Show(
                    "文書識別コードが採番できませんでした",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ShowFM040603
    '   名称　：発信文書ファイル名入力画面表示処理
    '   概要  ：
    '   引数　：ByRef ioFileName As String = ファイル名
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書ファイル名入力画面表示</summary>
    ''' <param name="ioFileName">ファイル名</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ShowFM040603(
        ByRef ioFileName As String
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsFM040603 As FM040603 = Nothing   ' 発信文書ファイル名入力画面クラス
        Dim diaRet As DialogResult = Nothing    ' メッセージボックス押下結果

        Try
            ' 発信文書ファイル名入力画面クラス生成
            clsFM040603 = New FM040603

            ' 発信文書ファイル名入力画面表示
            diaRet = clsFM040603.ShowDialog()

            ' ダイアログ結果判定
            If diaRet = Windows.Forms.DialogResult.Cancel Then
                Return blnRet
            End If

            ' ファイル名設定
            ioFileName = clsFM040603.strFileName

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            If Not clsFM040603 Is Nothing Then
                ' 発信文書ファイル名入力画面クラス解放
                clsFM040603.Dispose()
            End If

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetWorkDir
    '   名称　：ワークフォルダ取得処理
    '   概要  ：ワークフォルダを取得する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/18(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/18(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ワークフォルダ取得処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function GetWorkDir(
        ByVal clsDb As CLAccessMdb
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String                ' SQL
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim intRet As Integer = 0           ' 処理件数
        Dim strRet As String = ""           ' 処理結果パス
        Dim strNow As String = ""           ' 現在時刻（yyyyMMdd）

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT l_name" & vbCrLf
            strSql = strSql & "   FROM constant_dtl" & vbCrLf
            strSql = strSql & "  WHERE c_constant = 'ISSUE_DOCUMENT_WORK'" & vbCrLf
            strSql = strSql & "    AND c_constant_seq = '01'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 0 Then
                Call MessageBox.Show(
                    "発行文書作業先が取得できません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' ワークフォルダ取得
            mStrWorkDir = dtRet.Rows(0).Item(0).ToString()
            ' 対象のディレクトリを指定
            Dim baseTempDir = MDFile.GetDirPath(mStrWorkDir)
            Dim namaHaed As String = MDFile.GetName(mStrWorkDir, False)
            Dim dirs = MDFile.GetDirs(baseTempDir, namaHaed + "*", SearchOption.TopDirectoryOnly)

            strNow = Now.ToString("yyyyMMdd")
            mStrWorkDirTo = mStrWorkDir & strNow & "\"

            For Each dir As String In dirs
                MDFile.DirDelete(dir, True)
                If dir + "\" = mStrWorkDirTo Then
                    Dim n As Integer = 0
                    For i = 0 To 10
                        If MDFile.ExistDir(mStrWorkDirTo) Then
                            n += 1
                            mStrWorkDirTo = mStrWorkDir & strNow & n & "\"
                        Else
                            Exit For
                        End If
                    Next i
                    Continue For
                End If
            Next

            ' 下1桁が "\" ではない場合、"\" を付与
            If mStrWorkDir.Substring(mStrWorkDir.Length - 1, 1) <> "\" Then
                mStrWorkDir = mStrWorkDir & "\"
            End If

            ' フォルダ存在チェック
            If MDFile.DirExists(mStrWorkDir) Then
                ' フォルダ作成処理
                If MDFile.DirCreate(mStrWorkDir) = False Then
                    Return blnRet
                End If
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSaveDir
    '   名称　：保存先フォルダ取得処理
    '   概要  ：保存先フォルダを取得する。
    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/18(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/18(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>保存先フォルダ取得処理</summary>
    ''' <param name="clsDb">データベースクラス</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function GetSaveDir(
        ByVal clsDb As CLAccessMdb
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String                ' SQL
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        Dim intRet As Integer = 0           ' 処理件数

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT l_name" & vbCrLf
            strSql = strSql & "   FROM constant_dtl" & vbCrLf
            strSql = strSql & "  WHERE c_constant = 'ISSUE_DOCUMENT_SAVE'" & vbCrLf
            strSql = strSql & "    AND c_constant_seq = '01'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet = 0 Then
                Call MessageBox.Show(
                    "発行文書保存先が取得できません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 保存先フォルダ取得
            mStrSaveDir = dtRet.Rows(0).Item(0).ToString()

            ' 下1桁が "\" ではない場合、"\" を付与
            If mStrSaveDir.Substring(mStrSaveDir.Length - 1, 1) <> "\" Then
                mStrSaveDir = mStrSaveDir & "\"
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelFileNameChangeLocal
    '   名称　：エクセルファイル別名保存
    '   概要  ：
    '   引数　：ByVal NewFilename As String = 保存するファイル名
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>エクセルファイル別名保存</summary>
    ''' <param name="NewFilename">保存するファイル名</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelFileNameChangeLocal(
        ByVal NewFilename As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果 

        Try
            ' ファイル名が重複しないように同名のファイルは削除する
            System.IO.File.Delete(NewFilename)

            ' 保存
            Me.CallExcelMacro("DocEvent.SaveAsCommand", New Object() {NewFilename})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetFormatDate
    '   名称　：発行日フォーマット変換処理
    '   概要  ：
    '   引数　：ByVal iDatApplyDate As DateTime = 発行日,
    '           ByVal iBlnWarekiFlg As Boolean  = 和暦フラグ,
    '           ByRef ioStrDate     As String   = 変換後の発行日
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発行日フォーマット変換処理</summary>
    ''' <param name="iDatApplyDate">発行日</param>
    ''' <param name="iBlnWarekiFlg">和暦フラグ</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function GetFormatDate(
        ByVal iDatApplyDate As DateTime,
        ByVal iBlnWarekiFlg As Boolean,
        ByRef ioStrDate As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果
        Dim pvr As Globalization.CultureInfo = New Globalization.CultureInfo("ja-JP")

        Try
            ' 和暦チェックボックス判定
            If iBlnWarekiFlg Then
                pvr.DateTimeFormat.Calendar = New System.Globalization.JapaneseCalendar
                ' 和暦：和暦チェックボックスチェック有
                ioStrDate = PublicCommand.ConvertHanToZen(iDatApplyDate.ToString("gyy年 MM月 dd日", pvr))
            Else
                ' 西暦：和暦チェックボックスチェック無
                ioStrDate = PublicCommand.ConvertHanToZen(iDatApplyDate.ToString("yyyy年 MM月 dd日"))
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock() As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果

        Try
            '-----------------------------------------------
            '   コントロール表示・非表示設定
            '-----------------------------------------------
            If Me.strStatus = STATUS_INSERT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                '===========================================
                '   新規登録・コピーして編集
                '===========================================
                ' 文書番号
                Me.grpDocumentNo.Enabled = True             ' 文書番号グループボックス
                Me.btnDocNumber.Enabled = True              ' 文書番号の採番ボタン
                ' 発行日
                Me.grpIssueDate.Enabled = True              ' 発行日グループボックス
                Me.dtpIssueDate.Enabled = True              ' 発行日コンボボックス
                Me.chkWareki.Enabled = True                 ' 和暦チェックボックス

                Me.btnApply.Enabled = True                  ' 発行日適用ボタン
                Me.btnInsertDb.Enabled = True               ' DB登録ボタン

            ElseIf Me.strStatus = STATUS_EDIT Then
                '===========================================
                '   編集
                '===========================================
                ' 文書番号
                If MDChk.ChkNull(Me.strDocNumber) Then
                    ' 文書番号採番無し
                    Me.grpDocumentNo.Enabled = True         ' 文書番号グループボックス
                    Me.btnDocNumber.Enabled = True          ' 文書番号の採番ボタン
                Else
                    ' 文書晩後採番有り
                    Me.grpDocumentNo.Enabled = False        ' 文書番号グループボックス
                    Me.btnDocNumber.Enabled = False         ' 文書番号の採番ボタン
                End If
                ' 発行日
                Me.grpIssueDate.Enabled = True              ' 発行日グループボックス
                Me.dtpIssueDate.Enabled = True              ' 発行日コンボボックス
                Me.chkWareki.Enabled = True                 ' 和暦チェックボックス

                Me.btnApply.Enabled = True                  ' 発行日適用ボタン
                Me.btnInsertDb.Enabled = True               ' DB登録ボタン

            ElseIf Me.strStatus = STATUS_SHOW Then
                '===========================================
                '   表示
                '===========================================
                ' 文書番号
                Me.grpDocumentNo.Enabled = False            ' 文書番号グループボックス
                Me.btnDocNumber.Enabled = False             ' 文書番号の採番ボタン
                ' 発行日
                Me.grpIssueDate.Enabled = False             ' 発行日グループボックス
                Me.dtpIssueDate.Enabled = False             ' 発行日コンボボックス
                Me.chkWareki.Enabled = False                ' 和暦チェックボックス

                Me.btnApply.Enabled = False                 ' 発行日適用ボタン
                Me.btnInsertDb.Enabled = False              ' DB登録ボタン

            End If

            ' 総合OAに戻るボタンは常に表示
            Me.btnBack.Enabled = True

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：CheckBeforeExecute
    '   名称　：エクセル制御前チェック
    '   概要  ：
    '   引数　：なし
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>エクセル制御前チェック</summary>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function CheckBeforeExecute() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            Me.SelectRange("A1:A1")
            If Me.GetIsOutMsgBox Then
                Call CLMsg.Show("OE0002")
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SelectRange
    '   名称　：エクセル選択
    '   概要  ：
    '   引数　：ByVal strRange As String = 
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>エクセル選択</summary>
    ''' <param name="strRange"></param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function SelectRange(
        ByVal strRange As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            Me.CallExcelMacro("DocCommand.SelectRange", New Object() {strRange})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSubjectCell
    '   名称　：標題出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 日付出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>標題出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">標題出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function GetSubjectCell(
        ByRef ioStrCell As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 標題出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetSubjectCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetIssueDateCell
    '   名称　：日付出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 日付出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>日付出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">日付出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function GetIssueDateCell(
        ByRef ioStrCell As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 日付出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetIssueDateCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeUpdateIdCell
    '   名称　：委員会変更ID出力位置セル取得処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 委員会変更ID出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>委員会変更ID出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">委員会変更ID出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function GetCommitteeUpdateIdCell(
        ByRef ioStrCell As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetMemberTblCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetIsOutMsgBox
    '   名称　：メッセージ表示取得
    '   概要  ：
    '   引数　：なし
    '   戻り値：True：, False：
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>メッセージ表示取得</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetIsOutMsgBox() As Boolean

        Try
            Dim obj2 As Object = Me.CallExcelMacro("DocCommand.GetIsOutMsgBox")
            Return ((Not obj2 Is Nothing) AndAlso CBool(obj2))

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ConvertHanToZen
    '   名称　：半角英字→全角英字変換処理
    '   概要  ：
    '   引数　：ByVal iStrBefore As String = 変換前半角英字文字列,
    '           ByRef ioStrAfter As String = 変換後全角英字文字列
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>英字半角→英字全角変換処理</summary>
    ''' <param name="iStrBefore">変換前半角英字文字列</param>
    ''' <param name="ioStrAfter">変換後全角英字文字列</param>
    ''' <returns>変換後の全角英字文字列</returns>
    ''' <remarks></remarks>
    Public Function ConvertHanToZen(
        ByVal iStrBefore As String,
        ByRef ioStrAfter As String
    ) As String

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            Dim str2 As String = String.Copy(iStrBefore)
            Dim chArray As Char() = New Char() {"Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ", "Ｇ", "Ｈ", "Ｉ", "Ｊ", "Ｋ", "Ｌ", "Ｍ", "Ｎ", "Ｏ", "Ｐ", "Ｑ", "Ｒ", "Ｓ", "Ｔ", "Ｕ", "Ｖ", "Ｗ", "Ｘ", "Ｙ", "Ｚ"}
            Dim chArray2 As Char() = New Char() {"A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c}
            Dim i As Integer = 0
            Do While ((i < chArray.Length) AndAlso (i < chArray2.Length))
                str2 = str2.Replace(chArray2(i), chArray(i))
                i += 1
            Loop

            ' 変換後全角英字文字列設定
            ioStrAfter = str2

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetCellData
    '   名称　：Excelセル位置データ設定処理
    '   概要  ：
    '   引数　：ByVal strCell As String = 文字列を設定するセル位置,
    '           ByVal strData As String = セル位置に設定する文字列
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：2015/03/20(金)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '       　：2015/03/20(金)  y.fujisaku  操作中「適用」押下時のエラー回避
    '***************************************************************************************************
    ''' <summary>Excelセル位置データ設定処理</summary>
    ''' <param name="strCell">文字列を設定するセル位置</param>
    ''' <param name="strData">セル位置に設定する文字列</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelSetCellData(
        ByVal strCell As String,
        ByVal strData As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 
            If PublicCommand.StrnumToInt(strCell) <> 0 Then
                ' Excelマクロ実行
                Me.CallExcelMacro("DocCommand.SetDataAppointCellStr", New Object() {strCell, strData})
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            'Call CLMsg.ShowEtarnal( _
            '    Err.Number, _
            '    Err.Description, _
            '    SCREEN_ID, _
            '    SCREEN_NAME, _
            '    System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            ')

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetDocNumberCell
    '   名称　：ドキュメント番号セル取得
    '   概要  ：
    '   引数　：ByRef ioStrDocNumber As String = ドキュメント番号セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ドキュメント番号セル取得処理</summary>
    ''' <param name="ioStrDocNumber">ドキュメント番号セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function GetDocNumberCell(
        ByRef ioStrDocNumber As String
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ioStrDocNumber = Me.CallExcelMacro("DocConst.GetDocNumberCell", New Object(0 - 1) {}).ToString

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：CallExcelMacro
    '   名称　：マクロ起動処理
    '   概要  ：マクロを起動する。
    '   引数　：ByVal strMacro As String = マクロ名,
    '           Optional ByVal pParam As Object() = Nothing = パラメータ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>マクロ起動処理</summary>
    ''' <param name="strMacro">マクロ名</param>
    ''' <param name="pParam">パラメータ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Public Function CallExcelMacro(
        ByVal strMacro As String,
        Optional ByVal pParam As Object() = Nothing
    ) As Object

        Dim obj4 As Object = New Object()

        Try
            If pParam Is Nothing OrElse pParam.Length = 0 Then
                obj4 = Me.mXlsAp.Run(strMacro)
            ElseIf pParam.Length = 1 Then
                obj4 = Me.mXlsAp.Run(strMacro, pParam(0))
            ElseIf pParam.Length = 2 Then
                obj4 = Me.mXlsAp.Run(strMacro, pParam(0), pParam(1))
            End If

        Catch exception As COMException
            Dim errorCode As Integer = exception.ErrorCode
            If (errorCode <= -2147352560) Then
                If (errorCode <= -2147417851) Then
                    Select Case errorCode
                        Case -2147418111
                            CLMsg.Show("OE0002")
                        Case -2147417851
                            CLMsg.Show("OE0002")
                    End Select
                ElseIf ((errorCode <> -2147417848) AndAlso (errorCode = -2147352560)) Then
                    Return Nothing
                End If
            Else
                Select Case errorCode
                    Case -2146827284
                        MessageBox.Show("Excel : " & "マクロ「" & strMacro & "」が見つからないか、セルが編集中のため、マクロ起動が出来ません。")
                    Case -2146788248
                        MessageBox.Show("Excel : " & "オブジェクト参照が設定されていません。")
                    Case -2146777998
                        CLMsg.Show("OE0002")
                    Case -2147417851
                        CLMsg.Show("OE0001")
                End Select
            End If
            Throw
        End Try

        Return obj4

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetAddinFileName
    '   名称　：アドインファイル名取得処理
    '   概要  ：アドインファイル名を取得する。
    '   引数　：ByRef ioStrFileFullPath As String = アドインファイル名
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>アドインファイル名取得</summary>
    ''' <param name="ioStrFileFullPath">アドインファイル名</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetAddinFileName(
        ByRef ioStrFileFullPath As String
    ) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb      ' データベースクラス
        Dim strSql As String = ""                       ' SQL
        Dim dtRet As DataTable = Nothing                ' 処理結果データテーブル
        Dim intRet As Integer = 0                       ' 処理件数

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.s_doc_id" & vbCrLf                 ' 文書ID
            strSql = strSql & "   FROM dispatch_template AS a" & vbCrLf     ' 発信テンプレートテーブル
            strSql = strSql & "  WHERE a.k_extension = 'TPL_XLA'" & vbCrLf  ' 拡張子種別が 'TPL_XLA' のもの
            strSql = strSql & "    AND a.c_template  = 'ADD'" & vbCrLf      ' テンプレート区分が 'ADD' のもの
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If intRet <> 1 Then
                Call CLMsg.Show("DE0013", "テンプレート")
                Return blnRet
            End If

            ' 処理結果にファイル名を設定
            ioStrFileFullPath = dtRet.Rows(0).Item(0).ToString()

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsDispatchDocument
    '   名称　：発信文書情報存在確認処理
    '   概要  ：発信文書情報が存在するか確認する。
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iIntDocId    As Integer     = 文書識別コード,
    '           ByVal iStrDocCode  As String      = 管理コード,
    '           ByVal iStrPeriodId As String      = 期ID
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書情報存在確認処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iIntDocId">文書識別コード</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsDispatchDocument(
        ByVal iClsDb As CLAccessMdb,
        ByVal iIntDocId As Integer,
        ByVal iStrDocCode As String,
        ByVal iStrPeriodId As String
    ) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL
        Dim dtRet As DataTable = Nothing                ' 処理結果データテーブル
        Dim intRet As Integer = 0                       ' 処理件数

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_doc_id" & vbCrLf                             ' 文書識別コード
            strSql = strSql & "       ,a.c_doc_code" & vbCrLf                           ' 管理コード
            strSql = strSql & "       ,a.c_period_id" & vbCrLf                          ' 期ID
            strSql = strSql & "   FROM dispatch_document AS a" & vbCrLf                 ' 発信文書
            strSql = strSql & "  WHERE a.c_doc_id    = " & iIntDocId & vbCrLf           ' 文書識別コードと同じもの
            strSql = strSql & "    AND a.c_doc_code  = '" & iStrDocCode & "'" & vbCrLf  ' 管理コードと同じもの
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf ' 期IDと同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 処理件数チェック
            If dtRet.Rows.Count = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateDispatchDocument
    '   名称　：発信文書情報更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '   備考　：※ 新規作成で文書番号採番後、コピーして編集で文書番号採番後、
    '              編集で文書番号採番前・後の処理
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書情報更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateDispatchDocument(
        ByVal iClsDb As CLAccessMdb
    ) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            '---------------------------------------------------------------
            '   SQL文作成
            '---------------------------------------------------------------
            If Me.strStatus = STATUS_INSERT Then
                '===================================
                '   新規作成
                '===================================
                ' 文書番号採番後のみ
                If Me.btnDocNumber.Enabled = False Then
                    strSql = "" & vbCrLf
                    strSql = strSql & " UPDATE dispatch_document" & vbCrLf                                      ' 発信文書
                    strSql = strSql & "    SET s_doc_number  = '" & Me.strDocNumber & "'           " & vbCrLf   ' 採番した文書番号
                    strSql = strSql & "      ,c_period_id_D  = '" & Me.strPeriodIdD & "'           " & vbCrLf   ' 期IDＤ文書用　2012/06/27 追加
                    strSql = strSql & "      ,s_doc_number_D = '" & Me.strDocNumberD & "'          " & vbCrLf   ' 採番した文書番号Ｄ文書用　2012/06/27 追加
                    strSql = strSql & "       ,d_issue       = '" & Me.strIssueDate & "'           " & vbCrLf   ' 発行日
                    strSql = strSql & "       ,l_subject     = '" & Me.strSubject & "'             " & vbCrLf   ' 標題
                    strSql = strSql & "       ,d_up          = '" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf   ' 更新日
                    strSql = strSql & "       ,c_user_id_up  = '" & MDLoginInfo.UserId & "'        " & vbCrLf   ' 更新者
                    strSql = strSql & "  WHERE c_doc_id      =  " & Me.intDocId & "                " & vbCrLf   ' 採番した文書識別コードと同じもの
                    strSql = strSql & "    AND c_doc_code    = '" & Me.strDocCode & "'             " & vbCrLf   ' 管理コードと同じもの
                    strSql = strSql & "    AND c_period_id   = '" & Me.strPeriodId & "'            " & vbCrLf   ' 期IDと同じもの
                    strSql = strSql & ";" & vbCrLf
                End If
            ElseIf Me.strStatus = STATUS_EDIT Then
                '===================================
                '   編集
                '===================================
                ' 文書番号採番後
                strSql = "" & vbCrLf
                strSql = strSql & " UPDATE dispatch_document" & vbCrLf                                      ' 発信文書
                strSql = strSql & "    SET s_doc_number  = '" & Me.strDocNumber & "'           " & vbCrLf   ' 採番した文書番号
                strSql = strSql & "      ,c_period_id_D  = '" & Me.strPeriodIdD & "'           " & vbCrLf   ' 期IDＤ文書用　2012/06/27 追加
                strSql = strSql & "      ,s_doc_number_D = '" & Me.strDocNumberD & "'          " & vbCrLf   ' 採番した文書番号Ｄ文書用　2012/06/27 追加
                strSql = strSql & "       ,d_issue       = '" & Me.strIssueDate & "'           " & vbCrLf   ' 発行日
                strSql = strSql & "       ,s_doc_id      = '" & Me.strDocId & "'               " & vbCrLf   ' 文書ID（フルパス）
                strSql = strSql & "       ,l_subject     = '" & Me.strSubject & "'             " & vbCrLf   ' 標題
                strSql = strSql & "       ,d_up          = '" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf   ' 更新日
                strSql = strSql & "       ,c_user_id_up  = '" & MDLoginInfo.UserId & "'        " & vbCrLf   ' 更新者
                strSql = strSql & "  WHERE c_doc_id      =  " & Me.intDocId & "                " & vbCrLf   ' 文書識別コードと同じもの
                strSql = strSql & "    AND c_doc_code    = '" & Me.strDocCode & "'             " & vbCrLf   ' 管理コードと同じもの
                strSql = strSql & "    AND c_period_id   = '" & Me.strPeriodId & "'            " & vbCrLf   ' 期IDと同じもの
                strSql = strSql & ";" & vbCrLf
            ElseIf Me.strStatus = STATUS_COPY_EDIT Then
                '===================================
                '   コピーして編集
                '===================================
                If Me.btnDocNumber.Enabled = False Then
                    ' 文書番号採番後のみ
                    strSql = "" & vbCrLf
                    strSql = strSql & " UPDATE dispatch_document" & vbCrLf                                      ' 発信文書
                    strSql = strSql & "    SET s_doc_number  = '" & Me.strDocNumber & "'           " & vbCrLf   ' 採番した文書番号
                    strSql = strSql & "      ,c_period_id_D  = '" & Me.strPeriodIdD & "'           " & vbCrLf   ' 期IDＤ文書用　2012/06/27 追加
                    strSql = strSql & "      ,s_doc_number_D = '" & Me.strDocNumberD & "'          " & vbCrLf   ' 採番した文書番号Ｄ文書用　2012/06/27 追加
                    strSql = strSql & "       ,d_issue       = '" & Me.strIssueDate & "'           " & vbCrLf   ' 発行日
                    strSql = strSql & "       ,l_subject     = '" & Me.strSubject & "'             " & vbCrLf   ' 標題
                    strSql = strSql & "       ,d_up          = '" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf   ' 更新日
                    strSql = strSql & "       ,c_user_id_up  = '" & MDLoginInfo.UserId & "'        " & vbCrLf   ' 更新者
                    strSql = strSql & "  WHERE c_doc_id      =  " & Me.intDocId & "                " & vbCrLf   ' 採番した文書識別コードと同じもの
                    strSql = strSql & "    AND c_doc_code    = '" & Me.strDocCode & "'             " & vbCrLf   ' 管理コードと同じもの
                    strSql = strSql & "    AND c_period_id   = '" & Me.strPeriodId & "'            " & vbCrLf   ' 期IDと同じもの
                    strSql = strSql & ";" & vbCrLf
                End If
            End If

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理件数チェック
            If intRet = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertDispatchDocument
    '   名称　：発信文書情報登録処理
    '   概要  ：
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '   備考　：※ 新規登録の文書番号採番前・後、コピーして編集の文書番号採番前・後の処理
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書情報登録処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertDispatchDocument(
        ByVal iClsDb As CLAccessMdb
    ) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " INSERT INTO dispatch_document (" & vbCrLf
            strSql = strSql & "     c_doc_id     " & vbCrLf                         ' 01. 文書識別コード
            strSql = strSql & "    ,c_doc_code   " & vbCrLf                         ' 02. 管理コード
            strSql = strSql & "    ,c_period_id  " & vbCrLf                         ' 03. 期ID
            strSql = strSql & "    ,s_doc_number " & vbCrLf                         ' 04. 文書番号
            strSql = strSql & "    ,c_period_id_D" & vbCrLf                         ' 03. 期ID    (Ｄ文書用) 2012/06/27 追加
            strSql = strSql & "   ,s_doc_number_D" & vbCrLf                         ' 05. 文書番号(Ｄ文書用) 2012/06/27 追加
            strSql = strSql & "    ,s_subject_seq" & vbCrLf                         ' 06. 標題枝番
            strSql = strSql & "    ,c_template   " & vbCrLf                         ' 07. テンプレート区分
            strSql = strSql & "    ,s_doc_id     " & vbCrLf                         ' 08. 文書ID（文書フルパス）
            strSql = strSql & "    ,l_file       " & vbCrLf                         ' 09. ファイル名
            strSql = strSql & "    ,d_issue      " & vbCrLf                         ' 10. 発行日
            strSql = strSql & "    ,l_subject    " & vbCrLf                         ' 11. 標題
            strSql = strSql & "    ,l_biko       " & vbCrLf                         ' 12. 備考
            strSql = strSql & "    ,d_ins        " & vbCrLf                         ' 13. 作成日
            strSql = strSql & "    ,c_user_id_ins" & vbCrLf                         ' 14. 作成者個人ID
            strSql = strSql & "    ,d_up         " & vbCrLf                         ' 15. 更新日
            strSql = strSql & "    ,c_user_id_up " & vbCrLf                         ' 16. 更新者個人ID
            strSql = strSql & " ) VALUES (" & vbCrLf
            strSql = strSql & "      " & Me.intDocId & "                " & vbCrLf  ' 採番した文書識別コード
            strSql = strSql & "    ,'" & Me.strDocCode & "'             " & vbCrLf  ' 管理コード
            strSql = strSql & "    ,'" & Me.strPeriodId & "'            " & vbCrLf  ' 期ID
            ' ↓　文書番号採番前は空文字、採番後は採番された番号が文書番号に格納
            strSql = strSql & "    ,'" & Me.strDocNumber & "'           " & vbCrLf  ' 文書番号
            strSql = strSql & "    ,'" & Me.strPeriodIdD & "'            " & vbCrLf  ' 期ID(Ｄ文書用)     2012/06/27 追加
            strSql = strSql & "    ,'" & Me.strDocNumberD & "'          " & vbCrLf  ' 文書番号(Ｄ文書用)  2012/06/27 追加
            strSql = strSql & "    , " & Me.intSubjectSeq & "           " & vbCrLf  ' 標題枝番
            strSql = strSql & "    ,'" & Me.strTemplate & "'            " & vbCrLf  ' テンプレート区分
            strSql = strSql & "    ,'" & Me.strDocId & "'               " & vbCrLf  ' 文書ID（文書フルパス）
            strSql = strSql & "    ,'" & Me.strFile & "'                " & vbCrLf  ' ファイル名
            strSql = strSql & "    ,'" & Me.strIssueDate & "'           " & vbCrLf  ' 発行日
            strSql = strSql & "    ,'" & Me.strSubject & "'             " & vbCrLf  ' 標題
            strSql = strSql & "    ,''                                  " & vbCrLf  ' 備考
            strSql = strSql & "    ,'" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf  ' 登録日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'        " & vbCrLf  ' 登録者
            strSql = strSql & "    ,'" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf  ' 更新日
            strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'        " & vbCrLf  ' 更新者
            strSql = strSql & ");" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理件数チェック
            If intRet = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetWorkDirFile
    '   名称　：ワークフォルダ・ファイル名取得処理
    '   概要  ：ワークフォルダ名・ファイル名を取得する。
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ワークフォルダ・ファイル名取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetWorkDirFile(
        ByVal iClsDb As CLAccessMdb
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '=======================================================================
            '   ワークTo名取得
            '=======================================================================
            '　ワークフォルダTo名
            If Me.GetWorkDir(iClsDb) = False Then
                Return blnRet
            End If

            ' ワークファイルTo名
            If Me.strStatus = STATUS_INSERT Then
                '-----------------------------------
                '   新規作成
                '-----------------------------------
                ' 発信文書新規作成画面で選択した標題に対してのテンプレートファイル名
                mStrWorkFileTo = Me.strTemplateFile
            ElseIf Me.strStatus = STATUS_SHOW _
            Or Me.strStatus = STATUS_EDIT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                '-----------------------------------
                '   表示・編集・コピーして編集
                '-----------------------------------
                ' 発信文書検索画面でで表示しているファイル名は、実際のファイル名ではないので
                ' 実際のファイル名が格納されている文書ID（文書フルパス）からファイル名のみを取得する。
                If MDFile.FileGetFileName(
                    Me.strDocId,
                    mStrWorkFileTo
                ) = False Then
                    Return blnRet
                End If
            End If

            '=======================================================================
            '   ワークFrom名取得
            '=======================================================================
            ' ワークフォルダFrom名
            If Me.strStatus = STATUS_INSERT Then
                '-----------------------------------
                '   新規登録
                '-----------------------------------
                ' テンプレートファイル名（フルパス）からフォルダ名を取得
                If MDFile.FileGetDirName(
                    Me.strTemplateFileFull,
                    mStrWorkDirFrom
                ) = False Then
                    Call MessageBox.Show(
                        "ワークフォルダFrom [ " & Me.strDocId & " ] からフォルダ名を取得できませんでした。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                    )
                    Return blnRet
                End If

            ElseIf Me.strStatus = STATUS_SHOW _
            Or Me.strStatus = STATUS_EDIT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                '-----------------------------------
                '   表示・編集・コピーして編集
                '-----------------------------------
                ' 実際のファイル名が格納されている文書ID（文書フルパス）からフォルダ名を取得
                If MDFile.FileGetDirName(
                    Me.strDocId,
                    mStrWorkDirFrom
                ) = False Then
                    Call MessageBox.Show(
                        "ワークフォルダFrom [ " & Me.strDocId & " ] からフォルダ名を取得できませんでした。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                    )
                    Return blnRet
                End If
            End If

            ' ワークファイルFrom名
            If Me.strStatus = STATUS_INSERT Then
                '-----------------------------------
                '   新規登録
                '-----------------------------------
                ' ワークファイルTo同様に発信文書新規作成画面で選択した標題に対してのテンプレートファイル名
                mStrWorkFileFrom = Me.strTemplateFile
            ElseIf Me.strStatus = STATUS_SHOW _
            Or Me.strStatus = STATUS_EDIT _
            Or Me.strStatus = STATUS_COPY_EDIT Then
                '-----------------------------------
                '   表示・編集・コピーして編集
                '-----------------------------------
                ' ファイル名取得処理
                If MDFile.FileGetFileName(
                    Me.strDocId,
                    mStrWorkFileFrom
                ) = False Then
                    Call MessageBox.Show(
                        "ワークフォルダFrom [ " & Me.strDocId & " ] からフォルダ名を取得できませんでした。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                    )
                    Return blnRet
                End If

            End If

            '=======================================================================
            '   ワークフォルダコピー
            '=======================================================================
            '---------------------------------------
            '   ワークFrom存在チェック
            '---------------------------------------
            ' フォルダ存在確認
            If MDFile.DirExists(mStrWorkDirFrom) = False Then
                Call MessageBox.Show(
                    "ワークフォルダFrom [ " & mStrWorkDirFrom & " ] は存在しません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If
            ' ファイル存在確認
            If MDFile.FileExists(mStrWorkDirFrom & mStrWorkFileFrom) = False Then
                Call MessageBox.Show(
                    "ワークファイルFrom [ " & mStrWorkDirFrom & mStrWorkFileFrom & " ] は存在しません。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If
            '---------------------------------------
            '   ワークTo存在チェック
            '---------------------------------------
            ' フォルダ存在確認
            If MDFile.DirExists(mStrWorkDirTo) = False Then
                If MDFile.DirCreate(mStrWorkDirTo) = False Then
                    Call MessageBox.Show(
                        "ワークフォルダTo [ " & mStrWorkDirTo & " ] は作成できませんでした。",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button1
                    )
                    Return blnRet
                End If
            End If
            ' ファイル存在確認
            ' これからコピーするのでチェックはしないよ
            'If MDFile.FileExists(mStrWorkDirTo & mStrWorkFileTo) Then
            '    Call MessageBox.Show( _
            '        "ワークファイルTo [ " & mStrWorkDirTo & mStrWorkFileTo & " ] は存在しません。", _
            '        "エラー", _
            '        MessageBoxButtons.OK, _
            '        MessageBoxIcon.Warning, _
            '        MessageBoxDefaultButton.Button1 _
            '    )
            '    Return blnRet
            'End If

            '---------------------------------------
            '   ファイルコピー
            '---------------------------------------
            If MDFile.FileCopy(
                mStrWorkDirFrom & mStrWorkFileFrom,
                mStrWorkDirTo & mStrWorkFileTo,
                True
            ) = False Then
                Call MessageBox.Show(
                    "ワークファイルがコピーできませんでした。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateAddinFile
    '   名称　：アドインファイルコピー処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>アドインファイルコピー処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CopyAddinFile() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strAddFileFrom As String = ""       ' 作成元アドインファイル
        Dim strAddFileTo As String = ""         ' 作成アドインファイル

        Try
            '-----------------------------------------------------------
            '   アドインファイル名取得
            '-----------------------------------------------------------
            ' 作成元アドインファイル名（フルパス）取得処理
            If Me.GetAddinFileName(strAddFileFrom) = False Then
                Call MessageBox.Show(
                    "アドインファイル名（フルパス）が取得できませんでした。",
                     "エラー",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning,
                     MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 作成先アドインファイル名取得
            If MDFile.FileGetFileName(
                strAddFileFrom,
                strAddFileTo
            ) = False Then
                Call MessageBox.Show(
                    "[ " & strAddFileFrom & " ] からアドインファイル名を取得できませんでした。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If
            ' 作成先アドインファイル名（フルパス）取得
            strAddFileTo = mStrWorkDirTo & strAddFileTo

            '-----------------------------------------------------------
            '   アドインファイルコピー
            '-----------------------------------------------------------
            ' アドインファイル存在チェック
            If MDFile.FileExists(strAddFileFrom) = False Then
                Call CLMsg.Show("GE0090", "アドインファイル：" & strAddFileFrom)
                Return blnRet
            End If
            ' ファイルコピー処理
            If MDFile.FileCopy(
                strAddFileFrom,
                strAddFileTo,
                True
            ) = False Then
                Call MessageBox.Show(
                    "[ " & strAddFileFrom & " ] からアドインファイル名をコピーできませんでした。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1
                )
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertCoommitteeUpdateListOut
    '   名称　：委員会更新一覧登録処理
    '   概要  ：
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/04/9(月)  
    '   更新日：2013/11/09(土)　Fujisaku
    '   備考　：※ 新規登録の文書番号採番前・後、コピーして編集の文書番号採番前・後の処理
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/04/09(月)  新規作成
    ' 　　　　：2013/11/09(土)　PK重複可能性排除
    '***************************************************************************************************
    ''' <summary>委員会更新一覧登録処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InsertCoommitteeUpdateListOut(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrCommitteeUpdate() As String,
        ByVal iStrDocumentOut As String()
    ) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        ' 更新データが存在しない場合は何もしない
        If iStrCommitteeUpdate Is Nothing _
        Or iStrDocumentOut Is Nothing Then
            Return True
        End If

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル

        Try
            For i = 0 To iStrCommitteeUpdate.Length - 1

                ' COUNT SQL文作成
                strSql = "" & vbCrLf
                strSql = strSql & "SELECT COUNT(*) " & vbCrLf
                strSql = strSql & "FROM committee_update_list_out " & vbCrLf
                strSql = strSql & "WHERE c_committee_update = '" & iStrCommitteeUpdate(i) & "'" & vbCrLf
                strSql = strSql & " AND  c_doc_id           = " & Me.intDocId & vbCrLf
                strSql = strSql & " AND  c_doc_code         = '" & Me.strDocCode & "'" & vbCrLf
                strSql = strSql & " AND  c_period_id        = '" & Me.strPeriodId & "'" & vbCrLf
                strSql = strSql & ";" & vbCrLf

                ' COUNT SQL実行
                dtRet = iClsDb.ExecuteSql(strSql)

                ' 件数チェック PK重複しない場合のみINSERT
                If dtRet.Rows(0).Item(0) = 0 Then

                    ' INSERT SQL文作成
                    strSql = "" & vbCrLf
                    strSql = strSql & " INSERT INTO committee_update_list_out (" & vbCrLf
                    strSql = strSql & "     c_committee_update" & vbCrLf                    ' 01. 委員会名簿変更ＩＤ
                    strSql = strSql & "    ,c_doc_id          " & vbCrLf                    ' 02. 文書識別コード
                    strSql = strSql & "    ,c_doc_code        " & vbCrLf                    ' 03. 管理コード
                    strSql = strSql & "    ,c_period_id       " & vbCrLf                    ' 04. 期ID
                    strSql = strSql & "    ,s_subject_seq     " & vbCrLf                    ' 05. 標題枝番
                    strSql = strSql & "    ,k_document_out    " & vbCrLf                    ' 06. 帳票出力済フラグ
                    strSql = strSql & "    ,d_ins             " & vbCrLf                    ' 07. 作成日
                    strSql = strSql & "    ,c_user_id_ins     " & vbCrLf                    ' 08. 作成者個人ＩＤ
                    strSql = strSql & " ) VALUES (" & vbCrLf
                    strSql = strSql & "     '" & iStrCommitteeUpdate(i) & "'    " & vbCrLf
                    strSql = strSql & "    , " & Me.intDocId & "                " & vbCrLf
                    strSql = strSql & "    ,'" & Me.strDocCode & "'             " & vbCrLf
                    strSql = strSql & "    ,'" & Me.strPeriodId & "'            " & vbCrLf
                    strSql = strSql & "    ,'" & Me.intSubjectSeq & "'          " & vbCrLf
                    strSql = strSql & "    ,'" & iStrDocumentOut(i) & "'        " & vbCrLf
                    strSql = strSql & "    ,'" & Now.ToString("yyyy/MM/dd") & "'" & vbCrLf
                    strSql = strSql & "    ,'" & MDLoginInfo.UserId & "'        " & vbCrLf
                    strSql = strSql & ");" & vbCrLf

                    ' SQL実行
                    intRet = iClsDb.ExecuteNonQuery(strSql)

                    ' 処理件数チェック
                    If intRet = 0 Then
                        Return blnRet
                    End If
                End If
            Next

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCoommitteeUpdateListOut
    '   名称　：委員会更新一覧取得処理
    '   概要  ：
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iIntDocId    As Integer     = 文書識別コード,
    '           ByVal iStrDocCode  As String      = 管理コード,
    '           ByVal iStrPeriodId As String      = 期ID
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/04/09(月)  
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/04/09(月)  新規作成
    '***************************************************************************************************
    ''' <summary>委員会更新一覧取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iIntDocId">文書識別コード</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetCoommitteeUpdateListOut(
        ByVal iClsDb As CLAccessMdb,
        ByVal iIntDocId As Integer,
        ByVal iStrDocCode As String,
        ByVal iStrPeriodId As String
    ) As DataTable

        Dim strSql As String = ""                       ' SQL
        Dim dtRet As DataTable = Nothing                ' 処理結果データテーブル

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_doc_id" & vbCrLf                             ' 文書識別コード
            strSql = strSql & "       ,a.c_doc_code" & vbCrLf                           ' 管理コード
            strSql = strSql & "       ,a.c_period_id" & vbCrLf                          ' 期ID
            strSql = strSql & "       ,a.c_committee_update" & vbCrLf                   ' 委員会名簿変更ＩＤ
            strSql = strSql & "       ,a.k_document_out" & vbCrLf                       ' 帳票出力済フラグ
            strSql = strSql & "   FROM committee_update_list_out AS a" & vbCrLf         ' 
            strSql = strSql & "  WHERE a.c_doc_id    = " & iIntDocId & vbCrLf           ' 文書識別コードと同じもの
            strSql = strSql & "    AND a.c_doc_code  = '" & iStrDocCode & "'" & vbCrLf  ' 管理コードと同じもの
            strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf ' 期IDと同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return dtRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateCommitteeUpdateListOutputFlag
    '   名称　：委員会更新一覧帳票出力フラグ更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb       As CLAccessMdb = データベースクラス,
    '           ByVal iIntDocId    As Integer     = 更新データ,
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/04/09(月)  
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/04/09(月)  新規作成
    '***************************************************************************************************
    ''' <summary>委員会更新一覧帳票出力フラグ更新処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iData">更新データ</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateCommitteeUpdateListOutputFlag(
        ByVal iClsDb As CLAccessMdb,
        ByVal iData As DataTable
    ) As Boolean

        ' 更新データが存在しない場合は何もしない
        If iData Is Nothing Then
            Return True
        End If
        If iData.Rows.Count = 0 Then
            Return True
        End If

        Try
            For Each row As DataRow In iData.Rows
                ' 帳票出力フラグ更新
                Me.UpdateCommitteeUpdateList(
                    iClsDb,
                    row.Item(3),
                    MDLoginInfo.Ksh,
                    row.Item(2),
                    row.Item(4)
                )

                ' 更新済みデータ削除
                Me.DeleteCommitteeUpdateListOut(
                    iClsDb,
                    row.Item(3),
                    row.Item(1),
                    row.Item(2)
                )
            Next

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(
                Err.Number,
                Err.Description,
                SCREEN_ID,
                SCREEN_NAME,
                System.Reflection.MethodInfo.GetCurrentMethod.Name()
            )

        End Try

        ' 戻り値設定
        Return True

    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateCommitteeUpdateList
    '   名称　：委員会更新一覧更新処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス,
    '           ByVal iStrCommitteeUpdate As String      = 委員会名簿変更ID,
    '           ByVal iStrKsh             As String      = 会社コード,
    '           ByVal iStrPeriodId        As String      = 期ID,
    '           ByVal iStrDocumentOut     As String      = 帳票出力済フラグ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/30(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/30(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>委員会更新一覧処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrCommitteeUpdate">委員会名簿変更ID</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrDocumentOut">帳票出力済フラグ</param>
    ''' <param name="iStrDetails"></param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateCommitteeUpdateList1(
        ByVal iClsDb As CLAccessMdb,
        ByVal iStrCommitteeUpdate() As String,
        ByVal iStrKsh As String,
        ByVal iStrPeriodId As String,
        ByVal iStrDocumentOut As String(),
        ByVal iStrDetails As String()
    ) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim intRet As Integer = 0                   ' 処理件数
        Dim strSql As String = ""                   ' SQL文
        Dim strVal As String = ""                   ' 更新帳票出力済みフラグ

        Try

            For i = 0 To iStrCommitteeUpdate.Length - 1

                '-----------------------------------------------
                '   更新値取得
                '-----------------------------------------------
                ' 追加・削除の場合、更新帳票出力済みフラグ + 1
                ' 長の交代の場合、  更新帳票出力済みフラグ + 2
                strVal = CStr(CInt(iStrDocumentOut(i)) + CInt(iStrDetails(i)))

                ' SQL文作成
                strSql = "" & vbCrLf
                strSql = strSql & " UPDATE committee_update_list" & vbCrLf
                strSql = strSql & "    SET k_document_out     = '" & strVal & "'" & vbCrLf
                strSql = strSql & "  WHERE c_committee_update = '" & iStrCommitteeUpdate(i) & "'"
                strSql = strSql & "    AND c_ksh              = '" & iStrKsh & "'"
                strSql = strSql & "    AND c_period_id        = '" & iStrPeriodId & "'"
                strSql = strSql & "    AND k_document_out     = '" & iStrDocumentOut(i) & "'" & vbCrLf
                strSql = strSql & ";" & vbCrLf

                ' SQL実行
                intRet = iClsDb.ExecuteNonQuery(strSql)

                ' 処理件数チェック
                If intRet <= 0 Then
                    Return blnRet
                End If

            Next

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    ''' <summary>
    ''' 委員会更新一覧更新処理
    ''' </summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrCommitteeUpdate">委員会名簿変更ID</param>
    ''' <param name="iStrKsh">会社コード</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrDocumentOut">帳票出力済フラグ</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function UpdateCommitteeUpdateList( _
        ByVal iClsDb As CLAccessMdb, _
        ByVal iStrCommitteeUpdate As String, _
        ByVal iStrKsh As String, _
        ByVal iStrPeriodId As String, _
        ByVal iStrDocumentOut As String _
    ) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim intRet As Integer = 0                   ' 処理件数
        Dim strSql As String = ""                   ' SQL文
        Dim strVal As String = "3"                   ' 更新帳票出力済みフラグ

        Try

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE committee_update_list" & vbCrLf
            strSql = strSql & "    SET k_document_out     = '" & strVal & "'" & vbCrLf
            strSql = strSql & "  WHERE c_committee_update = '" & iStrCommitteeUpdate & "'"
            strSql = strSql & "    AND c_ksh              = '" & iStrKsh & "'"
            strSql = strSql & "    AND c_period_id        = '" & iStrPeriodId & "'"
            strSql = strSql & "    AND k_document_out     = '" & iStrDocumentOut & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            intRet = iClsDb.ExecuteNonQuery(strSql)

            ' 処理件数チェック
            If intRet <= 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 例外をスロー
            Throw ex
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteCommitteeUpdateListOut
    '   名称　：委員会更新一覧削除処理
    '   概要  ：
    '   引数　：ByVal iClsDb              As CLAccessMdb = データベースクラス,
    '           ByVal iStrCommitteeUpdate As String      = 委員会名簿変更ID,
    '           ByVal iStrPeriodId        As String      = 期ID,
    '           ByVal iStrDocCode     As String          = 管理コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/30(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/30(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>委員会更新一覧削除処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrCommitteeUpdate">委員会名簿変更ID</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <param name="iStrDocCode">管理コード</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteCommitteeUpdateListOut( _
        ByVal iClsDb As CLAccessMdb, _
        ByVal iStrCommitteeUpdate As String, _
        ByVal iStrDocCode As String, _
        ByVal iStrPeriodId As String _
    ) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim intRet As Integer = 0                   ' 処理件数
        Dim strSql As String = ""                   ' SQL文
        Dim strVal As String = ""                   ' 更新帳票出力済みフラグ

        Try

            For i = 0 To iStrCommitteeUpdate.Length - 1

                ' SQL文作成
                strSql = "" & vbCrLf
                strSql = strSql & " DELETE FROM committee_update_list_out" & vbCrLf
                strSql = strSql & "  WHERE c_committee_update = '" & iStrCommitteeUpdate & "'"
                strSql = strSql & "    AND c_period_id        = '" & iStrPeriodId & "'"
                strSql = strSql & "    AND c_doc_code         = '" & iStrDocCode & "'" & vbCrLf
                strSql = strSql & ";" & vbCrLf

                ' SQL実行
                intRet = iClsDb.ExecuteNonQuery(strSql)

                ' 処理件数チェック
                If intRet <= 0 Then
                    Return blnRet
                End If

            Next

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 例外をスロー
            Throw ex
        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region "Excel用関数"
    '***************************************************************************************************
    '   ＩＤ　：ExcelOpen
    '   名称　：Excelオープン処理（参照設定のヴァージョンに依存されるもの）
    '   概要  ：
    '   引数　：Byval iFile As String = オープンするファイル名（フルパス）
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '   備考　：プライベート変数の各Excelオブジェクトの定義も変更すること
    '***************************************************************************************************
    ''' <summary>Excelオープン処理</summary>
    ''' <param name="iFile">オープンするファイル名（フルパス）</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelOpen( _
        ByVal iFile As String _
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim decExcelVer As Decimal = 0          ' Excelヴァージョン

        Try
            ' Excel アプリケーションオブジェクト生成
            mXlsAp = New Microsoft.Office.Interop.Excel.Application

            ' Excel Workbooksオブジェクト生成
            mXlsWbs = mXlsAp.Workbooks

            ' Excel Workbookオブジェクト生成（ワークブックを開く）
            mXlsWb = mXlsWbs.Open(iFile)

            ' Excel Sheetsオブジェクト生成
            mXlsSs = mXlsWb.Sheets

            ' Excel Worksheetオブジェクト生成（1番目のワークシート選択）
            mXlsWs = mXlsSs(1)

            '-------------------------------------------------------
            '   Excel表示位置設定
            '-------------------------------------------------------
            ' エクセルウィンドウがエクセルコントロールウィンドウと並ぶように位置を設定
            mXlsAp.WindowState = -4143
            mXlsAp.Left = 1
            mXlsAp.Top = 1
            mXlsAp.Width = (System.Windows.Forms.Screen.GetWorkingArea(Me).Width - Me.Width) * 72 / Me.CreateGraphics.DpiX
            mXlsAp.Height = System.Windows.Forms.Screen.GetWorkingArea(Me).Height * 72 / Me.CreateGraphics.DpiY

            ' エクセルを表示
            mXlsAp.Visible = True
            mXlsAp.UserControl = True

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' Excel Worksheetオブジェクト解放
            If Not mXlsWs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWs)
            End If

            ' Excel Sheetsオブジェクト解放
            If Not mXlsSs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsSs)
            End If

            ' Excel Workbookオブジェクト解放
            If Not mXlsWb Is Nothing Then
                Try
                    mXlsWb.Close()
                Finally
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWb)
                End Try
            End If

            ' Excel Worksheetsオブジェクト解放
            If Not mXlsWbs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWbs)
            End If

            ' Excel アプリケーションオブジェクト解放
            If Not mXlsAp Is Nothing Then
                Try
                    ' Excelを閉じる
                    mXlsAp.Quit()
                Finally
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsAp)
                End Try
            End If

            ' ガベージコレクションメモリ開放
            System.GC.Collect()

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelClose
    '   名称　：Excelクローズ処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelクローズ処理</summary>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelClose() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' Excel Worksheetオブジェクト解放
            If Not mXlsWs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWs)
                mXlsWs = Nothing
            End If

            ' Excel Sheetsオブジェクト解放
            If Not mXlsSs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsSs)
                mXlsSs = Nothing
            End If

            ' Excel Workbookオブジェクト解放
            If Not mXlsWb Is Nothing Then
                Try
                    mXlsWb.Close()
                Finally
                    If Not mXlsWb Is Nothing Then
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWb)
                    End If
                End Try
            End If

            ' Excel Workbooksオブジェクト解放
            If Not mXlsWbs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWbs)
                mXlsWbs = Nothing
            End If

            ' Excel アプリケーションオブジェクト解放
            If Not mXlsAp Is Nothing Then
                Try
                    ' Excelを閉じる
                    mXlsAp.Quit()
                Finally
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsAp)
                    mXlsAp = Nothing
                End Try
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' Excel Worksheetオブジェクト解放
            If Not mXlsWs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWs)
            End If

            ' Excel Sheetsオブジェクト解放
            If Not mXlsSs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsSs)
            End If

            ' Excel Workbookオブジェクト解放
            If Not mXlsWb Is Nothing Then
                Try
                    mXlsWb.Close()
                Finally
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWb)
                End Try
            End If

            ' Excel Workbooksオブジェクト解放
            If Not mXlsWbs Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsWbs)
            End If

            ' Excel アプリケーションオブジェクト解放
            If Not mXlsAp Is Nothing Then
                Try
                    ' Excelを閉じる
                    mXlsAp.Quit()
                Finally
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(mXlsAp)
                End Try
            End If

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )
        Finally
            ' ガベージコレクションメモリ開放
            System.GC.Collect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetCellValue
    '   名称　：Excelセル値設定処理
    '   概要  ：
    '   引数　：ByVal iLngrow  As Long   = ロー値,
    '           ByVal iLngcol  As Long   = カラム値,
    '           ByVal strValue As String = 設定する文字列
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル値設定処理</summary>
    ''' <param name="iLngrow">ロー値</param>
    ''' <param name="iLngcol">カラム値</param>
    ''' <param name="strValue">設定する文字列</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetCellValue( _
        ByVal iLngrow As Long, _
        ByVal iLngcol As Long, _
        ByVal strValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim xlsRag As Excel.Range           ' Excelレンジオブジェクト

        Try
            'xlsWS.Cells(x, y).Value のように ピリオドを２つ以上書くと、エクセルのプロセスが終了できない
            xlsRag = Me.mXlsWs.Cells(iLngrow, iLngcol)
            xlsRag.Value = strValue

            Call Me.ExcelMacro(xlsRag)

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetCellValue
    '   名称　：Excelセル値取得処理
    '   概要  ：
    '   引数　：ByVal iLngrow  As Long   = ロー値,
    '           ByVal iLngcol  As Long   = カラム値,
    '           ByVal strValue As String = 取得した文字列
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル値取得処理</summary>
    ''' <param name="iLngrow">ロー値</param>
    ''' <param name="iLngcol">カラム値</param>
    ''' <param name="ioStrValue">取得した文字列</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelGetCellValue( _
        ByVal iLngrow As Long, _
        ByVal iLngcol As Long, _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim xlsRag As Excel.Range           ' Excelレンジオブジェクト

        Try
            'xlsWS.Cells(x, y).Value のように ピリオドを２つ以上書くと、エクセルのプロセスが終了できない
            xlsRag = Me.mXlsWs.Cells(iLngrow, iLngcol)
            ioStrValue = xlsRag.Value

            Call Me.ExcelMacro(xlsRag)

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelMacro
    '   名称　：Excelマクロ実行処理
    '   概要  ：
    '   引数　：ByVal o As Object
    '   戻り値：なし
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '   備考　：エクセルのオブジェクトを開放する(エクセルのプロセスを正しく終了させるため)
    '           ※参照　http://support.microsoft.com/default.aspx?scid=kb;ja;317109
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelマクロ実行処理</summary>
    ''' <param name="o"></param>
    ''' <remarks></remarks>
    Private Sub ExcelMacro(ByVal o As Object)

        On Error GoTo ErrHander

        System.Runtime.InteropServices.Marshal.ReleaseComObject(o)
        o = Nothing

ErrHander:
        Resume Next
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：ExcelSave
    '   名称　：Excelファイル上書き保存処理
    '   概要  ：
    '   引数　：ByVal iFile As String = ファイル名（フルパス）
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelファイル上書き保存処理</summary>
    ''' <param name="iFile"></param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSave( _
        ByVal iFile As String _
    ) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            ' ブックの保存
            Me.CallExcelMacro("DocEvent.SaveCommand")
            'Me.mXlsWb.SaveAs(FileName:=iFile, FileFormat:=XL_EXCEL9795)
            'Me.mXlsWbs(1).SaveAs(FileName:=iFile, FileFormat:=56)
            'Me.mXlsWb.Save()
            'Me.mXlsAp.EnableEvents = False ' BeforeSaveイベントが二重に走るのを防ぐ
            'Me.mXlsWb.Save()
            'Me.mXlsAp.EnableEvents = True
            'Me.mXlsWb.Saved = True

            '' 保存マクロ実行
            'Me.CallExcelMacro("DocEvent.SaveCommand")

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            'Call CLMsg.ShowEtarnal( _
            '    Err.Number, _
            '    Err.Description, _
            '    SCREEN_ID, _
            '    SCREEN_NAME, _
            '    System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            ')

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelOutput
    '   名称　：Excelデータ出力処理
    '   概要  ：
    '   引数　：
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel出力処理</summary>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelOutput() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '-------------------------------------------------------------------
            '   文書番号
            '-------------------------------------------------------------------
            ' Excelセル値文書番号設定処理
            If Me.ExcelSetDocNumber( _
                StrConv( _
                    Me.strDocCode, _
                    VbStrConv.Wide _
                ), _
                PublicCommand.ConvertHanToZen(Me.intPeriod.ToString), _
                PublicCommand.ConvertHanToZen(Me.strPeriodIdD.ToString), _
                PublicCommand.ConvertHanToZen(Me.strDocNumber), _
                PublicCommand.ConvertHanToZen(Me.strDocNumberD) _
            ) = False Then
                'PublicCommand.ConvertHanToZen(Me.strDocNumber)) = False Then '2012/06/27
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   発行日
            '-------------------------------------------------------------------
            ' Excelセル値発行日設定処理
            If Me.ExcelSetIssueDate() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   標題（テンプレートすべて出力対象）
            '-------------------------------------------------------------------
            If Me.ExcelSetSubject() = False Then
                Return blnRet
            End If

            ''-------------------------------------------------------------------
            ''   最初の文章（テンプレートすべて出力対象）
            ''-------------------------------------------------------------------
            'If Me.ExcelSetNoticeSentence() = False Then
            '    Return blnRet
            'End If

            ''-------------------------------------------------------------------
            ''   任期についての文章（001・006・010テンプレートのみ出力対象）
            ''-------------------------------------------------------------------
            'If Me.strTemplate = TEMPLATE_KBN_001 _
            'Or Me.strTemplate = TEMPLATE_KBN_006 _
            'Or Me.strTemplate = TEMPLATE_KBN_010 Then
            '    If Me.ExcelSetServiceDateFromTo() = False Then
            '        Return blnRet
            '    End If
            'End If

            ''-------------------------------------------------------------------
            ''   記（テンプレートすべて出力対象）
            ''-------------------------------------------------------------------
            'If Me.ExcelSetDescription() = False Then
            '    Return blnRet
            'End If

            ''-------------------------------------------------------------------
            ''   最後の文章（001テンプレートのみ出力対象）
            ''-------------------------------------------------------------------
            'If Me.strTemplate = TEMPLATE_KBN_001 Then
            '    If Me.ExcelSetFinishSentence("") = False Then
            '        Return blnRet
            '    End If
            'End If

            ''-------------------------------------------------------------------
            ''   以上（テンプレートすべて出力対象）
            ''-------------------------------------------------------------------
            'If Me.ExcelSetClosingRemarks() = False Then
            '    Return blnRet
            'End If

            '-------------------------------------------------------
            '   Excel選択委員会の追加・削除情報出力処理
            '   新規作成かつ詳細設定分類が追加・削除選択データが存在する場合に実行
            '-------------------------------------------------------
            If Me.strStatus = STATUS_INSERT _
            AndAlso Me.strTemplate = "003" _
            AndAlso Me.strCommitteeUpdate IsNot Nothing Then
                If Me.ExcelSetCommitteeUpdateId() = False Then
                    Return blnRet
                End If
            End If

            '-------------------------------------------------------
            '   Excel選択委員会の長の交代情報出力処理
            '   新規作成かつ詳細設定分類が変更かつ選択データが存在する場合に実行
            '-------------------------------------------------------
            If Me.strStatus = STATUS_INSERT _
            AndAlso Me.strTemplate = "013" _
            AndAlso Me.strCommitteeUpdate IsNot Nothing Then
                If Me.ExcelSetCommitteeTopChangeInfo() = False Then
                    Return blnRet
                End If
            End If

        Catch ex As Exception
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetDocNumber
    '   名称　：Excelセル値文書番号設定処理
    '   概要  ：
    '   引数　：ByVal strDocCode   As String = 文書識別コード,
    '           ByVal strPeriod    As String = 期,
    '           ByVal strDocNumber As String = 文書番号
    '   戻り値：True：正常, False：異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル値文書番号設定処理</summary>
    ''' <param name="iStrDocCode">文書識別コード</param>
    ''' <param name="iStrPeriod">期</param>
    ''' <param name="iStrDocNumber">文書番号</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelSetDocNumber( _
        ByVal iStrDocCode As String, _
        ByVal iStrPeriod As String, _
        ByVal iStrPeriodD As String, _
        ByVal iStrDocNumber As String, _
        ByVal iStrDocNumberD As String _
    ) As Boolean
        'ByVal iStrDocNumber As String) As Boolean 2012/06/27

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strData As String = ""          ' 入力文字
        Dim strCellPosition As String = ""  ' ドキュメント番号セル位置

        Try
            ' セル出力文字列設定
            strData = "全日空乗組発" & "{code}" & "第" & "{period}-{number}" & "号"

            ' ドキュメント番号チェック   2012/06/27 修正 START
            If ChkNull(iStrDocNumber) Then
                If iStrDocCode = "Ｄ" Then
                    strData = strData.Replace("{code}", iStrDocCode).Replace("{period}", strPeriodIdD).Replace("{number}", "＊＊")
                Else
                    strData = strData.Replace("{code}", iStrDocCode).Replace("{period}", iStrPeriod).Replace("{number}", "＊＊")
                End If
            Else
                If iStrDocCode = "Ｄ" Then
                    strData = strData.Replace("{code}", iStrDocCode).Replace("{period}", _
                              StrConv((strPeriodIdD), VbStrConv.Wide)).Replace("{number}", StrConv(iStrDocNumberD, VbStrConv.Wide))
                Else
                    strData = strData.Replace("{code}", iStrDocCode).Replace("{period}", iStrPeriod).Replace("{number}", iStrDocNumber)
                End If

            End If
            ' ドキュメント番号チェック   2012/06/27 修正 END
            ' ドキュメント番号セル取得処理
            If Me.GetDocNumberCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            ' セル位置データ設定処理
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strData _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）

            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetIssueDate
    '   名称　：Excelセル値発行日設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/28(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル値発行日設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetIssueDate() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strIssueDateFormatAfter As String = ""      ' フォーマット変換後の発行日
        Dim strCellPosition As String = ""              ' 日付出力位置セル

        Try
            '-------------------------------------------------------
            '   プロパティに発行日設定
            '-------------------------------------------------------
            Me.strIssueDate = Me.dtpIssueDate.Value.Date.ToString("yyyyMMdd")

            '-------------------------------------------------------
            '   発行日フォーマット変換処理
            '-------------------------------------------------------
            If Me.GetFormatDate( _
                Me.dtpIssueDate.Value, _
                Me.chkWareki.Checked, _
                strIssueDateFormatAfter _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   日付出力位置セル取得処理
            '-------------------------------------------------------
            If Me.GetIssueDateCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strIssueDateFormatAfter _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------
            '   Excelファイル保存処理
            '-------------------------------------------------------
            If Me.ExcelSave( _
                Me.mStrWorkDirTo & Me.mStrWorkFileTo _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetSubject
    '   名称　：Excelセル値標題設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル値標題設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetSubject() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strCellPosition As String = ""              ' 標題出力位置セル

        Try
            '-------------------------------------------------------------------
            '   標題出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.GetSubjectCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                Me.strSubject _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetNoticeSentence
    '   名称　：Excel最初の文章設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel最初の文章設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetNoticeSentence() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strValue As String = ""             ' 最初の文書内容
        Dim strCellPosition As String = ""      ' 出力位置セル

        Try
            '-------------------------------------------------------------------
            '   最初の文章出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetNoticeSentenceCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   最初の文章内容取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetNoticeSentence( _
                strValue _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strValue _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetServiceDateFromTo
    '   名称　：Excel任期についての文章設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel任期についての文章設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetServiceDateFromTo() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strValue As String = ""             ' 任期についての文章内容
        Dim strCellPosition As String = ""      ' 出力位置セル

        Try
            '-------------------------------------------------------------------
            '   任期についての文章出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetServiceDateFromToCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   任期についての文章内容取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetServiceDateFromTo( _
                "", _
                "", _
                strValue _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strValue _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetDescription
    '   名称　：Excel記設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel記設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetDescription() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strValue As String = ""                     ' 記内容
        Dim strCellPosition As String = ""              ' 出力位置セル

        Try
            '-------------------------------------------------------------------
            '   記出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetDescriptionCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   記内容取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetDescription( _
                strValue _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strValue _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetFinishSentence
    '   名称　：Excel最後の文章設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel最後の文章設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetFinishSentence( _
        ByVal iStrRepleace As String _
    ) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strValue As String = ""             ' 最後の文書内容
        Dim strCellPosition As String = ""      ' 出力位置セル

        Try
            '-------------------------------------------------------------------
            '   最後の文章出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetFinishSentenceCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            strCellPosition.Replace("{R}", iStrRepleace)

            '-------------------------------------------------------------------
            '   最後の文章内容取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetFinishSentence( _
                strValue _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strValue _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetClosingRemarks
    '   名称　：Excel以上設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excel以上設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetClosingRemarks() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strValue As String = ""             ' 記内容
        Dim strCellPosition As String = ""      ' 記出力位置セル

        Try
            '-------------------------------------------------------------------
            '   以上出力位置セル取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetClosingRemarksCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   以上内容取得処理
            '-------------------------------------------------------------------
            If Me.ExcelGetClosingRemarks( _
                strValue _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   セル位置データ設定処理
            '-------------------------------------------------------------------
            If Me.ExcelSetCellData( _
                strCellPosition, _
                strValue _
            ) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetCommitteeUpdateId
    '   名称　：Excelセル委員会変更ID設定処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>Excelセル委員会変更ID設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExcelSetCommitteeUpdateId() As Boolean

        Dim blnRet As Boolean = False                      ' 処理結果
        Dim strCellPosition As String = ""                 ' 委員会変更ID出力位置セル
        Dim dtAddDeleteInfo As DataTable = Nothing         ' 出力する追加・削除情報
        Dim intDataCnt As Integer = 0                      ' 出力データカウント
        Dim strCurrentInsertKind As String = String.Empty  ' 出力中の追加対象フラグ
        Dim strCurrentInsert As String = String.Empty      ' 出力中の追加対象フラグ文字列  
        Dim strCurrentCommitteeId As String = String.Empty ' 出力中の委員会ID
        Dim intStartOutPutRow As Integer = 15              ' 出力開始位置
        Dim strOutputInsert As String = "A"                ' 追加削除区分の出力位置 
        Dim strOutputCommittee As String = "E"             ' 委員会名の出力位置 
        Dim strOutputUserId As String = "F"                ' 社員番号の出力位置
        Dim strOutputUserName As String = "I"              ' 社員名の出力位置
        Dim strOutputBelonging As String = "N"             ' 組合支部の出力位置

        Try
            ' DB
            ' 委員会変更IDより出力情報を取得
            dtAddDeleteInfo = Me.GetCommitteeUpdateInfo()
            ' 委員会変更ID出力位置セル取得処理
            If Me.GetCommitteeUpdateIdCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   追加・削除データ設定
            '-------------------------------------------------------------------
            If dtAddDeleteInfo.Rows.Count > 0 Then
                strCurrentInsertKind = dtAddDeleteInfo.Rows(0).Item("k_committee_insert")
                strCurrentCommitteeId = dtAddDeleteInfo.Rows(0).Item("c_committee_id")

                ' 追加削除区分の文言設定
                If strCurrentInsertKind = "0" Then
                    strCurrentInsert = "１．追加"
                ElseIf strCurrentInsertKind = "1" Then
                    strCurrentInsert = "１．削除"
                End If
                ' 追加削除区分出力
                If Me.ExcelSetCellData( _
                    strCellPosition, _
                    strCurrentInsert _
                ) = False Then
                    Return blnRet
                End If
                ' 委員会名出力
                If Me.ExcelSetCellData( _
                    strOutputCommittee & intStartOutPutRow.ToString, _
                    dtAddDeleteInfo.Rows(0).Item("l_name") _
                ) = False Then
                    Return blnRet
                End If
                ' 出力位置加算
                intStartOutPutRow = intStartOutPutRow + 1

                Do While (intDataCnt < dtAddDeleteInfo.Rows.Count)

                    ' 追加削除区分が直前のデータと異なるかチェック
                    If dtAddDeleteInfo.Rows(intDataCnt).Item("k_committee_insert").Equals(strCurrentInsertKind) = False Then
                        ' 出力位置加算
                        intStartOutPutRow = intStartOutPutRow + 2
                        ' 追加削除区分の文言設定
                        strCurrentInsert = "２．削除"
                        ' 追加削除区分出力
                        If Me.ExcelSetCellData( _
                            strOutputInsert & intStartOutPutRow.ToString, _
                            strCurrentInsert _
                        ) = False Then
                            Return blnRet
                        End If
                        ' 出力位置加算
                        intStartOutPutRow = intStartOutPutRow + 1
                        ' 委員会名出力
                        If Me.ExcelSetCellData( _
                            strOutputCommittee & intStartOutPutRow.ToString, _
                            dtAddDeleteInfo.Rows(intDataCnt).Item("l_name") _
                        ) = False Then
                            Return blnRet
                        End If
                        ' 出力位置加算
                        intStartOutPutRow = intStartOutPutRow + 1

                        ' 出力中の追加削除区分の更新
                        strCurrentInsertKind = dtAddDeleteInfo.Rows(intDataCnt).Item("k_committee_insert")
                        ' 出力中の委員会IDを更新
                        strCurrentCommitteeId = dtAddDeleteInfo.Rows(intDataCnt).Item("c_committee_id")

                    Else
                        ' 出力委員会が直前のデータと異なるかチェック
                        If dtAddDeleteInfo.Rows(intDataCnt).Item("c_committee_id").Equals(strCurrentCommitteeId) = False Then
                            ' 出力位置加算
                            intStartOutPutRow = intStartOutPutRow + 1
                            ' 委員会名出力
                            If Me.ExcelSetCellData( _
                                strOutputCommittee & intStartOutPutRow.ToString, _
                                dtAddDeleteInfo.Rows(intDataCnt).Item("l_name") _
                            ) = False Then
                                Return blnRet
                            End If
                            ' 出力位置加算
                            intStartOutPutRow = intStartOutPutRow + 1

                            ' 出力中の委員会IDを更新
                            strCurrentCommitteeId = dtAddDeleteInfo.Rows(intDataCnt).Item("c_committee_id")
                        End If
                    End If

                    ' 社員番号出力
                    If Me.ExcelSetCellData( _
                        strOutputUserId & intStartOutPutRow.ToString, _
                        dtAddDeleteInfo.Rows(intDataCnt).Item("c_user_id") _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 社員名出力
                    If Me.ExcelSetCellData( _
                        strOutputUserName & intStartOutPutRow.ToString, _
                        dtAddDeleteInfo.Rows(intDataCnt).Item("user_name") _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 組合支部出力
                    If Me.ExcelSetCellData( _
                        strOutputBelonging & intStartOutPutRow.ToString, _
                        "(" & dtAddDeleteInfo.Rows(intDataCnt).Item("Belonging") & ")" _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 出力位置加算
                    intStartOutPutRow = intStartOutPutRow + 1

                    ' 次のデータへ
                    intDataCnt = intDataCnt + 1
                Loop
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelSetCommitteeTopChangeInfo
    '   名称　：Excelセル長の交代情報設定処理
    '   概要  ：新規作成画面にて選択された長の交代情報をエクセルに出力
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function ExcelSetCommitteeTopChangeInfo() As Boolean

        Dim blnRet As Boolean = False                      ' 処理結果
        Dim strCellPosition As String = String.Empty       ' 委員会変更ID出力位置セル
        Dim dtTopChangeInfo As DataTable = Nothing         ' 出力する追加・削除情報
        Dim intDataCnt As Integer = 0                      '出力データカウント
        Dim strCurrentPost As String = String.Empty        '出力中の役職
        Dim intStartOutPutRow As Integer = 17              '出力開始位置
        Dim strOutputInsert As String = "E"                '委員会名＋役職名の出力位置 
        Dim strOutputUserId As String = "I"                '社員番号の出力位置
        Dim strOutputUserName As String = "L"              '社員名の出力位置
        Dim strOutputBelonging As String = "S"             '組合支部の出力位置
        Dim strComiitteePost As String = String.Empty      '連番＋委員会名＋役職出力用 
        Dim intCount As Integer = 1                        '出力対象の委員会・役職の連番用

        Try
            ' 長の交代情報取得
            dtTopChangeInfo = Me.GetCommitteeTopChangeInfo()
            ' 委員会変更ID出力位置セル取得処理
            If Me.GetCommitteeUpdateIdCell( _
                strCellPosition _
            ) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------
            '   長の交代情報設定
            '-------------------------------------------------------------------
            If dtTopChangeInfo.Rows.Count > 0 Then
                strCurrentPost = dtTopChangeInfo.Rows(0).Item("s_committee_seq")

                '出力委員会・役職の文言設定
                strComiitteePost = StrConv( _
                    intCount.ToString, _
                    VbStrConv.Wide) & _
                    "．　" & _
                    dtTopChangeInfo.Rows(0).Item("l_name") & _
                    "　" & _
                    dtTopChangeInfo.Rows(0).Item("post_name")

                ' 委員会・役職の文言出力
                If Me.ExcelSetCellData( _
                    strCellPosition, _
                    strComiitteePost _
                ) = False Then
                    Return blnRet
                End If

                Do While (intDataCnt < dtTopChangeInfo.Rows.Count)
                    ' 出力する役職が直前のデータと異なるかチェック
                    If dtTopChangeInfo.Rows(intDataCnt).Item("s_committee_seq").Equals(strCurrentPost) = False Then
                        ' 委員会・役職の連番加算
                        intCount = intCount + 1
                        ' 出力位置加算
                        intStartOutPutRow = intStartOutPutRow + 1
                        ' 出力委員会・役職の文言設定
                        strComiitteePost = StrConv( _
                            intCount.ToString, _
                            VbStrConv.Wide) & _
                            "．　" & _
                            dtTopChangeInfo.Rows(intDataCnt).Item("l_name") & _
                            "　" & _
                            dtTopChangeInfo.Rows(intDataCnt).Item("post_name")
                        ' 追加削除区分出力
                        If Me.ExcelSetCellData( _
                            strOutputInsert & intStartOutPutRow.ToString, _
                            strComiitteePost _
                        ) = False Then
                            Return blnRet
                        End If
                        ' 出力位置加算
                        intStartOutPutRow = intStartOutPutRow + 1

                        ' 出力中の役職を更新
                        strCurrentPost = dtTopChangeInfo.Rows(intDataCnt).Item("s_committee_seq")
                    End If

                    ' 社員番号出力
                    If Me.ExcelSetCellData( _
                        strOutputUserId & intStartOutPutRow.ToString, _
                        dtTopChangeInfo.Rows(intDataCnt).Item("c_user_id") _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 社員名出力
                    If Me.ExcelSetCellData( _
                        strOutputUserName & intStartOutPutRow.ToString, _
                        dtTopChangeInfo.Rows(intDataCnt).Item("user_name") _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 組合支部出力
                    If Me.ExcelSetCellData( _
                        strOutputBelonging & intStartOutPutRow.ToString, _
                        "(" & dtTopChangeInfo.Rows(intDataCnt).Item("Belonging") & ")" _
                    ) = False Then
                        Return blnRet
                    End If
                    ' 出力位置加算
                    intStartOutPutRow = intStartOutPutRow + 1

                    ' 次のデータへ
                    intDataCnt = intDataCnt + 1
                Loop
            End If

            ' 処理結果に正常を設定
            blnRet = True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        Return blnRet
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetDescription
    '   名称　：記内容取得処理
    '   概要  ：
    '   引数　：ByRef ioStrValue As String = 記内容
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>記内容取得処理</summary>
    ''' <param name="ioStrValue">記内容</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetDescription( _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 記内容
            ioStrValue = Me.CallExcelMacro("DocConst.GetDescription", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetNoticeSentence
    '   名称　：最初の文書内容取得処理
    '   概要  ：
    '   引数　：ByRef ioStrValue As String = 最初の文書内容
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>最初の文書内容取得処理</summary>
    ''' <param name="ioStrValue">最初の文書内容</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetNoticeSentence( _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 最初の文書内容
            ioStrValue = Me.CallExcelMacro("DocConst.GetNoticeSentence", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetServiceDateFromTo
    '   名称　：任期についての文章内容取得処理
    '   概要  ：
    '   引数　：ByVal iStrFrom   As String = 任期開始,
    '           ByVal iStrTo     As String = 任期終了,
    '           ByRef ioStrValue As String = 任期についての文章内容
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>任期についての文章内容取得処理</summary>
    ''' <param name="iStrFrom">任期開始</param>
    ''' <param name="iStrTo">任期終了</param>
    ''' <param name="ioStrValue">任期についての文章内容</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetServiceDateFromTo( _
        ByVal iStrFrom As String, _
        ByVal iStrTo As String, _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果
        Dim strValue As String = ""     ' 任期についての文書内容枠組み

        Try
            ' 任期についての文章内容
            strValue = Me.CallExcelMacro("DocConst.GetServiceDateFromTo", New String(0 - 1) {})

            ' 任期開始終了を変換
            ioStrValue = strValue.Replace("{from}", iStrFrom).Replace("{to}", iStrTo)

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetFinishSentence
    '   名称　：最後の文書内容取得処理
    '   概要  ：
    '   引数　：ByRef ioStrValue As String = 最後の文書内容
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>最後の文書出力位置セル取得処理</summary>
    ''' <param name="ioStrValue">最後の文書内容</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetFinishSentence( _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 最後の文書内容
            ioStrValue = Me.CallExcelMacro("DocConst.GetFinishSentence", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetClosingRemarks
    '   名称　：以上内容取得処理
    '   概要  ：
    '   引数　：ByRef ioStrValue As String = 記内容
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>以上内容取得処理</summary>
    ''' <param name="ioStrValue">記内容</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetClosingRemarks( _
        ByRef ioStrValue As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 以上内容
            ioStrValue = Me.CallExcelMacro("DocConst.GetClosingRemarks", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetDescriptionCell
    '   名称　：記出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>記出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetDescriptionCell( _
        ByRef ioStrCell As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 記出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetDescriptionCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetNoticeSentenceCell
    '   名称　：最初の文書出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>最初の文書出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetNoticeSentenceCell( _
        ByRef ioStrCell As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 最初の文書出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetNoticeSentenceCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetServiceDateFromToCell
    '   名称　：任期についての文章出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>任期についての文章出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetServiceDateFromToCell( _
        ByRef ioStrCell As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 任期についての文章出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetServiceDateFromToCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetFinishSentenceCell
    '   名称　：最後の文書出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>最後の文書出力位置取得セル処理</summary>
    ''' <param name="ioStrCell">出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetFinishSentenceCell( _
        ByRef ioStrCell As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 最後の文書出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetFinishSentenceCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExcelGetClosingRemarksCell
    '   名称　：以上出力位置取得セル処理
    '   概要  ：
    '   引数　：ByRef ioStrCell As String = 出力位置セル
    '   戻り値：True：正常, False：異常
    '   作成日：2012/03/20(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>以上出力位置セル取得処理</summary>
    ''' <param name="ioStrCell">出力位置セル</param>
    ''' <returns>True：正常, False：異常</returns>
    ''' <remarks></remarks>
    Public Function ExcelGetClosingRemarksCell( _
        ByRef ioStrCell As String _
    ) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 以上出力位置セル設定
            ioStrCell = Me.CallExcelMacro("DocConst.GetClosingRemarksCell", New String(0 - 1) {})

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, _
                SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

End Class

#End Region
