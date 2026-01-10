#Region "FM080102"
'===========================================================================================================
'   クラスＩＤ　　：FM080102
'   クラス名称　　：労金データ作成－振込データ未作成の組合員
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common

Public Class FM080102
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Const MAX_DATE As String = "99999999"
    Private _strCloseDayKind As String = String.Empty
    Private _strCloseDate As String = String.Empty

#Region "コンストラクタ"
    Public Sub New(ByVal strPayKind As String, ByVal strCloseDate As String, ByVal strCloseDayKind As String)
        InitializeComponent()

        Me.lblPayKind.Text = strPayKind
        Me.lblPayClose.Text = strCloseDate
        _strCloseDate = strCloseDate
        _strCloseDayKind = strCloseDayKind
    End Sub
#End Region

#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：FM080102_Load
    '   名称　：
    '   概要　：
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub FM080102_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Me.Cursor = Cursors.WaitCursor
        '振込データ未作成組合員表示
        Call Me.ShowNotSendMember()
        Me.Cursor = Cursors.Default
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "OKボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnOK_Click
    '   名称　：OKボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
#End Region

#Region "キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：dgdNotMadeMember_KeyDown
    '   名称　：キーダウン
    '   概要　：
    '   作成日：2012/05/02(水) Fujisaku
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/05/02(水) Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub dgdNotMadeMember_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgdNotMadeMember.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Close()
        End If
    End Sub
#End Region

#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ShowNotSendMember
    '   名称　：選択された締日に対する振込データ未作成の組合員を表示します
    '   概要　：
    '   作成日：2012/02/03(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/03(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowNotSendMember()
        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim intCnt As Integer = 0
        '現在日付をスラッシュを除いた形で取得
        Dim strDateNow As String = Now.ToString("yyyyMMdd")

        Try
            strSql = "SELECT CLng(staf.c_staf_id) as 社員番号, " &
                     "       staf.c_dezit as ＣＤ, " &
                     "       staf.l_name as 名前, " &
                     "       staf.belonging as 組合支部, " &
                     "       CLOSE_MEM.s_pay as 金額 " &
                     "FROM staf_bank_close_member CLOSE_MEM " &
                     "LEFT JOIN " &
                     "(SELECT attr1.*,dtl1.l_name AS belonging " &
                     " FROM staf_attribute AS attr1, " &
                     "      (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " &
                     "       FROM staf_attribute " &
                     "       WHERE d_from <= '" & strDateNow & "' " &
                     "       GROUP BY c_user_id, c_ksh, c_staf_id " &
                     "      ) AS attr2, " &
                     "      constant_dtl AS dtl1 " &
                     " WHERE attr1.c_user_id = attr2.c_user_id " &
                     " AND attr1.c_staf_id = attr2.c_staf_id " &
                     " AND attr1.c_ksh = attr2.c_ksh " &
                     " AND attr1.d_from = attr2.now_from " &
                     " AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr1.k_belonging " &
                     ") AS staf " &
                     "ON  staf.c_user_id = CLOSE_MEM.c_user_id " &
                     "WHERE CLOSE_MEM.d_pay_close = '" & _strCloseDate.Replace("/", "") & "' " &
                     "AND CLOSE_MEM.k_daily_pay_kind = '" & _strCloseDayKind & "' " &
                     "AND d_bank_send = '" & MAX_DATE & "' " &
                     "ORDER BY CLng(STAF.c_staf_id) "

            'DB接続開始
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRet.Rows
                    Me.dgdNotMadeMember.Rows.Add()
                    '項番
                    Me.dgdNotMadeMember(0, intCnt).Value = intCnt + 1
                    '社員番号
                    Me.dgdNotMadeMember(1, intCnt).Value = dtRow.Item("社員番号")
                    'CD
                    Me.dgdNotMadeMember(2, intCnt).Value = dtRow.Item("ＣＤ")
                    '名前
                    Me.dgdNotMadeMember(3, intCnt).Value = dtRow.Item("名前")
                    '組合支部
                    Me.dgdNotMadeMember(4, intCnt).Value = dtRow.Item("組合支部")
                    '金額
                    Me.dgdNotMadeMember(5, intCnt).Value = dtRow.Item("金額")
                    intCnt = intCnt + 1
                Next
            End If
        Catch ex As Exception
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try
    End Sub
#End Region

End Class
#End Region
