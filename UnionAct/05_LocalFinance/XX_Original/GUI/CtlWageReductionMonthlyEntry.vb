'===========================================================================================================
'   クラスＩＤ　　：CtlWageReductionMonthlyEntry
'   クラス名称　　：賃金カット - 月例・時間内（照会・新規登録）・争議行為（照会・新規登録）画面クラス
'   備考  　　　　：賃金カット - 月例・時間内（照会）画面
'                   賃金カット - 月例・時間内（新規登録）画面
'                   賃金カット - 月例・争議行為（照会）画面
'                   賃金カット - 月例・争議行為（新規登録）画面
'===========================================================================================================

Imports C1.Win.C1FlexGrid
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs.WageReduction
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionConst
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.Document
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Namespace GUI.FinancialAffairs.WageReduction
    Public Class CtlWageReductionMonthlyEntry
        Inherits CtlWageReductionEntryBase

        Private _business As WageReductionMonthlyCommand
        Private _kind As WAGE_REDUCTION_KIND
        Private components As IContainer
        Private lblCount As Label
        Private lblSummary As Label

#Region " 列挙 "
        ''' <summary>画面カラム</summary>
        ''' <remarks></remarks>
        Private Enum COLINDX
            SEQ = 0                         ' 01. No
            EMPLOYEE_NUMBER = 1             ' 02. 社員番号
            DIGIT = 2                       ' 03. CD
            NAME = 3                        ' 04. 名前
            STAF_KIND = 4                   ' 05. 組合員種別
            COMPANY_BRANCH = 5              ' 06. 会社所属
            UNION_BRANCH = 6                ' 07. 組合支部
            LICENSE = 7                     ' 08. 資格
            MODEL = 8                       ' 09. 機種
            DEDUCTION = 9                   ' 10. 控除額
            USER_ID = 10                    ' 11. ユーザーID
        End Enum
