Imports UnionAct.NSMDInfo
Imports UnionAct.DAO.Master
Imports UnionAct.Framework

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports UnionAct.DAO
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.UnionException
Imports System
Imports System.Data
Imports System.Reflection

Namespace DAO.FinancialAffairs.DailyAllowance
    Public Class DailyPayCloseDao
        Inherits AbstractDao
        ' ログ出力オブジェクト
        Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        'Implements IDailyPayCloseDao
        ' Methods
        'Public Sub CalcDailyAllowanceClose(ByVal objLoginSession As LoginSession, ByVal strDailyPayKind As String, ByVal strCloseDate As String)
        '    Try
        '        Dim ds As DataSet = Me.GetPrev2DailyPayClose(strDailyPayKind)
        '        Dim strArray As String() = New String() {" NULL, ", " :prev_daily_pay_close, "}
        '        Dim index As Integer = If((ds Is Nothing), 0, 1)
        '        Dim command As New NpgsqlCommand(("DailyAllowanceClose( " & "  " & ":c_ksh, " & "  " & ":d_daily_pay_close, " & strArray(index) & "    " & ":k_daily_pay_kind, " & "    " & ":c_user_id) "), MyBase.GetNpgsqlConnection) With { _
        '            .CommandType = CommandType.StoredProcedure _
        '        }
        '        command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
        '        command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
        '        command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
        '        If (Not ds Is Nothing) Then
        '            command.Parameters.Item("prev_daily_pay_close").Value = ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("max").ToString
        '        End If
        '        command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
        '        command.Parameters.Item("c_user_id").Value = MDLoginInfo.UserId
        '        If (CInt(command.ExecuteScalar) < 0) Then
        '            Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0 - 1) {})
        '        End If
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
        '    End Try
        'End Sub

        ' 日当計算
        Public Sub CalcDailyAllowanceClose(ByVal strDailyPayKind As String, ByVal strCloseDate As String)
            ' 前回締日
            Dim ds As DataSet = Me.GetPrev2DailyPayClose(strDailyPayKind)
            Dim strPrevCloseDate As String
            If Not ds Is Nothing Then
                strPrevCloseDate = ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("max").ToString
            Else
                strPrevCloseDate = GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
            End If
            ' 前々回締日
            Dim strPrev2CloseDate As String = ""
            If strPrevCloseDate <> "" Then
                strPrev2CloseDate = GetPrevDailyPayClose(strDailyPayKind, strPrevCloseDate)
            End If
            If strPrev2CloseDate = "" Then
                strPrev2CloseDate = Format(PublicCommand.GetMonthEnd(MDLoginInfo.PeriodFrom, -1), "yyyyMMdd")
            End If
            Dim strUserId As String = MDLoginInfo.UserId
            Dim strKeyDate As String = MDMasterCommon.GetKeyDate

            ' トランザクション開始
            Dim objTran As NpgsqlTransaction = MyBase.GetNpgsqlConnection.BeginTransaction
            Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection, objTran)
            Dim strSql As String = ""
            SetExecParam(command, strDailyPayKind, strPrevCloseDate, strCloseDate, MDLoginInfo.Ksh)

            Try
                ' 所属委員会別に日当明細(call_roll_user_dtl)の日当計算区分を更新
                UpdateDailyPayKind(strDailyPayKind, command, strPrevCloseDate, strCloseDate)

                ' 日当マスタよりデータ取得
                Dim htMasterData As Hashtable = GetDailyPayData(strDailyPayKind, command, strPrevCloseDate, strCloseDate)

                ' 当期（前回締め日翌日から今回締め日）の日当明細を更新（日当金額）
                UpdateDetailDailyPay(strDailyPayKind, command, htMasterData, strPrevCloseDate, strCloseDate)

                ' 役員マスタ、中央執行昼食費マスタよりデータ取得
                htMasterData.Clear()
                htMasterData = GetOfficerAndLunchPayData(strDailyPayKind, command, strPrevCloseDate, strCloseDate)

                ' 当期（前回締め日翌日から今回締め日）の日当明細を更新（役員手当金額？、昼食日金額）
                UpdateDetailLunchPay(strDailyPayKind, command, htMasterData, strPrevCloseDate, strCloseDate)

                ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新
                ' （前回締日以降に更新された日当明細の次期差分額更新）
                UpdatePrevDetailNextPay(strDailyPayKind, command, htMasterData, strPrev2CloseDate, strPrevCloseDate)

                ' DGM日当額の控除
                SubtractDgmPay(strDailyPayKind, command, strCloseDate)

                ' 日当明細を集計し、日当計算詳細 (call_roll_user)を更新/追加
                UpdateInsertCallRollUser(strDailyPayKind, command, strKeyDate, strUserId, strPrevCloseDate, strCloseDate)

                ' 日当明細を集計し、支部別日当合計 (daily_pay_close_dtl)を更新
                UpdateDailyPayCloseDtl(strDailyPayKind, command, strKeyDate, strUserId, strPrevCloseDate, strCloseDate)

                ' 支部別日当合計を集計し、締め日一覧 (daily_pay_close)を更新
                UpdateDailyPayClose(strDailyPayKind, command, strKeyDate, strUserId, strPrevCloseDate, strCloseDate)

                ' コミット
                objTran.Commit()
            Catch ex As Exception
                ' ロールバック
                objTran.Rollback()
                Throw ex
                'MsgBox(ex.Message)
            End Try
        End Sub

        ' 実行時パラメータ設定
        Private Sub SetExecParam(ByVal objCommand As NpgsqlCommand, ByVal strDailyPayKind As String, ByVal strPrevCloseDate As String, ByVal strCloseDate As String, ByVal strKsh As String)
            objCommand.Parameters.Clear()
            objCommand.SetSql("UPDATE daily_pay_calc_temp SET d_daily_pay_close=:d_daily_pay_close, d_prev_daily_pay_close=:d_prev_daily_pay_close,c_ksh=:c_ksh,k_daily_pay_kind=:k_daily_pay_kind")
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            objCommand.Parameters.Item("d_prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("c_ksh").Value = strKsh
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind

            If objCommand.ExecuteNonQuery() = 0 Then
                objCommand.Parameters.Clear()
                objCommand.SetSql("INSERT INTO daily_pay_calc_temp (c_user_id, d_daily_pay_close, d_prev_daily_pay_close, c_ksh, k_daily_pay_kind) VALUES(:c_user_id,:d_daily_pay_close, :d_prev_daily_pay_close, :c_ksh,:k_daily_pay_kind)")
                objCommand.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_prev_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Item("c_user_id").Value = MDLoginInfo.UserId
                objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                objCommand.Parameters.Item("d_prev_daily_pay_close").Value = strPrevCloseDate
                objCommand.Parameters.Item("c_ksh").Value = strKsh
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.ExecuteNonQuery()
            End If

        End Sub

        ' 所属委員会別に日当明細の計算区分、締日を更新
        Private Sub UpdateDailyPayKind(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)
            Dim strSql As String = ""
            Select Case strDailyPayKind
                Case "01"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE (c_committee_id <> :exective_committe_id AND " &
                                       "(c_committee_id <> :brance_commite_id019 OR (c_committee_id = :brance_commite_id019 AND s_committee_seq > '3')) AND " &
                                       "(c_committee_id <> :brance_commite_id029 OR (c_committee_id = :brance_commite_id029 AND s_committee_seq > '3')) AND " &
                                       " c_committee_id <> :brance_commite_idDGM) AND " &
                                       ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                                       "Format(d_years, 'yyyyMMdd') <= :d_years "

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id019", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id029", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_idDGM", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "001"
                    objCommand.Parameters.Item("brance_commite_id019").Value = "019"
                    objCommand.Parameters.Item("brance_commite_id029").Value = "029"
                    objCommand.Parameters.Item("brance_commite_idDGM").Value = "DGM"
                    objCommand.Parameters.Item("d_years").Value = strCloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                    objCommand.ExecuteNonQuery()

                Case "02"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE (c_committee_id = :brance_commite_id019 OR " &
                                       "c_committee_id = :brance_commite_id029) AND " &
                                       "(s_committee_seq = '1' OR s_committee_seq = '2' OR s_committee_seq = '3') AND " &
                                       ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                                       "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "


                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id019", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id029", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("brance_commite_id019").Value = "019"
                    objCommand.Parameters.Item("brance_commite_id029").Value = "029"
                    objCommand.Parameters.Item("d_years").Value = strCloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                    objCommand.ExecuteNonQuery()

                Case "03"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE c_committee_id = :exective_committe_id AND " &
                                       ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                                       "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "001"
                    objCommand.Parameters.Item("d_years").Value = strCloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                    objCommand.ExecuteNonQuery()

                Case "04"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_food_expenses = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE c_committee_id = :exective_committe_id AND " &
                                       ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                                       "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "DGM"
                    objCommand.Parameters.Item("d_years").Value = strCloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                    objCommand.ExecuteNonQuery()

                Case Else
                    Return
            End Select
        End Sub

        ' 所属委員会別に日当明細の計算区分、締日を更新
        Private Sub UpdatePrevDailyPayKind(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrev2CloseDate As String, ByVal strPrevCloseDate As String, ByVal strUpdateDate As String)
            Dim strSql As String = ""
            Select Case strDailyPayKind
                Case "01"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE (c_committee_id <> :exective_committe_id AND " &
                                       "(c_committee_id <> :brance_commite_id019 OR (c_committee_id = :brance_commite_id019 AND s_committee_seq > '3')) AND " &
                                       "(c_committee_id <> :brance_commite_id029 OR (c_committee_id = :brance_commite_id029 AND s_committee_seq > '3')) AND " &
                                       " c_committee_id <> :brance_commite_idDGM) AND k_daily_pay_kind IS NULL " &
                                       "AND :prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                                       "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id019", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id029", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_idDGM", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "001"
                    objCommand.Parameters.Item("brance_commite_id019").Value = "019"
                    objCommand.Parameters.Item("brance_commite_id029").Value = "029"
                    objCommand.Parameters.Item("brance_commite_idDGM").Value = "DGM"
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrev2CloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strPrevCloseDate
                    objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                    objCommand.ExecuteNonQuery()

                Case "02"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE (c_committee_id = :brance_commite_id019 OR " &
                                       "c_committee_id = :brance_commite_id029) AND " &
                                       "(s_committee_seq = '1' OR s_committee_seq = '2' OR s_committee_seq = '3') AND " &
                                       "k_daily_pay_kind IS NULL " &
                                       "AND :prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                                       "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"


                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id019", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("brance_commite_id029", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("brance_commite_id019").Value = "019"
                    objCommand.Parameters.Item("brance_commite_id029").Value = "029"
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrev2CloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strPrevCloseDate
                    objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                    objCommand.ExecuteNonQuery()

                Case "03"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_daily_pay = 0, s_food_expenses = 0, s_next_balance_daily_pay = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE c_committee_id = :exective_committe_id AND " &
                                       "k_daily_pay_kind IS NULL " &
                                       "AND :prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                                       "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "001"
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrev2CloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strPrevCloseDate
                    objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                    objCommand.ExecuteNonQuery()

                Case "04"
                    strSql = "UPDATE call_roll_user_dtl SET k_daily_pay_kind = :k_daily_pay_kind, " &
                                 "d_daily_pay_close = CONVERT(DATE,:d_daily_pay_close,112)," &
                                 "s_food_expenses = 0, s_next_balance_food_expenses = 0 " &
                                 "WHERE c_committee_id = :exective_committe_id AND " &
                                       "k_daily_pay_kind IS NULL " &
                                       "AND :prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                                       "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"

                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSql)
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("exective_committe_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))

                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("exective_committe_id").Value = "DGM"
                    objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrev2CloseDate
                    objCommand.Parameters.Item("d_daily_pay_close").Value = strPrevCloseDate
                    objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                    objCommand.ExecuteNonQuery()

                Case Else
                    Return
            End Select
        End Sub

        ' DGM日当額の控除
        Private Sub SubtractDgmPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strCloseDate As String)
            Dim strSql As String
            Dim strSqlUpd As String
            strSql = "SELECT DTL.c_user_id, DTL.d_years, DTL.s_day, DTL.c_committee_id, DTL.s_committee_seq," &
                     " DTL.s_daily_pay - DGM.s_daily_pay AS s_daily_pay FROM call_roll_user_dtl AS DTL," &
                     "(SELECT c_user_id, d_years, s_day, c_committee_id, d_daily_pay_close, s_daily_pay FROM call_roll_user_dtl " &
                     "WHERE call_roll_user_dtl.c_committee_id='DGM' And Format(d_daily_pay_close,'yyyyMMdd')=:d_daily_pay_close)  AS DGM " &
                     "WHERE DTL.c_user_id=DGM.c_user_id AND DTL.s_day=DGM.s_day AND DTL.d_daily_pay_close=DGM.d_daily_pay_close AND " &
                     "DTL.c_committee_id <>DGM.c_committee_id AND DTL.s_daily_pay > DGM.s_daily_pay;"
            strSqlUpd = "UPDATE call_roll_user_dtl SET s_daily_pay=:s_daily_pay " &
                        "WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years) AND s_day=CONVERT(DATE,:s_day) AND c_committee_id=:c_committee_id AND s_committee_seq=:s_committee_seq"

            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            Dim dReader As NpgsqlDataReader = objCommand.ExecuteReader

            For Each Row As DataRow In dReader.getTable().Rows()
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSqlUpd)
                objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_day", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                objCommand.Parameters.Item("s_daily_pay").Value = Row.Item(5)
                objCommand.Parameters.Item("c_user_id").Value = Row.Item(0)
                objCommand.Parameters.Item("d_years").Value = Format(Row.Item(1), "yyyy/MM/dd")
                objCommand.Parameters.Item("s_day").Value = Format(Row.Item(2), "yyyy/MM/dd")
                objCommand.Parameters.Item("c_committee_id").Value = Row.Item(3)
                objCommand.Parameters.Item("s_committee_seq").Value = Row.Item(4)
                objCommand.ExecuteNonQuery()
            Next
        End Sub

        ' 日当明細を集計し、日当計算詳細 (call_roll_user)を更新/追加
        Private Sub UpdateInsertCallRollUser(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strKeyDate As String, ByVal strUserId As String, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)

            ' 日当明細の集計
            Dim dReader As NpgsqlDataReader = SumCallRollUserDtl(strDailyPayKind, objCommand, strKeyDate, strUserId, strPrevCloseDate, strCloseDate)
            If Not dReader.HasRows Then
                Return
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
            End If

            ' 支部別日当合計 (daily_pay_close_dtl)を更新/追加
            Dim strSqlUpd As String
            Dim strSqlIns As String
            Dim iResult As Integer
            strSqlUpd = "UPDATE call_roll_user SET " &
                     "c_period_id=:c_period_id, d_up_close=CONVERT(DATE,:d_up_close,112),s_daily_pay_total=:s_daily_pay_total,s_food_expenses_total=:s_food_expenses_total," &
                     "s_next_balance_daily_pay_total=:s_next_balance_daily_pay_total,s_next_balance_food_expenses_total=:s_next_balance_food_expenses_total," &
                     "d_ins=CONVERT(DATE,:d_ins,112),c_user_id_ins=:c_user_id_ins " &
                     "WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years) AND k_daily_pay_kind=:k_daily_pay_kind AND d_daily_pay_close=CONVERT(DATE,:d_daily_pay_close)"
            '        "d_up_close=CONVERT(DATE,:d_up_close,112),s_daily_pay_total=:s_daily_pay_total,s_food_expenses_total=:s_food_expenses_total," & _
            '        "WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years) AND c_period_id=:c_period_id AND k_daily_pay_kind=:k_daily_pay_kind AND d_daily_pay_close=CONVERT(DATE,:d_daily_pay_close)"

            strSqlIns = "INSERT INTO call_roll_user" &
                     "(c_user_id,d_years,c_period_id,k_daily_pay_kind,d_daily_pay_close,d_up_close," &
                     "s_daily_pay_total,s_food_expenses_total,s_next_balance_daily_pay_total,s_next_balance_food_expenses_total," &
                     "d_ins,c_user_id_ins) VALUES(:c_user_id,CONVERT(DATE,:d_years),:c_period_id,:k_daily_pay_kind,CONVERT(DATE,:d_daily_pay_close),CONVERT(DATE,:d_up_close,112)," &
                     ":s_daily_pay_total,:s_food_expenses_total,:s_next_balance_daily_pay_total,:s_next_balance_food_expenses_total," &
                     "CONVERT(DATE,:d_ins,112),:c_user_ins)"

            For Each Row As DataRow In dReader.getTable().Rows()
                ' 更新
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSqlUpd)
                objCommand.Parameters.Add(New NpgsqlParameter("d_up_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay_total", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_food_expenses_total", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_next_balance_daily_pay_total", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_next_balance_food_expenses_total", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                objCommand.Parameters.Item("d_up_close").Value = strKeyDate
                objCommand.Parameters.Item("s_daily_pay_total").Value = Row.Item(6)
                objCommand.Parameters.Item("s_food_expenses_total").Value = Row.Item(7)
                objCommand.Parameters.Item("s_next_balance_daily_pay_total").Value = Row.Item(8)
                objCommand.Parameters.Item("s_next_balance_food_expenses_total").Value = Row.Item(9)
                objCommand.Parameters.Item("d_ins").Value = strKeyDate
                objCommand.Parameters.Item("c_user_id_ins").Value = strUserId
                objCommand.Parameters.Item("c_user_id").Value = Row.Item(0)
                objCommand.Parameters.Item("d_years").Value = Row.Item(1)
                objCommand.Parameters.Item("c_period_id").Value = MDLoginInfo.PeriodId
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.Parameters.Item("d_daily_pay_close").Value = Row.Item(4)
                iResult = objCommand.ExecuteNonQuery()

                ' 追加
                If iResult = 0 Then
                    objCommand.Parameters.Clear()
                    objCommand.SetSql(strSqlIns)
                    objCommand.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_up_close", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay_total", DbType.Int32))
                    objCommand.Parameters.Add(New NpgsqlParameter("s_food_expenses_total", DbType.Int32))
                    objCommand.Parameters.Add(New NpgsqlParameter("s_next_balance_daily_pay_total", DbType.Int32))
                    objCommand.Parameters.Add(New NpgsqlParameter("s_next_balance_food_expenses_total", DbType.Int32))
                    objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
                    objCommand.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                    objCommand.Parameters.Item("c_user_id").Value = Row.Item(0)
                    objCommand.Parameters.Item("d_years").Value = Row.Item(1)
                    objCommand.Parameters.Item("c_period_id").Value = MDLoginInfo.PeriodId
                    objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                    objCommand.Parameters.Item("d_daily_pay_close").Value = Row.Item(4)
                    objCommand.Parameters.Item("d_up_close").Value = strKeyDate
                    objCommand.Parameters.Item("s_daily_pay_total").Value = Row.Item(6)
                    objCommand.Parameters.Item("s_food_expenses_total").Value = Row.Item(7)
                    objCommand.Parameters.Item("s_next_balance_daily_pay_total").Value = Row.Item(8)
                    objCommand.Parameters.Item("s_next_balance_food_expenses_total").Value = Row.Item(9)
                    objCommand.Parameters.Item("d_ins").Value = strKeyDate
                    objCommand.Parameters.Item("c_user_ins").Value = strUserId
                    objCommand.ExecuteNonQuery()
                End If
            Next

        End Sub

        ' 日当マスタよりデータ取得
        Private Function GetDailyPayData(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrevCloseDate As String, ByVal strCloseDate As String) As Hashtable
            Dim htControlMap As Hashtable = New Hashtable
            Dim strKey As String
            Dim strSql As String

            strSql = "SELECT DTL.c_daily_pay_id, DTL.c_menu_seq, MASTER.s_daily_pay " & _
                         "FROM call_roll_user_dtl DTL, " & _
                             "(SELECT MST.c_daily_pay_id, MST.c_menu_seq, MST.s_daily_pay FROM daily_pay_master_dtl AS MST," & _
                             "(SELECT M.c_daily_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM daily_pay_master_dtl AS M " & _
                             "WHERE M.d_from<:d_key_date And :d_key_date<=M.d_to GROUP BY M.c_daily_pay_id)  AS MT " & _
                             "WHERE MT.max_id=MST.c_daily_pay_id AND MT.max_d_from=MST.d_from) AS MASTER " & _
                         "WHERE DTL.c_daily_pay_id = MASTER.c_daily_pay_id AND " & _
                               "DTL.c_menu_seq = MASTER.c_menu_seq AND DTL.k_daily_pay_kind = :k_daily_pay_kind AND " & _
                               ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
                               "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "
            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_key_date", DbType.String))
            MDMasterCommon.GetKeyDate()
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            objCommand.Parameters.Item("d_key_date").Value = MDMasterCommon.GetKeyDate()
            Dim dReader As NpgsqlDataReader = objCommand.ExecuteReader

            For Each Row As DataRow In dReader.getTable().Rows()
                strKey = Row.Item(0).ToString.Trim & "," & Row.Item(1)
                If Not htControlMap.ContainsKey(strKey) Then
                    htControlMap.Add(strKey, Row.Item(2))
                End If
            Next

            Return htControlMap
        End Function

        ' 当期（前回締め日翌日から今回締め日）の日当明細を更新（日当金額）
        Private Sub UpdateDetailDailyPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)
            Dim strSql As String
            strSql = "UPDATE call_roll_user_dtl SET s_daily_pay=:s_daily_pay " & _
                         "WHERE c_daily_pay_id = :c_daily_pay_id AND " & _
                               "c_menu_seq = :c_menu_seq AND k_daily_pay_kind = :k_daily_pay_kind AND " & _
                               ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
                               "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "
            Dim cols As String()

            For Each de As DictionaryEntry In htControlMap
                cols = Split(de.Key.ToString(), ",", -1, CompareMethod.Text)
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSql)
                objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_daily_pay_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_menu_seq", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                objCommand.Parameters.Item("s_daily_pay").Value = de.Value
                objCommand.Parameters.Item("c_daily_pay_id").Value = cols(0)
                objCommand.Parameters.Item("c_menu_seq").Value = Integer.Parse(cols(1))
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                objCommand.ExecuteNonQuery()
            Next
        End Sub

        ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新（日当金額）
        Private Sub UpdateDetailPrevDailyPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable, ByVal strPrevCloseDate As String, ByVal strCloseDate As String, ByVal strUpdateDate As String)
            Dim strSql As String
            strSql = "UPDATE call_roll_user_dtl SET s_next_balance_daily_pay=:s_daily_pay " & _
                         "WHERE c_daily_pay_id = :c_daily_pay_id AND " & _
                               "c_menu_seq = :c_menu_seq AND k_daily_pay_kind = :k_daily_pay_kind AND " & _
                               ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " & _
                               "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"
            Dim cols As String()

            For Each de As DictionaryEntry In htControlMap
                cols = Split(de.Key.ToString(), ",", -1, CompareMethod.Text)
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSql)
                objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_daily_pay_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_menu_seq", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))
                objCommand.Parameters.Item("s_daily_pay").Value = de.Value
                objCommand.Parameters.Item("c_daily_pay_id").Value = cols(0)
                objCommand.Parameters.Item("c_menu_seq").Value = Integer.Parse(cols(1))
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                objCommand.ExecuteNonQuery()
            Next
        End Sub

        ' 当期（前回締め日翌日から今回締め日）の日当明細を更新（役員手当金額？、昼食日金額）
        Private Sub UpdateDetailLunchPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)
            Dim strSql As String
            'strSql = "UPDATE call_roll_user_dtl SET s_daily_pay=:s_daily_pay,s_food_expenses=:s_food_expenses " & _
            strSql = "UPDATE call_roll_user_dtl SET s_food_expenses=:s_food_expenses " & _
                         "WHERE c_committee_id = :c_committee_id AND " & _
                               "s_committee_seq = :s_committee_seq AND k_daily_pay_kind = :k_daily_pay_kind AND " & _
                               "k_food_expenses='1' AND " & _
                               ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
                               "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "
            Dim cols As String()
            Dim vals As String()

            For Each de As DictionaryEntry In htControlMap
                cols = Split(de.Key.ToString(), ",", -1, CompareMethod.Text)
                vals = Split(de.Value.ToString(), ",", -1, CompareMethod.Text)
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSql)
                'objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_food_expenses", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                'objCommand.Parameters.Item("s_daily_pay").Value = Integer.Parse(vals(0))
                objCommand.Parameters.Item("s_food_expenses").Value = Integer.Parse(vals(1))
                objCommand.Parameters.Item("c_committee_id").Value = cols(0)
                objCommand.Parameters.Item("s_committee_seq").Value = Integer.Parse(cols(1))
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                objCommand.ExecuteNonQuery()
            Next
        End Sub

        ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新（役員手当金額？、昼食日金額）
        Private Sub UpdateDetailPrevLunchPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable, ByVal strPrevCloseDate As String, ByVal strCloseDate As String, ByVal strUpdateDate As String)
            Dim strSql As String
            'strSql = "UPDATE call_roll_user_dtl SET s_daily_pay=:s_daily_pay,s_food_expenses=:s_food_expenses " & _
            strSql = "UPDATE call_roll_user_dtl SET s_next_balance_food_expenses=:s_food_expenses " & _
                         "WHERE c_committee_id = :c_committee_id AND " & _
                               "s_committee_seq = :s_committee_seq AND k_daily_pay_kind = :k_daily_pay_kind AND " & _
                               "k_food_expenses='1' AND " & _
                               ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " & _
                               "AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"
            Dim cols As String()
            Dim vals As String()

            For Each de As DictionaryEntry In htControlMap
                cols = Split(de.Key.ToString(), ",", -1, CompareMethod.Text)
                vals = Split(de.Value.ToString(), ",", -1, CompareMethod.Text)
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSql)
                'objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_food_expenses", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))
                'objCommand.Parameters.Item("s_daily_pay").Value = Integer.Parse(vals(0))
                objCommand.Parameters.Item("s_food_expenses").Value = Integer.Parse(vals(1))
                objCommand.Parameters.Item("c_committee_id").Value = cols(0)
                objCommand.Parameters.Item("s_committee_seq").Value = Integer.Parse(cols(1))
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
                objCommand.ExecuteNonQuery()
            Next
        End Sub

        ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新
        ' （前回締日以前に更新された日当明細の次期差分額更新）
        Private Sub UpdatePrevDetailNextPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable, ByVal strPrev2CloseDate As String, ByVal strPrevCloseDate As String)
            ' 前回締め日が未指定の場合、処理しない
            If strPrevCloseDate = "" Then
                Return
            End If

            ' 前回締日更新日取得
            Dim strUpdateDate As String = GetPrevCloseDateUpdateDate(strDailyPayKind, objCommand, strPrevCloseDate)
            log.Debug("前々回締日[" & strPrev2CloseDate & "] 前回締日[" & strPrevCloseDate & "]更新日=" & strUpdateDate)

            ' 日当計算区分の設定(新規追加日当明細)
            UpdatePrevDailyPayKind(strDailyPayKind, objCommand, strPrev2CloseDate, strPrevCloseDate, strUpdateDate)

            ' 日当マスタよりデータ取得
            Dim htMasterData As Hashtable = GetDailyPayData(strDailyPayKind, objCommand, strPrev2CloseDate, strPrevCloseDate)
            log.Debug("日当マスタ該当データ数=" & htMasterData.Count)

            ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新（日当金額）
            UpdateDetailPrevDailyPay(strDailyPayKind, objCommand, htMasterData, strPrev2CloseDate, strPrevCloseDate, strUpdateDate)

            ' 役員マスタ、中央執行昼食費マスタよりデータ取得
            htMasterData = GetOfficerAndLunchPayData(strDailyPayKind, objCommand, strPrev2CloseDate, strPrevCloseDate)
            log.Debug("昼食費マスタ該当データ数=" & htMasterData.Count)

            ' 前期（前々回締め日翌日から前回締め日）の日当明細を更新（役員手当金額？、昼食日金額）
            UpdateDetailPrevLunchPay(strDailyPayKind, objCommand, htMasterData, strPrev2CloseDate, strPrevCloseDate, strUpdateDate)

            ' 前期差分額の計算
            htMasterData = SumPrevNextDailyPay(strDailyPayKind, objCommand, strPrev2CloseDate, strPrevCloseDate, strUpdateDate)
            log.Debug("前期差分額データ数=" & htMasterData.Count)

            ' 前期差分額の更新
            UpdatePrevNextDailyPay(strDailyPayKind, objCommand, htMasterData)
        End Sub

        ' 前回締め日更新日の取得
        Private Function GetPrevCloseDateUpdateDate(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrevCloseDate As String)
            Dim strSql As String
            strSql = "SELECT d_ins FROM daily_pay_close WHERE d_daily_pay_close=:d_daily_pay_close AND c_ksh=:c_ksh AND k_daily_pay_kind=:k_daily_pay_kind"
            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Item("c_ksh").Value = MDLoginInfo.Ksh
            objCommand.Parameters.Item("d_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            Return Format(objCommand.ExecuteScalar, "yyyyMMdd")
        End Function

        ' 前期差分額の取得
        Private Function SumPrevNextDailyPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrevCloseDate As String, ByVal strCloseDate As String, ByVal strUpdateDate As String) As Hashtable
            Dim htControlMap As Hashtable = New Hashtable
            Dim strKey As String
            Dim strSql As String

            strSql = "SELECT c_user_id, d_years, s_day, c_committee_id, s_committee_seq, " & _
                     "IIF(ISNULL(s_next_balance_daily_pay),0,s_next_balance_daily_pay) - IIF(ISNULL(s_daily_pay),0,s_daily_pay) AS s_balance_daily_pay," & _
                     "IIF(ISNULL(s_next_balance_food_expenses),0,s_next_balance_food_expenses)-IIF(ISNULL(s_food_expenses),0,s_food_expenses) AS s_balance_food_expenses " & _
                     "FROM call_roll_user_dtl AS DTL " & _
                     "WHERE :prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close AND " & _
                     "k_daily_pay_kind = :k_daily_pay_kind AND (:strUpdateDate < Format(d_up, 'yyyyMMdd') OR :strUpdateDate <= Format(d_ins, 'yyyyMMdd'))"
            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("strUpdateDate", DbType.String))
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("strUpdateDate").Value = strUpdateDate
            Dim dReader As NpgsqlDataReader = objCommand.ExecuteReader

            For Each Row As DataRow In dReader.getTable().Rows()
                strKey = Row.Item(0).ToString.Trim & "," & Row.Item(1) & "," & Row.Item(2) & "," & Row.Item(3) & "," & Row.Item(4)
                If Not htControlMap.ContainsKey(strKey) Then
                    htControlMap.Add(strKey, Row.Item(5) & "," & Row.Item(6))
                End If
            Next

            Return htControlMap
        End Function

        ' 前期差分額の更新
        Private Sub UpdatePrevNextDailyPay(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal htControlMap As Hashtable)
            Dim strSql As String
            'strSql = "UPDATE call_roll_user_dtl SET s_next_balance_daily_pay=:s_daily_pay,s_next_balance_food_expenses=:s_food_expenses,d_ins=CONVERT(DATE,:d_ins),d_up=CONVERT(DATE,:d_up) " & _
            strSql = "UPDATE call_roll_user_dtl SET s_next_balance_daily_pay=:s_daily_pay,s_next_balance_food_expenses=:s_food_expenses " &
                         "WHERE c_user_id = :c_user_id AND d_years = CONVERT(DATE,:d_years) AND s_day = CONVERT(DATE,:s_day) AND " &
                               "c_committee_id = :c_committee_id AND s_committee_seq = :s_committee_seq AND " &
                               "k_daily_pay_kind = :k_daily_pay_kind"
            Dim cols As String()
            Dim vals As String()
            Dim iCount As Integer = 0
            Dim iPay1 As Integer = 0
            Dim iPay2 As Integer = 0

            For Each de As DictionaryEntry In htControlMap
                cols = Split(de.Key.ToString(), ",", -1, CompareMethod.Text)
                vals = Split(de.Value.ToString(), ",", -1, CompareMethod.Text)
                iPay1 = Integer.Parse(vals(0))
                iPay2 = Integer.Parse(vals(1))

                'If iPay1 <> 0 Or iPay2 <> 0 Then
                objCommand.Parameters.Clear()
                objCommand.SetSql(strSql)
                objCommand.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("s_food_expenses", DbType.Int32))
                objCommand.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_day", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
                'objCommand.Parameters.Add(New NpgsqlParameter("d_up", DbType.String))
                objCommand.Parameters.Item("s_daily_pay").Value = iPay1
                objCommand.Parameters.Item("s_food_expenses").Value = iPay2
                objCommand.Parameters.Item("c_user_id").Value = cols(0)
                objCommand.Parameters.Item("d_years").Value = cols(1)
                objCommand.Parameters.Item("s_day").Value = cols(2)
                objCommand.Parameters.Item("c_committee_id").Value = cols(3)
                objCommand.Parameters.Item("s_committee_seq").Value = cols(4)
                objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'objCommand.Parameters.Item("d_ins").Value = cols(2)
                'objCommand.Parameters.Item("d_up").Value = cols(2)
                iCount = iCount + objCommand.ExecuteNonQuery()
                'End If
            Next
            log.Debug("前期明細更新数=" & iCount.ToString)
        End Sub

        ' 役員マスタ、中央執行昼食費マスタよりデータ取得
        Private Function GetOfficerAndLunchPayData(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strPrevCloseDate As String, ByVal strCloseDate As String) As Hashtable
            Dim htControlMap As Hashtable = New Hashtable
            Dim strKey As String
            Dim strSql As String

            strSql = "SELECT call_roll_user_dtl.c_committee_id, call_roll_user_dtl.s_committee_seq," & _
                     "IIF(ISNULL(officer_pay_master.s_officer_pay),0,officer_pay_master.s_officer_pay) AS s_officer_pay," & _
                     "IIF(ISNULL(executive_lunch_pay_master.s_pay),0,executive_lunch_pay_master.s_pay) AS s_lunch_pay " & _
                     "FROM ((call_roll_user_dtl " & _
                     "INNER JOIN committee_dtl ON (call_roll_user_dtl.s_committee_seq = committee_dtl.s_committee_seq) AND " & _
                     "(call_roll_user_dtl.c_committee_id = committee_dtl.c_committee_id)) " & _
                         "LEFT JOIN (SELECT MST.d_from, MST.c_officer_pay_id, MST.s_officer_pay FROM officer_pay_master AS MST," & _
                         " (SELECT M.c_officer_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM officer_pay_master AS M " & _
                         "WHERE M.d_from<=:d_key_date And :d_key_date<=M.d_to GROUP BY M.c_officer_pay_id) AS MT " & _
                         "WHERE MT.max_id=MST.c_officer_pay_id AND MT.max_d_from=MST.d_from) AS officer_pay_master " & _
                         "ON committee_dtl.c_officer_pay_id = officer_pay_master.c_officer_pay_id) " & _
                         "LEFT JOIN (SELECT MST.d_from, MST.c_executive_lunch_pay_id, MST.s_pay FROM executive_lunch_pay_master AS MST," & _
                         " (SELECT M.c_executive_lunch_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM executive_lunch_pay_master AS M " & _
                         "WHERE M.d_from<=:d_key_date And :d_key_date<=M.d_to GROUP BY M.c_executive_lunch_pay_id) AS MT " & _
                         "WHERE MT.max_id=MST.c_executive_lunch_pay_id AND MT.max_d_from=MST.d_from) AS executive_lunch_pay_master " & _
                         "ON committee_dtl.c_executive_lunch_pay_id = executive_lunch_pay_master.c_executive_lunch_pay_id " & _
                     "WHERE call_roll_user_dtl.k_food_expenses='1' AND " & _
                     ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
                     "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close "
            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_key_date", DbType.String))
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            'objCommand.Parameters.Item("d_key_date").Value = MDMasterCommon.GetKeyDate()
            objCommand.Parameters.Item("d_key_date").Value = strCloseDate
            Dim dReader As NpgsqlDataReader = objCommand.ExecuteReader

            For Each Row As DataRow In dReader.getTable().Rows()
                strKey = Row.Item(0).ToString.Trim & "," & Row.Item(1)
                If Not htControlMap.ContainsKey(strKey) Then
                    htControlMap.Add(strKey, Row.Item(2) & "," & Row.Item(3))
                End If
            Next

            Return htControlMap
        End Function

        ' 日当明細の集計
        Private Function SumCallRollUserDtl(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strKeyDate As String, ByVal strUserId As String, ByVal strPrevCloseDate As String, ByVal strCloseDate As String) As NpgsqlDataReader
            Dim strSql As String
            Dim strSql4 As String
            'strSql = "SELECT c_user_id,d_years,:c_period_id,k_daily_pay_kind,call_roll_user_dtl.d_daily_pay_close,CONVERT(DATE,:d_up_close,112)," & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_daily_pay),0,call_roll_user_dtl.s_daily_pay)) AS s_daily_pay, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_food_expenses),0,call_roll_user_dtl.s_food_expenses)) AS s_food_expenses, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_daily_pay),0,call_roll_user_dtl.s_next_balance_daily_pay)) AS s_next_balance_daily_pay, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_food_expenses),0,call_roll_user_dtl.s_next_balance_food_expenses)) AS s_next_balance_food_expenses, " & _
            '         "DCONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " & _
            '         "FROM call_roll_user_dtl INNER JOIN committee ON call_roll_user_dtl.c_committee_id = committee.c_committee_id " & _
            '         "WHERE call_roll_user_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " & _
            '         ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
            '         "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " & _
            '         "GROUP BY c_user_id, d_years, call_roll_user_dtl.d_daily_pay_close, call_roll_user_dtl.d_years, call_roll_user_dtl.k_daily_pay_kind, committee.k_belonging, call_roll_user_dtl.c_committee_id "
            strSql = "SELECT c_user_id,d_years,c_period_id,k_daily_pay_kind,d_daily_pay_close,d_up_close," &
                     "SUM(DATA.s_daily_pay) AS s_daily_pay, SUM(DATA.s_food_expenses) AS s_food_expenses," &
                     "SUM(DATA.s_next_balance_daily_pay) AS s_next_balance_daily_pay, SUM(DATA.s_next_balance_food_expenses) AS s_next_balance_food_expenses FROM (" &
                     "SELECT DTL.c_user_id,d_years,:c_period_id AS c_period_id,k_daily_pay_kind,DTL.d_daily_pay_close,CONVERT(DATE,:d_up_close,112) AS d_up_close," &
                     "Sum(IIF(ISNULL(DTL.s_daily_pay),0,DTL.s_daily_pay)) AS s_daily_pay, " &
                     "Sum(IIF(ISNULL(DTL.s_food_expenses),0,DTL.s_food_expenses)) AS s_food_expenses, " &
                     "0 AS s_next_balance_daily_pay, " &
                     "0 AS s_next_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl AS DTL " &
                     "WHERE DTL.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     "Format(d_daily_pay_close, 'yyyyMMdd') = :d_daily_pay_close " &
                     "GROUP BY DTL.c_user_id, d_years, DTL.k_daily_pay_kind, DTL.d_daily_pay_close " &
                     "UNION ALL " &
                     "SELECT DTL.c_user_id,CONVERT(DATE,LEFT(:d_daily_pay_close, 6) & '01',112) AS d_years,:c_period_id AS c_period_id,k_daily_pay_kind,CONVERT(DATE,:d_daily_pay_close,112) AS d_daily_pay_close,CONVERT(DATE,:d_up_close,112) AS d_up_close," &
                     "0 AS s_daily_pay, " &
                     "0 AS s_food_expenses, " &
                     "Sum(IIF(ISNULL(DTL.s_next_balance_daily_pay),0,DTL.s_next_balance_daily_pay)) AS s_next_balance_daily_pay, " &
                     "Sum(IIF(ISNULL(DTL.s_next_balance_food_expenses),0,DTL.s_next_balance_food_expenses)) AS s_next_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl AS DTL " &
                     "WHERE DTL.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     "Format(d_daily_pay_close, 'yyyyMMdd') = :prev_daily_pay_close " &
                     "GROUP BY DTL.c_user_id, d_years, DTL.k_daily_pay_kind, DTL.d_daily_pay_close " &
                     ") AS DATA GROUP BY c_user_id, d_years, c_period_id, k_daily_pay_kind, d_daily_pay_close, d_up_close"
            strSql4 = "SELECT call_roll_user_dtl.c_user_id,d_years,:c_period_id,k_daily_pay_kind,call_roll_user_dtl.d_daily_pay_close,CONVERT(DATE,:d_up_close,112)," &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_daily_pay),0,call_roll_user_dtl.s_daily_pay)) AS s_daily_pay, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_food_expenses),0,call_roll_user_dtl.s_food_expenses)) AS s_food_expenses, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_daily_pay),0,call_roll_user_dtl.s_next_balance_daily_pay)) AS s_next_balance_daily_pay, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_food_expenses),0,call_roll_user_dtl.s_next_balance_food_expenses)) AS s_next_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl " &
                     "INNER JOIN (SELECT saft_view.c_user_id, saft_view.k_belonging FROM staf_attribute_full_time_view saft_view, " &
                     "(SELECT s1.c_user_id, Max(s1.d_from) AS d_from_max FROM staf_attribute_full_time_view s1 GROUP BY s1.c_user_id) max_saft_view " &
                     " WHERE saft_view.c_user_id = max_saft_view.c_user_id AND saft_view.d_from = max_saft_view.d_from_max ) AS staf " &
                     "ON call_roll_user_dtl.c_user_id = staf.c_user_id " &
                     "WHERE call_roll_user_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                     "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                     "GROUP BY call_roll_user_dtl.c_user_id, d_years, d_daily_pay_close, d_years, k_daily_pay_kind, k_belonging, c_committee_id "

            objCommand.Parameters.Clear()
            If strDailyPayKind = "04" Then
                objCommand.SetSql(strSql4)
            Else
                objCommand.SetSql(strSql)
            End If
            objCommand.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_up_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            'objCommand.Parameters.Add(New NpgsqlParameter("prev2_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Item("c_period_id").Value = MDLoginInfo.PeriodId
            objCommand.Parameters.Item("d_up_close").Value = strKeyDate
            objCommand.Parameters.Item("d_ins").Value = strKeyDate
            objCommand.Parameters.Item("c_user_id_ins").Value = strUserId
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            'objCommand.Parameters.Item("prev2_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            Dim dReader As NpgsqlDataReader = objCommand.ExecuteReader

            Return dReader
        End Function

        ' 日当明細を集計し、支部別日当合計 (daily_pay_close_dtl)を更新
        Private Sub UpdateDailyPayCloseDtl(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strKeyDate As String, ByVal strUserId As String, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)
            Dim strSql As String
            Dim strSql4 As String
            Dim strSqlCt As String
            'strSql = "INSERT INTO daily_pay_close_dtl" & _
            '         "(d_daily_pay_close, d_years, c_ksh, k_daily_pay_kind, k_belonging, c_committee_id," & _
            '         "s_daily_pay, s_food_expenses, s_balance_daily_pay, s_balance_food_expenses, d_ins, c_user_id_ins) " & _
            '         "SELECT Format(call_roll_user_dtl.d_daily_pay_close, 'yyyyMMdd'), call_roll_user_dtl.d_years, :c_ksh AS c_ksh, " & _
            '         "call_roll_user_dtl.k_daily_pay_kind, IIF(ISNULL(committee.k_belonging),'',committee.k_belonging), call_roll_user_dtl.c_committee_id, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_daily_pay),0,call_roll_user_dtl.s_daily_pay)) AS s_daily_pay, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_food_expenses),0,call_roll_user_dtl.s_food_expenses)) AS s_food_expenses, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_daily_pay),0,call_roll_user_dtl.s_next_balance_daily_pay)) AS s_next_balance_daily_pay, " & _
            '         "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_food_expenses),0,call_roll_user_dtl.s_next_balance_food_expenses)) AS s_next_balance_food_expenses, " & _
            '         "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " & _
            '         "FROM call_roll_user_dtl INNER JOIN committee ON call_roll_user_dtl.c_committee_id = committee.c_committee_id " & _
            '         "WHERE call_roll_user_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " & _
            '         ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
            '         "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " & _
            '         "GROUP BY call_roll_user_dtl.d_daily_pay_close, call_roll_user_dtl.d_years, call_roll_user_dtl.k_daily_pay_kind, committee.k_belonging, call_roll_user_dtl.c_committee_id "
            strSql = "INSERT INTO daily_pay_close_dtl" &
                     "(d_daily_pay_close, d_years, c_ksh, k_daily_pay_kind, k_belonging, c_committee_id," &
                     "s_daily_pay, s_food_expenses, s_balance_daily_pay, s_balance_food_expenses, d_ins, c_user_id_ins) " &
                     "SELECT d_daily_pay_close, d_years, c_ksh, k_daily_pay_kind, k_belonging, c_committee_id," &
                     "SUM(DATA.s_daily_pay) AS s_daily_pay, SUM(DATA.s_food_expenses) AS s_food_expenses, " &
                     "SUM(DATA.s_balance_daily_pay) AS s_balance_daily_pay, SUM(DATA.s_balance_food_expenses) AS s_balance_food_expenses, " &
                     "d_ins, c_user_id_ins FROM (" &
                     "SELECT Format(DTL.d_daily_pay_close, 'yyyyMMdd') AS d_daily_pay_close, DTL.d_years, :c_ksh AS c_ksh, " &
                     "DTL.k_daily_pay_kind, k_belonging, DTL.c_committee_id, " &
                     "Sum(IIF(ISNULL(DTL.s_daily_pay),0,DTL.s_daily_pay)) AS s_daily_pay, " &
                     "Sum(IIF(ISNULL(DTL.s_food_expenses),0,DTL.s_food_expenses)) AS s_food_expenses, " &
                     "0 AS s_balance_daily_pay, " &
                     "0 AS s_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl AS DTL INNER JOIN staf_attribute_latest_view ON DTL.c_user_id = staf_attribute_latest_view.c_user_id " &
                     "WHERE DTL.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     "Format(d_daily_pay_close, 'yyyyMMdd') = :d_daily_pay_close " &
                     "GROUP BY DTL.d_daily_pay_close, DTL.d_years, DTL.k_daily_pay_kind, k_belonging, DTL.c_committee_id " &
                     "UNION ALL " &
                     "SELECT :d_daily_pay_close AS d_daily_pay_close, CONVERT(DATE,LEFT(:d_daily_pay_close, 6) & '01',112) AS d_years, :c_ksh AS c_ksh, " &
                     "DTL.k_daily_pay_kind, k_belonging, DTL.c_committee_id, " &
                     "0 AS s_daily_pay, " &
                     "0 AS s_food_expenses, " &
                     "Sum(IIF(ISNULL(DTL.s_next_balance_daily_pay),0,DTL.s_next_balance_daily_pay)) AS s_balance_daily_pay, " &
                     "Sum(IIF(ISNULL(DTL.s_next_balance_food_expenses),0,DTL.s_next_balance_food_expenses)) AS s_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl AS DTL INNER JOIN staf_attribute_latest_view ON DTL.c_user_id = staf_attribute_latest_view.c_user_id " &
                     "WHERE DTL.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     "Format(d_daily_pay_close, 'yyyyMMdd') = :prev_daily_pay_close " &
                     "GROUP BY DTL.d_daily_pay_close, DTL.d_years, DTL.k_daily_pay_kind, k_belonging, DTL.c_committee_id" &
                     ") AS DATA GROUP BY d_daily_pay_close, d_years, c_ksh, k_daily_pay_kind, k_belonging, c_committee_id, d_ins, c_user_id_ins"
            strSql4 = "INSERT INTO daily_pay_close_dtl" &
                     "(d_daily_pay_close, d_years, c_ksh, k_daily_pay_kind, k_belonging, c_committee_id," &
                     "s_daily_pay, s_food_expenses, s_balance_daily_pay, s_balance_food_expenses, d_ins, c_user_id_ins) " &
                     "SELECT Format(call_roll_user_dtl.d_daily_pay_close, 'yyyyMMdd'), call_roll_user_dtl.d_years, :c_ksh AS c_ksh, " &
                     "call_roll_user_dtl.k_daily_pay_kind, k_belonging, call_roll_user_dtl.c_committee_id, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_daily_pay),0,call_roll_user_dtl.s_daily_pay)) AS s_daily_pay, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_food_expenses),0,call_roll_user_dtl.s_food_expenses)) AS s_food_expenses, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_daily_pay),0,call_roll_user_dtl.s_next_balance_daily_pay)) AS s_next_balance_daily_pay, " &
                     "Sum(IIF(ISNULL(call_roll_user_dtl.s_next_balance_food_expenses),0,call_roll_user_dtl.s_next_balance_food_expenses)) AS s_next_balance_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM call_roll_user_dtl " &
                     "INNER JOIN (SELECT saft_view.c_user_id, saft_view.k_belonging FROM staf_attribute_full_time_view saft_view, " &
                     "(SELECT s1.c_user_id, Max(s1.d_from) AS d_from_max FROM staf_attribute_full_time_view s1 GROUP BY s1.c_user_id) max_saft_view " &
                     " WHERE saft_view.c_user_id = max_saft_view.c_user_id AND saft_view.d_from = max_saft_view.d_from_max ) AS staf " &
                     "ON call_roll_user_dtl.c_user_id = staf.c_user_id " &
                     "WHERE call_roll_user_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                     "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                     "GROUP BY d_daily_pay_close, d_years, k_daily_pay_kind, k_belonging, c_committee_id "
            strSqlCt = "SELECT COUNT(*) FROM call_roll_user_dtl " & _
                     "WHERE call_roll_user_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " & _
                     ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " & _
                     "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close"
            'todo:
            objCommand.Parameters.Clear()
            objCommand.SetSql(strSqlCt)
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            Dim iCount As Integer = CInt(objCommand.ExecuteScalar())
            If iCount = 0 Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
            End If

            objCommand.Parameters.Clear()
            If strDailyPayKind = "04" Then
                objCommand.SetSql(strSql4)
            Else
                objCommand.SetSql(strSql)
            End If
            objCommand.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Item("c_ksh").Value = MDLoginInfo.Ksh
            objCommand.Parameters.Item("d_ins").Value = strKeyDate
            objCommand.Parameters.Item("c_user_id_ins").Value = strUserId
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            objCommand.ExecuteNonQuery()
        End Sub

        ' 支部別日当合計を集計し、締め日一覧 (daily_pay_close)を更新
        Private Sub UpdateDailyPayClose(ByVal strDailyPayKind As String, ByVal objCommand As NpgsqlCommand, ByVal strKeyDate As String, ByVal strUserId As String, ByVal strPrevCloseDate As String, ByVal strCloseDate As String)
            Dim strSql As String
            strSql = "INSERT INTO daily_pay_close(d_daily_pay_close, c_ksh, k_daily_pay_kind, s_daily_pay_total, s_food_expenses_total, d_ins, c_user_id_ins) " &
                     "SELECT d_daily_pay_close, c_ksh, k_daily_pay_kind, " &
                     "Sum(daily_pay_close_dtl.s_daily_pay + s_balance_daily_pay) AS s_daily_pay, " &
                     "Sum(s_food_expenses + s_balance_food_expenses) AS s_food_expenses, " &
                     "CONVERT(DATE,:d_ins,112) AS d_ins, :c_user_id_ins AS c_user_id_ins " &
                     "FROM daily_pay_close_dtl " &
                     "WHERE daily_pay_close_dtl.k_daily_pay_kind=:k_daily_pay_kind AND " &
                     ":prev_daily_pay_close < Format(d_years, 'yyyyMMdd') AND " &
                     "Format(d_years, 'yyyyMMdd') <= :d_daily_pay_close " &
                     "GROUP BY d_daily_pay_close, c_ksh, k_daily_pay_kind"

            objCommand.Parameters.Clear()
            objCommand.SetSql(strSql)
            objCommand.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
            objCommand.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
            objCommand.Parameters.Item("d_ins").Value = strKeyDate
            objCommand.Parameters.Item("c_user_id_ins").Value = strUserId
            objCommand.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
            objCommand.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
            objCommand.Parameters.Item("d_daily_pay_close").Value = strCloseDate
            objCommand.ExecuteNonQuery()
        End Sub

        Public Sub DeleteDailyAllowanceClose(ByVal strDailyPayKind As String, ByVal strCloseDate As String)
            Try
                Dim str2 As String = Nothing
                Dim ds As DataSet = Me.GetPrev2DailyPayClose(strDailyPayKind)
                If (Not ds Is Nothing) Then
                    Dim str3 As String = ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("max").ToString
                    If (Not String.IsNullOrEmpty(str3) AndAlso Not strCloseDate.Equals(str3)) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0 - 1) {})
                    End If
                    str2 = ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("next_max").ToString
                Else
                    str2 = Nothing
                End If
                'Dim cmdText As String = "DELETE FROM " & " " & "daily_pay_close_dtl WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "  " & "AND d_daily_pay_close = :d_close_date; DELETE FROM " & "   " & "daily_pay_close WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND d_daily_pay_close = :d_close_date; UPDATE " & "    " & "call_roll_user SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & "    " & "d_up_close = NULL, " & "   " & "s_daily_pay_total = 0, " & "   " & "s_food_expenses_total = 0 WHERE " & "  " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND d_daily_pay_close = :d_close_date; UPDATE " & "    " & "call_roll_user_dtl SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & " " & "d_up_close = NULL, " & "    " & "s_daily_pay = 0, " & " " & "s_food_expenses = 0 WHERE " & "    " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND d_daily_pay_close = :d_close_date; "
                Dim cmdText As String
                If strDailyPayKind = UnionConst.DAILY_PAY_KIND_DGM Then
                    cmdText = "DELETE FROM " & "  " & "daily_pay_close_dtl WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "  " & "AND d_daily_pay_close = :d_close_date; DELETE FROM " & "   " & "daily_pay_close WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND d_daily_pay_close = :d_close_date; UPDATE " & "    " & "call_roll_user SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & "    " & "d_up_close = NULL, " & "   " & "s_daily_pay_total = 0, " & "   " & "s_food_expenses_total = 0 WHERE " & "  " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_close_date; UPDATE " & " " & "call_roll_user_dtl SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & "  " & "d_up_close = NULL " & "   " & "WHERE " & "    " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_close_date; "
                Else
                    cmdText = "DELETE FROM " & "  " & "daily_pay_close_dtl WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "  " & "AND d_daily_pay_close = :d_close_date; DELETE FROM " & "   " & "daily_pay_close WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND d_daily_pay_close = :d_close_date; UPDATE " & "    " & "call_roll_user SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & "    " & "d_up_close = NULL, " & "   " & "s_daily_pay_total = 0, " & "   " & "s_food_expenses_total = 0 WHERE " & "  " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_close_date; UPDATE " & " " & "call_roll_user_dtl SET " & "   " & "d_daily_pay_close = NULL, " & "    " & "k_daily_pay_kind = NULL, " & "  " & "d_up_close = NULL, " & "   " & "s_daily_pay = 0, " & " " & "s_food_expenses = 0 WHERE " & "    " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_close_date; "
                End If
                If Not String.IsNullOrEmpty(str2) Then
                    'cmdText = (cmdText & "UPDATE " & " " & "call_roll_user SET  " & "  " & "s_next_balance_daily_pay_total = 0, " & "  " & "s_next_balance_food_expenses_total = 0 WHERE " & "  " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND d_daily_pay_close = :d_before_close_date; UPDATE " & " " & "call_roll_user_dtl SET  " & "  " & "s_next_balance_daily_pay = 0, " & "  " & "s_next_balance_food_expenses = 0 WHERE " & "   " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND d_daily_pay_close = :d_before_close_date; ")
                    cmdText = (cmdText & "UPDATE " & "  " & "call_roll_user SET  " & "  " & "s_next_balance_daily_pay_total = 0, " & "  " & "s_next_balance_food_expenses_total = 0 WHERE " & "  " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_before_close_date; UPDATE " & "  " & "call_roll_user_dtl SET  " & "    " & "s_next_balance_daily_pay = 0, " & "    " & "s_next_balance_food_expenses = 0 WHERE " & "   " & "k_daily_pay_kind = :k_daily_pay_kind " & " " & "AND Format(d_daily_pay_close,'yyyyMMdd') = :d_before_close_date; ")
                End If
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_close_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_before_close_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                command.Parameters.Item("d_close_date").Value = strCloseDate
                command.Parameters.Item("d_before_close_date").Value = str2
                'command.ExecuteReader()
                command.ExecuteNonQuery()
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

        Public Function GetCommitteeBelongCloseList(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strPrevCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim cmdText As String = "SELECT " & "  " & "daily_pay_close_dtl_A.d_daily_pay_close, " & " " & "TO_DATE(daily_pay_close_dtl_A.d_daily_pay_close, 'yyyyMMdd') AS " & "締め日" & ", " & " " & "daily_pay_close_dtl_A.k_belonging, " & "   " & "belonging_view_A.l_name AS " & "支部" & ", " & "   " & "daily_pay_close_dtl_A.sum_daily_pay, " & "   " & "daily_pay_close_dtl_A.sum_daily_pay AS " & "今回日当計" & ", " & " " & "daily_pay_close_dtl_A.sum_balance_daily_pay, " & " " & "daily_pay_close_dtl_A.sum_balance_daily_pay AS " & "前回差分計" & ", " & "    " & "daily_pay_close_dtl_A.belonging_daily_pay, " & "   " & "daily_pay_close_dtl_A.belonging_daily_pay AS " & "支部別日当額計" & " FROM " & "   " & "(SELECT " & "      " & "d_daily_pay_close, " & "       " & "k_belonging, " & "     " & "SUM(s_daily_pay) AS sum_daily_pay, " & "       " & "SUM(s_balance_daily_pay) AS sum_balance_daily_pay, " & "        " & "(SUM(s_daily_pay) + SUM(s_balance_daily_pay)) AS belonging_daily_pay " & " " & "FROM " & "     " & "daily_pay_close_dtl " & "    " & "WHERE " & "        " & "c_ksh = :c_ksh " & "       " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "     " & "AND d_daily_pay_close = :d_close_date " & "    " & "GROUP BY " & "     " & "d_daily_pay_close, " & "       " & "k_belonging " & "  " & ") daily_pay_close_dtl_A LEFT OUTER JOIN " & "  " & "(SELECT " & "      " & "belonging_view.c_constant_seq, " & "        " & "belonging_view.d_from, " & "       " & "belonging_view.l_name " & "    " & "FROM " & "     " & "belonging_view, " & "      " & "(SELECT " & "            " & "c_constant_seq, " & "          " & "MAX(d_from) AS d_from " & "        " & "FROM " & "         " & "belonging_view " & "       " & "WHERE " & "            " & "((belonging_view.d_from <= :d_close_date " & "             " & "AND belonging_view.d_to >= :d_close_date) " & "            " & "OR (belonging_view.d_from <= :d_prev_date " & "             " & "AND belonging_view.d_to >= :d_prev_date) " & "         " & "OR (d_from >= :d_prev_date " & "               " & "AND d_to <= :d_close_date)) " & "        " & "GROUP BY " & "         " & "c_constant_seq " & "       " & ") belonging_view_MAX " & " " & "WHERE " & "        " & "belonging_view.c_constant_seq = belonging_view_MAX.c_constant_seq " & "       " & "AND belonging_view.d_from = belonging_view_MAX.d_from " & "    " & ") belonging_view_A ON " & "    " & "belonging_view_A.c_constant_seq = daily_pay_close_dtl_A.k_belonging ORDER BY " & " " & "daily_pay_close_dtl_A.k_belonging "
                Dim cmdText As String = "SELECT " & "   " & "daily_pay_close_dtl_A.d_daily_pay_close, " & " " & "CONVERT(DATE,daily_pay_close_dtl_A.d_daily_pay_close,112) AS " & "締め日" & ", " & "  " & "daily_pay_close_dtl_A.k_belonging, " & "   " & "belonging_view_A.l_name AS " & "支部" & ", " & "   " & "daily_pay_close_dtl_A.sum_daily_pay, " & "   " & "daily_pay_close_dtl_A.sum_daily_pay AS " & "今回日当計,daily_pay_close_dtl_A.sum_food_expenses,daily_pay_close_dtl_A.sum_food_expenses AS 昼食費計" & ", " & " " & "daily_pay_close_dtl_A.sum_balance_daily_pay, " & " " & "daily_pay_close_dtl_A.sum_balance_daily_pay AS " & "前回差分計,daily_pay_close_dtl_A.sum_balance_food_expenses,daily_pay_close_dtl_A.sum_balance_food_expenses AS 前回差分昼食費計" & ", " & "    " & "daily_pay_close_dtl_A.belonging_daily_pay, " & "   " & "daily_pay_close_dtl_A.belonging_daily_pay AS " & "支部別日当額計" & " FROM " & "  " & "(SELECT " & "      " & "d_daily_pay_close, " & "       " & "k_belonging, " & "     " & "SUM(s_daily_pay) AS sum_daily_pay,SUM(s_food_expenses) AS sum_food_expenses, " & "       " & "SUM(s_balance_daily_pay) AS sum_balance_daily_pay,SUM(s_balance_food_expenses) AS sum_balance_food_expenses, " & "       " & "(SUM(s_daily_pay) + SUM(s_balance_daily_pay)+SUM(s_food_expenses) + SUM(s_balance_food_expenses)) AS belonging_daily_pay " & " " & "FROM " & "     " & "daily_pay_close_dtl " & "   " & "WHERE " & "        " & "c_ksh = :c_ksh " & "       " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "     " & "AND d_daily_pay_close = :d_close_date " & "  " & "GROUP BY " & "     " & "d_daily_pay_close, " & "       " & "k_belonging " & "  " & ") daily_pay_close_dtl_A LEFT OUTER JOIN " & "  " & "(SELECT " & "      " & "belonging_view.c_constant_seq, " & "      " & "belonging_view.d_from, " & "       " & "belonging_view.l_name " & "    " & "FROM " & "     " & "belonging_view, " & "      " & "(SELECT " & "          " & "c_constant_seq, " & "          " & "MAX(belonging_view.d_from) AS d_from " & "     " & "FROM " & "         " & "belonging_view " & "       " & "WHERE " & "         " & "((belonging_view.d_from <= :d_close_date " & "             " & "AND belonging_view.d_to >= :d_close_date) " & "            " & "OR (belonging_view.d_from <= :d_prev_date " & "              " & "AND belonging_view.d_to >= :d_prev_date) " & "         " & "OR (d_from >= :d_prev_date " & "               " & "AND d_to <= :d_close_date)) " & "     " & "GROUP BY " & "         " & "c_constant_seq " & "       " & ") belonging_view_MAX " & " " & "WHERE " & "        " & "belonging_view.c_constant_seq = belonging_view_MAX.c_constant_seq " & "        " & "AND belonging_view.d_from = belonging_view_MAX.d_from " & "    " & ") belonging_view_A ON " & "    " & "belonging_view_A.c_constant_seq = daily_pay_close_dtl_A.k_belonging ORDER BY " & "  " & "daily_pay_close_dtl_A.k_belonging " & UtDb.DbOrderOffset() 'ok

                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_close_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_prev_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                command.Parameters.Item("d_close_date").Value = strCloseDate
                command.Parameters.Item("d_prev_date").Value = strPrevCloseDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                Dim strTable As String '= If(strDailyPayKind.Equals("01"), "committee_belong_close_list", "branch_belong_close_list")
                Select Case strDailyPayKind
                    Case "01"
                        strTable = "committee_belong_close_list"
                    Case "02"
                        strTable = "branch_belong_close_list"
                    Case Else
                        strTable = "dgm_belong_close_list"
                End Select
                ds.Tables.Add(MyBase.CreateSomeDataSet(strTable, dReader))
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

        Public Function GetCommitteeBelongCloseListPrint(ByVal strBelongingId As String, ByVal strDailyPayKind As String, ByVal strCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim cmdText As String = "SELECT " & "   " & "c_committee_id, " & "  " & "d_daily_pay_close, " & "   " & "k_belonging, " & " " & "(SUM(daily_pay_close_dtl.s_daily_pay) + SUM(daily_pay_close_dtl.s_balance_daily_pay)) AS s_daily_pay FROM " & " " & "daily_pay_close_dtl WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND k_belonging = :k_belonging " & "   " & "AND d_daily_pay_close = :d_daily_pay_close GROUP BY " & "  " & "c_committee_id, " & "  " & "d_daily_pay_close, " & "  " & "k_belonging ORDER BY " & " " & "c_committee_id "
                Dim cmdText As String = "SELECT " & "   " & "c_committee_id, " & "  " & "d_daily_pay_close, " & "   " & "k_belonging, " & " " & "(SUM(daily_pay_close_dtl.s_daily_pay) + SUM(daily_pay_close_dtl.s_balance_daily_pay)) AS s_daily_pay,(SUM(daily_pay_close_dtl.s_food_expenses) + SUM(daily_pay_close_dtl.s_balance_food_expenses)) AS s_food_expenses FROM " & " " & "daily_pay_close_dtl WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND k_belonging = :k_belonging " & "   " & "AND d_daily_pay_close = :d_daily_pay_close GROUP BY " & "  " & "c_committee_id, " & "  " & "d_daily_pay_close, " & "  " & "k_belonging ORDER BY " & " " & "c_committee_id " & UtDb.DbOrderOffset
                'todo:
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                command.Parameters.Item("k_belonging").Value = strBelongingId
                command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                Dim strTable As String '= If(strDailyPayKind.Equals("01"), "committee_belong_close_list_print", "branch_belong_close_list_print")
                Select Case strDailyPayKind
                    Case "01"
                        strTable = "committee_belong_close_list_print"
                    Case "02"
                        strTable = "branch_belong_close_list_print"
                    Case Else
                        strTable = "dgm_belong_close_list_print"
                End Select
                ds.Tables.Add(MyBase.CreateSomeDataSet(strTable, dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeCloseDtl(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strPrevCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim command As New NpgsqlCommand(("SELECT " &
       "FALSE AS print_check, " &
       "call_roll_user_info.c_user_id AS c_user_id, " &
       "call_roll_user_info.c_user_id AS " & "個人認証ＩＤ" & ", " &
       "Long.Parse(Format(staf_attribute.c_staf_id, '9999999999')) AS c_staf_id, " &
       "staf_attribute.c_staf_id AS " & "社員番号" & ", " &
       "staf_attribute.l_name AS l_name, " &
       "staf_attribute.l_name AS " & "氏名" & ", " &
       "staf_attribute.k_qualification AS k_qualification, " &
       "qualification.l_omission_name AS " & "資格" & ", " &
       "model.l_name AS " & "機種" & ", " &
       "staf_attribute.k_belonging AS k_belonging, " &
       "staf_attribute.k_belonging AS " & "支部" & ", " &
       "call_roll_user_info.sum_daily_pay AS sum_daily_pay, " &
       "call_roll_user_info.sum_daily_pay AS " & "今回日当計" & ", " &
       "call_roll_user_info.sum_balance_daily_pay AS sum_balance_daily_pay, " &
       "call_roll_user_info.sum_balance_daily_pay AS " & "前回差分計" & ", " &
       "call_roll_user_info.sum_daily_pay + call_roll_user_info.sum_balance_daily_pay AS " & "日当計" & " " &
      "FROM " &
       "(SELECT " &
        "call_roll_user_A.c_user_id AS c_user_id," &
        "COALESCE(SUM(call_roll_user_A.sum_daily_pay), 0) AS sum_daily_pay, " &
        "COALESCE(SUM(call_roll_user_A.sum_balance_daily_pay), 0) AS sum_balance_daily_pay " &
       "FROM " &
        "( " &
         "(SELECT " &
          "HDR.c_user_id AS c_user_id, " &
          "HDR.d_daily_pay_close AS d_daily_pay_close, " &
          "SUM(HDR.s_daily_pay_total) AS sum_daily_pay, " &
          "0 AS sum_balance_daily_pay " &
         "FROM " &
          "call_roll_user HDR, " &
          "(SELECT " &
           "c_user_id, " &
           "d_years " &
          "FROM " &
           "call_roll_user_dtl " &
          "WHERE " &
           "k_daily_pay_kind = :k_daily_pay_kind " &
          "GROUP BY " &
           "c_user_id, " &
           "d_years " &
          ") DTL " &
         "WHERE " &
          "HDR.d_daily_pay_close = :d_daily_pay_close " &
          "AND HDR.s_daily_pay_total <> 0 " &
          "AND DTL.c_user_id = HDR.c_user_id " &
          "AND DTL.d_years = HDR.d_years " &
         "GROUP BY " &
           "HDR.c_user_id, " &
           "HDR.d_daily_pay_close " &
         " ) " &
        "UNION ALL " &
         "(SELECT " &
          "HDR.c_user_id AS c_user_id, " &
          "HDR.d_daily_pay_close AS d_daily_pay_close, " &
          "0 AS sum_daily_pay, " &
          "SUM(HDR.s_next_balance_daily_pay_total) AS sum_balance_daily_pay " &
         "FROM " &
          "call_roll_user HDR, " &
          "(SELECT " &
           "c_user_id, " &
           "d_years " &
          "FROM " &
           "call_roll_user_dtl " &
          "WHERE " &
           "k_daily_pay_kind = :k_daily_pay_kind " &
          "GROUP BY " &
           "c_user_id, " &
           "d_years " &
          ") DTL " &
         "WHERE " &
          "HDR.d_daily_pay_close = :prev_daily_pay_close " &
          "AND HDR.s_next_balance_daily_pay_total <> 0 " &
          "AND DTL.c_user_id = HDR.c_user_id " &
          "AND DTL.d_years = HDR.d_years " &
         "GROUP BY " &
          "HDR.c_user_id, " &
          "HDR.d_daily_pay_close " &
         " ) " &
        ") call_roll_user_A " &
       "GROUP BY " &
        "call_roll_user_A.c_user_id " &
       ") call_roll_user_info , " & MyBase.GetStafAttributeView(":d_daily_pay_close", "staf_attribute", True, New String() {"*"}) & " " &
       "LEFT OUTER JOIN " &
        "qualification_view qualification " &
       "ON " &
        "staf_attribute.k_qualification = qualification.c_constant_seq " &
       "AND qualification.d_from<=:d_daily_pay_close AND :d_daily_pay_close<=qualification.d_to " &
       "LEFT OUTER JOIN " &
        "model_view model " &
       "ON " &
        "staf_attribute.k_model = model.c_constant_seq " &
       "AND model.d_from<=:d_daily_pay_close AND :d_daily_pay_close<=model.d_to " &
       "WHERE " &
        "call_roll_user_info.c_user_id = staf_attribute.c_user_id " &
      "ORDER BY " &
       "staf_attribute.k_belonging, " &
       "Long.Parse(Format(staf_attribute.c_staf_id, '9999999999')), " &
       "call_roll_user_info.c_user_id " & UtDb.DbOrderOffset & ";"), MyBase.GetNpgsqlConnection)    'ok
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                'command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                'command.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate

                'Dim command1 As New NpgsqlCommand("update daily_pay_calc_temp set d_daily_pay_close=:d_daily_pay_close,d_prev_daily_pay_close=:prev_daily_pay_close,c_ksh=:c_ksh,k_daily_pay_kind=:k_daily_pay_kind", MyBase.GetNpgsqlConnection)
                'command1.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'command1.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                'command1.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                'command1.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command1.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'command1.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                'command1.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                'command1.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                'command1.ExecuteNonQuery()
                'todo:
                Dim command2 As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                SetExecParam(command2, strDailyPayKind, strPrevCloseDate, strCloseDate, MDLoginInfo.Ksh)
                If strDailyPayKind = "04" Then
                    command2.SetSql("select * from sum_daily_pay_dgm_view")
                Else
                    command2.SetSql("select * from sum_daily_pay_view")
                End If
                Dim dReader As NpgsqlDataReader = command2.ExecuteReader
                If Not dReader.HasRows Then
                    'If strDailyPayKind <> "04" Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                    'Else
                    'Return set2
                    'End If
                End If
                Dim ds As New DataSet
                Dim strTable As String '= If(strDailyPayKind.Equals("01"), "committee_close_dtl", "branch_close_dtl")
                Select Case strDailyPayKind
                    Case "01"
                        strTable = "committee_close_dtl"
                    Case "02"
                        strTable = "branch_close_dtl"
                    Case Else
                        strTable = "dgm_close_dtl"
                End Select
                ds.Tables.Add(MyBase.CreateSomeDataSet(strTable, dReader))
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

        Public Function GetCommitteeCloseDtlDgm(ByVal strCloseDate As String, ByVal strPrevCloseDate As String, Optional ByVal fClosed As Boolean = False) As DataSet
            Dim set2 As DataSet
            Try
                Dim strSql As String
                strSql = "SELECT False AS print_check, DTL.s_day, DTL.s_day AS 開催日, " &
                             "DTL.c_user_id, DTL.c_user_id AS 個人認証ＩＤ, ATTR.c_staf_id, " &
                             "CLng(ATTR.c_staf_id) AS 社員番号, ATTR.staf_attribute_l_name, " &
                             "ATTR.staf_attribute_l_name AS 氏名, ATTR.k_qualification, " &
                             "ATTR.qualification_latest_view_l_omission_name AS 資格, ATTR.model_latest_view_l_name AS 機種, " &
                             "ATTR.k_belonging, ATTR.belonging_latest_view_l_name AS 支部, DTL.s_daily_pay AS sum_daily_pay, " &
                             "DTL.s_daily_pay AS 今回日当計, DTL.s_next_balance_daily_pay AS sum_balance_daily_pay, DTL.s_next_balance_daily_pay AS 前回差分計, " &
                             "DTL.s_daily_pay+DTL.s_next_balance_daily_pay AS 日当計 " &
                         "FROM call_roll_user_dtl AS DTL INNER JOIN staf_attribute_latest_view AS ATTR ON DTL.c_user_id = ATTR.c_user_id " &
                         "WHERE DTL.k_daily_pay_kind='04'"
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                SetExecParam(command, "04", strPrevCloseDate, strCloseDate, MDLoginInfo.Ksh)
                If fClosed Then
                    strSql += " AND IsNull(DTL.d_daily_pay_close) "
                    strSql += " AND Format(DTL.s_day, 'yyyyMMdd')>='" & MDLoginInfo.PeriodFrom & "'" &
                              " AND Format(DTL.s_day, 'yyyyMMdd')<='" & MDLoginInfo.PeriodTo & "'"
                Else
                    strSql += " AND Format(DTL.d_daily_pay_close, 'yyyyMMdd')='" & strCloseDate & "'"
                End If
                strSql += " ORDER BY DTL.s_day, CLng(ATTR.c_staf_id)" & UtDb.DbOrderOffset() & ";"
                'todo:
                command.SetSql(strSql)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("dgm_close_dtl", dReader))
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

        Public Function GetCommitteeCloseDtlPrint(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strPrevCloseDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim cmdText As String = "SELECT " & " " & "call_roll_user_AA.c_user_id AS c_user_id, " & "    " & "call_roll_user_AA.c_committee_id AS c_committee_id, " & "   " & "call_roll_user_AA.k_daily_pay_kind AS k_daily_pay_kind, " & "  " & "staf_attribute.c_staf_id AS c_staf_id, " & "   " & "staf_attribute.l_name AS l_name, " & " " & "staf_attribute.k_belonging AS k_belonging, " & " " & "staf_attribute.k_qualification AS k_qualification, " & "   " & "SUM(sum_daily_pay) AS sum_daily_pay, " & " " & "SUM(sum_balance_daily_pay) AS sum_balance_daily_pay FROM " & "    " & "(SELECT " & "      " & "call_roll_user_info.c_user_id AS c_user_id, " & "      " & "call_roll_user_info.c_committee_id AS c_committee_id, " & "        " & "call_roll_user_info.k_daily_pay_kind AS k_daily_pay_kind, " & "        " & "call_roll_user_info.d_daily_pay_close AS d_daily_pay_close, " & "       " & "call_roll_user_info.sum_daily_pay AS sum_daily_pay, " & "      " & "call_roll_user_info.sum_balance_daily_pay AS sum_balance_daily_pay, " & "      " & "MAX(staf_attribute.d_from) AS max_d_from " & " " & "FROM " & "     " & "(SELECT " & "          " & "call_roll_user_A.c_user_id AS c_user_id, " & "         " & "call_roll_user_A.c_committee_id AS c_committee_id, " & "            " & "call_roll_user_A.k_daily_pay_kind AS k_daily_pay_kind, " & "           " & "call_roll_user_A.d_daily_pay_close AS d_daily_pay_close, " & "           " & "COALESCE(SUM(call_roll_user_A.sum_daily_pay), 0) AS sum_daily_pay, " & "           " & "COALESCE(SUM(call_roll_user_A.sum_balance_daily_pay), 0) AS sum_balance_daily_pay " & "       " & "FROM " & "         " & "( " & "                " & "(SELECT " & "                  " & "HDR.c_user_id AS c_user_id, " & "                  " & "DTL.c_committee_id AS c_committee_id, " & "                    " & "DTL.k_daily_pay_kind AS k_daily_pay_kind, " & "                    " & "HDR.d_daily_pay_close AS d_daily_pay_close, " & "                   " & "SUM(DTL.s_daily_pay) AS sum_daily_pay, " & "                   " & "0 AS sum_balance_daily_pay " & "               " & "FROM " & "                 " & "call_roll_user HDR, " & "                  " & "(SELECT " & "                      " & "c_user_id, " & "                       " & "d_years, " & "                      " & "c_committee_id, " & "                      " & "k_daily_pay_kind, " & "                        " & "SUM(s_daily_pay) AS s_daily_pay " & "                    " & "FROM " & "                     " & "call_roll_user_dtl " & "                   " & "GROUP BY " & "                     " & "c_user_id, " & "                       " & "d_years, " & "                     " & "c_committee_id, " & "                      " & "k_daily_pay_kind " & "                 " & ") DTL " & "                " & "WHERE " & "                    " & "HDR.d_daily_pay_close = :d_daily_pay_close " & "                   " & "AND HDR.s_daily_pay_total <> 0 " & "                   " & "AND DTL.c_user_id = HDR.c_user_id " & "                    " & "AND DTL.d_years = HDR.d_years " & "                    " & "AND DTL.k_daily_pay_kind = :k_daily_pay_kind " & "              " & "GROUP BY " & "                 " & "HDR.c_user_id, " & "                   " & "DTL.c_committee_id, " & "                  " & "DTL.k_daily_pay_kind, " & "                  " & "HDR.d_daily_pay_close " & "                " & "ORDER BY " & "                 " & "HDR.c_user_id) " & "           " & "UNION ALL " & "                " & "(SELECT " & "                  " & "HDR.c_user_id AS c_user_id, " & "                  " & "DTL.c_committee_id AS c_committee_id, " & "                    " & "DTL.k_daily_pay_kind AS k_daily_pay_kind, " & "                 " & "HDR.d_daily_pay_close AS d_daily_pay_close, " & "                  " & "0 AS sum_daily_pay, " & "                  " & "SUM(DTL.s_next_balance_daily_pay) AS sum_balance_daily_pay " & "               " & "FROM " & "                 " & "call_roll_user HDR, " & "                  " & "(SELECT " & "                      " & "c_user_id, " & "                       " & "d_years, " & "                     " & "c_committee_id, " & "                      " & "k_daily_pay_kind, " & "                     " & "SUM(s_next_balance_daily_pay) AS s_next_balance_daily_pay " & "                    " & "FROM " & "                     " & "call_roll_user_dtl " & "                   " & "GROUP BY " & "                     " & "c_user_id, " & "                       " & "d_years, " & "                     " & "c_committee_id, " & "                       " & "k_daily_pay_kind " & "                 " & ") DTL " & "                " & "WHERE " & "                    " & "HDR.d_daily_pay_close = :prev_daily_pay_close " & "                  " & "AND HDR.s_next_balance_daily_pay_total <> 0 " & "                  " & "AND DTL.c_user_id = HDR.c_user_id " & "                   " & "AND DTL.d_years = HDR.d_years " & "                    " & "AND DTL.k_daily_pay_kind = :k_daily_pay_kind " & "             " & "GROUP BY " & "                 " & "HDR.c_user_id, " & "                   " & "DTL.c_committee_id, " & "                  " & "DTL.k_daily_pay_kind, " & "                    " & "HDR.d_daily_pay_close " & "             " & "ORDER BY " & "                 " & "HDR.c_user_id) " & "           " & ") call_roll_user_A " & "       " & "GROUP BY " & "         " & "call_roll_user_A.c_user_id, " & "            " & "call_roll_user_A.c_committee_id, " & "         " & "call_roll_user_A.k_daily_pay_kind, " & "           " & "call_roll_user_A.d_daily_pay_close " & "      " & "ORDER BY " & "         " & "call_roll_user_A.c_user_id, " & "          " & "call_roll_user_A.d_daily_pay_close " & "       " & ") call_roll_user_info " & "    " & "LEFT OUTER JOIN " & "      " & "staf_attribute " & "   " & "ON " & "       " & "staf_attribute.c_user_id = call_roll_user_info.c_user_id " & "     " & "AND staf_attribute.d_from < call_roll_user_info.d_daily_pay_close " & "    " & "GROUP BY " & "     " & "call_roll_user_info.c_user_id, " & "       " & "call_roll_user_info.c_committee_id, " & "       " & "call_roll_user_info.k_daily_pay_kind, " & "        " & "call_roll_user_info.d_daily_pay_close, " & "       " & "call_roll_user_info.sum_daily_pay, " & "     " & "call_roll_user_info.sum_balance_daily_pay " & "    " & ") call_roll_user_AA, " & " " & "staf_attribute WHERE " & " " & "call_roll_user_AA.c_user_id = staf_attribute.c_user_id " & "  " & "AND call_roll_user_AA.max_d_from = staf_attribute.d_from GROUP BY " & "    " & "call_roll_user_AA.c_user_id, " & " " & "call_roll_user_AA.c_committee_id, " & "    " & "call_roll_user_AA.k_daily_pay_kind, " & "  " & "staf_attribute.c_staf_id, " & "    " & "staf_attribute.l_name, " & "   " & "staf_attribute.k_belonging, " & "   " & "staf_attribute.k_qualification ORDER BY " & "  " & "staf_attribute.k_belonging, " & "  " & "Long.Parse(Format(staf_attribute.c_staf_id, '9999999999')), " & "    " & "call_roll_user_AA.c_user_id "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                'command.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                'command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                SetExecParam(command, strDailyPayKind, strPrevCloseDate, strCloseDate, MDLoginInfo.Ksh)
                If strDailyPayKind = "04" Then
                    Dim strSql As String
                    strSql = "SELECT DTL.c_user_id, DTL.c_committee_id, DTL.k_daily_pay_kind, DTL.d_daily_pay_close," &
                             "    ATTR.c_staf_id, ATTR.staf_attribute_l_name AS l_name, ATTR.k_belonging, ATTR.k_qualification," &
                             "    Sum(DTL.s_daily_pay) AS sum_daily_pay, Sum(DTL.s_next_balance_daily_pay) AS sum_balance_daily_pay " &
                             "FROM call_roll_user_dtl AS DTL " &
                             "INNER JOIN staf_attribute_latest_view AS ATTR ON DTL.c_user_id = ATTR.c_user_id " &
                             "WHERE DTL.c_committee_id='DGM' AND Format(DTL.d_daily_pay_close, 'yyyyMMdd')=:strCloseDate " &
                             "GROUP BY DTL.c_user_id, DTL.c_committee_id, DTL.k_daily_pay_kind, DTL.d_daily_pay_close," &
                             "ATTR.c_staf_id, ATTR.staf_attribute_l_name, ATTR.k_belonging, ATTR.k_qualification"
                    command.SetSql(strSql)
                    'command.SetSql("select * from sum_daily_pay_dgm_print_view where c_committee_id='DGM' and Format(d_daily_pay_close, 'yyyyMMdd')=:strCloseDate")
                    command.Parameters.Add(New NpgsqlParameter("strCloseDate", DbType.String))
                    command.Parameters.Item("strCloseDate").Value = strCloseDate
                Else
                    command.SetSql("select * from sum_daily_pay_commitee_print_view")
                End If
                'todo:
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                Dim strTable As String '= If(strDailyPayKind.Equals("01"), "committee_close_dtl_print", "branch_close_dtl_print")
                Select Case strDailyPayKind
                    Case "01"
                        strTable = "committee_close_dtl_print"
                    Case "02"
                        strTable = "branch_close_dtl_print"
                    Case Else
                        strTable = "dgm_close_dtl_print"
                End Select
                ds.Tables.Add(MyBase.CreateSomeDataSet(strTable, dReader))
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

        Public Function GetCommitteeCloseList(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strMasterDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim command As New NpgsqlCommand(("SELECT " & "    " & "daily_pay_close_AA.d_daily_pay_close, " & "    " & "TO_DATE(daily_pay_close_AA.d_daily_pay_close, 'yyyyMMdd') AS " & "締め日" & ", " & "    " & "daily_pay_close_AA.d_begin, " & "  " & "TO_CHAR(daily_pay_close_AA.d_begin, 'yyyy/MM') AS """ & "対象年月" & "(" & "始" & ")"", " & "    " & "daily_pay_close_AA.d_end, " & "    " & "TO_CHAR(daily_pay_close_AA.d_end, 'yyyy/MM') AS """ & "対象年月" & "(" & "終" & ")"", " & "    " & "daily_pay_close_AA.s_daily_pay, " & " " & "daily_pay_close_AA.s_daily_pay AS " & "日当額計" & ", " & "    " & "daily_pay_close_AA.d_ins, " & "    " & "daily_pay_close_AA.d_ins AS " & "登録年月日" & ", " & "    " & "daily_pay_close_AA.c_user_id_ins, " & "    " & "staf_view.l_name AS " & "担当者" & " FROM " & "    " & "(SELECT " & "      " & "daily_pay_close_A.d_daily_pay_close, " & "      " & "daily_pay_close_dtl_A.d_begin, " & "       " & "daily_pay_close_dtl_A.d_end, " & "     " & "daily_pay_close_A.s_daily_pay, " & "       " & "daily_pay_close_A.d_ins, " & "       " & "daily_pay_close_A.c_user_id_ins " & "  " & "FROM " & "     " & "(SELECT " & "          " & "d_daily_pay_close, " & "           " & "SUM(s_daily_pay_total) AS s_daily_pay, " & "          " & "d_ins, " & "           " & "c_user_id_ins " & "        " & "FROM " & "         " & "daily_pay_close " & "      " & "WHERE " & "            " & "c_ksh = :c_ksh " & "           " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "         " & "AND d_daily_pay_close LIKE '" & strCloseDate & "%' " & "        " & "GROUP BY " & "         " & "d_daily_pay_close, " & "           " & "d_ins, " & "           " & "c_user_id_ins " & "        " & ") daily_pay_close_A, " & "     " & "(SELECT " & "          " & "d_daily_pay_close, " & "           " & "MIN(d_years) AS d_begin, " & "         " & "MAX(d_years) AS d_end, " & "           " & "d_ins, " & "            " & "c_user_id_ins " & "        " & "FROM " & "         " & "daily_pay_close_dtl " & "      " & "WHERE " & "            " & "c_ksh = :c_ksh " & "           " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "       " & "GROUP BY " & "         " & "d_daily_pay_close, " & "           " & "d_ins, " & "           " & "c_user_id_ins " & "        " & ") daily_pay_close_dtl_A " & "  " & "WHERE " & "        " & "daily_pay_close_A.d_daily_pay_close = daily_pay_close_dtl_A.d_daily_pay_close " & "        " & "AND daily_pay_close_A.d_ins = daily_pay_close_dtl_A.d_ins " & "     " & "AND daily_pay_close_A.c_user_id_ins = daily_pay_close_dtl_A.c_user_id_ins " & "    " & ") daily_pay_close_AA LEFT OUTER JOIN " & "   " & "(SELECT " & "      " & "staf_attribute_full_time_view.c_user_id, " & "     " & "staf_attribute_full_time_view.l_name, " & "        " & "staf_attribute_full_time_view.d_from " & "    " & "FROM " & "     " & "staf_attribute_full_time_view, " & "       " & "(SELECT " & "          " & "c_user_id, " & "           " & "MAX(d_from) AS d_from " & "        " & "FROM " & "         " & "staf_attribute_full_time_view " & "        " & "WHERE " & "            " & "d_from <= :d_master_date " & "     " & "GROUP BY " & "          " & "c_user_id " & "        " & ") staf_attribute_full_time_max " & "   " & "WHERE " & "        " & "staf_attribute_full_time_view.c_user_id = staf_attribute_full_time_max.c_user_id " & "       " & "AND staf_attribute_full_time_view.d_from = staf_attribute_full_time_max.d_from " & "   " & ") staf_view ON " & "   " & "staf_view.c_user_id = daily_pay_close_AA.c_user_id_ins ORDER BY " & " " & "daily_pay_close_AA.d_daily_pay_close, " & "    " & "daily_pay_close_AA.d_begin, " & "  " & "d_end "), MyBase.GetNpgsqlConnection)
                Dim command As New NpgsqlCommand(("SELECT " & " " & "daily_pay_close_AA.d_daily_pay_close, " & "    " & "CONVERT(DATE,daily_pay_close_AA.d_daily_pay_close,112) AS " & "締め日" & ", " & " " & "daily_pay_close_AA.d_begin, " & "  " & "TO_CHAR(daily_pay_close_AA.d_begin, 'yyyy/MM') AS [対象年月(始)], " & "daily_pay_close_AA.d_end, " & "   " & "TO_CHAR(daily_pay_close_AA.d_end, 'yyyy/MM') AS [対象年月(終)], " & "daily_pay_close_AA.s_daily_pay, " & " " & "daily_pay_close_AA.s_daily_pay AS " & "日当額計" & ",daily_pay_close_AA.s_food_expenses,daily_pay_close_AA.s_food_expenses AS 昼食費計, " & "   " & "daily_pay_close_AA.d_ins, " & "    " & "daily_pay_close_AA.d_ins AS " & "登録年月日" & ", " & "    " & "daily_pay_close_AA.c_user_id_ins, " & "    " & "staf_view.l_name AS " & "担当者" & " FROM " & "    " & "(SELECT " & "      " & "daily_pay_close_A.d_daily_pay_close, " & "     " & "daily_pay_close_dtl_A.d_begin, " & "        " & "daily_pay_close_dtl_A.d_end, " & "     " & "daily_pay_close_A.s_daily_pay, daily_pay_close_A.s_food_expenses," & "       " & "daily_pay_close_A.d_ins, " & "     " & "daily_pay_close_A.c_user_id_ins " & "    " & "FROM " & "     " & "(SELECT " & "          " & "d_daily_pay_close, " & "           " & "SUM(s_daily_pay_total) AS s_daily_pay,SUM(s_food_expenses_total) AS s_food_expenses, " & "           " & "d_ins, " & "          " & "c_user_id_ins " & "        " & "FROM " & "         " & "daily_pay_close " & "      " & "WHERE " & "            " & "c_ksh = :c_ksh " & "           " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "         " & "AND d_daily_pay_close LIKE '" & strCloseDate & "%' " & "       " & "GROUP BY " & "         " & "d_daily_pay_close, " & "            " & "d_ins, " & "           " & "c_user_id_ins " & "        " & ") daily_pay_close_A, " & "     " & "(SELECT " & "          " & "d_daily_pay_close, " & "         " & "MIN(d_years) AS d_begin, " & "         " & "MAX(d_years) AS d_end, " & "           " & "d_ins, " & "           " & "c_user_id_ins " & "        " & "FROM " & "            " & "daily_pay_close_dtl " & "      " & "WHERE " & "            " & "c_ksh = :c_ksh " & "           " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "     " & "GROUP BY " & "         " & "d_daily_pay_close, " & "           " & "d_ins, " & "           " & "c_user_id_ins " & "        " & ") daily_pay_close_dtl_A " & "  " & "WHERE " & "     " & "daily_pay_close_A.d_daily_pay_close = daily_pay_close_dtl_A.d_daily_pay_close " & "        " & "AND daily_pay_close_A.d_ins = daily_pay_close_dtl_A.d_ins " & "      " & "AND daily_pay_close_A.c_user_id_ins = daily_pay_close_dtl_A.c_user_id_ins " & "    " & ") daily_pay_close_AA LEFT OUTER JOIN " & " " & "(SELECT " & "      " & "staf_attribute_full_time_view.c_user_id, " & "        " & "staf_attribute_full_time_view.l_name, " & "        " & "staf_attribute_full_time_view.d_from " & " " & "FROM " & "     " & "staf_attribute_full_time_view, " & "       " & "(SELECT " & "          " & "c_user_id, " & "           " & "MAX(staf_attribute_full_time_view.d_from) AS d_from " & "       " & "FROM " & "         " & "staf_attribute_full_time_view " & "        " & "WHERE " & "            " & "d_from <= :d_master_date " & "     " & "GROUP BY " & "         " & "c_user_id " & "        " & ") staf_attribute_full_time_max " & "   " & "WHERE " & "        " & "staf_attribute_full_time_view.c_user_id = staf_attribute_full_time_max.c_user_id " & "      " & "AND staf_attribute_full_time_view.d_from = staf_attribute_full_time_max.d_from " & "   " & ") staf_view ON " & "   " & "staf_view.c_user_id = daily_pay_close_AA.c_user_id_ins ORDER BY " & "    " & "daily_pay_close_AA.d_daily_pay_close, " & "    " & "daily_pay_close_AA.d_begin, " & "  " & "d_end " & UtDb.DbOrderOffset), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_master_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                command.Parameters.Item("d_master_date").Value = strMasterDate
                'todo:
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                Dim strTable As String '= If(strDailyPayKind.Equals("01"), "committee_close_list", "branch_close_list")
                Select Case strDailyPayKind
                    Case "01"
                        strTable = "committee_close_list"
                    Case "02"
                        strTable = "branch_close_list"
                    Case Else
                        strTable = "dgm_close_list"
                End Select
                ds.Tables.Add(MyBase.CreateSomeDataSet(strTable, dReader))
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

        Public Function GetExecutiveCloseDtl(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strPrevCloseDate As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim strArray As String() = New String() {"", _
      "UNION ALL" & _
       "(SELECT " & _
        "HDR.c_user_id AS c_user_id, " & _
        "HDR.d_daily_pay_close AS d_daily_pay_close, " & _
        "0 AS sum_daily_pay, " & _
        "0 AS sum_food_expenses, " & _
        "SUM(HDR.s_next_balance_daily_pay_total) AS sum_balance_daily_pay, " & _
        "SUM(HDR.s_next_balance_food_expenses_total) AS sum_balance_food_expenses " & _
       "FROM " & _
        "call_roll_user HDR, " & _
        "(SELECT " & _
         "c_user_id, " & _
         "d_years, " & _
         "c_committee_id, " & _
         "k_daily_pay_kind " & _
        "FROM " & _
         "call_roll_user_dtl " & _
        "GROUP BY " & _
         "c_user_id, " & _
         "d_years, " & _
         "c_committee_id, " & _
         "k_daily_pay_kind " & _
        ") DTL " & _
       "WHERE " & _
        "Format(HDR.d_daily_pay_close,'yyyyMMdd') = :prev_daily_pay_close " & _
        "AND (HDR.s_next_balance_daily_pay_total <> 0 " & _
         "OR HDR.s_next_balance_food_expenses_total <> 0) " & _
        "AND DTL.c_user_id = HDR.c_user_id " & _
        "AND DTL.d_years = HDR.d_years " & _
        "AND DTL.k_daily_pay_kind = :k_daily_pay_kind " & _
       "GROUP BY " & _
        "HDR.c_user_id, " & _
        "HDR.d_daily_pay_close " & _
       " )"}
                Dim index As Integer = If((DateTime.ParseExact(strPrevCloseDate, "yyyyMMdd", Nothing).AddDays(1).Month = DateTime.ParseExact(strDate, "yyyyMMdd", Nothing).Month), 1, 0)
                Dim command As New NpgsqlCommand(String.Concat(New String() {
      "SELECT " &
       "FALSE AS print_check, " &
       "call_roll_user_info.c_user_id AS c_user_id, " &
       "call_roll_user_info.c_user_id AS " & "個人認証ＩＤ" & ", " &
       "Long.Parse(Format(staf_attribute.c_staf_id, '9999999999')) AS c_staf_id, " &
       "staf_attribute.c_staf_id AS " & "社員番号" & ", " &
       "staf_attribute.l_name AS l_name, " &
       "staf_attribute.l_name AS " & "氏名" & ", " &
       "staf_attribute.k_belonging AS k_belonging, " &
       "staf_attribute.k_qualification AS k_qualification, " &
       "qualification.l_omission_name AS " & "資格" & ", " &
       "staf_attribute.k_model AS k_model, " &
       "model.l_name AS " & "機種" & "," &
       "call_roll_user_info.sum_daily_pay AS sum_daily_pay, " &
       "call_roll_user_info.sum_daily_pay AS " & "当月日当計" & ", " &
       "call_roll_user_info.sum_food_expenses AS sum_food_expenses, " &
       "call_roll_user_info.sum_food_expenses AS " & "中執昼食費計" & ", " &
       "call_roll_user_info.sum_balance_daily_pay AS sum_balance_daily_pay, " &
       "call_roll_user_info.sum_balance_daily_pay AS " & "前回差分計" & ", " &
       "call_roll_user_info.sum_balance_food_expenses AS sum_balance_food_expenses, " &
       "call_roll_user_info.sum_balance_food_expenses AS " & "前回差分昼食費計" & ", " &
       "call_roll_user_info.sum_daily_pay + " &
        "call_roll_user_info.sum_food_expenses + " &
        "call_roll_user_info.sum_balance_daily_pay + " &
        "call_roll_user_info.sum_balance_food_expenses AS " & "日当計" & " " &
      "FROM " &
       "(SELECT " &
        "call_roll_user_A.c_user_id AS c_user_id," &
        "COALESCE(SUM(call_roll_user_A.sum_daily_pay), 0) AS sum_daily_pay, " &
        "COALESCE(SUM(call_roll_user_A.sum_food_expenses), 0) AS sum_food_expenses, " &
        "COALESCE(SUM(call_roll_user_A.sum_balance_daily_pay), 0) AS sum_balance_daily_pay, " &
        "COALESCE(SUM(call_roll_user_A.sum_balance_food_expenses), 0) AS sum_balance_food_expenses " &
       "FROM " &
        "( " &
         "(SELECT " &
          "HDR.c_user_id AS c_user_id, " &
          "HDR.d_daily_pay_close AS d_daily_pay_close, " &
          "SUM(HDR.s_daily_pay_total) AS sum_daily_pay, " &
          "SUM(HDR.s_food_expenses_total) AS sum_food_expenses, " &
          "0 AS sum_balance_daily_pay, " &
          "0 AS sum_balance_food_expenses " &
         "FROM " &
          "call_roll_user HDR, " &
          "(SELECT " &
           "c_user_id, " &
           "d_years, " &
           "c_committee_id, " &
           "k_daily_pay_kind " &
          "FROM " &
           "call_roll_user_dtl " &
          "GROUP BY " &
           "c_user_id, " &
           "d_years, " &
           "c_committee_id, " &
           "k_daily_pay_kind " &
          ") DTL " &
         "WHERE " &
          "Format(HDR.d_daily_pay_close,'yyyyMMdd') = :d_daily_pay_close " &
          "AND (HDR.s_daily_pay_total <> 0 OR HDR.s_food_expenses_total <> 0 OR HDR.s_next_balance_daily_pay_total <> 0 OR HDR.s_next_balance_food_expenses_total <> 0) " &
          "AND Format(HDR.d_years,'yyyyMMdd') = :d_date " &
          "AND DTL.c_user_id = HDR.c_user_id " &
          "AND DTL.d_years = HDR.d_years " &
          "AND DTL.k_daily_pay_kind = :k_daily_pay_kind " &
         "GROUP BY " &
          "HDR.c_user_id, " &
          "HDR.d_daily_pay_close " &
         ")" &
       "," & strArray(index) & ", " &
        ") call_roll_user_A " &
       "GROUP BY " &
        "call_roll_user_A.c_user_id" &
       ") call_roll_user_info , " & MyBase.GetStafAttributeView(":d_daily_pay_close", "staf_attribute", True, New String() {"*"}) &
       "LEFT OUTER JOIN " &
        "qualification_view qualification " &
       "ON " &
        "staf_attribute.k_qualification = qualification.c_constant_seq " &
       "AND :d_daily_pay_close BETWEEN qualification.d_from AND qualification.d_to " &
       "LEFT OUTER JOIN " &
        "model_view model " &
       "ON " &
        "staf_attribute.k_model = model.c_constant_seq " &
       "AND :d_daily_pay_close BETWEEN model.d_from AND model.d_to " &
       "WHERE " &
        "call_roll_user_info.c_user_id = staf_attribute.c_user_id " &
      "ORDER BY " &
       "staf_attribute.k_belonging, " &
       "Long.Parse(Format(staf_attribute.c_staf_id, '9999999999')), " &
       "call_roll_user_info.c_user_id" & UtDb.DbOrderOffset & "; "}), MyBase.GetNpgsqlConnection)
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("prev_daily_pay_close", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                'command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                'command.Parameters.Item("prev_daily_pay_close").Value = strPrevCloseDate
                'command.Parameters.Item("d_date").Value = strDate
                'Dim dReader As NpgsqlDataReader = command.ExecuteReader
                'todo:
                Dim command1 As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                SetExecParam(command1, strDailyPayKind, strDate, strCloseDate, MDLoginInfo.Ksh)
                command1.SetSql("select * from sum_exective_daily_pay_detail_view")
                Dim dReader As NpgsqlDataReader = command1.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("executive_close_dtl", dReader))
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

        Public Function GetExecutiveCloseList(ByVal strDailyPayKind As String, ByVal strCloseDate As String, ByVal strMasterDate As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim command As New NpgsqlCommand(("SELECT " & "    " & "daily_pay_close_dtl_A.d_daily_pay_close, " & " " & "CONVERT(DATE,daily_pay_close_dtl_A.d_daily_pay_close,112) AS " & "締め日" & ", " & " " & "daily_pay_close_dtl_A.d_years, " & "   " & "daily_pay_close_dtl_A.d_years AS " & "精算年月" & ", " & " " & "daily_pay_close_dtl_A.s_daily_pay, " & " " & "daily_pay_close_dtl_A.s_balance_daily_pay, " & "   " & "daily_pay_close_dtl_A.s_daily_pay_total, " & " " & "daily_pay_close_dtl_A.s_daily_pay_total AS " & "中執日当額計" & ", " & "  " & "daily_pay_close_dtl_A.s_food_expenses, " & "   " & "daily_pay_close_dtl_A.s_balance_food_expenses, " & "   " & "daily_pay_close_dtl_A.s_food_expenses_total, " & " " & "daily_pay_close_dtl_A.s_food_expenses_total AS " & "中執昼食費計" & ", " & "   " & "daily_pay_close_dtl_A.d_ins, " & " " & "daily_pay_close_dtl_A.d_ins AS " & "登録年月日" & ", " & "  " & "daily_pay_close_dtl_A.c_user_id_ins, " & " " & "staf_view.l_name AS " & "担当者" & " FROM " & "    " & "(SELECT " & "        " & "d_daily_pay_close, " & "       " & "d_years, " & "     " & "SUM(daily_pay_close_dtl.s_daily_pay) AS s_daily_pay, " & "     " & "SUM(daily_pay_close_dtl.s_balance_daily_pay) AS s_balance_daily_pay, " & "        " & "SUM(s_daily_pay + s_balance_daily_pay) AS s_daily_pay_total, " & "     " & "SUM(daily_pay_close_dtl.s_food_expenses) AS s_food_expenses, " & "     " & "SUM(daily_pay_close_dtl.s_balance_food_expenses) AS s_balance_food_expenses, " & "     " & "SUM(s_food_expenses + s_balance_food_expenses) AS s_food_expenses_total, " & "      " & "d_ins, " & "       " & "c_user_id_ins " & "    " & "FROM " & "     " & "daily_pay_close_dtl " & "  " & "WHERE " & "        " & "c_ksh = :c_ksh " & "     " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "     " & "AND d_daily_pay_close LIKE '" & strCloseDate & "%' " & "   " & "GROUP BY " & "     " & "d_daily_pay_close, " & "       " & "d_years, " & "     " & "d_ins, " & "       " & "c_user_id_ins " & "    " & ") daily_pay_close_dtl_A LEFT OUTER JOIN " & "  " & "(SELECT " & "       " & "staf_attribute_full_time_view.c_user_id, " & "     " & "staf_attribute_full_time_view.l_name, " & "        " & "staf_attribute_full_time_view.d_from " & "   " & "FROM " & "     " & "staf_attribute_full_time_view, " & "       " & "(SELECT " & "          " & "c_user_id, " & "           " & "MAX(staf_attribute_full_time_view.d_from) AS d_from " & "     " & "FROM " & "         " & "staf_attribute_full_time_view " & "        " & "WHERE " & "            " & "d_from <= :d_master_date " & "     " & "GROUP BY " & "         " & "c_user_id " & "        " & ") staf_attribute_full_time_max " & "   " & "WHERE " & "        " & "staf_attribute_full_time_view.c_user_id = staf_attribute_full_time_max.c_user_id " & "      " & "AND staf_attribute_full_time_view.d_from = staf_attribute_full_time_max.d_from " & "   " & ") staf_view ON " & "   " & "staf_view.c_user_id = daily_pay_close_dtl_A.c_user_id_ins ORDER BY " & " " & "daily_pay_close_dtl_A.d_years"), MyBase.GetNpgsqlConnection)
                'command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("d_master_date", DbType.String))
                'command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                'command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                'command.Parameters.Item("d_master_date").Value = strMasterDate
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                SetExecParam(command, strDailyPayKind, strCloseDate, strCloseDate, MDLoginInfo.Ksh)
                command.SetSql("select * from sum_exective_daily_pay_view")
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("executive_close_list", dReader))
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

        Public Function GetPrev2DailyPayClose(ByVal strDailyPayKind As String) As DataSet
            Dim set2 As DataSet
            Try
                'Dim cmdText As String = "SELECT " & "  " & "A.max, " & "   " & "MAX(B.d_daily_pay_close) AS next_max FROM " & "    " & "(SELECT " & "      " & "MAX(d_daily_pay_close) AS max " & " " & "FROM " & "     " & "daily_pay_close " & "  " & "WHERE " & "        " & "c_ksh = :c_ksh " & "       " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "   " & ") A LEFT OUTER JOIN " & "  " & "daily_pay_close B ON " & " " & "B.c_ksh = :c_ksh " & " " & "AND B.k_daily_pay_kind = :k_daily_pay_kind " & "   " & "AND B.d_daily_pay_close < A.max GROUP BY " & "    " & "A.max "
                Dim cmdText As String = "SELECT a.[max], MAX(b.d_daily_pay_close) AS next_max FROM (SELECT MAX(d_daily_pay_close) AS [max] FROM daily_pay_close WHERE k_daily_pay_kind=:k_daily_pay_kind AND c_ksh=:c_ksh) AS a, daily_pay_close AS b WHERE k_daily_pay_kind=:k_daily_pay_kind AND c_ksh=:c_ksh AND a.max>b.d_daily_pay_close GROUP BY a.max, b.d_daily_pay_close ORDER BY b.d_daily_pay_close DESC" & UtDb.DbOrderOffset
                'todo:
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("prev_daily_pay_close", dReader))
                If String.IsNullOrEmpty(ds.Tables.Item("prev_daily_pay_close").Rows.Item(0).Item("max").ToString) Then
                    Return Nothing
                End If
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetPrevDailyPayClose(ByVal strDailyPayKind As String, ByVal strCloseDate As String) As String
            Dim str2 As String
            Try
                Dim cmdText As String = "SELECT " & "   " & "MAX(d_daily_pay_close) AS prev_max FROM " & "  " & "daily_pay_close WHERE " & "    " & "c_ksh = :c_ksh " & "   " & "AND k_daily_pay_kind = :k_daily_pay_kind " & "  " & "AND d_daily_pay_close < :d_daily_pay_close "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_daily_pay_close", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_daily_pay_kind").Value = strDailyPayKind
                command.Parameters.Item("d_daily_pay_close").Value = strCloseDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                str2 = MyBase.CreateSomeDataSet("prev_daily_pay_close", dReader).Rows.Item(0).Item("prev_max").ToString
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

    End Class
End Namespace
