'===========================================================================================================
'   クラスＩＤ　　：WageReductionBonusCommand
'   クラス名称　　：賃金カット一時金コマンドクラス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.DAO

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports log4net
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.DAO.FinancialAffairs.WageReduction

Namespace Business.FinancialAffairs.WageReduction
    Friend Class WageReductionBonusCommand
        Inherits WageReductionBase
        'Implements IWageReductionBonusCommand

#Region " GetStafInfoBounus：社員情報取得処理（一時金） "
        ''' <summary>社員情報取得処理（一時金）</summary>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="c_staf_id_list">社員番号リスト</param>
        ''' <returns>Excelデータ</returns>
        ''' <remarks></remarks>
        Public Function GetStafInfoBounus( _
            ByVal TargetYM As String, _
            ByVal c_staf_id_list As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                ' Excelデータ取得処理
                tbl = (New BonusBaseDao).GetStafInfoBounus( _
                    PublicCommand.GetKsh, _
                    TargetYM, _
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

#Region " GetData：一時金：照会データ取得処理 "
        ''' <summary>一時金：照会データ取得処理</summary>
        ''' <param name="TargetYM">対象年</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns>一時金：照会データ</returns>
        ''' <remarks></remarks>
        Public Function GetData( _
            ByVal TargetYM As String, _
            ByVal strBonusName As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                ' 一時金：照会データ取得処理
                'table = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusInTimeDao"), BonusBaseDao).GetTable(PublicCommand.GetKsh, TargetYM, (TargetYM & "01"))
                tbl = (New BonusInTimeDao).GetTable( _
                    PublicCommand.GetKsh, _
                    TargetYM, _
                    (TargetYM & "01"), _
                    strBonusName _
                )

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return tbl

        End Function
#End Region

#Region " GetListDetail： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetListDetail( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            ByVal strBonusName As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing
            Dim truncPlace As Integer = 0

            Try
                truncPlace = MyBase.GetTruncPlace((TargetYear & TargetMonth & "01"))
                'table = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusBaseDao"), BonusBaseDao).GetPrintDetailListData(PublicCommand.GetKsh, (TargetYear & TargetMonth), truncPlace, (TargetYear & TargetMonth & "01"))
                tbl = (New BonusBaseDao).GetPrintDetailListData( _
                    PublicCommand.GetKsh, _
                    (TargetYear & TargetMonth), _
                    truncPlace, _
                    (TargetYear & TargetMonth & "01"), _
                    strBonusName _
                )

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
                tbl.Columns.Add("s_break", GetType(Integer))
                row = tbl.NewRow
                row.Item("l_year") = TargetYear
                row.Item("l_month") = TargetMonth
                row.Item("s_break") = MyBase.GetTruncateAmount((TargetYear & TargetMonth & "01"))
                tbl.Rows.Add(row)

            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " GetListPrintData： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListPrintData( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            Optional ByVal strBonusName As String = "" _
        ) As DataSet

            Dim set2 As DataSet
            Dim ds As New DataSet("dsWageCutCoverLotal")

            Try
                Dim listHeader As DataTable = Me.GetListHeader(TargetYear, TargetMonth)
                ds.Tables.Add(listHeader)
                listHeader = Me.GetListDetail(TargetYear, TargetMonth, strBonusName)
                ds.Tables.Add(listHeader)
                set2 = ds

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return set2

        End Function
#End Region

