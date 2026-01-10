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
    Public Class BudgetaryProcessCommand
        Inherits AbstractCommand
        'Implements IBudgetaryProcessCommand
        ' Methods
        Private Sub CheckRevenueExpenditureTimeStamp(ByVal strRevenueDate As String, ByVal dTimeUp As DateTime)
            Try
                Dim timeStamp As DataSet = (New RevenueExpenditureDao).GetTimeStamp(strRevenueDate)
                MyBase.CheckTimeStamp(timeStamp.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up").ToString, dTimeUp.ToString)
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

        Private Sub ConvertReviseRevenueTotal(ByRef dSetRevenue As DataSet, ByVal lngReviseRevenueTotal As Long)
            Try
                Dim str As String = "revenue_budgetary_process"
                If (dSetRevenue.Tables.Item(str).Rows.Count <> 0) Then
                    dSetRevenue.Tables.Item(str).Rows.Item(0).Item("îıçl") = Nothing
                Else
                    dSetRevenue.Tables.Item(str).Rows.Add(dSetRevenue.Tables.Item(str).NewRow)
                End If
                dSetRevenue.Tables.Item(str).Rows.Item(0).Item("çÄñ⁄î‘çÜ") = "1"
                dSetRevenue.Tables.Item(str).Rows.Item(0).Item("çÄñ⁄ñº") = "ëgçáîÔ"
                dSetRevenue.Tables.Item(str).Rows.Item(0).Item("ã‡äz") = lngReviseRevenueTotal
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

        Public Sub Entry(ByVal dSetNew As DataSet, ByVal isNewEntry As Boolean, ByVal isRevise As Boolean, ByVal dTimeUp As DateTime)
            Dim objTran As NpgsqlTransaction = Nothing
            Try
                Me.CheckRevenueExpenditureTimeStamp(dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(0).Item("d_revenue_str").ToString, dTimeUp)
                objTran = CommonDaoClass.connNpgsql.BeginTransaction
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                If isNewEntry Then
                    Me._objRevenue.InsertRevenueBudgetaryProcess(command, dSetNew)
                Else
                    Me._objRevenue.UpdateRevenueBudgetaryProcess(command, dSetNew)
                    Dim i As Integer
                    For i = 0 To dSetNew.Tables.Item("revenue_budgetary_process").Rows.Count - 1
                        Me._objRevenue.DeleteRevenueBudgetaryProcessDtl(command, dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("d_revenue_str").ToString, dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("k_budgetary_kind").ToString, dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("k_budgetary_process").ToString)
                    Next i
                End If
                Dim dao As New RevenueExpenditureDao
                Me._objRevenue.InsertRevenueBudgetaryProcessDtl(command, dSetNew)
                dao.UpdateRevenueExpenditureForBudgetary(command, dSetNew, isRevise)
                objTran.Commit()
            Catch exception As AppUnionException
                If Not objTran Is Nothing Then
                    objTran.Rollback()
                End If
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                If Not objTran Is Nothing Then
                    objTran.Rollback()
                End If
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                If Not objTran Is Nothing Then
                    objTran.Rollback()
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Function GetRevenueBudgetary(ByVal strRevenueDate As String, ByVal strLastRevenueDate As String, ByVal isNewEntry As Boolean, ByVal isRevise As Boolean, ByVal lngReviseRevenueTotal As Long) As DataSet
            Dim set4 As DataSet
            Try
                Dim ds As New DataSet
                Dim str As String = Me.GetRevenueStr(strRevenueDate, strLastRevenueDate, isNewEntry, isRevise)
                Dim strBudgetaryKind As String = If(Me.GetReviseFlag(isNewEntry, isRevise), "02", "01")
                Dim dSetRevenue As DataSet = Me._objRevenue.GetRevenueBudgetaryProcessData(str, strBudgetaryKind, "01")
                If (isNewEntry AndAlso Not isRevise) Then
                    If (dSetRevenue.Tables.Item("revenue_budgetary_process").Rows.Count = 0) Then
                        strBudgetaryKind = "01"
                        dSetRevenue = Me._objRevenue.GetRevenueBudgetaryProcessData(str, strBudgetaryKind, "01")
                    End If
                    Me.ConvertReviseRevenueTotal(dSetRevenue, lngReviseRevenueTotal)
                End If
                dSetRevenue.Tables.Item("revenue_budgetary_process").TableName = "Revenue"
                Dim set3 As DataSet = Me._objRevenue.GetRevenueBudgetaryProcessData(str, strBudgetaryKind, "02")
                set3.Tables.Item("revenue_budgetary_process").TableName = "Expend"
                ds.Tables.Add(dSetRevenue.Tables.Item("Revenue").Copy)
                ds.Tables.Add(set3.Tables.Item("Expend").Copy)
                set4 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set4
        End Function

        Private Function GetRevenueStr(ByVal strRevenueDate As String, ByVal strLastRevenueDate As String, ByVal isNewEntry As Boolean, ByVal isRevise As Boolean) As String
            Dim str As String
            Try
                If (isNewEntry AndAlso Not isRevise) Then
                    Return strLastRevenueDate
                End If
                str = strRevenueDate
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str
        End Function

        Private Function GetReviseFlag(ByVal isNewEntry As Boolean, ByVal isRevise As Boolean) As Boolean
            Dim flag As Boolean
            Try
                If ((isNewEntry AndAlso Not isRevise) OrElse (Not isNewEntry AndAlso isRevise)) Then
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Sub UpdateRevenueSeton(ByVal strRevenueDate As String, ByVal dTimeUp As DateTime, ByVal strUserIDup As String)
            'Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Me.CheckRevenueExpenditureTimeStamp(strRevenueDate, dTimeUp)
                Dim dao As New RevenueExpenditureDao
                dao.UpdateRevenueSetonFlag(strRevenueDate, PublicCommand.GetNow, strUserIDup)
                'objTran.Commit()
            Catch exception As SysUnionException
                'objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                'objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
        End Sub


        ' Fields
        Private _objRevenue As New RevenueBudgetaryProcessDao
        Private Const TABLE_EXPEND As String = "Expend"
        Private Const TABLE_REVENUE As String = "Revenue"
    End Class
End Namespace
