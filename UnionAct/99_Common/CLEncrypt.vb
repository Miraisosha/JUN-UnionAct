#Region "NSCLEncrypt"
'===========================================================================================================
'   ネームスペース：NSCLEncrypt
'   クラスＩＤ　　：CLEncrypt
'   クラス名称　　：暗号化復号化関連クラス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon


Namespace NSCLEncrypt
    Public Class CLEncrypt

#Region "定数"
        ' 画面関連
        Private Const SCREEN_ID = SCREEN_ID_CLENCRYPT                       ' CLEncrypt
        Private Const SCREEN_NAME = SCREEN_NAME_CLENCRYPT                   ' 暗号化復号化関連クラス
        ' log4net
        Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "関数"
        '***************************************************************************************************
        '   ＩＤ　：Encrypt
        '   名称　：文字列暗号化処理
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pStrData As String = 暗号化する文字列
        '           ByVal pStrPass As String = 暗号化に使用するパスワード
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/07(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/07(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>文字列暗号化処理</summary>
        ''' <param name="pStrData">暗号化する文字列</param>
        ''' <param name="pStrPass">暗号化に使用するパスワード</param>
        ''' <returns>暗号化された文字列</returns>
        Public Shared Function Encrypt(ByVal pStrData As String, _
                                       ByVal pStrPass As String) As String
            Dim strRet As String = ""               ' 処理結果暗号化文字列
            Try
                ' RijndaelManagedオブジェクト作成
                Dim objRij As New System.Security.Cryptography.RijndaelManaged()
                ' パスワードから共有キーと初期化ベクタ作成
                Dim bytKey As Byte() = Nothing      ' 共有キー
                Dim bytIv As Byte() = Nothing       ' ベクタ
                Call GenerateKeyFromPass(pStrPass, objRij.KeySize, bytKey, objRij.BlockSize, bytIv)
                objRij.Key = bytKey
                objRij.IV = bytIv
                ' 文字列をバイト型配列に変換
                Dim bytStr As Byte() = System.Text.Encoding.UTF8.GetBytes(pStrData)
                ' 暗号化オブジェクト作成
                Dim encryptor As System.Security.Cryptography.ICryptoTransform = objRij.CreateEncryptor()
                ' バイト型配列を暗号化
                Dim encBytes As Byte() = encryptor.TransformFinalBlock(bytStr, 0, bytStr.Length)
                ' 閉じる
                encryptor.Dispose()
                ' バイト型配列を文字列に変換して返す
                strRet = System.Convert.ToBase64String(encBytes)
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値設定
            Return strRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：Decrypt
        '   名称　：文字列復号化処理
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pStrData As String = 暗号化された文字列
        '           ByVal pStrPass As String = 暗号化に使用したパスワード
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/07(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/07(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>文字列復号化処理</summary>
        ''' <param name="pStrData">暗号化された文字列</param>
        ''' <param name="pStrPass">暗号化に使用したパスワード</param>
        ''' <returns>復号化された文字列</returns>
        Public Shared Function Decrypt(ByVal pStrData As String, _
                                       ByVal pStrPass As String) As String
            Dim strRet As String = ""                   ' 処理結果復号化文字列
            Try
                ' RijndaelManagedオブジェクトを作成
                Dim objRij As New System.Security.Cryptography.RijndaelManaged()
                ' パスワードから共有キーと初期化ベクタ作成
                Dim bytKey As Byte() = Nothing          ' 共有キー
                Dim bytIv As Byte() = Nothing           ' ベクタ
                Call GenerateKeyFromPass(pStrPass, objRij.KeySize, bytKey, objRij.BlockSize, bytIv)
                objRij.Key = bytKey
                objRij.IV = bytIv
                ' 文字列をバイト型配列に戻す
                Dim bytStr As Byte() = System.Convert.FromBase64String(pStrData)
                ' 暗号化オブジェクト作成
                Dim decryptor As System.Security.Cryptography.ICryptoTransform = objRij.CreateDecryptor()
                ' バイト型配列を復号化
                ' 復号化に失敗すると例外CryptographicExceptionが発生
                Dim decBytes As Byte() = decryptor.TransformFinalBlock(bytStr, 0, bytStr.Length)
                ' 閉じる
                decryptor.Dispose()
                ' バイト型配列を文字列に戻して返す
                strRet = System.Text.Encoding.UTF8.GetString(decBytes)
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            ' 戻り値設定
            Return strRet
        End Function

        '***************************************************************************************************
        '   ＩＤ　：GenerateKeyFromPass
        '   名称　：共有キー初期化ベクタ生成処理
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pStrPass As String = 基になるパスワード
        '           ByVal pIntkeySize As Integer = 共有キーのサイズ（ビット）
        '           ByRef pBytKey As Byte() = 作成された共有キー
        '           ByVal pIntBlockSize As Integer = 初期化ベクタのサイズ（ビット）
        '           ByRef pBytIv As Byte() = 作成された初期化ベクタ
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/07(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/07(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>パスワードから共有キーと初期化ベクタ生成</summary>
        ''' <param name="pStrPass">基になるパスワード</param>
        ''' <param name="pIntkeySize">共有キーのサイズ（ビット）</param>
        ''' <param name="pBytKey">作成された共有キー</param>
        ''' <param name="pIntBlockSize">初期化ベクタのサイズ（ビット）</param>
        ''' <param name="pBytIv">作成された初期化ベクタ</param>
        Private Shared Sub GenerateKeyFromPass(ByVal pStrPass As String, _
                                               ByVal pIntkeySize As Integer, _
                                               ByRef pBytKey As Byte(), _
                                               ByVal pIntBlockSize As Integer, _
                                               ByRef pBytIv As Byte())
            Try
                ' パスワードから共有キーと初期化ベクタ作成
                Dim salt As Byte() = System.Text.Encoding.UTF8.GetBytes("saltは必ず8バイト以上")
                ' Rfc2898DeriveBytesオブジェクト作成
                Dim deriveBytes As New System.Security.Cryptography.Rfc2898DeriveBytes(pStrPass, salt)
                '.NET Framework 1.1以下の時は、PasswordDeriveBytesを使用
                'Dim deriveBytes As New System.Security.Cryptography.PasswordDeriveBytes(strPass, salt)
                ' 反復処理回数を指定する デフォルトで1000回
                deriveBytes.IterationCount = 1000
                ' 共有キーと初期化ベクタ生成
                pBytKey = deriveBytes.GetBytes(pIntkeySize \ 8)
                pBytIv = deriveBytes.GetBytes(pIntBlockSize \ 8)
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, _
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
