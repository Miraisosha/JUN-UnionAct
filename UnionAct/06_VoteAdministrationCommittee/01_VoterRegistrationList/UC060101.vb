#Region "UC060101"
'===========================================================================================================
'   クラスＩＤ　　：UC060101
'   クラス名称　　：選挙人名簿画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon

Public Class UC060101
#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC060101                     ' UC060101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC060101                 ' 選挙人名簿画面
    ' ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '************************************************************************************
    '   ＩＤ　：UC060101_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/16(水) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/16(水) Ryu  新規作成
    '************************************************************************************
    Private Sub UC060101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Dim sql As String = ""                                                          ' SQL文
        Dim dbAccess As New CLAccessMdb                                                 ' DBアクセス
        Dim dt As DataTable = Nothing                                                   ' データテーブル
        Try
            Call dbAccess.Connect()                                                     ' データベースに接続
            ' 支部コンボボックス
            Call CreateCboConstantDtl(dbAccess, cmbBelong, CONSTANT_ID_UI_SHIBU, False)
            ' 組合種別チェックボックス
            'sql = "select c_constant_seq, l_name from constant_dtl where c_constant = '" + CONSTANT_ID_STAF_KIND + "'"
            ' SQL
            sql = "" & vbCrLf
            sql = sql & " SELECT c_constant_seq" & vbCrLf
            sql = sql & "       ,l_name" & vbCrLf
            sql = sql & "   FROM constant_dtl" & vbCrLf
            sql = sql & "  WHERE c_constant = '" & CONSTANT_ID_STAF_KIND + "'" & vbCrLf
            sql = sql & ";" & vbCrLf
            ' データを取得
            dt = dbAccess.ExecuteSql(sql)
            Call dbAccess.Disconnect()                                                  ' データベースの接続を切断
            clbKind.Items.Clear()
            If dt.Rows.Count > 0 Then
                clbKind.DataSource = dt
                clbKind.DisplayMember = "l_name"
                clbKind.ValueMember = "c_constant_seq"
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnPrintIDAndName_Click
    '   名称　：IDと名前印刷
    '   概要　：組合員のIDと名前を印刷
    '   作成日：2011/11/16(水) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/16(水) Ryu  新規作成
    '************************************************************************************
    Private Sub btnPrintIDAndName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintIDAndName.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Dim strDate10 As String = ""                                                    ' 適用日付
        Dim dFrom As String = ""                                                        ' 前期の開始日
        Dim dt As DataTable = Nothing                                                   ' データテーブル
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument          ' IDと名前印刷レポート
        Dim fmPrint As FM000205                                                         ' 印刷プレビュー
        Dim ds As DS0601P1                                                              ' データセット
        Try
            dFrom = getPrePeriodStartDate()
            If dFrom = "" Then
                Exit Sub
            End If
            strDate10 = Format(dtpDate.Value, DATE_YYYYMMDD_FORMAT)
            ' 適用日付が適切でない場合
            If strDate10 < dFrom Then
                CLMsg.Show("GE0099", dFrom)
                Exit Sub
            End If
            ' 組合員種別が未選択の場合
            If clbKind.CheckedItems.Count = 0 Then
                If CLMsg.Show("GW0006") = DialogResult.No Then
                    Exit Sub
                End If
            End If
            ' データ取得
            dt = getPrintData()
            ' 抽出条件に一致する組合員が存在しない場合
            If dt.Rows.Count = 0 Then
                CLMsg.Show("DI0001")
                Exit Sub
            End If
            ' 印刷プレイビュー
            fmPrint = New FM000205
            ' レポート生成
            reportObj = New CR0601P1
            fmPrint.ButtonShowType = 2
            fmPrint.PrintCntVisible = True
            fmPrint.PrintAreaVisible = True
            fmPrint.ObjResource = reportObj
            ' 詳細データセット
            ds = New DS0601P1
            Dim drDetail As DS0601P1.dtDetailRow
            For Each row As DataRow In dt.Rows
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                drDetail.c_staf_id = row("c_staf_id")
                drDetail.l_name = row("l_name")
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail)
            Next
            ' ヘッダーデータセット
            Dim drHeader As DS0601P1.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.area_local = cmbBelong.Text
            drHeader._date = Format(dtpDate.Value, DATE_YYYYMMDD_KANJI_FORMAT)
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)
            ' データをレポートにセット
            reportObj.SetDataSource(ds)
            ' 印刷プレビューを表示
            Call fmPrint.ShowDialog()
            ' 印刷プレビューで押下ボタンにより処理を分ける
            Select Case fmPrint.IntQlickBtnFlag
                Case 1
                Case 2
                    ' キャンセル
                Case 3
                    ' 印刷
                    fmPrint.PrintOut()
            End Select
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnPrintOnlyName_Click
    '   名称　：名前のみ印刷
    '   概要　：名前のみ印刷
    '   作成日：2011/11/17(木) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnPrintOnlyName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintOnlyName.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Dim strDate10 As String = ""                                                    ' 適用日付
        Dim dFrom As String = ""                                                        ' 前期の開始日
        Dim dt As DataTable = Nothing                                                   ' データテーブル
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument          ' IDと名前印刷レポート
        Dim fmPrint As FM000205                                                         ' 印刷画面
        Dim ds As DS0601P2                                                              ' レポートのデータセット
        Try
            dFrom = getPrePeriodStartDate()
            If dFrom = "" Then
                Exit Sub
            End If
            strDate10 = Format(dtpDate.Value, DATE_YYYYMMDD_FORMAT)
            ' 適用日付が適切でない場合
            If strDate10 < dFrom Then
                CLMsg.Show("GE0099", dFrom)
                Exit Sub
            End If
            ' 組合員種別が未選択の場合
            If clbKind.CheckedItems.Count = 0 Then
                If CLMsg.Show("GW0006") = DialogResult.No Then
                    Exit Sub
                End If
            End If
            ' データ取得
            dt = getPrintData()
            ' 抽出条件に一致する組合員が存在しない場合
            If dt.Rows.Count = 0 Then
                CLMsg.Show("DI0001")
                Exit Sub
            End If
            ' 印刷プレイビュー
            fmPrint = New FM000205
            ' レポート生成
            reportObj = New CR0601P2
            fmPrint.ButtonShowType = 2
            fmPrint.PrintCntVisible = True
            fmPrint.PrintAreaVisible = True
            fmPrint.ObjResource = reportObj
            ' 詳細データセット
            ds = New DS0601P2
            Dim drDetail As DS0601P2.dtDetailRow
            For Each row As DataRow In dt.Rows
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                drDetail.l_name = row("l_name")
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail)
            Next
            ' ヘッダーデータセット
            Dim drHeader As DS0601P2.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.area_local = cmbBelong.Text
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)
            ' データをレポートにセット
            reportObj.SetDataSource(ds)
            ' 印刷プレビューを表示
            Call fmPrint.ShowDialog()
            ' 印刷プレビューで押下ボタンにより処理を分ける
            Select Case fmPrint.IntQlickBtnFlag
                Case 1

                Case 2
                    ' キャンセル
                Case 3
                    ' 印刷
                    fmPrint.PrintOut()
            End Select
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub
#End Region

