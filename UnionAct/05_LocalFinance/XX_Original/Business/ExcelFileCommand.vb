'===========================================================================================================
'   クラスＩＤ　　：ExcelFileCommand
'   クラス名称　　：Excel関連クラス
'   備考  　　　　：
'===========================================================================================================

Imports Microsoft.Office.Interop
Imports log4net
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.FinancialAffairs.WageReduction
Imports UnionAct.GUI.Document

Namespace Business.FinancialAffairs.WageReduction
    Public Class ExcelFileCommand
        Inherits WageReductionBase
        'Implements IExcelFileCommand

#Region " 列挙 "
        ''' <summary>Excel取込ファイル列挙（時間内・争議）</summary>
        ''' <remarks></remarks>
        Private Enum XLS_COL_IN_TIME_STRIKE
            WORK_LOCATION = 1                   ' 01. 勤務地
            STAF_NO = 2                         ' 02. 社員番号
            DIGIT = 3                           ' 03. CD
            ASTERISK = 4                        ' 04. *
            NAME = 5                            ' 05. 氏名
            NO_WORKING_DAYS = 6                 ' 06. 不就労・勤務日数
            NO_WORKING_HOURS = 7                ' 07. 不就労・時間数
            DEDUCTION = 8                       ' 08. 控除額
        End Enum

        ''' <summary>Excel取込ファイル列挙（一時金）</summary>
        ''' <remarks></remarks>
        Private Enum XLS_COL_BOUNUS
            WORK_LOCATION = 1                   ' 01. 勤務地
            STAF_NO = 2                         ' 02. 社員番号
            DIGIT = 3                           ' 03. CD
            NAME = 4                            ' 04. 氏名
            NO_WORKING_IN_TIME = 5              ' 05. 不就労・時間内
            NO_WORKING_STRIKE = 6               ' 06. 不就労・争議
            ABSENCE_DAYS = 7                    ' 07. 欠勤・無給日数
            CALC_NO_WORKING_DAYS = 8            ' 08. 計算元不就労日数
            CALC_ABSENCE_DAYS = 9               ' 09. 計算元欠勤日数
            DEDUCTION_IN_TIME = 10              ' 10. 控除額・不就労（時間内）
            DEDUCTION_STRIKE = 11               ' 11. 控除額・不就労（争議）
            DEDUCTION_TOTAL = 12                ' 12. 控除額・不就労合計
        End Enum
#End Region

#Region " 定数 "
        ' Excel読込開始行
        Private Const READ_FIRST_ROW_IN_TIME_STRIKE As Integer = 4  ' 月例：時間内・争議行為
        Private Const READ_FIRST_ROW_BOUNUS As Integer = 4          ' 一時金

        ' 件数判定列
        Private Const COUNT_JUDGE_COL As Integer = 1                ' 社員番号列

        '-------------------------------------------------------------------------------------------
        '   文字列
        '-------------------------------------------------------------------------------------------
        ' 月例：時間内・争議行為
        Private Const STR_IN_TIME_STRIKE_WORK_LOCATION As String = "勤務地"                 ' 01. 勤務地
        Private Const STR_IN_TIME_STRIKE_STAF_NO As String = "社員番号"                     ' 02. 社員番号
        Private Const STR_IN_TIME_STRIKE_DIGIT As String = "CD"                             ' 03. CD
        Private Const STR_IN_TIME_STRIKE_ASTERISK As String = ""                            ' 04. *
        Private Const STR_IN_TIME_STRIKE_NAME As String = "氏名"                            ' 05. 氏名
        Private Const STR_IN_TIME_STRIKE_NO_WORKING_DAYS As String = "不就労・勤務日数"     ' 06. 不就労・勤務日数
        Private Const STR_IN_TIME_STRIKE_NO_WORKING_HOURS As String = "不就労・時間数"      ' 07. 不就労・時間数
        Private Const STR_IN_TIME_STRIKE_DEDUCTION As String = "控除額"                     ' 08. 控除額

        ' 一時金
        Private Const STR_BOUNUS_WORK_LOCATION As String = "勤務地"                         ' 01. 勤務地
        Private Const STR_BOUNUS_STAF_NO = "社員番号"                                       ' 02. 社員番号
        Private Const STR_BOUNUS_DIGIT As String = "CD"                                     ' 03. CD
        Private Const STR_BOUNUS_NAME As String = "氏名"                                    ' 04. 氏名
        Private Const STR_BOUNUS_NO_WORKING_IN_TIME As String = "不就労・時間内"            ' 05. 不就労・時間内
        Private Const STR_BOUNUS_NO_WORKING_STRIKE As String = "不就労・争議"               ' 06. 不就労・争議
        Private Const STR_BOUNUS_ABSENCE_DAYS As String = "欠勤・無給日数"                  ' 07. 欠勤・無給日数
        Private Const STR_BOUNUS_CALC_NO_WORKING_DAYS As String = "計算元不就労日数"        ' 08. 計算元不就労日数
        Private Const STR_BOUNUS_CALC_ABSENCE_DAYS As String = "計算元欠勤日数"             ' 09. 計算元欠勤日数
        Private Const STR_BOUNUS_DEDUCTION_IN_TIME As String = "控除額・不就労（時間内）"   ' 10. 控除額・不就労（時間内）
        Private Const STR_BOUNUS_DEDUCTION_STRIKE As String = "控除額・不就労（争議）"      ' 11. 控除額・不就労（争議）
        Private Const STR_BOUNUS_DEDUCTION_TOTAL As String = "控除額・不就労合計"           ' 12. 控除額・不就労合計

#End Region

