Public Class FM090101

    Private Sub FM090101_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strMaintenanceMenu(6, 1) As String
        Dim strRow(1) As String
        Dim col(1) As DataGridViewTextBoxColumn
        Dim i As Integer
        Dim dgv As DataGridView

        'strMaintenanceMenu(0, 0) = "定数マスタメンテナンス"
        strMaintenanceMenu(0, 0) = "パスワードマスタメンテナンス"
        strMaintenanceMenu(5, 0) = "委員会マスタメンテナンス"
        'strMaintenanceMenu(3, 0) = "期マスタメンテナンス"
        strMaintenanceMenu(1, 0) = "日当マスタメンテナンス"
        strMaintenanceMenu(2, 0) = "昼食費マスタメンテナンス"
        strMaintenanceMenu(3, 0) = "役員手当マスタメンテナンス"
        'strMaintenanceMenu(7, 0) = "課税率マスタメンテナンス"
        'strMaintenanceMenu(8, 0) = "切捨て金額マスタメンテナンス"
        strMaintenanceMenu(4, 0) = "郵便番号マスタメンテナンス"
        'strMaintenanceMenu(10, 0) = "画面マスタメンテナンス"
        'strMaintenanceMenu(11, 0) = "メニューコントロールマスタメンテナンス"
        'strMaintenanceMenu(12, 0) = "銀行マスタメンテナンス"
        'strMaintenanceMenu(5, 0) = "専従職員権限マスタメンテナンス"
        'strMaintenanceMenu(14, 0) = "会社マスタメンテナンス"

        'strMaintenanceMenu(0, 1) = "UC090102"
        strMaintenanceMenu(0, 1) = "UC090104"
        strMaintenanceMenu(5, 1) = "UC090106"
        'strMaintenanceMenu(3, 1) = "UC090110"
        strMaintenanceMenu(1, 1) = "UC090112"
        strMaintenanceMenu(2, 1) = "UC090114"
        strMaintenanceMenu(3, 1) = "UC090116"
        'strMaintenanceMenu(7, 1) = "UC090118"
        'strMaintenanceMenu(8, 1) = "UC090120"
        strMaintenanceMenu(4, 1) = "UC090122"
        'strMaintenanceMenu(10, 1) = "UC090123"
        'strMaintenanceMenu(11, 1) = "UC090125"
        'strMaintenanceMenu(12, 1) = "UC090128"
        'strMaintenanceMenu(5, 1) = "UC090130"
        'strMaintenanceMenu(14, 1) = "UC090133"

        dgv = Me.DataGridView1
        dgv.RowHeadersWidth = 60

        'DataGridViewに列を追加する
        For i = 0 To 1
            col(i) = New DataGridViewTextBoxColumn
            dgv.Columns.Add(col(i))
            dgv.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Select Case i
                Case 0
                    dgv.Columns(i).Width = 230
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

    Private Sub DataGridView1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles DataGridView1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Me.OpenMaintenanceControl()
        End If
    End Sub
End Class