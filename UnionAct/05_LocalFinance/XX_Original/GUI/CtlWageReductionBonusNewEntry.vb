'===========================================================================================================
'   クラスＩＤ　　：CtlWageReductionBonusNewEntry
'   クラス名称　　：賃金カット - 一時金（新規登録）画面クラス
'   備考  　　　　：
'===========================================================================================================

Imports C1.Win.C1FlexGrid
Imports log4net
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs.WageReduction
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.Document
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLAccessMdb

Namespace GUI.FinancialAffairs.WageReduction
    Public Class CtlWageReductionBonusNewEntry
        Inherits CtlWageReductionEntryBase

        Private _business As WageReductionBonusCommand
        Private _logger As ILog
        Private _strBonusName As String
        Private components As IContainer
        Private lblCount As Label
        Private lblSumInTime As Label
        Private lblSumStrike As Label
        Private WithEvents cmbCutOnceName As System.Windows.Forms.ComboBox
        Private WithEvents Label9 As System.Windows.Forms.Label
        Private lblSumTotal As Label

#Region " 列挙 "
        ''' <summary>画面カラム</summary>
        ''' <remarks></remarks>
        Private Enum COLIDX
            SEQ = 0                     ' 01. No
            EMPLOYEE_NUMBER = 1         ' 02. 社員番号
            DIGIT = 2                   ' 03. CD
            NAME = 3                    ' 04. 名前
            STAF_KIND = 4               ' 05. 組合委員種別
            COMPANY_BRANCH = 5          ' 06. 会社所属
            UNION_BRANCH = 6            ' 07. 組合支部
            LICENSE = 7                 ' 08. 資格
            BONUS_NAME = 8              ' 09. 一時金名称（隠し項目）
            INTIME_DEDUCTION = 9        ' 10. 時間内控除額
            STRIKE_DEDUCTION = 10       ' 11. 争議行為控除額
            DEDUCTION_SUM = 11          ' 12. 控除額計
            USER_ID = 12                ' 13. ユーザーID
        End Enum
