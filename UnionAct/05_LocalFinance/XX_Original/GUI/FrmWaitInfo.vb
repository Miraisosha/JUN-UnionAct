Imports log4net
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException

Namespace GUI.Document
    Public Class FrmWaitInfo
        Inherits Form
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Shared Sub CloseWaitForm()
            Try
                SyncLock FrmWaitInfo.syncObject
                    If ((Not FrmWaitInfo._frmWaitInfo Is Nothing) AndAlso Not FrmWaitInfo._frmWaitInfo.IsDisposed) Then
                        If FrmWaitInfo._frmWaitInfo.InvokeRequired Then
                            Try
                                FrmWaitInfo._frmWaitInfo.Invoke(New MethodInvoker(AddressOf FrmWaitInfo._frmWaitInfo.Close))
                            Catch exception As Exception
                                FrmWaitInfo.logger.Info(exception.Message)
                                'TODO Dim class2 As New FactoryBusClass
                                'TODO DirectCast(class2.GetObject("Business.Common.LogCommand"), ILogCommand).LogOutput("INFO", exception.Message)
                            End Try
                        Else
                            FrmWaitInfo._frmWaitInfo.Close()
                        End If
                    End If
                    If (Not FrmWaitInfo._frmMain Is Nothing) Then
                        FrmWaitInfo._frmMain.Activate()
                    End If
                    FrmWaitInfo._frmWaitInfo = Nothing
                    FrmWaitInfo._thread = Nothing
                    FrmWaitInfo._frmMain = Nothing
                End SyncLock
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.components = New Container
            Me.label1 = New Label
            Me.timerWait = New System.Windows.Forms.Timer(Me.components)
            MyBase.SuspendLayout()
            Me.label1.AutoSize = True
            Me.label1.Location = New Point(&H15, &H1B)
            Me.label1.Margin = New Padding(4, 0, 4, 0)
            Me.label1.Name = "label1"
            Me.label1.Size = New Size(150, &H10)
            Me.label1.TabIndex = 0
            Me.label1.Text = ChrW(12375) & ChrW(12400) & ChrW(12425) & ChrW(12367) & ChrW(12362) & ChrW(24453) & ChrW(12385) & ChrW(12367) & ChrW(12384) & ChrW(12373) & ChrW(12356) & ChrW(12290)
            Me.label1.UseWaitCursor = True
            Me.timerWait.Enabled = True
            Me.timerWait.Interval = 500
            AddHandler Me.timerWait.Tick, New EventHandler(AddressOf Me.timerWait_Tick)
            MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
            MyBase.AutoScaleMode = AutoScaleMode.Font
            MyBase.ClientSize = New Size(&HC1, 70)
            MyBase.ControlBox = False
            MyBase.Controls.Add(Me.label1)
            Me.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
            MyBase.Margin = New Padding(4)
            MyBase.MaximizeBox = False
            MyBase.MinimizeBox = False
            MyBase.Name = "FrmWaitInfo"
            MyBase.ShowInTaskbar = False
            MyBase.StartPosition = FormStartPosition.CenterScreen
            Me.Text = ChrW(20966) & ChrW(29702) & ChrW(20013) & ChrW(65294) & ChrW(65294) & ChrW(65294)
            MyBase.TopMost = True
            MyBase.UseWaitCursor = True
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout()
        End Sub

        Public Shared Sub ShowWaitForm(ByVal frm As Form)
            Try
                If ((FrmWaitInfo._frmWaitInfo Is Nothing) AndAlso (FrmWaitInfo._thread Is Nothing)) Then
                    FrmWaitInfo._frmMain = frm
                    FrmWaitInfo._thread = New Thread(New ThreadStart(AddressOf FrmWaitInfo.StartThread))
                    FrmWaitInfo._thread.Name = "FrmWaitInfo"
                    FrmWaitInfo._thread.IsBackground = True
                    FrmWaitInfo._thread.Start()
                End If
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

        Private Shared Sub StartThread()
            Try
                FrmWaitInfo._frmWaitInfo = New FrmWaitInfo
                Application.Run(FrmWaitInfo._frmWaitInfo)
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

        Private Sub timerWait_Tick(ByVal sender As Object, ByVal e As EventArgs)
            Dim str As String = Regex.Replace(Me.Text, "[^" & ChrW(65294) & "]", String.Empty)
            Me.Text = If((str.Length = 3), Me.Text.Replace(ChrW(65294), String.Empty), (Me.Text & ChrW(65294)))
        End Sub


        ' Properties
        Public Shared ReadOnly Property WaitInfoForm() As FrmWaitInfo
            Get
                Return FrmWaitInfo._frmWaitInfo
            End Get
        End Property


        ' Fields
        Private Shared _frmMain As Form = Nothing
        Private Shared _frmWaitInfo As FrmWaitInfo = Nothing
        Private Shared _thread As Thread = Nothing
        Private components As IContainer
        Private Const DOT As String = ChrW(65294)
        Private Const DOT_MAX_COUNT As Integer = 3
        Private label1 As Label
        Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private Shared ReadOnly syncObject As Object = New Object
        Private timerWait As System.Windows.Forms.Timer
    End Class
End Namespace

