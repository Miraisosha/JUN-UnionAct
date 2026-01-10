#Region "MDApplyStrikeCommon"
'===========================================================================================================
'   モジュールＩＤ　　：MDApplyStrikeCommon
'   モジュール名称　　：時間内組合活動共通モジュール
'   備考  　　    　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Module MDApplyStrikeCommon

#Region "ログ出力オブジェクト"
    ' log4net
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Const SCREEN_ID = "MDApplyStrikeCommon"
    Private Const SCREEN_NAME = "MDApplyStrikeCommon"
#End Region

#Region "申請可能上限数取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetStrikeKindMeaning
    '   名称　：申請可能上限数取得処理
    '   概要  ：申請種類を元に、申請可能上限数を返す
    '   引数　：strApplyClassify 種別コード"01"等
    '   作成日：2012/01/19
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木)　新規作成 Fujisaku
    '***************************************************************************************************
    Friend Function GetApplyStrikeLimit(ByVal strApplyClassify As String) As String

        Dim retValue As String = 0                  ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            ' 定数詳細値取得
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "  l_name" & vbCrLf               ' 上限回数
            strSql = strSql & "FROM constant_dtl " & vbCrLf
            strSql = strSql & "WHERE c_constant = 'APPLY_STRIKE_LIMIT'"
            strSql = strSql & " AND c_constant_seq = '" & strApplyClassify & "' " & vbCrLf

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            retValue = tbRet.Rows(0).Item("l_name")

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
        Return retValue
    End Function

#End Region

#Region "現在申請数取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetStrikeKindMeaning
    '   名称　：現在申請数取得処理
    '   概要  ：申請種類と通期ＩＤを元に、現在の申請数を返す
    '   引数　：strApplyClassify  種別コード("01"等)
    '         ：strApplyMonth     対象申請の最小月("201112"等、union_info_c_period_idに対応)
    '         ：strUnionMeeting   現在入力中の中執活動/組合大会の組合大会会議通知会議番号
    '         ：strApplyArea      申請地区区分(01:東京, 02:大阪)　省略可、「04:各専門委員会、部会」の時のみ使用
    '   作成日：2012/01/19
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木)　新規作成 Fujisaku
    '         ：2012/02/04(土) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    Friend Function GetApplyCount(ByVal strApplyClassify As String, ByVal strApplyMonth As String, ByVal strUnionMeeting As String, Optional ByVal strApplyArea As String = "") As String
        Dim retValue As String = 0                  ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Dim strKsh As String = NSMDInfo.Ksh
        Dim strTermSt As String = ""
        Dim strTermEd As String = ""

        Try
            '対象期間終了月取得
            GetTermStartEnd(strApplyClassify, strApplyMonth, strTermSt, strTermEd)

            Select Case strApplyClassify
                Case "01", "02", "03"
                    ' 申請数（単位：回数）取得
                    strSql = ""
                    strSql = strSql & "SELECT a_strike.union_info_c_period_id," & vbCrLf
                    strSql = strSql & "       a_strike.union_info_c_union_meeting" & vbCrLf
                    strSql = strSql & "FROM apply_strike a_strike," & vbCrLf
                    strSql = strSql & "    (SELECT c_strike_id, k_apply_area, c_staf_id, d_strike" & vbCrLf
                    strSql = strSql & "     FROM apply_strike_member_date" & vbCrLf
                    strSql = strSql & "     WHERE FORMAT(d_strike, 'yyyyMM') >= " & strTermSt & "" & vbCrLf
                    strSql = strSql & "      AND  FORMAT(d_strike, 'yyyyMM') <= " & strTermEd & "" & vbCrLf
                    strSql = strSql & "      AND (c_cancel_strike_id is null OR c_cancel_strike_id = '' )" & vbCrLf
                    strSql = strSql & "    ) m_date " & vbCrLf
                    strSql = strSql & "WHERE a_strike.c_ksh = '" & strKsh & "'" & vbCrLf
                    If (strApplyClassify.Equals("01") OrElse strApplyClassify.Equals("02")) Then     ' 覚書(へ)の場合は01と02合算
                        strSql = strSql & " AND  (a_strike.k_apply_classify = '01'" & vbCrLf
                        strSql = strSql & "    OR a_strike.k_apply_classify = '02')" & vbCrLf
                    Else
                        strSql = strSql & " AND  a_strike.k_apply_classify = '" & strApplyClassify & "'" & vbCrLf
                    End If
                    strSql = strSql & " AND  a_strike.k_cancel = '0'" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_replace = '0'" & vbCrLf
                    strSql = strSql & " AND  a_strike.c_strike_id = m_date.c_strike_id" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_apply_area = m_date.k_apply_area" & vbCrLf
                    strSql = strSql & " AND  a_strike.union_info_c_union_meeting <> '" & strUnionMeeting & "' " & vbCrLf
                    strSql = strSql & "GROUP BY a_strike.union_info_c_period_id," & vbCrLf
                    strSql = strSql & "         a_strike.union_info_c_union_meeting" & vbCrLf

                Case "04", "05", "06", "07"
                    ' 申請数（単位：人日）取得
                    strSql = ""
                    strSql = strSql & "SELECT m_date.c_staf_id" & vbCrLf
                    strSql = strSql & "FROM apply_strike a_strike," & vbCrLf
                    strSql = strSql & "    (SELECT c_strike_id, k_apply_area, c_staf_id, d_strike" & vbCrLf
                    strSql = strSql & "     FROM apply_strike_member_date" & vbCrLf
                    strSql = strSql & "     WHERE FORMAT(d_strike, 'yyyyMM') >= " & strTermSt & "" & vbCrLf
                    strSql = strSql & "      AND  FORMAT(d_strike, 'yyyyMM') <= " & strTermEd & "" & vbCrLf
                    strSql = strSql & "      AND (c_cancel_strike_id is null OR c_cancel_strike_id = '' )" & vbCrLf
                    strSql = strSql & "     ) m_date " & vbCrLf
                    strSql = strSql & "WHERE a_strike.c_ksh = '" & strKsh & "'" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_apply_classify = '" & strApplyClassify & "'" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_cancel = '0'" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_replace = '0'" & vbCrLf
                    strSql = strSql & " AND  a_strike.c_strike_id = m_date.c_strike_id" & vbCrLf
                    strSql = strSql & " AND  a_strike.k_apply_area = m_date.k_apply_area" & vbCrLf
                    If strApplyClassify.Equals("04") And Not ChkNull(strApplyArea) Then  '04:専門部委員会、で、エリア指定時のみ"
                        strSql = strSql & " AND  a_strike.k_apply_area = '" & strApplyArea & "'"
                    End If
                Case Else
                    Return 0
            End Select

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            retValue = tbRet.Rows.Count

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return retValue
    End Function