#End Region

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="strNameForRight">画面ID</param>
        ''' <param name="kind">種別</param>
        ''' <param name="CancelHandler"></param>
        ''' <remarks></remarks>
        Public Sub New( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal strNameForRight As String, _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal CancelHandler As EventHandler _
        )

            MyBase.New(TargetYear, TargetMonth, strNameForRight, CancelHandler)
            Me.InitializeComponent()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(40, "fixed_col", False, False, False, False, True), New GridSettingInfo(100, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(&H23, "readonly_col", False, False, False, False, True), New GridSettingInfo(140, "name_col", False, False, True, False, True), New GridSettingInfo(&H73, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
            MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(40, "fixed_col", False, False, False, False, True), New GridSettingInfo(100, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(&H23, "readonly_col", False, False, False, False, True), New GridSettingInfo(140, "name_col", False, False, True, False, True), New GridSettingInfo(&H73, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
            'MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(40, "fixed_col", False, False, False, False, True), New GridSettingInfo(100, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(&H23, "readonly_col", False, False, False, False, True), New GridSettingInfo(140, "name_col", False, False, True, False, True), New GridSettingInfo(&H73, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True)}
            'MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(40, "fixed_col", False, False, False, False, True), New GridSettingInfo(100, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(&H23, "readonly_col", False, False, False, False, True), New GridSettingInfo(140, "name_col", False, False, True, False, True), New GridSettingInfo(&H73, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(70, "readonly_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True)}
            Try
                Me._kind = kind
                Me._business = New WageReductionMonthlyCommand
                Me.AddFlexGridStyle()
                MyBase.SetValidator(New ValidateDelegate(AddressOf Me.ValidateGridData))
            Catch exception As Exception
                If TypeOf exception Is SysUnionException Then
                    DirectCast(exception, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})
            End Try
        End Sub
#End Region

#Region " btnInputFile_Click：ファイル取込ボタン押下処理 "
        ''' <summary>ファイル取込ボタン押下処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub btnInputFile_Click(ByVal sender As Object, ByVal e As EventArgs)

            Dim ofd As OpenFileDialog = Nothing     ' ダイアログボックス
            Dim file As String = ""                 ' ファイル名
            Dim targetYM As String = ""             ' 対象年月
            Dim tblXls As DataTable = Nothing       ' Excel読込データ
            Dim tblStaf As DataTable = Nothing      ' 社員情報データ
            Dim staf_id_list As String = ""         ' 社員番号リスト
            Dim lngDeductionReverse As Long = 0     ' 控除額（マイナス値 ⇒ プラス値）

            ' データチェック
            If Me.flxList.Rows.Count > 2 Then
                ' 破棄確認メッセージ
                If CLMsg.Show("GQ0007") = DialogResult.No Then
                    ' 処理を抜ける
                    Exit Sub
                End If
            End If

            Try
                ' マウスカーソル砂時計
                Cursor.Current = Cursors.WaitCursor

                ' コマンドオブジェクト生成
                Dim command As ExcelFileCommand = New ExcelFileCommand

                '-----------------------------------------------------------------------------------
                '   「ファイルを開く」ダイアログボックス表示処理
                '-----------------------------------------------------------------------------------
                ofd = command.ShowOpenExcelFileDialog()

                ' キャンセル押下以外の場合
                If (Not ofd Is Nothing) Then

                    ' しばらくお待ちください画面表示
                    FrmWaitInfo.ShowWaitForm(Nothing)

                    '-------------------------------------------------------------------------------
                    '   Excelファイルチェック・読込処理（時間内・争議講義）
                    '-------------------------------------------------------------------------------
                    ' ファイル名取得
                    file = ofd.FileName

                    ' 対象年月取得
                    targetYM = Me.cmbYear.Text & Me.cmbMonth.Text

                    ' Excelファイルチェック・読込処理（時間内・争議講義）
                    tblXls = command.CheckReadExcelInTimeStrike(file, targetYM)

                    '-------------------------------------------------------------------------------
                    '   社員情報取得
                    '-------------------------------------------------------------------------------
                    ' 社員番号リスト取得（カンマ区切り）
                    For i As Integer = 0 To tblXls.Rows.Count - 1 Step 1
                        If i = 0 Then
                            staf_id_list = "'" & tblXls.Rows(i).Item(1) & "'"                       ' 1レコード目
                        Else
                            staf_id_list += ",'" & tblXls.Rows(i).Item(1) & "'"                     ' 2レコード以降
                        End If
                    Next i

                    ' 社員番号リストの先頭・後尾の「'」を削除
                    staf_id_list = staf_id_list.Substring(1, staf_id_list.Length - 1)               ' 先頭「'」の文字を削除
                    staf_id_list = staf_id_list.Substring(0, staf_id_list.Length - 1)               ' 後尾「'」の文字を削除

                    ' Excelデータ取込処理（時間内・争議行為）
                    tblStaf = Me.GetStafInfoInTimeStrike(Me.cmbYear.Text, Me.cmbMonth.Text, staf_id_list)

                    '-------------------------------------------------------------------------------
                    '   画面データ設定
                    '-------------------------------------------------------------------------------
                    ' フレックスグリッド描画停止
                    Me.flxList.BeginUpdate()

                    ' フレックスグリッドクリア
                    For m As Integer = 0 To Me.flxList.Rows.Count - 2 Step 1
                        Me.flxList.RemoveItem()
                    Next m

                    ' 件数分新規行追加
                    Me.flxList.Rows.Add(tblStaf.Rows.Count)

                    ' データ数分ループ
                    For j As Integer = 0 To tblStaf.Rows.Count - 1 Step 1

                        ' 各情報設定
                        Dim k As Integer = j + 1
                        Me.flxList.Rows(k).Item(0) = k.ToString()                                   ' 01. No
                        Me.flxList.Rows(k).Item(1) = tblStaf.Rows(j).Item(0).ToString()             ' 02. 社員番号
                        Me.flxList.Rows(k).Item(2) = tblStaf.Rows(j).Item(1).ToString()             ' 03. CD
                        Me.flxList.Rows(k).Item(3) = tblStaf.Rows(j).Item(2).ToString()             ' 04. 名前
                        Me.flxList.Rows(k).Item(4) = tblStaf.Rows(j).Item(3).ToString()             ' 05. 組合員種別
                        Me.flxList.Rows(k).Item(5) = tblStaf.Rows(j).Item(4).ToString()             ' 06. 会社所属
                        Me.flxList.Rows(k).Item(6) = tblStaf.Rows(j).Item(5).ToString()             ' 07. 組合支部
                        Me.flxList.Rows(k).Item(7) = tblStaf.Rows(j).Item(6).ToString()             ' 08. 資格
                        Me.flxList.Rows(k).Item(8) = tblStaf.Rows(j).Item(7).ToString()             ' 09. 機種
                        Me.flxList.Rows(k).Item(9) = ""                                             ' 10. 控除額
                        Me.flxList.Rows(k).Item(10) = tblStaf.Rows(j).Item(8).ToString()            ' 11. ユーザーID

                        '---------------------------------------------------------------------------
                        '   10. 控除額
                        '---------------------------------------------------------------------------
                        ' 社員番号を突き合わせて、控除額を設定
                        For l As Integer = 0 To tblXls.Rows.Count - 1 Step 1
                            ' 読み取ったExcelデータの社員番号と社員情報の社員番号が同じかチェック
                            If tblXls.Rows(l).Item(1).ToString() = tblStaf.Rows(j).Item(0).ToString() Then
                                ' マイナス値をプラス値に変換して設定
                                lngDeductionReverse = CLng(tblXls.Rows(l).Item(7)) * -1
                                Me.flxList.Rows(k).Item(9) = lngDeductionReverse.ToString()
                                Exit For
                            End If
                        Next l
                    Next j

                    ' フレックスグリッド描画再開
                    Me.flxList.EndUpdate()

                    ' フレックスグリッド再描画
                    Me.flxList.Refresh()

                    ' 件数・控除額計取得処理
                    Call CalcTotal()

                    ' ファイル取込完了メッセージ
                    CLMsg.Show("GI0047")
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
                ' しばらくお待ちください画面非表示
                FrmWaitInfo.CloseWaitForm()

                ' マウスカーソルデフォルト
                Cursor.Current = Cursors.Default

            End Try

        End Sub
