'===========================================================================================================
'   クラスＩＤ　　：CtlWageReductionEntryBase
'   クラス名称　　：賃金カット - 月例・時間内・争議行為ベース画面
'   備考  　　　　：
'===========================================================================================================

Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.UnionConst
Imports UnionAct.Business.Common
Imports UnionAct.Business.FinancialAffairs.WageReduction
Imports UnionAct.GUI.Common
Imports UnionAct.MDMode
Imports UnionAct.NSCLMsg

Namespace GUI.FinancialAffairs.WageReduction
    Public Class CtlWageReductionEntryBase
        Inherits FinancialAffairsBase

        Private _cancel As EventHandler

        Private _validator As ValidateDelegate
        Private _exception As AppUnionException
        Private _mode As MODE                               ' モード
        Private _year As String                             ' 年
        Private _month As String                            ' 月
        Private components As IContainer
        Protected _original As DataTable
        Protected _settingInEdit As GridSettingInfo()
        Protected _settingInRef As GridSettingInfo()
        Protected _beforeEditValue As Object
        Private _strNameForRight As String                  ' 画面ID
        Protected cmbYear As ComboBox                       ' 年コンボボックス
        Protected label6 As Label                           ' 年ラベル
        Protected cmbMonth As ComboBox                      ' 月コンボボックス
        Protected label7 As Label                           ' 月ラベル
        Protected WithEvents flxList As C1FlexGrid          ' フレックスグリッド
        Protected WithEvents btnInputFile As Button         ' ファイル取込ボタン
        Protected WithEvents btnOutputFile As Button        ' ファイル出力ボタン
        Protected WithEvents btnChange As Button            ' 内容変更ボタン
        Protected WithEvents btnRegist As Button            ' 登録ボタン
        Protected WithEvents btnPrint As Button             ' 一覧プレ印刷ボタン
        Protected WithEvents btnBackOrCancel As Button      ' 戻る・キャンセルボタン

        ' Nested Types
        Public Class LocalStyle
            ' Fields
            Public Const STYLE_DEDUCTION_COL As String = "deduction_col"
            Public Const STYLE_DEDUCTION_REF_COL As String = "deduction_ref_col"
        End Class

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        ''' <summary>コンストラクタ</summary>
        ''' <param name="Year">対象年</param>
        ''' <param name="Month">対象月</param>
        ''' <param name="strNameForRight">画面ID</param>
        ''' <param name="CancelHandler"></param>
        ''' <remarks></remarks>
        Public Sub New( _
            ByVal Year As String, _
            ByVal Month As String, _
            ByVal strNameForRight As String, _
            ByVal CancelHandler As EventHandler _
        )

            MyBase.New()
            Me.InitializeComponent()
            Me.TargetYear = Year                            ' 対象年
            Me.TargetMonth = Month                          ' 対象月
            Me._strNameForRight = strNameForRight           ' 画面ID
            Me.btnRegist.Enabled = Me.HasEntryPower         ' 登録ボタン活性・非活性
            Me.btnChange.Enabled = Me.HasEntryPower         ' 内容変更ボタン活性・非活性
            Me.btnPrint.Enabled = Me.HasPrintPower          ' 一覧プレ印刷ボタン活性・非活性
            Me.btnInputFile.Enabled = Me.HasInputPower      ' ファイル出力ボタン活性・非活性
            Me.btnOutputFile.Enabled = Me.HasOutputPower    ' ファイル出力ボタン活性・非活性
            Me._cancel = CancelHandler

        End Sub
#End Region

