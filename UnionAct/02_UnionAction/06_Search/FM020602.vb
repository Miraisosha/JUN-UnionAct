#Region "FM020602"
'===========================================================================================================
'   クラスＩＤ　　：FM020602
'   クラス名称　　：検索結果画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports System.Collections

Public Class FM020602

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM020602                              ' FM020602
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM020602                          ' 検索結果画面
    Public Const ADDNUM_IDX As Integer = 1
    Public Const ADDNUM_LEN As Integer = 7
    Public Const ADDRESS_IDX As Integer = 8
    Public Const ADDRESS_LEN As Integer = 14
    'Public Const ADDRESS_LEN As Integer = 13
    Public Const BARCODE_LEN As Integer = &H17
    Public Const CC1 As Byte = &H3A
    Public Const CC2 As Byte = &H3B
    Public Const CC3 As Byte = 60
    Public Const CC4 As Byte = &H3D
    Public Const CC5 As Byte = &H3E
    Public Const CC6 As Byte = &H3F
    Public Const CC7 As Byte = &H40
    Public Const CC8 As Byte = &H41
    Public Const CHKDGT_IDX As Integer = &H15
    Public Const CHKDGT_MULT As Integer = &H13
    Public Shared ReadOnly ChkDgtChr As Byte() = New Byte() {&H30, &H31, 50, &H33, &H34, &H35, &H36, &H37, &H38, &H39, &H2D, &H3A, &H3B, 60, &H3D, &H3E, &H3F, &H40, &H41}

    Public Shared ReadOnly HanAlphaLower As Char() = New Char() {"a"c, "b"c, "c"c, "d"c, "e"c, "f"c, "g"c, "h"c, "i"c, "j"c, "k"c, "l"c, "m"c, "n"c, "o"c, "p"c, "q"c, "r"c, "s"c, "t"c, "u"c, "v"c, "w"c, "x"c, "y"c, "z"c}
    Public Shared ReadOnly HanAlphaUpper As Char() = New Char() {"A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c}
    Public Shared ReadOnly ZenAlphaLower As Char() = New Char() {"ａ", "ｂ", "ｃ", "ｄ", "ｅ", "ｆ", "ｇ", "ｈ", "ｉ", "ｊ", "ｋ", "ｌ", "ｍ", "ｎ", "ｏ", "ｐ", "ｑ", "ｒ", "ｓ", "ｔ", "ｕ", "ｖ", "ｗ", "ｘ", "ｙ", "ｚ"}
    Public Shared ReadOnly ZenAlphaUpper As Char() = New Char() {"Ａ", "Ｂ", "Ｃ", "Ｄ", "Ｅ", "Ｆ", "Ｇ", "Ｈ", "Ｉ", "Ｊ", "Ｋ", "Ｌ", "Ｍ", "Ｎ", "Ｏ", "Ｐ", "Ｑ", "Ｒ", "Ｓ", "Ｔ", "Ｕ", "Ｖ", "Ｗ", "Ｘ", "Ｙ", "Ｚ"}

    Public Shared ReadOnly ZenDigit As Char() = New Char() {"０", "１", "２", "３", "４", "５", "６", "７", "８", "９", "－", "‐", "ー", "ｰ", "＆", "／", "・", "．"}
    Public Shared ReadOnly HanDigit As Char() = New Char() {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "-"c, "-"c, "-"c, "-"c, "&"c, "/"c, "･", "."c}

    Public Shared ReadOnly KanDigit As Char() = New Char() {"〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"}

    Public Shared ReadOnly SpecifiedPause As String() = New String() {"丁目", "丁", "番地", "番", "号", "地割", "線", "の", "ノ"}
    Public Const START_CODE As Byte = &H42
    Public Const STOP_CODE As Byte = &H43
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：FM020602_Load
    '   名称　：フォームロード
    '   概要　：画面サイズ変更制御、画面位置制御、コンボボックス作成。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub FM020602_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As DataTable = Nothing
        Try
            '画面サイズ変更不可
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            '画面中心表示
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            'データグリッドビューソート禁止
            For Each c As DataGridViewColumn In dgdSearchResult.Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next c
            '給紙トレイコンボボックス作成
            cboTack.DisplayMember = "SourceName"
            Dim pkSource As System.Drawing.Printing.PaperSource
            Dim printDoc As System.Windows.Forms.PrintDialog = New System.Windows.Forms.PrintDialog()
            For i = 0 To printDoc.PrinterSettings.PaperSources.Count - 1
                pkSource = printDoc.PrinterSettings.PaperSources.Item(i)
                cboTack.Items.Add(pkSource)
            Next
            'コンボボックスの初期値を設定
            If (Me.cboTack.Items.Count > 0) Then
                Me.cboTack.SelectedIndex = 0
            End If
            '権限設定
            dt = MDCommon.getGrant(SCREEN_ID_UC020601)
            If dt.Rows(0).Item(5) = 0 Then
                Me.btnPrint.Enabled = False
                Me.btnTack.Enabled = False
                Me.btnPrintAddress.Enabled = False
            End If
            If dt.Rows(0).Item(6) = 0 Then
                Me.btnFileOutPut.Enabled = False
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "キャンセルボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：画面を閉じる。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "全データ選択ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnAllCheck_Click
    '   名称　：全データ選択ボタンクリック
    '   概要　：全組合員の情報を選択する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnAllCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheck.Click
        Try
            '1列目のチェックボックスをすべて選択する。
            For Each Row As DataGridViewRow In dgdSearchResult.Rows
                Row.Cells.Item(0).Value = True
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "全データ選択解除ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnAllCheck_Click
    '   名称　：全データ選択解除ボタンクリック
    '   概要　：全組合員の情報の選択を解除する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnAllNoCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllNoCheck.Click
        Dim Cells As DataGridViewCellCollection
        Dim Cell As DataGridViewCell
        Try
            '1列目のチェックボックスをすべて選択解除する。
            For Each Row As DataGridViewRow In dgdSearchResult.Rows
                Cells = Row.Cells
                Cell = Cells.Item(0)
                Cell.Value = False
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "プレ印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：プレ印刷ボタンクリック
    '   概要　：「検索結果」印刷プレビュー画面を表示する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim strArray() As String = Nothing
            Dim Count As Integer = 0
            Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument      'IDと名前印刷レポート
            Dim fmPrint As FM000205                                                     '印刷プレビュー
            Dim ds As DS0206P1                                                          'データセット
            Dim i As Integer = 0
            Dim drHeader As DS0206P1.search_list_headerRow
            Dim drPrint As DS0206P1.search_list_printRow
            Dim dt As DataTable
            Dim getdtColumnName As String = ""
            Dim setColumnName As String = ""
            Dim getColumnName As String = ""
            '背景初期化
            Me.pnlSelect.BackColor = System.Drawing.SystemColors.Window
            '印刷項目のチェックボックス群の確認
            If funcSumCheckedBox(strArray, Count) = False Then
                Exit Sub
            End If
            '印刷対象の組合員が選択されていない場合はエラーメッセージを出力
            If chkMem(False) = False Then
                Exit Sub
            End If
            '選択項目数に応じてエラーメッセージを出力
            If Count = 0 Then
                CLMsg.Show("GE0054")
                SetErr(Me.pnlSelect)
                Exit Sub
            ElseIf Count > 10 Then
                CLMsg.Show("GE0055")
                SetErr(Me.pnlSelect)
                Exit Sub
            End If
            ds = New DS0206P1
            'ヘッダー用テーブルに値を追加
            drHeader = ds.search_list_header.NewRow
            drHeader.BeginEdit()
            For Each Array As String In strArray
                i = i + 1
                getdtColumnName = "l_title_" + i.ToString
                drHeader(getdtColumnName) = Array
            Next
            ds.search_list_header.Rows.Add(drHeader)
            '検索結果内容テーブルに値を追加
            For Each row As DataGridViewRow In dgdSearchResult.Rows
                '1列目のチェックがついている場合のみデータテーブルに追加
                If row.Cells.Item(0).Value = True Then
                    drPrint = ds.search_list_print.NewRow
                    drPrint.BeginEdit()
                    '選択された項目をデータテーブルに項目追加する。
                    i = 0
                    For Each Item As String In strArray
                        i = i + 1
                        '値が入っていない場合を考慮してNull確認を行う。
                        If ChkNull(Item) = False Then
                            If Not (row.Cells.Item(Item).Value Is DBNull.Value) Then
                                getColumnName = row.Cells.Item(Item).Value
                                setColumnName = "l_Column_" + i.ToString
                                drPrint(setColumnName) = getColumnName
                            End If
                        End If
                    Next
                    ds.search_list_print.Rows.Add(drPrint)
                End If
            Next
            dt = ds.search_list_header
            '印刷プレビュー
            fmPrint = New FM000205
            reportObj = New CR0206P1(dt)
            fmPrint.ButtonShowType = 2
            fmPrint.PrintCntVisible = True
            fmPrint.PrintAreaVisible = True
            fmPrint.ObjResource = reportObj
            'データセットを設定
            reportObj.SetDataSource(ds)
            Call fmPrint.ShowDialog()
            Select Case fmPrint.IntQlickBtnFlag
                Case 1
                Case 2
                    'キャンセル
                Case 3
                    '印刷
                    fmPrint.PrintOut()
            End Select
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "住所録ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrintAddress_Click
    '   名称　：住所録ボタンクリック
    '   概要　：「住所録」印刷プレビュー画面を表示する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnPrintAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintAddress.Click
        Dim ds As DS0206P3
        Dim drDetail As DS0206P3.dtDetailRow
        Dim fmPrint As FM000203
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument      'IDと名前印刷レポート
        Try
            '印刷対象の組合員が選択されていない場合はエラーメッセージを出力
            If chkMem(False) = False Then
                Exit Sub
            End If
            ds = New DS0206P3
            Dim strColumns As String() = {"社員番号", "名前", "電話番号１", "名前カナ", "住所区分", "郵便番号", "市区町村", "番地等", "建物等", "住所"}
            Dim dt As New DataTable
            Dim dc As DataColumn = Nothing
            Dim drow As DataRow = Nothing
            For Each Array As String In strColumns
                dc = New DataColumn(Array)
                dt.Columns.Add(dc)
            Next
            'データグリッドビューに表示されているすべてのデータに対して処理を行う
            For Each dgrow As DataGridViewRow In dgdSearchResult.Rows
                'チェックされているもののみ出力する
                If dgrow.Cells.Item(0).Value = True Then
                    drow = dt.NewRow
                    drow.BeginEdit()
                    If Not ((dgrow.Cells.Item("社員番号").Value) Is DBNull.Value) Then
                        drow.Item("社員番号") = dgrow.Cells.Item("社員番号").Value
                    End If
                    If Not ((dgrow.Cells.Item("名前").Value) Is DBNull.Value) Then
                        drow.Item("名前") = dgrow.Cells.Item("名前").Value
                    End If
                    If Not ((dgrow.Cells.Item("電話番号１").Value) Is DBNull.Value) Then
                        drow.Item("電話番号１") = dgrow.Cells.Item("電話番号１").Value
                    End If
                    If Not ((dgrow.Cells.Item("名前カナ").Value) Is DBNull.Value) Then
                        drow.Item("名前カナ") = dgrow.Cells.Item("名前カナ").Value
                    End If
                    '住所区分が設定されている場合に出力する内容を確認する。
                    If Not ((dgrow.Cells.Item("住所区分").Value) Is DBNull.Value) Then
                        '住所区分が国内の場合、郵便番号・市区町村・番地等・建物等を設定する。
                        drow.Item("住所区分") = dgrow.Cells.Item("住所区分").Value
                        If dgrow.Cells.Item("住所区分").Value = "0" Then
                            '郵便番号が登録されていないユーザーは住所情報を表示しない。
                            If Not ((dgrow.Cells.Item("郵便番号").Value) Is DBNull.Value) Then
                                drow.Item("郵便番号") = dgrow.Cells.Item("郵便番号").Value
                                If Not ((dgrow.Cells.Item("市区町村").Value) Is DBNull.Value) Then
                                    drow.Item("市区町村") = dgrow.Cells.Item("市区町村").Value
                                End If
                                If Not ((dgrow.Cells.Item("番地等").Value) Is DBNull.Value) Then
                                    drow.Item("番地等") = dgrow.Cells.Item("番地等").Value
                                End If
                                If Not ((dgrow.Cells.Item("建物等").Value) Is DBNull.Value) Then
                                    drow.Item("建物等") = dgrow.Cells.Item("建物等").Value
                                End If
                            End If
                        Else
                            '住所が海外の場合は、郵便番号登録不可のため、有/無に関係なく住所を表示する。
                            If Not ((dgrow.Cells.Item("住所").Value) Is DBNull.Value) Then
                                drow.Item("住所") = dgrow.Cells.Item("住所").Value
                            End If
                        End If
                    End If
                    dt.Rows.Add(drow)
                End If
            Next
            '出力する情報のみのデータテーブルにソートをかける
            Dim dtRow As DataRow() = Nothing
            dtRow = dt.Select(Nothing, "名前カナ ASC").Clone
            Dim dtSort As New DataTable
            dtSort = dt.Clone
            'ソートされたデータテーブルを作成する
            For Each row2 As DataRow In dtRow
                dtSort.ImportRow(row2)
            Next
            'ソートされたテーブルをもとに出力用のデータテーブルを作成する。
            For Each dtSortrow As DataRow In dtSort.Rows
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                If Not (dtSortrow.Item("社員番号") Is DBNull.Value) Then
                    drDetail.c_staf_id = dtSortrow.Item("社員番号")
                End If
                If Not (dtSortrow.Item("名前") Is DBNull.Value) Then
                    drDetail.l_name = dtSortrow.Item("名前")
                End If
                If Not (dtSortrow.Item("電話番号１") Is DBNull.Value) Then
                    drDetail.l_tell_1 = dtSortrow.Item("電話番号１")
                End If
                If Not (dtSortrow.Item("名前カナ") Is DBNull.Value) Then
                    drDetail.l_name_kna = dtSortrow.Item("名前カナ")
                End If
                '住所区分が設定されている場合に出力する内容を確認する。
                If Not (dtSortrow.Item("住所区分") Is DBNull.Value) Then
                    '住所区分が国内の場合、郵便番号・市区町村・番地等・建物等を設定する。
                    If dtSortrow.Item("住所区分") = "0" Then
                        '郵便番号が登録されていないユーザーは住所情報を表示しない。
                        If Not (dtSortrow.Item("郵便番号") Is DBNull.Value) Then
                            drDetail.l_add_number = dtSortrow.Item("郵便番号")
                            If Not (dtSortrow.Item("市区町村") Is DBNull.Value) Then
                                drDetail.l_cities = dtSortrow.Item("市区町村")
                            End If
                            If Not (dtSortrow.Item("番地等") Is DBNull.Value) Then
                                drDetail.l_add_ather = dtSortrow.Item("番地等")
                            End If
                            If Not (dtSortrow.Item("建物等") Is DBNull.Value) Then
                                drDetail.l_building = dtSortrow.Item("建物等")
                            End If
                        End If
                    Else
                        '住所が海外の場合は、郵便番号登録不可のため、有/無に関係なく住所を表示する。
                        If Not (dtSortrow.Item("住所") Is DBNull.Value) Then
                            drDetail.l_cities = dtSortrow.Item("住所")
                        End If
                    End If
                End If
                ds.dtDetail.Rows.Add(drDetail)
            Next
            '印刷プレビュー
            fmPrint = New FM000203
            reportObj = New CR0206P3
            fmPrint.ButtonShowType = 3
            fmPrint.PrintCntVisible = False
            fmPrint.ObjResource = reportObj
            'データセットを設定
            reportObj.SetDataSource(ds)
            Call fmPrint.ShowDialog()
            Select Case fmPrint.IntQlickBtnFlag
                Case 1
                Case 2
                    'キャンセル
                Case 3
                    '印刷
                    fmPrint.PrintOut()
            End Select
            fmPrint.Dispose()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "タックシールボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：タックシールボタンクリック
    '   概要　：「タックシール」を出力する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnTack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTack.Click
        Try
            Dim BarCode As String = ""
            Dim ds As DS0206P2
            Dim drHeader As DS0206P2.dtHeaderRow
            Dim fmPrint As FM000203
            Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument      'IDと名前印刷レポート
            Dim pkSource As System.Drawing.Printing.PaperSource
            Dim printDoc As System.Windows.Forms.PrintDialog = New System.Windows.Forms.PrintDialog()
            Dim daiRet As DialogResult = Nothing    ' 確認メッセージ結果
            Dim strRemove() As String = Nothing
            Dim SkipFlg As Boolean = False
            '印刷対象組合員確認
            If chkMem(True, strRemove) = False Then
                Exit Sub
            End If
            '印刷続行確認
            daiRet = CLMsg.Show("GQ0015", "タックシール給紙方法")
            If daiRet = DialogResult.No Then
                Exit Sub
            End If
            '出力処理
            ds = New DS0206P2
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            For Each Row As DataGridViewRow In dgdSearchResult.Rows
                '選択されているユーザーのみ処理を行う。
                If Row.Cells.Item(0).Value = True Then
                    'スキップフラグ初期化
                    SkipFlg = False
                    If Not (strRemove Is Nothing) Then
                        For Each Array As String In strRemove
                            If Row.Cells.Item("社員番号").Value.ToString = Array Then
                                SkipFlg = True
                            End If
                        Next
                    End If
                    If SkipFlg = False Then
                        drHeader = ds.dtHeader.NewRow
                        drHeader.BeginEdit()
                        '郵便番号設定
                        If Not (Row.Cells.Item("郵便番号").Value Is DBNull.Value) Then
                            drHeader.l_add_number = Row.Cells.Item("郵便番号").Value
                        End If
                        '都道府県設定
                        If Not (Row.Cells.Item("都道府県").Value Is DBNull.Value) Then
                            drHeader.l_prefectures = Row.Cells.Item("都道府県").Value
                        End If
                        '市区町村設定
                        If Not (Row.Cells.Item("市区町村").Value Is DBNull.Value) Then
                            drHeader.l_cities = Row.Cells.Item("市区町村").Value
                        End If
                        '番地等設定
                        If Not (Row.Cells.Item("番地等").Value Is DBNull.Value) Then
                            drHeader.l_add_ather = Row.Cells.Item("番地等").Value
                        End If
                        '建物等設定
                        If Not (Row.Cells.Item("建物等").Value Is DBNull.Value) Then
                            drHeader.l_building = Row.Cells.Item("建物等").Value
                        End If
                        '住所区分設定
                        If Not (Row.Cells.Item("住所区分").Value Is DBNull.Value) Then
                            drHeader.k_foreign = Row.Cells.Item("住所区分").Value
                        End If
                        '海外アドレス1設定
                        If Not (Row.Cells.Item("海外アドレス1").Value Is DBNull.Value) Then
                            drHeader.l_foreign_adress_1 = Row.Cells.Item("海外アドレス1").Value
                        End If
                        '海外アドレス2設定
                        If Not (Row.Cells.Item("海外アドレス2").Value Is DBNull.Value) Then
                            drHeader.l_foreign_adress_2 = Row.Cells.Item("海外アドレス2").Value
                        End If
                        '海外アドレス3設定
                        If Not (Row.Cells.Item("海外アドレス3").Value Is DBNull.Value) Then
                            drHeader.l_foreign_adress_3 = Row.Cells.Item("海外アドレス3").Value
                        End If
                        '海外アドレス4設定
                        If Not (Row.Cells.Item("海外アドレス4").Value Is DBNull.Value) Then
                            drHeader.l_foreign_adress_4 = Row.Cells.Item("海外アドレス4").Value
                        End If
                        '海外アドレス5設定
                        If Not (Row.Cells.Item("海外アドレス5").Value Is DBNull.Value) Then
                            drHeader.l_foreign_adress_5 = Row.Cells.Item("海外アドレス5").Value
                        End If
                        '名前設定
                        If Not (Row.Cells.Item("名前").Value Is DBNull.Value) Then
                            drHeader.l_name = Row.Cells.Item("名前").Value
                        End If
                        '社員番号設定
                        If Not (Row.Cells.Item("社員番号").Value Is DBNull.Value) Then
                            drHeader.c_staf_id = Row.Cells.Item("社員番号").Value
                        End If
                        '機種略称設定
                        If Not (Row.Cells.Item("機種略称").Value Is DBNull.Value) Then
                            drHeader.k_model = Row.Cells.Item("機種略称").Value
                        End If
                        '機長略称設定
                        If Not (Row.Cells.Item("機種略称").Value Is DBNull.Value) Then
                            drHeader.k_qualification = Row.Cells.Item("機種略称").Value
                        End If
                        '正組合員種別略称
                        If Not (Row.Cells.Item("組合員種別略称").Value Is DBNull.Value) Then
                            drHeader.k_staf_kind = Row.Cells.Item("組合員種別略称").Value
                        End If
                        'バーコード設定
                        If Not (Row.Cells.Item("").Value Is DBNull.Value) Then
                            BarCode = GetBarCode(Row.Cells.Item("住所区分").Value.ToString, _
                                                 Row.Cells.Item("郵便番号").Value.ToString, _
                                                 Row.Cells.Item("番地等").Value.ToString, _
                                                 Row.Cells.Item("建物等").Value.ToString)
                            drHeader.bar_code = BarCode
                        End If
                        'データセットに追加
                        ds.dtHeader.Rows.Add(drHeader)
                    End If
                End If
            Next
            If ds.dtHeader.Rows.Count = 0 Then
                CLMsg.Show("RI0001")
                Exit Sub
            End If
            fmPrint = New FM000203
            reportObj = New CR0206P2
            fmPrint.ButtonShowType = 3
            fmPrint.PrintCntVisible = False
            fmPrint.ObjResource = reportObj
            'データセットを設定
            reportObj.SetDataSource(ds)
            '給紙トレイを設定
            pkSource = printDoc.PrinterSettings.PaperSources.Item(Me.cboTack.SelectedIndex)
            reportObj.PrintOptions.CustomPaperSource = pkSource
            '印刷プレビューはないため、そのまま印刷開始
            fmPrint.PrintOut()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "ファイル出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnFileOutPut_Click
    '   名称　：ファイル出力ボタンクリック
    '   概要　：CSVファイルを出力する。
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnFileOutPut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileOutPut.Click
        Try
            Dim sfd As New SaveFileDialog With { _
            .FileName = "無題" & ".csv", _
            .InitialDirectory = "C:\", _
            .Filter = "CSV" & "ファイル" & "(*.csv)|*.csv;|" & "すべてのファイル" & "(*.*)|*.*", _
            .FilterIndex = 0, _
            .Title = "ファイルの保存先を選択してください", _
            .OverwritePrompt = True, _
            .CheckPathExists = True}
            Dim strFileName As String = ""
            '印刷対象の組合員が選択されていない場合はエラーメッセージを出力
            If chkMem(False) = False Then
                Exit Sub
            End If
            '保存ボタンをクリックされたら出力処理を行う
            If (sfd.ShowDialog = DialogResult.OK) Then
                '出力ファイルパスを取得
                strFileName = sfd.FileName
                If funcPutFile(strFileName) = False Then
                    CLMsg.Show("BE0022", strFileName)
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region
#End Region

#Region "関数"
#Region "選択印刷項目数確認処理"
    '***************************************************************************************************
    '   ＩＤ　：funcSumCheckedBox
    '   名称　：選択印刷項目数確認処理
    '   概要　：印刷項目として選択されている値のリストと項目数を返却する。
    '   引数　：ByRef strArray() As String  = 選択項目リスト
    '       　：ByRef Count      As Integer = 選択項目数(住所は4項目分)
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '           2012/02/01(水)  m.suzuki  メールアドレス1・2追加
    '***************************************************************************************************
    ''' <summary>選択印刷項目数確認</summary>
    ''' <param name="strArray">選択項目リスト</param>
    ''' <param name="Count">選択項目数(住所は4項目分)</param> 
    ''' <remarks></remarks>
    Function funcSumCheckedBox(ByRef strArray() As String, ByRef Count As Integer) As Boolean
        Dim booRet As Boolean = False
        Try
            '社員番号の確認
            If Me.chkEmpNo.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkEmpNo.Text
                Count = Count + 1
            End If
            '名前の確認
            If Me.chkEmpName.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkEmpName.Text
                Count = Count + 1
            End If
            '名前カナの確認
            If Me.chkEmpNameKna.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkEmpNameKna.Text
                Count = Count + 1
            End If
            '組合員種別の確認
            If Me.chkMemberKind.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkMemberKind.Text
                Count = Count + 1
            End If
            'ステータスの確認
            If Me.chkStatus.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkStatus.Text
                Count = Count + 1
            End If
            '組合支部の確認
            If Me.chkBranch.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkBranch.Text
                Count = Count + 1
            End If
            '資格の確認
            If Me.chkCrewLicence.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkCrewLicence.Text
                Count = Count + 1
            End If
            '機種の確認
            If Me.chkModel.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkModel.Text
                Count = Count + 1
            End If
            '会社所属
            If Me.chkCompany.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkCompany.Text
                Count = Count + 1
            End If
            '職場
            If Me.chkOffice.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkOffice.Text
                Count = Count + 1
            End If
            '所属会社
            If Me.chkAttachCompany.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkAttachCompany.Text
                Count = Count + 1
            End If
            '機長年月日
            If Me.chkCap.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkCap.Text
                Count = Count + 1
            End If
            '加入年月日
            If Me.chkJoin.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkJoin.Text
                Count = Count + 1
            End If
            '生年月日
            If Me.chkBirth.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkBirth.Text
                Count = Count + 1
            End If
            '入社年月日
            If Me.chkEntry.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkEntry.Text
                Count = Count + 1
            End If
            '退職年月日
            If Me.chkRetire.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkRetire.Text
                Count = Count + 1
            End If
            '勤務状態
            If Me.chkWorkForm.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkWorkForm.Text
                Count = Count + 1
            End If
            '郵便番号
            If Me.chkAddNumber.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkAddNumber.Text
                Count = Count + 1
            End If
            '住所…住所を印刷項目に選択した場合は、7項目までしか選択できないため＋４インクリメントする。
            If Me.chkAddress.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkAddress.Text
                Count = Count + 4
            End If
            '電話番号1
            If Me.chkTell1.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkTell1.Text
                Count = Count + 1
            End If
            '電話番号2
            If Me.chkTell2.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkTell2.Text
                Count = Count + 1
            End If
            ' Insert 2012/02/01(水) m.suzuki  メールアドレス1・2追加 Start
            'メールアドレス1
            If Me.chkMail1.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkMail1.Text
                Count = Count + 1
            End If
            'メールアドレス2
            If Me.chkMail2.Checked = True Then
                ReDim Preserve strArray(Count)
                strArray(Count) = Me.chkMail2.Text
                Count = Count + 1
            End If
            ' Insert 2012/02/01(水) m.suzuki  メールアドレス1・2追加 End
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return booRet
    End Function
#End Region

#Region "バーコード取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetBarCode
    '   名称　：バーコード取得処理
    '   概要　：
    '   引数　：ByVal strForeign   As String = , 
    '           ByVal strAddNumber As String = , 
    '           ByVal strAddAther  As String = , 
    '           ByVal strBuilding  As String = 
    '   戻り値：
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>バーコード取得処理</summary>
    ''' <param name="strForeign"></param>
    ''' <param name="strAddNumber"></param>
    ''' <param name="strAddAther"></param>
    ''' <param name="strBuilding"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBarCode(ByVal strForeign As String, ByVal strAddNumber As String, ByVal strAddAther As String, ByVal strBuilding As String) As String
        Dim str As String = ""
        Try
            Dim destinationArray As Byte() = New Byte(&H17 - 1) {}
            'すべての英字を大文字英字(半角)、すべての数字を半角に変換する。（郵便番号）
            strAddNumber = Me.ConvertZenToHan(strAddNumber)
            'すべての英字を大文字英字(半角)、すべての数字を半角に変換する。（番地等）
            strAddAther = Me.ConvertZenToHan(strAddAther)
            'すべての英字を大文字英字(半角)、すべての数字を半角に変換する。（建物等）
            strBuilding = Me.ConvertZenToHan(strBuilding)
            '郵便番号の"-"を除外する。　→　バーコード表示する場合に"-"はいらないため。
            strAddNumber = strAddNumber.Replace("-", "")
            If ((strForeign.Equals("1") OrElse strAddNumber.Equals("")) OrElse (strAddNumber.EndsWith("00") OrElse (strAddNumber.Length <> 7))) Then
                Return ""
            End If
            destinationArray(0) = &H42
            Dim i As Integer
            For i = 0 To strAddNumber.Length - 1
                destinationArray((1 + i)) = CByte(Microsoft.VisualBasic.AscW(strAddNumber.Chars(i)))
                'destinationArray((1 + i)) = CByte(Microsoft.VisualBasic.Val(strAddNumber.Chars(i)))
            Next i
            Array.Copy(Me.CreateAddressNum((strAddAther & strBuilding)), 0, destinationArray, 8, 13)
            destinationArray(&H15) = Me.CalcCheckDigit(destinationArray)
            destinationArray(&H16) = &H43
            str = System.Text.Encoding.GetEncoding("shift_jis").GetString(destinationArray)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return str
    End Function
#End Region

#Region "英字全角半角変換処理"
    '***************************************************************************************************
    '   ＩＤ　：ConvertZenToHan
    '   名称　：英字全角半角変換処理
    '   概要　：英数全角文字列を半角文字列に変換する処理
    '   引数　：ByVal str As String = 変換前の全角文字列
    '   戻り値：半角に変換した文字列
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>英字全角半角変換処理</summary>
    ''' <param name="str">変換前の全角文字列</param>
    ''' <returns>半角に変換した文字列</returns>
    ''' <remarks></remarks>
    Private Function ConvertZenToHan(ByVal str As String) As String
        Dim str3 As String = ""
        Try
            Dim str2 As String = String.Copy(str)
            Dim i As Integer
            'すべての英字を大文字英字(半角)に変換する。
            For i = 0 To HanAlphaUpper.Length - 1
                str2 = str2.Replace(ZenAlphaUpper(i), HanAlphaUpper(i)).Replace(ZenAlphaLower(i), HanAlphaUpper(i)).Replace(HanAlphaLower(i), HanAlphaUpper(i))
            Next i
            'すべての全角数字を半角数字に変換する。
            Dim j As Integer
            For j = 0 To HanDigit.Length - 1
                str2 = str2.Replace(ZenDigit(j), HanDigit(j))
            Next j
            '返却結果
            str3 = str2
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '変換後の文字列を返却する。
        Return str3
    End Function
#End Region

#Region "CreateAddressNum"
    '***************************************************************************************************
    '   ＩＤ　：CreateAddressNum
    '   名称　：
    '   概要　：
    '   引数　：ByVal strAddress As String
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="strAddress"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAddressNum(ByVal strAddress As String) As Byte()
        Dim buffer2 As Byte() = Nothing
        Try
            Dim lstAddressNum As New List(Of Byte)
            strAddress = strAddress.Replace("&", "")
            strAddress = strAddress.Replace("/", "")
            strAddress = strAddress.Replace("･", "")
            strAddress = strAddress.Replace(".", "")
            strAddress = strAddress.Insert(0, "-")
            strAddress = strAddress.Insert(strAddress.Length, "-")
            Dim i As Integer = 1
            Do While (i <= (strAddress.Length - 2))
                If (Me.IsDigit(strAddress.Chars(i)) OrElse (strAddress.Chars(i) = "-"c)) Then
                    lstAddressNum.Add(CByte(Microsoft.VisualBasic.AscW(strAddress.Chars(i))))
                    'lstAddressNum.Add(CByte(Microsoft.VisualBasic.Val(strAddress.Chars(i))))
                ElseIf Me.IsUpAlpha(strAddress.Chars(i)) Then
                    If (Not Me.IsUpAlpha(strAddress.Chars((i - 1))) AndAlso Not Me.IsUpAlpha(strAddress.Chars((i + 1)))) Then
                        If (Me.IsDigit(strAddress.Chars((i - 1))) AndAlso (strAddress.Chars(i) = "F"c)) Then
                            If Me.IsDigit(strAddress.Chars((i + 1))) Then
                                lstAddressNum.Add(&H2D)
                            End If
                        Else
                            lstAddressNum.Add(CByte(Microsoft.VisualBasic.AscW(strAddress.Chars(i))))
                            'lstAddressNum.Add(CByte(Microsoft.VisualBasic.Val(strAddress.Chars(i))))
                        End If
                    End If
                Else
                    lstAddressNum.Add(&H2D)
                End If
                i += 1
            Loop
            lstAddressNum.Add(&H2D)
            lstAddressNum = Me.DeleteHyphen(lstAddressNum)
            lstAddressNum = Me.ConvertAlphaToCode(lstAddressNum)
            Dim buffer As Byte() = New Byte(13 - 1) {}
            Dim index As Integer = 0
            Do While ((index < lstAddressNum.Count) AndAlso (index < 13))
                buffer(index) = lstAddressNum.Item(index)
                index += 1
            Loop
            Do While (index < 13)
                buffer(index) = &H3D
                index += 1
            Loop
            buffer2 = buffer
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return buffer2
    End Function
#End Region

#Region "IsDegit(Byte)"
    '***************************************************************************************************
    '   ＩＤ　：IsDigit
    '   名称　：
    '   概要　：
    '   引数　：ByVal by As Byte
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="by"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsDigit(ByVal by As Byte) As Boolean
        Return ((by >= &H30) AndAlso (by <= &H39))
    End Function
#End Region

#Region "IsDegit(Char)"
    '***************************************************************************************************
    '   ＩＤ　：IsDigit
    '   名称　：
    '   概要　：
    '   引数　：ByVal ch As Char
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="ch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsDigit(ByVal ch As Char) As Boolean
        Return ((ch >= "0"c) AndAlso (ch <= "9"c))
    End Function
#End Region

#Region "IsUpAlpha(Byte)"
    '***************************************************************************************************
    '   ＩＤ　：IsUpAlpha
    '   名称　：
    '   概要　：
    '   引数　：ByVal by As Byte
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="by"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsUpAlpha(ByVal by As Byte) As Boolean
        Return ((by >= &H41) AndAlso (by <= 90))
    End Function
#End Region

#Region "IsUpAlpha(Char)"
    '***************************************************************************************************
    '   ＩＤ　：IsUpAlpha
    '   名称　：
    '   概要　：
    '   引数　：ByVal ch As Char
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="ch"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsUpAlpha(ByVal ch As Char) As Boolean
        Return ((ch >= "A"c) AndAlso (ch <= "Z"c))
    End Function
#End Region

#Region "DeleteHyphen"
    '***************************************************************************************************
    '   ＩＤ　：DeleteHyphen
    '   名称　：
    '   概要　：
    '   引数　：ByVal lstAddressNum As List(Of Byte)
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="lstAddressNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteHyphen(ByVal lstAddressNum As List(Of Byte)) As List(Of Byte)
        Dim list As List(Of Byte) = Nothing
        Try
            Dim i As Integer = 0
            Do Until i > (lstAddressNum.Count - 1) - 1
                If ((lstAddressNum.Item(i) = &H2D) AndAlso (lstAddressNum.Item((i + 1)) = &H2D)) Then
                    lstAddressNum.RemoveAt((i + 1))
                    i -= 1
                End If

                i += 1
            Loop
            Dim j As Integer = 1
            Do While (j <= (lstAddressNum.Count - 1))
                If Me.IsUpAlpha(lstAddressNum.Item(j)) Then
                    If (lstAddressNum.Item((j + 1)) = &H2D) Then
                        lstAddressNum.RemoveAt((j + 1))
                    End If
                    If (lstAddressNum.Item((j - 1)) = &H2D) Then
                        lstAddressNum.RemoveAt((j - 1))
                        j = If(((j - 1) < 1), 1, (j - 1))
                    End If
                End If
                j += 1
            Loop
            If (lstAddressNum.Item(0) = &H2D) Then
                lstAddressNum.RemoveAt(0)
            End If
            If ((lstAddressNum.Count > 1) AndAlso (lstAddressNum.Item((lstAddressNum.Count - 1)) = &H2D)) Then
                lstAddressNum.RemoveAt((lstAddressNum.Count - 1))
            End If
            list = lstAddressNum
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return list
    End Function
#End Region

#Region "ConvertAlphaToCode"
    '***************************************************************************************************
    '   ＩＤ　：ConvertAlphaToCode
    '   名称　：
    '   概要　：
    '   引数　：ByVal lstAddressNum As List(Of Byte)
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="lstAddressNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConvertAlphaToCode(ByVal lstAddressNum As List(Of Byte)) As List(Of Byte)
        Dim list2 As List(Of Byte) = Nothing
        Try
            Dim list As New List(Of Byte)
            Dim buffer As Byte() = New Byte() {&H3A, &H3B, 60}
            Dim i As Integer
            For i = 0 To lstAddressNum.Count - 1
                If Me.IsUpAlpha(lstAddressNum.Item(i)) Then
                    Dim num2 As Byte = (lstAddressNum.Item(i) - &H41)
                    list.Add(buffer((num2 / 10)))
                    list.Add(CByte(((num2 Mod 10) + &H30)))
                Else
                    list.Add(lstAddressNum.Item(i))
                End If
            Next i
            list2 = list
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return list2
    End Function
#End Region

#Region "CalcCheckDigit"
    '***************************************************************************************************
    '   ＩＤ　：CalcCheckDigit
    '   名称　：
    '   概要　：
    '   引数　：ByVal byBarCode As Byte()
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="byBarCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalcCheckDigit(ByVal byBarCode As Byte()) As Byte
        Dim chkDgtChr As Byte
        Try
            Dim num As Integer = 0
            Dim i As Integer
            For i = 1 To &H15 - 1
                num = (num + Me.GetChkDgtNum(byBarCode(i)))
            Next i
            Dim num3 As Integer = ((&H13 - (num Mod &H13)) Mod &H13)
            chkDgtChr = Me.GetChkDgtChr(num3)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return chkDgtChr
    End Function
#End Region

#Region "GetChkDgtNum"
    '***************************************************************************************************
    '   ＩＤ　：GetChkDgtNum
    '   名称　：
    '   概要　：
    '   引数　：ByVal character As Byte
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="character"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChkDgtNum(ByVal character As Byte) As Integer
        Dim num2 As Integer
        Try
            Dim i As Integer
            For i = 0 To ChkDgtChr.Length - 1
                If (character = ChkDgtChr(i)) Then
                    Return i
                End If
            Next i
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return num2
    End Function
#End Region

#Region "GetChkDgtChr"
    '***************************************************************************************************
    '   ＩＤ　：GetChkDgtChr
    '   名称　：
    '   概要　：
    '   引数　：ByVal num As Integer
    '   戻り値：
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/16(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="num"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChkDgtChr(ByVal num As Integer) As Byte
        Dim num2 As Byte
        Try
            If ((num < 0) OrElse (num >= ChkDgtChr.Length)) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0 - 1) {})
                CLMsg.Show("BE0001")
            End If
            num2 = ChkDgtChr(num)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return num2
    End Function
#End Region

#Region "組合員選択確認"
    '***************************************************************************************************
    '   ＩＤ　：chkMem
    '   名称　：組合員選択確認
    '   概要　：組合員が選択されているか確認する。
    '   引数　：ByVal chkPostCode As Boolean = True(郵便番号の確認有)/False(郵便番号の確認無)
    '       　：ByRef strRemove() As String  = 除外項目リスト
    '   戻り値：booRet As boolean = True/False
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>組合員選択確認</summary>
    ''' <param name="chkPostCode">郵便番号確認有/無</param>
    ''' <param name="strRemove">除外項目リスト</param> 
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function chkMem(ByVal chkPostCode As Boolean, Optional ByRef strRemove() As String = Nothing) As Boolean
        Dim booRet As Boolean = False
        Dim chkCount As Integer = 0             '選択されている組合員の数
        Dim RemoveCnt As Integer = 0            '除外する組合員の数
        Dim NotAddInfoCnt As Integer = 0        '住所情報が登録されていない組合員の数
        Dim NotPostCodeCnt As Integer = 0       '郵便番号が設定されていないまたは特殊パターン組合員の数
        Dim strErrMsg() As String = Nothing     '郵便番号(住所情報)が存在しない組合員の数
        Dim strErrMsg2() As String = Nothing    '住所情報は登録されているが、海外アドレスまたは郵便番号の下2桁が"00"の組合員の数
        Dim ErrForm As FM000105
        Dim strText As String = ""
        Dim strText2 As String = ""
        Try
            '印刷対象の組合員が選択されていない場合はエラーメッセージを出力
            For Each dr As DataGridViewRow In dgdSearchResult.Rows
                '選択されている組合員の数を確認する。
                If dr.Cells.Item(0).Value = True Then
                    chkCount = chkCount + 1
                    If chkPostCode = True Then
                        '郵便番号(住所情報)が設定されていない場合
                        If (dr.Cells.Item("郵便番号").Value Is DBNull.Value) And ((dr.Cells.Item("住所区分").Value.ToString = "0") Or (dr.Cells.Item("住所区分").Value Is DBNull.Value)) Then
                            '登録されていない場合
                            'strErrMsgの0番目は定型文が入るため、先にインクリメント
                            NotAddInfoCnt = NotAddInfoCnt + 1
                            'エラーメッセージに追加
                            ReDim Preserve strErrMsg(NotAddInfoCnt)
                            strErrMsg(NotAddInfoCnt) = "社員番号：" & dr.Cells.Item("社員番号").Value.ToString & " 名前：" & dr.Cells.Item("名前").Value.ToString
                            '除外リストに追加する。
                            ReDim Preserve strRemove(RemoveCnt)
                            strRemove(RemoveCnt) = dr.Cells.Item("社員番号").Value.ToString
                            '除外組合員数インクリメント
                            RemoveCnt = RemoveCnt + 1
                        Else
                            '郵便番号(住所情報)が設定されている場合
                            If ((dr.Cells.Item("住所区分").Value.ToString = "1")) Then
                                'strErrMsg2の0番目は定型文が入るため、先にインクリメント
                                NotPostCodeCnt = NotPostCodeCnt + 1
                                'エラーメッセージに追加
                                ReDim Preserve strErrMsg2(NotPostCodeCnt)
                                strErrMsg2(NotPostCodeCnt) = "社員番号：" & dr.Cells.Item("社員番号").Value.ToString & " 名前：" & dr.Cells.Item("名前").Value.ToString
                            End If
                            '郵便番号(住所情報)が設定されている場合
                            If (Not (dr.Cells.Item("郵便番号").Value Is DBNull.Value)) Then
                                If ((Strings.Right((dr.Cells.Item("郵便番号").Value), 2)) = "00") Then
                                    'strErrMsg2の0番目は定型文が入るため、先にインクリメント
                                    NotPostCodeCnt = NotPostCodeCnt + 1
                                    'エラーメッセージに追加
                                    ReDim Preserve strErrMsg2(NotPostCodeCnt)
                                    strErrMsg2(NotPostCodeCnt) = "社員番号：" & dr.Cells.Item("社員番号").Value.ToString & " 名前：" & dr.Cells.Item("名前").Value.ToString
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            '選択されている組合員が存在しない場合のメッセージ
            If chkCount = 0 Then
                CLMsg.Show("BE0020", "印刷")
                Return booRet
            End If
            'エラーメッセージの確認
            If Not (strErrMsg Is Nothing) Then

                '選択されたユーザーすべてに住所情報が設定されていない場合は、エラーメッセージを表示
                If chkCount = NotAddInfoCnt Then
                    CLMsg.Show("BE0019")
                    Return False
                End If

                '1行目に固定文字列を追加する。
                strErrMsg(0) = "以下の方は住所情報が存在しないため、タックシール印刷はできません。"
                '最後の行に固定文字列を追加する。
                ReDim Preserve strErrMsg(NotAddInfoCnt + 1)
                strErrMsg(NotAddInfoCnt + 1) = "タックシールが出力されませんがよろしいですか。"
                ErrForm = New FM000105
                'エラーメッセージをフォームに設定
                For Each Array As String In strErrMsg
                    strText = strText & Array & vbCrLf
                Next
                'テキスト初期化
                ErrForm.txtErr.Clear()
                ErrForm.txtErr.Text = strText
                'エラーメッセージ用フォーム(FM000105)を呼び出す
                ErrForm.ShowDialog()
                'エラーメッセージの確認
                If ErrForm.DialogResult = Windows.Forms.DialogResult.No Then
                    '印刷処理を中断する。
                    Return False
                End If
                ErrForm.Dispose()
            End If
            'エラーメッセージ2の確認
            If Not (strErrMsg2 Is Nothing) Then
                '1行目に固定文字列を追加する。
                strErrMsg2(0) = "以下の方は郵便番号が登録されていないか、代表番号（下2桁が'00'）になっています。"
                '最後の行に固定文字列を追加する。
                ReDim Preserve strErrMsg2(NotPostCodeCnt + 1)
                strErrMsg2(NotPostCodeCnt + 1) = "タックシールにバーコードが出力されませんがよろしいですか。"
                ErrForm = New FM000105
                'エラーメッセージをフォームに設定
                For Each Array As String In strErrMsg2
                    strText2 = strText2 & Array & vbCrLf
                Next
                'テキスト初期化
                ErrForm.txtErr.Clear()
                ErrForm.txtErr.Text = strText2
                'エラーメッセージ用フォーム(FM000105)を呼び出す
                ErrForm.ShowDialog()
                'エラーメッセージの確認
                If ErrForm.DialogResult = Windows.Forms.DialogResult.No Then
                    '印刷処理を中断する。
                    Return False
                End If
                ErrForm.Dispose()
            End If
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return booRet
    End Function
#End Region

#Region "ファイル出力"
    '***************************************************************************************************
    '   ＩＤ　：funcPutFile
    '   名称　：ファイル出力
    '   概要　：CSVファイルを出力する。
    '   引数　：ByVal strFileName As String = CSVファイル名
    '   戻り値：booRet As boolean = True/False
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>ファイル出力</summary>
    ''' <param name="strFileName">CSVファイル名</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function funcPutFile(ByVal strFileName As String) As Boolean
        Dim booRet As Boolean = False
        Dim sr As System.IO.StreamWriter = Nothing
        Try
            'CSVで保存するDataTable
            Dim dt As DataTable = funcCreateCSVDT()
            '保存先のCSVファイルのパス
            Dim csvPath As String = strFileName
            'CSVファイルに書き込むときに使うEncoding
            Dim enc As System.Text.Encoding = _
                System.Text.Encoding.GetEncoding("Shift_JIS")

            '開く
            sr = New System.IO.StreamWriter(csvPath, False, enc)

            Dim colCount As Integer = dt.Columns.Count
            Dim lastColIndex As Integer = colCount - 1

            'ヘッダを書き込む
            Dim i As Integer
            For i = 0 To colCount - 1
                'ヘッダの取得
                Dim field As String = dt.Columns(i).Caption
                '"で囲む必要があるか調べる
                If field.IndexOf(ControlChars.Quote) > -1 OrElse _
                    field.IndexOf(","c) > -1 OrElse _
                    field.IndexOf(ControlChars.Cr) > -1 OrElse _
                    field.IndexOf(ControlChars.Lf) > -1 OrElse _
                    field.StartsWith(" ") OrElse _
                    field.StartsWith(ControlChars.Tab) OrElse _
                    field.EndsWith(" ") OrElse _
                    field.EndsWith(ControlChars.Tab) Then
                    If field.IndexOf(ControlChars.Quote) > -1 Then
                        '"を""とする
                        field = field.Replace("""", """""")
                    End If
                    field = """" + field + """"
                End If
                'フィールドを書き込む
                sr.Write(field)
                'カンマを書き込む
                If lastColIndex > i Then
                    sr.Write(","c)
                End If
            Next i
            '改行する
            sr.Write(ControlChars.Cr + ControlChars.Lf)
            'レコードを書き込む
            Dim row As DataRow
            For Each row In dt.Rows
                For i = 0 To colCount - 1
                    'フィールドの取得
                    Dim field As String = row(i).ToString()
                    '"で囲む必要があるか調べる
                    If field.IndexOf(ControlChars.Quote) > -1 OrElse _
                        field.IndexOf(","c) > -1 OrElse _
                        field.IndexOf(ControlChars.Cr) > -1 OrElse _
                        field.IndexOf(ControlChars.Lf) > -1 OrElse _
                        field.StartsWith(" ") OrElse _
                        field.StartsWith(ControlChars.Tab) OrElse _
                        field.EndsWith(" ") OrElse _
                        field.EndsWith(ControlChars.Tab) Then
                        If field.IndexOf(ControlChars.Quote) > -1 Then
                            '"を""とする
                            field = field.Replace("""", """""")
                        End If
                        field = """" + field + """"
                    End If
                    'フィールドを書き込む
                    sr.Write(field)
                    'カンマを書き込む
                    If lastColIndex > i Then
                        sr.Write(","c)
                    End If
                Next i
                '改行する
                sr.Write(ControlChars.Cr + ControlChars.Lf)
            Next row
            '閉じる
            sr.Close()
            booRet = True
        Catch ioex As System.IO.IOException
            log.Fatal(ioex.Message)                                                         ' ログ出力（IOException）
            MessageBox.Show(ioex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            If Not sr Is Nothing Then
                sr.Close()
            End If
        End Try
        '結果返却
        Return booRet
    End Function
#End Region

#Region "CSV出力用データテーブル作成"
    '***************************************************************************************************
    '   ＩＤ　：funcCreateCSVDT
    '   名称　：CSV出力用データテーブル作成
    '   概要　：CSVファイルに出力するためのデータテーブルを作成する。
    '   戻り値：dtRet As DataTable = CSV出力用データテーブル
    '   作成日：2011/12/02(土)  y.nakano
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(土)  y.nakano  新規作成
    '           2012/02/01(水)  m.suzuki  メールアドレス1・2追加
    '***************************************************************************************************
    ''' <summary>CSV出力用データテーブル作成</summary>
    ''' <returns>CSV出力用データテーブル</returns>
    ''' <remarks></remarks>
    Function funcCreateCSVDT() As DataTable
        Dim dtRet As DataTable = Nothing
        Dim dtRow As DataRow = Nothing
        ' Update 2012/02/01(水) m.suzuki  メールアドレス1・2追加 Start
        Dim strColumns() As String = {"社員番号", "名前", "名前カナ", "組合員種別", "ステータス", _
                                      "組合支部", "資格", "機種", "会社所属", "職場", _
                                      "所属会社", "機長年月日", "加入年月日", "生年月日", "入社年月日", _
                                      "退職年月日", "勤務状態", "郵便番号", "住所", "電話番号１", _
                                      "電話番号２", "メールアドレス１", "メールアドレス２"}
        'Dim strColumns() As String = {"社員番号", "名前", "名前カナ", "組合員種別", "ステータス", "組合支部", _
        '                              "資格", "機種", "会社所属", "職場", "所属会社", "機長年月日", "加入年月日", _
        '                              "生年月日", "入社年月日", "退職年月日", "勤務状態", "郵便番号", "住所", "電話番号１", "電話番号２"}
        ' Update 2012/02/01(水) m.suzuki  メールアドレス1・2追加 End
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            dtRet = New DataTable
            'データテーブルに列を追加する。
            For Each Array As String In strColumns
                dtRet.Columns.Add(Array)
            Next
            '選択されているユーザーのみ情報出力する。
            For Each dgvRow As DataGridViewRow In dgdSearchResult.Rows
                If dgvRow.Cells.Item(0).Value = True Then
                    dtRow = dtRet.NewRow
                    dtRow.BeginEdit()
                    '各列に値を追加していく
                    For Each Array As String In strColumns
                        dtRow.Item(Array) = dgvRow.Cells.Item(Array).Value
                    Next
                    'データテーブルに行を追加
                    dtRet.Rows.Add(dtRow)
                End If
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return dtRet
    End Function
#End Region
#End Region

End Class

#End Region

