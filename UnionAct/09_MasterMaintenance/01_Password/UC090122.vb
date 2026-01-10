'===========================================================================================================
'   クラスＩＤ　　：UC090122
'   クラス名称　　：郵便番号マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports System.Data.OleDb
Imports Microsoft.VisualBasic.FileIO
Imports UnionAct.NSCLAccessMdb

Public Class UC090122

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try
            '▼OpenFileDialogダイアログ
            Dim ofd As New OpenFileDialog()
            ofd.FileName = "ken_all.csv"
            'ofd.InitialDirectory = "C:\"
            ofd.Filter = "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*"
            ofd.FilterIndex = 1
            ofd.Title = "ファイルを選択してください"
            ofd.RestoreDirectory = True

            '▼CSVファイル選択＆取込み
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim dlg As FM090122 = New FM090122()
                dlg.Owner = ParentForm
                dlg.Show()
                importCsv(ofd.FileName, dlg.txtCount)
                dlg.lblFinished.Visible = True
                dlg.btnOK.Enabled = True
            End If
        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    Private Sub importCsv(ByVal strPath As String, ByRef txtCounter As Control)
        Dim con As OleDbConnection = Nothing
        Dim tran As System.Data.OleDb.OleDbTransaction = Nothing

        Try
            '▼DB接続
            Dim cs As String = MDSystemInfo.DbConnectionString
            'Dim cs As String = "Provider=" & MDSystemInfo.AccessProvider
            'cs = cs & ";Data Source=" & MDSystemInfo.AccessPath & MDSystemInfo.AccessName
            'cs = cs & ";Persist Security Info=" & AccessPersistSecurity
            ''cs = cs & ";User ID=" & AccessUserId
            'cs = cs & ";Jet OLEDB:Database Password=" & AccessPassword
            con = New OleDbConnection(cs)
            con.Open()
            Dim cmd As OleDbCommand = con.CreateCommand
            Dim parser As New TextFieldParser(strPath, System.Text.Encoding.GetEncoding("Shift_JIS"))
            Dim cnt As Integer = 0

            tran = con.BeginTransaction()
            cmd.Transaction = tran

            '▼全削除
            cmd.CommandText = "DELETE FROM zip_code_full"
            cmd.ExecuteNonQuery()
            log.Debug(cmd.CommandText)

            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",")

            '▼データの更新
            While Not parser.EndOfData
                Application.DoEvents()
                Dim row As String() = parser.ReadFields()
                cmd.CommandText = String.Format("insert into zip_code_full (c_zip_code, l_prefectures_kna, l_cities_kna, l_cities_area_kna, l_prefectures, l_cities, l_cities_area, c_old_zip_code, d_ins) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", _
                                                  row(2), row(3), row(4), row(5), row(6), row(7), row(8), row(1), Date.Today.ToString)
                cmd.ExecuteNonQuery()
                cnt = cnt + 1
                txtCounter.Text = cnt
                txtCounter.Refresh()
            End While

            cmd.Dispose()
            tran.Commit()

        Catch ex As Exception
            tran.Rollback()
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            con.Dispose()
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            Dim dgv As DataGridView
            dgv = Me.DataGridView1

            '▼データの検索
            clsMdb.Connect()
            table = clsMdb.ExecuteSql("SELECT " & UtDb.DbStrZipCode("c_zip_code") & ",l_prefectures,l_cities,l_cities_area,l_prefectures_kna,l_cities_kna,l_cities_area_kna,d_ins FROM zip_code_full WHERE l_prefectures='" & Me.cboPrefectures.Text & "' ORDER BY c_zip_code")

            '▼値の表示
            DataGridView1.DataSource = table
            Me.gbxGridView.Visible = True

            '▼ヘッダーの設定
            dgv.RowHeadersVisible = False
            dgv.Columns(0).HeaderText = "郵便番号"
            dgv.Columns(1).HeaderText = "都道府県名"
            dgv.Columns(2).HeaderText = "市区町村名"
            dgv.Columns(3).HeaderText = "町域名"
            dgv.Columns(4).HeaderText = "カナ都道府県名"
            dgv.Columns(5).HeaderText = "カナ市区町村名"
            dgv.Columns(6).HeaderText = "カナ町域名"
            dgv.Columns(7).HeaderText = "作成日"
            dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.Columns(0).Width = 100
            dgv.Columns(1).Width = 120
            dgv.Columns(2).Width = 120
            dgv.Columns(3).Width = 120
            dgv.Columns(4).Width = 140
            dgv.Columns(5).Width = 140
            dgv.Columns(6).Width = 140
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

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub cboPrefectures_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboPrefectures.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Dim clsMdb As New CLAccessMdb
            Dim table As New DataTable
            Try
                Dim dgv As DataGridView
                dgv = Me.DataGridView1

                '▼データの検索
                clsMdb.Connect()
                table = clsMdb.ExecuteSql("SELECT " & UtDb.DbStrZipCode("c_zip_code") & ",l_prefectures,l_cities,l_cities_area,l_prefectures_kna,l_cities_kna,l_cities_area_kna,d_ins FROM zip_code_full WHERE l_prefectures='" & Me.cboPrefectures.Text & "' ORDER BY c_zip_code")

                '▼値の表示
                DataGridView1.DataSource = table
                Me.gbxGridView.Visible = True

                '▼ヘッダーの設定
                dgv.RowHeadersVisible = False
                dgv.Columns(0).HeaderText = "郵便番号"
                dgv.Columns(1).HeaderText = "都道府県名"
                dgv.Columns(2).HeaderText = "市区町村名"
                dgv.Columns(3).HeaderText = "町域名"
                dgv.Columns(4).HeaderText = "カナ都道府県名"
                dgv.Columns(5).HeaderText = "カナ市区町村名"
                dgv.Columns(6).HeaderText = "カナ町域名"
                dgv.Columns(7).HeaderText = "作成日"
                dgv.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter

                '▼行ヘッダーの幅を調節する
                dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
                dgv.Columns(0).Width = 100
                dgv.Columns(1).Width = 120
                dgv.Columns(2).Width = 120
                dgv.Columns(3).Width = 120
                dgv.Columns(4).Width = 140
                dgv.Columns(5).Width = 140
                dgv.Columns(6).Width = 140
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
