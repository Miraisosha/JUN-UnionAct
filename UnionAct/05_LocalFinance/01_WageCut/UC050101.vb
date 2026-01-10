#Region " UC050101 "
'===========================================================================================================
'   クラスＩＤ　　：UC050101
'   クラス名称　　：賃金カット画面クラス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.Business.FinancialAffairs
Imports C1.Win.C1FlexGrid
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports System.Reflection
Imports UnionAct.Framework
Imports UnionAct.Framework.UnionConst
Imports UnionAct.Business.FinancialAffairs.WageReduction
Imports UnionAct.GUI.FinancialAffairs.WageReduction
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb

Public Class UC050101
    Inherits FinancialAffairsBase

#Region " コンストラクタ "
    ''' <summary>コンストラクタ</summary>
    ''' <remarks></remarks>
    Public Sub New()

        MyBase.New()
        Me._setteing_monthly = New GridSettingInfo() {New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(70, "readonly_col", False, False, False, False, True), New GridSettingInfo(100, "persons_col", False, False, True, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True)}
        Me._setteing_bonus = New GridSettingInfo() {New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(70, "readonly_col", False, False, False, False, True), New GridSettingInfo(100, "persons_col", False, False, True, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True)}
        Me.InitializeComponent()
        Me.tabWageReduction.TabPages.RemoveAt(2)
        Me._monthly = New WageReductionMonthlyCommand
        Me._bonus = New WageReductionBonusCommand

        ' フレックスグリッドスタイル設定処理
        Me.AddGridStyle()

        ' 月例タブ内情報初期化処理
        Me.InitMonthly()

        ' 一時金タブ内情報初期化処理
        Me.InitBonus()

    End Sub
#End Region

#Region " イベント "
#Region " 月例タブ "
#Region " 月例タブ内：対象年コンボボックスキーダウン処理 "
    ''' <summary>月例タブ内：対象年コンボボックスキーダウン処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmbMonthlyYear_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cmbMonthlyYear.KeyDown

        ' Enterキー押下時
        If (e.KeyCode = Keys.Return) Then
            ' 月例検索処理
            Me.QueryMonthly()
        End If

    End Sub
#End Region

#Region " 月例タブ内：対象月コンボボックスキーダウン処理 "
    ''' <summary>月例タブ内：対象月コンボボックスキーダウン処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmbMonthlyMonth_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cmbMonthlyMonth.KeyDown

        ' Enterキー押下時
        If (e.KeyCode = Keys.Return) Then
            ' 月例タブ内：月例検索処理
            Me.QueryMonthly()
        End If

    End Sub
#End Region

#Region " 月例タブ内：対象年月コンボボックス変更処理 "
    ''' <summary>月例タブ内：対象月コンボボックスキーダウン処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MonthlySearchConditionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMonthlyYear.SelectedIndexChanged, cmbMonthlyMonth.SelectedIndexChanged

        ' 月例タブ内の各ボタン非活性
        Me.btnRefInTime.Enabled = False                 ' 時間内：照会ボタン非活性
        Me.btnRefStrike.Enabled = False                 ' 争議行為：照会ボタン非活性
        Me.btnPrintMonthlyDetail.Enabled = False        ' 一覧プレ印刷ボタン非活性
        Me.btnPrintMonthlySummary.Enabled = False       ' 合計プレ印刷ボタン非活性

    End Sub
#End Region

#Region " 月例タブ内：検索ボタン押下処理 "
    ''' <summary>月例タブ：検索ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnMonthlySearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMonthlySearch.Click

        Try
            ' 月例タブ内：月例検索処理
            Me.QueryMonthly()

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 月例タブ内：時間内：照会ボタン押下処理 "
    ''' <summary>月例タブ内：時間内：照会ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRefInTime_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefInTime.Click

        Try
            ' 賃金カット - 月例・時間内画面表示（照会）
            Me.ShowDetailMonthly(Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text, WAGE_REDUCTION_KIND.IN_TIME, "賃金カット" & " - " & "月例・時間内")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 月例タブ内：時間内：新規登録ボタン押下処理 "
    ''' <summary>月例タブ内：時間内：新規登録ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNewInTime_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewInTime.Click

        Try
            ' 賃金カット - 月例・時間内画面表示（新規登録）
            Me.ShowDetailMonthly(Nothing, Nothing, WAGE_REDUCTION_KIND.IN_TIME, "賃金カット" & " - " & "月例・時間内")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 月例タブ内：争議行為：照会ボタン押下処理 "
    ''' <summary>月例タブ内：争議行為：照会ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRefStrike_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefStrike.Click

        Try
            ' 賃金カット - 月例・争議行為画面表示（照会）
            Me.ShowDetailMonthly(Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text, WAGE_REDUCTION_KIND.STRIKE, "賃金カット" & " - " & "月例・争議行為")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 月例タブ内：争議行為：新規登録ボタン押下処理 "
    ''' <summary>月例タブ内：争議行為：新規登録ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNewStrike_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewStrike.Click

        Try
            ' 賃金カット - 月例・争議行為画面表示（新規登録）
            Me.ShowDetailMonthly(Nothing, Nothing, WAGE_REDUCTION_KIND.STRIKE, "賃金カット" & " - " & "月例・争議行為")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 月例タブ内：一覧プレ印刷ボタン押下処理 "
    ''' <summary>月例タブ内：一覧プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintMonthlyDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintMonthlyDetail.Click

        Try
            ' 賃金カット補填レポート
            Dim viewer As New ReportViewer(Me._monthly.GetListPrintData(WAGE_REDUCTION_KIND.BOTH, Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text), New CR0501P1)

            ' プレビュー画面表示
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

#Region " 月例タブ内：合計プレ印刷ボタン押下処理 "
    ''' <summary>月例タブ内：合計プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintMonthlySummary_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintMonthlySummary.Click

        Try
            ' 賃金カット補填合計レポート
            Dim viewer As New ReportViewer(Me._monthly.GetSummaryPrintData(WAGE_REDUCTION_KIND.BOTH, Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text), New CR0501P2)

            ' プレビュー画面表示
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
#End Region