#Region " CheckReadExcelInTimeStrike：Excelファイルチェック・読込処理（時間内・争議講義） "
        ''' <summary>Excelファイルチェック・読込処理（時間内・争議講義）</summary>
        ''' <param name="i_file">Excelファイル名（フルパス）</param>
        ''' <param name="i_targetYM">対象年月</param>
        ''' <returns>Excel読込データ</returns>
        ''' <remarks></remarks>
        Public Function CheckReadExcelInTimeStrike( _
            ByVal i_file As String, _
            ByVal i_targetYM As String _
        ) As DataTable

            ' Excel関連オブジェクト
            Dim xlsApp As Excel.Application = Nothing           ' Excelアプリケーションオブジェクト
            Dim xlsBooks As Excel.Workbooks = Nothing           ' Excelブックスオブジェクト
            Dim xlsBook As Excel.Workbook = Nothing             ' Excelブックオブジェクト
            Dim xlsSheets As Excel.Sheets = Nothing             ' Excelシーツオブジェクト
            Dim xlsSheet As Excel.Worksheet = Nothing           ' Excelシートオブジェクト
            Dim xlsCells As Excel.Range = Nothing               ' Excelセルオブジェクト

            ' Excelデータ情報
            Dim strWorkLocation As String = ""                  ' 01. 勤務地
            Dim strStafNo As String = ""                        ' 02. 社員番号
            Dim strDigit As String = ""                         ' 03. CD
            Dim strAsterisk As String = ""                      ' 04. *
            Dim strName As String = ""                          ' 05. 氏名
            Dim strNoWorkingDays As String = ""                 ' 06. 不就労・勤務日数
            Dim strNoWorkingHours As String = ""                ' 06. 不就労・時間数
            Dim strDeduction As String = ""                     ' 07. 控除額（文字列型）
            Dim lngDeduction As Long = 0                        ' 07. 控除額（数値型）

            Dim intRecCnt As Integer = 0                        ' 件数
            Dim intLastRow As Integer = 0                       ' 読込最終行
            Dim intReadAllCnt As Integer = 0                    ' 読込件数
            Dim intReadCnt As Integer = 0                       ' 読込件数（控除額 0 以外）
            Dim lstStafNo As New List(Of String)                ' 社員番号リスト（重複チェック用）
            Dim strMsgArg As String = ""                        ' メッセージ引数

            Dim tbl As DataTable = Nothing                      ' Excelデータ読込用データテーブル
            Dim row As DataRow = Nothing                        ' Excelデータ読込用データロー

            Try
                ' Excelデータ読込用データテーブル定義
                tbl = New DataTable("xlsDataInTimeStrike")
                tbl.Columns.Add("work_location")                ' 01. 勤務地
                tbl.Columns.Add("staf_no")                      ' 02. 社員番号
                tbl.Columns.Add("digit")                        ' 03. CD
                tbl.Columns.Add("asterisk")                     ' 04. *
                tbl.Columns.Add("name")                         ' 05. 氏名
                tbl.Columns.Add("no_working_days")              ' 06. 不就労・勤務時間
                tbl.Columns.Add("no_working_hours")             ' 07. 不就労・時間数
                tbl.Columns.Add("deduction")                    ' 08. 控除額

                ' Excelアプリケーションオブジェクト生成
                xlsApp = New Excel.Application()
                xlsApp.Visible = False                          ' Excelアプリケーション非表示
                xlsApp.DisplayAlerts = False                    ' 警告メッセージ非表示

                ' Excelブックスオブジェクト生成
                xlsBooks = xlsApp.Workbooks
                Try
                    ' Excelブックオブジェクト生成
                    xlsBook = xlsBooks.Open(i_file, ReadOnly:=False, IgnoreReadOnlyRecommended:=True)
                    Try
                        ' Excelシーツオブジェクト生成
                        xlsSheets = xlsBook.Worksheets
                        Try
                            ' Excelシートオブジェクト生成
                            xlsSheet = xlsSheets.Item(1)
                            Try
                                ' Excelセルオブジェクト生成
                                xlsCells = xlsSheet.Cells
                                Try
                                    '---------------------------------------------------------------
                                    '   ヘッダーチェック処理（時間内・争議行為）
                                    '---------------------------------------------------------------
                                    If Me.CheckHeadInTimeStrike(xlsCells) = False Then
                                        FrmWaitInfo.CloseWaitForm()     ' しばらくお待ちください画面非表示
                                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0101", New String(0 - 1) {})
                                    End If

                                    '---------------------------------------------------------------
                                    '   最終行取得処理
                                    '---------------------------------------------------------------
                                    intRecCnt = GetLastRow(xlsCells, READ_FIRST_ROW_IN_TIME_STRIKE, COUNT_JUDGE_COL)

                                    ' 最終行チェック
                                    If intRecCnt = 0 Then
                                        FrmWaitInfo.CloseWaitForm()     ' しばらくお待ちください画面非表示
                                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0238", New String(0 - 1) {})
                                    End If

                                    ' 読込最終行取得（件数 + 読込開始行の4 - 1）
                                    intLastRow = intRecCnt + READ_FIRST_ROW_IN_TIME_STRIKE - 1

                                    '---------------------------------------------------------------
                                    '   バックカラー初期化処理
                                    '---------------------------------------------------------------
                                    SetIniCellBackColor( _
                                        xlsCells, _
                                        READ_FIRST_ROW_IN_TIME_STRIKE, _
                                        XLS_COL_IN_TIME_STRIKE.WORK_LOCATION, _
                                        intLastRow, _
                                        XLS_COL_IN_TIME_STRIKE.DEDUCTION _
                                    )

                                    ' 件数チェック
                                    If intLastRow > 0 Then

                                        ' 4行目から件数分ループ
                                        For i As Integer = READ_FIRST_ROW_IN_TIME_STRIKE To intLastRow Step 1

                                            ' 読込件数インクリメント
                                            intReadAllCnt += 1

                                            ' 各情報取得
                                            strWorkLocation = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.WORK_LOCATION)       ' 01. 勤務地
                                            strStafNo = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.STAF_NO)                   ' 02. 社員番号
                                            strDigit = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DIGIT)                      ' 03. CD
                                            strAsterisk = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.ASTERISK)                ' 04. *
                                            strName = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NAME)                        ' 05. 氏名
                                            strNoWorkingDays = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NO_WORKING_DAYS)    ' 06. 不就労・勤務日数
                                            strNoWorkingHours = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NO_WORKING_HOURS)  ' 07. 不就労・時間数
                                            strDeduction = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DEDUCTION)              ' 08. 控除額

                                            ' 控除額が 0 かチェック
                                            ' 控除額が 0 のレコードは取込無し。控除額が0以外のレコードが取込対象
                                            If strDeduction <> "0" Then

                                                ' 読込件数（控除額 0 以外）インクリメント
                                                intReadCnt += 1

                                                ' 社員番号リストに追加
                                                lstStafNo.Add(strStafNo)

                                                '===================================================
                                                '
                                                '   社員番号チェック
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   存在チェック
                                                '---------------------------
                                                ' 社員番号が存在するかチェック
                                                If MyBase.FindExistMember(strStafNo, i_targetYM) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_IN_TIME_STRIKE.STAF_NO)     ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の社員番号[" & strStafNo & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0090", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   重複チェック
                                                '---------------------------
                                                ' Excelファイル内に同一の社員番号が存在するかチェック
                                                If lstStafNo.FindAll(Function(s) s.Contains(strStafNo)).Count > 1 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_IN_TIME_STRIKE.STAF_NO)     ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の社員番号[" & strStafNo & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0181", New String() {strMsgArg})
                                                End If

                                                '===================================================
                                                '
                                                '   控除額
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   数値チェック
                                                '---------------------------
                                                ' 控除額が数値かチェック
                                                If NSMDChk.MDChk.ChkNumber(strDeduction) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DEDUCTION)   ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額[" & strDeduction & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0237", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   プラスチェック
                                                '---------------------------
                                                ' 控除金額がプラスかチェック
                                                lngDeduction = CLng(strDeduction)
                                                If lngDeduction > 0 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DEDUCTION)   ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額[" & strDeduction & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0236", New String() {strMsgArg})
                                                End If

                                                ' データロー作成
                                                row = tbl.NewRow
                                                row("work_location") = strWorkLocation          ' 01. 勤務地
                                                row("staf_no") = strStafNo                      ' 02. 社員番号
                                                row("digit") = strDigit                         ' 03. CD
                                                row("asterisk") = strAsterisk                   ' 04. *
                                                row("name") = strName                           ' 05. 氏名
                                                row("no_working_days") = strNoWorkingDays       ' 06. 不就労・勤務時間
                                                row("no_working_hours") = strNoWorkingHours     ' 07. 不就労・時間数
                                                row("deduction") = strDeduction                 ' 08. 控除額

                                                ' データ追加
                                                tbl.Rows.Add(row)

                                            End If
                                        Next i
                                    End If
                                Finally
                                    XlsObjectFreedom(xlsCells)  ' Excelセルオブジェクト開放
                                End Try
                            Finally
                                XlsObjectFreedom(xlsSheet)      ' Excelシートオブジェクト開放
                            End Try
                        Finally
                            XlsObjectFreedom(xlsSheets)         ' Excelシーツオブジェクト開放
                        End Try
                    Finally
                        If Not xlsBook Is Nothing Then
                            Try
                                xlsBook.Save()                  ' 上書き保存
                            Finally
                                XlsObjectFreedom(xlsBook)       ' Excelブックオブジェクト開放
                            End Try
                        End If
                    End Try
                Finally
                    XlsObjectFreedom(xlsBooks)                  ' Excelブックスオブジェクト開放
                End Try
            Finally
                If Not xlsApp Is Nothing Then
                    Try
                        xlsApp.Quit()                           ' Excelアプリケーション終了
                    Finally
                        XlsObjectFreedom(xlsApp)                ' Excelアプリケーションオブジェクト開放
                    End Try
                End If
            End Try

            ' 戻り値設定
            Return tbl

        End Function
#End Region

#Region " CheckReadExcelBounus：Excelファイルチェック・読込処理（一時金） "
        ''' <summary>Excelファイルチェック・読込処理（一時金）</summary>
        ''' <param name="i_file">ファイル名（フルパス）</param>
        ''' <param name="i_targetYM">対象年月</param>
        ''' <returns>True：正常終了, False：異常終了</returns>
        ''' <remarks></remarks>
        Public Function CheckReadExcelBounus(
            ByVal i_file As String,
            ByVal i_targetYM As String
        ) As DataTable

            ' Excel関連オブジェクト
            Dim xlsApp As Excel.Application = Nothing           ' Excelアプリケーションオブジェクト
            Dim xlsBooks As Excel.Workbooks = Nothing           ' Excelブックスオブジェクト
            Dim xlsBook As Excel.Workbook = Nothing             ' Excelブックオブジェクト
            Dim xlsSheets As Excel.Sheets = Nothing             ' Excelシーツオブジェクト
            Dim xlsSheet As Excel.Worksheet = Nothing           ' Excelシートオブジェクト
            Dim xlsCells As Excel.Range = Nothing               ' Excelセルオブジェクト

            ' Excelデータ情報
            Dim strWorkLocation As String = ""                  ' 01. 勤務地
            Dim strStafNo As String = ""                        ' 02. 社員番号
            Dim strDigit As String = ""                         ' 03. CD
            Dim strName As String = ""                          ' 04. 氏名
            Dim strNoWorkingInTime As String = ""               ' 05. 不就労・時間内
            Dim strNoWorkingStrike As String = ""               ' 06. 不就労・争議
            Dim strAbsenceDays As String = ""                   ' 07. 欠勤・無給日数
            Dim strCalcNoWorkingDays As String = ""             ' 08. 計算元不就労日数
            Dim strCalcAbsenceDays As String = ""               ' 09. 計算元欠勤日数
            Dim strDeductionInTime As String = ""               ' 10. 控除額・不就労（時間内）（文字列型）
            Dim lngDeductionInTime As Long = 0                  ' 10. 控除額・不就労（時間内）（数値型）
            Dim strDeductionStrike As String = ""               ' 11. 控除額・不就労（争議）（文字列型）
            Dim lngDeductionStrike As Long = 0                  ' 11. 控除額・不就労（争議）（数値型）
            Dim strDeductionTotal As String = ""                ' 12. 控除額・不就労合計（文字列型）
            Dim lngDeductionTotal As Long = 0                   ' 12. 控除額・不就労合計（数値型）

            Dim intRecCnt As Integer = 0                        ' 件数
            Dim intLastRow As Integer = 0                       ' 読込最終行
            Dim intReadAllCnt As Integer = 0                    ' 読込件数
            Dim intReadCnt As Integer = 0                       ' 読込件数（控除額・不就労（時間内）控除額・不就労（争議） 0 以外）
            Dim lstStafNo As New List(Of String)                ' 社員番号リスト（重複チェック用）
            Dim lngCalTotal As Long = 0                         ' 控除額・不就労（時間内） + 控除額・不就労（争議）
            Dim strMsgArg As String = ""                        ' メッセージ引数

            Dim tbl As DataTable = Nothing                      ' Excelデータ読込用データテーブル
            Dim row As DataRow = Nothing                        ' Excelデータ読込用データロー

            Try
                ' Excelデータ読込用データテーブル定義
                tbl = New DataTable("xlsDataBounus")
                tbl.Columns.Add("work_location")                ' 01. 勤務地
                tbl.Columns.Add("staf_no")                      ' 02. 社員番号
                tbl.Columns.Add("digit")                        ' 03. CD
                tbl.Columns.Add("name")                         ' 04. 氏名
                tbl.Columns.Add("no_working_in_time")           ' 05. 不就労・時間内
                tbl.Columns.Add("no_working_strike")            ' 06. 不就労・争議
                tbl.Columns.Add("absence_days")                 ' 07. 欠勤・無給日数
                tbl.Columns.Add("calc_no_working_days")         ' 08. 計算元不就労日数
                tbl.Columns.Add("calc_absence_days")            ' 09. 計算元欠勤日数
                tbl.Columns.Add("deduction_in_time")            ' 10. 控除額・不就労（時間内）
                tbl.Columns.Add("deduction_strike")             ' 11. 控除額・不就労（争議）
                tbl.Columns.Add("deduction_total")              ' 12. 控除額・不就労合計

                ' Excelアプリケーションオブジェクト生成
                xlsApp = New Excel.Application()
                xlsApp.Visible = False                          ' Excelアプリケーション非表示
                xlsApp.DisplayAlerts = False                    ' 警告メッセージ非表示

                ' Excelブックスオブジェクト生成
                xlsBooks = xlsApp.Workbooks

                Try
                    ' Excelブックオブジェクト生成
                    xlsBook = xlsBooks.Open(i_file)
                    Try
                        ' Excelシーツオブジェクト生成
                        xlsSheets = xlsBook.Worksheets
                        Try
                            ' Excelシートオブジェクト生成
                            xlsSheet = xlsSheets.Item(1)
                            Try
                                ' Excelセルオブジェクト生成
                                xlsCells = xlsSheet.Cells
                                Try
                                    '---------------------------------------------------------------
                                    '   ヘッダーチェック処理（一時金）
                                    '---------------------------------------------------------------
                                    If Me.CheckHeadBounus(xlsCells) = False Then
                                        FrmWaitInfo.CloseWaitForm()     ' しばらくお待ちください画面非表示
                                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0101", New String(0 - 1) {})
                                    End If

                                    '---------------------------------------------------------------
                                    '   最終行取得処理
                                    '---------------------------------------------------------------
                                    intRecCnt = GetLastRow(xlsCells, READ_FIRST_ROW_BOUNUS, COUNT_JUDGE_COL)

                                    ' 最終行チェック
                                    If intRecCnt = 0 Then
                                        FrmWaitInfo.CloseWaitForm()     ' しばらくお待ちください画面非表示
                                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0238", New String(0 - 1) {})
                                    End If

                                    ' 読込最終行取得（件数 + 読込開始行の4 - 1）
                                    intLastRow = intRecCnt + READ_FIRST_ROW_BOUNUS - 1

                                    '---------------------------------------------------------------
                                    '   バックカラー初期化処理
                                    '---------------------------------------------------------------
                                    SetIniCellBackColor(
                                        xlsCells,
                                        READ_FIRST_ROW_BOUNUS,
                                        XLS_COL_BOUNUS.WORK_LOCATION,
                                        intLastRow,
                                        XLS_COL_BOUNUS.DEDUCTION_TOTAL
                                    )

                                    ' 件数チェック
                                    If intLastRow > 0 Then

                                        ' 4行目から件数分ループ
                                        For i As Integer = READ_FIRST_ROW_BOUNUS To intLastRow Step 1

                                            ' 読込件数インクリメント
                                            intReadAllCnt += 1

                                            ' 各情報取得
                                            strWorkLocation = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.WORK_LOCATION)               ' 01. 勤務地
                                            strStafNo = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.STAF_NO)                           ' 02. 社員番号
                                            strDigit = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DIGIT)                              ' 03. CD
                                            strName = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NAME)                                ' 04. 氏名
                                            strNoWorkingInTime = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NO_WORKING_IN_TIME)       ' 05. 不就労・時間内
                                            strNoWorkingStrike = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NO_WORKING_STRIKE)        ' 06. 不就労・争議
                                            strAbsenceDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.ABSENCE_DAYS)                 ' 07. 欠勤・無給日数
                                            strCalcNoWorkingDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.CALC_NO_WORKING_DAYS)   ' 08. 計算元不就労日数
                                            strCalcAbsenceDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.CALC_ABSENCE_DAYS)        ' 09. 計算元欠勤日数
                                            strDeductionInTime = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_IN_TIME)        ' 10. 控除額・不就労（時間内）
                                            strDeductionStrike = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_STRIKE)         ' 11. 控除額・不就労（争議）
                                            strDeductionTotal = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_TOTAL)           ' 12. 控除額・不就労合計

                                            ' 控除額・不就労（時間内）と控除額・不就労（争議）が 0 かチェック
                                            ' 控除額が 0 のレコードは取込無し。控除額が 0 以外のレコードが取込対象
                                            If Not ((strDeductionInTime = "0") _
                                            AndAlso (strDeductionStrike = "0")) Then

                                                ' 読込件数（不就労（時間内）・控除額・不就労（争議） 0 以外）インクリメント
                                                intReadCnt += 1

                                                ' 社員番号リストに追加
                                                lstStafNo.Add(strStafNo)

                                                '===================================================
                                                '
                                                '   社員番号チェック
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   存在チェック
                                                '---------------------------
                                                ' 社員番号が存在するかチェック
                                                If MyBase.FindExistMember(strStafNo, i_targetYM) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.STAF_NO)             ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の社員番号[" & strStafNo & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0090", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   重複チェック
                                                '---------------------------
                                                ' Excelファイル内に同一の社員番号が存在するかチェック
                                                If lstStafNo.FindAll(Function(s) s.Contains(strStafNo)).Count > 1 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.STAF_NO)             ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の社員番号[" & strStafNo & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0181", New String() {strMsgArg})
                                                End If

                                                '===================================================
                                                '
                                                '   控除額・不就労（時間内）チェック
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   数値チェック処理
                                                '---------------------------
                                                ' 控除額・不就労（時間内）が数値かチェック
                                                If NSMDChk.MDChk.ChkNumber(strDeductionInTime) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_IN_TIME)   ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労（時間内）[" & strDeductionInTime & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0237", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   プラスチェック
                                                '---------------------------
                                                ' 控除額・不就労（時間内）がプラスかチェック
                                                lngDeductionInTime = CLng(strDeductionInTime)
                                                If lngDeductionInTime > 0 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_IN_TIME)   ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労（時間内）[" & strDeductionInTime & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0236", New String() {strMsgArg})
                                                End If

                                                '===================================================
                                                '
                                                '   控除額・不就労（争議）チェック
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   数値チェック
                                                '---------------------------
                                                ' 控除額・不就労（争議）が数値かチェック
                                                If NSMDChk.MDChk.ChkNumber(strDeductionStrike) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_STRIKE)    ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労（争議）[" & strDeductionStrike & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0237", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   プラスチェック
                                                '---------------------------
                                                ' 控除額・不就労（争議）がプラスかチェック
                                                lngDeductionStrike = CLng(strDeductionStrike)
                                                If lngDeductionStrike > 0 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_IN_TIME)   ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労（争議）[" & strDeductionStrike & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0236", New String() {strMsgArg})
                                                End If

                                                '===================================================
                                                '
                                                '   控除額・不就労合計チェック
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   数値チェック
                                                '---------------------------
                                                ' 控除額・不就労合計が数値かチェック
                                                If NSMDChk.MDChk.ChkNumber(strDeductionTotal) = False Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_TOTAL)     ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労合計[" & strDeductionTotal & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0237", New String() {strMsgArg})
                                                End If

                                                '---------------------------
                                                '   プラスチェック
                                                '---------------------------
                                                ' 控除額・不就労合計がプラスかチェック
                                                lngDeductionTotal = CLng(strDeductionTotal)
                                                If lngDeductionTotal > 0 Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_TOTAL)     ' エラー箇所バックカラー設定処理
                                                    strMsgArg = i.ToString & "行目の控除額・不就労合計[" & strDeductionTotal & "]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0236", New String() {strMsgArg})
                                                End If

                                                '===================================================
                                                '
                                                '   控除額・不就労（争議）
                                                '   控除額・不就労（時間内）
                                                '   控除額・不就労合計
                                                '
                                                '===================================================
                                                '---------------------------
                                                '   整合性チェック
                                                '---------------------------
                                                ' 控除額・不就労（時間内） + 控除額・不就労（争議） = 控除額・不就労合計かチェック
                                                lngCalTotal = lngDeductionInTime + lngDeductionStrike
                                                If lngCalTotal <> lngDeductionTotal Then
                                                    FrmWaitInfo.CloseWaitForm()                                             ' しばらくお待ちください画面非表示
                                                    Me.SetErrCellBackColor(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_TOTAL)     ' エラー箇所バックカラー設定処理
                                                    strMsgArg = "[控除額・不就労（時間内）] と [控除額・不就労（争議）]"
                                                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0021", New String() {strMsgArg})
                                                End If

                                                ' データロー作成
                                                row = tbl.NewRow
                                                row("work_location") = strWorkLocation              ' 01. 勤務地
                                                row("staf_no") = strStafNo                          ' 02. 社員番号
                                                row("digit") = strDigit                             ' 03. CD
                                                row("name") = strName                               ' 04. 氏名
                                                row("no_working_in_time") = strNoWorkingInTime      ' 05. 不就労・時間内
                                                row("no_working_strike") = strNoWorkingStrike       ' 06. 不就労・争議
                                                row("absence_days") = strAbsenceDays                ' 07. 欠勤・無給日数
                                                row("calc_no_working_days") = strCalcNoWorkingDays  ' 08. 計算元不就労日数
                                                row("calc_absence_days") = strCalcAbsenceDays       ' 09. 計算元欠勤日数
                                                row("deduction_in_time") = strDeductionInTime       ' 10. 控除額・不就労（時間内）
                                                row("deduction_strike") = strDeductionStrike        ' 11. 控除額・不就労（争議）
                                                row("deduction_total") = strDeductionTotal          ' 12. 控除額・不就労合計

                                                ' データ追加
                                                tbl.Rows.Add(row)
                                            End If
                                        Next i
                                    End If
                                Finally
                                    XlsObjectFreedom(xlsCells)  ' Excelセルオブジェクト開放
                                End Try
                            Finally
                                XlsObjectFreedom(xlsSheet)      ' Excelシートオブジェクト開放
                            End Try
                        Finally
                            XlsObjectFreedom(xlsSheets)         ' Excelシーツオブジェクト開放
                        End Try
                    Finally
                        If Not xlsBook Is Nothing Then
                            Try
                                xlsBook.Save()                  ' 上書き保存
                            Finally
                                XlsObjectFreedom(xlsBook)       ' Excelブックオブジェクト開放
                            End Try
                        End If
                    End Try
                Finally
                    XlsObjectFreedom(xlsBooks)                  ' Excelブックスオブジェクト開放
                End Try
            Finally
                If Not xlsApp Is Nothing Then
                    Try
                        xlsApp.Quit()                           ' Excelアプリケーション終了
                    Finally
                        XlsObjectFreedom(xlsApp)                ' Excelアプリケーションオブジェクト開放
                    End Try
                End If
            End Try

            ' 戻り値設定
            Return tbl

        End Function
