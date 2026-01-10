#Region "MDNameStrikeCommon"
'===========================================================================================================
'   モジュールＩＤ　　：MDNameStrikeCommon
'   モジュール名称　　：指名ストライキ共通モジュール
'   備考  　　    　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb              ' ローカルレプリカ
Imports UnionAct.NSCLAccessMdbMst           ' サーバデザインマスタ
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Module MDNameStrikeCommon
    ' log4net
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private Const LOG_SCREEN_ID = "MDNameStrikeCommon"
    Private Const LOG_SCREEN_NAME = "MDNameStrikeCommon"
    '組合長
    Friend Const UNION_LEADERNAME_HEADER = "組合長"

    '指名ストライキ種別
    Friend Const STRIKE_KIND_NOTICE As String = "01"                                '通告
    Friend Const STRIKE_KIND_CALLOFF As String = "02"                               '解除

    '時間枠コード
    Friend Const TIME_FRAME_72 As String = "0" '72時間枠
    Friend Const TIME_FRAME_24 As String = "1" '24時間枠
    '通告番号種別
    Friend Const NOTICE_KIND As String = "Ｂ(ＡＮＡ宛)"
    '未確定通告書番号
    Friend Const NOTICE_NUMBER_UNDEFINE As String = "*****"
    '
    'Friend Const UPDATE_COUNT_FORMAT As String = "・・{0}・・"

    'name_strike用構造体
    Friend Structure nameStrikeStructureData
        Public intIndex As Integer
        Public strNameStrikeId As String
        Public strKsh As String
        Public strPeriodId As String
        Public strStrikeKind As String
        Public strNameStrikeNumber As String
        Public strNameStrikeKind As String
        Public strApplyArea As String
        Public strStandName As String
        Public strStrikeId As String
        Public dateInfo As String
        Public strFightNumber As String
        Public strCancelNumber As String
        Public strOperationFrom As String
        Public strOperationTimeFrom As String
        Public strOperationTo As String
        Public strOperationTimeTo As String
        Public strTimeFrame As String
        Public strRelatedNumber As String
        Public strRelatedNameStrikeId As String
        Public strNote As String
        Public dateInsert As String
        Public strInsertUserId As String
        Public dateUpdate As String
        Public strUpdateUserId As String
        Public intUpdateCount As Integer
    End Structure

    'name_strike_member_date用構造体
    Friend Structure nameStrikeMemberStructureData
        Public intIndex As Integer
        Public strNameStrikeId As String
        Public strUserId As String
        Public dateInfo As String
        Public strCancelNameStrikeId As String
        Public strCancelDate As String
        Public strNote As String
        Public dateInsert As String
        Public strInsertUserId As String
        Public dateUpdate As String
        Public strUpdateUserId As String
        Public intUpdateCount As Integer
    End Structure

#Region "登録されている組合員取得"
    '***************************************************************************************************
    '   ＩＤ　：GetBelongUnionMember
    '   名称　：登録されている組合員所属
    '   概要　：指名ストライキ文書に登録されている組合員を取得して返却します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function GetBelongUnionMember(ByVal strNameStrikeId As String) As DataTable
        'DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        'SQL
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing
        '現在日付
        Dim strNow As String = Now.ToString("yyyyMMdd")

        Try
            strSql = ""
            strSql = strSql & "SELECT t1.c_user_id AS c_user_id, t1.c_cancel_name_strike_id AS cancel_id, t1.d_strike, attr1.l_name AS c_name " & vbCrLf
            strSql = strSql & "      ,dtl1.l_name AS arealocal, dtl2.l_name AS model, dtl3.l_omission_name AS qualification " & vbCrLf
            strSql = strSql & "      ,dtl4.l_name AS belonging " & vbCrLf
            strSql = strSql & "      ,dtl1.l_omission_name AS local_omission, dtl2.l_omission_name AS model_omission " & vbCrLf
            strSql = strSql & "FROM name_strike_member_date AS t1, " & vbCrLf
            strSql = strSql & "     staf_attribute AS attr1, " & vbCrLf
            strSql = strSql & "     (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "      FROM staf_attribute " & vbCrLf
            strSql = strSql & "      WHERE d_from <= '" & strNow & "' " & vbCrLf '現在以前の最新のユーザー情報
            strSql = strSql & "      GROUP BY c_user_id, c_ksh, c_staf_id " & vbCrLf
            strSql = strSql & "     )  AS attr2, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl1, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl2, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl3, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl4 " & vbCrLf
            strSql = strSql & "WHERE t1.c_name_strike_id = '" & strNameStrikeId & "' " & vbCrLf '指名ストライキ文書ID
            strSql = strSql & "  AND t1.c_user_id = attr1.c_user_id " & vbCrLf
            strSql = strSql & "  AND attr1.c_user_id = attr2.c_user_id " & vbCrLf
            strSql = strSql & "  AND attr1.c_ksh     = attr2.c_ksh" & vbCrLf
            strSql = strSql & "  AND attr1.d_from    = attr2.now_from " & vbCrLf
            strSql = strSql & "  AND dtl1.c_constant = 'AREA_LOCAL' AND dtl1.c_constant_seq = attr1.k_local " & vbCrLf
            strSql = strSql & "  AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr1.k_model " & vbCrLf
            strSql = strSql & "  AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr1.k_qualification " & vbCrLf
            strSql = strSql & "  AND dtl4.c_constant = 'BELONGING' AND dtl4.c_constant_seq = attr1.k_belonging " & vbCrLf
            strSql = strSql & "ORDER BY CLng(t1.c_user_id) "
            'DB接続
            clsDb.Connect()

            'SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "ShowBelongUnionMember")
            log.Fatal(ex.Message)
        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function
