Imports C1.Win.C1FlexGrid
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.RevenueExpenditure.UnionForm
Imports UnionAct.NSCLMsg
Imports UnionAct.GUI.UnionComponent

Namespace GUI.Common
    Public Class ValidatorUtility
        ' Methods
        Public Shared Function CheckInputAttribute(ByVal attr As EFieldAttribute, ByVal target As String) As Boolean
            If ((target Is Nothing) OrElse (target.Length = 0)) Then
                Return True
            End If
            If (attr = EFieldAttribute.NONE) Then
                Return True
            End If
            Dim flag As Boolean = True
            Select Case attr
                Case EFieldAttribute.NUMERIC
                    Return Regex.IsMatch(target, "^[0-9]+$")
                Case EFieldAttribute.HALF_ALPHABET
                    Return Regex.IsMatch(target, "^[a-zA-Z]+$")
                Case EFieldAttribute.HALF_ALPHANUMERIC
                    Return Regex.IsMatch(target, "^[0-9a-zA-Z]+$")
                Case EFieldAttribute.DATE
                    Try
                        If Not target.Equals("    /  /") Then
                            Dim time As DateTime
                            flag = DateTime.TryParseExact(target, "yyyy/MM/dd", Nothing, DateTimeStyles.AllowWhiteSpaces, time)
                        End If
                    Catch exception1 As Exception
                        flag = False
                    End Try
                    Return flag
                Case EFieldAttribute.HALF_KANA
                    Return Regex.IsMatch(target, "^[" & "¡" & "-" & "ß" & "\s]+$")
                Case EFieldAttribute.WIDE_CHAR
                    Return Regex.IsMatch(target, "^[ " & "@" & "]*[^ -~" & "¡" & "-" & "ß" & "]*[ " & "@" & "]*$")
                Case EFieldAttribute.MONTH
                    flag = Regex.IsMatch(target, "^[0-9]+$")
                    Return ((Not flag OrElse ((Integer.Parse(target) >= 1) AndAlso (12 >= Integer.Parse(target)))) AndAlso flag)
            End Select
            Return flag
        End Function

        Public Shared Function GetAllControls(ByVal top As Control) As Control()
            Dim list As New ArrayList
            Dim control As Control
            For Each control In top.Controls
                list.Add(control)
                list.AddRange(ValidatorUtility.GetAllControls(control))
            Next
            Return DirectCast(list.ToArray(GetType(Control)), Control())
        End Function

        Public Shared Sub ResetBackGroundColor(ByVal ctlTarget As Control)
            Dim allControls As Control() = ValidatorUtility.GetAllControls(ctlTarget)
            Dim i As Integer
            For i = 0 To allControls.Length - 1
                If TypeOf allControls(i) Is IPersonalComponent Then
                    allControls(i).BackColor = Color.Empty
                End If
            Next i
        End Sub

        Public Shared Sub ResetEnabled(ByVal ctlTarget As Control)
            Dim allControls As Control() = ValidatorUtility.GetAllControls(ctlTarget)
            Dim i As Integer
            For i = 0 To allControls.Length - 1
                If TypeOf allControls(i) Is IPersonalComponent Then
                    allControls(i).Enabled = True
                End If
            Next i
        End Sub

        Public Shared Sub ResetErrorBackGroundColor(ByVal ctlContainer As Control)
            Try
                Dim control As Control
                For Each control In ValidatorUtility.GetAllControls(ctlContainer)
                    If (control.BackColor.Equals(Color.LightPink) OrElse control.BackColor.Equals(Color.LightSalmon)) Then
                        control.BackColor = SystemColors.Window
                    End If
                Next
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Shared Sub ValidateAll(ByVal ctlTarget As Control)
            ValidatorUtility.ValidateAll(ctlTarget, False)
        End Sub

        Public Shared Sub ValidateAll(ByVal ctlTarget As Control, ByVal chkUnVisibleCtl As Boolean)
            Dim exception As AppUnionException = Nothing
            Dim allControls As Control() = ValidatorUtility.GetAllControls(ctlTarget)
            Dim i As Integer
            For i = 0 To allControls.Length - 1
                Dim method As MethodInfo = allControls(i).GetType.GetMethod("Validate", New Type(0 - 1) {})
                If (Not method Is Nothing) Then
                    Try
                        If (allControls(i).Enabled AndAlso (chkUnVisibleCtl OrElse allControls(i).Visible)) Then
                            method.Invoke(allControls(i), Nothing)
                            allControls(i).BackColor = Color.Empty
                        End If
                    Catch exception2 As Exception
                        Dim innerException As Exception = exception2.InnerException
                        Dim msgId As String = ""
                        If TypeOf innerException Is NotEntryException Then
                            msgId = "GE0006"
                            allControls(i).BackColor = Color.LightPink
                        ElseIf TypeOf innerException Is InvalidAttributeException Then
                            msgId = "GE0019"
                            allControls(i).BackColor = Color.LightSalmon
                        ElseIf TypeOf innerException Is InvalidDateException Then
                            msgId = "GE0020"
                            allControls(i).BackColor = Color.LightSalmon
                        ElseIf TypeOf innerException Is InvalidTimeException Then
                            msgId = "GE0021"
                            allControls(i).BackColor = Color.LightSalmon
                        Else
                            If Not TypeOf innerException Is InvalidMoneyValueException Then
                                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
                            End If
                            msgId = "GE0167"
                            allControls(i).BackColor = Color.LightSalmon
                        End If
                        If (exception Is Nothing) Then
                            exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, msgId, New String() {DirectCast(innerException, InvalidInputException).InvalidFieldName})
                        Else
                            exception.AddExceptionData(msgId, New String() {DirectCast(innerException, InvalidInputException).InvalidFieldName})
                        End If
                    End Try
                End If
            Next i
            If (Not exception Is Nothing) Then
                Throw exception
            End If
        End Sub

        Public Shared Function ValidateNumericValue(ByVal strInput As String) As Boolean
            Dim flag As Boolean
            Try
                Dim num As Long
                flag = ValidatorUtility.ValidateNumericValue(strInput, num)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return flag
        End Function

        Public Shared Function ValidateNumericValue(ByVal strInput As String, <Out()> ByRef lngValue As Long) As Boolean
            Dim flag As Boolean
            Try
                If Not Long.TryParse(strInput.Replace(",", ""), lngValue) Then
                    If Not String.IsNullOrEmpty(strInput) Then
                        CLMsg.Show("GE0178")
                    End If
                    Return False
                End If
                flag = True
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Shared Sub ValidateNumericValue(ByVal flxTarget As C1FlexGrid, ByVal Fire As ValidateNumericDelegate, ByVal e As ValidateEditEventArgs)
            Try
                Dim num As Long
                If ValidatorUtility.ValidateNumericValue(flxTarget.Editor.Text, num) Then
                    Fire.Invoke(flxTarget, num, e.Row, e.Col)
                ElseIf String.IsNullOrEmpty(flxTarget.Editor.Text) Then
                    flxTarget.Item(e.Row, e.Col) = DBNull.Value
                Else
                    flxTarget.FinishEditing(True)
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

        Public Shared Function ValidateNumericValue(ByVal flxFirer As C1FlexGrid, <Out()> ByRef lngValue As Long, ByVal allowNull As Boolean, ByVal e As ValidateEditEventArgs) As Boolean
            Dim flag As Boolean
            Try
                If Not Long.TryParse(flxFirer.Editor.Text, lngValue) Then
                    Dim messageId As String = ""
                    If String.IsNullOrEmpty(flxFirer.Editor.Text) Then
                        If allowNull Then
                            flxFirer.Item(e.Row, e.Col) = DBNull.Value
                            Return True
                        End If
                        If MDFinanceCommon.IsEmptyCell(flxFirer, e.Row, e.Col) Then
                            Return True
                        End If
                        messageId = "GE0180"
                    Else
                        messageId = "GE0178"
                    End If
                    CLMsg.Show(messageId)
                    Return False
                End If
                flag = True
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

    End Class
End Namespace
