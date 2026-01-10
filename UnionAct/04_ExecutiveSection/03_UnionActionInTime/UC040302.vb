'===========================================================================================================
'   クラスＩＤ　　：UC040302
'   クラス名称　　：時間内組合活動 - 申請画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLAccessMdbMst
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDChk
Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common

Public Class UC040302
#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040301
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040301
#End Region

#Region "内部プロパティ"
    Private strReplaceNumber As String
    Private minDate As String                       ' 最小日付
    Private errMsgOther As New ArrayList            ' その他の活動エラーリスト
    Private errMsgName As New List(Of String)       ' 指名ストライキエラー
    Private numUseCol As Integer = 5                ' 表示日付件数(初期値5)
    Private blnDateChk As Boolean = False            ' 日付警告用

    Public intAlreadyUser As Integer

    ' 表示日付件数
    Public Property pnumUseCol() As Integer
        Get
            Return numUseCol
        End Get
        Set(ByVal value As Integer)
            numUseCol = value
        End Set
    End Property
#End Region

#Region "ログ出力オブジェクト"
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
#Region "UC040302_Load"
    '***************************************************************************************************
    '   ＩＤ　：UC040302_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2012/01/19(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040302_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' 処理開始ログ
        Try
            Dim cellstyle As C1.Win.C1FlexGrid.CellStyle = flxNameAndDate.Styles.Add("lightyellow")
            cellstyle.BackColor = Color.LightYellow

            flxNameAndDate.Cols.Count = 20
            'flxNameAndDate.AllowEditing = True
            flxNameAndDate.Cols(0).Caption = "社員番号"
            flxNameAndDate.Cols(0).Width = 80
            flxNameAndDate.Cols(1).Caption = "氏名"
            flxNameAndDate.Cols(1).Width = 130
            flxNameAndDate.Cols(1).AllowEditing = False
            flxNameAndDate.Cols(1).Style = cellstyle
            flxNameAndDate.Cols(2).Caption = "所属"
            flxNameAndDate.Cols(2).Width = 50
            flxNameAndDate.Cols(2).AllowEditing = False
            flxNameAndDate.Cols(2).Style = cellstyle
            flxNameAndDate.Cols(3).Caption = "機種"
            flxNameAndDate.Cols(3).Width = 50
            flxNameAndDate.Cols(3).AllowEditing = False
            flxNameAndDate.Cols(3).Style = cellstyle
            flxNameAndDate.Cols(4).Caption = "資格"
            flxNameAndDate.Cols(4).Width = 50
            flxNameAndDate.Cols(4).AllowEditing = False
            flxNameAndDate.Cols(4).Style = cellstyle

            flxNameAndDate.Cols(5).Caption = "日付１"
            flxNameAndDate.Cols(5).Width = 105
            flxNameAndDate.Cols(6).Caption = "日付２"
            flxNameAndDate.Cols(6).Width = 105
            flxNameAndDate.Cols(7).Caption = "日付３"
            flxNameAndDate.Cols(7).Width = 105
            flxNameAndDate.Cols(8).Caption = "日付４"
            flxNameAndDate.Cols(8).Width = 105
            flxNameAndDate.Cols(9).Caption = "日付５"
            flxNameAndDate.Cols(9).Width = 105
            flxNameAndDate.Cols(10).Caption = "日付６"
            flxNameAndDate.Cols(10).Width = 105
            flxNameAndDate.Cols(11).Caption = "日付７"
            flxNameAndDate.Cols(11).Width = 105
            flxNameAndDate.Cols(12).Caption = "日付８"
            flxNameAndDate.Cols(12).Width = 105
            flxNameAndDate.Cols(13).Caption = "日付９"
            flxNameAndDate.Cols(13).Width = 105
            flxNameAndDate.Cols(14).Caption = "日付１０"
            flxNameAndDate.Cols(14).Width = 105
            flxNameAndDate.Cols(15).Caption = "日付１１"
            flxNameAndDate.Cols(15).Width = 105
            flxNameAndDate.Cols(16).Caption = "日付１２"
            flxNameAndDate.Cols(16).Width = 105
            flxNameAndDate.Cols(17).Caption = "日付１３"
            flxNameAndDate.Cols(17).Width = 105
            flxNameAndDate.Cols(18).Caption = "日付１４"
            flxNameAndDate.Cols(18).Width = 105
            flxNameAndDate.Cols(19).Caption = "日付１５"
            flxNameAndDate.Cols(19).Width = 105
            Call Me.gridColsVisibleSet(numUseCol)

            flxNameAndDate.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
            flxNameAndDate.AutoResize = False
            flxNameAndDate.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row

            strReplaceNumber = cmbReplaceNumber.Text
            Dim cs As C1.Win.C1FlexGrid.CellStyle = flxNameAndDate.Styles.Add("lightpink")
            cs.BackColor = Color.Pink
            cs = flxNameAndDate.Styles.Add("datetype")
            cs.BackColor = Color.White
            cs.DataType = Type.GetType("System.DateTime")
            cs = flxNameAndDate.Styles.Add("normal")
            cs.BackColor = Color.White
            cs = flxNameAndDate.Styles.Add("lightyellow")
            cs.BackColor = Color.LightYellow

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "btnCancel_Click"
    'キャンセルボタン
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Dim pn As Panel
            Dim uc As Control
            Dim cntDateRows As Integer
            Dim cntDateMax As Integer = 5   ' 日付の最大表示件数(初期値5)

            If flxNameAndDate.AllowEditing Then
                If CLMsg.Show("GQ0007") = DialogResult.No Then
                    Exit Sub
                Else
                    '内容変更の場合、内容参考画面を表示
                    If lblStrikeID.Text <> "" Then
                        Cursor.Current = Cursors.WaitCursor
                        Dim dt As DataTable = getlMeetingByStrikeID(Me.lblStrikeID.Text, Me.lblApplyArea.Text)
                        If dt.Rows.Count > 0 Then
                            Me.cmbApplyMeetinglist.Text = dt.Rows(0)("l_meeting")
                            Me.txtStandName.Text = dt.Rows(0)("l_stand_name")
                        End If
                        dt = getUserByStrikeID(Me.lblStrikeID.Text, Me.lblApplyArea.Text)
                        If dt.Rows.Count > 0 Then
                            flxNameAndDate.Rows.Count = dt.Rows.Count + 1
                            For iCounter As Integer = 0 To dt.Rows.Count - 1
                                For isubCounter As Integer = 0 To 19
                                    If Not IsDBNull(dt.Rows(iCounter)(isubCounter)) Then
                                        flxNameAndDate.SetData(iCounter + 1, isubCounter, dt.Rows(iCounter)(isubCounter))
                                        '組合員単位の日付件数保存
                                        cntDateRows = isubCounter - 4
                                    Else
                                        flxNameAndDate.SetData(iCounter + 1, isubCounter, "")
                                    End If
                                Next
                                '日付件数の最大値保存
                                If cntDateRows > cntDateMax Then
                                    cntDateMax = cntDateRows
                                End If
                            Next
                        End If
                        ' 表示内容に合わせて日付表示制御
                        Me.numUseCol = cntDateMax
                        Call Me.gridColsVisibleSet(numUseCol)

                        cmbApplyMeetinglist.Enabled = False
                        btnConfirm.Visible = False
                        btnModify.Visible = True
                        btnPrinting.Enabled = True
                        btnAdd.Visible = False
                        btnRemove.Visible = False
                        btnCancel.Text = "戻る"
                        flxNameAndDate.AllowEditing = False
                        Cursor.Current = Cursors.Default
                        Exit Sub
                    End If
                End If
            End If

            '新規作成の場合、時間内活動画面へ遷移
            Me.Visible = False

            pn = ParentForm.Controls(MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC040301)

            If uc Is Nothing Then
                uc = New UC040301
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            Me.Dispose()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：btnAdd_Click"
    ' 組合員の追加
    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            Cursor.Current = Cursors.WaitCursor
            Dim cForm1 As New FM000204()
            Dim dt As DataTable = Nothing
            Dim iCounter As Integer = 0
            Dim toUserList() As String = Nothing
            Dim toUserNum As Integer = 0

            If flxNameAndDate.Rows.Count > 1 Then
                For toUserNum = 1 To flxNameAndDate.Rows.Count - 1
                    If Not IsDBNull(flxNameAndDate.GetData(toUserNum, 0)) Then
                        If Trim(flxNameAndDate.GetData(toUserNum, 0)) <> "" Then
                            ReDim Preserve toUserList(toUserNum - 1)
                            toUserList(toUserNum - 1) = Trim(flxNameAndDate.GetData(toUserNum, 0))
                        End If
                    End If
                Next
            End If
            cForm1.AllowDeleteMember = False
            cForm1.StafIDList = toUserList
            ' モーダルで表示する
            cForm1.ShowDialog()
            dt = cForm1.SelectMemberList

            If dt.Rows.Count > 0 Then
                flxNameAndDate.Rows.Count = dt.Rows.Count + 1
                For iCounter = 0 To dt.Rows.Count - 1
                    flxNameAndDate.SetData(iCounter + 1, 0, dt.Rows(iCounter)("個人認証ID"))
                    flxNameAndDate.SetData(iCounter + 1, 1, dt.Rows(iCounter)("名前"))
                    flxNameAndDate.SetData(iCounter + 1, 2, dt.Rows(iCounter)("会社所属簡略"))
                    flxNameAndDate.SetData(iCounter + 1, 3, dt.Rows(iCounter)("機種簡略"))
                    flxNameAndDate.SetData(iCounter + 1, 4, dt.Rows(iCounter)("資格"))
                Next
            End If
            ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            cForm1.Dispose()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040302, SCREEN_NAME_UC040302, "UC040302_Load")
        Finally
            Cursor.Current = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：btnRemove_Click"
    ' 選択行の削除
    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim obj As Object
        Try
            obj = flxNameAndDate.GetData(flxNameAndDate.Row, 0)
            If Not IsDBNull(obj) Then
                If Trim(obj) <> "" Then
                    If CLMsg.Show("GQ0011") = DialogResult.Yes Then
                        flxNameAndDate.RemoveItem(flxNameAndDate.Row)
                    End If
                Else
                    flxNameAndDate.RemoveItem(flxNameAndDate.Row)
                End If
            Else
                flxNameAndDate.RemoveItem(flxNameAndDate.Row)
            End If
            If flxNameAndDate.Rows.Count < 2 Then
                flxNameAndDate.Rows.Add(1)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040302, SCREEN_NAME_UC040302, "btnRemove_Click")
        End Try
    End Sub
