Imports UnionAct.DAO.FinancialAffairs
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports log4net
Imports UnionAct.DAO.Master
Imports UnionAct.NSMDInfo
Imports UnionAct.Framework

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Collections
Imports System.Data
Imports System.Reflection
Imports System.Text

Namespace DAO.FinancialAffairs.WithHolding
    Public Class WithHoldingDao
        Inherits FinancialAffairsBaseDao

        ' Methods
        Public Sub New()
        End Sub

        Public Sub New(ByVal strCut As String)
            Me._strCut = strCut
        End Sub

        Private Sub AddCutDivParameterValue( _
            ByRef command As NpgsqlCommand _
        )
            Try
                command.Parameters.Add("k_daily_pay_kind", Me._strCut)
                command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        ''' <summary>
        ''' 源泉徴収集計処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="TruncPlace">切捨て桁数</param>
        ''' <param name="UserId">作成者個人ＩＤ</param>
        ''' <returns>件数</returns>
        ''' <remarks></remarks>
        Public Function Calcuration( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal TruncPlace As Integer, _
            ByVal UserId As String _
        ) As Integer

            ' トランザクション開始
            Dim objTran As NpgsqlTransaction = MyBase.GetNpgsqlConnection.BeginTransaction
            Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection, objTran)
            Dim strSql As String = ""
            Dim num As Integer

            Try
                ' 既存レコード削除
                DeleteCurrentRec(command, Me._strCut, TargetYM)

                ' 月例賃金カット・一時金カットデータから源泉徴収基礎データを作成
                CreateWidthHoldingBase(command, CompanyCode, TargetYM, TruncPlace)

                ' 役員手当金額計算
                'If Me._strCut = UnionConst.DAILY_PAY_KIND_PAYCUT Then
                GetOfficerPay(command, CompanyCode, TargetYM, CriterionDate, TruncPlace, UserId)
                'End If

                ' 源泉徴収額計算
                num = CalcTaxation(command, CompanyCode, TargetYM, CriterionDate, TruncPlace, UserId)

                ' コミット
                objTran.Commit()

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch ex As Exception
                ' ロールバック
                objTran.Rollback()
                Throw ex
                'MsgBox(ex.Message)
            End Try
            Return num
        End Function
        'Public Function Calcuration(ByVal CompanyCode As String, ByVal TargetYM As String, ByVal CriterionDate As String, ByVal TruncPlace As Integer, ByVal UserId As String) As Integer
        '    Dim num As Integer
        '    Try
        '        Dim message As String = "ExecWithholding(:CompanyCode, :TargetYM, :CriterionDate, :TruncPlace, :UserId, :CutDiv)"
        '        WithHoldingDao._logger.Debug(message)
        '        Dim command As New NpgsqlCommand(message, MyBase.GetNpgsqlConnection) With { _
        '            .CommandType = CommandType.StoredProcedure _
        '        }
        '        command.Parameters.Add(New NpgsqlParameter("CompanyCode", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("CriterionDate", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("TruncPlace", DbType.Int32))
        '        command.Parameters.Add(New NpgsqlParameter("UserId", DbType.String))
        '        command.Parameters.Item("CompanyCode").Value = CompanyCode
        '        command.Parameters.Item("TargetYM").Value = TargetYM
        '        command.Parameters.Item("CriterionDate").Value = CriterionDate
        '        command.Parameters.Item("TruncPlace").Value = TruncPlace
        '        command.Parameters.Item("UserId").Value = UserId
        '        command.Parameters.Add("CutDiv", Me._strCut)
        '        Dim obj2 As Object = command.ExecuteScalar
        '        num = If(((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull), 0, CInt(obj2))
        '    Catch exception As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
        '    Catch exception2 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
        '    End Try
        '    Return num
        'End Function

        ''' <summary>
        ''' 源泉徴収データ削除
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="strCut">日当計算区分</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <remarks></remarks>
        Private Sub DeleteCurrentRec( _
            ByVal command As NpgsqlCommand, _
            ByVal strCut As String, _
            ByVal TargetYM As String _
        )
            Dim strSql As String = ""

            ' SQL文作成
            strSql = ""
            strSql += "DELETE" & vbCrLf
            strSql += "  FROM taxation_total" & vbCrLf                                      ' 源泉徴収テーブル
            strSql += " WHERE FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf               ' 集計年月と同じもの
            strSql += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf                ' 日当計算区分と同じもの

            ' バインド変数追加
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))          ' 集計年月
            command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))  ' 日当計算区分

            ' バインド変数値設定
            command.Parameters.Item("TargetYM").Value = TargetYM                            ' 集計年月
            command.Parameters.Item("k_daily_pay_kind").Value = strCut                      ' 日当計算区分

            ' SQL文設定
            command.SetSql(strSql)

            ' SQL実行
            command.ExecuteNonQuery()
        End Sub

        ''' <summary>
        ''' 源泉徴収基礎データ作成処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="TruncPlace">切捨て桁数</param>
        ''' <remarks>月例賃金カット・一時金カットデータから源泉徴収基礎データを作成</remarks>
        Private Sub CreateWidthHoldingBase( _
            ByVal command As NpgsqlCommand, _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal TruncPlace As Integer _
        )
            Dim strSql05 As String = ""
            Dim strSql06 As String = ""

            ' UPD 2016/07/28 カラム追加（課税フラグ） Start
            ' 月例集計
            strSql05 = ""
            strSql05 += "INSERT INTO taxation_total(" & vbCrLf          ' 源泉徴収テーブル
            strSql05 += "    d_years" & vbCrLf                          ' 01. 集計年月
            strSql05 += "   ,k_daily_pay_kind" & vbCrLf                 ' 02. 日当計算区分
            strSql05 += "   ,c_user_id" & vbCrLf                        ' 03. 個人認証ID
            strSql05 += "   ,s_pay_time_cut_monthly" & vbCrLf           ' 04. 月例賃金控除額
            strSql05 += "   ,s_pay_strike_cut_monthly" & vbCrLf         ' 05. 月例賃金控除額（ストライキ）
            strSql05 += "   ,s_pay_time_cut_once" & vbCrLf              ' 06. 一時金控除額
            strSql05 += "   ,s_pay_strike_cut_once" & vbCrLf            ' 07. 一時金控除額（ストライキ）
            strSql05 += "   ,s_pay_time_cut_monthly_break" & vbCrLf     ' 08. 切捨て額（月例控除）
            strSql05 += "   ,s_pay_strike_cut_monthly_break" & vbCrLf   ' 09. 切捨て額（月例ストライキ控除）
            strSql05 += "   ,s_pay_time_cut_once_break" & vbCrLf        ' 10. 切捨て額（一時金控除）
            strSql05 += "   ,s_pay_strike_cut_once_break" & vbCrLf      ' 11. 切捨て額（一時金ストライキ控除）
            strSql05 += "   ,c_taxation_flag" & vbCrLf                  ' 12. 課税フラグ('0' : 役員手当のみ課税, '1' : 役員手当月例控除とも課税)
            strSql05 += ")" & vbCrLf
            strSql05 += "SELECT d_years" & vbCrLf
            strSql05 += "      ,cut_div" & vbCrLf
            strSql05 += "      ,c_user_id" & vbCrLf
            strSql05 += "      ,SUM(pay_time_cut_monthly) AS sum_time_cut_monthly" & vbCrLf
            strSql05 += "      ,SUM(pay_strike_cut_monthly) AS sum_strike_cut_monthly" & vbCrLf
            strSql05 += "      ,SUM(pay_time_cut_once) AS sum_time_cut_once" & vbCrLf
            strSql05 += "      ,SUM(pay_strike_cut_once) AS sum_strike_cut_once" & vbCrLf
            strSql05 += "      ,sum_time_cut_monthly -" & MDFinanceCommon.Trunc("sum_time_cut_monthly", TruncPlace) & " AS sum_time_cut_monthly_break" & vbCrLf
            strSql05 += "      ,sum_strike_cut_monthly - " & MDFinanceCommon.Trunc("sum_strike_cut_monthly", TruncPlace) & " AS sum_strike_cut_monthly_break" & vbCrLf
            strSql05 += "      ,sum_time_cut_once - " & MDFinanceCommon.Trunc("sum_time_cut_once", TruncPlace) & " AS sum_time_cut_once_break" & vbCrLf
            strSql05 += "      ,sum_strike_cut_once - " & MDFinanceCommon.Trunc("sum_strike_cut_once", TruncPlace) & " AS sum_strike_cut_once_break" & vbCrLf
            strSql05 += "      ,'1'" & vbCrLf
            strSql05 += "  FROM (" & vbCrLf
            strSql05 += "        SELECT d_years" & vbCrLf
            strSql05 += "              ,'05' AS cut_div" & vbCrLf
            strSql05 += "              ,c_user_id" & vbCrLf
            strSql05 += "              ,pay_strike_cut AS pay_strike_cut_monthly" & vbCrLf
            strSql05 += "              ,pay_time_cut AS pay_time_cut_monthly" & vbCrLf
            strSql05 += "              ,0 AS pay_strike_cut_once" & vbCrLf
            strSql05 += "              ,0 AS pay_time_cut_once" & vbCrLf
            strSql05 += "          FROM (" & vbCrLf
            strSql05 += "                SELECT c_user_id" & vbCrLf
            strSql05 += "                      ,d_years" & vbCrLf
            strSql05 += "                      ,s_pay_cut AS pay_strike_cut" & vbCrLf
            strSql05 += "                      ,0 AS pay_time_cut" & vbCrLf
            strSql05 += "                  FROM pay_strike_cut_monthly" & vbCrLf
            strSql05 += "                UNION " & vbCrLf
            strSql05 += "                SELECT c_user_id" & vbCrLf
            strSql05 += "                      ,d_years" & vbCrLf
            strSql05 += "                      ,0 AS pay_cut_strike" & vbCrLf
            strSql05 += "                      ,s_pay_cut AS pay_time_cut" & vbCrLf
            strSql05 += "                  FROM pay_time_cut_monthly" & vbCrLf
            strSql05 += "               ) AS CUT_M" & vbCrLf
            strSql05 += "       ) AS CUT" & vbCrLf
            strSql05 += " WHERE FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
            strSql05 += " GROUP BY c_user_id" & vbCrLf
            strSql05 += "         ,cut_div" & vbCrLf
            strSql05 += "         ,d_years" & vbCrLf
            strSql05 += " ORDER BY c_user_id" & vbCrLf
            strSql05 += "         ,cut_div" & vbCrLf
            strSql05 += "         ,d_years" & vbCrLf
            strSql05 += ";" & vbCrLf
            'Dim strSql05 As String = "INSERT INTO taxation_total(d_years,k_daily_pay_kind,c_user_id,s_pay_time_cut_monthly," & _
            '                         "s_pay_strike_cut_monthly,s_pay_time_cut_once,s_pay_strike_cut_once," & _
            '                         "s_pay_time_cut_monthly_break,s_pay_strike_cut_monthly_break," & _
            '                         "s_pay_time_cut_once_break,s_pay_strike_cut_once_break) " & _
            '                         "SELECT d_years, cut_div, c_user_id," & _
            '                           "SUM(pay_time_cut_monthly) AS sum_time_cut_monthly, SUM(pay_strike_cut_monthly) AS sum_strike_cut_monthly," & _
            '                           "SUM(pay_time_cut_once) AS sum_time_cut_once, SUM(pay_strike_cut_once) AS sum_strike_cut_once," & _
            '                           "sum_time_cut_monthly -" & MDFinanceCommon.Trunc("sum_time_cut_monthly", TruncPlace) & " AS sum_time_cut_monthly_break," & _
            '                           "sum_strike_cut_monthly - " & MDFinanceCommon.Trunc("sum_strike_cut_monthly", TruncPlace) & " AS sum_strike_cut_monthly_break," & _
            '                           "sum_time_cut_once - " & MDFinanceCommon.Trunc("sum_time_cut_once", TruncPlace) & " AS sum_time_cut_once_break," & _
            '                           "sum_strike_cut_once - " & MDFinanceCommon.Trunc("sum_strike_cut_once", TruncPlace) & " AS sum_strike_cut_once_break " & _
            '                           "FROM (" & _
            '                             "SELECT " & _
            '                               "d_years, '05' AS cut_div, c_user_id, pay_strike_cut AS pay_strike_cut_monthly, pay_time_cut AS pay_time_cut_monthly," & _
            '                               "0 AS pay_strike_cut_once, 0 AS pay_time_cut_once " & _
            '                               "FROM (" & _
            '                                 "SELECT c_user_id,d_years,s_pay_cut AS pay_strike_cut, 0 AS pay_time_cut FROM pay_strike_cut_monthly " & _
            '                                 "UNION " & _
            '                                 "SELECT c_user_id,d_years,0 AS pay_cut_strike, s_pay_cut AS pay_time_cut FROM pay_time_cut_monthly " & _
            '                               ") AS CUT_M " & _
            '                              ")  AS CUT " & _
            '                         "WHERE FORMAT(d_years, 'yyyyMM')=:TargetYM" & _
            '                         "GROUP BY c_user_id, cut_div, d_years " & _
            '                         "ORDER BY c_user_id, cut_div, d_years;"

            ' 一時金集計用
            strSql06 = ""
            strSql06 += "INSERT INTO taxation_total(" & vbCrLf
            strSql06 += "    d_years" & vbCrLf                          ' 01. 集計年月
            strSql06 += "   ,k_daily_pay_kind" & vbCrLf                 ' 02. 日当計算区分
            strSql06 += "   ,c_user_id" & vbCrLf                        ' 03. 個人認証ID
            strSql06 += "   ,s_pay_time_cut_monthly" & vbCrLf           ' 04. 月例賃金控除額
            strSql06 += "   ,s_pay_strike_cut_monthly" & vbCrLf         ' 05. 月例賃金控除額（ストライキ）
            strSql06 += "   ,s_pay_time_cut_once" & vbCrLf              ' 06. 一時金控除額
            strSql06 += "   ,s_pay_strike_cut_once" & vbCrLf            ' 07. 一時金控除額（ストライキ）
            strSql06 += "   ,s_pay_time_cut_monthly_break" & vbCrLf     ' 08. 切捨て額（月例控除）
            strSql06 += "   ,s_pay_strike_cut_monthly_break" & vbCrLf   ' 09. 切捨て額（月例ストライキ控除）
            strSql06 += "   ,s_pay_time_cut_once_break" & vbCrLf        ' 10. 切捨て額（一時金控除）
            strSql06 += "   ,s_pay_strike_cut_once_break" & vbCrLf      ' 11. 切捨て額（一時金ストライキ控除）
            strSql06 += "   ,c_pay_once_name" & vbCrLf                  ' 12. 一時金名称
            strSql06 += "   ,c_taxation_flag" & vbCrLf                  ' 13. 課税フラグ('0' : 役員手当のみ課税, '1' : 役員手当月例控除とも課税)
            strSql06 += ")" & vbCrLf
            strSql06 += "SELECT d_years" & vbCrLf
            strSql06 += "      ,cut_div" & vbCrLf
            strSql06 += "      ,c_user_id" & vbCrLf
            strSql06 += "      ,SUM(pay_time_cut_monthly) AS sum_time_cut_monthly" & vbCrLf
            strSql06 += "      ,SUM(pay_strike_cut_monthly) AS sum_strike_cut_monthly" & vbCrLf
            strSql06 += "      ,SUM(pay_time_cut_once) AS sum_time_cut_once" & vbCrLf
            strSql06 += "      ,SUM(pay_strike_cut_once) AS sum_strike_cut_once" & vbCrLf
            strSql06 += "      ,sum_time_cut_monthly -" & MDFinanceCommon.Trunc("sum_time_cut_monthly", TruncPlace) & " AS sum_time_cut_monthly_break" & vbCrLf
            strSql06 += "      ,sum_strike_cut_monthly - " & MDFinanceCommon.Trunc("sum_strike_cut_monthly", TruncPlace) & " AS sum_strike_cut_monthly_break" & vbCrLf
            strSql06 += "      ,sum_time_cut_once - " & MDFinanceCommon.Trunc("sum_time_cut_once", TruncPlace) & " AS sum_time_cut_once_break" & vbCrLf
            strSql06 += "      ,sum_strike_cut_once - " & MDFinanceCommon.Trunc("sum_strike_cut_once", TruncPlace) & " AS sum_strike_cut_once_break" & vbCrLf
            strSql06 += "      ,c_pay_once_name" & vbCrLf
            strSql06 += "      ,'1'" & vbCrLf
            strSql06 += "  FROM (" & vbCrLf
            strSql06 += "        SELECT d_years" & vbCrLf
            strSql06 += "              ,'06' AS cut_div" & vbCrLf
            strSql06 += "              ,c_user_id" & vbCrLf
            strSql06 += "              ,0 AS pay_strike_cut_monthly" & vbCrLf
            strSql06 += "              ,0 AS pay_time_cut_monthly" & vbCrLf
            strSql06 += "              ,pay_strike_cut AS pay_strike_cut_once" & vbCrLf
            strSql06 += "              ,pay_time_cut AS pay_time_cut_once" & vbCrLf
            strSql06 += "              ,c_pay_once_name" & vbCrLf
            strSql06 += "          FROM (" & vbCrLf
            strSql06 += "                SELECT c_user_id" & vbCrLf
            strSql06 += "                      ,d_years" & vbCrLf
            strSql06 += "                      ,s_pay_cut AS pay_strike_cut" & vbCrLf
            strSql06 += "                      ,0 AS pay_time_cut" & vbCrLf
            strSql06 += "                      ,c_pay_once_name" & vbCrLf
            strSql06 += "                  FROM pay_strike_cut_once" & vbCrLf
            strSql06 += "                  UNION" & vbCrLf
            strSql06 += "                SELECT c_user_id" & vbCrLf
            strSql06 += "                      ,d_years" & vbCrLf
            strSql06 += "                      ,0 AS pay_cut_strike" & vbCrLf
            strSql06 += "                      ,s_pay_cut AS pay_time_cut" & vbCrLf
            strSql06 += "                      ,c_pay_once_name" & vbCrLf
            strSql06 += "                  FROM pay_time_cut_once" & vbCrLf
            strSql06 += "               ) AS CUT_O" & vbCrLf
            strSql06 += "       ) AS CUT" & vbCrLf
            strSql06 += " WHERE FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
            strSql06 += " GROUP BY c_user_id" & vbCrLf
            strSql06 += "         ,cut_div" & vbCrLf
            strSql06 += "         ,d_years" & vbCrLf
            strSql06 += "         ,c_pay_once_name" & vbCrLf
            strSql06 += " ORDER BY c_user_id" & vbCrLf
            strSql06 += "         ,cut_div" & vbCrLf
            strSql06 += "         ,d_years" & vbCrLf
            strSql06 += ";" & vbCrLf
            ' MOD 2012/06/15
            'Dim strSql06 As String = "INSERT INTO taxation_total(d_years,k_daily_pay_kind,c_user_id,s_pay_time_cut_monthly," & _
            '                         "s_pay_strike_cut_monthly,s_pay_time_cut_once,s_pay_strike_cut_once," & _
            '                         "s_pay_time_cut_monthly_break,s_pay_strike_cut_monthly_break," & _
            '                         "s_pay_time_cut_once_break,s_pay_strike_cut_once_break,c_pay_once_name) " & _
            '                         "SELECT d_years, cut_div, c_user_id," & _
            '                           "SUM(pay_time_cut_monthly) AS sum_time_cut_monthly, SUM(pay_strike_cut_monthly) AS sum_strike_cut_monthly," & _
            '                           "SUM(pay_time_cut_once) AS sum_time_cut_once, SUM(pay_strike_cut_once) AS sum_strike_cut_once," & _
            '                           "sum_time_cut_monthly -" & MDFinanceCommon.Trunc("sum_time_cut_monthly", TruncPlace) & " AS sum_time_cut_monthly_break," & _
            '                           "sum_strike_cut_monthly - " & MDFinanceCommon.Trunc("sum_strike_cut_monthly", TruncPlace) & " AS sum_strike_cut_monthly_break," & _
            '                           "sum_time_cut_once - " & MDFinanceCommon.Trunc("sum_time_cut_once", TruncPlace) & " AS sum_time_cut_once_break," & _
            '                           "sum_strike_cut_once - " & MDFinanceCommon.Trunc("sum_strike_cut_once", TruncPlace) & " AS sum_strike_cut_once_break," & _
            '                           "c_pay_once_name " & _
            '                           "FROM " & _
            '                           "(" & _
            '                             "SELECT " & _
            '                               "d_years, '06' AS cut_div, c_user_id, 0 AS pay_strike_cut_monthly, 0 AS pay_time_cut_monthly, " & _
            '                               "pay_strike_cut AS pay_strike_cut_once, pay_time_cut AS pay_time_cut_once,c_pay_once_name " & _
            '                               "FROM ( " & _
            '                                 "SELECT c_user_id,d_years,s_pay_cut AS pay_strike_cut, 0 AS pay_time_cut, c_pay_once_name FROM pay_strike_cut_once " & _
            '                                 "UNION " & _
            '                                 "SELECT c_user_id,d_years,0 AS pay_cut_strike, s_pay_cut AS pay_time_cut, c_pay_once_name FROM pay_time_cut_once " & _
            '                               ") AS CUT_O " & _
            '                              ")  AS CUT " & _
            '                         "WHERE FORMAT(d_years, 'yyyyMM')=:TargetYM" & _
            '                         "GROUP BY c_user_id, cut_div, d_years, c_pay_once_name " & _
            '                         "ORDER BY c_user_id, cut_div, d_years;"
            ' UPD 2016/07/28 カラム追加（課税フラグ） End

            command.Parameters.Clear()
            command.SetSql(IIf(Me._strCut = UnionConst.DAILY_PAY_KIND_PAYCUT, strSql05, strSql06))
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
            command.Parameters.Item("TargetYM").Value = TargetYM
            Dim result As Integer = command.ExecuteNonQuery()
        End Sub

        ''' <summary>
        ''' 役員手当金額計算処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="TruncPlace">切捨て桁数</param>
        ''' <param name="UserId">作成者個人ＩＤ</param>
        ''' <remarks></remarks>
        Private Sub GetOfficerPay( _
            ByVal command As NpgsqlCommand, _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal TruncPlace As Integer, _
            ByVal UserId As String _
        )

            Dim dPreReader As NpgsqlDataReader = Nothing
            Dim dReader As NpgsqlDataReader = Nothing
            Dim intTarget As Integer
            Dim strSql As String = ""                       ' 期取得SQL文
            Dim strPreSql As String = ""                    ' メインSQL文
            Dim strSqlUpd As String = ""                    ' 更新SQL文
            Dim strSqlIns As String = ""                    ' 登録SQL文

            '-----------------------------------------------------------------------------------
            '   期マスタ参照
            '-----------------------------------------------------------------------------------
            ' SQL作成
            strPreSql = ""
            strPreSql += "SELECT LEFT(d_from, 4)" & vbCrLf
            strPreSql += "  FROM period" & vbCrLf
            strPreSql += " WHERE LEFT(d_from, 6) <= :TargetYM" & vbCrLf
            strPreSql += "   AND LEFT(d_to, 6) >= :TargetYM" & vbCrLf

            command.Parameters.Clear()                                              ' パラメータクリア
            command.SetSql(strPreSql)                                               ' SQL文設定
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))  ' バインド変数追加
            command.Parameters.Item("TargetYM").Value = TargetYM                    ' バインド変数値設定
            dPreReader = command.ExecuteReader                                      ' SQL実行

            ' 件数取得
            intTarget = CInt(dPreReader.getTable().Rows().Item(0).ItemArray(0).ToString)

            ' 2016/08/23(火) Update Start 委員会ＩＤだけではなく、委員会ＩＤと委員会ＩＤ枝番毎の最大適用開始年月日のものを取得するように修正
            ' ユーザ別月別役員手当合計集計
            strSql = ""
            strSql += "SELECT LIST.c_user_id" & vbCrLf
            strSql += "      ,MAX(MASTER.s_officer_pay) AS sum_officer_pay" & vbCrLf
            strSql += "  FROM ((" & vbCrLf
            '                   委員会一覧明細から対象年月以下で、委員会ＩＤ・委員会ＩＤ枝番毎に最大適用開始年月日のものを取得
            '                   ⇒使用しない枝番発生問題により、最大値適用開始年月日は委員会単位に修正 2017/06/06
            '                   ⇒この事象が2018年修正により再発の為対応、AS LIST内のAS MTのDroup byを修正 2019/05/31
            strSql += "         SELECT MST.c_user_id" & vbCrLf
            strSql += "               ,MST.c_committee_id" & vbCrLf
            strSql += "               ,MST.s_committee_seq" & vbCrLf
            strSql += "               ,MT.max_d_from" & vbCrLf
            strSql += "           FROM committee_list_dtl AS MST" & vbCrLf
            strSql += "               ,(" & vbCrLf
            strSql += "                 SELECT M.c_committee_id AS max_id" & vbCrLf
            strSql += "                       ,MAX(M.d_from) AS max_d_from" & vbCrLf
            strSql += "                   FROM committee_list_dtl AS M" & vbCrLf
            strSql += "                  WHERE M.d_from < :CriterionDate" & vbCrLf
            strSql += "                  GROUP BY M.c_committee_id" & vbCrLf
            strSql += "                ) AS MT" & vbCrLf
            strSql += "          WHERE MT.max_id = MST.c_committee_id" & vbCrLf
            strSql += "            AND MT.max_d_from = MST.d_from" & vbCrLf
            strSql += "          GROUP BY MST.c_user_id" & vbCrLf
            strSql += "                  ,MST.c_committee_id" & vbCrLf
            strSql += "                  ,MST.s_committee_seq" & vbCrLf
            strSql += "                  ,MT.max_d_from" & vbCrLf
            strSql += "       ) AS LIST" & vbCrLf
            strSql += "       INNER JOIN (" & vbCrLf
            '                     委員会マスタ詳細(役職マスタ)から対象年月以下で、委員会ＩＤ・委員会ＩＤ枝番毎の最大適用開始年月日のものを取得
            strSql += "           SELECT COM_DTL.c_committee_id" & vbCrLf
            strSql += "                 ,COM_DTL.s_committee_seq" & vbCrLf
            strSql += "                 ,COM_DTL.c_officer_pay_id" & vbCrLf
            strSql += "                 ,COM_DTL.ds_from" & vbCrLf
            strSql += "                 ,COM_DTL.ds_to" & vbCrLf
            strSql += "             FROM (" & vbCrLf
            strSql += "                   SELECT MST.c_committee_id" & vbCrLf
            strSql += "                         ,MST.s_committee_seq" & vbCrLf
            strSql += "                         ,MST.c_officer_pay_id" & vbCrLf
            strSql += "                         ,:d_service_from & MST.d_service_from AS ds_from" & vbCrLf
            strSql += "                         ,:d_service_to & MST.d_service_to AS ds_to" & vbCrLf
            strSql += "                     FROM committee_dtl AS MST" & vbCrLf
            strSql += "                         ,(" & vbCrLf
            strSql += "                           SELECT M.c_committee_id AS max_id" & vbCrLf
            strSql += "                                 ,MAX(M.d_from) AS max_d_from" & vbCrLf
            strSql += "                             FROM committee_dtl AS M" & vbCrLf
            strSql += "                            WHERE M.d_from < :CriterionDate" & vbCrLf
            strSql += "                              AND :CriterionDate <= M.d_to" & vbCrLf
            strSql += "                            GROUP BY M.c_committee_id" & vbCrLf
            strSql += "                                    ,M.s_committee_seq" & vbCrLf
            strSql += "                          ) AS MT" & vbCrLf
            strSql += "                    WHERE MT.max_id = MST.c_committee_id" & vbCrLf
            strSql += "                      AND MT.max_d_from = MST.d_from" & vbCrLf
            strSql += "                    UNION" & vbCrLf
            strSql += "                    SELECT MST.c_committee_id" & vbCrLf
            strSql += "                          ,MST.s_committee_seq" & vbCrLf
            strSql += "                          ,MST.c_officer_pay_id" & vbCrLf
            strSql += "                          ,:d_service_before & MST.d_service_from AS ds_from" & vbCrLf
            strSql += "                          ,:d_service_from & MST.d_service_to AS ds_to" & vbCrLf
            strSql += "                      FROM committee_dtl AS MST" & vbCrLf
            strSql += "                          ,(" & vbCrLf
            strSql += "                            SELECT M.c_committee_id AS max_id" & vbCrLf
            strSql += "                                  ,MAX(M.d_from) AS max_d_from" & vbCrLf
            strSql += "                              FROM committee_dtl AS M" & vbCrLf
            strSql += "                             WHERE M.d_from < :CriterionDate" & vbCrLf
            strSql += "                               AND :CriterionDate <= M.d_to" & vbCrLf
            strSql += "                             GROUP BY M.c_committee_id" & vbCrLf
            strSql += "                                     ,M.s_committee_seq" & vbCrLf
            strSql += "                           ) AS MT" & vbCrLf
            strSql += "                     WHERE MT.max_id = MST.c_committee_id" & vbCrLf
            strSql += "                       AND MT.max_d_from=MST.d_from" & vbCrLf
            strSql += "                  ) AS COM_DTL" & vbCrLf
            strSql += "            WHERE COM_DTL.ds_from <= :TargetYM" & vbCrLf
            strSql += "              AND :TargetYM <= COM_DTL.ds_to" & vbCrLf
            strSql += "       ) AS DTL" & vbCrLf
            strSql += "       ON  (LIST.s_committee_seq = DTL.s_committee_seq)" & vbCrLf
            strSql += "       AND (LIST.c_committee_id = DTL.c_committee_id))" & vbCrLf
            strSql += "       INNER JOIN (" & vbCrLf
            '                      役員手当マスタから対象年月以下で、役員手当ＩＤ毎に最大適用開始年月日のものを取得
            strSql += "            SELECT MST.c_officer_pay_id" & vbCrLf
            strSql += "                  ,MST.s_officer_pay" & vbCrLf
            strSql += "              FROM officer_pay_master AS MST" & vbCrLf
            strSql += "                  ,(" & vbCrLf
            strSql += "                    SELECT M.c_officer_pay_id AS max_id" & vbCrLf
            strSql += "                          ,MAX(M.d_from) AS max_d_from" & vbCrLf
            strSql += "                      FROM officer_pay_master AS M" & vbCrLf
            strSql += "                     WHERE M.d_from < :CriterionDate" & vbCrLf
            strSql += "                       AND :CriterionDate <= M.d_to" & vbCrLf
            strSql += "                     GROUP BY M.c_officer_pay_id" & vbCrLf
            strSql += "                   ) AS MT" & vbCrLf
            strSql += "             WHERE MT.max_id = MST.c_officer_pay_id" & vbCrLf
            strSql += "               AND MT.max_d_from = MST.d_from" & vbCrLf
            strSql += "       ) AS MASTER" & vbCrLf
            strSql += "       ON DTL.c_officer_pay_id = MASTER.c_officer_pay_id" & vbCrLf
            strSql += " WHERE DTL.ds_from <= LEFT(LIST.max_d_from,6)" & vbCrLf
            strSql += "   AND LEFT(LIST.max_d_from,6) <= DTL.ds_to" & vbCrLf
            strSql += " GROUP BY LIST.c_user_id" & vbCrLf
            'strSql = "SELECT LIST.c_user_id, MAX(MASTER.s_officer_pay) AS sum_officer_pay " & _
            '         "FROM ((SELECT MST.c_user_id, MST.c_committee_id, MST.s_committee_seq , MT.max_d_from FROM committee_list_dtl AS MST, (SELECT M.c_committee_id AS max_id, MAX(M.d_from) AS max_d_from FROM committee_list_dtl AS M " & _
            '         "WHERE M.d_from<:CriterionDate GROUP BY M.c_committee_id) AS MT WHERE MT.max_id=MST.c_committee_id AND MT.max_d_from=MST.d_from GROUP BY MST.c_user_id, MST.c_committee_id, MST.s_committee_seq, MT.max_d_from) AS LIST " & _
            '         "INNER JOIN (SELECT COM_DTL.c_committee_id, COM_DTL.s_committee_seq, COM_DTL.c_officer_pay_id, COM_DTL.ds_from, COM_DTL.ds_to FROM (" & _
            '         "SELECT MST.c_committee_id, MST.s_committee_seq, MST.c_officer_pay_id, :d_service_from & MST.d_service_from AS ds_from, :d_service_to & MST.d_service_to AS ds_to " & _
            '         "FROM committee_dtl AS MST, (SELECT M.c_committee_id AS max_id, MAX(M.d_from) AS max_d_from FROM committee_dtl AS M WHERE M.d_from<:CriterionDate And :CriterionDate<=M.d_to GROUP BY M.c_committee_id) AS MT " & _
            '         "WHERE MT.max_id=MST.c_committee_id AND MT.max_d_from=MST.d_from " & _
            '         "UNION " & _
            '         "SELECT MST.c_committee_id, MST.s_committee_seq, MST.c_officer_pay_id, :d_service_before & MST.d_service_from AS ds_from, :d_service_from & MST.d_service_to AS ds_to " & _
            '         "FROM committee_dtl AS MST, (SELECT M.c_committee_id AS max_id, MAX(M.d_from) AS max_d_from FROM committee_dtl AS M WHERE M.d_from<:CriterionDate And :CriterionDate<=M.d_to GROUP BY M.c_committee_id) AS MT " & _
            '         "WHERE MT.max_id=MST.c_committee_id AND MT.max_d_from=MST.d_from " & _
            '         ") AS COM_DTL WHERE COM_DTL.ds_from<=:TargetYM And :TargetYM<=COM_DTL.ds_to) AS DTL " & _
            '         "ON (LIST.s_committee_seq = DTL.s_committee_seq) AND (LIST.c_committee_id = DTL.c_committee_id))  " & _
            '         "INNER JOIN (SELECT MST.c_officer_pay_id, MST.s_officer_pay FROM officer_pay_master AS MST, (SELECT M.c_officer_pay_id AS max_id, MAX(M.d_from) AS max_d_from FROM officer_pay_master AS M " & _
            '         "WHERE M.d_from<:CriterionDate And :CriterionDate<=M.d_to GROUP BY M.c_officer_pay_id)  AS MT WHERE MT.max_id=MST.c_officer_pay_id AND MT.max_d_from=MST.d_from) AS MASTER ON DTL.c_officer_pay_id = MASTER.c_officer_pay_id  " & _
            '         "WHERE DTL.ds_from<=LEFT(LIST.max_d_from,6) AND LEFT(LIST.max_d_from,6)<= DTL.ds_to  " & _
            '         "GROUP BY LIST.c_user_id"
            ' 2016/08/23(火) Update End 委員会ＩＤだけではなく、委員会ＩＤと委員会ＩＤ枝番毎の最大適用開始年月日のものを取得するように修正

            command.Parameters.Clear()
            command.SetSql(strSql)
            command.Parameters.Add(New NpgsqlParameter("CriterionDate", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("d_service_from", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("d_service_to", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("d_service_before", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
            command.Parameters.Item("CriterionDate").Value = CriterionDate
            command.Parameters.Item("d_service_from").Value = CStr(intTarget)
            command.Parameters.Item("d_service_to").Value = CStr(intTarget + 1)
            command.Parameters.Item("d_service_before").Value = CStr(intTarget - 1)
            command.Parameters.Item("TargetYM").Value = TargetYM
            dReader = command.ExecuteReader

            '-----------------------------------------------------------------------------------
            '   役員手当額更新・登録
            '-----------------------------------------------------------------------------------
            ' 更新SQL文作成
            strSqlUpd = ""
            strSqlUpd += "UPDATE taxation_total" & vbCrLf
            strSqlUpd += "   SET s_officer_pay = :s_officer_pay" & vbCrLf
            strSqlUpd += " WHERE c_user_id = :c_user_id" & vbCrLf
            strSqlUpd += "   AND FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
            strSqlUpd += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            'strSqlUpd = "UPDATE taxation_total SET s_officer_pay=:s_officer_pay WHERE c_user_id=:c_user_id AND FORMAT(d_years, 'yyyyMM')=:TargetYM AND k_daily_pay_kind=:k_daily_pay_kind"

            ' UPD 2016/07/28 カラム追加（課税フラグ） Start
            ' 登録SQL文作成
            strSqlIns = ""
            strSqlIns += "INSERT INTO taxation_total(" & vbCrLf
            strSqlIns += "    d_years" & vbCrLf
            strSqlIns += "   ,k_daily_pay_kind" & vbCrLf
            strSqlIns += "   ,c_user_id" & vbCrLf
            strSqlIns += "   ,s_officer_pay" & vbCrLf
            strSqlIns += "   ,c_taxation_flag" & vbCrLf
            strSqlIns += ") VALUES (" & vbCrLf
            strSqlIns += "    CONVERT(DATE,:TargetYM,112)" & vbCrLf
            strSqlIns += "   ,:k_daily_pay_kind" & vbCrLf
            strSqlIns += "   ,:c_user_id" & vbCrLf
            strSqlIns += "   ,:s_officer_pay" & vbCrLf
            strSqlIns += "   ,'1'" & vbCrLf
            strSqlIns += ")" & vbCrLf
            strSqlIns += ";" & vbCrLf
            'strSqlIns = "INSERT INTO taxation_total(d_years,k_daily_pay_kind,c_user_id,s_officer_pay) VALUES(CONVERT(DATE,:TargetYM,112),:k_daily_pay_kind,:c_user_id,:s_officer_pay)"
            ' UPD 2016/07/28 カラム追加（課税フラグ） End

            For Each Row As DataRow In dReader.getTable().Rows()
                ' 更新処理
                command.Parameters.Clear()
                command.SetSql(strSqlUpd)
                command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("s_officer_pay").Value = Row.Item(1)
                command.Parameters.Item("c_user_id").Value = Row.Item(0)
                command.Parameters.Item("TargetYM").Value = TargetYM
                command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
                Dim result = command.ExecuteNonQuery()

                If Me._strCut = UnionConst.DAILY_PAY_KIND_PAYCUT And result = 0 Then
                    ' 登録処理
                    command.Parameters.Clear()
                    command.SetSql(strSqlIns)
                    command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    command.Parameters.Item("s_officer_pay").Value = Row.Item(1)
                    command.Parameters.Item("c_user_id").Value = Row.Item(0)
                    command.Parameters.Item("TargetYM").Value = TargetYM & "01"
                    command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
                    Dim result2 As Integer = command.ExecuteNonQuery()
                End If
            Next

            ' 特例パターン計算（中央委員・産別の長兼任者）
            If TargetYM.Substring(4, 2) = "09" Then
                sepPluralistSpecial(command, intTarget)
            End If
        End Sub

        ''' <summary>
        ''' 役員手当金額計算のサブクラス、兼任者の9月特例計算
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="intTargetY">集計年月</param>
        ''' <remarks></remarks>
        Private Sub sepPluralistSpecial( _
            ByVal command As NpgsqlCommand, _
            ByVal intTargetY As Integer _
        )

            Dim dPreReader As NpgsqlDataReader = Nothing
            Dim dReader As NpgsqlDataReader = Nothing
            Dim strSql As String = ""
            Dim strPreSql As String = ""
            Dim strSqlUpd As String = ""

            ' 今期の兼任確定者は9月の中央委員日当が支払われない
            ' 今期の兼務者検索
            strPreSql = ""
            strPreSql += "SELECT snbt.c_user_id" & vbCrLf
            strPreSql += "  FROM (" & vbCrLf
            strPreSql += "         SELECT c_user_id" & vbCrLf
            strPreSql += "           FROM committee_list_dtl" & vbCrLf
            strPreSql += "          WHERE c_committee_id IN ('042','043','044','045')" & vbCrLf
            strPreSql += "            AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "            AND LEFT(d_from, 6) = :targetY & '10'" & vbCrLf
            strPreSql += "       ) AS snbt" & vbCrLf
            strPreSql += "      ,(" & vbCrLf
            strPreSql += "        SELECT c_user_id" & vbCrLf
            strPreSql += "          FROM committee_list_dtl" & vbCrLf
            strPreSql += "         WHERE c_committee_id = '002'" & vbCrLf
            strPreSql += "           AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "           AND LEFT(d_from, 6) = :targetY & '09'" & vbCrLf
            strPreSql += "       ) AS chuo" & vbCrLf
            strPreSql += " WHERE snbt.c_user_id = chuo.c_user_id" & vbCrLf
            'strPreSql = "SELECT snbt.c_user_id FROM " & _
            '            "(SELECT c_user_id FROM committee_list_dtl WHERE c_committee_id IN ('042','043','044','045') AND s_committee_seq = '1' AND LEFT(d_from, 6) = :targetY & '10') AS snbt," & _
            '            "(SELECT c_user_id FROM committee_list_dtl WHERE c_committee_id = '002' AND s_committee_seq = '1' AND LEFT(d_from, 6) = :targetY & '09') AS chuo " & _
            '            "WHERE snbt.c_user_id = chuo.c_user_id"

            command.Parameters.Clear()
            command.SetSql(strPreSql)
            command.Parameters.Add(New NpgsqlParameter("targetY", DbType.String))
            command.Parameters.Item("targetY").Value = CStr(intTargetY)
            dPreReader = command.ExecuteReader

            ' 中央委員の役員手当を除外して役員手当集計
            strSql = ""
            strSql += "SELECT LIST.c_user_id" & vbCrLf
            strSql += "      ,IIF(ISNULL(MAX(MASTER.s_officer_pay)), 0, MAX(MASTER.s_officer_pay)) AS sum_officer_pay" & vbCrLf
            strSql += "  FROM ((" & vbCrLf
            strSql += "         SELECT byuser.c_user_id" & vbCrLf
            strSql += "               ,prdif.c_officer_pay_id" & vbCrLf
            strSql += "           FROM (" & vbCrLf
            strSql += "                 SELECT c_committee_list" & vbCrLf
            strSql += "                       ,c_user_id" & vbCrLf
            strSql += "                       ,c_committee_id" & vbCrLf
            strSql += "                       ,s_committee_seq" & vbCrLf
            strSql += "                   FROM committee_list_dtl"
            strSql += "                  WHERE c_user_id = :c_user_id"
            strSql += "                    AND d_from < :targetY & '0930'" & vbCrLf
            strSql += "                ) AS byuser" & vbCrLf
            strSql += "               ,(" & vbCrLf
            strSql += "                 SELECT t5.c_committee_id" & vbCrLf
            strSql += "                       ,t5.s_committee_seq" & vbCrLf
            strSql += "                       ,t6.c_period_id" & vbCrLf
            strSql += "                       ,t5.c_officer_pay_id" & vbCrLf
            strSql += "                   FROM committee_dtl AS t5" & vbCrLf
            strSql += "                       ,period_service_diff AS t6" & vbCrLf
            strSql += "                  WHERE t5.s_from_diff = t6.service_diff" & vbCrLf
            strSql += "                    AND t6.service_from <= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t6.service_to >= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t5.d_from <= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t5.d_to >= :targetY & '0901'" & vbCrLf
            strSql += "                ) AS prdif" & vbCrLf
            strSql += "          WHERE byuser.c_committee_id = prdif.c_committee_id" & vbCrLf
            strSql += "            AND byuser.s_committee_seq = prdif.s_committee_seq" & vbCrLf
            strSql += "            AND LEFT(byuser.c_committee_list, 3) = prdif.c_period_id" & vbCrLf
            strSql += "            AND byuser.c_committee_id <> '002'" & vbCrLf
            strSql += "        ) AS LIST" & vbCrLf
            strSql += "        LEFT JOIN (" & vbCrLf
            strSql += "            SELECT paymst.c_officer_pay_id" & vbCrLf
            strSql += "                  ,paymst.s_officer_pay" & vbCrLf
            strSql += "              FROM officer_pay_master AS paymst" & vbCrLf
            strSql += "                  ,(" & vbCrLf
            strSql += "                    SELECT c_officer_pay_id AS max_id" & vbCrLf
            strSql += "                          ,MAX(d_from) AS max_d_from" & vbCrLf
            strSql += "                      FROM officer_pay_master" & vbCrLf
            strSql += "                     WHERE d_from < :targetY & '0930'" & vbCrLf
            strSql += "                       AND :targetY & '0930' <= d_to" & vbCrLf
            strSql += "                     GROUP BY c_officer_pay_id" & vbCrLf
            strSql += "                   ) AS maxmst" & vbCrLf
            strSql += "             WHERE maxmst.max_id = paymst.c_officer_pay_id" & vbCrLf
            strSql += "               AND maxmst.max_d_from = paymst.d_from" & vbCrLf
            strSql += "        ) AS MASTER " & vbCrLf
            strSql += "        ON LIST.c_officer_pay_id = MASTER.c_officer_pay_id" & vbCrLf
            strSql += ") GROUP BY LIST.c_user_id" & vbCrLf
            'strSql = "SELECT LIST.c_user_id, IIF(ISNULL(MAX(MASTER.s_officer_pay)), 0, MAX(MASTER.s_officer_pay)) AS sum_officer_pay FROM (" & _
            '         " (SELECT byuser.c_user_id, prdif.c_officer_pay_id FROM" & _
            '         "  (SELECT c_committee_list, c_user_id, c_committee_id, s_committee_seq FROM committee_list_dtl WHERE c_user_id = :c_user_id AND d_from < :targetY & '0930') AS byuser," & _
            '         "  (SELECT t5.c_committee_id, t5.s_committee_seq, t6.c_period_id, t5.c_officer_pay_id FROM committee_dtl AS t5, period_service_diff AS t6" & _
            '         "  WHERE t5.s_from_diff = t6.service_diff AND t6.service_from <= :targetY & '0901' AND t6.service_to >= :targetY & '0901' AND t5.d_from  <= :targetY & '0901' AND t5.d_to >= :targetY & '0901') AS prdif" & _
            '         " WHERE byuser.c_committee_id = prdif.c_committee_id AND byuser.s_committee_seq = prdif.s_committee_seq AND LEFT(byuser.c_committee_list, 3) = prdif.c_period_id AND byuser.c_committee_id <> '002') AS LIST" & _
            '         " LEFT JOIN" & _
            '         " (SELECT paymst.c_officer_pay_id, paymst.s_officer_pay FROM officer_pay_master AS paymst, " & _
            '         "  (SELECT c_officer_pay_id AS max_id, MAX(d_from) AS max_d_from FROM officer_pay_master WHERE d_from < :targetY & '0930' And :targetY & '0930' <= d_to GROUP BY c_officer_pay_id)  AS maxmst" & _
            '         "  WHERE maxmst.max_id=paymst.c_officer_pay_id AND maxmst.max_d_from=paymst.d_from) AS MASTER " & _
            '         " ON LIST.c_officer_pay_id = MASTER.c_officer_pay_id " & _
            '         ") GROUP by LIST.c_user_id"

            For Each Row1 As DataRow In dPreReader.getTable().Rows()
                command.Parameters.Clear()
                command.SetSql(strSql)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("targetY", DbType.String))
                command.Parameters.Item("c_user_id").Value = Row1.Item(0)
                command.Parameters.Item("targetY").Value = CStr(intTargetY)
                dReader = command.ExecuteReader

                ' 役員手当額更新
                strSqlUpd = ""
                strSqlUpd += "UPDATE taxation_total" & vbCrLf
                strSqlUpd += "   SET s_officer_pay = :s_officer_pay" & vbCrLf
                strSqlUpd += " WHERE c_user_id=:c_user_id" & vbCrLf
                strSqlUpd += "   AND FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
                strSqlUpd += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                'strSqlUpd = "UPDATE taxation_total SET s_officer_pay=:s_officer_pay WHERE c_user_id=:c_user_id AND FORMAT(d_years, 'yyyyMM')=:TargetYM AND k_daily_pay_kind=:k_daily_pay_kind"
                For Each Row2 As DataRow In dReader.getTable().Rows()
                    command.Parameters.Clear()
                    command.SetSql(strSqlUpd)
                    command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    command.Parameters.Item("s_officer_pay").Value = Row2.Item(1)
                    command.Parameters.Item("c_user_id").Value = Row2.Item(0)
                    command.Parameters.Item("TargetYM").Value = CStr(intTargetY) & "09"
                    command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
                    command.ExecuteNonQuery()
                Next
            Next

            ' 前期の兼任者は9月まで中央委員日当が支払われる
            ' 前期の兼務者検索
            strPreSql = ""
            strPreSql += "SELECT snbt.c_user_id" & vbCrLf
            strPreSql += "  FROM (" & vbCrLf
            strPreSql += "        SELECT c_user_id " & vbCrLf
            strPreSql += "          FROM committee_list_dtl AS t42" & vbCrLf
            strPreSql += "              ,(" & vbCrLf
            strPreSql += "                SELECT Max(d_from) AS max_d_from" & vbCrLf
            strPreSql += "                  FROM committee_list_dtl" & vbCrLf
            strPreSql += "                 WHERE c_committee_id = '042'" & vbCrLf
            strPreSql += "                   AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND LEFT(d_from, 6) <= :targetY & '09'" & vbCrLf
            strPreSql += "               ) AS ansiMax" & vbCrLf
            strPreSql += "         WHERE t42.c_committee_id = '042'" & vbCrLf
            strPreSql += "           AND t42.s_committee_seq = '1'" & vbCrLf
            strPreSql += "           AND t42.d_from = ansiMax.max_d_from" & vbCrLf
            strPreSql += "        UNION ALL" & vbCrLf
            strPreSql += "        SELECT c_user_id" & vbCrLf
            strPreSql += "          FROM committee_list_dtl AS t43" & vbCrLf
            strPreSql += "              ,(" & vbCrLf
            strPreSql += "                SELECT Max(d_from) AS max_d_from" & vbCrLf
            strPreSql += "                  FROM committee_list_dtl" & vbCrLf
            strPreSql += "                 WHERE c_committee_id = '043'" & vbCrLf
            strPreSql += "                   AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND LEFT(d_from, 6) <= :targetY & '09'" & vbCrLf
            strPreSql += "               ) AS jikmMax" & vbCrLf
            strPreSql += "         WHERE t43.c_committee_id = '043'" & vbCrLf
            strPreSql += "           AND t43.s_committee_seq = '1'" & vbCrLf
            strPreSql += "           AND t43.d_from = jikmMax.max_d_from" & vbCrLf
            strPreSql += "        UNION ALL" & vbCrLf
            strPreSql += "        SELECT c_user_id" & vbCrLf
            strPreSql += "          FROM committee_list_dtl AS t44" & vbCrLf
            strPreSql += "              ,(" & vbCrLf
            strPreSql += "                SELECT Max(d_from) AS max_d_from" & vbCrLf
            strPreSql += "                  FROM committee_list_dtl" & vbCrLf
            strPreSql += "                 WHERE c_committee_id = '044'" & vbCrLf
            strPreSql += "                   AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND LEFT(d_from, 6) <= :targetY & '09'" & vbCrLf
            strPreSql += "               ) AS kkrrMax" & vbCrLf
            strPreSql += "         WHERE t44.c_committee_id = '044'" & vbCrLf
            strPreSql += "           AND t44.s_committee_seq = '1'" & vbCrLf
            strPreSql += "           AND t44.d_from = kkrrMax.max_d_from" & vbCrLf
            strPreSql += "        UNION ALL" & vbCrLf
            strPreSql += "        SELECT c_user_id" & vbCrLf
            strPreSql += "          FROM committee_list_dtl AS t45" & vbCrLf
            strPreSql += "              ,(" & vbCrLf
            strPreSql += "                SELECT Max(d_from) AS max_d_from" & vbCrLf
            strPreSql += "                  FROM committee_list_dtl" & vbCrLf
            strPreSql += "                 WHERE c_committee_id = '045'" & vbCrLf
            strPreSql += "                   AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND LEFT(d_from, 6) <= :targetY & '09'" & vbCrLf
            strPreSql += "               ) AS asapMax" & vbCrLf
            strPreSql += "         WHERE t45.c_committee_id = '045'" & vbCrLf
            strPreSql += "           AND t45.s_committee_seq = '1'" & vbCrLf
            strPreSql += "           AND t45.d_from = asapMax.max_d_from) AS snbt" & vbCrLf
            strPreSql += "      ,(" & vbCrLf
            strPreSql += "        SELECT c_user_id" & vbCrLf
            strPreSql += "          FROM committee_list_dtl AS t02" & vbCrLf
            strPreSql += "              ,(" & vbCrLf
            strPreSql += "                SELECT Max(d_from) AS max_d_from" & vbCrLf
            strPreSql += "                  FROM committee_list_dtl" & vbCrLf
            strPreSql += "                 WHERE c_committee_id = '002'" & vbCrLf
            strPreSql += "                   AND s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND LEFT(d_from, 6) <= :targetY & '08') AS chuoMax" & vbCrLf
            strPreSql += "                 WHERE t02.c_committee_id = '002'" & vbCrLf
            strPreSql += "                   AND t02.s_committee_seq = '1'" & vbCrLf
            strPreSql += "                   AND t02.d_from = chuoMax.max_d_from" & vbCrLf
            strPreSql += "               ) AS chuo" & vbCrLf
            strPreSql += "         WHERE snbt.c_user_id = chuo.c_user_id" & vbCrLf
            'strPreSql = "SELECT snbt.c_user_id FROM " & _
            '            "(SELECT c_user_id FROM committee_list_dtl AS t42, (SELECT Max(d_from) AS max_d_from FROM committee_list_dtl WHERE c_committee_id = '042' AND s_committee_seq = '1' AND LEFT(d_from, 6) <= :targetY & '09') AS ansiMax WHERE t42.c_committee_id = '042' AND t42.s_committee_seq = '1' AND t42.d_from = ansiMax.max_d_from UNION ALL" & _
            '            " SELECT c_user_id FROM committee_list_dtl AS t43, (SELECT Max(d_from) AS max_d_from FROM committee_list_dtl WHERE c_committee_id = '043' AND s_committee_seq = '1' AND LEFT(d_from, 6) <= :targetY & '09') AS jikmMax WHERE t43.c_committee_id = '043' AND t43.s_committee_seq = '1' AND t43.d_from = jikmMax.max_d_from UNION ALL" & _
            '            " SELECT c_user_id FROM committee_list_dtl AS t44, (SELECT Max(d_from) AS max_d_from FROM committee_list_dtl WHERE c_committee_id = '044' AND s_committee_seq = '1' AND LEFT(d_from, 6) <= :targetY & '09') AS kkrrMax WHERE t44.c_committee_id = '044' AND t44.s_committee_seq = '1' AND t44.d_from = kkrrMax.max_d_from UNION ALL" & _
            '            " SELECT c_user_id FROM committee_list_dtl AS t45, (SELECT Max(d_from) AS max_d_from FROM committee_list_dtl WHERE c_committee_id = '045' AND s_committee_seq = '1' AND LEFT(d_from, 6) <= :targetY & '09') AS asapMax WHERE t45.c_committee_id = '045' AND t45.s_committee_seq = '1' AND t45.d_from = asapMax.max_d_from) AS snbt," & _
            '            "(SELECT c_user_id FROM committee_list_dtl AS t02, (SELECT Max(d_from) AS max_d_from FROM committee_list_dtl WHERE c_committee_id = '002' AND s_committee_seq = '1' AND LEFT(d_from, 6) <= :targetY & '08') AS chuoMax WHERE t02.c_committee_id = '002' AND t02.s_committee_seq = '1' AND t02.d_from = chuoMax.max_d_from) AS chuo " & _
            '            "WHERE snbt.c_user_id = chuo.c_user_id"

            command.Parameters.Clear()
            command.SetSql(strPreSql)
            command.Parameters.Add(New NpgsqlParameter("targetY", DbType.String))
            command.Parameters.Item("targetY").Value = CStr(intTargetY)
            dPreReader = command.ExecuteReader

            ' 中央委員の役員手当を追加して役員手当集計
            strSql = ""
            strSql += "SELECT LIST.c_user_id" & vbCrLf
            strSql += "      ,MAX(MASTER.s_officer_pay) AS sum_officer_pay" & vbCrLf
            strSql += "  FROM ((" & vbCrLf
            strSql += "         SELECT byuser.c_user_id" & vbCrLf
            strSql += "               ,prdif.c_officer_pay_id" & vbCrLf
            strSql += "           FROM (" & vbCrLf
            strSql += "                 SELECT c_committee_list" & vbCrLf
            strSql += "                       ,c_user_id" & vbCrLf
            strSql += "                       ,c_committee_id" & vbCrLf
            strSql += "                       ,s_committee_seq" & vbCrLf
            strSql += "                   FROM committee_list_dtl" & vbCrLf
            strSql += "                  WHERE c_user_id = :c_user_id" & vbCrLf
            strSql += "                    AND d_from < :targetY & '0930'" & vbCrLf
            strSql += "                ) AS byuser" & vbCrLf
            strSql += "               ,(" & vbCrLf
            strSql += "                 SELECT t5.c_committee_id" & vbCrLf
            strSql += "                       ,t5.s_committee_seq" & vbCrLf
            strSql += "                       ,t6.c_period_id" & vbCrLf
            strSql += "                       ,t5.c_officer_pay_id" & vbCrLf
            strSql += "                   FROM committee_dtl AS t5" & vbCrLf
            strSql += "                       ,period_service_diff AS t6 " & vbCrLf
            strSql += "                  WHERE t5.s_from_diff = t6.service_diff" & vbCrLf
            strSql += "                    AND t6.service_from <= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t6.service_to >= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t5.d_from  <= :targetY & '0901'" & vbCrLf
            strSql += "                    AND t5.d_to >= :targetY & '0901'" & vbCrLf
            strSql += "                ) AS prdif" & vbCrLf
            strSql += "          WHERE byuser.c_committee_id = prdif.c_committee_id" & vbCrLf
            strSql += "            AND byuser.s_committee_seq = prdif.s_committee_seq" & vbCrLf
            strSql += "            AND LEFT(byuser.c_committee_list, 3) = prdif.c_period_id" & vbCrLf
            strSql += "         UNION ALL" & vbCrLf
            strSql += "         SELECT :c_user_id" & vbCrLf
            strSql += "               ,c_officer_pay_id" & vbCrLf
            strSql += "           FROM committee_dtl" & vbCrLf
            strSql += "          WHERE c_committee_id = '002'" & vbCrLf
            strSql += "            AND s_committee_seq = '1'" & vbCrLf
            strSql += "            AND d_from <= :targetY & '0801'" & vbCrLf
            strSql += "            AND d_to >= :targetY & '0801'" & vbCrLf
            strSql += "        ) AS LIST" & vbCrLf
            strSql += "        LEFT JOIN (" & vbCrLf
            strSql += "            SELECT paymst.c_officer_pay_id" & vbCrLf
            strSql += "                  ,paymst.s_officer_pay" & vbCrLf
            strSql += "              FROM officer_pay_master AS paymst" & vbCrLf
            strSql += "                  ,(SELECT c_officer_pay_id AS max_id" & vbCrLf
            strSql += "                          ,MAX(d_from) AS max_d_from" & vbCrLf
            strSql += "                      FROM officer_pay_master" & vbCrLf
            strSql += "                     WHERE d_from < :targetY & '0930'" & vbCrLf
            strSql += "                       AND :targetY & '0930' <= d_to" & vbCrLf
            strSql += "                     GROUP BY c_officer_pay_id" & vbCrLf
            strSql += "                   ) AS maxmst" & vbCrLf
            strSql += "             WHERE maxmst.max_id = paymst.c_officer_pay_id" & vbCrLf
            strSql += "               AND maxmst.max_d_from = paymst.d_from" & vbCrLf
            strSql += "        ) AS MASTER" & vbCrLf
            strSql += "        ON LIST.c_officer_pay_id = MASTER.c_officer_pay_id" & vbCrLf
            strSql += "       )" & vbCrLf
            strSql += " GROUP BY LIST.c_user_id" & vbCrLf
            'strSql = "SELECT LIST.c_user_id, MAX(MASTER.s_officer_pay) AS sum_officer_pay FROM (" & _
            '         " (SELECT byuser.c_user_id, prdif.c_officer_pay_id FROM" & _
            '         "  (SELECT c_committee_list, c_user_id, c_committee_id, s_committee_seq FROM committee_list_dtl WHERE c_user_id = :c_user_id AND d_from < :targetY & '0930') AS byuser," & _
            '         "  (SELECT t5.c_committee_id, t5.s_committee_seq, t6.c_period_id, t5.c_officer_pay_id FROM committee_dtl AS t5, period_service_diff AS t6 " & _
            '         "  WHERE t5.s_from_diff = t6.service_diff AND t6.service_from <= :targetY & '0901' AND t6.service_to >= :targetY & '0901' AND t5.d_from  <= :targetY & '0901' AND t5.d_to >= :targetY & '0901') AS prdif" & _
            '         " WHERE byuser.c_committee_id = prdif.c_committee_id AND byuser.s_committee_seq = prdif.s_committee_seq AND LEFT(byuser.c_committee_list, 3) = prdif.c_period_id" & _
            '         " UNION ALL" & _
            '         " SELECT :c_user_id, c_officer_pay_id FROM committee_dtl WHERE c_committee_id = '002' AND  s_committee_seq = '1' AND  d_from <= :targetY & '0801' AND  d_to >= :targetY & '0801') AS LIST" & _
            '         " LEFT JOIN" & _
            '         " (SELECT paymst.c_officer_pay_id, paymst.s_officer_pay FROM officer_pay_master AS paymst," & _
            '         "  (SELECT c_officer_pay_id AS max_id, MAX(d_from) AS max_d_from FROM officer_pay_master WHERE d_from < :targetY & '0930' And :targetY & '0930' <= d_to GROUP BY c_officer_pay_id)  AS maxmst" & _
            '         "  WHERE maxmst.max_id=paymst.c_officer_pay_id AND maxmst.max_d_from=paymst.d_from) AS MASTER " & _
            '         " ON LIST.c_officer_pay_id = MASTER.c_officer_pay_id" & _
            '         ") Group by LIST.c_user_id"

            For Each Row3 As DataRow In dPreReader.getTable().Rows()
                command.Parameters.Clear()
                command.SetSql(strSql)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("targetY", DbType.String))
                command.Parameters.Item("c_user_id").Value = Row3.Item(0)
                command.Parameters.Item("targetY").Value = CStr(intTargetY)
                dReader = command.ExecuteReader

                ' 役員手当額更新
                strSqlUpd = ""
                strSqlUpd += "UPDATE taxation_total" & vbCrLf
                strSqlUpd += "   SET s_officer_pay = :s_officer_pay" & vbCrLf
                strSqlUpd += " WHERE c_user_id = :c_user_id" & vbCrLf
                strSqlUpd += "   AND FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
                strSqlUpd += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                'strSqlUpd = "UPDATE taxation_total SET s_officer_pay=:s_officer_pay WHERE c_user_id=:c_user_id AND FORMAT(d_years, 'yyyyMM')=:TargetYM AND k_daily_pay_kind=:k_daily_pay_kind"
                For Each Row4 As DataRow In dReader.getTable().Rows()
                    command.Parameters.Clear()
                    command.SetSql(strSqlUpd)
                    command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    command.Parameters.Item("s_officer_pay").Value = Row4.Item(1)
                    command.Parameters.Item("c_user_id").Value = Row4.Item(0)
                    command.Parameters.Item("TargetYM").Value = CStr(intTargetY) & "09"
                    command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
                    command.ExecuteNonQuery()
                Next
            Next
        End Sub

        ''' <summary>
        ''' 源泉徴収額計算処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="TruncPlace">切捨て桁数</param>
        ''' <param name="UserId">作成者個人ＩＤ</param>
        ''' <returns>件数</returns>
        ''' <remarks></remarks>
        Private Function CalcTaxation( _
            ByVal command As NpgsqlCommand, _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal TruncPlace As Integer, _
            ByVal UserId As String _
        ) As Integer

            Dim strSql As String = ""
            Dim strSqlUpd As String = ""
            Dim strSqlUpdAdd As String = ""
            Dim strSqlUqdTmp As String = ""
            Dim dReader As NpgsqlDataReader = Nothing
            Dim iOfficerPay As Integer
            Dim iTax1 As Integer = 0
            Dim iTax2 As Integer = 0
            'Dim iTax3 As Integer
            Dim iCount As Integer = 0
            Dim sKind As String

            ' Mod2012/11/09 組合員種別取得
            ' Mod2013/05/30 一時金名称取得
            strSql = ""
            strSql += "SELECT tt.d_years" & vbCrLf                                                          ' 01. 集計年月
            strSql += "      ,tt.k_daily_pay_kind" & vbCrLf                                                 ' 02. 日当計算区分
            strSql += "      ,tt.c_user_id" & vbCrLf                                                        ' 03. 個人認証ID
            strSql += "      ,tt.s_officer_pay" & vbCrLf                                                    ' 04. 役員手当
            strSql += "      ,(tt.s_pay_time_cut_monthly" & vbCrLf
            strSql += "      + tt.s_pay_strike_cut_monthly" & vbCrLf
            strSql += "      - tt.s_pay_time_cut_monthly_break" & vbCrLf
            strSql += "      - tt.s_pay_strike_cut_monthly_break) AS taxation_monthly" & vbCrLf             ' 05. 月例控除額
            strSql += "      ,(tt.s_pay_time_cut_once" & vbCrLf
            strSql += "      + tt.s_pay_strike_cut_once" & vbCrLf
            strSql += "      - tt.s_pay_time_cut_once_break" & vbCrLf
            strSql += "      - tt.s_pay_strike_cut_once_break) AS taxtion_once, sa.k_staf_kind" & vbCrLf    ' 06. 一時金控除額
            strSql += "      ,tt.c_pay_once_name" & vbCrLf                                                  ' 07. 一時金名称
            strSql += "  FROM taxation_total AS tt" & vbCrLf
            strSql += "      ,staf_attribute sa" & vbCrLf
            strSql += "      ,(" & vbCrLf
            strSql += "        SELECT c_user_id" & vbCrLf
            strSql += "              ,max(d_from) AS max_d_from" & vbCrLf
            strSql += "          FROM staf_attribute" & vbCrLf
            strSql += "         WHERE d_from <= :TargetYM & '01'" & vbCrLf
            strSql += "         GROUP BY c_user_id" & vbCrLf
            strSql += "       ) AS msa" & vbCrLf
            strSql += " WHERE FORMAT(d_years, 'yyyyMM') = :TargetYM" & vbCrLf
            strSql += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            strSql += "   AND tt.c_user_id = sa.c_user_id" & vbCrLf
            strSql += "   AND sa.c_user_id = msa.c_user_id" & vbCrLf
            strSql += "   AND sa.d_from = msa.max_d_from" & vbCrLf
            'strSql = "SELECT tt.d_years, tt.k_daily_pay_kind, tt.c_user_id, tt.s_officer_pay," & _
            '         "tt.s_pay_time_cut_monthly + tt.s_pay_strike_cut_monthly - tt.s_pay_time_cut_monthly_break - tt.s_pay_strike_cut_monthly_break AS taxation_monthly," & _
            '         "tt.s_pay_time_cut_once + tt.s_pay_strike_cut_once - tt.s_pay_time_cut_once_break - tt.s_pay_strike_cut_once_break AS taxtion_once, sa.k_staf_kind, tt.c_pay_once_name " & _
            '         "FROM taxation_total AS tt, staf_attribute sa, " & _
            '         " (SELECT c_user_id, max(d_from) AS max_d_from FROM staf_attribute WHERE d_from <= :TargetYM&'01' GROUP BY c_user_id) AS msa " & _
            '         "WHERE FORMAT(d_years, 'yyyyMM')=:TargetYM AND k_daily_pay_kind=:k_daily_pay_kind " & _
            '         " AND tt.c_user_id = sa.c_user_id AND sa.c_user_id = msa.c_user_id  AND sa.d_from = msa.max_d_from"
            'strSql = "SELECT d_years, k_daily_pay_kind, c_user_id, s_officer_pay," & _
            '         "s_pay_time_cut_monthly+s_pay_strike_cut_monthly-s_pay_time_cut_monthly_break-s_pay_strike_cut_monthly_break AS taxation_monthly," & _
            '         "s_pay_time_cut_once+s_pay_strike_cut_once-s_pay_time_cut_once_break-s_pay_strike_cut_once_break AS taxtion_once " & _
            '         "FROM taxation_total WHERE FORMAT(d_years, 'yyyyMM')=:TargetYM AND k_daily_pay_kind=:k_daily_pay_kind"

            command.Parameters.Clear()
            command.SetSql(strSql)
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
            command.Parameters.Item("TargetYM").Value = TargetYM
            command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut
            dReader = command.ExecuteReader

            strSqlUpd = ""
            strSqlUpd += "UPDATE taxation_total" & vbCrLf                                   ' 源泉徴収テーブル
            strSqlUpd += "   SET s_officer_pay = :s_officer_pay" & vbCrLf                   ' 役員手当
            strSqlUpd += "      ,s_cut_monthly_taxation = :s_cut_monthly_taxation" & vbCrLf ' 課税対象額（月例）
            strSqlUpd += "      ,s_cut_once_taxation = :s_cut_once_taxation" & vbCrLf       ' 課税対象額（一時金）
            strSqlUpd += "      ,d_ins = GETDATE()" & vbCrLf                                      ' 作成日
            strSqlUpd += "      ,c_user_id_ins = :c_user_ins" & vbCrLf                      ' 作成者個人ＩＤ
            strSqlUpd += " WHERE c_user_id = :c_user_id" & vbCrLf                           ' 個人認証IDと同じもの
            strSqlUpd += "   AND d_years = CONVERT(DATE,:d_years)" & vbCrLf                    ' 集計年月と同じもの
            strSqlUpd += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf             ' 日当計算区分と同じもの
            'strSqlUpd = "UPDATE taxation_total SET s_officer_pay=:s_officer_pay,s_cut_monthly_taxation=:s_cut_monthly_taxation, s_cut_once_taxation=:s_cut_once_taxation, " & _
            '            "d_ins=GETDATE(), c_user_id_ins=:c_user_ins WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years) AND k_daily_pay_kind=:k_daily_pay_kind"
            'strSqlUpd = "UPDATE taxation_total SET s_cut_monthly_taxation=:s_cut_monthly_taxation, s_cut_once_taxation=:s_cut_once_taxation, s_taxation=:s_taxation, " & _
            '            "d_ins=NOGETDATE()W, c_user_id_ins=:c_user_ins WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years) AND k_daily_pay_kind=:k_daily_pay_kind"
            strSqlUpdAdd = " AND c_pay_once_name=:c_pay_once_name" & vbCrLf                 ' 一時金名称と同じもの

            For Each Row As DataRow In dReader.getTable().Rows()
                strSqlUqdTmp = strSqlUpd
                ' 税額取得
                iOfficerPay = CInt(Row.Item(3))
                sKind = Row.Item(6)
                iTax1 = 0
                iTax2 = 0
                If iOfficerPay > 0 Then
                    If Me._strCut = UnionConst.DAILY_PAY_KIND_PAYCUT Then
                        iTax1 = GetTax(command, iOfficerPay + CInt(Row.Item(4)), CompanyCode, TargetYM)
                        iTax2 = 0
                    Else
                        '一時金の場合のみ、UPDATE対象の条件に一時金名称を追加する
                        strSqlUqdTmp = strSqlUpd + strSqlUpdAdd
                        iTax1 = 0
                        iTax2 = GetTax(command, CInt(Row.Item(5)), CompanyCode, TargetYM)
                        iOfficerPay = 0
                    End If
                ElseIf sKind = UnionConst.STAF_KIND_SENIOR Then
                    ' 役員手当0円でシニア組合員の場合、シニア特別手当と判断して課税対象とする Mod2012/11/09
                    If Me._strCut = UnionConst.DAILY_PAY_KIND_PAYCUT Then
                        iTax1 = GetTax(command, CInt(Row.Item(4)), CompanyCode, TargetYM)
                        iTax2 = 0
                    End If
                End If

                ' レコード更新
                command.Parameters.Clear()
                command.SetSql(strSqlUqdTmp)
                command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_cut_monthly_taxation", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_cut_once_taxation", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                command.Parameters.Item("s_officer_pay").Value = iOfficerPay
                command.Parameters.Item("s_cut_monthly_taxation").Value = iTax1
                command.Parameters.Item("s_cut_once_taxation").Value = iTax2
                command.Parameters.Item("c_user_ins").Value = UserId
                command.Parameters.Item("c_user_id").Value = Row.Item(2)
                command.Parameters.Item("d_years").Value = Row.Item(0)
                command.Parameters.Item("k_daily_pay_kind").Value = Row.Item(1)
                command.Parameters.Item("c_pay_once_name").Value = Row.Item(7)
                iCount += command.ExecuteNonQuery()
            Next
            Return iCount
        End Function

        ''' <summary>
        ''' 源泉徴収額計算処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="value">源泉徴収額</param>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <returns>件数</returns>
        ''' <remarks></remarks>
        Private Function GetTax( _
            ByVal command As NpgsqlCommand, _
            ByVal value As Integer, _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String _
        ) As Integer

            Dim strSql As String = ""

            ' SQL文作成
            strSql = ""
            strSql += "SELECT IIF(DTL.s_taxation = 0" & vbCrLf
            strSql += "          ,:s_taxation * DTL.s_tax_rate_dtl / 100" & vbCrLf
            strSql += "          ,DTL.s_taxation + (:s_taxation - DTL.s_lower) * DTL.s_tax_rate_dtl / 100" & vbCrLf
            strSql += "       ) AS tax_value" & vbCrLf
            strSql += "  FROM tax_rate_dtl AS DTL" & vbCrLf
            strSql += " WHERE :s_taxation >= DTL.s_lower" & vbCrLf
            strSql += "   AND :s_taxation < DTL.s_upper" & vbCrLf
            strSql += "   AND DTL.d_from <= :TargetYM" & vbCrLf
            strSql += "   AND :TargetYM <= DTL.d_to" & vbCrLf
            'strSql = "SELECT IIF(DTL.s_taxation=0,:s_taxation * DTL.s_tax_rate_dtl / 100, DTL.s_taxation + (:s_taxation - DTL.s_lower) * DTL.s_tax_rate_dtl / 100) AS tax_value " & _
            '          "FROM tax_rate_dtl AS DTL WHERE :s_taxation>=DTL.s_lower And :s_taxation<DTL.s_upper " & _
            '          " And DTL.d_from<=:TargetYM And :TargetYM<=DTL.d_to "

            ' パラメータクリア
            command.Parameters.Clear()

            ' SQL文設定
            command.SetSql(strSql)

            ' バインド変数追加
            command.Parameters.Add(New NpgsqlParameter("s_taxation", DbType.Int32))     ' 源泉徴収額
            command.Parameters.Add(New NpgsqlParameter("TargetYM", DbType.String))      ' 集計年月

            ' バインド変数値設定
            command.Parameters.Item("s_taxation").Value = value                         ' 源泉徴収額
            command.Parameters.Item("TargetYM").Value = TargetYM & "01"                 ' 集計年月初日

            ' SQL実行
            Return CInt(Fix(command.ExecuteScalar()))
        End Function

        ''' <summary>
        ''' 最小最大集計年取得処理
        ''' </summary>
        ''' <returns>最小最大集計年リスト（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetExistYears() As DataTable

            Dim table As DataTable
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT k_daily_pay_kind" & vbCrLf
                cmdText += "      ,FORMAT(MAX(d_years), 'yyyy') as [max]" & vbCrLf
                cmdText += "      ,FORMAT(MIN(d_years), 'yyyy') as [min]" & vbCrLf
                cmdText += "  FROM taxation_total" & vbCrLf
                cmdText += " GROUP BY k_daily_pay_kind" & vbCrLf
                cmdText += " ORDER BY k_daily_pay_kind" & vbCrLf
                'Dim cmdText As String = "select k_daily_pay_kind,TO_CHAR(MAX(d_years), 'yyyy') as [max],TO_CHAR(MIN(d_years), 'yyyy') as [min] " & _
                '                        "from taxation_total group by k_daily_pay_kind order by k_daily_pay_kind"

                table = MyBase.CreateSomeDataSet("taxation_total", New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection).ExecuteReader)
            Catch exception As BaseUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        ''' <summary>
        ''' 源泉徴収 - 月例賃金集計タブ　課税非対象者の一覧プレ印刷ボタン押下時
        ''' 源泉徴収 - 一時金集計タブ　課税非対象者の一覧プレ印刷ボタン押下時
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns>課税非対象者一覧（データテーブル）</returns>
        ''' <remarks>
        ''' MOD 2012/06/15
        ''' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' </remarks>
        Public Function GetMonthlyNonTaxableListReportData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal OnceName As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyNonTaxableReportListMap
            Dim cmdText1 As String = ""
            Dim cmdText2 As String = ""

            Try
                '-------------------------------------------------------------------------------
                '   SQL文作成
                '-------------------------------------------------------------------------------
                ' 一時金名称無し
                cmdText1 = ""
                cmdText1 += "SELECT nontaxable_persons.c_staf_id" & vbCrLf                              ' 01. 社員番号
                cmdText1 += "      ,nontaxable_persons.name" & vbCrLf                                   ' 02. 名前
                cmdText1 += "      ,u_branch.l_name" & vbCrLf                                           ' 03. 支部
                cmdText1 += "      ,qualification_view.l_omission_name" & vbCrLf                        ' 04. 資格
                cmdText1 += "      ,nontaxable_persons.monthly_cut" & vbCrLf                            ' 05. 月例控除額
                cmdText1 += "      ,nontaxable_persons.bonus_cut" & vbCrLf                              ' 06. 一時金控除額
                cmdText1 += "      ,nontaxable_persons.[truncate]" & vbCrLf                               ' 07. 切捨て額
                cmdText1 += "      ,nontaxable_persons.cut_sum - nontaxable_persons.[truncate]" & vbCrLf  ' 08. 差引支給額
                cmdText1 += "  FROM (" & vbCrLf
                cmdText1 += "           (" & vbCrLf
                cmdText1 += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
                cmdText1 += "                  ,member.l_name AS name" & vbCrLf
                cmdText1 += "                  ,nontaxable.monthly_cut AS monthly_cut" & vbCrLf
                cmdText1 += "                  ,nontaxable.bonus_cut AS bonus_cut" & vbCrLf
                cmdText1 += "                  ,nontaxable.monthly_cut + nontaxable.bonus_cut AS cut_sum" & vbCrLf
                cmdText1 += "                  ,nontaxable.[truncate] AS [truncate]" & vbCrLf
                cmdText1 += "                  ,member.k_belonging AS k_belonging" & vbCrLf
                cmdText1 += "                  ,member.k_qualification AS k_qualification" & vbCrLf
                cmdText1 += "              FROM (" & vbCrLf
                cmdText1 += "                    SELECT (s_pay_time_cut_monthly"
                cmdText1 += "                          + s_pay_strike_cut_monthly) AS monthly_cut" & vbCrLf
                cmdText1 += "                          ,(s_pay_time_cut_once"
                cmdText1 += "                          + s_pay_strike_cut_once) AS bonus_cut" & vbCrLf
                cmdText1 += "                          ,(s_pay_time_cut_monthly_break"
                cmdText1 += "                          + s_pay_strike_cut_monthly_break"
                cmdText1 += "                          + s_pay_time_cut_once_break"
                cmdText1 += "                          + s_pay_strike_cut_once_break) AS [truncate]" & vbCrLf
                cmdText1 += "                          ,taxation_total.c_user_id AS c_user_id" & vbCrLf
                cmdText1 += "                      FROM taxation_total" & vbCrLf
                cmdText1 += "                     WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText1 += "                       AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                cmdText1 += "                       AND c_user_id NOT IN (" & vbCrLf
                cmdText1 += "                            SELECT taxation_total.c_user_id" & vbCrLf
                cmdText1 += "                              FROM taxation_total" & vbCrLf
                cmdText1 += "                             WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText1 += "                               AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                cmdText1 += "                             GROUP BY c_user_id" & vbCrLf
                cmdText1 += "                            HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
                cmdText1 += "                                    OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
                cmdText1 += "                                    OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
                cmdText1 += "                           )" & vbCrLf
                cmdText1 += "                   ) nontaxable" & vbCrLf
                cmdText1 += "                   LEFT OUTER JOIN (" & vbCrLf
                cmdText1 += "                       SELECT A1.*" & vbCrLf
                cmdText1 += "                         FROM staf_attribute A1" & vbCrLf
                cmdText1 += "                             ,(" & vbCrLf
                cmdText1 += "                               SELECT c_user_id" & vbCrLf
                cmdText1 += "                                     ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText1 += "                                 FROM staf_attribute" & vbCrLf
                cmdText1 += "                                WHERE d_from <= :d_from" & vbCrLf
                cmdText1 += "                                  AND c_ksh <= :c_ksh" & vbCrLf
                cmdText1 += "                                GROUP BY c_user_id" & vbCrLf
                cmdText1 += "                              ) B1" & vbCrLf
                cmdText1 += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText1 += "                          AND A1.d_from = B1.d_from" & vbCrLf
                cmdText1 += "                   ) member" & vbCrLf
                cmdText1 += "                   ON nontaxable.c_user_id = member.c_user_id" & vbCrLf
                cmdText1 += "           ) nontaxable_persons" & vbCrLf
                cmdText1 += "           LEFT OUTER JOIN (" & vbCrLf
                cmdText1 += "               SELECT A3.*" & vbCrLf
                cmdText1 += "                 FROM belonging_view A3" & vbCrLf
                cmdText1 += "                     ,(" & vbCrLf
                cmdText1 += "                       SELECT c_constant_seq" & vbCrLf
                cmdText1 += "                             ,MAX(d_from) AS d_from" & vbCrLf
                cmdText1 += "                         FROM belonging_view" & vbCrLf
                cmdText1 += "                        WHERE d_from <= :d_from" & vbCrLf
                cmdText1 += "                        GROUP BY c_constant_seq" & vbCrLf
                cmdText1 += "                      ) B3" & vbCrLf
                cmdText1 += "                WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                cmdText1 += "                  AND A3.d_from = B3.d_from" & vbCrLf
                cmdText1 += "           ) u_branch" & vbCrLf
                cmdText1 += "           ON nontaxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
                cmdText1 += "       ) LEFT OUTER JOIN qualification_view" & vbCrLf
                cmdText1 += "       ON nontaxable_persons.k_qualification = qualification_view.c_constant_seq" & vbCrLf
                cmdText1 += " ORDER BY nontaxable_persons.k_belonging" & vbCrLf
                cmdText1 += "         ,RIGHT('0000000000' + nontaxable_persons.c_staf_id, 10)" & vbCrLf

                ' 一時金名称有
                cmdText2 = ""
                cmdText2 += "SELECT nontaxable_persons.c_staf_id" & vbCrLf                              ' 01. 社員番号
                cmdText2 += "      ,nontaxable_persons.name" & vbCrLf                                   ' 02. 名前
                cmdText2 += "      ,u_branch.l_name" & vbCrLf                                           ' 03. 支部
                cmdText2 += "      ,qualification_view.l_omission_name" & vbCrLf                        ' 04. 資格
                cmdText2 += "      ,nontaxable_persons.monthly_cut" & vbCrLf                            ' 05. 月例控除額
                cmdText2 += "      ,nontaxable_persons.bonus_cut" & vbCrLf                              ' 06. 一時金控除額
                cmdText2 += "      ,nontaxable_persons.[truncate]" & vbCrLf                               ' 07. 切捨て額
                cmdText2 += "      ,nontaxable_persons.cut_sum - nontaxable_persons.[truncate]" & vbCrLf  ' 08. 差引支給額
                cmdText2 += "  FROM (" & vbCrLf
                cmdText2 += "           (" & vbCrLf
                cmdText2 += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
                cmdText2 += "                  ,member.l_name AS name" & vbCrLf
                cmdText2 += "                  ,nontaxable.monthly_cut AS monthly_cut" & vbCrLf
                cmdText2 += "                  ,nontaxable.bonus_cut AS bonus_cut" & vbCrLf
                cmdText2 += "                  ,(nontaxable.monthly_cut"
                cmdText2 += "                  + nontaxable.bonus_cut) AS cut_sum" & vbCrLf
                cmdText2 += "                  ,nontaxable.[truncate] AS [truncate]" & vbCrLf
                cmdText2 += "                  ,member.k_belonging AS k_belonging" & vbCrLf
                cmdText2 += "                  ,member.k_qualification AS k_qualification" & vbCrLf
                cmdText2 += "              FROM (" & vbCrLf
                cmdText2 += "                    SELECT (s_pay_time_cut_monthly"
                cmdText2 += "                          + s_pay_strike_cut_monthly) AS monthly_cut" & vbCrLf
                cmdText2 += "                          ,(s_pay_time_cut_once"
                cmdText2 += "                          + s_pay_strike_cut_once) AS bonus_cut" & vbCrLf
                cmdText2 += "                          ,(s_pay_time_cut_monthly_break"
                cmdText2 += "                          + s_pay_strike_cut_monthly_break"
                cmdText2 += "                          + s_pay_time_cut_once_break"
                cmdText2 += "                          + s_pay_strike_cut_once_break) AS [truncate]" & vbCrLf
                cmdText2 += "                          ,taxation_total.c_user_id AS c_user_id" & vbCrLf
                cmdText2 += "                      FROM taxation_total" & vbCrLf
                cmdText2 += "                     WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText2 += "                       AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                cmdText2 += "                       AND c_pay_once_name = :c_pay_once_name" & vbCrLf
                cmdText2 += "                       AND c_user_id NOT IN (" & vbCrLf
                cmdText2 += "                           SELECT taxation_total.c_user_id" & vbCrLf
                cmdText2 += "                             FROM taxation_total" & vbCrLf
                cmdText2 += "                            WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText2 += "                              AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                cmdText2 += "                            GROUP BY c_user_id" & vbCrLf
                cmdText2 += "                           HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
                cmdText2 += "                                  OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
                cmdText2 += "                                  OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
                cmdText2 += "                           )" & vbCrLf
                cmdText2 += "                   ) nontaxable" & vbCrLf
                cmdText2 += "                   LEFT OUTER JOIN (" & vbCrLf
                cmdText2 += "                       SELECT A1.*" & vbCrLf
                cmdText2 += "                         FROM staf_attribute A1" & vbCrLf
                cmdText2 += "                             ,(" & vbCrLf
                cmdText2 += "                               SELECT c_user_id" & vbCrLf
                cmdText2 += "                                     ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText2 += "                                 FROM staf_attribute" & vbCrLf
                cmdText2 += "                                WHERE d_from <= :d_from" & vbCrLf
                cmdText2 += "                                  AND c_ksh <= :c_ksh" & vbCrLf
                cmdText2 += "                                GROUP BY c_user_id" & vbCrLf
                cmdText2 += "                              ) B1" & vbCrLf
                cmdText2 += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText2 += "                          AND A1.d_from = B1.d_from" & vbCrLf
                cmdText2 += "                   ) member" & vbCrLf
                cmdText2 += "                   ON nontaxable.c_user_id = member.c_user_id" & vbCrLf
                cmdText2 += "           ) nontaxable_persons" & vbCrLf
                cmdText2 += "           LEFT OUTER JOIN (" & vbCrLf
                cmdText2 += "               SELECT A3.*" & vbCrLf
                cmdText2 += "                 FROM belonging_view A3" & vbCrLf
                cmdText2 += "                     ,(" & vbCrLf
                cmdText2 += "                       SELECT c_constant_seq" & vbCrLf
                cmdText2 += "                             ,MAX(d_from) AS d_from" & vbCrLf
                cmdText2 += "                         FROM belonging_view" & vbCrLf
                cmdText2 += "                        WHERE d_from <= :d_from" & vbCrLf
                cmdText2 += "                        GROUP BY c_constant_seq" & vbCrLf
                cmdText2 += "                      ) B3" & vbCrLf
                cmdText2 += "                WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                cmdText2 += "                  AND A3.d_from = B3.d_from" & vbCrLf
                cmdText2 += "           ) u_branch" & vbCrLf
                cmdText2 += "           ON nontaxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
                cmdText2 += "       ) LEFT OUTER JOIN qualification_view" & vbCrLf
                cmdText2 += "       ON nontaxable_persons.k_qualification = qualification_view.c_constant_seq" & vbCrLf
                cmdText2 += " ORDER BY nontaxable_persons.k_belonging" & vbCrLf
                cmdText2 += "         ,RIGHT('0000000000' + nontaxable_persons.c_staf_id, 10)" & vbCrLf

                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode

                ' 一時金名称有無判定
                If OnceName = "" Then
                    ' 一時金名称無し
                    command.SetSql(cmdText1)
                Else
                    ' 一時金名称有り
                    command.SetSql(cmdText2)
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If
                Me.AddCutDivParameterValue(command)
                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText1)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収 - 課税対象者月例賃金の明細印刷ボタン押下
        ''' 源泉徴収 - 課税対象者一時金の明細印刷ボタン押下
        ''' 源泉徴収 - 課税非対象者月例賃金の明細印刷ボタン押下
        ''' 源泉徴収 - 課税非対象者一時金の明細印刷ボタン押下
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="SelectedMembers">対象社員番号リスト</param>
        ''' <param name="Truncate">置換文字列</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税対象者一覧・課税非対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetMonthlyReportDetailData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal SelectedMembers As ArrayList, _
            ByVal Truncate As Integer, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyReportDetailMap
            Dim format As String = ""
            Dim builder As New StringBuilder("")

            format += "SELECT member.c_staf_id AS c_staf_id" & vbCrLf                   ' 01. 社員番号
            format += "      ,member.l_name AS l_name" & vbCrLf                         ' 02. 名前
            format += "      ,model.l_name AS k_model" & vbCrLf                         ' 03. 機種
            format += "      ,license.l_omission_name AS k_qualification" & vbCrLf      ' 04. 資格
            format += "      ,{0} AS s_break" & vbCrLf                                  ' 05. 切捨て単位額
            format += "      ,FORMAT(withholding.d_years, 'MM') AS l_month" & vbCrLf    ' 06. 対象月
            format += "      ,withholding.s_pay_time_cut_monthly_break" & vbCrLf        ' 07. 月例時間内賃金控除切捨額
            format += "      ,withholding.s_pay_strike_cut_monthly_break" & vbCrLf      ' 08. 月例争議行為賃金控除切捨額
            format += "      ,withholding.s_cut_monthly_taxation" & vbCrLf              ' 09. 月例源泉徴収額
            format += "      ,c_branch.l_name AS k_local" & vbCrLf                      ' 10. 支部
            format += "      ,withholding.s_pay_time_cut_monthly" & vbCrLf              ' 11. 月例時間内賃金控除
            format += "      ,withholding.s_pay_strike_cut_monthly" & vbCrLf            ' 12. 月例争議行為賃金控除
            format += "      ,withholding.s_cut_once_taxation" & vbCrLf                 ' 13. 一時金源泉徴収額
            format += "      ,withholding.s_officer_pay" & vbCrLf                       ' 14. 役員手当
            format += "      ,withholding.s_pay_time_cut_once_break" & vbCrLf           ' 15. 一時金時間内控除切捨て額
            format += "      ,withholding.s_pay_strike_cut_once_break" & vbCrLf         ' 16. 一時金争議行為控除切捨て額
            format += "      ,withholding.s_pay_time_cut_once" & vbCrLf                 ' 17. 一時金時間内控除
            format += "      ,withholding.s_pay_strike_cut_once" & vbCrLf               ' 18. 一時金争議行為控除
            format += "      ,withholding.c_taxation_flag" & vbCrLf                     ' 19. 課税フラグ
            format += "  FROM (((((" & vbCrLf
            format += "       SELECT c_user_id" & vbCrLf
            format += "             ,d_years" & vbCrLf
            format += "             ,s_pay_time_cut_monthly" & vbCrLf
            format += "             ,s_pay_time_cut_monthly_break" & vbCrLf
            format += "             ,s_pay_strike_cut_monthly" & vbCrLf
            format += "             ,s_pay_strike_cut_monthly_break" & vbCrLf
            format += "             ,s_cut_monthly_taxation" & vbCrLf
            format += "             ,s_pay_time_cut_once" & vbCrLf
            format += "             ,s_pay_time_cut_once_break" & vbCrLf
            format += "             ,s_pay_strike_cut_once" & vbCrLf
            format += "             ,s_pay_strike_cut_once_break" & vbCrLf
            format += "             ,s_cut_once_taxation" & vbCrLf
            format += "             ,s_officer_pay" & vbCrLf
            format += "             ,c_taxation_flag" & vbCrLf
            format += "         FROM taxation_total" & vbCrLf
            format += "        WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'format += "          AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            format += "          AND c_user_id IN ( {1} ) ) withholding" & vbCrLf
            format += "       LEFT OUTER JOIN (" & vbCrLf
            format += "           SELECT A1.*" & vbCrLf
            format += "             FROM staf_attribute A1" & vbCrLf
            format += "                 ,(" & vbCrLf
            format += "                   SELECT c_user_id" & vbCrLf
            format += "                         ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            format += "                     FROM staf_attribute" & vbCrLf
            format += "                    WHERE d_from <= :d_from" & vbCrLf
            format += "                      AND c_ksh <= :c_ksh" & vbCrLf
            format += "                    GROUP BY c_user_id" & vbCrLf
            format += "                  ) B1" & vbCrLf
            format += "            WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            format += "              AND A1.d_from = B1.d_from" & vbCrLf
            format += "       ) member" & vbCrLf
            format += "       ON withholding.c_user_id = member.c_user_id" & vbCrLf
            format += "       ) LEFT OUTER JOIN (" & vbCrLf
            format += "           SELECT A2.*" & vbCrLf
            format += "             FROM area_local_view A2" & vbCrLf
            format += "                 ,(" & vbCrLf
            format += "                   SELECT c_constant_seq" & vbCrLf
            format += "                         ,MAX(area_local_view.d_from) AS d_from" & vbCrLf
            format += "                     FROM area_local_view" & vbCrLf
            format += "                    WHERE d_from <= :d_from" & vbCrLf
            format += "                    GROUP BY c_constant_seq" & vbCrLf
            format += "                  ) B2" & vbCrLf
            format += "            WHERE A2.c_constant_seq = B2.c_constant_seq" & vbCrLf
            format += "              AND A2.d_from = B2.d_from" & vbCrLf
            format += "       ) c_branch" & vbCrLf
            format += "       ON member.k_local = c_branch.c_constant_seq" & vbCrLf
            format += "       )" & vbCrLf
            format += "       LEFT OUTER JOIN (" & vbCrLf
            format += "           SELECT A4.*" & vbCrLf
            format += "             FROM qualification_view A4" & vbCrLf
            format += "                 ,(" & vbCrLf
            format += "                   SELECT c_constant_seq" & vbCrLf
            format += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            format += "                     FROM qualification_view" & vbCrLf
            format += "                    WHERE d_from <= :d_from" & vbCrLf
            format += "                    GROUP BY c_constant_seq" & vbCrLf
            format += "                  ) B4" & vbCrLf
            format += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            format += "              AND A4.d_from = B4.d_from" & vbCrLf
            format += "       ) license" & vbCrLf
            format += "       ON member.k_qualification = license.c_constant_seq" & vbCrLf
            format += "       )" & vbCrLf
            format += "       LEFT OUTER JOIN (" & vbCrLf
            format += "           SELECT A5.*" & vbCrLf
            format += "             FROM model_view A5" & vbCrLf
            format += "                 ,(" & vbCrLf
            format += "                   SELECT c_constant_seq" & vbCrLf
            format += "                         ,MAX(model_view.d_from) AS d_from" & vbCrLf
            format += "                     FROM model_view" & vbCrLf
            format += "                    WHERE d_from <= :d_from" & vbCrLf
            format += "                    GROUP BY c_constant_seq" & vbCrLf
            format += "                  ) B5" & vbCrLf
            format += "            WHERE A5.c_constant_seq = B5.c_constant_seq" & vbCrLf
            format += "              AND A5.d_from = B5.d_from" & vbCrLf
            format += "       ) model" & vbCrLf
            format += "       ON member.k_model = model.c_constant_seq" & vbCrLf
            format += "       )" & vbCrLf
            format += " ORDER BY RIGHT('0000000000' + member.c_staf_id, 10) " & vbCrLf

            Try
                Dim i As Integer
                For i = 0 To SelectedMembers.Count - 1
                    If (builder.Length > 0) Then
                        builder.Append((",'" & CStr(SelectedMembers.Item(i)) & "'"))
                    Else
                        builder.Append(("'" & CStr(SelectedMembers.Item(i)) & "'"))
                    End If
                Next i
                Dim cmdText As String = String.Format(format, Truncate.ToString, builder.ToString)
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                Me.AddCutDivParameterValue(command)
                WithHoldingDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ' MOD 2012/06/24
        ' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' <summary>
        ''' 源泉徴収　月例賃金集計タブ　課税非対象者の照会ボタン押下時処理
        ''' 源泉徴収　一時金タブ　課税非対象者の照会ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="UnionBranch"></param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns>課税非対象者一覧（データテーブル）</returns>
        ''' <remarks>
        ''' MOD 2012/06/24
        ''' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' </remarks>
        Public Function GetNonTaxableData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal UnionBranch As String, _
            ByVal CriterionDate As String, _
            Optional ByVal OnceName As String = "" _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyNonTaxableDetailListMap
            Dim cmdText1 As String = ""
            Dim cmdText2 As String = ""

            ' 一時金名称無
            cmdText1 = ""
            cmdText1 += "SELECT 0 AS [" & map.GetLogicalName(0) & "]" & vbCrLf                              ' 01. チェックボックス
            cmdText1 += "      ,taxable_persons.c_staf_id AS [" & map.GetLogicalName(1) & "]" & vbCrLf      ' 02. 社員番号
            cmdText1 += "      ,taxable_persons.name AS [" & map.GetLogicalName(2) & "]" & vbCrLf           ' 03. 氏名
            cmdText1 += "      ,license.l_omission_name AS [" & map.GetLogicalName(3) & "]" & vbCrLf        ' 04. 資格
            cmdText1 += "      ,taxable_persons.monthly_cut AS [" & map.GetLogicalName(4) & "]" & vbCrLf    ' 05. 月例控除
            cmdText1 += "      ,taxable_persons.bonus_cut AS [" & map.GetLogicalName(5) & "]" & vbCrLf      ' 06. 一時金控除
            cmdText1 += "      ,taxable_persons.[truncate] AS [" & map.GetLogicalName(6) & "]" & vbCrLf       ' 07. 切捨て額
            cmdText1 += "      ,(taxable_persons.taxable" & vbCrLf
            cmdText1 += "      - taxable_persons.[truncate]" & vbCrLf
            cmdText1 += "      - taxable_persons.withholding) AS [" & map.GetLogicalName(7) & "]" & vbCrLf  ' 08. 差引支給額
            cmdText1 += "      ,taxable_persons.c_user_id AS [" & map.GetLogicalName(8) & "]" & vbCrLf      ' 09. ユーザID
            cmdText1 += "  FROM (" & vbCrLf
            cmdText1 += "        SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText1 += "              ,member.l_name AS name" & vbCrLf
            cmdText1 += "              ,taxable.remuneration AS remuneration" & vbCrLf
            cmdText1 += "              ,taxable.monthly_cut AS monthly_cut" & vbCrLf
            cmdText1 += "              ,taxable.bonus_cut AS bonus_cut" & vbCrLf
            cmdText1 += "              ,(taxable.remuneration" & vbCrLf
            cmdText1 += "              + taxable.monthly_cut" & vbCrLf
            cmdText1 += "              + taxable.bonus_cut) AS taxable" & vbCrLf
            cmdText1 += "              ,taxable.[truncate] AS [truncate]" & vbCrLf
            cmdText1 += "              ,taxable.withholding AS withholding" & vbCrLf
            cmdText1 += "              ,taxable.c_user_id AS c_user_id" & vbCrLf
            cmdText1 += "              ,member.k_belonging AS k_belonging" & vbCrLf
            cmdText1 += "              ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText1 += "          FROM (" & vbCrLf
            cmdText1 += "                SELECT s_officer_pay AS remuneration" & vbCrLf
            cmdText1 += "                      ,(s_pay_time_cut_monthly" & vbCrLf
            cmdText1 += "                      + s_pay_strike_cut_monthly) AS monthly_cut" & vbCrLf
            cmdText1 += "                      ,(s_pay_time_cut_once" & vbCrLf
            cmdText1 += "                      + s_pay_strike_cut_once) AS bonus_cut" & vbCrLf
            cmdText1 += "                      ,(s_pay_time_cut_monthly_break" & vbCrLf
            cmdText1 += "                      + s_pay_strike_cut_monthly_break" & vbCrLf
            cmdText1 += "                      + s_pay_time_cut_once_break" & vbCrLf
            cmdText1 += "                      + s_pay_strike_cut_once_break) AS [truncate]" & vbCrLf
            cmdText1 += "                      ,(s_cut_monthly_taxation" & vbCrLf
            cmdText1 += "                      + s_cut_once_taxation) AS withholding" & vbCrLf
            cmdText1 += "                      ,taxation_total.c_user_id AS c_user_id" & vbCrLf
            cmdText1 += "                  FROM taxation_total" & vbCrLf
            cmdText1 += "                 WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                   AND c_user_id NOT IN (" & vbCrLf
            cmdText1 += "                       SELECT c_user_id" & vbCrLf
            cmdText1 += "                         FROM taxation_total" & vbCrLf
            cmdText1 += "                        WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                          AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                        GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                       HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText1 += "                              OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText1 += "                              OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText1 += "                       )" & vbCrLf
            cmdText1 += "               ) taxable" & vbCrLf
            cmdText1 += "               LEFT OUTER JOIN (" & vbCrLf
            cmdText1 += "                   SELECT A1.*" & vbCrLf
            cmdText1 += "                     FROM staf_attribute A1" & vbCrLf
            cmdText1 += "                         ,(" & vbCrLf
            cmdText1 += "                           SELECT c_user_id" & vbCrLf
            cmdText1 += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText1 += "                             FROM staf_attribute" & vbCrLf
            cmdText1 += "                            WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                              AND c_ksh <= :c_ksh" & vbCrLf
            cmdText1 += "                            GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                          ) B1" & vbCrLf
            cmdText1 += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText1 += "                      AND A1.d_from = B1.d_from" & vbCrLf
            cmdText1 += "               ) member" & vbCrLf
            cmdText1 += "               ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText1 += "         WHERE member.k_belonging = :k_belonging" & vbCrLf
            cmdText1 += "       ) taxable_persons" & vbCrLf
            cmdText1 += "       LEFT OUTER JOIN (" & vbCrLf
            cmdText1 += "           SELECT A4.*" & vbCrLf
            cmdText1 += "             FROM qualification_view A4" & vbCrLf
            cmdText1 += "                 ,(" & vbCrLf
            cmdText1 += "                   SELECT c_constant_seq" & vbCrLf
            cmdText1 += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            cmdText1 += "                     FROM qualification_view" & vbCrLf
            cmdText1 += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText1 += "                  ) B4" & vbCrLf
            cmdText1 += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            cmdText1 += "              AND A4.d_from = B4.d_from" & vbCrLf
            cmdText1 += "       ) license" & vbCrLf
            cmdText1 += "       ON taxable_persons.k_qualification = license.c_constant_seq" & vbCrLf
            cmdText1 += " ORDER BY RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf

            ' 一時金名称有
            cmdText2 = ""
            cmdText2 += "SELECT 0 AS [" & map.GetLogicalName(0) & "]" & vbCrLf                              ' 01. チェックボックス
            cmdText2 += "      ,taxable_persons.c_staf_id AS [" & map.GetLogicalName(1) & "]" & vbCrLf      ' 02. 社員番号
            cmdText2 += "      ,taxable_persons.name AS [" & map.GetLogicalName(2) & "]" & vbCrLf           ' 03. 氏名
            cmdText2 += "      ,license.l_omission_name AS [" & map.GetLogicalName(3) & "]" & vbCrLf        ' 04. 資格
            cmdText2 += "      ,taxable_persons.monthly_cut AS [" & map.GetLogicalName(4) & "]" & vbCrLf    ' 05. 月例控除
            cmdText2 += "      ,taxable_persons.bonus_cut AS [" & map.GetLogicalName(5) & "]" & vbCrLf      ' 06. 一時金控除
            cmdText2 += "      ,taxable_persons.[truncate] AS [" & map.GetLogicalName(6) & "]" & vbCrLf       ' 07. 切捨て額
            cmdText2 += "      ,(taxable_persons.taxable" & vbCrLf
            cmdText2 += "      - taxable_persons.[truncate]" & vbCrLf
            cmdText2 += "      - taxable_persons.withholding) AS [" & map.GetLogicalName(7) & "]" & vbCrLf  ' 08. 差引支給額
            cmdText2 += "      ,taxable_persons.c_user_id AS [" & map.GetLogicalName(8) & "]" & vbCrLf      ' 09. ユーザID
            cmdText2 += "  FROM (" & vbCrLf
            cmdText2 += "        SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText2 += "              ,member.l_name AS name" & vbCrLf
            cmdText2 += "              ,taxable.remuneration AS remuneration" & vbCrLf
            cmdText2 += "              ,taxable.monthly_cut AS monthly_cut" & vbCrLf
            cmdText2 += "              ,taxable.bonus_cut AS bonus_cut" & vbCrLf
            cmdText2 += "              ,(taxable.remuneration" & vbCrLf
            cmdText2 += "              + taxable.monthly_cut" & vbCrLf
            cmdText2 += "              + taxable.bonus_cut) AS taxable" & vbCrLf
            cmdText2 += "              ,taxable.[truncate] AS [truncate]" & vbCrLf
            cmdText2 += "              ,taxable.withholding AS withholding" & vbCrLf
            cmdText2 += "              ,taxable.c_user_id AS c_user_id" & vbCrLf
            cmdText2 += "              ,member.k_belonging AS k_belonging" & vbCrLf
            cmdText2 += "              ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText2 += "          FROM (" & vbCrLf
            cmdText2 += "                SELECT s_officer_pay AS remuneration" & vbCrLf
            cmdText2 += "                      ,(s_pay_time_cut_monthly" & vbCrLf
            cmdText2 += "                      + s_pay_strike_cut_monthly) AS monthly_cut" & vbCrLf
            cmdText2 += "                      ,(s_pay_time_cut_once" & vbCrLf
            cmdText2 += "                      + s_pay_strike_cut_once) AS bonus_cut" & vbCrLf
            cmdText2 += "                      ,(s_pay_time_cut_monthly_break" & vbCrLf
            cmdText2 += "                      + s_pay_strike_cut_monthly_break" & vbCrLf
            cmdText2 += "                      + s_pay_time_cut_once_break" & vbCrLf
            cmdText2 += "                      + s_pay_strike_cut_once_break) AS [truncate]" & vbCrLf
            cmdText2 += "                      ,(s_cut_monthly_taxation" & vbCrLf
            cmdText2 += "                      + s_cut_once_taxation) AS withholding" & vbCrLf
            cmdText2 += "                      ,taxation_total.c_user_id AS c_user_id" & vbCrLf
            cmdText2 += "                  FROM taxation_total" & vbCrLf
            cmdText2 += "                 WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                   AND c_pay_once_name = :c_pay_once_name" & vbCrLf
            cmdText2 += "                   AND c_user_id NOT IN (" & vbCrLf
            cmdText2 += "                       SELECT c_user_id" & vbCrLf
            cmdText2 += "                         FROM taxation_total" & vbCrLf
            cmdText2 += "                        WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                          AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                        GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                       HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText2 += "                              OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText2 += "                              OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText2 += "                       )" & vbCrLf
            cmdText2 += "               ) taxable" & vbCrLf
            cmdText2 += "               LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "                   SELECT A1.*" & vbCrLf
            cmdText2 += "                     FROM staf_attribute A1" & vbCrLf
            cmdText2 += "                         ,(" & vbCrLf
            cmdText2 += "                           SELECT c_user_id" & vbCrLf
            cmdText2 += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText2 += "                             FROM staf_attribute" & vbCrLf
            cmdText2 += "                            WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                              AND c_ksh <= :c_ksh" & vbCrLf
            cmdText2 += "                            GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                          ) B1" & vbCrLf
            cmdText2 += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText2 += "                      AND A1.d_from = B1.d_from" & vbCrLf
            cmdText2 += "               ) member" & vbCrLf
            cmdText2 += "               ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText2 += "         WHERE member.k_belonging = :k_belonging" & vbCrLf
            cmdText2 += "       ) taxable_persons" & vbCrLf
            cmdText2 += "       LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "           SELECT A4.*" & vbCrLf
            cmdText2 += "             FROM qualification_view A4" & vbCrLf
            cmdText2 += "                 ,(" & vbCrLf
            cmdText2 += "                   SELECT c_constant_seq" & vbCrLf
            cmdText2 += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            cmdText2 += "                     FROM qualification_view" & vbCrLf
            cmdText2 += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText2 += "                  ) B4" & vbCrLf
            cmdText2 += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            cmdText2 += "              AND A4.d_from = B4.d_from" & vbCrLf
            cmdText2 += "       ) license" & vbCrLf
            cmdText2 += "       ON taxable_persons.k_qualification = license.c_constant_seq" & vbCrLf
            cmdText2 += " ORDER BY RIGHT('0000000000' + taxable_persons.c_staf_id)" & vbCrLf

            Try
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("k_belonging").Value = UnionBranch
                If OnceName = "" Then
                    command.SetSql(cmdText1)
                Else
                    command.SetSql(cmdText2)
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If
                Me.AddCutDivParameterValue(command)
                WithHoldingDao._logger.Debug(cmdText1)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("withholding_non_taxable_list", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　月例賃金集計タブ　検索ボタン押下時処理
        ''' 源泉徴収　一時金集計タブ　検索ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns>課税非対象者合計一覧（データテーブル）</returns>
        ''' <remarks>
        ''' MOD 2012/06/15
        ''' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' </remarks>
        Public Function GetNonTaxableSummary( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal OnceName As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyNonTaxableSumListMap
            Dim cmdText1 As String = ""
            Dim cmdText2 As String = ""

            ' 一時金名称無
            cmdText1 = ""
            cmdText1 += "SELECT u_branch.l_name AS [" & map.GetLogicalName(0) & "]" & vbCrLf            ' 01. 支部
            cmdText1 += "      ,subtotal.s_cut_monthly AS [" & map.GetLogicalName(1) & "]" & vbCrLf     ' 02. 月例賃金控除額
            cmdText1 += "      ,subtotal.s_cut_once AS [" & map.GetLogicalName(2) & "]" & vbCrLf        ' 03. 一時金控除額
            cmdText1 += "      ,(subtotal.s_cut_monthly" & vbCrLf                                       ' 04. 控除額計
            cmdText1 += "      + subtotal.s_cut_once ) AS [" & map.GetLogicalName(3) & "]" & vbCrLf
            cmdText1 += "      ,subtotal.s_break AS [" & map.GetLogicalName(4) & "]" & vbCrLf           ' 05. 切捨て額
            cmdText1 += "      ,(subtotal.s_cut_monthly" & vbCrLf
            cmdText1 += "      + subtotal.s_cut_once" & vbCrLf
            cmdText1 += "      - subtotal.s_break) AS [" & map.GetLogicalName(5) & "]" & vbCrLf         ' 06. 差引支給額
            cmdText1 += " FROM (" & vbCrLf
            cmdText1 += "       SELECT member.k_belonging" & vbCrLf
            cmdText1 += "             ,(SUM(taxable.s_pay_time_cut_monthly)" & vbCrLf
            cmdText1 += "             + SUM(taxable.s_pay_strike_cut_monthly)) AS s_cut_monthly" & vbCrLf
            cmdText1 += "             ,(SUM(taxable.s_pay_time_cut_monthly_break)" & vbCrLf
            cmdText1 += "             + SUM(taxable.s_pay_strike_cut_monthly_break)" & vbCrLf
            cmdText1 += "             + SUM(taxable.s_pay_time_cut_once_break)" & vbCrLf
            cmdText1 += "             + SUM(taxable.s_pay_strike_cut_once_break)) AS s_break" & vbCrLf
            cmdText1 += "             ,(SUM(taxable.s_pay_time_cut_once)" & vbCrLf
            cmdText1 += "             + SUM(taxable.s_pay_strike_cut_once)) AS s_cut_once" & vbCrLf
            cmdText1 += "         FROM (" & vbCrLf
            cmdText1 += "               SELECT *" & vbCrLf
            cmdText1 += "                 FROM taxation_total" & vbCrLf
            cmdText1 += "                WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                  AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                  AND c_user_id NOT IN (" & vbCrLf
            cmdText1 += "                      SELECT c_user_id" & vbCrLf
            cmdText1 += "                        FROM taxation_total" & vbCrLf
            cmdText1 += "                       WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                         AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                       GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                      HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText1 += "                             OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText1 += "                             OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText1 += "                      )" & vbCrLf
            cmdText1 += "              ) taxable " & vbCrLf
            cmdText1 += "              LEFT OUTER JOIN(" & vbCrLf
            cmdText1 += "                  SELECT A1.*" & vbCrLf
            cmdText1 += "                    FROM staf_attribute A1" & vbCrLf
            cmdText1 += "                        ,(" & vbCrLf
            cmdText1 += "                          SELECT c_user_id" & vbCrLf
            cmdText1 += "                                ,MAX(d_from) AS d_from" & vbCrLf
            cmdText1 += "                            FROM staf_attribute" & vbCrLf
            cmdText1 += "                           WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                             AND c_ksh <= :c_ksh" & vbCrLf
            cmdText1 += "                           GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                         ) B1" & vbCrLf
            cmdText1 += "                   WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText1 += "                     AND A1.d_from = B1.d_from" & vbCrLf
            cmdText1 += "              ) member" & vbCrLf
            cmdText1 += "              ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText1 += "        GROUP BY member.k_belonging" & vbCrLf
            cmdText1 += "      ) subtotal" & vbCrLf
            cmdText1 += "      LEFT OUTER JOIN (" & vbCrLf
            cmdText1 += "          SELECT A3.*" & vbCrLf
            cmdText1 += "            FROM belonging_view A3" & vbCrLf
            cmdText1 += "                ,(" & vbCrLf
            cmdText1 += "                  SELECT c_constant_seq" & vbCrLf
            cmdText1 += "                        ,MAX(d_from) AS d_from" & vbCrLf
            cmdText1 += "                    FROM belonging_view" & vbCrLf
            cmdText1 += "                   WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                   GROUP BY c_constant_seq" & vbCrLf
            cmdText1 += "                 ) B3" & vbCrLf
            cmdText1 += "           WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText1 += "             AND A3.d_from = B3.d_from" & vbCrLf
            cmdText1 += "      ) u_branch" & vbCrLf
            cmdText1 += "      ON subtotal.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText1 += "ORDER BY subtotal.k_belonging" & vbCrLf

            ' 一時名称有
            cmdText2 = ""
            cmdText2 += "SELECT u_branch.l_name AS [" & map.GetLogicalName(0) & "]" & vbCrLf
            cmdText2 += "      ,subtotal.s_cut_monthly AS [" & map.GetLogicalName(1) & "]" & vbCrLf
            cmdText2 += "      ,subtotal.s_cut_once AS [" & map.GetLogicalName(2) & "]" & vbCrLf
            cmdText2 += "      ,(subtotal.s_cut_monthly" & vbCrLf
            cmdText2 += "      + subtotal.s_cut_once) AS [" & map.GetLogicalName(3) & "]" & vbCrLf
            cmdText2 += "      ,subtotal.s_break AS [" & map.GetLogicalName(4) & "]" & vbCrLf
            cmdText2 += "      ,(subtotal.s_cut_monthly" & vbCrLf
            cmdText2 += "      + subtotal.s_cut_once" & vbCrLf
            cmdText2 += "      - subtotal.s_break) AS [" & map.GetLogicalName(5) & "]" & vbCrLf
            cmdText2 += "  FROM (" & vbCrLf
            cmdText2 += "        SELECT member.k_belonging" & vbCrLf
            cmdText2 += "              ,(SUM(taxable.s_pay_time_cut_monthly)" & vbCrLf
            cmdText2 += "              + SUM(taxable.s_pay_strike_cut_monthly)) AS s_cut_monthly" & vbCrLf
            cmdText2 += "              ,(SUM(taxable.s_pay_time_cut_monthly_break)" & vbCrLf
            cmdText2 += "              + SUM(taxable.s_pay_strike_cut_monthly_break)" & vbCrLf
            cmdText2 += "              + SUM(taxable.s_pay_time_cut_once_break)" & vbCrLf
            cmdText2 += "              + SUM(taxable.s_pay_strike_cut_once_break)) AS s_break" & vbCrLf
            cmdText2 += "              ,(SUM(taxable.s_pay_time_cut_once)" & vbCrLf
            cmdText2 += "              + SUM(taxable.s_pay_strike_cut_once)) AS s_cut_once" & vbCrLf
            cmdText2 += "          FROM (" & vbCrLf
            cmdText2 += "                SELECT *" & vbCrLf
            cmdText2 += "                  FROM taxation_total" & vbCrLf
            cmdText2 += "                 WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                   AND c_pay_once_name = :c_pay_once_name" & vbCrLf
            cmdText2 += "                   AND c_user_id NOT IN (" & vbCrLf
            cmdText2 += "                       SELECT c_user_id" & vbCrLf
            cmdText2 += "                         FROM taxation_total" & vbCrLf
            cmdText2 += "                        WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                          AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                          GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                          HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText2 += "                                 OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText2 += "                                 OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText2 += "                       )" & vbCrLf
            cmdText2 += "               ) taxable" & vbCrLf
            cmdText2 += "               LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "                   SELECT A1.*" & vbCrLf
            cmdText2 += "                     FROM staf_attribute A1" & vbCrLf
            cmdText2 += "                         ,(" & vbCrLf
            cmdText2 += "                           SELECT c_user_id" & vbCrLf
            cmdText2 += "                                 ,MAX(d_from) AS d_from" & vbCrLf
            cmdText2 += "                             FROM staf_attribute" & vbCrLf
            cmdText2 += "                            WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                              AND c_ksh <= :c_ksh" & vbCrLf
            cmdText2 += "                            GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                          ) B1" & vbCrLf
            cmdText2 += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText2 += "                      AND A1.d_from = B1.d_from" & vbCrLf
            cmdText2 += "               ) member" & vbCrLf
            cmdText2 += "               ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText2 += "         GROUP BY member.k_belonging" & vbCrLf
            cmdText2 += "       ) subtotal" & vbCrLf
            cmdText2 += "       LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "           SELECT A3.*" & vbCrLf
            cmdText2 += "             FROM belonging_view A3" & vbCrLf
            cmdText2 += "                 ,(" & vbCrLf
            cmdText2 += "                   SELECT c_constant_seq" & vbCrLf
            cmdText2 += "                         ,MAX(d_from) AS d_from" & vbCrLf
            cmdText2 += "                     FROM belonging_view" & vbCrLf
            cmdText2 += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText2 += "                  ) B3" & vbCrLf
            cmdText2 += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText2 += "              AND A3.d_from = B3.d_from" & vbCrLf
            cmdText2 += "       ) u_branch" & vbCrLf
            cmdText2 += "       ON subtotal.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText2 += " ORDER BY subtotal.k_belonging" & vbCrLf
            'Dim cmdText1 As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", subtotal.s_cut_monthly AS ", map.GetLogicalName(1), ", subtotal.s_cut_once AS ", map.GetLogicalName(2), ", subtotal.s_cut_monthly + subtotal.s_cut_once AS ", map.GetLogicalName(3), ", subtotal.s_break AS ", map.GetLogicalName(4), ", subtotal.s_cut_monthly + subtotal.s_cut_once - subtotal.s_break AS ", map.GetLogicalName(5), " FROM (SELECT member.k_belonging, SUM(taxable.s_pay_time_cut_monthly) + SUM(taxable.s_pay_strike_cut_monthly) AS s_cut_monthly, SUM(taxable.s_pay_time_cut_monthly_break) + SUM(taxable.s_pay_strike_cut_monthly_break) + SUM(taxable.s_pay_time_cut_once_break) + SUM(taxable.s_pay_strike_cut_once_break) AS s_break, SUM(taxable.s_pay_time_cut_once) + SUM(taxable.s_pay_strike_cut_once) AS s_cut_once FROM ( SELECT * FROM taxation_total WHERE TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind AND  ( c_user_id NOT IN (select " & _
            '    "c_user_id from taxation_total where TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind group by c_user_id having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) ) taxable LEFT OUTER JOIN(SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON taxable.c_user_id = member.c_user_id GROUP BY member.k_belonging ) subtotal LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON subtotal.k_belonging = u_branch.c_constant_seq ORDER BY subtotal.k_belonging"})
            'Dim cmdText2 As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", subtotal.s_cut_monthly AS ", map.GetLogicalName(1), ", subtotal.s_cut_once AS ", map.GetLogicalName(2), ", subtotal.s_cut_monthly + subtotal.s_cut_once AS ", map.GetLogicalName(3), ", subtotal.s_break AS ", map.GetLogicalName(4), ", subtotal.s_cut_monthly + subtotal.s_cut_once - subtotal.s_break AS ", map.GetLogicalName(5), " FROM (SELECT member.k_belonging, SUM(taxable.s_pay_time_cut_monthly) + SUM(taxable.s_pay_strike_cut_monthly) AS s_cut_monthly, SUM(taxable.s_pay_time_cut_monthly_break) + SUM(taxable.s_pay_strike_cut_monthly_break) + SUM(taxable.s_pay_time_cut_once_break) + SUM(taxable.s_pay_strike_cut_once_break) AS s_break, SUM(taxable.s_pay_time_cut_once) + SUM(taxable.s_pay_strike_cut_once) AS s_cut_once FROM ( SELECT * FROM taxation_total WHERE TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind AND c_pay_once_name = :c_pay_once_name AND  ( c_user_id NOT IN (select " & _
            '    "c_user_id from taxation_total where TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind group by c_user_id having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) ) taxable LEFT OUTER JOIN(SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON taxable.c_user_id = member.c_user_id GROUP BY member.k_belonging ) subtotal LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON subtotal.k_belonging = u_branch.c_constant_seq ORDER BY subtotal.k_belonging"})

            Try
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                If OnceName = "" Then
                    command.SetSql(cmdText1)
                Else
                    command.SetSql(cmdText2)
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If
                Me.AddCutDivParameterValue(command)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("withholding_non_taxable_summary", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　検索ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税非対象者合計一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetNonTaxableSumUpData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpNonTaxableSumListMap
            Dim cmdText As String = ""

            ' SQL作成
            cmdText = ""
            cmdText += "SELECT u_branch.l_name AS [" & map.GetLogicalName(0) & "]" & vbCrLf                 ' 01. 支部
            cmdText += "      ,nontaxable_sumup.monthly_cover AS [" & map.GetLogicalName(1) & "]" & vbCrLf  ' 02. 月例賃金控除補填額
            cmdText += "      ,nontaxable_sumup.bonus_cover AS [" & map.GetLogicalName(2) & "]" & vbCrLf    ' 03. 一時金控除補填額
            cmdText += "      ,nontaxable_sumup.total_pay AS [" & map.GetLogicalName(3) & "]" & vbCrLf      ' 04. 支給総額
            cmdText += "  FROM (" & vbCrLf
            cmdText += "        SELECT member.k_belonging AS k_belonging" & vbCrLf
            cmdText += "              ,SUM(nontaxable.monthly_cover) AS monthly_cover" & vbCrLf
            cmdText += "              ,SUM(nontaxable.bonus_cover) AS bonus_cover" & vbCrLf
            cmdText += "              ,(SUM(nontaxable.monthly_cover)" & vbCrLf
            cmdText += "              + SUM(nontaxable.bonus_cover)) AS total_pay" & vbCrLf
            cmdText += "          FROM (" & vbCrLf
            cmdText += "                SELECT c_user_id" & vbCrLf
            cmdText += "                      ,(s_pay_time_cut_monthly" & vbCrLf
            cmdText += "                      - s_pay_time_cut_monthly_break)" & vbCrLf
            cmdText += "                      + (s_pay_strike_cut_monthly" & vbCrLf
            cmdText += "                      - s_pay_strike_cut_monthly_break) AS monthly_cover" & vbCrLf
            cmdText += "                      ,(s_pay_time_cut_once" & vbCrLf
            cmdText += "                      + s_pay_strike_cut_once)" & vbCrLf
            cmdText += "                      - (s_pay_time_cut_once_break" & vbCrLf
            cmdText += "                      + s_pay_strike_cut_once_break) AS bonus_cover" & vbCrLf
            cmdText += "                  FROM taxation_total" & vbCrLf
            cmdText += "                 WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
            cmdText += "                   AND NOT (s_officer_pay <> 0" & vbCrLf
            cmdText += "                       OR s_cut_monthly_taxation <> 0" & vbCrLf
            cmdText += "                       OR s_cut_once_taxation <> 0)" & vbCrLf
            cmdText += "               ) nontaxable" & vbCrLf
            cmdText += "               LEFT OUTER JOIN (" & vbCrLf
            cmdText += "                   SELECT A1.*" & vbCrLf
            cmdText += "                     FROM staf_attribute A1" & vbCrLf
            cmdText += "                         ,(" & vbCrLf
            cmdText += "                           SELECT c_user_id" & vbCrLf
            cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText += "                             FROM staf_attribute" & vbCrLf
            cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
            cmdText += "                            GROUP BY c_user_id" & vbCrLf
            cmdText += "                          ) B1" & vbCrLf
            cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
            cmdText += "               ) member" & vbCrLf
            cmdText += "               ON member.c_user_id = nontaxable.c_user_id" & vbCrLf
            cmdText += "         GROUP BY member.k_belonging" & vbCrLf
            cmdText += "       ) nontaxable_sumup" & vbCrLf
            cmdText += "       LEFT OUTER JOIN (" & vbCrLf
            cmdText += "           SELECT A3.*" & vbCrLf
            cmdText += "             FROM belonging_view A3" & vbCrLf
            cmdText += "                 ,(" & vbCrLf
            cmdText += "                   SELECT c_constant_seq" & vbCrLf
            cmdText += "                         ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
            cmdText += "                     FROM belonging_view" & vbCrLf
            cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText += "                  ) B3" & vbCrLf
            cmdText += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText += "              AND A3.d_from = B3.d_from" & vbCrLf
            cmdText += "       ) u_branch" & vbCrLf
            cmdText += "       ON nontaxable_sumup.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText += " ORDER BY nontaxable_sumup.k_belonging" & vbCrLf
            'todo:
            'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", nontaxable_sumup.monthly_cover AS ", map.GetLogicalName(1), ", nontaxable_sumup.bonus_cover AS ", map.GetLogicalName(2), ", nontaxable_sumup.total_pay AS ", map.GetLogicalName(3), " FROM ( SELECT member.k_belonging AS k_belonging, SUM(nontaxable.monthly_cover) AS monthly_cover, SUM(nontaxable.bonus_cover) AS bonus_cover, SUM(nontaxable.monthly_cover) + SUM(nontaxable.bonus_cover) AS total_pay FROM ( SELECT c_user_id, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND NOT " & _
            '    "(s_officer_pay <> 0 OR s_cut_monthly_taxation <> 0 OR s_cut_once_taxation <> 0) ) nontaxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = nontaxable.c_user_id GROUP BY member.k_belonging ) nontaxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(belonging_view.d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON nontaxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY nontaxable_sumup.k_belonging"})
            'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS """, map.GetLogicalName(0), """, nontaxable_sumup.monthly_cover AS """, map.GetLogicalName(1), """, nontaxable_sumup.bonus_cover AS """, map.GetLogicalName(2), """, nontaxable_sumup.total_pay AS """, map.GetLogicalName(3), """ FROM ( SELECT member.k_belonging AS k_belonging, SUM(nontaxable.monthly_cover) AS monthly_cover, SUM(nontaxable.bonus_cover) AS bonus_cover, SUM(nontaxable.monthly_cover) + SUM(nontaxable.bonus_cover) AS total_pay FROM ( SELECT c_user_id, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND  ( (c_user_id, d_years) NOT IN (select " & _
            '    "c_user_id, d_years from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) ) nontaxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = nontaxable.c_user_id GROUP BY member.k_belonging ) nontaxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON nontaxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY nontaxable_sumup.k_belonging"})
            'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", nontaxable_sumup.monthly_cover AS ", map.GetLogicalName(1), ", nontaxable_sumup.bonus_cover AS ", map.GetLogicalName(2), ", nontaxable_sumup.total_pay AS ", map.GetLogicalName(3), " FROM ( SELECT member.k_belonging AS k_belonging, SUM(nontaxable.monthly_cover) AS monthly_cover, SUM(nontaxable.bonus_cover) AS bonus_cover, SUM(nontaxable.monthly_cover) + SUM(nontaxable.bonus_cover) AS total_pay FROM ( SELECT c_user_id, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND  ( (c_user_id) NOT IN (select " & _
            '    "c_user_id from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0))) ) nontaxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = nontaxable.c_user_id GROUP BY member.k_belonging ) nontaxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(belonging_view.d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON nontaxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY nontaxable_sumup.k_belonging"})

            Try
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                WithHoldingDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("dtDetail_nontaxable", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収 - 課税非対象者累計画面の社員番号リンクボタン押下時
        ''' 源泉徴収 - 課税非対象者累計画面の明細印刷ボタン押下時
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="SelectedMembers">対象社員番号リスト</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税非対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetPaymentSlipReportDetailData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal SelectedMembers As ArrayList, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingPaymentReportDetailMap
            Dim builder As New StringBuilder("")
            Dim format As String = ""

            ' SQL文作成
            format = ""
            format += "SELECT nontaxable_sumup.year" & vbCrLf       ' 01. 対象年
            format += "      ,nontaxable_sumup.c_staf_id" & vbCrLf  ' 02. 社員番号
            format += "      ,nontaxable_sumup.name" & vbCrLf       ' 03. 名前
            format += "      ,model.l_omission_name" & vbCrLf       ' 04. 機種
            format += "      ,license.l_omission_name" & vbCrLf     ' 05. 資格
            format += "      ,u_branch.l_omission_name" & vbCrLf    ' 06. 支部
            format += "      ,nontaxable_sumup.total_pay" & vbCrLf  ' 07. 支給総額
            format += "  FROM (" & vbCrLf
            format += "           (" & vbCrLf
            format += "               (" & vbCrLf
            format += "                   (" & vbCrLf
            format += "                    SELECT nontaxable.year AS [year]" & vbCrLf
            format += "                          ,member.c_staf_id AS c_staf_id" & vbCrLf
            format += "                          ,member.l_name AS name" & vbCrLf
            format += "                          ,member.k_model" & vbCrLf
            format += "                          ,member.k_qualification AS k_qualification" & vbCrLf
            format += "                          ,member.k_belonging" & vbCrLf
            format += "                          ,nontaxable.monthly_cover + nontaxable.bonus_cover AS total_pay" & vbCrLf
            format += "                          ,nontaxable.c_user_id" & vbCrLf
            format += "                      FROM (" & vbCrLf
            format += "                            SELECT c_user_id" & vbCrLf
            format += "                                  ,FORMAT(d_years, 'yyyy') AS [year]" & vbCrLf
            format += "                                  ,(SUM(s_pay_time_cut_monthly)" & vbCrLf
            format += "                                  - SUM(s_pay_time_cut_monthly_break))" & vbCrLf
            format += "                                  + (SUM(s_pay_strike_cut_monthly)" & vbCrLf
            format += "                                  - SUM(s_pay_strike_cut_monthly_break)) AS monthly_cover" & vbCrLf
            format += "                                  ,(SUM(s_pay_time_cut_once)" & vbCrLf
            format += "                                  + SUM(s_pay_strike_cut_once))" & vbCrLf
            format += "                                  - (SUM(s_pay_time_cut_once_break)" & vbCrLf
            format += "                                  + SUM(s_pay_strike_cut_once_break)) AS bonus_cover" & vbCrLf
            format += "                              FROM taxation_total" & vbCrLf
            format += "                             WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
            format += "                               AND NOT (s_officer_pay <> 0" & vbCrLf
            format += "                                   OR s_cut_monthly_taxation <> 0" & vbCrLf
            format += "                                   OR s_cut_once_taxation <> 0)" & vbCrLf
            format += "                               AND c_user_id IN ({0})" & vbCrLf
            format += "                             GROUP BY c_user_id, FORMAT(d_years, 'yyyy')" & vbCrLf
            format += "                           ) nontaxable" & vbCrLf
            format += "                           LEFT OUTER JOIN (" & vbCrLf
            format += "                               SELECT A1.*" & vbCrLf
            format += "                                 FROM staf_attribute A1" & vbCrLf
            format += "                                     ,(" & vbCrLf
            format += "                                       SELECT c_user_id" & vbCrLf
            format += "                                             ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            format += "                                         FROM staf_attribute" & vbCrLf
            format += "                                        WHERE d_from <= :d_from" & vbCrLf
            format += "                                          AND c_ksh <= :c_ksh" & vbCrLf
            format += "                                        GROUP BY c_user_id" & vbCrLf
            format += "                                      ) B1" & vbCrLf
            format += "                                WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            format += "                                  AND A1.d_from = B1.d_from" & vbCrLf
            format += "                           ) member" & vbCrLf
            format += "                           ON member.c_user_id = nontaxable.c_user_id" & vbCrLf
            format += "                   ) nontaxable_sumup" & vbCrLf
            format += "                   LEFT OUTER JOIN (" & vbCrLf
            format += "                       SELECT A5.*" & vbCrLf
            format += "                         FROM model_view A5" & vbCrLf
            format += "                             ,(" & vbCrLf
            format += "                               SELECT c_constant_seq" & vbCrLf
            format += "                                     ,MAX(model_view.d_from) AS d_from" & vbCrLf
            format += "                                 FROM model_view" & vbCrLf
            format += "                                WHERE d_from <= :d_from" & vbCrLf
            format += "                                GROUP BY c_constant_seq" & vbCrLf
            format += "                              ) B5" & vbCrLf
            format += "                        WHERE A5.c_constant_seq = B5.c_constant_seq" & vbCrLf
            format += "                          AND A5.d_from = B5.d_from" & vbCrLf
            format += "                   ) model" & vbCrLf
            format += "                   ON nontaxable_sumup.k_model = model.c_constant_seq" & vbCrLf
            format += "               ) LEFT OUTER JOIN (" & vbCrLf
            format += "                   SELECT A4.*" & vbCrLf
            format += "                     FROM qualification_view A4" & vbCrLf
            format += "                         ,(" & vbCrLf
            format += "                           SELECT c_constant_seq" & vbCrLf
            format += "                                 ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            format += "                             FROM qualification_view" & vbCrLf
            format += "                            WHERE d_from <= :d_from" & vbCrLf
            format += "                            GROUP BY c_constant_seq" & vbCrLf
            format += "                          ) B4" & vbCrLf
            format += "                    WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            format += "                      AND A4.d_from = B4.d_from" & vbCrLf
            format += "               ) license ON nontaxable_sumup.k_qualification = license.c_constant_seq" & vbCrLf
            format += "           ) LEFT OUTER JOIN (" & vbCrLf
            format += "               SELECT A3.*" & vbCrLf
            format += "                 FROM belonging_view A3" & vbCrLf
            format += "                     ,(" & vbCrLf
            format += "                       SELECT c_constant_seq" & vbCrLf
            format += "                             ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
            format += "                         FROM belonging_view" & vbCrLf
            format += "                        WHERE d_from <= :d_from" & vbCrLf
            format += "                        GROUP BY c_constant_seq" & vbCrLf
            format += "                      ) B3" & vbCrLf
            format += "                WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            format += "                  AND A3.d_from = B3.d_from" & vbCrLf
            format += "           ) u_branch" & vbCrLf
            format += "           ON nontaxable_sumup.k_belonging = u_branch.c_constant_seq" & vbCrLf
            format += "       )" & vbCrLf
            format += " ORDER BY RIGHT('0000000000' + nontaxable_sumup.c_staf_id, 10)" & vbCrLf
            'todo:

            Try
                Dim i As Integer
                For i = 0 To SelectedMembers.Count - 1
                    If (builder.Length > 0) Then
                        builder.Append((",'" & CStr(SelectedMembers.Item(i)) & "'"))
                    Else
                        builder.Append(("'" & CStr(SelectedMembers.Item(i)) & "'"))
                    End If
                Next i
                Dim cmdText As String = String.Format(format, builder.ToString)
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                WithHoldingDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtHeader", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　月例賃金集計タブ　課税対象者の一覧プレ印刷ボタン押下時
        ''' 源泉徴収　一時金集計タブ　課税対象者の一覧プレ印刷ボタン押下時
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時名称</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks>
        ''' MOD 2012/06/15
        ''' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' MOD 2017/08/25 c_taxation_flag 追加
        ''' </remarks>
        Public Function GetPrintListData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal OnceName As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyTaxableReportListMap
            Dim cmdText1 As String = ""
            Dim cmdText2 As String = ""

            '-----------------------------------------------------------------------------------
            '   SQL文作成
            '-----------------------------------------------------------------------------------
            ' 一時金名称無
            cmdText1 = ""
            cmdText1 += "SELECT taxable_persons.c_staf_id AS [" & map.GetLogicalName(0) & "]" & vbCrLf          ' 01. 社員番号
            cmdText1 += "      ,taxable_persons.name AS [" & map.GetLogicalName(1) & "]" & vbCrLf               ' 02. 名前
            cmdText1 += "      ,u_branch.l_name AS [" & map.GetLogicalName(2) & "]" & vbCrLf                    ' 03. 支部
            cmdText1 += "      ,qualification_view.l_omission_name AS [" & map.GetLogicalName(3) & "]" & vbCrLf ' 04. 資格
            cmdText1 += "      ,taxable_persons.remuneration AS [" & map.GetLogicalName(4) & "]" & vbCrLf       ' 05. 役員手当
            cmdText1 += "      ,taxable_persons.i_monthly_cut AS [" & map.GetLogicalName(5) & "]" & vbCrLf      ' 06. 時間内月例控除額
            cmdText1 += "      ,taxable_persons.i_bonus_cut AS [" & map.GetLogicalName(6) & "]" & vbCrLf        ' 07. 時間内一時金控除額
            cmdText1 += "      ,taxable_persons.s_monthly_cut AS [" & map.GetLogicalName(7) & "]" & vbCrLf      ' 08. 争議行為月例控除額
            cmdText1 += "      ,taxable_persons.s_bonus_cut AS [" & map.GetLogicalName(8) & "]" & vbCrLf        ' 09. 争議行為一時金控除額
            cmdText1 += "      ,taxable_persons.monthly_trunc AS [" & map.GetLogicalName(9) & "]" & vbCrLf      ' 10. 時間内切捨て額
            cmdText1 += "      ,taxable_persons.bonus_trunc AS [" & map.GetLogicalName(10) & "]" & vbCrLf       ' 11. 争議行為切捨て額
            cmdText1 += "      ,taxable_persons.withholding AS [" & map.GetLogicalName(11) & "]" & vbCrLf       ' 12. 源泉徴収額
            cmdText1 += "      ,taxable_persons.[truncate] AS [" & map.GetLogicalName(12) & "]" & vbCrLf          ' 13. 切捨て額
            cmdText1 += "      ,taxable_persons.c_taxation_flag AS [" & map.GetLogicalName(13) & "]" & vbCrLf     ' 14. 課税フラグ
            cmdText1 += "  FROM (" & vbCrLf
            cmdText1 += "           (" & vbCrLf
            cmdText1 += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText1 += "                  ,member.l_name AS name" & vbCrLf
            cmdText1 += "                  ,member.k_belonging AS k_belonging" & vbCrLf
            cmdText1 += "                  ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText1 += "                  ,taxable.remuneration AS remuneration" & vbCrLf
            cmdText1 += "                  ,taxable.i_monthly_cut AS i_monthly_cut" & vbCrLf
            cmdText1 += "                  ,taxable.i_bonus_cut AS i_bonus_cut" & vbCrLf
            cmdText1 += "                  ,taxable.s_monthly_cut AS s_monthly_cut" & vbCrLf
            cmdText1 += "                  ,taxable.s_bonus_cut AS s_bonus_cut" & vbCrLf
            cmdText1 += "                  ,taxable.monthly_trunc AS monthly_trunc" & vbCrLf
            cmdText1 += "                  ,taxable.bonus_trunc AS bonus_trunc" & vbCrLf
            cmdText1 += "                  ,taxable.withholding AS withholding" & vbCrLf
            cmdText1 += "                  ,(taxable.monthly_trunc + taxable.bonus_trunc) AS [truncate]" & vbCrLf
            cmdText1 += "                  ,taxable.c_user_id AS c_user_id" & vbCrLf
            cmdText1 += "                  ,taxable.c_taxation_flag AS c_taxation_flag" & vbCrLf
            cmdText1 += "              FROM (" & vbCrLf
            cmdText1 += "                    SELECT s_officer_pay AS remuneration" & vbCrLf
            cmdText1 += "                          ,s_pay_time_cut_monthly AS i_monthly_cut" & vbCrLf
            cmdText1 += "                          ,s_pay_time_cut_once AS i_bonus_cut" & vbCrLf
            cmdText1 += "                          ,s_pay_strike_cut_monthly AS s_monthly_cut" & vbCrLf
            cmdText1 += "                          ,s_pay_strike_cut_once AS s_bonus_cut" & vbCrLf
            cmdText1 += "                          ,(s_pay_time_cut_monthly_break + s_pay_strike_cut_monthly_break) AS monthly_trunc" & vbCrLf
            cmdText1 += "                          ,(s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_trunc" & vbCrLf
            cmdText1 += "                          ,(s_cut_monthly_taxation + s_cut_once_taxation) AS withholding" & vbCrLf
            cmdText1 += "                          ,taxation_total.c_user_id AS c_user_id" & vbCrLf
            cmdText1 += "                          ,c_taxation_flag AS c_taxation_flag" & vbCrLf
            cmdText1 += "                      FROM taxation_total" & vbCrLf
            cmdText1 += "                     WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                       AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                       AND c_user_id IN (" & vbCrLf
            cmdText1 += "                           SELECT c_user_id" & vbCrLf
            cmdText1 += "                             FROM taxation_total" & vbCrLf
            cmdText1 += "                            WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText1 += "                              AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText1 += "                            GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                           HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText1 += "                                  OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText1 += "                                  OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText1 += "                           )" & vbCrLf
            cmdText1 += "                   ) taxable" & vbCrLf
            cmdText1 += "                   LEFT OUTER JOIN (" & vbCrLf
            cmdText1 += "                       SELECT A1.*" & vbCrLf
            cmdText1 += "                         FROM staf_attribute A1" & vbCrLf
            cmdText1 += "                             ,(" & vbCrLf
            cmdText1 += "                               SELECT c_user_id" & vbCrLf
            cmdText1 += "                                     ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText1 += "                                 FROM staf_attribute" & vbCrLf
            cmdText1 += "                                WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                                  AND c_ksh <= :c_ksh" & vbCrLf
            cmdText1 += "                                GROUP BY c_user_id" & vbCrLf
            cmdText1 += "                              ) B1" & vbCrLf
            cmdText1 += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText1 += "                          AND A1.d_from = B1.d_from" & vbCrLf
            cmdText1 += "                   ) member" & vbCrLf
            cmdText1 += "                   ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText1 += "           ) taxable_persons" & vbCrLf
            cmdText1 += "           LEFT OUTER JOIN qualification_view" & vbCrLf
            cmdText1 += "           ON taxable_persons.k_qualification = qualification_view.c_constant_seq" & vbCrLf
            cmdText1 += "       ) LEFT OUTER JOIN (" & vbCrLf
            cmdText1 += "           SELECT A3.*" & vbCrLf
            cmdText1 += "             FROM belonging_view A3" & vbCrLf
            cmdText1 += "                 ,(" & vbCrLf
            cmdText1 += "                   SELECT c_constant_seq" & vbCrLf
            cmdText1 += "                         ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
            cmdText1 += "                     FROM belonging_view" & vbCrLf
            cmdText1 += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText1 += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText1 += "                  ) B3" & vbCrLf
            cmdText1 += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText1 += "              AND A3.d_from = B3.d_from" & vbCrLf
            cmdText1 += "       ) u_branch" & vbCrLf
            cmdText1 += "       ON taxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText1 += " ORDER BY RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf
            'todo:

            '一時金名称無
            cmdText2 = ""
            cmdText2 += "SELECT taxable_persons.c_staf_id AS [" & map.GetLogicalName(0) & "]" & vbCrLf          ' 01. 社員番号
            cmdText2 += "      ,taxable_persons.name AS [" & map.GetLogicalName(1) & "]" & vbCrLf               ' 02. 名前
            cmdText2 += "      ,u_branch.l_name AS [" & map.GetLogicalName(2) & "]" & vbCrLf                    ' 03. 支部
            cmdText2 += "      ,qualification_view.l_omission_name AS [" & map.GetLogicalName(3) & "]" & vbCrLf ' 04. 資格
            cmdText2 += "      ,taxable_persons.remuneration AS [" & map.GetLogicalName(4) & "]" & vbCrLf       ' 05. 役員手当
            cmdText2 += "      ,taxable_persons.i_monthly_cut AS [" & map.GetLogicalName(5) & "]" & vbCrLf      ' 06. 時間内月例控除額
            cmdText2 += "      ,taxable_persons.i_bonus_cut AS [" & map.GetLogicalName(6) & "]" & vbCrLf        ' 07. 時間内一時金控除額
            cmdText2 += "      ,taxable_persons.s_monthly_cut AS [" & map.GetLogicalName(7) & "]" & vbCrLf      ' 08. 争議行為月例控除額
            cmdText2 += "      ,taxable_persons.s_bonus_cut AS [" & map.GetLogicalName(8) & "]" & vbCrLf        ' 09. 争議行為一時金控除額
            cmdText2 += "      ,taxable_persons.monthly_trunc AS [" & map.GetLogicalName(9) & "]" & vbCrLf      ' 10. 時間内切捨て額
            cmdText2 += "      ,taxable_persons.bonus_trunc AS [" & map.GetLogicalName(10) & "]" & vbCrLf       ' 11. 争議行為切捨て額
            cmdText2 += "      ,taxable_persons.withholding AS [" & map.GetLogicalName(11) & "]" & vbCrLf       ' 12. 源泉徴収額
            cmdText2 += "      ,taxable_persons.[truncate] AS [" & map.GetLogicalName(12) & "]" & vbCrLf          ' 13. 切捨て額
            cmdText2 += "      ,taxable_persons.c_taxation_flag AS [" & map.GetLogicalName(13) & "]" & vbCrLf     ' 14. 課税フラグ
            cmdText2 += "  FROM (" & vbCrLf
            cmdText2 += "           (" & vbCrLf
            cmdText2 += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText2 += "                  ,member.l_name AS name" & vbCrLf
            cmdText2 += "                  ,member.k_belonging AS k_belonging" & vbCrLf
            cmdText2 += "                  ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText2 += "                  ,taxable.remuneration AS remuneration" & vbCrLf
            cmdText2 += "                  ,taxable.i_monthly_cut AS i_monthly_cut" & vbCrLf
            cmdText2 += "                  ,taxable.i_bonus_cut AS i_bonus_cut" & vbCrLf
            cmdText2 += "                  ,taxable.s_monthly_cut AS s_monthly_cut" & vbCrLf
            cmdText2 += "                  ,taxable.s_bonus_cut AS s_bonus_cut" & vbCrLf
            cmdText2 += "                  ,taxable.monthly_trunc AS monthly_trunc" & vbCrLf
            cmdText2 += "                  ,taxable.bonus_trunc AS bonus_trunc" & vbCrLf
            cmdText2 += "                  ,taxable.withholding AS withholding" & vbCrLf
            cmdText2 += "                  ,(taxable.monthly_trunc + taxable.bonus_trunc) AS [truncate]" & vbCrLf
            cmdText2 += "                  ,taxable.c_user_id AS c_user_id" & vbCrLf
            cmdText2 += "                  ,taxable.c_taxation_flag AS c_taxation_flag" & vbCrLf
            cmdText2 += "              FROM (" & vbCrLf
            cmdText2 += "                    SELECT s_officer_pay AS remuneration" & vbCrLf
            cmdText2 += "                          ,s_pay_time_cut_monthly AS i_monthly_cut" & vbCrLf
            cmdText2 += "                          ,s_pay_time_cut_once AS i_bonus_cut" & vbCrLf
            cmdText2 += "                          ,s_pay_strike_cut_monthly AS s_monthly_cut" & vbCrLf
            cmdText2 += "                          ,s_pay_strike_cut_once AS s_bonus_cut" & vbCrLf
            cmdText2 += "                          ,(s_pay_time_cut_monthly_break + s_pay_strike_cut_monthly_break) AS monthly_trunc" & vbCrLf
            cmdText2 += "                          ,(s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_trunc" & vbCrLf
            cmdText2 += "                          ,(s_cut_monthly_taxation + s_cut_once_taxation) AS withholding" & vbCrLf
            cmdText2 += "                          ,taxation_total.c_user_id AS c_user_id" & vbCrLf
            cmdText2 += "                          ,taxation_total.c_taxation_flag AS c_taxation_flag" & vbCrLf
            cmdText2 += "                      FROM taxation_total" & vbCrLf
            cmdText2 += "                     WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                       AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                       AND c_pay_once_name = :c_pay_once_name" & vbCrLf
            cmdText2 += "                       AND c_user_id IN (" & vbCrLf
            cmdText2 += "                           SELECT c_user_id" & vbCrLf
            cmdText2 += "                             FROM taxation_total" & vbCrLf
            cmdText2 += "                            WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
            'cmdText2 += "                              AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
            cmdText2 += "                            GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                           HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText2 += "                                  OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText2 += "                                  OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText2 += "                           )" & vbCrLf
            cmdText2 += "                   ) taxable" & vbCrLf
            cmdText2 += "                   LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "                       SELECT A1.*" & vbCrLf
            cmdText2 += "                         FROM staf_attribute A1" & vbCrLf
            cmdText2 += "                             ,(" & vbCrLf
            cmdText2 += "                               SELECT c_user_id" & vbCrLf
            cmdText2 += "                                     ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText2 += "                                 FROM staf_attribute" & vbCrLf
            cmdText2 += "                                WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                                  AND c_ksh <= :c_ksh" & vbCrLf
            cmdText2 += "                                GROUP BY c_user_id" & vbCrLf
            cmdText2 += "                              ) B1" & vbCrLf
            cmdText2 += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText2 += "                          AND A1.d_from = B1.d_from" & vbCrLf
            cmdText2 += "                   ) member" & vbCrLf
            cmdText2 += "                   ON taxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText2 += "           ) taxable_persons" & vbCrLf
            cmdText2 += "           LEFT OUTER JOIN qualification_view" & vbCrLf
            cmdText2 += "           ON taxable_persons.k_qualification = qualification_view.c_constant_seq" & vbCrLf
            cmdText2 += "       ) LEFT OUTER JOIN (" & vbCrLf
            cmdText2 += "           SELECT A3.*" & vbCrLf
            cmdText2 += "             FROM belonging_view A3" & vbCrLf
            cmdText2 += "                 ,(" & vbCrLf
            cmdText2 += "                   SELECT c_constant_seq" & vbCrLf
            cmdText2 += "                         ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
            cmdText2 += "                     FROM belonging_view" & vbCrLf
            cmdText2 += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText2 += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText2 += "                  ) B3" & vbCrLf
            cmdText2 += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText2 += "              AND A3.d_from = B3.d_from" & vbCrLf
            cmdText2 += "       ) u_branch" & vbCrLf
            cmdText2 += "       ON taxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText2 += " ORDER BY RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf
            'todo:

            Try
                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                If OnceName = "" Then
                    command.SetSql(cmdText1)
                Else
                    command.SetSql(cmdText2)
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If
                Me.AddCutDivParameterValue(command)
                WithHoldingDao._logger.Debug(cmdText1)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　課税非対象者の照会ボタン押下
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="UnionBranch"></param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税非対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpNonTaxableDetailListData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal UnionBranch As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpNonTaxableDetailListMap
            Dim cmdText As String = ""

            cmdText += "SELECT 0" & vbCrLf                              ' 01. チェックボックス
            cmdText += "      ,taxable_sumup.c_staf_id" & vbCrLf        ' 02. 社員番号
            cmdText += "      ,taxable_sumup.name" & vbCrLf             ' 03. 名前
            cmdText += "      ,license.l_omission_name" & vbCrLf        ' 04. 資格
            cmdText += "      ,taxable_sumup.monthly_cover" & vbCrLf    ' 05. 月例控除補填額
            cmdText += "      ,taxable_sumup.bonus_cover" & vbCrLf      ' 06. 一時金控除補填額
            cmdText += "      ,taxable_sumup.total_pay" & vbCrLf        ' 07. 支給総額
            cmdText += "      ,taxable_sumup.c_user_id" & vbCrLf        ' 08. ユーザID
            cmdText += "  FROM (" & vbCrLf
            cmdText += "        SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText += "              ,member.l_name AS name" & vbCrLf
            cmdText += "              ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText += "              ,taxable.monthly_cover AS monthly_cover" & vbCrLf
            cmdText += "              ,taxable.bonus_cover AS bonus_cover" & vbCrLf
            cmdText += "              ,(taxable.monthly_cover" & vbCrLf
            cmdText += "              + taxable.bonus_cover) AS total_pay" & vbCrLf
            cmdText += "              ,taxable.c_user_id" & vbCrLf
            cmdText += "          FROM (" & vbCrLf
            cmdText += "                SELECT c_user_id" & vbCrLf
            cmdText += "                      ,(SUM(s_pay_time_cut_monthly)" & vbCrLf
            cmdText += "                      - SUM(s_pay_time_cut_monthly_break))" & vbCrLf
            cmdText += "                      + (SUM(s_pay_strike_cut_monthly)" & vbCrLf
            cmdText += "                      - SUM(s_pay_strike_cut_monthly_break)) AS monthly_cover" & vbCrLf
            cmdText += "                      ,(SUM(s_pay_time_cut_once)" & vbCrLf
            cmdText += "                      + SUM(s_pay_strike_cut_once))" & vbCrLf
            cmdText += "                      - (SUM(s_pay_time_cut_once_break)" & vbCrLf
            cmdText += "                      + SUM(s_pay_strike_cut_once_break)) AS bonus_cover" & vbCrLf
            cmdText += "                  FROM taxation_total" & vbCrLf
            cmdText += "                 WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
            cmdText += "                   AND NOT (s_officer_pay <> 0" & vbCrLf
            cmdText += "                       OR s_cut_monthly_taxation <> 0" & vbCrLf
            cmdText += "                       OR s_cut_once_taxation <> 0)" & vbCrLf
            cmdText += "                 GROUP BY c_user_id" & vbCrLf
            cmdText += "               ) taxable" & vbCrLf
            cmdText += "               LEFT OUTER JOIN (" & vbCrLf
            cmdText += "                   SELECT A1.*" & vbCrLf
            cmdText += "                     FROM staf_attribute A1" & vbCrLf
            cmdText += "                         ,(" & vbCrLf
            cmdText += "                           SELECT c_user_id" & vbCrLf
            cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText += "                             FROM staf_attribute" & vbCrLf
            cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
            cmdText += "                            GROUP BY c_user_id" & vbCrLf
            cmdText += "                          ) B1" & vbCrLf
            cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
            cmdText += "               ) member" & vbCrLf
            cmdText += "               ON member.c_user_id = taxable.c_user_id" & vbCrLf
            cmdText += "         WHERE member.k_belonging = :k_belonging" & vbCrLf
            cmdText += "       ) taxable_sumup" & vbCrLf
            cmdText += "       LEFT OUTER JOIN (" & vbCrLf
            cmdText += "           SELECT A4.*" & vbCrLf
            cmdText += "             FROM qualification_view A4" & vbCrLf
            cmdText += "                 ,(" & vbCrLf
            cmdText += "                   SELECT c_constant_seq" & vbCrLf
            cmdText += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            cmdText += "                     FROM qualification_view" & vbCrLf
            cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText += "                  ) B4" & vbCrLf
            cmdText += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            cmdText += "              AND A4.d_from = B4.d_from" & vbCrLf
            cmdText += "       ) license" & vbCrLf
            cmdText += "       ON taxable_sumup.k_qualification = license.c_constant_seq" & vbCrLf
            cmdText += " ORDER BY RIGHT('0000000000' + taxable_sumup.c_staf_id, 10)" & vbCrLf
            'todo:
            Try
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("k_belonging").Value = UnionBranch
                WithHoldingDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　課税非対象者の一覧プレ印刷ボタン押下
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税非対象者（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpNonTaxableListReportData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpNonTaxableListReportMap
            Dim cmdText As String = ""

            ' SQL文作成
            cmdText += "SELECT taxable_persons.c_staf_id" & vbCrLf      ' 01. 社員番号
            cmdText += "      ,taxable_persons.name" & vbCrLf           ' 02. 氏名
            cmdText += "      ,u_branch.l_name" & vbCrLf                ' 03. 支部
            cmdText += "      ,license.l_omission_name" & vbCrLf        ' 04. 資格
            cmdText += "      ,taxable_persons.monthly_cover" & vbCrLf  ' 05. 月例補填
            cmdText += "      ,taxable_persons.bonus_cover" & vbCrLf    ' 06. 一時金補填
            cmdText += "  FROM (" & vbCrLf
            cmdText += "           (" & vbCrLf
            cmdText += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
            cmdText += "                  ,member.l_name AS name" & vbCrLf
            cmdText += "                  ,(nontaxable.monthly_cut" & vbCrLf
            cmdText += "                  - nontaxable.monthly_trunc) AS monthly_cover" & vbCrLf
            cmdText += "                  ,(nontaxable.bonus_cut" & vbCrLf
            cmdText += "                  - nontaxable.bonus_trunc) AS bonus_cover" & vbCrLf
            cmdText += "                  ,member.k_belonging AS k_belonging" & vbCrLf
            cmdText += "                  ,member.k_qualification AS k_qualification" & vbCrLf
            cmdText += "              FROM (" & vbCrLf
            cmdText += "                    SELECT c_user_id" & vbCrLf
            cmdText += "                          ,(SUM(s_pay_time_cut_monthly)" & vbCrLf
            cmdText += "                          + SUM(s_pay_strike_cut_monthly)) AS monthly_cut" & vbCrLf
            cmdText += "                          ,(SUM(s_pay_time_cut_monthly_break)" & vbCrLf
            cmdText += "                          + SUM(s_pay_strike_cut_monthly_break)) AS monthly_trunc" & vbCrLf
            cmdText += "                          ,(SUM(s_pay_time_cut_once)" & vbCrLf
            cmdText += "                          + SUM(s_pay_strike_cut_once)) AS bonus_cut" & vbCrLf
            cmdText += "                          ,(SUM(s_pay_time_cut_once_break)" & vbCrLf
            cmdText += "                          + SUM(s_pay_strike_cut_once_break)) AS bonus_trunc" & vbCrLf
            cmdText += "                      FROM taxation_total" & vbCrLf
            cmdText += "                     WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
            cmdText += "                       AND c_user_id NOT IN (" & vbCrLf
            cmdText += "                           SELECT c_user_id" & vbCrLf
            cmdText += "                             FROM taxation_total" & vbCrLf
            cmdText += "                            WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
            cmdText += "                            GROUP BY c_user_id" & vbCrLf
            cmdText += "                                    ,d_years" & vbCrLf
            cmdText += "                           HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
            cmdText += "                                  OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
            cmdText += "                                  OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
            cmdText += "                           )" & vbCrLf
            cmdText += "                     GROUP BY c_user_id" & vbCrLf
            cmdText += "                   ) nontaxable" & vbCrLf
            cmdText += "                   LEFT OUTER JOIN (" & vbCrLf
            cmdText += "                       SELECT A1.*" & vbCrLf
            cmdText += "                         FROM staf_attribute A1" & vbCrLf
            cmdText += "                             ,(" & vbCrLf
            cmdText += "                               SELECT c_user_id" & vbCrLf
            cmdText += "                                     ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
            cmdText += "                                 FROM staf_attribute" & vbCrLf
            cmdText += "                                WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                                  AND c_ksh <= :c_ksh" & vbCrLf
            cmdText += "                                GROUP BY c_user_id" & vbCrLf
            cmdText += "                              ) B1" & vbCrLf
            cmdText += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
            cmdText += "                          AND A1.d_from = B1.d_from" & vbCrLf
            cmdText += "                   ) member" & vbCrLf
            cmdText += "                   ON nontaxable.c_user_id = member.c_user_id" & vbCrLf
            cmdText += "           ) taxable_persons" & vbCrLf
            cmdText += "           LEFT OUTER JOIN (" & vbCrLf
            cmdText += "               SELECT A3.*" & vbCrLf
            cmdText += "                 FROM belonging_view A3" & vbCrLf
            cmdText += "                     ,(" & vbCrLf
            cmdText += "                       SELECT c_constant_seq" & vbCrLf
            cmdText += "                             ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
            cmdText += "                         FROM belonging_view" & vbCrLf
            cmdText += "                        WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                        GROUP BY c_constant_seq" & vbCrLf
            cmdText += "                      ) B3" & vbCrLf
            cmdText += "                WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
            cmdText += "                  AND A3.d_from = B3.d_from" & vbCrLf
            cmdText += "           ) u_branch" & vbCrLf
            cmdText += "           ON taxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
            cmdText += "       ) LEFT OUTER JOIN (" & vbCrLf
            cmdText += "           SELECT A4.*" & vbCrLf
            cmdText += "             FROM qualification_view A4" & vbCrLf
            cmdText += "                 ,(" & vbCrLf
            cmdText += "                   SELECT c_constant_seq" & vbCrLf
            cmdText += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
            cmdText += "                     FROM qualification_view" & vbCrLf
            cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
            cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
            cmdText += "                  ) B4" & vbCrLf
            cmdText += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
            cmdText += "              AND A4.d_from = B4.d_from" & vbCrLf
            cmdText += "       ) license" & vbCrLf
            cmdText += "       ON taxable_persons.k_qualification = license.c_constant_seq" & vbCrLf
            cmdText += " ORDER BY taxable_persons.k_belonging" & vbCrLf
            cmdText += "         ,RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf

            Try
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                WithHoldingDao._logger.Debug(cmdText)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収 - 課税対象者累計画面の明細印刷ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="SelectedMembers">対象社員番号リスト</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpReportDetailData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal SelectedMembers As ArrayList, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim dao As New ConstantTblDao
            Dim ds As DataSet = dao.GetConstantKind("FIX_ADDRESS_INFO", CriterionDate)
            Dim rowArray As DataRow() = ds.Tables.Item("constant_dtl").Select("c_constant_seq = '03'")
            Dim addr1 As String = rowArray(0).Item("l_omission_name").ToString
            rowArray = ds.Tables.Item("constant_dtl").Select("c_constant_seq = '04'")
            Dim addr2 As String = rowArray(0).Item("l_omission_name").ToString
            Dim table2 As DataTable
            Dim map As New WithholdingSumUpReportDetailMap
            Dim builder As New StringBuilder("")
            Dim i As Integer
            Dim format As String = ""

            Try
                ' SQL文作成
                format = ""
                format += "SELECT taxable_sumup.year" & vbCrLf                                                              ' 01. 対象年
                format += "      ,taxable_sumup.c_staf_id" & vbCrLf                                                         ' 02. 社員番号
                format += "      ,taxable_sumup.name" & vbCrLf                                                              ' 03. 名前
                format += "      ,model.l_omission_name" & vbCrLf                                                           ' 04. 機種
                format += "      ,license.l_omission_name" & vbCrLf                                                         ' 05. 資格
                format += "      ,u_branch.l_omission_name" & vbCrLf                                                        ' 06. 支部
                format += "      ,IIF(ISNULL(address.l_add_number), '', address.l_add_number) AS l_add_number" & vbCrLf     ' 07. 郵便番号
                format += "      ,IIF(ISNULL(address.l_prefectures), '', address.l_prefectures) AS l_prefectures" & vbCrLf  ' 08. 都道府県
                format += "      ,IIF(ISNULL(address.l_cities), '', address.l_cities) AS l_cities" & vbCrLf                 ' 09. 市区町村
                format += "      ,IIF(ISNULL(address.l_add_ather), '', address.l_add_ather) AS l_add_ather" & vbCrLf        ' 10. 番地等
                format += "      ,IIF(ISNULL(address.l_building), '', address.l_building) AS l_building" & vbCrLf           ' 11. 建物名等
                format += "      ,taxable_sumup.total_pay" & vbCrLf                                                         ' 12. 給与の合計
                format += "      ,taxable_sumup.withholding" & vbCrLf                                                       ' 13. 源泉徴収額
                format += "      ,'" & addr1 & "' AS addname1" & vbCrLf                                                     ' 14. 組合住所1
                format += "      ,'" & addr2 & "' AS addname2" & vbCrLf                                                     ' 15. 組合住所2
                format += "  FROM (" & vbCrLf
                format += "           (" & vbCrLf
                format += "               (" & vbCrLf
                format += "                   (" & vbCrLf
                format += "                       (" & vbCrLf
                format += "                        SELECT taxable.year AS [year]" & vbCrLf
                format += "                              ,member.c_staf_id AS c_staf_id" & vbCrLf
                format += "                              ,member.l_name AS name" & vbCrLf
                format += "                              ,member.k_model" & vbCrLf
                format += "                              ,member.k_qualification AS k_qualification" & vbCrLf
                format += "                              ,member.k_belonging" & vbCrLf
                format += "                              ,(taxable.s_officer_pay"
                format += "                              + taxable.monthly_cover"
                format += "                              + taxable.bonus_cover) AS total_pay" & vbCrLf
                format += "                              ,taxable.withholding AS withholding" & vbCrLf
                format += "                              ,taxable.c_user_id" & vbCrLf
                format += "                          FROM (" & vbCrLf
                format += "                                SELECT c_user_id" & vbCrLf
                format += "                                      ,FORMAT(d_years, 'yyyy') AS [year]" & vbCrLf
                format += "                                      ,SUM(taxation_total.s_officer_pay) AS s_officer_pay" & vbCrLf
                format += "                                      ,(SUM(s_pay_time_cut_monthly)"
                format += "                                      - SUM(s_pay_time_cut_monthly_break))"
                format += "                                      + (SUM(s_pay_strike_cut_monthly)"
                format += "                                      - SUM(s_pay_strike_cut_monthly_break)) AS monthly_cover" & vbCrLf
                format += "                                      ,(SUM(s_pay_time_cut_once)"
                format += "                                      + SUM(s_pay_strike_cut_once))"
                format += "                                      - (SUM(s_pay_time_cut_once_break)"
                format += "                                      + SUM(s_pay_strike_cut_once_break)) AS bonus_cover" & vbCrLf
                format += "                                      ,(SUM(s_cut_monthly_taxation)"
                format += "                                      + SUM(s_cut_once_taxation)) AS withholding" & vbCrLf
                format += "                                  FROM taxation_total" & vbCrLf
                format += "                                 WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
                format += "                                   AND (s_officer_pay <> 0" & vbCrLf
                format += "                                       OR s_cut_monthly_taxation <> 0" & vbCrLf
                format += "                                       OR s_cut_once_taxation <> 0)" & vbCrLf
                format += "                                   AND c_user_id IN ({0})" & vbCrLf
                format += "                                 GROUP BY c_user_id" & vbCrLf
                format += "                                         ,FORMAT(d_years, 'yyyy')" & vbCrLf
                format += "                               ) taxable" & vbCrLf
                format += "                               LEFT OUTER JOIN (" & vbCrLf
                format += "                                   SELECT A1.*" & vbCrLf
                format += "                                     FROM staf_attribute A1" & vbCrLf
                format += "                                         ,(" & vbCrLf
                format += "                                           SELECT c_user_id" & vbCrLf
                format += "                                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                format += "                                             FROM staf_attribute" & vbCrLf
                format += "                                            WHERE d_from <= :d_from" & vbCrLf
                format += "                                              AND c_ksh <= :c_ksh" & vbCrLf
                format += "                                            GROUP BY c_user_id" & vbCrLf
                format += "                                          ) B1" & vbCrLf
                format += "                                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                format += "                                      AND A1.d_from = B1.d_from" & vbCrLf
                format += "                               ) member" & vbCrLf
                format += "                               ON member.c_user_id = taxable.c_user_id" & vbCrLf
                format += "                       ) taxable_sumup" & vbCrLf
                format += "                       LEFT OUTER JOIN (" & vbCrLf
                format += "                           SELECT A5.*" & vbCrLf
                format += "                             FROM model_view A5" & vbCrLf
                format += "                                 ,(" & vbCrLf
                format += "                                   SELECT c_constant_seq" & vbCrLf
                format += "                                         ,MAX(model_view.d_from) AS d_from" & vbCrLf
                format += "                                     FROM model_view" & vbCrLf
                format += "                                    WHERE d_from <= :d_from" & vbCrLf
                format += "                                    GROUP BY c_constant_seq" & vbCrLf
                format += "                                  ) B5" & vbCrLf
                format += "                            WHERE A5.c_constant_seq = B5.c_constant_seq" & vbCrLf
                format += "                              AND A5.d_from = B5.d_from" & vbCrLf
                format += "                       ) model" & vbCrLf
                format += "                       ON taxable_sumup.k_model = model.c_constant_seq" & vbCrLf
                format += "                   ) LEFT OUTER JOIN (" & vbCrLf
                format += "                       SELECT A4.*" & vbCrLf
                format += "                         FROM qualification_view A4" & vbCrLf
                format += "                             ,(SELECT c_constant_seq" & vbCrLf
                format += "                                     ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
                format += "                                 FROM qualification_view" & vbCrLf
                format += "                                WHERE d_from <= :d_from" & vbCrLf
                format += "                                GROUP BY c_constant_seq" & vbCrLf
                format += "                              ) B4" & vbCrLf
                format += "                        WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
                format += "                          AND A4.d_from = B4.d_from" & vbCrLf
                format += "                   ) license" & vbCrLf
                format += "                   ON taxable_sumup.k_qualification = license.c_constant_seq" & vbCrLf
                format += "               ) LEFT OUTER JOIN (" & vbCrLf
                format += "                   SELECT A3.*" & vbCrLf
                format += "                     FROM belonging_view A3" & vbCrLf
                format += "                         ,(" & vbCrLf
                format += "                           SELECT c_constant_seq" & vbCrLf
                format += "                                 ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
                format += "                             FROM belonging_view" & vbCrLf
                format += "                            WHERE d_from <= :d_from" & vbCrLf
                format += "                            GROUP BY c_constant_seq" & vbCrLf
                format += "                          ) B3" & vbCrLf
                format += "                    WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                format += "                      AND A3.d_from = B3.d_from" & vbCrLf
                format += "               ) u_branch" & vbCrLf
                format += "               ON taxable_sumup.k_belonging = u_branch.c_constant_seq" & vbCrLf
                format += "           ) LEFT OUTER JOIN (" & vbCrLf
                format += "               SELECT A6.*" & vbCrLf
                format += "                 FROM staf_address A6" & vbCrLf
                format += "                     ,(" & vbCrLf
                format += "                       SELECT c_user_id" & vbCrLf
                format += "                             ,MAX(staf_address.d_from) AS d_from" & vbCrLf
                format += "                         FROM staf_address" & vbCrLf
                format += "                        WHERE d_from <= :d_from" & vbCrLf
                format += "                          AND k_main_add = 'True'" & vbCrLf
                format += "                        GROUP BY c_user_id" & vbCrLf
                format += "                      ) B6" & vbCrLf
                format += "                WHERE A6.c_user_id = B6.c_user_id" & vbCrLf
                format += "                  AND A6.d_from = B6.d_from" & vbCrLf
                format += "                  AND k_main_add = 'True'" & vbCrLf
                format += "           ) address" & vbCrLf
                format += "           ON taxable_sumup.c_user_id = address.c_user_id" & vbCrLf
                format += "       )" & vbCrLf
                format += " ORDER BY RIGHT('0000000000' + taxable_sumup.c_staf_id, 10)" & vbCrLf

                For i = 0 To SelectedMembers.Count - 1
                    If (builder.Length > 0) Then
                        builder.Append((",'" & CStr(SelectedMembers.Item(i)) & "'"))
                    Else
                        builder.Append(("'" & CStr(SelectedMembers.Item(i)) & "'"))
                    End If
                Next i
                Dim cmdText As String = String.Format(format, builder.ToString)
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtHeader", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　課税対象者の照会ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="UnionBranch">所属支部</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpTaxableDetailListData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal UnionBranch As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpTaxableDetailListMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT 0" & vbCrLf                                                      ' 01. チェックボックス
                cmdText += "      ,taxable_sumup.c_staf_id" & vbCrLf                                ' 02. 社員番号
                cmdText += "      ,taxable_sumup.name" & vbCrLf                                     ' 03. 名前
                cmdText += "      ,license.l_omission_name" & vbCrLf                                ' 04. 資格
                cmdText += "      ,taxable_sumup.remuneration" & vbCrLf                             ' 05. 役員手当
                cmdText += "      ,taxable_sumup.monthly_cover" & vbCrLf                            ' 06. 月例控除補填
                cmdText += "      ,taxable_sumup.bonus_cover" & vbCrLf                              ' 07. 一時金控除補填
                cmdText += "      ,taxable_sumup.total_pay" & vbCrLf                                ' 08. 総支給額
                cmdText += "      ,taxable_sumup.withholding" & vbCrLf                              ' 09. 源泉徴収額
                cmdText += "      ,(taxable_sumup.total_pay"
                cmdText += "      - taxable_sumup.withholding)" & vbCrLf                            ' 10. 差引支給額
                cmdText += "      ,taxable_sumup.c_user_id" & vbCrLf                                ' 11. ユーザID
                cmdText += "      ,taxable_sumup.monthly_withholding" & vbCrLf                      ' 12. 源泉徴収額(月例)
                cmdText += "      ,taxable_sumup.once_withholding" & vbCrLf                         ' 13. 源泉徴収額(一時金)
                cmdText += "  FROM (" & vbCrLf
                cmdText += "        SELECT member.c_staf_id AS c_staf_id" & vbCrLf
                cmdText += "              ,member.l_name AS name" & vbCrLf
                cmdText += "              ,member.k_qualification AS k_qualification" & vbCrLf
                cmdText += "              ,taxable.s_officer_pay AS remuneration" & vbCrLf
                cmdText += "              ,taxable.monthly_cover AS monthly_cover" & vbCrLf
                cmdText += "              ,taxable.bonus_cover AS bonus_cover" & vbCrLf
                cmdText += "              ,(taxable.s_officer_pay"
                cmdText += "              + taxable.monthly_cover"
                cmdText += "              + taxable.bonus_cover) AS total_pay" & vbCrLf
                cmdText += "              ,(taxable.monthly_withholding"
                cmdText += "              + taxable.once_withholding) AS withholding" & vbCrLf
                cmdText += "              ,taxable.c_user_id" & vbCrLf
                cmdText += "              ,taxable.monthly_withholding AS monthly_withholding" & vbCrLf
                cmdText += "              ,taxable.once_withholding AS once_withholding" & vbCrLf
                cmdText += "          FROM (" & vbCrLf
                cmdText += "                SELECT c_user_id" & vbCrLf
                cmdText += "                      ,SUM(taxation_total.s_officer_pay) AS s_officer_pay" & vbCrLf
                cmdText += "                      ,(SUM(s_pay_time_cut_monthly)" & vbCrLf
                cmdText += "                      - SUM(s_pay_time_cut_monthly_break))"
                cmdText += "                      + (SUM(s_pay_strike_cut_monthly)"
                cmdText += "                      - SUM(s_pay_strike_cut_monthly_break)) AS monthly_cover" & vbCrLf
                cmdText += "                      ,(SUM(s_pay_time_cut_once)"
                cmdText += "                      + SUM(s_pay_strike_cut_once))"
                cmdText += "                      - (SUM(s_pay_time_cut_once_break)"
                cmdText += "                      + SUM(s_pay_strike_cut_once_break)) AS bonus_cover" & vbCrLf
                cmdText += "                      ,SUM(s_cut_monthly_taxation) AS monthly_withholding" & vbCrLf
                cmdText += "                      ,SUM(s_cut_once_taxation) AS once_withholding" & vbCrLf
                cmdText += "                  FROM taxation_total" & vbCrLf
                cmdText += "                 WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
                cmdText += "                   AND (s_officer_pay <> 0" & vbCrLf
                cmdText += "                       OR s_cut_monthly_taxation <> 0" & vbCrLf
                cmdText += "                       OR s_cut_once_taxation <> 0)" & vbCrLf
                cmdText += "                 GROUP BY c_user_id" & vbCrLf
                cmdText += "               ) taxable" & vbCrLf
                cmdText += "               LEFT OUTER JOIN (" & vbCrLf
                cmdText += "                   SELECT A1.*" & vbCrLf
                cmdText += "                     FROM staf_attribute A1" & vbCrLf
                cmdText += "                         ,(" & vbCrLf
                cmdText += "                           SELECT c_user_id" & vbCrLf
                cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText += "                             FROM staf_attribute" & vbCrLf
                cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
                cmdText += "                            GROUP BY c_user_id" & vbCrLf
                cmdText += "                          ) B1" & vbCrLf
                cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
                cmdText += "               ) member" & vbCrLf
                cmdText += "               ON member.c_user_id = taxable.c_user_id" & vbCrLf
                cmdText += "         WHERE member.k_belonging = :k_belonging" & vbCrLf
                cmdText += "       ) taxable_sumup" & vbCrLf
                cmdText += "       LEFT OUTER JOIN (" & vbCrLf
                cmdText += "           SELECT A4.*" & vbCrLf
                cmdText += "             FROM qualification_view A4" & vbCrLf
                cmdText += "                 ,(" & vbCrLf
                cmdText += "                   SELECT c_constant_seq" & vbCrLf
                cmdText += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
                cmdText += "                     FROM qualification_view" & vbCrLf
                cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                  ) B4" & vbCrLf
                cmdText += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
                cmdText += "              AND A4.d_from = B4.d_from" & vbCrLf
                cmdText += "       ) license" & vbCrLf
                cmdText += "       ON taxable_sumup.k_qualification = license.c_constant_seq" & vbCrLf
                cmdText += " ORDER BY RIGHT('0000000000' + taxable_sumup.c_staf_id, 10)" & vbCrLf

                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("k_belonging").Value = UnionBranch

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　課税対象者の一覧プレ印刷ボタン押下時処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpTaxableListReportData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpTaxableListReportMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT taxable_persons.c_staf_id" & vbCrLf      ' 01. 社員番号
                cmdText += "      ,taxable_persons.name" & vbCrLf           ' 02. 氏名
                cmdText += "      ,u_branch.l_name" & vbCrLf                ' 03. 支部
                cmdText += "      ,license.l_omission_name" & vbCrLf        ' 04. 資格
                cmdText += "      ,taxable_persons.remuneration" & vbCrLf   ' 05. 役員手当
                cmdText += "      ,taxable_persons.monthly_cover" & vbCrLf  ' 06. 月例補填
                cmdText += "      ,taxable_persons.bonus_cover" & vbCrLf    ' 07. 一時金補填
                cmdText += "      ,taxable_persons.withholding" & vbCrLf    ' 08. 源泉徴収額
                cmdText += "  FROM (" & vbCrLf
                cmdText += "           (" & vbCrLf
                cmdText += "            SELECT member.c_staf_id AS c_staf_id" & vbCrLf
                cmdText += "                  ,member.l_name AS name" & vbCrLf
                cmdText += "                  ,taxable.remuneration AS remuneration" & vbCrLf
                cmdText += "                  ,(taxable.monthly_cut"
                cmdText += "                  - taxable.monthly_trunc) AS monthly_cover" & vbCrLf
                cmdText += "                  ,(taxable.bonus_cut"
                cmdText += "                  - taxable.bonus_trunc) AS bonus_cover" & vbCrLf
                cmdText += "                  ,taxable.withholding AS withholding" & vbCrLf
                cmdText += "                  ,taxable.c_user_id AS c_user_id" & vbCrLf
                cmdText += "                  ,member.k_belonging AS k_belonging" & vbCrLf
                cmdText += "                  ,member.k_qualification AS k_qualification" & vbCrLf
                cmdText += "              FROM (" & vbCrLf
                cmdText += "                    SELECT c_user_id" & vbCrLf
                cmdText += "                          ,SUM(s_officer_pay) AS remuneration" & vbCrLf
                cmdText += "                          ,(SUM(s_pay_time_cut_monthly)"
                cmdText += "                          + SUM(s_pay_strike_cut_monthly)) AS monthly_cut" & vbCrLf
                cmdText += "                          ,(SUM(s_pay_time_cut_monthly_break)"
                cmdText += "                          + SUM(s_pay_strike_cut_monthly_break)) AS monthly_trunc" & vbCrLf
                cmdText += "                          ,(SUM(s_pay_time_cut_once)"
                cmdText += "                          + SUM(s_pay_strike_cut_once)) AS bonus_cut" & vbCrLf
                cmdText += "                          ,(SUM(s_pay_time_cut_once_break)"
                cmdText += "                          + SUM(s_pay_strike_cut_once_break)) AS bonus_trunc" & vbCrLf
                cmdText += "                          ,(SUM(s_cut_monthly_taxation)"
                cmdText += "                          + SUM(s_cut_once_taxation)) AS withholding" & vbCrLf
                cmdText += "                      FROM taxation_total" & vbCrLf
                cmdText += "                     WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
                cmdText += "                       AND c_user_id IN (" & vbCrLf
                cmdText += "                           SELECT c_user_id" & vbCrLf
                cmdText += "                             FROM taxation_total" & vbCrLf
                cmdText += "                            WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
                cmdText += "                            GROUP BY c_user_id" & vbCrLf
                cmdText += "                            HAVING SUM(s_officer_pay) <> 0" & vbCrLf
                cmdText += "                                OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
                cmdText += "                                OR SUM(s_cut_once_taxation) <> 0" & vbCrLf
                cmdText += "                           )" & vbCrLf
                cmdText += "                     GROUP BY c_user_id" & vbCrLf
                cmdText += "                   ) taxable" & vbCrLf
                cmdText += "                   LEFT OUTER JOIN (" & vbCrLf
                cmdText += "                       SELECT A1.*" & vbCrLf
                cmdText += "                         FROM staf_attribute A1" & vbCrLf
                cmdText += "                             ,(" & vbCrLf
                cmdText += "                              SELECT c_user_id" & vbCrLf
                cmdText += "                                    ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText += "                                FROM staf_attribute" & vbCrLf
                cmdText += "                               WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                                 AND c_ksh <= :c_ksh" & vbCrLf
                cmdText += "                               GROUP BY c_user_id" & vbCrLf
                cmdText += "                              ) B1" & vbCrLf
                cmdText += "                        WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText += "                          AND A1.d_from = B1.d_from" & vbCrLf
                cmdText += "                   ) member" & vbCrLf
                cmdText += "                   ON taxable.c_user_id = member.c_user_id" & vbCrLf
                cmdText += "           ) taxable_persons" & vbCrLf
                cmdText += "           LEFT OUTER JOIN (" & vbCrLf
                cmdText += "               SELECT A3.*" & vbCrLf
                cmdText += "                 FROM belonging_view A3" & vbCrLf
                cmdText += "                     ,(" & vbCrLf
                cmdText += "                       SELECT c_constant_seq" & vbCrLf
                cmdText += "                             ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
                cmdText += "                         FROM belonging_view" & vbCrLf
                cmdText += "                        WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                        GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                      ) B3" & vbCrLf
                cmdText += "                WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                cmdText += "                  AND A3.d_from = B3.d_from" & vbCrLf
                cmdText += "           ) u_branch" & vbCrLf
                cmdText += "           ON taxable_persons.k_belonging = u_branch.c_constant_seq" & vbCrLf
                cmdText += "       ) LEFT OUTER JOIN (" & vbCrLf
                cmdText += "           SELECT A4.*" & vbCrLf
                cmdText += "             FROM qualification_view A4" & vbCrLf
                cmdText += "                 ,(" & vbCrLf
                cmdText += "                   SELECT c_constant_seq" & vbCrLf
                cmdText += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
                cmdText += "                     FROM qualification_view" & vbCrLf
                cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                  ) B4" & vbCrLf
                cmdText += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
                cmdText += "              AND A4.d_from = B4.d_from" & vbCrLf
                cmdText += "       ) license" & vbCrLf
                cmdText += "       ON taxable_persons.k_qualification = license.c_constant_seq" & vbCrLf
                cmdText += " ORDER BY taxable_persons.k_belonging" & vbCrLf
                cmdText += "         ,RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf

                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　累計タブ　検索処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYear">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks></remarks>
        Public Function GetSumUpTaxableSumData( _
            ByVal CompanyCode As String, _
            ByVal TargetYear As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingSumUpTaxableSumListMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT u_branch.l_name AS [" & map.GetLogicalName(0) & "]" & vbCrLf                 ' 01. 支部
                cmdText += "      ,taxable_sumup.remuneration AS [" & map.GetLogicalName(1) & "]" & vbCrLf      ' 02. 役員手当額
                cmdText += "      ,taxable_sumup.monthly_cover AS [" & map.GetLogicalName(2) & "]" & vbCrLf     ' 03. 月例賃金控除補填額
                cmdText += "      ,taxable_sumup.bonus_cover AS [" & map.GetLogicalName(3) & "]" & vbCrLf       ' 04. 一時金控除補填額
                cmdText += "      ,taxable_sumup.total_pay AS [" & map.GetLogicalName(4) & "]" & vbCrLf         ' 05. 支給総額
                cmdText += "      ,taxable_sumup.withholding AS [" & map.GetLogicalName(5) & "]" & vbCrLf       ' 06. 源泉徴収額
                cmdText += "      ,(taxable_sumup.total_pay"
                cmdText += "      - taxable_sumup.withholding) AS [" & map.GetLogicalName(6) & "]" & vbCrLf     ' 07. 差引支給額
                cmdText += "  FROM (" & vbCrLf
                cmdText += "        SELECT member.k_belonging AS k_belonging" & vbCrLf
                cmdText += "              ,SUM(taxable.s_officer_pay) AS remuneration" & vbCrLf
                cmdText += "              ,SUM(taxable.monthly_cover) AS monthly_cover" & vbCrLf
                cmdText += "              ,SUM(taxable.bonus_cover) AS bonus_cover" & vbCrLf
                cmdText += "              ,(SUM(taxable.s_officer_pay)"
                cmdText += "              + SUM(taxable.monthly_cover)"
                cmdText += "              + SUM(taxable.bonus_cover)) AS total_pay" & vbCrLf
                cmdText += "              ,SUM(taxable.withholding) AS withholding" & vbCrLf
                cmdText += "          FROM (" & vbCrLf
                cmdText += "                SELECT c_user_id" & vbCrLf
                cmdText += "                      ,s_officer_pay" & vbCrLf
                cmdText += "                      ,((s_pay_time_cut_monthly"
                cmdText += "                       - s_pay_time_cut_monthly_break)"
                cmdText += "                       + (s_pay_strike_cut_monthly"
                cmdText += "                       - s_pay_strike_cut_monthly_break)) AS monthly_cover" & vbCrLf
                cmdText += "                      ,((s_pay_time_cut_once"
                cmdText += "                       + s_pay_strike_cut_once)"
                cmdText += "                       - (s_pay_time_cut_once_break"
                cmdText += "                       + s_pay_strike_cut_once_break)) AS bonus_cover" & vbCrLf
                cmdText += "                      ,(s_cut_monthly_taxation"
                cmdText += "                      + s_cut_once_taxation) AS withholding" & vbCrLf
                cmdText += "                  FROM taxation_total" & vbCrLf
                cmdText += "                 WHERE FORMAT(d_years, 'yyyy') = :d_years" & vbCrLf
                cmdText += "                   AND (s_officer_pay <> 0" & vbCrLf
                cmdText += "                       OR s_cut_monthly_taxation <> 0" & vbCrLf
                cmdText += "                       OR s_cut_once_taxation <> 0)" & vbCrLf
                cmdText += "               ) taxable" & vbCrLf
                cmdText += "               LEFT OUTER JOIN (" & vbCrLf
                cmdText += "                   SELECT A1.*" & vbCrLf
                cmdText += "                     FROM staf_attribute A1" & vbCrLf
                cmdText += "                         ,(" & vbCrLf
                cmdText += "                           SELECT c_user_id" & vbCrLf
                cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText += "                             FROM staf_attribute" & vbCrLf
                cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
                cmdText += "                            GROUP BY c_user_id" & vbCrLf
                cmdText += "                          ) B1" & vbCrLf
                cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
                cmdText += "               ) member" & vbCrLf
                cmdText += "               ON member.c_user_id = taxable.c_user_id" & vbCrLf
                cmdText += "         GROUP BY member.k_belonging" & vbCrLf
                cmdText += "       ) taxable_sumup" & vbCrLf
                cmdText += "       LEFT OUTER JOIN (" & vbCrLf
                cmdText += "           SELECT A3.*" & vbCrLf
                cmdText += "             FROM belonging_view A3" & vbCrLf
                cmdText += "                 ,(" & vbCrLf
                cmdText += "                   SELECT c_constant_seq" & vbCrLf
                cmdText += "                         ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
                cmdText += "                     FROM belonging_view" & vbCrLf
                cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                  ) B3" & vbCrLf
                cmdText += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                cmdText += "              AND A3.d_from = B3.d_from" & vbCrLf
                cmdText += "       ) u_branch" & vbCrLf
                cmdText += "       ON taxable_sumup.k_belonging = u_branch.c_constant_seq" & vbCrLf
                cmdText += " ORDER BY taxable_sumup.k_belonging" & vbCrLf
                'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", taxable_sumup.remuneration AS ", map.GetLogicalName(1), ", taxable_sumup.monthly_cover AS ", map.GetLogicalName(2), ", taxable_sumup.bonus_cover AS ", map.GetLogicalName(3), ", taxable_sumup.total_pay AS ", map.GetLogicalName(4), ", taxable_sumup.withholding AS ", map.GetLogicalName(5), ", (taxable_sumup.total_pay - taxable_sumup.withholding) AS ", map.GetLogicalName(6), " FROM ( SELECT member.k_belonging AS k_belonging, SUM(taxable.s_officer_pay) AS remuneration, SUM(taxable.monthly_cover) AS monthly_cover, SUM(taxable.bonus_cover) AS bonus_cover, SUM(taxable.s_officer_pay) + SUM(taxable.monthly_cover) + SUM(taxable.bonus_cover) AS total_pay, SUM(taxable.withholding) AS withholding FROM ( SELECT c_user_id, s_officer_pay, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover, (s_cut_monthly_taxation + s_cut_once_taxation) AS withholding FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND " & _
                '    "(s_officer_pay <> 0 OR s_cut_monthly_taxation <> 0 OR s_cut_once_taxation <> 0)) taxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = taxable.c_user_id GROUP BY member.k_belonging ) taxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(belonging_view.d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON taxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY taxable_sumup.k_belonging"})
                'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS """, map.GetLogicalName(0), """, taxable_sumup.remuneration AS """, map.GetLogicalName(1), """, taxable_sumup.monthly_cover AS """, map.GetLogicalName(2), """, taxable_sumup.bonus_cover AS """, map.GetLogicalName(3), """, taxable_sumup.total_pay AS """, map.GetLogicalName(4), """, taxable_sumup.withholding AS """, map.GetLogicalName(5), """, (taxable_sumup.total_pay - taxable_sumup.withholding) AS """, map.GetLogicalName(6), """ FROM ( SELECT member.k_belonging AS k_belonging, SUM(taxable.s_officer_pay) AS remuneration, SUM(taxable.monthly_cover) AS monthly_cover, SUM(taxable.bonus_cover) AS bonus_cover, SUM(taxable.s_officer_pay) + SUM(taxable.monthly_cover) + SUM(taxable.bonus_cover) AS total_pay, SUM(taxable.withholding) AS withholding FROM ( SELECT c_user_id, s_officer_pay, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover, (s_cut_monthly_taxation + s_cut_once_taxation) AS withholding FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND  ( (c_user_id, d_years) IN (select " & _
                '    "c_user_id, d_years from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0)))) taxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = taxable.c_user_id GROUP BY member.k_belonging ) taxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON taxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY taxable_sumup.k_belonging"})
                'Dim cmdText As String = String.Concat(New String() {"SELECT u_branch.l_name AS ", map.GetLogicalName(0), ", taxable_sumup.remuneration AS ", map.GetLogicalName(1), ", taxable_sumup.monthly_cover AS ", map.GetLogicalName(2), ", taxable_sumup.bonus_cover AS ", map.GetLogicalName(3), ", taxable_sumup.total_pay AS ", map.GetLogicalName(4), ", taxable_sumup.withholding AS ", map.GetLogicalName(5), ", (taxable_sumup.total_pay - taxable_sumup.withholding) AS ", map.GetLogicalName(6), " FROM ( SELECT member.k_belonging AS k_belonging, SUM(taxable.s_officer_pay) AS remuneration, SUM(taxable.monthly_cover) AS monthly_cover, SUM(taxable.bonus_cover) AS bonus_cover, SUM(taxable.s_officer_pay) + SUM(taxable.monthly_cover) + SUM(taxable.bonus_cover) AS total_pay, SUM(taxable.withholding) AS withholding FROM ( SELECT c_user_id, s_officer_pay, (s_pay_time_cut_monthly - s_pay_time_cut_monthly_break) + (s_pay_strike_cut_monthly - s_pay_strike_cut_monthly_break) AS monthly_cover, (s_pay_time_cut_once + s_pay_strike_cut_once) - (s_pay_time_cut_once_break + s_pay_strike_cut_once_break) AS bonus_cover, (s_cut_monthly_taxation + s_cut_once_taxation) AS withholding FROM taxation_total WHERE TO_CHAR(d_years, 'yyyy') = :d_years AND  ( (c_user_id) IN (select " & _
                '    "c_user_id from taxation_total where TO_CHAR(d_years, 'yyyy') = :d_years group by c_user_id, d_years having (SUM(s_officer_pay) <> 0 OR SUM(s_cut_monthly_taxation) <> 0 OR SUM(s_cut_once_taxation) <> 0)))) taxable LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON member.c_user_id = taxable.c_user_id GROUP BY member.k_belonging ) taxable_sumup LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(belonging_view.d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON taxable_sumup.k_belonging = u_branch.c_constant_seq ORDER BY taxable_sumup.k_belonging"})

                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYear
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("dtDetail_taxable", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収 - 課税対象者月例賃金
        ''' 源泉徴収 - 課税対象者一時金
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="UnionBranch">所属支部</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks>
        ''' MOD 2012/06/24
        ''' MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' MOD 2016/12/07 月例賃金 or 一時金（一時金名称有無等）のSQL文作成修正
        ''' </remarks>
        Public Function GetTaxableData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal UnionBranch As String, _
            ByVal CriterionDate As String, _
            Optional ByVal OnceName As String = "" _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyTaxableDetailListMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT CONVERT( bit, 'false' )" & vbCrLf                                                                  ' 01. チェックボックス
                cmdText += "      ,taxable_persons.c_staf_id" & vbCrLf                                              ' 02. 社員番号
                cmdText += "      ,taxable_persons.name" & vbCrLf                                                   ' 03. 氏名
                cmdText += "      ,license.l_omission_name" & vbCrLf                                                ' 04. 資格
                cmdText += "      ,taxable_persons.remuneration" & vbCrLf                                           ' 05. 役員手当
                cmdText += "      ,taxable_persons.monthly_cut" & vbCrLf                                            ' 06. 月例控除
                cmdText += "      ,taxable_persons.bonus_cut" & vbCrLf                                              ' 07. 一時金控除
                cmdText += "      ,taxable_persons.taxable" & vbCrLf                                                ' 08. 課税対象額
                cmdText += "      ,taxable_persons.[truncate]" & vbCrLf                                               ' 09. 切捨て額
                cmdText += "      ,taxable_persons.withholding" & vbCrLf                                            ' 10. 源泉徴収額
                cmdText += "      ,taxable_persons.withholding_monthly" & vbCrLf                                    ' 11. 源泉徴収額(月例)
                cmdText += "      ,taxable_persons.withholding_bonus" & vbCrLf                                      ' 12. 源泉徴収(一時金)

                '-----------------------------------------------------------
                '   13. 差引支給額
                '-----------------------------------------------------------
                ' 日当計算区分判定
                If Me._strCut.Equals("05") Then
                    ' 源泉徴収 - 課税対象者月例賃金
                    cmdText += "      ,(taxable_persons.remuneration"
                    cmdText += "      + taxable_persons.monthly_cut"
                    cmdText += "      - taxable_persons.[truncate]"
                    cmdText += "      - taxable_persons.withholding) AS [" & map.GetLogicalName(10) & "]" & vbCrLf
                Else
                    ' 源泉徴収 - 課税対象者一時金
                    cmdText += "      ,taxable_persons.taxable"
                    cmdText += "     - taxable_persons.[truncate]"
                    cmdText += "     - taxable_persons.withholding AS [" & map.GetLogicalName(10) & "]" & vbCrLf
                End If

                cmdText += "      ,taxable_persons.c_user_id AS [" & map.GetLogicalName(13) & "]" & vbCrLf          ' 14. ユーザID
                cmdText += "      ,taxable_persons.c_taxation_flag" & vbCrLf                                        ' 15. 課税フラグ
                cmdText += "  FROM (" & vbCrLf
                cmdText += "        SELECT member.c_staf_id AS c_staf_id" & vbCrLf
                cmdText += "              ,member.l_name AS name" & vbCrLf
                cmdText += "              ,taxable.remuneration AS remuneration" & vbCrLf
                cmdText += "              ,taxable.monthly_cut AS monthly_cut" & vbCrLf
                cmdText += "              ,taxable.bonus_cut AS bonus_cut" & vbCrLf
                cmdText += "              ,IIF(taxable.c_taxation_flag = '0'"
                cmdText += "                  ,taxable.remuneration"
                cmdText += "                  ,(taxable.remuneration"
                cmdText += "                  + taxable.monthly_cut"
                cmdText += "                  + taxable.bonus_cut)) as taxable" & vbCrLf
                cmdText += "              ,taxable.[truncate] AS [truncate]" & vbCrLf
                cmdText += "              ,taxable.withholding AS withholding" & vbCrLf
                cmdText += "              ,taxable.withholding_monthly AS withholding_monthly" & vbCrLf
                cmdText += "              ,taxable.withholding_bonus AS withholding_bonus" & vbCrLf
                cmdText += "              ,taxable.c_user_id AS c_user_id" & vbCrLf
                cmdText += "              ,taxable.c_taxation_flag AS c_taxation_flag" & vbCrLf
                cmdText += "              ,member.k_belonging AS k_belonging" & vbCrLf
                cmdText += "              ,member.k_qualification AS k_qualification" & vbCrLf
                cmdText += "          FROM (" & vbCrLf
                cmdText += "                SELECT s_officer_pay AS remuneration" & vbCrLf
                cmdText += "                      ,(s_pay_time_cut_monthly"
                cmdText += "                      + s_pay_strike_cut_monthly) AS monthly_cut" & vbCrLf
                cmdText += "                      ,(s_pay_time_cut_once"
                cmdText += "                      + s_pay_strike_cut_once) AS bonus_cut" & vbCrLf
                cmdText += "                      ,(s_pay_time_cut_monthly_break"
                cmdText += "                      + s_pay_strike_cut_monthly_break"
                cmdText += "                      + s_pay_time_cut_once_break"
                cmdText += "                      + s_pay_strike_cut_once_break) AS [truncate]" & vbCrLf
                cmdText += "                      ,(s_cut_monthly_taxation"
                cmdText += "                      + s_cut_once_taxation) AS withholding" & vbCrLf
                cmdText += "                      ,s_cut_monthly_taxation AS withholding_monthly" & vbCrLf
                cmdText += "                      ,s_cut_once_taxation AS withholding_bonus" & vbCrLf
                cmdText += "                      ,taxation_total.c_user_id AS c_user_id" & vbCrLf
                cmdText += "                      ,taxation_total.c_taxation_flag AS c_taxation_flag" & vbCrLf
                cmdText += "                  FROM taxation_total" & vbCrLf
        cmdText += "                 WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
        ' 2026/06/18 MOD START なぜコメントにしたのか不明？？　これをコメントにするとユーザが寿福表示されてしまう
        cmdText += "                   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf

        ' 条件に一時金名称があれば、追加
        If OnceName.Length > 0 Then
                    cmdText += "                   AND c_pay_once_name = :c_pay_once_name" & vbCrLf
                End If

                cmdText += "                   AND (c_user_id IN (" & vbCrLf
                cmdText += "                        SELECT c_user_id" & vbCrLf
                cmdText += "                          FROM taxation_total" & vbCrLf
                cmdText += "                         WHERE FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf
        ' 2026/06/18 MOD START なぜコメントにしたのか不明？？　これをコメントにするとユーザが寿福表示されてしまう
        cmdText += "                           AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
        cmdText += "                         GROUP BY c_user_id" & vbCrLf
        cmdText += "                        HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
                cmdText += "                               OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
                cmdText += "                               OR SUM(s_cut_once_taxation) <> 0)" & vbCrLf
                cmdText += "                       ))" & vbCrLf
                cmdText += "               ) taxable" & vbCrLf
                cmdText += "               LEFT OUTER JOIN (" & vbCrLf
                cmdText += "                   SELECT A1.*" & vbCrLf
                cmdText += "                     FROM staf_attribute A1" & vbCrLf
                cmdText += "                         ,(" & vbCrLf
                cmdText += "                           SELECT c_user_id" & vbCrLf
                cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText += "                             FROM staf_attribute" & vbCrLf
                cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
                cmdText += "                            GROUP BY c_user_id" & vbCrLf
                cmdText += "                          ) B1" & vbCrLf
                cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
                cmdText += "               ) member" & vbCrLf
                cmdText += "               ON taxable.c_user_id = member.c_user_id" & vbCrLf
                cmdText += "         WHERE member.k_belonging = :k_belonging" & vbCrLf
                cmdText += "       ) taxable_persons" & vbCrLf
                cmdText += "       LEFT OUTER JOIN (" & vbCrLf
                cmdText += "           SELECT A4.*" & vbCrLf
                cmdText += "             FROM qualification_view A4" & vbCrLf
                cmdText += "                 ,(" & vbCrLf
                cmdText += "                   SELECT c_constant_seq" & vbCrLf
                cmdText += "                         ,MAX(qualification_view.d_from) AS d_from" & vbCrLf
                cmdText += "                     FROM qualification_view" & vbCrLf
                cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                  ) B4" & vbCrLf
                cmdText += "            WHERE A4.c_constant_seq = B4.c_constant_seq" & vbCrLf
                cmdText += "              AND A4.d_from = B4.d_from" & vbCrLf
                cmdText += "       ) license" & vbCrLf
                cmdText += "       ON taxable_persons.k_qualification = license.c_constant_seq" & vbCrLf
                cmdText += " ORDER BY RIGHT('0000000000' + taxable_persons.c_staf_id, 10)" & vbCrLf

                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("k_belonging").Value = UnionBranch

                ' SQL設定
                command.SetSql(cmdText)

                ' 条件に一時金名称がある場合、追加
                If OnceName <> "" Then
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If

                Me.AddCutDivParameterValue(command)

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("withholding_taxable_list", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収　月例賃金集計タブ　検索処理
        ''' 源泉徴収　一時金集計タブ　検索処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="CriterionDate">集計年月月末日</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns>課税対象者一覧（データテーブル）</returns>
        ''' <remarks>
        '''  MOD 2016/07/28 カラム追加（課税フラグ）
        '''  MOD 2012/06/15
        '''  MOD 2012/11/13 AND k_daily_pay_kind = :k_daily_pay_kind 追加
        ''' </remarks>
        Public Function GetTaxableSummary( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal OnceName As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New WithholdingMonthlyTaxableSumListMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT u_branch.l_name AS " & map.GetLogicalName(0) & vbCrLf        ' 01. 支部
                cmdText += "      ,subtotal.s_officer_pay AS " & map.GetLogicalName(1) & vbCrLf ' 02. 役員手当額
                cmdText += "      ,subtotal.s_cut_monthly AS " & map.GetLogicalName(2) & vbCrLf ' 03. 月例賃金控除額
                cmdText += "      ,subtotal.s_cut_once AS " & map.GetLogicalName(3) & vbCrLf    ' 04. 一時金控除額
                cmdText += "      ,subtotal.tax AS " & map.GetLogicalName(4) & vbCrLf           ' 05. 課税対象額
                cmdText += "      ,subtotal.s_break AS " & map.GetLogicalName(5) & vbCrLf       ' 06. 切捨て額
                cmdText += "      ,subtotal.s_taxation AS " & map.GetLogicalName(6) & vbCrLf    ' 07. 源泉徴収額
                cmdText += "      ,(subtotal.s_officer_pay"
                cmdText += "     + subtotal.s_cut_monthly"
                cmdText += "     + subtotal.s_cut_once"
                cmdText += "     - subtotal.s_break"
                cmdText += "     - subtotal.s_taxation) AS " & map.GetLogicalName(7) & vbCrLf   ' 08. 差引支給額
                cmdText += "  FROM (" & vbCrLf
                cmdText += "        SELECT member.k_belonging" & vbCrLf
                cmdText += "              ,SUM(taxable.s_officer_pay) AS s_officer_pay" & vbCrLf
                cmdText += "              ,(SUM(taxable.s_pay_time_cut_monthly)"
                cmdText += "              + SUM(taxable.s_pay_strike_cut_monthly)) AS s_cut_monthly" & vbCrLf
                cmdText += "              ,(SUM(taxable.s_pay_time_cut_monthly_break)"
                cmdText += "              + SUM(taxable.s_pay_strike_cut_monthly_break)"
                cmdText += "              + SUM(taxable.s_pay_time_cut_once_break)"
                cmdText += "              + SUM(taxable.s_pay_strike_cut_once_break)) AS s_break" & vbCrLf
                cmdText += "              ,SUM(taxable.s_cut_monthly_taxation) AS s_monthly_taxation" & vbCrLf
                cmdText += "              ,(SUM(taxable.s_pay_time_cut_once)"
                cmdText += "              + SUM(taxable.s_pay_strike_cut_once)) AS s_cut_once" & vbCrLf
                cmdText += "              ,(SUM(taxable.s_cut_monthly_taxation)"
                cmdText += "              + SUM(taxable.s_cut_once_taxation)) AS s_taxation" & vbCrLf
                cmdText += "              ,SUM(taxable.tax) AS tax" & vbCrLf
                cmdText += "          FROM (" & vbCrLf
                cmdText += "                SELECT *" & vbCrLf
                cmdText += "                      ,IIF(taxation_total.c_taxation_flag = '0'" & vbCrLf
                cmdText += "                          ,taxation_total.s_officer_pay" & vbCrLf
                cmdText += "                          ,(taxation_total.s_officer_pay"
                cmdText += "                          + taxation_total.s_pay_time_cut_monthly"
                cmdText += "                          + taxation_total.s_pay_strike_cut_monthly"
                cmdText += "                          + taxation_total.s_pay_time_cut_once"
                cmdText += "                          + taxation_total.s_pay_strike_cut_once)) AS tax" & vbCrLf
                cmdText += "                  FROM taxation_total" & vbCrLf
                cmdText += "                 WHERE TO_CHAR(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText += "                   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf

                ' 条件に一時金名称があれば、追加
                If OnceName.Length > 0 Then
                    cmdText += "                   AND c_pay_once_name = :c_pay_once_name" & vbCrLf
                End If

                cmdText += "                   AND (c_user_id IN (" & vbCrLf
                cmdText += "                        SELECT c_user_id" & vbCrLf
                cmdText += "                          FROM taxation_total" & vbCrLf
                cmdText += "                         WHERE TO_CHAR(d_years, 'yyyyMM') = :d_years" & vbCrLf
                'cmdText += "                           AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                cmdText += "                         GROUP BY c_user_id" & vbCrLf
                cmdText += "                        HAVING (SUM(s_officer_pay) <> 0" & vbCrLf
                cmdText += "                               OR SUM(s_cut_monthly_taxation) <> 0" & vbCrLf
                cmdText += "                               OR SUM(s_cut_once_taxation) <> 0" & vbCrLf
                cmdText += "                       )" & vbCrLf
                cmdText += "               ))) taxable" & vbCrLf
                cmdText += "               LEFT OUTER JOIN(" & vbCrLf
                cmdText += "                   SELECT A1.*" & vbCrLf
                cmdText += "                     FROM staf_attribute A1" & vbCrLf
                cmdText += "                         ,(" & vbCrLf
                cmdText += "                           SELECT c_user_id" & vbCrLf
                cmdText += "                                 ,MAX(staf_attribute.d_from) AS d_from" & vbCrLf
                cmdText += "                             FROM staf_attribute" & vbCrLf
                cmdText += "                            WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                              AND c_ksh <= :c_ksh" & vbCrLf
                cmdText += "                            GROUP BY c_user_id" & vbCrLf
                cmdText += "                          ) B1" & vbCrLf
                cmdText += "                    WHERE A1.c_user_id = B1.c_user_id" & vbCrLf
                cmdText += "                      AND A1.d_from = B1.d_from" & vbCrLf
                cmdText += "               ) member" & vbCrLf
                cmdText += "               ON taxable.c_user_id = member.c_user_id" & vbCrLf
                cmdText += "         GROUP BY member.k_belonging" & vbCrLf
                cmdText += "       ) subtotal" & vbCrLf
                cmdText += "       LEFT OUTER JOIN (" & vbCrLf
                cmdText += "           SELECT A3.*" & vbCrLf
                cmdText += "             FROM belonging_view A3" & vbCrLf
                cmdText += "                 ,(" & vbCrLf
                cmdText += "                   SELECT c_constant_seq" & vbCrLf
                cmdText += "                         ,MAX(belonging_view.d_from) AS d_from" & vbCrLf
                cmdText += "                     FROM belonging_view" & vbCrLf
                cmdText += "                    WHERE d_from <= :d_from" & vbCrLf
                cmdText += "                    GROUP BY c_constant_seq" & vbCrLf
                cmdText += "                  ) B3" & vbCrLf
                cmdText += "            WHERE A3.c_constant_seq = B3.c_constant_seq" & vbCrLf
                cmdText += "              AND A3.d_from = B3.d_from" & vbCrLf
                cmdText += "       ) u_branch" & vbCrLf
                cmdText += "       ON subtotal.k_belonging = u_branch.c_constant_seq" & vbCrLf
                cmdText += " ORDER BY subtotal.k_belonging " & vbCrLf

                Dim command As New NpgsqlCommand("", MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                If OnceName.Length > 0 Then
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Item("c_pay_once_name").Value = OnceName
                End If
                command.SetSql(cmdText)
                Me.AddCutDivParameterValue(command)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.DataReader2LogicalDataTable("withholding_taxable_summary", map, dReader)
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収額取得処理
        ''' </summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="Taxable">課税対象額</param>
        ''' <param name="CriterionDate">対象年月月末日</param>
        ''' <returns>源泉徴収額一覧（データテーブル）</returns>
        ''' <remarks>課税率マスタ・課税率詳細から源泉徴収額情報を取得</remarks>
        Public Function GetTaxMaster( _
            ByVal CompanyCode As String, _
            ByVal Taxable As Long, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim table2 As DataTable
            Dim map As New TaxRateMap
            Dim cmdText As String = ""

            Try
                ' SQL文作成
                cmdText = ""
                cmdText += "SELECT tax_rate_dtl.s_lower AS [" & map.GetLogicalName(0) & "]" & vbCrLf        ' 01. 下限金額
                cmdText += "      ,tax_rate_dtl.s_upper AS [" & map.GetLogicalName(1) & "]" & vbCrLf        ' 02. 上限金額
                cmdText += "      ,tax_rate_dtl.s_taxation AS [" & map.GetLogicalName(2) & "]" & vbCrLf     ' 03. 源泉徴収額
                cmdText += "      ,tax_rate_dtl.s_tax_rate_dtl AS [" & map.GetLogicalName(3) & "]" & vbCrLf ' 04. 課税率
                cmdText += "  FROM (" & vbCrLf
                '                   課税率マスタから対象年月と会社コードを条件に適用開始日を取得
                cmdText += "        SELECT c_ksh" & vbCrLf
                cmdText += "              ,MAX(tax_rate.d_from) AS d_from" & vbCrLf
                cmdText += "          FROM tax_rate" & vbCrLf
                cmdText += "         WHERE c_ksh = :c_ksh" & vbCrLf
                cmdText += "           AND d_from <= :d_from" & vbCrLf
                cmdText += "         GROUP BY c_ksh" & vbCrLf
                cmdText += "       ) tax_rate_view" & vbCrLf
                cmdText += "      ,tax_rate_dtl" & vbCrLf
                cmdText += " WHERE tax_rate_dtl.c_ksh = tax_rate_view.c_ksh" & vbCrLf
                cmdText += "   AND tax_rate_dtl.d_from = tax_rate_view.d_from" & vbCrLf
                cmdText += "   AND tax_rate_dtl.s_lower <= :taxable" & vbCrLf
                cmdText += "   AND tax_rate_dtl.s_upper > :taxable" & vbCrLf
                'Dim cmdText As String = String.Concat(New String() {"SELECT tax_rate_dtl.s_lower AS """, map.GetLogicalName(0), """, tax_rate_dtl.s_upper AS """, map.GetLogicalName(1), """, tax_rate_dtl.s_taxation AS """, map.GetLogicalName(2), """, tax_rate_dtl.s_tax_rate_dtl AS """, map.GetLogicalName(3), """ FROM ( SELECT c_ksh, MAX(tax_rate.d_from) AS d_from FROM tax_rate WHERE c_ksh = :c_ksh AND d_from <= :d_from GROUP BY c_ksh ) tax_rate_view, tax_rate_dtl WHERE tax_rate_dtl.c_ksh = tax_rate_view.c_ksh AND tax_rate_dtl.d_from = tax_rate_view.d_from AND tax_rate_dtl.s_lower <= :taxable AND tax_rate_dtl.s_upper > :taxable"})

                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("taxable", DbType.Int32))
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("taxable").Value = Taxable

                ' ログ出力
                WithHoldingDao._logger.Debug(cmdText)

                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.DataReader2LogicalDataTable("withholding_taxable_list", map, dReader)
                If (table.Rows.Count = 0) Then
                    Throw New DataNotFoundException
                End If
                If (table.Rows.Count > 1) Then
                    Throw New TooManyRowsException
                End If
                table2 = table
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As TooManyRowsException
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ''' <summary>
        ''' 源泉徴収対象データ有無判定処理
        ''' </summary>
        ''' <param name="TargetYM">集計年月</param>
        ''' <returns>True：対象データ有, False：対象データ無</returns>
        ''' <remarks></remarks>
        Public Function IsExists( _
            ByVal TargetYM As String _
        ) As Boolean

            Dim flag As Boolean
            Dim message As String = ""

            Try
                ' SQL文作成
                message = ""
                message += "SELECT COUNT(*)" & vbCrLf
                message += "  FROM taxation_total" & vbCrLf
                message += " WHERE FORMAT(d_years, 'yyyyMM') = '" & TargetYM & "'" & vbCrLf
                'message += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                'Dim message As String = ("SELECT COUNT(*) FROM taxation_total WHERE TO_CHAR(d_years, 'yyyyMM') = '" & TargetYM & "' AND k_daily_pay_kind = :k_daily_pay_kind ")

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(message)

                Dim command As New NpgsqlCommand(message, MyBase.GetNpgsqlConnection)
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                Me.AddCutDivParameterValue(command)
                Dim obj2 As Object = command.ExecuteScalar
                If (((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull) OrElse (CLng(obj2) = 0)) Then
                    Return False
                End If
                flag = True
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        ''' <summary>
        ''' 対象年月の源泉徴収データ有無判定処理
        ''' </summary>
        ''' <param name="TargetYM">集計年月</param>
        ''' <returns>True：対象データ有, False：対象データ無</returns>
        ''' <remarks>源泉徴収　再集計ボタン押下・押下不可時に使用</remarks>
        Public Function IsGreaterThanExists( _
            ByVal TargetYM As String _
        ) As Boolean

            Dim flag As Boolean
            Dim message As String = ""

            Try
                ' SQL文作成
                message = ""
                message += "SELECT COUNT(*)" & vbCrLf
                message += "  FROM taxation_total" & vbCrLf
                message += " WHERE FORMAT(d_years, 'yyyyMM') > '" & TargetYM & "'" & vbCrLf
                'message += "   AND k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf
                'Dim message As String = ("SELECT COUNT(*) FROM taxation_total WHERE TO_CHAR(d_years, 'yyyyMM') > '" & TargetYM & "' AND k_daily_pay_kind = :k_daily_pay_kind ")

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(message)

                Dim command As New NpgsqlCommand(message, MyBase.GetNpgsqlConnection)
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                Me.AddCutDivParameterValue(command)
                Dim obj2 As Object = command.ExecuteScalar
                If (((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull) OrElse (CLng(obj2) = 0)) Then
                    Return False
                End If
                flag = True
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        ''' <summary>
        ''' 源泉徴収 - 課税対象者月例賃金画面　内容変更後の登録・更新処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="TargetYM">集計年月</param>
        ''' <param name="TargetUserId">個人認証ID</param>
        ''' <param name="Remuneration">役員手当</param>
        ''' <param name="Withholding">課税対象額（月例）</param>
        ''' <param name="userId">更新者個人ＩＤ</param>
        ''' <remarks></remarks>
        Public Sub Update( _
            ByVal command As NpgsqlCommand, _
            ByVal TargetYM As String, _
            ByVal TargetUserId As String, _
            ByVal Remuneration As Long, _
            ByVal Withholding As Long, _
            ByVal userId As String _
        )

            Dim message As String = ""
            Dim num As Integer = 0

            Try
                ' 源泉徴収更新SQL文作成
                If Remuneration > 0 Then
                    ' 役員手当あり
                    message += ""
                    message += "UPDATE taxation_total" & vbCrLf                                     ' 源泉徴収テーブル
                    message += "   SET s_officer_pay = :s_officer_pay" & vbCrLf                     ' 役員手当
                    message += "      ,s_cut_monthly_taxation = :s_cut_monthly_taxation" & vbCrLf   ' 課税対象額(月例)
                    message += "      ,d_up = GETDATE()" & vbCrLf                                       ' 更新日
                    message += "      ,c_user_id_up = :c_user_id_up" & vbCrLf                       ' 更新者個人ＩＤ
                    message += "      ,s_up = s_up + 1" & vbCrLf                                    ' 更新回数
                    message += "      ,c_taxation_flag = :c_taxation_flag" & vbCrLf                 ' 課税フラグ
                    message += " WHERE c_user_id = :c_user_id" & vbCrLf                             ' 個人認証IDと同じもの
                    message += "   AND FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf               ' 集計年月と同じもの
                    'message += "   AND k_daily_pay_kind = :k_daily_pay_kind " & vbCrLf              ' 日当計算区分と同じもの
                Else
                    ' 役員手当なし
                    message = ""
                    message += "UPDATE taxation_total" & vbCrLf                                     ' 源泉徴収テーブル
                    message += "   SET s_officer_pay = :s_officer_pay" & vbCrLf                     ' 役員手当
                    message += "      ,s_cut_monthly_taxation = :s_cut_monthly_taxation" & vbCrLf   ' 課税対象額(月例)
                    message += "      ,s_cut_once_taxation = :s_cut_monthly_taxation" & vbCrLf      ' 課税対象額（一時金）
                    message += "      ,d_up = GETDATE()" & vbCrLf                                       ' 更新日
                    message += "      ,c_user_id_up = :c_user_id_up" & vbCrLf                       ' 更新者個人ＩＤ
                    message += "      ,s_up = s_up + 1" & vbCrLf                                    ' 更新回数
                    message += "      ,c_taxation_flag = :c_taxation_flag" & vbCrLf                 ' 課税フラグ
                    message += " WHERE c_user_id = :c_user_id" & vbCrLf                             ' 個人認証IDと同じもの
                    message += "   AND FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf               ' 集計年月と同じもの
                    'message += "   AND k_daily_pay_kind = :k_daily_pay_kind " & vbCrLf              ' 日当計算区分と同じもの
                End If
                'Dim message As String = If((Remuneration > 0), "UPDATE taxation_total SET s_officer_pay = :s_officer_pay, s_cut_monthly_taxation = :s_cut_monthly_taxation,d_up = GETDATE(),c_user_id_up = :c_user_id_up,s_up = s_up + 1,c_taxation_flag = :c_taxation_flag WHERE c_user_id = :c_user_id AND TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind ", "UPDATE taxation_total SET s_officer_pay = :s_officer_pay, s_cut_monthly_taxation = :s_cut_monthly_taxation,s_cut_once_taxation = :s_cut_monthly_taxation,d_up = GETDATE(),c_user_id_up = :c_user_id_up,s_up = s_up + 1,c_taxation_flag = :c_taxation_flag WHERE c_user_id = :c_user_id AND TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind ")
                'Dim message As String = If((Remuneration > 0), "UPDATE taxation_total SET s_officer_pay = :s_officer_pay, s_cut_monthly_taxation = :s_cut_monthly_taxation,d_up = GETDATE(),c_user_id_up = :c_user_id_up,s_up = s_up + 1 WHERE c_user_id = :c_user_id AND TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind ", "UPDATE taxation_total SET s_officer_pay = :s_officer_pay, s_cut_monthly_taxation = :s_cut_monthly_taxation,s_cut_once_taxation = :s_cut_monthly_taxation,d_up = GETDATE(),c_user_id_up = :c_user_id_up,s_up = s_up + 1 WHERE c_user_id = :c_user_id AND TO_CHAR(d_years, 'yyyyMM') = :d_years AND k_daily_pay_kind = :k_daily_pay_kind ")

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(message)

                ' SQL文設定
                command.SetSql(message)

                ' パラメータクリア
                command.Parameters.Clear()

                ' パラメータ設定
                command.Parameters.Add(New NpgsqlParameter("s_officer_pay", DbType.Int32))          ' 役員手当
                command.Parameters.Add(New NpgsqlParameter("s_cut_monthly_taxation", DbType.Int32)) ' 課税対象額（月例）
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))          ' 更新者個人ＩＤ
                command.Parameters.Add(New NpgsqlParameter("c_taxation_flag", DbType.String))       ' 課税フラグ
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))             ' 個人認証ID
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))               ' 集計年月
                'command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))      ' 日当計算区分

                ' パラメータ値設定
                command.Parameters.Item("s_officer_pay").Value = Remuneration                       ' 役員手当
                command.Parameters.Item("s_cut_monthly_taxation").Value = Withholding               ' 課税対象額（月例）
                command.Parameters.Item("c_user_id_up").Value = userId                              ' 更新者個人ＩＤ
                command.Parameters.Item("c_taxation_flag").Value = "1"                              ' 課税フラグ
                command.Parameters.Item("c_user_id").Value = TargetUserId                           ' 個人認証ID
                command.Parameters.Item("d_years").Value = TargetYM                                 ' 集計年月

                ' パラメータ値設定（日当計算区分）
                Me.AddCutDivParameterValue(command)

                ' SQL実行
                num = command.ExecuteNonQuery
                If (num = 0) Then
                    Throw New DataNotFoundException
                End If
                If (num > 1) Then
                    Throw New TooManyRowsException
                End If
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As TooManyRowsException
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        ''' <summary>
        ''' 非課税対象者から課税対象者への源泉徴収更新処理
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="iYears">集計年月</param>
        ''' <param name="iUpdateUuserId">更新者個人ＩＤ</param>
        ''' <param name="iUuserId">個人認証ID</param>
        ''' <param name="iTax">課税対象額（月例）</param>
        ''' <remarks></remarks>
        Public Sub UpdateTaxation( _
            ByVal command As NpgsqlCommand, _
            ByVal iYears As String, _
            ByVal iUpdateUuserId As String, _
            ByVal iUuserId As String, _
            ByVal iTax As Long _
        )

            Dim strSql As String = ""
            Dim num As Integer = 0

            Try
                ' SQL文作成
                strSql = ""
                strSql += "update taxation_total" & vbCrLf                                              ' 源泉徴収テーブル更新
                strSql += "   set s_cut_monthly_taxation = :s_cut_monthly_taxation" & vbCrLf            ' 課税対象額（月例）
                strSql += "      ,d_up = GETDATE()" & vbCrLf                                                ' 更新日
                strSql += "      ,c_user_id_up = :c_user_id_up" & vbCrLf                                ' 更新者個人ID
                strSql += "      ,s_up = s_up + 1" & vbCrLf                                             ' 更新回数
                strSql += "      ,c_taxation_flag = :c_taxation_flag" & vbCrLf                          ' 課税フラグ
                strSql += " where FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf                        ' 集計年月が同じもの
                strSql += "   and k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf                        ' 日当計算区分が同じもの
                strSql += "   and c_user_id = :c_user_id " & vbCrLf                                     ' 個人認証IDが同じもの

                ' ログ出力(SQL)
                WithHoldingDao._logger.Debug(strSql)

                ' SQL文設定
                command.SetSql(strSql)

                ' パラメータ設定
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("s_cut_monthly_taxation", DbType.Int32))     ' 課税対象額（月例）
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))              ' 更新者個人ID
                command.Parameters.Add(New NpgsqlParameter("c_taxation_flag", DbType.String))           ' 課税フラグ
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))                   ' 集計年月
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))          ' 日当計算区分
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))                 ' 個人認証ID

                ' パラメータ値設定
                command.Parameters.Item("s_cut_monthly_taxation").Value = iTax                          ' 課税対象額（月例）
                command.Parameters.Item("c_taxation_flag").Value = "1"                                  ' 課税フラグ
                command.Parameters.Item("c_user_id_up").Value = iUpdateUuserId                          ' 更新者個人ID
                command.Parameters.Item("d_years").Value = iYears                                       ' 集計年月
                command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut                          ' 日当計算区分
                command.Parameters.Item("c_user_id").Value = iUuserId                                   ' 個人認証ID

                ' SQL実行
                num = command.ExecuteNonQuery
                If (num = 0) Then
                    Throw New DataNotFoundException
                End If
                If (num > 1) Then
                    Throw New TooManyRowsException
                End If
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As TooManyRowsException
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        ''' <summary>
        ''' 役員手当のみ課税対象額として源泉徴収更新
        ''' </summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="iYears">集計年月</param>
        ''' <param name="iUpdateUuserId">更新者個人ＩＤ</param>
        ''' <param name="iUuserId">個人認証ID</param>
        ''' <param name="iMonthlyTaxationTax">課税対象額（月例）</param>
        ''' <remarks></remarks>
        Public Sub UpdateTaxationOfficersAllowance( _
            ByVal command As NpgsqlCommand, _
            ByVal iYears As String, _
            ByVal iUpdateUuserId As String, _
            ByVal iUuserId As String, _
            ByVal iMonthlyTaxationTax As Long _
        )

            Dim strSql As String = ""
            Dim num As Integer = 0

            Try
                ' SQL文作成
                strSql = ""
                strSql += "update taxation_total" & vbCrLf                                              ' 源泉徴収テーブル更新
                strSql += "   set s_cut_monthly_taxation = :s_cut_monthly_taxation" & vbCrLf            ' 課税対象額（月例）
                strSql += "      ,d_up = GETDATE()" & vbCrLf                                                ' 更新日
                strSql += "      ,c_user_id_up = :c_user_id_up" & vbCrLf                                ' 更新者個人ID
                strSql += "      ,s_up = s_up + 1" & vbCrLf                                             ' 更新回数
                strSql += "      ,c_taxation_flag = :c_taxation_flag" & vbCrLf                          ' 課税フラグ
                strSql += " where FORMAT(d_years, 'yyyyMM') = :d_years" & vbCrLf                        ' 集計年月が同じもの
                strSql += "   and k_daily_pay_kind = :k_daily_pay_kind" & vbCrLf                        ' 日当計算区分が同じもの
                strSql += "   and c_user_id = :c_user_id " & vbCrLf                                     ' 個人認証IDが同じもの

                WithHoldingDao._logger.Debug(strSql)
                command.SetSql(strSql)

                ' パラメータ設定
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("s_cut_monthly_taxation", DbType.Int32))     ' 課税対象額（月例）
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))              ' 更新者個人ID
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))                   ' 集計年月
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))          ' 日当計算区分
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))                 ' 個人認証ID
                command.Parameters.Add(New NpgsqlParameter("c_taxation_flag", DbType.String))           ' 課税フラグ

                ' パラメータ値設定
                command.Parameters.Item("s_cut_monthly_taxation").Value = iMonthlyTaxationTax           ' 課税対象額（月例）
                command.Parameters.Item("c_user_id_up").Value = iUpdateUuserId                          ' 更新者個人ID
                command.Parameters.Item("d_years").Value = iYears                                       ' 集計年月
                command.Parameters.Item("k_daily_pay_kind").Value = Me._strCut                          ' 日当計算区分
                command.Parameters.Item("c_user_id").Value = iUuserId                                   ' 個人認証ID
                command.Parameters.Item("c_taxation_flag").Value = "0"                                  ' 課税フラグ
                num = command.ExecuteNonQuery
                If (num = 0) Then
                    Throw New DataNotFoundException
                End If
                If (num > 1) Then
                    Throw New TooManyRowsException
                End If
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As TooManyRowsException
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private _strCut As String

    End Class
End Namespace