#End Region

#Region " AddFlexGridStyle：フレックスグリッドスタイル設定処理（継承先） "
        ''' <summary>フレックスグリッドスタイル設定処理（継承先）</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub AddFlexGridStyle()
            MyBase.AddFlexGridStyle()
        End Sub
#End Region

#Region " AfterEditDeduction：控除額セル編集後処理 "
        ''' <summary>控除額セル編集後処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub AfterEditDeduction(ByRef e As RowColEventArgs)

            Try
                ' 控除額取得
                Dim num As Long = If(((MyBase.flxList.Item(e.Row, e.Col) Is Nothing) OrElse (MyBase.flxList.Item(e.Row, e.Col) Is DBNull.Value)), 0, CLng(MyBase.flxList.Item(e.Row, e.Col)))

                ' マイナス値チェック
                If (num < 0) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0035", New String(0 - 1) {})
                End If
                MyBase.flxList.Select((e.Row + 1), 1)

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                e.Cancel = True

            End Try

        End Sub
#End Region

#Region " CalcDeductionSummary：控除額計取得処理 "
        ''' <summary>控除額計取得処理</summary>
        ''' <returns>控除額計</returns>
        ''' <remarks></remarks>
        Private Function CalcDeductionSummary() As Long

            ' 控除額計
            Dim num As Long = 0

            ' フレックスグリッド件数分ループ
            For i As Integer = 1 To MyBase.flxList.Rows.Count - 1

                If (MyBase.flxList.Item(i, 9) Is Nothing) _
                OrElse (TypeOf MyBase.flxList.Item(i, 9) Is DBNull) Then
                    num += 0                                    ' 控除額が無い場合、0
                Else
                    num += CLng(MyBase.flxList.Item(i, 9))      ' 控除額が有る場合、控除額を足し込む
                End If
            Next i

            ' 戻り値に控除額計を設定
            Return num

        End Function
#End Region

#Region " CalcTotal：件数・控除額計取得処理 "
        ''' <summary>件数・控除額計取得処理</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub CalcTotal()

            ' 件数取得
            Me.lblCount.Text = MyBase.CountValidRows(1).ToString("###,##0")

            ' 控除額計取得
            Me.lblSummary.Text = Me.CalcDeductionSummary.ToString("###,###,##0")

        End Sub
#End Region

#Region " CreateSaveDataTable：登録内容取得処理 "
        ''' <summary>登録内容取得処理</summary>
        ''' <param name="TargetDate">対象年月日</param>
        ''' <returns>登録内容データ</returns>
        ''' <remarks></remarks>
        Private Function CreateSaveDataTable(ByVal TargetDate As String) As DataTable

            Dim map As New PayCutMap
            Dim table As DataTable = map.CreateDataTableLogiName(If((Me._kind = WAGE_REDUCTION_KIND.IN_TIME), "pay_time_cut_monthly", "pay_strike_cut_monthly"))
            Dim values As Object() = New Object(map.ColumnCount - 1) {}

            ' フレックスグリッド件数分ループ
            For i As Integer = 1 To MyBase.flxList.Rows.Count - 1

                ' 社員番号有無チェック
                If (Not MyBase.flxList.Item(i, 10) Is Nothing) Then
                    values(0) = MyBase.flxList.Item(i, 10)              ' 個人認証ＩＤ：社員番号
                    values(1) = TargetDate                              ' 対象年月：対象年月日
                    values(2) = MyBase.flxList.Item(i, 9)               ' 賃金内カット金額：控除額
                    values(4) = MyBase.flxList.Item(i, 9)               ' カット金額合計：控除額
                    table.Rows.Add(values)                              ' データ追加
                End If
            Next i
            Return table

        End Function
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