#End Region

#Region "登録されている組合員取得（一時保存）"
    '***************************************************************************************************
    '   ＩＤ　：GetBelongUnionMember
    '   名称　：登録されている組合員取得（一時保存）
    '   概要　：指名ストライキ文書に登録されている組合員を取得して返却します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function GetBelongUnionMemberWork(ByVal intIndex As Integer) As DataTable
        'DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        'SQL
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing
        '現在日付
        Dim strNow As String = Now.ToString("yyyyMMdd")

        Try
            strSql = ""
            strSql = strSql & "SELECT t1.c_user_id AS c_user_id, t1.c_cancel_name_strike_id, t1.d_strike AS d_strike, attr1.l_name AS c_name " & vbCrLf
            strSql = strSql & "      ,dtl1.l_name AS arealocal, dtl2.l_name AS model, dtl3.l_omission_name AS qualification " & vbCrLf
            strSql = strSql & "      ,dtl4.l_name AS belonging " & vbCrLf
            strSql = strSql & "      ,dtl1.l_omission_name AS local_omission, dtl2.l_omission_name AS model_omission " & vbCrLf
            strSql = strSql & "FROM name_strike_member_date_work AS t1, " & vbCrLf
            strSql = strSql & "     staf_attribute AS attr1, " & vbCrLf
            strSql = strSql & "     (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "      FROM staf_attribute " & vbCrLf
            strSql = strSql & "      WHERE d_from <= '" & strNow & "' " & vbCrLf '現在以前の最新のユーザー情報
            strSql = strSql & "      GROUP BY c_user_id, c_ksh, c_staf_id " & vbCrLf
            strSql = strSql & "     )  AS attr2, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl1, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl2, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl3, " & vbCrLf
            strSql = strSql & "     constant_dtl AS dtl4 " & vbCrLf
            strSql = strSql & "WHERE t1.s_index = " & intIndex & vbCrLf '指名ストライキ文書ID
            strSql = strSql & "  AND t1.c_user_id = attr1.c_user_id " & vbCrLf
            strSql = strSql & "  AND attr1.c_user_id = attr2.c_user_id " & vbCrLf
            strSql = strSql & "  AND attr1.c_ksh     = attr2.c_ksh" & vbCrLf
            strSql = strSql & "  AND attr1.d_from    = attr2.now_from " & vbCrLf
            strSql = strSql & "  AND dtl1.c_constant = 'AREA_LOCAL' AND dtl1.c_constant_seq = attr1.k_local " & vbCrLf
            strSql = strSql & "  AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr1.k_model " & vbCrLf
            strSql = strSql & "  AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr1.k_qualification " & vbCrLf
            strSql = strSql & "  AND dtl4.c_constant = 'BELONGING' AND dtl4.c_constant_seq = attr1.k_belonging " & vbCrLf
            strSql = strSql & "ORDER BY CLng(t1.c_user_id) "
            'DB接続
            clsDb.Connect()

            'SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "ShowBelongUnionMember")
            log.Fatal(ex.Message)
        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function
#End Region