#End Region

#Region " ReadExcelInTimeStrike：Excelファイル読込処理（時間内・争議行為） "
        ''' <summary>Excelファイル読込処理（時間内・争議行為）</summary>
        ''' <param name="file"></param>
        ''' <returns>Excelファイル読込情報</returns>
        ''' <remarks></remarks>
        Public Function ReadExcelInTimeStrike(ByVal file As String) As DataTable

            ' Excel関連オブジェクト
            Dim xlsApp As Excel.Application = Nothing       ' Excelアプリケーションオブジェクト
            Dim xlsBooks As Excel.Workbooks = Nothing       ' Excelブックスオブジェクト
            Dim xlsBook As Excel.Workbook = Nothing         ' Excelブックオブジェクト
            Dim xlsSheets As Excel.Sheets = Nothing         ' Excelシーツオブジェクト
            Dim xlsSheet As Excel.Worksheet = Nothing       ' Excelシートオブジェクト
            Dim xlsCells As Excel.Range = Nothing           ' Excelセルオブジェクト
            Dim xlsRange As Excel.Range = Nothing           ' Excelレンジオブジェクト

            ' Excelデータ
            Dim strWorkLocation As String = ""              ' 01. 勤務地
            Dim strStafNo As String = ""                    ' 02. 社員番号
            Dim strDigit As String = ""                     ' 03. CD
            Dim strAskrisk As String = ""                   ' 04. *
            Dim strName As String = ""                      ' 05. 氏名
            Dim strNoWorkingDays As String = ""             ' 06. 不就労・勤務日数
            Dim strNoWorkingHours As String = ""            ' 07. 不就労・時間数
            Dim strDeduction As String = ""                 ' 08. 控除額

            Dim intRecCnt As Integer = 0                    ' 件数
            Dim intLastRow As Integer = 0                   ' 読込最終行

            ' データテーブル定義
            Dim table As DataTable = New DataTable("")
            table.Columns.Add("work_location")              ' 01. 勤務地
            table.Columns.Add("employee_number")            ' 02. 社員番号
            table.Columns.Add("digit")                      ' 03. CD
            table.Columns.Add("asterisk")                   ' 04. *
            table.Columns.Add("name")                       ' 05. 名前
            table.Columns.Add("unempoyed_working_days")     ' 06. 不就労・勤務時間
            table.Columns.Add("unempoyed_working_hours")    ' 07. 不就労・時間数
            table.Columns.Add("deduction")                  ' 08. 控除額

            Try
                ' Excelアプリケーションオブジェクト生成
                xlsApp = New Excel.Application()
                xlsApp.Visible = False                      ' Excelアプリケーション非表示
                xlsApp.DisplayAlerts = False                ' 警告メッセージ非表示

                Try
                    ' Excelブックスオブジェクト生成
                    xlsBooks = xlsApp.Workbooks

                    Try
                        ' Excelブックオブジェクト生成
                        xlsBook = xlsBooks.Open(file)

                        Try
                            ' Excelソーツオブジェクト生成
                            xlsSheets = xlsBook.Worksheets

                            Try
                                ' Excelソートオブジェクト生成
                                xlsSheet = xlsSheets.Item(1)

                                Try
                                    ' Excelセルオブジェクト生成
                                    xlsCells = xlsSheet.Cells

                                    '---------------------------------------------------------------
                                    '   最終行取得処理
                                    '---------------------------------------------------------------
                                    intRecCnt = GetLastRow(xlsCells, READ_FIRST_ROW_IN_TIME_STRIKE, COUNT_JUDGE_COL)

                                    ' 読込最終行取得（件数 + 読込開始行の4 - 1）
                                    intLastRow = intRecCnt + READ_FIRST_ROW_IN_TIME_STRIKE - 1

                                    ' 件数チェック
                                    If intLastRow > 0 Then

                                        ' 4行目から件数分ループ
                                        For i As Integer = READ_FIRST_ROW_IN_TIME_STRIKE To intLastRow Step 1

                                            ' 各情報取得
                                            strWorkLocation = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.WORK_LOCATION)       ' 01. 勤務地
                                            strStafNo = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.STAF_NO)                   ' 02. 社員番号
                                            strDigit = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DIGIT)                      ' 03. CD
                                            strAskrisk = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.ASTERISK)                 ' 04. *
                                            strName = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NAME)                        ' 05. 氏名
                                            strNoWorkingDays = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NO_WORKING_DAYS)    ' 06. 不就労・勤務日数
                                            strNoWorkingHours = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.NO_WORKING_HOURS)  ' 07. 不就労・時間数
                                            strDeduction = GetCellValue(xlsCells, i, XLS_COL_IN_TIME_STRIKE.DEDUCTION)              ' 08. 控除額

                                            ' 控除額が 0 かチェック
                                            ' 控除額が 0 のレコードは取込無し。控除額が0以外のレコードが取込対象
                                            If strDeduction <> "0" Then

                                                ' レコード追加
                                                Dim row As DataRow = Nothing
                                                row = table.NewRow
                                                row("work_location") = strWorkLocation              ' 01. 勤務地
                                                row("employee_number") = strStafNo                  ' 02. 社員番号
                                                row("digit") = strDigit                             ' 03. CD
                                                row("asterisk") = strAskrisk                        ' 04. *
                                                row("name") = strName                               ' 05. 名前
                                                row("unempoyed_working_days") = strNoWorkingDays    ' 06. 不就労・勤務時間
                                                row("unempoyed_working_hours") = strNoWorkingHours  ' 07. 不就労・時間数
                                                row("deduction") = strDeduction                     ' 08. 控除額
                                                table.Rows.Add(row)
                                            End If
                                        Next i
                                    End If
                                Finally
                                    XlsObjectFreedom(xlsCells)  ' Excelセルオブジェクト開放
                                End Try
                            Finally
                                XlsObjectFreedom(xlsSheet)      ' Excelシートオブジェクト開放
                            End Try
                        Finally
                            XlsObjectFreedom(xlsSheets)         ' Excelオブジェクト開放
                        End Try
                    Finally
                        If Not xlsBook Is Nothing Then
                            Try
                                xlsBook.Close()                 ' Excelファイルを閉じる
                            Finally
                                XlsObjectFreedom(xlsBook)       ' Excelブックオブジェクト開放
                            End Try
                        End If
                    End Try
                Finally
                    XlsObjectFreedom(xlsBooks)                  ' Excelブックスオブジェクト開放
                End Try
            Finally
                If Not xlsApp Is Nothing Then
                    Try
                        xlsApp.Quit()                           ' Excelアプリケーション終了
                    Finally
                        XlsObjectFreedom(xlsApp)                ' Excelアプリケーションオブジェクト開放
                    End Try
                End If
            End Try

            ' 戻り値設定
            Return table

        End Function