#Region " プロパティ "
        ''' <summary>データ行チェック</summary>
        Protected ReadOnly Property HasDataRow() As Boolean
            Get
                Return (Me.flxList.Rows.Count <> 2)
            End Get
        End Property

        ''' <summary>登録権限有無チェック</summary>
        Protected ReadOnly Property HasEntryPower() As Boolean
            Get
                Return MDFinanceCommon.GetEntryPower(Me._strNameForRight)
            End Get
        End Property

        ''' <summary></summary>
        Protected ReadOnly Property HasException() As Boolean
            Get
                If (Me._exception Is Nothing) Then
                    Return False
                End If
                Return True
            End Get
        End Property

        ''' <summary>ファイル取込権限チェック</summary>
        Protected ReadOnly Property HasInputPower() As Boolean
            Get
                Return MDFinanceCommon.GetInputPower(Me._strNameForRight)
            End Get
        End Property

        ''' <summary>ファイル出力権限チェック</summary>
        Protected ReadOnly Property HasOutputPower() As Boolean
            Get
                Return MDFinanceCommon.GetOutputPower(Me._strNameForRight)
            End Get
        End Property

        ''' <summary>印刷権限チェック</summary>
        Protected ReadOnly Property HasPrintPower() As Boolean
            Get
                Return MDFinanceCommon.GetPrintPower(Me._strNameForRight)
            End Get
        End Property

        ''' <summary>新規登録チェック</summary>
        Protected ReadOnly Property IsNew() As Boolean
            Get
                ' 対象年・対象月が無い場合、新規登録とみなす
                Return ((Me.TargetYear.Length = 0) AndAlso (Me.TargetMonth.Length = 0))
            End Get
        End Property

        ''' <summary>モード取得</summary>
        Protected Property ScreenMode() As Integer
            Get
                Return Me._mode
            End Get
            Set(ByVal value As Integer)
                Me._mode = value
                ' モード判定
                Select Case Me._mode
                    Case MODE.NEWENTRY, MODE.EDIT
                        Me.SetEditMode()        ' 修正モード設定処理
                        Return
                    Case MODE.REFER
                        Me.SetReferMode()       ' 照会モード設定処理
                        Return
                End Select
            End Set
        End Property

        ''' <summary>対象年</summary>
        Protected Property TargetYear() As String
            Get
                If (Not Me._year Is Nothing) Then
                    Return Me._year
                End If
                Return ""
            End Get
            Set(ByVal value As String)
                Me._year = If((value Is Nothing), "", value)
            End Set
        End Property

        ''' <summary>対象月</summary>
        Protected Property TargetMonth() As String
            Get
                If (Not Me._month Is Nothing) Then
                    Return Me._month
                End If
                Return ""
            End Get
            Set(ByVal value As String)
                Me._month = If((value Is Nothing), "", value)
            End Set
        End Property
#End Region

#Region " イベント "
#Region " CtlWageReductionEntryBase_Load：ロード処理 "
        ''' <summary>ロード処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub CtlWageReductionEntryBase_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            Try
                ' コンボボックス設定処理
                Me.SetComboItems()

                ' 照会・新規登録判定
                If Not Me.IsNew Then
                    ' 照会
                    Me.Query(Me.TargetYear, Me.TargetMonth)
                    Me.ScreenMode = MODE.REFER
                Else
                    ' 新規登録
                    Me.ScreenMode = MODE.NEWENTRY
                    Me.cmbMonth.Focus()
                End If
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try

        End Sub
#End Region

#Region " btnInputFile_Click：ファイル取込ボタン押下処理 "
        ''' <summary>ファイル取込ボタン押下処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub btnInputFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnInputFile.Click

        End Sub
#End Region

#Region " btnOutputFile_Click：ファイル出力ボタン押下処理 "
        ''' <summary>ファイル出力ボタン押下処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnOutputFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOutputFile.Click

            Try
                ' マウスカーソル砂時計
                Cursor.Current = Cursors.WaitCursor

                Dim class2 As New FactoryBusClass
                Dim command As CSVFileCommand = New CSVFileCommand
                Dim outputFileName As String = Me.GetOutputFileName
                Dim sfd As SaveFileDialog = command.ShowSaveCSVFileDialog((Regex.Replace(outputFileName, "[\\/:*?\""<>|]", "") & ".csv"))
                If (Not sfd Is Nothing) Then
                    Dim dTable As DataTable = DirectCast(Me.flxList.DataSource, DataTable).Copy
                    Dim str2 As String
                    For Each str2 In Me.GetNotOutputFileColumns
                        dTable.Columns.Remove(str2)
                    Next
                    command.WriteCSVFile(sfd, dTable, False)

                    ' 出力完了メッセージ
                    CLMsg.Show("GI0028")
                End If

            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})

            Finally
                ' マウスカーソルデフォルト
                Cursor.Current = Cursors.Default
            End Try

        End Sub
#End Region

#Region " btnChange_Click：内容変更ボタン押下処理 "
        ''' <summary>内容変更ボタン押下処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChange.Click

            ' 修正モード設定
            Me.ScreenMode = MODE.EDIT

            ' フレックスグリッドフォーカス設定
            Me.flxList.Focus()

        End Sub
#End Region

#Region " btnRegist_Click：登録ボタン押下処理 "
        ''' <summary>登録ボタン押下処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnRegist_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegist.Click

            Try
                Me.ValidateFields()
                If Me.IsNew Then
                    ' 新規登録
                    Me.SaveNewData(Me.cmbYear.Text, Me.cmbMonth.Text)
                Else
                    ' 内容変更
                    Me.UpdateData(Me.TargetYear, Me.TargetMonth)
                End If

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString, vbExclamation)

            Catch exception3 As Exception
                ' ADD 2012/06/14
            End Try

        End Sub
#End Region

#Region " btnPrint_Click：一覧プレ印刷ボタン押下処理 "
        ''' <summary>一覧プレ印刷ボタン押下処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click

            Try
                Cursor.Current = Cursors.WaitCursor
                Me.PrintList()
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})

            Finally
                Cursor.Current = Cursors.Default

            End Try

        End Sub
