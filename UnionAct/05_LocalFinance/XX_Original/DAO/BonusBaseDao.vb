'===========================================================================================================
'   クラスＩＤ　　：BonusBaseDao
'   クラス名称　　：賃金カット（一時金）DAOベースクラス
'   備考  　　　　：
'===========================================================================================================
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping
Imports log4net

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection

Namespace DAO.FinancialAffairs.WageReduction
    Public Class BonusBaseDao
        Inherits WageReductionBaseDao

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Protected Const IN_LINE_VIEW_CUT As String = "(SELECT I1.c_user_id AS c_user_id, I1.s_pay_cut AS i_cut, S1.s_pay_cut AS s_cut FROM (SELECT c_user_id, s_pay_cut FROM pay_time_cut_once WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) I1, (SELECT c_user_id, s_pay_cut FROM pay_strike_cut_once WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) S1 WHERE I1.c_user_id = S1.c_user_id) cut "

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New("wage_reduction_bonus_list")
        End Sub

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
        ''' <param name="TruncPlace"></param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function FindSummury(
            ByVal CompanyCode As String,
            ByVal TargetYM As String,
            ByVal CriterionDate As String,
            ByVal TruncPlace As Integer,
            Optional ByVal strBonusName As String = ""
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionBonusListMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT c_branch.l_name"
            sql += "      ,COUNT(cut.c_user_id)"
            sql += "      ,SUM(cut.i_cut)"
            sql += "      ,SUM(cut.s_cut)"
            sql += "      ,SUM(cut.cut_sum)"
            sql += "      ,SUM(cut.[truncate])"
            sql += "      ,SUM(cut.cut_sum) - SUM(cut.[truncate])"
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                SELECT I1.c_user_id AS c_user_id"
            sql += "                      ,I1.s_pay_cut AS i_cut"
            sql += "                      ,S1.s_pay_cut AS s_cut"
            sql += "                      ,I1.s_pay_cut + S1.s_pay_cut AS cut_sum"
            sql += "                      ," & MDFinanceCommon.Trunc("I1.s_pay_cut + S1.s_pay_cut", TruncPlace) & " AS [truncate]"
            sql += "                  FROM ("
            sql += "                        SELECT c_user_id"
            sql += "                              ,s_pay_cut"
            sql += "                              ,c_pay_once_name"
            sql += "                          FROM pay_time_cut_once"
            sql += "                         WHERE TO_CHAR(d_years,'yyyyMM') = :d_years"

            If Trim(strBonusName).Length > 0 Then
                sql += "                           AND c_pay_once_name = '" & strBonusName & "'"
            End If

            sql += "                       ) I1"
            sql += "                       ,("
            sql += "                         SELECT c_user_id"
            sql += "                               ,s_pay_cut"
            sql += "                               ,c_pay_once_name"
            sql += "                           FROM pay_strike_cut_once"
            sql += "                          WHERE TO_CHAR(d_years, 'yyyyMM') = :d_years"

            If Trim(strBonusName).Length > 0 Then
                sql += "                           AND c_pay_once_name = '" & strBonusName & "'"
            End If

            sql += "                        ) S1"
            sql += "                  WHERE I1.c_user_id = S1.c_user_id"
            sql += "                    AND I1.c_pay_once_name = S1.c_pay_once_name"
            sql += "               ) cut"
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
            sql += "               ON cut.c_user_id = member.c_user_id"
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
            sql += " ORDER BY member.k_local"

            Try
                ' コマンドオブジェクト性絵師
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_years").Value = TargetYM

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                tbl = MyBase.DataReader2LogicalDataTable("wage_reduction_bonus_list", map, dReader)

            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetPrintDetailListData： "
        ''' <summary></summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="TruncPlace"></param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPrintDetailListData(
            ByVal CompanyCode As String,
            ByVal TargetYM As String,
            ByVal TruncPlace As Integer,
            ByVal CriterionDate As String,
            ByVal strBonusName As String
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionListBonusReportMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT member.c_staf_id"        ' 01. 社員番号
            sql += "      ,member.l_name"           ' 02. 氏名
            sql += "      ,c_branch.l_name"         ' 03. 会社所属
            sql += "      ,cut.i_cut"               ' 04. 賃金内カット金額（時間内）
            sql += "      ,cut.s_cut"               ' 05. 賃金内カット金額（ストライキ）
            sql += "  FROM ("
            sql += "           ("
            '                      賃金カット情報（一時金時間内・一時金ストライキ）
            sql += "               ("
            sql += "                SELECT a.c_user_id"             ' 社員番号
            sql += "                      ,a.s_pay_cut AS i_cut"    ' 賃金内カット金額（時間内）
            sql += "                      ,b.s_pay_cut AS s_cut"    ' 賃金内カット金額（ストライキ）
            sql += "                  FROM pay_time_cut_once a"
            sql += "                       INNER JOIN pay_strike_cut_once b"
            sql += "                       ON  a.c_user_id       = b.c_user_id"
            sql += "                       AND a.c_pay_once_name = b.c_pay_once_name"
            sql += "                 WHERE TO_CHAR(a.d_years,'yyyyMM') = :d_years"
            sql += "                   AND TO_CHAR(b.d_years,'yyyyMM') = :d_years"

            ' 一時金名称が有れば、条件に追加
            If Trim(strBonusName).Length > 0 Then
                sql += "                   AND a.c_pay_once_name = '" & strBonusName & "'"
                sql += "                   AND b.c_pay_once_name = '" & strBonusName & "'"
            End If

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

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("d_years").Value = TargetYM

                ' ログ出力
                BonusBaseDao._logger.Debug(sql)

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
        ''' <param name="TruncPlace"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPrintSummaryDetailData(
            ByVal TargetYM As String,
            ByVal TruncPlace As Integer
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionListSummaryReportMap
            Dim sql As String = ""

            'Dim cmdText As String = String.Concat(New Object() {"SELECT SUM(cut.i_cut) AS ", map.GetPhysicalName(0), ", SUM(cut.i_cover) AS ", map.GetPhysicalName(1), ", SUM(cut.i_dues) AS ", map.GetPhysicalName(2), ", SUM(cut.s_cut) AS ", map.GetPhysicalName(3), ", SUM(cut.s_cover) AS ", map.GetPhysicalName(4), ", SUM(cut.s_dues) AS ", map.GetPhysicalName(5), " FROM (SELECT i_cut.c_user_id AS c_user_id,i_cut.s_pay_cut AS i_cut,i_cut.cover AS i_cover,i_cut.s_pay_cut - i_cut.cover AS i_dues,s_cut.s_pay_cut AS s_cut,s_cut.cover AS s_cover,s_cut.s_pay_cut - s_cut.cover AS s_dues FROM (SELECT c_user_id, s_pay_cut," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover FROM pay_time_cut_once WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) i_cut, (SELECT c_user_id, s_pay_cut," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover FROM pay_strike_cut_once WHERE TO_CHAR(d_years,'yyyyMM') = :d_years) s_cut WHERE i_cut.c_user_id = s_cut.c_user_id) cut "})

            ' SQL
            sql += "SELECT SUM(cut.i_cut)   AS " & map.GetPhysicalName(0)
            sql += "      ,SUM(cut.i_cover) AS " & map.GetPhysicalName(1)
            sql += "      ,SUM(cut.i_dues)  AS " & map.GetPhysicalName(2)
            sql += "      ,SUM(cut.s_cut)   AS " & map.GetPhysicalName(3)
            sql += "      ,SUM(cut.s_cover) AS " & map.GetPhysicalName(4)
            sql += "      ,SUM(cut.s_dues)  AS " & map.GetPhysicalName(5)
            sql += "  FROM ("
            sql += "        SELECT i_cut.c_user_id               AS c_user_id"
            sql += "              ,i_cut.s_pay_cut               AS i_cut"
            sql += "              ,i_cut.cover                   AS i_cover"
            sql += "              ,i_cut.s_pay_cut - i_cut.cover AS i_dues"
            sql += "              ,s_cut.s_pay_cut               AS s_cut"
            sql += "              ,s_cut.cover                   AS s_cover"
            sql += "              ,s_cut.s_pay_cut - s_cut.cover AS s_dues"
            sql += "          FROM ("
            '                       賃金カット情報（一時金時間内）
            sql += "                SELECT c_user_id"
            sql += "                      ,s_pay_cut"
            sql += "                      ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover"
            sql += "                  FROM pay_time_cut_once"
            sql += "                 WHERE TO_CHAR(d_years,'yyyyMM') = :d_years"
            sql += "               ) i_cut"
            '                       賃金カット情報（一時金ストライキ）
            sql += "              ,("
            sql += "                SELECT c_user_id"
            sql += "                      ,s_pay_cut"
            sql += "                      ," & MDFinanceCommon.Trunc("s_pay_cut", TruncPlace) & " AS cover"
            sql += "                  FROM pay_strike_cut_once"
            sql += "                 WHERE TO_CHAR(d_years,'yyyyMM') = :d_years"
            sql += "               ) s_cut"
            sql += "         WHERE i_cut.c_user_id = s_cut.c_user_id"
            sql += "       ) cut"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' ログ出力
                BonusBaseDao._logger.Debug(sql)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_years").Value = TargetYM

                ' ログ出力
                BonusBaseDao._logger.Debug(sql)

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

#Region " GetTable：一時金照会データ取得処理 "
        ''' <summary>一時金照会データ取得処理</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns>一時金照会データ</returns>
        ''' <remarks></remarks>
        Public Function GetTable(
            ByVal CompanyCode As String,
            ByVal TargetYM As String,
            ByVal CriterionDate As String,
            ByVal strBonusName As String
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionBonusJoinDetailMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT RIGHT('0000000000' + member.c_staf_id, 10) AS " & map.GetLogicalName(0)  ' 01. 社員番号
            sql += "      ,member.c_dezit                         AS " & map.GetLogicalName(1)  ' 02. CD
            sql += "      ,member.l_name                          AS " & map.GetLogicalName(2)  ' 03. 名前
            sql += "      ,staf_kind.l_name                       AS " & map.GetLogicalName(3)  ' 04. 組合員種別
            sql += "      ,c_branch.l_name                        AS " & map.GetLogicalName(4)  ' 05. 会社所属
            sql += "      ,u_branch.l_name                        AS " & map.GetLogicalName(5)  ' 06. 組合支部
            sql += "      ,license.l_omission_name                AS " & map.GetLogicalName(6)  ' 07. 資格
            sql += "      ,cut.c_pay_once_name                    AS " & map.GetLogicalName(7)  ' 08. 一時金名称
            sql += "      ,cut.i_cut                              AS " & map.GetLogicalName(8)  ' 09. 時間内控除額
            sql += "      ,cut.s_cut                              AS " & map.GetLogicalName(9)  ' 10. 争議行為控除額
            sql += "      ,cut.i_cut + cut.s_cut                  AS " & map.GetLogicalName(10) ' 11. 控除額計
            sql += "      ,member.c_user_id                       AS " & map.GetLogicalName(11) ' 12. 個人ID
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                   ("
            sql += "                       ("
            '                                   賃金カット情報（一時金時間内・一時金ストライキ）
            sql += "                           ("
            sql += "                            SELECT a.c_user_id"
            sql += "                                  ,a.s_pay_cut as i_cut"
            sql += "                                  ,b.s_pay_cut AS s_cut"
            sql += "                                  ,a.c_pay_once_name"
            sql += "                              FROM pay_time_cut_once a"
            sql += "                                   INNER JOIN pay_strike_cut_once b"
            sql += "                                   ON  a.c_user_id       = b.c_user_id"
            sql += "                                   AND a.c_pay_once_name = b.c_pay_once_name"
            sql += "                             WHERE TO_CHAR(a.d_years,'yyyyMM') = :d_years"
            sql += "                               AND TO_CHAR(b.d_years,'yyyyMM') = :d_years"

            ' 一時金名称が有れば、条件に追加
            If Trim(strBonusName).Length > 0 Then
                sql += "                               AND a.c_pay_once_name = '" & strBonusName & "'"
                sql += "                               AND b.c_pay_once_name = '" & strBonusName & "'"
            End If

            sql += "                           ) cut"

            '                                  組合員属性情報
            sql += "                           LEFT OUTER JOIN ("
            sql += "                               SELECT A1.*"
            sql += "                                 FROM staf_attribute A1"
            sql += "                                     ,("
            sql += "                                       SELECT c_user_id"
            sql += "                                             ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                                         FROM staf_attribute"
            sql += "                                        WHERE d_from <= :d_from"
            sql += "                                          AND c_ksh  <= :c_ksh"
            sql += "                                        GROUP BY c_user_id"
            sql += "                                      ) B1"
            sql += "                                WHERE A1.c_user_id = B1.c_user_id"
            sql += "                                  AND A1.d_from    = B1.d_from"
            sql += "                           ) member"
            sql += "                           ON (cut.c_user_id = member.c_user_id)"
            '                              会社所属情報
            sql += "                       ) LEFT OUTER JOIN ("
            sql += "                           SELECT A2.*"
            sql += "                             FROM area_local_view A2"
            sql += "                                 ,("
            sql += "                                   SELECT c_constant_seq"
            sql += "                                         ,MAX(area_local_view.d_from) AS d_from"
            sql += "                                     FROM area_local_view"
            sql += "                                    WHERE d_from <= :d_from"
            sql += "                                    GROUP BY c_constant_seq"
            sql += "                                  ) B2"
            sql += "                            WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                              AND A2.d_from         = B2.d_from"
            sql += "                       ) c_branch"
            sql += "                       ON (member.k_local = c_branch.c_constant_seq)"
            '                          組合支部情報
            sql += "                   ) LEFT OUTER JOIN ("
            sql += "                       SELECT A3.*"
            sql += "                         FROM belonging_view A3"
            sql += "                             ,("
            sql += "                               SELECT c_constant_seq"
            sql += "                                     ,MAX(belonging_view.d_from) AS d_from"
            sql += "                                 FROM belonging_view"
            sql += "                                WHERE d_from <= :d_from"
            sql += "                                GROUP BY c_constant_seq"
            sql += "                              ) B3"
            sql += "                        WHERE A3.c_constant_seq = B3.c_constant_seq"
            sql += "                          AND A3.d_from         = B3.d_from"
            sql += "                   ) u_branch"
            sql += "                   ON (member.k_belonging = u_branch.c_constant_seq)"
            '                      資格情報
            sql += "               ) LEFT OUTER JOIN ("
            sql += "                   SELECT A4.*"
            sql += "                     FROM qualification_view A4"
            sql += "                         ,("
            sql += "                           SELECT c_constant_seq"
            sql += "                                 ,MAX(qualification_view.d_from) AS d_from"
            sql += "                             FROM qualification_view"
            sql += "                            WHERE d_from <= :d_from"
            sql += "                            GROUP BY c_constant_seq"
            sql += "                          ) B4"
            sql += "                    WHERE A4.c_constant_seq = B4.c_constant_seq"
            sql += "                      AND A4.d_from         = B4.d_from"
            sql += "               ) license"
            sql += "               ON (member.k_qualification = license.c_constant_seq)"
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
            sql += "                  AND A6.d_from = B6.d_from"
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
                BonusBaseDao._logger.Debug(sql)

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

#Region " GetStafInfoBounus：社員情報取得処理（一時金Excelデータ） "
        ''' <summary>社員情報取得処理（一時金Excelデータ）</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="CriterionDate">適用日</param>
        ''' <param name="c_staf_id_list">社員番号リスト</param>
        ''' <returns>社員情報（一時金Excelデータ）</returns>
        ''' <remarks></remarks>
        Public Function GetStafInfoBounus( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
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
            sql += "                                       AND c_ksh  <= :c_ksh"
            sql += "                                     GROUP BY c_user_id"
            sql += "                                   ) B1"
            sql += "                             WHERE A1.c_user_id = B1.c_user_id"
            sql += "                               AND A1.d_from    = B1.d_from"
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
            sql += "               ) model"
            sql += "               ON member.k_model = model.c_constant_seq"
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
            sql += " WHERE RIGHT('000000'+member.c_staf_id,6) IN (:c_staf_id_list)"
            sql += " ORDER BY member.c_staf_id"

            Try
                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' バインド変数定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_staf_id_list", DbType.String))

                ' バインド変数設定
                command.Parameters.Item("d_from").Value = CriterionDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                command.Parameters.Item("c_staf_id_list").Value = c_staf_id_list

                ' ログ出力
                BonusBaseDao._logger.Debug(sql)

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