#End Region

#Region " ReadExcelBounus：Excelファイル読込処理（一時金） "
        ''' <summary>Excelファイル読込処理（一時金）</summary>
        ''' <param name="file"></param>
        ''' <returns>Excelファイル読込情報</returns>
        ''' <remarks></remarks>
        Public Function ReadExcelBounus(ByVal file As String) As DataTable

            ' Excel関連オブジェクト
            Dim xlsApp As Excel.Application = Nothing           ' Excelアプリケーションオブジェクト
            Dim xlsBooks As Excel.Workbooks = Nothing           ' Excelブックスオブジェクト
            Dim xlsBook As Excel.Workbook = Nothing             ' Excelブックオブジェクト
            Dim xlsSheets As Excel.Sheets = Nothing             ' Excelシーツオブジェクト
            Dim xlsSheet As Excel.Worksheet = Nothing           ' Excelシートオブジェクト
            Dim xlsCells As Excel.Range = Nothing               ' Excelセルオブジェクト
            Dim xlsRange As Excel.Range = Nothing               ' Excelレンジオブジェクト
            Dim intRecCnt As Integer = 0                        ' 件数
            Dim intLastRow As Integer = 0                       ' 読み込み最終行

            ' Excelデータ
            Dim strWorkLocation As String = ""                  ' 01. 勤務地
            Dim strStafNo As String = ""                        ' 02. 社員番号
            Dim strDigit As String = ""                         ' 03. CD
            Dim strName As String = ""                          ' 04. 名前
            Dim strNoWorkingInTime As String = ""               ' 05. 不就労・時間内
            Dim strNoWorkingStrike As String = ""               ' 06. 不就労・争議
            Dim strAbsenceDays As String = ""                   ' 07. 欠勤・無給日数
            Dim strCalcNoWorkingDays As String = ""             ' 08. 計算元不就労日数
            Dim strCalcAbsenceDays As String = ""               ' 09. 計算元欠勤日数
            Dim strDeductionInTime As String = ""               ' 10. 控除額・不就労（時間内）
            Dim strDeductionStrike As String = ""               ' 11. 控除額・不就労（争議）
            Dim strDeductionTotal As String = ""                ' 12. 控除額・不就労合計

            ' データテーブル定義
            Dim table As DataTable = New DataTable("")
            table.Columns.Add("work_location")                  ' 01. 勤務地
            table.Columns.Add("staf_no")                        ' 02. 社員番号
            table.Columns.Add("digit")                          ' 03. CD
            table.Columns.Add("name")                           ' 04. 名前
            table.Columns.Add("no_working_in_time")             ' 05. 不就労・時間内
            table.Columns.Add("no_working_strike")              ' 06. 不就労・争議
            table.Columns.Add("absence_days")                   ' 07. 欠勤・無給日数
            table.Columns.Add("calc_no_working_days")           ' 08. 計算元不就労日数
            table.Columns.Add("calc_absence_Days")              ' 09. 計算元欠勤日数
            table.Columns.Add("deduction_in_time")              ' 10. 控除額・不就労（時間内）
            table.Columns.Add("deduction_strike")               ' 11. 控除額・不就労（争議）
            table.Columns.Add("deduction_total")                ' 12. 控除額・不就労合計

            Try
                ' Excelアプリケーションオブジェクト生成
                xlsApp = New Excel.Application()
                xlsApp.Visible = False                          ' Excelアプリケーション非表示
                xlsApp.DisplayAlerts = False                    ' 警告メッセージ非表示

                ' Excelブックスオブジェクト生成
                xlsBooks = xlsApp.Workbooks

                Try
                    ' Excelブックオブジェクト生成
                    xlsBook = xlsBooks.Open(file)
                    Try
                        ' Excelソーツオブジェクト生成
                        xlsSheets = xlsBook.Worksheets
                        Try
                            ' Excelソートオブジェクト生成
                            xlsSheet = xlsSheets.Item(1)
                            Try
                                ' Excelセルオブジェクト生成
                                xlsCells = xlsSheet.Cells
                                Try
                                    '---------------------------------------------------------------
                                    '   最終行取得処理
                                    '---------------------------------------------------------------
                                    intRecCnt = GetLastRow(xlsCells, READ_FIRST_ROW_BOUNUS, COUNT_JUDGE_COL)

                                    ' 読込最終行取得（件数 + 読込開始行の4 - 1）
                                    intLastRow = intRecCnt + READ_FIRST_ROW_BOUNUS - 1

                                    ' 件数チェック
                                    If intLastRow > 0 Then

                                        ' 4行目から件数分ループ
                                        For i As Integer = READ_FIRST_ROW_BOUNUS To intLastRow Step 1

                                            ' 各情報取得
                                            strWorkLocation = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.WORK_LOCATION)               ' 01. 勤務地
                                            strStafNo = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.STAF_NO)                           ' 02. 社員番号
                                            strDigit = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DIGIT)                              ' 03. CD
                                            strName = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NAME)                                ' 04. 名前
                                            strNoWorkingInTime = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NO_WORKING_IN_TIME)       ' 05. 不就労・時間内
                                            strNoWorkingStrike = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.NO_WORKING_STRIKE)        ' 06. 不就労・争議
                                            strAbsenceDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.ABSENCE_DAYS)                 ' 07. 欠勤・無給日数
                                            strCalcNoWorkingDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.CALC_NO_WORKING_DAYS)   ' 08. 計算元不就労日数
                                            strCalcAbsenceDays = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.CALC_ABSENCE_DAYS)        ' 09. 計算元欠勤日数
                                            strDeductionInTime = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_IN_TIME)        ' 10. 控除額・不就労（時間内）
                                            strDeductionStrike = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_STRIKE)         ' 11. 控除額・不就労（争議）
                                            strDeductionTotal = GetCellValue(xlsCells, i, XLS_COL_BOUNUS.DEDUCTION_TOTAL)           ' 12. 控除額・不就労合計

                                            ' 控除額・不就労（時間内）と控除額・不就労（争議）が 0 かチェック
                                            ' 控除額が 0 のレコードは取込無し。控除額が 0 以外のレコードが取込対象
                                            If (strDeductionInTime <> "0") _
                                            AndAlso (strDeductionStrike <> "0") Then

                                                ' レコード追加
                                                Dim row As DataRow
                                                row = table.NewRow
                                                row("work_location") = strWorkLocation              ' 01. 勤務地
                                                row("employee_number") = strStafNo                  ' 02. 社員番号
                                                row("digit") = strDigit                             ' 03. CD
                                                row("name") = strName                               ' 04. 名前
                                                row("no_working_in_time") = strNoWorkingInTime      ' 05. 不就労・時間内
                                                row("no_working_strike") = strNoWorkingStrike       ' 06. 不就労・争議
                                                row("absence_days") = strAbsenceDays                ' 07. 欠勤・無給日数
                                                row("calc_no_working_days") = strCalcNoWorkingDays  ' 08. 計算元不就労日数
                                                row("calc_absence_Days") = strCalcAbsenceDays       ' 09. 計算元欠勤日数
                                                row("deduction_in_time") = strDeductionInTime       ' 10. 控除額・不就労（時間内）
                                                row("deduction_strike") = strDeductionStrike        ' 11. 控除額・不就労（争議）
                                                row("deduction_total") = strDeductionTotal          ' 12. 控除額・不就労合計
                                                table.Rows.Add(row)
                                            End If
                                        Next i
                                    End If
                                Finally
                                    XlsObjectFreedom(xlsCells)  ' Excelセルオブジェクト開放
                                End Try
                            Finally
                                XlsObjectFreedom(xlsSheet)      ' Excelシートオブジェクト開放
                            End Try
                        Finally
                            XlsObjectFreedom(xlsSheets)         ' Excelオブジェクト開放
                        End Try
                    Finally
                        If Not xlsBook Is Nothing Then
                            Try
                                xlsBook.Close()                 ' Excelファイルを閉じる
                            Finally
                                XlsObjectFreedom(xlsBook)       ' Excelブックオブジェクト開放
                            End Try
                        End If
                    End Try
                Finally
                    XlsObjectFreedom(xlsBooks)                  ' Excelブックスオブジェクト開放
                End Try
            Finally
                If Not xlsApp Is Nothing Then
                    Try
                        xlsApp.Quit()                           ' Excelアプリケーション終了
                    Finally
                        XlsObjectFreedom(xlsApp)                ' Excelアプリケーションオブジェクト開放
                    End Try
                End If
            End Try

            ' 戻り値設定
            Return table

        End Function
