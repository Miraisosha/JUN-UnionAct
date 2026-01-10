Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.DAO

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.RevenueExpenditure
Imports UnionAct.Framework.Command

Namespace Business.RevenueExpenditure
    Public Class AllottedChargeCommand
        Inherits AbstractCommand
        'Implements IAllottedChargeCommand
        ' Methods
        Private Sub ConvertBasisDateToNextYear(ByRef dTblAllotted As DataTable, ByVal strStartRev As String, ByVal strLastRev As String)
            Try
                Dim time As DateTime = DateTime.ParseExact(strStartRev, "yyyyMMdd", Nothing)
                Dim time2 As DateTime = DateTime.ParseExact(strLastRev, "yyyyMMdd", Nothing)
                Dim num As Integer = (time.Year - time2.Year)
                Dim i As Integer
                For i = 0 To dTblAllotted.Rows.Count - 1
                    If Not dTblAllotted.Rows.Item(i).IsNull("äÓèÄåé") Then
                        dTblAllotted.Rows.Item(i).Item("äÓèÄåé") = Convert.ToDateTime(dTblAllotted.Rows.Item(i).Item("äÓèÄåé")).AddYears(num)
                    End If
                Next i
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub Entry(ByVal dSetNew As DataSet, ByVal isNewEntry As Boolean, ByVal dTimeUp As DateTime)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                Dim dao As New RevenueExpenditureDao
                Dim timeStamp As DataSet = dao.GetTimeStamp(command, dSetNew.Tables.Item("revenue_allotted_charge").Rows.Item(0).Item("d_revenue_str").ToString)
                MyBase.CheckTimeStamp(timeStamp.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up").ToString, dTimeUp.ToString)
                If isNewEntry Then
                    Me._daoAlloted.Insert(command, dSetNew)
                Else
                    Me._daoAlloted.Update(command, dSetNew)
                End If
                dao.UpdateRevenueExpenditureForAllotted(command, dSetNew.Tables.Item("revenue_allotted_charge").Rows.Item(0).Item("d_revenue_str").ToString, Convert.ToDateTime(dSetNew.Tables.Item("revenue_allotted_charge").Rows.Item(0).Item("d_up")), dSetNew.Tables.Item("revenue_allotted_charge").Rows.Item(0).Item("c_user_id_up").ToString)
                objTran.Commit()
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Function GetAllottedCharge(ByVal strStartRevenue As String, ByVal strLastRevenue As String, ByVal isNewEntry As Boolean) As DataSet
            Dim set2 As DataSet
            Try
                Dim allottedChargeData As DataSet = Me._daoAlloted.GetAllottedChargeData(strStartRevenue, If(isNewEntry, strLastRevenue, strStartRevenue))
                If isNewEntry Then
                    Dim dTblAllotted As DataTable = allottedChargeData.Tables.Item("revenue_allotted_charge")
                    If Not String.IsNullOrEmpty(strLastRevenue) Then
                        Me.ConvertBasisDateToNextYear(dTblAllotted, strStartRevenue, strLastRevenue)
                    End If
                    Me.InsertMemberNumber(dTblAllotted, strStartRevenue)
                End If
                set2 = allottedChargeData
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

        Public Function GetTargetMemberCount(ByVal strRevenue As String, ByVal strBasisDate As String) As Integer
            Dim targetMemberCount As Integer
            Try
                targetMemberCount = (New RevenueExpenditureDao).GetTargetMemberCount(strRevenue, PublicCommand.GetMonthEnd(strBasisDate, -1).ToString("yyyyMMdd"))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return targetMemberCount
        End Function

        Private Sub InsertMemberNumber(ByRef dTblAllotted As DataTable, ByVal strRevenue As String)
            Try
                Dim i As Integer
                For i = 0 To dTblAllotted.Rows.Count - 1
                    If Not dTblAllotted.Rows.Item(i).IsNull("äÓèÄåé") Then
                        dTblAllotted.Rows.Item(i).Item("ëŒè€ëgçáàıêî") = (New RevenueExpenditureDao).GetTargetMemberCount(strRevenue, Convert.ToDateTime(dTblAllotted.Rows.Item(i).Item("äÓèÄåé")).AddDays(-1).ToString("yyyyMMdd"))
                    End If
                Next i
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub


        ' Fields
        Private _daoAlloted As New RevenueAllottedChargeDao
        Private Const CONS_BASIS_DATE As String = "äÓèÄåé"
    End Class
End Namespace
