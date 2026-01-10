Imports UnionAct.DAO
Imports UnionAct.Framework.Mapping
Imports log4net
Imports System.Reflection

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Collections
Imports System.Data

Namespace DAO.FinancialAffairs
    Public MustInherit Class FinancialAffairsBaseDao
        Inherits AbstractDao
        ' Methods
        Protected Sub New()
        End Sub

        Public Function DataReader2LogicalDataTable(ByVal TableName As String, ByVal map As EntityMap, ByVal dReader As NpgsqlDataReader) As DataTable
            Dim dTable As DataTable = map.CreateDataTableLogiName(TableName)
            Me.TransferData(map, dReader, dTable)
            Return dTable
            'dReader.getTable.TableName = TableName
            'Return dReader.getTable
        End Function

        Public Function DataReader2PhysicalDataTable(ByVal TableName As String, ByVal map As EntityMap, ByVal dReader As NpgsqlDataReader) As DataTable
            Dim dTable As DataTable = map.CreateDataTablePhysName(TableName)
            Me.TransferData(map, dReader, dTable)
            Return dTable
        End Function

        Private Sub TransferData(ByVal map As EntityMap, ByVal dReader As NpgsqlDataReader, ByRef dTable As DataTable)
            Dim values As Object() = New Object(map.ColumnCount - 1) {}
            Dim record As DataRow
            For Each record In DirectCast(dReader, IEnumerable)
                Me.GetValues(record, values)
                dTable.Rows.Add(values)
            Next
        End Sub

        Private Sub GetValues(ByVal record As DataRow, ByRef values As Object())
            Dim index As Integer = 0
            For Each elem As Object In record.ItemArray
                values(index) = elem
                index = index + 1
                If values.Length = index Then
                    Exit For
                End If
            Next
        End Sub

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Protected Const DATATABLE_NAME_DETAIL As String = "dtDetail"
        Protected Const DATATABLE_NAME_HEADER As String = "dtHeader"
        Protected Const IN_LINE_VIEW_ADDRESS As String = "( SELECT A6.* FROM staf_address A6, ( SELECT c_user_id, MAX(d_from) AS d_from FROM staf_address WHERE d_from <= :d_from AND k_main_add = '1' GROUP BY c_user_id ) B6 WHERE A6.c_user_id = B6.c_user_id AND A6.d_from = B6.d_from AND k_main_add = '1' ) address "
        Protected Const IN_LINE_VIEW_COMPANY_BRANCH As String = "(SELECT A2.* FROM area_local_view A2, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM area_local_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B2 WHERE A2.c_constant_seq = B2.c_constant_seq AND A2.d_from = B2.d_from ) c_branch "
        Protected Const IN_LINE_VIEW_LICENSE As String = "(SELECT A4.* FROM qualification_view A4, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM qualification_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B4 WHERE A4.c_constant_seq = B4.c_constant_seq AND A4.d_from = B4.d_from ) license "
        Protected Const IN_LINE_VIEW_MEMBER As String = "(SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member "
        Protected Const IN_LINE_VIEW_MODEL As String = "(SELECT A5.* FROM model_view A5, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM model_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B5 WHERE A5.c_constant_seq = B5.c_constant_seq AND A5.d_from = B5.d_from ) model "
        Protected Const IN_LINE_VIEW_PAYER_ADDRESS1 As String = "(SELECT ADDR1.l_omission_name AS addname FROM constant_dtl ADDR1, ( SELECT c_constant, c_constant_seq, MAX(d_from) AS d_from FROM constant_dtl WHERE c_constant = 'FIX_ADDRESS_INFO' AND c_constant_seq = '03' AND d_from <= :d_from GROUP BY c_constant, c_constant_seq ) ADDR2 WHERE  ADDR1.c_constant = ADDR2.c_constant AND ADDR1.c_constant_seq = ADDR2.c_constant_seq AND ADDR1.d_from = ADDR2.d_from ) payer_address1 "
        Protected Const IN_LINE_VIEW_PAYER_ADDRESS2 As String = "(SELECT ADDR1.l_omission_name AS addname FROM constant_dtl ADDR1, ( SELECT c_constant, c_constant_seq, MAX(d_from) AS d_from FROM constant_dtl WHERE c_constant = 'FIX_ADDRESS_INFO' AND c_constant_seq = '04' AND d_from <= :d_from GROUP BY c_constant, c_constant_seq ) ADDR2 WHERE  ADDR1.c_constant = ADDR2.c_constant AND ADDR1.c_constant_seq = ADDR2.c_constant_seq AND ADDR1.d_from = ADDR2.d_from ) payer_address2 "
        Private Const IN_LINE_VIEW_SELECT_SUMUP_TAXABLE_MEMBER As String = "(select c_user_id, d_years from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))"
        Private Const IN_LINE_VIEW_SELECT_TAXABLE_MEMBER As String = "(select c_user_id from taxation_total where TO_CHAR(d_years, 'yyyyMM') = :d_years group by c_user_id having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))"
        Protected Const IN_LINE_VIEW_UNION_BRANCH As String = "(SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch "
        Protected Const NONTAXABLE_CONDITION As String = " ( c_user_id NOT IN (select c_user_id from taxation_total where TO_CHAR(d_years, 'yyyyMM') = :d_years group by c_user_id having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) "
        Protected Const SUMUP_NONTAXABLE_CONDITION As String = " ( (c_user_id, d_years) NOT IN (select c_user_id, d_years from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) "
        Protected Const SUMUP_TAXABLE_CONDITION As String = " ( (c_user_id, d_years) IN (select c_user_id, d_years from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) "
        Protected Const TAXABLE_CONDITION As String = " ( c_user_id IN (select c_user_id from taxation_total where TO_CHAR(d_years, 'yyyyMM') = :d_years group by c_user_id having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) "
        Private Const TAXABLE_WHERE_STRING As String = " (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0) "
    End Class
End Namespace