#Region "指名ストライキ文書の登録"
    '***************************************************************************************************
    '   ＩＤ　：InsertStrikeData
    '   名称　：指名ストライキ文書の登録
    '   概要　：指名ストライキ文書をname_strikeテーブルに登録します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function InsertNameStrikeData(
        ByVal clsDb As CLAccessMdb,
        ByVal iData As nameStrikeStructureData
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '指名ストライキ文書の登録
            strSql = "INSERT INTO name_strike ( " &
                     " c_name_strike_id " &
                     ",c_ksh " &
                     ",c_period_id " &
                     ",k_strike_info " &
                     ",c_name_strike_info " &
                     ",k_name_strike_kind " &
                     ",k_apply_area " &
                     ",l_stand_name " &
                     ",c_strike_id " &
                     ",d_info " &
                     ",c_fight " &
                     ",c_cancel " &
                     ",d_operation_from " &
                     ",d_operation_time_from " &
                     ",d_operation_to " &
                     ",d_operation_time_to " &
                     ",k_time_frame " &
                     ",c_related_info " &
                     ",c_really_name_strike_id " &
                     ",l_biko " &
                     ",d_ins " &
                     ",c_user_id_ins " &
                     ",d_up " &
                     ",c_user_id_up " &
                     ",s_up " &
                     ") VALUES( " &
                     "'" & iData.strNameStrikeId & "' " &
                     ",'" & iData.strKsh & "' " &
                     ",'" & iData.strPeriodId & "' " &
                     ",'" & iData.strStrikeKind & "' " &
                     ",'" & iData.strNameStrikeNumber & "' " &
                     ",'" & iData.strNameStrikeKind & "' " &
                     ",'" & iData.strApplyArea & "' " &
                     ",'" & iData.strStandName & "' " &
                     ",'" & iData.strStrikeId & "' " &
                     ",'" & iData.dateInfo & "' " &
                     ",'" & iData.strFightNumber & "' " &
                     ",'" & iData.strCancelNumber & "' " &
                     ",'" & iData.strOperationFrom & "' " &
                     ",'" & iData.strOperationTimeFrom & "' " &
                     ",'" & iData.strOperationTo & "' " &
                     ",'" & iData.strOperationTimeTo & "' " &
                     ",'" & iData.strTimeFrame & "' " &
                     ",'" & iData.strRelatedNumber & "' " &
                     ",'" & iData.strRelatedNameStrikeId & "' " &
                     ",'" & iData.strNote & "' " &
                     "," & iData.dateInsert &
                     ",'" & iData.strInsertUserId & "' " &
                     "," & iData.dateUpdate &
                     ",'" & iData.strUpdateUserId & "' " &
                     "," & iData.intUpdateCount & " )"

            '-------------------------------------------
            '   SQL実行
            '-------------------------------------------
            intRet = clsDb.ExecuteNonQueryKeyErr(strSql)

            'If intRet = -2 Or intRetMst = -2 Then
            If intRet = -2 Then
                'PK重複エラーの場合メッセージ表示後に異常終了を返却
                CLMsg.Show("DE0015")
                Return blnRet
            ElseIf intRet < 1 Then
                'ElseIf intRet < 1 And intRetMst < 1 Then
                '登録できていなかった場合異常終了を返却
                CLMsg.Show("DE0015")
                Return blnRet
            Else
                '問題なく登録できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()        ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function
#End Region

#Region "内容変更-指名ストライキ文書の更新"
    '***************************************************************************************************
    '   ＩＤ　：UpdateNameSrikeData
    '   名称　：内容変更-指名ストライキ文書の更新
    '   概要　：指名ストライキ文書を更新します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function UpdateNameStrikeData(
        ByVal clsDb As CLAccessMdb,
        ByVal uData As nameStrikeStructureData
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1
        'Dim intRetMst As Integer = -1

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '指名ストライキ文書の更新
            strSql = "UPDATE name_strike SET " &
                     " l_stand_name = '" & uData.strStandName & "' " &
                     ",d_operation_from = '" & uData.strOperationFrom & "' " &
                     ",d_operation_time_from = '" & uData.strOperationTimeFrom & "' " &
                     ",d_operation_to  = '" & uData.strOperationTo & "' " &
                     ",d_operation_time_to = '" & uData.strOperationTimeTo & "' " &
                     ",k_time_frame = '" & uData.strTimeFrame & "' " &
                     ",l_biko = '" & uData.strNote & "' " &
                     ",d_up = " & uData.dateUpdate &
                     ",c_user_id_up = '" & uData.strUpdateUserId & "' " &
                     ",s_up = " & uData.intUpdateCount &
                     " WHERE c_name_strike_id = '" & uData.strNameStrikeId & "' " '& _
            '" AND c_ksh = '" & uData.strKsh & "' " & _
            '" AND c_period_id = '" & uData.strPeriodId & "' "

            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQuery(strSql)              ' ローカルレプリカ

            ' 処理結果判定
            'If intRet < 1 And intRetMst < 1 Then
            If intRet < 1 Then
                '登録できていなかった場合異常終了を返却
                Return blnRet
            Else
                '問題なく登録できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()                ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function
#End Region

#Region "一時保存指名ストライキ文書の更新"
    '***************************************************************************************************
    '   ＩＤ　：UpdateNameStrikeDataWork
    '   名称　：一時保存指名ストライキ文書の更新
    '   概要　：一時保存指名ストライキ文書を更新します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function UpdateNameStrikeDataWork(
        ByVal clsDb As CLAccessMdb,
        ByVal uData As nameStrikeStructureData
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '指名ストライキ文書の更新
            strSql = "UPDATE name_strike_work SET " &
                     " l_stand_name = '" & uData.strStandName & "' " &
                     ",d_operation_from = '" & uData.strOperationFrom & "' " &
                     ",d_operation_time_from = '" & uData.strOperationTimeFrom & "' " &
                     ",d_operation_to = '" & uData.strOperationTo & "' " &
                     ",d_operation_time_to = '" & uData.strOperationTimeTo & "' " &
                     ",k_time_frame = '" & uData.strTimeFrame & "' " &
                     ",l_biko = '" & uData.strNote & "' " &
                     ",d_up = " & uData.dateUpdate &
                     ",c_user_id_up = '" & uData.strUpdateUserId & "' " &
                     ",s_up = " & uData.intUpdateCount &
                     " WHERE s_index = " & uData.intIndex

            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQuery(strSql)              ' ローカルレプリカ

            ' 処理結果判定
            'If intRet < 1 Or intRetMst < 1 Then
            If intRet < 1 Then
                '登録できていなかった場合異常終了を返却
                Return blnRet
            Else
                '問題なく登録できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            'ロールバック
            clsDb.RollbackTran()
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function
#End Region

