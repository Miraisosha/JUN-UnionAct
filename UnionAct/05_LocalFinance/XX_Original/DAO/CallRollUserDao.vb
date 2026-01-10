#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.Framework
Imports UnionAct.NSMDInfo

Namespace DAO.Activity
    Public Class CallRollUserDao
        Inherits AbstractDao
        'Implements ICallRollUserDao
        ' Methods
        Public Function GetBalanceInputDaysMonthList(ByVal IsExecutive As Boolean, ByVal strUserId As String, ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CallRollUserDtlMap
                Dim command As New NpgsqlCommand(String.Concat(New String() { "SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND c_committee_id = :c_committee_id " & "	" & "AND d_years = TO_DATE(:d_date, 'yyyyMMdd') " & "	" & "AND ((s_next_balance_daily_pay IS NOT NULL AND s_next_balance_daily_pay <> 0) " & "		" & "OR (s_next_balance_food_expenses IS NOT NULL AND s_next_balance_food_expenses <> 0)) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id ORDER BY " & "	" & "s_day " }), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_committee_id").Value = "001"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetBalanceInputDaysMonthsList(ByVal IsExecutive As Boolean, ByVal strUserId As String, ByVal strDFrom As String, ByVal strDTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CallRollUserDtlMap
                Dim command As New NpgsqlCommand(String.Concat(New String() { "SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_years >= TO_DATE(:d_from, 'yyyyMMdd') " & "	" & "AND d_years <= TO_DATE(:d_to, 'yyyyMMdd') " & "	" & "AND ((s_next_balance_daily_pay IS NOT NULL AND s_next_balance_daily_pay <> 0) " & "		" & "OR (s_next_balance_food_expenses IS NOT NULL AND s_next_balance_food_expenses <> 0)) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id ORDER BY " & "	" & "s_day " }), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_to", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                command.Parameters.Item("d_to").Value = strDTo
                command.Parameters.Item("c_committee_id").Value = "001"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCallRollAppointDailyPayId(ByVal strDailyPayId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = _
					 "SELECT" & _
						 "call_roll_info.c_user_id," & _
						 "staf_attribute_full_time_view.c_staf_id," & _
						 "staf_attribute_full_time_view.l_name," & _
						 "call_roll_info.user_d_from," & _
						 "call_roll_info.d_years," & _
						 "call_roll_info.s_day," & _
						 "call_roll_info.c_committee_id," & _
						 "committee.l_name AS committee_name," & _
						 "call_roll_info.committee_d_from" & _
					 "FROM" & _
						 "(SELECT" & _
							 "call_roll_user_dtl_AA.c_user_id," & _
							 "MAX(call_roll_user_dtl_AA.user_d_from) AS user_d_from," & _
							 "call_roll_user_dtl_AA.d_years," & _
							 "call_roll_user_dtl_AA.s_day," & _
							 "call_roll_user_dtl_AA.c_committee_id," & _
							 "MAX(call_roll_user_dtl_AA.committee_d_from) AS committee_d_from" & _
						 "FROM" & _
							 "(SELECT" & _
								 "call_roll_user_dtl_A.c_user_id," & _
								 "staf_attribute_full_time_view.d_from AS user_d_from," & _
								 "call_roll_user_dtl_A.d_years," & _
								 "call_roll_user_dtl_A.s_day," & _
								 "call_roll_user_dtl_A.c_committee_id," & _
								 "committee.d_from AS committee_d_from" & _
							 "FROM" & _
								 "(SELECT" & _
									 "c_user_id," & _
									 "d_years," & _
									 "s_day," & _
									 "c_committee_id" & _
								 "FROM" & _
									 "call_roll_user_dtl" & _
								 "WHERE" & _
									 "d_years >= TO_DATE(:d_date, 'yyyyMMdd')" & _
									 "AND (c_menu_seq IS NOT NULL AND c_menu_seq <> 0)" & _
									 "AND c_daily_pay_id = :c_daily_pay_id" & _
								 ") call_roll_user_dtl_A" & _
							 "LEFT OUTER JOIN" & _
								 "staf_attribute_full_time_view" & _
							 "ON" & _
								 "staf_attribute_full_time_view.c_user_id = call_roll_user_dtl_A.c_user_id" & _
							 "LEFT OUTER JOIN" & _
								 "committee" & _
							 "ON" & _
								 "committee.c_ksh = :c_ksh" & _
								 "AND committee.c_committee_id = call_roll_user_dtl_A.c_committee_id" & _
							 ") call_roll_user_dtl_AA" & _
						 "WHERE" & _
							 "call_roll_user_dtl_AA.user_d_from <= TO_CHAR(call_roll_user_dtl_AA.s_day, 'yyyyMMdd')" & _
							 "AND call_roll_user_dtl_AA.committee_d_from <= TO_CHAR(call_roll_user_dtl_AA.s_day, 'yyyyMMdd')" & _
						 "GROUP BY" & _
							 "call_roll_user_dtl_AA.c_user_id," & _
							 "call_roll_user_dtl_AA.d_years," & _
							 "call_roll_user_dtl_AA.s_day," & _
							 "call_roll_user_dtl_AA.c_committee_id" & _
						 ") call_roll_info" & _
					 "LEFT OUTER JOIN" & _
						 "staf_attribute_full_time_view" & _
					 "ON" & _
						 "call_roll_info.c_user_id = staf_attribute_full_time_view.c_user_id" & _
						 "AND call_roll_info.user_d_from = staf_attribute_full_time_view.d_from" & _
					 "LEFT OUTER JOIN" & _
						 "committee" & _
					 "ON" & _
						 "call_roll_info.c_committee_id = committee.c_committee_id" & _
						 "AND call_roll_info.committee_d_from = committee.d_from" & _
					 "ORDER BY" & _
						 "call_roll_info.c_committee_id," & _
						 "call_roll_info.d_years," & _
						 "TO_NUMBER(staf_attribute_full_time_view.c_staf_id, '9999999999')," & _
						 "call_roll_info.s_day;"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_daily_pay_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_daily_pay_id").Value = strDailyPayId
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCallRollAppointMenuSeq(ByVal nMenuSeq As Integer, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = _
					 "SELECT" & _
						 "call_roll_info.c_user_id," & _
						 "staf_attribute_full_time_view.c_staf_id," & _
						 "staf_attribute_full_time_view.l_name," & _
						 "call_roll_info.user_d_from," & _
						 "call_roll_info.d_years," & _
						 "call_roll_info.s_day," & _
						 "call_roll_info.c_committee_id," & _
						 "committee.l_name AS committee_name," & _
						 "call_roll_info.committee_d_from" & _
					 "FROM" & _
						 "(SELECT" & _
							 "call_roll_user_dtl_AA.c_user_id," & _
							 "MAX(call_roll_user_dtl_AA.user_d_from) AS user_d_from," & _
							 "call_roll_user_dtl_AA.d_years," & _
							 "call_roll_user_dtl_AA.s_day," & _
							 "call_roll_user_dtl_AA.c_committee_id," & _
							 "MAX(call_roll_user_dtl_AA.committee_d_from) AS committee_d_from" & _
						 "FROM" & _
							 "(SELECT" & _
								 "call_roll_user_dtl_A.c_user_id," & _
								 "staf_attribute_full_time_view.d_from AS user_d_from," & _
								 "call_roll_user_dtl_A.d_years," & _
								 "call_roll_user_dtl_A.s_day," & _
								 "call_roll_user_dtl_A.c_committee_id," & _
								 "committee.d_from AS committee_d_from" & _
							 "FROM" & _
								 "(SELECT" & _
									 "c_user_id," & _
									 "d_years," & _
									 "s_day," & _
									 "c_committee_id" & _
								 "FROM" & _
									 "call_roll_user_dtl" & _
								 "WHERE" & _
									 "d_years >= TO_DATE(:d_date, 'yyyyMMdd')" & _
									 "AND (c_menu_seq IS NOT NULL AND c_menu_seq = :c_menu_seq)" & _
								 ") call_roll_user_dtl_A" & _
							 "LEFT OUTER JOIN" & _
								 "staf_attribute_full_time_view" & _
							 "ON" & _
								 "staf_attribute_full_time_view.c_user_id = call_roll_user_dtl_A.c_user_id" & _
							 "LEFT OUTER JOIN" & _
								 "committee" & _
							 "ON" & _
								 "committee.c_ksh = :c_ksh" & _
								 "AND committee.c_committee_id = call_roll_user_dtl_A.c_committee_id" & _
							 ") call_roll_user_dtl_AA" & _
						 "WHERE" & _
							 "call_roll_user_dtl_AA.user_d_from <= TO_CHAR(call_roll_user_dtl_AA.s_day, 'yyyyMMdd')" & _
							 "AND call_roll_user_dtl_AA.committee_d_from <= TO_CHAR(call_roll_user_dtl_AA.s_day, 'yyyyMMdd')" & _
						 "GROUP BY" & _
							 "call_roll_user_dtl_AA.c_user_id," & _
							 "call_roll_user_dtl_AA.d_years," & _
							 "call_roll_user_dtl_AA.s_day," & _
							 "call_roll_user_dtl_AA.c_committee_id" & _
						 ") call_roll_info" & _
					 "LEFT OUTER JOIN" & _
						 "staf_attribute_full_time_view" & _
					 "ON" & _
						 "call_roll_info.c_user_id = staf_attribute_full_time_view.c_user_id" & _
						 "AND call_roll_info.user_d_from = staf_attribute_full_time_view.d_from" & _
					 "LEFT OUTER JOIN" & _
						 "committee" & _
					 "ON" & _
						 "call_roll_info.c_committee_id = committee.c_committee_id" & _
						 "AND call_roll_info.committee_d_from = committee.d_from" & _
					 "ORDER BY" & _
						 "call_roll_info.c_committee_id," & _
						 "call_roll_info.d_years," & _
						 "TO_NUMBER(staf_attribute_full_time_view.c_staf_id, '9999999999')," & _
						 "call_roll_info.s_day;"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_menu_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_menu_seq").Value = nMenuSeq
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCallRollOldDate(ByVal strDailyPayKind As String) As String
            Dim str3 As String
            Try 
                Dim command As NpgsqlCommand
                Dim str As String = ""
                Dim str4 As String = strDailyPayKind
                If (str4 Is Nothing) Then
                    goto Label_02ED
                End If
                If Not (str4 = "01") Then
                    If (str4 = "02") Then
                        goto Label_0187
                    End If
                    If (str4 = "03") Then
                        goto Label_02E5
                    End If
                    goto Label_02ED
                End If
                str = " c_committee_id <> '001'"
                If ((UnionConst.DailyPayKind02Id.Length <> 0) AndAlso (UnionConst.DailyPayKind02Seq.Length <> 0)) Then
                    str = (((str & " AND (") & " NOT(c_committee_id = '" & UnionConst.DailyPayKind02Id(0) & "' ") & " AND (s_committee_seq = '" & UnionConst.DailyPayKind02Seq(0)(0).ToString & "' ")
                    Dim i As Integer
                    For i = 1 To UnionConst.DailyPayKind02Seq(0).Length - 1
                        str = (str & " OR s_committee_seq = '" & UnionConst.DailyPayKind02Seq(0)(i).ToString & "' ")
                    Next i
                    str = (str & "))) ")
                    Dim j As Integer
                    For j = 1 To UnionConst.DailyPayKind02Id.Length - 1
                        str = ((str & " AND (NOT(c_committee_id = '" & UnionConst.DailyPayKind02Id(j) & "' ") & " AND (s_committee_seq = '" & UnionConst.DailyPayKind02Seq(j)(0).ToString & "' ")
                        Dim k As Integer
                        For k = 1 To UnionConst.DailyPayKind02Seq(j).Length - 1
                            str = (str & " OR s_committee_seq = '" & UnionConst.DailyPayKind02Seq(j)(k).ToString & "' ")
                        Next k
                        str = (str & "))) ")
                    Next j
                End If
                goto Label_02F5
            Label_0187:
                str = " c_committee_id <> '001'"
                If ((UnionConst.DailyPayKind02Id.Length <> 0) AndAlso (UnionConst.DailyPayKind02Seq.Length <> 0)) Then
                    str = (((str & " AND ((") & " (c_committee_id = '" & UnionConst.DailyPayKind02Id(0) & "' ") & " AND (s_committee_seq = '" & UnionConst.DailyPayKind02Seq(0)(0).ToString & "' ")
                    Dim m As Integer
                    For m = 1 To UnionConst.DailyPayKind02Seq(0).Length - 1
                        str = (str & " OR s_committee_seq = '" & UnionConst.DailyPayKind02Seq(0)(m).ToString & "' ")
                    Next m
                    str = (str & "))) ")
                    Dim n As Integer
                    For n = 1 To UnionConst.DailyPayKind02Id.Length - 1
                        str = ((str & " OR ((c_committee_id = '" & UnionConst.DailyPayKind02Id(n) & "' ") & " AND (s_committee_seq = '" & UnionConst.DailyPayKind02Seq(n)(0).ToString & "' ")
                        Dim num6 As Integer
                        For num6 = 1 To UnionConst.DailyPayKind02Seq(n).Length - 1
                            str = (str & " OR s_committee_seq = '" & UnionConst.DailyPayKind02Seq(n)(num6).ToString & "' ")
                        Next num6
                        str = (str & "))) ")
                    Next n
                    str = (str & ") ")
                End If
                goto Label_02F5
            Label_02E5:
                str = " c_committee_id = '001'"
                goto Label_02F5
            Label_02ED:
                Return Nothing
            Label_02F5:
                'command = New NpgsqlCommand(("SELECT " & "	" & "TO_CHAR(MIN(d_years), 'yyyyMMdd') AS min FROM " & "	" & "call_roll_user_dtl WHERE " & str), MyBase.GetNpgsqlConnection)
                command = New NpgsqlCommand(("SELECT " & "	" & "Format(MIN(d_years), 'yyyyMMdd') AS minyear FROM " & "	" & "call_roll_user_dtl WHERE " & str), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader)
                If table.Rows.Item(0).Item("minyear").ToString.Equals("") Then
                    Return Nothing
                End If
                str3 = table.Rows.Item(0).Item("minyear").ToString
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return str3
        End Function

        Public Function GetCommitteeCallRollUserDtl(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str As String = ""
                If Not String.IsNullOrEmpty(strCommitteeSeq) Then
                    str = " AND call_roll_user_dtl.s_committee_seq = :s_committee_seq "
                Else
                    strCommitteeSeq = "-1"
                End If
                Dim command As New NpgsqlCommand( _
					 "SELECT" & _
						 "call_roll_user_dtl_AA.c_user_id," & _
						 "staf_attribute_view.c_staf_id AS " & "é–àıî‘çÜ" & "," & _
						 "TO_NUMBER(staf_attribute_view.c_staf_id, '9999999999')," & _
						 "staf_attribute_view.l_name AS " & "ñºëO" & "," & _
						 "call_roll_user_dtl_AA.s_day" & _
					 "FROM" & _
						 "(SELECT" & _
							 "call_roll_user_dtl_A.c_user_id," & _
							 "call_roll_user_dtl_A.s_day," & _
							 "MAX(staf_attribute_full_time_view.d_from) AS d_from" & _
						 "FROM" & _
							 "(SELECT" & _
								 "call_roll_user_dtl.c_user_id," & _
								 "call_roll_user_dtl.s_day" & _
							 "FROM" & _
								 "call_roll_user," & _
								 "call_roll_user_dtl" & _
							 "WHERE" & _
								 "call_roll_user.c_user_id = call_roll_user_dtl.c_user_id" & _
								 "AND call_roll_user.d_years = call_roll_user_dtl.d_years" & _
								 "AND call_roll_user_dtl.c_committee_id = :c_committee_id" & _
								 str & _
								 "AND call_roll_user_dtl.d_years >= TO_DATE(:d_years, 'yyyyMMdd')" & _
								 "AND call_roll_user_dtl.c_menu_seq IS NOT NULL" & _
							 ") call_roll_user_dtl_A" & _
						 "LEFT OUTER JOIN" & _
							 "staf_attribute_full_time_view" & _
						 "ON" & _
							 "staf_attribute_full_time_view.c_user_id = call_roll_user_dtl_A.c_user_id" & _
							 "AND staf_attribute_full_time_view.d_from <= TO_CHAR(call_roll_user_dtl_A.s_day, 'yyyyMMdd')" & _
						 "GROUP BY" & _
							 "call_roll_user_dtl_A.c_user_id," & _
							 "call_roll_user_dtl_A.s_day" & _
						 ") call_roll_user_dtl_AA" & _
					 "LEFT OUTER JOIN" & _
						 "staf_attribute_full_time_view staf_attribute_view" & _
					 "ON" & _
						 "call_roll_user_dtl_AA.c_user_id = staf_attribute_view.c_user_id" & _
						 "AND call_roll_user_dtl_AA.d_from = staf_attribute_view.d_from" & _
					 "ORDER BY" & _
						 "TO_NUMBER(staf_attribute_view.c_staf_id, '9999999999')," & _
						 "call_roll_user_dtl_AA.s_day; ", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = Integer.Parse(strCommitteeSeq)
                command.Parameters.Item("d_years").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                Dim table As DataTable = MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader)
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetInputDaysMonthList(ByVal IsExecutive As Boolean, ByVal strUserId As String, ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CallRollUserDtlMap
                'Dim command As New NpgsqlCommand(String.Concat(New String() { "SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND c_committee_id = :c_committee_id " & "	" & "AND d_years = TO_DATE(:d_date, 'yyyyMMdd') " & "	" & "AND (s_daily_pay <> 0 OR s_food_expenses <> 0) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id ORDER BY " & "	" & "s_day " }), MyBase.GetNpgsqlConnection)
                Dim command As New NpgsqlCommand(String.Concat(New String() {"SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND c_committee_id = :c_committee_id " & "	" & "AND d_years = CONVERT(DATE,:d_date, 112) " & "	" & "AND (s_daily_pay <> 0 OR s_food_expenses <> 0) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id ORDER BY " & "	" & "s_day "}), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_committee_id").Value = "001"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetInputDaysMonthsList(ByVal IsExecutive As Boolean, ByVal strUserId As String, ByVal strDFrom As String, ByVal strDTo As String, ByVal strDailyPayKind As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim map As New CallRollUserDtlMap
                'Dim command As New NpgsqlCommand(String.Concat(New String() { "SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_years >= TO_DATE(:d_from, 'yyyyMMdd') " & "	" & "AND d_years <= TO_DATE(:d_to, 'yyyyMMdd') " & "	" & "AND (s_daily_pay <> 0 OR s_food_expenses <> 0) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id ORDER BY " & "	" & "s_day " }), MyBase.GetNpgsqlConnection)
                Dim command As New NpgsqlCommand(String.Concat(New String() {"SELECT ", map.ToPhysicalString(""), " FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_years >= CONVERT(DATE,:d_from,112) " & "	" & "AND d_years <= CONVERT(DATE,:d_to,112) " & "	" & "AND (s_daily_pay <> 0 OR s_food_expenses <> 0) " & "	" & "AND c_committee_id ", If(IsExecutive, "=", "<>"), " :c_committee_id AND k_daily_pay_kind=:k_daily_pay_kind ORDER BY " & "	" & "s_day "}), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_to", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                command.Parameters.Item("d_to").Value = strDTo
                command.Parameters.Item("c_committee_id").Value = "001"
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
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

        Public Function GetPersonalAttendance(ByVal strUserId As String, ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CallRollUserDtlMap
                Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM call_roll_user_dtl WHERE c_user_id = :c_user_id AND d_years = TO_DATE(:d_date, 'yyyyMMdd') AND c_committee_id = :c_committee_id ORDER BY s_day "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function IsExistForCommittee(ByVal strCommitteeId As String, ByVal strDate As String) As Boolean
            'Public Function IsExistForCommittee(ByVal objLoginSession As LoginSession, ByVal strCommitteeId As String, ByVal strDate As String) As Boolean
            Dim flag As Boolean
            Try
                Dim cmdText As String = "SELECT " & "	" & "COUNT(call_roll_user.c_user_id) AS count FROM " & "	" & "call_roll_user, " & "	" & "(SELECT " & "		" & "c_user_id, " & "		" & "d_years " & "	" & "FROM " & "		" & "call_roll_user_dtl " & "	" & "WHERE " & "		" & "d_years = TO_DATE(:d_date, 'yyyyMMdd') " & "		" & "AND c_committee_id = :c_committee_id " & "	" & ") call_roll_user_dtl_A WHERE " & "	" & "call_roll_user.c_user_id = call_roll_user_dtl_A.c_user_id " & "	" & "AND call_roll_user.d_years = call_roll_user_dtl_A.d_years " & "	" & "AND call_roll_user.c_period_id = :c_period_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_period_id").Value = Integer.Parse(MDLoginInfo.PeriodName) 'TODO
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return False
                End If
                flag = (CLng(MyBase.CreateSomeDataSet("call_roll_user", dReader).Rows.Item(0).Item(0)) > 0)
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
            Return flag
        End Function

        Public Function IsExistForCommittee(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String) As Boolean
            Dim flag As Boolean
            Try
                Dim cmdText As String = "SELECT " & "	" & "COUNT(c_user_id) FROM " & "	" & "call_roll_user_dtl WHERE " & "	" & "d_years = TO_DATE(:d_date, 'yyyyMMdd') " & "	" & "AND c_committee_id = :c_committee_id " & "	" & "AND s_committee_seq = :s_committee_seq "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = Integer.Parse(strCommitteeSeq)
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return False
                End If
                flag = (CLng(MyBase.CreateSomeDataSet("call_roll_user", dReader).Rows.Item(0).Item(0)) > 0)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return flag
        End Function

        Public Function IsExistForDailyPayKind(ByVal strDailyPayKind As String, ByVal strDate As String) As DataSet
            'Public Function IsExistForDailyPayKind(ByVal objLoginSession As LoginSession, ByVal strDailyPayKind As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim str2 As String
                Dim str As String = ""
                Dim str3 As String = strDailyPayKind
                If (str3 Is Nothing) Then
                    GoTo Label_00AE
                End If
                If Not (str3 = "01") Then
                    If (str3 = "02") Then
                        GoTo Label_0046
                    End If
                    If (str3 = "03") Then
                        GoTo Label_00A6
                    End If
                    If (str3 = "04") Then
                        GoTo Label_00A7
                    End If
                    GoTo Label_00AE
                End If
                str = " committee_list_dtl_A.c_committee_id <> '001' AND "
                GoTo Label_00C9
Label_0046:
                If (UnionConst.DailyPayKind02Id.Length = 0) Then
                    Return Nothing
                End If
                str = (" ( committee_list_dtl_A.c_committee_id = '" & UnionConst.DailyPayKind02Id(0) & "' ")
                Dim i As Integer
                For i = 1 To UnionConst.DailyPayKind02Id.Length - 1
                    str = (str & " OR committee_list_dtl_A.c_committee_id = '" & UnionConst.DailyPayKind02Id(i) & "' ")
                Next i
                str = (str & " ) AND ")
                GoTo Label_00C9
Label_00A6:
                str = " committee_list_dtl_A.c_committee_id = '001' AND "
                GoTo Label_00C9
Label_00A7:
                str = " committee_list_dtl_A.c_committee_id = 'DGM' AND "
                GoTo Label_00C9
Label_00AE:
                Throw New SysUnionException(MethodBase.GetCurrentMethod, New ArgumentException, "DE0001", New String(0 - 1) {})
Label_00C9:
                str2 = _
				 "SELECT " & _
					 "committee_list_dtl_B.c_committee_id AS c_committee_id, " & _
					 "committee.l_name AS committee_name " & _
				 "FROM " & _
					 "(" & _
					 "SELECT committee_list_dtl_call_roll.c_committee_id FROM " & _
						 "(" & _
						 "SELECT * FROM " & _
							 "(" & _
							 "SELECT" & _
							 " VIEW_1.* " & _
							 "FROM" & _
							 " committee_list_dtl_term_view VIEW_1," & _
							 " (" & _	
								 "SELECT" & _
								 " c_committee_list, max(committee_list_dtl.d_from) AS d_from " & _
								 "FROM" & _
								 " committee_list_dtl " & _
								 "WHERE" & _
								 " d_from <= :d_date" & _
								 "GROUP BY" & _
								 " c_committee_list" & _
							 " ) COM_1 " & _
							 "WHERE" & _
								 " VIEW_1.c_committee_list = COM_1.c_committee_list" & _
							 " AND VIEW_1.c_ksh = :c_ksh " & _
							 " AND VIEW_1.d_from = COM_1.d_from" & _
							 " AND VIEW_1.master_d_from <= :d_date " & _
							 " AND VIEW_1.master_d_to >= :d_date " & _
							 " AND VIEW_1.d_term_end >= :d_date" & _
							 ") committee_list_dtl_A " & _
						 "WHERE "
                Dim command As New NpgsqlCommand((str2 & str &
                         "  committee_list_dtl_A.c_user_id NOT IN " &
                             "(" &
                             "SELECT" &
                             " IIF(IsNull(call_roll_user.c_user_id),'XXXXXXX',call_roll_user.c_user_id) AS c_user_id " &
                             "FROM call_roll_user " &
                             "WHERE call_roll_user.d_years = CONVERT(DATE,:d_date,112) " &
                             "GROUP BY call_roll_user.c_user_id" &
                             ")" &
                         ") committee_list_dtl_call_roll " &
                     "GROUP BY committee_list_dtl_call_roll.c_committee_id" &
                     ") committee_list_dtl_B " &
                     "LEFT OUTER JOIN committee ON " &
                     "( " & "	" & "committee.c_committee_id = committee_list_dtl_B.c_committee_id " &
                     "AND committee.c_ksh  = :c_ksh  " &
                     "AND committee.d_from <= :d_date " &
                     "AND" & "	" & "committee.d_to >= :d_date)") _
                , MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("call_roll_user_dtl", dReader))
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

        Public Function SelectCountCallRollUserData(ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strDate As String, ByVal strUserId As String) As Integer
            Dim num As Integer
            Try
                Dim cmdText As String = "select count(a.c_user_id) from call_roll_user a, call_roll_user_dtl b where a.c_user_id      = b.c_user_id       and a.d_years        = b.d_years         and a.c_period_id    = :c_period_id      and b.c_committee_id = :c_committee_id   and b.d_years       >= to_date(:d_years, 'yyyyMMdd') "
                If Not strUserId.Equals("") Then
                    cmdText = (cmdText & "  and b.c_user_id = :c_user_id   and b.c_menu_seq is not null ")
                End If
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_years").Value = strDate
                command.Parameters.Item("c_user_id").Value = strUserId
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

        Public Function SelectCountExceptBranchOffice(ByVal strDate As String, ByVal strUserId As String) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = _
					 "SELECT" & _
					 " COUNT(call_roll_user_dtl.c_user_id)" & _
					 "FROM" & _
						 "call_roll_user_dtl" & "	" & "--" & "èoåáïÎä«óù" & _
					 "WHERE" & _
						 "c_committee_id" & "	" & "<> :executive_committee_id " & _
					 " AND" & "	" & "(NOT(c_committee_id = :branch_tky_committee_id AND s_committee_seq = 1) " & _
					 " AND" & "	" & " NOT(c_committee_id = :branch_tky_committee_id AND s_committee_seq = 2) " & _
					 " AND" & "	" & " NOT(c_committee_id = :branch_tky_committee_id AND s_committee_seq = 3) )" & _
					 " AND" & "	" & "(NOT(c_committee_id = :branch_osk_committee_id AND s_committee_seq = 1) " & _
					 " AND" & "	" & " NOT(c_committee_id = :branch_osk_committee_id AND s_committee_seq = 2) " & _
					 " AND" & "	" & " NOT(c_committee_id = :branch_osk_committee_id AND s_committee_seq = 3) ) " & _
					 " AND     d_years >= TO_DATE(:d_years, 'yyyyMMdd')" & _
					 " AND     c_user_id = :c_user_id" & _
					 " AND     c_menu_seq IS NOT NULL;"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("executive_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("branch_tky_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("branch_osk_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Item("executive_committee_id").Value = "001"
                command.Parameters.Item("branch_tky_committee_id").Value = "019"
                command.Parameters.Item("branch_osk_committee_id").Value = "029"
                command.Parameters.Item("d_years").Value = strDate
                command.Parameters.Item("c_user_id").Value = strUserId
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

        Public Function SelectCountExceptExecutiveCommittee(ByVal strPeriodId As String, ByVal strDate As String, ByVal strUserId As String) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = _
					 "SELECT" & _
						 "COUNT(call_roll_user.c_user_id)" & _
					 "FROM" & _
						 "call_roll_user," & _
						 "call_roll_user_dtl" & _
					 "WHERE" & _
						 "call_roll_user.c_user_id = call_roll_user_dtl.c_user_id" & _
						 "AND call_roll_user.d_years = call_roll_user_dtl.d_years" & _
						 "AND call_roll_user_dtl.c_committee_id <> :executive_committee_id" & _
						 "AND call_roll_user_dtl.d_years >= TO_DATE(:d_years, 'yyyyMMdd')" & _
						 "AND call_roll_user_dtl.c_user_id = :c_user_id" & _
						 "AND call_roll_user_dtl.c_menu_seq IS NOT NULL;"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("executive_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("executive_committee_id").Value = "001"
                command.Parameters.Item("d_years").Value = strDate
                command.Parameters.Item("c_user_id").Value = strUserId
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

    End Class
End Namespace
