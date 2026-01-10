Imports C1.Win.C1FlexGrid
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework
Imports UnionAct.MDFinanceCommon

Namespace GUI.FinancialAffairs
    Public Class FinancialAffairsUtility
        ' Methods
        Public Shared Sub AddStyleSummaryListFlexGrid(ByVal flex As C1FlexGrid())
        End Sub

        Public Shared Sub ApplyGridStyle(ByVal flxGrid As C1FlexGrid, ByVal Settings As GridSettingInfo())
            If (Not Settings Is Nothing) Then
                Dim i As Integer
                'Dim counter As Integer
                'If Settings.Length < flxGrid.Cols.Count Then
                '    counter = flxGrid.Cols.Count
                'Else
                '    counter = Settings.Length
                'End If

                For i = 0 To Settings.Length - 1 'TODO
                    'For i = 0 To flxGrid.Cols.Count - 1
                    flxGrid.Cols.Item(i).TextAlignFixed = TextAlignEnum.CenterCenter
                    flxGrid.Cols.Item(i).Style = flxGrid.Styles.Item(Settings(i).StyleName)
                    flxGrid.Cols.Item(i).Width = Settings(i).Width
                    'flxGrid.Cols.Item(i).Caption = flxGrid.Cols.Item(i).Caption & "[" & i & "]"
                    flxGrid.Cols.Item(i).AllowMerging = Settings(i).AllowMerging
                    flxGrid.Cols.Item(i).AllowResizing = Settings(i).AllowResizing
                    flxGrid.Cols.Item(i).AllowSorting = Settings(i).AllowSorting
                Next i
            End If
        End Sub

        Public Shared Sub CheckNetBankDataHasMade(ByVal strCloseDay As String, ByVal strDayPayKind As String)
            Try 
                'Dim class2 As New FactoryBusClass
                'Dim command As INetBankCommand = DirectCast(class2.GetObject("Business.NetBank.NetBankCommand"), INetBankCommand)
                'If command.CheckDataHasMade(strCloseDay, strDayPayKind) Then
                If MDFinanceCommon.NetBank_CheckDataHasMade(strCloseDay, strDayPayKind) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0153", New String() { strCloseDay.Substring(0, 4), strCloseDay.Substring(4, 2) })
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0  - 1) {})
            End Try
        End Sub

        Public Shared Function CreateWithholdingReportHeader(ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable
        'Public Shared Function CreateWithholdingReportHeader(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal Business As IWithholdingCommand) As DataTable
            Dim table2 As DataTable
            Try 
                Dim table As New DataTable("dtHeader")
                table.Columns.Add("month", GetType(String))
                table.Columns.Add("area_local", GetType(String))
                table.Columns.Add("s_break", GetType(String))
                Dim row As DataRow = table.NewRow
                row.Item("month") = TargetMonth
                row.Item("area_local") = ""
                'row.Item("s_break") = Business.GetTruncateAmount(CommonUtility.GetLastDay((TargetYear & TargetMonth))).ToString
                row.Item("s_break") = MDFinanceCommon.FinancialAffairs_GetTruncateAmount(CommonUtility.GetLastDay((TargetYear & TargetMonth))).ToString
                table.Rows.Add(row)
                table2 = table
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return table2
        End Function

        Public Shared Function GetGridFontBold() As Font
            Return New Font("‚l‚r" & " " & "ƒSƒVƒbƒN", 12!, FontStyle.Bold)
        End Function

        Public Shared Function GetGridFontNormal() As Font
            Return New Font("‚l‚r" & " " & "ƒSƒVƒbƒN", 12!)
        End Function

        Public Shared Function LeftZeroTrim(ByVal src As String) As String
            Dim str As String = src
            If ((src.Length > 0) AndAlso src.Substring(0, 1).Equals("0")) Then
                str = FinancialAffairsUtility.LeftZeroTrim(src.Substring(1))
            End If
            Return str
        End Function

        Public Shared Sub SetZeroValueToLabels(ByVal ParamArray lblTargets As Label())
            Try 
                Dim label As Label
                For Each label In lblTargets
                    label.Text = "0"
                Next
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0  - 1) {})
            End Try
        End Sub

    End Class
End Namespace
