#Region "NSCLAccessMdbMst"
'===========================================================================================================
'   ネームスペース：NSCLAccessMdbMst
'   クラスＩＤ　　：CLAccessMdbMst
'   クラス名称　　：データベース関連クラス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDFile
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDCommon

Namespace NSCLAccessMdbMst
    Public Class CLAccessMdbMst

#Region "定数・変数"
        ' データベース関連
        Private _cn As System.Data.OleDb.OleDbConnection = Nothing          ' コネクションオブジェクト
        Private _tran As System.Data.OleDb.OleDbTransaction = Nothing       ' トランザクションオブジェクト
        Private _cm As System.Data.OleDb.OleDbCommand = Nothing             ' コマンドオブジェクト
        Private _da As System.Data.OleDb.OleDbDataAdapter = Nothing         ' データアダプタオブジェクト
        ' 画面関連
        Private Const SCREEN_ID = SCREEN_ID_CLACCESSMDB                     ' 画面ID
        Private Const SCREEN_NAME = SCREEN_NAME_CLACCESSMDB                 ' 画面名
        ' log4net
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "関数"
        '***************************************************************************************************
        '   ＩＤ　：New
        '   名称　：クラス生成処理
        '   概要　：クラスが生成された時に初期処理を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>クラス生成処理</summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        '***************************************************************************************************
        '   ＩＤ　：Connect
        '   名称　：データベース接続処理
        '   概要　：データベースの接続を行う。
        '   引数　：Optional ByVal pIntTimeOut As Integer = タイムアウト値
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>データベース接続処理</summary>
        ''' <param name="pIntTimeOut">タイムアウト値</param>
        ''' <remarks></remarks>
        Public Sub Connect(Optional ByVal pIntTimeOut As Integer = -1)
            Dim cs As String = ""
            Try
                '-------------------------------------------------------------------------------
                '   MDBファイル存在チェック
                '-------------------------------------------------------------------------------
                If FileExists(MDSystemInfo.AccessPath & MDSystemInfo.AccessName) = False Then
                    Call CLMsg.Show("GE0090", MDSystemInfo.AccessPath & MDSystemInfo.AccessName)
                    Exit Sub
                End If
                '-------------------------------------------------------------------------------
                '   コネクションオブジェクト生成
                '-------------------------------------------------------------------------------
                If _cn Is Nothing Then
                    _cn = New System.Data.OleDb.OleDbConnection
                End If
                '-------------------------------------------------------------------------------
                '   接続文字列設定　【デザインマスタ】を使用
                '-------------------------------------------------------------------------------
                cs = ""
                cs = cs & "Provider=" & MDSystemInfo.AccessProvider
                cs = cs & ";Data Source=" & MDSystemInfo.AccessMstPath & MDSystemInfo.AccessMstName
                cs = cs & ";Persist Security Info=" & AccessPersistSecurity
                'cs = cs & ";User ID=" & AccessUserId
                cs = cs & ";Jet OLEDB:Database Password=" & AccessPassword
                ' タイムアウト値設定
                If pIntTimeOut > -1 Then
                    cs = cs & ";Connect Timeout=" & pIntTimeOut.ToString()
                End If
                _cn.ConnectionString = cs
                ' データベースオープン
                _cn.Open()
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：Disconnect
        '   名称　：データベース切断処理
        '   概要　：データベース切断を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>データベース切断処理</summary>
        ''' <remarks></remarks>
        Public Sub Disconnect()
            Try
                ' データベースクローズ
                If (_cn) IsNot Nothing Then
                    If _cn.State = ConnectionState.Open Then
                        _cn.Close()
                    End If
                    _cn = Nothing
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：ExecuteSql
        '   名称　：SQL実行処理
        '   概要　：SQLを実行して結果をデータテーブルに返す。
        '   引数　：ByVal          pStrSql     As String  = SQL文,
        '           Optional ByVal pIntTimeOut As Integer = タイムアウト値
        '   戻り値：ExecuteSql As DataTable = SQL結果
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>SQL実行処理</summary>
        ''' <param name="pStrSql">SQL文</param>
        ''' <param name="pIntTimeOut">タイムアウト値</param>
        ''' <returns>SQL結果</returns>
        ''' <remarks></remarks>
        Public Function ExecuteSql(ByVal pStrSql As String, _
                                   Optional ByVal pIntTimeOut As Integer = -1) As DataTable
            log.Debug(pStrSql)                                                  ' ログ出力（SQL文）
            Dim dtRet As DataTable = Nothing                                    ' 処理結果データテーブル
            Try
                dtRet = New DataTable
                _cm = New System.Data.OleDb.OleDbCommand(pStrSql, _cn, _tran)   ' コマンドオブジェクト生成
                If pIntTimeOut > -1 Then                                        ' タイムアウト値設定
                    _cm.CommandTimeout = pIntTimeOut
                End If
                _da = New System.Data.OleDb.OleDbDataAdapter(_cm)               ' データアダプタオブジェクト生成
                _da.Fill(dtRet)                                                 ' データ更新
                _da.Dispose()                                                   ' データアダプタ開放
                _cm.Dispose()                                                   ' コマンドオブジェクト開放
            Catch ex As Exception
                log.Fatal(ex.Message)                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return dtRet                                                        ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ExecuteNonQuery
        '   名称　：SQL実行処理（INSERT, UPDATE, DELETE）
        '   概要　：SQLを実行して結果件数を返す。
        '   引数　：ByVal          pStrSql     As String  = SQL文,
        '           Optional ByVal pIntTimeOut As Integer = タイムアウト値
        '   戻り値：ExecuteNonQuery As Integer = SQL実行結果件数
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>SQL実行処理</summary>
        ''' <param name="pStrSql">SQL文</param>
        ''' <param name="pIntTimeOut">タイムアウト値</param>
        ''' <returns>SQL実行結果件数</returns>
        ''' <remarks></remarks>
        Public Function ExecuteNonQuery(ByVal pStrSql As String, _
                                        Optional ByVal pIntTimeOut As Integer = -1) As Integer
            log.Debug(pStrSql)                                                  ' ログ出力（SQL文）
            Dim intRet As Integer = -1                                          ' 実行結果件数 ( INSERT：登録件数, UPDATE：更新件数, DELETE：削除件数 )
            Try
                _cm = New System.Data.OleDb.OleDbCommand(pStrSql, _cn, _tran)   ' コマンドオブジェクト生成
                If pIntTimeOut > -1 Then                                        ' タイムアウト値設定
                    _cm.CommandTimeout = pIntTimeOut
                End If
                intRet = _cm.ExecuteNonQuery()                                  ' 実行結果件数取得
            Catch ex As Exception
                log.Fatal(ex.Message)                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return intRet                                                       ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：ExecuteNonQueryKeyErr
        '   名称　：SQL実行処理（INSERT, UPDATE, DELETE）
        '   概要　：SQLを実行して結果件数を返す。
        '   引数　：ByVal pStrSql As String = SQL文
        '   戻り値：ExecuteNonQuery As Integer = SQL実行結果件数
        '           ※ キー重複エラー時は、-2を返す
        '   作成日：2012/03/30(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/30(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>SQL実行処理（キー重複エラー対応）</summary>
        ''' <param name="pStrSql">SQL文</param>
        ''' <returns>SQL実行結果件数</returns>
        ''' <remarks></remarks>
        Public Function ExecuteNonQueryKeyErr(ByVal pStrSql As String) As Integer
            log.Debug(pStrSql)                                                  ' ログ出力（SQL文）
            Dim intRet As Integer = 0
            Try
                _cm = New System.Data.OleDb.OleDbCommand(pStrSql, _cn, _tran)   ' コマンドオブジェクト生成
                intRet = _cm.ExecuteNonQuery()                                  ' 実行結果件数取得
            Catch ex As OleDb.OleDbException
                log.Fatal(ex.Message)                                           ' ログ出力（致命的エラー）
                If ex.Errors(0).SQLState = "3022" Then                          ' キー重複エラーの場合、-2 を返す
                    intRet = -2
                    Return intRet
                End If
                Call CLMsg.ShowConnectErr(Err.Number, _
                          Err.Description, _
                          SCREEN_ID, _
                          SCREEN_NAME, _
                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            Catch ex2 As Exception
                log.Fatal(ex2.Message)                                          ' ログ出力（致命的エラー）
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return intRet                                                       ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：BeginTran
        '   名称　：トランザクション開始処理
        '   概要　：トランザクションの開始を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>トランザクション開始処理</summary>
        ''' <remarks></remarks>
        Public Sub BeginTran()
            Try
                ' トランザクション開始
                _tran = _cn.BeginTransaction()
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：CommitTran
        '   名称　：トランザクション確定処理
        '   概要　：トランザクションの確定を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>トランザクション確定処理</summary>
        ''' <remarks></remarks>
        Public Sub CommitTran()
            Try
                ' トランザクション確定
                If _tran Is Nothing = False Then
                    _tran.Commit()
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                _tran = Nothing
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：RollbackTran
        '   名称　：トランザクション取消処理
        '   概要　：トランザクションの取消を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>トランザクション取消処理</summary>
        ''' <remarks></remarks>
        Public Sub RollbackTran()
            Try
                ' トランザクション取消
                If _tran Is Nothing = False Then
                    _tran.Rollback()
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                _tran = Nothing
            End Try
        End Sub

        '***************************************************************************************************
        '   ＩＤ　：Compact
        '   名称　：最適化処理
        '   概要　：AccessMDBの最適化処理を行う。
        '   引数　：ByVal pStrMdbFile As String = 最適化するAccessMDBファイル
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '   備　考：プロジェクト(P) → 参照の追加(R) → 「COMタブ」 で
        '           「Microsoft Jet and Replication Objects 2.6 Library」 追加
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary></summary>
        ''' <param name="pStrMdbFile">AccessMDBファイル</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function Compact(ByVal pStrMdbFile As String) As Boolean
            Dim blnRet As Boolean = False                                       ' 処理結果
            Dim strFileTmp As String = ""                                       ' コピー先ファイル（一時ファイル）
            Try
                ' 最適化前データベースファイル存在チェック
                If FileExists(pStrMdbFile) = False Then
                    Return blnRet
                End If
                ' ファイル名取得（一時ファイル）
                strFileTmp = System.IO.Path.GetTempFileName                     ' 0バイトの一時ファイルをディスク上に作成し、フルパスを取得
                Dim jro As New JRO.JetEngine                                    ' ジェットエンジンオブジェクト
                Dim cs As String = "Provider=Microsoft.Jet.OLEDB.4.0;"          ' 接続文字列
                ' 一時ファイル削除
                If FileDelete(strFileTmp, True) = False Then                    ' ファイル名のみ取得する為、一時ファイル自体消す
                    Return blnRet
                End If
                ' 最適化処理
                Dim oldDb As String = cs & "Data Source=" & pStrMdbFile         ' 最適化前データベース取得
                Dim newDb As String = cs & "Data Source=" & strFileTmp          ' 最適化後データベース取得
                jro.CompactDatabase(oldDb, newDb)
                ' ファイルコピー
                If FileCopy(pStrMdbFile, strFileTmp, True) = False Then
                    Return blnRet
                End If
                blnRet = True                                                   ' 処理結果に正常を設定
            Catch ex As Exception
                log.Fatal(ex.Message)                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                Call FileDelete(strFileTmp, True)                               ' 一時ファイルがある場合、削除する
            End Try
            Return blnRet                                                       ' 戻り値設定
        End Function

        '***************************************************************************************************
        '   ＩＤ　：Finalize
        '   名称　：クラス破棄処理
        '   概要　：クラスが破棄された終了処理を行う。
        '   引数　：なし
        '   戻り値：なし
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>クラス破棄処理</summary>
        ''' <remarks></remarks>
        Protected Overrides Sub Finalize()
            Try
                ' データベース切断処理呼び出し
                Call Disconnect()
                ' ファイナライズ
                MyBase.Finalize()
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowConnectErr(Err.Number, _
                                          Err.Description, _
                                          SCREEN_ID, _
                                          SCREEN_NAME, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
        End Sub
#End Region

    End Class
End Namespace
#End Region
