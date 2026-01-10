Imports UnionAct.Business.FinancialAffairs
Imports UnionAct.DAO
Imports UnionAct.Framework
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.UnionException
Imports log4net
Imports System
Imports System.Collections
Imports System.Data
Imports System.Reflection
Imports UnionAct.DAO.FinancialAffairs.WithHolding
Imports UnionAct.NpgsqlDummy

Namespace Business.FinancialAffairs.WithHolding
    Public Class WithholdingCommand
        Inherits FinancialAffairsBase
        'Implements IWithholdingCommand
        ' Methods
        Private dao As WithHoldingDao
        Public Sub New()
            dao = New WithHoldingDao
        End Sub

        Public Sub New(ByVal strCut As String)
            Me._strCut = strCut
            dao = New WithHoldingDao(Me._strCut)
        End Sub

        Public Function CalcWithholding(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal Taxable As Long) As Long
            Dim num2 As Long
            Try
                Dim table As DataTable = Me.GetDao(New Object(0 - 1) {}).GetTaxMaster(PublicCommand.GetKsh, Taxable, CommonUtility.GetLastDay(TargetYear, TargetMonth))
                Dim strArray As String() = New String() {"課税対象額" & ":", Taxable.ToString("###,###,##0"), " " & "課税上限額" & "(" & "以上" & "):", CLng(table.Rows.Item(0).Item(0)).ToString("###,###,##0"), " " & "課税下限額" & "(" & "未満" & "):", CLng(table.Rows.Item(0).Item(1)).ToString("###,###,##0"), " " & "課税額" & ":", CLng(table.Rows.Item(0).Item(2)).ToString("###,###,##0"), " " & "課税率" & ":", CDbl(table.Rows.Item(0).Item(3)).ToString("##0.000")}
                WithholdingCommand._logger.Debug(String.Concat(strArray))
                Dim num As Long = If((CLng(table.Rows.Item(0).Item(2)) <> 0), CLng(table.Rows.Item(0).Item(2) + (Taxable - table.Rows.Item(0).Item(0)) * (CDbl(table.Rows.Item(0).Item(3)) / 100)), CLng(Fix(Taxable * (CDbl(table.Rows.Item(0).Item(3)) / 100))))
                WithholdingCommand._logger.Debug(("源泉徴収額" & ":" & num.ToString("###,###,###")))
                num2 = num
            Catch exception As DataNotFoundException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0047", New String() {("\" & Taxable.ToString("###,###,###"))})
            Catch exception2 As TooManyRowsException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception2, "GE0048", New String() {("\" & Taxable.ToString("###,###,###"))})
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Function ExecuteCalcWithholding(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UserId As String) As Integer
            Dim num2 As Integer
            Try
                Dim truncPlace As Integer = MyBase.GetTruncPlace(CommonUtility.GetLastDay(TargetYear, TargetMonth))
                WithholdingCommand._logger.Debug(("切捨て位置：" & truncPlace))
                num2 = Me.GetDao(New Object() {Me._strCut}).Calcuration(PublicCommand.GetKsh, (TargetYear & TargetMonth), CommonUtility.GetLastDay(TargetYear, TargetMonth), truncPlace, UserId)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Private Function GetDao(ByVal ParamArray args As Object()) As WithHoldingDao
            'Return DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WithHolding.WithHoldingDao", args), WithHoldingDao)
            Return dao
        End Function

        Public Function GetExistYear() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim existYears As DataTable = Me.GetDao(New Object(0 - 1) {}).GetExistYears
                ds.Tables.Add(existYears)
                set2 = ds
            Catch exception As BaseUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return set2
        End Function

        Public Function GetMaxYear() As Integer
            Return Me.GetMaxYear("04")
        End Function

        Public Function GetMaxYear(ByVal strCut As String) As Integer
            Dim num2 As Integer
            Try
                num2 = 0
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Function GetMinYear() As Integer
            Return Me.GetMinYear("04")
        End Function

        Public Function GetMinYear(ByVal strCut As String) As Integer
            Dim num2 As Integer
            Try
                num2 = 0
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        ' MOD 2012/06/24
        Public Function GetMonthlyNonTaxableData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String, Optional ByVal OnceName As String = "") As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object() {Me._strCut}).GetNonTaxableData(PublicCommand.GetKsh, (TargetYear & TargetMonth), UnionBranch, CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function GetMonthlyNonTaxableListReportData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal OnceName As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim monthlyNonTaxableReportHeader As DataTable = Me.GetMonthlyNonTaxableReportHeader(TargetYear, TargetMonth)
                ds.Tables.Add(monthlyNonTaxableReportHeader)
                Dim table As DataTable = Me.GetDao(New Object() {Me._strCut}).GetMonthlyNonTaxableListReportData(PublicCommand.GetKsh, (TargetYear & TargetMonth), CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Private Function GetMonthlyNonTaxableReportHeader(ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As New DataTable("dtHeader")
                table.Columns.Add("month", GetType(String))
                table.Columns.Add("s_break", GetType(String))
                Dim row As DataRow = table.NewRow
                row.Item("month") = TargetMonth
                row.Item("s_break") = MyBase.GetTruncateAmount(CommonUtility.GetLastDay(TargetYear, TargetMonth)).ToString
                table.Rows.Add(row)
                table2 = table
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function GetMonthlyReportDetailData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal Selectedmembers As ArrayList) As DataSet
            Dim set2 As DataSet
            Try
                Dim truncateAmount As Integer = MyBase.GetTruncateAmount(CommonUtility.GetLastDay(TargetYear, TargetMonth))
                Dim ds As New DataSet
                Dim table As DataTable = Me.GetDao(New Object() {Me._strCut}).GetMonthlyReportDetailData(PublicCommand.GetKsh, (TargetYear & TargetMonth), Selectedmembers, truncateAmount, CommonUtility.GetLastDay(TargetYear, TargetMonth))
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetMonthlyReportListData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal OnceName As String) As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object() {Me._strCut}).GetPrintListData(PublicCommand.GetKsh, (TargetYear & TargetMonth), CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        ' MOD 2012/06/24
        Public Function GetMonthlyTaxableData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String, Optional ByVal OnceName As String = "") As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object() {Me._strCut}).GetTaxableData(PublicCommand.GetKsh, (TargetYear & TargetMonth), UnionBranch, CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Private Function GetMonthlyTaxableSummary(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal OnceName As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = Me.GetDao(New Object() {Me._strCut}).GetTaxableSummary(PublicCommand.GetKsh, (TargetYear & TargetMonth), CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
                If (table.Rows.Count > 0) Then
                    Dim num As Long = 0
                    Dim num2 As Long = 0
                    Dim num3 As Long = 0
                    Dim num4 As Long = 0
                    Dim num5 As Long = 0
                    Dim num6 As Long = 0
                    Dim num7 As Long = 0
                    Dim row2 As DataRow
                    For Each row2 In table.Rows
                        num = (num + CLng(row2.Item(1)))
                        num2 = (num2 + CLng(row2.Item(2)))
                        num3 = (num3 + CLng(row2.Item(3)))
                        num4 = (num4 + CLng(row2.Item(4)))
                        num5 = (num5 + CLng(row2.Item(5)))
                        num6 = (num6 + CLng(row2.Item(6)))
                        num7 = (num7 + CLng(row2.Item(7)))
                    Next
                    Dim row As DataRow = table.NewRow
                    row.Item(0) = "合計"
                    row.Item(1) = num
                    row.Item(2) = num2
                    row.Item(3) = num3
                    row.Item(4) = num4
                    row.Item(5) = num5
                    row.Item(6) = num6
                    row.Item(7) = num7
                    table.Rows.Add(row)
                End If
                table2 = table
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Function GetNonMonthlyTaxableSummary(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal OnceName As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = Me.GetDao(New Object() {Me._strCut}).GetNonTaxableSummary(PublicCommand.GetKsh, (TargetYear & TargetMonth), CommonUtility.GetLastDay(TargetYear, TargetMonth), OnceName)
                If (table.Rows.Count > 0) Then
                    Dim num As Long = 0
                    Dim num2 As Long = 0
                    Dim num3 As Long = 0
                    Dim num4 As Long = 0
                    Dim num5 As Long = 0
                    Dim row2 As DataRow
                    For Each row2 In table.Rows
                        num = (num + CLng(row2.Item(1)))
                        num2 = (num2 + CLng(row2.Item(2)))
                        num3 = (num3 + CLng(row2.Item(3)))
                        num4 = (num4 + CLng(row2.Item(4)))
                        num5 = (num5 + CLng(row2.Item(5)))
                    Next
                    Dim row As DataRow = table.NewRow
                    row.Item(0) = "合計"
                    row.Item(1) = num
                    row.Item(2) = num2
                    row.Item(3) = num3
                    row.Item(4) = num4
                    row.Item(5) = num5
                    table.Rows.Add(row)
                End If
                table2 = table
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function GetSumUpNonTaxableData(ByVal TargetYear As String, ByVal UnionBranch As String) As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object(0 - 1) {}).GetSumUpNonTaxableDetailListData(PublicCommand.GetKsh, TargetYear, UnionBranch, (TargetYear & "1231"))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function GetSumUpNonTaxableDetailReportData(ByVal TargetYear As String, ByVal Selectedmembers As ArrayList) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim table As DataTable = Me.GetDao(New Object(0 - 1) {}).GetPaymentSlipReportDetailData(PublicCommand.GetKsh, TargetYear, Selectedmembers, (TargetYear & "1231"))
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetSumUpNonTaxableListReportData(ByVal TargetYear As String) As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object(0 - 1) {}).GetSumUpNonTaxableListReportData(PublicCommand.GetKsh, TargetYear, (TargetYear & "1231"))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function GetSumUpTaxableData(ByVal TargetYear As String, ByVal UnionBranch As String) As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object(0 - 1) {}).GetSumUpTaxableDetailListData(PublicCommand.GetKsh, TargetYear, UnionBranch, (TargetYear & "1231"))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function GetSumUpTaxableDetailReportData(ByVal TargetYear As String, ByVal Selectedmembers As ArrayList) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim table As DataTable = Me.GetDao(New Object(0 - 1) {}).GetSumUpReportDetailData(PublicCommand.GetKsh, TargetYear, Selectedmembers, (TargetYear & "1231"))
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetSumUpTaxableListReportData(ByVal TargetYear As String) As DataTable
            Dim table As DataTable
            Try
                table = Me.GetDao(New Object(0 - 1) {}).GetSumUpTaxableListReportData(PublicCommand.GetKsh, TargetYear, (TargetYear & "1231"))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function IsGreaterThanExists(ByVal TargetYear As String, ByVal TargetMonth As String) As Boolean
            Dim flag As Boolean
            Try
                flag = Me.GetDao(New Object() {Me._strCut}).IsGreaterThanExists((TargetYear & TargetMonth))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Function IsWithholdingExists(ByVal TargetYear As String, ByVal TargetMonth As String) As Boolean
            Dim flag As Boolean
            Try
                flag = Me.GetDao(New Object() {Me._strCut}).IsExists((TargetYear & TargetMonth))
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Function QueryMonthlySummary(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal OnceName As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim monthlyTaxableSummary As DataTable = Me.GetMonthlyTaxableSummary(TargetYear, TargetMonth, OnceName)
                ds.Tables.Add(monthlyTaxableSummary)
                Dim nonMonthlyTaxableSummary As DataTable = Me.GetNonMonthlyTaxableSummary(TargetYear, TargetMonth, OnceName)
                ds.Tables.Add(nonMonthlyTaxableSummary)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Private Function QueryNonTaxableSumUp(ByVal TargetYear As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = Me.GetDao(New Object(0 - 1) {}).GetNonTaxableSumUpData(PublicCommand.GetKsh, TargetYear, (TargetYear & "1231"))
                If (table.Rows.Count > 0) Then
                    Dim num As Long = 0
                    Dim num2 As Long = 0
                    Dim num3 As Long = 0
                    Dim row2 As DataRow
                    For Each row2 In table.Rows
                        num = (num + CLng(row2.Item(1)))
                        num2 = (num2 + CLng(row2.Item(2)))
                        num3 = (num3 + CLng(row2.Item(3)))
                    Next
                    Dim row As DataRow = table.NewRow
                    row.Item(0) = "合計"
                    row.Item(1) = num
                    row.Item(2) = num2
                    row.Item(3) = num3
                    table.Rows.Add(row)
                End If
                table2 = table
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function QuerySumUpSummary(ByVal TargetYear As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim table As DataTable = Me.QueryTaxableSumUp(TargetYear)
                ds.Tables.Add(table)
                Dim table2 As DataTable = Me.QueryNonTaxableSumUp(TargetYear)
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Private Function QueryTaxableSumUp(ByVal TargetYear As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = Me.GetDao(New Object(0 - 1) {}).GetSumUpTaxableSumData(PublicCommand.GetKsh, TargetYear, (TargetYear & "1231"))
                If (table.Rows.Count > 0) Then
                    Dim num As Long = 0
                    Dim num2 As Long = 0
                    Dim num3 As Long = 0
                    Dim num4 As Long = 0
                    Dim num5 As Long = 0
                    Dim num6 As Long = 0
                    Dim row2 As DataRow
                    For Each row2 In table.Rows
                        num = (num + CLng(row2.Item(1)))
                        num2 = (num2 + CLng(row2.Item(2)))
                        num3 = (num3 + CLng(row2.Item(3)))
                        num4 = (num4 + CLng(row2.Item(4)))
                        num5 = (num5 + CLng(row2.Item(5)))
                        num6 = (num6 + CLng(row2.Item(6)))
                    Next
                    Dim row As DataRow = table.NewRow
                    row.Item(0) = "合計"
                    row.Item(1) = num
                    row.Item(2) = num2
                    row.Item(3) = num3
                    row.Item(4) = num4
                    row.Item(5) = num5
                    row.Item(6) = num6
                    table.Rows.Add(row)
                End If
                table2 = table
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function UpdateData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal SaveData As DataTable, ByVal userid As String) As Integer
            Dim num2 As Integer
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
            Try
                Dim dao As WithHoldingDao = Me.GetDao(New Object() {Me._strCut})
                Dim num As Integer = 0
                'DbTransaction.NpgsqlBeginTransaction()
                Dim row As DataRow
                For Each row In SaveData.Rows
                    If CBool(row.Item(0)) Then
                        dao.Update(command, (TargetYear & TargetMonth), CStr(row.Item(13)), CLng(row.Item(5)), CLng(row.Item(11)), userid)
                        num += 1
                    End If
                Next
                objTran.Commit()
                num2 = num
            Catch exception As DataNotFoundException
                objTran.Rollback()
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0052", New String(0 - 1) {})
            Catch exception2 As TooManyRowsException
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            Catch exception3 As SysUnionException
                objTran.Rollback()
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Function UpdateDataTaxation( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal UpdateUserId As String, _
            ByVal dt As DataTable _
        ) As Integer

            Dim num2 As Integer
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
            Try
                Dim dao As WithHoldingDao = Me.GetDao(New Object() {Me._strCut})
                Dim num As Integer = 0
                ' 配列リスト分ループ
                For i = 0 To dt.Rows.Count - 1
                    ' 源泉徴収額更新処理
                    dao.UpdateTaxation(command, (TargetYear & TargetMonth), UpdateUserId, dt.Rows(i).Item(0), dt.Rows(i).Item(1))
                    num += 1
                Next i
                objTran.Commit()
                num2 = num
            Catch exception As DataNotFoundException
                objTran.Rollback()
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0052", New String(0 - 1) {})
            Catch exception2 As TooManyRowsException
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            Catch exception3 As SysUnionException
                objTran.Rollback()
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Public Function UpdateDataTaxationOfficersAllowance( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal UpdateUserId As String, _
            ByVal dt As DataTable _
        ) As Integer

            Dim num2 As Integer
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
            Try
                Dim dao As WithHoldingDao = Me.GetDao(New Object() {Me._strCut})
                Dim num As Integer = 0
                ' 配列リスト分ループ
                For i = 0 To dt.Rows.Count - 1
                    ' 役員手当のみ課税対象額として源泉徴収更新
                    dao.UpdateTaxationOfficersAllowance(command, (TargetYear & TargetMonth), UpdateUserId, dt.Rows(i).Item(0), dt.Rows(i).Item(1))
                    num += 1
                Next i
                objTran.Commit()
                num2 = num
            Catch exception As DataNotFoundException
                objTran.Rollback()
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0052", New String(0 - 1) {})
            Catch exception2 As TooManyRowsException
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            Catch exception3 As SysUnionException
                objTran.Rollback()
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private _strCut As String
        Private Const TOTAL_BRANCH_NAME As String = "合計"

        ' Nested Types
        Private Enum COLIDX_MONTHLY_TAXABLE_SUM
            ' Fields
            BONUS_WAGE_REDUCTION = 3
            BRANCH_NAME = 0
            DIRECTORS_REMUNERATION = 1
            MONTHLY_WAGE_REDUCTION = 2
            PAYOUT = 7
            TAXABLE = 4
            TRUNCATE = 5
            WITHHOLDING = 6
        End Enum

        Private Enum COLIDX_SUMUP_NONTAXABLE_SUM
            ' Fields
            BONUS_COVER = 2
            BRANCH = 0
            MONTHLY_COVER = 1
            TOTAL_PAY = 3
        End Enum

        Private Enum COLIDX_SUMUP_TAXABLE_SUM
            ' Fields
            BONUS_COVER = 3
            BRANCH = 0
            DIRECTORS_REMUNERATION = 1
            MONTHLY_COVER = 2
            PAY_OUT = 6
            TOTAL_PAY = 4
            WITHHOLDING = 5
        End Enum

        Private Enum COLIDX_TAXABLE_DETAIL_LIST
            ' Fields
            ALLOWANCE = 12
            BONUS_DEDUCTION = 6
            CHECK = 0
            CUT_OFF = 8
            DIRECTORS_REMUNERATION = 4
            EMPLOYEE_NUMBER = 1
            LICENSE = 3
            MONTHLY_DEDUCTION = 5
            NAME = 2
            TAXABLE = 7
            USER_ID = 13
            WITHHOLDING = 9
            WITHHOLDING_BONUS = 11
            WITHHOLDING_MONTHLY = 10
        End Enum

        Private Enum CONIDX_MONTHLY_NONTAXABLE_SUM
            ' Fields
            BONUS_WAGE_REDUCTION = 2
            BRANCH_NAME = 0
            MONTHLY_WAGE_REDUCTION = 1
            PAYOUT = 5
            TAXABLE = 3
            TRUNCATE = 4
        End Enum
    End Class
End Namespace