#Region " FindMember：社員情報取得処理 "
        ''' <summary>社員情報取得処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub FindMember(ByRef e As RowColEventArgs)

            Dim table As DataTable = Nothing
            Dim num As Long = 0

            Try
                ' 社員番号有無チェック
                If MyBase.IsEmptyCell(MyBase.flxList.Item(e.Row, e.Col)) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0022", New String() {MyBase.flxList.Item(0, e.Col).ToString})
                End If

                ' 社員番号存在確認
                If MyBase.IsDupulicate(MyBase.flxList, MyBase.flxList.Item(e.Row, e.Col), (e.Row + 1), 1) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0023", New String(0 - 1) {})
                End If
                num = Long.Parse(MyBase.flxList.Item(e.Row, e.Col).ToString)

                Try
                    Dim employeeNumber As String = FinancialAffairsUtility.LeftZeroTrim(num.ToString)
                    table = MyBase.GetMemberInfo(DirectCast(Me._business, WageReductionBase), employeeNumber, MyBase.cmbYear.Text, MyBase.cmbMonth.Text)
                Catch exception1 As DataNotFoundException
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0025", New String() {num.ToString})
                End Try
                MyBase.flxList.Item(e.Row, 2) = table.Rows.Item(0).Item(0)
                MyBase.flxList.Item(e.Row, 3) = table.Rows.Item(0).Item(1)
                MyBase.flxList.Item(e.Row, 5) = table.Rows.Item(0).Item(2)
                MyBase.flxList.Item(e.Row, 6) = table.Rows.Item(0).Item(3)
                MyBase.flxList.Item(e.Row, 7) = table.Rows.Item(0).Item(4)
                MyBase.flxList.Item(e.Row, 8) = table.Rows.Item(0).Item(5)
                MyBase.flxList.Item(e.Row, 10) = table.Rows.Item(0).Item(6)
                MyBase.flxList.Item(e.Row, 4) = table.Rows.Item(0).Item(7)
                MyBase.flxList.Select(e.Row, 9)

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MyBase.flxList.Item(e.Row, e.Col) = MyBase._beforeEditValue
                e.Cancel = True

            Catch exception2 As Exception
                If TypeOf exception2 Is SysUnionException Then
                    DirectCast(exception2, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception2
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try

        End Sub
#End Region

#Region " flxList_AfterSort：フレックスグリッドソート後処理 "
        ''' <summary>フレックスグリッドソート後処理</summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub flxList_AfterSort(ByVal sender As Object, ByVal e As SortColEventArgs) Handles flxList.AfterSort
            Me.SetCellStyleAgain()
        End Sub
#End Region

#Region " GetOutputFileName：ファイル出力名取得処理 "
        ''' <summary>ファイル出力名取得処理</summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetOutputFileName() As String

            ' ファイル名
            Dim value As String = ""

            ' 種別判定
            If Me._kind.Equals(WAGE_REDUCTION_KIND.IN_TIME) Then
                '-----------------------------------------------------------------------------------
                '   時間内
                '-----------------------------------------------------------------------------------
                value += "月例・時間内 - " & MyBase.cmbYear.Text & "年" & MyBase.cmbMonth.Text & "月分"     ' 「月例・時間内 - 9999年99月分」
            Else
                '-----------------------------------------------------------------------------------
                '   争議行為
                '-----------------------------------------------------------------------------------
                value += "月例・争議行為 - " & MyBase.cmbYear.Text & "年" & MyBase.cmbMonth.Text & "月分"   ' 「月例・争議行為 - 9999年99月分」
            End If

            ' 戻り値にファイル名設定
            Return value

        End Function
#End Region

#Region " GridAfterEdit：データグリッド編集後処理 "
        ''' <summary>データグリッド編集後処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub GridAfterEdit(ByRef e As RowColEventArgs)

            ' 編集カラム判定
            Select Case e.Col
                Case 1
                    '-------------------------------------------------------------------------------
                    '   1：社員番号
                    '-------------------------------------------------------------------------------
                    ' 社員情報取得処理
                    Me.FindMember(e)

                    ' 件数・控除額計取得処理
                    Me.CalcTotal()

                Case 9
                    '-------------------------------------------------------------------------------
                    '   9：控除額
                    '-------------------------------------------------------------------------------
                    ' 控除額セル編集後処理
                    Me.AfterEditDeduction(e)

                    ' 件数・控除額計取得処理
                    Me.CalcTotal()

            End Select

        End Sub
