'===========================================================================================================
'   クラスＩＤ　　：CtlWageReductionBonusEntry
'   クラス名称　　：賃金カット - 一時金（照会）画面
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
Imports UnionAct.GUI.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.Command
Imports UnionAct.Framework
Imports UnionAct.Framework.UnionConst
Imports UnionAct.Business.FinancialAffairs.WageReduction
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLAccessMdb

Namespace GUI.FinancialAffairs.WageReduction
    Public Class CtlWageReductionBonusEntry
        Inherits CtlWageReductionEntryBase

        ' Fields
        Private _business As WageReductionBonusCommand
        Private _logger As ILog
        Private _strBonusName As String
        Private components As IContainer
        Private lblCount As Label
        Private lblSumInTime As Label
        Private lblSumStrike As Label
        Private lblSumTotal As Label

#Region " 列挙 "
        ''' <summary>一時金（照会）画面カラム</summary>
        ''' <remarks></remarks>
        Private Enum COLIDX
            SEQ = 0                         ' 01. No
            EMPLOYEE_NUMBER = 1             ' 02. 社員番号
            DIGIT = 2                       ' 03. CD
            NAME = 3                        ' 04. 名前
            STAF_KIND = 4                   ' 05. 組合員種別
            COMPANY_BRANCH = 5              ' 06. 会社所属
            UNION_BRANCH = 6                ' 07. 組合支部
            LICENSE = 7                     ' 08. 資格
            BONUS_NAME = 8                  ' 09. 一時金名称
            INTIME_DEDUCTION = 9            ' 10. 時間内控除額
            STRIKE_DEDUCTION = 10           ' 11. 争議行為控除額
            DEDUCTION_SUM = 11              ' 12. 控除額計
            USER_ID = 12                    ' 13. ユーザーID
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

            ' 賃金カット - 月例・時間内・争議行為ベース画面
            MyBase.New(TargetYear, TargetMonth, strNameForRight, CancelHandler)
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me._strBonusName = strBonusName
            Me.InitializeComponent()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(50, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "bonusname_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True), New GridSettingInfo(120, "deduction_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_ref_col", False, False, True, False, True), New GridSettingInfo(115, "readonly_col", False, False, False, False, False)}
            MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(50, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "bonusname_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True), New GridSettingInfo(120, "deduction_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_ref_col", False, False, True, False, True), New GridSettingInfo(115, "readonly_col", False, False, False, False, False)}
            'MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(55, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_col", False, False, True, False, True), New GridSettingInfo(110, "deduction_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_ref_col", False, False, True, False, True)}
            'MyBase._settingInEdit = New GridSettingInfo() {New GridSettingInfo(30, "fixed_col", False, False, False, False, True), New GridSettingInfo(75, "employee_number_col_nolink", False, False, True, True, True), New GridSettingInfo(30, "readonly_col", False, False, False, False, True), New GridSettingInfo(110, "name_col", False, False, True, False, True), New GridSettingInfo(100, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(75, "readonly_col", False, False, True, False, True), New GridSettingInfo(55, "readonly_col", False, False, True, False, True), New GridSettingInfo(105, "deduction_col", False, False, True, True, True), New GridSettingInfo(110, "deduction_col", False, False, True, True, True), New GridSettingInfo(105, "deduction_ref_col", False, False, True, False, True)}
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

            Me.lblCount.Text = MyBase.CountValidRows(1).ToString("###,###,##0")     ' 件数取得処理
            Dim numArray As Long() = Me.CalcDeductionSummary                        ' 時間内控除額合計・争議行為控除額合計・控除額計合計取得処理
            Me.lblSumInTime.Text = numArray(0).ToString("###,###,##0")              ' 時間内控除額合計
            Me.lblSumStrike.Text = numArray(1).ToString("###,###,##0")              ' 争議行為控除額合計
            Me.lblSumTotal.Text = numArray(2).ToString("###,###,##0")               ' 控除額計合計

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
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                If (Not MyBase.flxList.Item(i, COLIDX.USER_ID) Is Nothing) Then
                    values(0) = MyBase.flxList.Item(i, COLIDX.USER_ID)
                    values(1) = TargetDate
                    values(2) = MyBase.flxList.Item(i, COLIDX.INTIME_DEDUCTION)
                    values(3) = MyBase.flxList.Item(i, COLIDX.BONUS_NAME)
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
                    ' 時間内控除額・争議行為控除額
                    Me.AfterEditDeduction(e)
                    Exit Select
                Case 1
                    ' 
                    Me.FindMember(e)
                    Exit Select
            End Select

            Me.CalcTotal()

        End Sub
#End Region

#Region " コントロール初期化処理 "
        ''' <summary>コントロール初期化処理</summary>
        ''' <remarks></remarks>
        Private Sub InitializeComponent()
            Me.lblSumTotal = New System.Windows.Forms.Label()
            Me.lblCount = New System.Windows.Forms.Label()
            Me.lblSumStrike = New System.Windows.Forms.Label()
            Me.lblSumInTime = New System.Windows.Forms.Label()
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmbYear
            '
            Me.cmbYear.Size = New System.Drawing.Size(63, 24)
            '
            'cmbMonth
            '
            Me.cmbMonth.Size = New System.Drawing.Size(50, 24)
            '
            'flxList
            '
            Me.flxList.ColumnInfo = "13,1,0,0,0,110,Columns:"
            Me.flxList.Location = New System.Drawing.Point(14, 50)
            Me.flxList.Rows.Count = 2
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(993, 609)
            '
            'btnChange
            '
            Me.btnChange.TabIndex = 10
            '
            'btnRegist
            '
            Me.btnRegist.TabIndex = 9
            '
            'btnPrint
            '
            Me.btnPrint.TabIndex = 11
            '
            'btnBackOrCancel
            '
            Me.btnBackOrCancel.TabIndex = 12
            Me.btnBackOrCancel.Text = "キャンセル"
            '
            'lblSumTotal
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
            'lblCount
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
            'lblSumStrike
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
            'lblSumInTime
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
            'CtlWageReductionBonusEntry
            '
            Me.Controls.Add(Me.lblCount)
            Me.Controls.Add(Me.lblSumTotal)
            Me.Controls.Add(Me.lblSumInTime)
            Me.Controls.Add(Me.lblSumStrike)
            Me.Name = "CtlWageReductionBonusEntry"
            Me.Controls.SetChildIndex(Me.cmbMonth, 0)
            Me.Controls.SetChildIndex(Me.label7, 0)
            Me.Controls.SetChildIndex(Me.cmbYear, 0)
            Me.Controls.SetChildIndex(Me.label6, 0)
            Me.Controls.SetChildIndex(Me.flxList, 0)
            Me.Controls.SetChildIndex(Me.lblSumStrike, 0)
            Me.Controls.SetChildIndex(Me.lblSumInTime, 0)
            Me.Controls.SetChildIndex(Me.lblSumTotal, 0)
            Me.Controls.SetChildIndex(Me.lblCount, 0)
            Me.Controls.SetChildIndex(Me.btnInputFile, 0)
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)
            Me.Controls.SetChildIndex(Me.btnRegist, 0)
            Me.Controls.SetChildIndex(Me.btnChange, 0)
            Me.Controls.SetChildIndex(Me.btnPrint, 0)
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)
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
                    ), New CR0501P3
                )
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