#Region " GetMaxYM： "
        ''' <summary></summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMaxYM() As String

            Dim maxYears As String

            Try
                'maxYears = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusInTimeDao"), BonusBaseDao).GetMaxYears
                maxYears = (New BonusInTimeDao).GetMaxYears

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
                Dim minYear As Integer = 0
                Dim num2 As Integer = 0
                Try
                    minYear = (New BonusInTimeDao).GetMinYear
                Catch exception1 As DataNotFoundException
                    minYear = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
                End Try
                Try
                    'num2 = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusStrikeDao"), BonusBaseDao).GetMinYear
                    num2 = (New BonusStrikeDao).GetMinYear
                Catch exception3 As DataNotFoundException
                    num2 = Integer.Parse(PublicCommand.GetSystemDate.Substring(0, 4))
                End Try
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

#Region " GetSummaryDetail： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <param name="strBonusName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSummary( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String, _
            Optional ByVal strBonusName As String = "" _
        ) As DataTable

            Dim table2 As DataTable

            Try
                Dim truncPlace As Integer = MyBase.GetTruncPlace((TargetYear & TargetMonth & "01"))
                'Dim table As DataTable = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusBaseDao"), BonusBaseDao).FindSummury(PublicCommand.GetKsh, (TargetYear & TargetMonth), (TargetYear & TargetMonth & "01"), truncPlace, strBonusName)
                Dim table As DataTable = (New BonusBaseDao).FindSummury(PublicCommand.GetKsh, (TargetYear & TargetMonth), (TargetYear & TargetMonth & "01"), truncPlace, strBonusName)
                If (table.Rows.Count > 0) Then
                    Dim numArray As Long() = New Long(table.Columns.Count - 1) {}
                    Dim row As DataRow
                    For Each row In table.Rows
                        Dim j As Integer
                        For j = 1 To table.Columns.Count - 1
                            numArray(j) = (numArray(j) + CLng(row.Item(j)))
                        Next j
                    Next
                    Dim row2 As DataRow = table.NewRow
                    row2.Item(0) = "合計"
                    Dim i As Integer
                    For i = 1 To table.Columns.Count - 1
                        row2.Item(i) = numArray(i)
                    Next i
                    table.Rows.Add(row2)
                End If
                table2 = table

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})

            End Try

            Return table2

        End Function
#End Region

#Region " GetSummaryDetail： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetSummaryDetail( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataTable

            Dim printSummaryDetailData As DataTable

            Try
                Dim truncPlace As Integer = MyBase.GetTruncPlace((TargetYear & TargetMonth & "01"))
                printSummaryDetailData = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusBaseDao"), BonusBaseDao).GetPrintSummaryDetailData((TargetYear & TargetMonth), truncPlace)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return printSummaryDetailData

        End Function
#End Region

#Region " GetSummaryPrintData： "
        ''' <summary></summary>
        ''' <param name="TargetYear">対象年</param>
        ''' <param name="TargetMonth">対象月</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSummaryPrintData( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As DataSet

            Dim set2 As DataSet
            Dim ds As New DataSet("dsWageCutCoverTotal")

            Try
                Dim listHeader As DataTable = Me.GetListHeader(TargetYear, TargetMonth)
                ds.Tables.Add(listHeader)
                listHeader = Me.GetSummaryDetail(TargetYear, TargetMonth)
                ds.Tables.Add(listHeader)
                set2 = ds

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return set2

        End Function
#End Region

#Region " IsTargetYearsExists： "
        ''' <summary></summary>
        ''' <param name="TargetYM">対象年月</param>
        ''' <param name="OnceName">一時金名称</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsTargetYearsExists( _
            ByVal TargetYM As String, _
            Optional ByVal OnceName As String = "" _
        ) As Boolean

            Dim flag As Boolean

            Try
                flag = (New BonusInTimeDao).IsTargetYearsExists(TargetYM, OnceName)

            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

            Return flag

        End Function
#End Region

#Region " SaveData：新規登録処理 "
        ''' <summary>新規登録処理</summary>
        ''' <param name="SaveData">登録データ</param>
        ''' <param name="Register"></param>
        ''' <remarks></remarks>
        Public Sub SaveData( _
            ByVal SaveData As DataSet, _
            ByVal Register As String _
        )

            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)

            Try
                'DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusInTimeDao"), BonusBaseDao).Save(command, SaveData.Tables.Item("pay_time_cut_once"), Register, False)
                'DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusStrikeDao"), BonusBaseDao).Save(command, SaveData.Tables.Item("pay_strike_cut_once"), Register, False)
                Dim dao As New BonusInTimeDao
                Dim dao2 As New BonusStrikeDao
                dao.Save(command, SaveData.Tables.Item("pay_time_cut_once"), Register, False)
                dao2.Save(command, SaveData.Tables.Item("pay_strike_cut_once"), Register, False)
                objTran.Commit()

            Catch exception As SysUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception

            End Try

        End Sub
#End Region

#Region " UpdateData：内容変更（更新）処理 "
        ''' <summary>内容変更（更新）処理</summary>
        ''' <param name="TargetYM">対象年</param>
        ''' <param name="SaveData">対象月</param>
        ''' <param name="Register"></param>
        ''' <remarks></remarks>
        Public Sub UpdateData( _
            ByVal TargetYM As String, _
            ByVal SaveData As DataSet, _
            ByVal Register As String _
        )

            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)

            Try
                'Dim dao As BonusBaseDao = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusInTimeDao"), BonusBaseDao)
                'Dim dao2 As BonusBaseDao = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.BonusStrikeDao"), BonusBaseDao)
                Dim dao As New BonusInTimeDao
                Dim dao2 As New BonusStrikeDao
                'WageReductionBonusCommand._logger.Debug(("時間内削除件数：" & dao.Delete(command, TargetYM, False).ToString))  ' MOD 2012/06/15 
                'WageReductionBonusCommand._logger.Debug(("時間内削除件数：" & dao2.Delete(command, TargetYM, False).ToString)) ' MOD 2012/06/15
                If (Not SaveData Is Nothing) Then
                    dao.Save2(command, SaveData.Tables.Item("pay_time_cut_once"), Register, False)    ' MOD 2012/06/15
                    dao2.Save2(command, SaveData.Tables.Item("pay_strike_cut_once"), Register, False) ' MOD 2012/06/15
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
        Private Const WAGE_REDUCTION_CLASS As String = "一時金"
    End Class
End Namespace
