'===========================================================================================================
'   ネームスペース：MDFinanceCommon
'   モジュールＩＤ：MDFinanceCommon
'   モジュール名称：日当計算用共通モジュール
'   備考  　　　　：日当計算関連逆コンパイルソースの修正数を削減する為、一部メソッドを抜粋
'===========================================================================================================
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon
Imports UnionAct.Framework.UnionException
Imports System.Reflection
Imports UnionAct.Framework.Command
Imports C1.Win.C1FlexGrid
Imports UnionAct.NSCLMsg
Imports System.Text.RegularExpressions

Module MDFinanceCommon

    ' NetBanckコマンド CheckDataHasMadeメソッド
    Public Function NetBank_CheckDataHasMade(ByVal strCloseDay As String, ByVal strDayPayKind As String) As Boolean
        Dim dataHasMade As Boolean
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Try
            Dim cmdText As String = String.Format("select d_pay_close from staf_bank_close where d_pay_close = '{0}' and k_daily_pay_kind = '{1}' ", _
                                                  strCloseDay, strDayPayKind)
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(cmdText)
            dataHasMade = table.Rows.Count > 0
        Catch exception As DataNotFoundException
            Throw exception
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
        Finally
            table.Dispose()
            clsMdb.Disconnect()
        End Try
        Return dataHasMade
    End Function

    ' FinancialAffairsコマンド GetTruncateAmountメソッド
    Public Function FinancialAffairs_GetTruncateAmount(ByVal CriterionDate As String) As Integer
        Return GetTruncPrice(CriterionDate, PublicCommand.GetKsh)
    End Function

    ' PriceBreakDaoクラス GetTruncPriceメソッド
    Public Function GetTruncPrice(ByVal ApplyDate As String, Optional ByVal CompanyCode As String = "") As Integer
        Dim clsMdb As New CLAccessMdb
        Dim table As New DataTable
        Dim num As Integer
        Try
            Dim cmdText As String
            If CompanyCode = "" Then
                cmdText = String.Format("SELECT A.s_break AS s_break FROM price_break A, (SELECT MAX(d_from) AS d_from FROM price_break WHERE d_from <= '{0}') B WHERE A.d_from = B.d_from", _
                                        ApplyDate)
            Else
                cmdText = String.Format("SELECT A.s_break AS s_break FROM price_break A, (SELECT MAX(d_from) AS d_from FROM price_break WHERE c_ksh = '{0}' AND d_from <= '{1}') B WHERE A.c_ksh = '{2}' AND A.d_from = B.d_from", _
                                        CompanyCode, ApplyDate, CompanyCode)
            End If
            clsMdb.Connect()
            table = clsMdb.ExecuteSql(cmdText)
            Dim obj2 As Object = table.Rows(0)("s_break")
            If (TypeOf obj2 Is DBNull OrElse (obj2 Is Nothing)) Then
                Throw New DataNotFoundException
            End If
            num = CInt(obj2)
        Catch exception As DataNotFoundException
            Throw exception
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
        Finally
            table.Dispose()
            clsMdb.Disconnect()
        End Try
        Return num
    End Function

    ' 参照権限取得
    Public Function GetReferencePower(ByVal strFormName As String) As Boolean
        Dim dt As DataTable = Nothing
        Dim strPrint As String = ""
        dt = MDCommon.getGrant(strFormName)
        strPrint = dt.Rows(0).Item(3).ToString
        Return strPrint = "1"
    End Function

    ' 入力権限取得
    Public Function GetEntryPower(ByVal strFormName As String) As Boolean
        Dim dt As DataTable = Nothing
        Dim strReg As String = ""
        dt = MDCommon.getGrant(strFormName)
        strReg = dt.Rows(0).Item(4).ToString
        Return strReg = "1"
    End Function

    ' 印刷権限取得
    Public Function GetPrintPower(ByVal strFormName As String) As Boolean
        Dim dt As DataTable = Nothing
        Dim strPrint As String = ""
        dt = MDCommon.getGrant(strFormName)
        strPrint = dt.Rows(0).Item(5).ToString
        Return strPrint = "1"
    End Function

    ' ファイル取込権限取得
    Public Function GetInputPower(ByVal strFormName As String) As Boolean
        Dim dt As DataTable = Nothing
        Dim strPrint As String = ""
        dt = MDCommon.getGrant(strFormName)
        strPrint = dt.Rows(0).Item(6).ToString
        Return strPrint = "1"
    End Function

    ' ファイル出力権限取得
    Public Function GetOutputPower(ByVal strFormName As String) As Boolean
        Dim dt As DataTable = Nothing
        Dim strPrint As String = ""
        dt = MDCommon.getGrant(strFormName)
        strPrint = dt.Rows(0).Item(6).ToString
        Return strPrint = "1"
    End Function

    ' 指定単位で切捨て
    Public Function Trunc(ByVal strColumn As String, ByVal iTruncPlace As Integer) As String
        Dim truncVal As Double = 10 ^ (iTruncPlace * -1)
        Return String.Format("IIF((({0}) % {1})=0,{0},(ROUND(({0}) / {1} + 0.5, 0) - 1) * {2})", strColumn, truncVal.ToString, truncVal.ToString)
        'TRUNC(I1.s_pay_cut + S1.s_pay_cut, " & TruncPlace & ")
    End Function

    ' PersonalFlexGridクラス SetColsWidthメソッド
    Public Sub SetColsWidth(ByRef flxList As C1FlexGrid, ByVal dicColWidthPair As Dictionary(Of String, Integer))
        Try
            Dim i As Integer
            For i = 0 To flxList.Cols.Count - 1
                flxList.Cols.Item(i).Visible = False
            Next i
            Dim str As String
            For Each str In dicColWidthPair.Keys
                If (dicColWidthPair.Item(str) = 0) Then
                    flxList.Cols.Item(str).Visible = False
                Else
                    flxList.Cols.Item(str).Visible = True
                    flxList.Cols.Item(str).Width = dicColWidthPair.Item(str)
                End If
            Next

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    ' PersonalFlexGridクラス AdjustTextAlignメソッド
    Public Sub AdjustTextAlign(ByRef flxList As C1FlexGrid)
        Try
            Dim i As Integer
            For i = 0 To flxList.Cols.Count - 1
                Select Case GetTextAlign(flxList, i)
                    Case "LEFT"
                        flxList.Cols.Item(i).TextAlign = TextAlignEnum.LeftCenter
                        Exit Select
                    Case "RIGHT"
                        flxList.Cols.Item(i).TextAlign = TextAlignEnum.RightCenter
                        Exit Select
                    Case "CENTER"
                        flxList.Cols.Item(i).TextAlign = TextAlignEnum.CenterCenter
                        Exit Select
                End Select
                flxList.Cols.Item(i).TextAlignFixed = TextAlignEnum.CenterCenter
            Next i
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Function GetTextAlign(ByRef flxList As C1FlexGrid, ByVal iColIndex As Integer) As String 'TODO ColAlign
        Dim textAlignByCaption As String 'TODO ColAlign
        Try
            If (((flxList.Cols.Item(iColIndex).DataType Is GetType(Integer)) OrElse (flxList.Cols.Item(iColIndex).DataType Is GetType(Double))) OrElse (flxList.Cols.Item(iColIndex).DataType Is GetType(Long))) Then
                Return "RIGHT"
            End If
            If Regex.IsMatch(flxList.Cols.Item(iColIndex).Caption, "日" & "(?!" & "当" & ")") Then
                Return "LEFT"
            End If
            textAlignByCaption = GetTextAlignByCaption(flxList, iColIndex)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return textAlignByCaption
    End Function

    Private Function GetTextAlignByCaption(ByRef flxList As C1FlexGrid, ByVal iColIndex As Integer) As String 'TODO ColAlign
        Dim lEFT As String 'TODO ColAlign
        Try
            Dim array As String() = New String() {"社員番号", "社番", "日当計", "日当額計", "支部別日当額計", "中執日当額計", "中執昼食費計", "当月日当計", "今回日当計", "前回差分計", "前回差分昼食費計", "振込金額", "支払元金", "振込手数料", "調整金額", "役員手当", "中央執行昼食費", "保険料率", "割合（本人）", "割合（組合）"}
            Dim strArray2 As String() = New String() {"支部", "組合支部", "会社所属", "所属", "所属会社", "ステータス", "種別", "機種", "資格", "24H", "時", "ｵﾘｼﾞﾅﾙ勤務", "Up/Uj", "時間帯" & "(From)", "時間帯" & "(To)", "期", "会社名", "振込状況", "表示名称", "給与出力区分", "一時金出力区分", "割合区分", "収入予想状況", "乗員計画状況", "分担金状況", "予算登録状況", "性別", "対象月", "年齢", "管理" & "cd"}
            Dim strArray3 As String() = New String() {"名前", "名前（カナ）", "氏名", "金額関連", "日付関連", "通知番号", "争議番号", "申請番号", "通告番号", "解除番号", "闘争指令", "解除指令", "争議行為通告番号", "パスワード", "組合員種別", "部／委員会", "職場", "地区", "勤務形態", "性別", "乗務資格", "住所", "電話番号", "備考", "場所", "分類", "事件", "会議名", "会議場", "役職", "時間関連", "場所", "開催目的", "議題", "担当者", "対象年月" & "(" & "始" & ")", "対象年月" & "(" & "終" & ")", "精算年月", "種類"}
            Dim caption As String = flxList.Cols.Item(iColIndex).Caption
            If (0 <= System.Array.IndexOf(Of String)(array, caption)) Then
                Return "RIGHT"
            End If
            If (0 <= System.Array.IndexOf(Of String)(strArray2, caption)) Then
                Return "CENTER"
            End If
            If (0 <= System.Array.IndexOf(Of String)(strArray3, caption)) Then
                Return "LEFT"
            End If
            lEFT = "LEFT"
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return lEFT
    End Function

    ' PersonalFlexGridクラス CheckMouseCursorPointメソッド
    Public Function CheckMouseCursorPoint(ByVal ctlFlexGrid As C1FlexGrid) As Boolean
        Dim flag As Boolean
        Try
            If ctlFlexGrid.HitTest.Type.Equals(HitTestTypeEnum.Cell) Then
                Return True
            End If
            flag = False
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    ' PersonalFlexGridクラス CheckRowIsSelectedメソッド
    Public Function CheckRowIsSelected(ByVal ctlFlexGrid As C1FlexGrid) As Boolean
        Dim flag As Boolean
        Try
            flag = (0 < ctlFlexGrid.Row)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    ' PersonalFlexGridクラス GetErrorStyleメソッド
    Public Function GetErrorStyle(ByVal ctlFlexGrid As C1FlexGrid, ByVal isRequire As Boolean) As CellStyle
        Dim style2 As CellStyle
        Try
            Dim name As String = If(isRequire, "RequireError", "AttributeError")
            If ctlFlexGrid.Styles.Contains(name) Then
                Return ctlFlexGrid.Styles.Item(name)
            End If
            Dim style As CellStyle = ctlFlexGrid.Styles.Add(name)
            style.BackColor = If(isRequire, Color.LightPink, Color.LightSalmon)
            style2 = style
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return style2
    End Function

    ' PersonalFlexGridクラス GetCantEditStyleメソッド
    Public Function GetCantEditStyle(ByVal ctlFlexGrid As C1FlexGrid) As CellStyle
        Dim style2 As CellStyle
        Try
            If ctlFlexGrid.Styles.Contains("CantEdit") Then
                Return ctlFlexGrid.Styles.Item("CantEdit")
            End If
            Dim style As CellStyle = ctlFlexGrid.Styles.Add("CantEdit")
            style.BackColor = Color.LightYellow
            style2 = style
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return style2
    End Function

    ' PersonalFlexGridクラス IsEmptyCellメソッド
    Public Function IsEmptyCell(ByVal ctlFlexGrid As C1FlexGrid, ByVal iRow As Integer, ByVal iCol As Integer) As Boolean
        Dim flag As Boolean
        Try
            flag = ((ctlFlexGrid.Item(iRow, iCol) Is Nothing) OrElse String.IsNullOrEmpty(ctlFlexGrid.Item(iRow, iCol).ToString))
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    ' PersonalFlexGridクラス IsEmptyCellメソッド
    Public Function IsEmptyCell(ByVal ctlFlexGrid As C1FlexGrid, ByVal iRow As Integer, ByVal strColumnCaption As String) As Boolean
        Dim flag As Boolean
        Try
            flag = IsEmptyCell(ctlFlexGrid, iRow, ctlFlexGrid.Cols.Item(strColumnCaption).Index)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    Public Function MakeEmptyListTable(ByVal strTable As String) As DataTable
        ' Create a new DataTable.
        Dim table As DataTable = New DataTable(strTable)

        ' Declare variables for DataColumn and DataRow objects.
        Dim column As DataColumn
        'Dim row As DataRow

        ' Create new DataColumn, set DataType, ColumnName 
        ' and add to DataTable.    
        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "d_daily_pay_close"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "締め日"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "k_belonging"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "支部"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "sum_daily_pay"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "今回日当計"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "sum_balance_daily_pay"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "前回差分計"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "belonging_daily_pay"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "支部別日当額計"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        ' Create second column.
        'column = New DataColumn()
        'column.DataType = System.Type.GetType("System.String")
        'column.ColumnName = "ParentItem"
        'column.AutoIncrement = False
        'column.Caption = "ParentItem"
        'column.ReadOnly = False
        'column.Unique = False
        'table.Columns.Add(column)

        ' Make the ID column the primary key column.
        Dim PrimaryKeyColumns(0) As DataColumn
        PrimaryKeyColumns(0) = table.Columns("d_daily_pay_close")
        table.PrimaryKey = PrimaryKeyColumns

        ' Instantiate the DataSet variable.
        'Dim ds As New DataSet

        ' Add the new DataTable to the DataSet.
        'ds.Tables.Add(table)

        ' Create three new DataRow objects and add 
        ' them to the DataTable
        'Dim i As Integer
        'For i = 0 To 2
        '    row = table.NewRow()
        '    row("id") = i
        '    row("ParentItem") = "ParentItem " + i.ToString()
        '    table.Rows.Add(row)
        'Next i
        Return table
    End Function

    Public Function MakeEmptyDtlTable(ByVal strTable As String) As DataTable
        ' Create a new DataTable.
        Dim table As DataTable = New DataTable(strTable)

        ' Declare variables for DataColumn and DataRow objects.
        Dim column As DataColumn
        'Dim row As DataRow

        ' Create new DataColumn, set DataType, ColumnName 
        ' and add to DataTable.    
        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "d_daily_pay_close"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "締め日"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "d_begin"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "対象年月(始)"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "d_end"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "対象年月(終)"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "s_daily_pay"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "日当額計"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "d_ins"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "登録年月日"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "c_user_id_ins"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "担当者"
        column.ReadOnly = True
        column.Unique = True
        table.Columns.Add(column)
        ' Create second column.
        'column = New DataColumn()
        'column.DataType = System.Type.GetType("System.String")
        'column.ColumnName = "ParentItem"
        'column.AutoIncrement = False
        'column.Caption = "ParentItem"
        'column.ReadOnly = False
        'column.Unique = False
        'table.Columns.Add(column)

        ' Make the ID column the primary key column.
        Dim PrimaryKeyColumns(0) As DataColumn
        PrimaryKeyColumns(0) = table.Columns("d_daily_pay_close")
        table.PrimaryKey = PrimaryKeyColumns

        ' Instantiate the DataSet variable.
        'Dim ds As New DataSet

        ' Add the new DataTable to the DataSet.
        'ds.Tables.Add(table)

        ' Create three new DataRow objects and add 
        ' them to the DataTable
        'Dim i As Integer
        'For i = 0 To 2
        '    row = table.NewRow()
        '    row("id") = i
        '    row("ParentItem") = "ParentItem " + i.ToString()
        '    table.Rows.Add(row)
        'Next i
        Return table
    End Function

    Public Function MakeEmptyDetailTable(ByVal strTable As String) As DataTable
        ' Create a new DataTable.
        Dim table As DataTable = New DataTable(strTable)

        ' Declare variables for DataColumn and DataRow objects.
        Dim column As DataColumn

        ' Create new DataColumn, set DataType, ColumnName 
        ' and add to DataTable.    
        column = New DataColumn()
        column.DataType = GetType(Boolean)
        column.ColumnName = "print_check"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "s_day"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(DateTime)
        column.ColumnName = "開催日"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "c_user_id"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "個人認証ＩＤ"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "c_staf_id"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "社員番号"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "l_name"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "氏名"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "k_qualification"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "資格"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "機種"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "k_belonging"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "支部"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(String)
        column.ColumnName = "sum_daily_pay"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "今回日当計"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "sum_balance_daily_pay"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "前回差分計"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)

        column = New DataColumn()
        column.DataType = GetType(Integer)
        column.ColumnName = "日当計"
        column.ReadOnly = False
        column.Unique = False
        table.Columns.Add(column)
        ' Create second column.
        'column = New DataColumn()
        'column.DataType = System.Type.GetType("System.String")
        'column.ColumnName = "ParentItem"
        'column.AutoIncrement = False
        'column.Caption = "ParentItem"
        'column.ReadOnly = False
        'column.Unique = False
        'table.Columns.Add(column)
        Dim row As DataRow = table.NewRow()
        row("print_check") = False
        row("開催日") = Date.Today
        row("sum_daily_pay") = 0
        row("今回日当計") = 3000
        row("sum_balance_daily_pay") = 0
        row("前回差分計") = 0
        row("日当計") = 0
        table.Rows.Add(row)

        ' Make the ID column the primary key column.
        'Dim PrimaryKeyColumns(0) As DataColumn
        'PrimaryKeyColumns(0) = table.Columns("d_daily_pay_close")
        'table.PrimaryKey = PrimaryKeyColumns

        ' Instantiate the DataSet variable.
        'Dim ds As New DataSet

        ' Add the new DataTable to the DataSet.
        'ds.Tables.Add(table)

        ' Create three new DataRow objects and add 
        ' them to the DataTable
        'Dim i As Integer
        'For i = 0 To 2
        '    row = table.NewRow()
        '    row("id") = i
        '    row("ParentItem") = "ParentItem " + i.ToString()
        '    table.Rows.Add(row)
        'Next i
        Return table
    End Function

    'Public Function CreateEmptyDataSet(ByVal strTable As String) As DataTable
    '    Dim table2 As DataTable
    '    Try
    '        Dim table As New DataTable(strTable)
    '        Dim strArray As String() = New String() {"varchar", "bpchar", "int4", "int8", "timestamp", "oid", "bit", "numeric", "date", "text", "bool", "unknown"}
    '        Dim typeArray As Type() = New Type() {GetType(String), GetType(String), GetType(Integer), GetType(Long), GetType(DateTime), GetType(Integer), GetType(Byte), GetType(Double), GetType(DateTime), GetType(String), GetType(Boolean), GetType(Object)}
    '        table.Clear()
    '        Dim i As Integer
    '        For i = 0 To dReader.FieldCount - 1
    '            Dim dataTypeName As String = dReader.GetDataTypeName(i)
    '            Dim index As Integer = 0
    '            Do While (index < strArray.Length)
    '                If dataTypeName.Equals(strArray(index)) Then
    '                    Exit Do
    '                End If
    '                index += 1
    '            Loop
    '            If (index = strArray.Length) Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"テーブル項目の型名"})
    '            End If
    '            table.Columns.Add(dReader.GetName(i), typeArray(index))
    '        Next i
    '        Dim values As Object() = New Object(dReader.FieldCount - 1) {}
    '        Do While dReader.Read
    '            Dim j As Integer
    '            For j = 0 To dReader.FieldCount - 1
    '                values(j) = dReader.Item(dReader.GetName(j))
    '            Next j
    '            table.Rows.Add(values)
    '        Loop
    '        dReader.Close()
    '        table2 = table
    '    Catch exception As AppUnionException
    '        exception.AddMethodName(MethodBase.GetCurrentMethod)
    '        Throw exception
    '    Catch exception2 As SysUnionException
    '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '        Throw exception2
    '    Catch exception3 As NpgsqlException
    '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '    Catch exception4 As Exception
    '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '    End Try
    '    Return table2
    'End Function

End Module