#Region " 一時金タブ "
#Region " 一時金タブ内：対象年コンボボックスキーダウン処理 "
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmbBonusYear_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cmbBonusYear.KeyDown

        ' Enterキー押下時
        If (e.KeyCode = Keys.Return) Then
            ' ' 月例検索処理
            Me.QueryBonus()
        End If
    End Sub
#End Region

#Region " 一時金タブ内：対象月コンボボックスキーダウン処理 "
    ''' <summary>一時金タブ内：対象月コンボボックスキーダウン処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmbBonusMonth_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cmbBonusMonth.KeyDown

        ' Enterキー押下時
        If (e.KeyCode = Keys.Return) Then
            ' 月例検索処理
            Me.QueryBonus()
        End If

    End Sub
#End Region

#Region " 一時金タブ内：対象年月コンボボックス変更処理 "
    ''' <summary>一時金タブ内：対象年月コンボボックス変更処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BonusSearchConditionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbBonusYear.SelectedIndexChanged, cmbBonusMonth.SelectedIndexChanged

        ' 一時金タブ内の各ボタン非活性
        Me.btnRefBonus.Enabled = False                  ' 参照ボタン非活性
        Me.btnPrintBonusDetail.Enabled = False          ' 一覧プレ印刷ボタン非活性
        Me.btnPrintBonusTotal.Enabled = False           ' 合計プレ印刷ボタン非活性

    End Sub
#End Region

#Region " 一時金タブ内：検索ボタン押下処理 "
    ''' <summary>一時金タブ内：検索ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBonusSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBonusSearch.Click

        Try
            ' 一時金タブ内：月例検索処理
            Me.QueryBonus(cmbCutOnceName.Text)

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 一時金タブ内：照会ボタン押下処理 "
    ''' <summary>一時金タブ内：照会ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRefBonus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefBonus.Click

        Try
            ' "賃金カット - 一時金画面表示
            Me.ShowDetailBonus(Me.cmbBonusYear.Text, Me.cmbBonusMonth.Text, Me.cmbCutOnceName.Text, "賃金カット" & " - " & "一時金")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region

#Region " 一時金タブ内：一覧プレ印刷ボタン押下処理 "
    ''' <summary>一時金タブ内：一覧プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintBonusDetail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintBonusDetail.Click

        Try
            ' 一時金　賃金カット補填レポート
            Dim viewer As New ReportViewer(Me._bonus.GetListPrintData(Me.cmbBonusYear.Text, Me.cmbBonusMonth.Text, Me.cmbCutOnceName.Text), New CR0501P3)

            ' プレビュー画面表示
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

#Region " 一時金タブ内：合計プレ印刷ボタン押下処理 "
    ''' <summary>一時金タブ内：合計プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintBonusTotal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintBonusTotal.Click

        Try
            ' 一時金　賃金カット補填合計レポート
            Dim viewer As New ReportViewer(Me.CreateBonusTotalPrintData(Me.cmbBonusYear.Text, Me.cmbBonusMonth.Text), New CR0501P4)

            ' プレビュー画面表示
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

