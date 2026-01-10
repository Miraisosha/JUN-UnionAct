'===========================================================================================================
'   クラスＩＤ　　：WageReductionMonthlyCommand
'   クラス名称　　：賃金カット月例コマンドクラス
'   備考  　　　　：
'===========================================================================================================

'Imports DAO.FinancialAffairs.WageReduction
'Imports Framework
'Imports Framework.Command
'Imports Framework.UnionException
Imports log4net
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionConst
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.FinancialAffairs.WageReduction
Imports UnionAct.NpgsqlDummy
Imports UnionAct.DAO

Namespace Business.FinancialAffairs.WageReduction
    Public Class WageReductionMonthlyCommand
        Inherits WageReductionBase
        'Implements IWageReductionMonthlyCommand

#Region " GetDao：月例：時間内・争議行為DAO取得処理 "
        ''' <summary>月例：時間内・争議行為DAO取得処理</summary>
        ''' <param name="kind">種別</param>
        ''' <returns>
        ''' 月例：時間内照会データ取得DAO
        ''' 月例：争議行為照会データ取得DAO
        ''' </returns>
        ''' <remarks>
        ''' 種別：0・1 の場合「月例：時間内照会データ取得DAO」を返す
        ''' 種別：2    の場合「月例：争議行為照会データ取得DAO」を返す
        ''' </remarks>
        Private Function GetDao(ByVal kind As WAGE_REDUCTION_KIND) As MonthlyBaseDao

            ' 種別判定
            If (kind = WAGE_REDUCTION_KIND.BOTH) _
            OrElse (kind = WAGE_REDUCTION_KIND.IN_TIME) Then
                Return New MonthlyInTimeDao     ' 時間内：照会データ取得DAO
            Else
                Return New MonthlyStrikeDao     ' 争議行為：照会データ取得DAO
            End If

        End Function
#End Region

