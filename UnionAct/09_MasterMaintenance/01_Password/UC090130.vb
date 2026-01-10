'===========================================================================================================
'   クラスＩＤ　　：UC090130
'   クラス名称　　：専従職員権限マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Public Class UC090130
    Private Const TITLE As String = "専従職員権限マスタメンテナンス"

    Private Sub UC090112_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼コントロール初期化
            Me.cboToDate.MinDate = MDMasterCommon.GetNextStartDate(Now)
            Me.cboToDate.Value = Me.cboToDate.MinDate

            '▼データの検索
            Dim strDate As String = Format(Date.Today, "yyyyMMdd")
            clsMdb.Connect()
            Dim strSql = String.Format("SELECT Format(full_time_control.c_full_time_control_id,'000') AS Id" &
                                      ",full_time_control.l_name," & UtDb.DbStrYYYYMMDDtoDateText("full_time_control.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("full_time_control.d_to") &
                                      ",full_time_control.l_biko,full_time_control.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM full_time_control" &
                                      " LEFT JOIN staf_attribute_latest_view ON full_time_control.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " WHERE (full_time_control.d_from<='{0}' AND full_time_control.d_to='" & MDMasterCommon.DEFAULT_TO_DATE & "')" &
                                      " OR (full_time_control.d_to<>'" & MDMasterCommon.DEFAULT_TO_DATE & "'" &
                                      " AND full_time_control.d_from<='{1}' AND '{2}'<=full_time_control.d_to)" &
                                      " ORDER BY full_time_control.c_full_time_control_id,full_time_control.d_to", strDate, strDate, strDate)
            table = clsMdb.ExecuteSql(strSql)

            '▼値の表示
            MakeGridView(table, Me.DataGridView1)

            '▼データの検索
            table = clsMdb.ExecuteSql("SELECT Format(full_time_control.c_full_time_control_id,'000') AS Id" &
                                      ",full_time_control.l_name," & UtDb.DbStrYYYYMMDDtoDateText("full_time_control.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("full_time_control.d_to") &
                                      ",full_time_control.l_biko,full_time_control.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM full_time_control" &
                                      " LEFT JOIN staf_attribute_latest_view ON full_time_control.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " ORDER BY full_time_control.c_full_time_control_id,full_time_control.d_from ASC")

            '▼値の表示
            MakeGridView(table, Me.DataGridView2)

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub
    Private Sub btnShowDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDetail.Click
        Try
            Dim pn As Panel
            Dim uc As UC090131
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String

            If Me.DataGridView1.SelectedRows.Count <> 1 Then
                MsgBox("行を選択して下さい。", vbExclamation, TITLE)
                Return
            Else
                strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
                strName = Me.DataGridView1.SelectedRows(0).Cells(1).Value
                strFromDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
                strToDate = Me.DataGridView1.SelectedRows(0).Cells(3).Value
                strBiko = Me.DataGridView1.SelectedRows(0).Cells(4).Value
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090131)

            If uc Is Nothing Then
                uc = New UC090131
                uc.setData(strId, strFromDate, strToDate, strName, strBiko)
                uc.setMode("READONLY")
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

    Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        Try
            Dim pn As Panel
            Dim uc As UC090131

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090131)

            If uc Is Nothing Then
                uc = New UC090131
                uc.setMode("INSERT")
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

    Private Sub DataGridView1_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex < 0 And e.RowIndex >= 0 Then
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)

            '行番号を描画する
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), e.CellStyle.Font, indexRect, e.CellStyle.ForeColor, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        btnShowDetail_Click(sender, e)
    End Sub

    Private Sub btnSetEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetEnd.Click
        If Me.DataGridView1.SelectedRows.Count <> 1 Then
            MsgBox("行を選択して下さい。", vbExclamation, TITLE)
            Return
        End If

        If Me.DataGridView1.SelectedRows(0).Cells(3).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
            MsgBox("適用終了日付が'" & MDMasterCommon.DEFAULT_TO_DATE_STR & "'以外は終了設定できません。", vbExclamation, TITLE)
            Return
        End If

        If CLMsg.Show("GQ0037") = DialogResult.No Then
            Return
        End If

        Dim clsMdb As New CLAccessMdb
        Try
            '▼データの更新
            Dim strId As String = Me.DataGridView1.SelectedRows(0).Cells(0).Value
            Dim strFrom As String = MDMasterCommon.DateValueStr(Me.DataGridView1.SelectedRows(0).Cells(2).Value)
            Dim strTo As String = MDMasterCommon.DateValueStr(Me.DataGridView1.SelectedRows(0).Cells(3).Value)
            Dim result As Integer = -1
            clsMdb.Connect()
            clsMdb.BeginTran()
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE full_time_control SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_full_time_control_id={3} and d_from='{4}' and d_to='{5}'", _
                                              MDMasterCommon.DateValueStr(Me.cboToDate.Text), Date.Today.ToString, MDLoginInfo.UserId, Integer.Parse(strId), strFrom, strTo))

            If result <> -1 Then
                result = clsMdb.ExecuteNonQuery(String.Format("UPDATE full_time_control_dtl SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_full_time_control_id={3} and d_from='{4}' and d_to='{5}'", _
                                                  MDMasterCommon.DateValueStr(Me.cboToDate.Text), Date.Today.ToString, MDLoginInfo.UserId, Integer.Parse(strId), strFrom, strTo))
            End If

            If result <> -1 Then
                clsMdb.CommitTran()
                Me.DataGridView1.SelectedRows(0).Cells(3).Value = Format(Me.cboToDate.Value, "yyyy/MM/dd")
            Else
                clsMdb.RollbackTran()
            End If

        Catch ex As Exception
            clsMdb.RollbackTran()
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Sub btnInsertHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertHistory.Click
        If Me.DataGridView1.SelectedRows.Count <> 1 Then
            MsgBox("行を選択して下さい。", vbExclamation, TITLE)
            Return
        End If

        If Me.DataGridView1.SelectedRows(0).Cells(3).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
            MsgBox("適用終了日付が'" & MDMasterCommon.DEFAULT_TO_DATE_STR & "'以外は履歴登録できません。", vbExclamation, TITLE)
            Return
        End If

        Try
            Dim pn As Panel
            Dim uc As UC090131
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String

            strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
            strName = Me.DataGridView1.SelectedRows(0).Cells(1).Value
            strFromDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
            strToDate = Me.DataGridView1.SelectedRows(0).Cells(3).Value
            strBiko = Me.DataGridView1.SelectedRows(0).Cells(4).Value
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090131)

            If uc Is Nothing Then
                uc = New UC090131
                uc.setData(strId, strFromDate, strToDate, strName, strBiko)
                uc.setMode("ADDHISTORY")
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

    Private Sub MakeGridView(ByVal table As DataTable, ByRef dgv As DataGridView)
        Try
            '▼値の表示
            dgv.DataSource = table

            '▼ヘッダーの設定
            dgv.Columns(0).HeaderText = "専従職員ID"
            dgv.Columns(0).Visible = False
            dgv.Columns(1).HeaderText = "名称"
            dgv.Columns(2).HeaderText = "適用開始日付"
            dgv.Columns(3).HeaderText = "適用終了日付"
            dgv.Columns(4).HeaderText = "備考"
            dgv.Columns(5).HeaderText = "作成日"
            dgv.Columns(6).HeaderText = "作成者"

            '▼行ヘッダーに行番号を表示する
            dgv.TopLeftHeaderCell.Value = "項番"
            'For i = 0 To dgv.Rows.Count - 1
            '    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
            'Next i

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.RowHeadersWidth = 50
            dgv.Columns(1).Width = 150
            dgv.Columns(2).Width = 150
            dgv.Columns(3).Width = 130
            dgv.AllowUserToAddRows = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView2_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView2.CellPainting
        If e.ColumnIndex < 0 And e.RowIndex >= 0 Then
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)

            '行番号を描画する
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), e.CellStyle.Font, indexRect, e.CellStyle.ForeColor, TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.DoubleClick
        btnShowHistory_Click(sender, e)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Me.DataGridView2.SelectedRows.Count <> 1 Then
            MsgBox("行を選択して下さい。", vbExclamation, TITLE)
            Return
        End If

        If MDMasterCommon.IsFutureMonth(Me.DataGridView2.SelectedRows(0).Cells(2).Value) = False Then
            MsgBox("適用開始日付が現在より未来月の場合のみ、データを削除することができます。", vbExclamation, TITLE)
            Return
        End If

        If CLMsg.Show("GQ0031") = DialogResult.No Then
            Return
        End If

        Dim clsMdb As New CLAccessMdb
        Try
            '▼データの削除
            Dim strId As String = Me.DataGridView2.SelectedRows(0).Cells(0).Value
            Dim strFrom As String = MDMasterCommon.DateValueStr(Me.DataGridView2.SelectedRows(0).Cells(2).Value)
            Dim strTo As String = MDMasterCommon.DateValueStr(Me.DataGridView2.SelectedRows(0).Cells(3).Value)
            Dim result As Integer = -1
            clsMdb.Connect()
            clsMdb.BeginTran()
            result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM full_time_control WHERE c_full_time_control_id={0} and d_from='{1}' and d_to='{2}'", _
                                            Integer.Parse(strId), strFrom, strTo))
            If result <> -1 Then
                result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM full_time_control_dtl WHERE c_full_time_control_id={0} and d_from='{1}' and d_to='{2}'", _
                                                Integer.Parse(strId), strFrom, strTo))
            End If

            If result <> -1 Then
                '▼同一ID最新レコードの適用日付更新
                result = updateNextMaxTodate(Integer.Parse(strId), clsMdb)
            End If

            If result <> -1 Then
                clsMdb.CommitTran()
                Me.DataGridView2.Rows.RemoveAt(Me.DataGridView2.SelectedRows(0).Index)
            Else
                clsMdb.RollbackTran()
            End If


        Catch ex As Exception
            clsMdb.RollbackTran()
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Function updateNextMaxTodate(ByVal intId As Integer, ByVal clsMdb As CLAccessMdb) As Integer
        Dim strSql As String = String.Format("UPDATE full_time_control SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_full_time_control_id={0} AND d_to=(SELECT max(d_to) AS max_d_to FROM full_time_control" & _
                                             " WHERE c_full_time_control_id={1} )", intId, intId)
        log.Debug(strSql)
        Dim result As Integer = clsMdb.ExecuteNonQuery(strSql)

        If result = -1 Then Return -1

        strSql = String.Format("UPDATE full_time_control_dtl SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_full_time_control_id={0} AND d_to=(SELECT max(d_to) AS max_d_to FROM full_time_control_dtl" & _
                                             " WHERE c_full_time_control_id={1})", intId, intId)
        log.Debug(strSql)
        Return clsMdb.ExecuteNonQuery(strSql)
    End Function

    Private Sub btnShowHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowHistory.Click
        If Me.DataGridView2.SelectedRows.Count <> 1 Then
            MsgBox("行を選択して下さい。", vbExclamation, TITLE)
            Return
        End If

        Try
            Dim pn As Panel
            Dim uc As UC090131
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String

            strId = Me.DataGridView2.SelectedRows(0).Cells(0).Value
            strName = Me.DataGridView2.SelectedRows(0).Cells(1).Value
            strFromDate = Me.DataGridView2.SelectedRows(0).Cells(2).Value
            strToDate = Me.DataGridView2.SelectedRows(0).Cells(3).Value
            strBiko = Me.DataGridView2.SelectedRows(0).Cells(4).Value
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090131)

            If uc Is Nothing Then
                uc = New UC090131
                uc.setData(strId, strFromDate, strToDate, strName, strBiko)
                uc.setMode("UPDATEHISTORY")
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

    Private Sub loadData()
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼データの検索
            Dim strWhere As String = ""
            strWhere = String.Format("WHERE Format(full_time_control.c_full_time_control_id ,'000') LIKE '{0}%'", "")
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT Format(full_time_control.c_full_time_control_id,'000') AS Id" &
                                      ",full_time_control.l_name,full_time_control.d_from,full_time_control.d_to" &
                                      ",full_time_control.l_biko,full_time_control.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM full_time_control" &
                                      " INNER JOIN staf_attribute_latest_view ON full_time_control.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " {0} ORDER BY full_time_control.c_full_time_control_id,full_time_control.d_from ASC", strWhere))

            '▼値の表示
            MakeGridView(table, Me.DataGridView2)

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
