'Imports UnionAct.GUI.FinancialAffairs
'Imports log4net
'Imports System
'Imports System.ComponentModel
'Imports System.Drawing
'Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs.WithHolding
'Imports CrystalDecisions.CrystalReports.Engine

Imports C1.Win.C1FlexGrid
'Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
'Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
'Imports UnionAct.MDMode
Imports log4net
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlMonthlyNonTaxableDetail
        Inherits CtlCutDivNonTaxableDetail
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
            MyBase.SetValidator(New ValidateDelegate(AddressOf Me.ValidateRows))
        End Sub

        ''' <summary>
        ''' 内容変更ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChange.Click

            Me.CalcTotal(False)

            ' モード変更処理
            MyBase.ScreenMode = MODE.EDIT

            ' チェックサポート処理（前月（7月）に役員から外れた中執のメンバーにチェックを付ける）
            Call LostUserCheckMain()

            ' 情報メッセージ表示
            Call CLMsg.Show("GI0045")

        End Sub

        ''' <summary>
        ''' 登録ボタン押下処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnRegist_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegist.Click

            Dim num As Integer = 0              ' チェック件数

            Try
                ' 課税非対象者リスト件数分ループ
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
                    ' 情報メッセージボックス表示
                    If (CLMsg.Show("GQ0067", "対象年月", MyBase.TargetYear, MyBase.TargetMonth) <> DialogResult.No) Then
                        ' 更新処理
                        Call Me.UpdateData()
                        ' 課税非対象者リスト再取得
                        Me.Query(MyBase.TargetYear, MyBase.TargetMonth, MyBase.cmbBelonging.SelectedValue)
                        ' 参照モード
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

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Dim nMonthly As Long
            If isError Then
                MyBase.CalcTotal(isError)
                Me.lblSumMonthly.Text = "#VALUE!"
            Else
                nMonthly = 0
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    nMonthly = (nMonthly + MyBase.GetMoneyValue(Of Long)(i, COLIDX.MONTHLY_DEDUCTION))
                Next i
                MyBase.CalcTotal(isError)
                Me.lblSumMonthly.Text = nMonthly.ToString("###,###,##0")
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Function GetBusObj() As WithholdingCommand
            Return MyBase.CreateBusinessObject(New Object() {"05"})
        End Function

        Protected Overrides Function GetOutputFileName() As String
            Return String.Concat(New String() {"月例賃金" & " - " & "課税非対象者", MyBase.lblYear.Text, "年", MyBase.lblMonth.Text, "月分" & " ", MyBase.cmbBelonging.Text})
        End Function

        Private Sub InitializeComponent()
            Me.lblSumMonthly = New System.Windows.Forms.Label
            Me.btnChange = New System.Windows.Forms.Button
            Me.btnRegist = New System.Windows.Forms.Button
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblSumPayOut
            '
            Me.lblSumPayOut.Location = New System.Drawing.Point(702, 668)
            '
            'lblSumTruncate
            '
            Me.lblSumTruncate.Location = New System.Drawing.Point(626, 668)
            '
            'btnAllCheckOff
            '
            Me.btnAllCheckOff.Location = New System.Drawing.Point(238, 671)
            '
            'btnAllCheckOn
            '
            Me.btnAllCheckOn.Location = New System.Drawing.Point(206, 671)
            '
            'btnBackOrCancel
            '
            Me.btnBackOrCancel.TabIndex = 16
            '
            'btnOutputFile
            '
            Me.btnOutputFile.Location = New System.Drawing.Point(199, 717)
            '
            'btnPrintDetails
            '
            Me.btnPrintDetails.Location = New System.Drawing.Point(459, 717)
            Me.btnPrintDetails.TabIndex = 14
            '
            'btnPrintList
            '
            Me.btnPrintList.Location = New System.Drawing.Point(589, 717)
            Me.btnPrintList.TabIndex = 15
            '
            'btnShow
            '
            Me.btnShow.Location = New System.Drawing.Point(538, 16)
            '
            'cmbBelonging
            '
            Me.cmbBelonging.Location = New System.Drawing.Point(446, 16)
            '
            'flxList
            '
            Me.flxList.Location = New System.Drawing.Point(206, 55)
            Me.flxList.Rows.Count = 1
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(615, 610)
            '
            'label6
            '
            Me.label6.Location = New System.Drawing.Point(280, 20)
            '
            'label7
            '
            Me.label7.Location = New System.Drawing.Point(345, 21)
            '
            'lblBelongLocal
            '
            Me.lblBelongLocal.Location = New System.Drawing.Point(403, 21)
            '
            'lblMonth
            '
            Me.lblMonth.Location = New System.Drawing.Point(310, 16)
            '
            'lblYear
            '
            Me.lblYear.Location = New System.Drawing.Point(226, 16)
            '
            'lblSumMonthly
            '
            Me.lblSumMonthly.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumMonthly.Location = New System.Drawing.Point(526, 668)
            Me.lblSumMonthly.Name = "lblSumMonthly"
            Me.lblSumMonthly.Size = New System.Drawing.Size(99, 23)
            Me.lblSumMonthly.TabIndex = 10
            Me.lblSumMonthly.Text = "999,999,999"
            Me.lblSumMonthly.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'btnChange
            '
            Me.btnChange.Location = New System.Drawing.Point(328, 717)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New System.Drawing.Size(116, 32)
            Me.btnChange.TabIndex = 19
            Me.btnChange.Text = "内容変更"
            Me.btnChange.UseVisualStyleBackColor = True
            '
            'btnRegist
            '
            Me.btnRegist.Location = New System.Drawing.Point(329, 717)
            Me.btnRegist.Name = "btnRegist"
            Me.btnRegist.Size = New System.Drawing.Size(116, 32)
            Me.btnRegist.TabIndex = 18
            Me.btnRegist.Text = "登録"
            Me.btnRegist.UseVisualStyleBackColor = True
            '
            'CtlMonthlyNonTaxableDetail
            '
            Me.Controls.Add(Me.btnChange)
            Me.Controls.Add(Me.btnRegist)
            Me.Controls.Add(Me.lblSumMonthly)
            Me.Margin = New System.Windows.Forms.Padding(4)
            Me.Name = "CtlMonthlyNonTaxableDetail"
            Me.Controls.SetChildIndex(Me.lblSumMonthly, 0)
            Me.Controls.SetChildIndex(Me.lblSumTruncate, 0)
            Me.Controls.SetChildIndex(Me.lblSumPayOut, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOff, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOn, 0)
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)
            Me.Controls.SetChildIndex(Me.label7, 0)
            Me.Controls.SetChildIndex(Me.label6, 0)
            Me.Controls.SetChildIndex(Me.lblYear, 0)
            Me.Controls.SetChildIndex(Me.lblMonth, 0)
            Me.Controls.SetChildIndex(Me.lblBelongLocal, 0)
            Me.Controls.SetChildIndex(Me.cmbBelonging, 0)
            Me.Controls.SetChildIndex(Me.btnShow, 0)
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)
            Me.Controls.SetChildIndex(Me.btnPrintList, 0)
            Me.Controls.SetChildIndex(Me.btnPrintDetails, 0)
            Me.Controls.SetChildIndex(Me.flxList, 0)
            Me.Controls.SetChildIndex(Me.btnRegist, 0)
            Me.Controls.SetChildIndex(Me.btnChange, 0)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Protected Overrides Sub ResetTotalLabels()
            MyBase.ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumMonthly})
        End Sub

        ''' <summary>
        ''' 変更モード
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub SetEditMode()
            MyBase.SetEditMode()                                        ' コントロール変更モード
            MyBase.flxList.AllowSorting = AllowSortingEnum.None         ' ソート不可
            Me.btnRegist.Visible = True                                 ' 内容変更ボタン表示
            Me.btnChange.Visible = False                                ' 登録ボタン非表示
            Me.btnAllCheckOn.Enabled = True                             ' オールチェックボタン押下可
            Me.btnAllCheckOff.Enabled = True                            ' オール未チェックボタン押下可
        End Sub

        ''' <summary>
        ''' 参照モード
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub SetReferMode()
            MyBase.SetReferMode()                                       ' コントロール参照モード
            MyBase.flxList.AllowSorting = AllowSortingEnum.SingleColumn ' ソート可
            Me.btnRegist.Visible = False                                ' 内容変更ボタン非表示
            Me.btnChange.Visible = True                                 ' 登録ボタン非表示
            Me.btnAllCheckOn.Enabled = True                             ' オールチェックボタン押下可
            Me.btnAllCheckOff.Enabled = True                            ' オール未チェックボタン押下可
        End Sub

        ''' <summary>
        ''' 更新処理
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub UpdateData()

            ' データ配列作成
            Dim dt As New DataTable
            dt.Columns.Add("userId", GetType(String))           ' 社員番号
            dt.Columns.Add("tax", GetType(Long))                ' 課税対象額（月例）
            dt.Columns.Add("taxationFlag", GetType(String))     ' 課税フラグ

            ' 課税非対象者リスト件数分ループ
            For i = 1 To MyBase.flxList.Rows.Count - 1
                ' チェックボックスにチェックが付いているかチェック
                If MyBase.flxList.Item(i, COLIDX.CHECK) Then
                    ' 社員番号取得
                    Dim strUserId As String = MyBase.flxList.Item(i, COLIDX.USER_ID).ToString
                    ' 月例控除額取得
                    Dim monthlyDedution As Long = MyBase.flxList.Item(i, COLIDX.MONTHLY_DEDUCTION)
                    ' 月例控除額を元に源泉徴収額を課税対象額（月例）として取得
                    Dim tax As Long = MyBase._business.CalcWithholding(MyBase.TargetYear, MyBase.TargetMonth, monthlyDedution)
                    ' 社員番号と課税対象額（月例）と課税フラグを対象分、配列に取得
                    dt.Rows.Add(strUserId, tax, "1")
                End If
            Next i

            ' 更新処理
            Try
                MyBase._business.UpdateDataTaxation(MyBase.TargetYear, MyBase.TargetMonth, MDLoginInfo.UserId, dt)
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
                cellStyle = MyBase.flxList.GetCellStyle(i, COLIDX.MONTHLY_DEDUCTION)
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
                'Return "Report.Withholding.RptTaxNoIntendedPayTable_local_cut_monthly"
                Return New CR0503PD
            End Get
        End Property

        ''' <summary>
        ''' チェックサポートメイン処理（前月（7月）に役員から外れた中執のメンバーにチェックを付ける）
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LostUserCheckMain()

            ' 課税非対象者リスト件数分ループ
            For i = 1 To MyBase.flxList.Rows.Count - 1
                ' チェックボックスのチェックを未チェックにする
                MyBase.flxList.Item(i, COLIDX.CHECK) = False
            Next i

            ' 対象年月が8月かチェック
            If MyBase.TargetMonth = "08" Then
                ' 前月（7月）に役員から外れた中執のメンバーにチェックを付ける
                Call Me.LostUserCheck(MyBase.TargetYear, MDLoginInfo.PeriodId)
            End If

        End Sub

        ''' <summary>
        ''' チェックサポート処理（前月（7月）に役員から外れた中執のメンバーにチェックを付ける）
        ''' </summary>
        ''' <param name="iTargetYear"></param>
        ''' <param name="iPeriodId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function LostUserCheck( _
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
                '   前月（7月）の中執役員メンバを取得
                '---------------------------------------------------------------------------
                ' 委員会名簿IDから社員番号を取得
                strSql += "SELECT com_list_dtl.c_user_id" & vbCrLf
                strSql += "  FROM committee_list_dtl AS com_list_dtl" & vbCrLf
                strSql += "      ,(" & vbCrLf
                '                 委員会IDから委員会名簿IDと委員会IDと適用開始年月日を取得
                strSql += "       SELECT t1.c_committee_list" & vbCrLf
                strSql += "             ,t1.c_committee_id" & vbCrLf
                strSql += "             ,t1.d_from" & vbCrLf
                strSql += "         FROM committee_list AS t1" & vbCrLf
                '                         対象年月・委員会ID・期・会社コードから委員会IDを取得
                strSql += "             ,(SELECT c_committee_id" & vbCrLf
                strSql += "                     ,MAX(d_from) AS now_from" & vbCrLf
                strSql += "                 FROM committee_list" & vbCrLf
                strSql += "                WHERE c_committee_id = '001'" & vbCrLf
                strSql += "                  AND left(d_from, 6) <= '" & iTargetYear & "07'" & vbCrLf
                strSql += "                  AND c_period_id = (" & vbCrLf
                strSql += "                      SELECT c_period_id" & vbCrLf
                strSql += "                        FROM period" & vbCrLf
                strSql += "                       WHERE '" & iTargetYear & "0701" & "' BETWEEN d_from AND d_to" & vbCrLf
                strSql += "                      )" & vbCrLf
                strSql += "                GROUP BY c_committee_id" & vbCrLf
                strSql += "              ) AS t2" & vbCrLf
                strSql += "        WHERE t1.c_committee_id = t2.c_committee_id" & vbCrLf
                strSql += "          AND t1.d_from = t2.now_from" & vbCrLf
                strSql += "       ) AS com_list" & vbCrLf
                strSql += "      ,committee_dtl AS com_dtl"
                strSql += " WHERE com_list.c_committee_list = com_list_dtl.c_committee_list" & vbCrLf
                '             委員会マスタ詳細（役職マスタ）結合
                strSql += "   AND com_list.c_committee_id = com_dtl.c_committee_id"
                strSql += "   AND com_list_dtl.s_committee_seq = com_dtl.s_committee_seq"
                strSql += "   AND com_list.d_from BETWEEN com_dtl.d_from AND d_to"
                '                 役員手当があるメンバ
                strSql += "   AND com_dtl.c_officer_pay_id <> ''"

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
        Private _validator As ValidateDelegate
        Private components As IContainer
        Private WithEvents btnChange As Button
        Private WithEvents btnRegist As Button
        Private lblSumMonthly As Label
        Private _logger As ILog
    End Class
End Namespace
