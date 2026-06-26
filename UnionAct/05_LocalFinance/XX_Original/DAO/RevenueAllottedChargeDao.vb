Imports UnionAct.Framework.Mapping

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace DAO.RevenueExpenditure
    Public Class RevenueAllottedChargeDao
        Inherits AbstractDao
        'Implements IRevenueAllottedChargeDao
        ' Methods
        Public Function GetAllottedChargeData(ByVal strDateForItem As String, ByVal strDateForAllotted As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim cmdText As String = "select EXP_ITEM.*, to_date(ALLOT.d_standard_date, 'yyyyMMdd') as " & "äÓèÄåé" & ", ALLOT.s_unit_price as " & "éxï•íPâø" & ", ALLOT.s_year_month as " & "îNä‘éxï•åéêî" & ", ALLOT.s_member as " & "ëŒè€ëgçáàıêî" & ", ALLOT.s_total as " & "çáåvã‡äz" & " from (select HED.c_expend_item, HED.d_from, DTL.c_expend_item_seq, HED.l_item_name as " & "â»ñ⁄" & ", DTL.l_item_dtl_name as " & "â»ñ⁄ç◊ñ⁄" & " from (select c_expend_item, d_from, l_item_name, s_item_order from expend_item where d_from <= :date_expend_item and d_to >= :date_expend_item ) HED, expend_item_dtl DTL where HED.c_expend_item = DTL.c_expend_item and HED.d_from = DTL.d_from order by HED.s_item_order, DTL.s_item_dtl_order ) EXP_ITEM left outer join revenue_allotted_charge ALLOT on ALLOT.d_revenue_str = :d_revenue_str and ALLOT.c_expend_item = EXP_ITEM.c_expend_item and ALLOT.c_expend_item_seq = EXP_ITEM.c_expend_item_seq "
                'Dim cmdText As String = "select EXP_ITEM.*, ALLOT.d_standard_date as " & "äÓèÄåé" & ", ALLOT.s_unit_price as " & "éxï•íPâø" & ", ALLOT.s_year_month as " & "îNä‘éxï•åéêî" & ", ALLOT.s_member as " & "ëŒè€ëgçáàıêî" & ", ALLOT.s_total as " & "çáåvã‡äz" & " from (select HED.c_expend_item, HED.d_from, DTL.c_expend_item_seq, HED.l_item_name as " & "â»ñ⁄" & ", DTL.l_item_dtl_name as " & "â»ñ⁄ç◊ñ⁄" & " from (select c_expend_item, d_from, l_item_name, s_item_order from expend_item where d_from <= :date_expend_item and d_to >= :date_expend_item ) HED, expend_item_dtl DTL where HED.c_expend_item = DTL.c_expend_item and HED.d_from = DTL.d_from order by HED.s_item_order, DTL.s_item_dtl_order ) EXP_ITEM left outer join revenue_allotted_charge ALLOT on ALLOT.c_expend_item = EXP_ITEM.c_expend_item and ALLOT.c_expend_item_seq = EXP_ITEM.c_expend_item_seq WHERE ALLOT.d_revenue_str = :d_revenue_str "
                Dim cmdText As String = "select EXP_ITEM.*, IIF(ALLOT.d_standard_date IS NULL, NULL, FORMAT(CONVERT(DATE,ALLOT.d_standard_date,112), 'yyyy/MM/dd')) as " & "äÓèÄåé" & ", ALLOT.s_unit_price as " & "éxï•íPâø" & ", ALLOT.s_year_month as " & "îNä‘éxï•åéêî" & ", ALLOT.s_member as " & "ëŒè€ëgçáàıêî" & ", ALLOT.s_total as " & "çáåvã‡äz" & " from (select HED.c_expend_item, HED.d_from, DTL.c_expend_item_seq, HED.l_item_name as " & "â»ñ⁄" & ", DTL.l_item_dtl_name as " & "â»ñ⁄ç◊ñ⁄" & " from (select c_expend_item, d_from, l_item_name, s_item_order from expend_item where d_from <= :date_expend_item and d_to >= :date_expend_item ) HED, expend_item_dtl DTL where HED.c_expend_item = DTL.c_expend_item and HED.d_from = DTL.d_from order by HED.s_item_order, DTL.s_item_dtl_order OFFSET 0 ROWS) EXP_ITEM left outer join revenue_allotted_charge ALLOT on ALLOT.c_expend_item = EXP_ITEM.c_expend_item and ALLOT.c_expend_item_seq = EXP_ITEM.c_expend_item_seq"
                If strDateForAllotted <> "" Then
                    cmdText += " WHERE ALLOT.d_revenue_str = :d_revenue_str "
                End If
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("date_expend_item", DbType.String)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("date_expend_item").Value = strDateForItem
                command.Parameters.Item("d_revenue_str").Value = strDateForAllotted
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("revenue_allotted_charge", dReader))
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

        Public Sub Insert(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet)
            Try
                MyBase.InsertData(command, New RevenueAllottedChargeMap, dSetNew, "revenue_allotted_charge")
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub Update(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet)
            Try
                Dim cmdText As String = "update revenue_allotted_charge set s_unit_price = :s_unit_price, s_year_month = :s_year_month, d_standard_date = :d_standard_date, s_member = :s_member, s_total = :s_total, d_up = :d_up, c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str and c_expend_item = :c_expend_item and c_expend_item_seq = :c_expend_item_seq "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                Dim i As Integer
                For i = 0 To dSetNew.Tables.Item("revenue_allotted_charge").Rows.Count - 1
                    command.SetSql(cmdText)
                    command.Parameters.Clear()
                    Dim row As DataRow = dSetNew.Tables.Item("revenue_allotted_charge").Rows.Item(i)
                    command.Parameters.Add("s_unit_price", DbType.Int32)
                    command.Parameters.Add("s_year_month", DbType.Int32)
                    command.Parameters.Add("d_standard_date", DbType.String)
                    command.Parameters.Add("s_member", DbType.Int32)
                    command.Parameters.Add("s_total", DbType.Int32)
                    command.Parameters.Add("d_up", DbType.String)
                    command.Parameters.Add("c_user_id_up", DbType.String)
                    command.Parameters.Add("d_revenue_str", DbType.String)
                    command.Parameters.Add("c_expend_item", DbType.String)
                    command.Parameters.Add("c_expend_item_seq", DbType.Int32)
                    command.Parameters.Item("s_unit_price").Value = row.Item("s_unit_price")
                    command.Parameters.Item("s_year_month").Value = row.Item("s_year_month")
                    command.Parameters.Item("d_standard_date").Value = row.Item("d_standard_date")
                    command.Parameters.Item("s_member").Value = row.Item("s_member")
                    command.Parameters.Item("s_total").Value = row.Item("s_total")
                    command.Parameters.Item("d_up").Value = row.Item("d_up")
                    command.Parameters.Item("c_user_id_up").Value = row.Item("c_user_id_up")
                    command.Parameters.Item("d_revenue_str").Value = row.Item("d_revenue_str")
                    command.Parameters.Item("c_expend_item").Value = row.Item("c_expend_item")
                    command.Parameters.Item("c_expend_item_seq").Value = row.Item("c_expend_item_seq")
                    If (command.ExecuteNonQuery <> 1) Then
                        Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                    End If
                Next i
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
        End Sub

    End Class
End Namespace