#Region " 一時金タブ内：新規登録ボタン押下処理 "
    ''' <summary>一時金タブ内：新規登録ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNewBonus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewBonus.Click

        Try
            ' 賃金カット - 一時金画面表示（新規登録）
            Me.ShowDetailBonus(Nothing, Nothing, Nothing, "賃金カット" & " - " & "一時金")

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        End Try

    End Sub
#End Region
#End Region

#Region " 累計タブ "
#Region " 累計タブ内：検索ボタン押下処理 "
    ''' <summary>累計タブ内：検索ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSumUpQuery_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSumUpQuery.Click

        Try
            ' 累計：検索処理
            Me.QuerySumUp(Me.cmbSumUpYear.Text)

            ' 検索結果が2件以上の場合、各ボタン活性
            ' 検索結果が2件未満の場合、各ボタン非活性
            Dim blnEnabled As Boolean = (Me.flxSumUp.Rows.Count > 2)
            Me.btnRefSumUp.Enabled = blnEnabled                         ' 照会ボタン活性・非活性
            Me.btnPrintListSumUp.Enabled = blnEnabled                   ' 一覧プレ印刷ボタン活性・非活性
            Me.btnPrintSummarySumUp.Enabled = blnEnabled                ' 合計プレ印刷ボタン活性・非活性

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
        End Try

    End Sub

#Region " 累計タブ内：一覧プレ印刷ボタン押下処理 "
    ''' <summary>累計タブ内：一覧プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintListSumUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintListSumUp.Click

    End Sub
#End Region
#End Region
#End Region
#End Region

#Region " メソッド "
#Region " フレックスグリッドスタイル設定処理 "
    ''' <summary>フレックスグリッドスタイル設定処理</summary>
    ''' <remarks></remarks>
    Private Sub AddGridStyle()

        ' フレックスグリッド3つ定義
        Dim gridArray As C1FlexGrid() = New C1FlexGrid() {Me.flxInTime, Me.flxStrike, Me.flxBonus}

        ' フレックスグリッド数ループ
        For i As Integer = 0 To gridArray.Length - 1

            ' 行のフォント設定
            gridArray(i).Styles.Add("data_row").Font = FinancialAffairsUtility.GetGridFontNormal

            ' 合計行
            Dim style As CellStyle = gridArray(i).Styles.Add("total_row")
            style.Font = FinancialAffairsUtility.GetGridFontBold            ' フォント設定
            style.BackColor = Color.Khaki                                   ' バックカラー設定

            ' 読取専用カラムスタイル
            style = gridArray(i).Styles.Add("readonly_col")                 ' スタイル追加
            style.DataType = GetType(String)                                ' データ型設定
            style.TextAlign = TextAlignEnum.CenterCenter                    ' 表示位置設定：横中央揃え：縦中央揃え

            ' 数値カラムスタイル
            style = gridArray(i).Styles.Add("noedit_money_col")             ' スタイル追加
            style.DataType = GetType(Long)                                  ' データ型設定
            style.TextAlign = TextAlignEnum.RightCenter                     ' 表示位置設定：横右詰め：縦中央揃え
            style.Format = "N0"
        Next i

    End Sub
#End Region

#Region " CreateBonusTotalDetail： "
    ''' <summary></summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBonusTotalDetail() As DataTable

        Dim table As New DataTable("dtDetail")
        table.Columns.Add("s_time_cut_total", GetType(Long))
        table.Columns.Add("s_strike_cut_total", GetType(Long))
        table.Columns.Add("s_cut_total", GetType(Long))
        table.Columns.Add("s_cover_total", GetType(Long))
        table.Columns.Add("s_union_total", GetType(Integer))

        Dim row As DataRow = table.NewRow
        Dim row2 As Row = Me.flxBonus.Rows.Item((Me.flxBonus.Rows.Count - 1))
        For i As Integer = 3 To Me.flxBonus.Cols.Count - 1
            row.Item((i - 3)) = row2.Item(i)
        Next i
        table.Rows.Add(row)
        Return table

    End Function
#End Region

#Region " CreateBonusTotalHeader： "
    ''' <summary></summary>
    ''' <param name="TargetYear"></param>
    ''' <param name="TargetMonth"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBonusTotalHeader( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String _
    ) As DataTable

        Dim table As New DataTable("dtHeader")
        table.Columns.Add("l_year", GetType(String))
        table.Columns.Add("l_month", GetType(String))
        Dim row As DataRow = table.NewRow
        row.Item("l_year") = TargetYear
        row.Item("l_month") = TargetMonth
        table.Rows.Add(row)
        Return table

    End Function
#End Region

#Region " CreateBonusTotalPrintData： "
    ''' <summary></summary>
    ''' <param name="TargetYear"></param>
    ''' <param name="TargetMonth"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBonusTotalPrintData( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String _
    ) As DataSet

        Dim set2 As DataSet

        Try
            Dim ds As New DataSet
            Dim table As DataTable = Me.CreateBonusTotalHeader(TargetYear, TargetMonth)
            ds.Tables.Add(table)
            Dim table2 As DataTable = Me.CreateBonusTotalDetail
            ds.Tables.Add(table2)
            set2 = ds

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})

        End Try

        Return set2

    End Function
