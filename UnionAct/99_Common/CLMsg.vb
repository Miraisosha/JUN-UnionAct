#Region "NSCLMsg"
'===========================================================================================================
'   ネームスペース：NSCLMsg
'   クラスＩＤ　　：CLMsg
'   クラス名称　　：メッセージ関連クラス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDFile
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDCommon
Imports UnionAct.GUI.Document

Namespace NSCLMsg
    Public Class CLMsg

#Region "定数"
        ' log4net
        Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "関数"
        '***************************************************************************************************
        '   ＩＤ　：ShowConnectErr
        '   名称　：接続エラーメッセージボックス表示処理
        '   概要　：メッセージＩＤを元に各処理からメッセージ情報を取得してメッセージボックスを表示する。
        '   引数　：ByVal pLngErrNo       As Long   = エラー番号 ( Err.Number )
        '           ByVal pStrErr         As String = エラー内容 ( Err.Description )
        '           ByVal pStrScreenId    As String = 画面ID
        '           ByVal pStrScreenName  As String = 画面名称
        '           ByVal pStrErrPosition As String = エラー箇所
        '   戻り値：なし
        '   作成日：2011/11/10(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/10(木)  m.suzuki  新規作成
        '       　：2012/08/14(火)  Fujisaku  同期処理追加
        '***************************************************************************************************
        Public Shared Sub ShowConnectErr(ByVal pLngErrNo As Long, _
                                         ByVal pStrErr As String, _
                                         ByVal pStrScreenId As String, _
                                         ByVal pStrScreenName As String, _
                                         ByVal pStrErrPosition As String)
            ' エラー内容
            Dim strMsg As String = ""
            Try
                ' エラーメッセージ表示
                Call CLMsg.Show("FE0003", _
                                pLngErrNo, _
                                pStrErr, _
                                pStrScreenId, _
                                pStrScreenName, _
                                pStrErrPosition)
                ' 終了前にデータ同期
                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True)
                ' アプリケーション終了
                End
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：ShowEtarnal
        '   名称　：致命的エラーメッセージボックス表示処理
        '   概要　：致命的エラーメッセージボックスを表示する。
        '   引数　：ByVal pLngErrNo       As Long   = エラー番号 ( Err.Number )
        '           ByVal pStrErr         As String = エラー内容 ( Err.Description )
        '           ByVal pStrScreenId    As String = 画面ID
        '           ByVal pStrScreenName  As String = 画面名称
        '           ByVal pStrErrPosition As String = エラー箇所
        '   戻り値：なし
        '   作成日：2011/11/10(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/10(木)  m.suzuki  新規作成
        '       　：2012/08/14(火)  Fujisaku  同期処理追加
        '***************************************************************************************************
        Public Shared Sub ShowEtarnal(ByVal pLngErrNo As Long, _
                                      ByVal pStrErr As String, _
                                      ByVal pStrScreenId As String, _
                                      ByVal pStrScreenName As String, _
                                      ByVal pStrErrPosition As String)
            ' エラー内容
            Dim strMsg As String = ""
            Try
                ' エラーメッセージ表示
                Call CLMsg.Show("FE0004", _
                                pLngErrNo, _
                                pStrErr, _
                                pStrScreenId, _
                                pStrScreenName, _
                                pStrErrPosition)

                ' 終了前にデータ同期
                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True)
                ' アプリケーション終了
                End
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：Show
        '   名称　：メッセージボックス表示処理
        '   概要　：メッセージＩＤを元に各処理からメッセージ情報を取得してメッセージボックスを表示する。
        '   引数　：ByVal pStrMsgId As String = メッセージID
        '           Optional ByVal pStrArg0  As String = 変換文字列0
        '           Optional ByVal pStrArg1  As String = 変換文字列1
        '           Optional ByVal pStrArg2  As String = 変換文字列2
        '           Optional ByVal pStrArg3  As String = 変換文字列3
        '           Optional ByVal pStrArg4  As String = 変換文字列4
        '   戻り値：押下ボタン
        '   作成日：2011/10/31(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/31(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>メッセージボックス表示処理</summary>
        ''' <param name="pStrMsgId">メッセージID</param>
        ''' <param name="pStrArg0">変換文字列0</param>
        ''' <param name="pStrArg1">変換文字列1</param>
        ''' <param name="pStrArg2">変換文字列2</param>
        ''' <param name="pStrArg3">変換文字列3</param>
        ''' <param name="pStrArg4">変換文字列4</param>
        ''' <returns>押下ボタン</returns>
        ''' <remarks></remarks>
        Public Shared Function Show(ByVal pStrMsgId As String, _
                                    Optional ByVal pStrArg0 As String = "", _
                                    Optional ByVal pStrArg1 As String = "", _
                                    Optional ByVal pStrArg2 As String = "", _
                                    Optional ByVal pStrArg3 As String = "", _
                                    Optional ByVal pStrArg4 As String = "") As DialogResult
            Dim DiaRet As DialogResult = Nothing                            ' メッセージボックス押下ボタン戻り値 
            Dim strMsgFrame As String = ""                                  ' メッセージ内容（枠）
            Dim strMsg As String = ""                                       ' メッセージ内容（完成版）
            Dim btnMsgButton As MessageBoxButtons = Nothing                 ' メッセージボタン
            Dim strMsgCaption As String = ""                                ' メッセージタイトル
            Dim icoMsgIcon As MessageBoxIcon = Nothing                      ' メッセージアイコン
            Dim MsgDefaultButton As MessageBoxDefaultButton = Nothing       ' メッセージデフォルトボタン
            Dim blnNotContinue As Boolean = False                           ' 処理継続フラグ（True：継続なし, False：継続あり）
            Dim blnOutputDb As Boolean = False                              ' データベース出力フラグ（True：出力あり, False：出力なし）
            Try
                '-----------------------------------------------------------------------------------
                '   メッセージID
                '-----------------------------------------------------------------------------------
                ' メッセージボックス存在チェック
                If ChkNull(pStrMsgId) Then
                    Call MessageBox.Show("メッセージIDがありません！")
                    Return DiaRet
                End If
                '-----------------------------------------------------------------------------------
                '   タイトル
                '-----------------------------------------------------------------------------------
                ' タイトル取得処理呼び出し
                strMsgCaption = GetTitile(pStrMsgId)
                ' タイトル存在チェック
                If ChkNull(strMsgCaption) Then
                    Call MessageBox.Show("メッセージタイトルができませんでした！")
                    Return DiaRet
                End If
                '-----------------------------------------------------------------------------------
                '   ボタン属性
                '-----------------------------------------------------------------------------------
                ' ボタン属性取得処理呼び出し
                btnMsgButton = GetButton(pStrMsgId)
                ' ボタン属性存在チェック
                If ChkNull(btnMsgButton) Then
                    Call MessageBox.Show("ボタン属性が取得できませんでした！")
                    Return DiaRet
                End If
                '-----------------------------------------------------------------------------------
                '   アイコン属性
                '-----------------------------------------------------------------------------------
                ' アイコン属性取得処理呼び出し
                icoMsgIcon = GetIcon(pStrMsgId)
                ' アイコン属性存在チェック
                If ChkNull(icoMsgIcon) Then
                    Call MessageBox.Show("アイコン属性が取得できませんでした！")
                    Return DiaRet
                End If
                '-----------------------------------------------------------------------------------
                '   メッセージ情報取得処理
                '-----------------------------------------------------------------------------------
                ' メッセージ情報取得処理呼び出し
                If GetMsgInfo(pStrMsgId, blnNotContinue, blnOutputDb, strMsgFrame) = False Then
                    Return DiaRet
                End If
                ' メッセージ内容存在チェック
                If ChkNull(strMsgFrame) Then
                    Call MessageBox.Show("メッセージ内容が取得できませんでした！")
                    Return DiaRet
                End If
                ' 処理継続フラグ存在チェック
                If ChkNull(blnNotContinue) Then
                    Call MessageBox.Show("処理継続フラグが取得できませんでした！")
                    Return DiaRet
                End If
                ' データベース出力フラグ存在チェック
                If ChkNull(blnOutputDb) Then
                    Call MessageBox.Show("データベース出力フラグが取得できませんでした！")
                    Return DiaRet
                End If
                '-----------------------------------------------------------------------------------
                '   メッセージ内容変換
                '-----------------------------------------------------------------------------------
                If (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") And (pStrArg3 <> "") And (pStrArg4 <> "") Then
                    ' 変換文字列5つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                    strMsg = strMsg.Replace("{3}", pStrArg3)
                    strMsg = strMsg.Replace("{4}", pStrArg4)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") And (pStrArg3 <> "") Then
                    ' 変換文字列4つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                    strMsg = strMsg.Replace("{3}", pStrArg3)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") Then
                    ' 変換文字列3つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") Then
                    ' 変換文字列2つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                ElseIf (pStrArg0 <> "") Then
                    ' 変換文字列1つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                Else
                    ' 変換文字列がない場合
                    strMsg = strMsgFrame
                End If
                '-----------------------------------------------------------------------------------
                '   メッセージボックスデフォルトボタン
                '-----------------------------------------------------------------------------------
                ' メッセージボックスデフォルトボタン取得
                If btnMsgButton = MessageBoxButtons.YesNo Then
                    ' ボタン属性が「はい・いいえ」の場合、２つのボタンとみなし、ボタン２をデフォルトにする
                    MsgDefaultButton = MessageBoxDefaultButton.Button2
                Else
                    ' その他の場合、１つのボタンとみなし、ボタン１をデフォルトにする
                    MsgDefaultButton = MessageBoxDefaultButton.Button1
                End If
                '-----------------------------------------------------------------------------------
                '   メッセージボックス表示
                '-----------------------------------------------------------------------------------
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
                ' しばらくお待ちくださいフォーム非表示
                Call FrmWaitInfo.CloseWaitForm()
                DiaRet = MessageBox.Show(strMsg, _
                                         strMsgCaption, _
                                         btnMsgButton, _
                                         icoMsgIcon, _
                                         MessageBoxDefaultButton.Button1)
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
                ' しばらくお待ちくださいフォーム非表示
                Call FrmWaitInfo.CloseWaitForm()
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID_CLMSG, _
                                       SCREEN_NAME_CLMSG, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値格納
            Return DiaRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GetMsg
        '   名称　：メッセージ内容取得処理
        '   概要　：メッセージＩＤを元に各処理からメッセージ情報を取得する。
        '   引数　：ByVal pStrMsgId         As String = メッセージID
        '           Optional ByVal pStrArg0 As String = 変換文字列0
        '           Optional ByVal pStrArg1 As String = 変換文字列1
        '           Optional ByVal pStrArg2 As String = 変換文字列2
        '           Optional ByVal pStrArg3 As String = 変換文字列3
        '           Optional ByVal pStrArg4 As String = 変換文字列4
        '   戻り値：メッセージ内容
        '   作成日：2011/11/07(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/07(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>メッセージ内容取得処理</summary>
        ''' <param name="pStrMsgId">メッセージID</param>
        ''' <param name="pStrArg0">変換文字列0</param>
        ''' <param name="pStrArg1">変換文字列1</param>
        ''' <param name="pStrArg2">変換文字列2</param>
        ''' <param name="pStrArg3">変換文字列3</param>
        ''' <param name="pStrArg4">変換文字列4</param>
        ''' <returns>メッセージ内容</returns>
        ''' <remarks></remarks>
        Public Shared Function GetMsg(ByVal pStrMsgId As String, _
                                      Optional ByVal pStrArg0 As String = "", _
                                      Optional ByVal pStrArg1 As String = "", _
                                      Optional ByVal pStrArg2 As String = "", _
                                      Optional ByVal pStrArg3 As String = "", _
                                      Optional ByVal pStrArg4 As String = "") As String
            Dim strRet As String = ""                                       ' メッセージ内容戻り値 
            Dim strMsgFrame As String = ""                                  ' メッセージ内容（枠）
            Dim strMsg As String = ""                                       ' メッセージ内容（完成版）
            Dim btnMsgButton As MessageBoxButtons = Nothing                 ' メッセージボタン
            Dim strMsgCaption As String = ""                                ' メッセージタイトル
            Dim icoMsgIcon As MessageBoxIcon = Nothing                      ' メッセージアイコン
            Dim MsgDefaultButton As MessageBoxDefaultButton = Nothing       ' メッセージデフォルトボタン
            Dim blnNotContinue As Boolean = False                           ' 処理継続フラグ（True：継続なし, False：継続あり）
            Dim blnOutputDb As Boolean = False                              ' データベース出力フラグ（True：出力あり, False：出力なし）
            Try
                '-----------------------------------------------------------------------------------
                '   メッセージID
                '-----------------------------------------------------------------------------------
                ' メッセージID存在チェック
                If ChkNull(pStrMsgId) Then
                    Call MessageBox.Show("メッセージIDがありません！")
                    Return strRet
                End If
                '-----------------------------------------------------------------------------------
                '   タイトル
                '-----------------------------------------------------------------------------------
                ' メッセージボックスタイトル取得処理呼び出し
                strMsgCaption = "メッセージボックス表示テスト"
                ' メッセージボックスタイトル存在チェック
                If ChkNull(strMsgCaption) Then
                    Call MessageBox.Show("メッセージタイトルができませんでした！")
                    Return strRet
                End If
                '-----------------------------------------------------------------------------------
                '   アイコン属性
                '-----------------------------------------------------------------------------------
                ' アイコン属性取得
                icoMsgIcon = MessageBoxIcon.Error
                '-----------------------------------------------------------------------------------
                '   メッセージ情報取得処理
                '-----------------------------------------------------------------------------------
                ' メッセージ情報取得処理呼び出し
                If GetMsgInfo(pStrMsgId, blnNotContinue, blnOutputDb, strMsgFrame) = False Then
                    Return strRet
                End If
                ' メッセージ内容存在チェック
                If ChkNull(strMsgFrame) Then
                    Call MessageBox.Show("メッセージ内容が取得できませんでした！")
                    Return strRet
                End If
                ' 処理継続フラグ存在チェック
                If ChkNull(blnNotContinue) Then
                    Call MessageBox.Show("処理継続フラグが取得できませんでした！")
                    Return strRet
                End If
                ' データベース出力フラグ存在チェック
                If ChkNull(blnOutputDb) Then
                    Call MessageBox.Show("データベース出力フラグが取得できませんでした！")
                    Return strRet
                End If
                ' 変換文字
                If (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") And (pStrArg3 <> "") And (pStrArg4 <> "") Then
                    ' 変換文字列5つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                    strMsg = strMsg.Replace("{3}", pStrArg3)
                    strMsg = strMsg.Replace("{4}", pStrArg4)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") And (pStrArg3 <> "") Then
                    ' 変換文字列4つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                    strMsg = strMsg.Replace("{3}", pStrArg3)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") And (pStrArg2 <> "") Then
                    ' 変換文字列3つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                    strMsg = strMsg.Replace("{2}", pStrArg2)
                ElseIf (pStrArg0 <> "") And (pStrArg1 <> "") Then
                    ' 変換文字列2つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                    strMsg = strMsg.Replace("{1}", pStrArg1)
                ElseIf (pStrArg0 <> "") Then
                    ' 変換文字列1つの場合
                    strMsg = strMsgFrame.Replace("{0}", pStrArg0)
                Else
                    ' 変換文字列がない場合
                    strMsg = strMsgFrame
                End If
                ' 処理結果設定
                strRet = strMsg
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID_CLMSG, _
                                       SCREEN_NAME_CLMSG, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値格納
            Return strRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GetButton
        '   名称　：ボタン属性取得処理
        '   概要　：メッセージIDの頭1文字からメッセージボックスボタンの属性を取得する。
        '   引数　：ByVal pStrMsgId As String = メッセージID
        '   戻り値：GetButton As MessageBoxButtons = メッセージボックスボタン
        '   作成日：2011/10/31(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/31(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>ボタン属性取得処理</summary>
        ''' <param name="pStrMsgId">メッセージID</param>
        ''' <returns>メッセージボックスボタン</returns>
        ''' <remarks></remarks>
        Public Shared Function GetButton(ByVal pStrMsgId As String) As MessageBoxButtons
            Dim btnMsgRet As MessageBoxButtons = Nothing                ' 処理結果
            Dim str1 As String = ""                                     ' メッセージID頭2文字目から1文字
            Dim str2 As String = ""                                     ' メッセージID頭1文字目から2文字
            Try
                ' メッセージIDの頭文字取得
                str1 = Mid(pStrMsgId, 2, 1)                             ' 頭2文字目から1文字取得　GW0001 → W
                str2 = Mid(pStrMsgId, 1, 2)                             ' 頭1文字目から2文字取得　GW0001 → GW
                ' メッセージID判定
                If pStrMsgId = "GW0001" Then
                    ' GW0001の場合
                    btnMsgRet = MessageBoxButtons.OK                    ' 「OK」ボタンのみ
                Else
                    ' GW0001ではない場合
                    If str2.Equals("GW") Then
                        btnMsgRet = MessageBoxButtons.YesNo             '「はい」「いいえ」ボタン
                    Else
                        If str1.Equals("Q") Then
                            btnMsgRet = MessageBoxButtons.YesNo         '「はい」「いいえ」ボタン
                        Else
                            btnMsgRet = MessageBoxButtons.OK            ' 「OK」ボタンのみ
                        End If
                    End If
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number,
                                       Err.Description,
                                       SCREEN_ID_CLMSG,
                                       SCREEN_NAME_CLMSG,
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値格納
            Return btnMsgRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GetTitle
        '   名称　：タイトル取得処理
        '   概要　：メッセージＩＤの頭2文字目からメッセージタイトルを取得する。
        '   引数　：ByVal pStrMsgId As String = メッセージＩＤ
        '   戻り値：GetTitle As String = メッセージタイトル
        '   作成日：2011/11/15(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/15(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>タイトル取得処理</summary>
        ''' <param name="pStrMsgId">メッセージＩＤ</param>
        ''' <returns>メッセージタイトル</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTitile(ByVal pStrMsgId As String) As String
            Dim strRet As String = ""       ' 処理結果
            Try
                ' メッセージIDの頭2文字目取得
                Dim str As String = Mid(pStrMsgId, 2, 1)
                If str.Equals("E") Then
                    ' エラー
                    strRet = "エラー"
                ElseIf str.Equals("W") Then
                    ' ワーニング
                    strRet = "注意"
                ElseIf str.Equals("I") Then
                    ' インフォメーション
                    strRet = "インフォメーション"
                ElseIf str.Equals("Q") Then
                    ' クエスチョン
                    strRet = "問合せ"
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number,
                                       Err.Description,
                                       SCREEN_ID_CLMSG,
                                       SCREEN_NAME_CLMSG,
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値格納
            Return strRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GetIcon
        '   名称　：アイコン属性取得処理
        '   概要　：メッセージＩＤの頭1文字からメッセージボックスアイコンの属性を取得する。
        '   引数　：ByVal pStrMsgId As String = メッセージＩＤ
        '   戻り値：GetIcon As MessageBoxIcon = メッセージボックスアイコン
        '   作成日：2011/10/31(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/31(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>アイコン属性取得処理</summary>
        ''' <param name="pStrMsgId">メッセージＩＤ</param>
        ''' <returns>メッセージボックスアイコン</returns>
        ''' <remarks></remarks>
        Public Shared Function GetIcon(ByVal pStrMsgId As String) As MessageBoxIcon
            Dim icoMsgRet As MessageBoxIcon = Nothing       ' 処理結果
            Try
                ' メッセージIDの頭2文字目取得
                Dim str As String = Mid(pStrMsgId, 2, 1)
                If str.Equals("E") Then
                    ' エラー
                    icoMsgRet = MessageBoxIcon.Error
                ElseIf str.Equals("W") Then
                    ' ワーニング
                    icoMsgRet = MessageBoxIcon.Warning
                ElseIf str.Equals("I") Then
                    ' インフォメーション
                    icoMsgRet = MessageBoxIcon.Information
                ElseIf str.Equals("Q") Then
                    ' クエスチョン
                    icoMsgRet = MessageBoxIcon.Question
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID_CLMSG, _
                                       SCREEN_NAME_CLMSG, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値格納
            Return icoMsgRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GetOutputMsg
        '   名称　：メッセージ内容取得処理
        '   概要　：メッセージIDからメッセージ内容を取得する。
        '   引数　：ByVal pStrMsgId       As String  = メッセージID
        '           ByRef pBlnContinueFlg As Boolean = 処理継続フラグ
        '           ByRef pBlnDbFlg       As Boolean = データベース出力フラグ
        '           ByRef pStrMsg         As String  = メッセージ内容
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/10/31(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/31(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>メッセージ情報取得処理</summary>
        ''' <param name="pStrMsgId">メッセージID</param>
        ''' <param name="pBlnContinueFlg">処理継続フラグ</param>
        ''' <param name="pBlnDbFlg">データベース出力フラグ</param>
        ''' <param name="pStrMsg">メッセージ内容</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Shared Function GetMsgInfo(ByVal pStrMsgId As String, _
                                          ByRef pBlnContinueFlg As Boolean, _
                                          ByRef pBlnDbFlg As Boolean, _
                                          ByRef pStrMsg As String) As Boolean
            Dim blnRet As Boolean = False                       ' 処理結果
            Dim blnContinueFlg As Boolean = Nothing             ' 処理継続フラグ
            Dim blnDbFlg As Boolean = Nothing                   ' データベース出力フラグ
            Dim strMsg As String = ""                           ' メッセージ内容
            Dim strXmlFile As String = ""                       ' XMLファイル
            Dim xmlNode As System.Xml.XmlNode = Nothing         ' メッセージIDに対するノード
            Dim xmlDoc As System.Xml.XmlDocument = Nothing      ' XMLドキュメントオブジェクト
            Try
                ' XMLファイル名取得（フルパス）
                strXmlFile = MDSystemInfo.MessagePath & MDSystemInfo.MessageName
                ' XMLファイル存在チェック
                If FileExists(strXmlFile) = False Then
                    Return blnRet
                End If
                ' XML読込
                xmlDoc = New System.Xml.XmlDocument             ' XMLドキュメントオブジェクト生成
                xmlDoc.Load(strXmlFile)                         ' XMLファイル読み込み
                ' メッセージIDに対するノードがあるかチェック取得
                If Not xmlDoc.SelectSingleNode("/ExceptionMsgData/Message[@ID='" & pStrMsgId & "']") Is Nothing Then
                    ' ある場合、メッセージID取得
                    xmlNode = xmlDoc.SelectSingleNode("/ExceptionMsgData/Message[@ID='" & pStrMsgId & "']")
                    ' 処理継続フラグ取得
                    If Not xmlNode.SelectSingleNode("NotContinue").InnerText Is Nothing Then
                        blnContinueFlg = System.Convert.ToBoolean(xmlNode.SelectSingleNode("NotContinue").InnerText)
                    Else
                        ' ない場合、異常で処理終了
                        Return blnRet
                    End If
                    ' データベース出力フラグ取得
                    If Not xmlNode.SelectSingleNode("OutputDb").InnerText Is Nothing Then
                        blnDbFlg = Convert.ToBoolean(System.Convert.ToBoolean(xmlNode.SelectSingleNode("OutputDb").InnerText))
                    Else
                        ' ない場合、異常で処理終了
                        Return blnRet
                    End If
                    ' メッセージ内容取得
                    If Not xmlNode.SelectSingleNode("OutputMsg").InnerText Is Nothing Then
                        strMsg = System.Convert.ToString(xmlNode.SelectSingleNode("OutputMsg").InnerText)
                        ' メッセージID : メッセージ内容
                        strMsg = pStrMsgId & " : " & strMsg
                    Else
                        ' ない場合、異常で処理終了
                        Return blnRet
                    End If
                Else
                    ' ない場合、異常で処理終了
                    Return blnRet
                End If
                ' アウトプット引数格納
                pBlnContinueFlg = blnContinueFlg
                pBlnDbFlg = blnDbFlg
                pStrMsg = strMsg
                ' 戻り値に正常を格納
                blnRet = True
                ' 処理終了
                Return blnRet
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID_CLMSG, _
                                       SCREEN_NAME_CLMSG, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
        End Function
#End Region

    End Class
End Namespace
#End Region