#End Region

#Region " InitializeComponent：コントロール初期化処理 "
        ''' <summary>コントロール初期化処理</summary>
        Private Sub InitializeComponent()

            Me.lblCount = New System.Windows.Forms.Label
            Me.lblSummary = New System.Windows.Forms.Label
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmbMonth：月コンボボックス
            '
            Me.cmbMonth.Location = New System.Drawing.Point(180, 18)
            Me.cmbMonth.Size = New System.Drawing.Size(50, 24)
            '
            'label7：月ラベル
            '
            Me.label7.Location = New System.Drawing.Point(236, 23)
            '
            'cmbYear：年コンボボックス
            '
            Me.cmbYear.Location = New System.Drawing.Point(86, 18)
            Me.cmbYear.Size = New System.Drawing.Size(63, 24)
            '
            'label6：年ラベル
            '
            Me.label6.Location = New System.Drawing.Point(155, 22)
            '
            'flxList：フレックスグリッド
            '
            Me.flxList.ColumnInfo = "11,1,0,0,0,110,Columns:"
            Me.flxList.Location = New System.Drawing.Point(73, 50)
            Me.flxList.Rows.Count = 2
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(880, 609)
            '
            'btnFileInput：ファイル取込ボタン
            '
            Me.btnInputFile.Location = New System.Drawing.Point(116, 704)
            Me.btnInputFile.Text = "ファイル取込"
            '
            'btnOutputFile：ファイル出力ボタン
            '
            Me.btnOutputFile.Location = New System.Drawing.Point(268, 704)
            '
            'btnChange：内容変更ボタン
            '
            Me.btnChange.Location = New System.Drawing.Point(420, 704)
            Me.btnChange.TabIndex = 10
            '
            'btnRegist：登録ボタン
            '
            Me.btnRegist.Location = New System.Drawing.Point(420, 704)
            Me.btnRegist.TabIndex = 7
            '
            'btnPrint：一覧プレ印刷ボタン
            '
            Me.btnPrint.Location = New System.Drawing.Point(572, 704)
            '
            'btnBackOrCancel：戻る・キャンセルボタン
            '   
            Me.btnBackOrCancel.Location = New System.Drawing.Point(724, 704)
            Me.btnBackOrCancel.Text = "キャンセル"
            '
            'lblCount：件数ラベル
            '
            Me.lblCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCount.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCount.Location = New System.Drawing.Point(136, 665)
            Me.lblCount.Name = "lblCount"
            Me.lblCount.Size = New System.Drawing.Size(100, 23)
            Me.lblCount.TabIndex = 5
            Me.lblCount.Text = "999,999"
            Me.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.lblCount.Visible = False
            '
            'lblSummary：控除額計ラベル
            '
            Me.lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSummary.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSummary.Location = New System.Drawing.Point(814, 665)
            Me.lblSummary.Name = "lblSummary"
            Me.lblSummary.Size = New System.Drawing.Size(122, 23)
            Me.lblSummary.TabIndex = 6
            Me.lblSummary.Text = "999,999,999"
            Me.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'CtlWageReductionMonthlyEntry
            '
            Me.Controls.Add(Me.lblSummary)
            Me.Controls.Add(Me.lblCount)
            Me.Name = "CtlWageReductionMonthlyEntry"
            Me.Controls.SetChildIndex(Me.lblCount, 0)
            Me.Controls.SetChildIndex(Me.lblSummary, 0)
            Me.Controls.SetChildIndex(Me.btnInputFile, 0)           ' ファイル取込ボタン
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)          ' ファイル出力ボタン
            Me.Controls.SetChildIndex(Me.btnChange, 0)              ' 内容変更ボタン
            Me.Controls.SetChildIndex(Me.btnRegist, 0)              ' 登録ボタン
            Me.Controls.SetChildIndex(Me.btnPrint, 0)               ' 一覧プレ印刷ボタン
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)        ' 戻る・キャンセルボタン
            Me.Controls.SetChildIndex(Me.flxList, 0)                ' フレックスグリッド
            Me.Controls.SetChildIndex(Me.label7, 0)                 ' 月ラベル
            Me.Controls.SetChildIndex(Me.cmbMonth, 0)               ' 月コンボボックス
            Me.Controls.SetChildIndex(Me.label6, 0)                 ' 年ラベル
            Me.Controls.SetChildIndex(Me.cmbYear, 0)                ' 年コンボボックス
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
#End Region

#Region " PrintList：一覧プレ印刷ボタン押下処理 "
        ''' <summary>一覧プレ印刷ボタン押下処理</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub PrintList()

            Try
                Dim viewer As New ReportViewer(Me._business.GetListPrintData(Me._kind, MyBase.cmbYear.Text, MyBase.cmbMonth.Text), New CR0501P1)
                viewer.ReportViewerShow()

            Catch exception As AppUnionException
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

            End Try

        End Sub