#End Region

#Region " ctlDetail_Cancel： "
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ctlDetail_Cancel(ByVal sender As Object, ByVal e As EventArgs)

        Me._detail.Dispose()
        Utilities.RestoreUserControl()
        If TypeOf sender Is CtlWageReductionMonthlyEntry Then
            Me.btnMonthlySearch_Click(Me.btnMonthlySearch, EventArgs.Empty)
        ElseIf TypeOf sender Is CtlWageReductionBonusEntry Then
            Me.btnBonusSearch_Click(Me.btnBonusSearch, EventArgs.Empty)
        End If

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

#Region " FormatBonusGridHeader： "
    ''' <summary></summary>
    ''' <param name="flex"></param>
    ''' <remarks></remarks>
    Private Sub FormatBonusGridHeader(ByVal flex As C1FlexGrid)

        flex.Styles.Normal.WordWrap = False
        flex.Rows.Fixed = 2
        flex.AllowMerging = AllowMergingEnum.FixedOnly
        flex.Rows.Item(0).AllowMerging = True
        flex.Cols.Item(0).AllowMerging = True
        flex.Cols.Item(1).AllowMerging = True

        Dim cg As CellRange
        cg = flex.GetCellRange(0, 0, 1, 0)
        cg.Data = "所属"
        cg = flex.GetCellRange(0, 1, 1, 1)
        cg.Data = "対象人数"
        cg = flex.GetCellRange(0, 2, 0, 4)
        cg.Data = "時間内"
        cg = flex.GetCellRange(0, 5, 0, 7)
        cg.Data = "争議行為"
        cg = flex.GetCellRange(0, 8, 0, 10)
        cg.Data = "合計"

        For i As Integer = 0 To 3 - 1
            flex.Item(1, (2 + (i * 3))) = "控除額計"
            flex.Item(1, (3 + (i * 3))) = "補填額計"
            flex.Item(1, (4 + (i * 3))) = "切捨額計"
        Next i

    End Sub
#End Region

#Region " FormatMonthlyGrid： "
    ''' <summary></summary>
    ''' <param name="flxGrid"></param>
    ''' <remarks></remarks>
    Private Sub FormatMonthlyGrid(ByVal flxGrid As C1FlexGrid)

        FinancialAffairsUtility.ApplyGridStyle(flxGrid, Me._setteing_monthly)
        If (flxGrid.Rows.Count > 1) Then
            Dim i As Integer
            For i = 1 To (flxGrid.Rows.Count - 1) - 1
                flxGrid.Rows.Item(i).Style = flxGrid.Styles.Item("data_row")
            Next i
            flxGrid.Rows.Item((flxGrid.Rows.Count - 1)).Style = flxGrid.Styles.Item("total_row")
        End If
        flxGrid.Refresh()

    End Sub
#End Region

#Region " FormatSumUpGrid：フレックスグリッド初期化処理 "
    ''' <summary>フレックスグリッド初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub FormatSumUpGrid()

        Me.flxSumUp.Styles.Normal.WordWrap = False
        Me.flxSumUp.Rows.Fixed = 2
        Me.flxSumUp.AllowMerging = AllowMergingEnum.FixedOnly
        Me.flxSumUp.Rows.Item(0).AllowMerging = True
        Me.flxSumUp.Cols.Item(0).AllowMerging = True
        Me.flxSumUp.Cols.Item(1).AllowMerging = True
        Dim cg As CellRange
        cg = Me.flxSumUp.GetCellRange(0, 0, 1, 0)
        cg.Data = "所属"
        cg = Me.flxSumUp.GetCellRange(0, 1, 0, 3)
        cg.Data = "時間内"
        cg = Me.flxSumUp.GetCellRange(0, 4, 0, 6)
        cg.Data = "争議行為"
        cg = Me.flxSumUp.GetCellRange(0, 7, 0, 9)
        cg.Data = "合計"
        Dim i As Integer
        For i = 0 To 3 - 1
            Me.flxSumUp.Item(1, (1 + (i * 3))) = "控除額計"
            Me.flxSumUp.Item(1, (2 + (i * 3))) = "補填額計"
            Me.flxSumUp.Item(1, (3 + (i * 3))) = "切捨額計"
        Next i

    End Sub
#End Region

#Region " InitBonus：一時金タブ内情報初期化処理 "
    ''' <summary>一時金タブ内情報初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitBonus()

        ' 一時金タブ内ボタン初期化処理
        Me.InitBonusButtons()

        ' 一時金タブ内コンボボックス初期化処理
        Me.InitBonusCombo()

        ' 一時金タブ内フレックスグリッド初期化処理
        Me.FormatBonusGrid()

    End Sub
#End Region

