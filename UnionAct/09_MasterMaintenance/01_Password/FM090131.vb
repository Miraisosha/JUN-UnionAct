'===========================================================================================================
'   クラスＩＤ　　：FM090131
'   クラス名称　　：専従職員権限マスタメンテナンス - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg

Public Class FM090131
    Private cstrMenuId As String
    Private cstrControlId As String
    Private cintPeriod As Integer

    Public Sub initData(ByVal strName As String, ByVal strControl As String, ByVal strPeriod As String, _
                        ByVal strMenuId As String, ByVal strControlId As String, ByVal intPeriod As Integer)
        Me.txtName.Text = strName
        Me.txtControl.Text = strControl
        Me.txtPeriod.Text = strPeriod
        cstrMenuId = strMenuId
        cstrControlId = strControlId
        cintPeriod = intPeriod
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Private Sub FM090131_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            loadData()

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
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(String.Format("SELECT screen.l_screen_name, IIF(f_refer='1','可','不可')," & _
                                                    "IIF(f_regist='1','可','不可'), IIF(f_print='1','可','不可')," & _
                                                    "IIF(f_file='1','可','不可'), IIF(f_master='1','可','不可')," & _
                                                    "Mid(menucontrol_dtl.b_power,1,1) AS f_refer," & _
                                                    "Mid(menucontrol_dtl.b_power,2,1) AS f_regist," & _
                                                    "Mid(menucontrol_dtl.b_power,3,1) AS f_print," & _
                                                    "Mid(menucontrol_dtl.b_power,4,1) AS f_file," & _
                                                    "Mid(menucontrol_dtl.b_power,5,1) AS f_master" & _
                                                    " FROM menucontrol_dtl LEFT JOIN screen ON menucontrol_dtl.c_screen_id=screen.c_screen_id" & _
                                                    " WHERE menucontrol_dtl.c_menu_id='{0}' AND menucontrol_dtl.c_control_screen_kind='{1}'" _
                                                    , cstrMenuId, cstrControlId))

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

    Private Sub MakeGridView(ByVal table As DataTable, ByRef dgv As DataGridView)
        Try
            '▼値の表示
            dgv.DataSource = table

            '▼ヘッダーの設定
            dgv.Columns(0).HeaderText = "画面名"
            dgv.Columns(1).HeaderText = "参照"
            dgv.Columns(2).HeaderText = "登録"
            dgv.Columns(3).HeaderText = "印刷"
            dgv.Columns(4).HeaderText = "ファイル出力"
            dgv.Columns(5).Visible = False
            dgv.Columns(6).Visible = False
            dgv.Columns(7).Visible = False
            dgv.Columns(8).Visible = False
            dgv.Columns(9).Visible = False
            dgv.Columns(10).Visible = False

            '▼行ヘッダーの幅を調節する
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders)
            dgv.Columns(0).Width = 150
            dgv.Columns(1).Width = 65
            dgv.Columns(2).Width = 65
            dgv.Columns(3).Width = 65
            dgv.Columns(4).Width = 115
            dgv.RowHeadersVisible = False
            dgv.AllowUserToAddRows = False
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class