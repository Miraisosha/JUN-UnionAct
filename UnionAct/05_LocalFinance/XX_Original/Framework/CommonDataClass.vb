Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Threading
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException

Namespace Framework.Interface
    Public Class CommonDataClass
        ' Methods
        Private Sub New()
        End Sub

        Public Sub ExitAllProcess()
            Dim i As Integer
            For i = 0 To CommonDataClass.Instance.lstProcess.Count - 1
                If Not CommonDataClass.Instance.lstProcess.Item(i).HasExited Then
                    CommonDataClass.Instance.lstProcess.Item(i).Kill()
                End If
            Next i
        End Sub

        Public Sub Process_Exited(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Public Sub WaitForAppExit()
            Cursor.Current = Cursors.WaitCursor
            Try
                Dim flag As Boolean
                Do
                    Thread.Sleep(500)
                    flag = True
                    Dim i As Integer
                    For i = 0 To CommonDataClass.Instance.lstProcess.Count - 1
                        If Not CommonDataClass.Instance.lstProcess.Item(i).HasExited Then
                            flag = False
                            Exit For
                        End If
                    Next i
                Loop While Not flag
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End Sub


        ' Properties
        Public Shared ReadOnly Property Instance() As CommonDataClass
            Get
                Return CommonDataClass.this_instance
            End Get
        End Property


        ' Fields
        Private Shared this_instance As CommonDataClass = New CommonDataClass
        Public IsTrainingMode As Boolean
        Public lstProcess As List(Of Process) = New List(Of Process)
        Public mutexObj As Mutex
        Public strConStr As String
        Public strConStrTraining As String
        Public strCStaffCode As String
        Public strKsh As String
    End Class
End Namespace
