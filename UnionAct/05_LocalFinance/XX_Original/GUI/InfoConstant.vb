Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.DAO.Master
Imports UnionAct.Business.Master

Namespace GUI.Common
    Public Class InfoConstant
        Inherits MasterBase
        ' Methods
        Public Sub New(ByVal basisDate As String)
            Try
                MyBase.strPKColumnName1 = "c_constant"
                MyBase.strPKColumnName2 = "c_constant_seq"
                MyBase.strTargetTableName = "constant_dtl"
                MyBase.strWaColumnName = "l_name"
                MyBase.strCodeColumnName = "c_constant_seq"
                'Dim class2 As New FactoryBusClass
                'MyBase.dSetMaster = DirectCast(class2.GetObject("Business.Master.ConstantTblCommand"), IConstantTblCommand).GetConstantTbl(basisDate)
                Dim command As New ConstantTblCommand
                MyBase.dSetMaster = command.GetConstantTbl(basisDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Function GetCodeByName(ByVal strWaColumnData As String) As String
            Dim convertedData As String
            Try
                Dim strFilter As String = (MyBase.strWaColumnName & " = '" & strWaColumnData & "'")
                convertedData = MyBase.GetConvertedData(strFilter, MyBase.strCodeColumnName)
            Catch exception As AppUnionException
                Throw exception
            Catch exception2 As SysUnionException
                Throw exception2
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0001", New String(0 - 1) {})
            End Try
            Return convertedData
        End Function

        Public Overrides Function GetListDataTable(ByVal strData As String, ByVal strSearchColumn As String) As DataTable
            Dim table3 As DataTable
            Try
                Dim listDataTable As DataTable = MyBase.GetListDataTable(strData, strSearchColumn)
                Dim table2 As DataTable = listDataTable.Clone
                Dim view As New DataView(listDataTable) With { _
                    .Sort = "s_order" _
                }
                Dim view2 As DataRowView
                For Each view2 In view
                    table2.ImportRow(view2.Row)
                Next
                table3 = table2
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Public Function GetShortName2ByCode(ByVal headerKey As String, ByVal detailKey As String) As String
            Dim convertedData As String
            Try
                Dim strFilter As String = String.Concat(New String() {"(", MyBase.strPKColumnName1, " = '", headerKey, "') and (", MyBase.strPKColumnName2, " = '", detailKey, "')"})
                convertedData = MyBase.GetConvertedData(strFilter, "l_omission_name_2")
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return convertedData
        End Function

        Public Function GetShortNameByCode(ByVal headerKey As String, ByVal detailKey As String) As String
            Dim convertedData As String
            Try
                Dim strFilter As String = String.Concat(New String() {"(", MyBase.strPKColumnName1, " = '", headerKey, "') and (", MyBase.strPKColumnName2, " = '", detailKey, "')"})
                convertedData = MyBase.GetConvertedData(strFilter, "l_omission_name")
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return convertedData
        End Function

    End Class
End Namespace