#End Region

#Region "イベント：btnConfirm_Click"
    ' 登録確認
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim resourceObj As CrystalDecisions.CrystalReports.Engine.ReportDocument        'レポート'日程表FLEXGRID
        Dim fmPrint As FM000203                                                         '印刷プレビュー
        Dim ds As DS0403P1                                                              'データセット
        Dim intCounter As Integer                                                       'ユーザーカウンター
        Dim dbAccess As New CLAccessMdb                                                 'DBアクセス（ローカルレプリカ）
        Dim iDateCounter As Integer                                                     '日付カウンター
        Dim flg As Boolean
        Dim iUserNum As Integer                                                         '組合員数
        Dim outErrMsg As String                                                         '出力するエラーメッセージ
        Dim intUserCounter As Long                                                      '組合員参加数
        Dim strSql As String
        Dim c_application As String
        Dim intRetCd As Integer
        Dim intRetCdMst As Integer
        Dim outPurposeMsg As String                                                     '「１．目的」に表示する文字列

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            'カーソルを砂時計に変更
            Cursor.Current = Cursors.WaitCursor
            'エラーリストを初期化
            errMsgOther.Clear()
            errMsgName.Clear()
            outErrMsg = ""
            blnDateChk = False

            ' ****************************************
            '   入力確認
            ' ****************************************
            ' グリッド行が無い(ヘッダのみ)の場合
            If flxNameAndDate.Rows.Count < 2 Then
                If lblStrikeID.Text <> "" Then
                    ' 変更の場合
                    CLMsg.Show("GE0046")
                Else
                    ' 登録の場合
                    CLMsg.Show("GE0072")
                End If
                Exit Sub
            End If


            ' 日付入力チェック
            iUserNum = 0
            For intCounter = 1 To flxNameAndDate.Rows.Count - 1
                If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) And Trim(flxNameAndDate.GetData(intCounter, 0)) <> "" Then
                    flg = False
                    iUserNum = iUserNum + 1
                    For iDateCounter = 5 To (numUseCol + 4)
                        If Trim(flxNameAndDate.GetData(intCounter, iDateCounter)) <> "" Then
                            If IsDate(flxNameAndDate.GetData(intCounter, iDateCounter)) Then
                                flxNameAndDate.SetData(intCounter, iDateCounter, Format(Date.Parse(flxNameAndDate.GetData(intCounter, iDateCounter)), "yyyy/MM/dd"))
                                intUserCounter = intUserCounter + 1
                            Else
                                MsgBox("日付の形式は正しくありません。2012/02/01のように入力してください。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "エラー")
                                flxNameAndDate.Row = intCounter
                                flxNameAndDate.SetCellStyle(intCounter, iDateCounter, flxNameAndDate.Styles("lightpink"))
                                flxNameAndDate.StartEditing(intCounter, iDateCounter)
                                Exit Sub
                            End If
                            flg = True
                        End If
                    Next
                    ' 日付入力が1件も無い行があればエラー
                    If Not flg Then
                        CLMsg.Show("GE0059")
                        flxNameAndDate.StartEditing(intCounter, 5)
                        Exit Sub
                    End If
                End If
            Next

            ' グリッド行が全て組合員選択されてない場合
            If iUserNum = 0 Then
                If lblStrikeID.Text <> "" Then
                    CLMsg.Show("GE0046")
                Else
                    CLMsg.Show("GE0072")
                End If
                Exit Sub
            End If

            ' 会議名の文字数上限
            Dim intByteAppClassify As Integer
            intByteAppClassify = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(txtApplyClassify.Text)
            If ChkLengthB(cmbApplyMeetinglist.Text, 48 - intByteAppClassify) Then
                cmbApplyMeetinglist.BackColor = Color.White
            Else
                CLMsg.Show("GE0067", CStr((48 - intByteAppClassify) / 2))
                cmbApplyMeetinglist.BackColor = Color.Pink
                Exit Sub
            End If

            '申請者文字数
            If ChkLengthB(txtStandName.Text, 30) Then
                txtStandName.BackColor = Color.White
            Else
                CLMsg.Show("GE0103")
                txtStandName.BackColor = Color.Pink
                Exit Sub
            End If

            For iRow As Integer = 1 To flxNameAndDate.Rows.Count - 1
                For iColumn As Integer = 0 To 19
                    If iColumn = 0 Or iColumn > 4 Then
                        flxNameAndDate.SetCellStyle(iRow, iColumn, flxNameAndDate.Styles("normal"))
                    Else
                        flxNameAndDate.SetCellStyle(iRow, iColumn, flxNameAndDate.Styles("lightyellow"))
                    End If
                Next
            Next

            '===============================================================================================================
            '   データベース接続
            '===============================================================================================================
            dbAccess.Connect()          ' ローカルレプリカ
            'dbAccessMst.Connect()       ' サーバデザインマスタ

            '社員が存在するかチェック
            For iStafCounter As Integer = 1 To flxNameAndDate.Rows.Count - 1
                If Not IsDBNull(flxNameAndDate.GetData(iStafCounter, 0)) Then
                    Dim strStafID As String = CStr(flxNameAndDate.GetData(iStafCounter, 0))
                    If strStafID <> "" Then
                        Dim subSql = "select * from staf_attribute_latest_view where c_staf_id='" + strStafID + "' and c_ksh='" + MDLoginInfo.Ksh + "'"
                        Dim subdt = dbAccess.ExecuteSql(subSql)
                        If subdt.Rows.Count < 1 Then
                            CLMsg.Show("GE0025", strStafID)
                            flxNameAndDate.Row = iStafCounter
                            flxNameAndDate.SetCellStyle(iStafCounter, 0, flxNameAndDate.Styles("lightpink"))
                            flxNameAndDate.StartEditing(iStafCounter, 0)
                            Exit Sub
                        End If
                    End If
                End If
            Next

            '日付チェック
            If CheckInputFutureDate() = False Then
                Exit Sub
            End If
            If errMsgOther.Count > 0 Then
                errMsgOther.Insert(0, "以下の方は既に同一日が時間内組合活動で設定されています。")
                ' エラーメッセージボックス表示
                Dim clsUC999999 As New UC999999     ' メッセージボックスクラス生成
                clsUC999999.errMsgList = errMsgOther
                ' メインメニュー画面表示
                Call clsUC999999.ShowDialog()
                Exit Sub
            End If
            If errMsgName.Count > 0 Then
                errMsgName.Insert(0, vbCrLf)
                For iMsg As Integer = 0 To errMsgName.Count - 1
                    outErrMsg = outErrMsg + errMsgName(iMsg) + vbCrLf
                Next
                CLMsg.Show("GE0186", "指名ストライキ", outErrMsg)
                Exit Sub
            End If

            '申請回数チェック
            Dim strUnionMeeting As String = ""
            If lblApplyClassify.Text = "01" Or lblApplyClassify.Text = "02" Then
                strUnionMeeting = lblTermID.Text
            ElseIf lblApplyClassify.Text = "03" Then
                strUnionMeeting = txtMeetingNo.Text
            End If

            If lblStrikeID.Text <> "" Then
                If lblApplyClassify.Text = "01" Or lblApplyClassify.Text = "02" Or lblApplyClassify.Text = "03" Then
                    If CLng(GetApplyStrikeLimit(lblApplyClassify.Text)) <= CLng(GetApplyCount(lblApplyClassify.Text, minDate, strUnionMeeting)) Then
                        If CLMsg.Show("GW0002") = DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                ElseIf lblApplyClassify.Text = "99" Then
                Else
                    '内容変更のとき、すでにに登録されているユーザー分をカウントから外す
                    If CLng(GetApplyStrikeLimit(lblApplyClassify.Text)) < (CLng(GetApplyCount(lblApplyClassify.Text, minDate, "")) + intUserCounter - intAlreadyUser) Then
                        If CLMsg.Show("GW0002") = DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                End If
            Else
                If lblApplyClassify.Text = "01" Or lblApplyClassify.Text = "02" Or lblApplyClassify.Text = "03" Then
                    If CLng(GetApplyStrikeLimit(lblApplyClassify.Text)) <= CLng(GetApplyCount(lblApplyClassify.Text, minDate, strUnionMeeting)) Then
                        If CLMsg.Show("GW0002") = DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                ElseIf lblApplyClassify.Text = "99" Then
                Else
                    If CLng(GetApplyStrikeLimit(lblApplyClassify.Text)) < (CLng(GetApplyCount(lblApplyClassify.Text, minDate, "")) + intUserCounter) Then
                        If CLMsg.Show("GW0002") = DialogResult.No Then
                            Exit Sub
                        End If
                    End If
                End If
            End If

            ' ****************************************
            '   印刷プレビュー表示準備
            ' ****************************************
            Dim strInputDate As String       ' 表示用日付(yy/MM/DD型)

            FrmWaitInfo.ShowWaitForm(Nothing)
            resourceObj = New CR0403P1
            fmPrint = New FM000203
            ds = New DS0403P1
            fmPrint.ButtonShowType = 2
            fmPrint.PrintCntVisible = False
            fmPrint.ObjResource = resourceObj
            '詳細データセット
            Dim drDetail As DS0403P1.dtDetailRow
            flxNameAndDate.Sort(C1.Win.C1FlexGrid.SortFlags.Ascending, 0)
            If flxNameAndDate.Rows.Count > 1 Then
                For intCounter = 1 To flxNameAndDate.Rows.Count - 1
                    If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) Then
                        If Trim(flxNameAndDate.GetData(intCounter, 0)) <> "" Then
                            Dim numNw As Integer = 5                ' グリッド上の日付位置
                            Dim numEd As Integer = numUseCol + 4    ' 日付終了位置
                            Dim inputNum As Integer                 ' 帳票の日付位置
