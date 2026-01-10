Imports System
Imports System.Data

Namespace Framework.Mapping
    Public Class ColumnMap
        ' Methods
        Private Sub New()
            Me.strPhysicalName = ""
            Me.strLogicalName = ""
        End Sub

        Public Sub New(ByVal PhysicalName As String, ByVal LogicalName As String, ByVal SystemDataType As Type)
            Me.strPhysicalName = ""
            Me.strLogicalName = ""
            Me.strPhysicalName = PhysicalName
            Me.strLogicalName = LogicalName
            Me.typeSystem = SystemDataType
        End Sub


        ' Properties
        Public ReadOnly Property DbDataType As DbType
            Get
                Dim ansiString As DbType = DbType.AnsiString
                Select Case Type.GetTypeCode(Me.typeSystem)
                    Case TypeCode.Boolean
                        Return DbType.Boolean
                    Case TypeCode.Char, TypeCode.SByte, TypeCode.UInt16
                        Return ansiString
                    Case TypeCode.Byte
                        Return DbType.Byte
                    Case TypeCode.Int16
                        Return DbType.Int16
                    Case TypeCode.Int32
                        Return DbType.Int32
                    Case TypeCode.Double
                        Return DbType.Double
                    Case TypeCode.Decimal, (TypeCode.DateTime Or TypeCode.Object)
                        Return ansiString
                    Case TypeCode.DateTime
                        Return DbType.DateTime
                    Case TypeCode.String
                        Return DbType.String
                End Select
                Return ansiString
            End Get
        End Property

        Public ReadOnly Property LogicalName As String
            Get
                Return Me.strLogicalName
            End Get
        End Property

        Public ReadOnly Property PhysicalName As String
            Get
                Return Me.strPhysicalName
            End Get
        End Property

        Public ReadOnly Property SystemDataType As Type
            Get
                Return Me.typeSystem
            End Get
        End Property


        ' Fields
        Private strLogicalName As String
        Private strPhysicalName As String
        Private typeSystem As Type
    End Class
End Namespace