#End Region

#Region "覚書を表示処理"
    '***************************************************************************************************
    '   ＩＤ　：ShowOboegaki
    '   名称　：覚書を表示処理
    '   概要  ：覚書のPDFを表示する
    '   引数　：なし
    '   作成日：2012/01/23
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月)　新規作成 Fujisaku
    '***************************************************************************************************
    Friend Function ShowOboegaki() As Boolean
        Dim blnRet As Boolean = False             ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Dim strFileName As String = "JikannaiOboe.pdf"
        Dim strFilePath As String

        Try
            ' SQL作成
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "  l_omission_name" & vbCrLf               ' 略名称
            strSql = strSql & "FROM constant_dtl " & vbCrLf
            strSql = strSql & "WHERE c_constant = 'OUTER_FILES_PATH'" & vbCrLf
            strSql = strSql & " AND  l_name = '" & strFileName & "'" & vbCrLf

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' 件数チェック
            If intRetCnt = 0 Then
                ' ファイルパス取得失敗時はエラー
                Call CLMsg.Show("BE0015")
                Return blnRet
            Else
                strFilePath = NVL(tbRet.Rows(0).Item(0))
                ' Windowsの関連付けで実行
                CreateObject("WScript.Shell").Run(Chr(34) & strFilePath & strFileName & Chr(34), vbNormalFocus)
            End If

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "GetTermStartEnd"
    Public Sub GetTermStartEnd(ByVal strApplyClassify As String, ByVal strKeyDate As String, ByRef strKeyDateS As String, ByRef strKeyDateE As String)
        Try
            Dim str As String = strKeyDate.Substring(0, 4)
            Dim num As Integer = Convert.ToInt32(str)
            Dim str2 As String = strKeyDate.Substring(4, 2)
            Dim num2 As Integer = Convert.ToInt32(str2)
            Select Case strApplyClassify
                Case "01", "02", "06", "07"
                    If ((num2 Mod 2) <> 0) Then
                        Exit Select
                    End If
                    strKeyDateS = (str & str2)
                    num2 += 1
                    If (num2 > 12) Then
                        num += 1
                        num2 = 1
                    End If
                    strKeyDateE = (num.ToString & num2.ToString("00"))
                    Return
                Case "03", "04"
                    If ((8 > num2) OrElse (num2 > 12)) Then
                        GoTo Label_017E
                    End If
                    strKeyDateS = (num.ToString & "08")
                    num += 1
                    strKeyDateE = (num.ToString & "07")
                    Return
                Case "05"
                    If ((9 > num2) OrElse (num2 > 12)) Then
                        GoTo Label_01E2
                    End If
                    strKeyDateS = (num.ToString & "09")
                    num += 1
                    strKeyDateE = (num.ToString & "08")
                    Return
                Case Else
                    Call CLMsg.Show("BE0018")
            End Select
            strKeyDateE = (str & str2)
            num2 -= 1
            If (num2 < 1) Then
                num -= 1
                num2 = 12
            End If
            strKeyDateS = (num.ToString & num2.ToString("00"))
            Return
