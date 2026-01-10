Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSCLMsg
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.GUI.Common

Namespace GUI.RevenueExpenditure
    Public Class RevenueExpenditureBase
        Inherits UserControl
        ' Methods
        Public Sub New()
            Me._factory = New FactoryBusClass
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal isGetEntryRight As Boolean, ByVal isPrintRight As Boolean, ByVal isReferenceRight As Boolean, ByVal strRevenueTitle As String, ByVal dTimeUp As Object, ByVal strRevenueStartDate As String, ByVal strRevenueEndDate As String, ByVal strLastRevenueStartDate As String, ByVal _isNewFlg As Boolean, ByVal _isChangeFlg As Boolean)
            Me.New()
            Me._IsGetEntryRight = isGetEntryRight
            Me._IsPrintRight = isPrintRight
            Me._IsReferenceRight = isReferenceRight
            Me._RevenueTitle = strRevenueTitle
            If ((Not dTimeUp Is Nothing) AndAlso (Not dTimeUp Is DBNull.Value)) Then
                Me._dtDup = CDate(dTimeUp)
            End If
            Me._RevenueStartDate = strRevenueStartDate
            Me._RevenueEndDate = strRevenueEndDate
            Me._LastRevenueStartDate = strLastRevenueStartDate
            Me._IsNewFlg = _isNewFlg
            Me._IsChangeFlg = _isChangeFlg
        End Sub

        Protected Sub CheckUpdateMessage(ByVal ControlName As String)
            Try
                Dim revenueCommand As String = (New RevenueExpenditureListCommand).GetRevenueCommand(Me._RevenueStartDate, ControlName)
                If Not revenueCommand.Equals("") Then
                    CLMsg.Show("GI0039", revenueCommand)
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

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            MyBase.SuspendLayout()
            MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
            MyBase.AutoScaleMode = AutoScaleMode.Font
            Me.Font = New Font("MS UI Gothic", 12.0!)
            MyBase.Margin = New Padding(4, 4, 4, 4)
            MyBase.Name = "RevenueExpenditureBase"
            MyBase.Size = New Size(&HE1, 200)
            MyBase.ResumeLayout(False)
        End Sub


        ' Properties
        Protected Property dtDup() As DateTime
            Get
                Return Me._dtDup
            End Get
            Set(ByVal value As DateTime)
                Me._dtDup = value
            End Set
        End Property

        Protected Property IsChangeFlg() As Boolean
            Get
                Return Me._IsChangeFlg
            End Get
            Set(ByVal value As Boolean)
                Me._IsChangeFlg = value
            End Set
        End Property

        Protected Property IsGetEntryRight() As Boolean
            Get
                Return Me._IsGetEntryRight
            End Get
            Set(ByVal value As Boolean)
                Me._IsGetEntryRight = value
            End Set
        End Property

        Protected Property IsNewFlg() As Boolean
            Get
                Return Me._IsNewFlg
            End Get
            Set(ByVal value As Boolean)
                Me._IsNewFlg = value
            End Set
        End Property

        Protected Property IsPrintRight() As Boolean
            Get
                Return Me._IsPrintRight
            End Get
            Set(ByVal value As Boolean)
                Me._IsPrintRight = value
            End Set
        End Property

        Protected Property IsReferenceRight() As Boolean
            Get
                Return Me._IsReferenceRight
            End Get
            Set(ByVal value As Boolean)
                Me._IsReferenceRight = value
            End Set
        End Property

        Protected Property LastRevenueStartDate() As String
            Get
                Return Me._LastRevenueStartDate
            End Get
            Set(ByVal value As String)
                Me._LastRevenueStartDate = value
            End Set
        End Property

        Protected Property RevenueEndDate() As String
            Get
                Return Me._RevenueEndDate
            End Get
            Set(ByVal value As String)
                Me._RevenueEndDate = value
            End Set
        End Property

        Protected Property RevenueStartDate() As String
            Get
                Return Me._RevenueStartDate
            End Get
            Set(ByVal value As String)
                Me._RevenueStartDate = value
            End Set
        End Property

        Protected Property RevenueTitle() As String
            Get
                Return Me._RevenueTitle
            End Get
            Set(ByVal value As String)
                Me._RevenueTitle = value
            End Set
        End Property

        Public Sub SetValidator(ByVal NewValidator As ValidateDelegate)
            Me._validator = NewValidator
        End Sub

        Public Overridable Sub ValidateDefault()
            ValidatorUtility.ValidateAll(Me)
        End Sub

        Public Overridable Sub ValidateFields()
            If (Not Me._validator Is Nothing) Then
                Me._validator.Invoke()
            End If
        End Sub

        ' Fields
        Private _validator As ValidateDelegate
        Protected _dtDup As DateTime
        Protected _factory As FactoryBusClass
        Protected _IsChangeFlg As Boolean
        Protected _IsGetEntryRight As Boolean
        Protected _IsNewFlg As Boolean
        Protected _IsPrintRight As Boolean
        Protected _IsReferenceRight As Boolean
        Protected _LastRevenueStartDate As String
        Protected _RevenueEndDate As String
        Protected _RevenueStartDate As String
        Protected _RevenueTitle As String
        Private components As IContainer

        ' Nested Types
        Public Class [Const]
            ' Fields
            Public Const DETAIL_VIEW_MODE As String = "è⁄ç◊ï\é¶"
            Public Const NEW_ENTRY_MODE As String = "êVãKìoò^"

            ' Nested Types
            Public Class Grid
                ' Fields
                Public Const FONTNAME_GRIDDATA As String = "ÇlÇr" & " " & "ÉSÉVÉbÉN"
                Public Const FONTSIZE_GRIDDATA As Integer = 12
                Public Const MONEY_FORMAT As String = "N0"

                ' Nested Types
                Public Class Style
                    ' Fields
                    Public Const STYLE_COL_LINK As String = "col_link"
                    Public Const STYLE_COL_NOLINK As String = "col_nolink"
                End Class
            End Class
        End Class
    End Class
End Namespace