#Region " GetStafInfoInTimeStrike：社員情報取得処理（時間内・争議行為） "
        ''' <summary>社員情報取得処理（時間内・争議行為）</summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns>Excelデータ</returns>
        ''' <remarks></remarks>
        Public Function GetStafInfoInTimeStrike( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String, _
            ByVal c_staf_id_list As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                ' 社員情報取得処理（時間内・争議行為）
                tbl = Me.GetDao(kind).GetStafInfoInTimeStrike( _
                    PublicCommand.GetKsh, _
                    (TargetYM & "01"), _
                    c_staf_id_list _
                )

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return tbl

        End Function
#End Region

#Region " GetData：照会データ取得処理 "
        ''' <summary>照会データ取得処理</summary>
        ''' <param name="kind"></param>
        ''' <param name="TargetYM"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetData( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                ' 照会データ取得
                tbl = Me.GetDao(kind).GetTable( _
                    PublicCommand.GetKsh, _
                    TargetYM, _
                    (TargetYM & "01") _
                )

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return tbl

        End Function
#End Region

#Region " GetInTimeSummary： "
        ''' <summary></summary>
        ''' <param name="targetYM">対象年月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetInTimeSummary(ByVal targetYM As String) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                tbl = Me.GetSummary(targetYM, Me.GetDao(WAGE_REDUCTION_KIND.IN_TIME))

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetListDetail： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetListDetail( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                Dim truncPlace As Integer = MyBase.GetTruncPlace((TargetYM & "01"))
                Dim dao As MonthlyBaseDao = Me.GetDao(kind)

                Select Case kind
                    Case WAGE_REDUCTION_KIND.BOTH
                        tbl = dao.GetPrintDetailData(PublicCommand.GetKsh, TargetYM, truncPlace, (TargetYM & "01"))
                        Exit Select
                    Case WAGE_REDUCTION_KIND.IN_TIME
                        tbl = DirectCast(dao, MonthlyInTimeDao).GetPrintDetailData(PublicCommand.GetKsh, TargetYM, truncPlace, (TargetYM & "01"))
                        Exit Select
                    Case WAGE_REDUCTION_KIND.STRIKE
                        tbl = DirectCast(dao, MonthlyStrikeDao).GetPrintDetailData(PublicCommand.GetKsh, TargetYM, truncPlace, (TargetYM & "01"))
                        Exit Select
                End Select

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return tbl

        End Function
#End Region

#Region " GetListHeader： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetListHeader( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim row As DataRow = Nothing

            Try
                tbl = New DataTable("dtHeader")
                tbl.Columns.Add("l_year", GetType(String))
                tbl.Columns.Add("l_month", GetType(String))
                tbl.Columns.Add("l_title", GetType(String))
                row = tbl.NewRow
                row.Item("l_year") = TargetYear
                row.Item("l_month") = TargetMonth
                row.Item("l_title") = "月例"
                tbl.Rows.Add(row)

            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetListPrintData： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListPrintData( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataSet

            Dim ds As New DataSet("dsWageCutCoverLotal")

            Try
                Dim listHeader As DataTable = Me.GetListHeader(TargetYear, TargetMonth)
                ds.Tables.Add(listHeader)
                listHeader = Me.GetListDetail(kind, (TargetYear & TargetMonth))
                ds.Tables.Add(listHeader)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return ds

        End Function
#End Region

#Region " GetMaxYM： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxYM(ByVal kind As WAGE_REDUCTION_KIND) As String

            Dim maxYears As String

            Try
                maxYears = Me.GetDao(kind).GetMaxYears

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            End Try

            Return maxYears

        End Function
#End Region

#Region " GetMinYear： "
        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMinYear() As Integer

            Dim num3 As Integer

            Try
                Dim minYear As Integer = Me.GetMinYear(WAGE_REDUCTION_KIND.IN_TIME)
                Dim num2 As Integer = Me.GetMinYear(WAGE_REDUCTION_KIND.STRIKE)
                num3 = If((minYear >= num2), num2, minYear)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return num3

        End Function
#End Region

#Region " GetMinYear： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMinYear(ByVal kind As WAGE_REDUCTION_KIND) As Integer

            Dim num2 As Integer

            Try
                Dim minYear As Integer = 0
                Try
                    minYear = Me.GetDao(kind).GetMinYear
                Catch exception1 As DataNotFoundException
                    minYear = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
                End Try
                num2 = minYear

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return num2

        End Function
#End Region

#Region " GetStrikeSummary： "
        ''' <summary></summary>
        ''' <param name="targetYM">対象年月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStrikeSummary(ByVal targetYM As String) As DataTable

            Dim summary As DataTable

            Try
                summary = Me.GetSummary(targetYM, Me.GetDao(WAGE_REDUCTION_KIND.STRIKE))

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return summary

        End Function
#End Region

#Region " GetSummary： "
        ''' <summary></summary>
        ''' <param name="targetYM">対象年月</param>
        ''' <param name="dao">DAOオブジェクト</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetSummary( _
            ByVal targetYM As String, _
            ByVal dao As MonthlyBaseDao _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim truncPlace As Integer = 0

            Try
                ' 切捨て桁数取得
                truncPlace = MyBase.GetTruncPlace((targetYM & "01"))

                ' ログ出力
                WageReductionMonthlyCommand._logger.Debug(("切捨て位置：" & truncPlace))

                tbl = dao.FindSummury(PublicCommand.GetKsh, targetYM, (targetYM & "01"), truncPlace)
                If (tbl.Rows.Count > 0) Then
                    Dim num2 As Integer = 0
                    Dim num3 As Long = 0
                    Dim num4 As Long = 0
                    Dim num5 As Integer = 0
                    Dim row As DataRow
                    For Each row In tbl.Rows
                        num2 = (num2 + CInt(row.Item(1)))
                        num3 = (num3 + CLng(row.Item(2)))
                        num4 = (num4 + CLng(row.Item(3)))
                        num5 = (num5 + CInt(row.Item(4)))
                    Next
                    Dim row2 As DataRow = tbl.NewRow
                    row2.Item(0) = "合計"
                    row2.Item(1) = num2
                    row2.Item(2) = num3
                    row2.Item(3) = num4
                    row2.Item(4) = num5
                    tbl.Rows.Add(row2)
                End If

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetSummaryDetail： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetSummaryDetail( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim truncPlace As Integer = 0

            Try
                ' 切捨て桁数取得
                truncPlace = MyBase.GetTruncPlace((TargetYM & "01"))
                tbl = Me.GetDao(kind).GetPrintSummaryDetailData(TargetYM, truncPlace)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return tbl

        End Function
#End Region

#Region " GetSummaryPrintData： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSummaryPrintData( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataSet

            Dim ds As New DataSet("dsWageCutCoverTotal")

            Try
                Dim listHeader As DataTable = Me.GetListHeader(TargetYear, TargetMonth)
                ds.Tables.Add(listHeader)
                listHeader = Me.GetSummaryDetail(kind, (TargetYear & TargetMonth))
                ds.Tables.Add(listHeader)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try

            Return ds

        End Function
#End Region

#Region " IsTargetYearsExists： "
        ''' <summary></summary>
        ''' <param name="kind">種別</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsTargetYearsExists( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String _
        ) As Boolean

            Dim flag As Boolean
            Try
                flag = Me.GetDao(kind).IsTargetYearsExists(TargetYM)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try

            Return flag

        End Function
#End Region

#Region " SaveData：新規登録処理 "
        ''' <summary>新規登録処理</summary>
        ''' <param name="kind">種別</param>
        ''' <param name="SaveData">登録データ</param>
        ''' <param name="register">登録者</param>
        ''' <remarks></remarks>
        Public Sub SaveData( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal SaveData As DataTable, _
            ByVal register As String _
        )
            Try
                Me.GetDao(kind).Save( _
                    New NpgsqlCommand("", CommonDaoClass.connNpgsql), _
                    SaveData, _
                    register, _
                    True _
                )

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

        End Sub
#End Region

#Region " UpdateData：内容変更（更新）処理 "
        ''' <summary>内容変更（更新）処理</summary>
        ''' <param name="kind"></param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="SaveData">更新データ</param>
        ''' <param name="register"></param>
        ''' <remarks></remarks>
        Public Sub UpdateData( _
            ByVal kind As WAGE_REDUCTION_KIND, _
            ByVal TargetYM As String, _
            ByVal SaveData As DataTable, _
            ByVal register As String _
        )

            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)

            Try
                WageReductionMonthlyCommand._logger.Debug(("削除件数：" & Me.GetDao(kind).Delete(command, TargetYM, True).ToString))
                If ((Not SaveData Is Nothing) AndAlso (SaveData.Rows.Count > 0)) Then
                    Me.GetDao(kind).Save(command, SaveData, register, True)
                End If
                objTran.Commit()

            Catch exception As SysUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try

        End Sub
#End Region

        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private Const WAGE_REDUCTION_CLASS As String = "月例"

        ' Nested Types
        Private Enum COLUMN_INDEX_MONTHLY
            ' Fields
            BRANCH = 0
            PERSONS = 1
            WAGE_REDUCTION = 2
            COVER = 3
            DUES = 4
        End Enum
    End Class
End Namespace