#Region "指名ストライキ者の登録"
    '***************************************************************************************************
    '   ＩＤ　：InsertNameStrikeMemberData
    '   名称　：指名ストライキ者の登録
    '   概要　：指名ストライキ者のname_strike_member_dateテーブルに登録します
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function InsertNameStrikeMemberData(
        ByVal clsDb As CLAccessMdb,
        ByVal dataList As List(Of nameStrikeMemberStructureData)
    ) As Boolean

        Dim strSql As String = String.Empty
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ
        Dim blnRet As Boolean = False

        Try
            '
            For Each iData As nameStrikeMemberStructureData In dataList
                strSql = "INSERT INTO name_strike_member_date ( " &
                         " c_name_strike_id " &
                         ",c_user_id " &
                         ",d_strike " &
                         ",c_cancel_name_strike_id " &
                         ",d_cancel_info " &
                         ",l_biko " &
                         ",d_ins " &
                         ",c_user_id_ins " &
                         ",d_up " &
                         ",c_user_id_up " &
                         ",s_up " &
                         ") VALUES( " &
                         "'" & iData.strNameStrikeId & "' " &
                         ",'" & iData.strUserId & "' " &
                         "," & iData.dateInfo &
                         ",'" & iData.strCancelNameStrikeId & "' " &
                         "," & iData.strCancelDate &
                         ",'" & iData.strNote & "' " &
                         "," & iData.dateInsert &
                         ",'" & iData.strInsertUserId & "' " &
                         "," & iData.dateUpdate &
                         ",'" & iData.strUpdateUserId & "' " &
                         "," & iData.intUpdateCount & ") "

                '-------------------------------------------------------------------------------
                '   SQL実行
                '-------------------------------------------------------------------------------
                intRet = clsDb.ExecuteNonQuery(strSql)                  ' ローカルレプリカ

                ' 処理結果判定
                'If Not intRet = 1 Or Not intRetMst = 1 Then
                If Not intRet = 1 Then
                    '登録に失敗した場合処理終了
                    Return blnRet
                End If
            Next
            blnRet = True

        Catch ex As Exception
            ' トランザクション取り消し
            clsDb.RollbackTran()            ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertNameStrikeMemberData")
            log.Fatal(ex.Message)
        End Try

        Return blnRet
    End Function
#End Region

#Region "一部解除対象のストライキ者更新"
    '***************************************************************************************************
    '   ＩＤ　：UpdateReleaseMemberData
    '   名称　：一部解除時の指名ストライキ者更新
    '   概要　：解除対象となったストライキ者の、通告側の情報を更新します
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function UpdateReleaseMemberData(
        ByVal clsDb As CLAccessMdb,
        ByVal strOrgStrikeId As String,
        ByVal strNewStrikeId As String,
        ByVal strDateRelease As String,
        ByVal releaseList As List(Of String)
    ) As Boolean

        Dim strSql As String = String.Empty
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ
        Dim blnRet As Boolean = False

        Try
            '
            For Each strUserId As String In releaseList
                'TODO:一部解除フラグのチェック
                strSql = "UPDATE name_strike_member_date SET " &
                         "c_cancel_name_strike_id = '" & strNewStrikeId & "' " &
                         ",d_cancel_info = '" & strDateRelease & "' " &
                         "WHERE c_name_strike_id = '" & strOrgStrikeId & "' " &
                         "AND c_user_id = '" & strUserId & "' "

                '-------------------------------------------------------------------------------
                '   SQL実行
                '-------------------------------------------------------------------------------
                intRet = clsDb.ExecuteNonQuery(strSql)          ' ローカルレプリカ

                ' 処理結果判定
                'If Not intRet = 1 Or Not intRetMst = 1 Then
                If Not intRet = 1 Then
                    '登録に失敗した場合処理終了
                    Return blnRet
                End If
            Next
            blnRet = True
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()                ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "UpdateReleaseMemberData")
            log.Fatal(ex.Message)
        End Try

        Return blnRet
    End Function
#End Region

#Region "指名ストライキ者の削除"
    '***************************************************************************************************
    '   ＩＤ　：DeleteNameStrikeMemberData
    '   名称　：指名ストライキ者の削除
    '   概要　：内容変更時、文書に登録済みの指名ストライキ者をいったん削除します
    '   作成日：2012/01/17(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function DeleteNameStrikeMemberData(
        ByVal clsDb As CLAccessMdb,
        ByVal strNewNameStrikeId As String
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '指名ストライキ文書の更新
            strSql = "DELETE FROM name_strike_member_date " &
                     " WHERE c_name_strike_id = '" & strNewNameStrikeId & "' "
            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQuery(strSql)

            ' 処理結果判定
            'If intRet < 1 Or intRetMst < 1 Then
            If intRet < 1 Then
                '削除できていなかった場合異常終了を返却
                Return blnRet
            Else
                '問題なく削除できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()            ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function

