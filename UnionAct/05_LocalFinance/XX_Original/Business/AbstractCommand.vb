Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace Business.Common
    Public MustInherit Class AbstractCommand
        ' Methods
        Protected Sub New()
        End Sub

        Public Sub CheckTimeStamp(ByVal strDB As String, ByVal strTarget As String)
            Try
                If Not strDB.Equals(strTarget) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "BE0005", New String(0 - 1) {})
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub CheckTimeStamp(ByVal dSetDB As DataSet, ByVal dSetTarget As DataSet, ByVal strTableName As String)
            Try
                Dim i As Integer
                For i = 0 To dSetTarget.Tables.Item(strTableName).Rows.Count - 1
                    Me.CheckTimeStamp(dSetDB.Tables.Item(strTableName).Rows.Item(i).Item("d_up").ToString, dSetTarget.Tables.Item(strTableName).Rows.Item(i).Item("d_up").ToString)
                Next i
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        'Protected Function GetDaoFactory() As FactoryDaoClass
        '    Dim class2 As FactoryDaoClass
        '    Try
        '        class2 = New FactoryDaoClass
        '    Catch exception As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "BE0001", New String(0 - 1) {})
        '    End Try
        '    Return class2
        'End Function

        'Protected Function GetDaoObject(Of TypeofDaoInterface As Class)(ByVal strClassName As String) As TypeofDaoInterface
        '    Dim local As TypeofDaoInterface
        '    Try
        '        local = TryCast(Me.GetDaoFactory.GetObject(strClassName), TypeofDaoInterface)
        '    Catch exception As BaseUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    End Try
        '    Return local
        'End Function

        'Public Sub SetConnectionString(ByVal strConn As String)
        '    CommonDataClass.Instance.strConStr = strConn
        'End Sub

        'Public Sub SetConnectionTraining(ByVal strConn As String)
        '    CommonDataClass.Instance.strConStrTraining = strConn
        'End Sub

        'Public Sub SetKsh(ByVal strKsh As String)
        '    CommonDataClass.Instance.strKsh = strKsh
        'End Sub

    End Class
End Namespace