#Region "関数"
    '************************************************************************************
    '   ＩＤ　：getPrintData
    '   名称　：データ取得
    '   概要　：印刷するデータを取得
    '   作成日：2011/11/17(木) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木) Ryu  新規作成
    '************************************************************************************
    Private Function getPrintData()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Dim sql As String = ""                                                          ' SQL文
        Dim iCounter As Integer = 0                                                     ' 組合員種別カウンター
        Dim dbAccess As New CLAccessMdb                                                 ' DBアクセス
        Dim dt As DataTable = Nothing                                                   ' データテーブル
        Dim strDate8 As String = ""                                                     ' 適用日付
        Try
            strDate8 = Format(dtpDate.Value, DATE_YYYYMMDD_8_FORMAT)
            sql = " SELECT DISTINCT c_staf_id,l_name,l_name_kna FROM staf_attribute WHERE format(d_join, 'yyyyMMdd') <= '" + strDate8 + "' and k_belonging = '" + cmbBelong.SelectedValue + "' "
            iCounter = 1
            ' 組合員種別選択ありの場合
            If clbKind.CheckedItems.Count > 0 Then
                sql = sql + " AND ( k_staf_kind = '"
                For Each strValue As DataRowView In clbKind.CheckedItems
                    sql = sql + strValue.Row("c_constant_seq").ToString() + "'"
                    If iCounter < clbKind.CheckedItems.Count Then
                        sql = sql + " OR k_staf_kind = '"
                    End If
                    iCounter = iCounter + 1
                Next
                sql = sql + ")"
            End If
            sql = sql + " ORDER BY l_name_kna, l_name"
            Call dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            Call dbAccess.Disconnect()
            Return dt
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return dt                                                                           ' 戻り値
    End Function

    '************************************************************************************
    '   ＩＤ　：getPrePeriodStartDate
    '   名称　：前期開始日取得
    '   概要　：前期開始日取得、ログイン期の開始日より古い、直近の期の開始日を取得する
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Function getPrePeriodStartDate() As String
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim rtnValue As String = ""                                                         ' 戻り値
        Dim sql As String = ""                                                              ' SQL分
        Dim dbAccess As New CLAccessMdb                                                     ' DBアクセス
        Dim dt As DataTable = Nothing                                                       ' データテーブル
        Try
            ' SQL作成
            sql = "" & vbCrLf
            sql = sql & " SELECT d_from" & vbCrLf
            sql = sql & "   FROM period" & vbCrLf
            sql = sql & "  WHERE c_ksh = '" & MDLoginInfo.Ksh & "'" & vbCrLf
            sql = sql & "    AND d_from < '" & MDLoginInfo.PeriodFrom & "'" & vbCrLf
            sql = sql & "  ORDER BY d_from DESC" & vbCrLf
            sql = sql & ";" & vbCrLf
            'sql = "Select d_from from period where c_ksh='" + MDLoginInfo.Ksh + "' and d_from<'" + MDLoginInfo.PeriodFrom + "' order by d_from DESC"
            Call dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                rtnValue = dt.Rows(0).Item(0).ToString()
                If InStr(rtnValue, "/") < 1 Then
                    rtnValue = Mid(rtnValue, 1, 4) + "/" + Mid(rtnValue, 5, 2) + "/" + Mid(rtnValue, 7, 2)
                End If
            Else
                CLMsg.Show("GE0001")
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call dbAccess.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return rtnValue                                                                     ' 戻り値
    End Function
#End Region

End Class

#End Region