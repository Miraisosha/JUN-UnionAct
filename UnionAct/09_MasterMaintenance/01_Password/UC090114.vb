'===========================================================================================================
'   クラスＩＤ　　：UC090114
'   クラス名称　　：昼食費マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Public Class UC090114
    Private Const TITLE As String = "昼食費マスタメンテナンス"

    Private Sub UC090114_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            '▼コントロール初期化
            Me.cboToDate.MinDate = MDMasterCommon.GetMonthTopDate(Now)
            Me.cboToDate.Value = Me.cboToDate.MinDate

            '▼データの検索
            clsMdb.Connect()
            Dim strDate As String = Format(Date.Today, "yyyyMMdd")
            Dim strSql = String.Format("SELECT executive_lunch_pay_master.c_executive_lunch_pay_id AS Id" &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("executive_lunch_pay_master.d_from") &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("executive_lunch_pay_master.d_to") &
                                      ",executive_lunch_pay_master.s_pay,executive_lunch_pay_master.l_biko" &
                                      ",executive_lunch_pay_master.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM executive_lunch_pay_master" &
                                      " LEFT JOIN staf_attribute_latest_view ON executive_lunch_pay_master.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " WHERE (executive_lunch_pay_master.d_from<='{0}' and executive_lunch_pay_master.d_to='" & MDMasterCommon.DEFAULT_TO_DATE & "')" &
                                      " or (executive_lunch_pay_master.d_to<>'" & MDMasterCommon.DEFAULT_TO_DATE & "'" &
                                      " And executive_lunch_pay_master.d_from<='{1}' and '{2}'<=executive_lunch_pay_master.d_to)" &
                                      " ORDER BY executive_lunch_pay_master.c_executive_lunch_pay_id,executive_lunch_pay_master.d_to", strDate, strDate, strDate)
            table = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)
            '" AND staf_attribute.c_ksh = '" & MDLoginInfo.OffceCode & "'" & _

            '▼値の表示
            MakeGridView(table, Me.DataGridView1)

            table = clsMdb.ExecuteSql("SELECT executive_lunch_pay_master.c_executive_lunch_pay_id" &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("executive_lunch_pay_master.d_from") &
                                      "," & UtDb.DbStrYYYYMMDDtoDateText("executive_lunch_pay_master.d_to") &
                                      ",executive_lunch_pay_master.s_pay,executive_lunch_pay_master.l_biko" &
                                      ",executive_lunch_pay_master.d_ins,staf_attribute_latest_view.staf_attribute_l_name FROM executive_lunch_pay_master" &
                                      " LEFT JOIN staf_attribute_latest_view ON executive_lunch_pay_master.c_user_id_ins = staf_attribute_latest_view.c_user_id" &
                                      " ORDER BY executive_lunch_pay_master.c_executive_lunch_pay_id,executive_lunch_pay_master.d_to")

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
            Dim uc As UC090115
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strPay As String
            Dim strBiko As String

            If Me.DataGridView1.SelectedRows.Count <> 1 Then
                MsgBox("行を選択して下さい。", vbExclamation, TITLE)
                Return
            Else
                strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
                strFromDate = Me.DataGridView1.SelectedRows(0).Cells(1).Value
                strToDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
                strPay = Me.DataGridView1.SelectedRows(0).Cells(3).Value
                If Not Me.DataGridView1.SelectedRows(0).Cells(4).Value Is DBNull.Value Then
                    strBiko = Me.DataGridView1.SelectedRows(0).Cells(4).Value
                Else
                    strBiko = ""
                End If
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090115)

            If uc Is Nothing Then
                uc = New UC090115
                uc.setData(strId, strFromDate, strToDate, strPay, strBiko)
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
            Dim uc As UC090115

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090115)

            If uc Is Nothing Then
                uc = New UC090115
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
            clsMdb.ExecuteSql(String.Format("UPDATE executive_lunch_pay_master SET d_to='{0}',d_ins='{1}',c_user_id_ins='{2}' WHERE c_executive_lunch_pay_id='{3}' AND d_from='{4}' AND d_to='99999999'", _
                                              strTo, Date.Today, MDLoginInfo.UserId, strId, MDMasterCommon.DateValueStr(strFrom)))
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
            Dim uc As UC090115
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strPay As String
            Dim strBiko As String

            strId = Me.DataGridView1.SelectedRows(0).Cells(0).Value
            strFromDate = Me.DataGridView1.SelectedRows(0).Cells(1).Value
            strToDate = Me.DataGridView1.SelectedRows(0).Cells(2).Value
            strPay = Me.DataGridView1.SelectedRows(0).Cells(3).Value
            If Not Me.DataGridView1.SelectedRows(0).Cells(4).Value Is DBNull.Value Then
                strBiko = Me.DataGridView1.SelectedRows(0).Cells(4).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090115)

            If uc Is Nothing Then
                uc = New UC090115
                uc.setData(strId, strFromDate, strToDate, strPay, strBiko)
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
            dgv.TopLeftHeaderCell.Value = "項番"
            dgv.Columns(0).HeaderText = "昼食費ID"
            dgv.Columns(1).HeaderText = "適用開始日付"
            dgv.Columns(2).HeaderText = "適用終了日付"
            dgv.Columns(3).HeaderText = "金額"
            dgv.Columns(4).HeaderText = "備考"
            dgv.Columns(5).HeaderText = "更新日"
            dgv.Columns(6).HeaderText = "更新者"
            dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgv.Columns(3).DefaultCellStyle.Format = "#,0"

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.RowHeadersWidth = 50
            dgv.Columns(0).Width = 160
            dgv.Columns(1).Width = 150
            dgv.Columns(2).Width = 150
            dgv.AllowUserToAddRows = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            '▼行ヘッダーに行番号を表示する
            'For i = 0 To dgv.Rows.Count - 1
            '    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
            'Next i

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
            result = clsMdb.ExecuteNonQuery(String.Format("DELETE FROM executive_lunch_pay_master WHERE c_executive_lunch_pay_id='{0}' and d_from='{1}' and d_to='{2}'", _
                                   strId, MDMasterCommon.DateValueStr(strFrom), MDMasterCommon.DateValueStr(strTo)))
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
        Dim strSql As String = String.Format("UPDATE executive_lunch_pay_master SET d_to = '" & MDMasterCommon.DEFAULT_TO_DATE & "' " & _
                                             "WHERE c_executive_lunch_pay_id='{0}' AND d_to=(SELECT max(d_to) AS max_d_to FROM executive_lunch_pay_master" & _
                                             " WHERE c_executive_lunch_pay_id='{1}')", strId, strId)
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
            Dim uc As UC090115
            Dim strId As String
            Dim strFromDate As String
            Dim strToDate As String
            Dim strPay As String
            Dim strBiko As String

            strId = Me.DataGridView2.SelectedRows(0).Cells(0).Value
            strFromDate = Me.DataGridView2.SelectedRows(0).Cells(1).Value
            strToDate = Me.DataGridView2.SelectedRows(0).Cells(2).Value
            strPay = Me.DataGridView2.SelectedRows(0).Cells(3).Value
            If Not Me.DataGridView2.SelectedRows(0).Cells(4).Value Is DBNull.Value Then
                strBiko = Me.DataGridView2.SelectedRows(0).Cells(4).Value
            Else
                strBiko = ""
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090115)

            If uc Is Nothing Then
                uc = New UC090115
                uc.setData(strId, strFromDate, strToDate, strPay, strBiko)
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

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