#Region " InitBonusButtons：一時金タブ内ボタン初期化処理 "
    ''' <summary>一時金タブ内ボタン初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitBonusButtons()

        ' 参照権限が有る場合、一時金タブ：検索ボタン活性
        ' 参照権限が無い場合、一時金タブ：検索ボタン非活性
        Me.btnBonusSearch.Enabled = MDFinanceCommon.GetReferencePower("UC050101")

        Me.btnRefBonus.Enabled = False              ' 一時金タブ：照会ボタン非活性
        Me.btnPrintBonusDetail.Enabled = False      ' 一時金タブ：一覧プレ印刷ボタン非活性
        Me.btnPrintBonusTotal.Enabled = False       ' 一時金タブ：合計プレ印刷ボタン非活性非活性

        ' 入力権限が有る場合、一時金タブ：新規登録ボタン活性
        ' 入力権限が有る場合、一時金タブ：新規登録ボタン非活性
        Me.btnNewBonus.Enabled = MDFinanceCommon.GetEntryPower("UC050101")

    End Sub
#End Region

#Region " InitBonusCombo：一時金タブ内コンボボックス初期化処理 "
    ''' <summary>一時金タブ内コンボボックス初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitBonusCombo()

        Dim minYear As Integer = 0
        Dim item As Integer = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
        Me.cmbBonusYear.Items.Clear()
        minYear = Me._bonus.GetMinYear
        If (minYear > item) Then
            Me.cmbBonusYear.Items.Add(item)
        Else
            Dim i As Integer = minYear
            Do While (i <= item)
                Me.cmbBonusYear.Items.Add(i)
                i += 1
            Loop
        End If
        Me.cmbBonusYear.SelectedIndex = If((Me.cmbBonusYear.Items.Count > 0), 0, -1)
        Me.cmbBonusMonth.Items.Clear()
        Me.cmbBonusMonth.Items.AddRange(UnionConst.MONTH_RANGE)
        Me.cmbBonusMonth.SelectedIndex = (Integer.Parse(PublicCommand.GetSystemDate.Substring(4, 2)) - 1)

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

    End Sub
#End Region

#Region " FormatBonusGrid：一時金タブ内フレックスグリッド初期化処理 "
    ''' <summary>一時金タブ内フレックスグリッド初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub FormatBonusGrid()

        ' 一時金タブ内フレックスグリッド初期設定処理
        FinancialAffairsUtility.ApplyGridStyle(Me.flxBonus, Me._setteing_bonus)

        If (Me.flxBonus.Rows.Count > 1) Then
            Dim i As Integer
            For i = 1 To (Me.flxBonus.Rows.Count - 1) - 1
                Me.flxBonus.Rows.Item(i).Style = Me.flxBonus.Styles.Item("data_row")
            Next i
            Me.flxBonus.Rows.Item((Me.flxBonus.Rows.Count - 1)).Style = Me.flxBonus.Styles.Item("total_row")
        End If

        ' フレックスグリッド再描画
        Me.flxBonus.Refresh()

    End Sub
#End Region

#Region " InitMonthly：月例タブ内情報初期化処理 "
    ''' <summary>月例タブ内情報初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitMonthly()

        Me.InitMonthlyButtons()             ' 月例：ボタン初期化処理
        Me.InitMonthlyCombo()               ' 月例：コンボボックス初期化処理
        Me.FormatMonthlyGrid(Me.flxInTime)  ' 月例：時間内フレックスグリッド初期化処理
        Me.FormatMonthlyGrid(Me.flxStrike)  ' 月例：争議行為フレックスグリッド初期化処理

    End Sub
#End Region

