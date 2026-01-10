'===========================================================================================================
'   クラスＩＤ　　：WageReductionBaseDao
'   クラス名称　　：賃金カットDAOベースクラス
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
    Public MustInherit Class WageReductionBaseDao
        Inherits FinancialAffairsBaseDao

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)

#Region " プロパティ "
        ' テーブル名
        Private _tableName As String
        Protected ReadOnly Property TableName() As String
            Get
                Return Me._tableName
            End Get
        End Property
#End Region

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub

        ''' <summary>コンストラクタ</summary>
        ''' <param name="TableName">テーブル名</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal TableName As String)
            Me._tableName = TableName
        End Sub
#End Region

#Region " Delete：削除処理 "
        ''' <summary>削除処理</summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="AutoCommit">自動コミット（True：, False：）</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Public Function Delete( _
            ByVal command As NpgsqlCommand, _
            ByVal TargetYM As String, _
            ByVal AutoCommit As Boolean _
        ) As Integer

            Dim num As Integer = 0
            Dim sql As String = ""

            Try
                ' SQL
                sql += "DELETE"
                sql += "  FROM " & Me.TableName
                sql += " WHERE TO_CHAR(d_years, 'yyyyMM') = '" & TargetYM & "'"

                ' ログ出力
                WageReductionBaseDao._logger.Debug(sql)

                ' SQL設定
                command.SetSql(sql)

                ' SQL実行
                num = command.ExecuteNonQuery

                'If AutoCommit Then
                '    objTran.Commit()
                'End If

            Catch exception As NpgsqlException
                'If AutoCommit Then
                '    objTran.Rollback()
                'End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                'If AutoCommit Then
                '    objTran.Rollback()
                'End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return num

        End Function
#End Region

#Region " FindMember：社員情報取得処理 "
        ''' <summary>社員情報取得処理</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="CriterionDate">適用日（yyyyMMdd）</param>
        ''' <returns>社員情報</returns>
        ''' <remarks></remarks>
        Public Function FindMember( _
            ByVal CompanyCode As String, _
            ByVal EmployeeNumber As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionMemberMap
            Dim sql As String = ""

            'Dim message As String = (String.Concat(New String() {"SELECT member.c_dezit AS """, map.GetLogicalName(0), """, member.l_name AS """, map.GetLogicalName(1), """, c_branch.l_name AS """, map.GetLogicalName(2), """, u_branch.l_name AS """, map.GetLogicalName(3), """, license.l_omission_name AS """, map.GetLogicalName(4), """, model.l_name AS """, map.GetLogicalName(5), """, member.c_user_id AS """, map.GetLogicalName(6), """, staf_kind.l_name AS """, map.GetLogicalName(7), """ FROM ((((((SELECT A1.* FROM staf_attribute A1, (SELECT c_user_id, MAX(staf_attribute.d_from) AS d_from FROM staf_attribute WHERE d_from <= :d_from AND c_ksh <= :c_ksh GROUP BY c_user_id) B1 WHERE A1.c_user_id = B1.c_user_id AND A1.d_from = B1.d_from ) member LEFT OUTER JOIN (SELECT A2.* FROM area_local_view A2, (SELECT c_constant_seq, MAX(area_local_view.d_from) AS d_from FROM area_local_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B2 WHERE A2.c_constant_seq = B2.c_constant_seq AND A2.d_from = B2.d_from ) c_branch ON member.k_local = c_branch.c_constant_seq) LEFT OUTER JOIN (SELECT A3.* FROM belonging_view A3, (SELECT c_constant_seq, MAX(belonging_view.d_from) AS d_from FROM belonging_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B3 WHERE A3.c_constant_seq = B3.c_constant_seq AND A3.d_from = B3.d_from ) u_branch ON member.k_belonging = u_branch.c_constant_seq) LEFT OUTER JOIN (SELECT A4.* FROM qualification_view A4, (SELECT c_constant_seq, MAX(qualification_view.d_from) AS d_from FROM qualification_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B4 WHERE A4.c_constant_seq = B4.c_constant_seq AND A4.d_from = B4.d_from ) license ON member.k_qualification = license.c_constant_seq) LEFT OUTER JOIN (SELECT A5.* FROM model_view A5, (SELECT c_constant_seq, MAX(model_view.d_from) AS d_from FROM model_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B5 WHERE A5.c_constant_seq = B5.c_constant_seq AND A5.d_from = B5.d_from ) model ON member.k_model = model.c_constant_seq) " & _
            '"LEFT OUTER JOIN (SELECT A6.* FROM staf_kind_view A6, (SELECT c_constant_seq, MAX(staf_kind_view.d_from) AS d_from FROM staf_kind_view WHERE d_from <= :d_from GROUP BY c_constant_seq) B6 WHERE A6.c_constant_seq = B6.c_constant_seq AND A6.d_from = B6.d_from ) staf_kind ON (member.k_staf_kind = staf_kind.c_constant_seq)) " & _
            '"WHERE member.c_staf_id = :c_staf_id"}) & " ORDER BY member.d_join DESC ")

            ' SQL
            sql += "SELECT member.c_dezit          AS """ & map.GetLogicalName(0) & """"    ' 01. CD
            sql += "      ,member.l_name           AS """ & map.GetLogicalName(1) & """"    ' 02. 氏名
            sql += "      ,c_branch.l_name         AS """ & map.GetLogicalName(2) & """"    ' 03. 会社所属
            sql += "      ,u_branch.l_name         AS """ & map.GetLogicalName(3) & """"    ' 04. 組合支部
            sql += "      ,license.l_omission_name AS """ & map.GetLogicalName(4) & """"    ' 05. 機種
            sql += "      ,model.l_name            AS """ & map.GetLogicalName(5) & """"    ' 06. 資格
            sql += "      ,member.c_user_id        AS """ & map.GetLogicalName(6) & """"    ' 07. 個人ID
            sql += "      ,staf_kind.l_name        AS """ & map.GetLogicalName(7) & """"    ' 08. 組合員種別
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                   ("
            sql += "                       ("
            '                                  組合員属性情報
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
            sql += "                                  AND A2.d_from         = B2.d_from"
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
            sql += "                              AND A3.d_from         = B3.d_from"
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
            sql += "                           ) B5"
            sql += "                    WHERE A5.c_constant_seq = B5.c_constant_seq"
            sql += "                      AND A5.d_from         = B5.d_from"
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
            sql += "                       ) B6"
            sql += "                 WHERE A6.c_constant_seq = B6.c_constant_seq"
            sql += "                   AND A6.d_from         = B6.d_from"
            sql += "           ) staf_kind"
            sql += "           ON (member.k_staf_kind = staf_kind.c_constant_seq)"
            sql += "       )"
            sql += " WHERE member.c_staf_id = :c_staf_id"
            sql += " ORDER BY member.d_join DESC"

            Try
                ' ログ出力
                WageReductionBaseDao._logger.Debug(sql)

                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' 条件変数バインド定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))        ' 適用日
                command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))     ' 社員番号
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))         ' 会社コード（所属会社）

                ' 条件変数バインド設定
                command.Parameters.Item("d_from").Value = CriterionDate                     ' 適用日
                command.Parameters.Item("c_staf_id").Value = EmployeeNumber                 ' 社員番号
                command.Parameters.Item("c_ksh").Value = CompanyCode                        ' 会社コード（所属会社）

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New DataNotFoundException
                End If
                tbl = MyBase.DataReader2LogicalDataTable("wage_reduction_member_info", map, dReader)

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            ' 戻り値設定
            Return tbl

        End Function
#End Region

#Region " FindMemberList：社員情報取得処理（Excelデータ取込） "
        ''' <summary>社員情報取得処理（Excelデータ取込）</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="EmployeeNumberList">社員番号リスト（カンマ区切り）</param>
        ''' <param name="CriterionDate">適用日（yyyyMMdd）</param>
        ''' <returns>社員情報</returns>
        ''' <remarks>
        ''' 社員番号リストの先頭と後尾のシングルクォーテーションは、バインド後に付くので無しの設定
        ''' 例：000001','000002','000003','000004','000005
        ''' バインド前：000001','000002','000003','000004','000005
        ''' バインド後：'000001','000002','000003','000004','000005'
        ''' </remarks>
        Public Function FindMemberList( _
            ByVal CompanyCode As String, _
            ByVal EmployeeNumberList As String, _
            ByVal CriterionDate As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim map As New WageReductionMemberMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT member.c_dezit          AS """ & map.GetLogicalName(0) & """"    ' 01. CD
            sql += "      ,member.l_name           AS """ & map.GetLogicalName(1) & """"    ' 02. 名前
            sql += "      ,c_branch.l_name         AS """ & map.GetLogicalName(2) & """"    ' 03. 会社所属
            sql += "      ,u_branch.l_name         AS """ & map.GetLogicalName(3) & """"    ' 04. 組合支部
            sql += "      ,license.l_omission_name AS """ & map.GetLogicalName(4) & """"    ' 05. 機種
            sql += "      ,model.l_name            AS """ & map.GetLogicalName(5) & """"    ' 06. 資格
            sql += "      ,member.c_user_id        AS """ & map.GetLogicalName(6) & """"    ' 07. 個人ID
            sql += "      ,staf_kind.l_name        AS """ & map.GetLogicalName(7) & """"    ' 08. 組合員種別
            sql += "  FROM ("
            sql += "           ("
            sql += "               ("
            sql += "                   ("
            sql += "                       ("
            '                                  組合員属性情報
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
            '                                   会社所属情報
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
            '                               組合支部情報
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
            '                           機種情報
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
            '                       資格情報
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
            sql += "                  AND A6.d_from         = B6.d_from"
            sql += "           ) staf_kind"
            sql += "           ON (member.k_staf_kind = staf_kind.c_constant_seq"
            sql += "           )"
            sql += "       )"
            sql += " WHERE member.c_staf_id IN (:c_staf_id_list)"
            sql += " ORDER BY member.d_join DESC"

            Try
                ' ログ出力
                WageReductionBaseDao._logger.Debug(sql)

                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' 条件変数バインド定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))        ' 適用日
                command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))     ' 社員番号
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))         ' 会社コード（所属会社）

                ' 条件変数バインド設定
                command.Parameters.Item("d_from").Value = CriterionDate                     ' 適用日
                command.Parameters.Item("c_staf_id").Value = EmployeeNumberList             ' 社員番号
                command.Parameters.Item("c_ksh").Value = CompanyCode                        ' 会社コード（所属会社）

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New DataNotFoundException
                End If

                tbl = MyBase.DataReader2LogicalDataTable("wage_reduction_member_info", map, dReader)

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            ' 戻り値設定
            Return tbl

        End Function
#End Region

#Region " FindExistMember：社員番号存在チェック処理 "
        ''' <summary>社員番号存在チェック処理</summary>
        ''' <param name="CompanyCode">会社コード</param>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="CriterionDate">適用日（yyyyMMdd）</param>
        ''' <returns>True：存在する, False：存在しない</returns>
        ''' <remarks></remarks>
        Public Function FindExistMember( _
            ByVal CompanyCode As String, _
            ByVal EmployeeNumber As String, _
            ByVal CriterionDate As String _
        ) As Boolean

            Dim flg As Boolean = False              ' 処理結果（True：存在する, False：存在しない）
            Dim map As New WageReductionMemberMap
            Dim sql As String = ""

            ' SQL
            sql += "SELECT member.c_dezit          AS """ & map.GetLogicalName(0) & """"    ' 01. CD
            sql += "      ,member.l_name           AS """ & map.GetLogicalName(1) & """"    ' 02. 名前
            sql += "      ,c_branch.l_name         AS """ & map.GetLogicalName(2) & """"    ' 03. 会社所属
            sql += "      ,u_branch.l_name         AS """ & map.GetLogicalName(3) & """"    ' 04. 組合支部
            sql += "      ,license.l_omission_name AS """ & map.GetLogicalName(4) & """"    ' 05. 機種
            sql += "      ,model.l_name            AS """ & map.GetLogicalName(5) & """"    ' 06. 資格
            sql += "      ,member.c_user_id        AS """ & map.GetLogicalName(6) & """"    ' 07. 個人ID
            sql += "      ,staf_kind.l_name        AS """ & map.GetLogicalName(7) & """"    ' 08. 組合員種別
            sql += "  FROM ("
            sql += "            ("
            sql += "                ("
            sql += "                    ("
            sql += "                        ("
            '                                    組合員属性情報
            sql += "                            ("
            sql += "                             SELECT A1.*"
            sql += "                               FROM staf_attribute A1"
            sql += "                                   ,("
            sql += "                                     SELECT c_user_id"
            sql += "                                           ,MAX(staf_attribute.d_from) AS d_from"
            sql += "                                       FROM staf_attribute"
            sql += "                                      WHERE d_from <= :d_from"
            sql += "                                        AND c_ksh  <= :c_ksh"
            sql += "                                      GROUP BY c_user_id"
            sql += "                                    ) B1"
            sql += "                              WHERE A1.c_user_id = B1.c_user_id"
            sql += "                                AND A1.d_from    = B1.d_from"
            sql += "                            ) member"
            '                                   会社所属情報
            sql += "                            LEFT OUTER JOIN ("
            sql += "                                SELECT A2.*"
            sql += "                                  FROM area_local_view A2"
            sql += "                                      ,("
            sql += "                                        SELECT c_constant_seq"
            sql += "                                              ,MAX(area_local_view.d_from) AS d_from"
            sql += "                                          FROM area_local_view"
            sql += "                                         WHERE d_from <= :d_from"
            sql += "                                         GROUP BY c_constant_seq"
            sql += "                                       ) B2"
            sql += "                                 WHERE A2.c_constant_seq = B2.c_constant_seq"
            sql += "                                   AND A2.d_from         = B2.d_from"
            sql += "                            ) c_branch"
            sql += "                            ON member.k_local = c_branch.c_constant_seq"
            '                               組合支部情報
            sql += "                        ) LEFT OUTER JOIN ("
            sql += "                            SELECT A3.*"
            sql += "                              FROM belonging_view A3"
            sql += "                                  ,("
            sql += "                                    SELECT c_constant_seq"
            sql += "                                          ,MAX(belonging_view.d_from) AS d_from"
            sql += "                                      FROM belonging_view"
            sql += "                                     WHERE d_from <= :d_from"
            sql += "                                     GROUP BY c_constant_seq"
            sql += "                                   ) B3"
            sql += "                             WHERE A3.c_constant_seq = B3.c_constant_seq"
            sql += "                               AND A3.d_from         = B3.d_from"
            sql += "                        ) u_branch"
            sql += "                        ON member.k_belonging = u_branch.c_constant_seq"
            '                           機種情報
            sql += "                    ) LEFT OUTER JOIN ("
            sql += "                        SELECT A4.*"
            sql += "                          FROM qualification_view A4"
            sql += "                              ,("
            sql += "                                SELECT c_constant_seq"
            sql += "                                      ,MAX(qualification_view.d_from) AS d_from"
            sql += "                                  FROM qualification_view"
            sql += "                                 WHERE d_from <= :d_from"
            sql += "                                 GROUP BY c_constant_seq"
            sql += "                               ) B4"
            sql += "                         WHERE A4.c_constant_seq = B4.c_constant_seq"
            sql += "                           AND A4.d_from = B4.d_from"
            sql += "                    ) license"
            sql += "                    ON member.k_qualification = license.c_constant_seq"
            '                       資格情報
            sql += "                ) LEFT OUTER JOIN ("
            sql += "                    SELECT A5.*"
            sql += "                      FROM model_view A5"
            sql += "                          ,("
            sql += "                            SELECT c_constant_seq"
            sql += "                                  ,MAX(model_view.d_from) AS d_from"
            sql += "                              FROM model_view"
            sql += "                             WHERE d_from <= :d_from"
            sql += "                             GROUP BY c_constant_seq"
            sql += "                           ) B5"
            sql += "                     WHERE A5.c_constant_seq = B5.c_constant_seq"
            sql += "                       AND A5.d_from = B5.d_from"
            sql += "                ) model"
            sql += "                ON member.k_model = model.c_constant_seq"
            '                   組合員種別
            sql += "            ) LEFT OUTER JOIN ("
            sql += "                SELECT A6.*"
            sql += "                  FROM staf_kind_view A6"
            sql += "                      ,("
            sql += "                        SELECT c_constant_seq"
            sql += "                              ,MAX(staf_kind_view.d_from) AS d_from"
            sql += "                          FROM staf_kind_view"
            sql += "                         WHERE d_from <= :d_from"
            sql += "                         GROUP BY c_constant_seq"
            sql += "                       ) B6"
            sql += "                 WHERE A6.c_constant_seq = B6.c_constant_seq"
            sql += "                   AND A6.d_from         = B6.d_from"
            sql += "            ) staf_kind"
            sql += "            ON (member.k_staf_kind = staf_kind.c_constant_seq)"
            sql += "       )"
            sql += " WHERE FORMAT(member.c_staf_id, '000000') = :c_staf_id"
            sql += " ORDER BY member.d_join DESC"

            Try
                ' ログ出力
                WageReductionBaseDao._logger.Debug(sql)

                ' コマンドオブジェクト生成
                Dim command As New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' 条件変数バインド定義
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))        ' 適用日
                command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))     ' 社員番号
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))         ' 会社コード（所属会社）

                ' 条件変数バインド設定
                command.Parameters.Item("d_from").Value = CriterionDate                     ' 適用日
                command.Parameters.Item("c_staf_id").Value = EmployeeNumber                 ' 社員番号
                command.Parameters.Item("c_ksh").Value = CompanyCode                        ' 会社コード（所属会社）

                ' SQL実行
                Dim dReader As NpgsqlDataReader = command.ExecuteReader

                ' 結果判定
                If Not dReader.HasRows Then
                    flg = False
                Else
                    flg = True
                End If

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            ' 戻り値設定
            Return flg

        End Function
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
        Public MustOverride Function FindSummury( _
            ByVal CompanyCode As String, _
            ByVal TargetYM As String, _
            ByVal CriterionDate As String, _
            ByVal TruncPlace As Integer, _
            Optional ByVal strBonusName As String = "" _
        ) As DataTable

        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxYears() As String

            Dim str As String
            Dim sql As String = ""
            Dim obj As Object = Nothing
            Dim command As NpgsqlCommand = Nothing

            Try
                ' SQL
                sql += "SELECT DISTINCT(MAX(TO_CHAR(d_years, 'yyyyMM')))"
                sql += "  FROM " & Me.TableName

                ' コマンドオブジェクト生成
                command = New NpgsqlCommand(sql, MyBase.GetNpgsqlConnection)

                ' SQL実行
                obj = command.ExecuteScalar()

                If ((obj Is Nothing) OrElse TypeOf obj Is DBNull) Then
                    Throw New DataNotFoundException
                End If
                str = CStr(obj)

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            Return str

        End Function
#End Region

#Region " GetMinYear： "
        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMinYear() As Integer
            Dim num As Integer
            Try
                Dim obj2 As Object = New NpgsqlCommand(("SELECT DISTINCT(MIN(TO_CHAR(d_years,'yyyy'))) FROM " & Me.TableName), MyBase.GetNpgsqlConnection).ExecuteScalar
                If ((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull) Then
                    Throw New DataNotFoundException
                End If
                num = Integer.Parse(CStr(obj2))
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0004", New String(0 - 1) {})
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
            Return num
        End Function
#End Region

#Region " IsTargetYearsExists： "
        ''' <summary></summary>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="OnceName">一時名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsTargetYearsExists( _
            ByVal TargetYM As String, _
            Optional ByVal OnceName As String = "" _
        ) As Boolean

            Dim flag As Boolean

            Try
                Dim obj2 As Object
                If OnceName = "" Then
                    obj2 = New NpgsqlCommand(String.Concat(New String() {"SELECT COUNT(*) FROM ", Me.TableName, " WHERE TO_CHAR(d_years, 'yyyyMM') = '", TargetYM, "'"}), MyBase.GetNpgsqlConnection).ExecuteScalar
                Else
                    obj2 = New NpgsqlCommand(String.Concat(New String() {"SELECT COUNT(*) FROM ", Me.TableName, " WHERE TO_CHAR(d_years, 'yyyyMM') = '", TargetYM, "'", " AND c_pay_once_name='", OnceName, "'"}), MyBase.GetNpgsqlConnection).ExecuteScalar
                End If
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
#End Region

#Region " Save：新規登録処理 "
        ''' <summary>新規登録処理</summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="data">登録データ</param>
        ''' <param name="register">登録者ID</param>
        ''' <param name="AutoCommit">自動コミット（True：, False：）</param>
        ''' <remarks></remarks>
        Public Sub Save( _
            ByVal command As NpgsqlCommand, _
            ByVal data As DataTable, _
            ByVal register As String, _
            ByVal AutoCommit As Boolean _
        )

            ' SQL
            Dim sql As String = ""
            sql += "INSERT INTO " & Me.TableName & " ("
            sql += "    c_user_id"              ' 01. 個人認証ＩＤ
            sql += "   ,d_years"                ' 02. 対象年月
            sql += "   ,s_pay_cut"              ' 03. 賃金内カット金額
            sql += "   ,c_pay_once_name"        ' 04. 一時金名称
            sql += "   ,d_ins"                  ' 05. 作成日
            sql += "   ,c_user_id_ins"          ' 06. 作成者個人ＩＤ
            sql += ") VALUES ("
            sql += "    :c_user_id"
            sql += "   ,CONVERT(DATE,:d_years,112)"
            sql += "   ,:s_pay_cut"
            sql += "   ,:c_pay_once_name"
            sql += "   ,GETDATE()"
            sql += "   ,:c_user_ins"
            sql += ")"

            Try
                ' ログ出力
                WageReductionBaseDao._logger.Debug(sql)
                For Each row As DataRow In data.Rows
                    If row.Item(4) <> 0 Then
                        ' SQL設定
                        command.SetSql(sql)

                        ' バインド変数定義
                        command.Parameters.Clear()
                        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))         ' 01. 個人認証ID
                        command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))           ' 02. 対象年月
                        command.Parameters.Add(New NpgsqlParameter("s_pay_cut", DbType.Int32))          ' 03. 賃金内カット金額
                        command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))   ' 04. 一時金名称
                        command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))        ' 05. 作成者個人ID

                        ' バインド変数設定
                        command.Parameters.Item("c_user_id").Value = row.Item(0).ToString               ' 01. 社員番号
                        command.Parameters.Item("d_years").Value = row.Item(1)                          ' 02. 対象年月
                        command.Parameters.Item("s_pay_cut").Value = row.Item(2)                        ' 03. 控除額
                        command.Parameters.Item("c_pay_once_name").Value = row.Item(3)                  ' 04. 一時金名称
                        command.Parameters.Item("c_user_ins").Value = register                          ' 05. 登録者ID

                        ' SQL実行
                        command.ExecuteNonQuery()
                    End If
                Next
                'If AutoCommit Then
                '    objTran.Commit()
                'End If

            Catch exception As NpgsqlException
                'If AutoCommit Then
                '    objTran.Rollback()
                'End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})

            Catch exception2 As Exception
                'If AutoCommit Then
                '    objTran.Rollback()
                'End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

        End Sub
#End Region

#Region " Save2： "
        ''' <summary></summary>
        ''' <param name="command">コマンドオブジェクト</param>
        ''' <param name="data">登録更新削除データ</param>
        ''' <param name="register">登録者</param>
        ''' <param name="AutoCommit">自動コミット（True：, False：）</param>
        ''' <remarks></remarks>
        Public Sub Save2( _
            ByVal command As NpgsqlCommand, _
            ByVal data As DataTable, _
            ByVal register As String, _
            ByVal AutoCommit As Boolean _
        )
            Dim sqlIns As String = ("INSERT INTO " & Me.TableName & " (c_user_id, d_years, s_pay_cut, c_pay_once_name, d_ins, c_user_id_ins) VALUES (:c_user_id, CONVERT(DATE,:d_years,112), :s_pay_cut, :c_pay_once_name, GETDATE, :c_user_ins)")
            Dim sqlUpd As String = ("UPDATE " & Me.TableName & " SET s_pay_cut=:s_pay_cut, d_ins=GETDATE(), c_user_id_ins=:c_user_ins WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years,112) AND c_pay_once_name=:c_pay_once_name")
            Dim sqlDel As String = ("DELETE FROM " & Me.TableName & " WHERE c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years,112) AND c_pay_once_name=:c_pay_once_name")

            Try
                WageReductionBaseDao._logger.Debug(sqlUpd)
                Dim row As DataRow
                For Each row In data.Rows
                    command.SetSql(sqlUpd)
                    command.Parameters.Clear()
                    command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("s_pay_cut", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                    command.Parameters.Item("c_user_id").Value = row.Item(0).ToString
                    command.Parameters.Item("d_years").Value = row.Item(1)
                    command.Parameters.Item("s_pay_cut").Value = row.Item(2)
                    command.Parameters.Item("c_pay_once_name").Value = row.Item(3)
                    command.Parameters.Item("c_user_ins").Value = register
                    If command.ExecuteNonQuery() <> 1 Then
                        If row.Item(4) <> 0 Then
                            WageReductionBaseDao._logger.Debug(sqlIns)
                            command.SetSql(sqlIns)
                            command.Parameters.Clear()
                            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                            command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                            command.Parameters.Add(New NpgsqlParameter("s_pay_cut", DbType.Int32))
                            command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                            command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                            command.Parameters.Item("c_user_id").Value = row.Item(0).ToString
                            command.Parameters.Item("d_years").Value = row.Item(1)
                            command.Parameters.Item("s_pay_cut").Value = row.Item(2)
                            command.Parameters.Item("c_pay_once_name").Value = row.Item(3)
                            command.Parameters.Item("c_user_ins").Value = register
                            command.ExecuteNonQuery()
                        End If
                    ElseIf row.Item(4) = 0 Then
                        WageReductionBaseDao._logger.Debug(sqlDel)
                        command.SetSql(sqlDel)
                        command.Parameters.Clear()
                        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                        command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                        command.Parameters.Add(New NpgsqlParameter("c_pay_once_name", DbType.String))
                        command.Parameters.Item("c_user_id").Value = row.Item(0).ToString
                        command.Parameters.Item("d_years").Value = row.Item(1)
                        command.Parameters.Item("c_pay_once_name").Value = row.Item(3)
                        command.ExecuteNonQuery()
                    End If
                Next
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0 - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
        End Sub
#End Region

    End Class
End Namespace