#End Region

#Region "一時保存指名ストライキ者の削除"
    '***************************************************************************************************
    '   ＩＤ　：DeleteNameStrikeMemberDataWork
    '   名称　：一時保存指名ストライキ者の削除
    '   概要　：一時保存データを再度一時保存する際に文書に登録済みの指名ストライキ者をいったん削除します
    '   作成日：2012/01/17(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function DeleteNameStrikeWork(
        ByVal clsDb As CLAccessMdb,
        ByVal intNewIndex As Integer
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '一時保存の指名ストライキ者の削除
            strSql = "DELETE FROM name_strike_work " &
                     " WHERE s_index = " & intNewIndex

            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQuery(strSql)              ' ローカルレプリカ

            ' 処理結果判定
            'If intRet < 1 Or intRetMst < 1 Then
            If intRet < 1 Then
                '削除できていなかった場合異常終了を返却
                Return blnRet
            Else
                '問題なく削除できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()            ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function

#End Region

#Region "一時保存指名ストライキ者の削除"
    '***************************************************************************************************
    '   ＩＤ　：DeleteNameStrikeMemberDataWork
    '   名称　：一時保存指名ストライキ者の削除
    '   概要　：一時保存データを再度一時保存する際に文書に登録済みの指名ストライキ者をいったん削除します
    '   作成日：2012/01/17(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function DeleteNameStrikeMemberDataWork(
        ByVal clsDb As CLAccessMdb,
        ByVal intNewIndex As Integer
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '一時保存の指名ストライキ者の削除
            strSql = "DELETE FROM name_strike_member_date_work " &
                     " WHERE s_index = " & intNewIndex
            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQuery(strSql)              ' ローカルレプリカ

            ' 処理結果判定
            'If intRet < 0 Or intRetMst < 0 Then
            If intRet < 0 Then
                '削除できていなかった場合異常終了を返却
                Return blnRet
            Else
                '問題なく削除できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            'トランザクション取消
            clsDb.RollbackTran()            ' ローカルレプリカ
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, LOG_SCREEN_ID, LOG_SCREEN_NAME, "DeleteNameStrikeMemberDataWork")
        End Try

        Return blnRet
    End Function

#End Region

#Region "一時保存指名ストライキ文書の登録"
    '***************************************************************************************************
    '   ＩＤ　：InsertNameStrikeDataWork
    '   名称　：一時保存指名ストライキ文書の登録
    '   概要　：一時保存指名ストライキ文書をname_strike_workテーブルに登録します
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function InsertNameStrikeDataWork(
        ByVal clsDb As CLAccessMdb,
        ByVal iData As nameStrikeStructureData
    ) As Boolean

        '処理結果
        Dim blnRet As Boolean = False
        '登録用SQL
        Dim strSql As String = String.Empty
        '登録結果件数
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '一時保存指名ストライキ文書の登録
            strSql = "INSERT INTO name_strike_work ( " &
                     " s_index " &
                     " ,c_name_strike_id " &
                     ",c_ksh " &
                     ",c_period_id " &
                     ",k_strike_info " &
                     ",c_name_strike_info " &
                     ",k_name_strike_kind " &
                     ",k_apply_area " &
                     ",l_stand_name " &
                     ",c_strike_id " &
                     ",d_info " &
                     ",c_fight " &
                     ",c_cancel " &
                     ",d_operation_from " &
                     ",d_operation_time_from " &
                     ",d_operation_to " &
                     ",d_operation_time_to " &
                     ",k_time_frame " &
                     ",c_related_info " &
                     ",c_really_name_strike_id " &
                     ",l_biko " &
                     ",d_ins " &
                     ",c_user_id_ins " &
                     ",d_up " &
                     ",c_user_id_up " &
                     ",s_up " &
                     ") VALUES( " & iData.intIndex &
                     ",'" & iData.strNameStrikeId & "' " &
                     ",'" & iData.strKsh & "' " &
                     ",'" & iData.strPeriodId & "' " &
                     ",'" & iData.strStrikeKind & "' " &
                     ",'" & iData.strNameStrikeNumber & "' " &
                     ",'" & iData.strNameStrikeKind & "' " &
                     ",'" & iData.strApplyArea & "' " &
                     ",'" & iData.strStandName & "' " &
                     ",'" & iData.strStrikeId & "' " &
                     ",'" & iData.dateInfo & "' " &
                     ",'" & iData.strFightNumber & "' " &
                     ",'" & iData.strCancelNumber & "' " &
                     ",'" & iData.strOperationFrom & "' " &
                     ",'" & iData.strOperationTimeFrom & "' " &
                     ",'" & iData.strOperationTo & "' " &
                     ",'" & iData.strOperationTimeTo & "' " &
                     ",'" & iData.strTimeFrame & "' " &
                     ",'" & iData.strRelatedNumber & "' " &
                     ",'" & iData.strRelatedNameStrikeId & "' " &
                     ",'" & iData.strNote & "' " &
                     "," & iData.dateInsert &
                     ",'" & iData.strInsertUserId & "' " &
                     "," & iData.dateUpdate &
                     ",'" & iData.strUpdateUserId & "' " &
                     "," & iData.intUpdateCount & " )"

            '-----------------------------------------------------------------------------------
            '   SQL実行
            '-----------------------------------------------------------------------------------
            intRet = clsDb.ExecuteNonQueryKeyErr(strSql)            ' ローカルレプリカ

            ' 処理結果判定
            'If intRet = -2 Or intRetMst = -2 Then
            If intRet = -2 Then
                'PK重複エラーの場合メッセージ表示後に異常終了を返却
                CLMsg.Show("DE0015")
                Return blnRet
                'ElseIf intRet < 1 Or intRetMst < 1 Then
            ElseIf intRet < 1 Then
                '登録できていなかった場合異常終了を返却
                CLMsg.Show("DE0005")
                Return blnRet
            Else
                '問題なく登録できた場合正常終了を返却
                blnRet = True
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()                ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertStrikeData")
            log.Fatal(ex.Message)
        Finally
        End Try

        Return blnRet
    End Function