LBL_nextRow:
                            inputNum = 1
                            drDetail = ds.dtDetail.NewRow
                            drDetail.BeginEdit()

                            '社員番号
                            drDetail.c_staf_id = Trim(CStr(flxNameAndDate.GetData(intCounter, 0)))
                            '氏名
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 1)) Then
                                drDetail.l_name = flxNameAndDate.GetData(intCounter, 1)
                            Else
                                drDetail.l_name = ""
                            End If
                            '所属
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 2)) Then
                                drDetail.belonging = flxNameAndDate.GetData(intCounter, 2)
                            Else
                                drDetail.belonging = ""
                            End If
                            '機種
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 3)) Then
                                drDetail.model = flxNameAndDate.GetData(intCounter, 3)
                            Else
                                drDetail.model = ""
                            End If
                            '資格
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 4)) Then
                                drDetail.qualification = flxNameAndDate.GetData(intCounter, 4)
                            Else
                                drDetail.qualification = ""
                            End If

                            '日付
                            For iCol As Integer = numNw To numEd
                                If Not IsDBNull(flxNameAndDate.GetData(intCounter, iCol)) Then
                                    If Trim(flxNameAndDate.GetData(intCounter, iCol)) <> "" Then
                                        strInputDate = Format(Date.Parse(flxNameAndDate.GetData(intCounter, iCol)), "yy/MM/dd")
                                        If strInputDate <> "" Then
                                            drDetail("d_strike" + CStr(inputNum)) = strInputDate
                                            inputNum = inputNum + 1
                                            ' 5件処理毎に次の行へ移動
                                            If inputNum = 6 And Not (iCol = numEd) Then
                                                drDetail.EndEdit()
                                                ds.dtDetail.Rows.Add(drDetail)
                                                numNw = numNw + 1 '現在カーソル位置+1
                                                GoTo LBL_nextRow
                                            End If
                                        End If
                                    End If
                                End If
                                numNw = numNw + 1 '現在カーソル位置+1
                            Next

                            ' データセット登録、日付の無い空行であれば削除
                            If drDetail.Isd_strike1Null() Then
                                drDetail.CancelEdit()
                            Else
                                drDetail.EndEdit()
                                ds.dtDetail.Rows.Add(drDetail)
                            End If
                        End If
                    End If
                Next
                Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
                Dim intRest As Integer = (intRowCnt Mod 20)
                If intRest > 0 Then
                    Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 20)
                    Do While (intRowCnt < 20 * (intQuotient + 1))
                        drDetail = ds.dtDetail.NewRow
                        ds.dtDetail.Rows.Add(drDetail)
                        intRowCnt = intRowCnt + 1
                    Loop
                End If
            End If

            'Header情報セット
            Dim drHeader As DS0403P1.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.application_date = Now.Date
            drHeader.apply_area = txtApplyArea.Text
            If lblStrikeID.Text <> "" Then
                drHeader.c_application = txtApplyNumber.Text.Substring(txtApplyNumber.Text.IndexOf("-") + 1)
                drHeader.l_omission_name = txtApplyNumber.Text.Substring(0, txtApplyNumber.Text.IndexOf("-"))
            Else
                drHeader.c_application = txtApplyNumber.Text
                drHeader.l_omission_name = CStr(MDLoginInfo.Period)
            End If
            drHeader.leader_name = txtStandName.Text

            '社長氏名取得
            drHeader.president_name = GetPresidentName(dbAccess)

            '「目的」取得
            '2012/05/30「上記の会議以外」選択時は、固定文言に変更
            If lblApplyClassify.Text = "99" Then
                outPurposeMsg = "覚書(イ～ニ)で定めてある事項"
            Else
                outPurposeMsg = txtApplyClassify.Text
            End If

            If Trim(cmbApplyMeetinglist.Text) <> "" Then
                drHeader.purpose = outPurposeMsg + "(" + cmbApplyMeetinglist.Text + ")"
            Else
                drHeader.purpose = outPurposeMsg
            End If
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)

            'Footer情報セット
            Dim drFooter As DS0403P1.dtFooterRow
            drFooter = ds.dtFooter.NewRow
            drFooter.BeginEdit()
            'ここにFooter情報をセットする
            If chkReplace.Checked Then
                drFooter.replace_c_application = cmbReplaceNumber.Text.Substring(cmbReplaceNumber.Text.IndexOf("-") + 1)
                drFooter.replace_l_omission_name = cmbReplaceNumber.Text.Substring(0, cmbReplaceNumber.Text.IndexOf("-"))
            End If
            If lblStrikeID.Text <> "" Then
                drFooter.s_up = GetRevision(CInt(getSUPByStrikeID(dbAccess, lblStrikeID.Text, lblApplyArea.Text)) + 1)
            Else
                drFooter.s_up = GetRevision(0)
            End If

            drFooter.EndEdit()
            ds.dtFooter.Rows.Add(drFooter)

            resourceObj.SetDataSource(ds)
            FrmWaitInfo.CloseWaitForm()
            ' プレビュー呼び出し
            Call fmPrint.ShowDialog()

            ' ****************************************
            '   登録本処理
            ' ****************************************
            Select Case fmPrint.IntQlickBtnFlag
                Case 4
                    '===================================================================================================
                    '
                    '   登録処理
                    '
                    '===================================================================================================
                    '印刷(登録)
                    '同期処理による最新データの取得 SEQ対応によって前同期を省略 2013/04/19
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    If lblStrikeID.Text = "" Then
                        '新規申請書
                        'apply_strikeテーブルへの登録
                        'c_applicationを取得
                        c_application = CStr(getC_application())
                        ds.dtHeader.Rows.Item(0).Item("c_application") = c_application
                        resourceObj.SetDataSource(ds)

                        '-----------------------------------------------------------------------
                        '   SQL作成
                        '-----------------------------------------------------------------------
                        strSql = "Insert into apply_strike(c_strike_id,c_ksh,c_period_id,k_apply_area,c_application,d_application,l_stand_name,k_cancel,k_replace,c_replace_strike_id,k_apply_classify,l_meeting,union_info_c_period_id,union_info_c_union_meeting,d_ins,c_user_id_ins,d_up,c_user_id_up,s_up) values("
                        strSql = strSql + "'" + CStr(MDLoginInfo.Period) + "-" + c_application + "',"
                        strSql = strSql + "'" + MDLoginInfo.Ksh + "',"
                        strSql = strSql + "'" + MDLoginInfo.PeriodId + "',"
                        strSql = strSql + "'" + lblApplyArea.Text + "',"
                        strSql = strSql + "'" + c_application + "',"
                        strSql = strSql + "'" + txtApplyDate.Text + "',"
                        strSql = strSql + "'" + txtStandName.Text + "',"
                        strSql = strSql + "'0',"
                        strSql = strSql + "'0',"
                        If chkReplace.Checked Then
                            strSql = strSql + "'" + cmbReplaceNumber.Text + "',"
                        Else
                            strSql = strSql + "'',"
                        End If
                        strSql = strSql + "'" + lblApplyClassify.Text + "',"
                        strSql = strSql + "'" + cmbApplyMeetinglist.Text + "',"
                        strSql = strSql + "'" + minDate + "',"
                        If lblApplyClassify.Text = "01" Or lblApplyClassify.Text = "02" Then
                            strSql = strSql + "'" + lblTermID.Text + "',"
                        ElseIf lblApplyClassify.Text = "03" Then
                            strSql = strSql + "'" + txtMeetingNo.Text + "',"
                        Else
                            strSql = strSql + "'',"
                        End If
                        strSql = strSql + "'" + Now + "',"
                        strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                        strSql = strSql + "'" + Now + "',"
                        strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                        strSql = strSql + "0)"

                        '-----------------------------------------------------------------------
                        '   トランザクション開始
                        '-----------------------------------------------------------------------
                        dbAccess.BeginTran()                ' ローカルレプリカ
                        'dbAccessMst.BeginTran()             ' サーバデザインマスタ

                        '-------------------------------------------
                        '   SQL実行
                        '-------------------------------------------
                        ' 連番発番Insertは専用メソッド使用
                        intRetCd = dbAccess.ExecuteNonQueryKeyErr(strSql)
                        'intRetCdMst = dbAccessMst.ExecuteNonQueryKeyErr(strSql)
                        ' 結果判定
                        If intRetCd = -2 Then
                            '    If intRetCd = -2 _
                            'Or intRetCdMst = -2 Then
                            ' キー重複エラーの場合
                            CLMsg.Show("DE0015")
                            ' トランザクション取消
                            dbAccess.RollbackTran()         ' ローカルレプリカ
                            'dbAccessMst.RollbackTran()      ' サーバデザインマスタ
                            Exit Sub
                        ElseIf Not intRetCd = 1 Then
                            'ElseIf Not intRetCd = 1 _
                            '    Or Not intRetCdMst = 1 Then
                            ' その他のエラー場合
                            CLMsg.Show("DE0005")
                            ' トランザクション取消
                            dbAccess.RollbackTran()         ' ローカルレプリカ
                            'dbAccessMst.RollbackTran()      ' サーバデザインマスタ
                            Exit Sub
                        End If

                        '===================================================================================================
                        '
                        '   登録処理
                        '
                        '===================================================================================================
                        'apply_strike_member_dateテーブルへの登録
                        If flxNameAndDate.Rows.Count > 1 Then
                            For intCounter = 1 To flxNameAndDate.Rows.Count - 1
                                If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) Then
                                    If Trim(CStr(flxNameAndDate.GetData(intCounter, 0))) <> "" Then
                                        For iSub As Integer = 5 To (numUseCol + 4)
                                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, iSub)) Then
                                                Dim obj As Object = CStr(flxNameAndDate.GetData(intCounter, iSub))
                                                If obj <> "" Then
                                                    '-------------------------------------------
                                                    '   SQL作成
                                                    '-------------------------------------------
                                                    strSql = ""
                                                    strSql = "Insert into apply_strike_member_date(c_strike_id,k_apply_area,c_staf_id,d_strike,k_cancel,d_ins,c_user_id_ins,d_up,c_user_id_up,s_up) values("
                                                    strSql = strSql + "'" + CStr(MDLoginInfo.Period) + "-" + c_application + "',"
                                                    strSql = strSql + "'" + lblApplyArea.Text + "',"
                                                    strSql = strSql + "'" + CStr(flxNameAndDate.GetData(intCounter, 0)) + "',"
                                                    strSql = strSql + "'" + obj + "',"
                                                    strSql = strSql + "'0',"
                                                    strSql = strSql + "'" + Now + "',"
                                                    strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                                                    strSql = strSql + "'" + Now + "',"
                                                    strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                                                    strSql = strSql + "0)"

                                                    '-------------------------------------------
                                                    '   SQL実行
                                                    '-------------------------------------------
                                                    ' ローカルレプリカ・サーバデザインマスタどちらかでエラーかチェック
                                                    If dbAccess.ExecuteNonQuery(strSql) = -1 Then
                                                        ' トランザクション取消
                                                        dbAccess.RollbackTran()         ' ローカルレプリカ
                                                        Exit Sub
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            Next
                        End If
                        '差替えあった場合
                        If chkReplace.Checked Then
                            '===============================================================================================
                            '
                            '   更新処理
                            '
                            '===============================================================================================
                            '-------------------------------------------------------------------
                            '   SQL作成
                            '-------------------------------------------------------------------
                            strSql = "update apply_strike set k_replace='1',s_up=s_up+1,d_up='" + Now + "',c_user_id_up='" + MDLoginInfo.UserId + "'"
                            strSql = strSql + " where c_strike_id='" + cmbReplaceNumber.Text + "' and k_apply_area = '" + lblApplyArea.Text + "'"

                            '-------------------------------------------------------------------
                            '   SQL実行
                            '-------------------------------------------------------------------
                            ' ローカルレプリカ・サーバデザインマスタどちらかでエラーかチェック
                            If dbAccess.ExecuteNonQuery(strSql) = -1 Then
                                ' トランザクション取消
                                dbAccess.RollbackTran()         ' ローカルレプリカ
                            End If
                        End If

                        ' トランザクション確定
                        dbAccess.CommitTran()

                        ' SEQUENCファイルに値を反映
                        Dim strSeqName As String = "seq_apl_err_302.txt"
                        If Me.lblApplyArea.Text = "01" Then
                            strSeqName = "seq_apl_tyo_" + MDLoginInfo.PeriodId + ".txt"
                        ElseIf Me.lblApplyArea.Text = "02" Then
                            strSeqName = "seq_apl_osa_" + MDLoginInfo.PeriodId + ".txt"
                        End If
                        Dim sw As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName, False)
                        sw.Write(c_application)
                        sw.Close()

                        '帳票を出力
                        fmPrint.PrintOut()
                    Else
                        '===================================================================================================
                        '
                        '   更新処理
                        '
                        '===================================================================================================
                        '-----------------------------------------------------------------------
                        '   SQL作成
                        '-----------------------------------------------------------------------
                        '内容変更
                        'apply_strike
                        strSql = "update apply_strike set l_meeting='" + cmbApplyMeetinglist.Text + "',"
                        strSql = strSql + "l_stand_name='" + txtStandName.Text + "',"
                        strSql = strSql + "d_up='" + Now + "',"
                        strSql = strSql + "c_user_id_up='" + MDLoginInfo.UserId + "',"
                        strSql = strSql + "s_up=s_up+1 "
                        strSql = strSql + "where c_strike_id='" + lblStrikeID.Text + "'"
                        strSql = strSql + " and k_apply_area='" + lblApplyArea.Text + "'"

                        '-----------------------------------------------------------------------
                        '   SQL実行
                        '-----------------------------------------------------------------------
                        ' ローカルレプリカ・サーバデザインマスタどちらかでエラーかチェック
                        If dbAccess.ExecuteNonQuery(strSql) = -1 Then
                            ' トランザクション取消
                            dbAccess.RollbackTran()         ' ローカルレプリカ
                        End If

                        '===================================================================================================
                        '
                        '   削除処理
                        '
                        '===================================================================================================
                        '-----------------------------------------------------------------------
                        '   SQL作成
                        '-----------------------------------------------------------------------
                        'apply_strike_member_dateテーブルに該当組合員をいったん削除
                        strSql = "delete from apply_strike_member_date "
                        strSql = strSql & "where c_strike_id='" + lblStrikeID.Text + "' and k_apply_area = '" + lblApplyArea.Text + "'"

                        '-----------------------------------------------------------------------
                        '   SQL実行
                        '-----------------------------------------------------------------------
                        ' ローカルレプリカ・サーバデザインマスタどちらかでエラーかチェック
                        If dbAccess.ExecuteNonQuery(strSql) = -1 Then
                            ' トランザクション取消
                            dbAccess.RollbackTran()         ' ローカルレプリカ
                        End If

                        '===================================================================================================
                        '
                        '   登録処理
                        '
                        '===================================================================================================
                        'apply_strike_member_dateテーブルへの登録
                        If flxNameAndDate.Rows.Count > 1 Then
                            For intCounter = 1 To flxNameAndDate.Rows.Count - 1
                                If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) Then
                                    If Trim(CStr(flxNameAndDate.GetData(intCounter, 0))) <> "" Then
                                        For iSub As Integer = 5 To (numUseCol + 4)
                                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, iSub)) Then
                                                Dim obj As Object = CStr(flxNameAndDate.GetData(intCounter, iSub))
                                                If obj <> "" Then
                                                    '-------------------------------------------
                                                    '   SQL作成
                                                    '-------------------------------------------
                                                    strSql = ""
                                                    strSql = "Insert into apply_strike_member_date(c_strike_id,k_apply_area,c_staf_id,d_strike,k_cancel,d_ins,c_user_id_ins,d_up,c_user_id_up,s_up) values("
                                                    strSql = strSql + "'" + lblStrikeID.Text + "',"
                                                    strSql = strSql + "'" + lblApplyArea.Text + "',"
                                                    strSql = strSql + "'" + CStr(flxNameAndDate.GetData(intCounter, 0)) + "',"
                                                    strSql = strSql + "'" + obj + "',"
                                                    strSql = strSql + "'0',"
                                                    strSql = strSql + "'" + Now + "',"
                                                    strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                                                    strSql = strSql + "'" + Now + "',"
                                                    strSql = strSql + "'" + MDLoginInfo.UserId + "',"
                                                    strSql = strSql + "0)"

                                                    '-------------------------------------------
                                                    '   SQL実行
                                                    '-------------------------------------------
                                                    ' ローカルレプリカ・サーバデザインマスタどちらかでエラーかチェック
                                                    If dbAccess.ExecuteNonQuery(strSql) = -1 Then
                                                        ' トランザクション取消
                                                        dbAccess.RollbackTran()         ' ローカルレプリカ
                                                        Exit Sub
                                                    End If
                                                End If
                                            End If
                                        Next
                                    End If
                                End If
                            Next
                        End If

                        ' トランザクション確定
                        dbAccess.CommitTran()           ' ローカルレプリカ

                        ' 印刷
                        fmPrint.PrintOut()
                    End If

                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)

                    '時間内活動画面へ遷移
                    Dim pn As Panel
                    Dim uc As UC040301
                    Dim selTab As Integer
                    Me.Visible = False
                    pn = ParentForm.Controls(MAIN_PANEL_ID)
                    uc = pn.Controls(SCREEN_ID_UC040301)
                    If uc Is Nothing Then
                        uc = New UC040301
                        Call pn.Controls.Add(uc)
                    Else
                        ' 開いてたタブに応じて再検索フラグをセット、VisibleChange時に再検索実行
                        selTab = uc.tbcMemberActivityInTerm.SelectedIndex()
                        uc.blnRefFlg = selTab + 1
                        uc.Visible = True
                    End If

                    Me.Dispose()
                Case 2
                    'キャンセル
            End Select

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            '===============================================================================================================
            '   データベース切断
            '===============================================================================================================
            dbAccess.Disconnect()               ' ローカルレプリカ

            '-----------------------------------------------------------------------------------
            '   データベースオブジェクト開放
            '-----------------------------------------------------------------------------------
            ' ローカルレプリカ
            If Not dbAccess Is Nothing Then
                dbAccess = Nothing
            End If

            FrmWaitInfo.CloseWaitForm()
            Cursor.Current = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：flxNameAndDate_AfterEdit"
    ' 「氏名および月日」グリッド変更時
    Private Sub flxNameAndDate_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxNameAndDate.AfterEdit
        Try
            Dim iCol As Integer = flxNameAndDate.Col
            If iCol = 0 Or iCol > 4 Then
                If Not Me.gridEdit() Then
                    'エラー時は変更取り消し
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：flxNameAndDate_StartEdit"
    Private Sub flxNameAndDate_StartEdit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flxNameAndDate.StartEdit
        Try
            Dim iCol As Integer = flxNameAndDate.Col
            Dim iRow As Integer = flxNameAndDate.Row
            ' 日付項目が編集モードになったとき、空欄なら現在日設定
            If iCol > 4 Then
                If flxNameAndDate.GetData(iRow, iCol) = "" Then
                    flxNameAndDate.SetData(iRow, iCol, Format(Now, "yyyy/MM/"))
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：flxNameAndDate_KeyPress"
    ' 「氏名および月日」グリッドキー押下
    Private Sub flxNameAndDate_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles flxNameAndDate.KeyPress
        Try
            ' Enterキーの場合のみ
            If e.KeyChar = Chr(Keys.Enter) Then
                Call Me.gridEdit()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：btnReplaceNumber_Click"
    ' 差替え内容表示クリック
    Private Sub btnReplaceNumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceNumber.Click
        Dim cntDateRows As Integer
        Dim cntDateMax As Integer = 5   ' 日付の最大表示件数(初期値5)

        Try
            Cursor.Current = Cursors.WaitCursor
            If cmbReplaceNumber.Text = "" Then
                Cursor.Current = Cursors.Default
                CLMsg.Show("GE0006", "差替え申請番号")
                cmbReplaceNumber.BackColor = Color.Pink
                Exit Sub
            Else
                cmbReplaceNumber.BackColor = Color.White
                Dim dt As DataTable = getlMeetingByStrikeID(Me.cmbReplaceNumber.Text, Me.lblApplyArea.Text)
                If dt.Rows.Count > 0 Then
                    Me.cmbApplyMeetinglist.Text = dt.Rows(0)("l_meeting")
                    Me.txtStandName.Text = dt.Rows(0)("l_stand_name")
                End If

                dt = getUserByStrikeID(Me.cmbReplaceNumber.Text, Me.lblApplyArea.Text)
                'flxNameAndDate.DataSource = dt
                If dt.Rows.Count > 0 Then
                    flxNameAndDate.Rows.Count = dt.Rows.Count + 1
                    For iCounter As Integer = 0 To dt.Rows.Count - 1
                        For isubCounter As Integer = 0 To 19
                            If Not IsDBNull(dt.Rows(iCounter)(isubCounter)) Then
                                flxNameAndDate.SetData(iCounter + 1, isubCounter, dt.Rows(iCounter)(isubCounter))
                                '組合員単位の日付件数保存
                                cntDateRows = isubCounter - 4
                            End If
                        Next
                        '日付件数の最大値保存
                        If cntDateRows > cntDateMax Then
                            cntDateMax = cntDateRows
                        End If
                    Next
                End If
                ' 差替え内容に合わせて日付表示制御
                Me.numUseCol = cntDateMax
                Call Me.gridColsVisibleSet(numUseCol)

                flxNameAndDate.AllowEditing = False
                'flxNameAndDate.Cols(0).AllowEditing = False
                cmbApplyMeetinglist.Enabled = False
                txtStandName.ReadOnly = True
                btnAdd.Enabled = False
                btnRemove.Enabled = False
                btnConfirm.Enabled = True
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：cmbReplaceNumber_SelectedIndexChanged"
    Private Sub cmbReplaceNumber_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReplaceNumber.SelectedIndexChanged
        Try
            If strReplaceNumber <> cmbReplaceNumber.Text Then
                flxNameAndDate.Rows.Count = 1
                flxNameAndDate.Rows.Add()
                strReplaceNumber = cmbReplaceNumber.Text
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：lklMemo_LinkClicked"
    Private Sub lklMemo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lklMemo.LinkClicked
        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 「覚書を表示」の共通処理起動
            ShowOboegaki()
        Catch ex As Exception
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：btnModify_Click"
    '「内容変更」ボタン押下
    Private Sub btnModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModify.Click
        Try
            flxNameAndDate.AllowEditing = True
            cmbApplyMeetinglist.Enabled = True
            Call Utilities.SetCanEditToControl(True, cmbApplyMeetinglist)
            txtStandName.ReadOnly = False
            btnPrinting.Enabled = False
            btnModify.Visible = False
            btnConfirm.Visible = True
            btnConfirm.Enabled = True
            btnAdd.Visible = True
            btnAdd.Enabled = True
            btnRemove.Visible = True
            btnRemove.Enabled = True
            btnCancel.Text = "キャンセル"
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "イベント：btnPrinting_Click"
    ' 印刷ボタン押下(参照時)
    Private Sub btnPrinting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinting.Click
        Dim resourceObj As CrystalDecisions.CrystalReports.Engine.ReportDocument        'レポート'日程表FLEXGRID
        Dim fmPrint As FM000203                                                         '印刷プレビュー
        Dim ds As DS0403P1                                                              'データセット
        Dim dbAccess As New CLAccessMdb                                                 'DBアクセス
        Dim outPurposeMsg As String                                                     '「１．目的」に表示する文字列

        Try
            FrmWaitInfo.ShowWaitForm(Nothing)

            If lblISDelete.Text = "1" Then
                '取り消し
                resourceObj = New CR0403P2
            Else
                '内容変更
                resourceObj = New CR0403P1
            End If

            '印刷プレビュー
            Dim strInputDate As String       ' 表示用日付(yy/MM/DD型)

            fmPrint = New FM000203
            ds = New DS0403P1
            fmPrint.ButtonShowType = 3
            fmPrint.PrintCntVisible = False
            fmPrint.ObjResource = resourceObj
            '詳細データセット
            Dim drDetail As DS0403P1.dtDetailRow
            If flxNameAndDate.Rows.Count > 1 Then
                For intCounter = 1 To flxNameAndDate.Rows.Count - 1
                    If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) Then
                        If Trim(flxNameAndDate.GetData(intCounter, 0)) <> "" Then
                            Dim numNw As Integer = 5                ' グリッド上の日付位置
                            Dim numEd As Integer = numUseCol + 4    ' 日付終了位置
                            Dim inputNum As Integer                 ' 帳票の日付位置
