Imports System

Namespace GUI.FinancialAffairs
    Public Class GridSettingInfo
        ' Methods
        Private Sub New()
        End Sub

        Public Sub New(ByVal Width As Integer, ByVal StyleName As String, ByVal Margeable As Boolean, ByVal Resizable As Boolean, ByVal Sortable As Boolean)
            Me._width = Width
            Me._style = StyleName
            Me._mergeable = Margeable
            Me._resizable = Resizable
            Me._sortable = Sortable
        End Sub

        Public Sub New(ByVal Width As Integer, ByVal StyleName As String, ByVal Margeable As Boolean, ByVal Resizable As Boolean, ByVal Sortable As Boolean, ByVal Editable As Boolean, ByVal Visible As Boolean)
            Me._width = Width
            Me._style = StyleName
            Me._mergeable = Margeable
            Me._resizable = Resizable
            Me._sortable = Sortable
            Me._editable = Editable
            Me._visible = Visible
        End Sub


        ' Properties
        Public ReadOnly Property AllowEditing As Boolean
            Get
                Return Me._editable
            End Get
        End Property

        Public ReadOnly Property AllowMerging As Boolean
            Get
                Return Me._mergeable
            End Get
        End Property

        Public ReadOnly Property AllowResizing As Boolean
            Get
                Return Me._resizable
            End Get
        End Property

        Public ReadOnly Property AllowSorting As Boolean
            Get
                Return Me._sortable
            End Get
        End Property

        Public ReadOnly Property StyleName As String
            Get
                Return Me._style
            End Get
        End Property

        Public ReadOnly Property Visible As Boolean
            Get
                Return Me._visible
            End Get
        End Property

        Public ReadOnly Property Width As Integer
            Get
                Return Me._width
            End Get
        End Property


        ' Fields
        Private _editable As Boolean
        Private _mergeable As Boolean
        Private _resizable As Boolean
        Private _sortable As Boolean
        Private _style As String
        Private _visible As Boolean
        Private _width As Integer
    End Class
End Namespace