#End Region

#Region " btnBackOrCancel_Click：戻る・キャンセルボタン押下処理 "
        ''' <summary>戻る・キャンセルボタン押下処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnBackOrCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackOrCancel.Click

            ' モード判定
            Select Case Me.ScreenMode

                Case MODE.NEWENTRY
                    '-------------------------------------------------------------------------------
                    '   新規登録
                    '-------------------------------------------------------------------------------
                    ' 破棄確認メッセージ
                    If (CLMsg.Show("GQ0007") <> DialogResult.No) Then
                        Me.FireCancel(Me, EventArgs.Empty)
                        Return
                    End If
                    Return

                Case MODE.EDIT
                    '-------------------------------------------------------------------------------
                    '   修正
                    '-------------------------------------------------------------------------------
                    ' 破棄確認メッセージ
                    If (CLMsg.Show("GQ0007") <> DialogResult.Yes) Then
                        Exit Select
                    End If
                    Me.ScreenMode = MODE.REFER
                    Return

                Case MODE.REFER
                    Me.FireCancel(Me, EventArgs.Empty)
                    Exit Select

                Case Else
                    '-------------------------------------------------------------------------------
                    '   その他
                    '-------------------------------------------------------------------------------
                    Return

            End Select

        End Sub
#End Region
#End Region

#Region " AddFlexGridStyle：フレックスグリッドスタイル設定処理 "
        ''' <summary>フレックスグリッドスタイル設定処理</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub AddFlexGridStyle()

            '---------------------------------------------------------------------------------------
            '   フレックスグリッドスタイル設定
            '---------------------------------------------------------------------------------------
            Dim style As CellStyle = Nothing

            ' 固定カラム
            style = Me.flxList.Styles.Add("fixed_col")                      ' スタイル追加
            style.Font = FinancialAffairsUtility.GetGridFontNormal          ' フォント設定：フレックスグリッドフォント通常設定処理
            style.DataType = GetType(Integer)                               ' データタイプ設定：整数型
            style.TextAlign = TextAlignEnum.RightCenter                     ' 表示位置設定：横右詰め・縦中央揃え

            ' 社員番号カラム
            style = Me.flxList.Styles.Add("employee_number_col_nolink")     ' スタイル追加
            style.Font = FinancialAffairsUtility.GetGridFontNormal          ' フォント設定：フレックスグリッドフォント通常設定処理
            style.DataType = GetType(Long)                                  ' データタイプ設定：長整数型
            style.TextAlign = TextAlignEnum.RightCenter                     ' 表示位置設定：横右詰め・縦中央揃え

            ' 名前カラム
            style = Me.flxList.Styles.Add("name_col")                       ' スタイル追加
            style.Font = FinancialAffairsUtility.GetGridFontNormal          ' フォント設定：フレックスグリッドフォント通常設定処理
            style.DataType = GetType(String)                                ' データタイプ設定：文字列方
            style.TextAlign = TextAlignEnum.LeftCenter                      ' 表示位置設定：横左詰め・縦中央揃え
            style.BackColor = Color.LightYellow                             ' 背景色設定：薄黄色

            ' 読取専用カラム
            style = Me.flxList.Styles.Add("readonly_col")                   ' スタイル追加
            style.Font = FinancialAffairsUtility.GetGridFontNormal          ' フォント設定：フレックスグリッドフォント通常設定処理
            style.DataType = GetType(String)                                ' データタイプ設定：文字列方
            style.TextAlign = TextAlignEnum.CenterCenter                    ' 表示位置設定：横中央揃え・縦中央揃え
            style.BackColor = Color.LightYellow                             ' 背景色設定：薄黄色

            ' 控除額計カラム
            style = Me.flxList.Styles.Add("deduction_col")                  ' スタイル追加
            style.Font = FinancialAffairsUtility.GetGridFontNormal          ' フォント設定：フレックスグリッドフォント通常設定処理
            style.DataType = GetType(Long)                                  ' データタイプ設定：長整数型
            style.TextAlign = TextAlignEnum.RightCenter                     ' 表示位置設定：横右詰め・縦中央揃え
            style.Format = "N0"                                             ' 書式設定：カンマ付小数点なしの数値

            ' エラーセル
            Me.flxList.Styles.Add("error_cell").BackColor = Color.LightPink ' スタイル追加

            '---------------------------------------------------------------------------------------
            '   ヘッダーカラムタイトル設定
            '---------------------------------------------------------------------------------------
            flxList.Cols.Item(1).Caption = "社員番号"                       ' 01. 社員番号
            flxList.Cols.Item(2).Caption = "CD"                             ' 02. CD
            flxList.Cols.Item(3).Caption = "名前"                           ' 03. 名前
            flxList.Cols.Item(4).Caption = "組合員種別"                     ' 04. 組合員種別
            flxList.Cols.Item(5).Caption = "会社所属"                       ' 05. 会社所属
            flxList.Cols.Item(6).Caption = "組合支部"                       ' 06. 組合支部
            flxList.Cols.Item(7).Caption = "資格"                           ' 07. 資格
            flxList.Cols.Item(8).Caption = "機種"                           ' 08. 機種
            flxList.Cols.Item(9).Caption = "控除額計"                       ' 09. 控除額計

        End Sub