#Region " Query：検索処理 "
        ''' <summary>検索処理</summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <remarks></remarks>
        Protected Overrides Sub Query(
            ByVal TargetYear As String,
            ByVal TargetMonth As String
        )

            Try
                Dim data As DataTable = Me._business.GetData(
                    (TargetYear & TargetMonth),
                    Me._strBonusName
                )

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
                If Me._business.IsTargetYearsExists((TargetYear & TargetMonth)) Then
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
            Dim findNo As Integer = 0 ' mod 2012/06/15
            Dim rt As Boolean         ' mod 2012/06/15
            Dim newStyle As CellStyle = Nothing
            MyBase.ClearException()
            If (MyBase.IsNew AndAlso Not MyBase.HasDataRow) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0027", New String(0 - 1) {})
            End If
            Dim i As Integer
            For i = 1 To (MyBase.flxList.Rows.Count - 1) - 1
                newStyle = Nothing
                Try
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, COLIDX.EMPLOYEE_NUMBER)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0028", New String() {i.ToString})
                        Throw New NotEntryException(MyBase.flxList.Item(0, COLIDX.EMPLOYEE_NUMBER).ToString)
                    End If
                    If MyBase.IsEmptyCell(MyBase.flxList.Item(i, COLIDX.USER_ID)) Then
                        newStyle = MyBase.flxList.Styles.Item("error_cell")
                        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0030", New String() {i.ToString})
                        Throw New InvalidAttributeException(MyBase.flxList.Item(0, 1).ToString)
                    End If
                    rt = MyBase.IsDupulicate2(MyBase.flxList, MyBase.flxList.Item(i, COLIDX.EMPLOYEE_NUMBER), (i + 1), COLIDX.EMPLOYEE_NUMBER, findNo) ' mod 2012/06/15 
                    If rt And MyBase.flxList.Item(i, COLIDX.BONUS_NAME) = MyBase.flxList.Item(findNo, COLIDX.BONUS_NAME) Then                          ' mod 2012/06/15                                                                                          ' mod 2012/06/15
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
                ' mod 2012/12/12 0,0 Record Delete
                'Try
                '    If ((num = 0) AndAlso (num2 = 0)) Then
                '        newStyle = MyBase.flxList.Styles.Item("error_cell")
                '        MyBase.AddValidateError(MethodBase.GetCurrentMethod, "GE0037", New String() {i.ToString})
                '        Throw New InvalidAttributeException((MyBase.flxList.Item(0, COLIDX.INTIME_DEDUCTION).ToString & "・" & MyBase.flxList.Item(0, COLIDX.STRIKE_DEDUCTION).ToString))
                '    End If
                'Catch exception4 As InvalidInputException
                'Finally
                MyBase.flxList.SetCellStyle(i, COLIDX.INTIME_DEDUCTION, newStyle)
                MyBase.flxList.SetCellStyle(i, COLIDX.STRIKE_DEDUCTION, newStyle)
                'End Try
            Next i
            If MyBase.HasException Then
                MyBase.FireInvalidEntryError()
            End If
        End Sub

        Private Sub btnRegist_Click(sender As Object, e As EventArgs) Handles btnRegist.Click

        End Sub
#End Region

    End Class
End Namespace