#Region " InitMonthlyButtons：月例：ボタン初期化処理 "
    ''' <summary>月例：ボタン初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitMonthlyButtons()

        ' 各ボタン制御
        Me.btnMonthlySearch.Enabled = MDFinanceCommon.GetReferencePower("UC050101") ' 月例：検索ボタン
        Me.btnRefInTime.Enabled = False                                             ' 月例：時間内：照会ボタン非活性
        Me.btnRefStrike.Enabled = False                                             ' 月例：争議行為：照会ボタン非活性
        Me.btnPrintMonthlyDetail.Enabled = False                                    ' 月例：一覧プレ印刷ボタン非活性
        Me.btnPrintMonthlySummary.Enabled = False                                   ' 月例：合計プレ印刷ボタン非活性

        ' なぜ2回処理してる？？
        Me.btnMonthlySearch.Enabled = MDFinanceCommon.GetReferencePower("UC050101") ' 月例：検索ボタン
        Me.btnRefInTime.Enabled = MDFinanceCommon.GetReferencePower("UC050101")     ' 月例：時間内：参照ボタン
        Me.btnNewInTime.Enabled = MDFinanceCommon.GetEntryPower("UC050101")         ' 月例：時間内：新規作成ボタン
        Me.btnRefStrike.Enabled = MDFinanceCommon.GetReferencePower("UC050101")     ' 月例：争議行為：照会ボタン
        Me.btnNewStrike.Enabled = MDFinanceCommon.GetEntryPower("UC050101")         ' 月例：争議行為：新規登録ボタン

    End Sub
#End Region

#Region " InitMonthlyCombo：月例：コンボボックス初期化処理 "
    ''' <summary>月例：コンボボックス初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitMonthlyCombo()

        Dim minYear As Integer = 0
        Dim item As Integer = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
        Me.cmbMonthlyYear.Items.Clear()
        minYear = Me._monthly.GetMinYear
        If (minYear > item) Then
            Me.cmbMonthlyYear.Items.Add(item)
        Else
            Dim i As Integer = minYear
            Do While (i <= item)
                Me.cmbMonthlyYear.Items.Add(i)
                i += 1
            Loop
        End If
        Me.cmbMonthlyYear.SelectedIndex = If((Me.cmbMonthlyYear.Items.Count > 0), 0, -1)
        Me.cmbMonthlyMonth.Items.Clear()
        Me.cmbMonthlyMonth.Items.AddRange(UnionConst.MONTH_RANGE)
        Me.cmbMonthlyMonth.SelectedIndex = (Integer.Parse(PublicCommand.GetSystemDate.Substring(4, 2)) - 1)

    End Sub
#End Region

#Region " InitSumUp：累計タブ初期化処理 "
    ''' <summary>累計タブ初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitSumUp()

        ' 累計タブ：ボタン初期化処理
        Me.InitSumUpButtons()

        ' 累計タブ：コンボボックス初期化処理
        Me.InitSumUpCombo()

        ' フレックスグリッド初期化処理
        Me.FormatSumUpGrid()

    End Sub
#End Region

#Region " InitSumUpButtons：累計タブ：ボタン初期化処理 "
    ''' <summary>累計タブ：ボタン初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitSumUpButtons()

        ' 累計タブ：参照ボタン非活性
        Me.btnRefSumUp.Enabled = False

    End Sub
#End Region

#Region " InitSumUpCombo：累計タブ：コンボボックス初期化処理 "
    ''' <summary>累計タブ：コンボボックス初期化処理</summary>
    ''' <remarks></remarks>
    Private Sub InitSumUpCombo()

        Dim minYear As Integer = 0
        Dim num2 As Integer = 0
        Dim num3 As Integer = 0

        ' システム日付取得（マシン日付）
        Dim item As Integer = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))

        ' 対象年コンボボックスリストクリア
        Me.cmbSumUpYear.Items.Clear()
        minYear = Me._monthly.GetMinYear
        num2 = Me._bonus.GetMinYear
        num3 = If((minYear < num2), minYear, num2)
        If (num3 > item) Then
            Me.cmbSumUpYear.Items.Add(item)
        End If
        Dim i As Integer = num3
        Do While (i <= item)
            Me.cmbSumUpYear.Items.Add(i)
            i += 1
        Loop

        ' 対象年コンボボックスリスト件数チェック
        If (Me.cmbSumUpYear.Items.Count > 0) Then
            Me.cmbSumUpYear.SelectedIndex = 0       ' 1件以上ある場合、先頭行選択
        Else
            Me.cmbSumUpYear.SelectedIndex = -1      ' 0件の場合、未選択
        End If

    End Sub
#End Region