#End Region

#Region " GetCell：セル値取得処理 "
        ''' <summary>セル値取得処理</summary>
        ''' <param name="sheet">Excelシートオブジェクト</param>
        ''' <param name="row">行</param>
        ''' <param name="col">列</param>
        ''' <returns>セル値</returns>
        ''' <remarks></remarks>
        Private Function GetCell( _
            ByVal sheet As Excel.Worksheet, _
            ByVal row As Integer, _
            ByVal col As Integer _
        ) As String

            ' Excelレンジオブジェクト
            Dim cells As Excel.Range = Nothing      ' Excelセルオブジェクト
            Dim range As Excel.Range = Nothing      ' Excelセルオブジェクト
            Dim value As String = ""

            Try
                ' Excelオブジェクト生成
                cells = sheet.Cells                 ' Excelセルオブジェクト生成
                range = cells(row, col)             ' Excelレンジオブジェクト生成
                value = range.Value                 ' セル値取得

            Finally
                XlsObjectFreedom(range)             ' Excelレンジオブジェクト生成
                XlsObjectFreedom(cells)             ' Excelセルオブジェクト開放

            End Try

            ' 戻り値にセル値設定
            Return value

        End Function
#End Region

#Region " GetLastRow：最終行取得処理 "
        ''' <summary>最終行取得処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="targetRow">開始対象行</param>
        ''' <param name="targetCol">開始対象列</param>
        ''' <returns>最終行</returns>
        ''' <remarks>開始対象行列から入力されている最終行を取得する</remarks>
        Private Function GetLastRow( _
            ByVal xlsCell As Excel.Range, _
            ByVal targetRow As Integer, _
            ByVal targetCol As Integer _
        ) As Integer

            ' 各オブジェクト定義
            Dim targetRange As Excel.Range = Nothing                ' Excel対象レンジオブジェクト
            Dim endRange As Excel.Range = Nothing                   ' Excel最終入力レンジオブジェクト
            Dim lastRow As Integer = 0                              ' 最終行数
            Dim nullRow As Integer = 0                              ' 余白行数

            ' 余白行数取得
            If targetRow <> 1 Then
                nullRow = targetRow - 1                             ' 開始対象行が 1 では無い場合、余白行を計算して取得
            End If

            Try
                ' Excel対象レンジオブジェクト生成
                targetRange = xlsCell(targetRow, targetCol)

                ' Excel対象レンジ値チェック
                If targetRange.Value Is Nothing Then
                    '-----------------------------------------------
                    '   値が無い場合
                    '-----------------------------------------------
                    lastRow = 0
                Else
                    '-----------------------------------------------
                    '   値が有る場合
                    '-----------------------------------------------
                    Try
                        ' Excel最終入力レンジオブジェクト生成
                        endRange = targetRange.End(Excel.XlDirection.xlDown)

                        ' 最終行取得（入力最終セル行数 - 余白行数）
                        lastRow = endRange.Row - nullRow
                    Finally
                        XlsObjectFreedom(endRange)                  ' Excel最終入力レンジオブジェクト
                    End Try
                End If
            Finally
                XlsObjectFreedom(targetRange)                       ' Excel対象レンジオブジェクト開放
            End Try

            ' 戻り値設定
            Return lastRow

        End Function
