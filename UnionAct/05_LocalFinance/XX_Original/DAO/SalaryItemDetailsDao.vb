Imports UnionAct.Framework.UnionException

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection

Namespace DAO.Master
    Public Class SalaryItemDetailsDao
        Inherits AbstractDao
        'Implements ISalaryItemDetailsDao
        ' Methods
        Public Function GetAllData(ByVal strBasisDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select c_salary_item, d_from, k_salary_put, k_bonus_put, k_salary_item_classify, l_item_name, k_tax, k_deposit, k_magnification, s_order, k_deduction_put, c_deduction_items, c_deduction_fraction, c_deduction_decimal_point, s_deduction_order, l_biko from salary_item_details where d_from <= :d_from and :d_from <= d_to order by s_order "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_from", strBasisDate)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("salary_item_details", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetDepositItemColumns(ByVal strBasisDate As String, ByVal isSalary As Boolean, ByVal isUnionBear As Boolean) As DataSet
            Dim set2 As DataSet
            Try
                Dim command As New NpgsqlCommand(("select SAL.c_salary_item, SAL.l_item_name from salary_item_details SAL where SAL.d_from <= :d_from and :d_from <= SAL.d_to " & If(isSalary, "and SAL.k_salary_put = :k_salary_put ", "and SAL.k_bonus_put = :k_bonus_put ") & If(isUnionBear, "and SAL.k_deduction_put = :k_deduction_put ", "and SAL.k_deposit = :k_deposit ") & "order by SAL.s_order "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_from", strBasisDate)
                command.Parameters.Add("k_deposit", "1")
                command.Parameters.Add("k_salary_put", "1")
                command.Parameters.Add("k_bonus_put", "1")
                command.Parameters.Add("k_deduction_put", "1")
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("salary_item_details", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

    End Class
End Namespace
