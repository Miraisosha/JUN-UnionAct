'===========================================================================================================
'   クラスＩＤ　　：UC090131
'   クラス名称　　：専従職員権限マスタメンテナンス - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk

Public Class UC090131

    Private Const TITLE As String = "専従職員権限マスタメンテナンス"
    Private cStrMode As String
    Private cStrFromData As String

    Dim htControlMap As Hashtable = New Hashtable


    Public Sub setData(ByVal strId As String, ByVal strFromDate As String, ByVal strToDate As String, ByVal strName As String, ByVal strBiko As String)
        Me.txtId.Text = strId
        Me.cboFromDate.Value = strFromDate
        cStrFromData = strFromDate
        Me.txtToDate.Text = strToDate
        Me.txtName.Text = strName
        Me.txtBiko.Text = strBiko
    End Sub

    Public Sub setMode(ByVal strMode As String)
        cStrMode = strMode

        If cStrMode = "READONLY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            Me.btnConfirm.Text = "内容変更"
            Me.btnConfirm.Enabled = False
            Me.btnCancel.Text = "戻る"
            Me.cboFromDate.Enabled = False
            'Me.btnShowDetail.Enabled = False
            Me.txtName.ReadOnly = True
            Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        ElseIf cStrMode = "ADDHISTORY" Then
            Me.lbl.Text = TITLE & " - 履歴登録"
            Me.cboFromDate.MinDate = Me.cboFromDate.Value.AddDays(1)
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
            'Me.txtName.ReadOnly = True
            'Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        ElseIf cStrMode = "UPDATEHISTORY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            Me.btnConfirm.Text = "内容変更"
            Me.btnCancel.Text = "戻る"
            'Me.btnShowDetail.Enabled = False
            If MDMasterCommon.IsFutureMonth(cboFromDate.Value) = False Then
                Me.btnConfirm.Enabled = False
            Else
                Me.btnConfirm.Enabled = True
            End If
            Me.cboFromDate.Enabled = False
            'Me.txtName.ReadOnly = True
            'Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        Else
            Me.cboFromDate.MinDate = MDMasterCommon.GetNextStartDate(Now)
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
        End If
    End Sub

    Private Sub UC090131_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        initControlMap()
        If cStrMode = "ADDHISTORY" Then
            loadDetailData(Me.txtId.Text, cStrFromData, Me.txtToDate.Text)
        ElseIf cStrMode = "INSERT" Then
            addInitData()
        Else
            loadDetailData(Me.txtId.Text, Me.cboFromDate.Value, Me.txtToDate.Text)
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090130)

            If uc Is Nothing Then
                uc = New UC090130
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
            ElseIf cStrMode = "ADDHISTORY" Then
                '▼履歴登録
                result = insertData(clsMdb, Me.txtId.Text, DataGridView1)
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
        If cStrMode = "INSERT" And Me.txtName.Text.Length = 0 Then
            MsgBox("名称を入力して下さい。", vbExclamation, TITLE)
            Me.txtName.Focus()
            Return False
        End If
        Return True
    End Function

    Private Function getMaxId(ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Dim intId As Integer = 1
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql("SELECT max(c_full_time_control_id) AS max_id FROM full_time_control")
            intId = table.Rows(0)("max_id")
            intId = intId + 1

        Catch ex As Exception
            'MsgBox(ex.Message)
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
        End Try
        Return intId
    End Function

    Private Function insertData(ByVal clsMdb As CLAccessMdb, ByVal strId As String, ByVal dgv As DataGridView)
        Dim strControlId1 As String
        Dim strControlId2 As String
        Dim strControlId3 As String
        Dim strMenuId As String
        Dim result As Integer = -1

        result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO full_time_control (c_full_time_control_id, d_from, d_to, l_name, d_service_from, d_service_to, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", _
                                          strId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), Me.txtName.Text, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strControlId1 = convControlToId(dgv.Rows(i).Cells(1).Value)
            strControlId2 = convControlToId(dgv.Rows(i).Cells(2).Value)
            strControlId3 = convControlToId(dgv.Rows(i).Cells(3).Value)
            strMenuId = dgv.Rows(i).Cells(5).Value
            result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO full_time_control_dtl (c_full_time_control_id, c_menu_id, d_from, d_to, c_now_control_screen_kind, c_before_control_screen_kind, c_two_before_control_screen_kind, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", _
                                              strId, strMenuId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), strControlId1, strControlId2, strControlId3, "", Date.Today.ToString, MDLoginInfo.UserId))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Function updateData(ByVal clsMdb As CLAccessMdb, ByVal strId As String, ByVal dgv As DataGridView)
        Dim strControlId1 As String
        Dim strControlId2 As String
        Dim strControlId3 As String
        Dim strMenuId As String
        Dim result As Integer = -1

        result = clsMdb.ExecuteNonQuery(String.Format("UPDATE full_time_control SET l_name='{0}',l_biko='{1}' WHERE c_full_time_control_id={2} AND d_from='{3}' AND d_to='{4}'", _
                                          Me.txtName.Text, Me.txtBiko.Text, Integer.Parse(strId), MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strControlId1 = convControlToId(dgv.Rows(i).Cells(1).Value)
            strControlId2 = convControlToId(dgv.Rows(i).Cells(2).Value)
            strControlId3 = convControlToId(dgv.Rows(i).Cells(3).Value)
            strMenuId = dgv.Rows(i).Cells(5).Value
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE full_time_control_dtl SET c_now_control_screen_kind='{0}',c_before_control_screen_kind='{1}',c_two_before_control_screen_kind='{2}' WHERE c_full_time_control_id={3} AND c_menu_id='{4}' AND d_from='{5}' AND d_to='{6}'", _
                                              strControlId1, strControlId2, strControlId3, Integer.Parse(strId), strMenuId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Sub loadDetailData(ByVal strId As String, ByVal strFromDate As String, ByVal strToDate As String)
        Dim table As New DataTable
        Dim clsMdb As New CLAccessMdb
        Try
            '▼SQL実行
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT menucontrol.l_name, full_time_control_dtl.c_now_control_screen_kind, full_time_control_dtl.c_before_control_screen_kind, full_time_control_dtl.c_two_before_control_screen_kind, full_time_control_dtl.c_full_time_control_id, full_time_control_dtl.c_menu_id FROM full_time_control_dtl INNER JOIN menucontrol ON full_time_control_dtl.c_menu_id = menucontrol.c_menu_id WHERE c_full_time_control_id={0} AND full_time_control_dtl.d_from='{1}' AND full_time_control_dtl.d_to='{2}' ORDER BY full_time_control_dtl.c_menu_id", Integer.Parse(strId), MDMasterCommon.DateValueStr(strFromDate), MDMasterCommon.DateValueStr(strToDate)))

            '▼データ表示
            DataGridView1.Columns.Add("メニュー名称", "メニュー名称")
            DataGridView1.Columns.Add("現在期の操作権限", "現在期の操作権限")
            DataGridView1.Columns.Add("前期の操作権限", "前期の操作権限")
            DataGridView1.Columns.Add("前々期以前の操作権限", "前々期以前の操作権限")
            DataGridView1.Columns.Add("画面制御ID", "画面制御ID")
            DataGridView1.Columns.Add("メニューID", "メニューID")

            For Each Row As DataRow In table.Rows
                Dim item As New DataGridViewRow
                item.CreateCells(DataGridView1)

                With item
                    .Cells(0).Value = Row.Item(0)
                    .Cells(1) = makeControlCombo(clsMdb, Row.Item(5))
                    .Cells(2) = makeControlCombo(clsMdb, Row.Item(5))
                    .Cells(3) = makeControlCombo(clsMdb, Row.Item(5))
                    .Cells(1).Value = convControlIdToText(Row.Item(1))
                    .Cells(2).Value = convControlIdToText(Row.Item(2))
                    .Cells(3).Value = convControlIdToText(Row.Item(3))
                    .Cells(4).Value = Row.Item(4)
                    .Cells(5).Value = Row.Item(5)
                End With
                DataGridView1.Rows.Add(item)
            Next

            '▼セルスタイルの設定
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(0).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info)
            DataGridView1.Columns(4).Visible = False
            DataGridView1.Columns(5).Visible = False

            '▼行ヘッダーの幅を調節する
            DataGridView1.Columns(0).Width = 200
            DataGridView1.Columns(1).Width = 200
            DataGridView1.Columns(2).Width = 200
            DataGridView1.Columns(3).Width = 200
            DataGridView1.RowHeadersVisible = False
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

    Private Sub addInitData()
        Dim table As New DataTable
        Dim clsMdb As New CLAccessMdb
        Try
            '▼SQL実行
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT l_name,c_menu_id FROM menucontrol WHERE d_from<='{0}' AND d_to>='{1}' ORDER BY c_menu_id,d_from", MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today)))

            '▼データ表示
            DataGridView1.Columns.Add("メニュー名称", "メニュー名称")
            DataGridView1.Columns.Add("現在期の操作権限", "現在期の操作権限")
            DataGridView1.Columns.Add("前期の操作権限", "前期の操作権限")
            DataGridView1.Columns.Add("前々期以前の操作権限", "前々期以前の操作権限")
            DataGridView1.Columns.Add("画面制御ID", "画面制御ID")
            DataGridView1.Columns.Add("メニューID", "メニューID")

            For Each Row As DataRow In table.Rows
                Dim item As New DataGridViewRow
                item.CreateCells(DataGridView1)

                With item
                    .Cells(0).Value = Row.Item(0)
                    .Cells(1) = makeControlCombo(clsMdb, Row.Item(1))
                    .Cells(2) = makeControlCombo(clsMdb, Row.Item(1))
                    .Cells(3) = makeControlCombo(clsMdb, Row.Item(1))
                    .Cells(1).Value = "すべて処理不可"
                    .Cells(2).Value = "すべて処理不可"
                    .Cells(3).Value = "すべて処理不可"
                    .Cells(4).Value = ""
                    .Cells(5).Value = Row.Item(1)
                End With
                DataGridView1.Rows.Add(item)
            Next

            '▼セルスタイルの設定
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(0).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info)
            DataGridView1.Columns(4).Visible = False
            DataGridView1.Columns(5).Visible = False

            '▼行ヘッダーの幅を調節する
            DataGridView1.Columns(0).Width = 200
            DataGridView1.Columns(1).Width = 200
            DataGridView1.Columns(2).Width = 200
            DataGridView1.Columns(3).Width = 200
            DataGridView1.RowHeadersVisible = False
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

    Private Function makeControlCombo(ByVal clsMdb As CLAccessMdb, ByVal strMenuId As String) As DataGridViewComboBoxCell
        Dim table As New DataTable
        Dim comboBoxCol As DataGridViewComboBoxCell
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql(String.Format("SELECT l_power_name,c_control_screen_kind FROM control_dtl WHERE c_menu_id='{0}' AND d_from<='{1}' AND d_to>='{2}' ORDER BY c_menu_id,d_from", strMenuId, MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today)))

            '▼コンボボックス作成
            comboBoxCol = New DataGridViewComboBoxCell
            If table.Rows.Count > 0 Then
                For Each Row As DataRow In table.Rows
                    comboBoxCol.Items.Add(Row.Item(0))
                Next
            Else
                comboBoxCol.Items.Add("すべて処理不可")
            End If

        Finally
            '▼後処理
            table.Dispose()
        End Try
        Return comboBoxCol
    End Function

    Private Function makeControlCombo1(ByVal clsMdb As CLAccessMdb, ByVal strMenuId As String, ByVal strCaption As String) As DataGridViewComboBoxColumn
        Dim table As New DataTable
        Dim comboBoxCol As DataGridViewComboBoxColumn
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql(String.Format("SELECT l_power_name FROM control_dtl WHERE c_menu_id='{0}' AND d_from<='{1}' AND d_to>='{2}' ORDER BY c_menu_id,d_from", strMenuId, MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today)))

            '▼コンボボックス作成
            comboBoxCol = New DataGridViewComboBoxColumn
            comboBoxCol.HeaderText = strCaption
            For Each Row As DataRow In table.Rows
                comboBoxCol.Items.Add(Row.Item(0))
            Next

        Finally
            '▼後処理
            table.Dispose()
        End Try
        Return comboBoxCol
    End Function

    Private Sub initControlMap()
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼SQL実行
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT l_power_name,c_control_screen_kind FROM control_dtl WHERE d_from<='{0}' AND d_to>='{1}' ORDER BY c_menu_id,d_from", MDMasterCommon.DateValueStr(Date.Today), MDMasterCommon.DateValueStr(Date.Today)))

            '▼変換テーブルにデータ追加
            For Each Row As DataRow In table.Rows
                If Not htControlMap.ContainsKey(Row.Item(0).ToString.Trim) Then
                    htControlMap.Add(Row.Item(0).ToString.Trim, Row.Item(1))
                End If
            Next

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Function convControlToId(ByVal strControl As String) As String
        Return CType(htControlMap(strControl), String)
    End Function

    Private Function convControlIdToText(ByVal strControlId As String) As String
        Dim strResult As String = "すべて処理不可"

        If htControlMap.ContainsValue(strControlId) Then
            For Each de As DictionaryEntry In htControlMap
                If strControlId = de.Value Then
                    strResult = de.Key
                    Exit For
                End If
            Next
        End If

        Return strResult
    End Function

    Private Sub btnShowDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDetail.Click
        Dim dlg As FM090131 = New FM090131()

        If Me.DataGridView1.CurrentCellAddress.X < 1 Or Me.DataGridView1.CurrentCellAddress.X > 3 Then
            MsgBox("操作権限を選択して下さい。", vbExclamation, TITLE)
            Return
        End If

        Dim strPeriod As String
        If Me.DataGridView1.CurrentCellAddress.X = 1 Then
            strPeriod = "現在期"
        ElseIf Me.DataGridView1.CurrentCellAddress.X = 2 Then
            strPeriod = "前期"
        Else
            strPeriod = "前々期以前"
        End If

        dlg.initData(Me.DataGridView1.CurrentRow.Cells(0).Value, Me.DataGridView1.CurrentCell.Value, strPeriod, _
                        Me.DataGridView1.CurrentRow.Cells(5).Value, convControlToId(Me.DataGridView1.CurrentCell.Value), Me.DataGridView1.CurrentCellAddress.X)
        dlg.Owner = ParentForm
        dlg.Show()

    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
