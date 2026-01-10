'===========================================================================================================
'   ネームスペース：MDMasterCommon
'   モジュールＩＤ：MDMasterCommon
'   モジュール名称：マスタメンテ共通
'   備考  　　　　：マスタメンテナンス用共通モジュール
'===========================================================================================================
Imports UnionAct.NSMDInfo

Module MDMasterCommon
    ' デフォルト適用終了日付
    Public Const DEFAULT_TO_DATE_STR As String = "9999/99/99"
    Public Const DEFAULT_TO_DATE As String = "99999999"
    Public Const DEFAULT_TO_DATE_WA As String = "9999年99月99日"

    ' 翌月初日の取得
    Public Function GetNextStartDate(ByVal d As DateTime) As DateTime
        Return d.AddDays(-(d.Day - 1)).AddMonths(1)
    End Function

    ' 当月初日の取得
    Public Function GetMonthTopDate(ByVal d As DateTime) As DateTime
        Return d.AddDays(-(d.Day - 1))
    End Function

    ' 当月末日の取得
    Public Function GetMonthLastDate(ByVal d As DateTime) As DateTime
        Return d.AddMonths(1).AddDays(-d.Day)
    End Function

    ' 未来月の判定
    Public Function IsFutureMonth(ByVal strDate As String) As Boolean
        Dim aDate As Date = DateValue(strDate)
        Dim nDate As Date = GetNextStartDate(Now)

        If aDate.Year < nDate.Year Then
            Return False
        ElseIf aDate.Year > nDate.Year Then
            Return True
        ElseIf aDate.Month >= nDate.Month Then
            Return True
        Else
            Return False
        End If
    End Function

    ' DateValue 9999年99月99日 9999/99/99対応
    Public Function DateValueStr(ByVal strDate As String) As String
        If strDate = "9999/99/99" Or strDate = "9999年99月99日" Then
            Return DEFAULT_TO_DATE
        Else
            Return Format(DateValue(strDate), "yyyyMMdd")
        End If
    End Function

    ' 処理日取得
    Public Function GetKeyDate() As String
        Dim strToday As String = Format(Date.Today, "yyyyMMdd")
        If MDLoginInfo.PeriodFrom <= strToday And strToday <= MDLoginInfo.PeriodTo Then
            Return strToday
        Else
            Return MDLoginInfo.PeriodTo
        End If
    End Function
End Module
