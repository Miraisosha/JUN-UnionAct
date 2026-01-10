Imports System
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace Business.Common
    Public Class FactoryBusClass
        ' Methods
        Public Function GetObject(ByVal strClassName As String) As Object
            Dim obj3 As Object
            Try
                Dim obj2 As Object = Nothing
                Try
                    obj2 = Activator.CreateInstance(Type.GetType(strClassName))
                Catch exception As TargetInvocationException
                    Throw exception.InnerException
                End Try
                obj3 = obj2
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return obj3
        End Function

        Public Function GetObject(ByVal strClassName As String, ByVal ParamArray args As Object()) As Object
            Dim obj3 As Object
            Try
                Dim obj2 As Object = Nothing
                Try
                    obj2 = Activator.CreateInstance(Type.GetType(strClassName), args)
                Catch exception As TargetInvocationException
                    Throw exception.InnerException
                End Try
                obj3 = obj2
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "BE0001", New String(0 - 1) {})
            End Try
            Return obj3
        End Function

    End Class
End Namespace
