Imports System

Namespace DAO.FinancialAffairs.WageReduction
    Friend Class BonusStrikeDao
        Inherits BonusBaseDao
        ' Methods
        Public Sub New()
            MyBase.New("pay_strike_cut_once")
        End Sub


        ' Fields
        Private Const TABLE_NAME As String = "pay_strike_cut_once"
    End Class
End Namespace