#End Region

#Region " AddValidateError：エラーメッセージ追加処理 "
        ''' <summary>エラーメッセージ追加処理</summary>
        ''' <param name="MethodName">メソッド名</param>
        ''' <param name="MessageId">メッセージID</param>
        ''' <param name="args">メッセージパラメータ</param>
        ''' <remarks></remarks>
        Protected Sub AddValidateError( _
            ByVal MethodName As MethodBase, _
            ByVal MessageId As String, _
            ByVal ParamArray args As String() _
        )

            If (Me._exception Is Nothing) Then
                Me._exception = New AppUnionException(MethodName, New Exception, MessageId, args)
            Else
                Me._exception.AddExceptionData(MessageId, args)
            End If

        End Sub
#End Region

#Region " FindMemberList：社員情報取得処理（Excelデータ取込） "
        ''' <summary>社員情報取得処理（Excelデータ取込）</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub FindMemberList(ByVal xlsTable As DataTable)
        End Sub
#End Region

#Region " CalcTotal：件数・控除額計取得処理（継承元） "
        ''' <summary>件数・控除額計取得処理（継承元）</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub CalcTotal()
        End Sub
#End Region

#Region " EnableComboBox： "
        ''' <summary></summary>
        ''' <param name="fEnabeed"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub EnableComboBox(ByVal fEnabeed As Boolean)
        End Sub
#End Region

#Region " CancelMultiSelect： "
        ''' <summary></summary>
        ''' <remarks></remarks>
        Protected Sub CancelMultiSelect()
            Me.flxList.RowSel = Me.flxList.Row
        End Sub
#End Region

#Region " ClearException： "
        ''' <summary></summary>
        ''' <remarks></remarks>
        Protected Sub ClearException()
            Me._exception = Nothing
        End Sub
#End Region

#Region " CountValidRows：件数取得処理 "
        ''' <summary>件数取得処理</summary>
        ''' <param name="ValidateColumn">対象カラム</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function CountValidRows(ByVal ValidateColumn As Integer) As Integer

            ' 件数
            Dim num As Integer = 0

            ' フレックスグリッド件数分ループ
            For i As Integer = 1 To Me.flxList.Rows.Count - 1
                ' 対象カラムが空白の場合、0。空白以外を 1としてカウント
                num = (num + If((Me.flxList.Item(i, ValidateColumn) Is Nothing), 0, 1))
            Next i

            ' 戻り値に件数を設定
            Return num

        End Function
#End Region

#Region " DeleteRow： "
        ''' <summary></summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub DeleteRow(ByVal e As RowColEventArgs)
            Dim result As DialogResult = CLMsg.Show("GQ0008")
            e.Cancel = (result = DialogResult.No)
        End Sub
#End Region

#Region " Dispose：リソース開放処理 "
        ''' <summary>リソース開放処理</summary>
        ''' <param name="disposing"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region

#Region " FireCancel： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub FireCancel(ByVal sender As Object, ByVal e As EventArgs)
            If (Not Me._cancel Is Nothing) Then
                Me._cancel.Invoke(sender, e)
            End If
        End Sub
#End Region

#Region " FireCancel： "
        ''' <summary></summary>
        ''' <remarks></remarks>
        Protected Sub FireInvalidEntryError()
            If (Not Me._exception Is Nothing) Then
                Throw Me._exception
            End If
        End Sub
#End Region



#Region " flxList_AfterAddRow： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_AfterAddRow(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.AfterAddRow
            Me.flxList.Item(e.Row, 0) = e.Row
        End Sub
#End Region

#Region " flxList_AfterDeleteRow： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_AfterDeleteRow(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.AfterDeleteRow
            Me.NumberingSequence()
            Me.CalcTotal()
        End Sub
#End Region

#Region " flxList_AfterEdit： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_AfterEdit(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.AfterEdit
            Me.GridAfterEdit(e)
        End Sub
#End Region

#Region " flxList_AfterSort： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_AfterSort(ByVal sender As Object, ByVal e As SortColEventArgs) Handles flxList.AfterSort
            Me.NumberingSequence()
        End Sub
#End Region