#End Region

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <param name="strNameForRight">画面ID</param>
        ''' <param name="CancelHandler"></param>
        ''' <remarks></remarks>
        Public Sub New( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal strBonusName As String, _
            ByVal strNameForRight As String, _
            ByVal CancelHandler As EventHandler _
        )

            MyBase.New(TargetYear, TargetMonth, strNameForRight, CancelHandler)
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me._strBonusName = strBonusName
            Me.InitializeComponent()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(50, "readonly_col", False, False, True, False, True), New GridSettingInfo(0, "bonusname_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True), New GridSettingInfo(120, "deduction_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_ref_col", False, False, True, False, True), New GridSettingInfo(115, "readonly_col", False, False, False, False, False)}
            MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(50, "readonly_col", False, False, True, False, True), New GridSettingInfo(0, "bonusname_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True), New GridSettingInfo(120, "deduction_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_ref_col", False, False, True, False, True), New GridSettingInfo(115, "readonly_col", False, False, False, False, False)}
            'MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(55, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_ref_col", False, False, True, False, True)}
            'MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(55, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True), New GridSettingInfo(105, "deduction_ref_col", False, False, True, False, True)}

            ' ADD 2012/06/24
            Dim clsMdb As New CLAccessMdb
            Dim table As DataTable
            clsMdb.Connect()
            table = clsMdb.ExecuteSql("SELECT DISTINCT c_pay_once_name FROM (SELECT c_pay_once_name FROM pay_strike_cut_once UNION SELECT c_pay_once_name FROM pay_time_cut_once)  AS CUT")
            If table.Rows.Count > 0 Then
                If table.Rows(0).Item(0) Is DBNull.Value Then
                ElseIf Trim(table.Rows(0).Item(0)) <> "" Then
                    table.Rows.InsertAt(table.NewRow, 0)
                End If
            End If
            Me.cmbCutOnceName.DataSource = table
            Me.cmbCutOnceName.DisplayMember = "c_pay_once_name"
            clsMdb.Disconnect()
            ' ADD 2012/06/24
            Try
                Me._business = New WageReductionBonusCommand
                Me.AddFlexGridStyle()
                MyBase.SetValidator(New ValidateDelegate(AddressOf Me.ValidateGridData))
            Catch exception As Exception
                If TypeOf exception Is SysUnionException Then
                    DirectCast(exception, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
        End Sub
#End Region

#Region " イベント "
#Region " btnInputFile_Click：ファイル取込ボタン押下処理 "
        ''' <summary>ファイル取込ボタン押下処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub btnInputFile_Click( _
            ByVal sender As Object, _
            ByVal e As EventArgs _
        )

            Dim ofd As OpenFileDialog = Nothing         ' ダイアログボックス
            Dim file As String = ""                     ' ファイル名
            Dim targetYM As String = ""                 ' 対象年月
            Dim tblXls As DataTable = Nothing           ' Excel読込データ
            Dim tblStaf As DataTable = Nothing          ' 社員情報データ
            Dim staf_id_list As String = ""             ' 社員番号リスト

            Dim lngDeductionInTimeReverse As Long = 0   ' 時間内控除額（マイナス値 ⇒ プラス値）
            Dim lngDeductionStrikeReverse As Long = 0   ' 争議行為控除額（マイナス値 ⇒ プラス値）
            Dim lngDeductionTotalReverse As Long = 0    ' 控除額計（マイナス値 ⇒ プラス値）

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
                    '   Excelファイルチェック処理（一時金）
                    '-------------------------------------------------------------------------------
                    ' ファイル名取得
                    file = ofd.FileName

                    ' 対象年月取得
                    targetYM = Me.cmbYear.Text & Me.cmbMonth.Text

                    ' Excelファイルチェック・読込処理（一時金）
                    tblXls = command.CheckReadExcelBounus(file, targetYM)

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

                    ' Excelデータ取込処理（一時金）
                    tblStaf = Me.GetStafInfoBounus(Me.cmbYear.Text, Me.cmbMonth.Text, staf_id_list)

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
                        Me.flxList.Rows(k).Item(8) = ""                                             ' 09. 一時金名称
                        Me.flxList.Rows(k).Item(9) = "0"                                            ' 10. 時間内控除額
                        Me.flxList.Rows(k).Item(10) = "0"                                           ' 11. 争議行為控除額
                        Me.flxList.Rows(k).Item(11) = "0"                                           ' 12. 控除額計
                        Me.flxList.Rows(k).Item(12) = tblStaf.Rows(j).Item(8).ToString()            ' 13. ユーザーID

                        '---------------------------------------------------------------------------
                        '   10. 時間内控除額・11. 争議行為控除額・ 12. 控除額計
                        '---------------------------------------------------------------------------
                        ' 社員番号を突き合わせて、時間内控除額・争議行為控除額・控除額計を設定
                        For l As Integer = 0 To tblXls.Rows.Count - 1 Step 1
                            ' 読み取ったExcelデータの社員番号と社員情報の社員番号が同じかチェック
                            If tblXls.Rows(l).Item(1).ToString() = tblStaf.Rows(j).Item(0).ToString() Then
                                ' マイナス値をプラス値に変換して設定
                                lngDeductionInTimeReverse = CLng(tblXls.Rows(j).Item(9)) * -1
                                lngDeductionStrikeReverse = CLng(tblXls.Rows(j).Item(10)) * -1
                                lngDeductionTotalReverse = CLng(tblXls.Rows(j).Item(11)) * -1
                                Me.flxList.Rows(k).Item(9) = lngDeductionInTimeReverse.ToString()   ' 10. 時間内控除額
                                Me.flxList.Rows(k).Item(10) = lngDeductionStrikeReverse.ToString()  ' 11. 争議行為控除額
                                Me.flxList.Rows(k).Item(11) = lngDeductionTotalReverse.ToString()   ' 12. 控除額計
                                Exit For
                            End If
                        Next l
                    Next j

                    ' フレックスグリッド描画再開
                    Me.flxList.EndUpdate()

                    ' フレックスグリッド再描画
                    Me.flxList.Refresh()

                    ' 件数・時間内控除額合計・争議行為控除額合計・控除額計合計取得処理
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
#End Region

#Region " AddFlexGridStyle：フレックスグリッドスタイル設定処理 "
        ''' <summary>フレックスグリッドスタイル設定処理</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub AddFlexGridStyle()

            MyBase.AddFlexGridStyle()
            Dim style As CellStyle = MyBase.flxList.Styles.Add("deduction_ref_col")
            style.Font = FinancialAffairsUtility.GetGridFontNormal
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.RightCenter
            style.Format = "N0"
            style.BackColor = Color.LightYellow

            flxList.Cols.Item(1).Caption = "社員番号"
            flxList.Cols.Item(2).Caption = "CD"
            flxList.Cols.Item(3).Caption = "名前"
            flxList.Cols.Item(4).Caption = "組合員種別"
            flxList.Cols.Item(5).Caption = "会社所属"
            flxList.Cols.Item(6).Caption = "組合支部"
            flxList.Cols.Item(7).Caption = "資格"
            flxList.Cols.Item(8).Caption = "一時金名称"
            flxList.Cols.Item(9).Caption = "時間内控除額"
            flxList.Cols.Item(10).Caption = "争議行為控除額"
            flxList.Cols.Item(11).Caption = "控除額計"

        End Sub
#End Region

#Region " AfterEditDeduction：控除額セル編集後処理 "
        ''' <summary>控除額セル編集後処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub AfterEditDeduction(ByRef e As RowColEventArgs)

            Try
                Dim nullable As Long? = DirectCast(MyBase.flxList.Item(e.Row, e.Col), Long?)
                If (Not nullable.HasValue OrElse (nullable < 0)) Then
                    e.Cancel = True
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0035", New String(0 - 1) {})
                End If
                Dim num As Long = If(((MyBase.flxList.Item(e.Row, COLIDX.INTIME_DEDUCTION) Is Nothing) OrElse (MyBase.flxList.Item(e.Row, COLIDX.INTIME_DEDUCTION) Is DBNull.Value)), 0, CLng(MyBase.flxList.Item(e.Row, COLIDX.INTIME_DEDUCTION)))
                Dim num2 As Long = If(((MyBase.flxList.Item(e.Row, COLIDX.STRIKE_DEDUCTION) Is Nothing) OrElse (MyBase.flxList.Item(e.Row, COLIDX.STRIKE_DEDUCTION) Is DBNull.Value)), 0, CLng(MyBase.flxList.Item(e.Row, COLIDX.STRIKE_DEDUCTION)))
                MyBase.flxList.Item(e.Row, COLIDX.DEDUCTION_SUM) = (num + num2)
                If (e.Col = COLIDX.INTIME_DEDUCTION) Then
                    MyBase.flxList.Select(e.Row, COLIDX.STRIKE_DEDUCTION)
                Else
                    MyBase.flxList.Select((e.Row + 1), 1)
                End If

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

#Region " CalcDeductionSummary：時間内控除額合計・争議行為控除額合計・控除額計合計取得処理 "
        ''' <summary>時間内控除額合計・争議行為控除額合計・控除額計合計取得処理</summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CalcDeductionSummary() As Long()

            Dim numArray As Long() = New Long(3 - 1) {}
            For i As Integer = 1 To MyBase.flxList.Rows.Count - 1
                numArray(0) = (numArray(0) + If(((MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) Is Nothing) OrElse TypeOf MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) Is DBNull), 0, CLng(MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION))))
                numArray(1) = (numArray(1) + If(((MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION) Is Nothing) OrElse TypeOf MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION) Is DBNull), 0, CLng(MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION))))
                numArray(2) = (numArray(2) + If(((MyBase.flxList.Item(i, 11) Is Nothing) OrElse TypeOf MyBase.flxList.Item(i, 11) Is DBNull), 0, CLng(MyBase.flxList.Item(i, 11))))
            Next i
            Me._logger.Debug(("時間内控除額合計" & "  :[" & numArray(0) & "]"))
            Me._logger.Debug(("争議行為控除額合計" & ":[" & numArray(1) & "]"))
            Me._logger.Debug(("控除額計合計" & "      :[" & numArray(2) & "]"))
            Return numArray

        End Function