LBL_nextRow:
                            inputNum = 1
                            drDetail = ds.dtDetail.NewRow
                            drDetail.BeginEdit()
                            '社員番号
                            drDetail.c_staf_id = Trim(flxNameAndDate.GetData(intCounter, 0))
                            '氏名
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 1)) Then
                                drDetail.l_name = flxNameAndDate.GetData(intCounter, 1)
                            Else
                                drDetail.l_name = ""
                            End If
                            '所属
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 2)) Then
                                drDetail.belonging = flxNameAndDate.GetData(intCounter, 2)
                            Else
                                drDetail.belonging = ""
                            End If
                            '機種
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 3)) Then
                                drDetail.model = flxNameAndDate.GetData(intCounter, 3)
                            Else
                                drDetail.model = ""
                            End If
                            '資格
                            If Not IsDBNull(flxNameAndDate.GetData(intCounter, 4)) Then
                                drDetail.qualification = flxNameAndDate.GetData(intCounter, 4)
                            Else
                                drDetail.qualification = ""
                            End If
                            '日付
                            For iCol As Integer = numNw To numEd
                                If Not IsDBNull(flxNameAndDate.GetData(intCounter, iCol)) Then
                                    If flxNameAndDate.GetData(intCounter, iCol) <> "" Then
                                        strInputDate = Format(Date.Parse(flxNameAndDate.GetData(intCounter, iCol)), "yy/MM/dd")
                                        If strInputDate <> "" Then
                                            drDetail("d_strike" + CStr(inputNum)) = strInputDate
                                            inputNum = inputNum + 1
                                            ' 5件処理毎に次の行へ移動
                                            If inputNum = 6 And Not (iCol = numEd) Then
                                                drDetail.EndEdit()
                                                ds.dtDetail.Rows.Add(drDetail)
                                                numNw = numNw + 1 '現在カーソル位置+1
                                                GoTo LBL_nextRow
                                            End If
                                        End If
                                    End If
                                End If
                                numNw = numNw + 1 '現在カーソル位置+1
                            Next

                            ' データセット登録、日付の無い空行であれば削除
                            If drDetail.Isd_strike1Null() Then
                                drDetail.CancelEdit()
                            Else
                                drDetail.EndEdit()
                                ds.dtDetail.Rows.Add(drDetail)
                            End If
                        End If
                    End If
                Next
                Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
                Dim intRest As Integer = (intRowCnt Mod 20)
                If intRest > 0 Then
                    Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 20)
                    Do While (intRowCnt < 20 * (intQuotient + 1))
                        drDetail = ds.dtDetail.NewRow
                        ds.dtDetail.Rows.Add(drDetail)
                        intRowCnt = intRowCnt + 1
                    Loop
                End If
            End If
            'Header情報セット
            Dim drHeader As DS0403P1.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.application_date = DateValue(txtApplyDate.Text)
            drHeader.apply_area = txtApplyArea.Text
            drHeader.c_application = txtApplyNumber.Text.Substring(txtApplyNumber.Text.IndexOf("-") + 1)
            drHeader.l_omission_name = txtApplyNumber.Text.Substring(0, txtApplyNumber.Text.IndexOf("-"))
            drHeader.leader_name = txtStandName.Text

            '社長氏名取得
            dbAccess.Connect()
            drHeader.president_name = GetPresidentName(dbAccess)

            '「目的」取得
            '2012/05/30「上記の会議以外」選択時は、固定文言に変更
            If lblApplyClassify.Text = "99" Then
                outPurposeMsg = "覚書(イ～ニ)で定めてある事項"
            Else
                outPurposeMsg = txtApplyClassify.Text
            End If

            If Trim(cmbApplyMeetinglist.Text) <> "" Then
                drHeader.purpose = outPurposeMsg + "(" + cmbApplyMeetinglist.Text + ")"
            Else
                drHeader.purpose = outPurposeMsg
            End If
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)

            'Footer情報セット
            Dim drFooter As DS0403P1.dtFooterRow
            drFooter = ds.dtFooter.NewRow
            drFooter.BeginEdit()
            'ここにFooter情報をセットする
            If chkReplace.Checked Then
                drFooter.replace_c_application = cmbReplaceNumber.Text.Substring(cmbReplaceNumber.Text.IndexOf("-") + 1)
                drFooter.replace_l_omission_name = cmbReplaceNumber.Text.Substring(0, cmbReplaceNumber.Text.IndexOf("-"))
            End If
            drFooter.s_up = GetRevision(getSUPByStrikeID(dbAccess, lblStrikeID.Text, lblApplyArea.Text))
            drFooter.EndEdit()
            ds.dtFooter.Rows.Add(drFooter)

            resourceObj.SetDataSource(ds)
            FrmWaitInfo.CloseWaitForm()
            Call fmPrint.ShowDialog()
            Select Case fmPrint.IntQlickBtnFlag
                Case 3
                    '印刷(登録)
                    fmPrint.PrintOut()
                Case 2
                    'キャンセル
            End Select

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
            FrmWaitInfo.CloseWaitForm()
        End Try
    End Sub
