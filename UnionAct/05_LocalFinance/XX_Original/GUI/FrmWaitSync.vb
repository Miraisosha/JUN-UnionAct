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
    Public Class FrmWaitSync
        Inherits Form
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Shared Sub CloseWaitForm()
            Try
                SyncLock FrmWaitSync.syncObject
                    If ((Not FrmWaitSync._FrmWaitSync Is Nothing) AndAlso Not FrmWaitSync._FrmWaitSync.IsDisposed) Then
                        If FrmWaitSync._FrmWaitSync.InvokeRequired Then
                            Try
                                FrmWaitSync._FrmWaitSync.Invoke(New MethodInvoker(AddressOf FrmWaitSync._FrmWaitSync.Close))
                            Catch exception As Exception
                                FrmWaitSync.logger.Info(exception.Message)
                                'TODO Dim class2 As New FactoryBusClass
                                'TODO DirectCast(class2.GetObject("Business.Common.LogCommand"), ILogCommand).LogOutput("INFO", exception.Message)
                            End Try
                        Else
                            FrmWaitSync._FrmWaitSync.Close()
                        End If
                    End If
                    If (Not FrmWaitSync._frmMain Is Nothing) Then
                        FrmWaitSync._frmMain.Activate()
                    End If
                    FrmWaitSync._FrmWaitSync = Nothing
                    FrmWaitSync._thread = Nothing
                    FrmWaitSync._frmMain = Nothing
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
            Me.components = New System.ComponentModel.Container
            Me.label1 = New System.Windows.Forms.Label
            Me.timerWait = New System.Windows.Forms.Timer(Me.components)
            MyBase.SuspendLayout()
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(10, 19)
            Me.label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(176, 32)
            Me.label1.TabIndex = 0
            Me.label1.Text = "マスタデータと同期中です。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "しばらくお待ちください。"
            Me.label1.UseWaitCursor = True
            Me.timerWait.Enabled = True
            Me.timerWait.Interval = 500
            Me.ClientSize = New System.Drawing.Size(193, 70)
            Me.ControlBox = False
            Me.Controls.Add(Me.label1)
            Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.Margin = New System.Windows.Forms.Padding(4)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FrmWaitSync"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "通信中．．．"
            Me.TopMost = True
            Me.UseWaitCursor = True
            Me.ResumeLayout(False)
            Me.PerformLayout()
            AddHandler Me.timerWait.Tick, New EventHandler(AddressOf Me.timerWait_Tick)
            MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
            MyBase.AutoScaleMode = AutoScaleMode.Font

            'Me.label1.Text = ChrW(12375) & ChrW(12400) & ChrW(12425) & ChrW(12367) & ChrW(12362) & ChrW(24453) & ChrW(12385) & ChrW(12367) & ChrW(12384) & ChrW(12373) & ChrW(12356) & ChrW(12290)
            'AddHandler Me.timerWait.Tick, New EventHandler(AddressOf Me.timerWait_Tick)
            'MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
            'MyBase.AutoScaleMode = AutoScaleMode.Font
            'MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
            'Me.Text = ChrW(20966) & ChrW(29702) & ChrW(20013) & ChrW(65294) & ChrW(65294) & ChrW(65294)
        End Sub

        Public Shared Sub ShowWaitForm(ByVal frm As Form)
            Try
                If ((FrmWaitSync._FrmWaitSync Is Nothing) AndAlso (FrmWaitSync._thread Is Nothing)) Then
                    FrmWaitSync._frmMain = frm
                    FrmWaitSync._thread = New Thread(New ThreadStart(AddressOf FrmWaitSync.StartThread))
                    FrmWaitSync._thread.Name = "FrmWaitSync"
                    FrmWaitSync._thread.IsBackground = True
                    FrmWaitSync._thread.Start()
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
                FrmWaitSync._FrmWaitSync = New FrmWaitSync
                Application.Run(FrmWaitSync._FrmWaitSync)
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
        Public Shared ReadOnly Property WaitInfoForm() As FrmWaitSync
            Get
                Return FrmWaitSync._FrmWaitSync
            End Get
        End Property


        ' Fields
        Private Shared _frmMain As Form = Nothing
        Private Shared _FrmWaitSync As FrmWaitSync = Nothing
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