#End Region

#Region " CalcTotal：件数・時間内控除額合計・争議行為控除額合計・控除額計合計取得処理 "
        ''' <summary>件数・時間内控除額合計・争議行為控除額合計・控除額計合計取得処理</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub CalcTotal()

            Me.lblCount.Text = MyBase.CountValidRows(1).ToString("###,###,##0")
            Dim numArray As Long() = Me.CalcDeductionSummary
            Me.lblSumInTime.Text = numArray(0).ToString("###,###,##0")
            Me.lblSumStrike.Text = numArray(1).ToString("###,###,##0")
            Me.lblSumTotal.Text = numArray(2).ToString("###,###,##0")

        End Sub
#End Region

#Region " EnableComboBox： "
        ''' <summary></summary>
        ''' <param name="fEnabeed"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub EnableComboBox(ByVal fEnabeed As Boolean)

            If fEnabeed Then
                'If Not flxList.Styles.Contains("OnceName") Then
                Dim clsMdb As New CLAccessMdb
                Dim table As DataTable
                clsMdb.Connect()
                'table = clsMdb.ExecuteSql("SELECT DISTINCT c_pay_once_name AS val, c_pay_once_name AS txt FROM (SELECT c_pay_once_name FROM pay_strike_cut_once UNION SELECT c_pay_once_name FROM pay_time_cut_once)  AS CUT")
                table = clsMdb.ExecuteSql("SELECT DISTINCT c_pay_once_name AS val FROM (SELECT c_pay_once_name FROM pay_strike_cut_once UNION SELECT c_pay_once_name FROM pay_time_cut_once)  AS CUT")
                clsMdb.Disconnect()
                Dim oneCell As CellStyle = flxList.Styles.Add("OnceName")
                oneCell.TextAlign = TextAlignEnum.CenterCenter
                '空白行を追加
                'Dim newRow As DataRow = table.NewRow
                'newRow(0) = " "
                'newRow(1) = " "
                'table.Rows.InsertAt(newRow, 0)

                'Dim columnNames As String() = New String() {"txt"}
                'Dim dictionary As New MultiColumnDictionary(table, "val", columnNames, 0)
                'oneCell.DataMap = dictionary
                'oneCell.Editor = New ComboBox
                'flxList.Cols.Item(8).Style = oneCell

                Dim combo As New ComboBox
                For Each row As DataRow In table.Rows
                    combo.Items.Add(row(0))
                Next
                oneCell.DataType = Type.GetType("System.String")
                oneCell.Editor = combo
                flxList.Cols.Item(COLIDX.BONUS_NAME).Style = oneCell
            End If

        End Sub
#End Region

#Region " CreateSaveDataTable：登録内容取得処理 "
        ''' <summary>登録内容取得処理</summary>
        ''' <param name="TargetDate">対象年月日</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CreateSaveDataTable(ByVal TargetDate As String) As DataSet

            Dim map As New PayCutMap
            Dim table As DataTable = map.CreateDataTableLogiName("pay_time_cut_once")
            Dim table2 As DataTable = map.CreateDataTableLogiName("pay_strike_cut_once")
            Dim values As Object() = New Object(map.ColumnCount - 1) {}
            For i As Integer = 1 To MyBase.flxList.Rows.Count - 1
                If (Not MyBase.flxList.Item(i, COLIDX.USER_ID) Is Nothing) Then
                    values(0) = MyBase.flxList.Item(i, COLIDX.USER_ID)
                    values(1) = TargetDate
                    values(2) = MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION)
                    'values(3) = MyBase.flxList.Item(i, COLIDX.BONUS_NAME) ' MOD 2012/06/24
                    values(3) = Me.cmbCutOnceName.Text                     ' MOD 2012/06/24
                    values(4) = MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) + MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION)
                    table.Rows.Add(values)
                    values(2) = MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION)
                    table2.Rows.Add(values)
                End If
            Next i
            Dim ds As New DataSet
            ds.Tables.Add(table)
            ds.Tables.Add(table2)
            Return ds

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
        Private Sub FindMember(ByRef e As RowColEventArgs)

            Dim tbl As DataTable = Nothing

            Try
                If MyBase.IsEmptyCell(MyBase.flxList.Item(e.Row, e.Col)) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0022", New String() {MyBase.flxList.Item(0, e.Col).ToString})
                End If
                ' mod 2012/06/15 If MyBase.IsDupulicate(MyBase.flxList, MyBase.flxList.Item(e.Row, e.Col), (e.Row + 1), 1) Then
                ' mod 2012/06/15 Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0023", New String(0 - 1) {})
                ' mod 2012/06/15 End If
                Try
                    tbl = MyBase.GetMemberInfo(DirectCast(Me._business, WageReductionBase), MyBase.flxList.Item(e.Row, e.Col).ToString, MyBase.cmbYear.Text, MyBase.cmbMonth.Text)
                Catch exception1 As DataNotFoundException
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0025", New String() {MyBase.flxList.Item(e.Row, e.Col).ToString})
                End Try
                MyBase.flxList.Item(e.Row, COLIDX.DIGIT) = tbl.Rows.Item(0).Item(0)
                MyBase.flxList.Item(e.Row, COLIDX.NAME) = tbl.Rows.Item(0).Item(1)
                MyBase.flxList.Item(e.Row, COLIDX.COMPANY_BRANCH) = tbl.Rows.Item(0).Item(2)
                MyBase.flxList.Item(e.Row, COLIDX.UNION_BRANCH) = tbl.Rows.Item(0).Item(3)
                MyBase.flxList.Item(e.Row, COLIDX.LICENSE) = tbl.Rows.Item(0).Item(4)
                MyBase.flxList.Item(e.Row, COLIDX.USER_ID) = tbl.Rows.Item(0).Item(6)
                MyBase.flxList.Item(e.Row, COLIDX.STAF_KIND) = tbl.Rows.Item(0).Item(7)
                MyBase.flxList.Select(e.Row, COLIDX.BONUS_NAME)
                ' ADD 2012/06/15
                If e.Row > 1 Then
                    MyBase.flxList.Item(e.Row, COLIDX.BONUS_NAME) = MyBase.flxList.Item(e.Row - 1, COLIDX.BONUS_NAME)
                End If

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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})

            End Try

        End Sub
#End Region

#Region " GetOutputFileName：ファイル出力名取得処理 "
        ''' <summary>ファイル出力名取得処理</summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetOutputFileName() As String
            Return String.Concat(New String() {"一時金" & " - ", MyBase.cmbYear.Text, "年", MyBase.cmbMonth.Text, "月分"})
        End Function
