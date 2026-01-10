'===========================================================================================================
'   クラスＩＤ　　：UC090104
'   クラス名称　　：パスワードマスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports System.Data.OleDb
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg

Public Class UC090104

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            Dim strSql As String
            Dim dgv As DataGridView
            dgv = Me.DataGridView1

            '▼データの検索
            clsMdb.Connect()
            strSql = "SELECT c_staf_id,l_name," & UtDb.DbStrYYYYMMDDtoDateText("d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("d_to") & ",FORMAT(d_ins, 'yyyy/MM/dd'),l_name_upd,c_user_id FROM password_list_view"
            If Me.txtStaffNo.Text.Length > 0 And Me.txtStaffNamekana.Text.Length = 0 Then
                strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%'"
            ElseIf Me.txtStaffNo.Text.Length = 0 And Me.txtStaffNamekana.Text.Length > 0 Then
                strSql = strSql & " WHERE l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
            Else
                strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%' And l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
            End If
            strSql = strSql & "order by CLng(c_staf_id) ASC"    'ok
            table = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)

            '▼値の表示
            dgv.DataSource = table
            Me.gbxGridView.Visible = True

            '▼ヘッダーの設定
            dgv.Columns(0).HeaderText = "社員番号"
            dgv.Columns(1).HeaderText = "名前"
            dgv.Columns(2).HeaderText = "適用開始日付"
            dgv.Columns(3).HeaderText = "適用終了日付"
            dgv.Columns(4).HeaderText = "更新日"
            dgv.Columns(5).HeaderText = "更新者"
            dgv.Columns(6).HeaderText = ""
            dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).Visible = False

            '▼行ヘッダーに行番号を表示する
            dgv.TopLeftHeaderCell.Value = "項番"
            For i = 0 To dgv.Rows.Count - 1
                dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
            Next i

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth( _
                DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.Columns(1).Width = 180
            dgv.Columns(2).Width = 130
            dgv.Columns(3).Width = 130
            dgv.Columns(5).Width = 180
            dgv.Columns(6).Width = 0
            dgv.AllowUserToAddRows = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Sub btnPasswordReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPasswordReset.Click
        Try
            Dim pn As Panel
            Dim uc As UC090105
            Dim staffNo As String
            Dim staffName As String
            Dim userId As String

            If Me.DataGridView1.SelectedRows.Count <> 1 Then
                MsgBox("行を選択して下さい。", vbExclamation, "パスワードマスタメンテナンス")
                log.Error("行を選択して下さい。[パスワードマスタメンテナンス]")
                Return
            Else
                staffNo = Me.DataGridView1.SelectedRows(0).Cells(0).Value
                staffName = Me.DataGridView1.SelectedRows(0).Cells(1).Value
                userId = Me.DataGridView1.SelectedRows(0).Cells(6).Value
            End If
            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090105)

            If uc Is Nothing Then
                uc = New UC090105
                uc.setData(staffNo, staffName, userId)
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        btnPasswordReset_Click(sender, e)
    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub txtStaffNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStaffNo.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Dim clsMdb As New CLAccessMdb
            Dim table As New DataTable
            Try
                Dim strSql As String
                Dim dgv As DataGridView
                dgv = Me.DataGridView1

                '▼データの検索
                clsMdb.Connect()
                strSql = "SELECT c_staf_id,l_name," & UtDb.DbStrYYYYMMDDtoDateText("d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("d_to") & ",FORMAT(d_ins, 'yyyy/MM/dd'),l_name_upd,c_user_id FROM password_list_view"
                If Me.txtStaffNo.Text.Length > 0 And Me.txtStaffNamekana.Text.Length = 0 Then
                    strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%'"
                ElseIf Me.txtStaffNo.Text.Length = 0 And Me.txtStaffNamekana.Text.Length > 0 Then
                    strSql = strSql & " WHERE l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
                Else
                    strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%' And l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
                End If
                strSql = strSql & "order by CLng(c_staf_id) ASC"
                table = clsMdb.ExecuteSql(strSql)
                log.Debug(strSql)

                '▼値の表示
                dgv.DataSource = table
                Me.gbxGridView.Visible = True

                '▼ヘッダーの設定
                dgv.Columns(0).HeaderText = "社員番号"
                dgv.Columns(1).HeaderText = "名前"
                dgv.Columns(2).HeaderText = "適用開始日付"
                dgv.Columns(3).HeaderText = "適用終了日付"
                dgv.Columns(4).HeaderText = "更新日"
                dgv.Columns(5).HeaderText = "更新者"
                dgv.Columns(6).HeaderText = ""
                dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(6).Visible = False

                '▼行ヘッダーに行番号を表示する
                dgv.TopLeftHeaderCell.Value = "項番"
                For i = 0 To dgv.Rows.Count - 1
                    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
                Next i

                '▼行ヘッダーの幅を調節する
                dgv.AutoResizeRowHeadersWidth( _
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
                dgv.Columns(1).Width = 180
                dgv.Columns(2).Width = 130
                dgv.Columns(3).Width = 130
                dgv.Columns(5).Width = 180
                dgv.Columns(6).Width = 0
                dgv.AllowUserToAddRows = False
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

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

    Private Sub txtStaffNamekana_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStaffNamekana.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Dim clsMdb As New CLAccessMdb
            Dim table As New DataTable
            Try
                Dim strSql As String
                Dim dgv As DataGridView
                dgv = Me.DataGridView1

                '▼データの検索
                clsMdb.Connect()
                strSql = "SELECT c_staf_id,l_name," & UtDb.DbStrYYYYMMDDtoDateText("d_from") & "," & UtDb.DbStrYYYYMMDDtoDateText("d_to") & ",FORMAT(d_ins, 'yyyy/MM/dd'),l_name_upd,c_user_id FROM password_list_view"
                If Me.txtStaffNo.Text.Length > 0 And Me.txtStaffNamekana.Text.Length = 0 Then
                    strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%'"
                ElseIf Me.txtStaffNo.Text.Length = 0 And Me.txtStaffNamekana.Text.Length > 0 Then
                    strSql = strSql & " WHERE l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
                Else
                    strSql = strSql & " WHERE c_staf_id Like '" & Me.txtStaffNo.Text & "%' And l_name_kna Like '%" & Me.txtStaffNamekana.Text & "%'"
                End If
                strSql = strSql & "order by CLng(c_staf_id) ASC"
                table = clsMdb.ExecuteSql(strSql)
                log.Debug(strSql)

                '▼値の表示
                dgv.DataSource = table
                Me.gbxGridView.Visible = True

                '▼ヘッダーの設定
                dgv.Columns(0).HeaderText = "社員番号"
                dgv.Columns(1).HeaderText = "名前"
                dgv.Columns(2).HeaderText = "適用開始日付"
                dgv.Columns(3).HeaderText = "適用終了日付"
                dgv.Columns(4).HeaderText = "更新日"
                dgv.Columns(5).HeaderText = "更新者"
                dgv.Columns(6).HeaderText = ""
                dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(6).Visible = False

                '▼行ヘッダーに行番号を表示する
                dgv.TopLeftHeaderCell.Value = "項番"
                For i = 0 To dgv.Rows.Count - 1
                    dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
                Next i

                '▼行ヘッダーの幅を調節する
                dgv.AutoResizeRowHeadersWidth( _
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
                dgv.Columns(1).Width = 180
                dgv.Columns(2).Width = 130
                dgv.Columns(3).Width = 130
                dgv.Columns(5).Width = 180
                dgv.Columns(6).Width = 0
                dgv.AllowUserToAddRows = False
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

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
