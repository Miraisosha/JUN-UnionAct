Imports UnionAct.Business.Common
Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports UnionAct.Framework
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.FinancialAffairs
Imports log4net
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs
Imports UnionAct.NSCLMsg
Imports UnionAct.Business.FinancialAffairs.WithHolding
Imports CrystalDecisions.CrystalReports.Engine
Imports UnionAct.NSCLAccessMdb

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlWithHoldingDetailBase
        Inherits FinancialAffairsBase
        ' Events
        Private _cancel As EventHandler
        ' Methods
        'Private command As New WithholdingCommand

        ' Methods
        Public Sub New()
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me._mode = MODE.REFER
            Me._branch = -1
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal Year As String, ByVal Month As String, ByVal strNameForRight As String, ByVal CancelHandler As EventHandler)
            MyBase.New()
            Me._logger = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
            Me._mode = MODE.REFER
            Me._branch = -1
            Me.InitializeComponent()
            Me.TargetYear = Year
            Me.TargetMonth = Month
            Me._cancel = CancelHandler
            Me.SetComboItems(Me.TargetYear, Me.TargetMonth)
            Me._branch = Me.cmbBelonging.SelectedIndex
            Me._business = Me.GetBusObj
            Me._strNameForRight = strNameForRight
            Me.btnAllCheckOn.Enabled = Me.HasPrintPower
            Me.btnAllCheckOff.Enabled = Me.HasPrintPower
            Me.btnPrintDetails.Enabled = Me.HasPrintPower
            Me.btnPrintList.Enabled = Me.HasPrintPower
            Me.btnOutputFile.Enabled = Me.HasOutputPower
        End Sub

        Protected Overridable Sub AddFlexGridStyle()
            Dim style As CellStyle = Nothing
            style = Me.flxList.Styles.Add("check_col")
            style.DataType = GetType(Boolean)
            style.ImageAlign = ImageAlignEnum.CenterCenter
            style = Me.flxList.Styles.Add("employee_number_col_nolink")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!)
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.RightCenter
            style.BackColor = Color.LightYellow
            style.ForeColor = SystemColors.WindowText
            style = Me.flxList.Styles.Add("employee_number_col_link")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!, FontStyle.Underline)
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.RightCenter
            style.BackColor = Color.LightYellow
            style.ForeColor = Color.Blue
            style = Me.flxList.Styles.Add("name_col")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!)
            style.DataType = GetType(String)
            style.TextAlign = TextAlignEnum.LeftCenter
            style.BackColor = Color.LightYellow
            style = Me.flxList.Styles.Add("readonly_col")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!)
            style.DataType = GetType(String)
            style.TextAlign = TextAlignEnum.CenterCenter
            style.BackColor = Color.LightYellow
            style = Me.flxList.Styles.Add("noedit_money_col")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!)
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.RightCenter
            style.Format = "N0"
            style.BackColor = Color.LightYellow
            style = Me.flxList.Styles.Add("hyper_link_col")
            style.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!)
            style.DataType = GetType(String)
            style.TextAlign = TextAlignEnum.CenterCenter
            style.BackColor = Color.LightYellow
        End Sub

        ''' <summary>
        ''' オールチェックOFFボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnAllCheckOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAllCheckOff.Click
            Me.SetRowCheck(False)
        End Sub

        ''' <summary>
        ''' オールチェックONボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnAllCheckOn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAllCheckOn.Click
            Me.SetRowCheck(True)
        End Sub

        ''' <summary>
        ''' キャンセル・戻るボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnBackOrCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackOrCancel.Click

            Select Case Me.ScreenMode
                Case MODE.EDIT
                    ' 編集モード（内容変更ボタン押下時）
                    If (CLMsg.Show("GQ0007") <> DialogResult.Yes) Then
                        Exit Select
                    End If
                    Me.ScreenMode = MODE.REFER
                    Return

                Case MODE.EDIT_TAXATION
                    ' 編集モード（非課税チェックボタン押下）
                    If (CLMsg.Show("GQ0007") <> DialogResult.Yes) Then
                        Exit Select
                    End If
                    Me.ScreenMode = MODE.REFER
                    Return

                Case MODE.REFER
                    ' 参照モード
                    Me.FireCancel(Me, EventArgs.Empty)
                    Exit Select
                Case Else
                    Return

            End Select
        End Sub

        ''' <summary>
        ''' ファイル出力ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnOutputFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOutputFile.Click
            Try
                Cursor.Current = Cursors.WaitCursor
                Dim class2 As New FactoryBusClass
                'Dim command As ICSVFileCommand = DirectCast(class2.GetObject("Business.Common.CSVFileCommand"), ICSVFileCommand)
                Dim command As New CSVFileCommand
                Dim outputFileName As String = Me.GetOutputFileName
                Dim sfd As SaveFileDialog = command.ShowSaveCSVFileDialog((Regex.Replace(outputFileName, "[\\/:*?\""<>|]", "") & ".csv"))
                If (Not sfd Is Nothing) Then
                    Dim dTblGridOrg As DataTable = DirectCast(Me.flxList.DataSource, DataTable)
                    Dim dTblGrid As DataTable = dTblGridOrg.Copy
                    Dim str2 As String
                    For Each str2 In Me.GetNotOutputFileColumns
                        If dTblGrid.Columns.Contains(str2) Then
                            dTblGrid.Columns.Remove(str2)
                        End If
                    Next
                    Me.ChangeColumnOrder(dTblGrid)
                    command.WriteCSVFile(sfd, dTblGrid, False)
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
                'Throw exception2
                MsgBox(exception2.ToString, vbExclamation)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ''' <summary>
        ''' 明細印刷ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnPrintDetails_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintDetails.Click
            Try
                Me.PrintDetailBySelected()
            Catch exception As AppUnionException
                Me.Cursor = Cursors.Default
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                Me.Cursor = Cursors.Default
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString, vbExclamation)
            End Try
        End Sub

        ''' <summary>
        ''' 一覧プレ印刷ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnPrintList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintList.Click
            Try
                Cursor.Current = Cursors.WaitCursor
                Me.PrintList()
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
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub

        ''' <summary>
        ''' 表示ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShow.Click
            If ((Me.ScreenMode <> MODE.EDIT) OrElse (CLMsg.Show("GQ0007") <> DialogResult.No)) Then
                Me._branch = Me.cmbBelonging.SelectedIndex
                'Me.Query(Me.TargetYear, Me.TargetMonth, Me.UnionBranch)
                Me.Query(Me.TargetYear, Me.TargetMonth, Me.cmbBelonging.SelectedValue)
                Me.ScreenMode = MODE.REFER
            End If
        End Sub

        Protected Overridable Sub CalcTotal(ByVal isError As Boolean)
        End Sub

        Protected Overridable Sub ChangeColumnOrder(ByRef dTblGrid As DataTable)
        End Sub

        Private Sub cmbBelonging_SelectionChangeCommitted(ByVal sender As Object, ByVal e As EventArgs) Handles cmbBelonging.SelectionChangeCommitted
            Try
                If (Not Me.flxList.DataSource Is Nothing) Then
                    DirectCast(Me.flxList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.SetEnabledProperty(False, New Control() {Me.btnOutputFile, Me.btnPrintDetails, Me.btnPrintList})
                Me.ResetTotalLabels()
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Function CreateBusinessObject(ByVal ParamArray args As Object()) As WithholdingCommand
            'Return AbstractGui.GetBusinessObject(Of IWithholdingCommand)("Business.FinancialAffairs.WithHolding.WithholdingCommand", args)
            If args.Length = 0 Then
                Return New WithholdingCommand()
            Else
                Return New WithholdingCommand(args(0))
            End If
        End Function

        Protected Function CreateSumUpReportHeader(ByVal TargetYear As String) As DataTable
            Dim table As New DataTable("dtHeader")
            table.Columns.Add("year", GetType(String))
            Dim row As DataRow = table.NewRow
            row.Item("year") = TargetYear
            table.Rows.Add(row)
            Return table
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub FireCancel(ByVal sender As Object, ByVal e As EventArgs)
            If (Not Me._cancel Is Nothing) Then
                Me._cancel.Invoke(Me, EventArgs.Empty)
            End If
        End Sub

        Private Sub flxList_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles flxList.MouseMove
            Me.Cursor = If(Me.IsOnHyperLinkCell(e), Cursors.Hand, Cursors.Default)
        End Sub

        Private Sub flxList_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles flxList.MouseUp
            If Me.IsOnHyperLinkCell(e) Then
                If Not Me.HasPrintPower Then
                    CLMsg.Show("GE0034")
                Else
                    Me.Cursor = Cursors.WaitCursor
                    Dim info As HitTestInfo = Me.flxList.HitTest(e.X, e.Y)
                    Me.PreviewDetail(info.Row)
                    Me.Cursor = Cursors.Default
                End If
            End If
        End Sub

        Protected Overridable Function GetBusObj() As WithholdingCommand
            Return Me.CreateBusinessObject(New Object(0 - 1) {})
        End Function

        Protected Function GetMoneyValue(Of T)(ByVal row As Integer, ByVal col As Integer) As T
            If Not MDFinanceCommon.IsEmptyCell(Me.flxList, row, col) Then
                Return DirectCast(Me.flxList.Item(row, col), T)
            End If
            Return CType(Nothing, T)
        End Function

        Protected Overridable Function GetNotOutputFileColumns() As String()
            Return New String() {"　", "ユーザＩＤ"}
        End Function

        Protected Overridable Function GetOutputFileName() As String
            Return Nothing
        End Function

        Protected Function GetSelectedMembers(ByVal CheckCol As Integer, ByVal UserIdCol As Integer) As ArrayList
            Dim list As New ArrayList
            Dim i As Integer
            For i = 1 To Me.flxList.Rows.Count - 1
                If CBool(Me.flxList.Rows.Item(i).Item(CheckCol)) Then
                    list.Add(Me.flxList.Rows.Item(i).Item(UserIdCol).ToString)
                End If
            Next i
            Return list
        End Function

        Protected Overridable Sub InitGrid(ByVal Settings As GridSettingInfo())
            Me.flxList.AllowEditing = True
            Dim i As Integer
            For i = 0 To Me.flxList.Cols.Count - 2 'TODO
                Me.flxList.Cols.Item(i).AllowEditing = Settings(i).AllowEditing
                Me.flxList.Cols.Item(i).Visible = Settings(i).Visible
            Next i
            Me.flxList.AllowAddNew = False
            Me.flxList.AllowDelete = False
            Me.flxList.AllowResizing = AllowResizingEnum.None
            FinancialAffairsUtility.ApplyGridStyle(Me.flxList, Settings)
        End Sub

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CtlWithHoldingDetailBase))
            Me.btnShow = New System.Windows.Forms.Button
            Me.cmbBelonging = New System.Windows.Forms.ComboBox
            Me.lblBelongLocal = New System.Windows.Forms.Label
            Me.lblMonth = New System.Windows.Forms.Label
            Me.lblYear = New System.Windows.Forms.Label
            Me.label6 = New System.Windows.Forms.Label
            Me.label7 = New System.Windows.Forms.Label
            Me.btnPrintDetails = New System.Windows.Forms.Button
            Me.btnPrintList = New System.Windows.Forms.Button
            Me.btnBackOrCancel = New System.Windows.Forms.Button
            Me.flxList = New C1.Win.C1FlexGrid.C1FlexGrid
            Me.btnAllCheckOff = New System.Windows.Forms.Button
            Me.btnAllCheckOn = New System.Windows.Forms.Button
            Me.btnOutputFile = New System.Windows.Forms.Button
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnShow
            '
            Me.btnShow.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.btnShow.Location = New System.Drawing.Point(345, 15)
            Me.btnShow.Margin = New System.Windows.Forms.Padding(4)
            Me.btnShow.Name = "btnShow"
            Me.btnShow.Size = New System.Drawing.Size(57, 26)
            Me.btnShow.TabIndex = 6
            Me.btnShow.Text = "表示"
            Me.btnShow.UseVisualStyleBackColor = True
            '
            'cmbBelonging
            '
            Me.cmbBelonging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbBelonging.FormattingEnabled = True
            Me.cmbBelonging.Location = New System.Drawing.Point(253, 15)
            Me.cmbBelonging.Margin = New System.Windows.Forms.Padding(4)
            Me.cmbBelonging.Name = "cmbBelonging"
            Me.cmbBelonging.Size = New System.Drawing.Size(83, 24)
            Me.cmbBelonging.TabIndex = 5
            Me.cmbBelonging.Tag = ""
            '
            'lblBelongLocal
            '
            Me.lblBelongLocal.AutoSize = True
            Me.lblBelongLocal.Location = New System.Drawing.Point(210, 20)
            Me.lblBelongLocal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.lblBelongLocal.Name = "lblBelongLocal"
            Me.lblBelongLocal.Size = New System.Drawing.Size(40, 16)
            Me.lblBelongLocal.TabIndex = 4
            Me.lblBelongLocal.Text = "支部"
            '
            'lblMonth
            '
            Me.lblMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblMonth.Location = New System.Drawing.Point(117, 15)
            Me.lblMonth.Name = "lblMonth"
            Me.lblMonth.Size = New System.Drawing.Size(32, 23)
            Me.lblMonth.TabIndex = 2
            Me.lblMonth.Text = "99"
            Me.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblYear
            '
            Me.lblYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblYear.Location = New System.Drawing.Point(33, 15)
            Me.lblYear.Name = "lblYear"
            Me.lblYear.Size = New System.Drawing.Size(52, 23)
            Me.lblYear.TabIndex = 0
            Me.lblYear.Text = "9999"
            Me.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label6
            '
            Me.label6.AutoSize = True
            Me.label6.Location = New System.Drawing.Point(87, 19)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(24, 16)
            Me.label6.TabIndex = 1
            Me.label6.Text = "年"
            '
            'label7
            '
            Me.label7.AutoSize = True
            Me.label7.Location = New System.Drawing.Point(152, 20)
            Me.label7.Name = "label7"
            Me.label7.Size = New System.Drawing.Size(40, 16)
            Me.label7.TabIndex = 3
            Me.label7.Text = "月分"
            '
            'btnPrintDetails
            '
            Me.btnPrintDetails.Location = New System.Drawing.Point(367, 717)
            Me.btnPrintDetails.Name = "btnPrintDetails"
            Me.btnPrintDetails.Size = New System.Drawing.Size(116, 32)
            Me.btnPrintDetails.TabIndex = 11
            Me.btnPrintDetails.Text = "明細印刷"
            Me.btnPrintDetails.UseVisualStyleBackColor = True
            '
            'btnPrintList
            '
            Me.btnPrintList.Location = New System.Drawing.Point(543, 717)
            Me.btnPrintList.Name = "btnPrintList"
            Me.btnPrintList.Size = New System.Drawing.Size(116, 32)
            Me.btnPrintList.TabIndex = 12
            Me.btnPrintList.Text = "一覧プレ印刷"
            Me.btnPrintList.UseVisualStyleBackColor = True
            '
            'btnBackOrCancel
            '
            Me.btnBackOrCancel.Location = New System.Drawing.Point(719, 717)
            Me.btnBackOrCancel.Name = "btnBackOrCancel"
            Me.btnBackOrCancel.Size = New System.Drawing.Size(116, 32)
            Me.btnBackOrCancel.TabIndex = 13
            Me.btnBackOrCancel.Text = "ｷｬﾝｾﾙ"
            Me.btnBackOrCancel.UseVisualStyleBackColor = True
            '
            'flxList
            '
            Me.flxList.AllowAddNew = True
            Me.flxList.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
            Me.flxList.AllowEditing = False
            Me.flxList.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            Me.flxList.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:true;}" & Global.Microsoft.VisualBasic.ChrW(9)
            Me.flxList.Location = New System.Drawing.Point(11, 59)
            Me.flxList.Name = "flxList"
            Me.flxList.Rows.Count = 1
            Me.flxList.Rows.DefaultSize = 22
            Me.flxList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.flxList.Size = New System.Drawing.Size(1004, 611)
            Me.flxList.StyleInfo = resources.GetString("flxList.StyleInfo")
            Me.flxList.TabIndex = 7
            '
            'btnAllCheckOff
            '
            Me.btnAllCheckOff.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.btnAllCheckOff.Location = New System.Drawing.Point(10, 702)
            Me.btnAllCheckOff.Name = "btnAllCheckOff"
            Me.btnAllCheckOff.Size = New System.Drawing.Size(26, 25)
            Me.btnAllCheckOff.TabIndex = 9
            Me.btnAllCheckOff.Text = "□"
            Me.btnAllCheckOff.UseVisualStyleBackColor = True
            '
            'btnAllCheckOn
            '
            Me.btnAllCheckOn.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.btnAllCheckOn.Location = New System.Drawing.Point(10, 674)
            Me.btnAllCheckOn.Name = "btnAllCheckOn"
            Me.btnAllCheckOn.Size = New System.Drawing.Size(26, 25)
            Me.btnAllCheckOn.TabIndex = 8
            Me.btnAllCheckOn.Text = ChrW(9745)
            Me.btnAllCheckOn.UseVisualStyleBackColor = True
            '
            'btnOutputFile
            '
            Me.btnOutputFile.Location = New System.Drawing.Point(196, 717)
            Me.btnOutputFile.Name = "btnOutputFile"
            Me.btnOutputFile.Size = New System.Drawing.Size(116, 32)
            Me.btnOutputFile.TabIndex = 10
            Me.btnOutputFile.Text = "ファイル出力"
            Me.btnOutputFile.UseVisualStyleBackColor = True
            '
            'CtlWithHoldingDetailBase
            '
            Me.Controls.Add(Me.btnOutputFile)
            Me.Controls.Add(Me.btnAllCheckOff)
            Me.Controls.Add(Me.btnAllCheckOn)
            Me.Controls.Add(Me.flxList)
            Me.Controls.Add(Me.btnPrintDetails)
            Me.Controls.Add(Me.btnPrintList)
            Me.Controls.Add(Me.btnBackOrCancel)
            Me.Controls.Add(Me.btnShow)
            Me.Controls.Add(Me.cmbBelonging)
            Me.Controls.Add(Me.lblBelongLocal)
            Me.Controls.Add(Me.lblMonth)
            Me.Controls.Add(Me.lblYear)
            Me.Controls.Add(Me.label6)
            Me.Controls.Add(Me.label7)
            Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.Name = "CtlWithHoldingDetailBase"
            Me.Size = New System.Drawing.Size(1026, 759)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Protected Function IsOnHyperLinkCell(ByVal e As MouseEventArgs) As Boolean
            Dim info As HitTestInfo = Me.flxList.HitTest(e.X, e.Y)
            Dim flag As Boolean = False
            If (info.Type = HitTestTypeEnum.Cell) Then
                Dim style As CellStyle = Me.flxList.Cols.Item(info.Column).Style
                If (((style Is Nothing) OrElse (style.Name Is Nothing)) OrElse Not style.Name.Equals("employee_number_col_link")) Then
                    Return flag
                End If
                Using graphics As Graphics = Me.flxList.CreateGraphics
                    Dim rectangle As Rectangle = Me.flxList.GetCellRect(info.Row, info.Column, False)
                    Dim width As Integer = CInt(graphics.MeasureString(Me.flxList.Item(info.Row, info.Column).ToString, style.Font).Width)
                    If (((style.TextAlign = TextAlignEnum.RightCenter) OrElse (style.TextAlign = TextAlignEnum.RightBottom)) OrElse (style.TextAlign = TextAlignEnum.RightTop)) Then
                        If ((rectangle.Right - e.X) <= width) Then
                            flag = True
                        End If
                        Return flag
                    End If
                    If (((style.TextAlign <> TextAlignEnum.LeftCenter) AndAlso (style.TextAlign <> TextAlignEnum.LeftBottom)) AndAlso (style.TextAlign <> TextAlignEnum.LeftTop)) Then
                        Return flag
                    End If
                    If ((e.X - rectangle.Left) <= width) Then
                        flag = True
                    End If
                End Using
            End If
            Return flag
        End Function

        Protected Overridable Sub PreviewDetail(ByVal row As Integer)
        End Sub

        Protected Overridable Sub PreviewDetail2(ByVal row As Integer)
        End Sub

        Protected Overridable Sub PrintDetail(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean)
            Try
                'Me.PrintDetail(SelectedMembers, Preview, "Report.Withholding.RptWageCutCoverNews_cut_monthly")
                Me.PrintDetailPreview(SelectedMembers, Preview, New CR0503P3)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        ' 月例　課税対象者・課税非対象者
        Protected Overridable Sub PrintDetail2(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean)
            Try
                'Me.PrintDetail(SelectedMembers, Preview, "Report.Withholding.RptWageCutCoverNews_cut_monthly")
                'Me.PrintDetail(SelectedMembers, Preview, New CR0503P3)
                Me.PrintDetailPreview2(SelectedMembers, Preview)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Sub PrintDetailPreview(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean, ByVal clsReportName As ReportClass)
            If Not Preview Then
                If (SelectedMembers.Count <= 0) Then
                    CLMsg.Show("GE0065")
                    Return
                End If
                If (CLMsg.Show("GQ0017") = DialogResult.No) Then
                    Return
                End If
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim dSet As DataSet = Me._business.GetMonthlyReportDetailData(Me.TargetYear, Me.TargetMonth, SelectedMembers)
                Me.Cursor = Cursors.Default
                Dim viewer As New ReportViewer(dSet, clsReportName)
                If Preview Then
                    viewer.ReportViewerShow()
                Else
                    viewer.PrintOut()
                End If
                'viewer.RptDataDispose()
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

        Protected Sub PrintDetailPreview2(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean)
            Dim clsReportName As ReportClass = Nothing
            If Not Preview Then
                If (SelectedMembers.Count <= 0) Then
                    CLMsg.Show("GE0065")
                    Return
                End If
                If (CLMsg.Show("GQ0017") = DialogResult.No) Then
                    Return
                End If
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim dSet As DataSet = Me._business.GetMonthlyReportDetailData(Me.TargetYear, Me.TargetMonth, SelectedMembers)

                ' 課税フラグ判定
                If dSet.Tables(0).Rows(0).Item("c_taxation_flag").ToString = "0" Then
                    ' 課税フラグが 0 の場合、「(今月分は、賃金カットを除く役員手当のみ源泉徴収しております)」ラベルの帳票
                    clsReportName = New CR0503P3_2
                Else
                    ' 課税フラグが 1 の場合、「(今月分は、先月まで役員だった為、賃金カットのみ課税対象となっています)」ラベルの帳票
                    If CLng(dSet.Tables(0).Rows(0).Item("s_officer_pay")) = 0 _
                    And CLng(dSet.Tables(0).Rows(0).Item("s_cut_monthly_taxation")) + CLng(dSet.Tables(0).Rows(0).Item("s_cut_once_taxation")) > 0 Then
                        '  役員手当が 0円で、課税対象額（月例）か課税対象額（一時金）が1円以上
                        clsReportName = New CR0503P3_3
                    Else
                        ' 通常、「(役員手当が支払われていない方は　源泉徴収しておりません)」ラベルの帳票
                        clsReportName = New CR0503P3
                    End If
                End If

                Me.Cursor = Cursors.Default
                Dim viewer As New ReportViewer(dSet, clsReportName)
                If Preview Then
                    viewer.ReportViewerShow()
                Else
                    viewer.PrintOut()
                End If
                'viewer.RptDataDispose()
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

        Protected Overridable Sub PrintDetailBySelected()
        End Sub

        Protected Overridable Sub PrintList()
        End Sub

        Protected Overridable Sub Query(ByVal TargetYear As String, ByVal UnionBranch As String)
        End Sub

        Protected Overridable Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String)
        End Sub

        Protected Overridable Sub ResetTotalLabels()
        End Sub

        Protected Function SelectUnVisibleColumns(ByVal flxGrid As C1FlexGrid) As List(Of String)
            Dim list As New List(Of String)
            Dim column As Column
            For Each column In DirectCast(flxGrid.Cols, IEnumerable)
                If (Not column.Visible OrElse (column.Width = 0)) Then
                    list.Add(column.Name)
                End If
            Next
            Return list
        End Function

        Private Sub SetComboItems(ByVal TargetYear As String, ByVal TargetMonth As String)
            Dim masterObj As New InfoConstant(CommonUtility.GetLastDay(TargetYear, TargetMonth))
            'TODO Me.cmbBelonging.SetItems(masterObj)
            Dim clsMdb As New CLAccessMdb
            clsMdb.Connect()
            NSMDCommon.CreateCboConstantDtl(clsMdb, Me.cmbBelonging, "BELONGING", False)
            clsMdb.Disconnect()
            If (Me.cmbBelonging.Items.Count > 0) Then
                Me.cmbBelonging.SelectedIndex = 0
            End If
        End Sub

        ''' <summary>
        ''' 編集モード（内容変更ボタン押下後）設定
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetEditMode()

            ' グリッド
            Me.flxList.SuspendLayout()                  ' レイアウトロジックロック
            Me.InitGrid(If((Me._settingInEdit Is Nothing), Me._settingInRef, Me._settingInEdit))
            Me.flxList.ResumeLayout()                   ' レイアウトロジック解除

            Me.cmbBelonging.SelectedIndex = Me._branch
            Me.cmbBelonging.Enabled = False             ' 支部コンボボックス非活性
            Me.btnShow.Enabled = False                  ' 表示ボタン非活性
            Me.btnAllCheckOn.Enabled = False            ' オールチェックONボタン非活性
            Me.btnAllCheckOff.Enabled = False           ' オールチェックOFFボタン非活性
            Me.btnPrintDetails.Enabled = False          ' 明細印刷ボタン非活性
            Me.btnPrintList.Enabled = False             ' 一覧プレ印刷ボタン非活性
            Me.btnOutputFile.Enabled = False            ' ファイル出力ボタン非活性
            Me.btnBackOrCancel.Text = "キャンセル"
        End Sub

        ''' <summary>
        ''' 編集モード（非課税チェックボタン押下後）設定
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetEditTaxationMode()

            ' グリッド
            Me.flxList.SuspendLayout()                  ' レイアウトロジックロック
            Me.flxList.Cols(0).AllowEditing = True      ' チェックボックス使用可
            For i = 1 To Me.flxList.Rows.Count - 1
                Me.flxList.Item(i, 0) = False           ' すべてチェックを外す
            Next i
            'Me.InitGrid(If((Me._settingInEdit Is Nothing), Me._settingInRef, Me._settingInEdit))
            Me.flxList.ResumeLayout()                   ' レイアウトロジック解除

            Me.cmbBelonging.SelectedIndex = Me._branch
            Me.cmbBelonging.Enabled = False             ' 支部コンボボックス非活性
            Me.btnShow.Enabled = False                  ' 表示ボタン非活性
            Me.btnAllCheckOn.Enabled = False            ' オールチェックONボタン非活性
            Me.btnAllCheckOff.Enabled = False           ' オールチェックOFFボタン非活性
            Me.btnPrintDetails.Enabled = False          ' 明細印刷ボタン非活性
            Me.btnPrintList.Enabled = False             ' 一覧プレ印刷ボタン非活性
            Me.btnOutputFile.Enabled = False            ' ファイル出力ボタン非活性
            Me.btnBackOrCancel.Text = "キャンセル"
        End Sub

        ''' <summary>
        ''' 参照モード設定
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SetReferMode()
            Me.flxList.SuspendLayout()
            Dim fEnable As Boolean
            If (Not Me._original Is Nothing) Then
                Dim result As DataTable = Me._original.Copy
                Me.ShowData(result)
                Me.CalcTotal(False)
                Me.InitGrid(Me._settingInRef)
                fEnable = (result.Rows.Count > 0)
            End If
            Me.flxList.ResumeLayout()
            Me.cmbBelonging.SelectedIndex = Me._branch
            Me.cmbBelonging.Enabled = True                      ' 支部コンボボックス活性
            Me.btnShow.Enabled = True                           ' 表示ボタン活性
            Me.btnAllCheckOn.Enabled = Me.HasPrintPower
            Me.btnAllCheckOff.Enabled = Me.HasPrintPower
            If (fEnable) Then
                Me.btnPrintDetails.Enabled = Me.HasPrintPower
                Me.btnPrintList.Enabled = Me.HasPrintPower
                Me.btnOutputFile.Enabled = Me.HasOutputPower
            Else
                Me.btnPrintDetails.Enabled = False              ' 明細印刷ボタン非活性
                Me.btnPrintList.Enabled = False                 ' 一覧プレ印刷ボタン非活性
                Me.btnOutputFile.Enabled = False                ' ファイル出力非活性
            End If
            Me.btnBackOrCancel.Text = "戻る"
        End Sub

        Private Sub SetRowCheck(ByVal Checked As Boolean)
            Me.flxList.SuspendLayout()
            Dim column As Column
            For Each column In DirectCast(Me.flxList.Cols, IEnumerable)
                If column.Style.DataType.Equals(GetType(Boolean)) Then
                    Dim i As Integer
                    For i = 1 To Me.flxList.Rows.Count - 1
                        Me.flxList.Rows.Item(i).Item(column.Index) = Checked
                    Next i
                End If
            Next
            Me.flxList.ResumeLayout()
        End Sub

        Private Sub ShowData(ByVal result As DataTable)
            Me.flxList.DataSource = result
        End Sub

        Private Sub WithHoldingDetailBase_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Try
                If (((Not Me.TargetYear Is Nothing) AndAlso (Me.TargetYear.Length <> 0)) AndAlso ((Not Me.TargetMonth Is Nothing) AndAlso (Me.TargetMonth.Length <> 0))) Then
                    Me.Query(Me.TargetYear, Me.TargetMonth, Me.UnionBranch)
                    Me.ScreenMode = MODE.REFER
                End If
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub


        ' Properties
        Protected ReadOnly Property HasEntryPower() As Boolean
            Get
                Return MDFinanceCommon.GetEntryPower(Me._strNameForRight)
            End Get
        End Property

        Protected ReadOnly Property HasOutputPower() As Boolean
            Get
                Return MDFinanceCommon.GetOutputPower(Me._strNameForRight)
            End Get
        End Property

        Protected ReadOnly Property HasPrintPower() As Boolean
            Get
                Return MDFinanceCommon.GetPrintPower(Me._strNameForRight)
            End Get
        End Property

        Protected Property ScreenMode() As Integer
            Get
                Return Me._mode
            End Get
            Set(ByVal value As Integer)
                Me._mode = value
                Select Case Me._mode
                    Case MODE.EDIT
                        Me.SetEditMode()
                        Return
                    Case MODE.REFER
                        Me.SetReferMode()
                        Return
                    Case MODE.EDIT_TAXATION
                        Me.SetEditTaxationMode()
                End Select
            End Set
        End Property

        Protected Property TargetMonth() As String
            Get
                Return Me._month
            End Get
            Set(ByVal value As String)
                Me._month = If((value Is Nothing), "", value)
                Me.lblMonth.Text = Me._month
            End Set
        End Property

        Protected Property TargetYear() As String
            Get
                Return Me._year
            End Get
            Set(ByVal value As String)
                Me._year = value
                Me.lblYear.Text = Me._year
            End Set
        End Property

        Protected ReadOnly Property UnionBranch() As String
            Get
                'Return Me.cmbBelonging.GetSelectedItem("c_constant_seq")
                Return Me.cmbBelonging.SelectedValue
            End Get
        End Property


        ' Fields
        Protected _branch As Integer
        Protected _business As WithholdingCommand
        Private _logger As ILog
        Private _mode As MODE
        Private _month As String
        Protected _original As DataTable
        Protected _settingInEdit As GridSettingInfo()
        Protected _settingInRef As GridSettingInfo()
        Private _strNameForRight As String
        Private _year As String
        Protected WithEvents btnAllCheckOff As Button
        Protected WithEvents btnAllCheckOn As Button
        Protected WithEvents btnBackOrCancel As Button
        Public WithEvents btnOutputFile As Button
        Protected WithEvents btnPrintDetails As Button
        Protected WithEvents btnPrintList As Button
        Protected WithEvents btnShow As Button
        Protected WithEvents cmbBelonging As ComboBox
        Private components As IContainer
        Protected WithEvents flxList As C1FlexGrid
        Protected label6 As Label
        Protected label7 As Label
        Protected lblBelongLocal As Label
        Protected lblMonth As Label
        Protected lblYear As Label

        ' Nested Types
        Protected Class LocalStyle
            ' Fields
            Public Const STYLE_CHECK_COL As String = "check_col"
            Public Const STYLE_DIRECTORS_REMUNERATION As String = "directors_remuneration_col"
        End Class

        Protected Delegate Sub SingleParameterDelegate(Of T)(ByVal param As T)
    End Class
End Namespace
