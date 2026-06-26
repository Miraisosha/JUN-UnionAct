'===========================================================================================================
'   クラスＩＤ　　：MonthlyBaseDao
'   クラス名称　　：賃金カット（月例）DAOベースクラス
'   備考  　　　　：
'===========================================================================================================

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
    Public MustInherit Class MonthlyBaseDao
        Inherits WageReductionBaseDao

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Protected Const IN_LINE_VIEW_CUT As String = "(SELECT * FROM {0} WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) cut "

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <param name="TableName">テーブル名</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal TableName As String)
            MyBase.New(TableName)
        End Sub
#End Region

#Region " FindSummury： "
        ''' <summary></summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <param name="TruncPlace">切捨て単位</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function FindSummury( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal TruncPlace As Integer, _
            Optional ByVal strBonusName As String = "" _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionMonthlyListMap
            Dim sql As String = ""

            'TODO Dim cmdText As String = String.Concat(New Object() {"SELECT c_branch.l_name AS """, map.GetLogicalName(0), """, COUNT(cut.c_user_id) AS """, map.GetLogicalName(1), """, SUM(cut.s_pay_cut) AS """, map.GetLogicalName(2), """, SUM(cut.cover) AS """, map.GetLogicalName(3), """, SUM(cut.s_pay_cut) - SUM(cut.cover) AS """, map.GetLogicalName(4), """ FROM (SELECT c_user_id, s_pay_cut, TRUNC(s_pay_cut, ", TruncPlace, ") AS cover FROM ", MyBase.TableName, " WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) cut LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON cut.c_user_id = member.c_user_id LEFT OUTER JOIN (SELECT A2.* FROM area_local_view A2, (SELECT c_constant_seq, MAX(d_from) AS d_from FROM area_local_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B2 WHERE A2.c_constant_seq = B2.c_constant_seq AND A2.d_from = B2.d_from ) c_branch ON member.k_local = c_branch.c_constant_seq GROUP BY c_branch.l_name, member.k_local ORDER BY member.k_local"})
            'Dim cmdText As String = String.Concat(New Object() {"SELECT c_branch.l_name AS """, map.GetLogicalName(0), """, COUNT(cut.c_user_id) AS """, map.GetLogicalName(1), """, SUM(cut.s_pay_cut) AS """, map.GetLogicalName(2), """, SUM(cut.cover) AS """, map.GetLogicalName(3), """, SUM(cut.s_pay_cut) - SUM(cut.cover) AS """, map.GetLogicalName(4), """ FROM (((SELECT c_user_id, s_pay_cut," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover FROM ", MyBase.TableName, " WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) cut LEFT OUTER JOIN (SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member ON cut.c_user_id = member.c_user_id) LEFT OUTER JOIN (SELECT A2.* FROM area_local_view A2, (SELECT c_constant_seq, MAX(area_local_view.d_from) AS d_from FROM area_local_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B2 WHERE A2.c_constant_seq = B2.c_constant_seq AND A2.d_from = B2.d_from ) c_branch ON member.k_local = c_branch.c_constant_seq) GROUP BY c_branch.l_name, member.k_local ORDER BY member.k_local"})

            ' SQL
            sql += "SELECT c_branch.l_name      AS '" & map.GetLogicalName(0) & "'"
            sql += "      ,COUNT(cut.c_user_id) AS '" & map.GetLogicalName(1) & "'"
            sql += "      ,SUM(cut.s_pay_cut)   AS '" & map.GetLogicalName(2) & "'"
            sql += "      ,SUM(cut.cover)       AS '" & map.GetLogicalName(3) & "'"
            sql += "      ,SUM(cut.s_pay_cut) - SUM(cut.cover) AS '" & map.GetLogicalName(4) & "'"
            sql += "  FROM ("
            sql += "           ("
            '                      賃金カット情報
            sql += "               ("
            sql += "                SELECT c_user_id"
            sql += "                      ,s_pay_cut"
            sql += "                      ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover"
            sql += "                  FROM " & MyBase.TableName
            sql += "                 WHERE TO_CHAR(d_years,'yyyyMM') = :d_years"
            sql += "               ) cut"
            '                      組合員属性情報
            sql += "               LEFT OUTER JOIN ("
            sql += "                   SELECT A1.*"
            sql += "                     FROM staf_attribute A1"
            sql += "                         ,("
            sql += "                           SELECT c_user_id"
            sql += "                                 ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                             FROM staf_attribute"
            sql += "                            WHERE d_from <= :d_from"
            sql += "                              AND c_ksh  <= :c_ksh"
            sql += "                            GROUP BY c_user_id"
            sql += "                          ) B1"
            sql += "                    WHERE A1.c_user_id = B1.c_user_id"
            sql += "                      AND A1.d_from    = B1.d_from"
            sql += "               ) member"
            sql += "               ON cut.c_user_id = member.c_user_id"
            '                  会社所属情報
            sql += "           ) LEFT OUTER JOIN ("
            sql += "               SELECT A2.*"
            sql += "                 FROM area_local_view A2"
            sql += "                     ,("
            sql += "                       SELECT c_constant_seq"
            sql += "                             ,MAX(area_local_view.d_from) AS d_from"
            sql += "                         FROM area_local_view"
            sql += "                        WHERE d_from <= :d_from"
            sql += "                        GROUP BY c_constant_seq"
            sql += "                      ) B2"
            sql += "                WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                  AND A2.d_from         = B2.d_from"
            sql += "           ) c_branch"
            sql += "           ON member.k_local = c_branch.c_constant_seq"
            sql += "       )"
            sql += " GROUP BY c_branch.l_name"
            sql += "         ,member.k_local"
            sql += " ORDER BY member.k_local"   'ok

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_years").Value = TargetYM
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_from").Value = CriterionDate

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2LogicalDataTable("wage_reduction_monthly_list", map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetPrintDetailData： "
        ''' <summary></summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="TruncPlace">切捨て単位</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPrintDetailData( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal TruncPlace As Integer, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionListMonthlyReportMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT member.c_staf_id AS " & map.GetPhysicalName(0)       ' 01. 社員番号
            sql += "      ,member.l_name    AS " & map.GetPhysicalName(1)       ' 02. 氏名
            sql += "      ,c_branch.l_name  AS " & map.GetPhysicalName(2)       ' 03. 会社所属情
            sql += "      ,cut.i_cut        AS " & map.GetPhysicalName(3)       ' 04. 
            sql += "      ,cut.i_cover      AS " & map.GetPhysicalName(4)       ' 05. 
            sql += "      ,cut.i_dues       AS " & map.GetPhysicalName(5)       ' 06. 
            sql += "      ,cut.s_cut        AS " & map.GetPhysicalName(6)       ' 07. 
            sql += "      ,cut.s_cover      AS " & map.GetPhysicalName(7)       ' 08. 
            sql += "      ,cut.s_dues       AS " & map.GetPhysicalName(8)       ' 09. 
            sql += "  FROM ("
            sql += "           ("
            '                       賃金カット情報
            sql += "               ("
            sql += "                SELECT uis.c_user_id"
            sql += "                      ,SUM(uis.i_cut)   AS i_cut"
            sql += "                      ,SUM(uis.i_cover) AS i_cover"
            sql += "                      ,SUM(uis.i_dues)  AS i_dues"
            sql += "                      ,SUM(uis.s_cut)   AS s_cut"
            sql += "                      ,SUM(uis.s_cover) AS s_cover"
            sql += "                      ,SUM(uis.s_dues)  AS s_dues"
            sql += "                  FROM ("
            '                                       賃金カット（月例時間内）
            sql += "                                SELECT c_user_id"
            sql += "                                      ,s_pay_cut AS i_cut"
            sql += "                                      ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS i_cover"
            sql += "                                      ,s_pay_cut - " & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS i_dues"
            sql += "                                      ,0 AS s_cut"
            sql += "                                      ,0 AS s_cover"
            sql += "                                      ,0 AS s_dues"
            sql += "                                  FROM pay_time_cut_monthly"
            sql += "                                 WHERE FORMAT(d_years,'yyyyMM') = :d_years"
            sql += "                                UNION"
            '                                       賃金カット（月例ストライキ）
            sql += "                                SELECT c_user_id"
            sql += "                                      ,0 AS i_cut"
            sql += "                                      ,0 AS i_cover"
            sql += "                                      ,0 AS i_dues"
            sql += "                                      ,s_pay_cut AS s_cut"
            sql += "                                      ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS s_cover"
            sql += "                                      ,s_pay_cut - " & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " As s_dures"
            sql += "                                  FROM pay_strike_cut_monthly"
            sql += "                                 WHERE FORMAT(d_years,'yyyyMM') = :d_years"
            sql += "                       ) AS uis"
            sql += "                 GROUP BY uis.c_user_id"
            sql += "               ) cut"
            '                      組合員属性情報
            sql += "               LEFT OUTER JOIN ("
            sql += "                   SELECT A1.*"
            sql += "                     FROM staf_attribute A1"
            sql += "                         ,("
            sql += "                           SELECT c_user_id"
            sql += "                                 ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                             FROM staf_attribute"
            sql += "                            WHERE d_from <= :d_from"
            sql += "                              AND c_ksh  <= :c_ksh"
            sql += "                            GROUP BY c_user_id"
            sql += "                          ) B1"
            sql += "                    WHERE A1.c_user_id = B1.c_user_id"
            sql += "                      AND A1.d_from = B1.d_from"
            sql += "               ) member"
            sql += "               ON (cut.c_user_id = member.c_user_id)"
            '                  会社所属情報
            sql += "           ) LEFT OUTER JOIN ("
            sql += "               SELECT A2.*"
            sql += "                 FROM area_local_view A2"
            sql += "                     ,("
            sql += "                       SELECT c_constant_seq"
            sql += "                             ,MAX(area_local_view.d_from) AS d_from"
            sql += "                         FROM area_local_view"
            sql += "                        WHERE d_from <= :d_from"
            sql += "                        GROUP BY c_constant_seq"
            sql += "                      ) B2"
            sql += "                WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                  AND A2.d_from         = B2.d_from"
            sql += "           ) c_branch"
            sql += "           ON (member.k_local = c_branch.c_constant_seq)"
            sql += "       )"
            sql += " ORDER BY member.k_local"
            sql += "         ,RIGHT('0000000000' + member.c_staf_id, 10)"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_years").Value = TargetYM

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetPrintSummaryDetailData： "
        ''' <summary></summary>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="TruncPlace">切捨て単位</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPrintSummaryDetailData( _
            ByVal TargetYM As String, _
            ByVal TruncPlace As Integer _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionListSummaryReportMap
            Dim sql As String = ""

            'Dim cmdText As String = String.Concat(New Object() {"SELECT SUM(cut.i_cut) AS ", map.GetPhysicalName(0), ", SUM(cut.i_cover) AS ", map.GetPhysicalName(1), ", SUM(cut.i_dues) AS ", map.GetPhysicalName(2), ", SUM(cut.s_cut) AS ", map.GetPhysicalName(3), ", SUM(cut.s_cover) AS ", map.GetPhysicalName(4), ", SUM(cut.s_dues) AS ", map.GetPhysicalName(5), " FROM (SELECT COALESCE(i_cut.c_user_id, s_cut.c_user_id) AS c_user_id,COALESCE(i_cut.s_pay_cut, 0) AS i_cut,COALESCE(i_cut.cover, 0) AS i_cover,COALESCE(i_cut.s_pay_cut, 0) - COALESCE(i_cut.cover, 0) AS i_dues,COALESCE(s_cut.s_pay_cut, 0) AS s_cut,COALESCE(s_cut.cover, 0) AS s_cover,COALESCE(s_cut.s_pay_cut, 0) - COALESCE(s_cut.cover, 0) AS s_dues FROM (SELECT c_user_id, s_pay_cut, TRUNC(s_pay_cut, ", TruncPlace, ") AS cover FROM pay_time_cut_monthly WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) i_cut FULL OUTER JOIN (SELECT c_user_id, s_pay_cut, TRUNC(s_pay_cut, ", TruncPlace, ") AS cover FROM pay_strike_cut_monthly WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) s_cut ON (i_cut.c_user_id = s_cut.c_user_id)) cut "})

            ' SQL
            sql += "SELECT SUM(cut.i_cut)   AS " & map.GetPhysicalName(0)
            sql += "      ,SUM(cut.i_cover) AS " & map.GetPhysicalName(1)
            sql += "      ,SUM(cut.i_dues)  AS " & map.GetPhysicalName(2)
            sql += "      ,SUM(cut.s_cut)   AS " & map.GetPhysicalName(3)
            sql += "      ,SUM(cut.s_cover) AS " & map.GetPhysicalName(4)
            sql += "      ,SUM(cut.s_dues)  AS " & map.GetPhysicalName(5)
            sql += "  FROM ("
            sql += "        SELECT uis.c_user_id"
            sql += "              ,SUM(uis.i_cut)   AS i_cut"
            sql += "              ,SUM(uis.i_cover) AS i_cover"
            sql += "              ,SUM(uis.i_dues)  AS i_dues"
            sql += "              ,SUM(uis.s_cut)   AS s_cut"
            sql += "              ,SUM(uis.s_cover) AS s_cover"
            sql += "              ,SUM(uis.s_dues)  AS s_dues"
            sql += "          FROM ("
            '                               賃金カット情報（月例時間内）
            sql += "                        SELECT c_user_id"
            sql += "                              ,s_pay_cut AS i_cut"
            sql += "                              ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS i_cover"
            sql += "                              ,s_pay_cut - " & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS i_dues"
            sql += "                              ,0 AS s_cut"
            sql += "                              ,0 AS s_cover"
            sql += "                              ,0 AS s_dues"
            sql += "                          FROM pay_time_cut_monthly"
            sql += "                         WHERE FORMAT(d_years,'yyyyMM') = :d_years"
            sql += "                       UNION "
            '                               賃金カット情報（月例ストライキ）
            sql += "                        SELECT c_user_id"
            sql += "                              ,0 AS i_cut"
            sql += "                              ,0 AS i_cover"
            sql += "                              ,0 AS i_dues"
            sql += "                              ,s_pay_cut AS s_cut"
            sql += "                              ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS s_cover"
            sql += "                              ,s_pay_cut - " & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " As s_dures"
            sql += "                         FROM pay_strike_cut_monthly"
            sql += "                        WHERE FORMAT(d_years,'yyyyMM') = :d_years"
            sql += "                       ) AS uis"
            sql += "                 GROUP BY uis.c_user_id"
            sql += "               ) cut"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))   ' 対象年月

                ' バインド変数設定
                command.Parameters.Item("d_years").Value = TargetYM                     ' 対象年月

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                'SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2PhysicalDataTable("dtDetail", map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetTable：照会データ取得処理 "
        ''' <summary>照会データ取得処理</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <returns>照会データ</returns>
        ''' <remarks></remarks>
        Public Function GetTable( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionMonthlyDetailMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT RIGHT('0000000000' + member.c_staf_id, 10) AS " & map.GetLogicalName(0)  ' 01. 社員番号
            sql += "      ,member.c_dezit                         AS " & map.GetLogicalName(1)  ' 02. CD
            sql += "      ,member.l_name                          AS " & map.GetLogicalName(2)  ' 03. 氏名
            sql += "      ,staf_kind.l_name                       AS " & map.GetLogicalName(3)  ' 04. 組合員種別
            sql += "      ,c_branch.l_name                        AS " & map.GetLogicalName(4)  ' 05. 会社所属
            sql += "      ,u_branch.l_name                        AS " & map.GetLogicalName(5)  ' 06. 組合支部
            sql += "      ,license.l_omission_name                AS " & map.GetLogicalName(6)  ' 07. 資格
            sql += "      ,model.l_name                           AS " & map.GetLogicalName(7)  ' 08. 機種
            sql += "      ,cut.s_pay_cut                          AS " & map.GetLogicalName(8)  ' 09. 賃金内カット金額
            sql += "      ,member.c_user_id                       AS " & map.GetLogicalName(9)  ' 10. 個人ID
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                   ("
            sql += "                       ("
            sql += "                           ("
            '                                       賃金カット情報
            sql += "                               ("
            sql += "                                SELECT *"
            sql += "                                  FROM " & MyBase.TableName
            sql += "                                 WHERE TO_CHAR(d_years,'yyyyMM') = :d_years"
            sql += "                               ) cut"
            '                                      組合員属性情報
            sql += "                               LEFT OUTER JOIN ("
            sql += "                                   SELECT A1.*"
            sql += "                                     FROM staf_attribute A1"
            sql += "                                         ,("
            sql += "                                           SELECT c_user_id"
            sql += "                                                 ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                                             FROM staf_attribute"
            sql += "                                            WHERE d_from <= :d_from"
            sql += "                                              AND c_ksh  <= :c_ksh"
            sql += "                                            GROUP BY c_user_id"
            sql += "                                          ) B1"
            sql += "                                    WHERE A1.c_user_id = B1.c_user_id"
            sql += "                                      AND A1.d_from    = B1.d_from"
            sql += "                               ) member"
            sql += "                               ON (cut.c_user_id = member.c_user_id)"
            '                                  会社所属情報
            sql += "                           ) LEFT OUTER JOIN ("
            sql += "                               SELECT A2.*"
            sql += "                                 FROM area_local_view A2"
            sql += "                                     ,("
            sql += "                                       SELECT c_constant_seq"
            sql += "                                             ,MAX(area_local_view.d_from) AS d_from"
            sql += "                                         FROM area_local_view"
            sql += "                                        WHERE d_from <= :d_from"
            sql += "                                        GROUP BY c_constant_seq"
            sql += "                                      ) B2"
            sql += "                                WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                                  AND A2.d_from = B2.d_from"
            sql += "                           ) c_branch"
            sql += "                           ON (member.k_local = c_branch.c_constant_seq)"
            '                              組合支部情報
            sql += "                       ) LEFT OUTER JOIN ("
            sql += "                           SELECT A3.*"
            sql += "                             FROM belonging_view A3"
            sql += "                                 ,("
            sql += "                                   SELECT c_constant_seq"
            sql += "                                         ,MAX(belonging_view.d_from) AS d_from"
            sql += "                                     FROM belonging_view"
            sql += "                                    WHERE d_from <= :d_from"
            sql += "                                    GROUP BY c_constant_seq"
            sql += "                                  ) B3"
            sql += "                            WHERE A3.c_constant_seq = B3.c_constant_seq"
            sql += "                              AND A3.d_from = B3.d_from"
            sql += "                       ) u_branch"
            sql += "                       ON (member.k_belonging = u_branch.c_constant_seq)"
            '                          資格情報
            sql += "                   ) LEFT OUTER JOIN ("
            sql += "                       SELECT A4.*"
            sql += "                         FROM qualification_view A4"
            sql += "                             ,("
            sql += "                               SELECT c_constant_seq"
            sql += "                                     ,MAX(qualification_view.d_from) AS d_from"
            sql += "                                 FROM qualification_view"
            sql += "                                WHERE d_from <= :d_from"
            sql += "                                GROUP BY c_constant_seq"
            sql += "                              ) B4"
            sql += "                        WHERE A4.c_constant_seq = B4.c_constant_seq"
            sql += "                          AND A4.d_from         = B4.d_from"
            sql += "                   ) license"
            sql += "                   ON (member.k_qualification = license.c_constant_seq)"
            '                      機種情報
            sql += "               ) LEFT OUTER JOIN ("
            sql += "                   SELECT A5.*"
            sql += "                     FROM model_view A5"
            sql += "                         ,("
            sql += "                           SELECT c_constant_seq"
            sql += "                                 ,MAX(model_view.d_from) AS d_from"
            sql += "                             FROM model_view"
            sql += "                            WHERE d_from <= :d_from"
            sql += "                            GROUP BY c_constant_seq"
            sql += "                          ) B5"
            sql += "                    WHERE A5.c_constant_seq = B5.c_constant_seq"
            sql += "                      AND A5.d_from         = B5.d_from"
            sql += "               ) model"
            sql += "               ON (member.k_model = model.c_constant_seq)"
            '                  組合員種別情報
            sql += "           ) LEFT OUTER JOIN ("
            sql += "               SELECT A6.*"
            sql += "                 FROM staf_kind_view A6"
            sql += "                     ,("
            sql += "                       SELECT c_constant_seq"
            sql += "                             ,MAX(staf_kind_view.d_from) AS d_from"
            sql += "                         FROM staf_kind_view"
            sql += "                        WHERE d_from <= :d_from"
            sql += "                        GROUP BY c_constant_seq"
            sql += "                      ) B6"
            sql += "                WHERE A6.c_constant_seq = B6.c_constant_seq"
            sql += "                  AND A6.d_from         = B6.d_from"
            sql += "           ) staf_kind"
            sql += "           ON (member.k_staf_kind = staf_kind.c_constant_seq)"
            sql += "       )"
            sql += " ORDER BY member.k_local"
            sql += "         ,RIGHT('0000000000' + member.c_staf_id, 10)"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))    ' 適用日
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))     ' 会社コード
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))   ' 対象年月

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate                 ' 適用日
                command.Parameters.Item("c_ksh").Value = CompanyCode                    ' 会社コード
                command.Parameters.Item("d_years").Value = TargetYM                     ' 対象年月

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2LogicalDataTable(MyBase.TableName, map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetStafInfoInTimeStrike：社員情報取得処理（時間内・争議行為） "
        ''' <summary>社員情報取得処理（時間内・争議行為）</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <param name="c_staf_id_list">社員番号リスト</param>
        ''' <returns>社員情報（Excelデータ取込）</returns>
        ''' <remarks></remarks>
        Public Function GetStafInfoInTimeStrike( _
            ByVal CompanyCode As String, _
            ByVal CriterionDate As String, _
            ByVal c_staf_id_list As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionMonthlyMemberListMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT RIGHT('000000'+member.c_staf_id,6) AS """ & map.GetLogicalName(0) & """"  ' 01. 社員番号
            sql += "      ,member.c_dezit                    AS """ & map.GetLogicalName(1) & """"  ' 02. CD
            sql += "      ,member.l_name                     AS """ & map.GetLogicalName(2) & """"  ' 03. 名前
            sql += "      ,staf_kind.l_name                  AS """ & map.GetLogicalName(3) & """"  ' 04. 組合員種別
            sql += "      ,c_branch.l_name                   AS """ & map.GetLogicalName(4) & """"  ' 05. 会社所属
            sql += "      ,u_branch.l_name                   AS """ & map.GetLogicalName(5) & """"  ' 06. 組合支部
            sql += "      ,license.l_omission_name           AS """ & map.GetLogicalName(6) & """"  ' 07. 資格
            sql += "      ,model.l_name                      AS """ & map.GetLogicalName(7) & """"  ' 08. 機種
            sql += "      ,member.c_user_id                  AS """ & map.GetLogicalName(8) & """"  ' 09. 個人ID
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                   ("
            sql += "                       ("
            '                                   組合員属性情報
            sql += "                           ("
            sql += "                            SELECT A1.*"
            sql += "                              FROM staf_attribute A1"
            sql += "                                  ,("
            sql += "                                    SELECT c_user_id"
            sql += "                                          ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                                      FROM staf_attribute"
            sql += "                                     WHERE d_from <= :d_from"
            sql += "                                       AND c_ksh <= :c_ksh"
            sql += "                                     GROUP BY c_user_id"
            sql += "                                   ) B1"
            sql += "                             WHERE A1.c_user_id = B1.c_user_id"
            sql += "                               AND A1.d_from = B1.d_from"
            sql += "                           ) member"
            '                                  会社所属情報
            sql += "                           LEFT OUTER JOIN ("
            sql += "                               SELECT A2.*"
            sql += "                                 FROM area_local_view A2"
            sql += "                                     ,("
            sql += "                                       SELECT c_constant_seq"
            sql += "                                             ,MAX(area_local_view.d_from) AS d_from"
            sql += "                                         FROM area_local_view"
            sql += "                                        WHERE d_from <= :d_from"
            sql += "                                        GROUP BY c_constant_seq"
            sql += "                                      ) B2"
            sql += "                                WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                                  AND A2.d_from = B2.d_from"
            sql += "                           ) c_branch"
            sql += "                           ON member.k_local = c_branch.c_constant_seq"
            '                              組合支部情報
            sql += "                       ) LEFT OUTER JOIN ("
            sql += "                           SELECT A3.*"
            sql += "                             FROM belonging_view A3"
            sql += "                                 ,("
            sql += "                                   SELECT c_constant_seq"
            sql += "                                         ,MAX(belonging_view.d_from) AS d_from"
            sql += "                                     FROM belonging_view"
            sql += "                                    WHERE d_from <= :d_from"
            sql += "                                    GROUP BY c_constant_seq"
            sql += "                                  ) B3"
            sql += "                            WHERE A3.c_constant_seq = B3.c_constant_seq"
            sql += "                              AND A3.d_from = B3.d_from"
            sql += "                       ) u_branch"
            sql += "                       ON member.k_belonging = u_branch.c_constant_seq"
            '                          機種情報
            sql += "                   ) LEFT OUTER JOIN ("
            sql += "                       SELECT A4.*"
            sql += "                         FROM qualification_view A4"
            sql += "                             ,("
            sql += "                               SELECT c_constant_seq"
            sql += "                                     ,MAX(qualification_view.d_from) AS d_from"
            sql += "                                 FROM qualification_view"
            sql += "                                WHERE d_from <= :d_from"
            sql += "                                GROUP BY c_constant_seq"
            sql += "                              ) B4"
            sql += "                        WHERE A4.c_constant_seq = B4.c_constant_seq"
            sql += "                          AND A4.d_from = B4.d_from"
            sql += "                   ) license"
            sql += "                   ON member.k_qualification = license.c_constant_seq"
            '                      資格情報
            sql += "               ) LEFT OUTER JOIN ("
            sql += "                   SELECT A5.*"
            sql += "                     FROM model_view A5"
            sql += "                         ,("
            sql += "                           SELECT c_constant_seq"
            sql += "                                 ,MAX(model_view.d_from) AS d_from"
            sql += "                             FROM model_view"
            sql += "                            WHERE d_from <= :d_from"
            sql += "                            GROUP BY c_constant_seq"
            sql += "                          ) B5"
            sql += "                    WHERE A5.c_constant_seq = B5.c_constant_seq"
            sql += "                      AND A5.d_from = B5.d_from"
            sql += "               ) model ON member.k_model = model.c_constant_seq"
            '                  組合員種別
            sql += "           ) LEFT OUTER JOIN ("
            sql += "               SELECT A6.*"
            sql += "                 FROM staf_kind_view A6"
            sql += "                     ,("
            sql += "                       SELECT c_constant_seq"
            sql += "                             ,MAX(staf_kind_view.d_from) AS d_from"
            sql += "                         FROM staf_kind_view"
            sql += "                        WHERE d_from <= :d_from"
            sql += "                        GROUP BY c_constant_seq"
            sql += "                      ) B6"
            sql += "                WHERE A6.c_constant_seq = B6.c_constant_seq"
            sql += "                  AND A6.d_from = B6.d_from"
            sql += "           ) staf_kind"
            sql += "           ON (member.k_staf_kind = staf_kind.c_constant_seq)"
            sql += "       )"
            sql += " WHERE RIGHT('000000'+member.c_staf_id, 6) IN (:c_staf_id_list)"
            sql += " ORDER BY member.c_staf_id"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))            ' 適用日
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))             ' 会社コード
                command.Parameters.Add(New NpgsqlParameter("c_staf_id_list", DbType.String))    ' 社員番号リスト

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate                         ' 適用日
                command.Parameters.Item("c_ksh").Value = CompanyCode                            ' 会社コード
                command.Parameters.Item("c_staf_id_list").Value = c_staf_id_list                ' 社員番号リスト

                ' ログ出力
                MonthlyBaseDao._logger.Debug(sql)

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2LogicalDataTable(MyBase.TableName, map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

    End Class
End Namespace