#Region " QueryBonus：一時金：検索メイン処理 "
    ''' <summary>一時金：検索メイン処理</summary>
    ''' <param name="strBonusName">一時金名称</param>
    ''' <remarks></remarks>
    Private Sub QueryBonus(Optional ByVal strBonusName As String = "")

        Try
            ' 一時金：検索処理
            Me.QueryBonus(Me.cmbBonusYear.Text, Me.cmbBonusMonth.Text, strBonusName)
            If (Me.flxBonus.Rows.Count = 1) Then
                Me.btnRefBonus.Enabled = False
                Me.btnPrintBonusTotal.Enabled = False
                Me.btnPrintBonusDetail.Enabled = False
            Else
                Me.btnRefBonus.Enabled = MDFinanceCommon.GetReferencePower(MyBase.Name)
                Me.btnPrintBonusTotal.Enabled = MDFinanceCommon.GetPrintPower(MyBase.Name)
                Me.btnPrintBonusDetail.Enabled = MDFinanceCommon.GetPrintPower(MyBase.Name)
            End If

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})

        End Try

    End Sub
#End Region

#Region " QueryMonthly：月例：検索処理 "
    ''' <summary>一時金：検索処理</summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <param name="strBonusName">一時金名称</param>
    ''' <remarks></remarks>
    Private Sub QueryBonus( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String, _
        ByVal strBonusName As String _
    )

        Dim result As DataTable = Nothing

        Try
            result = Me._bonus.GetSummary(TargetYear, TargetMonth, strBonusName)

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        End Try

        Me.ShowBonusResult(TargetYear, TargetMonth, result)

    End Sub
#End Region

#Region " QueryMonthly：月例：検索処理 "
    ''' <summary>月例：検索処理</summary>
    ''' <remarks></remarks>
    Private Sub QueryMonthly()

        Try
            ' 時間内データ検索処理
            Me.QueryMonthlyInTime(Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text)

            ' 争議行為データ検索処理
            Me.QueryMonthlyStrike(Me.cmbMonthlyYear.Text, Me.cmbMonthlyMonth.Text)

            ' 時間内データ・争議行為データが無いかチェック
            If ((Me.flxStrike.Rows.Count = 1) _
            AndAlso (Me.flxInTime.Rows.Count = 1)) Then
                '-----------------------------------------------------------------------------------
                '   時間内・争議行為データが無い場合
                '-----------------------------------------------------------------------------------
                Me.btnPrintMonthlySummary.Enabled = False               ' 合計プレ印刷ボタン非活性
                Me.btnPrintMonthlyDetail.Enabled = False                ' 一覧プレ印刷ボタン非活性
            Else
                '-----------------------------------------------------------------------------------
                '   時間内・争議行為データが有る場合
                '-----------------------------------------------------------------------------------
                If MDFinanceCommon.GetPrintPower(MyBase.Name) Then
                    ' 印刷権限が有る場合
                    Me.btnPrintMonthlyDetail.Enabled = True             ' 一覧プレ印刷ボタン活性
                    Me.btnPrintMonthlySummary.Enabled = True            ' 合計プレ印刷ボタン活性
                Else
                    ' 印刷権限が無い場合
                    Me.btnPrintMonthlyDetail.Enabled = False            ' 一覧プレ印刷ボタン非活性
                    Me.btnPrintMonthlySummary.Enabled = False           ' 合計プレ印刷ボタン非活性
                End If
            End If

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
        End Try

    End Sub
#End Region

#Region " QueryMonthlyInTime：月例：時間内：検索処理 "
    ''' <summary>月例：時間内：検索処理</summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <remarks></remarks>
    Private Sub QueryMonthlyInTime( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String _
    )

        Dim result As DataTable = Nothing

        Try
            ' 月例：時間内：検索処理
            result = Me._monthly.GetInTimeSummary((TargetYear & TargetMonth))

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        End Try

        Me.ShowMonthlyResult(result, Me.grpInTime, String.Concat(New String() {"時間内（", TargetYear, "年", TargetMonth, "月分）"}), Me.flxInTime, Me.btnRefInTime, Me.btnNewInTime)

    End Sub
#End Region

#Region " QueryMonthlyStrike：月例：争議行為：検索処理 "
    ''' <summary>月例：争議行為：検索処理</summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <remarks></remarks>
    Private Sub QueryMonthlyStrike( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String _
    )

        Dim result As DataTable = Nothing

        Try
            result = Me._monthly.GetStrikeSummary((TargetYear & TargetMonth))

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        End Try

        Me.ShowMonthlyResult(result, Me.grpPersonalStrike, String.Concat(New String() {"争議行為（", TargetYear, "年", TargetMonth, "月分）"}), Me.flxStrike, Me.btnRefStrike, Me.btnNewStrike)

    End Sub
#End Region

#Region " QuerySumUp：累計タブ内：検索処理 "
    ''' <summary>累計タブ内：検索処理</summary>
    ''' <param name="TargetYear"></param>
    ''' <remarks></remarks>
    Private Sub QuerySumUp(ByVal TargetYear As String)

        Dim result As DataTable = Nothing

        Try
            result = Me._sumup.GetSummary(TargetYear)

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        End Try

        Me.ShowSumUpResult(TargetYear, result)

    End Sub
#End Region

#Region " SetEnabledByPowerInMonthly： "
    ''' <summary></summary>
    ''' <remarks></remarks>
    Private Sub SetEnabledByPowerInMonthly()

        Me.btnMonthlySearch.Enabled = MDFinanceCommon.GetReferencePower(MyBase.Name)
        Me.btnRefInTime.Enabled = MDFinanceCommon.GetReferencePower(MyBase.Name)
        Me.btnNewInTime.Enabled = MDFinanceCommon.GetEntryPower(MyBase.Name)
        Me.btnRefStrike.Enabled = MDFinanceCommon.GetReferencePower(MyBase.Name)
        Me.btnNewStrike.Enabled = MDFinanceCommon.GetEntryPower(MyBase.Name)
        Me.btnPrintMonthlyDetail.Enabled = MDFinanceCommon.GetPrintPower(MyBase.Name)
        Me.btnPrintMonthlySummary.Enabled = MDFinanceCommon.GetPrintPower(MyBase.Name)

    End Sub
