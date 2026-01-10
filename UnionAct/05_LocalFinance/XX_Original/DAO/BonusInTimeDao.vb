Imports System

Namespace DAO.FinancialAffairs.WageReduction
    Public Class BonusInTimeDao
        Inherits BonusBaseDao
        ' Methods
        Public Sub New()
            MyBase.New("pay_time_cut_once")
        End Sub


        ' Fields
        Private Const TABLE_NAME As String = "pay_time_cut_once"
    End Class
End Namespace
