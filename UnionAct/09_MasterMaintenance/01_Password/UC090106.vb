'===========================================================================================================
'   クラスＩＤ　　：UC090107
'   クラス名称　　：委員会マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Public Class UC090106
    Private Const TITLE As String = "委員会マスタメンテナンス"

    Private Sub UC090112_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼コントロール初期化
            'Me.cboToDate.MinDate = MDMasterCommon.GetMonthLastDate(Now)
            Me.cboToDate.MinDate = MDMasterCommon.GetMonthTopDate(Now)
            Me.cboToDate.Value = Me.cboToDate.MinDate

            '▼データの検索
            clsMdb.Connect()
            Dim strDate As String = Format(Date.Today, "yyyyMMdd")
            Dim strSql = String.Format("SELECT committee.c_committee_id AS Id,committee.l_name,committee.k_committee_kind" &
                                      ",committee.k_belonging," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_to") &
                                      ",committee.l_biko,committee.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM committee" &
                                      " LEFT JOIN staf_attribute_latest_view ON committee.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " WHERE (committee.d_from<='{0}' AND committee.d_to='" & MDMasterCommon.DEFAULT_TO_DATE & "')" &
                                      " OR (committee.d_to<>'" & MDMasterCommon.DEFAULT_TO_DATE & "'" &
                                      " AND committee.d_from<='{1}' AND '{2}'<=committee.d_to) AND committee.c_ksh='{3}'" &
                                      " ORDER BY committee.c_committee_id,committee.d_to", strDate, strDate, strDate, MDLoginInfo.Ksh)
            table = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)

            '▼値の表示
            MakeGridView(table, Me.DataGridView1)

            table = clsMdb.ExecuteSql("SELECT l_name AS id_name" & _
                                      " FROM committee WHERE c_ksh='" & MDLoginInfo.Ksh & "'" & _
                                      " GROUP BY c_committee_id,l_name" & _
                                      " ORDER BY c_committee_id,l_name")
            '▼値の表示
            lstPayId.DataSource = table
            lstPayId.DisplayMember = "id_name"

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            'table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub
    Private Sub btnShowDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowDetail.Click
        Try
            Dim pn As Panel
            Dim uc As UC090107
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String
            Dim strType As String
            Dim strBelonging As String

            If Me.DataGridView1.SelectedRows.Count <> 1 Then
                MsgBox("行を選択して下さい。", vbExclamation, TITLE)
                Return
            Else
                strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
                strName = Me.DataGridView1.SelectedRows(0).Cells(1).Value
                strType = Me.DataGridView1.SelectedRows(0).Cells(2).Value
                If Not Me.DataGridView1.SelectedRows(0).Cells(3).Value Is DBNull.Value Then
                    strBelonging = Me.DataGridView1.SelectedRows(0).Cells(3).Value
                Else
                    strBelonging = ""
                End If
                strFromDate = Me.DataGridView1.SelectedRows(0).Cells(4).Value
                strToDate = Me.DataGridView1.SelectedRows(0).Cells(5).Value
                If Not Me.DataGridView1.SelectedRows(0).Cells(6).Value Is DBNull.Value Then
                    strBiko = Me.DataGridView1.SelectedRows(0).Cells(6).Value
                Else
                    strBiko = ""
                End If
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090107)

            If uc Is Nothing Then
                uc = New UC090107
                uc.setData(strId, strFromDate, strToDate, strName, strBiko, strType, strBelonging)
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
            Dim uc As UC090107

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090107)

            If uc Is Nothing Then
                uc = New UC090107
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

        If Me.DataGridView1.SelectedRows(0).Cells(5).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
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
            Dim strFrom As String = MDMasterCommon.DateValueStr(Me.DataGridView1.SelectedRows(0).Cells(4).Value)
            Dim strTo As String = MDMasterCommon.DateValueStr(MDMasterCommon.GetMonthLastDate(Me.cboToDate.Value))
            Dim result As Integer = -1
            clsMdb.Connect()
            clsMdb.BeginTran()
            result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_committee_id='{3}' AND d_from='{4}' AND d_to='99999999' AND c_ksh='{5}'", _
                                              strTo, Date.Today.ToString, MDLoginInfo.UserId, strId, strFrom, MDLoginInfo.Ksh))
            If result <> -1 Then
                result = clsMdb.ExecuteNonQuery(String.Format("UPDATE committee_dtl SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_committee_id='{3}' and d_from='{4}' and d_to='99999999'", _
                                                  strTo, Date.Today.ToString, MDLoginInfo.UserId, strId, strFrom))
            End If

            If result <> -1 Then
                clsMdb.CommitTran()
                Me.DataGridView1.SelectedRows(0).Cells(5).Value = Format(MDMasterCommon.GetMonthLastDate(Me.cboToDate.Value), "yyyy/MM/dd")
            Else
                clsMdb.RollbackTran()
            End If

        Catch ex As Exception
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

        If Me.DataGridView1.SelectedRows(0).Cells(5).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
            MsgBox("適用終了日付が'" & MDMasterCommon.DEFAULT_TO_DATE_STR & "'以外は履歴登録できません。", vbExclamation, TITLE)
            Return
        End If

        Try
            Dim pn As Panel
            Dim uc As UC090107
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String
            Dim strType As String
            Dim strBelonging As String

            strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
            strName = Me.DataGridView1.SelectedRows(0).Cells(1).Value
            strType = Me.DataGridView1.SelectedRows(0).Cells(2).Value
            If Not Me.DataGridView1.SelectedRows(0).Cells(3).Value Is DBNull.Value Then
                strBelonging = Me.DataGridView1.SelectedRows(0).Cells(3).Value
            Else
                strBelonging = ""
            End If
            strFromDate = Me.DataGridView1.SelectedRows(0).Cells(4).Value
            strToDate = Me.DataGridView1.SelectedRows(0).Cells(5).Value
            If Not Me.DataGridView1.SelectedRows(0).Cells(6).Value Is DBNull.Value Then
                strBiko = Me.DataGridView1.SelectedRows(0).Cells(6).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090107)

            If uc Is Nothing Then
                uc = New UC090107
                uc.setData(strId, strFromDate, strToDate, strName, strBiko, strType, strBelonging)
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
            dgv.Columns(0).HeaderText = "委員会ID"
            dgv.Columns(1).HeaderText = "名称"
            dgv.Columns(2).HeaderText = "委員会種別"
            dgv.Columns(3).HeaderText = "組合支部"
            dgv.Columns(4).HeaderText = "適用開始日付"
            dgv.Columns(5).HeaderText = "適用終了日付"
            dgv.Columns(6).HeaderText = "備考"
            dgv.Columns(7).HeaderText = "更新日"
            dgv.Columns(8).HeaderText = "更新者"
            dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            '▼行ヘッダーに行番号を表示する
            dgv.TopLeftHeaderCell.Value = "項番"
            'For i = 0 To dgv.Rows.Count - 1
            '    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
            'Next i

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.RowHeadersWidth = 50
            dgv.Columns(0).Width = 80
            dgv.Columns(1).Width = 180
            dgv.Columns(2).Width = 120
            dgv.Columns(3).Width = 80
            dgv.Columns(4).Width = 100
            dgv.Columns(5).Width = 100
            dgv.Columns(6).Width = 180
            dgv.Columns(7).Width = 100
            dgv.Columns(8).Width = 100
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

        If MDMasterCommon.IsFutureMonth(Me.DataGridView2.SelectedRows(0).Cells(4).Value) = False Then
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
            Dim strFrom As String = MDMasterCommon.DateValueStr(Me.DataGridView2.SelectedRows(0).Cells(4).Value)
            Dim strTo As String = MDMasterCommon.DateValueStr(Me.DataGridView2.SelectedRows(0).Cells(5).Value)
            Dim result As Integer
            clsMdb.Connect()
            clsMdb.BeginTran()
            result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM committee WHERE c_committee_id='{0}' and d_from='{1}' and d_to='{2}' and c_ksh='{3}'", _
                                            strId, strFrom, strTo, MDLoginInfo.Ksh))
            If result <> -1 Then
                result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM committee_dtl WHERE c_committee_id='{0}' and d_from='{1}' and d_to='{2}'", _
                                                strId, strFrom, strTo))
            End If

            If result <> -1 Then
                '▼同一ID最新レコードの適用日付更新
                result = updateNextMaxTodate(strId, clsMdb)
            End If

            If result <> -1 Then
                clsMdb.CommitTran()
                Me.DataGridView2.Rows.RemoveAt(Me.DataGridView2.SelectedRows(0).Index)
            Else
                clsMdb.RollbackTran()
            End If

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Function updateNextMaxTodate(ByVal strId As String, ByVal clsMdb As CLAccessMdb) As Integer
        Dim strSql As String = String.Format("UPDATE committee SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_committee_id='{0}' and c_ksh='{1}' AND d_to=(SELECT max(d_to) AS max_d_to FROM committee" & _
                                             " WHERE c_committee_id='{2}' and c_ksh='{3}')", strId, MDLoginInfo.Ksh, strId, MDLoginInfo.Ksh)
        log.Debug(strSql)
        Dim result As Integer = clsMdb.ExecuteNonQuery(strSql)

        If result = -1 Then Return -1

        strSql = String.Format("UPDATE committee_dtl SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_committee_id='{0}' AND d_to=(SELECT max(d_to) AS max_d_to FROM committee_dtl" & _
                                             " WHERE c_committee_id='{1}')", strId, strId)
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
            Dim uc As UC090107
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strBiko As String
            Dim strType As String
            Dim strBelonging As String

            strId = Me.DataGridView2.SelectedRows(0).Cells(0).Value
            strName = Me.DataGridView2.SelectedRows(0).Cells(1).Value
            strType = Me.DataGridView2.SelectedRows(0).Cells(2).Value
            If Not Me.DataGridView2.SelectedRows(0).Cells(3).Value Is DBNull.Value Then
                strBelonging = Me.DataGridView2.SelectedRows(0).Cells(3).Value
            Else
                strBelonging = ""
            End If
            strFromDate = Me.DataGridView2.SelectedRows(0).Cells(4).Value
            strToDate = Me.DataGridView2.SelectedRows(0).Cells(5).Value
            If Not Me.DataGridView2.SelectedRows(0).Cells(6).Value Is DBNull.Value Then
                strBiko = Me.DataGridView2.SelectedRows(0).Cells(6).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090107)

            If uc Is Nothing Then
                uc = New UC090107
                uc.setData(strId, strFromDate, strToDate, strName, strBiko, strType, strBelonging)
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

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼データの検索
            Dim strWhere As String = ""
            If Me.lstPayId.Text.Length > 0 Then
                strWhere = String.Format("WHERE committee.l_name LIKE '{0}%'", Me.lstPayId.Text)
            End If
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT committee.c_committee_id AS Id,committee.l_name,committee.k_committee_kind" &
                                      ",committee.k_belonging," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_to") &
                                      ",committee.l_biko,committee.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM committee" &
                                      " LEFT JOIN staf_attribute_latest_view ON committee.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " {0} ORDER BY committee.c_committee_id,committee.d_from ASC", strWhere))

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

    Private Sub lstPayId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lstPayId.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Dim clsMdb As New CLAccessMdb
            Dim table As New DataTable
            Try
                '▼データの検索
                Dim strWhere As String = ""
                If Me.lstPayId.Text.Length > 0 Then
                    strWhere = String.Format("WHERE committee.l_name LIKE '{0}%'", Me.lstPayId.Text)
                End If
                clsMdb.Connect()
                table = clsMdb.ExecuteSql(String.Format("SELECT committee.c_committee_id AS Id,committee.l_name,committee.k_committee_kind" &
                                          ",committee.k_belonging," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("committee.d_to") &
                                          ",committee.l_biko,committee.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM committee" &
                                          " LEFT JOIN staf_attribute_latest_view ON committee.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                          " {0} ORDER BY committee.c_committee_id,committee.d_from ASC", strWhere))

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

        End If

    End Sub

End Class
