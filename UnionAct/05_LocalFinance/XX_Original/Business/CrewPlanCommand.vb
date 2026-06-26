Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.DAO
Imports UnionAct.Framework.Mapping
Imports UnionAct.DAO.RevenueExpenditure
Imports UnionAct.Business.Master

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.NSMDInfo

Namespace Business.RevenueExpenditure
    Public Class CrewPlanCommand
        'Implements ICrewPlanCommand
        ' Methods
        Public Sub CreateRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strEnd As String, ByVal strKsh As String)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                If (dao.CreateRevenueExpenditureRetireDtlWork(command, strMac, strControlName, strUserId, strStr, strEnd, strKsh) < 0) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0026", New String(0 - 1) {})
                End If
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

        Public Function GetRevenueExpenditurePromotionDtl(ByVal strRevenueStr As String) As DataTable
            Dim table3 As DataTable
            Try
                Dim revenueExpenditurePromotionDtl As DataTable = dao.GetRevenueExpenditurePromotionDtl(strRevenueStr)
                Dim map As New CrewPlanMap
                table3 = PublicCommand.ConvertPhysicalToLogical(revenueExpenditurePromotionDtl, map)
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

        Public Function GetRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String) As DataTable
            Dim table4 As DataTable
            Try
                Dim revenueExpenditureRetireDtlWork As DataTable = dao.GetRevenueExpenditureRetireDtlWork(strMac, strControlName)
                Dim dTableIn As DataTable = New RevenueExpenditurePromotionDtlMap().CreateDataTablePhysName("revenue_expenditure_retire_dtl_work")
                Dim rowArray As DataRow() = revenueExpenditureRetireDtlWork.Select
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    dTableIn.ImportRow(rowArray(i))
                Next i
                Dim j As Integer
                For j = 0 To rowArray.Length - 1
                    dTableIn.Rows.Item(j).Item("s_new_staff_member") = 0
                    dTableIn.Rows.Item(j).Item("s_new_cap_member") = 0
                Next j
                Dim map As New CrewPlanMap
                table4 = PublicCommand.ConvertPhysicalToLogical(dTableIn, map)
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

        Public Function GetUnionDues(ByVal iNewStaffAV As Integer, ByVal iCapUpAV As Integer, ByVal iRetireAV As Integer, ByVal iUnPromotionAV As Integer, ByVal iSeniorAV As Integer, ByVal strDfrom As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = New CrewPlanUnionDuesMap().CreateDataTablePhysName("revenue_expenditure_retire_dtl_work")
                Dim row As DataRow = table.NewRow
                table.Rows.Add(row)
                Dim command As New UnionDuesCommand
                Dim num As Long = command.GetUnionDuesCommand("01", "02", "01", iNewStaffAV, strDfrom)
                table.Rows.Item(0).Item("s_new_staff_union_dues") = num
                num = command.GetUnionDuesCommand("02", "99", "01", iNewStaffAV, strDfrom)
                table.Rows.Item(0).Item("s_entry_money") = num
                num = command.GetUnionDuesCommand("01", "02", "01", iCapUpAV, strDfrom)
                table.Rows.Item(0).Item("s_new_cap_union_dues") = num
                num = command.GetUnionDuesCommand("01", "01", "01", iCapUpAV, strDfrom)
                table.Rows.Item(0).Item("s_new_cap_union_dues") = (num - Convert.ToInt64(table.Rows.Item(0).Item("s_new_cap_union_dues").ToString))
                num = command.GetUnionDuesCommand("01", "01", "01", iRetireAV, strDfrom)
                table.Rows.Item(0).Item("s_cap_retire_union_dues") = num
                num = command.GetUnionDuesCommand("01", "02", "01", iRetireAV, strDfrom)
                table.Rows.Item(0).Item("s_cop_retire_union_dues") = num
                num = command.GetUnionDuesCommand("01", "03", "01", iRetireAV, strDfrom)
                table.Rows.Item(0).Item("s_fe_retire_union_dues") = num
                num = command.GetUnionDuesCommand("01", "01", "01", iUnPromotionAV, strDfrom)
                table.Rows.Item(0).Item("s_unpromotion_union_dues") = num
                num = command.GetUnionDuesCommand("01", "01", "02", iSeniorAV, strDfrom)
                table.Rows.Item(0).Item("s_senior_retire_union_dues") = num
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Sub InsertRevenueExpenditurePromotionDtl(ByVal dtParameter As DataTable, ByVal dtKeyData As DataTable, ByVal dtUpdate As DataTable)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                Dim bizcmd As New AllottedChargeCommand
                Dim timeStamp As DataSet = (New RevenueExpenditureDao).GetTimeStamp(command, dtKeyData.Rows.Item(0).Item("d_revenue_str").ToString)
                bizcmd.CheckTimeStamp(timeStamp.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up").ToString, dtKeyData.Rows.Item(0).Item("d_up").ToString)
                Dim map As New RevenueExpenditurePromotionDtlMap
                Dim map2 As New CrewPlanMap
                Dim dTable As DataTable = map.CreateDataTablePhysName("revenue_expenditure_promotion_dtl")
                Dim rowArray As DataRow() = PublicCommand.ConvertLogicalToPhysical(dtParameter, map2).Select
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    dTable.ImportRow(rowArray(i))
                Next i
                Dim j As Integer
                For j = 0 To rowArray.Length - 1
                    dTable.Rows.Item(j).Item("d_revenue_str") = dtKeyData.Rows.Item(0).Item("d_revenue_str").ToString
                Next j
                Dim k As Integer
                For k = 0 To rowArray.Length - 1
                    dTable.Rows.Item(k).Item("d_target") = dTable.Rows.Item(k).Item("d_target").ToString.Replace("/", "").Replace("-", "").Insert((dTable.Rows.Item(k).Item("d_target").ToString.Length - 1), "01")
                Next k
                dao.UpDateRevenueExpenditure(command, dtKeyData, dtUpdate, MDLoginInfo.UserId)
                dao.InsertRevenueExpenditurePromotionDtl(command, dTable, MDLoginInfo.UserId)
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

        Public Sub UpdateRevenueExpenditure(ByVal dtKeyData As DataTable, ByVal dtParameter As DataTable, ByVal dtUpData As DataTable)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                Dim bizcmd As New AllottedChargeCommand
                Dim timeStamp As DataSet = (New RevenueExpenditureDao).GetTimeStamp(command, dtKeyData.Rows.Item(0).Item("d_revenue_str").ToString)
                bizcmd.CheckTimeStamp(timeStamp.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up").ToString, dtKeyData.Rows.Item(0).Item("d_up").ToString)
                dao.UpDateRevenueExpenditure(command, dtKeyData, dtParameter, MDLoginInfo.UserId)
                dao.UpdateRevenueExpenditurePromotionDtl(command, dtKeyData, dtUpData, MDLoginInfo.UserId)
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


        ' Fields
        Private dao As New CrewPlanDao
        Private Const D_REVENUE_STR As String = "d_revenue_str"
        Private Const S_CAP_RETIRE_UNION_DUES As String = "s_cap_retire_union_dues"
        Private Const S_COP_RETIRE_UNION_DUES As String = "s_cop_retire_union_dues"
        Private Const S_ENTRY_DUES As String = "s_entry_money"
        Private Const S_FE_RETIRE_UNION_DUES As String = "s_fe_retire_union_dues"
        Private Const S_NEW_CAP_UNION_DUES As String = "s_new_cap_union_dues"
        Private Const S_NEW_STAFF_UNION_DUES As String = "s_new_staff_union_dues"
        Private Const S_SENIOR_RETIRE_UNION_DUES As String = "s_senior_retire_union_dues"
        Private Const S_UNPROMOTION_UNION_DUES As String = "s_unpromotion_union_dues"
    End Class
End Namespace
