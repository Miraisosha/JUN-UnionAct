#Region "DocTemplateBase"

Imports System.Data.OleDb
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDFile

Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel
Imports System.Runtime.InteropServices
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text

Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command


Public MustInherit Class DocTemplateBase

#Region "ログ出力オブジェクト"
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "プロパティ"
    Protected _docInf As DocumentInfo = Nothing 'ドキュメント情報
    Private xlsAP As Excel.Application
    Private xlsWBs As Excel.Workbooks
    Private xlsWB As Excel.Workbook
    Private xlsWS As Excel.Worksheet
#End Region

    ' Methods
    Public Sub New()
    End Sub

    Public Overridable Sub SetIniInfo(ByVal pDocInf As DocumentInfo, ByVal pAppExcel As Excel.Application, ByVal pWbBooks As Excel.Workbooks, ByVal pWbBook As Excel.Workbook, ByVal pWsOperation As Excel.Worksheet)
        xlsAP = pAppExcel
        xlsWBs = pWbBooks
        xlsWB = pWbBook
        xlsWS = pWsOperation
        _docInf = pDocInf

        '    Me.objLoginSession = objLoginSession
        '    Me.objDocumentData = objDocumentData
        '    Me.objExcelCommand = objExcelCommand
        '    Me.objExcelConst = objExcelConst
        '    Me.objSentenceData = New SentenceData(objExcelConst)
        '    objExcelCommand.SetScreenUpdating(False)
        '    Select Case objDocumentData.eStartFlg
        '        Case StartFlg.NEW
        '            Me.InitNewDocument
        '            Exit Select
        '        Case StartFlg.EDIT
        '            Me.InitEditDocument
        '            Exit Select
        '        Case StartFlg.COPYEDIT
        '            Me.InitCopyEditDocument
        '            Exit Select
        '    End Select
        '    objExcelCommand.SetScreenUpdating(True)
        '    objExcelCommand.SelectRange("A1:A1")
    End Sub


    Public Overridable Sub InitNewDocument()
        Try
            Me.SetColumnWidth("A1:AH1", 1.88!)
            Me.SetFontAll("ＭＳ" & " " & "明朝", 10.5!, False)
            Me.SetStringFormatAll()
            Me.SetPageNumber()
            Me.SetGridLines(False)
            Me.SetNormalView()
            Me.SetWorkbookTabs(False)
            Me.DeleteTurningPageAll()
            Me.SetTextHAlign(String.Format("{0}:{0}", Me.GetDocNumberCell), Constants.xlRight)
            Me.SetTextHAlign(String.Format("{0}:{0}", Me.GetIssueDateCell), Constants.xlRight)
            Dim today As DateTime = PublicCommand.GetToday
            Dim strDate As String = today.ToString("yyyyMMdd")
            Dim strDateFormat As String = PublicCommand.ConvertHanToZen(today.ToString("yyyy" & "年" & " MM" & "月" & " dd" & "日"))
            Me.ApplyIssueDate(strDate, strDateFormat)
            'ドキュメント番号の処理とりあえず米ントアウト
            'If (Me.objDocumentData.nDocNumber <= 0) Then
            '    Me.ApplyDocNumber(DocTemplateBase.ConvertHanToZen(Me.objDocumentData.strDocCode), Me.objDocumentData.strPeriodName, "＊＊", 0)
            'Else
            '    Dim strDocCode As String = DocTemplateBase.ConvertHanToZen(Me.objDocumentData.strDocCode)
            '    Dim strPeriod As String = PublicCommand.ConvertHanToZen(Me.objDocumentData.strPeriodName)
            '    Dim strDocNumber As String = PublicCommand.ConvertHanToZen(Me.objDocumentData.nDocNumber.ToString.PadLeft(2, "0"c))
            '    Me.ApplyDocNumber(strDocCode, strPeriod, strDocNumber, Me.objDocumentData.nDocNumber)
            'End If

            'Me.SetDefaultToFrom()
            Dim format As String = "A{0}:AH{0}"
            Dim num As Integer = PublicCommand.StrnumToInt(Me.GetSubjectCell)
            Me.MergeCell(String.Format(format, num.ToString), Constants.xlCenter)
            Me.SetFont(String.Format("{0}:{0}", Me.GetSubjectCell), "ＭＳ" & " " & "Ｐ明朝", 14.0!, True)
            'Dim strSubject As String = If(String.IsNullOrEmpty(Me.objDocumentData.strSubject), If(String.IsNullOrEmpty(Me.objDocumentData.strArbitrarySubject), "", Me.objDocumentData.strArbitrarySubject), Me.objDocumentData.strSubject)
            'strSubject = strSubject.Replace("{period}", Me.objDocumentData.strPeriodName)
            'Dim strSubject As String = _docInf.strSubject
            'strSubject = strSubject.Replace("{period}", PublicCommand.ConvertHanToZen(Me._docInf.strPeriodId))
            'Me.ApplySubject(strSubject)
            Me.UnLockedAll()
            Me.LockedCell((Me.GetDocNumberCell & ":" & Me.GetIssueDateCell))
            Me.LockedCell((Me.GetSubjectCell & ":" & Me.GetSubjectCell))
            'Catch exception As AppUnionException
            '    exception.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw exception
            'Catch exception2 As SysUnionException
            '    exception2.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw exception2
            'Catch exception3 As Exception
            '    Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
        Catch ex As Exception
            log.Fatal(ex.Message)
            MessageBox.Show(Err.Number.ToString & Err.Description & System.Reflection.MethodInfo.GetCurrentMethod.Name(), "エラー")
        End Try
    End Sub


    Public Overridable Sub ApplyAddDeleteMembers(ByRef nExcelRowNum As Integer)
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Try
            'If _docInf.ArrCommitteeUpdate Is Nothing OrElse _docInf.ArrCommitteeUpdate.Length = 0 Then
            '    Return
            'End If
            'Dim builder As New StringBuilder(" committee_update_list_dtl.c_committee_update = '" & _docInf.ArrCommitteeUpdate(0) & "' ")
            Dim i As Integer = 0
            'For i = 1 To _docInf.ArrCommitteeUpdate.Length - 1
            '    builder.Append(("OR committee_update_list_dtl.c_committee_update = '" & _docInf.ArrCommitteeUpdate(i) & "' "))
            'Next i

            Dim subCommitteeJoinSql = " LEFT JOIN committee " & vbCrLf
            subCommitteeJoinSql = subCommitteeJoinSql & " ON com_list.c_committee_id = committee.c_committee_id)" & vbCrLf
            subCommitteeJoinSql = subCommitteeJoinSql & ""

            Dim subCommitteeDtlJoinSql = " LEFT JOIN committee_dtl " & vbCrLf
            subCommitteeDtlJoinSql = subCommitteeDtlJoinSql & " ON (com_list.s_committee_seq = committee_dtl.s_committee_seq)" & vbCrLf
            subCommitteeDtlJoinSql = subCommitteeDtlJoinSql & " AND (com_list.c_committee_id = committee_dtl.c_committee_id))" & vbCrLf

            Dim subStaffJoinSql = " LEFT JOIN ( " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    SELECT" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.c_user_id AS user_id ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.l_name AS l_name ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.d_from AS d_from, " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      cd1.l_name as belonging_name " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    FROM staf_attribute AS staf_attr," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "         constant_dtl AS cd1," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      (SELECT c_user_id ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "              max(d_from) AS new_from" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       FROM staf_attribute " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       GROUP BY c_user_id" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      ) AS latest_attr  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    WHERE staf_attr.c_user_id = latest_attr.c_user_id  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    AND  staf_attr.d_from    = latest_attr.new_from  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    AND cd1.c_constant = 'BELONGING' AND cd1.c_constant_seq = staf_attr.k_belonging " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    ) AS staf_atr  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & " ON staf_atr.user_id = com_list.c_user_id " & vbCrLf

            Dim strMainSql As String = "SELECT " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_committee_update, " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_user_id AS 社員番号, " & vbCrLf
            strMainSql = strMainSql & "  staf_atr.l_name AS 名前, " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_committee_id, " & vbCrLf
            strMainSql = strMainSql & "  committee.l_name AS 委員会名, " & vbCrLf
            strMainSql = strMainSql & "  com_list.s_committee_seq, " & vbCrLf
            strMainSql = strMainSql & "  committee_dtl.l_name, " & vbCrLf
            strMainSql = strMainSql & "  com_list.k_committee_insert, " & vbCrLf
            strMainSql = strMainSql & "  staf_atr.belonging_name AS 支部 " & vbCrLf
            strMainSql = strMainSql & "FROM ((( " & vbCrLf
            strMainSql = strMainSql & "  SELECT " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_committee_update, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_user_id, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_committee_id , " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.s_committee_seq, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.k_committee_insert " & vbCrLf
            strMainSql = strMainSql & "  FROM committee_update_list_dtl" & vbCrLf
            'strMainSql = strMainSql & "  WHERE (" & builder.ToString & ")" & vbCrLf
            strMainSql = strMainSql & "   AND (k_committee_insert = '0' OR k_committee_insert = '1' ) " & vbCrLf
            strMainSql = strMainSql & "  ) AS com_list" & vbCrLf
          
            strMainSql = strMainSql & subCommitteeJoinSql & subCommitteeDtlJoinSql & subStaffJoinSql
            clsDb.Connect()
            Dim dtSqlResult As System.Data.DataTable = clsDb.ExecuteSql(strMainSql)
            Dim dtSqlResultSortable As System.Data.DataTable = New System.Data.DataTable
            If dtSqlResult.Rows.Count = 0 Then
                Return
            End If
            For intCnt = 0 To dtSqlResult.Columns.Count - 1
                dtSqlResultSortable.Columns.Add(dtSqlResult.Columns.Item(intCnt).ColumnName)
            Next
            dtSqlResultSortable.Columns.Add("s_staf_id", GetType(Integer))

            For intCnt = 0 To dtSqlResult.Rows.Count - 1
                dtSqlResultSortable.Rows.Add()
                For intCntCol = 0 To dtSqlResultSortable.Columns.Count - 1
                    If dtSqlResultSortable.Columns.Item(intCntCol).ColumnName = "s_staf_id" Then
                        Dim intUserId = 0
                        If Integer.TryParse(dtSqlResult.Rows(intCnt).Item("社員番号").ToString, intUserId) Then
                            dtSqlResultSortable.Rows(intCnt).Item(intCntCol) = intUserId
                        Else
                            dtSqlResultSortable.Rows(intCnt).Item(intCntCol) = dtSqlResult.Rows(intCnt).Item("社員番号")
                        End If
                    Else
                        dtSqlResultSortable.Rows(intCnt).Item(intCntCol) = dtSqlResult.Rows(intCnt).Item(intCntCol)
                    End If
                Next
            Next

            Dim num As Integer = 1
            Dim num2 As Byte = 0
            Dim dRowsMember As DataRow() = dtSqlResultSortable.Select("k_committee_insert = '0'", "c_committee_id, s_committee_seq, s_staf_id")

            If (dRowsMember.Length > 0) Then
                Me.WriteAddDeleteMembers((PublicCommand.ConvertHanToZen(num.ToString) & "．追加"), dRowsMember, nExcelRowNum)
                num += 1
                nExcelRowNum = (nExcelRowNum + 2)
                num2 = CByte((num2 Or 1))
            End If

            dRowsMember = dtSqlResultSortable.Select("k_committee_insert = '1'", "c_committee_id, s_committee_seq, s_staf_id")
            If (dRowsMember.Length > 0) Then
                Me.WriteAddDeleteMembers((PublicCommand.ConvertHanToZen(num.ToString) & "．削除"), dRowsMember, nExcelRowNum)
                num2 = CByte((num2 Or 2))
            End If

            ''追加のみならヘッダーは追加、削除のみなら削除、どちらもなら追加・削除にする
            'Dim strSubject As String = Me._docInf.strSubject
            'Select Case num2
            '    Case 1
            '        Me._docInf.strSubject = (strSubject.Substring(0, strSubject.IndexOf("の")) & "の追加について")
            '        Return
            '    Case 2
            '        Me._docInf.strSubject = (strSubject.Substring(0, strSubject.IndexOf("の")) & "の削除について")
            '        Return
            '    Case 3
            '        Me._docInf.strSubject = (strSubject.Substring(0, strSubject.IndexOf("の")) & "の追加・削除について")
            '        Return
            'End Select
        Catch ex As Exception
            log.Fatal(ex.Message)
            MessageBox.Show(Err.Number.ToString & Err.Description & System.Reflection.MethodInfo.GetCurrentMethod.Name(), "エラー")
        Finally
            clsDb.Disconnect()
        End Try

    End Sub


    Public Overridable Sub ApplyChangeMembers(ByRef nExcelRowNum As Integer)
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Try
            'If _docInf.ArrCommitteeUpdate Is Nothing OrElse _docInf.ArrCommitteeUpdate.Length = 0 Then
            '    Return
            'End If
            'Dim builder As New StringBuilder(" committee_update_list_dtl.c_committee_update = '" & _docInf.ArrCommitteeUpdate(0) & "' ")
            'Dim i As Integer
            'For i = 1 To _docInf.ArrCommitteeUpdate.Length - 1
            '    builder.Append(("OR committee_update_list_dtl.c_committee_update = '" & _docInf.ArrCommitteeUpdate(i) & "' "))
            'Next i

            Dim subCommitteeJoinSql = " LEFT JOIN committee " & vbCrLf
            subCommitteeJoinSql = subCommitteeJoinSql & " ON com_list.c_committee_id = committee.c_committee_id)" & vbCrLf
            subCommitteeJoinSql = subCommitteeJoinSql & ""

            Dim subCommitteeDtlJoinSql = " LEFT JOIN committee_dtl " & vbCrLf
            subCommitteeDtlJoinSql = subCommitteeDtlJoinSql & " ON (com_list.s_committee_seq = committee_dtl.s_committee_seq)" & vbCrLf
            subCommitteeDtlJoinSql = subCommitteeDtlJoinSql & " AND (com_list.c_committee_id = committee_dtl.c_committee_id))" & vbCrLf

            Dim subStaffJoinSql = " LEFT JOIN ( " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    SELECT" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.c_user_id AS user_id ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.l_name AS l_name ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      staf_attr.d_from AS d_from, " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      cd1.l_name as belonging_name " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    FROM staf_attribute AS staf_attr," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "         constant_dtl AS cd1," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      (SELECT c_user_id ," & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "              max(d_from) AS new_from" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       FROM staf_attribute " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "       GROUP BY c_user_id" & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "      ) AS latest_attr  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    WHERE staf_attr.c_user_id = latest_attr.c_user_id  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    AND  staf_attr.d_from    = latest_attr.new_from  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    AND cd1.c_constant = 'BELONGING' AND cd1.c_constant_seq = staf_attr.k_belonging " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & "    ) AS staf_atr  " & vbCrLf
            subStaffJoinSql = subStaffJoinSql & " ON staf_atr.user_id = com_list.c_user_id " & vbCrLf

            Dim strMainSql As String = "SELECT " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_committee_update, " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_user_id AS 社員番号, " & vbCrLf
            strMainSql = strMainSql & "  staf_atr.l_name AS 名前, " & vbCrLf
            strMainSql = strMainSql & "  com_list.c_committee_id, " & vbCrLf
            strMainSql = strMainSql & "  committee.l_name AS 委員会名, " & vbCrLf
            strMainSql = strMainSql & "  com_list.s_committee_seq, " & vbCrLf
            strMainSql = strMainSql & "  committee_dtl.l_name AS 役職名, " & vbCrLf
            strMainSql = strMainSql & "  com_list.k_committee_insert, " & vbCrLf
            strMainSql = strMainSql & "  staf_atr.belonging_name AS 支部, " & vbCrLf
            strMainSql = strMainSql & "  com_list.s_order " & vbCrLf
            strMainSql = strMainSql & "FROM ((( " & vbCrLf
            strMainSql = strMainSql & "  SELECT " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_committee_update, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_user_id, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.c_committee_id , " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.s_committee_seq, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.k_committee_insert, " & vbCrLf
            strMainSql = strMainSql & "    committee_update_list_dtl.s_order " & vbCrLf
            strMainSql = strMainSql & "  FROM committee_update_list_dtl" & vbCrLf
            'strMainSql = strMainSql & "  WHERE (" & builder.ToString & ")" & vbCrLf
            strMainSql = strMainSql & "   AND ((k_committee_insert = '0' OR k_committee_insert = '2') AND ( k_related_head = '1') ) " & vbCrLf
            strMainSql = strMainSql & "  ) AS com_list" & vbCrLf

            strMainSql = strMainSql & subCommitteeJoinSql & subCommitteeDtlJoinSql & subStaffJoinSql
            clsDb.Connect()
            Dim dtSqlResult As System.Data.DataTable = clsDb.ExecuteSql(strMainSql)
            Dim dtSqlResultSortable As System.Data.DataTable = New System.Data.DataTable
            If dtSqlResult.Rows.Count = 0 Then
                Return
            End If
            For intCnt = 0 To dtSqlResult.Columns.Count - 1
                dtSqlResultSortable.Columns.Add(dtSqlResult.Columns.Item(intCnt).ColumnName)
            Next
            For intCnt = 0 To dtSqlResult.Rows.Count - 1
                dtSqlResultSortable.Rows.Add()
                For intCntCol = 0 To dtSqlResultSortable.Columns.Count - 1
                    dtSqlResultSortable.Rows(intCnt).Item(intCntCol) = dtSqlResult.Rows(intCnt).Item(intCntCol)
                Next
            Next


            'Dim class2 As New FactoryBusClass
            'Dim command As ICommitteeUpdateListCommand = DirectCast(class2.GetObject("Business.Activity.CommitteeUpdateListCommand"), ICommitteeUpdateListCommand)
            'Dim dTable As DataTable = Command.GetCommitteeUpdateListDtl(Me.objDocumentData.strPeriodId, Me.objDocumentData.strAryCommitteeUpdate, Me.objDocumentData.strKeyDate, 0).Tables.Item("committee_update_list_dtl")
            If (dtSqlResultSortable.Rows.Count <> 0) Then
                Dim alphaString As String = DocTemplateBase.GetAlphaString(Me.GetMemberTblCell)
                Dim num As Integer = 1
                dtSqlResultSortable = PublicCommand.SortDataTable(dtSqlResultSortable, "s_order, s_committee_seq")
                For i = 0 To dtSqlResultSortable.Rows.Count - 1
                    Me.SetDataAppointCell((alphaString & CInt(nExcelRowNum).ToString), (PublicCommand.ConvertHanToZen(num.ToString) & "．"))
                    Me.SetDataAppointCell(("G" & CInt(nExcelRowNum).ToString), (dtSqlResultSortable.Rows.Item(i).Item("委員会名").ToString & "　" & dtSqlResultSortable.Rows.Item(i).Item("役職名").ToString))
                    nExcelRowNum += 1
                    Me.SetDataAppointCell(("J" & CInt(nExcelRowNum).ToString), dtSqlResultSortable.Rows.Item(i).Item("社員番号").ToString)
                    Me.SetDataAppointCell(("L" & CInt(nExcelRowNum).ToString), dtSqlResultSortable.Rows.Item(i).Item("名前").ToString)
                    Me.SetDataAppointCell(("R" & CInt(nExcelRowNum).ToString), ("（" & dtSqlResultSortable.Rows.Item(i).Item("支部").ToString & "）"))
                    Me.SetTextHAlign(String.Format("{0}:{0}", ("J" & CInt(nExcelRowNum).ToString)), Constants.xlRight)
                    num += 1
                    nExcelRowNum = (nExcelRowNum + 2)
                Next i
                '不明な処理とりあえず削除
                'Dim str2 As String = dtSqlResultSortable.Rows.Item(0).Item("委員会名").ToString
                'Dim str3 As String = dtSqlResultSortable.Rows.Item(0).Item("役職名").ToString
                'Dim ch As Char = str3.Chars(0)
                'Me.objDocumentData.strSubject = Me.objDocumentData.strSubject.Replace("部長、委員長", (str2 & If(ch.Equals("前"), str3.Substring(1), str3)))
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            MessageBox.Show(Err.Number.ToString & Err.Description & System.Reflection.MethodInfo.GetCurrentMethod.Name(), "エラー")

            '    Exception.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw Exception
            'Catch exception2 As SysUnionException
            '    exception2.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw exception2
            'Catch exception3 As Exception
            '    Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
        Finally
            clsDb.Disconnect()
        End Try

    End Sub

    Public Overridable Sub SetFixedSentence()
    End Sub

    Public Shared Function GetAlphaString(ByVal str As String) As String
        Dim str2 As String = Nothing
        Try
            Dim builder As New StringBuilder("")
            If (((str.Chars(0) < "A"c) OrElse (str.Chars(0) > "Z"c)) AndAlso ((str.Chars(0) < "a"c) OrElse (str.Chars(0) > "z"c))) Then
                Return ""
            End If
            builder.Append(str.Chars(0))
            Dim i As Integer
            For i = 1 To str.Length - 1
                If (((str.Chars(i) < "A"c) OrElse (str.Chars(i) > "Z"c)) AndAlso ((str.Chars(i) < "a"c) OrElse (str.Chars(i) > "z"c))) Then
                    Exit For
                End If
                builder.Append(str.Chars(i))
            Next i
            str2 = builder.ToString
        Catch exception As Exception

        End Try
        Return str2
    End Function
    Private Sub WriteAddDeleteMembers(ByVal strHead As String, ByVal dRowsMember As DataRow(), ByRef nExcelRowNum As Integer)

        Try
            'Dim strMainSql As String = ""  'SELECT文
            'Dim strWhereSql As String = "" 'SELECT文のWHERE句

            'strMainSql = strMainSql + strWhereSql
            'clsDb.Connect()
            'Dim dtSqlResult As System.Data.DataTable = clsDb.ExecuteSql(strMainSql)

            '追加or削除を表示
            'Me.objExcelCommand.SetDataAppointCell(("A" & CInt(nExcelRowNum).ToString), strHead)
            Me.SetDataAppointCell(("A" & CInt(nExcelRowNum).ToString), strHead)
            Dim str As String = ""
            Dim str2 As String = ""
            Dim i As Integer
            For i = 0 To dRowsMember.Length - 1
                Dim str3 As String = Nothing
                'If dRowsMember(i).Item("c_committee_id").ToString.Equals("002") Then
                '    str3 = Command.GetOtherHead(Me.objDocumentData.strPeriodId, dRowsMember(i).Item("c_committee_id").ToString, dRowsMember(i).Item("c_user_id").ToString, dRowsMember(i).Item("d_from").ToString)
                'End If
                If Not dRowsMember(i).Item("c_committee_id").ToString.Equals(str) Then
                    nExcelRowNum += 1
                    str = dRowsMember(i).Item("c_committee_id").ToString
                    Me.SetDataAppointCell(("E" & CInt(nExcelRowNum).ToString), dRowsMember(i).Item("委員会名").ToString)
                    nExcelRowNum += 1
                End If
                If (((i <= 0) OrElse Not dRowsMember(i).Item("c_committee_id").ToString.Equals(dRowsMember((i - 1)).Item("c_committee_id").ToString)) OrElse ((Integer.Parse(dRowsMember(i).Item("s_committee_seq").ToString) <> Integer.Parse(dRowsMember((i - 1)).Item("s_committee_seq").ToString)) OrElse Not dRowsMember(i).Item("c_user_id").ToString.Equals(str2))) Then
                    str2 = dRowsMember(i).Item("社員番号").ToString
                    Me.SetDataAppointCell(("G" & CInt(nExcelRowNum).ToString), dRowsMember(i).Item("社員番号").ToString)
                    Me.SetDataAppointCell(("I" & CInt(nExcelRowNum).ToString), dRowsMember(i).Item("名前").ToString)
                    Me.SetDataAppointCell(("N" & CInt(nExcelRowNum).ToString), ("（" & dRowsMember(i).Item("支部").ToString & "）"))
                    If Not String.IsNullOrEmpty(str3) Then
                        Me.SetDataAppointCell(("R" & CInt(nExcelRowNum).ToString), str3)
                    End If
                    Me.SetTextHAlign(String.Format("{0}:{0}", ("G" & CInt(nExcelRowNum).ToString)), Constants.xlRight)
                    nExcelRowNum += 1
                End If
            Next i
        Catch ex As Exception
            log.Fatal(ex.Message)
            MessageBox.Show(Err.Number.ToString & Err.Description & System.Reflection.MethodInfo.GetCurrentMethod.Name(), "エラー")

        End Try

    End Sub

    Public Function CallExcelMacro(ByVal strMacro As String, Optional ByVal pParam As Object() = Nothing) As Object
        Dim obj4 As Object = New Object()
        Try
            If pParam Is Nothing OrElse pParam.Length = 0 Then
                obj4 = Me.xlsAP.Run(strMacro)
            ElseIf pParam.Length = 1 Then
                obj4 = Me.xlsAP.Run(strMacro, pParam(0))
            ElseIf pParam.Length = 2 Then
                obj4 = Me.xlsAP.Run(strMacro, pParam(0), pParam(1))
            End If

        Catch exception As COMException
            Dim errorCode As Integer = exception.ErrorCode
            If (errorCode <= -2147352560) Then
                If (errorCode <= -2147417851) Then
                    Select Case errorCode
                        Case -2147418111
                            CLMsg.Show("OE0002")
                        Case -2147417851
                            CLMsg.Show("OE0002")
                    End Select
                ElseIf ((errorCode <> -2147417848) AndAlso (errorCode = -2147352560)) Then
                    Return Nothing
                End If
            Else
                Select Case errorCode
                    Case -2146827284
                        MessageBox.Show("Excel : " & "マクロ「" & strMacro & "」が見つからないか、セルが編集中のため、マクロ起動が出来ません。")
                    Case -2146788248
                        MessageBox.Show("Excel : " & "オブジェクト参照が設定されていません。")
                    Case -2146777998
                        CLMsg.Show("OE0002")
                    Case -2147417851
                        CLMsg.Show("OE0001")
                End Select
            End If
            Throw
        End Try
        Return obj4


    End Function

    
    Public Sub SetColumnWidth(ByVal strRange As String, ByVal width As Single)
        Me.CallExcelMacro("DocCommand.SetColumnWidth", New Object() {strRange, width})
    End Sub

    Public Sub MergeCell(ByVal strRange As String, ByVal xlAlign As Constants)
        Me.CallExcelMacro("DocCommand.MergeCell", New Object() {strRange, xlAlign})
    End Sub

    Public Sub UnLockedAll()
        Me.CallExcelMacro("DocCommand.UnLockedAll", New Object(0 - 1) {})
    End Sub

    Public Sub LockedCell(ByVal strRange As String)
        Me.CallExcelMacro("DocCommand.LockedCell", New Object() {strRange})
    End Sub

    Public Sub SetDataAppointCell(ByVal strCell As String, ByVal strData As String)
        If (PublicCommand.StrnumToInt(strCell) <> 0) Then
            Me.CallExcelMacro("DocCommand.SetDataAppointCellStr", New Object() {strCell, strData})
        End If
    End Sub

    Public Sub SetDataAppointCell(ByVal nRow As Integer, ByVal nCol As Integer, ByVal strData As String)
        If ((nRow <> 0) AndAlso (nCol <> 0)) Then
            Me.CallExcelMacro("DocCommand.SetDataAppointCellInt", New Object() {nRow, nCol, strData})
        End If
    End Sub

    Public Sub SetTextHAlign(ByVal strRange As String, ByVal xlAlign As Constants)
        Me.CallExcelMacro("DocCommand.SetTextHAlign", New Object() {strRange, xlAlign})
    End Sub

    Public Sub SetFontAll(ByVal strFont As String, ByVal fSize As Single, ByVal isBold As Boolean)
        Me.CallExcelMacro("DocCommand.SetFontAll", New Object() {strFont, fSize, isBold})
    End Sub

    Public Sub SetFont(ByVal strRange As String, ByVal strFont As String, ByVal fSize As Single, ByVal isBold As Boolean)
        Me.CallExcelMacro("DocCommand.SetFont", New Object() {strRange, strFont, fSize, isBold})
    End Sub

    Public Sub SetStringFormatAll()
        Me.CallExcelMacro("DocCommand.SetStringFormatAll", New Object(0 - 1) {})
    End Sub

    Public Sub SetPageNumber()
        Me.CallExcelMacro("DocCommand.SetPageNumber", New Object(0 - 1) {})
    End Sub

    Public Sub SetGridLines(ByVal isDisplay As Boolean)
        Me.CallExcelMacro("DocCommand.SetGridLines", New Object() {isDisplay})
    End Sub

    Public Sub SetNormalView()
        Me.CallExcelMacro("DocCommand.SetNormalView", New Object(0 - 1) {})
    End Sub

    Public Sub SetWorkbookTabs(ByVal isDisplay As Boolean)
        Me.CallExcelMacro("DocCommand.SetWorkbookTabs", New Object() {isDisplay})
    End Sub

    Public Sub DeleteTurningPageAll()
        Me.CallExcelMacro("DocCommand.DeleteTurningPageAll", New Object(0 - 1) {})
    End Sub

    Public Function GetNoticeSentence() As String
        Return Me.CallExcelMacro("GetNoticeSentence", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetNoticeSentenceCell() As String
        Return Me.CallExcelMacro("GetNoticeSentenceCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetDescription() As String
        Return Me.CallExcelMacro("GetDescription", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetDescriptionCell() As String
        Return Me.CallExcelMacro("GetDescriptionCell", New Object(0 - 1) {}).ToString
    End Function

    Public Sub SelectRange(ByVal strRange As String)
        Me.CallExcelMacro("DocCommand.SelectRange", New Object() {strRange})
    End Sub

    'sentencedata
    Public Function GetDocNumberCell() As String
        Return Me.CallExcelMacro("DocConst.GetDocNumberCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetIssueDateCell() As String
        Return Me.CallExcelMacro("DocConst.GetIssueDateCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetSubjectCell() As String
        Return Me.CallExcelMacro("DocConst.GetSubjectCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetClosingRemarks() As String
        Return Me.CallExcelMacro("GetClosingRemarks", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetClosingRemarksCell() As String
        Return Me.CallExcelMacro("GetClosingRemarksCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function GetMemberTblCell() As String
        Return Me.CallExcelMacro("DocConst.GetMemberTblCell", New Object(0 - 1) {}).ToString
    End Function

    Public Function SetArrayDataAppointCell(ByVal strStartCell As String, ByVal strAry As String(), ByVal nAddRows As Integer) As Integer
        Dim num As Integer = PublicCommand.StrnumToInt(strStartCell)
        Dim alphaString As String = DocTemplateBase.GetAlphaString(strStartCell)
        If ((strAry Is Nothing) OrElse (strAry.Length = 0)) Then
            Return num
        End If
        If (nAddRows = 0) Then
            Dim str2 As String = String.Join("", strAry)
            Me.CallExcelMacro("DocCommand.SetDataAppointCellStr", New Object() {strStartCell, str2})
            Return PublicCommand.StrnumToInt(strStartCell)
        End If
        Dim i As Integer
        For i = 0 To strAry.Length - 1
            Me.CallExcelMacro("DocCommand.SetDataAppointCellStr", New Object() {(alphaString & num.ToString), strAry(i)})
            num = (num + nAddRows)
        Next i
        Return (num - nAddRows)
    End Function


    Public Shared Function GetLfSplitStrArray(ByVal str As String) As String()
        Dim strArray2 As String() = Nothing
        Try
            If String.IsNullOrEmpty(str) Then
                Return New String(0 - 1) {}
            End If
            strArray2 = str.Split(New Char() {ChrW(10)})
        Catch exception As Exception

        End Try
        Return strArray2
    End Function

    Public Overridable Sub ApplyDocNumber(ByVal strDocCode As String, ByVal strPeriod As String, ByVal strDocNumber As String, ByVal nDocNumber As Integer)
        
    End Sub

    Public Overridable Sub ApplyFrom(ByVal strFromOrg As String, ByVal strFromName As String)
       
    End Sub

    Public Overridable Sub ApplyFrom(ByVal strFromOrg As String, ByVal strFromName As String, ByVal nAddRow As Integer)
       
    End Sub

    Public Overridable Sub ApplyIssueDate(ByVal strDate As String, ByVal strDateFormat As String)
     
    End Sub

    Public Overridable Sub ApplyMemberOfCommittee(ByRef nExcelRowNum As Integer)
        Try
            'Dim class2 As New FactoryBusClass
            'Dim committeeClassifiedList As DataSet = DirectCast(class2.GetObject("Business.Master.CommitteeClassifiedCommand"), ICommitteeClassifiedCommand).GetCommitteeClassifiedList(Me.objDocumentData.strKeyDate)
            'Dim table As System.Data.DataTable = committeeClassifiedList.Tables.Item("committee_classified")
            'Dim table2 As System.Data.DataTable = committeeClassifiedList.Tables.Item("committee_group")



            Dim strDate As String = ""
            Dim strSubSql As String = "  (SELECT " & vbCrLf
            strSubSql = strSubSql & "     c_classified_id," & vbCrLf
            strSubSql = strSubSql & "     d_from," & vbCrLf
            strSubSql = strSubSql & "     l_name AS 委員会分類名  ," & vbCrLf
            strSubSql = strSubSql & "     s_order " & vbCrLf
            strSubSql = strSubSql & "   FROM " & vbCrLf
            strSubSql = strSubSql & "     committee_classified" & vbCrLf
            strSubSql = strSubSql & "   WHERE" & vbCrLf
            strSubSql = strSubSql & "     d_from <= '" & strDate & "'" & vbCrLf
            strSubSql = strSubSql & "   AND d_to >= '" & strDate & "') committee_classified_A" & vbCrLf
            strSubSql = strSubSql & " " & vbCrLf

            Dim strLeftJoinSql As String = ""
            strLeftJoinSql = strLeftJoinSql & "LEFT OUTER JOIN " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "   (SELECT " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.c_classified_id,    " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.s_group_seq,    " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.d_from,    " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.s_order, " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.l_name AS 委員会名,  " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group_dtl.c_committee_id, " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group_dtl.l_name AS 支部名" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    FROM " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group, " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group_dtl   " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    WHERE " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.d_from <= :d_date  " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    AND" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.d_to >= :d_date" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    AND" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.c_classified_id = committee_group_dtl.c_classified_id  " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    AND" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.s_group_seq = committee_group_dtl.s_group_seq" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "    AND" & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "      committee_group.d_from = committee_group_dtl.d_from ) committee_group_A " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "ON  committee_classified_A.c_classified_id = committee_group_A.c_classified_id " & vbCrLf
            strLeftJoinSql = strLeftJoinSql & "AND committee_classified_A.d_from = committee_group_A.d_from " & vbCrLf

            Dim strOrderSql As String = ""
            strOrderSql = strOrderSql & "ORDER BY " & vbCrLf
            strOrderSql = strOrderSql & " committee_classified_A.s_order, committee_group_A.s_order, committee_group_A.c_committee_id" & vbCrLf


            Dim strMainSql As String = "SELECT " & vbCrLf
            strMainSql = strMainSql & "  committee_classified_A.c_classified_id, " & vbCrLf
            strMainSql = strMainSql & "  committee_group_A.s_group_seq, " & vbCrLf
            strMainSql = strMainSql & "  committee_group_A.c_committee_id, " & vbCrLf
            strMainSql = strMainSql & "  committee_classified_A.委員会分類名, " & vbCrLf
            strMainSql = strMainSql & "  committee.l_name AS 委員会名, " & vbCrLf
            strMainSql = strMainSql & "  committee_group_A.支部名, " & vbCrLf
            strMainSql = strMainSql & "  committee_classified_A.s_order AS hdr_s_order, " & vbCrLf
            strMainSql = strMainSql & "  committee_group_A.s_order AS dtl_s_order " & vbCrLf
            strMainSql = strMainSql & "FROM "
            strMainSql = strMainSql & strSubSql & strLeftJoinSql & strOrderSql


            'Dim num As Integer = nExcelRowNum
            'Dim num2 As Integer = nExcelRowNum
            'Dim num3 As Integer = 1
            'Dim flag As Boolean = False
            'Dim i As Integer
            'For i = 0 To table.Rows.Count - 1
            '    Dim flag2 As Boolean = False
            '    Dim str As String = table.Rows.Item(i).Item("c_classified_id").ToString
            '    Dim num5 As Integer = -1
            '    Dim isHeadWrite As Boolean = True
            '    num = nExcelRowNum
            '    If Not String.IsNullOrEmpty(table.Rows.Item(i).Item("委員会分類名").ToString) Then
            '        Me.SetFont(String.Format("{0}:{0}", ("B" & CInt(nExcelRowNum).ToString)), "ＭＳ" & " " & "明朝", 14.0!, True)
            '        Me.SetDataAppointCell(("B" & CInt(nExcelRowNum).ToString), table.Rows.Item(i).Item("委員会分類名").ToString)
            '        nExcelRowNum = (nExcelRowNum + 2)
            '    End If
            '    If str.Equals("04") Then
            '        isHeadWrite = False
            '    End If
            '    Dim rowArray As DataRow() = table2.Select(("c_classified_id = '" & str & "'"), "hdr_s_order, dtl_s_order, c_committee_id")
            '    Dim j As Integer
            '    For j = 0 To rowArray.Length - 1
            '        Dim num7 As Integer = Integer.Parse(rowArray(j).Item("s_group_seq").ToString)
            '        Dim str2 As String = rowArray(j).Item("c_committee_id").ToString
            '        Dim str3 As String = rowArray(j).Item("支部名").ToString
            '        If ((j < (rowArray.Length - 1)) AndAlso (num7 = Integer.Parse(rowArray((j + 1)).Item("s_group_seq").ToString))) Then
            '            Dim k As Integer
            '            For k = (j + 1) To rowArray.Length - 1
            '                If (num7 <> Integer.Parse(rowArray(k).Item("s_group_seq").ToString)) Then
            '                    Exit For
            '                End If
            '                str2 = (str2 & ", " & rowArray(k).Item("c_committee_id").ToString)
            '                str3 = (str3 & ", " & rowArray(k).Item("支部名").ToString)
            '            Next k
            '        End If
            '        num2 = nExcelRowNum
            '        If (num7 <> num5) Then
            '            num5 = num7
            '            Me.objExcelCommand.SetDataAppointCell(("A" & CInt(nExcelRowNum).ToString), ("（" & PublicCommand.ConvertHanToZen(num3.ToString) & "）" & rowArray(j).Item("委員会名").ToString))
            '            nExcelRowNum += 1
            '            If Not Me.WriteMemberOfCommittee(DocTemplateBase.GetCommaSplitStrArray(str2), DocTemplateBase.GetCommaSplitStrArray(str3), isHeadWrite, nExcelRowNum) Then
            '                Me.objExcelCommand.ClearAppointCell(String.Format("{0}:{0}", ("A" & num2.ToString)))
            '                nExcelRowNum = num2
            '            Else
            '                Dim num11 As Integer
            '                flag2 = True
            '                num3 += 1
            '                flag = True
            '                nExcelRowNum = num11 = (nExcelRowNum + 1)
            '                Me.objExcelCommand.AddTurningPage(num11)
            '            End If
            '        End If
            '    Next j
            '    If (Not flag2 AndAlso Not String.IsNullOrEmpty(table.Rows.Item(i).Item("委員会分類名").ToString)) Then
            '        Me.objExcelCommand.ClearAppointCell(String.Format("{0}:{0}", ("B" & num.ToString)))
            '        nExcelRowNum = num
            '    End If
            'Next i
            'Dim str4 As String = Me.objSentenceData.ClosingRemarksCell.Replace("{R}", CInt(nExcelRowNum).ToString)
            'Me.objExcelCommand.SetTextHAlign(String.Format("{0}:{0}", str4), Constants.xlRight)
            'Me.objExcelCommand.SetDataAppointCell(str4, Me.objSentenceData.ClosingRemarks)
            'If flag Then
            '    Me.objExcelCommand.SetScreenUpdating(True)
            '    Dim lastRow As Integer = Me.objExcelCommand.GetLastRow
            '    If (lastRow > 1) Then
            '        Me.objExcelCommand.SelectRange(String.Format("{0}:{0}", ("A" & lastRow.ToString)))
            '    End If
            '    Dim turningPageNum As Integer = Me.objExcelCommand.GetTurningPageNum
            '    If (turningPageNum > 1) Then
            '        Me.objExcelCommand.DeleteTurningPage(turningPageNum)
            '    End If
            'End If
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
    End Sub

    Public Overridable Sub ApplyOfficers()
    End Sub

    Public Overridable Sub ApplyPrefectureAirPort(ByRef nExcelRowNum As Integer)
        
    End Sub

    Public Overridable Sub ApplySubject(ByVal strSubject As String)
        Try
            Me.SetDataAppointCell(Me.GetSubjectCell, strSubject)
            'Me._docInf.strSubject = strSubject
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
        'Catch exception As AppUnionException
        '    exception.AddMethodName(MethodBase.GetCurrentMethod)
        '    Throw exception
        'Catch exception2 As SysUnionException
        '    exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '    Throw exception2
        'Catch exception3 As Exception
        '    Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
        'End Try
    End Sub

    Public Overridable Sub ApplyTo(ByVal strToOrg As String, ByVal strToName As String)
        
    End Sub

    Public Overridable Sub ApplyTo(ByVal strToOrg As String, ByVal strToName As String, ByVal nAddRow As Integer)
        
    End Sub



End Class


#End Region