#Region " flxList_BeforeDeleteRow： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_BeforeDeleteRow(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.BeforeDeleteRow
            Me.DeleteRow(e)
        End Sub
#End Region

#Region " flxList_SelChange： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_SelChange(ByVal sender As Object, ByVal e As EventArgs) Handles flxList.SelChange
            Me.CancelMultiSelect()
        End Sub
#End Region

#Region " flxList_StartEdit： "
        ''' <summary></summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_StartEdit(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.StartEdit
            Me._beforeEditValue = Me.flxList.Item(e.Row, e.Col)
        End Sub
#End Region



#Region " GetMemberInfo：社員情報取得処理 "
        ''' <summary>社員情報取得処理</summary>
        ''' <param name="business"></param>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns>社員情報</returns>
        ''' <remarks></remarks>
        Protected Function GetMemberInfo( _
            ByVal business As WageReductionBase, _
            ByVal EmployeeNumber As String, _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataTable
            Return business.FindMember(EmployeeNumber, (TargetYear & TargetMonth))
        End Function
#End Region

#Region " GetMemberInfoList：社員情報取得処理（Excelデータ取込） "
        ''' <summary>社員情報取得処理（Excelデータ取込）</summary>
        ''' <param name="business"></param>
        ''' <param name="EmployeeNumberList">社員番号リスト</param>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns>社員情報リスト</returns>
        ''' <remarks></remarks>
        Protected Function GetMemberInfoList( _
            ByVal business As WageReductionBase, _
            ByVal EmployeeNumberList As String, _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataTable
            Return business.FindMemberList(EmployeeNumberList, (TargetYear & TargetMonth))
        End Function
#End Region

#Region " FindExistsMember：社員番号存在チェック処理 "
        ''' <summary>社員番号存在チェック処理</summary>
        ''' <param name="business"></param>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象年</param>
        ''' <returns>True：存在する, False：存在しない</returns>
        ''' <remarks></remarks>
        Protected Function FindExistsMember( _
            ByVal business As WageReductionBase, _
            ByVal EmployeeNumber As String, _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As Boolean
            Return business.FindExistMember(EmployeeNumber, (TargetYear & TargetMonth))
        End Function
#End Region

#Region " GetNotOutputFileColumns： "
        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetNotOutputFileColumns() As String()
            Return New String() {"ユーザＩＤ"}
        End Function
#End Region

#Region " GetNotOutputFileColumns： "
        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetOutputFileName() As String
            Return Nothing
        End Function
#End Region

#Region " GridAfterEdit： "
        ''' <summary></summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub GridAfterEdit(ByRef e As RowColEventArgs)
        End Sub
#End Region

#Region " InitGrid：フレックスグリッド初期化処理 "
        ''' <summary>フレックスグリッド初期化処理</summary>
        ''' <param name="Settings"></param>
        ''' <param name="isEditable"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub InitGrid( _
            ByVal Settings As GridSettingInfo(), _
            ByVal isEditable As Boolean _
        )

            If (Not Settings Is Nothing) Then
                Me.flxList.AllowEditing = True                      ' 編集可能
                Me.flxList.AllowAddNew = isEditable                 ' 最終行に新しい行追加設定
                Me.flxList.AllowDelete = isEditable                 ' 削除可能設定
                Me.flxList.AllowResizing = AllowResizingEnum.None   ' 行列のサイズ変更をできなくする
                For i As Integer = 0 To Settings.Length - 1
                    'For i = 0 To Me.flxList.Cols.Count - 1
                    Me.flxList.Cols.Item(i).AllowEditing = Settings(i).AllowEditing
                    Me.flxList.Cols.Item(i).Visible = Settings(i).Visible
                Next i
                FinancialAffairsUtility.ApplyGridStyle(Me.flxList, Settings)
            End If

        End Sub
#End Region

#Region " コントロール初期化処理 "
        ''' <summary>コントロール初期化処理</summary>
        ''' <remarks></remarks>
        Private Sub InitializeComponent()

            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CtlWageReductionEntryBase))
            Me.cmbYear = New System.Windows.Forms.ComboBox          ' 年コンボボックス
            Me.label6 = New System.Windows.Forms.Label              ' 年ラベル
            Me.cmbMonth = New System.Windows.Forms.ComboBox         ' 月コンボボックス
            Me.label7 = New System.Windows.Forms.Label              ' 月ラベル
            Me.flxList = New C1.Win.C1FlexGrid.C1FlexGrid           ' フレックスグリッド
            Me.btnInputFile = New System.Windows.Forms.Button       ' ファイル取込ボタン
            Me.btnOutputFile = New System.Windows.Forms.Button      ' ファイル出力ボタン
            Me.btnRegist = New System.Windows.Forms.Button          ' 登録ボタン
            Me.btnChange = New System.Windows.Forms.Button          ' 内容変更ボタン
            Me.btnPrint = New System.Windows.Forms.Button           ' 一覧プレ印刷ボタン
            Me.btnBackOrCancel = New System.Windows.Forms.Button    ' 戻るボタン
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmbYear：年コンボボックス
            '
            Me.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbYear.FormattingEnabled = True
            Me.cmbYear.Items.AddRange(New Object() {"2006", "2007", "2008", "2009", "2010"})
            Me.cmbYear.Location = New System.Drawing.Point(86, 15)
            Me.cmbYear.Name = "cmbYear"
            Me.cmbYear.Size = New System.Drawing.Size(63, 24)
            Me.cmbYear.TabIndex = 0
            '
            'label6：年ラベル
            '
            Me.label6.AutoSize = True
            Me.label6.Location = New System.Drawing.Point(155, 19)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(24, 16)
            Me.label6.TabIndex = 1
            Me.label6.Text = "年"
            '
            'cmbMonth：月コンボボックス
            '
            Me.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbMonth.FormattingEnabled = True
            Me.cmbMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
            Me.cmbMonth.Location = New System.Drawing.Point(180, 15)
            Me.cmbMonth.MaxDropDownItems = 12
            Me.cmbMonth.Name = "cmbMonth"
            Me.cmbMonth.Size = New System.Drawing.Size(50, 24)
            Me.cmbMonth.TabIndex = 2
            '
            'label7：月ラベル
            '
            Me.label7.AutoSize = True
            Me.label7.Location = New System.Drawing.Point(236, 20)
            Me.label7.Name = "label7"
            Me.label7.Size = New System.Drawing.Size(24, 16)
            Me.label7.TabIndex = 3
            Me.label7.Text = "月"
            '
            'flxList：フレックスグリッド
            '
            Me.flxList.AllowAddNew = True
            Me.flxList.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
            Me.flxList.AllowEditing = False
            Me.flxList.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            Me.flxList.ColumnInfo = "12,1,0,0,0,110,Columns:"
            Me.flxList.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
            Me.flxList.Location = New System.Drawing.Point(86, 50)
            Me.flxList.Name = "flxList"
            Me.flxList.Rows.Count = 1
            Me.flxList.Rows.DefaultSize = 22
            Me.flxList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.flxList.Size = New System.Drawing.Size(855, 609)
            Me.flxList.StyleInfo = resources.GetString("flxList.StyleInfo")
            Me.flxList.TabIndex = 4
            '
            'btnInputFile：ファイル取込ボタン
            '
            Me.btnInputFile.Location = New System.Drawing.Point(116, 704)
            Me.btnInputFile.Name = "btnInputFile"
            Me.btnInputFile.Size = New System.Drawing.Size(116, 32)
            Me.btnInputFile.TabIndex = 5
            Me.btnInputFile.Text = "ファイル取込"
            Me.btnInputFile.UseVisualStyleBackColor = True
            '
            'btnOutputFile：ファイル出力ボタン
            '
            Me.btnOutputFile.Location = New System.Drawing.Point(268, 704)
            Me.btnOutputFile.Name = "btnOutputFile"
            Me.btnOutputFile.Size = New System.Drawing.Size(116, 32)
            Me.btnOutputFile.TabIndex = 6
            Me.btnOutputFile.Text = "ファイル出力"
            Me.btnOutputFile.UseVisualStyleBackColor = True
            '
            'btnChange：内容変更ボタン
            '
            Me.btnChange.Location = New System.Drawing.Point(420, 704)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New System.Drawing.Size(116, 32)
            Me.btnChange.TabIndex = 7
            Me.btnChange.Text = "内容変更"
            Me.btnChange.UseVisualStyleBackColor = True
            '
            'btnRegist：登録ボタン
            '
            Me.btnRegist.Location = New System.Drawing.Point(420, 704)
            Me.btnRegist.Name = "btnRegist"
            Me.btnRegist.Size = New System.Drawing.Size(116, 32)
            Me.btnRegist.TabIndex = 7
            Me.btnRegist.Text = "登録"
            Me.btnRegist.UseVisualStyleBackColor = True
            '
            'btnPrint：一覧プレ印刷ボタン
            '
            Me.btnPrint.Location = New System.Drawing.Point(572, 704)
            Me.btnPrint.Name = "btnPrint"
            Me.btnPrint.Size = New System.Drawing.Size(116, 32)
            Me.btnPrint.TabIndex = 8
            Me.btnPrint.Text = "一覧プレ印刷"
            Me.btnPrint.UseVisualStyleBackColor = True
            '
            'btnBackOrCancel：戻る・キャンセルボタン
            '
            Me.btnBackOrCancel.Location = New System.Drawing.Point(724, 704)
            Me.btnBackOrCancel.Name = "btnBackOrCancel"
            Me.btnBackOrCancel.Size = New System.Drawing.Size(116, 32)
            Me.btnBackOrCancel.TabIndex = 9
            Me.btnBackOrCancel.Text = "ｷｬﾝｾﾙ"
            Me.btnBackOrCancel.UseVisualStyleBackColor = True
            '
            'CtlWageReductionEntryBase
            '
            Me.Controls.Add(Me.cmbYear)             ' 年コンボボックス
            Me.Controls.Add(Me.label6)              ' 年ラベル
            Me.Controls.Add(Me.cmbMonth)            ' 月コンボボックス
            Me.Controls.Add(Me.label7)              ' 月ラベル
            Me.Controls.Add(Me.flxList)             ' フレックスグリッド
            Me.Controls.Add(Me.btnInputFile)        ' ファイル取込ボタン
            Me.Controls.Add(Me.btnOutputFile)       ' ファイル出力ボタン
            Me.Controls.Add(Me.btnChange)           ' 内容変更ボタン
            Me.Controls.Add(Me.btnRegist)           ' 登録ボタン
            Me.Controls.Add(Me.btnPrint)            ' 一覧プレ印刷ボタン
            Me.Controls.Add(Me.btnBackOrCancel)     ' 戻る・キャンセルボタン
            Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.Margin = New System.Windows.Forms.Padding(4)
            Me.Name = "CtlWageReductionEntryBase"
            Me.Size = New System.Drawing.Size(1026, 759)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
#End Region

#Region " IsDupulicate： "
        ''' <summary></summary>
        ''' <param name="grid"></param>
        ''' <param name="FindValue"></param>
        ''' <param name="StartRow"></param>
        ''' <param name="SerachColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function IsDupulicate( _
            ByVal grid As C1FlexGrid, _
            ByVal FindValue As Object, _
            ByVal StartRow As Integer, _
            ByVal SerachColumn As Integer _
        ) As Boolean
            Dim num As Integer = grid.FindRow(FindValue, StartRow, SerachColumn, True)
            Return ((num <> -1) AndAlso (num <> (StartRow - 1)))
        End Function
#End Region

#Region " IsDupulicate2： "
        ''' <summary></summary>
        ''' <param name="grid"></param>
        ''' <param name="FindValue"></param>
        ''' <param name="StartRow"></param>
        ''' <param name="SerachColumn"></param>
        ''' <param name="findNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function IsDupulicate2( _
            ByVal grid As C1FlexGrid, _
            ByVal FindValue As Object, _
            ByVal StartRow As Integer, _
            ByVal SerachColumn As Integer, _
            ByRef findNo As Integer _
        ) As Boolean
            Dim num As Integer = grid.FindRow(FindValue, StartRow, SerachColumn, True)
            findNo = num
            Return ((num <> -1) AndAlso (num <> (StartRow - 1)))
        End Function
#End Region

#Region " IsEmptyCell：セル値有無チェック処理 "
        ''' <summary>セル値有無チェック処理</summary>
        ''' <param name="cell"></param>
        ''' <returns>True：有り, False：無し</returns>
        ''' <remarks></remarks>
        Protected Function IsEmptyCell(ByVal cell As Object) As Boolean

            ' セル値有無チェック
            If ((Not cell Is Nothing) _
            AndAlso (cell.ToString.Length <> 0)) Then
                Return False
            Else
                Return True
            End If

        End Function
#End Region

#Region " NumberingSequence：NO列ナンバリング処理 "
        ''' <summary>NO列ナンバリング処理</summary>
        ''' <remarks></remarks>
        Protected Sub NumberingSequence()
            Dim i As Integer
            For i = 1 To (Me.flxList.Rows.Count - 1) - 1
                Me.flxList.Item(i, 0) = i
            Next i
        End Sub
#End Region

#Region " PrintList： "
        ''' <summary></summary>
        ''' <remarks></remarks>
        Protected Overridable Sub PrintList()

        End Sub
#End Region

#Region " Function：社員情報処理（時間内・争議行為） "
        ''' <summary>社員情報処理（時間内・争議行為）</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="staf_id_list">社員番号リスト</param>
        ''' <remarks></remarks>
        Protected Overridable Function GetStafInfoInTimeStrike( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal staf_id_list As String _
        ) As DataTable
            Return Nothing
        End Function
#End Region

#Region " GetStafInfoBounus：社員情報処理（一時金） "
        ''' <summary>社員情報処理（一時金）</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="staf_id_list">社員番号リスト</param>
        ''' <remarks></remarks>
        Protected Overridable Function GetStafInfoBounus( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal staf_id_list As String _
        ) As DataTable
            Return Nothing
        End Function
#End Region

#Region " Query：検索処理 "
        ''' <summary>検索処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overridable Sub Query( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        )
        End Sub
#End Region

#Region " SaveNewData：新規登録処理 "
        ''' <summary>新規登録処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overridable Sub SaveNewData( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        )
        End Sub
#End Region

#Region " SetComboItems：コンボボックス設定処理 "
        ''' <summary>コンボボックス設定処理</summary>
        ''' <remarks></remarks>
        Private Sub SetComboItems()

            If Me.IsNew Then
                ' コンボボックス設定処理（新規登録）
                Me.SetComboItemsForNewEntry()
            Else
                ' コンボボックス設定処理（参照）
                Me.SetComboItemsForRefer()
            End If

        End Sub
#End Region

#Region " SetComboItemsForNewEntry：コンボボックス設定処理（新規登録） "
        ''' <summary>コンボボックス設定処理（新規登録）</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetComboItemsForNewEntry()
        End Sub
#End Region

#Region " SetComboItemsForRefer：コンボボックス設定処理（照会） "
        ''' <summary>コンボボックス設定処理（照会）</summary>
        ''' <remarks></remarks>
        Private Sub SetComboItemsForRefer()

            ' 年コンボボックス
            Me.cmbYear.Items.Clear()                ' リストクリア
            Me.cmbYear.Items.Add(Me.TargetYear)     ' 対象年追加
            Me.cmbYear.SelectedIndex = 0            ' 1番目表示（対象年表示）

            ' 月コンボボックス
            Me.cmbMonth.Items.Clear()               ' リストクリア
            Me.cmbMonth.Items.Add(Me.TargetMonth)   ' 対象月追加
            Me.cmbMonth.SelectedIndex = 0           ' 1番目表示（対象月表示）

        End Sub
#End Region

#Region " SetEditMode：編集モード設定処理 "
        ''' <summary>編集モード設定処理</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetEditMode()

            Me.InitGrid(If((Me._settingInEdit Is Nothing), Me._settingInRef, Me._settingInEdit), True)

            Me.btnInputFile.Enabled = True                      ' ファイル取込ボタン表示
            Me.btnOutputFile.Enabled = False                    ' ファイル出力ボタン非表示
            Me.btnRegist.Visible = True                         ' 登録ボタン表示
            Me.btnChange.Visible = False                        ' 内容変更ボタン非表示
            Me.btnPrint.Enabled = False                         ' 一覧プレ印刷ボタン非表示
            Me.btnBackOrCancel.Text = "キャンセル"              ' 戻る・キャンセルボタン名設定
            Me.CalcTotal()
            Me.EnableComboBox(True)

        End Sub
#End Region

#Region " SetReferMode：照会モード設定処理 "
        ''' <summary>照会モード設定処理</summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetReferMode()

            If (Not Me._original Is Nothing) Then
                Dim result As DataTable = Me._original.Copy
                Me.ShowData(result)
                Me.NumberingSequence()
                Me.CalcTotal()
                Me.EnableComboBox(False)
                Me.InitGrid(Me._settingInRef, False)
            End If
            Me.btnInputFile.Visible = False                     ' ファイル取込ボタン非表示
            Me.btnRegist.Visible = False                        ' 登録ボタン非表示
            Me.btnChange.Visible = True                         ' 内容変更ボタン表示
            Me.btnPrint.Enabled = Me.HasPrintPower              ' 一覧プレ印刷ボタン
            Me.btnOutputFile.Enabled = Me.HasOutputPower        ' ファイル出力ボタン
            Me.btnBackOrCancel.Text = "戻る"                    ' 戻る・キャンセルボタン名設定

        End Sub
#End Region

#Region " ShowData：データ設定処理 "
        ''' <summary>データ設定処理</summary>
        ''' <param name="result"></param>
        ''' <remarks></remarks>
        Private Sub ShowData(ByVal result As DataTable)
            Me.flxList.DataSource = result
        End Sub
#End Region

#Region " UpdateData：データ内容変更処理 "
        ''' <summary>内容変更処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overridable Sub UpdateData(ByVal TargetYear As String, ByVal TargetMonth As String)
        End Sub
#End Region

#Region " SetValidator： "
        ''' <summary></summary>
        ''' <param name="NewValidator"></param>
        ''' <remarks></remarks>
        Public Sub SetValidator(ByVal NewValidator As ValidateDelegate)
            Me._validator = NewValidator
        End Sub
#End Region

#Region " ValidateFields： "
        ''' <summary></summary>
        ''' <remarks></remarks>
        Public Overridable Sub ValidateFields()
            If (Not Me._validator Is Nothing) Then
                Me._validator.Invoke()
            End If
        End Sub
#End Region

        'Protected Function GetFactory() As FactoryBusClass
        '    Return MyBase._factory
        'End Function

    End Class
End Namespace
