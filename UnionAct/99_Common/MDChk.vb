#Region "NSMDChk"
'===========================================================================================================
'   ネームスペース：NSMDChk
'   モジュールＩＤ：MDChk
'   モジュール名称：チェック関連モジュール
'   備考  　　　　：
'===========================================================================================================
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLAccessMdb

Namespace NSMDChk
    Public Module MDChk

#Region "定数・変数"
        ' 画面関連
        Private Const SCREEN_ID As String = SCREEN_ID_MDCHK             ' MDChk
        Private Const SCREEN_NAME As String = SCREEN_NAME_MDCHK         ' チェック関連モジュール
        ' log4net
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "関数"
        '***************************************************************************************************
        '   ＩＤ　：ChkNull
        '   名称　：NULLチェック処理
        '   概要　：文字列がNULLか空文字かEmptyのいづれかチェックする。
        '   引数　：ByVal pStr As String = チェック対象文字列
        '   戻り値：ByVal pStr As String = True ：Null/空文字/Empty,
        '                                  False：Null/空文字/Emptyではない
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        Public Function ChkNull(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = True                ' 処理結果
            Try
                '-------------------------------------------------
                '   Nothing判定
                '-------------------------------------------------
                If IsNothing(pStr) Then
                    Return blnRet
                End If
                '-------------------------------------------------
                '   空文字判定
                '-------------------------------------------------
                If pStr Is String.Empty Then
                    Return blnRet
                End If
                '-------------------------------------------------
                '   ""文字判定
                '-------------------------------------------------
                If pStr.Trim = "" Then
                    Return blnRet
                End If
                blnRet = False                          ' 処理結果に正常を設定
            Catch ex As Exception
                log.Fatal(ex.Message)                   ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                               ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkNumber
        '   名称　：数値チェック処理
        '   概要　：文字列が数値であるかチェックを行う。
        '   引数　：ByVal pStr As String = チェック対象文字列
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>数値チェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkNumber(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Try
                '-------------------------------------------------
                '   数値チェック
                '-------------------------------------------------
                If Double.TryParse(pStr, System.Globalization.NumberStyles.Any, Nothing, 0.0#) Then
                    blnRet = True               ' 正常
                Else
                    blnRet = False              ' 異常
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                       ' 戻り値格納
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkLength
        '   名称　：桁数チェック処理
        '   概要　：文字列の桁数が指定数を超えていないかチェックを行う。
        '   引数　：ByVal pStr As String  = チェック対象文字列,
        '           ByVal pInt As Integer = チェックする桁数
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>桁数チェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <param name="pInt">チェックする桁数</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkLength(ByVal pStr As String, _
                                  ByVal pInt As Integer) As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Try
                '-------------------------------------------------
                '   桁数チェック
                '-------------------------------------------------
                If pStr.Length > pInt Then
                    blnRet = False              ' 異常
                Else
                    blnRet = True               ' 正常
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                       ' 戻り値格納
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkLengthB
        '   名称　：バイト数チェック処理
        '   概要　：文字列のバイト数が指定数を超えていないかチェックを行う。
        '   引数　：ByVal pStr As String  = チェック対象文字列,
        '           ByVal pInt As Integer = チェックするバイト数
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/11/22(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/22(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>バイト数チェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <param name="pInt">チェックするバイト数</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkLengthB(ByVal pStr As String, _
                                   ByVal pInt As Integer) As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Try
                '-------------------------------------------------
                '   バイト数チェック
                '-------------------------------------------------
                If System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(pStr) > pInt Then
                    blnRet = False              ' 異常
                Else
                    blnRet = True               ' 正常
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                       ' 戻り値格納
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkDate
        '   名称　：日付チェック処理
        '   概要　：文字列が日付であるかチェックを行う。
        '   引数　：ByVal pStr As String  = チェック対象文字列
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>日付チェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkDate(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = False                                           ' 処理結果
            Dim strTarget As String = ""                                            ' チェック対象文字列
            Dim strYear As String = ""                                              ' 年
            Dim strMonth As String = ""                                             ' 月
            Dim strDay As String = ""                                               ' 日
            Dim intLastDay As Integer = 0                                           ' 月末日
            Try
                ' スラッシュがあれば、スラッシュを消す
                strTarget = pStr.Replace("/", "").Replace("-", "")
                '-------------------------------------------------------------------
                '   桁数チェック
                '-------------------------------------------------------------------
                If ChkLength(strTarget, 8) = False Then                             ' 8桁かチェック
                    Return blnRet
                End If
                '-------------------------------------------------------------------
                '   年チェック
                '-------------------------------------------------------------------
                strYear = Mid(strTarget, 1, 4)                                      ' 年取得
                If ChkNumber(strYear) = False Then                                  ' 数値チェック
                    Return blnRet
                End If
                If (CInt(strYear) < 1900) Or (CInt(strYear) > 2099) Then            ' 範囲チェック（1900～2099）
                    Return blnRet
                End If
                '-------------------------------------------------------------------
                '   月チェック
                '-------------------------------------------------------------------
                strMonth = Mid(strTarget, 5, 2)                                     ' 月取得
                If ChkNumber(strMonth) = False Then                                 ' 数値チェック
                    Return blnRet
                End If
                If (CInt(strMonth) < 1) Or (CInt(strMonth) > 12) Then               ' 範囲チェック（0～12）
                    Return blnRet
                End If
                '-------------------------------------------------------------------
                '   日チェック
                '-------------------------------------------------------------------
                strDay = Mid(strTarget, 7, 2)                                       ' 日取得
                If ChkNumber(strDay) = False Then                                   ' 数値チェック
                    Return blnRet
                End If
                intLastDay = DateTime.DaysInMonth(CInt(strYear), CInt(strMonth))    ' 月末日取得
                If (CInt(strDay) < 1) Or (CInt(strDay) > intLastDay) Then           ' 範囲チェック（1～月末日）
                    Return blnRet
                End If
                blnRet = True                                                       ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                                                           ' 戻り値格納
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkTime
        '   名称　：時刻チェック処理
        '   概要　：文字列が時刻であるかチェックを行う。
        '   引数　：ByVal pStr As String  = チェック対象文字列
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/12/23(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/12/23(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>時刻チェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkTime(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = False                                           ' 処理結果
            Dim strTarget As String = ""                                            ' チェック対象文字列
            Dim strHour As String = ""                                              ' 時
            Dim strMinutes As String = ""                                           ' 分
            Try
                ' スラッシュがあれば、スラッシュを消す
                strTarget = pStr.Replace(":", "")
                '-------------------------------------------------------------------
                '   桁数チェック
                '-------------------------------------------------------------------
                If ChkLength(strTarget, 4) = False Then                             ' 4桁かチェック
                    Return blnRet
                End If
                '-------------------------------------------------------------------
                '   時チェック
                '-------------------------------------------------------------------
                strHour = Mid(strTarget, 1, 2)                                      ' 時取得
                If ChkNumber(strHour) = False Then                                  ' 数値チェック
                    Return blnRet
                End If
                If (CInt(strHour) < 0) Or (CInt(strHour) > 23) Then                 ' 範囲チェック（00～23）
                    Return blnRet
                End If
                '-------------------------------------------------------------------
                '   分チェック
                '-------------------------------------------------------------------
                strMinutes = Mid(strTarget, 3, 2)                                   ' 月取得
                If ChkNumber(strMinutes) = False Then                               ' 数値チェック
                    Return blnRet
                End If
                If (CInt(strMinutes) < 0) Or (CInt(strMinutes) > 59) Then           ' 範囲チェック（0～59）
                    Return blnRet
                End If
                blnRet = True                                                       ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                                                           ' 戻り値格納
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkHankakuKana
        '   名称　：カナチェック処理
        '   概要　：文字列がカナかチェックを行う。
        '   引数　：ByVal pStr As String = チェック対象文字列
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/11/17(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/17(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>カナチェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkHankakuKana(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = False   ' 処理結果
            Dim intChk As Integer = 0       ' Unicode
            Try
                '===============================================================
                '   半角カタカナチェック
                '   Unicode
                '   " "(半角スペース) = 32
                '   "､"               = 65380
                '   "ﾟ"               = 65439
                '===============================================================
                ' 文字数分チェック
                For i = 1 To pStr.Length
                    intChk = AscW(Mid(pStr, i, 1))
                    ' " "(半角スペース)、"､"～"ﾟ" までを半角カタカナと判定
                    If Not ((intChk = 32) Or ((intChk >= 65380) And (intChk <= 65439))) Then
                        Return blnRet
                    End If
                Next
                blnRet = True
            Catch ex As Exception
                log.Fatal(ex.Message)       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkHankakuBigAlpha
        '   名称　：半角大文字アルファベットチェック処理
        '   概要　：文字列が半角大文字アルファベットかチェックを行う。
        '   引数　：ByVal pStr As String = チェック対象文字列
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/12/28(水)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/12/28(水)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>半角大文字アルファベットチェック処理</summary>
        ''' <param name="pStr">チェック対象文字列</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function ChkHankakuBigAlpha(ByVal pStr As String) As Boolean
            Dim blnRet As Boolean = False   ' 処理結果
            Dim intChk As Integer = 0       ' Unicode
            Try
                '===============================================================
                '   半角大文字アルファベットチェック
                '   Unicode
                '   "A" = 65
                '   "Z" = 90
                '===============================================================
                ' 文字数分チェック
                For i = 1 To pStr.Length
                    intChk = AscW(Mid(pStr, i, 1))
                    ' "A"～"Z" までを半角大文字アルファベットと判定
                    If Not ((intChk >= 65) And (intChk <= 90)) Then
                        Return blnRet
                    End If
                Next
                blnRet = True
            Catch ex As Exception
                log.Fatal(ex.Message)       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkDisplaySize
        '   名称　：画面解像度チェック処理
        '   概要  ：画面解像度のサイズが1280×1024かチェックを行う。
        '   引数　：なし
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/01(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/01(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>画面解像度チェック処理</summary>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkDisplaySize() As Boolean
            Dim blnRet As Boolean = False   ' 処理結果
            Try
                ' モニタサイズ取得
                Dim size As System.Drawing.Size = SystemInformation.PrimaryMonitorSize
                ' サイズチェックメッセージボックス
                'MessageBox.Show(size.Width & "×" & size.Height)
                ' 解像度が1280×1024かチェック
                If (size.Width = 1280) And (size.Height = 1024) Then
                    ' 処理結果に正常を格納
                    blnRet = True
                Else
                    ' 処理結果に異常を格納
                    blnRet = False
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値セット
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkDualProcess
        '   名称　：二重起動処理
        '   概要  ：同名のプロセスが起動しているかどうかチェック処理を行う。
        '   引数　：なし
        '   戻り値：True = 同名プロセス起動なし, False = 同名プロセス起動あり
        '   作成日：2011/12/23(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/12/23(金)  m.suzuki  新規作成
        '***************************************************************************************************
        Public Function ChkDualProcess() As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Dim strProcess As String = ""       ' アプリケーションプロセス名
            Try
                ' アプリケーションプロセス名取得    
                strProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName
                ' 同名プロセスが他に存在する場合は、既に起動中と判断
                If System.Diagnostics.Process.GetProcessesByName(strProcess).Length > 1 Then
                    Return False                ' 起動あり
                Else
                    Return True                 ' 起動なし
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                       ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkLineCnt
        '   名称　：行数チェック処理
        '   概要　：改行コードを含む文字列を分割して何行あるかチェックする。
        '   引数　：ByVal iStrChk As String = チェック対象文字列（改行コード含む）,
        '           ByVal iLngChk As Long   = チェックする行数
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/02/16(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/16(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>行数チェック処理</summary>
        ''' <param name="iStrChk">チェック対象文字列（改行コード含む）</param>
        ''' <param name="iLngChk">チェックする行数</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ChkLineCnt(ByVal iStrChk As String, _
                                   ByVal iLngChk As Long) As Boolean
            Dim blnRet As Boolean = False               ' 処理結果
            Dim strSplit() As String = Nothing          ' 分割した配列文字列
            Dim lngLineCnt As Long = 0                  ' チェックする文字列の行数
            Try
                strSplit = Split(iStrChk, vbCrLf)       ' 文字列を改行コードで分割
                lngLineCnt = UBound(strSplit) + 1       ' 配列最大数値取得
                If lngLineCnt <= iLngChk Then           ' 行数チェック
                    blnRet = True                       ' 戻り値に正常を設定
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                   ' ログ出力（致命的エラー）
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            Return blnRet                               ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkZenkaku
        '   名称　：全角チェック処理
        '   概要　：文字列がすべて全角文字列かチェックする。
        '   引数　：ByVal iStrChk As String = チェック対象文字列
        '   戻り値：True = すべて全角, False = 全角半角混合
        '   作成日：2012/02/20(月)  m.suzuki
        '   更新日：
        '   備考　：シフトJIS文字列は全角文字が2byte・半角文字が1byteの為、
        '           全角文字と 半角文字 * 2 したものが同じならならすべて全角文字とみなす。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/20(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>全角チェック処理</summary>
        ''' <param name="iStrChk">チェックする対象文字列</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkZenkaku(ByVal iStrChk As String) As Boolean
            Dim blnRet As Boolean = False                               ' 処理結果
            Dim lngLen As Long = 0                                      ' 文字数
            Dim lngLenB2 As Long = 0                                    ' バイト数（2倍）
            Dim encSjis As System.Text.Encoding = Nothing               ' SJISエンコードオブジェクト
            Try
                encSjis = System.Text.Encoding.GetEncoding("Shift_JIS") ' SJISエンコードオブジェクト生成
                lngLen = Len(iStrChk) * 2                               ' チェック対象の文字列の文字数取得
                lngLenB2 = encSjis.GetByteCount(iStrChk)                ' チェック対象の文字列のバイト数取得
                If lngLen = lngLenB2 Then                               ' 文字数 * 2とバイト数をチェック
                    blnRet = True                                       ' 同じ場合、処理結果に正常を設定
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            ' 戻り値設定
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkHankaku
        '   名称　：半角チェック処理
        '   概要　：文字列がすべて半角文字列かチェックする。
        '   引数　：ByVal iStrChk As String = チェック対象文字列
        '   戻り値：True = すべて半角, False = 全角半角混合
        '   作成日：2012/02/20(月)  m.suzuki
        '   更新日：
        '   備考　：シフトJIS文字列は全角文字が2byte・半角文字が1byteの為、
        '           全角文字と 半角文字 * 2 したものが同じならならすべて全角文字とみなす。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/20(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>全角チェック処理</summary>
        ''' <param name="iStrChk">チェックする対象文字列</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkHankaku(ByVal iStrChk As String) As Boolean
            Dim blnRet As Boolean = False                               ' 処理結果
            Dim lngLen As Long = 0                                      ' 文字数
            Dim lngLenB As Long = 0                                     ' バイト数
            Dim encSjis As System.Text.Encoding = Nothing               ' SJISエンコードオブジェクト
            Try
                encSjis = System.Text.Encoding.GetEncoding("Shift_JIS") ' SJISエンコードオブジェクト生成
                lngLen = Len(iStrChk)                                   ' チェック対象の文字列の文字数取得
                lngLenB = encSjis.GetByteCount(iStrChk)                 ' チェック対象の文字列のバイト数取得
                If lngLen = lngLenB Then                                ' 文字数とバイト数 * 2 をチェック
                    blnRet = True                                       ' 同じ場合、処理結果に正常を設定
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            ' 戻り値設定
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkLineLengh
        '   名称　：1行当たりの桁数チェック処理
        '   概要　：改行コードを含む文字列の1行当たりの桁数をチェックする。
        '   引数　：ByVal iStrChk As String = チェック対象文字列（改行コード含む）,
        '           ByVal iLngChk As Long   = チェックする桁数
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/02/20(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/20(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>全角チェック処理</summary>
        ''' <param name="iStrChk">チェックする対象文字列</param>
        ''' <param name="iLngChk">チェックする行数</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkLineLengh(ByVal iStrChk As String, _
                                     ByVal iLngChk As Long) As Boolean
            Dim blnRet As Boolean = False                   ' 処理結果
            Dim lngLineCnt As Long = 0                      ' 行数
            Dim lngChrCnt As Long = 0                       ' 文字数
            Dim strLine() As String = Nothing               ' 1行当たりの文字
            Try
                strLine = Split(iStrChk, vbCrLf)            ' 改行コードで文字列を分割
                lngLineCnt = UBound(strLine)                ' 文字列分割数取得
                For i = 0 To lngLineCnt                     ' 分割数分ループ
                    lngChrCnt = strLine(i).Length           ' 分割した文字列の文字数取得
                    If (lngChrCnt > iLngChk) Then           ' 分割した文字列の文字数とチェックする桁数と比較
                        Return blnRet                       ' チェックする桁数を超えた場合、
                    End If
                Next
                blnRet = True                               ' 処理結果に正常を設定
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            ' 戻り値設定
            Return blnRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkPeriod
        '   名称　：最新期チェック処理
        '   概要　：対象期IDが最新期かチェックする。
        '   引数　：ByVal iStrPeriodId    As String      = チェック期ID,
        '           Optional ByVal iClsDb As CLAccessMdb = データベースクラス
        '   戻り値：True = 最新期, False = 最新期以外
        '   作成日：2012/02/22(火)  m.suzuki
        '   更新日：
        '   備考　：データベースをコネクトしたまま使用する場合、第2引数にデータベースクラスを指定して下さい。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/22(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>最新期チェック処理</summary>
        ''' <param name="iStrPeriodId">チェックする期ID</param>
        ''' <param name="iClsDb">データベースクラス</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkNewPriod(ByVal iStrPeriodId As String, _
                                    Optional ByVal iClsDb As CLAccessMdb = Nothing) As Boolean

            Dim intRet As Integer = 0                           ' 処理結果件数
            Dim strSql As String = ""                           ' SQL
            Dim dtRet As DataTable = Nothing                    ' 処理結果データテーブル
            Dim clsDb As New CLAccessMdb                        ' データベースクラス

            Try
                ' SQL作成
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT DISTINCT a.c_period_id" & vbCrLf
                strSql = strSql & "   FROM period AS a" & vbCrLf
                strSql = strSql & "       ,ksh AS b" & vbCrLf
                strSql = strSql & "       ,( SELECT MAX(d.c_period_id) AS c_period_id" & vbCrLf
                strSql = strSql & "            FROM period AS d" & vbCrLf
                strSql = strSql & "                ,ksh AS e" & vbCrLf
                strSql = strSql & "           WHERE d.c_ksh = e.c_ksh ) AS c" & vbCrLf
                strSql = strSql & "  WHERE a.c_ksh = b.c_ksh" & vbCrLf
                strSql = strSql & "    AND a.c_period_id = c.c_period_id" & vbCrLf
                strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
                strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
                strSql = strSql & ";" & vbCrLf

                ' データベース接続
                If iClsDb Is Nothing Then
                    Call clsDb.Connect()
                    dtRet = clsDb.ExecuteSql(strSql)
                Else
                    dtRet = iClsDb.ExecuteSql(strSql)
                End If

                ' 件数取得
                intRet = dtRet.Rows.Count
                If intRet = 1 Then
                    Return True     ' 最新期
                Else
                    Return False    ' 最新期以外
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Finally
                ' データベース切断
                If iClsDb Is Nothing Then
                    Call clsDb.Disconnect()
                End If
            End Try
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ChkProhibitionString
        '   名称　：禁止文字チェック処理
        '   概要　：チェック対象文字列に禁止文字が使用されているかチェックする。
        '   引数　：ByVal iStrChk As String = チェック対象文字列
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/03/14(水)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/14(水)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>禁止文字チェック処理</summary>
        ''' <param name="iStrTarget">チェックする対象文字列</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ChkProhibitionString(ByVal iStrTarget As String) As Boolean

            Dim blnRet As Boolean = False                           ' 処理結果
            Dim strProhibition As String = "\/:*?""<>""""|"""""""   ' 禁止文字列
            Dim strChk1 As String = ""                              ' 比較用チェック対象文字列（1文字）
            Dim strChk2 As String = ""                              ' 比較用禁止文字列（1文字）

            Try
                ' チェック対象文字列数分ループ
                For i = 0 To iStrTarget.Length - 1
                    ' チェック対象文字列1文字取得
                    strChk1 = iStrTarget.Substring(i, 1)
                    For j = 0 To strProhibition.Length - 1
                        ' 比較禁止文字列1文字取得
                        strChk2 = strProhibition.Substring(j, 1)
                        ' チェック対象文字列と禁止文字列比較
                        If strChk1 = strChk2 Then
                            ' 同じだった場合、処理を抜ける
                            Exit For
                        End If
                    Next
                Next

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try

            ' 戻り値設定
            Return blnRet

        End Function
#End Region

    End Module
End Namespace
#End Region
