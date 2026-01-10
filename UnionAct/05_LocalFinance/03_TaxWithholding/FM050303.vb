Imports UnionAct.Framework.Command
Imports UnionAct.Framework
Imports UnionAct.GUI.FinancialAffairs.WithHolding

Public Class FM050303
    'Public Custom Event Cancel As EventHandler

    ' Methods
    Public Sub New(ByVal ExecuteHandler As SelectMonthEventHandler, ByVal CancelHandler As EventHandler)
        Me.UnionMessage = New UnionMessage
        Me.InitializeComponent()
        Me.Execute = ExecuteHandler
        'Me.Cancel = CancelHandler
        MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
        MyBase.ShowInTaskbar = False
        MyBase.ShowIcon = False
        Dim systemDate As String = PublicCommand.GetSystemDate
        Dim num As Integer = Integer.Parse(systemDate.Substring(0, 4))
        Me.cmbYear.Items.Clear()
        Dim i As Integer = (num - 1)
        Do While (i <= (num + 1))
            Me.cmbYear.Items.Add(i.ToString)
            i += 1
        Loop
        Me.cmbYear.SelectedIndex = 1
        Me.cmbMonth.Items.Clear()
        Me.cmbMonth.Items.AddRange(UnionConst.MONTH_RANGE)
        Me.cmbMonth.SelectedIndex = (Integer.Parse(systemDate.Substring(4, 2)) - 1)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        'If (Not Me.Cancel Is Nothing) Then
        '    Me.Cancel.Invoke(Me, EventArgs.Empty)
        'End If
        Me.Dispose(True)
    End Sub

    Private Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs)
        If ((Me.UnionMessage.ShowMessage("GQ0012", New String() {Me.cmbYear.Text, Me.cmbMonth.Text}) <> DialogResult.No) AndAlso (Not Me.Execute Is Nothing)) Then
            Dim args As New SelectMonthEventArgs(Me.cmbYear.Text, Me.cmbMonth.Text)
            Me.Execute.Invoke(Me, args)
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class