#End Region

#Region " GetLastCol：最終列取得処理 "
        ''' <summary>最終列取得処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="targetRow">開始対象行</param>
        ''' <param name="targetCol">開始対象列</param>
        ''' <returns>最終列</returns>
        ''' <remarks>開始対象行列から入力されている最終列を取得する</remarks>
        Private Function GetLastCol( _
            ByVal xlsCell As Excel.Range, _
            ByVal targetRow As Integer, _
            ByVal targetCol As Integer _
        ) As Integer

            ' 各オブジェクト定義
            Dim targetRange As Excel.Range                          ' Excelレンジオブジェクト（対象セル）
            Dim endRange As Excel.Range                             ' Excelレンジオブジェクト（最終入力セル）
            Dim lastCol As Integer = 0                              ' 最終列数
            Dim nullCol As Integer = 0                              ' 余白列数

            ' 余白列数取得
            If targetCol <> 1 Then
                nullCol = targetCol - 1                             ' 開始対象列が 1 では無い場合、余白列を計算して取得
            End If

            ' 各Excelオブジェクト生成
            targetRange = xlsCell(targetRow, targetCol)             ' 対象セル取得
            endRange = targetRange.End(Excel.XlDirection.xlToRight) ' 最終入力セル取得

            ' 最終列取得（入力最終セル列数 - 余白列数）
            lastCol = endRange.Column - nullCol

            ' 各オブジェクト開放（生成した逆順で開放）
            XlsObjectFreedom(endRange)                              ' Excelレンジオブジェクト（対象セル）
            XlsObjectFreedom(targetRange)                           ' Excelレンジオブジェクト（最終入力セル）

            ' 戻り値設定
            Return lastCol

        End Function