#End Region

#Region "一時保存指名ストライキ者の登録"
    '***************************************************************************************************
    '   ＩＤ　：InsertNameStrikeMemberDataWork
    '   名称　：一時保存指名ストライキ者の登録
    '   概要　：一時保存指名ストライキ者のname_strike_member_date_workテーブルに登録します
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function InsertNameStrikeMemberDataWork(
        ByVal clsDb As CLAccessMdb,
        ByVal dataList As List(Of nameStrikeMemberStructureData)
    ) As Boolean

        Dim strSql As String = String.Empty
        Dim intRet As Integer = -1              ' ローカルレプリカ
        'Dim intRetMst As Integer = -1           ' サーバデザインマスタ
        Dim blnRet As Boolean = False

        Try
            '
            For Each iData As nameStrikeMemberStructureData In dataList
                strSql = "INSERT INTO name_strike_member_date_work ( " &
                         " s_index " &
                         " ,c_name_strike_id " &
                         ",c_user_id " &
                         ",d_strike " &
                         ",c_cancel_name_strike_id " &
                         ",d_cancel_info " &
                         ",l_biko " &
                         ",d_ins " &
                         ",c_user_id_ins " &
                         ",d_up " &
                         ",c_user_id_up " &
                         ",s_up " &
                         ") VALUES( " &
                          iData.intIndex &
                         ",'" & iData.strNameStrikeId & "' " &
                         ",'" & iData.strUserId & "' " &
                         "," & iData.dateInfo &
                         ",'" & iData.strCancelNameStrikeId & "' " &
                         "," & iData.strCancelDate &
                         ",'" & iData.strNote & "' " &
                         "," & iData.dateInsert &
                         ",'" & iData.strInsertUserId & "' " &
                         "," & iData.dateUpdate &
                         ",'" & iData.strUpdateUserId & "' " &
                         "," & iData.intUpdateCount & ") "

                '-------------------------------------------------------------------------------
                '   SQL実行
                '-------------------------------------------------------------------------------
                intRet = clsDb.ExecuteNonQuery(strSql)              ' ローカルレプリカ

                ' 処理結果判定
                'If intRet = -1 Or intRetMst = -1 Then
                If intRet = -1 Then
                    '登録に失敗した場合処理終了
                    Return blnRet
                End If
            Next
            blnRet = True
        Catch ex As Exception
            ' トランザクション取消
            clsDb.RollbackTran()            ' ローカルレプリカ
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "InsertNameStrikeMemberData")
            log.Fatal(ex.Message)
        End Try

        Return blnRet
    End Function
#End Region

#Region "最新の通告番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetNoticeNumber
    '   名称　：最新の通告番号取得
    '   概要　：最新の通告番号を返却します
    '   作成日：2012/01/10(火) a.onuma
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Friend Function GetNoticeNumber() As String
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim dtrow As DataRow                                                                '一行のデータ
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim intSeqDb As Integer
        Dim intSeqText As Integer

        Try
            Call clsDb.Connect()
            '本登録テーブルからID取得
            strSql = "select max(CLng(c_strike_info))+1 as c_strike_max "
            strSql = strSql + " from   strike_list "
            strSql = strSql + " where  c_period_id = '" + NSMDInfo.PeriodId + "'"
            strSql = strSql + " group  by c_period_id "
            strSql = strSql + " UNION "
            strSql = strSql + " select max(CLng(c_name_strike_info))+1 as c_strike_max "
            strSql = strSql + " from   name_strike "
            strSql = strSql + " where  c_period_id = '" + NSMDInfo.PeriodId + "'"
            strSql = strSql + " group  by c_period_id "
            strSql = strSql + " order by 1 desc; "

            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                dtrow = dt.Rows(0)
                intSeqDb = CLng(CStr(dtrow("c_strike_max")))
            Else
                intSeqDb = 1
            End If

            ' TEXTから最新番号取得
            Dim strSeqName As String
            strSeqName = "seq_str_nms_" + NSMDInfo.PeriodId + ".txt"
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intSeqText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intSeqText = 1
            Catch ex As System.InvalidCastException
                intSeqText = 1
            End Try

            ' 値の大きいほうを採用
            If intSeqText >= intSeqDb Then
                Return intSeqText.ToString
            Else
                Return intSeqDb.ToString
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040202, SCREEN_ID_UC040202, "GetNoticeNumber")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try
    End Function

