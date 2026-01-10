Imports C1.Win.C1FlexGrid
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.MDMode
Imports log4net
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlMonthlyTaxableDetail
        Inherits CtlCutDivTaxableDetail
        ' Methods
        Public Sub New()
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal Year As String, ByVal Month As String, ByVal strNameForRight As String, ByVal CancelHandler As EventHandler)
            MyBase.New(Year, Month, strNameForRight, CancelHandler)
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me.InitializeComponent()
            Me.btnChange.Enabled = MyBase.HasEntryPower
            Me.btnNonTaxation.Enabled = MyBase.HasEntryPower
            MyBase.SetValidator(New ValidateDelegate(AddressOf Me.ValidateRows))
        End Sub

        ''' <summary>
        ''' 非課税チェックボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnNonTaxation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNonTaxation.Click

            Me.CalcTotal(False)

            ' モード変更
            MyBase.ScreenMode = MODE.EDIT_TAXATION

            ' チェックサポート処理（8月に中執役員となったメンバーにチェックを付ける）
            Call NewUserCheckMain()

            ' 情報メッセージ表示
            Call CLMsg.Show("GI0046")

        End Sub

        ''' <summary>
        ''' 非課税登録ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnNonTaxationRegist_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNonTaxationRegist.Click

            Dim num As Integer = 0              ' チェック件数

            Try
                ' 課税対象者リスト件数分ループ
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    ' チェックボックスにチェックが付いているかチェック
                    If MyBase.flxList.Item(i, COLIDX.CHECK) Then
                        num += 1
                    End If
                Next i

                ' 1つ以上チェックボックスにチェックが付いているかチェック
                If (num = 0) Then
                    Call CLMsg.Show("GI0010")
                Else
                    'Me.ValidateFields()
                    'Me.ValidateRows()
                    ' 確認メッセージ表示
                    If (CLMsg.Show("GQ0104", "対象年月", MyBase.TargetYear, MyBase.TargetMonth) <> DialogResult.No) Then
                        ' 更新処理
                        Me.UpdateDataTaxationOfficersAllowance()
                        'Me.Query(MyBase.TargetYear, MyBase.TargetMonth, MyBase.cmbBelonging.GetSelectedItem("c_constant_seq"))
                        ' 源泉徴収 - 課税対象者月例賃金情報取得
                        Me.Query(MyBase.TargetYear, MyBase.TargetMonth, MyBase.cmbBelonging.SelectedValue)
                        MyBase.ScreenMode = MODE.REFER
                    End If
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

        ' 内容変更ボタン押下処理
        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChange.Click
            Me.CalcTotal(False)
            MyBase.ScreenMode = MODE.EDIT
        End Sub

        ' 登録ボタン押下処理
        Private Sub btnRegist_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegist.Click
            Try
                Me.ValidateFields()
                Me.ValidateRows()

                ' 確認メッセージ表示
                If (CLMsg.Show("GQ0006", "対象年月", MyBase.TargetYear, MyBase.TargetMonth) <> DialogResult.No) Then
                    ' 更新処理
                    Me.UpdateData()
                    'Me.Query(MyBase.TargetYear, MyBase.TargetMonth, MyBase.cmbBelonging.GetSelectedItem("c_constant_seq"))
                    ' 源泉徴収 - 課税対象者月例賃金情報取得
                    Me.Query(MyBase.TargetYear, MyBase.TargetMonth, MyBase.cmbBelonging.SelectedValue)
                    MyBase.ScreenMode = MODE.REFER
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

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Dim nRemuneration As Long
            Dim nMonthly As Long
            Dim nTaxable As Long
            If isError Then
                MyBase.CalcTotal(isError)
                Me.lblSumTaxable.Text = "#VALUE!"
                Me.lblSumRemuneration.Text = "#VALUE!"
                Me.lblSumMonthly.Text = "#VALUE!"
            Else
                nRemuneration = 0
                nMonthly = 0
                nTaxable = 0
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    nRemuneration = (nRemuneration + MyBase.GetMoneyValue(Of Long)(i, COLIDX.DIRECTORS_REMUNERATION))
                    nMonthly = (nMonthly + MyBase.GetMoneyValue(Of Long)(i, COLIDX.MONTHLY_DEDUCTION))
                    nTaxable = (nTaxable + MyBase.GetMoneyValue(Of Long)(i, COLIDX.TAXABLE))
                Next i
                MyBase.CalcTotal(isError)
                Me.lblSumRemuneration.Text = nRemuneration.ToString("###,###,##0")
                Me.lblSumMonthly.Text = nMonthly.ToString("###,###,##0")
                Me.lblSumTaxable.Text = nTaxable.ToString("###,###,##0")
            End If
        End Sub

        ' 役員手当・月例控除ともに課税として計算
        Protected Sub CalcWithholding(ByVal row As Integer)
            Try
                Dim taxable As Long = 0
                Dim num2 As Long = 0
                Dim num3 As Long = CLng(MyBase.flxList.Item(row, COLIDX.DIRECTORS_REMUNERATION))
                taxable = (taxable + num3)
                taxable = (taxable + CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)))
                If (num3 > 0) Then
                    num2 = MyBase._business.CalcWithholding(MyBase.TargetYear, MyBase.TargetMonth, taxable)
                    MyBase.flxList.Item(row, COLIDX.TAXABLE) = (taxable + CLng(MyBase.flxList.Item(row, COLIDX.BONUS_DEDUCTION)))
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY) = num2
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING) = (CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING_BONUS)) + CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY)))
                    MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = ((CLng(MyBase.flxList.Item(row, COLIDX.TAXABLE)) - CLng(MyBase.flxList.Item(row, COLIDX.CUT_OFF))) - CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING)))
                Else
                    MyBase.flxList.Item(row, COLIDX.TAXABLE) = 0
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY) = 0
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING) = 0
                    MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = ((CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)) + CLng(MyBase.flxList.Item(row, COLIDX.BONUS_DEDUCTION))) - CLng(MyBase.flxList.Item(row, COLIDX.CUT_OFF)))
                End If
            Catch exception As AppUnionException
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            End Try
        End Sub

        ' iTaxFlag = True  の場合、役員手当・月例控除ともに課税対象として計算
        ' iTaxFlag = False の場合、役員手当のみ課税対象として計算
        Protected Sub CalcWithholding2(
            ByVal row As Integer,
            ByVal iTaxFlag As Boolean
        )
            Try
                Dim taxable As Long = 0     ' 課税対象額
                Dim num2 As Long = 0        ' 源泉徴収額

                ' 役員手当取得
                Dim num3 As Long = CLng(MyBase.flxList.Item(row, COLIDX.DIRECTORS_REMUNERATION))

                ' 課税対象額に役員手当追加
                taxable = (taxable + num3)

                ' 課税フラグチェック
                If iTaxFlag Then
                    ' 課税対象額に月例控除額を追加
                    taxable = (taxable + CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)))
                End If

                ' 課税対象額が、1円以上あるかチェック
                If (num3 > 0) Then
                    ' 課税対象額を元に税率表から源泉徴収額を求める
                    num2 = MyBase._business.CalcWithholding(MyBase.TargetYear, MyBase.TargetMonth, taxable)
                    ' 課税対象額
                    MyBase.flxList.Item(row, COLIDX.TAXABLE) = (taxable + CLng(MyBase.flxList.Item(row, COLIDX.BONUS_DEDUCTION)))
                    ' 源泉徴収額
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY) = num2
                    ' 
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING) = (CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING_BONUS)) + CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY)))
                    ' 差引支給額（役員手当 + 月例控除 - 源泉徴収額）
                    MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = num3 + CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)) - num2
                    'MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = ((CLng(MyBase.flxList.Item(row, COLIDX.TAXABLE)) - CLng(MyBase.flxList.Item(row, COLIDX.CUT_OFF))) - CLng(MyBase.flxList.Item(row, COLIDX.WITHHOLDING)))
                Else
                    ' 課税対象額
                    MyBase.flxList.Item(row, COLIDX.TAXABLE) = 0
                    ' 源泉徴収額
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING_MONTHLY) = 0
                    '
                    MyBase.flxList.Item(row, COLIDX.WITHHOLDING) = 0
                    ' 差引支給額（役員手当 + 月例控除 - 源泉徴収額）
                    MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = num3 + CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)) - num2
                    'MyBase.flxList.Item(row, COLIDX.ALLOWANCE) = ((CLng(MyBase.flxList.Item(row, COLIDX.MONTHLY_DEDUCTION)) + CLng(MyBase.flxList.Item(row, COLIDX.BONUS_DEDUCTION))) - CLng(MyBase.flxList.Item(row, COLIDX.CUT_OFF)))
                End If
            Catch exception As AppUnionException
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub flxList_AfterEdit(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.AfterEdit
            If (e.Col = COLIDX.DIRECTORS_REMUNERATION) Then
                Try
                    Dim num As Long = If((MyBase.flxList.Item(e.Row, e.Col) Is Nothing), 0, CLng(MyBase.flxList.Item(e.Row, e.Col)))
                    If (Me._before_remuneration <> num) Then
                        If (num < 0) Then
                            Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0049", New String(0 - 1) {})
                        End If
                        If (num = 0) Then
                            Dim num2 As Long = Me._before_remuneration
                            If (CLMsg.Show("GQ0014") = DialogResult.No) Then
                                MyBase.flxList.Item(e.Row, e.Col) = num2
                                e.Cancel = True
                                Return
                            End If
                        End If
                        MyBase.flxList.SetCellStyle(e.Row, e.Col, DirectCast(Nothing, CellStyle))
                        Me.CalcWithholding(e.Row)
                        Me.SetGridForEdit(e.Row)
                        Me.CalcTotal(False)
                    End If
                Catch exception As AppUnionException
                    MyBase.flxList.SetCellStyle(e.Row, e.Col, MyBase.flxList.Styles.Item("error_cell"))
                    MyBase.flxList.Item(e.Row, COLIDX.TAXABLE) = -1
                    MyBase.flxList.Item(e.Row, COLIDX.WITHHOLDING) = -1
                    MyBase.flxList.Item(e.Row, COLIDX.ALLOWANCE) = -1
                    e.Cancel = True
                    Me.CalcTotal(True)
                    Dim msg As New ExceptionMsg(exception)
                    If msg.IsNotContinue Then
                        CLMsg.Show("GE0001")
                        Return
                    End If
                    msg.ShowMessage()
                Catch exception2 As SysUnionException
                    e.Cancel = True
                    exception2.AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception2
                End Try
            End If
        End Sub

        Private Sub flxList_BeforeEdit(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.BeforeEdit
            Try
                If ((e.Col = COLIDX.DIRECTORS_REMUNERATION) AndAlso (e.Row >= MyBase.flxList.Rows.Fixed)) Then
                    Me._before_remuneration = If((MyBase.flxList.Item(e.Row, e.Col) Is Nothing), 0, CLng(MyBase.flxList.Item(e.Row, e.Col)))
                End If
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxList_SetupEditor(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxList.SetupEditor
            Dim editor As TextBox = TryCast(MyBase.flxList.Editor, TextBox)
            If (Not editor Is Nothing) Then
                editor.ImeMode = ImeMode.Disable
                editor.MaxLength = 7
            End If
        End Sub

        ' セルチェンジ
        Private Sub flxList_CellChanged(
            ByVal sender As Object,
            ByVal e As RowColEventArgs
        ) Handles flxList.CellChanged

            ' 非課税チェックモードの場合
            If MyBase.ScreenMode = MODE.EDIT_TAXATION Then

                ' チェックボックスにチェックが付いている場合
                If Me.flxList.Item(e.Row, COLIDX.CHECK) = True Then
                    '---------------------------------------------------
                    '   チェックボックスチェック
                    '---------------------------------------------------
                    ' 課税フラグチェック
                    If Me.flxList.Item(e.Row, COLIDX.TAXABLE_FLAG) = "1" Then
                        ' 役員手当のみ課税として自動計算
                        Call CalcWithholding2(e.Row, False)
                    End If
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.TAXABLE, MyBase.flxList.Styles.Item("changed_cell"))      ' 課税対象額フォント赤字
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.WITHHOLDING, MyBase.flxList.Styles.Item("changed_cell"))  ' 源泉徴収額フォント赤字
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.ALLOWANCE, MyBase.flxList.Styles.Item("changed_cell"))    ' 差引支給額フォント赤字
                Else
                    '---------------------------------------------------
                    '   チェックボックス未チェック
                    '---------------------------------------------------
                    ' 課税フラグチェック
                    If Me.flxList.Item(e.Row, COLIDX.TAXABLE_FLAG) = "1" Then
                        ' "1"の場合、役員手当・月例控除ともに課税として自動計算
                        Call CalcWithholding2(e.Row, True)
                    Else
                        ' "0"の場合、役員手当のみ課税として自動計算
                        Call CalcWithholding2(e.Row, False)
                    End If
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.TAXABLE, MyBase.flxList.Styles.Item("original_cell"))     ' 課税対象額フォント黒字
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.WITHHOLDING, MyBase.flxList.Styles.Item("original_cell")) ' 源泉対象額フォント黒字
                    MyBase.flxList.SetCellStyle(e.Row, COLIDX.ALLOWANCE, MyBase.flxList.Styles.Item("original_cell"))   ' 差引支給額フォント黒字
                End If
                ' 合計再計算
                Me.CalcTotal(False)
            End If

        End Sub

        Protected Overrides Function GetGridSetting(
            ByVal _mode As Integer
        ) As GridSettingInfo()
            Select Case _mode
                Case MODE.EDIT
                    ' 2016/12/07(水) 課税フラグカラム追加 Start
                    Return New GridSettingInfo() {
                        New GridSettingInfo(20, "check_col", False, False, False, False, True),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False),
                        New GridSettingInfo(90, "employee_number_col_nolink", False, False, True, False, True),
                        New GridSettingInfo(150, "name_col", False, False, False, False, True),
                        New GridSettingInfo(60, "readonly_col", False, False, True, False, True),
                        New GridSettingInfo(90, "directors_remuneration_col", False, False, True, True, True),
                        New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False),
                        New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(&H4B, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True),
                        New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False),
                        New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False),
                        New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False)
                    }
                    'Return New GridSettingInfo() {New GridSettingInfo(20, "check_col", False, False, False, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(90, "employee_number_col_nolink", False, False, True, False, True), New GridSettingInfo(150, "name_col", False, False, False, False, True), New GridSettingInfo(60, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "directors_remuneration_col", False, False, True, True, True), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(&H4B, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "readonly_col", True, True, True, True, True)}
                    ' 2016/12/07(水) 課税フラグカラム追加 End
                Case MODE.REFER
                    ' 2016/12/07(水) 課税フラグカラム追加 Start
                    Return New GridSettingInfo() {
                        New GridSettingInfo(20, "check_col", False, False, False, True, True),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False),
                        New GridSettingInfo(90, "employee_number_col_link", False, False, True, False, True),
                        New GridSettingInfo(150, "name_col", False, False, False, False, True),
                        New GridSettingInfo(60, "readonly_col", False, False, True, False, True),
                        New GridSettingInfo(90, "directors_remuneration_col", False, False, True, False, True),
                        New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False),
                        New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(&H4B, "noedit_money_col", False, False, True, False, True),
                        New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True),
                        New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False),
                        New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False),
                        New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False),
                        New GridSettingInfo(0, "readonly_col", False, False, False, False, False)
                    }
                    'Return New GridSettingInfo() {New GridSettingInfo(20, "check_col", False, False, False, True, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(90, "employee_number_col_link", False, False, True, False, True), New GridSettingInfo(150, "name_col", False, False, False, False, True), New GridSettingInfo(60, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "directors_remuneration_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(&H4B, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
                    ' 2016/12/07(水) 課税フラグカラム追加 End
            End Select
            Return Nothing
        End Function

        Private Function GetOriginalRemuneration(ByVal strUserId As String) As Long
            Return Convert.ToInt64(MyBase._original.Select(("ユーザＩＤ" & " = '" & strUserId & "'"))(0).Item(COLIDX.DIRECTORS_REMUNERATION - 1))
        End Function

        Protected Overrides Function GetOutputFileName() As String
            Return String.Concat(New String() {"月例賃金" & " - " & "課税対象者", MyBase.lblYear.Text, "年", MyBase.lblMonth.Text, "月分" & " ", MyBase.cmbBelonging.Text})
        End Function

        Private Sub InitializeComponent()
            Me.btnChange = New System.Windows.Forms.Button
            Me.lblSumRemuneration = New System.Windows.Forms.Label
            Me.lblSumMonthly = New System.Windows.Forms.Label
            Me.lblSumTaxable = New System.Windows.Forms.Label
            Me.btnRegist = New System.Windows.Forms.Button
            Me.btnNonTaxation = New System.Windows.Forms.Button
            Me.btnNonTaxationRegist = New System.Windows.Forms.Button
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblSumPayOut
            '
            Me.lblSumPayOut.Location = New System.Drawing.Point(847, 673)
            '
            'lblSumTruncate
            '
            Me.lblSumTruncate.Location = New System.Drawing.Point(671, 673)
            '
            'lblSumWithholding
            '
            Me.lblSumWithholding.Location = New System.Drawing.Point(747, 673)
            '
            'btnAllCheckOff
            '
            Me.btnAllCheckOff.Location = New System.Drawing.Point(91, 676)
            '
            'btnAllCheckOn
            '
            Me.btnAllCheckOn.Location = New System.Drawing.Point(59, 676)
            '
            'btnBackOrCancel
            '
            Me.btnBackOrCancel.Location = New System.Drawing.Point(798, 717)
            Me.btnBackOrCancel.TabIndex = 20
            '
            'btnOutputFile
            '
            Me.btnOutputFile.Location = New System.Drawing.Point(108, 717)
            '
            'btnPrintDetails
            '
            Me.btnPrintDetails.Location = New System.Drawing.Point(522, 717)
            Me.btnPrintDetails.TabIndex = 18
            '
            'btnPrintList
            '
            Me.btnPrintList.Location = New System.Drawing.Point(660, 717)
            Me.btnPrintList.TabIndex = 19
            '
            'btnShow
            '
            Me.btnShow.Location = New System.Drawing.Point(391, 18)
            '
            'cmbBelonging
            '
            Me.cmbBelonging.Location = New System.Drawing.Point(299, 18)
            '
            'flxList
            '
            Me.flxList.Location = New System.Drawing.Point(59, 59)
            Me.flxList.Rows.Count = 1
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(903, 611)
            '
            'label6
            '
            Me.label6.Location = New System.Drawing.Point(133, 22)
            '
            'label7
            '
            Me.label7.Location = New System.Drawing.Point(198, 23)
            '
            'lblBelongLocal
            '
            Me.lblBelongLocal.Location = New System.Drawing.Point(256, 23)
            '
            'lblMonth
            '
            Me.lblMonth.Location = New System.Drawing.Point(163, 18)
            '
            'lblYear
            '
            Me.lblYear.Location = New System.Drawing.Point(79, 18)
            '
            'btnChange
            '
            Me.btnChange.Location = New System.Drawing.Point(246, 717)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New System.Drawing.Size(116, 32)
            Me.btnChange.TabIndex = 17
            Me.btnChange.Text = "内容変更"
            Me.btnChange.UseVisualStyleBackColor = True
            '
            'lblSumRemuneration
            '
            Me.lblSumRemuneration.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumRemuneration.ForeColor = System.Drawing.Color.Blue
            Me.lblSumRemuneration.Location = New System.Drawing.Point(380, 673)
            Me.lblSumRemuneration.Name = "lblSumRemuneration"
            Me.lblSumRemuneration.Size = New System.Drawing.Size(89, 23)
            Me.lblSumRemuneration.TabIndex = 10
            Me.lblSumRemuneration.Text = "9,999,999"
            Me.lblSumRemuneration.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSumMonthly
            '
            Me.lblSumMonthly.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumMonthly.ForeColor = System.Drawing.Color.Blue
            Me.lblSumMonthly.Location = New System.Drawing.Point(470, 673)
            Me.lblSumMonthly.Name = "lblSumMonthly"
            Me.lblSumMonthly.Size = New System.Drawing.Size(99, 23)
            Me.lblSumMonthly.TabIndex = 11
            Me.lblSumMonthly.Text = "999,999,999"
            Me.lblSumMonthly.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSumTaxable
            '
            Me.lblSumTaxable.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumTaxable.ForeColor = System.Drawing.Color.Blue
            Me.lblSumTaxable.Location = New System.Drawing.Point(570, 673)
            Me.lblSumTaxable.Name = "lblSumTaxable"
            Me.lblSumTaxable.Size = New System.Drawing.Size(99, 23)
            Me.lblSumTaxable.TabIndex = 13
            Me.lblSumTaxable.Text = "999,999,999"
            Me.lblSumTaxable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'btnRegist
            '
            Me.btnRegist.Location = New System.Drawing.Point(246, 717)
            Me.btnRegist.Name = "btnRegist"
            Me.btnRegist.Size = New System.Drawing.Size(116, 32)
            Me.btnRegist.TabIndex = 17
            Me.btnRegist.Text = "登録"
            Me.btnRegist.UseVisualStyleBackColor = True
            '
            'btnNonTaxation
            '
            Me.btnNonTaxation.Location = New System.Drawing.Point(383, 717)
            Me.btnNonTaxation.Name = "btnNonTaxation"
            Me.btnNonTaxation.Size = New System.Drawing.Size(116, 32)
            Me.btnNonTaxation.TabIndex = 22
            Me.btnNonTaxation.Text = "非課税チェック"
            Me.btnNonTaxation.UseVisualStyleBackColor = True
            '
            'btnNonTaxationRegist
            '
            Me.btnNonTaxationRegist.Location = New System.Drawing.Point(383, 717)
            Me.btnNonTaxationRegist.Name = "btnNonTaxationRegist"
            Me.btnNonTaxationRegist.Size = New System.Drawing.Size(116, 32)
            Me.btnNonTaxationRegist.TabIndex = 23
            Me.btnNonTaxationRegist.Text = "非課税登録"
            Me.btnNonTaxationRegist.UseVisualStyleBackColor = True
            '
            'CtlMonthlyTaxableDetail
            '
            Me.Controls.Add(Me.btnNonTaxation)
            Me.Controls.Add(Me.btnNonTaxationRegist)
            Me.Controls.Add(Me.lblSumTaxable)
            Me.Controls.Add(Me.lblSumMonthly)
            Me.Controls.Add(Me.lblSumRemuneration)
            Me.Controls.Add(Me.btnChange)
            Me.Controls.Add(Me.btnRegist)
            Me.Margin = New System.Windows.Forms.Padding(4)
            Me.Name = "CtlMonthlyTaxableDetail"
            Me.Controls.SetChildIndex(Me.lblSumTruncate, 0)
            Me.Controls.SetChildIndex(Me.lblSumWithholding, 0)
            Me.Controls.SetChildIndex(Me.lblSumPayOut, 0)
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOn, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOff, 0)
            Me.Controls.SetChildIndex(Me.flxList, 0)
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)
            Me.Controls.SetChildIndex(Me.btnPrintList, 0)
            Me.Controls.SetChildIndex(Me.btnPrintDetails, 0)
            Me.Controls.SetChildIndex(Me.label7, 0)
            Me.Controls.SetChildIndex(Me.label6, 0)
            Me.Controls.SetChildIndex(Me.lblYear, 0)
            Me.Controls.SetChildIndex(Me.lblMonth, 0)
            Me.Controls.SetChildIndex(Me.lblBelongLocal, 0)
            Me.Controls.SetChildIndex(Me.cmbBelonging, 0)
            Me.Controls.SetChildIndex(Me.btnShow, 0)
            Me.Controls.SetChildIndex(Me.btnRegist, 0)
            Me.Controls.SetChildIndex(Me.btnChange, 0)
            Me.Controls.SetChildIndex(Me.lblSumRemuneration, 0)
            Me.Controls.SetChildIndex(Me.lblSumMonthly, 0)
            Me.Controls.SetChildIndex(Me.lblSumTaxable, 0)
            Me.Controls.SetChildIndex(Me.btnNonTaxationRegist, 0)
            Me.Controls.SetChildIndex(Me.btnNonTaxation, 0)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Protected Overrides Sub ResetTotalLabels()
            MyBase.ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumMonthly, Me.lblSumRemuneration, Me.lblSumTaxable})
        End Sub

        ' 照会モード
        Protected Overrides Sub SetReferMode()
            MyBase.SetReferMode()
            MyBase.flxList.AllowSorting = AllowSortingEnum.SingleColumn
            Me.btnRegist.Visible = False            ' 登録ボタン非表示
            Me.btnChange.Visible = True             ' 内容変更ボタン表示
            Me.btnChange.Enabled = True             ' 内容変更ボタン活性
            Me.btnNonTaxation.Visible = True        ' 非課税チェックボタン表示
            Me.btnNonTaxation.Enabled = True        ' 非課税チェックボタン非活性
            Me.btnNonTaxationRegist.Visible = False ' 非課税登録ボタン非表示
        End Sub

        ' 編集モード（内容変更ボタン押下時）
        Protected Overrides Sub SetEditMode()
            MyBase.SetEditMode()
            MyBase.flxList.AllowSorting = AllowSortingEnum.None
            Me.btnRegist.Visible = True             ' 登録ボタン表示
            Me.btnRegist.Enabled = True             ' 登録ボタン非活性
            Me.btnChange.Visible = False            ' 内容変更ボタン非表示
            Me.btnNonTaxationRegist.Visible = True  ' 非課税登録ボタン表示
            Me.btnNonTaxationRegist.Enabled = False ' 非課税登録ボタン非活性
            Me.btnNonTaxation.Visible = False       ' 非課税チェックボタン非表示
            'Me.btnRegist.Visible = True             ' 登録ボタン表示
            'Me.btnRegist.Enabled = True             ' 登録ボタン非活性
            'Me.btnChange.Visible = False            ' 内容変更ボタン非表示
            'Me.btnNonTaxationRegist.Visible = False ' 非課税登録ボタン非表示
            'Me.btnNonTaxation.Visible = True        ' 非課税チェックボタン表示
            'Me.btnNonTaxation.Enabled = False       ' 非課税チェックボタン非活性
        End Sub

        ' 編集モード（非課税チェックボタン押下時）
        Protected Overrides Sub SetEditTaxationMode()
            MyBase.SetEditTaxationMode()
            MyBase.flxList.AllowSorting = AllowSortingEnum.None
            Me.btnRegist.Visible = True             ' 登録ボタン表示
            Me.btnRegist.Enabled = False            ' 登録ボタン非活性
            Me.btnChange.Visible = False            ' 内容変更ボタン非表示
            Me.btnNonTaxationRegist.Visible = True  ' 非課税登録ボタン表示
            Me.btnNonTaxationRegist.Enabled = True  ' 非課税登録ボタン活性
            Me.btnNonTaxation.Visible = False       ' 非課税チェックボタン非活性
            'Me.btnRegist.Visible = False            ' 登録ボタン非表示
            'Me.btnChange.Visible = True             ' 内容変更ボタン表示
            'Me.btnChange.Enabled = False            ' 内容変更ボタン非活性
            'Me.btnNonTaxationRegist.Visible = True  ' 非課税登録ボタン表示
            'Me.btnNonTaxationRegist.Enabled = True  ' 非課税登録ボタン非活性
            'Me.btnNonTaxation.Visible = False       ' 非課税チェックボタン非活性
        End Sub

        Private Sub SetGridForEdit(ByVal row As Integer)
            Dim style As CellStyle = Nothing
            If (Me.GetOriginalRemuneration(MyBase.flxList.Item(row, COLIDX.USER_ID).ToString) <> CLng(MyBase.flxList.Item(row, COLIDX.DIRECTORS_REMUNERATION))) Then
                style = MyBase.flxList.Styles.Item("changed_cell")
            Else
                style = MyBase.flxList.Styles.Item("original_cell")
            End If
            Me.SetStyleForChangeableColumns(row, style)
        End Sub

        Private Sub SetStyleForChangeableColumns(ByVal row As Integer, ByVal style As CellStyle)
            MyBase.flxList.SetCellStyle(row, COLIDX.DIRECTORS_REMUNERATION, style)
            If Not style.Equals(MyBase.flxList.Styles.Item("error_cell")) Then
                MyBase.flxList.SetCellStyle(row, COLIDX.TAXABLE, style)
                MyBase.flxList.SetCellStyle(row, COLIDX.WITHHOLDING, style)
                MyBase.flxList.SetCellStyle(row, COLIDX.ALLOWANCE, style)
            End If
        End Sub

        ' 源泉徴収更新処理（非課税対象者→課税対象者）
        Private Sub UpdateData()
            Dim num As Integer = 0
            Dim saveData As DataTable = MyBase._original.Copy
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                Dim strUserId As String = MyBase.flxList.Item(i, COLIDX.USER_ID).ToString
                If (Me.GetOriginalRemuneration(strUserId) <> CLng(MyBase.flxList.Item(i, COLIDX.DIRECTORS_REMUNERATION))) Then
                    Dim rowArray As DataRow() = saveData.Select(("ユーザＩＤ" & " = '" & strUserId & "'"))
                    rowArray(0).Item(0) = True
                    rowArray(0).Item(COLIDX.DIRECTORS_REMUNERATION) = MyBase.flxList.Item(i, COLIDX.DIRECTORS_REMUNERATION)
                    rowArray(0).Item(COLIDX.WITHHOLDING_MONTHLY) = MyBase.flxList.Item(i, COLIDX.WITHHOLDING_MONTHLY)
                    num += 1
                End If
            Next i
            If (num = 0) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0010", New String(0 - 1) {})
            End If
            Try
                MyBase._business.UpdateData(MyBase.TargetYear, MyBase.TargetMonth, saveData, MDLoginInfo.UserId)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        ''' <summary>
        ''' 源泉徴収更新処理（役員手当のみ課税対象額として更新）
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdateDataTaxationOfficersAllowance()

            ' データ配列作成
            Dim dt As New DataTable
            dt.Columns.Add("userId", GetType(String))   ' 社員番号
            dt.Columns.Add("tax", GetType(Long))        ' 源泉徴収額

            ' 課税対象者リスト件数分ループ
            For i = 1 To MyBase.flxList.Rows.Count - 1
                ' チェックボックスにチェックが付いているかチェック
                If MyBase.flxList.Item(i, COLIDX.CHECK) Then
                    ' 社員番号取得
                    Dim strUserId As String = MyBase.flxList.Item(i, COLIDX.EMPLOYEE_NUMBER).ToString
                    ' 源泉徴収額取得
                    Dim tax As Long = MyBase.flxList.Item(i, COLIDX.WITHHOLDING).ToString
                    ' 社員番号と源泉徴収額を対象分、配列に取得
                    dt.Rows.Add(strUserId, tax)
                End If
            Next i

            ' 更新処理
            Try
                MyBase._business.UpdateDataTaxationOfficersAllowance(MyBase.TargetYear, MyBase.TargetMonth, MDLoginInfo.UserId, dt)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Private Sub ValidateRows()
            Dim num As Integer = 0
            Dim cellStyle As CellStyle = Nothing
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                cellStyle = MyBase.flxList.GetCellStyle(i, COLIDX.DIRECTORS_REMUNERATION)
                If ((Not cellStyle Is Nothing) AndAlso cellStyle.Equals(MyBase.flxList.Styles.Item("error_cell"))) Then
                    num += 1
                End If
            Next i
            If (num > 0) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0051", New String(0 - 1) {})
            End If
        End Sub

        ' Properties
        Protected Overrides ReadOnly Property CutDiv() As String
            Get
                Return "05"
            End Get
        End Property

        Protected Overrides ReadOnly Property ListReportName() As ReportClass
            Get
                'Return "Report.Withholding.RptWithholdingTable_local_cut_monthly"
                Return New CR0503P4
            End Get
        End Property

        ''' <summary>
        ''' チェックサポートメイン処理（8月に中執役員となったメンバーにチェックを付ける）
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub NewUserCheckMain()

            ' 課税対象者リスト件数分ループ
            For i = 1 To MyBase.flxList.Rows.Count - 1
                ' チェックボックスのチェックを未チェックにする
                MyBase.flxList.Item(i, COLIDX.CHECK) = False
            Next i

            ' 対象年月が8月かチェック
            If MyBase.TargetMonth = "08" Then
                ' 8月に中執役員となったメンバーにチェックを付ける
                Call Me.NewUserCheck(MyBase.TargetYear, MDLoginInfo.PeriodId)
            End If

        End Sub

        ''' <summary>
        ''' チェックサポート処理（8月に中執役員となったメンバにチェックを付ける）
        ''' </summary>
        ''' <param name="iTargetYear"></param>
        ''' <param name="iPeriodId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function NewUserCheck( _
            ByVal iTargetYear As String, _
            ByVal iPeriodId As String _
        ) As Boolean

            Dim blnRet As Boolean = False                       ' 処理結果
            Dim strSql As String = ""                           ' SQL文
            Dim clsDb As New UnionAct.NSCLAccessMdb.CLAccessMdb ' データベースクラス
            Dim tbRet As DataTable = Nothing                    ' 処理結果格納データテーブル
            Dim intRetCnt As Integer = 0                        ' 検索結果件数

            Try
                '---------------------------------------------------------------------------
                '   前月（7月）の中執役員メンバを除く今月（8月）の中執役員メンバを取得
                '---------------------------------------------------------------------------
                ' 今月（8月）の委員会名簿IDから社員番号を取得
                strSql += "SELECT com_list_dtl.c_user_id" & vbCrLf
                strSql += "  FROM committee_list_dtl AS com_list_dtl" & vbCrLf
                strSql += "      ,(" & vbCrLf
                '                 委員会IDから委員会名簿IDと委員会IDと適用開始年月日を取得
                strSql += "       SELECT t1.c_committee_list" & vbCrLf
                strSql += "             ,t1.c_committee_id" & vbCrLf
                strSql += "             ,t1.d_from" & vbCrLf
                strSql += "         FROM committee_list AS t1" & vbCrLf
                '                         対象年月・委員会ID・期・会社コードから委員会IDを取得
                strSql += "             ,(" & vbCrLf
                strSql += "               SELECT c_committee_id" & vbCrLf
                strSql += "                     ,MAX(d_from) AS now_from" & vbCrLf
                strSql += "                 FROM committee_list" & vbCrLf
                strSql += "                WHERE c_committee_id = '001'" & vbCrLf
                strSql += "                  AND left(d_from, 6) <= '" & iTargetYear & "08'" & vbCrLf
                strSql += "                  AND c_period_id = (" & vbCrLf
                strSql += "                      SELECT c_period_id" & vbCrLf
                strSql += "                        FROM period" & vbCrLf
                strSql += "                       WHERE '" & iTargetYear & "0801" & "' BETWEEN d_from AND d_to" & vbCrLf
                strSql += "                      )" & vbCrLf
                strSql += "                GROUP BY c_committee_id" & vbCrLf
                strSql += "              ) AS t2" & vbCrLf
                strSql += "        WHERE t1.c_committee_id = t2.c_committee_id" & vbCrLf
                strSql += "          AND t1.d_from = t2.now_from" & vbCrLf
                strSql += "       ) AS com_list" & vbCrLf
                strSql += "      ,committee_dtl AS com_dtl" & vbCrLf
                strSql += " WHERE com_list.c_committee_list = com_list_dtl.c_committee_list" & vbCrLf
                strSql += "   AND com_list.c_committee_id = com_dtl.c_committee_id" & vbCrLf
                strSql += "   AND com_list_dtl.s_committee_seq = com_dtl.s_committee_seq" & vbCrLf
                strSql += "   AND com_list.d_from BETWEEN com_dtl.d_from AND d_to" & vbCrLf
                strSql += "   AND com_dtl.c_officer_pay_id <> ''" & vbCrLf
                strSql += "   AND com_list_dtl.c_user_id NOT IN (" & vbCrLf
                '                 前月（7月）の委員会名簿IDから社員番号を取得
                strSql += "       SELECT com_list_dtl2.c_user_id" & vbCrLf
                strSql += "         FROM committee_list_dtl AS com_list_dtl2" & vbCrLf
                strSql += "             ,(" & vbCrLf
                '                         委員会IDから委員会名簿IDと委員会IDと適用開始年月日を取得
                strSql += "               SELECT t3.c_committee_list" & vbCrLf
                strSql += "                     ,t3.c_committee_id" & vbCrLf
                strSql += "                     ,t3.d_from" & vbCrLf
                strSql += "                 FROM committee_list AS t3" & vbCrLf
                '                                対象年月・委員会ID・期・会社コードから委員会IDを取得
                strSql += "                     ,(" & vbCrLf
                strSql += "                       SELECT c_committee_id" & vbCrLf
                strSql += "                             ,MAX(d_from) AS now_from" & vbCrLf
                strSql += "                         FROM committee_list" & vbCrLf
                strSql += "                        WHERE c_committee_id = '001'" & vbCrLf
                strSql += "                          AND left(d_from, 6) <= '" & iTargetYear & "07'" & vbCrLf
                strSql += "                          AND c_period_id = (" & vbCrLf
                strSql += "                              SELECT c_period_id" & vbCrLf
                strSql += "                                FROM period" & vbCrLf
                strSql += "                               WHERE '" & iTargetYear & "0701" & "' BETWEEN d_from AND d_to" & vbCrLf
                strSql += "                              )" & vbCrLf
                strSql += "                        GROUP BY c_committee_id" & vbCrLf
                strSql += "                      ) AS t4" & vbCrLf
                strSql += "                WHERE t3.c_committee_id = t4.c_committee_id" & vbCrLf
                strSql += "                  AND t3.d_from = t4.now_from" & vbCrLf
                strSql += "              ) AS com_list2" & vbCrLf
                strSql += "             ,committee_dtl AS com_dtl2" & vbCrLf
                strSql += "        WHERE com_list2.c_committee_list = com_list_dtl2.c_committee_list" & vbCrLf
                strSql += "          AND com_list2.c_committee_id = com_dtl2.c_committee_id" & vbCrLf
                strSql += "          AND com_list_dtl2.s_committee_seq = com_dtl2.s_committee_seq" & vbCrLf
                strSql += "          AND com_list2.d_from BETWEEN com_dtl2.d_from AND com_dtl2.d_to" & vbCrLf
                strSql += "          AND com_dtl2.c_officer_pay_id <> ''" & vbCrLf
                strSql += "       )" & vbCrLf
                '           社員番号で並替
                strSql += " ORDER BY com_list_dtl.c_user_id " & vbCrLf

                ' データベース接続
                Call clsDb.Connect()

                ' SQL実行
                tbRet = clsDb.ExecuteSql(strSql)

                ' 件数取得
                intRetCnt = tbRet.Rows.Count

                ' 件数チェック
                If intRetCnt > 0 Then
                    ' 課税非対象リスト数分ループ
                    For i = 1 To MyBase.flxList.Rows.Count - 1
                        ' 社員番号数分ループ
                        For j = 0 To intRetCnt - 1
                            ' 社員番号が同じかチェック
                            If MyBase.flxList.Item(i, COLIDX.USER_ID).ToString = tbRet.Rows(j).Item(0).ToString() Then
                                ' 社員番号が同じ場合、チェックボックスにチェック
                                MyBase.flxList.Item(i, COLIDX.CHECK) = True
                            End If
                        Next j
                    Next i
                End If

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Finally
                ' データベース切断
                Call clsDb.Disconnect()
            End Try

            ' 戻り値設定
            Return blnRet

        End Function

        ' Fields
        Private _before_remuneration As Long
        Private _logger As ILog
        Private WithEvents btnNonTaxation As Button
        Private WithEvents btnChange As Button
        Private WithEvents btnRegist As Button
        Private components As IContainer
        Private lblSumMonthly As Label
        Private lblSumRemuneration As Label
        Private WithEvents btnNonTaxationRegist As System.Windows.Forms.Button
        Private lblSumTaxable As Label
    End Class
End Namespace