#End Region

#Region "イベント：chkReplace_CheckStateChanged"
    '差替えチェックボックス値変更
    Private Sub chkReplace_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReplace.CheckStateChanged
        Try
            Dim flg As Boolean
            flg = False
            If cmbReplaceNumber.Items.Count < 1 Then
                If chkReplace.Checked Then
                    CLMsg.Show("GE0056")
                    chkReplace.Checked = False
                End If
                Exit Sub
            End If
            If chkReplace.Checked Then
                For iCounter As Integer = 1 To flxNameAndDate.Rows.Count - 1
                    If flxNameAndDate.GetData(iCounter, 0) <> "" Then
                        flg = True
                        Exit For
                    End If
                Next
                If flg Then
                    If CLMsg.Show("GQ0016") = DialogResult.Yes Then
                        cmbReplaceNumber.Enabled = True
                        btnReplaceNumber.Enabled = True
                        btnAdd.Enabled = False
                        btnRemove.Enabled = False
                        btnConfirm.Enabled = False
                        flxNameAndDate.Rows.Count = 1
                        flxNameAndDate.Rows.Add()
                        flxNameAndDate.AllowEditing = False
                    Else
                        chkReplace.Checked = False
                    End If
                Else
                    cmbReplaceNumber.Enabled = True
                    btnReplaceNumber.Enabled = True
                    btnAdd.Enabled = False
                    btnRemove.Enabled = False
                    btnConfirm.Enabled = False
                    flxNameAndDate.AllowEditing = False
                End If
            Else
                cmbReplaceNumber.Enabled = False
                btnReplaceNumber.Enabled = False
                btnAdd.Enabled = True
                btnRemove.Enabled = True
                btnConfirm.Enabled = True
                flxNameAndDate.AllowEditing = True
                flxNameAndDate.Cols(0).AllowEditing = True
                cmbApplyMeetinglist.Text = ""
                cmbReplaceNumber.SelectedIndex = 0
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#End Region

