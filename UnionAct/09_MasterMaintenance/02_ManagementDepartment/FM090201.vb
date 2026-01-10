Public Class FM090201
    Private Sub FM090201_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strMaintenanceMenu(1, 1) As String
        Dim strRow(1) As String
        Dim col(1) As DataGridViewTextBoxColumn
        Dim i As Integer
        Dim dgv As DataGridView

        strMaintenanceMenu(0, 0) = "支出科目マスタメンテナンス"
        strMaintenanceMenu(1, 0) = "組合費マスタメンテナンス"

        strMaintenanceMenu(0, 1) = "UC090202"
        strMaintenanceMenu(1, 1) = "UC090204"

        dgv = Me.DataGridView1
        dgv.RowHeadersWidth = 45

        'DataGridViewに列を追加する
        For i = 0 To 1
            col(i) = New DataGridViewTextBoxColumn
            dgv.Columns.Add(col(i))

            Select Case i
                Case 0
                    dgv.Columns(i).Width = 209
                    dgv.Columns(i).HeaderText = "項目一覧"
                Case 1
                    dgv.Columns(i).Visible = False
            End Select
        Next

        'テーブルの検索結果をDataGridViewに表示する
        For i = 0 To UBound(strMaintenanceMenu, 1)
            strRow(0) = strMaintenanceMenu(i, 0)
            strRow(1) = strMaintenanceMenu(i, 1)

            Call dgv.Rows.Add(strRow)
        Next

        '▼行ヘッダーに行番号を表示する
        dgv.TopLeftHeaderCell.Value = "項番"
        For i = 0 To dgv.Rows.Count - 1
            dgv.Rows(i).HeaderCell.Value = (i + 1).ToString()
        Next i
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call Me.Close()
    End Sub

    Private Sub OpenMaintenanceControl()
        Dim dgv As DataGridView
        Dim row As DataGridViewRow
        Dim strGamenID As String
        Dim type As Type
        Dim uc As Control
        Dim panel_uc As Control
        Dim fmMain As FM000102
        Dim fm As Form
        Dim i As Integer

        fmMain = Owner

        dgv = Me.DataGridView1
        row = dgv.CurrentRow '選択中の行を取得

        '画面IDの値を取得
        If TypeOf row.Cells(1).Value Is DBNull Then
            strGamenID = ""
        Else
            strGamenID = row.Cells(1).Value
        End If

        '画面IDが設定されているか？
        If strGamenID <> "" Then
            '画面IDに該当するコントロールのTypeオブジェクトを取得
            type = type.GetType("UnionAct." & strGamenID)

            'Typeオブジェクトが取得できたか？
            If Not type Is Nothing Then
                If strGamenID.Substring(0, 2) = "UC" Then
                    'コントロールのインスタンスを生成
                    uc = CType(Activator.CreateInstance(type), Control)

                    'パネルに開かれているコントロールを閉じる
                    For i = fmMain.pnlMain.Controls.Count - 1 To 0 Step -1
                        panel_uc = fmMain.pnlMain.Controls(i)
                        Call fmMain.pnlMain.Controls.Remove(panel_uc)
                    Next

                    '新しいコントロールをパネルに追加する
                    Call fmMain.pnlMain.Controls.Add(uc)
                Else
                    'フォームのインスタンスを生成
                    fm = CType(Activator.CreateInstance(type), Form)

                    'フォームを開く
                    Call fm.Show(Me)
                End If

                Call Me.Close()
            Else
                MsgBox("該当する画面がありません (画面ID = " & strGamenID & ")")
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call Me.OpenMaintenanceControl()
    End Sub

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        Call Me.OpenMaintenanceControl()
    End Sub
End Class