#End Region

#Region "最新の一部解除用通告番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetReleaseNumber
    '   名称　：最新の一部解除用通告番号取得
    '   概要　：最新の一部解除用通告番号を返却します
    '   作成日：2012/08/08(水) Fujisaku
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/08/08(水) Fujisaku  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Friend Function GetReleaseNumber(ByVal periodID As String) As String
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim dtrow As DataRow                                                                '一行のデータ
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim intSeqDb As Integer
        Dim intSeqText As Integer
        Try
            Call clsDb.Connect()
            '本登録テーブルからID取得
            strSql = ""
            strSql = strSql + "SELECT MAX(CLng(c_strike_info))+1 as c_strike_max "
            strSql = strSql + "FROM strike_list WHERE c_period_id = '" + periodID + "'"
            strSql = strSql + "GROUP BY c_period_id "
            strSql = strSql + "UNION "
            strSql = strSql + "SELECT MAX(CLng(c_name_strike_info))+1 as c_strike_max "
            strSql = strSql + "FROM name_strike WHERE c_period_id = '" + periodID + "'"
            strSql = strSql + "GROUP BY c_period_id "
            strSql = strSql + "ORDER BY 1 desc;"

            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                dtrow = dt.Rows(0)
                intSeqDb = CLng(CStr(dtrow("c_strike_max")))
            Else
                intSeqDb = 1
            End If

            ' TEXTから最新番号取得
            Dim strSeqName As String
            strSeqName = "seq_str_nms_" + periodID + ".txt"
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intSeqText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intSeqText = 1
            Catch ex As System.InvalidCastException
                intSeqText = 1
            End Try

            ' 値の大きいほうを採用
            If intSeqText >= intSeqDb Then
                Return intSeqText.ToString
            Else
                Return intSeqDb.ToString
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040403, SCREEN_ID_UC040403, "GetReleaseNumber")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try
    End Function

#End Region

#Region "最新の一時保存文書のインデックス取得"
    '***************************************************************************************************
    '   ＩＤ　：GetMaxIndex
    '   名称　：最新の一時保存文書のインデックス取得
    '   概要　：最新のインデックスを返却します
    '   作成日：2012/01/18(水) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function GetMaxIndex() As String
        Dim strMaxIndex As String = String.Empty
        'DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        '登録用SQL
        Dim strSql As String = String.Empty
        'SQL結果取得
        Dim dtRet As DataTable = Nothing
        '最大値保持用
        Dim intmaxNumber As Integer = 0

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '最大インデックスの取得
            strSql = "SELECT MAX(s_index) FROM name_strike_work "
            'DB接続開始
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows(0).Item(0).Equals(DBNull.Value) = False Then
                '問題なく取得できた場合、最大番号を受け取る
                intmaxNumber = dtRet.Rows(0).Item(0)
            End If
            intmaxNumber = intmaxNumber + 1
            strMaxIndex = intmaxNumber.ToString

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, LOG_SCREEN_ID, LOG_SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return strMaxIndex
    End Function

#End Region

#Region "最新の闘争または解除指令番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetFightNumber
    '   名称　：最新の闘争闘争または解除指令番号取得
    '   概要　：最新の闘争闘争または解除指令番号を返却します
    '   作成日：2012/01/10(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Friend Function GetFightCancelNumber(ByVal noticeKindFlg As String) As String
        Dim strFightNumber As String = String.Empty
        'DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        '登録用SQL
        Dim strSql As String = String.Empty
        'SQL結果取得
        Dim dtRet As DataTable = Nothing
        '最大値保持用
        Dim intmaxNumberDb As Integer = 1
        Dim intmaxNumberText As Integer = 1

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If noticeKindFlg = STRIKE_KIND_NOTICE Then
                strSql = "SELECT MAX(CLng(c_fight)) FROM name_strike  WHERE c_fight <> '' "
            Else
                strSql = "SELECT MAX(CLng(c_cancel)) FROM name_strike WHERE c_cancel <> '' "
            End If
            'DB接続開始
            clsDb.Connect()

            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows(0).Item(0).Equals(DBNull.Value) = False Then
                '最大の闘争指令番号 + 1
                intmaxNumberDb = dtRet.Rows(0).Item(0) + 1
            End If

            ' TEXTから最新番号取得
            Dim strSeqName As String
            If noticeKindFlg = STRIKE_KIND_NOTICE Then
                strSeqName = "seq_str_fit.txt"
            Else
                strSeqName = "seq_str_cnl.txt"
            End If
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intmaxNumberText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intmaxNumberText = 1
            Catch ex As System.InvalidCastException
                intmaxNumberText = 1
            End Try

            ' 値の大きいほうを採用
            If intmaxNumberText >= intmaxNumberDb Then
                strFightNumber = intmaxNumberText.ToString
            Else
                strFightNumber = intmaxNumberDb.ToString
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "GetFightNumber")
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Return strFightNumber
    End Function
