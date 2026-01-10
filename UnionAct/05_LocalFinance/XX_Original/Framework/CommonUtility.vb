Imports System
Imports System.Globalization
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace Framework
    Public Class CommonUtility
        ' Methods
        Public Shared Function GetLastDay(ByVal TargetYM As String) As String
            Dim str3 As String
            Try
                Dim s As String = TargetYM.Substring(0, 4)
                Dim str2 As String = TargetYM.Substring(4, 2)
                Dim daysInMonth As Integer = New JapaneseCalendar().GetDaysInMonth(Integer.Parse(s), Integer.Parse(str2))
                str3 = (s & str2 & daysInMonth.ToString("d2"))
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})
            End Try
            Return str3
        End Function

        Public Shared Function GetLastDay(ByVal TargetYear As String, ByVal TargetMonth As String) As String
            Dim str As String
            Try
                Dim daysInMonth As Integer = New JapaneseCalendar().GetDaysInMonth(Integer.Parse(TargetYear), Integer.Parse(TargetMonth))
                str = (TargetYear & TargetMonth & daysInMonth.ToString("d2"))
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0 - 1) {})
            End Try
            Return str
        End Function

    End Class
End Namespace
