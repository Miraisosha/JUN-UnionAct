'Imports Framework.Mapping
'Imports Framework.UnionException
Imports log4net
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection

Namespace DAO.FinancialAffairs.WageReduction
    Public Class MonthlyInTimeDao
        Inherits MonthlyBaseDao
        ' Methods
        Public Sub New()
            MyBase.New("pay_time_cut_monthly")
        End Sub

        Public Overloads Function GetPrintDetailData(ByVal CompanyCode As String, ByVal TargetYM As String, ByVal TruncPlace As Integer, ByVal CriterionDate As String) As DataTable
            Dim table2 As DataTable
            Dim map As New WageReductionListMonthlyReportMap
            Dim cmdText As String = String.Concat(New Object() {"SELECT member.c_staf_id AS ", map.GetPhysicalName(0), ", member.l_name AS ", map.GetPhysicalName(1), ", c_branch.l_name AS ", map.GetPhysicalName(2), ", cut.i_cut AS ", map.GetPhysicalName(3), ", cut.i_cover AS ", map.GetPhysicalName(4), ", cut.i_dues AS ", map.GetPhysicalName(5), ", 0 AS ", map.GetPhysicalName(6), ", 0 AS ", map.GetPhysicalName(7), ", 0 AS ", map.GetPhysicalName(8), " FROM (((SELECT i_cut.c_user_id AS c_user_id,IIF(ISNULL(i_cut.s_pay_cut), 0, i_cut.s_pay_cut) AS i_cut,IIF(ISNULL(i_cut.cover), 0, i_cut.cover) AS i_cover,IIF(ISNULL(i_cut.s_pay_cut), 0, i_cut.s_pay_cut) - IIF(ISNULL(i_cut.cover), 0, i_cut.cover) AS i_dues FROM (SELECT c_user_id, s_pay_cut," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover FROM pay_time_cut_monthly WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) i_cut ) cut LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON (cut.c_user_id = member.c_user_id)) LEFT OUTER JOIN (SELECT A2.* FROM area_local_view A2, (SELECT c_constant_seq, MAX(area_local_view.d_from) AS d_from FROM area_local_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B2 WHERE A2.c_constant_seq = B2.c_constant_seq AND A2.d_from = B2.d_from ) c_branch ON (member.k_local = c_branch.c_constant_seq)) ORDER BY member.k_local, RIGHT('0000000000' + member.c_staf_id, 10)"})
            Try
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                MonthlyInTimeDao._logger.Debug(cmdText)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_years").Value = TargetYM
                MonthlyInTimeDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Public Const TABLE_NAME As String = "pay_time_cut_monthly"
    End Class
End Namespace
