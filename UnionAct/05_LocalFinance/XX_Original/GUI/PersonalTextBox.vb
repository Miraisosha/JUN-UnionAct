Imports System
Imports System.Data
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common

Namespace GUI.UnionComponent
    Public Class PersonalTextBox
        Inherits TextBox
        'TODO Implements IPersonalComponent
        ' Methods
        Public Sub New()
            MyBase.ScrollBars = ScrollBars.None
            Me.Text = ""
            Me._validator = New ValidateDelegate(AddressOf Me.ValidateDefault)
        End Sub

        Public Sub AddValidator(ByVal NewValidator As Object) 'TODO Delegate)
            'TODO Me._validator = DirectCast(Delegate.Combine(Me._validator, DirectCast(NewValidator, ValidateDelegate)), ValidateDelegate)
            Me._validator = NewValidator
        End Sub

        Public Sub ClearValidator()
            Me._validator = Nothing
        End Sub

        Public Sub GetInformation()
            Try
                Dim str As String
                Dim class2 As New FactoryBusClass
                Dim informationData As DataSet = DirectCast(class2.GetObject("Business.Common.InformationTblCommand"), IInformationTblCommand).GetInformationData
                If (informationData.Tables.Item("information").Rows.Count > 0) Then
                    str = informationData.Tables.Item("information").Rows.Item(0).Item("l_message").ToString
                Else
                    str = ""
                End If
                Me.Text = str.Replace("\r\n", "")
                If (MyBase.Lines.Length >= 7) Then
                    MyBase.ScrollBars = ScrollBars.Vertical
                End If
            Catch exception1 As Exception
            End Try
        End Sub

        Public Function PressedEnterKey(ByVal e As KeyPressEventArgs) As Boolean
            Dim flag As Boolean
            Try
                flag = e.KeyChar.Equals("")
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Public Sub SetValidator(ByVal NewValidator As Object) 'TODO Delegate)
            Me._validator = DirectCast(NewValidator, ValidateDelegate)
        End Sub

        Public Overridable Sub Validate()
            Me._validator.Invoke()
        End Sub

        Public Overridable Sub ValidateDefault()
            If MyBase.Enabled Then
                If (Me.Require AndAlso ((Me.Text Is Nothing) OrElse (Me.Text.Length = 0))) Then
                    Throw New NotEntryException(Me.FieldName)
                End If
                If Not ValidatorUtility.CheckInputAttribute(Me.FieldAttribute, Me.Text) Then
                    Throw New InvalidAttributeException(Me.FieldName)
                End If
            End If
        End Sub


        ' Properties
        Public Property FieldAttribute As EFieldAttribute
            Get
                Return Me._attr
            End Get
            Set(ByVal value As EFieldAttribute)
                Me._attr = value
            End Set
        End Property

        Public ReadOnly Property FieldName As String
            Get
                If ((Not MyBase.Tag Is Nothing) AndAlso (MyBase.Tag.ToString.Length <> 0)) Then
                    Return MyBase.Tag.ToString
                End If
                Return MyBase.Name
            End Get
        End Property

        Public Property Require As Boolean
            Get
                Return Me._require
            End Get
            Set(ByVal value As Boolean)
                Me._require = value
            End Set
        End Property

        Public ReadOnly Property TextDataSet As String
            Get
                If (Me.Text = "") Then
                    Return Nothing
                End If
                Return Me.Text
            End Get
        End Property


        ' Fields
        Private _attr As EFieldAttribute
        Private _require As Boolean
        Private _validator As ValidateDelegate

        ' Nested Types
        Private Delegate Sub ValidateDelegate()
    End Class
End Namespace
