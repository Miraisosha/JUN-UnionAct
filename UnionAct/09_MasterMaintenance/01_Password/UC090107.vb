'===========================================================================================================
'   クラスＩＤ　　：UC090107
'   クラス名称　　：委員会マスタメンテナンス - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon

Public Class UC090107

    Private Const TITLE As String = "委員会マスタメンテナンス"
    Private cStrMode As String
    Private cStrFromData As String

    Public Sub setData(ByVal strId As String, ByVal strFromDate As String, ByVal strToDate As String, ByVal strName As String, ByVal strBiko As String, ByVal strType As String, ByVal strBelonging As String)
        Me.txtId.Text = strId
        Me.cboFromDate.Value = strFromDate
        cStrFromData = strFromDate
        Me.txtToDate.Text = strToDate
        Me.txtName.Text = strName
        Me.txtBiko.Text = strBiko
        Me.lstBelonging.Text = strBelonging
        Me.lstType.Text = strType
        Me.lstBelonging.Text = strBelonging
        Me.lstType.Text = strType
    End Sub

    Public Sub setMode(ByVal strMode As String)
        cStrMode = strMode

        If cStrMode = "READONLY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            'Me.btnConfirm.Text = "内容変更"
            Me.btnConfirm.Enabled = False
            Me.btnCopy.Enabled = False
            Me.lstCommittee.Enabled = False
            Me.DataGridView1.ReadOnly = True
            Me.btnCancel.Text = "戻る"
            Me.cboFromDate.Enabled = False
            Me.btnRowAdd.Enabled = False
            Me.btnRowDelete.Enabled = False
            Me.txtName.ReadOnly = True
            Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        ElseIf cStrMode = "ADDHISTORY" Then
            Me.lbl.Text = TITLE & " - 履歴登録"
            If (MDMasterCommon.GetMonthTopDate(Date.Today) > Me.cboFromDate.Value) Then
                Me.cboFromDate.MinDate = MDMasterCommon.GetMonthTopDate(Date.Today)
            Else
                Me.cboFromDate.MinDate = Me.cboFromDate.Value.AddMonths(1)
            End If
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
            'Me.lstBelonging.ReadOnly = True
            Me.lstBelonging.Enabled = False
            Me.lstBelonging.BackColor = Color.FromKnownColor(KnownColor.Info)

        ElseIf cStrMode = "UPDATEHISTORY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            'Me.btnConfirm.Text = "内容変更"
            Me.btnCancel.Text = "戻る"
            Me.btnRowAdd.Enabled = False
            Me.btnRowDelete.Enabled = False
            If MDMasterCommon.IsFutureMonth(cboFromDate.Value) = False Then
                Me.btnConfirm.Enabled = False
                Me.DataGridView1.ReadOnly = True
            Else
                Me.btnConfirm.Enabled = True
                Me.DataGridView1.ReadOnly = False
            End If
            Me.btnCopy.Enabled = False
            Me.lstCommittee.Enabled = False
            Me.cboFromDate.Enabled = False
            'Me.lstBelonging.ReadOnly = True
            Me.lstBelonging.Enabled = False
            Me.lstBelonging.BackColor = Color.FromKnownColor(KnownColor.Info)

        Else
            Me.cboFromDate.MinDate = MDMasterCommon.GetMonthTopDate(Date.Today)
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
        End If
    End Sub

    Private Sub UC090107_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If cStrMode = "ADDHISTORY" Then
            loadDetailData(Me.txtId.Text, cStrFromData, Me.txtToDate.Text)
        Else
            loadDetailData(Me.txtId.Text, Me.cboFromDate.Value, Me.txtToDate.Text)
        End If

        Dim clsMdb As New CLAccessMdb
        Try
            clsMdb.Connect()
            Dim strSql, strType, strBelonging As String
            strType = lstType.Text
            strBelonging = lstBelonging.Text
            strSql = "SELECT l_name AS name,c_committee_id as id" &
                     " FROM committee WHERE c_ksh='" & MDLoginInfo.Ksh & "'" &
                     " GROUP BY c_committee_id,l_name ORDER BY c_committee_id,l_name"   'chk
            MDCommon.CreateComboBoxNew(clsMdb, lstCommittee, strSql, "name", "id", False)

            strSql = "SELECT k_committee_kind AS id_name" &
                     " FROM committee WHERE c_ksh='" & MDLoginInfo.Ksh & "'" &
                     " GROUP BY k_committee_kind ORDER BY k_committee_kind DESC"    'chk
            MDCommon.CreateComboBoxNew(clsMdb, lstType, strSql, "id_name", "id_name", False)

            strSql = "SELECT l_name AS id_name" &
                     " FROM constant_dtl WHERE c_constant='BELONGING'" &
                     " ORDER BY c_constant_seq" 'chk
            MDCommon.CreateComboBoxNew(clsMdb, lstBelonging, strSql, "id_name", "id_name")
            lstType.Text = strType
            lstBelonging.Text = strBelonging
        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090106)

            If uc Is Nothing Then
                uc = New UC090106
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            Me.Dispose()
        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        '▼入力データチェック
        If checkData() = False Then
            Return
        End If
        If CLMsg.Show("GQ0001") = DialogResult.No Then
            Return
        End If

        Dim clsMdb As New CLAccessMdb
        Try
            '▼トランザクション開始
            Dim result As Integer = -1
            clsMdb.Connect()
            clsMdb.BeginTran()

            If cStrMode = "INSERT" Then
                '▼新規登録
                Me.txtId.Text = getMaxId(clsMdb)
                result = insertData(clsMdb, Me.txtId.Text, DataGridView1)
                ' 2018/12Add 暫定警告メッセージ追加
                ' 権限登録画面を開放していないため、注意喚起を行う
                CLMsg.Show("BW0200")
            ElseIf cStrMode = "ADDHISTORY" Then
                '▼履歴登録
                result = setEnd(clsMdb, Me.txtId.Text)
                If result <> -1 Then
                    result = insertData(clsMdb, Me.txtId.Text, DataGridView1)
                End If
            Else
                '▼更新
                result = updateData(clsMdb, Me.txtId.Text, DataGridView1)
            End If

            If result <> -1 Then
                '▼トランザクション終了
                clsMdb.CommitTran()
            Else
                '▼トランザクションロールバック
                clsMdb.RollbackTran()
            End If

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
            btnCancel_Click(sender, e)
        End Try
    End Sub

    Private Function checkData() As Boolean
        If cStrMode = "INSERT" And Me.txtName.Text.Trim.Length = 0 Then
            MsgBox("部／委員会名を入力して下さい。", vbExclamation, TITLE)
            Me.txtName.Focus()
            Return False
        End If

        If cStrMode = "INSERT" And Me.DataGridView1.Rows.Count = 0 Then
            MsgBox("明細行を作成して下さい。", vbExclamation, TITLE)
            Me.btnRowAdd.Focus()
            Return False
        Else
            Dim fError As Boolean = False
            Dim strErrors As String = ""
            Dim strError1 As String
            Dim strError2 As String
            Dim work As String

            For i = 0 To Me.DataGridView1.Rows.Count - 1
                strError1 = ""
                strError2 = ""
                Me.DataGridView1.Rows(i).Cells(1).Style.BackColor = Color.Empty
                Me.DataGridView1.Rows(i).Cells(2).Style.BackColor = Color.Empty
                Me.DataGridView1.Rows(i).Cells(3).Style.BackColor = Color.Empty
                Me.DataGridView1.Rows(i).Cells(6).Style.BackColor = Color.Empty
                Me.DataGridView1.Rows(i).Cells(7).Style.BackColor = Color.Empty

                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(1).Value) Then
                    strError1 = "[役職名]"
                    Me.DataGridView1.Rows(i).Cells(1).Style.BackColor = Color.LightPink
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(3).Value) Then
                    strError1 = strError1 & "[日当]"
                    Me.DataGridView1.Rows(i).Cells(3).Style.BackColor = Color.LightPink
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(6).Value) Then
                    strError1 = strError1 & "[任期開始]"
                    Me.DataGridView1.Rows(i).Cells(6).Style.BackColor = Color.LightPink
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(7).Value) Then
                    strError1 = strError1 & "[任期終了]"
                    Me.DataGridView1.Rows(i).Cells(7).Style.BackColor = Color.LightPink
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(2).Value) Then
                    strError1 = strError1 & "[任命最大数]"
                    Me.DataGridView1.Rows(i).Cells(2).Style.BackColor = Color.LightPink
                ElseIf Not MDChk.ChkNumber(Me.DataGridView1.Rows(i).Cells(2).Value) Then
                    strError2 = " [任命最大数]は数値を入力して下さい。"
                    Me.DataGridView1.Rows(i).Cells(2).Style.BackColor = Color.LightPink
                ElseIf Len(Me.DataGridView1.Rows(i).Cells(2).Value) > 5 Then
                    strError2 = " [任命最大数]は５桁以下の数値を入力して下さい。"
                    Me.DataGridView1.Rows(i).Cells(2).Style.BackColor = Color.LightPink
                End If
                If strError1 <> "" Or strError2 <> "" Then
                    fError = True
                    work = ""
                    If strError1 <> "" Then
                        Me.DataGridView1.Rows(i).ErrorText = strError1 & "が未入力です。" & strError2
                        'MsgBox(strError1 & "が未入力です。" & strError2)
                        work = strError1 & "が未入力です。" & strError2
                    Else
                        Me.DataGridView1.Rows(i).ErrorText = strError2
                        'MsgBox(strError2)
                        work = strError2
                    End If
                    If work <> "" Then
                        strErrors += vbCrLf & i + 1 & "行目：" & work
                    End If
                Else
                    Me.DataGridView1.Rows(i).ErrorText = ""
                End If
            Next i
            If fError Then
                MsgBox(strErrors, vbExclamation, TITLE)
                Return False
            End If
        End If
        Return True
    End Function

    Private Function getMaxId(ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Dim intId As Integer = 1
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql("SELECT max(c_committee_id) AS max_id FROM committee WHERE c_ksh='" & MDLoginInfo.Ksh & "'")
            intId = table.Rows(0)("max_id")
            intId = intId + 1

        Catch ex As Exception
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
        End Try
        Return Format(intId, "000")
    End Function

    Private Function insertData(ByVal clsMdb As CLAccessMdb, ByVal strId As String, ByVal dgv As DataGridView)
        Dim strName As String
        Dim strBiko As String
        Dim strHead As String
        Dim iMax As Integer
        Dim strDaily As String
        Dim strOfficer As String
        Dim strLunch As String
        Dim strStart As String
        Dim strEnd As String
        Dim iDiffStart As Integer
        Dim iDiffEnd As Integer
        Dim result As Integer

        result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO committee (c_committee_id, d_from, d_to, c_ksh, k_committee_kind, k_belonging, l_name, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", _
                                          strId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), MDLoginInfo.Ksh, Me.lstType.Text, Me.lstBelonging.Text, Me.txtName.Text, Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strHead = IIf(dgv.Rows(i).Cells(0).Value, "1", "0")
            strName = dgv.Rows(i).Cells(1).Value
            iMax = dgv.Rows(i).Cells(2).Value
            strDaily = dgv.Rows(i).Cells(3).Value
            strOfficer = MDCommon.NVL(dgv.Rows(i).Cells(4).Value)
            strLunch = MDCommon.NVL(dgv.Rows(i).Cells(5).Value)
            strStart = dgv.Rows(i).Cells(6).Value
            strEnd = dgv.Rows(i).Cells(7).Value
            iDiffStart = Integer.Parse(strStart) - Integer.Parse(Mid(PeriodFrom, 5, 2))
            iDiffEnd = Integer.Parse(strEnd) - Integer.Parse(Mid(PeriodTo, 5, 2))
            strBiko = MDCommon.NVL(dgv.Rows(i).Cells(8).Value)
            result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO committee_dtl (c_committee_id, s_committee_seq, d_from, d_to, l_name, s_appoint_max, c_daily_pay_id, c_officer_pay_id, c_executive_lunch_pay_id, d_service_from, d_service_to, s_from_diff, s_to_diff, l_biko, d_ins, c_user_id_ins, k_head_flg) VALUES('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}',{11},{12},'{13}','{14}','{15}','{16}')", _
                                              strId, i + 1, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), strName, iMax, strDaily, strOfficer, strLunch, strStart, strEnd, iDiffStart, iDiffEnd, strBiko, Date.Today.ToString, MDLoginInfo.UserId, strHead))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Function updateData(ByVal clsMdb As CLAccessMdb, ByVal strId As String, ByVal dgv As DataGridView)
        Dim strName As String
        Dim strBiko As String
        Dim strHead As String
        Dim iMax As Integer
        Dim strDaily As String
        Dim strOfficer As String
        Dim strLunch As String
        Dim strStart As String
        Dim strEnd As String
        Dim iDiffStart As Integer
        Dim iDiffEnd As Integer
        Dim result As Integer

        result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee SET k_committee_kind='{0}',k_belonging='{1}',l_name='{2}',l_biko='{3}',d_ins='{4}',c_user_id_ins='{5}' WHERE c_committee_id='{6}' AND d_from='{7}' AND d_to='{8}' AND c_ksh='{9}'", _
                                          Me.lstType.Text, Me.lstBelonging.Text, Me.txtName.Text, Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId, strId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), MDLoginInfo.Ksh))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strHead = IIf(dgv.Rows(i).Cells(0).Value, "1", "0")
            strName = dgv.Rows(i).Cells(1).Value
            iMax = dgv.Rows(i).Cells(2).Value
            strDaily = dgv.Rows(i).Cells(3).Value
            strOfficer = MDCommon.NVL(dgv.Rows(i).Cells(4).Value)
            strLunch = MDCommon.NVL(dgv.Rows(i).Cells(5).Value)
            strStart = dgv.Rows(i).Cells(6).Value
            strEnd = dgv.Rows(i).Cells(7).Value
            iDiffStart = Integer.Parse(strStart) - Integer.Parse(Mid(PeriodFrom, 5, 2))
            iDiffEnd = Integer.Parse(strEnd) - Integer.Parse(Mid(PeriodTo, 5, 2))
            strBiko = MDCommon.NVL(dgv.Rows(i).Cells(8).Value)
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee_dtl SET l_name='{0}',s_appoint_max={1},c_daily_pay_id='{2}',c_officer_pay_id='{3}',c_executive_lunch_pay_id='{4}',d_service_from='{5}',d_service_to='{6}',s_from_diff={7},s_to_diff={8},l_biko='{9}',d_ins='{10}',c_user_id_ins='{11}',k_head_flg='{12}' WHERE c_committee_id='{13}' AND s_committee_seq='{14}' AND d_from='{15}' AND d_to='{16}'", _
                                              strName, iMax, strDaily, strOfficer, strLunch, strStart, strEnd, iDiffStart, iDiffEnd, strBiko, Date.Today.ToString, MDLoginInfo.UserId, strHead, strId, i + 1, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Function setEnd(ByVal clsMdb As CLAccessMdb, ByVal strId As String)
        Dim result As Integer

        '▼データの更新
        Dim strFrom As String = Format(DateValue(cStrFromData), "yyyyMMdd")
        Dim strTo As String = Format(DateValue(Me.cboFromDate.Text).AddDays(-1), "yyyyMMdd")

        result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee SET d_to='{0}' WHERE c_committee_id='{1}' AND d_from='{2}' AND d_to='99999999' AND c_ksh='{3}'", _
                                          strTo, strId, strFrom, MDLoginInfo.Ksh))
        If result <> -1 Then
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee_dtl SET d_to='{0}' WHERE c_committee_id='{1}' and d_from='{2}' and d_to='99999999'", _
                                              strTo, strId, strFrom))
        End If

        Return result
    End Function

    Private Sub btnRowAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRowAdd.Click
        Me.DataGridView1.Rows.Add()
        'Me.DataGridView1.Rows(Me.DataGridView1.Rows.Count - 1).Cells(4).Value = Format(Date.Today, "yyyy/MM/dd")
        'Me.DataGridView1.Rows(Me.DataGridView1.Rows.Count - 1).Cells(5).Value = MDLoginInfo.UserId
    End Sub

    Private Sub btnRowDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRowDelete.Click
        If Not Me.DataGridView1.CurrentCell Is Nothing Then
            Me.DataGridView1.Rows.RemoveAt(Me.DataGridView1.CurrentCell.RowIndex)
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Me.DataGridView1.Columns.Clear()
        loadDetailData(Me.lstCommittee.SelectedValue)
    End Sub

    Private Sub loadDetailData(ByVal strId As String, Optional ByVal strFromDate As String = "", Optional ByVal strToDate As String = "")
        Dim table As New DataTable
        Dim clsMdb As New CLAccessMdb
        Try
            '▼SQL実行
            clsMdb.Connect()
            'todo:
            If strFromDate = "" And strToDate = "" Then
                table = clsMdb.ExecuteSql(String.Format("SELECT MST.k_head_flg, MST.l_name, MST.s_appoint_max, MST.c_daily_pay_id, MST.c_officer_pay_id, MST.c_executive_lunch_pay_id, MST.d_service_from, MST.d_service_to, MST.l_biko " &
                                                        "FROM committee_dtl AS MST, (SELECT M.c_committee_id AS max_id, MAX(M.d_from) AS max_d_from FROM committee_dtl AS M WHERE M.d_from<='{0}' And '{1}'<=M.d_to GROUP BY M.c_committee_id) AS MT " &
                                                        "WHERE MT.max_id=MST.c_committee_id AND MT.max_d_from=MST.d_from AND MST.c_committee_id='{2}' ORDER BY MST.c_committee_id, MST.s_committee_seq, MST.d_from" & UtDb.DbOrderOffset,
                                                        MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today), strId))
            Else
                table = clsMdb.ExecuteSql(String.Format("SELECT DTL.k_head_flg, DTL.l_name, DTL.s_appoint_max, DTL.c_daily_pay_id, DTL.c_officer_pay_id, DTL.c_executive_lunch_pay_id, DTL.d_service_from, DTL.d_service_to, DTL.l_biko " & 'ok
                                                        "FROM committee_dtl AS DTL " &
                                                        "WHERE c_committee_id='{0}' AND DTL.d_from='{1}' AND DTL.d_to='{2}' ORDER BY c_committee_id,s_committee_seq,DTL.d_from" & UtDb.DbOrderOffset, strId, MDMasterCommon.DateValueStr(strFromDate), MDMasterCommon.DateValueStr(strToDate)))
            End If

            addCheckBoxColumn("*")
            DataGridView1.Columns.Add("役職", "役職")
            DataGridView1.Columns.Add("任命最大数", "任命最大数")
            addDailyPayListColumn("日当", clsMdb)
            addOfficerPayListColumn("役員手当", clsMdb)
            addLunchPayListColumn("昼食費", clsMdb)
            addMMListColumn("任期開始日")
            addMMListColumn("任期終了日")
            DataGridView1.Columns.Add("備考", "備考")

            For Each Row As DataRow In table.Rows
                Dim item As New DataGridViewRow
                item.CreateCells(DataGridView1)

                With item
                    .Cells(0).Value = (Row.Item(0) = "1")
                    .Cells(1).Value = Row.Item(1)
                    .Cells(2).Value = Row.Item(2)
                    .Cells(3).Value = Row.Item(3)
                    .Cells(4).Value = Row.Item(4)
                    .Cells(5).Value = Row.Item(5)
                    .Cells(6).Value = Row.Item(6)
                    .Cells(7).Value = Row.Item(7)
                    .Cells(8).Value = Row.Item(8)
                End With
                DataGridView1.Rows.Add(item)
            Next

            '▼セルスタイルの設定
            DataGridView1.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridView1.Columns(0).DefaultCellStyle.GetHashCode()
            DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns(2).DefaultCellStyle.Format = "#,0"
            'DataGridView1.Columns(4).ReadOnly = True
            'DataGridView1.Columns(4).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.LightPink)
            'DataGridView1.Columns(5).ReadOnly = True
            'DataGridView1.Columns(5).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info)

            '▼行ヘッダーの幅を調節する
            DataGridView1.AutoResizeRowHeadersWidth( _
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            DataGridView1.Columns(0).Width = 20
            DataGridView1.Columns(2).Width = 85
            DataGridView1.Columns(3).Width = 150
            DataGridView1.Columns(4).Width = 200
            DataGridView1.Columns(5).Width = 70
            DataGridView1.Columns(6).Width = 70
            DataGridView1.Columns(7).Width = 70
            DataGridView1.Columns(8).Width = 120
            DataGridView1.AllowUserToAddRows = False

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Sub addCheckBoxColumn(ByVal strColumnName As String)
        Dim column As New DataGridViewCheckBoxColumn()
        column.HeaderText = strColumnName
        column.DataPropertyName = strColumnName
        DataGridView1.Columns.Add(column)
    End Sub

    Private Sub addDailyPayListColumn(ByVal strColumnName As String, ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql(String.Format("SELECT MST.c_daily_pay_id,MST.l_name FROM daily_pay_master AS MST, (SELECT M.c_daily_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM daily_pay_master AS M WHERE M.d_from<'{0}' And '{1}'<=M.d_to And M.c_ksh='{2}' GROUP BY M.c_daily_pay_id) AS MT WHERE MT.max_id=MST.c_daily_pay_id AND MT.max_d_from=MST.d_from ORDER BY MST.c_daily_pay_id,MST.d_from" & UtDb.DbOrderOffset, MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today), MDLoginInfo.Ksh)) 'ok

            '▼コンボボックス作成
            makeListColumn(strColumnName, table)
        Finally
            '▼後処理
            table.Dispose()
        End Try
    End Sub

    Private Sub addOfficerPayListColumn(ByVal strColumnName As String, ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql(String.Format("SELECT MST.c_officer_pay_id,FORMAT(MST.s_officer_pay,'#,0') & '：' & MST.l_name FROM officer_pay_master AS MST, (SELECT M.c_officer_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM officer_pay_master AS M WHERE M.d_from<'{0}' And '{1}'<=M.d_to And M.c_ksh='{2}' GROUP BY M.c_officer_pay_id) AS MT WHERE MT.max_id=MST.c_officer_pay_id AND MT.max_d_from=MST.d_from ORDER BY MST.c_officer_pay_id,MST.d_from" & UtDb.DbOrderOffset, MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today), MDLoginInfo.Ksh)) 'ok

            '▼コンボボックス作成
            makeListColumn(strColumnName, table)
        Finally
            '▼後処理
            table.Dispose()
        End Try
    End Sub

    Private Sub addLunchPayListColumn(ByVal strColumnName As String, ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql(String.Format("SELECT MST.c_executive_lunch_pay_id,FORMAT(MST.s_pay,'#,0') FROM executive_lunch_pay_master AS MST, (SELECT M.c_executive_lunch_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM executive_lunch_pay_master AS M WHERE M.d_from<'{0}' And '{1}'<=M.d_to GROUP BY M.c_executive_lunch_pay_id) AS MT WHERE MT.max_id=MST.c_executive_lunch_pay_id AND MT.max_d_from=MST.d_from ORDER BY MST.c_executive_lunch_pay_id,MST.d_from" & UtDb.DbOrderOffset, MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today)))  'ok

            '▼コンボボックス作成
            makeListColumn(strColumnName, table)
        Finally
            '▼後処理
            table.Dispose()
        End Try
    End Sub

    Private Sub makeListColumn(ByVal strColumnName As String, ByVal table As DataTable)
        '▼コンボボックス作成
        Dim list As New DataTable(strColumnName)
        list.Columns.Add("Display", GetType(String))
        list.Columns.Add("Value", GetType(String))
        If table.Rows.Count > 0 Then
            For Each Row As DataRow In table.Rows
                list.Rows.Add(Row.Item(1), Row.Item(0))
            Next
        End If
        list.Rows.Add("なし", "")

        Dim column As New DataGridViewComboBoxColumn()
        column.HeaderText = strColumnName
        column.DataPropertyName = strColumnName
        column.DataSource = list
        column.ValueMember = "Value"
        column.DisplayMember = "Display"
        DataGridView1.Columns.Add(column)
    End Sub

    Private Sub addMMListColumn(ByVal strColumnName As String)
        Dim column As New DataGridViewComboBoxColumn()
        column.HeaderText = strColumnName
        column.DataPropertyName = strColumnName
        '▼コンボボックス作成
        For i As Integer = 1 To 12
            column.Items.Add(Format(i, "00"))
        Next
        'column.ValueMember = "value"
        'column.DisplayMember = "display"
        DataGridView1.Columns.Add(column)
    End Sub

    Private Function addMMCombo() As DataGridViewComboBoxCell
        Dim comboBoxCol As New DataGridViewComboBoxCell

        '▼コンボボックス作成
        For i As Integer = 1 To 12
            comboBoxCol.Items.Add(Format(i, "00"))
        Next
        Return comboBoxCol
    End Function

    'Private Sub addNumColumn(ByVal strColumnName As String)
    '    'テーブルスタイルの取得
    '    Dim ts As DataGridTableStyle
    '    ts = DataGridView1.TableStyles("DataTable1")

    '    'DataGridTextBoxColumnの取得
    '    Dim cs As DataGridTextBoxColumn = _
    '        CType(ts.GridColumnStyles(0), DataGridTextBoxColumn)

    '    'TextBoxの取得
    '    Dim tb As TextBox = cs.TextBox

    '    'KeyPressイベントハンドラを追加
    '    AddHandler tb.KeyPress, AddressOf tb_KeyPress
    'End Sub

    'Private Sub tb_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
    '    '0-9の文字のみを許可する
    '    If e.KeyChar < "0"c Or e.KeyChar > "9"c Then
    '        e.Handled = True
    '    End If
    'End Sub

    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)

        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty Then
            Exit Sub
        End If

        If dgv.Columns(e.ColumnIndex).Name = "役職" AndAlso _
                e.FormattedValue.ToString().Length > 50 Then
            e.Cancel = True
        End If
        If dgv.Columns(e.ColumnIndex).Name = "任命最大数" AndAlso _
                e.FormattedValue.ToString().Length > 10 Then
            e.Cancel = True
        End If
        If dgv.Columns(e.ColumnIndex).Name = "備考" AndAlso _
                e.FormattedValue.ToString().Length > 100 Then
            e.Cancel = True
        End If
    End Sub

    Private Sub DataGridView1_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        If DataGridView1.CurrentCellAddress.X = 0 AndAlso _
            DataGridView1.IsCurrentCellDirty Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub DataGridView1_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        'チェックボックス列  
        If e.ColumnIndex = 0 Then
            If DataGridView1(e.ColumnIndex, e.RowIndex).Value Then
                '自分以外のチェックを解除
                For rowIndex As Integer = 0 To DataGridView1.RowCount - 1
                    If rowIndex <> e.RowIndex And DataGridView1(0, rowIndex).Value Then
                        DataGridView1(0, rowIndex).Value = False
                        DataGridView1(0, rowIndex).ReadOnly = False
                    End If
                Next
                'イベント発生抑制の為、ReadOnlyに設定  
                DataGridView1(e.ColumnIndex, e.RowIndex).ReadOnly = True
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEnter
        Select Case e.ColumnIndex
            Case 1, 8
                DataGridView1.ImeMode = Windows.Forms.ImeMode.Hiragana
            Case 0, 2 To 7
                DataGridView1.ImeMode = Windows.Forms.ImeMode.Disable
        End Select
    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