#Region "関数：SetStaffData"
    ' 組合員情報取得
    Private Function SetStaffData(ByVal strStaffId As String, ByVal iRowIndex As Integer) As Boolean

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim dbAccess As New CLAccessMdb
        Try
            If Me.flxNameAndDate.Item(iRowIndex, 0) Is System.DBNull.Value Then
                Return False
            Else
                dbAccess.Connect()
                Dim table1 As DataTable = GetUnionMemberData(dbAccess, strStaffId)
                If table1.Rows.Count > 0 Then
                    '氏名
                    Me.flxNameAndDate.Item(iRowIndex, 1) = table1.Rows(0).Item(1)
                    '所属
                    Me.flxNameAndDate.Item(iRowIndex, 2) = table1.Rows(0).Item(6)
                    '機種
                    Me.flxNameAndDate.Item(iRowIndex, 3) = table1.Rows(0).Item(7)
                    '資格
                    Me.flxNameAndDate.Item(iRowIndex, 4) = table1.Rows(0).Item(4)
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Function
#End Region

#Region "関数：getC_application"
    ' 申請番号の最大値取得
    Private Function getC_application() As Integer
        Dim dbAccess As New CLAccessMdb
        Dim dt As DataTable = Nothing
        Dim sql As String = ""
        Dim intSeqDb As Integer
        Dim intSeqText As Integer

        Try
            ' DBから最新番号取得
            sql = "Select max(CInt(c_application)) as max_c_application from apply_strike "
            sql = sql & "where Mid(c_strike_id,1,2)='" + CStr(MDLoginInfo.Period) + "' and k_apply_area = '" + Me.lblApplyArea.Text + "'"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)("max_c_application")) Then
                    If CStr(dt.Rows(0)("max_c_application")) <> "" Then
                        intSeqDb = CLng(dt.Rows(0)("max_c_application")) + 1
                    Else
                        intSeqDb = 1
                    End If
                Else
                    intSeqDb = 1
                End If
            Else
                intSeqDb = 1
            End If
            dbAccess.Disconnect()

            ' TEXTから最新番号取得
            Dim strSeqName As String = "seq_apl.txt"
            If Me.lblApplyArea.Text = "01" Then
                strSeqName = "seq_apl_tyo_" + MDLoginInfo.PeriodId + ".txt"
            ElseIf Me.lblApplyArea.Text = "02" Then
                strSeqName = "seq_apl_osa_" + MDLoginInfo.PeriodId + ".txt"
            End If
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intSeqText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intSeqText = 1
            Catch ex As System.InvalidCastException
                intSeqText = 1
            End Try

            ' 値の大きいほうを採用
            If intSeqText >= intSeqDb Then
                Return intSeqText
            Else
                Return intSeqDb
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
        End Try
    End Function
#End Region

