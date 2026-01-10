Imports UnionAct.DAO
Imports UnionAct.DAO.FinancialAffairs.DailyAllowance
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports System
Imports System.Collections
Imports System.Data
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Mapping
Imports UnionAct.DAO.Activity
Imports UnionAct.DAO.Master
Imports UnionAct.DAO.Member

Namespace Business.FinancialAffairs.DailyAllowance
    Public Class DailyAllowanceCloseListCommand
        'Implements IDailyAllowanceCloseListCommand
        ' Methods
        Public Sub New()
        End Sub

        Public Sub CalcDailyAllowanceClose(ByVal strDailyPayKind As String, ByVal strDate As String, ByVal isReset As Boolean)
            Dim clsMdb As New CLAccessMdb
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                'clsMdb.BeginTran()
                If isReset Then
                    dao.DeleteDailyAllowanceClose(strDailyPayKind, strDate)
                End If
                dao.CalcDailyAllowanceClose(strDailyPayKind, strDate)
                'clsMdb.CommitTran()
            Catch exception As AppUnionException
                clsMdb.RollbackTran()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                clsMdb.RollbackTran()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                clsMdb.RollbackTran()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub CheckAttendanceEntry(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strPrevClose As String)
            Try
                Dim dao As New CallRollUserDao
                Dim time As DateTime = DateTime.ParseExact(strPrevClose, "yyyyMMdd", Nothing).AddDays(1)
                Dim time2 As DateTime = DateTime.ParseExact(strCloseDate, "yyyyMMdd", Nothing)
                Dim dTable As New DataTable("call_roll_user_dtl")
                dTable.Columns.Add("committee_name", GetType(String))
                dTable.Columns.Add("d_year", GetType(Integer))
                dTable.Columns.Add("d_month", GetType(Integer))
                Dim year As Integer = time.Year
                Dim month As Integer = time.Month
                Do While True
                    If ((year >= time2.Year) AndAlso (month > time2.Month)) Then
                        Exit Do
                    End If
                    If ((month Mod 13) = 0) Then
                        month = 1
                        year += 1
                    End If
                    Dim strDate As String = (year.ToString.PadLeft(4, "0"c) & month.ToString.PadLeft(2, "0"c) & "01")
                    Dim ds As DataSet = dao.IsExistForDailyPayKind(strDailyPayKind, strDate)
                    If (Not ds Is Nothing) Then
                        Dim table2 As DataTable = ds.Tables.Item("call_roll_user_dtl")
                        Dim j As Integer
                        For j = 0 To table2.Rows.Count - 1
                            Dim row As DataRow = dTable.NewRow
                            row.Item("committee_name") = table2.Rows.Item(j).Item("committee_name").ToString
                            row.Item("d_year") = year
                            row.Item("d_month") = month
                            dTable.Rows.Add(row)
                        Next j
                    End If
                    month += 1 'ds
                Loop
                dTable = PublicCommand.SortDataTable(dTable, "committee_name ASC, d_year ASC, d_month ASC")
                Dim newValue As String = ""
                Dim i As Integer
                For i = 0 To dTable.Rows.Count - 1
                    Dim str4 As String = newValue
                    newValue = String.Concat(New String() {str4, dTable.Rows.Item(i).Item("committee_name").ToString, " " & "：" & " ", dTable.Rows.Item(i).Item("d_year").ToString.PadLeft(4, "0"c), "年", dTable.Rows.Item(i).Item("d_month").ToString.PadLeft(2, "0"c), "月"})
                Next i
                If Not newValue.Equals("") Then
                    Dim ex As New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BW0001", New String(0 - 1) {})
                    Dim msg As New ExceptionMsg(ex)
                    Dim text As String = msg.GetOutputMsg.Replace("{0}", newValue)
                    If (MessageBox.Show(Nothing, [text], "問合せ", MessageBoxButtons.YesNo) <> DialogResult.Yes) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE9001", New String(0 - 1) {})
                    End If
                End If
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Function GetBelongingName(ByVal strBelongingId As String, ByVal strDate As String, ByVal strPrevDate As String) As String
            Dim clsMdb As New CLAccessMdb
            Dim str2 As String
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IConstantTblDao = DirectCast(class2.GetObject("DAO.Master.ConstantTblDao"), IConstantTblDao)
                Dim dao As New ConstantTblDao
                Dim rowArray As DataRow() = dao.GetConstantKind("BELONGING", strDate).Tables.Item("constant_dtl").Select(String.Concat(New String() {"c_constant_seq = '", strBelongingId, "' AND ((d_from <= '", strPrevDate, "' AND d_to >= '", strPrevDate, "') OR (d_from <= '", strDate, "' AND d_to >= '", strDate, "') OR (d_from >= '", strPrevDate, "' AND d_to <= '", strDate, "'))"}), "d_from DESC")
                Dim str As String = ""
                If (rowArray.Length > 0) Then
                    str = rowArray(0).Item("l_name").ToString
                End If
                str2 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetCommitteeBelongCloseList(ByVal strDailyPayKind As String, ByVal strCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                set2 = dao.GetCommitteeBelongCloseList(strDailyPayKind, strCloseDate, prevDailyPayClose)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeBelongCloseListPrint(ByVal dTableView As DataTable, ByVal selectedIdx As Integer, ByVal strDailyPayKind As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet("dsLocalTotal")
                Dim map As New BelongingCloseListPrintMap
                Dim map2 As New BelongingCloseListPrintHeaderMap
                Dim table As DataTable = map.CreateDataTablePhysName("dtDetail")
                Dim table2 As DataTable = map2.CreateDataTablePhysName("dtHeader")
                Dim str As String '= If(strDailyPayKind.Equals("01"), "committee_belong_close_list_print", "branch_belong_close_list_print")
                Select Case strDailyPayKind
                    Case "01"
                        str = "committee_belong_close_list_print"
                    Case "02"
                        str = "branch_belong_close_list_print"
                    Case Else
                        str = "dgm_belong_close_list_print"
                End Select
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                'Dim dao1 As IConstantTblDao = DirectCast(class2.GetObject("DAO.Master.ConstantTblDao"), IConstantTblDao)
                Dim dao1 As New ConstantTblDao
                Dim row As DataRow = table2.NewRow
                row.Item("l_local_name") = dTableView.Rows.Item(selectedIdx).Item("支部").ToString
                Dim strCloseDate As String = dTableView.Rows.Item(selectedIdx).Item("d_daily_pay_close").ToString
                Dim time As DateTime = DateTime.ParseExact(Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate), "yyyyMMdd", Nothing).AddMonths(1)
                Dim time2 As DateTime = DateTime.ParseExact(dTableView.Rows.Item(selectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
                row.Item("d_from_year") = time.Year.ToString.PadLeft(4, "0"c)
                row.Item("d_from_month") = time.Month.ToString.PadLeft(2, "0"c)
                row.Item("d_to_year") = time2.Year.ToString.PadLeft(4, "0"c)
                row.Item("d_to_month") = time2.Month.ToString.PadLeft(2, "0"c)
                table2.Rows.Add(row)
                Dim strBelongingId As String = dTableView.Rows.Item(selectedIdx).Item("k_belonging").ToString
                Dim table3 As DataTable = dao.GetCommitteeBelongCloseListPrint(strBelongingId, strDailyPayKind, strCloseDate).Tables.Item(str)
                Dim i As Integer
                For i = 0 To table3.Rows.Count - 1
                    row = table.NewRow
                    row.Item("l_name") = Me.GetCommitteeName(table3.Rows.Item(i).Item("c_committee_id").ToString, time2.ToString("yyyyMMdd"), time.ToString("yyyyMMdd"))
                    row.Item("s_daily_pay") = Integer.Parse(table3.Rows.Item(i).Item("s_daily_pay").ToString)
                    row.Item("s_food_expenses") = Integer.Parse(table3.Rows.Item(i).Item("s_food_expenses").ToString)
                    table.Rows.Add(row)
                Next i
                ds.Tables.Add(table2)
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeCloseDtl(ByVal strBelongingId As String, ByVal strDailyPayKind As String, ByVal strCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim str As String '= If(strDailyPayKind.Equals("01"), "committee_close_dtl", "branch_close_dtl")
                Select Case strDailyPayKind
                    Case "01"
                        str = "committee_close_dtl"
                    Case "02"
                        str = "branch_close_dtl"
                    Case Else
                        str = "dgm_close_dtl"
                End Select
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                'Dim dao1 As IStafAttributeTblDao = DirectCast(class2.GetObject("DAO.Member.StafAttributeTblDao"), IStafAttributeTblDao)
                Dim dao1 As New StafAttributeTblDao
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                Dim table As DataTable = dao.GetCommitteeCloseDtl(strDailyPayKind, strCloseDate, prevDailyPayClose).Tables.Item(str).Copy
                Dim rowArray As DataRow() = table.Select(("k_belonging = '" & strBelongingId & "' "), "c_staf_id ASC")
                'Dim rowArray As DataRow() = table.Select(("k_belonging LIKE '" & "%" & "' "), "c_staf_id ASC")
                Dim str3 As String = Me.GetBelongingName(strBelongingId, strCloseDate, prevDailyPayClose)
                Dim table2 As DataTable = table.Clone
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    table2.ImportRow(rowArray(i))
                    'table2.Rows.Item(i).Item("支部") = Me.GetBelongingName(table2.Rows.Item(i).Item("k_belonging"), strCloseDate, prevDailyPayClose)
                    table2.Rows.Item(i).Item("支部") = str3
                Next i
                If (table2.Rows.Count < 1) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BI0004", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeCloseDtlDgm(ByVal strCloseDate As String, ByVal fClosed As Boolean) As DataSet
            Dim set2 As DataSet
            Try
                Dim dao As New DailyPayCloseDao
                Dim dao1 As New StafAttributeTblDao
                Dim table As DataTable = dao.GetCommitteeCloseDtlDgm(strCloseDate, strCloseDate, fClosed).Tables.Item("dgm_close_dtl").Copy
                'Dim rowArray As DataRow() = table.Select(("k_belonging = '" & strBelongingId & "' "), "c_staf_id ASC")
                'Dim str3 As String = Me.GetBelongingName(strBelongingId, strCloseDate, prevDailyPayClose)
                Dim table2 As DataTable = table.Clone
                Dim i As Integer = 0
                'For i = 0 To rowArray.Length - 1
                '    table2.ImportRow(rowArray(i))
                '    table2.Rows.Item(i).Item("支部") = str3
                'Next i
                'If (table2.Rows.Count < 1) Then
                '    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BI0004", New String(0 - 1) {})
                'End If
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeCloseDtlPrint(ByVal dtView As DataTable, ByVal printData As SelectedAllowancePrintData, ByVal strBelongingName As String, ByVal strDailyPayKind As String, ByVal strCloseDate As String) As DataSet
            Dim set3 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim dao2 As New CallRollUserDao
                'Dim dao1 As ICommitteeListDtlDao = DirectCast(class2.GetObject("DAO.Activity.CommitteeListDtlDao"), ICommitteeListDtlDao)
                Dim dao1 As New CommitteeListDtlDao
                Dim str As String '= If(strDailyPayKind.Equals("01"), "committee_close_dtl_print", "branch_close_dtl_print")
                Select Case strDailyPayKind
                    Case "01"
                        str = "committee_close_dtl_print"
                    Case "02"
                        str = "branch_close_dtl_print"
                    Case Else
                        str = "dgm_close_dtl_print"
                End Select
                Dim ds As New DataSet("dsCommitteeDailyPay")
                Dim dtDtl As DataTable = New CommitteeCloseDtlPrintMap().CreateDataTablePhysName("dtDetail")
                Dim time As DateTime = DateTime.ParseExact(Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate), "yyyyMMdd", Nothing).AddDays(1)
                Dim time2 As DateTime = DateTime.ParseExact(strCloseDate, "yyyyMMdd", Nothing)
                Dim strDFrom As String = time.ToString("yyyyMMdd")
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                Dim table2 As DataTable = dao.GetCommitteeCloseDtlPrint(strDailyPayKind, strCloseDate, prevDailyPayClose).Tables.Item(str).Copy
                Dim i As Integer
                If strDailyPayKind <> "04" Then
                    For i = 0 To printData.noCheckedRec.Length - 1
                        Dim set2 As DataSet = dao2.GetInputDaysMonthsList(False, dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString, strDFrom, strCloseDate, strDailyPayKind)
                        Dim drSumData As DataRow() = table2.Select(("c_user_id = '" & dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString & "' "))
                        Dim dtCallRoll As DataTable = If((set2 Is Nothing), Nothing, set2.Tables.Item("call_roll_user_dtl").Copy)
                        If (Not dtCallRoll Is Nothing) Then
                            Dim span As TimeSpan = DirectCast((time2 - time), TimeSpan)
                            If (dtCallRoll.Rows.Count > (span.Days + 1)) Then
                                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0 - 1) {})
                            End If
                        End If
                        dtDtl = Me.GetCommitteeCloseDtlPrintDetail(dtView.Rows.Item(printData.noCheckedRec(i)), drSumData, dtCallRoll, strCloseDate, dtDtl)
                    Next i
                Else
                    Dim strKey As String
                    Dim htUserIdMap As Hashtable = New Hashtable
                    Dim arUserIdList As ArrayList = New ArrayList

                    For i = 0 To printData.noCheckedRec.Length - 1
                        strKey = dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString
                        If Not htUserIdMap.ContainsKey(strKey) Then
                            htUserIdMap.Add(strKey, strKey)
                            arUserIdList.Add(strKey)
                        End If
                    Next i
                    ' strCloseDateが期初以前の場合は、当月初に差替える
                    If strDFrom > strCloseDate Then
                        strDFrom = strCloseDate.Substring(0, 6) & "01"
                    End If
                    For Each key As String In arUserIdList
                        For i = 0 To printData.noCheckedRec.Length - 1
                            If key = dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString Then
                                Dim set2 As DataSet = dao2.GetInputDaysMonthsList(False, key, strDFrom, strCloseDate, strDailyPayKind)
                                Dim drSumData As DataRow() = table2.Select(("c_user_id = '" & key & "' "))
                                Dim dtCallRoll As DataTable = If((set2 Is Nothing), Nothing, set2.Tables.Item("call_roll_user_dtl").Copy)
                                'If (Not dtCallRoll Is Nothing) Then
                                '    Dim span As TimeSpan = DirectCast((time2 - time), TimeSpan)
                                '    If (dtCallRoll.Rows.Count > (span.Days + 1)) Then
                                '        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0 - 1) {})
                                '    End If
                                'End If
                                dtDtl = Me.GetCommitteeCloseDtlDgmPrintDetail(dtView.Rows.Item(printData.noCheckedRec(i)), drSumData, dtCallRoll, strCloseDate, dtDtl)
                                Exit For
                            End If
                        Next i
                    Next

                End If
                ds.Tables.Add(dtDtl)
                set3 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set3
        End Function

        Private Function GetCommitteeCloseDtlPrintDetail(ByVal drView As DataRow, ByVal drSumData As DataRow(), ByVal dtCallRoll As DataTable, ByVal strCloseDate As String, ByVal dtDtl As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As ICommitteeDao = DirectCast(class2.GetObject("DAO.Master.CommitteeDao"), ICommitteeDao)
                Dim dao As New CommitteeDao
                'Dim dao2 As IDailyPayMasterDtlDao = DirectCast(class2.GetObject("DAO.Master.DailyPayMasterDtlDao"), IDailyPayMasterDtlDao)
                Dim dao2 As New DailyPayMasterDtlDao
                'Dim dao1 As IExecutiveLunchPayMasterDao = DirectCast(class2.GetObject("DAO.Master.ExecutiveLunchPayMasterDao"), IExecutiveLunchPayMasterDao)
                Dim dao1 As New ExecutiveLunchPayMasterDao
                If (Not dtCallRoll Is Nothing) Then
                    Dim k As Integer
                    For k = 0 To dtCallRoll.Rows.Count - 1
                        'If dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString.Equals("DGM") Then
                        '    Dim strDate As String = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyyMMdd")
                        'If ((Not dtCallRoll.Rows.Item(k).Item("s_daily_pay") Is Nothing) AndAlso (Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString) <> 0)) Then
                        'Dim table As DataTable = dao2.GetDailyPayDtl(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, dtCallRoll.Rows.Item(k).Item("s_committee_seq").ToString, strDate).Tables.Item("daily_pay_master_dtl").Copy
                        '--------------------------------------------------------
                        ' DGM日当はGetCommitteeCloseDtlDgmPrintDetailで出力する
                        'Dim str2 As String = "DGM日当"
                        'Dim rowArray As DataRow() = Nothing
                        ''rowArray = table.Select(("s_daily_pay = " & dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString))
                        ''If (rowArray.Length > 0) Then
                        ''    str2 = rowArray(0).Item("l_explain").ToString
                        ''End If
                        'Dim row As DataRow = dtDtl.NewRow
                        'row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        'row.Item("c_staf_id") = drView.Item("社員番号").ToString
                        'row.Item("name") = drView.Item("氏名").ToString
                        'row.Item("l_name") = "DGM" 'Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                        'row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString)
                        ''row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString) + _
                        ''Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                        'row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                        'row.Item("l_explain") = str2
                        'row.Item("k_model") = drView.Item("機種")
                        'dtDtl.Rows.Add(row)
                        '--------------------------------------------------------

                        'If Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString) <> 0 Then
                        '    row = dtDtl.NewRow
                        '    row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        '    row.Item("c_staf_id") = drView.Item("社員番号").ToString
                        '    row.Item("name") = drView.Item("氏名").ToString
                        '    row.Item("l_name") = Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                        '    row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                        '    row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                        '    row.Item("l_explain") = "昼食費" 'str2
                        '    row.Item("k_model") = drView.Item("機種")
                        '    dtDtl.Rows.Add(row)
                        'End If
                        'End If
                        If Not dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString.Equals("001") And _
                        Not dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString.Equals("DGM") Then
                            Dim strDate As String = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyyMMdd")
                            Dim objDPay As Object = dtCallRoll.Rows.Item(k).Item("s_daily_pay")
                            Dim objFPay As Object = dtCallRoll.Rows.Item(k).Item("s_food_expenses")
                            Dim row As DataRow = Nothing

                            If ((Not objDPay Is Nothing) AndAlso (Not objDPay Is DBNull.Value) AndAlso _
                                (Integer.Parse(objDPay.ToString) <> 0)) Then
                                Dim table As DataTable = dao2.GetDailyPayDtl(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, dtCallRoll.Rows.Item(k).Item("s_committee_seq").ToString, strDate).Tables.Item("daily_pay_master_dtl").Copy
                                Dim str2 As String = ""
                                Dim rowArray As DataRow() = Nothing
                                rowArray = table.Select(("s_daily_pay = " & dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString))
                                If (rowArray.Length > 0) Then
                                    str2 = rowArray(0).Item("l_explain").ToString
                                End If
                                row = dtDtl.NewRow()
                                row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                                row.Item("c_staf_id") = drView.Item("社員番号").ToString
                                row.Item("name") = drView.Item("氏名").ToString
                                row.Item("l_name") = Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                                row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString)
                                'row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString) + _
                                'Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                                row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                                row.Item("l_explain") = str2
                                row.Item("k_model") = drView.Item("機種")
                                dtDtl.Rows.Add(row)
                            End If
                            If ((Not objFPay Is Nothing) AndAlso (Not objFPay Is DBNull.Value) AndAlso _
                                (Integer.Parse(objFPay.ToString) <> 0)) Then
                                row = dtDtl.NewRow
                                row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                                row.Item("c_staf_id") = drView.Item("社員番号").ToString
                                row.Item("name") = drView.Item("氏名").ToString
                                row.Item("l_name") = Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                                row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                                row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                                row.Item("l_explain") = "昼食費" 'str2
                                row.Item("k_model") = drView.Item("機種")
                                dtDtl.Rows.Add(row)
                            End If
                        End If
                    Next k
                End If
                Dim hashtable As New Hashtable
                Dim i As Integer
                For i = 0 To drSumData.Length - 1
                    If ((Not drSumData(i).Item("sum_balance_daily_pay") Is Nothing) AndAlso (Integer.Parse(drSumData(i).Item("sum_balance_daily_pay").ToString) <> 0)) Then
                        Dim num3 As Integer = If(drSumData(i).Item("sum_balance_daily_pay").ToString.Equals(""), 0, Integer.Parse(drSumData(i).Item("sum_balance_daily_pay").ToString))
                        Dim key As String = drSumData(i).Item("c_committee_id").ToString
                        If Not hashtable.ContainsKey(key) Then
                            hashtable.Item(key) = 0
                        End If
                        hashtable.Item(key) = (CInt(hashtable.Item(key)) + num3)
                    End If
                Next i
                Dim table2 As DataTable = dao.GetDataOfDate(PublicCommand.GetKsh, strCloseDate).Tables.Item("committee")
                Dim j As Integer
                For j = 0 To table2.Rows.Count - 1
                    Dim str4 As String = table2.Rows.Item(j).Item("c_committee_id").ToString
                    If hashtable.ContainsKey(str4) Then
                        Dim row2 As DataRow = dtDtl.NewRow
                        row2.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        row2.Item("c_staf_id") = drView.Item("社員番号").ToString
                        row2.Item("name") = drView.Item("氏名").ToString
                        row2.Item("l_name") = Me.GetCommitteeName(str4, strCloseDate, strCloseDate)
                        row2.Item("s_daily_pay") = hashtable.Item(str4)
                        row2.Item("s_day") = ""
                        row2.Item("l_explain") = "前回差分"
                        row2.Item("k_model") = drView.Item("機種")
                        dtDtl.Rows.Add(row2)

                        'row2 = dtDtl.NewRow
                        'row2.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        'row2.Item("c_staf_id") = drView.Item("社員番号").ToString
                        'row2.Item("name") = drView.Item("氏名").ToString
                        'row2.Item("l_name") = Me.GetCommitteeName(str4, strCloseDate, strCloseDate)
                        'row2.Item("s_daily_pay") = hashtable.Item(str4)
                        'row2.Item("s_day") = ""
                        'row2.Item("l_explain") = "昼食費合計"
                        'row2.Item("k_model") = drView.Item("機種")
                        'dtDtl.Rows.Add(row2)
                    End If
                Next j
                table3 = dtDtl
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Private Function GetCommitteeCloseDtlDgmPrintDetail(ByVal drView As DataRow, ByVal drSumData As DataRow(), ByVal dtCallRoll As DataTable, ByVal strCloseDate As String, ByVal dtDtl As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                Dim dao As New CommitteeDao
                Dim dao2 As New DailyPayMasterDtlDao
                Dim dao1 As New ExecutiveLunchPayMasterDao
                If (Not dtCallRoll Is Nothing) Then
                    Dim k As Integer
                    For k = 0 To dtCallRoll.Rows.Count - 1
                        If dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString.Equals("DGM") Then
                            Dim strDate As String = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyyMMdd")
                            If ((Not dtCallRoll.Rows.Item(k).Item("s_daily_pay") Is Nothing) AndAlso (Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString) <> 0)) Then
                                'Dim table As DataTable = dao2.GetDailyPayDtl(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, dtCallRoll.Rows.Item(k).Item("s_committee_seq").ToString, strDate).Tables.Item("daily_pay_master_dtl").Copy
                                Dim str2 As String = "DGM日当"
                                Dim rowArray As DataRow() = Nothing
                                'rowArray = table.Select(("s_daily_pay = " & dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString))
                                'If (rowArray.Length > 0) Then
                                '    str2 = rowArray(0).Item("l_explain").ToString
                                'End If
                                Dim row As DataRow = dtDtl.NewRow
                                row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                                row.Item("c_staf_id") = drView.Item("社員番号").ToString
                                row.Item("name") = drView.Item("氏名").ToString
                                row.Item("l_name") = "DGM" 'Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                                row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString)
                                'row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_daily_pay").ToString) + _
                                'Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                                row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                                row.Item("l_explain") = str2
                                row.Item("k_model") = drView.Item("機種")
                                dtDtl.Rows.Add(row)

                                'If Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString) <> 0 Then
                                '    row = dtDtl.NewRow
                                '    row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                                '    row.Item("c_staf_id") = drView.Item("社員番号").ToString
                                '    row.Item("name") = drView.Item("氏名").ToString
                                '    row.Item("l_name") = Me.GetCommitteeName(dtCallRoll.Rows.Item(k).Item("c_committee_id").ToString, strDate, strDate)
                                '    row.Item("s_daily_pay") = Integer.Parse(dtCallRoll.Rows.Item(k).Item("s_food_expenses").ToString)
                                '    row.Item("s_day") = CDate(dtCallRoll.Rows.Item(k).Item("s_day")).ToString("yyyy/MM/dd")
                                '    row.Item("l_explain") = "昼食費" 'str2
                                '    row.Item("k_model") = drView.Item("機種")
                                '    dtDtl.Rows.Add(row)
                                'End If
                            End If
                        End If
                    Next k
                End If
                Dim hashtable As New Hashtable
                Dim i As Integer
                For i = 0 To drSumData.Length - 1
                    If ((Not drSumData(i).Item("sum_balance_daily_pay") Is Nothing) AndAlso (Integer.Parse(drSumData(i).Item("sum_balance_daily_pay").ToString) <> 0)) Then
                        Dim num3 As Integer = If(drSumData(i).Item("sum_balance_daily_pay").ToString.Equals(""), 0, Integer.Parse(drSumData(i).Item("sum_balance_daily_pay").ToString))
                        Dim key As String = drSumData(i).Item("c_committee_id").ToString
                        If Not hashtable.ContainsKey(key) Then
                            hashtable.Item(key) = 0
                        End If
                        hashtable.Item(key) = (CInt(hashtable.Item(key)) + num3)
                    End If
                Next i
                'Dim table2 As DataTable = dao.GetDataOfDate(PublicCommand.GetKsh, strCloseDate).Tables.Item("committee")
                'Dim j As Integer
                'For j = 0 To table2.Rows.Count - 1
                Dim str4 As String = "DGM" 'table2.Rows.Item(j).Item("c_committee_id").ToString
                If hashtable.ContainsKey(str4) Then
                    Dim row2 As DataRow = dtDtl.NewRow
                    row2.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                    row2.Item("c_staf_id") = drView.Item("社員番号").ToString
                    row2.Item("name") = drView.Item("氏名").ToString
                    row2.Item("l_name") = Me.GetCommitteeName(str4, strCloseDate, strCloseDate)
                    row2.Item("s_daily_pay") = hashtable.Item(str4)
                    row2.Item("s_day") = ""
                    row2.Item("l_explain") = "前回差分"
                    row2.Item("k_model") = drView.Item("機種")
                    dtDtl.Rows.Add(row2)

                    'row2 = dtDtl.NewRow
                    'row2.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                    'row2.Item("c_staf_id") = drView.Item("社員番号").ToString
                    'row2.Item("name") = drView.Item("氏名").ToString
                    'row2.Item("l_name") = Me.GetCommitteeName(str4, strCloseDate, strCloseDate)
                    'row2.Item("s_daily_pay") = hashtable.Item(str4)
                    'row2.Item("s_day") = ""
                    'row2.Item("l_explain") = "昼食費合計"
                    'row2.Item("k_model") = drView.Item("機種")
                    '    'dtDtl.Rows.Add(row2)
                End If
                'Next j
                table3 = dtDtl
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Public Function GetCommitteeCloseList(ByVal strDailyPayKind As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                set2 = dao.GetCommitteeCloseList(strDailyPayKind, strDate, Date.Today.ToString("yyyyMMdd")) 'TODO
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeName(ByVal strCommitteeId As String, ByVal strDate As String, ByVal strPrevDate As String) As String
            Dim str2 As String
            Try
                If strCommitteeId = "DGM" Then Return "DGM"
                'Dim class2 As New FactoryDaoClass
                'Dim dao As ICommitteeDao = DirectCast(class2.GetObject("DAO.Master.CommitteeDao"), ICommitteeDao)
                Dim dao As New CommitteeDao
                Dim rowArray As DataRow() = dao.GetAllData.Tables.Item("committee").Copy.Select(String.Concat(New String() {"c_committee_id = '", strCommitteeId, "' AND ((d_from <= '", strPrevDate, "' AND d_to >= '", strPrevDate, "') OR (d_from <= '", strDate, "' AND d_to >= '", strDate, "') OR (d_from >= '", strPrevDate, "' AND d_to <= '", strDate, "'))"}), "d_from DESC")
                Dim str As String = ""
                If (rowArray.Length > 0) Then
                    str = rowArray(0).Item("l_name").ToString
                End If
                str2 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetCommitteePostName(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String, ByVal strPrevDate As String) As String
            Dim str2 As String
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As ICommitteeDtlDao = DirectCast(class2.GetObject("DAO.Master.CommitteeDtlDao"), ICommitteeDtlDao)
                Dim dao As New CommitteeDtlDao
                Dim rowArray As DataRow() = dao.GetCommitteePostName(strCommitteeId).Tables.Item("committee_dtl").Copy.Select(String.Concat(New String() {"s_committee_seq = '", strCommitteeSeq, "' AND ((d_from <= '", strPrevDate, "' AND d_to >= '", strPrevDate, "') OR (d_from <= '", strDate, "' AND d_to >= '", strDate, "') OR (d_from >= '", strPrevDate, "' AND d_to <= '", strDate, "'))"}), "d_from DESC")
                Dim str As String = ""
                If (rowArray.Length > 0) Then
                    str = rowArray(0).Item("l_name").ToString
                End If
                str2 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetDailyPayKindAll(ByVal strDate As String) As DataSet
            Dim dailyPayKindAll As DataSet
            Try
                'Dim class2 As New FactoryDaoClass
                Dim dao As New ConstantTblDao
                dailyPayKindAll = dao.GetDailyPayKindAll(strDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return dailyPayKindAll
        End Function

        Private Function GetExecCloseDtlDetailMonthly(ByVal strBelongName As String, ByVal dTime As DateTime, ByVal drView As DataRow, ByVal htMonthly As Hashtable, ByVal dtDtl As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                Dim row As DataRow
                'Dim class2 As New FactoryDaoClass
                'Dim dao1 As ICommitteeListDtlDao = DirectCast(class2.GetObject("DAO.Activity.CommitteeListDtlDao"), ICommitteeListDtlDao)
                Dim dao1 As New CommitteeListDtlDao
                'Dim dao As IDailyPayMasterDtlDao = DirectCast(class2.GetObject("DAO.Master.DailyPayMasterDtlDao"), IDailyPayMasterDtlDao)
                Dim dao As New DailyPayMasterDtlDao
                'Dim dao2 As IExecutiveLunchPayMasterDao = DirectCast(class2.GetObject("DAO.Master.ExecutiveLunchPayMasterDao"), IExecutiveLunchPayMasterDao)
                Dim dao2 As New ExecutiveLunchPayMasterDao
                Dim strDate As String = PublicCommand.GetMonthEnd(dTime.ToString("yyyyMMdd")).ToString("yyyyMMdd")
                Dim table As DataTable = dao.GetDailyPayMasterDtl(strDate).Tables.Item("daily_pay_master_dtl").Copy
                Dim i As Integer = 0
                'For i = 0 To table.Rows.Count - 1
                '    Dim key As String = (table.Rows.Item(i).Item("c_daily_pay_id").ToString & table.Rows.Item(i).Item("s_daily_pay").ToString)
                '    If htMonthly.ContainsKey(key) Then
                '        row = dtDtl.NewRow
                '        row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                '        row.Item("c_staf_id") = drView.Item("社員番号").ToString
                '        row.Item("l_name") = drView.Item("氏名").ToString
                '        row.Item("k_model") = drView.Item("機種")
                '        row.Item("k_belonging") = strBelongName
                '        row.Item("l_explain") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).strExplain
                '        row.Item("s_pay") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iUnitPrice
                '        Dim str3 As String = String.Copy(DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).strAttendDays)
                '        Dim ch As Char = str3.Chars((str3.Length - 1))
                '        row.Item("s_day") = If(ch.Equals(","c), str3.Remove((str3.Length - 1)), str3)
                '        row.Item("s_day_total") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iAttendDaysNum
                '        row.Item("s_pay_day") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iDailyPay
                '        dtDtl.Rows.Add(row)
                '    End If
                'Next i
                'Dim table2 As DataTable = dao2.GetExecutiveLunchPayAll(strDate).Tables.Item("executive_lunch_pay_master").Copy
                'Dim j As Integer
                'For j = 0 To table2.Rows.Count - 1
                '    Dim str4 As String = ("中執昼食費" & table2.Rows.Item(j).Item("c_executive_lunch_pay_id").ToString)
                '    If htMonthly.ContainsKey(str4) Then
                '        row = dtDtl.NewRow
                '        row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                '        row.Item("c_staf_id") = drView.Item("社員番号").ToString
                '        row.Item("l_name") = drView.Item("氏名").ToString
                '        row.Item("k_belonging") = strBelongName
                '        row.Item("l_explain") = DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).strExplain
                '        row.Item("s_pay") = DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).iUnitPrice
                '        Dim str5 As String = String.Copy(DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).strAttendDays)
                '        Dim ch2 As Char = str5.Chars((str5.Length - 1))
                '        row.Item("s_day") = If(ch2.Equals(","c), str5.Remove((str5.Length - 1)), str5)
                '        row.Item("s_day_total") = DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).iAttendDaysNum
                '        row.Item("s_pay_day") = DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).iDailyPay
                '        dtDtl.Rows.Add(row)
                '    End If
                'Next j
                For Each id As String In htMonthly.Keys
                    If (Not id.Contains("中執昼食費")) And (Not id.Contains("前回差分")) Then
                        row = dtDtl.NewRow
                        row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        row.Item("c_staf_id") = drView.Item("社員番号").ToString
                        row.Item("l_name") = drView.Item("氏名").ToString
                        row.Item("k_model") = drView.Item("機種")
                        row.Item("k_belonging") = strBelongName
                        row.Item("l_explain") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strExplain
                        row.Item("s_pay") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iUnitPrice
                        Dim str3 As String = String.Copy(DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strAttendDays)
                        Dim ch As Char = str3.Chars((str3.Length - 1))
                        row.Item("s_day") = If(ch.Equals(","c), str3.Remove((str3.Length - 1)), str3)
                        row.Item("s_day_total") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iAttendDaysNum
                        row.Item("s_pay_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iDailyPay
                        dtDtl.Rows.Add(row)
                    End If
                Next
                For Each id As String In htMonthly.Keys
                    If id.Contains("中執昼食費") Then
                        row = dtDtl.NewRow
                        row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                        row.Item("c_staf_id") = drView.Item("社員番号").ToString
                        row.Item("l_name") = drView.Item("氏名").ToString
                        row.Item("k_belonging") = strBelongName
                        row.Item("l_explain") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strExplain
                        row.Item("s_pay") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iUnitPrice
                        Dim str5 As String = String.Copy(DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strAttendDays)
                        Dim ch2 As Char = str5.Chars((str5.Length - 1))
                        row.Item("s_day") = If(ch2.Equals(","c), str5.Remove((str5.Length - 1)), str5)
                        row.Item("s_day_total") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iAttendDaysNum
                        row.Item("s_pay_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iDailyPay
                        dtDtl.Rows.Add(row)
                    End If
                Next
                If htMonthly.ContainsKey("前回差分") Then
                    row = dtDtl.NewRow
                    row.Item("c_user_id") = drView.Item("個人認証ＩＤ").ToString
                    row.Item("c_staf_id") = drView.Item("社員番号").ToString
                    row.Item("l_name") = drView.Item("氏名").ToString
                    row.Item("k_belonging") = strBelongName
                    row.Item("s_pay") = 0
                    row.Item("s_day") = ""
                    row.Item("s_day_total") = 0
                    row.Item("l_explain") = "前回差分"
                    row.Item("s_pay_day") = DirectCast(htMonthly.Item("前回差分"), MonthlyAttendDtlData).iDailyPay
                    dtDtl.Rows.Add(row)
                End If
                table3 = dtDtl
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Private Function GetExecCloseListDetailMonthly(ByVal dTime As DateTime, ByVal drAllBelong As DataRow, ByVal htMonthly As Hashtable, ByVal dtDtl As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                Dim row As DataRow
                'Dim class2 As New FactoryDaoClass
                'Dim dao As ICommitteeListDtlDao = DirectCast(class2.GetObject("DAO.Activity.CommitteeListDtlDao"), ICommitteeListDtlDao)
                Dim dao As New CommitteeListDtlDao
                'Dim dao2 As IDailyPayMasterDtlDao = DirectCast(class2.GetObject("DAO.Master.DailyPayMasterDtlDao"), IDailyPayMasterDtlDao)
                Dim dao2 As New DailyPayMasterDtlDao
                'Dim dao3 As IExecutiveLunchPayMasterDao = DirectCast(class2.GetObject("DAO.Master.ExecutiveLunchPayMasterDao"), IExecutiveLunchPayMasterDao)
                Dim dao3 As New ExecutiveLunchPayMasterDao
                Dim strDate As String = PublicCommand.GetMonthEnd(dTime.ToString("yyyyMMdd")).ToString("yyyyMMdd")
                Dim str2 As String = Me.GetCommitteePostName("001", dao.GetCommitteeSeq("001", drAllBelong.Item("c_user_id").ToString, strDate).ToString, strDate, strDate)
                Dim str3 As String = Me.GetModelName(drAllBelong.Item("k_model").ToString, strDate, strDate)
                Dim str4 As String = Me.GetBelongingName(drAllBelong.Item("k_belonging").ToString, strDate, strDate)
                Dim str5 As String = Me.GetQualificationName(drAllBelong.Item("k_qualification").ToString, strDate, strDate)
                Dim table As DataTable = dao2.GetDailyPayMasterDtl(strDate).Tables.Item("daily_pay_master_dtl").Copy
                'Dim i As Integer
                'For i = 0 To table.Rows.Count - 1
                '    Dim key As String = (table.Rows.Item(i).Item("c_daily_pay_id").ToString & table.Rows.Item(i).Item("s_daily_pay").ToString)
                'If htMonthly.ContainsKey(key) Then
                '    row = dtDtl.NewRow
                '    row.Item("c_user_id") = drAllBelong.Item("c_user_id").ToString
                '    row.Item("c_staf_id") = drAllBelong.Item("c_staf_id").ToString
                '    row.Item("l_name") = drAllBelong.Item("l_name").ToString
                '    row.Item("k_model") = str3
                '    row.Item("k_belonging") = str4
                '    row.Item("k_qualification") = str5
                '    row.Item("l_post_name") = str2
                '    row.Item("s_unit_price") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iUnitPrice
                '    Dim str7 As String = String.Copy(DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).strAttendDays)
                '    Dim ch As Char = str7.Chars((str7.Length - 1))
                '    row.Item("l_day") = If(ch.Equals(","c), str7.Remove((str7.Length - 1)), str7)
                '    row.Item("s_day") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iAttendDaysNum
                '    row.Item("l_explain") = (DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).strExplain & "対象日")
                '    row.Item("s_pay_day") = DirectCast(htMonthly.Item(key), MonthlyAttendDtlData).iDailyPay
                '    dtDtl.Rows.Add(row)
                'End If
                'Next i

                'Dim table2 As DataTable = dao3.GetExecutiveLunchPayAll(strDate).Tables.Item("executive_lunch_pay_master").Copy
                'Dim j As Integer
                'For j = 0 To table2.Rows.Count - 1
                '    Dim str8 As String = ("中執昼食費" & table2.Rows.Item(j).Item("c_executive_lunch_pay_id").ToString)
                '    If htMonthly.ContainsKey(str8) Then
                '        row = dtDtl.NewRow
                '        row.Item("c_user_id") = drAllBelong.Item("c_user_id").ToString
                '        row.Item("c_staf_id") = drAllBelong.Item("c_staf_id").ToString
                '        row.Item("l_name") = drAllBelong.Item("l_name").ToString
                '        row.Item("k_model") = str3
                '        row.Item("k_belonging") = str4
                '        row.Item("k_qualification") = str5
                '        row.Item("l_post_name") = str2
                '        row.Item("s_unit_price") = DirectCast(htMonthly.Item(str8), MonthlyAttendDtlData).iUnitPrice
                '        Dim str9 As String = String.Copy(DirectCast(htMonthly.Item(str8), MonthlyAttendDtlData).strAttendDays)
                '        Dim ch2 As Char = str9.Chars((str9.Length - 1))
                '        row.Item("l_day") = If(ch2.Equals(","c), str9.Remove((str9.Length - 1)), str9)
                '        row.Item("s_day") = DirectCast(htMonthly.Item(str8), MonthlyAttendDtlData).iAttendDaysNum
                '        row.Item("l_explain") = (DirectCast(htMonthly.Item(str8), MonthlyAttendDtlData).strExplain & "対象日")
                '        row.Item("s_pay_day") = DirectCast(htMonthly.Item(str8), MonthlyAttendDtlData).iDailyPay
                '        dtDtl.Rows.Add(row)
                '    End If
                'Next j
                For Each id As String In htMonthly.Keys
                    If (Not id.Contains("中執昼食費")) And (Not id.Contains("前回差分")) Then
                        row = dtDtl.NewRow
                        row.Item("c_user_id") = drAllBelong.Item("c_user_id").ToString
                        row.Item("c_staf_id") = drAllBelong.Item("c_staf_id").ToString
                        row.Item("l_name") = drAllBelong.Item("l_name").ToString
                        row.Item("k_model") = str3
                        row.Item("k_belonging") = str4
                        row.Item("k_qualification") = str5
                        row.Item("l_post_name") = str2
                        row.Item("s_unit_price") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iUnitPrice
                        Dim str7 As String = String.Copy(DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strAttendDays)
                        Dim ch As Char = str7.Chars((str7.Length - 1))
                        row.Item("l_day") = If(ch.Equals(","c), str7.Remove((str7.Length - 1)), str7)
                        row.Item("s_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iAttendDaysNum
                        row.Item("l_explain") = (DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strExplain & "対象日")
                        row.Item("s_pay_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iDailyPay
                        dtDtl.Rows.Add(row)
                    End If
                Next
                For Each id As String In htMonthly.Keys
                    If id.Contains("中執昼食費") Then
                        row = dtDtl.NewRow
                        row.Item("c_user_id") = drAllBelong.Item("c_user_id").ToString
                        row.Item("c_staf_id") = drAllBelong.Item("c_staf_id").ToString
                        row.Item("l_name") = drAllBelong.Item("l_name").ToString
                        row.Item("k_model") = str3
                        row.Item("k_belonging") = str4
                        row.Item("k_qualification") = str5
                        row.Item("l_post_name") = str2
                        row.Item("s_unit_price") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iUnitPrice
                        Dim str9 As String = String.Copy(DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strAttendDays)
                        Dim ch2 As Char = str9.Chars((str9.Length - 1))
                        row.Item("l_day") = If(ch2.Equals(","c), str9.Remove((str9.Length - 1)), str9)
                        row.Item("s_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iAttendDaysNum
                        row.Item("l_explain") = (DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).strExplain & "対象日")
                        row.Item("s_pay_day") = DirectCast(htMonthly.Item(id), MonthlyAttendDtlData).iDailyPay
                        dtDtl.Rows.Add(row)
                    End If
                Next

                If htMonthly.ContainsKey("前回差分") Then
                    row = dtDtl.NewRow
                    row.Item("c_user_id") = drAllBelong.Item("c_user_id").ToString
                    row.Item("c_staf_id") = drAllBelong.Item("c_staf_id").ToString
                    row.Item("l_name") = drAllBelong.Item("l_name").ToString
                    row.Item("k_model") = str3
                    row.Item("k_belonging") = str4
                    row.Item("k_qualification") = str5
                    row.Item("l_post_name") = str2
                    row.Item("s_unit_price") = 0
                    row.Item("l_day") = ""
                    row.Item("s_day") = 0
                    row.Item("l_explain") = "前回差分"
                    row.Item("s_pay_day") = DirectCast(htMonthly.Item("前回差分"), MonthlyAttendDtlData).iDailyPay
                    dtDtl.Rows.Add(row)
                End If
                table3 = dtDtl
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Public Function GetExecutiveCloseDtl(ByVal strBelongingId As String, ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                Dim table As DataTable = dao.GetExecutiveCloseDtl(strDailyPayKind, strCloseDate, prevDailyPayClose, strDate).Tables.Item("executive_close_dtl").Copy
                Dim rowArray As DataRow() = table.Select(("k_belonging = '" & strBelongingId & "' "), "c_staf_id ASC")
                Dim table2 As DataTable = table.Clone
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    Dim row As DataRow = table2.NewRow
                    row.Item("print_check") = False
                    row.Item("c_user_id") = rowArray(i).Item("c_user_id").ToString
                    row.Item("個人認証ＩＤ") = rowArray(i).Item("個人認証ＩＤ").ToString
                    'TODO row.Item("c_staf_id") = Integer.Parse(rowArray(i).Item("c_staf_id").ToString) 
                    row.Item("c_staf_id") = rowArray(i).Item("c_staf_id").ToString
                    row.Item("社員番号") = rowArray(i).Item("社員番号").ToString
                    row.Item("l_name") = rowArray(i).Item("l_name").ToString
                    row.Item("氏名") = rowArray(i).Item("氏名").ToString
                    row.Item("k_belonging") = rowArray(i).Item("k_belonging").ToString
                    row.Item("k_qualification") = rowArray(i).Item("k_qualification").ToString
                    row.Item("資格") = rowArray(i).Item("資格").ToString
                    row.Item("k_model") = rowArray(i).Item("k_model").ToString
                    row.Item("機種") = rowArray(i).Item("機種").ToString
                    row.Item("sum_daily_pay") = Integer.Parse(rowArray(i).Item("sum_daily_pay").ToString)
                    row.Item("当月日当計") = Integer.Parse(rowArray(i).Item("当月日当計").ToString)
                    row.Item("sum_food_expenses") = Integer.Parse(rowArray(i).Item("sum_food_expenses").ToString)
                    row.Item("中執昼食費計") = Integer.Parse(rowArray(i).Item("中執昼食費計").ToString)
                    row.Item("sum_balance_daily_pay") = Integer.Parse(rowArray(i).Item("sum_balance_daily_pay").ToString)
                    row.Item("前回差分計") = Integer.Parse(rowArray(i).Item("前回差分計").ToString)
                    row.Item("sum_balance_food_expenses") = Integer.Parse(rowArray(i).Item("sum_balance_food_expenses").ToString)
                    row.Item("前回差分昼食費計") = Integer.Parse(rowArray(i).Item("前回差分昼食費計").ToString)
                    row.Item("日当計") = Integer.Parse(rowArray(i).Item("日当計").ToString)
                    table2.Rows.Add(row)
                Next i
                If (table2.Rows.Count < 1) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BI0004", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetExecutiveCloseDtlPrint(ByVal dtView As DataTable, ByVal printData As SelectedAllowancePrintData, ByVal strBelongingId As String, ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strDate As String) As DataSet
            Dim set3 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim dao2 As New CallRollUserDao
                Dim ds As New DataSet("dsCentralExecutiveActivityDA")
                Dim map As New ExecutiveCloseDtlPrintMap
                Dim map2 As New ExecutiveCloseDtlPrintHeaderMap
                Dim dtDtl As DataTable = map.CreateDataTablePhysName("dtDetail")
                Dim table As DataTable = map2.CreateDataTablePhysName("dtHeader")
                Dim row As DataRow = table.NewRow
                Dim dTime As DateTime = DateTime.ParseExact((strDate.Substring(0, 4) & strDate.Substring(4, 2) & "01"), "yyyyMMdd", Nothing)
                row.Item("s_year") = dTime.Year
                row.Item("s_month") = dTime.Month
                table.Rows.Add(row)
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                'New Hashtable
                Dim table3 As DataTable = dao.GetExecutiveCloseDtl(strDailyPayKind, strCloseDate, prevDailyPayClose, strDate).Tables.Item("executive_close_dtl").Copy
                Dim strBelongName As String = Me.GetBelongingName(strBelongingId, strDate, strDate)
                Dim i As Integer
                For i = 0 To printData.noCheckedRec.Length - 1
                    Dim set2 As DataSet = dao2.GetInputDaysMonthList(True, dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString, "001", strDate)
                    Dim drSumData As DataRow = table3.Select(("c_user_id = '" & dtView.Rows.Item(printData.noCheckedRec(i)).Item("個人認証ＩＤ").ToString & "' "))(0)
                    Dim dtCallRoll As DataTable = If((set2 Is Nothing), Nothing, set2.Tables.Item("call_roll_user_dtl").Copy)
                    If ((Not dtCallRoll Is Nothing) AndAlso (dtCallRoll.Rows.Count > DateTime.DaysInMonth(dTime.Year, dTime.Month))) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0 - 1) {})
                    End If
                    Dim htMonthly As Hashtable = Me.GetExecutiveCloseHTMonthly(dTime, drSumData, dtCallRoll)
                    dtDtl = Me.GetExecCloseDtlDetailMonthly(strBelongName, dTime, dtView.Rows.Item(printData.noCheckedRec(i)), htMonthly, dtDtl)
                Next i
                ds.Tables.Add(table)
                ds.Tables.Add(dtDtl)
                set3 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set3
        End Function

        Private Function GetExecutiveCloseHTMonthly(ByVal dTime As DateTime, ByVal drSumData As DataRow, ByVal dtCallRoll As DataTable) As Hashtable
            Dim hashtable2 As Hashtable
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IDailyPayMasterDtlDao = DirectCast(class2.GetObject("DAO.Master.DailyPayMasterDtlDao"), IDailyPayMasterDtlDao)
                Dim dao As New DailyPayMasterDtlDao
                'Dim dao2 As IExecutiveLunchPayMasterDao = DirectCast(class2.GetObject("DAO.Master.ExecutiveLunchPayMasterDao"), IExecutiveLunchPayMasterDao)
                Dim dao2 As New ExecutiveLunchPayMasterDao
                Dim hashtable As New Hashtable
                If (Not dtCallRoll Is Nothing) Then
                    Dim i As Integer
                    For i = 0 To dtCallRoll.Rows.Count - 1
                        If dtCallRoll.Rows.Item(i).Item("c_committee_id").ToString.Equals("001") Then
                            If ((Not dtCallRoll.Rows.Item(i).Item("s_daily_pay") Is DBNull.Value) AndAlso (Not dtCallRoll.Rows.Item(i).Item("s_daily_pay") Is Nothing) AndAlso (Integer.Parse(dtCallRoll.Rows.Item(i).Item("s_daily_pay").ToString) <> 0)) Then
                                Dim time As DateTime = CDate(dtCallRoll.Rows.Item(i).Item("s_day"))
                                Dim table As DataTable = dao.GetDailyPayDtl("001", dtCallRoll.Rows.Item(i).Item("s_committee_seq").ToString, (dTime.Year.ToString.PadLeft(4, ChrW(&H30)) & dTime.Month.ToString.PadLeft(2, ChrW(&H30)) & time.Day.ToString.PadLeft(2, ChrW(&H30)))).Tables.Item("daily_pay_master_dtl").Copy
                                Dim s As String = dtCallRoll.Rows.Item(i).Item("s_daily_pay").ToString
                                Dim seq As String = dtCallRoll.Rows.Item(i).Item("c_menu_seq").ToString
                                Dim key As String = (dtCallRoll.Rows.Item(i).Item("c_daily_pay_id").ToString & s)
                                Dim str3 As String = ""
                                'Dim rowArray As DataRow() = table.Select(("s_daily_pay = " & s))
                                Dim rowArray As DataRow() = table.Select(("c_menu_seq = " & seq))
                                If (rowArray.Length > 0) Then
                                    str3 = rowArray(0).Item("l_explain").ToString
                                End If
                                key = seq & key
                                If Not hashtable.ContainsKey(key) Then
                                    hashtable.Item(key) = New MonthlyAttendDtlData
                                End If
                                DirectCast(hashtable.Item(key), MonthlyAttendDtlData).iUnitPrice = Integer.Parse(dtCallRoll.Rows.Item(i).Item("s_daily_pay").ToString)
                                Dim data1 As MonthlyAttendDtlData = DirectCast(hashtable.Item(key), MonthlyAttendDtlData)
                                Dim time2 As DateTime = CDate(dtCallRoll.Rows.Item(i).Item("s_day"))
                                data1.strAttendDays = (data1.strAttendDays & time2.Day.ToString & ",")
                                Dim data2 As MonthlyAttendDtlData = DirectCast(hashtable.Item(key), MonthlyAttendDtlData)
                                data2.iAttendDaysNum += 1
                                DirectCast(hashtable.Item(key), MonthlyAttendDtlData).strExplain = str3
                                Dim data3 As MonthlyAttendDtlData = DirectCast(hashtable.Item(key), MonthlyAttendDtlData)
                                data3.iDailyPay = (data3.iDailyPay + Integer.Parse(s))
                            End If
                            If ((Not dtCallRoll.Rows.Item(i).Item("s_food_expenses") Is Nothing) AndAlso (Integer.Parse(dtCallRoll.Rows.Item(i).Item("s_food_expenses").ToString) <> 0)) Then
                                Dim time3 As DateTime = CDate(dtCallRoll.Rows.Item(i).Item("s_day"))
                                Dim table2 As DataTable = dao2.GetExecutiveLunchPay("001", dtCallRoll.Rows.Item(i).Item("s_committee_seq").ToString, (dTime.Year.ToString.PadLeft(4, ChrW(&H30)) & dTime.Month.ToString.PadLeft(2, ChrW(&H30)) & time3.Day.ToString.PadLeft(2, ChrW(&H30)))).Tables.Item("executive_lunch_pay_master").Copy
                                Dim str4 As String = ("中執昼食費" & table2.Rows.Item(0).Item("c_executive_lunch_pay_id").ToString)
                                If Not hashtable.ContainsKey(str4) Then
                                    hashtable.Item(str4) = New MonthlyAttendDtlData
                                End If
                                DirectCast(hashtable.Item(str4), MonthlyAttendDtlData).iUnitPrice = Integer.Parse(table2.Rows.Item(0).Item("s_pay").ToString)
                                Dim data4 As MonthlyAttendDtlData = DirectCast(hashtable.Item(str4), MonthlyAttendDtlData)
                                Dim time4 As DateTime = CDate(dtCallRoll.Rows.Item(i).Item("s_day"))
                                data4.strAttendDays = (data4.strAttendDays & time4.Day.ToString & ",")
                                Dim data5 As MonthlyAttendDtlData = DirectCast(hashtable.Item(str4), MonthlyAttendDtlData)
                                data5.iAttendDaysNum += 1
                                DirectCast(hashtable.Item(str4), MonthlyAttendDtlData).strExplain = "中執昼食費"
                                Dim data6 As MonthlyAttendDtlData = DirectCast(hashtable.Item(str4), MonthlyAttendDtlData)
                                data6.iDailyPay = (data6.iDailyPay + Integer.Parse(table2.Rows.Item(0).Item("s_pay").ToString))
                            End If
                        End If
                    Next i
                End If
                If (((Not drSumData.Item("sum_balance_daily_pay") Is Nothing) AndAlso (Integer.Parse(drSumData.Item("sum_balance_daily_pay").ToString) <> 0)) OrElse ((Not drSumData.Item("sum_balance_food_expenses") Is Nothing) AndAlso (Integer.Parse(drSumData.Item("sum_balance_food_expenses").ToString) <> 0))) Then
                    Dim num2 As Integer = If(drSumData.Item("sum_balance_daily_pay").ToString.Equals(""), 0, Integer.Parse(drSumData.Item("sum_balance_daily_pay").ToString))
                    Dim num3 As Integer = If(drSumData.Item("sum_balance_food_expenses").ToString.Equals(""), 0, Integer.Parse(drSumData.Item("sum_balance_food_expenses").ToString))
                    If Not hashtable.ContainsKey("前回差分") Then
                        hashtable.Item("前回差分") = New MonthlyAttendDtlData
                    End If
                    DirectCast(hashtable.Item("前回差分"), MonthlyAttendDtlData).strExplain = "前回差分"
                    Dim data7 As MonthlyAttendDtlData = DirectCast(hashtable.Item("前回差分"), MonthlyAttendDtlData)
                    data7.iDailyPay = (data7.iDailyPay + num2)
                    Dim data8 As MonthlyAttendDtlData = DirectCast(hashtable.Item("前回差分"), MonthlyAttendDtlData)
                    data8.iDailyPay = (data8.iDailyPay + num3)
                End If
                hashtable2 = hashtable
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return hashtable2
        End Function

        Public Function GetExecutiveCloseList(ByVal strDailyPayKind As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                set2 = dao.GetExecutiveCloseList(strDailyPayKind, strDate, Date.Today.ToString("yyyyMMdd")) 'TODO
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetExecutiveCloseListPrint(ByVal dtView As DataTable, ByVal selectedIdx As Integer, ByVal strDailyPayKind As String, Optional ByVal strSortOrder As String = "") As DataSet
            Dim set3 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim dao2 As New CallRollUserDao
                Dim ds As New DataSet("dsActivityDailyReport")
                Dim map As New ExecutiveCloseListPrintHeaderMap
                Dim map2 As New ExecutiveCloseListPrintMap
                Dim map3 As New ExecutiveCloseListPrintFooterMap
                Dim table As DataTable = map.CreateDataTablePhysName("dtHeader")
                Dim dtDtl As DataTable = map2.CreateDataTablePhysName("dtDetail")
                Dim executiveCloseListPrintFooter As DataTable = map3.CreateDataTablePhysName("dtFooter")
                Dim row As DataRow = table.NewRow
                Dim dTime As DateTime = CDate(dtView.Rows.Item(selectedIdx).Item("d_years"))
                row.Item("d_year") = dTime.Year
                row.Item("d_month") = dTime.Month
                table.Rows.Add(row)
                Dim strCloseDate As String = CDate(dtView.Rows.Item(selectedIdx).Item("締め日")).ToString("yyyyMMdd")
                Dim strDate As String = dTime.ToString("yyyyMMdd")
                Dim prevDailyPayClose As String = Me.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                Dim table4 As DataTable = dao.GetExecutiveCloseDtl(strDailyPayKind, strCloseDate, prevDailyPayClose, strDate).Tables.Item("executive_close_dtl").Copy
                Dim htSum As New Hashtable
                Dim i As Integer
                For i = 0 To table4.Rows.Count - 1
                    Dim set2 As DataSet = dao2.GetInputDaysMonthList(True, table4.Rows.Item(i).Item("c_user_id").ToString, "001", strDate)
                    Dim dtCallRoll As DataTable = If((set2 Is Nothing), Nothing, set2.Tables.Item("call_roll_user_dtl").Copy)
                    If ((Not dtCallRoll Is Nothing) AndAlso (dtCallRoll.Rows.Count > DateTime.DaysInMonth(dTime.Year, dTime.Month))) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0001", New String(0 - 1) {})
                    End If
                    Dim htMonthly As Hashtable = Me.GetExecutiveCloseHTMonthly(dTime, table4.Rows.Item(i), dtCallRoll)
                    dtDtl = Me.GetExecCloseListDetailMonthly(dTime, table4.Rows.Item(i), htMonthly, dtDtl)
                    Dim str4 As String
                    For Each str4 In htMonthly.Keys
                        If Not htSum.ContainsKey(str4) Then
                            htSum.Item(str4) = New MonthlyAttendFtrData
                        End If
                        DirectCast(htSum.Item(str4), MonthlyAttendFtrData).strExplain = DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).strExplain
                        Dim data1 As MonthlyAttendFtrData = DirectCast(htSum.Item(str4), MonthlyAttendFtrData)
                        data1.iTotal = (data1.iTotal + DirectCast(htMonthly.Item(str4), MonthlyAttendDtlData).iDailyPay)
                    Next
                Next i
                executiveCloseListPrintFooter = Me.GetExecutiveCloseListPrintFooter(htSum, strDate)
                ds.Tables.Add(table)
                ds.Tables.Add(dtDtl)
                ds.Tables.Add(executiveCloseListPrintFooter)
                set3 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set3
        End Function

        Private Function GetExecutiveCloseListPrintFooter(ByVal htSum As Hashtable, ByVal strDate As String) As DataTable
            Dim table4 As DataTable
            Try
                Dim row As DataRow
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IDailyPayMasterDtlDao = DirectCast(class2.GetObject("DAO.Master.DailyPayMasterDtlDao"), IDailyPayMasterDtlDao)
                Dim dao As New DailyPayMasterDtlDao
                'Dim dao2 As IExecutiveLunchPayMasterDao = DirectCast(class2.GetObject("DAO.Master.ExecutiveLunchPayMasterDao"), IExecutiveLunchPayMasterDao)
                Dim dao2 As New ExecutiveLunchPayMasterDao
                Dim table As DataTable = New ExecutiveCloseListPrintFooterMap().CreateDataTablePhysName("dtFooter")
                Dim table2 As DataTable = dao.GetDailyPayMasterDtl(strDate).Tables.Item("daily_pay_master_dtl").Copy
                Dim table3 As DataTable = dao2.GetExecutiveLunchPayAll(strDate).Tables.Item("executive_lunch_pay_master").Copy
                'Dim i As Integer
                'For i = 0 To table2.Rows.Count - 1
                '    Dim key As String = (table2.Rows.Item(i).Item("c_daily_pay_id").ToString & table2.Rows.Item(i).Item("s_daily_pay").ToString)
                '    If htSum.ContainsKey(key) Then
                '        row = table.NewRow
                '        row.Item("l_explain") = table2.Rows.Item(i).Item("l_explain").ToString
                '        row.Item("s_total") = DirectCast(htSum.Item(key), MonthlyAttendFtrData).iTotal
                '        table.Rows.Add(row)
                '    End If
                'Next i
                'Dim j As Integer
                'For j = 0 To table3.Rows.Count - 1
                '    Dim str2 As String = ("中執昼食費" & table3.Rows.Item(j).Item("c_executive_lunch_pay_id").ToString)
                '    If htSum.ContainsKey(str2) Then
                '        row = table.NewRow
                '        row.Item("l_explain") = "中執昼食費"
                '        row.Item("s_total") = DirectCast(htSum.Item(str2), MonthlyAttendFtrData).iTotal
                '        table.Rows.Add(row)
                '    End If
                'Next j

                For Each id As String In htSum.Keys
                    If Not id.Contains("中執昼食費") Then
                        For i = 0 To table2.Rows.Count - 1
                            If id = (table2.Rows.Item(i).Item("c_menu_seq").ToString & table2.Rows.Item(i).Item("c_daily_pay_id").ToString & table2.Rows.Item(i).Item("s_daily_pay").ToString) Then
                                row = table.NewRow
                                row.Item("l_explain") = table2.Rows.Item(i).Item("l_explain").ToString
                                If Len(row.Item("l_explain").ToString) > 0 Then
                                    row.Item("s_total") = DirectCast(htSum.Item(id), MonthlyAttendFtrData).iTotal
                                    table.Rows.Add(row)
                                End If
                                Exit For
                            End If
                        Next i
                    End If
                Next
                For Each id As String In htSum.Keys
                    If id.Contains("中執昼食費") Then
                        row = table.NewRow
                        row.Item("l_explain") = "中執昼食費"
                        row.Item("s_total") = DirectCast(htSum.Item(id), MonthlyAttendFtrData).iTotal
                        table.Rows.Add(row)
                    End If
                Next

                If htSum.ContainsKey("前回差分") Then
                    row = table.NewRow
                    row.Item("l_explain") = "前回差分"
                    row.Item("s_total") = DirectCast(htSum.Item("前回差分"), MonthlyAttendFtrData).iTotal
                    table.Rows.Add(row)
                End If
                table4 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table4
        End Function

        Public Function GetModelName(ByVal strModelId As String, ByVal strDate As String, ByVal strPrevDate As String) As String
            Dim str2 As String
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IConstantTblDao = DirectCast(class2.GetObject("DAO.Master.ConstantTblDao"), IConstantTblDao)
                Dim dao As New ConstantTblDao
                Dim rowArray As DataRow() = dao.GetConstantKind("MODEL", strDate).Tables.Item("constant_dtl").Select(String.Concat(New String() {"c_constant_seq = '", strModelId, "' AND ((d_from <= '", strPrevDate, "' AND d_to >= '", strPrevDate, "') OR (d_from <= '", strDate, "' AND d_to >= '", strDate, "') OR (d_from >= '", strPrevDate, "' AND d_to <= '", strDate, "'))"}), "d_from DESC")
                Dim str As String = ""
                If (rowArray.Length > 0) Then
                    str = rowArray(0).Item("l_name").ToString
                End If
                str2 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetPrev2DailyPayClose(ByVal strDailyPayKind As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim dao2 As New CallRollUserDao
                Dim ds As DataSet = dao.GetPrev2DailyPayClose(strDailyPayKind)
                If (ds Is Nothing) Then
                    ds = New DataSet
                    Dim table As New DataTable("prev_daily_pay_close")
                    table.Columns.Add("max", GetType(String))
                    table.Columns.Add("next_max", GetType(String))
                    Dim row As DataRow = table.NewRow
                    Dim callRollOldDate As String = dao2.GetCallRollOldDate(strDailyPayKind)
                    If (Not callRollOldDate Is Nothing) Then
                        callRollOldDate = DateTime.ParseExact(callRollOldDate, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    End If
                    Dim s As String = DateTime.ParseExact(MDLoginInfo.PeriodFrom, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    If String.IsNullOrEmpty(callRollOldDate) Then
                        row.Item("max") = s
                        row.Item("next_max") = DateTime.ParseExact(s, "yyyyMMdd", Nothing).AddMonths(-3).ToString("yyyyMMdd")
                    ElseIf (callRollOldDate.CompareTo(s) > 0) Then
                        row.Item("max") = callRollOldDate
                        row.Item("next_max") = s
                    ElseIf (callRollOldDate.CompareTo(s) < 0) Then
                        row.Item("max") = s
                        row.Item("next_max") = callRollOldDate
                    Else
                        row.Item("max") = s
                        row.Item("next_max") = DateTime.ParseExact(s, "yyyyMMdd", Nothing).AddMonths(-3).ToString("yyyyMMdd")
                    End If
                    table.Rows.Add(row)
                    ds.Tables.Add(table)
                ElseIf ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("next_max").ToString.Equals("") Then
                    Dim str3 As String = ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("max").ToString
                    Dim strB As String = Nothing
                    Dim str5 As String = dao2.GetCallRollOldDate(strDailyPayKind)
                    If (Not str5 Is Nothing) Then
                        str5 = DateTime.ParseExact(str5, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    End If
                    Dim str6 As String = DateTime.ParseExact(MDLoginInfo.PeriodFrom, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    If String.IsNullOrEmpty(str5) Then
                        strB = str6
                    Else
                        strB = If((str5.CompareTo(str6) < 0), str5, str6)
                    End If
                    If (str3.CompareTo(strB) <= 0) Then
                        ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("next_max") = DateTime.ParseExact(str3, "yyyyMMdd", Nothing).AddMonths(-3).ToString("yyyyMMdd")
                    Else
                        ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("next_max") = strB
                    End If
                End If
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetPrevDailyPayClose(ByVal strDailyPayKind As String, ByVal strCloseDate As String) As String
            Dim str4 As String
            Try
                'Dim dao As IDailyPayCloseDao = DirectCast(class2.GetObject("DAO.FinancialAffairs.DailyAllowance.DailyPayCloseDao"), IDailyPayCloseDao)
                Dim dao As New DailyPayCloseDao
                Dim dao2 As New CallRollUserDao
                Dim prevDailyPayClose As String = dao.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
                DateTime.ParseExact(strCloseDate, "yyyyMMdd", Nothing)
                If String.IsNullOrEmpty(prevDailyPayClose) Then
                    Dim callRollOldDate As String = dao2.GetCallRollOldDate(strDailyPayKind)
                    If (Not callRollOldDate Is Nothing) Then
                        callRollOldDate = DateTime.ParseExact(callRollOldDate, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    End If
                    Dim strB As String = DateTime.ParseExact(MDLoginInfo.PeriodFrom, "yyyyMMdd", Nothing).AddDays(-1).ToString("yyyyMMdd")
                    If String.IsNullOrEmpty(callRollOldDate) Then
                        prevDailyPayClose = strB
                    Else
                        prevDailyPayClose = If((callRollOldDate.CompareTo(strB) < 0), callRollOldDate, strB)
                    End If
                End If
                str4 = prevDailyPayClose
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str4
        End Function

        Public Function GetQualificationName(ByVal strQualificationId As String, ByVal strDate As String, ByVal strPrevDate As String) As String
            Dim str2 As String
            Try
                'Dim class2 As New FactoryDaoClass
                'Dim dao As IConstantTblDao = DirectCast(class2.GetObject("DAO.Master.ConstantTblDao"), IConstantTblDao)
                Dim dao As New ConstantTblDao
                Dim rowArray As DataRow() = dao.GetConstantKind("QUALIFICATION", strDate).Tables.Item("constant_dtl").Select(String.Concat(New String() {"c_constant_seq = '", strQualificationId, "' AND ((d_from <= '", strPrevDate, "' AND d_to >= '", strPrevDate, "') OR (d_from <= '", strDate, "' AND d_to >= '", strDate, "') OR (d_from >= '", strPrevDate, "' AND d_to <= '", strDate, "'))"}), "d_from DESC")
                Dim str As String = ""
                If (rowArray.Length > 0) Then
                    str = rowArray(0).Item("l_omission_name").ToString
                End If
                str2 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function


        ' Fields
        Public Const CONFORM_DAY As String = "対象日"
        Public Const DEF_NEXT_PREV_MONTH_DIFF As Integer = -3
        Public Const EXECUTIVE_LUNCH_EXPLAIN As String = "中執昼食費"
        Public Const PREV_BALANCE As String = "前回差分"

        ' Nested Types
        Private Class MonthlyAttendDtlData
            ' Fields
            Public iAttendDaysNum As Integer
            Public iDailyPay As Integer
            Public iUnitPrice As Integer
            Public strAttendDays As String = ""
            Public strExplain As String = ""
        End Class

        Private Class MonthlyAttendFtrData
            ' Fields
            Public iTotal As Integer
            Public strExplain As String = ""
        End Class
    End Class
End Namespace