#End Region

#Region " CheckHeadInTimeStrike：ヘッダーチェック処理（時間内・争議行為） "
        ''' <summary>ヘッダーチェック処理（時間内・争議行為）</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <remarks>True：正常, False：異常</remarks>
        Private Function CheckHeadInTimeStrike(ByVal xlsCell As Excel.Range) As Boolean

            ' 処理結果（True：正常, False：異常）
            Dim result As Boolean = False

            Try
                '===================================================================================
                '
                '   ヘッダーチェック
                '
                '===================================================================================
                '-------------------------------------------
                '   勤務地
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.WORK_LOCATION, STR_IN_TIME_STRIKE_WORK_LOCATION) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   社員番号
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.STAF_NO, STR_IN_TIME_STRIKE_STAF_NO) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   CD
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.DIGIT, STR_IN_TIME_STRIKE_DIGIT) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   *
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.ASTERISK, STR_IN_TIME_STRIKE_ASTERISK) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   氏名
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.NAME, STR_IN_TIME_STRIKE_NAME) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   不就労・勤務日数
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.NO_WORKING_DAYS, STR_IN_TIME_STRIKE_NO_WORKING_DAYS) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   不就労・時間数
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.NO_WORKING_HOURS, STR_IN_TIME_STRIKE_NO_WORKING_HOURS) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_IN_TIME_STRIKE.DEDUCTION, STR_IN_TIME_STRIKE_DEDUCTION) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額の隣のセル
                '-------------------------------------------
                ' ヘッダー文字があった場合、エラー
                If CheckCellValue(xlsCell, 3, 9, "") = False Then
                    Exit Try
                End If

                ' 正常終了
                result = True

            Catch ex As Exception

            End Try

            ' 戻り値設定
            Return result

        End Function
#End Region

#Region " CheckHeadBounus：ヘッダーチェック処理（一時金） "
        ''' <summary>ヘッダーチェック処理（一時金）</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <remarks>True：正常, False：異常</remarks>
        Private Function CheckHeadBounus(ByVal xlsCell As Excel.Range) As Boolean

            ' 処理結果（True：正常, False：異常）
            Dim result As Boolean = False

            Try
                '===================================================================================
                '   ヘッダーチェック
                '===================================================================================
                '-------------------------------------------
                '   勤務地
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.WORK_LOCATION, STR_BOUNUS_WORK_LOCATION) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   社員番号
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.STAF_NO, STR_BOUNUS_STAF_NO) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   CD
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.DIGIT, STR_BOUNUS_DIGIT) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   氏名
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.NAME, STR_BOUNUS_NAME) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   不就労・時間内
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.NO_WORKING_IN_TIME, STR_BOUNUS_NO_WORKING_IN_TIME) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   不就労・争議
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.NO_WORKING_STRIKE, STR_BOUNUS_NO_WORKING_STRIKE) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   欠勤・無給日数
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.ABSENCE_DAYS, STR_BOUNUS_ABSENCE_DAYS) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   計算元不就労日数
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.CALC_NO_WORKING_DAYS, STR_BOUNUS_CALC_NO_WORKING_DAYS) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   計算元欠勤日数
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.CALC_ABSENCE_DAYS, STR_BOUNUS_CALC_ABSENCE_DAYS) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額・不就労（時間内）
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.DEDUCTION_IN_TIME, STR_BOUNUS_DEDUCTION_IN_TIME) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額・不就労（争議）
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.DEDUCTION_STRIKE, STR_BOUNUS_DEDUCTION_STRIKE) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額・不就労合計
                '-------------------------------------------
                ' セル値チェック処理
                If CheckCellValue(xlsCell, 3, XLS_COL_BOUNUS.DEDUCTION_TOTAL, STR_BOUNUS_DEDUCTION_TOTAL) = False Then
                    Exit Try
                End If

                '-------------------------------------------
                '   控除額・不就労合計の隣のセル
                '-------------------------------------------
                ' ヘッダー文字があった場合、エラー
                If CheckCellValue(xlsCell, 3, 13, "") = False Then
                    Exit Try
                End If

                ' 正常終了
                result = True

            Catch ex As Exception

            End Try

            ' 戻り値設定
            Return result

        End Function
#End Region

#Region " CheckCellValue：セル値チェック処理 "
        ''' <summary>セル値チェック処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="row">チェックする行</param>
        ''' <param name="col">チェックする列</param>
        ''' <param name="value">チェックする文字列</param>
        ''' <remarks>True：正常, False：異常</remarks>
        Private Function CheckCellValue( _
            ByVal xlsCell As Excel.Range, _
            ByVal row As Integer, _
            ByVal col As Integer, _
            ByVal value As String _
        ) As Boolean

            Dim result As Boolean = False               ' 処理結果（True：正常, False：異常）
            Dim checkRange As Excel.Range = Nothing     ' Excelレンジオブジェクト

            Try
                ' 対象のレンジオブジェクト生成
                checkRange = xlsCell(row, col)

                ' 対象値が同じかチェック
                If checkRange.Value = value Then
                    result = True
                Else
                    result = False
                End If

            Finally
                ' Excelレンジオブジェクト
                XlsObjectFreedom(checkRange)

            End Try

            ' 戻り値設定
            Return result

        End Function