#End Region

#Region "入力時間のチェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkStrikeTime
    '   名称　：入力時間のチェック
    '   概要　：入力された数値が不正でないかチェックします
    '   作成日：2012/01/10(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) a.onuma  新規作成
    '***************************************************************************************************
    Friend Function ChkStrikeTime(ByVal txtTarget As TextBox) As Boolean
        Dim blnRet As Boolean = False
        Dim text As String = txtTarget.Text

        If ChkNull(text) = False Then 'テキスト入力されているか
            If ChkNumber(text) = True Then '数値型として扱えるか
                If ((CInt(text) >= 0) AndAlso (CInt(text) <= 24)) Then
                    '0～24の範囲内であれば正常終了を返却
                    blnRet = True
                End If
            End If
        Else
            txtTarget.Text = "0"
            blnRet = True
        End If

        Return blnRet
    End Function

#End Region

#Region "開始時間～終了時間までの時間を求める"
    '***************************************************************************************************
    '   ＩＤ　：GetTimeSpan
    '   名称　：開始時間～終了時間までの時間を求める
    '   概要  ：開始時間～終了時間までの時間を求める
    '           (開始終了それぞれ日付と時間のみを文字列で取得)
    '   引数　：なし
    '   作成日：2012/01/13 新規作成 somezaki
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function GetTimeSpan(ByVal pStartDate As String, ByVal pStartTime As String, ByVal pEndDate As String, ByVal pEndTime As String) As String
        Dim returnVal = ""
        Try
            Dim dtStDate As Date = Nothing
            Dim dtEndDate As Date = Nothing
            Dim strStDate As String = ""
            Dim strEndDate As String = ""

            If pStartTime.Length > 2 OrElse pEndTime.Length > 2 Then
                Return returnVal
            End If

            '時刻が24の場合は日付を1プラスして時刻を0時として日付型に変換する
            dtStDate = CDate(pStartDate + " 00:00:00")
            dtEndDate = CDate(pEndDate + " 00:00:00")

            If pStartTime = "24" Then
                dtStDate = dtStDate.AddDays(1)
                strStDate = "00"
            Else
                strStDate = pStartTime
            End If

            If pEndTime = "24" Then
                dtEndDate = dtEndDate.AddDays(1)
                strEndDate = "00"
            Else
                strEndDate = pEndTime
            End If

            dtStDate = DateTime.Parse(dtStDate.ToString("yyyy") + "/" + dtStDate.ToString("MM") + "/" + dtStDate.ToString("dd") + " " + strStDate.PadLeft(2, "0") + ":00:00")
            dtEndDate = DateTime.Parse(dtEndDate.ToString("yyyy") + "/" + dtEndDate.ToString("MM") + "/" + dtEndDate.ToString("dd") + " " + strEndDate.PadLeft(2, "0") + ":00:00")
            returnVal = DateDiff(DateInterval.Hour, dtStDate, dtEndDate).ToString()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, LOG_SCREEN_ID, LOG_SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
            Return ""
        End Try
        Return returnVal

    End Function
#End Region

#Region "文字のバイト数を返す"
    '***************************************************************************************************
    '   ＩＤ　：LengthByte
    '   名称　：文字のバイト数を返す
    '   概要  ：文字のバイト数を返す
    '   引数　：なし
    '   作成日：2012/01/19 新規作成 somezaki
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function LengthByte(ByVal stTarget) As Integer
        Return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget)
    End Function
#End Region


#Region "ストライキ種別の名称取得"
    '***************************************************************************************************
    '   ＩＤ　：GetStrikeKindMeaning
    '   名称　：ストライキ種別名称取得
    '   概要  ："01""02"で入力された種別を"通告"or"解除"で返す
    '   引数　：なし
    '   作成日：2012/01/16 新規作成 somezaki
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function GetStrikeKindMeaning(ByVal pSource As String) As String
        Dim retValue As String = ""
        Try
            If pSource.Trim = STRIKE_KIND_NOTICE Then
                retValue = "通告"
            ElseIf pSource.Trim = STRIKE_KIND_CALLOFF Then
                retValue = "解除"
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, LOG_SCREEN_ID, LOG_SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        Return retValue
    End Function

#End Region

End Module

#End Region