#End Region

#Region " GridAfterEdit：データグリッド編集後処理 "
        ''' <summary>データグリッド編集後処理</summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub GridAfterEdit(ByRef e As RowColEventArgs)

            Select Case e.Col
                Case COLIDX.INTIME_DEDUCTION, COLIDX.STRIKE_DEDUCTION
                    Me.AfterEditDeduction(e)
                    Exit Select
                Case 1
                    Me.FindMember(e)
                    Exit Select
            End Select

            Me.CalcTotal()

        End Sub
#End Region

#Region " InitializeComponent：コントロール初期化処理 "
        ''' <summary>コントロール初期化処理</summary>
        ''' <remarks></remarks>
        Private Sub InitializeComponent()
            Me.lblSumTotal = New System.Windows.Forms.Label
            Me.lblCount = New System.Windows.Forms.Label
            Me.lblSumStrike = New System.Windows.Forms.Label
            Me.lblSumInTime = New System.Windows.Forms.Label
            Me.cmbCutOnceName = New System.Windows.Forms.ComboBox
            Me.Label9 = New System.Windows.Forms.Label
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmbMonth：月コンボボックス
            '
            Me.cmbMonth.Size = New System.Drawing.Size(50, 24)
            '
            'cmbYear：年コンボボックス
            '
            Me.cmbYear.Size = New System.Drawing.Size(63, 24)
            '
            'flxList：フレックスグリッド
            '
            Me.flxList.ColumnInfo = "13,1,0,0,0,110,Columns:"
            Me.flxList.Location = New System.Drawing.Point(14, 50)
            Me.flxList.Rows.Count = 2
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(993, 609)
            '
            'btnInputFile：ファイル取込ボタン
            '
            Me.btnInputFile.Location = New System.Drawing.Point(116, 704)
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
            Me.btnRegist.TabIndex = 9
            '
            'btnPrint：一覧プレ印刷ボタン
            '
            Me.btnPrint.Location = New System.Drawing.Point(572, 704)
            Me.btnPrint.TabIndex = 11
            '
            'btnBackOrCancel：戻る・キャンセルボタン
            '
            Me.btnBackOrCancel.Location = New System.Drawing.Point(724, 704)
            Me.btnBackOrCancel.TabIndex = 12
            Me.btnBackOrCancel.Text = "キャンセル"
            '
            'lblSumTotal：控除額計ラベル
            '
            Me.lblSumTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumTotal.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumTotal.Location = New System.Drawing.Point(880, 662)
            Me.lblSumTotal.Name = "lblSumTotal"
            Me.lblSumTotal.Size = New System.Drawing.Size(109, 23)
            Me.lblSumTotal.TabIndex = 8
            Me.lblSumTotal.Text = "999,999,999"
            Me.lblSumTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCount：件数ラベル
            '
            Me.lblCount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCount.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCount.Location = New System.Drawing.Point(65, 662)
            Me.lblCount.Name = "lblCount"
            Me.lblCount.Size = New System.Drawing.Size(100, 23)
            Me.lblCount.TabIndex = 5
            Me.lblCount.Text = "999,999"
            Me.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.lblCount.Visible = False
            '
            'lblSumStrike：争議行為控除額計ラベル
            '
            Me.lblSumStrike.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumStrike.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumStrike.Location = New System.Drawing.Point(760, 662)
            Me.lblSumStrike.Name = "lblSumStrike"
            Me.lblSumStrike.Size = New System.Drawing.Size(118, 23)
            Me.lblSumStrike.TabIndex = 7
            Me.lblSumStrike.Text = "999,999,999"
            Me.lblSumStrike.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSumInTime：時間内控除額計ラベル
            '
            Me.lblSumInTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumInTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumInTime.Location = New System.Drawing.Point(637, 662)
            Me.lblSumInTime.Name = "lblSumInTime"
            Me.lblSumInTime.Size = New System.Drawing.Size(121, 23)
            Me.lblSumInTime.TabIndex = 6
            Me.lblSumInTime.Text = "999,999,999"
            Me.lblSumInTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cmbCutOnceName：一時金名称コンボボックス
            '
            Me.cmbCutOnceName.FormattingEnabled = True
            Me.cmbCutOnceName.Location = New System.Drawing.Point(357, 15)
            Me.cmbCutOnceName.MaxLength = 50
            Me.cmbCutOnceName.Name = "cmbCutOnceName"
            Me.cmbCutOnceName.Size = New System.Drawing.Size(153, 24)
            Me.cmbCutOnceName.TabIndex = 20
            '
            'Label9：一時金名称ラベル
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(268, 19)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(88, 16)
            Me.Label9.TabIndex = 21
            Me.Label9.Text = "一時金名称"
            '
            'CtlWageReductionBonusNewEntry
            '
            Me.Controls.Add(Me.cmbCutOnceName)                      ' 一時金名称コンボボックス
            Me.Controls.Add(Me.Label9)                              ' 一時金名称ラベル
            Me.Controls.Add(Me.lblSumInTime)                        ' 時間内控除額計ラベル
            Me.Controls.Add(Me.lblSumStrike)                        ' 争議行為控除額計ラベル
            Me.Controls.Add(Me.lblSumTotal)                         ' 控除額計ラベル
            Me.Controls.Add(Me.lblCount)                            ' 件数ラベル
            Me.Name = "CtlWageReductionBonusNewEntry"
            Me.Controls.SetChildIndex(Me.cmbMonth, 0)               ' 月コンボボックス
            Me.Controls.SetChildIndex(Me.label7, 0)                 ' 月ラベル
            Me.Controls.SetChildIndex(Me.cmbYear, 0)                ' 年コンボボックス
            Me.Controls.SetChildIndex(Me.label6, 0)                 ' 年ラベル
            Me.Controls.SetChildIndex(Me.Label9, 0)                 ' 一時金名称ラベル
            Me.Controls.SetChildIndex(Me.cmbCutOnceName, 0)         ' 一時金名称コンボボックス
            Me.Controls.SetChildIndex(Me.flxList, 0)                ' フレックスグリッド
            Me.Controls.SetChildIndex(Me.lblSumStrike, 0)           ' 争議行為控除額計ラベル
            Me.Controls.SetChildIndex(Me.lblSumInTime, 0)           ' 時間内控除額計ラベル
            Me.Controls.SetChildIndex(Me.lblSumTotal, 0)            ' 控除額計ラベル
            Me.Controls.SetChildIndex(Me.lblCount, 0)               ' 件数ラベル
            Me.Controls.SetChildIndex(Me.btnInputFile, 0)           ' ファイル取込ボタン
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)          ' ファイル出力ボタン
            Me.Controls.SetChildIndex(Me.btnRegist, 0)              ' 登録ボタン
            Me.Controls.SetChildIndex(Me.btnChange, 0)              ' 内容変更ボタン
            Me.Controls.SetChildIndex(Me.btnPrint, 0)               ' 一覧プレ印刷ボタン
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)        ' 戻る・キャンセルボタン
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
                Dim viewer As New ReportViewer(
                    Me._business.GetListPrintData(
                        MyBase.cmbYear.Text,
                        MyBase.cmbMonth.Text,
                        Me._strBonusName
                    ),
                New CR0501P3)

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

#Region " GetStafInfoBounus：社員情報取込処理（一時金） "
        ''' <summary>社員情報取込処理（一時金）</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="staf_id_list">社員番号リスト</param>
        ''' <remarks></remarks>
        Protected Overrides Function GetStafInfoBounus(
            ByVal TargetYear As String,
            ByVal TargetMonth As String,
            ByVal staf_id_list As String
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                tbl = Me._business.GetStafInfoBounus((TargetYear & TargetMonth), staf_id_list)
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
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overrides Sub Query(
            ByVal TargetYear As String,
            ByVal TargetMonth As String
        )

            Try
                Dim tbl As DataTable = Me._business.GetData((TargetYear & TargetMonth), Me._strBonusName)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})

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
                If Me._business.IsTargetYearsExists(
                    (TargetYear & TargetMonth),
                    Me.cmbCutOnceName.Text
                ) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0026", New String() {TargetYear, TargetMonth})
                End If
                If (CLMsg.Show("GQ0006", "対象年月", TargetYear, TargetMonth) <> DialogResult.No) Then
                    Dim saveData As DataSet = Me.CreateSaveDataTable((TargetYear & TargetMonth & "01"))
                    Me._business.SaveData(saveData, MDLoginInfo.UserId)
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

#Region " SetComboItemsForNewEntry：コンボボックス設定処理（新規登録） "
        ''' <summary>コンボボックス設定処理（新規登録）</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub SetComboItemsForNewEntry()

            Dim num2 As Integer = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
            MyBase.cmbYear.Items.Clear()
            Dim i As Integer = Me._business.GetMinYear
            Do While (i <= num2)
                MyBase.cmbYear.Items.Add(i)
                i += 1
            Loop
            MyBase.cmbMonth.Items.Clear()
            MyBase.cmbMonth.Items.AddRange(UnionConst.MONTH_RANGE)

            Try
                Dim maxYM As String = Me._business.GetMaxYM
                maxYM.Substring(0, 4)
                Dim s As String = maxYM.Substring(4, 2)
                If (s = "12") Then
                    MyBase.cmbYear.Items.Add((num2 + 1))
                    MyBase.cmbYear.SelectedIndex = (MyBase.cmbYear.FindString(maxYM.Substring(0, 4)) + 1)
                    MyBase.cmbMonth.SelectedIndex = 0
                Else
                    MyBase.cmbYear.SelectedIndex = MyBase.cmbYear.FindString(maxYM.Substring(0, 4))
                    MyBase.cmbMonth.SelectedIndex = Integer.Parse(s)
                End If

            Catch exception1 As DataNotFoundException
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
                Dim saveData As DataSet = Nothing
                If MyBase.HasDataRow Then
                    If (CLMsg.Show("GQ0006", "対象年月", TargetYear, TargetMonth) = DialogResult.No) Then
                        Return
                    End If
                    saveData = Me.CreateSaveDataTable((TargetYear & TargetMonth & "01"))
                Else
                    If (CLMsg.Show("GQ0009", TargetYear, TargetMonth) = DialogResult.No) Then
                        Return
                    End If
                    saveData = Nothing
                End If
                Me._business.UpdateData((TargetYear & TargetMonth), saveData, MDLoginInfo.UserId)
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

            Dim num As Long = 0
            Dim num2 As Long = 0
            Dim findNo As Integer = 0
            Dim rt As Boolean
            Dim newStyle As CellStyle = Nothing

            '---------------------------------------------------------------------------------------
            '   一時金名称未選択チェック
            '---------------------------------------------------------------------------------------
            If Me.cmbCutOnceName.Text = "" Then
                MessageBox.Show("一時金名称を入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.cmbCutOnceName.Focus()
                Throw New Exception("")
            End If

            MyBase.ClearException()
            If (MyBase.IsNew AndAlso Not MyBase.HasDataRow) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0027", New String(0 - 1) {})
            End If
            Dim i As Integer
            For i = 1 To (MyBase.flxList.Rows.Count - 1) - 1
                newStyle = Nothing
                Try
                    '-------------------------------------------------------------------------------
                    '   社員番号未入力チェック
                    '-------------------------------------------------------------------------------
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, COLIDX.EMPLOYEE_NUMBER)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0028", New String() {i.ToString})
                        Throw New NotEntryException(MyBase.flxList.Item(0, COLIDX.EMPLOYEE_NUMBER).ToString)
                    End If

                    '-------------------------------------------------------------------------------
                    '   社員情報存在チェック
                    '-------------------------------------------------------------------------------
                    ' ※ 社員番号入力時に社員情報を取得してユーザーIDを設定している
                    ' ユーザーIDカラムにデータが有る場合、存在とみなす
                    ' ユーザーIDカラムにデータが無い場合、未存在とみなす
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, COLIDX.USER_ID)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0030", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, 1).ToString)
                    End If

                    '-------------------------------------------------------------------------------
                    '   社員番号重複チェック
                    '-------------------------------------------------------------------------------
                    rt = MyBase.IsDupulicate2(MyBase.flxList, MyBase.flxList.Item(i, COLIDX.EMPLOYEE_NUMBER), (i + 1), COLIDX.EMPLOYEE_NUMBER, findNo)
                    If rt And MyBase.flxList.Item(i, COLIDX.BONUS_NAME) = MyBase.flxList.Item(findNo, COLIDX.BONUS_NAME) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0029", New String() {i.ToString})
                        Throw New DuplicateDataException(MyBase.flxList.Item(0, COLIDX.EMPLOYEE_NUMBER).ToString)
                    End If
                Catch exception1 As InvalidInputException
                Finally
                    MyBase.flxList.SetCellStyle(i, COLIDX.EMPLOYEE_NUMBER, newStyle)
                    MyBase.flxList.SetCellStyle(i, COLIDX.BONUS_NAME, newStyle) ' mod 2012/06/15
                End Try
                newStyle = Nothing
                num = -1
                Try
                    '-------------------------------------------------------------------------------
                    '   時間内控除額マイナスチェック
                    '-------------------------------------------------------------------------------
                    If ((MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) Is Nothing) OrElse (MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) Is DBNull.Value)) Then
                        MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION) = 0
                    End If
                    num = CLng(MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION))
                    If (num < 0) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0036", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, COLIDX.INTIME_DEDUCTION).ToString)
                    End If
                Catch exception2 As InvalidInputException
                Finally
                    MyBase.flxList.SetCellStyle(i, COLIDX.INTIME_DEDUCTION, newStyle)
                End Try
                newStyle = Nothing
                num2 = -1
                Try
                    '-------------------------------------------------------------------------------
                    '   争議行為控除額マイナスチェック
                    '-------------------------------------------------------------------------------
                    If ((MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION) Is Nothing) OrElse (MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION) Is DBNull.Value)) Then
                        MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION) = 0
                    End If
                    num2 = CLng(MyBase.flxList.Item(i, COLIDX.STRIKE_DEDUCTION))
                    If (num2 < 0) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0036", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, COLIDX.STRIKE_DEDUCTION).ToString)
                    End If
                Catch exception3 As InvalidInputException
                Finally
                    MyBase.flxList.SetCellStyle(i, COLIDX.STRIKE_DEDUCTION, newStyle)
                End Try
                newStyle = Nothing
                Try
                    '-------------------------------------------------------------------------------
                    '   時間内控除額・争議行為控除額ともに「0」かチェック
                    '-------------------------------------------------------------------------------
                    If ((num = 0) AndAlso (num2 = 0)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0037", New String() {i.ToString})
                        Throw New InvalidAttributeException((MyBase.flxList.Item(0, COLIDX.INTIME_DEDUCTION).ToString & "・" & MyBase.flxList.Item(0, COLIDX.STRIKE_DEDUCTION).ToString))
                    End If
                Catch exception4 As InvalidInputException
                Finally
                    MyBase.flxList.SetCellStyle(i, COLIDX.INTIME_DEDUCTION, newStyle)
                    MyBase.flxList.SetCellStyle(i, COLIDX.STRIKE_DEDUCTION, newStyle)
                End Try
            Next i
            If MyBase.HasException Then
                MyBase.FireInvalidEntryError()
            End If
        End Sub
#End Region

    End Class
End Namespace