Label_017E:
            strKeyDateE = (num.ToString & "07")
            num -= 1
            strKeyDateS = (num.ToString & "08")
            Return
Label_01E2:
            strKeyDateE = (num.ToString & "08")
            num -= 1
            strKeyDateS = (num.ToString & "09")
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "GetTermStartEnd")
        End Try
    End Sub
#End Region

#Region "setCounterByClassifyID"
    Public Sub setCounterByClassifyID(ByVal cn As Control, ByVal strClassifyID As String, ByVal strDate As String)
        Try
            Dim intPermission As String = GetApplyStrikeLimit(strClassifyID)
            Dim intCounter As String = GetApplyCount(strClassifyID, Format(Date.Parse(strDate), "yyyyMM"), "")
            cn.Controls("lblRestFrameCount").Text = CStr(CInt(intPermission) - CInt(intCounter)) + "回/" + CStr(intPermission) + "回"
            cn.Controls("lblUsedFrameCount").Text = CStr(intCounter) + "回"
            Dim strKikanStart As String = ""
            Dim strKikanEnd As String = ""
            GetTermStartEnd(strClassifyID, Format(Date.Parse(strDate), "yyyyMM"), strKikanStart, strKikanEnd)
            cn.Controls("lblDateFrom").Text = strKikanStart.Substring(0, 4) + "/" + strKikanStart.Substring(4, 2)
            cn.Controls("lblDateTo").Text = strKikanEnd.Substring(0, 4) + "/" + strKikanEnd.Substring(4, 2)

        Catch ex As Exception
            cn.Visible = False
        End Try
    End Sub
#End Region

#Region "社員番号より組合員の取得"
    '***************************************************************************************************
    '   ＩＤ　：GetUnionMemberData
    '   名称　：社員番号より組合員の取得
    '   概要　：受け取った社員番号の組合員情報をDataTable型で返却します
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：ロジックコピーしたが、DB接続を関数で渡すように修正
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Public Function GetUnionMemberData(ByVal clsDb As CLAccessMdb, ByVal strUserId As String) As DataTable
        '登録用SQL
        Dim strSql As String = String.Empty
        'SQL結果取得
        Dim dtRet As DataTable = Nothing
        '現在日付をスラッシュを除いた形で取得
        Dim strDateNow As String = Now.ToString("yyyyMMdd")

        Try
            'DB接続開始
            'clsDb.Connect()
            strSql = ""
            strSql = strSql & " SELECT attr1.c_user_id AS c_user_id, attr1.l_name AS l_name " & vbCrLf
            strSql = strSql & "      ,dtl1.l_name AS arealocal, dtl2.l_name AS model, dtl3.l_omission_name AS qualification " & vbCrLf
            strSql = strSql & "      ,dtl4.l_name AS belonging " & vbCrLf
            strSql = strSql & "      ,dtl1.l_omission_name AS local_omission, dtl2.l_omission_name AS model_omission " & vbCrLf
            strSql = strSql & "   FROM staf_attribute AS attr1, " & vbCrLf
            strSql = strSql & "        ( SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "            FROM staf_attribute " & vbCrLf
            strSql = strSql & "           WHERE d_from <= '" & strDateNow & "' " & vbCrLf '現在日以前の最新のユーザー情報
            strSql = strSql & "           GROUP BY c_user_id, c_ksh, c_staf_id ) AS attr2, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl1, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl2, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl3, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl4 " & vbCrLf
            strSql = strSql & "  WHERE attr1.c_user_id = '" & strUserId & "' " & vbCrLf '検索対象の社員番号
            strSql = strSql & "    AND attr1.c_user_id = attr2.c_user_id  " & vbCrLf
            strSql = strSql & "    AND attr1.c_ksh = attr2.c_ksh " & vbCrLf
            strSql = strSql & "    AND attr1.d_from = attr2.now_from " & vbCrLf
            strSql = strSql & "    AND attr1.k_staf_kind IN ('" & STAF_KIND_REGULAR & "','" & STAF_KIND_SENIOR & "')" & vbCrLf
            strSql = strSql & "    AND attr1.k_user_status = '" & USER_STATUS_ENTRY & "' " & vbCrLf
            strSql = strSql & "    AND dtl1.c_constant = 'AREA_LOCAL' AND dtl1.c_constant_seq = attr1.k_local " & vbCrLf
            strSql = strSql & "    AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr1.k_model " & vbCrLf
            strSql = strSql & "    AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr1.k_qualification " & vbCrLf
            strSql = strSql & "    AND dtl4.c_constant = 'BELONGING' AND dtl4.c_constant_seq = attr1.k_belonging " & vbCrLf
            strSql = strSql & "ORDER BY attr1.k_local, attr1.c_user_id "

            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "GetFightNumber")
            log.Fatal(ex.Message)

        Finally

        End Try

        Return dtRet
    End Function

#End Region

End Module

#End Region
