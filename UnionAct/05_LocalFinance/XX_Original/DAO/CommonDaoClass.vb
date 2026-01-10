#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System

Namespace DAO
    Public Class CommonDaoClass
        ' Fields
#If USE_POSTGRES Then
        Public Shared connNpgsql As NpgsqlConnection
        Public Shared tranNpgsql As NpgsqlTransaction
#Else
        Public Shared connNpgsql As New NpgsqlConnection
        Public Shared tranNpgsql As NpgsqlTransaction
#End If
    End Class
End Namespace
