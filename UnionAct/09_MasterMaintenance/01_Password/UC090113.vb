'===========================================================================================================
'   クラスＩＤ　　：UC090113
'   クラス名称　　：日当マスタメンテナンス - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon

Public Class UC090113

    Private Const TITLE As String = "日当マスタメンテナンス"
    Private cStrMode As String
    Private cStrFromData As String

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
            'Me.btnConfirm.Text = "内容変更"
            Me.btnConfirm.Enabled = False
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
            Me.txtName.ReadOnly = True
            Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        ElseIf cStrMode = "UPDATEHISTORY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            'Me.btnConfirm.Text = "内容変更"
            Me.btnCancel.Text = "戻る"
            Me.btnRowAdd.Enabled = False
            Me.btnRowDelete.Enabled = False
            If MDMasterCommon.IsFutureMonth(cboFromDate.Value) = False Then
                Me.btnConfirm.Enabled = False
            Else
                Me.btnConfirm.Enabled = True
            End If
            Me.cboFromDate.Enabled = False
            Me.txtName.ReadOnly = True
            Me.txtName.BackColor = Color.FromKnownColor(KnownColor.Info)

        Else
            Me.cboFromDate.MinDate = MDMasterCommon.GetMonthTopDate(Date.Today)
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
        End If
    End Sub

    Private Sub UC090113_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If cStrMode = "ADDHISTORY" Then
            loadDetailData(Me.txtId.Text, cStrFromData, Me.txtToDate.Text)
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
            uc = pn.Controls(MDConst.SCREEN_ID_UC090112)

            If uc Is Nothing Then
                uc = New UC090112
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
        If cStrMode = "INSERT" And Me.txtName.Text.Length = 0 Then
            MsgBox("日当名称を入力して下さい。", vbExclamation, TITLE)
            Me.txtName.Focus()
            Return False
        End If

        If cStrMode = "INSERT" And Me.DataGridView1.Rows.Count = 0 Then
            MsgBox("明細行を作成して下さい。", vbExclamation, TITLE)
            Me.btnRowAdd.Focus()
            Return False
        Else
            Dim fError As Boolean = False
            Dim strError1 As String
            Dim strError2 As String
            For i = 0 To Me.DataGridView1.Rows.Count - 1
                strError1 = ""
                strError2 = ""
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(0).Value) Then
                    strError1 = "[表示名称]"
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(1).Value) Then
                    strError1 = strError1 & "[説明]"
                End If
                If MDChk.ChkNull(Me.DataGridView1.Rows(i).Cells(2).Value) Then
                    strError1 = strError1 & "[日当金額]"
                ElseIf Not MDChk.ChkNumber(Me.DataGridView1.Rows(i).Cells(2).Value) Then
                    strError2 = " [日当金額]は数値を入力して下さい。"
                End If
                If strError1 <> "" Or strError2 <> "" Then
                    fError = True
                    If strError1 <> "" Then
                        Me.DataGridView1.Rows(i).ErrorText = strError1 & "が未入力です。" & strError2
                    Else
                        Me.DataGridView1.Rows(i).ErrorText = strError2
                    End If
                Else
                    Me.DataGridView1.Rows(i).ErrorText = ""
                End If
            Next i
            If fError Then
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
            table = clsMdb.ExecuteSql("SELECT max(c_daily_pay_id) AS max_id FROM daily_pay_master WHERE c_ksh='" & MDLoginInfo.Ksh & "'")
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
        Dim strExplain As String
        Dim strPay As String
        Dim strBiko As String
        Dim result As Integer

        result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO daily_pay_master (c_daily_pay_id, l_name, d_from, d_to, c_ksh, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", _
                                          strId, Me.txtName.Text, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), MDLoginInfo.Ksh, Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strName = dgv.Rows(i).Cells(0).Value
            strExplain = dgv.Rows(i).Cells(1).Value
            strPay = dgv.Rows(i).Cells(2).Value
            strBiko = MDCommon.NVL(dgv.Rows(i).Cells(3).Value)
            result = clsMdb.ExecuteNonQuery(String.Format("INSERT INTO daily_pay_master_dtl (c_daily_pay_id, c_menu_seq, d_from, d_to, l_name, l_explain, s_daily_pay, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", _
                                              strId, i + 1, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), strName, strExplain, strPay, strBiko, Date.Today.ToString, MDLoginInfo.UserId))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Function updateData(ByVal clsMdb As CLAccessMdb, ByVal strId As String, ByVal dgv As DataGridView)
        Dim strName As String
        Dim strExplain As String
        Dim strPay As String
        Dim strBiko As String
        Dim result As Integer

        result = clsMdb.ExecuteNonQuery(String.Format("UPDATE daily_pay_master SET l_biko='{0}' WHERE c_daily_pay_id='{1}' AND d_from='{2}' AND d_to='{3}'", _
                                          Me.txtBiko.Text, strId, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
        If result = -1 Then Return -1

        For i = 0 To dgv.Rows.Count - 1
            strName = dgv.Rows(i).Cells(0).Value
            strExplain = dgv.Rows(i).Cells(1).Value
            strPay = dgv.Rows(i).Cells(2).Value
            strBiko = MDCommon.NVL(dgv.Rows(i).Cells(3).Value)
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE daily_pay_master_dtl SET l_name='{0}',l_explain='{1}',s_daily_pay={2},l_biko='{3}' WHERE c_daily_pay_id='{4}' AND c_menu_seq='{5}' AND d_from='{6}' AND d_to='{7}'", _
                                              strName, strExplain, strPay, strBiko, strId, i + 1, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
            If result = -1 Then Return -1
        Next i
        Return result
    End Function

    Private Function setEnd(ByVal clsMdb As CLAccessMdb, ByVal strId As String)
        Dim result As Integer

        '▼データの更新
        Dim strFrom As String = Format(DateValue(cStrFromData), "yyyyMMdd")
        Dim strTo As String = Format(DateValue(Me.cboFromDate.Text).AddDays(-1), "yyyyMMdd")

        result = clsMdb.ExecuteNonQuery(String.Format("UPDATE daily_pay_master SET d_to='{0}' WHERE c_daily_pay_id='{1}' AND d_from='{2}' AND d_to='99999999' AND c_ksh='{3}'", _
                                          strTo, strId, strFrom, MDLoginInfo.Ksh))
        If result <> -1 Then
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE daily_pay_master_dtl SET d_to='{0}' WHERE c_daily_pay_id='{1}' and d_from='{2}' and d_to='99999999'", _
                                              strTo, strId, strFrom))
        End If

        Return result
    End Function

    Private Sub btnRowAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRowAdd.Click
        Me.DataGridView1.Rows.Add()
        Me.DataGridView1.Rows(Me.DataGridView1.Rows.Count - 1).Cells(4).Value = Format(Date.Today, "yyyy/MM/dd")
        Me.DataGridView1.Rows(Me.DataGridView1.Rows.Count - 1).Cells(5).Value = MDLoginInfo.UserId
        setRowNumber()
    End Sub

    Private Sub btnRowDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRowDelete.Click
        Me.DataGridView1.Rows.RemoveAt(Me.DataGridView1.CurrentCell.RowIndex)
        setRowNumber()
    End Sub

    Private Sub setRowNumber()
        For i = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(i).HeaderCell.Value = (i + 1).ToString()
        Next i
    End Sub

    Private Sub loadDetailData(ByVal strId As String, ByVal strFromDate As String, ByVal strToDate As String)
        Dim table As New DataTable
        Dim clsMdb As New CLAccessMdb
        Try
            '▼SQL実行
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT l_name,l_explain,s_daily_pay,l_biko,Format(d_ins,'yyyy/MM/dd'),c_user_id_ins FROM daily_pay_master_dtl WHERE c_daily_pay_id='{0}' AND d_from='{1}' AND d_to='{2}' ORDER BY c_daily_pay_id,c_menu_seq,d_from", strId, MDMasterCommon.DateValueStr(strFromDate), MDMasterCommon.DateValueStr(strToDate)))

            DataGridView1.Columns.Add("表示名称", "表示名称")
            DataGridView1.Columns.Add("説明", "説明")
            DataGridView1.Columns.Add("日当金額", "日当金額")
            DataGridView1.Columns.Add("備考", "備考")
            DataGridView1.Columns.Add("作成日", "作成日")
            DataGridView1.Columns.Add("作成者", "作成者")

            For Each Row As DataRow In table.Rows
                Dim item As New DataGridViewRow
                item.CreateCells(DataGridView1)

                With item
                    .Cells(0).Value = Row.Item(0)
                    .Cells(1).Value = Row.Item(1)
                    .Cells(2).Value = Row.Item(2)
                    .Cells(3).Value = Row.Item(3)
                    .Cells(4).Value = Row.Item(4)
                    .Cells(5).Value = Row.Item(5)
                End With
                DataGridView1.Rows.Add(item)
            Next

            '▼行ヘッダーに行番号を表示する
            DataGridView1.TopLeftHeaderCell.Value = "項番"
            setRowNumber()

            '▼セルスタイルの設定
            DataGridView1.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns(2).DefaultCellStyle.Format = "#,0"
            DataGridView1.Columns(4).ReadOnly = True
            DataGridView1.Columns(4).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info)
            DataGridView1.Columns(5).ReadOnly = True
            DataGridView1.Columns(5).DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Info)

            '▼行ヘッダーの幅を調節する
            DataGridView1.AutoResizeRowHeadersWidth( _
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            'dgv.Columns(1).Width = 200
            'dgv.Columns(5).Width = 200
            'dgv.Columns(6).Width = 0
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

    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)

        If e.RowIndex = dgv.NewRowIndex OrElse Not dgv.IsCurrentCellDirty Then
            Exit Sub
        End If

        If dgv.Columns(e.ColumnIndex).Name = "表示名称" AndAlso _
                e.FormattedValue.ToString().Length > 50 Then
            e.Cancel = True
        End If
        If dgv.Columns(e.ColumnIndex).Name = "説明" AndAlso _
                e.FormattedValue.ToString().Length > 100 Then
            e.Cancel = True
        End If
        If dgv.Columns(e.ColumnIndex).Name = "日当金額" AndAlso _
                e.FormattedValue.ToString().Length > 10 Then
            e.Cancel = True
        End If
        If dgv.Columns(e.ColumnIndex).Name = "備考" AndAlso _
                e.FormattedValue.ToString().Length > 100 Then
            e.Cancel = True
        End If
    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