#Region "関数：getUserByStrikeID"
    ' 氏名と月日情報取得
    Private Function getUserByStrikeID(ByVal strStrikeID As String, ByVal strApplyArea As String)
        Dim dt As New DataTable
        Dim retDt As New DataTable
        Dim dbAccess As New CLAccessMdb
        Try
            If strStrikeID <> "" Then

                Dim sql As String
                sql = "select c_staf_id as c_staf_id,d_strike,k_cancel from apply_strike_member_date "
                sql = sql & "where c_strike_id='" + strStrikeID + "' and k_apply_area = '" + strApplyArea + "'"
                sql = sql & "order by CLng(c_staf_id),d_strike"
                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)
                Dim intCurNum As Integer

                If dt.Rows.Count > 0 Then
                    retDt.Columns.Add("c_staf_id")
                    retDt.Columns.Add("l_name")
                    retDt.Columns.Add("k_belonging")
                    retDt.Columns.Add("k_model")
                    retDt.Columns.Add("k_qualification")
                    retDt.Columns.Add("d_strike1")
                    retDt.Columns.Add("d_strike2")
                    retDt.Columns.Add("d_strike3")
                    retDt.Columns.Add("d_strike4")
                    retDt.Columns.Add("d_strike5")
                    retDt.Columns.Add("d_strike6")
                    retDt.Columns.Add("d_strike7")
                    retDt.Columns.Add("d_strike8")
                    retDt.Columns.Add("d_strike9")
                    retDt.Columns.Add("d_strike10")
                    retDt.Columns.Add("d_strike11")
                    retDt.Columns.Add("d_strike12")
                    retDt.Columns.Add("d_strike13")
                    retDt.Columns.Add("d_strike14")
                    retDt.Columns.Add("d_strike15")
                    Dim subStafID As String
                    Dim subStafIDCur As String
                    subStafIDCur = dt.Rows(0)(0)
                    Dim newRow As DataRow = retDt.NewRow
                    newRow("c_staf_id") = subStafIDCur
                    newRow("d_strike1") = Format(dt.Rows(0)(1), DATE_YYYYMMDD_FORMAT)
                    intCurNum = 1

                    Dim subDt As DataTable = GetUnionMemberData(dbAccess, subStafIDCur)
                    If subDt.Rows.Count > 0 Then
                        '氏名
                        newRow("l_name") = subDt.Rows(0).Item(1)
                        '資格
                        newRow("k_qualification") = subDt.Rows(0).Item(4)
                        '機種
                        newRow("k_model") = subDt.Rows(0).Item(7)
                        '所属
                        newRow("k_belonging") = subDt.Rows(0).Item(6)
                    End If
                    'retDt.Rows.Add(newRow)

                    For iCounter As Integer = 1 To dt.Rows.Count - 1
                        subStafID = dt.Rows(iCounter)(0)
                        If subStafID = subStafIDCur Then
                            intCurNum = intCurNum + 1
                            newRow("d_strike" + CStr(intCurNum)) = Format(dt.Rows(iCounter)(1), DATE_YYYYMMDD_FORMAT)
                        Else
                            retDt.Rows.Add(newRow)
                            newRow = retDt.NewRow
                            newRow("c_staf_id") = subStafID

                            newRow("d_strike1") = Format(dt.Rows(iCounter)(1), DATE_YYYYMMDD_FORMAT)
                            intCurNum = 1

                            subDt = GetUnionMemberData(dbAccess, subStafID)
                            If subDt.Rows.Count > 0 Then
                                '氏名
                                newRow("l_name") = subDt.Rows(0).Item(1)
                                '資格
                                newRow("k_qualification") = subDt.Rows(0).Item(4)
                                '機種
                                newRow("k_model") = subDt.Rows(0).Item(7)
                                '所属
                                newRow("k_belonging") = subDt.Rows(0).Item(6)
                            End If
                            subStafIDCur = subStafID

                        End If
                    Next
                    retDt.Rows.Add(newRow)
                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try
        Return retDt
    End Function
#End Region

#Region "関数：getlMeetingByStrikeID"
    ' 会議名＆代表者名取得
    Private Function getlMeetingByStrikeID(ByVal strStrikeID As String, ByVal strApplyArea As String)
        Dim strRet As String
        Dim dt As New DataTable
        Dim dbAccess As New CLAccessMdb
        strRet = ""
        Try
            Dim sql As String
            sql = "select l_meeting,l_stand_name from apply_strike "
            sql = sql & "where c_strike_id='" + strStrikeID + "' and k_apply_area = '" + strApplyArea + "'"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
        End Try
        Return dt
    End Function
#End Region

#Region "関数：checkIfStafMultiAction"
    '他の時間内活動チェック
    ' チェック処理のうち重複登録チェックのみローカル・サーバ二重化対応 2015/03/30
    'Private Sub checkIfStafMultiAction(ByVal dbAccess As CLAccessMdb, ByVal strStafID As String, ByVal strDate As String, ByVal strStrikeID As String, ByVal strReplaceID As String)
    Private Sub checkIfStafMultiAction(ByVal dbAccess As CLAccessMdb, ByVal strStafID As String, ByVal strDate As String, ByVal strStrikeID As String, ByVal strReplaceID As String)
        Dim retStr As String
        Try

            Dim dt As DataTable
            Dim dtM As DataTable
            Dim sql As String
            sql = "SELECT apply_strike.c_strike_id, member_date.c_staf_id, member_date.l_name, format(member_date.d_strike,'yyyy/MM/dd') AS d_strike, apply_strike.k_apply_classify FROM "
            If strStrikeID <> "" Then
                If strReplaceID <> "" Then
                    sql = sql + "(select * from apply_strike where k_cancel<>'1' and k_replace<>'1' and c_strike_id<>'" + strStrikeID + "' and c_strike_id<>'" + strReplaceID + "') apply_strike inner join "
                Else
                    sql = sql + "(select * from apply_strike where k_cancel<>'1' and k_replace<>'1' and c_strike_id<>'" + strStrikeID + "') apply_strike inner join "
                End If
            Else
                If strReplaceID <> "" Then
                    sql = sql + "(select * from apply_strike where k_cancel<>'1' and k_replace<>'1' and c_strike_id<>'" + strReplaceID + "') apply_strike inner join "
                Else
                    sql = sql + "(select * from apply_strike where k_cancel<>'1' and k_replace<>'1' ) apply_strike inner join "
                End If

            End If
            sql = sql + "(select * from apply_strike_member_date as member_date, staf_attribute_full_time_now_name_view as attribute "
            sql = sql + " where member_date.c_staf_id='" + strStafID + "' and (member_date.k_cancel<>'1' or member_date.k_cancel is null) and format(member_date.d_strike,'yyyy/MM/dd')='" + strDate + "' and attribute.user_id='" + strStafID + "')as member_date"
            sql = sql + " on (apply_strike.c_strike_id = member_date.c_strike_id and apply_strike.k_apply_area = member_date.k_apply_area)"
            sql = sql + "ORDER BY d_strike"
            dt = dbAccess.ExecuteSql(sql)

            If dt.Rows.Count > 0 Or dtM.Rows.Count > 0 Then
                '行数が多い方(同数ならローカル側)を使用 2015/03/30
                If dt.Rows.Count >= dtM.Rows.Count Then
                    For iCounter As Integer = 0 To dt.Rows.Count - 1
                        errMsgOther.Add("氏名:" + dt.Rows(iCounter)("l_name") + "（申請書番号:" + dt.Rows(iCounter)("c_strike_id") + ", 日付:" + dt.Rows(iCounter)("d_strike") + "）")
                    Next
                Else
                    For iCounter As Integer = 0 To dtM.Rows.Count - 1
                        errMsgOther.Add("氏名:" + dtM.Rows(iCounter)("l_name") + "（申請書番号:" + dtM.Rows(iCounter)("c_strike_id") + ", 日付:" + dtM.Rows(iCounter)("d_strike") + "）")
                    Next
                End If
            Else
                retStr = ""
            End If
            '指名ストライキに同じ日に登録されているか
            sql = "select l_name,name_strike.c_name_strike_id,c_user_id,d_operation_from,d_operation_to,k_name_strike_kind from name_strike inner join "
            sql = sql + "(select c_name_strike_id,l_name,c_user_id from name_strike_member_date left join staf_attribute_full_time_now_name_view on name_strike_member_date.c_user_id=staf_attribute_full_time_now_name_view.user_id where name_strike_member_date.c_user_id='" + strStafID + "' and name_strike_member_date.c_cancel_name_strike_id='')as attribute on attribute.c_name_strike_id=name_strike.c_name_strike_id where format(name_strike.d_operation_from,'yyyy/MM/dd') <='" + strDate + "' and format(name_strike.d_operation_to,'yyyy/MM/dd') >='" + strDate + "' and name_strike.k_name_strike_kind = '01'"
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Or dtM.Rows.Count > 0 Then
                '行数が多い方(同数ならローカル側)を使用 2015/03/30
                If dt.Rows.Count >= dtM.Rows.Count Then
                    For iCounter As Integer = 0 To dt.Rows.Count - 1
                        errMsgName.Add("氏名:" + dt.Rows(iCounter)("l_name") + "（通告書番号:" + dt.Rows(iCounter)("c_name_strike_id") + ", 日付:" + strDate + "）")
                    Next
                Else
                    For iCounter As Integer = 0 To dtM.Rows.Count - 1
                        errMsgName.Add("氏名:" + dtM.Rows(iCounter)("l_name") + "（通告書番号:" + dtM.Rows(iCounter)("c_name_strike_id") + ", 日付:" + strDate + "）")
                    Next
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040302, SCREEN_NAME_UC040302, "checkIfStafMultiAction")
        End Try

    End Sub
#End Region