#End Region

#Region " ShowBonusResult： "
    ''' <summary></summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Private Sub ShowBonusResult( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String, _
        ByVal result As DataTable _
    )

        Me.flxBonus.DataSource = result
        Me.FormatBonusGrid()
        Me.grpBonusResult.Text = (TargetYear & "年" & TargetMonth & "月分")
        Me.btnRefBonus.Enabled = (result.Rows.Count <> 0)

    End Sub
#End Region

#Region " ShowDetailBonus：賃金カット - 一時金画面表示処理（新規登録・照会）"
    ''' <summary>賃金カット - 一時金画面表示処理（新規登録・照会）</summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <param name="strBonusName">一時金名称</param>
    ''' <param name="Title">画面タイトル</param>
    ''' <remarks></remarks>
    Private Sub ShowDetailBonus( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String, _
        ByVal strBonusName As String, _
        ByVal Title As String _
    )

        Try
            ' 対象年・対象月・一時金名称チェック
            If TargetYear Is Nothing And TargetMonth Is Nothing And strBonusName Is Nothing Then
                ' 賃金カット - 一時金（新規登録）画面表示
                Me._detail = New CtlWageReductionBonusNewEntry(TargetYear, TargetMonth, strBonusName, MyBase.Name, New EventHandler(AddressOf Me.ctlDetail_Cancel))
            Else
                ' 賃金カット - 一時金（照会）画面表示
                Me._detail = New CtlWageReductionBonusEntry(TargetYear, TargetMonth, strBonusName, MyBase.Name, New EventHandler(AddressOf Me.ctlDetail_Cancel))
            End If

            ' 賃金カット画面非表示
            MyBase.Visible = False
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), Title, New UserControl() {Me._detail})

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
        End Try

    End Sub
#End Region

#Region " ShowDetailMonthly：賃金カット - 月例・時間内争議行為画面表示処理（照会・新規登録） "
    ''' <summary>
    ''' 賃金カット - 月例・時間内画面表示（照会）
    ''' 賃金カット - 月例・時間内画面表示（新規登録）
    ''' 賃金カット - 月例・争議行為画面表示（照会）
    ''' 賃金カット - 月例・争議行為画面表示（新規登録）
    ''' </summary>
    ''' <param name="TargetYear">対象年</param>
    ''' <param name="TargetMonth">対象月</param>
    ''' <param name="kind">種別（1：時間内, 2：争議行為）</param>
    ''' <param name="Title">画面タイトル</param>
    ''' <remarks></remarks>
    Private Sub ShowDetailMonthly( _
        ByVal TargetYear As String, _
        ByVal TargetMonth As String, _
        ByVal kind As WAGE_REDUCTION_KIND, _
        ByVal Title As String _
    )

        Try
            ' 賃金カット - 月例・時間内・争議行為画面
            Me._detail = New CtlWageReductionMonthlyEntry(TargetYear, TargetMonth, MyBase.Name, kind, New EventHandler(AddressOf Me.ctlDetail_Cancel))

            ' 賃金カット画面非表示
            MyBase.Visible = False

            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), Title, New UserControl() {Me._detail})

        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception

        Catch exception2 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
        End Try

    End Sub
#End Region

#Region " ShowMonthlyResult：賃金カット - 一時金画面表示処理 "
    ''' <summary>賃金カット - 一時金画面表示処理</summary>
    ''' <param name="result"></param>
    ''' <param name="grpMonthly"></param>
    ''' <param name="GroupText"></param>
    ''' <param name="flxGrid"></param>
    ''' <param name="btnRef"></param>
    ''' <param name="btnNew"></param>
    ''' <remarks></remarks>
    Private Sub ShowMonthlyResult( _
        ByVal result As DataTable, _
        ByVal grpMonthly As GroupBox, _
        ByVal GroupText As String, _
        ByVal flxGrid As C1FlexGrid, _
        ByVal btnRef As Button, _
        ByVal btnNew As Button _
    )

        flxGrid.DataSource = result
        Me.FormatMonthlyGrid(flxGrid)

        ' タイトル設定
        grpMonthly.Text = GroupText
        btnRef.Enabled = ((result.Rows.Count <> 0) AndAlso MDFinanceCommon.GetReferencePower(MyBase.Name))

    End Sub
#End Region

#Region " ShowSumUpResult：累計画面表示処理 "
    ''' <summary>累計画面表示処理</summary>
    ''' <param name="TargetYear"></param>
    ''' <param name="result"></param>
    ''' <remarks></remarks>
    Private Sub ShowSumUpResult( _
        ByVal TargetYear As String, _
        ByVal result As DataTable _
    )

    End Sub
#End Region
#End Region

End Class
#End Region