#End Region

#Region " GetStafInfoInTimeStrike：社員情報取得処理（時間内・争議行為） "
        ''' <summary>社員情報取得処理（時間内・争議行為）</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="staf_id_list">社員番号リスト</param>
        ''' <remarks></remarks>
        Protected Overrides Function GetStafInfoInTimeStrike(
            ByVal TargetYear As String,
            ByVal TargetMonth As String,
            ByVal staf_id_list As String
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                tbl = Me._business.GetStafInfoInTimeStrike(Me._kind, (TargetYear & TargetMonth), staf_id_list)
                If (Not MyBase._original Is Nothing) Then
                    MyBase._original.Dispose()
                    MyBase._original = Nothing
                End If
                MyBase._original = tbl.Copy

            Catch exception As Exception
                If TypeOf exception Is SysUnionException Then
                    DirectCast(exception, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " Query：検索処理 "
        ''' <summary>検索処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overrides Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String)
            Try
                Dim data As DataTable = Me._business.GetData(Me._kind, (TargetYear & TargetMonth))
                If (Not MyBase._original Is Nothing) Then
                    MyBase._original.Dispose()
                    MyBase._original = Nothing
                End If
                MyBase._original = data.Copy
            Catch exception As Exception
                If TypeOf exception Is SysUnionException Then
                    DirectCast(exception, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})
            End Try
        End Sub
#End Region

#Region " SaveNewData：新規登録処理 "
        ''' <summary>新規登録処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overrides Sub SaveNewData(
            ByVal TargetYear As String,
            ByVal TargetMonth As String
        )

            Try
                ' 賃金控除明細登録済みチェック
                If Me._business.IsTargetYearsExists(Me._kind, (TargetYear & TargetMonth)) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0026", New String() {TargetYear, TargetMonth})
                End If

                ' 登録確認メッセージ
                If (CLMsg.Show("GQ0006", "対象年月", TargetYear, TargetMonth) <> DialogResult.No) Then
                    Dim saveData As DataTable = Me.CreateSaveDataTable((TargetYear & TargetMonth & "01"))
                    For Each row As DataRow In saveData.Rows
                        If (IsDBNull(row.Item(4)) OrElse row.Item(4) = Nothing) Then
                            MessageBox.Show("控除額計が入力されていない行があります")
                            Return
                        End If
                    Next
                    Me._business.SaveData(Me._kind, saveData, MDLoginInfo.UserId)
                    MyBase.FireCancel(Me, EventArgs.Empty)
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
                Throw exception2

            End Try

        End Sub
#End Region

#Region " SetCellStyleAgain：フレックスグリッドセルスタイル再設定処理 "
        ''' <summary>フレックスグリッドセルスタイル再設定処理</summary>
        ''' <remarks></remarks>
        Private Sub SetCellStyleAgain()

            Dim findValue As Long = 0
            Dim num2 As Long = 0
            Dim newStyle As CellStyle = Nothing
            Dim flag As Boolean = False

            For i As Integer = 1 To (MyBase.flxList.Rows.Count - 1) - 1
                If (Not MyBase.flxList.GetCellStyle(i, 1) Is Nothing) Then
                    flag = True
                    Exit For
                End If
                If (Not MyBase.flxList.GetCellStyle(i, 9) Is Nothing) Then
                    flag = True
                    Exit For
                End If
            Next i

            If flag Then
                For j As Integer = 1 To (MyBase.flxList.Rows.Count - 1) - 1
                    Try
                        newStyle = Nothing
                        If MyBase.IsEmptyCell(MyBase.flxList.Item(j, 1)) Then
                            newStyle = MyBase.flxList.Styles.Item("error_cell")
                        End If
                        findValue = CLng(MyBase.flxList.Item(j, 1))
                        If MyBase.IsEmptyCell(MyBase.flxList.Item(j, 10)) Then
                            newStyle = MyBase.flxList.Styles.Item("error_cell")
                        End If
                        If MyBase.IsDupulicate(MyBase.flxList, findValue, (j + 1), 1) Then
                            newStyle = MyBase.flxList.Styles.Item("error_cell")
                        End If
                        MyBase.flxList.SetCellStyle(j, 1, newStyle)
                        newStyle = Nothing
                        num2 = If(((MyBase.flxList.Item(j, 9) Is Nothing) OrElse (MyBase.flxList.Item(j, 9) Is DBNull.Value)), 0, CLng(MyBase.flxList.Item(j, 9)))
                        If (num2 <= 0) Then
                            newStyle = MyBase.flxList.Styles.Item("error_cell")
                        End If
                    Catch obj1 As Exception
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                    End Try
                    MyBase.flxList.SetCellStyle(j, 9, newStyle)
                Next j
            End If

        End Sub
#End Region

#Region " SetComboItemsForNewEntry：コンボボックス設定処理（新規登録） "
        ''' <summary>コンボボックス設定処理（新規登録）</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub SetComboItemsForNewEntry()

            '---------------------------------------------------------------------------------------
            '   年月コンボボックスリスト作成
            '---------------------------------------------------------------------------------------
            Dim num2 As Integer = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))    ' システム日付（マシン日付）の年取得
            MyBase.cmbYear.Items.Clear()                                                        ' 年コンボボックスリストクリア
            Dim minYear As Integer = Me._business.GetMinYear(Me._kind)                          ' 種別（時間内・争議行為）の最小年取得

            Try
                Dim maxYM As String = Me._business.GetMaxYM(Me._kind)                           ' 種別（時間内・争議行為）の最大年取得

                ' システム日付と最大年を比較
                If (Integer.Parse(maxYM.Substring(0, 4)) > num2) Then
                    ' システム日付の年よりも大きい取得した最大年月の年が大きい場合
                    num2 = Integer.Parse(maxYM.Substring(0, 4))                                 ' 最大年月の年を確保
                Else
                    ' システム日付の年が大きい場合
                    num2 = num2                                                                 ' システム日付の年を確保
                End If

            Catch exception1 As DataNotFoundException
            End Try

            ' 年コンボボックスリスト作成
            Dim i As Integer = minYear
            Do While (i <= num2)
                MyBase.cmbYear.Items.Add(i)                                                     ' 確保した年になるまで年コンボボックスリストに追加
                i += 1
            Loop

            ' 月コンボボックスリストクリア
            MyBase.cmbMonth.Items.Clear()

            ' 月コンボボックスリストに "01" ～ "12" を追加
            MyBase.cmbMonth.Items.AddRange(UnionConst.MONTH_RANGE)

            '---------------------------------------------------------------------------------------
            '   年月コンボボックスリスト選択
            '---------------------------------------------------------------------------------------
            Try
                Dim str2 As String = Me._business.GetMaxYM(Me._kind)                                        ' 最大年月取得
                Dim s As String = str2.Substring(4, 2)                                                      ' 最大年月取得の月取得

                ' 最大年月の月判定
                If (s = "12") Then
                    ' 最大年月の月が12月の場合
                    MyBase.cmbYear.Items.Add((num2 + 1))
                    MyBase.cmbYear.SelectedIndex = (MyBase.cmbYear.FindString(str2.Substring(0, 4)) + 1)    ' 最大年 + 1年を選択
                    MyBase.cmbMonth.SelectedIndex = 0                                                       ' 1月を選択
                Else
                    ' 最大年月の月が12月以外の場合
                    MyBase.cmbYear.SelectedIndex = MyBase.cmbYear.FindString(str2.Substring(0, 4))          ' 最大年選択
                    MyBase.cmbMonth.SelectedIndex = Integer.Parse(s)                                        ' 最大年月の月を選択
                End If

            Catch exception2 As DataNotFoundException
                MyBase.cmbYear.SelectedIndex = (MyBase.cmbYear.Items.Count - 1)
                MyBase.cmbMonth.SelectedIndex = (Integer.Parse(PublicCommand.GetSystemDate.Substring(4, 2)) - 1)

            End Try

        End Sub
#End Region

#Region " UpdateData：内容変更処理 "
        ''' <summary>内容変更処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overrides Sub UpdateData(
            ByVal TargetYear As String,
            ByVal TargetMonth As String
        )

            Try
                Dim saveData As DataTable = Nothing

                ' 未入力チェック（データ行有無チェック）
                If MyBase.HasDataRow Then
                    '-------------------------------------------------------------------------------
                    '   データ有り
                    '-------------------------------------------------------------------------------
                    ' 登録確認メッセージ表示
                    If (CLMsg.Show("GQ0006", "対象年月", TargetYear, TargetMonth) = DialogResult.No) Then
                        Return
                    End If
                    saveData = Me.CreateSaveDataTable((TargetYear & TargetMonth & "01"))
                Else
                    '-------------------------------------------------------------------------------
                    '   データ無し
                    '-------------------------------------------------------------------------------
                    ' 削除確認メッセージ表示
                    If (CLMsg.Show("GQ0009", TargetYear, TargetMonth) = DialogResult.No) Then
                        Return
                    End If
                    saveData = Nothing
                End If

                ' 更新処理
                Me._business.UpdateData(Me._kind, (TargetYear & TargetMonth), saveData, MDLoginInfo.UserId)
                MyBase.FireCancel(Me, EventArgs.Empty)

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            End Try

        End Sub
#End Region

#Region " ValidateGridData：フレックスグリッド検証処理 "
        ''' <summary>フレックスグリッド検証処理</summary>
        ''' <remarks></remarks>
        Private Sub ValidateGridData()

            Dim findValue As Long = 0
            Dim num2 As Long = 0
            Dim newStyle As CellStyle = Nothing
            MyBase.ClearException()

            '---------------------------------------------------------------------------------------
            '   未入力チェック
            '---------------------------------------------------------------------------------------
            ' 新規登録でフレックスグリッドデータが無いかチェック
            If (MyBase.IsNew AndAlso Not MyBase.HasDataRow) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0027", New String(0 - 1) {})
            End If

            ' データ数分ループ
            For i As Integer = 1 To (MyBase.flxList.Rows.Count - 1) - 1
                newStyle = Nothing
                Try
                    '-------------------------------------------------------------------------------
                    '   社員番号未入力チェック
                    '-------------------------------------------------------------------------------
                    ' 社員番号が無いかチェック
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, 1)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0028", New String() {i.ToString})
                        Throw New NotEntryException(MyBase.flxList.Item(0, 1).ToString)
                    End If

                    '-------------------------------------------------------------------------------
                    '   社員番号存在チェック
                    '-------------------------------------------------------------------------------
                    ' ユーザーIDが無いかチェック（社員番号入力時に社員情報を取得してユーザーID設定）
                    ' ユーザーIDが有る場合、社員情報が有りとみなす
                    ' ユーザーIDが無い場合、社員情報が無しとみなす
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, 10)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0030", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, 1).ToString)
                    End If

                    '-------------------------------------------------------------------------------
                    '   社員番号重複チェック
                    '-------------------------------------------------------------------------------
                    ' フレックスグリッド内に同一の社員番号があるかチェック
                    findValue = CLng(MyBase.flxList.Item(i, 1))
                    If MyBase.IsDupulicate(MyBase.flxList, findValue, (i + 1), 1) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0029", New String() {i.ToString})
                        Throw New DuplicateDataException(MyBase.flxList.Item(0, 1).ToString)
                    End If

                Catch exception1 As InvalidInputException
                End Try
                MyBase.flxList.SetCellStyle(i, 1, newStyle)

                '-----------------------------------------------------------------------------------
                '   控除金額マイナスチェック
                '-----------------------------------------------------------------------------------
                newStyle = Nothing
                Try
                    num2 = If(((MyBase.flxList.Item(i, 9) Is Nothing) OrElse (MyBase.flxList.Item(i, 9) Is DBNull.Value)), 0, CLng(MyBase.flxList.Item(i, 9)))
                    If (num2 < 0) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0036", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, 9).ToString)
                    End If
                Catch exception2 As InvalidInputException
                Finally
                    MyBase.flxList.SetCellStyle(i, 9, newStyle)
                End Try
            Next i

            If MyBase.HasException Then
                MyBase.FireInvalidEntryError()
            End If

        End Sub
#End Region

    End Class
End Namespace
