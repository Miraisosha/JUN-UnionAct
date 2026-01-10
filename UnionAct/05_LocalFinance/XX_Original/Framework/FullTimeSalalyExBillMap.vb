Imports System

Namespace Framework.Mapping
    Public Class FullTimeSalalyExBillMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(FullTimeSalalyExBillMap.mapFullTimeSalalyExBill)
        End Sub

        ' Fields
        Private Shared ReadOnly mapFullTimeSalalyExBill As ColumnMap() = New ColumnMap() { _
            New ColumnMap("k_salary_details", "賃金明細処理可否", GetType(String)), _
            New ColumnMap("s_salary_a", "合計本給" & "A" & "金額", GetType(Integer)), _
            New ColumnMap("s_salary_b", "合計本給" & "B" & "金額", GetType(Integer)), _
            New ColumnMap("d_overtime_work_time", "合計時間外時間", GetType(String)), _
            New ColumnMap("d_holiday_work_time", "合計休日労働時間", GetType(String)), _
            New ColumnMap("d_midnight_work_time", "合計深夜労働時間", GetType(String)), _
            New ColumnMap("d_old_overtime_work_time", "合計旧専従職員時間外時間", GetType(String)), _
            New ColumnMap("s_overtime_work_time", "合計専従職員時間外金額", GetType(Integer)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人" & "ID", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
