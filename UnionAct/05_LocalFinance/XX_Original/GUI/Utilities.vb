Imports C1.Win.C1FlexGrid
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common.UnionForm

Namespace GUI.Common
    Public Class Utilities
        ' Methods
        Public Shared Sub Centering(ByVal objBase As Object, ByVal objTarget As Object)
            Try
                Utilities.CenteringHorizontal(objBase, objTarget)
                Utilities.CenteringVertical(objBase, objTarget)
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

        Public Shared Sub CenteringHorizontal(ByVal objBase As Object, ByVal objTarget As Object)
            Try
                Dim type As Type = objBase.GetType
                Dim type2 As Type = objTarget.GetType
                Dim prop As PropertyInfo = type.GetProperty("Width")
                Dim info2 As PropertyInfo = type2.GetProperty("Width")
                If ((prop Is Nothing) OrElse (info2 Is Nothing)) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0001", New String(0 - 1) {})
                End If
                Dim num As Integer = CInt(prop.GetValue(objBase, Nothing))
                Dim num2 As Integer = CInt(info2.GetValue(objTarget, Nothing))
                type2.GetProperty("Left").SetValue(objTarget, CInt((num - num2) / 2), Nothing)
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

        Public Shared Sub CenteringVertical(ByVal objBase As Object, ByVal objTarget As Object)
            Try
                Dim type As Type = objBase.GetType
                Dim type2 As Type = objTarget.GetType
                Dim prop As PropertyInfo = type.GetProperty("Height")
                Dim info2 As PropertyInfo = type2.GetProperty("Height")
                If ((prop Is Nothing) OrElse (info2 Is Nothing)) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0001", New String(0 - 1) {})
                End If
                Dim num As Integer = CInt(prop.GetValue(objBase, Nothing))
                Dim num2 As Integer = CInt(info2.GetValue(objTarget, Nothing))
                type2.GetProperty("Top").SetValue(objTarget, CInt((num - num2) / 2), Nothing)
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

        Public Shared Sub CheckDateString(ByVal flxTarget As C1FlexGrid, ByVal iCurrentRow As Integer, ByVal iCurrentCol As Integer)
        End Sub

        Public Shared Sub ClearInputedData(ByVal ParamArray ctlGroup As Control())
            Try
                Dim i As Integer
                For i = 0 To ctlGroup.Length - 1
                    Utilities.ClearInputedData(ctlGroup(i))
                Next i
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

        Public Shared Sub ClearInputedData(ByVal ctlTarget As Control)
            Try
                If ctlTarget.HasChildren Then
                    Dim control As Control
                    For Each control In ctlTarget.Controls
                        Utilities.ClearInputedData(control)
                    Next
                ElseIf ctlTarget.HasChildren Then
                    Utilities.ClearInputedData(ctlTarget)
                Else
                    Dim prop As PropertyInfo = ctlTarget.GetType.GetProperty("SelectedIndex")
                    If (Not prop Is Nothing) Then
                        prop.SetValue(ctlTarget, -1, Nothing)
                    Else
                        prop = ctlTarget.GetType.GetProperty("Checked")
                        If (Not prop Is Nothing) Then
                            prop.SetValue(ctlTarget, False, Nothing)
                        Else
                            Dim method As MethodInfo = ctlTarget.GetType.GetMethod("Clear")
                            If (Not method Is Nothing) Then
                                method.Invoke(ctlTarget, Nothing)
                            End If
                        End If
                    End If
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

        Public Shared Sub ControlEdit(ByVal canEdit As Boolean, ByVal ctlParent As Control)
            Try
                Dim control As Control
                For Each control In ctlParent.Controls
                    If control.HasChildren Then
                        Utilities.ControlEdit(canEdit, control)
                    Else
                        Utilities.SetCanEditToControl(canEdit, control)
                    End If
                Next
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

        Public Shared Sub ControlEditForControlGroup(ByVal canEdit As Boolean, ByVal ParamArray ctlGroup As Control())
            Try
                Dim i As Integer
                For i = 0 To ctlGroup.Length - 1
                    Utilities.SetCanEditToControl(canEdit, ctlGroup(i))
                Next i
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

        Private Shared Sub ControlEnterEvent(ByVal canEdit As Boolean, ByVal ctlTarget As Control)
            Try
                RemoveHandler ctlTarget.Enter, New EventHandler(AddressOf Utilities.SetFocusToParent)
                ctlTarget.TabStop = canEdit
                ctlTarget.ContextMenu = If(canEdit, Nothing, New ContextMenu)
                If Not canEdit Then
                    AddHandler ctlTarget.Enter, New EventHandler(AddressOf Utilities.SetFocusToParent)
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

        Public Shared Function CreateObject(ByVal strClassName As String, ByVal ParamArray objLoginSession As Object()) As Object
            Dim obj3 As Object
            Try
                Dim type As Type = Type.GetType(strClassName)
                If (type Is Nothing) Then
                    obj3 = Nothing
                Else
                    Try
                        obj3 = Activator.CreateInstance(type, objLoginSession)
                    Catch exception As TargetInvocationException
                        'TODO AbstractGui.WriteLog((strClassName & "のコンストラクタ内：" & exception.InnerException.Message))
                        Throw exception
                    Catch exception2 As Exception
                        'TODO bstractGui.WriteLog((strClassName & "：" & exception2.Message))
                        Throw exception2
                    End Try
                End If
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return obj3
        End Function

        Public Shared Function GetDaysInMonthForDayCombo(ByVal strYear As String, ByVal strMonth As String) As String()
            Dim strArray As String()
            Try
                Dim time As DateTime
                Dim list As New List(Of String)
                If Not DateTime.TryParse((strYear & "/" & strMonth), time) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0020", New String() {"指定された年月"})
                End If
                Dim year As Integer = Convert.ToInt32(strYear)
                Dim month As Integer = Convert.ToInt32(strMonth)
                Dim num3 As Integer = DateTime.DaysInMonth(year, month)
                Dim i As Integer = 1
                Do While (i <= num3)
                    list.Add(String.Format("{0:D2}", i))
                    i += 1
                Loop
                strArray = list.ToArray
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return strArray
        End Function

        Public Shared Function GetDaysInMonthForDayCombo(ByVal strYear As String, ByVal strMonth As String, ByVal hasEmptyLine As Boolean) As String()
            Dim strArray As String()
            Try
                Dim list As New List(Of String)
                list.AddRange(Utilities.GetDaysInMonthForDayCombo(strYear, strMonth))
                If hasEmptyLine Then
                    list.Insert(0, "")
                End If
                strArray = list.ToArray
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return strArray
        End Function

        'Public Shared Function GetHeaderName() As String
        '    Dim str As String
        '    Try
        '        Dim control As UserControl
        '        For Each control In Utilities.PnlMainPanel.Controls
        '            If TypeOf control Is CtlHeader Then
        '                Return DirectCast(control.Tag, Label).Text.ToString
        '            End If
        '        Next
        '        str = Nothing
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        '    End Try
        '    Return str
        'End Function

        Public Shared Sub OverlayUserControl(ByVal pnlParent As Panel, ByVal strHeaderName As String, ByVal ParamArray uctlPaste As UserControl())
            Try
                pnlMainPanel = pnlParent
                Dim control As Control
                For Each control In pnlParent.Controls
                    'For Each control In Utilities.PnlMainPanel.Controls
                    If TypeOf control Is UserControl Then
                        If TypeOf control Is UserControl Then
                            control.Visible = False
                        End If
                    End If
                Next
                Utilities.ShowMenuItems(strHeaderName, uctlPaste)
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

        Public Shared Sub RemoveAllUserControl()
            Try
                Utilities.PnlMainPanel.Controls.Clear()
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

        Public Shared Sub RestoreUserControl()
            Try
                DirectCast(Utilities.PnlMainPanel.Parent, FM000102).SuspendLayout()
                Dim list As New List(Of Control)
                Dim control As Control
                For Each control In Utilities.PnlMainPanel.Controls
                    If TypeOf control Is UserControl Then
                        If control.Visible Then
                            list.Add(control)
                        Else
                            control.Visible = True
                            'TODO DirectCast(control, UserControl).ActionAfterResotreUserControl()
                        End If
                    End If
                Next
                Dim i As Integer
                For i = 0 To list.Count - 1
                    list.Item(i).Dispose()
                Next i
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                DirectCast(Utilities.PnlMainPanel.Parent, FM000102).ResumeLayout()
            End Try
        End Sub

        Public Shared Sub SetBackColorProperty(ByVal backColor As Color, ByVal ParamArray ctlTargets As Control())
            Try
                Dim control As Control
                For Each control In ctlTargets
                    control.BackColor = backColor
                Next
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

        Public Shared Sub SetCanEditToControl(ByVal canEdit As Boolean, ByVal ParamArray ctlTargets As Control())
            Try
                Dim control As Control
                For Each control In ctlTargets
                    Dim prop As PropertyInfo = control.GetType.GetProperty("AutoCheck")
                    If (Not prop Is Nothing) Then
                        prop.SetValue(control, canEdit, Nothing)
                    Else
                        Dim button As Button = TryCast(control, Button)
                        If (Not button Is Nothing) Then
                            button.Enabled = canEdit
                        Else
                            Utilities.ControlEnterEvent(canEdit, control)
                        End If
                    End If
                Next
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

        Private Shared Sub SetCanEditToControl(ByVal canEdit As Boolean, ByVal ctlTarget As Control)
            Try
                Dim prop As PropertyInfo = ctlTarget.GetType.GetProperty("AutoCheck")
                If (Not prop Is Nothing) Then
                    prop.SetValue(ctlTarget, canEdit, Nothing)
                Else
                    Dim button As Button = TryCast(ctlTarget, Button)
                    If (Not button Is Nothing) Then
                        button.Enabled = canEdit
                    Else
                        Utilities.ControlEnterEvent(canEdit, ctlTarget)
                    End If
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

        Public Shared Sub SetEnabledProperty(ByVal isEnable As Boolean, ByVal ParamArray ctlTargets As Control())
            Try
                Dim control As Control
                For Each control In ctlTargets
                    control.Enabled = isEnable
                Next
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

        Private Shared Sub SetFocusToParent(ByVal sender As Object, ByVal ea As EventArgs)
            Try
                Dim control As Control = TryCast(sender, Control)
                If (Not control Is Nothing) Then
                    control.Parent.Focus()
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

        Private Shared Sub SetHeaderName(ByVal lblHeader As Label, ByVal strHeaderName As String)
            Try
                lblHeader.Text = strHeaderName
                Utilities.Centering(lblHeader.Parent, lblHeader)
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

        Public Shared Sub SetVariousProperty(ByVal strPropertyName As String, ByVal objSetValue As Object, ByVal ParamArray ctlTargets As Control())
            Try
                Dim i As Integer
                For i = 0 To ctlTargets.Length - 1
                    If ctlTargets(i).HasChildren Then
                        Dim array As Control() = New Control(ctlTargets(i).Controls.Count - 1) {}
                        ctlTargets(i).Controls.CopyTo(array, 0)
                        Utilities.SetVariousProperty(strPropertyName, objSetValue, array)
                    End If
                    Dim prop As PropertyInfo = ctlTargets(i).GetType.GetProperty(strPropertyName)
                    If (Not prop Is Nothing) Then
                        prop.SetValue(ctlTargets(i), objSetValue, Nothing)
                    End If
                Next i
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

        Public Shared Sub SetVisibleProperty(ByVal isVisible As Boolean, ByVal ParamArray ctlTargets As Control())
            Try
                Dim control As Control
                For Each control In ctlTargets
                    control.Visible = isVisible
                Next
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

        Public Shared Sub ShowMenuItems(ByVal strHeaderName As String, ByVal ParamArray uctlPaste As UserControl())
            Try
                DirectCast(Utilities.PnlMainPanel.Parent, FM000102).SuspendLayout()
                Dim y As Integer = 0
                Dim header As New CtlHeader() With { _
                    .Parent = Utilities.PnlMainPanel, _
                    .Dock = DockStyle.None, _
                    .Location = New Point(0, 0) _
                }
                Utilities.SetHeaderName(DirectCast(header.Tag, Label), strHeaderName)
                y = (y + header.Size.Height)
                Dim builder As New StringBuilder
                Dim control As UserControl
                For Each control In uctlPaste
                    control.Parent = Utilities.PnlMainPanel
                    control.Dock = DockStyle.None
                    control.Location = New Point(0, y)
                    y = (y + control.Size.Height)
                    control.Focus()
                    builder.Append((control.GetType.Namespace & "." & control.Name & ","))
                Next
                log.Info(String.Concat(New String() {"画面遷移『", strHeaderName.ToString, "』" & " (CtlName" & "：", builder.ToString, ")"}))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                DirectCast(Utilities.PnlMainPanel.Parent, FM000102).ResumeLayout()
            End Try
        End Sub


        ' Properties
        'Public Shared Property BCenteringMainFormFlg() As Boolean
        '    Get
        '        Return Utilities.BCenteringMainFormFlg
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Utilities.BCenteringMainFormFlg = value
        '    End Set
        'End Property

        'Public Property PnlMainPanel() As Panel
        '    Get
        '        Return PnlMainPanel
        '    End Get
        '    Set(ByVal value As Panel)
        '        PnlMainPanel = value
        '    End Set
        'End Property


        ' Fields
        'Private Shared bCenteringMainFormFlg As Boolean
        Private Shared pnlMainPanel As Panel
        ' ログ出力オブジェクト
        Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    End Class
End Namespace