#End Region

#Region " SetErrCellBackColor：エラー箇所バックカラー設定処理 "
        ''' <summary>エラー箇所バックカラー設定処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="row">チェックする行</param>
        ''' <param name="col">チェックする列</param>
        ''' <remarks>True：正常, False：異常</remarks>
        Private Sub SetErrCellBackColor( _
            ByRef xlsCell As Excel.Range, _
            ByVal row As Integer, _
            ByVal col As Integer _
        )

            Dim errRange As Excel.Range = Nothing               ' Excelレンジオブジェクト
            Dim errInterior As Excel.Interior = Nothing         ' Excelインテリアオブジェクト

            Try
                ' Excelレンジオブジェクト生成
                errRange = xlsCell(row, col)
                Try
                    ' Excelインテリアオブジェクト生成
                    errInterior = errRange.Interior

                    ' 背景色設定
                    errInterior.Color = &HC1B6FF                ' 薄いピンク色
                    'errInterior.Color = Color.LightPink         ' 薄いピンク色
                    'errInterior.Color = Color.Red               ' 赤色
                    'errInterior.ColorIndex = 3                  ' 赤色
                Finally
                    XlsObjectFreedom(errInterior)               ' Excelインテリアオブジェクト開放
                End Try
            Finally
                XlsObjectFreedom(errInterior)                   ' Excelインテリアオブジェクト開放
            End Try
        End Sub
#End Region

#Region " SetIniCellBackColor：バックカラー初期化処理 "
        ''' <summary>バックカラー初期化処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="beginRow">範囲指定開始セル行</param>
        ''' <param name="beginCol">範囲指定開始セル列</param>
        ''' <param name="endRow">範囲指定最終セル行</param>
        ''' <param name="endCol">範囲指定最終セル列</param>
        ''' <remarks>True：正常, False：異常</remarks>
        Private Sub SetIniCellBackColor( _
            ByRef xlsCell As Excel.Range, _
            ByVal beginRow As Integer, _
            ByVal beginCol As Integer, _
            ByVal endRow As Integer, _
            ByVal endCol As Integer _
        )

            Dim targetRange As Excel.Range = Nothing        ' Excel範囲指定レンジオブジェクト
            Dim targetInterior As Excel.Interior = Nothing  ' Excelインテリアオブジェクト

            Try
                ' Excel範囲指定レンジオブジェクト生成
                targetRange = xlsCell.Range(xlsCell.Cells(beginRow, beginCol), xlsCell.Cells(endRow, endCol))
                Try
                    ' Excelインテリアオブジェクト生成
                    targetInterior = targetRange.Interior

                    ' 背景色設定
                    targetInterior.ColorIndex = 0           ' 色設定なし
                Finally
                    XlsObjectFreedom(targetInterior)        ' Excelインテリアオブジェクト開放
                End Try
            Finally
                XlsObjectFreedom(targetRange)               ' Excel範囲指定レンジオブジェクト開放
            End Try

        End Sub
#End Region

#Region " ShowOpenExcelFileDialog：ファイルを開くダイアログボックス表示処理 "
        ''' <summary>ファイルを開くダイアログボックス表示処理</summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ShowOpenExcelFileDialog() As OpenFileDialog

            ' 処理結果
            Dim resultDialog As OpenFileDialog

            Try
                ' ダイアログボックスオブジェクト生成
                Dim dialog As New OpenFileDialog

                ' ダイアログボックスの設定
                dialog.AddExtension = True                                  ' 拡張子が入力されなかったとき、拡張子を自動的に付ける場合は「True」（既定値）、付けない場合は「False」
                dialog.CheckFileExists = True                               ' 存在しないファイルを指定されたとき、警告を表示する場合は「True」（既定値）、表示しない場合は「False」
                dialog.CheckPathExists = True                               ' 存在しないパスを指定されたとき、警告を表示する場合は「True」（既定値）、表示しない場合は「False」
                dialog.Filter = "Excelファイル (*.xls;*.xlsx)|*.xls;*.xlsx" ' 「ファイルの種類」のフィルタ。「フィルタ1の説明 | フィルタ1のパターン | フィルタ2の説明 | フィルタ2のパターン・・・」のように指定
                dialog.FilterIndex = 1                                      ' 「ファイルの種類」の最初に表示するフィルタ。既定値は「1」
                dialog.InitialDirectory = "C:\"                             ' 「ファイルの場所」に表示するパス
                dialog.Multiselect = False                                  ' 複数のファイルを選択可能にする場合は「True」、複数選択不可の場合は「False」（既定値）
                dialog.ReadOnlyChecked = False                              ' 「読み取り専用ファイルとして開く」にチェックを付ける場合は「True」、付けない場合は「False」（既定値）
                dialog.RestoreDirectory = False                             ' ダイアログ ボックスを閉じる前に、現在のディレクトリを復元するかどうかを示す値を取得または設定
                dialog.ShowHelp = False                                     ' 「ヘルプ」ボタンを表示する場合は「True」、表示しない場合は「False」（既定値）
                dialog.ShowReadOnly = False                                 ' 「読み取り専用ファイルとして開く」チェックボックスを表示する場合は「True」、表示しない場合は「False」（既定値）
                dialog.Title = "取り込むファイルを選択してください"         ' ダイアログボックスのタイトルバーに表示する文字

                ' ダイアログボックス表示
                If (dialog.ShowDialog <> DialogResult.OK) Then
                    ' キャンセルボタン押下の場合
                    Return Nothing                                          ' 処理を抜ける
                End If

                ' 処理結果設定
                resultDialog = dialog

            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})

            End Try

            ' 戻り値設定
            Return resultDialog

        End Function
#End Region

#Region " XlsObjectFreedom：Excelオブジェクト開放処理 "
        ''' <summary>Excelオブジェクト開放処理</summary>
        ''' <param name="objCom"></param>
        ''' <remarks>COMオブジェクト開放</remarks>
        Private Sub XlsObjectFreedom(ByRef objCom As Object)

            ' COM オブジェクトの使用後、明示的に COM オブジェクトへの参照を解放
            Try
                '提供されたランタイム呼び出し可能ラッパーの参照カウントをデクリメント
                If Not objCom Is Nothing _
                AndAlso System.Runtime.InteropServices.Marshal.IsComObject(objCom) Then
                    Dim i As Integer
                    Do
                        i = System.Runtime.InteropServices.Marshal.ReleaseComObject(objCom)
                    Loop Until i <= 0
                End If
            Catch

            Finally
                ' 参照を解除
                objCom = Nothing
            End Try

        End Sub
#End Region

#Region " GetCellValue：セル値取得処理 "
        ''' <summary>セル値取得処理</summary>
        ''' <param name="xlsCell">セルオブジェクト</param>
        ''' <param name="row">対象行</param>
        ''' <param name="col">対象列</param>
        ''' <returns>値</returns>
        ''' <remarks>指定行列の値を取得する</remarks>
        Private Function GetCellValue( _
            ByVal xlsCell As Excel.Range, _
            ByVal row As Integer, _
            ByVal col As Integer _
        ) As String

            ' Excelレンジオブジェクト
            Dim xlsRange As Excel.Range = Nothing
            Dim value As String = ""

            ' Excelレンジオブジェクト生成
            xlsRange = xlsCell(row, col)

            ' 値取得
            value = xlsRange.Value

            ' Excelオブジェクト開放処理
            XlsObjectFreedom(xlsRange)

            ' 戻り値設定
            Return value

        End Function
#End Region

    End Class
End Namespace