#Region "関数：CheckPeriodCount"
    Private Function CheckPeriodCount(ByVal dbAccess As CLAccessMdb, ByVal strDate As String)
        Dim retFlg As String
        retFlg = ""
        Try
            Dim cmdText As String = "select c_period_id, d_from, d_to, c_ksh, k_period_kind, l_name, l_omission_name from period where d_from <='" + strDate + "' and '" + strDate + "' <= d_to "

            Dim dt As DataTable = dbAccess.ExecuteSql(cmdText)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("c_period_id")
            Else
                Cursor.Current = Cursors.Default
                CLMsg.Show("GE0102")
                Return ""
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        Return retFlg

    End Function
#End Region

#Region "関数：CheckInputFutureDate"
    ' 登録時日付チェック
    Private Function CheckInputFutureDate() As Boolean
        Dim oneUserPeriod As String
        Dim strHold As String
        Dim dbAccess As New CLAccessMdb         'DBアクセス（ローカルレプリカ）
        Dim strPre As String
        dbAccess.Connect()
        strHold = ""
        Try
            minDate = ""
            strHold = ""
            Dim curDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            For intCounter As Integer = 1 To flxNameAndDate.Rows.Count - 1
                If Not IsDBNull(flxNameAndDate.GetData(intCounter, 0)) And CStr(flxNameAndDate.GetData(intCounter, 0)) <> "" Then
                    strPre = ""
                    For intColNum As Integer = 5 To (numUseCol + 4)
                        If Not IsDBNull(flxNameAndDate.GetData(intCounter, intColNum)) Then
                            If CStr(flxNameAndDate.GetData(intCounter, intColNum)) <> "" Then
                                Dim onceDate As String = Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), DATE_YYYYMMDD_8_FORMAT)
                                ' 同一日付入力チェック
                                If strPre.Contains(onceDate) Then
                                    Cursor.Current = Cursors.Default
                                    CLMsg.Show("GE0057")
                                    flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                    flxNameAndDate.StartEditing(intCounter, intColNum)
                                    Return False
                                Else
                                    strPre = strPre & " , " & onceDate
                                End If

                                If onceDate < MDLoginInfo.PeriodFrom Then
                                    Cursor.Current = Cursors.Default
                                    CLMsg.Show("GE0061")
                                    flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                    flxNameAndDate.StartEditing(intCounter, intColNum)
                                    Return False
                                End If
                                'マスタデータで管理している日付かチェック
                                oneUserPeriod = CheckPeriodCount(dbAccess, onceDate)
                                If oneUserPeriod = "" Then
                                    flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                    flxNameAndDate.StartEditing(intCounter, intColNum)
                                    Return False
                                End If
                                '期をまたがっている
                                If strHold = "" Then
                                    strHold = oneUserPeriod
                                Else
                                    If oneUserPeriod <> strHold Then
                                        Cursor.Current = Cursors.Default
                                        CLMsg.Show("GE0062")
                                        flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                        flxNameAndDate.StartEditing(intCounter, intColNum)
                                        Return False
                                    End If
                                End If
                                '現在日付より先の日付チェック
                                If onceDate <= curDate Then
                                    If MDLoginInfo.CommitteeStatusFlg = 1 Then
                                        Cursor.Current = Cursors.Default
                                        CLMsg.Show("GE0066")
                                        flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                        flxNameAndDate.StartEditing(intCounter, intColNum)
                                        Return False
                                    Else
                                        ' 1回でも警告にOK押下していればスキップ
                                        If Not blnDateChk Then
                                            Cursor.Current = Cursors.Default
                                            If Not CLMsg.Show("GW0036") = DialogResult.Yes Then
                                                flxNameAndDate.SetCellStyle(intCounter, intColNum, flxNameAndDate.Styles("lightpink"))
                                                flxNameAndDate.StartEditing(intCounter, intColNum)
                                                Return False
                                            Else
                                                blnDateChk = True
                                                Cursor.Current = Cursors.WaitCursor
                                            End If
                                        End If
                                    End If
                                End If
                                '他の時間内活動チェック
                                'Call checkIfStafMultiAction(dbAccess, CStr(flxNameAndDate.GetData(intCounter, 0)), Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), DATE_yyyyMMdd_FORMAT), lblStrikeID.Text, cmbReplaceNumber.Text)
                                'Call checkIfStafMultiAction(dbAccess, dbAccessMst, CStr(flxNameAndDate.GetData(intCounter, 0)), Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), DATE_yyyyMMdd_FORMAT), lblStrikeID.Text, cmbReplaceNumber.Text)

                                '最小日付を取得
                                If minDate = "" Then
                                    minDate = Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), "yyyyMM")
                                Else
                                    If minDate > Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), "yyyyMM") Then
                                        minDate = Format(Date.Parse(flxNameAndDate.GetData(intCounter, intColNum)), "yyyyMM")
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try

        Return True
    End Function
#End Region

#Region "関数：getSUPByStrikeID"
    '更新回数取得
    Private Function getSUPByStrikeID(ByVal dbAccess As CLAccessMdb, ByVal strStrikeID As String, ByVal strApplyArea As String) As Integer
        Try
            Dim sql As String
            Dim dt As DataTable
            sql = "select s_up from apply_strike where c_strike_id='" + strStrikeID + "' and k_apply_area = '" + strApplyArea + "'"
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0)
            Else
                Return 0
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Function
#End Region

#Region "関数：gridEdit"
    '氏名と月日グリッド変更時処理
    Private Function gridEdit() As Boolean
        Dim rtnBln As Boolean = False
        Try
            Dim iCol As Integer = flxNameAndDate.Col
            Dim index As Integer = Me.flxNameAndDate.Row
            ' 組合員番号
            If iCol = 0 Then
                Dim strStaffId As Object = Me.flxNameAndDate.Item(index, 0)
                If Not strStaffId Is System.DBNull.Value Then
                    If Trim(strStaffId) <> "" Then
                        ' 重複入力チェック
                        For iCounter As Integer = 1 To flxNameAndDate.Rows.Count - 1
                            If strStaffId = flxNameAndDate.GetData(iCounter, 0) And iCounter <> index Then
                                CLMsg.Show("GE0009", strStaffId)
                                flxNameAndDate.SetCellStyle(index, iCol, flxNameAndDate.Styles("lightpink"))
                                flxNameAndDate.StartEditing(index, iCol)
                                Return rtnBln
                            Else
                                flxNameAndDate.SetCellStyle(index, iCol, flxNameAndDate.Styles("normal"))
                            End If
                        Next
                        ' 組合員情報取得
                        If SetStaffData(strStaffId.ToString, index) Then
                            flxNameAndDate.SetCellStyle(index, iCol, flxNameAndDate.Styles("normal"))
                            ' 最終行の場合、新規に一行を追加する
                            If index = flxNameAndDate.Rows.Count - 1 And flxNameAndDate.AllowEditing Then
                                Dim row As C1.Win.C1FlexGrid.Row = Me.flxNameAndDate.Rows.Add
                            End If
                        Else
                            CLMsg.Show("GE0025", strStaffId)
                            flxNameAndDate.SetData(index, 1, "")
                            flxNameAndDate.SetData(index, 2, "")
                            flxNameAndDate.SetData(index, 3, "")
                            flxNameAndDate.SetData(index, 4, "")
                            flxNameAndDate.SetCellStyle(index, iCol, flxNameAndDate.Styles("lightpink"))
                            'flxNameAndDate.StartEditing(index, iCol) ' 入力モードにしない
                            Return rtnBln
                        End If
                    End If
                End If
            Else
                ' 日付項目
                If iCol > 4 Then
                    If Not IsDBNull(flxNameAndDate.GetData(index, iCol)) Then
                        If flxNameAndDate.GetData(index, iCol) <> "" Then
                            If IsDate(flxNameAndDate.GetData(index, iCol)) Then
                                flxNameAndDate.SetData(index, iCol, Format(Date.Parse(flxNameAndDate.GetData(index, iCol)), "yyyy/MM/dd"))
                                ' 最終列の場合最大15まで日付を表示
                                If (iCol - 4) = numUseCol Then
                                    If Me.numUseCol < 15 Then
                                        Me.numUseCol = Me.numUseCol + 1
                                        Me.flxNameAndDate.Cols(Me.numUseCol + 4).Visible = True
                                    End If
                                End If
                            Else
                                flxNameAndDate.Row = index
                                flxNameAndDate.StartEditing(index, iCol)
                            End If
                        End If
                    End If
                End If
            End If

            rtnBln = True
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        Return rtnBln
    End Function
#End Region

#Region "関数：gridColsVisibleSet"
    ' 日付カラムの表示非表示一括変更
    Public Function gridColsVisibleSet(ByVal numMax As Integer) As Boolean
        Dim intMax As Integer
        Dim rtnBln As Boolean = False
        Try
            ' 5～15の範囲に強制的に補正
            If numMax < 5 Then
                intMax = 5
            ElseIf numMax > 15 Then
                intMax = 15
            Else
                intMax = numMax
            End If
            ' 補正した値でnumUseColを上書き
            Me.numUseCol = intMax

            ' numMaxに指定された日付まで表示
            For i As Integer = 1 To 15
                If i <= intMax Then
                    flxNameAndDate.Cols(i + 4).Visible = True
                Else
                    flxNameAndDate.Cols(i + 4).Visible = False
                End If
            Next

            rtnBln = True
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        Return rtnBln
    End Function
#End Region

End Class
