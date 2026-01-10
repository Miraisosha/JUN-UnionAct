'===========================================================================================================
'   クラスＩＤ　　：WageReductionBase
'   クラス名称　　：賃金カットベースクラス
'   備考  　　　　：
'===========================================================================================================
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.DAO.FinancialAffairs.WageReduction

Namespace Business.FinancialAffairs.WageReduction
    Public MustInherit Class WageReductionBase
        Inherits FinancialAffairsBase

#Region " New：コンストラクタ "
        ''' <summary>コンストラクタ</summary>
        ''' <remarks></remarks>
        Protected Sub New()
        End Sub
#End Region

#Region " FindMember：社員情報取得処理 "
        ''' <summary>社員情報取得処理</summary>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns>社員情報</returns>
        ''' <remarks></remarks>
        Public Function FindMember( _
            ByVal EmployeeNumber As String, _
            ByVal TargetYM As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                'table = DirectCast(MyBase._factory.GetObject("DAO.FinancialAffairs.WageReduction.MonthlyInTimeDao"), WageReductionBaseDao).FindMember(PublicCommand.GetKsh, EmployeeNumber, (TargetYM & "01"))
                tbl = (New MonthlyInTimeDao).FindMember( _
                    PublicCommand.GetKsh, _
                    EmployeeNumber, _
                    (TargetYM & "01") _
                )

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " FindMemberList：社員情報取得処理（Excelデータ取込） "
        ''' <summary>社員情報取得処理（Excelデータ取込）</summary>
        ''' <param name="EmployeeNumberList">社員番号リスト</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns>社員情報</returns>
        ''' <remarks></remarks>
        Public Function FindMemberList( _
            ByVal EmployeeNumberList As String, _
            ByVal TargetYM As String _
        ) As DataTable

            Dim tbl As DataTable = Nothing

            Try
                tbl = (New MonthlyInTimeDao).FindMemberList( _
                    PublicCommand.GetKsh, _
                    EmployeeNumberList, _
                    (TargetYM & "01") _
                )

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            Return tbl

        End Function
#End Region

#Region " FindExistMember：社員番号存在チェック処理 "
        ''' <summary>社員番号存在チェック処理</summary>
        ''' <param name="EmployeeNumber">社員番号</param>
        ''' <param name="TargetYM">対象年月</param>
        ''' <returns>True：存在する, False：存在しない</returns>
        ''' <remarks></remarks>
        Public Function FindExistMember( _
            ByVal EmployeeNumber As String, _
            ByVal TargetYM As String _
        ) As Boolean

            ' 処理結果（True：存在する, False：存在しない）
            Dim ExistFlg As Boolean = False

            Try
                ExistFlg = (New MonthlyInTimeDao).FindExistMember( _
                    PublicCommand.GetKsh, _
                    EmployeeNumber, _
                    (TargetYM & "01") _
                )

            Catch exception As DataNotFoundException
                Throw exception

            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2

            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})

            End Try

            Return ExistFlg

        End Function
#End Region

        ' Fields
        Protected Const COLUMN_NAME_MONTH As String = "l_month"
        Protected Const COLUMN_NAME_TITLE As String = "l_title"
        Protected Const COLUMN_NAME_TRUNCATE As String = "s_break"
        Protected Const COLUMN_NAME_YEAR As String = "l_year"
        Protected Const DATASET_NAME_LIST As String = "dsWageCutCoverLotal"
        Protected Const DATASET_NAME_TOTAL As String = "dsWageCutCoverTotal"
        Protected Const TOTAL_CELL_VALUE As String = "合計"

    End Class
End Namespace
