'===========================================================================================================
'   クラスＩＤ　　：UC090116
'   クラス名称　　：役員手当マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Public Class UC090116
    Private Const TITLE As String = "役員手当マスタメンテナンス"

    Private Sub UC090116_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼コントロール初期化
            Me.cboToDate.MinDate = MDMasterCommon.GetMonthTopDate(Now)
            Me.cboToDate.Value = Me.cboToDate.MinDate

            '▼データの検索
            clsMdb.Connect()
            Dim strDate As String = Format(Date.Today, "yyyyMMdd")
            Dim strSql = String.Format("SELECT officer_pay_master.c_officer_pay_id AS Id" &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_from") &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_to") &
                                      ",officer_pay_master.l_name,officer_pay_master.s_officer_pay,officer_pay_master.l_biko" &
                                      ",officer_pay_master.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM officer_pay_master" &
                                      " LEFT JOIN staf_attribute_latest_view ON officer_pay_master.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " AND officer_pay_master.c_ksh = staf_attribute_latest_view.c_ksh" &
                                      " WHERE (officer_pay_master.d_from<='{0}' AND officer_pay_master.d_to='" & MDMasterCommon.DEFAULT_TO_DATE & "')" &
                                      " OR (officer_pay_master.d_to<>'" & MDMasterCommon.DEFAULT_TO_DATE & "'" &
                                      " AND officer_pay_master.d_from<='{1}' AND '{2}'<=officer_pay_master.d_to) AND officer_pay_master.c_ksh = '{3}'" &
                                      " ORDER BY officer_pay_master.c_officer_pay_id,officer_pay_master.d_to", strDate, strDate, strDate, MDLoginInfo.Ksh)
            table = clsMdb.ExecuteSql(strSql)
            'table = clsMdb.ExecuteSql("SELECT officer_pay_master.c_officer_pay_id AS Id" & _
            '                          ",officer_pay_master.d_from,officer_pay_master.d_to" & _
            '                          ",officer_pay_master.l_name,officer_pay_master.s_officer_pay,officer_pay_master.l_biko" & _
            '                          ",officer_pay_master.d_ins,staf_attribute.l_name FROM officer_pay_master" & _
            '                          " LEFT JOIN staf_attribute ON officer_pay_master.c_user_id_ins = staf_attribute.c_user_id" & _
            '                          " WHERE (CONVERT(DATE,officer_pay_master.d_from)<=Format(Date(), 'yyyyMMdd') AND officer_pay_master.d_to='" & MDMasterCommon.DEFAULT_TO_DATE & "')" & _
            '                          " OR (officer_pay_master.d_to<>'" & MDMasterCommon.DEFAULT_TO_DATE & "'" & _
            '                          " AND CONVERT(DATE,officer_pay_master.d_from)<=Date() AND Date()<=CONVERT(DATE,officer_pay_master.d_to))" & _
            '                          " ORDER BY officer_pay_master.c_officer_pay_id,officer_pay_master.d_to")
            '▼値の表示
            MakeGridView(table, Me.DataGridView1)

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
            Dim uc As UC090117
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strPay As String
            Dim strBiko As String

            If Me.DataGridView1.SelectedRows.Count <> 1 Then
                MsgBox("行を選択して下さい。", vbExclamation, TITLE)
                Return
            Else
                strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
                strFromDate = Me.DataGridView1.SelectedRows(0).Cells(1).Value
                strToDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
                strName = Me.DataGridView1.SelectedRows(0).Cells(3).Value
                strPay = Me.DataGridView1.SelectedRows(0).Cells(4).Value
                If Not Me.DataGridView1.SelectedRows(0).Cells(5).Value Is DBNull.Value Then
                    strBiko = Me.DataGridView1.SelectedRows(0).Cells(5).Value
                Else
                    strBiko = ""
                End If
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090117)

            If uc Is Nothing Then
                uc = New UC090117
                uc.setData(strId, strFromDate, strToDate, strName, strPay, strBiko)
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
            Dim uc As UC090117

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090117)

            If uc Is Nothing Then
                uc = New UC090117
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

        If Me.DataGridView1.SelectedRows(0).Cells(2).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
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
            Dim strFrom As String = Me.DataGridView1.SelectedRows(0).Cells(1).Value
            Dim strTo As String = MDMasterCommon.DateValueStr(MDMasterCommon.GetMonthLastDate(Me.cboToDate.Value))
            clsMdb.Connect()
            clsMdb.ExecuteSql(String.Format("UPDATE officer_pay_master SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_officer_pay_id='{3}' AND c_ksh='{4}' AND d_from='{5}' AND d_to='99999999'", _
                                              strTo, Date.Today, MDLoginInfo.UserId, strId, MDLoginInfo.Ksh, MDMasterCommon.DateValueStr(strFrom)))
            Me.DataGridView1.SelectedRows(0).Cells(2).Value = Format(MDMasterCommon.GetMonthLastDate(Me.cboToDate.Value), "yyyy/MM/dd")

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

        If Me.DataGridView1.SelectedRows(0).Cells(2).Value <> MDMasterCommon.DEFAULT_TO_DATE_STR Then
            MsgBox("適用終了日付が'" & MDMasterCommon.DEFAULT_TO_DATE_STR & "'以外は履歴登録できません。", vbExclamation, TITLE)
            Return
        End If

        Try
            Dim pn As Panel
            Dim uc As UC090117
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strPay As String
            Dim strBiko As String

            strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
            strFromDate = Me.DataGridView1.SelectedRows(0).Cells(1).Value
            strToDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
            strName = Me.DataGridView1.SelectedRows(0).Cells(3).Value
            strPay = Me.DataGridView1.SelectedRows(0).Cells(4).Value
            If Not Me.DataGridView1.SelectedRows(0).Cells(5).Value Is DBNull.Value Then
                strBiko = Me.DataGridView1.SelectedRows(0).Cells(5).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090117)

            If uc Is Nothing Then
                uc = New UC090117
                uc.setData(strId, strFromDate, strToDate, strName, strPay, strBiko)
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
            dgv.Columns(0).HeaderText = "役員手当ID"
            dgv.Columns(1).HeaderText = "適用開始日付"
            dgv.Columns(2).HeaderText = "適用終了日付"
            dgv.Columns(3).HeaderText = "名称"
            dgv.Columns(4).HeaderText = "手当金額"
            dgv.Columns(5).HeaderText = "備考"
            dgv.Columns(6).HeaderText = "更新日"
            dgv.Columns(7).HeaderText = "更新者"
            dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgv.Columns(4).DefaultCellStyle.Format = "#,0"

            '▼行ヘッダーに行番号を表示する
            dgv.TopLeftHeaderCell.Value = "項番"
            'For i = 0 To dgv.Rows.Count - 1
            '    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
            'Next i

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.RowHeadersWidth = 50
            dgv.Columns(0).Width = 110
            dgv.Columns(1).Width = 150
            dgv.Columns(2).Width = 150
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

        If MDMasterCommon.IsFutureMonth(Me.DataGridView2.SelectedRows(0).Cells(1).Value) = False Then
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
            Dim strFrom As String = Me.DataGridView2.SelectedRows(0).Cells(1).Value
            Dim strTo As String = Me.DataGridView2.SelectedRows(0).Cells(2).Value
            Dim result As Integer
            clsMdb.Connect()
            clsMdb.BeginTran()
            result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM officer_pay_master WHERE c_officer_pay_id='{0}' and c_ksh='{1}' AND d_from='{2}' AND d_to='{3}'", _
                                            strId, MDLoginInfo.Ksh, MDMasterCommon.DateValueStr(strFrom), MDMasterCommon.DateValueStr(strTo)))
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
        Dim strSql As String = String.Format("UPDATE officer_pay_master SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_officer_pay_id='{0}' and c_ksh='{1}' AND d_to=(SELECT max(d_to) AS max_d_to FROM officer_pay_master" & _
                                             " WHERE c_officer_pay_id='{2}' and c_ksh='{3}')", strId, MDLoginInfo.Ksh, strId, MDLoginInfo.Ksh)
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
            Dim uc As UC090117
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strName As String
            Dim strPay As String
            Dim strBiko As String

            strId = Me.DataGridView2.SelectedRows(0).Cells(0).Value
            strFromDate = Me.DataGridView2.SelectedRows(0).Cells(1).Value
            strToDate = Me.DataGridView2.SelectedRows(0).Cells(2).Value
            strName = Me.DataGridView2.SelectedRows(0).Cells(3).Value
            strPay = Me.DataGridView2.SelectedRows(0).Cells(4).Value
            If Not Me.DataGridView2.SelectedRows(0).Cells(5).Value Is DBNull.Value Then
                strBiko = Me.DataGridView2.SelectedRows(0).Cells(5).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090117)

            If uc Is Nothing Then
                uc = New UC090117
                uc.setData(strId, strFromDate, strToDate, strName, strPay, strBiko)
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
            If Me.txtPayId.Text.Length > 0 Then
                strWhere = String.Format("WHERE officer_pay_master.c_ksh='{0}' and officer_pay_master.c_officer_pay_id LIKE '{1}%'", MDLoginInfo.Ksh, Me.txtPayId.Text)
            Else
                strWhere = String.Format("WHERE officer_pay_master.c_ksh='{0}'", MDLoginInfo.Ksh)
            End If
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT officer_pay_master.c_officer_pay_id AS Id" &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_to") &
                                      ",officer_pay_master.l_name,officer_pay_master.s_officer_pay,officer_pay_master.l_biko" &
                                      ",officer_pay_master.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM officer_pay_master" &
                                      " LEFT JOIN staf_attribute_latest_view ON officer_pay_master.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " and officer_pay_master.c_ksh = staf_attribute_latest_view.c_ksh" &
                                      " {0} ORDER BY officer_pay_master.c_officer_pay_id,officer_pay_master.d_to", strWhere))
            '                          " WHERE (CONVERT(DATE,officer_pay_master.d_from)<=Date() AND officer_pay_master.d_to='9999年99月99日')" & _
            '                          " OR (officer_pay_master.d_to<>'9999年99月99日'" & _
            '                          " AND CONVERT(DATE,officer_pay_master.d_from)<=Date() AND Date()<=CONVERT(DATE,officer_pay_master.d_to))" & _
            '                          " {0} ORDER BY officer_pay_master.c_officer_pay_id,officer_pay_master.d_to", strWhere))
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

    Private Sub txtPayId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPayId.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Dim clsMdb As New CLAccessMdb
            Dim table As New DataTable
            Try
                '▼データの検索
                Dim strWhere As String = ""
                If Me.txtPayId.Text.Length > 0 Then
                    strWhere = String.Format("WHERE officer_pay_master.c_ksh='{0}' and officer_pay_master.c_officer_pay_id LIKE '{1}%'", MDLoginInfo.Ksh, Me.txtPayId.Text)
                Else
                    strWhere = String.Format("WHERE officer_pay_master.c_ksh='{0}'", MDLoginInfo.Ksh)
                End If
                clsMdb.Connect()
                table = clsMdb.ExecuteSql(String.Format("SELECT officer_pay_master.c_officer_pay_id AS Id" &
                                          "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("officer_pay_master.d_to") &
                                          ",officer_pay_master.l_name,officer_pay_master.s_officer_pay,officer_pay_master.l_biko" &
                                          ",officer_pay_master.d_ins,staf_attribute.l_name FROM officer_pay_master" &
                                          " LEFT JOIN staf_attribute ON officer_pay_master.c_user_id_ins = staf_attribute.c_user_id" &
                                          " and officer_pay_master.c_ksh = staf_attribute.c_ksh" &
                                          " {0} ORDER BY officer_pay_master.c_officer_pay_id,officer_pay_master.d_to", strWhere))
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
