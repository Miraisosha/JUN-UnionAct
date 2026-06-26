#Region " NSMDFile "
'===========================================================================================================
'   ネームスペース：NSMDFile
'   モジュールＩＤ：MDFile
'   モジュール名称：ファイル関連モジュール
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLAccessMdb
Imports System.IO

Namespace NSMDFile
    Public Module MDFile

#Region " 定数 "
        ' 画面関連
        Private Const SCREEN_ID = SCREEN_ID_MDFILE                          ' MDFile
        Private Const SCREEN_NAME = SCREEN_NAME_MDFILE                      ' ファイル関連モジュール
        ' log4net初期化
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region " 関数 "
#Region " ファイル存在チェック処理 "
        '***************************************************************************************************
        '   ＩＤ　：FileExists
        '   名称　：ファイル存在チェック処理
        '   概要　：ファイルの存在チェックを行う。
        '   引数　：ByVal pStrFile As String = チェック対象ファイル（フルパス）
        '   戻り値：True = 存在する, False = 存在しない
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>ファイル存在チェック処理</summary>
        ''' <param name="pStrFile">チェック対象ファイル（フルパス）</param>
        ''' <returns>True = 存在する, False = 存在しない</returns>
        ''' <remarks></remarks>
        Public Function FileExists(ByVal pStrFile As String) As Boolean

            Dim blnRet As Boolean = False           ' 処理結果

            Try
                ' ファイル存在チェック
                If System.IO.File.Exists(pStrFile) Then
                    blnRet = True                           ' 存在する
                Else
                    blnRet = False                          ' 存在しない
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal(
                    Err.Number,
                    Err.Description,
                    SCREEN_ID,
                    SCREEN_NAME,
                    System.Reflection.MethodBase.GetCurrentMethod.Name()
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " ファイルコピー処理 "
        '***************************************************************************************************
        '   ＩＤ　：FileCopy
        '   名称　：ファイルコピー処理
        '   概要　：コピー元、コピー先の存在チェックを行い、ファイルのコピー処理を行う。
        '   引数　：ByVal pStrFileFrom            As String  = コピー元ファイル（フルパス）,
        '           ByVal pStrFleTo               As String  = コピー先ファイル（フルパス）,
        '           Optional ByVal overWritingFlg As Boolean = 上書きフラグ
        '               True  = コピー先ファイルがある場合、上書きコピーする。
        '               False = コピー先ファイルがある場合、上書きコピーしない。
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>ファイル存在チェック処理</summary>
        ''' <param name="pStrFileFrom">コピー元ファイル（フルパス）</param>
        ''' <param name="pStrFileTo">コピー先ファイル（フルパス）</param>
        ''' <param name="overWritingFlg">上書きフラグ</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function FileCopy(
            ByVal pStrFileFrom As String,
            ByVal pStrFileTo As String,
            Optional ByVal overWritingFlg As Boolean = False
        ) As Boolean

            Dim blnRet As Boolean = False           ' 処理結果

            Try
                ' コピー元ファイル存在チェック
                If System.IO.File.Exists(pStrFileFrom) = False Then
                    Return False
                End If

                ' コピー先ファイル存在チェック
                If System.IO.File.Exists(pStrFileTo) Then
                    ' 上書きフラグチェック
                    If overWritingFlg = False Then
                        Return False
                    End If
                End If

                ' ファイルコピー
                Call System.IO.File.Copy(pStrFileFrom, pStrFileTo, overWritingFlg)

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal(
                    Err.Number,
                    Err.Description,
                    SCREEN_ID,
                    SCREEN_NAME,
                    System.Reflection.MethodBase.GetCurrentMethod.Name()
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " ファイル削除処理 "
        '***************************************************************************************************
        '   ＩＤ　：FileDelete
        '   名称　：ファイル削除処理
        '   概要　：ファイルの存在チェックを行い、ファイルの削除処理を行う。
        '   引数　：ByVal pStrFile As String = 削除対象ファイル（フルパス）,
        '           Optional ByVal pBlnCompulsionDelFlg As Boolean = 強制削除フラグ
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/10/28(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/28(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>ファイル削除処理</summary>
        ''' <param name="pStrFile">削除対象ファイル（フルパス）</param>
        ''' <param name="pBlnCompulsionDelFlg">強制削除フラグ</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function FileDelete(
            ByVal pStrFile As String,
            Optional ByVal pBlnCompulsionDelFlg As Boolean = True
        ) As Boolean

            Dim blnRet As Boolean = False           ' 処理結果

            Try
                ' ファイル存在チェック
                If System.IO.File.Exists(pStrFile) = False Then
                    Call MessageBox.Show("FileDelete エラー : ファイルがありません！")
                    Return False
                End If

                ' 強制削除フラグチェック
                If pBlnCompulsionDelFlg Then

                    ' ファイル情報取得
                    Dim fileInfo As New System.IO.FileInfo(pStrFile)

                    ' 読み取り専用属性の場合、読み取り専用属性解除
                    If (fileInfo.Attributes _
                    And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                        fileInfo.Attributes = System.IO.FileAttributes.Normal
                    End If

                End If

                ' ファイル削除処理
                System.IO.File.Delete(pStrFile)

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal(
                    Err.Number,
                    Err.Description,
                    SCREEN_ID,
                    SCREEN_NAME,
                    System.Reflection.MethodBase.GetCurrentMethod.Name()
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " フォルダ存在チェック処理 "
        '***************************************************************************************************
        '   ＩＤ　：DirExists
        '   名称　：フォルダ存在チェック処理
        '   概要　：フォルダの存在チェックを行う。
        '   引数　：ByVal pStrDir As String = チェック対象フォルダ（フルパス）
        '   戻り値：True = 存在する, False = 存在しない
        '   作成日：2012/02/01(水)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/01(水)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>フォルダ存在チェック処理</summary>
        ''' <param name="pStrDir">チェック対象フォルダ（フルパス）</param>
        ''' <returns>True = 存在する, False = 存在しない</returns>
        ''' <remarks></remarks>
        Public Function DirExists(ByVal pStrDir As String) As Boolean

            Dim blnRet As Boolean = False                           ' 処理結果
            Dim strChkDir As String = ""                            ' チェックするフォルダ

            Try
                ' 最後に\が付いているかチェック
                If pStrDir.Substring(pStrDir.Length - 1) = "\" Then
                    strChkDir = pStrDir
                Else
                    strChkDir = pStrDir & "\"
                End If

                ' フォルダ存在チェック
                If System.IO.Directory.Exists(strChkDir) Then
                    blnRet = True                                   ' 存在する
                Else
                    blnRet = False                                  ' 存在しない
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal(
                    Err.Number,
                    Err.Description,
                    SCREEN_ID,
                    SCREEN_NAME,
                    System.Reflection.MethodBase.GetCurrentMethod.Name()
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region
        Public Function GetName(ByVal v_FilePath As String) As String
            Return GetName(v_FilePath, True)
        End Function

        Public Function GetName(ByVal v_FilePath As String, ByVal v_RetExtension As Boolean) As String
            If v_FilePath Is Nothing OrElse v_FilePath = "" Then Return ""
            Dim fi As FileInfo = New FileInfo(v_FilePath)

            If v_RetExtension Then
                Return fi.Name
            Else
                Return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length)
            End If
        End Function
        Public Function GetDirPath(ByVal v_FilePath As String) As String
            Try
                Return If(System.IO.Path.GetDirectoryName(v_FilePath), "")
            Catch
                Return ""
            End Try
        End Function
        Public Function ExistDir(ByVal v_DirPath As String) As Boolean
            Return System.IO.Directory.Exists(v_DirPath)
        End Function

        Public Function GetFiles(ByVal dir As String, ByVal searchPattern As String, ByVal vSearchOption As SearchOption) As String()
            If ExistDir(dir) = False Then Return New String(-1) {}
            Return System.IO.Directory.GetFiles(dir, searchPattern, vSearchOption)
        End Function
        Public Function GetDirs(ByVal dir As String, ByVal searchPattern As String, ByVal vSearchOption As SearchOption) As String()
            If ExistDir(dir) = False Then Return New String(-1) {}
            Return System.IO.Directory.GetDirectories(dir, searchPattern, vSearchOption)
        End Function
#Region " フォルダ作成処理 "
        '***************************************************************************************************
        '   ＩＤ　：DirCreate
        '   名称　：フォルダ作成処理
        '   概要　：フォルダの作成を行う。
        '   引数　：ByVal pStrDir As String = 作成フォルダ（フルパス）
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/03/16(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/16(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>フォルダ作成処理</summary>
        ''' <param name="iStrDir">作成するフォルダ（フルパス）</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function DirCreate(ByVal iStrDir As String) As Boolean

            Dim blnRet As Boolean = False       ' 処理結果
            Dim strChkDir As String = ""        ' 作成するフォルダ

            Try
                ' 末尾チェック
                If iStrDir.Substring(iStrDir.Length - 1) = "\" Then ' 最後に\が付いているかチェック
                    strChkDir = iStrDir
                Else
                    strChkDir = iStrDir & "\"
                End If

                ' フォルダ作成
                System.IO.Directory.CreateDirectory(iStrDir)

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal(
                    Err.Number,
                    Err.Description,
                    SCREEN_ID,
                    SCREEN_NAME,
                    System.Reflection.MethodBase.GetCurrentMethod.Name()
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region "ワークフォルダ取得処理"
        '    '***************************************************************************************************
        '    '   ＩＤ　：GetWorkDir
        '    '   名称　：ワークフォルダ取得処理
        '    '   概要  ：ワークフォルダを取得する。
        '    '   引数　：ByVal clsDb As CLAccessMdb = データベースクラス
        '    '   戻り値：True：正常, False：異常
        '    '   作成日：2012/03/18(日)  m.suzuki
        '    '   更新日：
        '    '---------------------------------------------------------------------------------------------------
        '    '   履歴　：2012/03/18(日)  m.suzuki  新規作成
        '    '***************************************************************************************************
        '    ''' <summary>ワークフォルダ取得処理</summary>
        '    ''' <param name="clsDb">データベースクラス</param>
        '    ''' <returns>True：正常, False：異常</returns>
        '    ''' <remarks></remarks>
        '    Public Function GetWorkDir(
        '    ByVal clsDb As CLAccessMdb
        ') As String

        '        'Dim blnRet As Boolean = False       ' 処理結果
        '        Dim strSql As String                ' SQL
        '        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル
        '        Dim intRet As Integer = 0           ' 処理件数
        '        Dim strRet As String = ""           ' 処理結果パス
        '        Dim strNow As String = ""           ' 現在時刻（yyyyMMdd）

        '        Try
        '            ' SQL作成
        '            strSql = "" & vbCrLf
        '            strSql = strSql & " SELECT l_name" & vbCrLf
        '            strSql = strSql & "   FROM constant_dtl" & vbCrLf
        '            strSql = strSql & "  WHERE c_constant = 'ISSUE_DOCUMENT_WORK'" & vbCrLf
        '            strSql = strSql & "    AND c_constant_seq = '01'" & vbCrLf
        '            strSql = strSql & ";" & vbCrLf

        '            ' SQL実行
        '            dtRet = clsDb.ExecuteSql(strSql)

        '            ' 処理件数取得
        '            intRet = dtRet.Rows.Count

        '            ' 処理件数チェック
        '            If intRet = 0 Then
        '                Call MessageBox.Show(
        '                "発行文書作業先が取得できません。",
        '                "エラー",
        '                MessageBoxButtons.OK,
        '                MessageBoxIcon.Warning,
        '                MessageBoxDefaultButton.Button1
        '            )
        '                Return ""
        '            End If

        '            ' ワークフォルダ取得
        '            mStrWorkDir = dtRet.Rows(0).Item(0).ToString()
        '            strNow = Now.ToString("yyyyMMdd")
        '            mStrWorkDirTo = mStrWorkDir & strNow & "\"

        '            '' 下1桁が "\" ではない場合、"\" を付与
        '            'If mStrWorkDir.Substring(mStrWorkDir.Length - 1, 1) <> "\" Then
        '            '    mStrWorkDir = mStrWorkDir & "\"
        '            'End If

        '            ' フォルダ存在チェック
        '            If MDFile.DirExists(mStrWorkDir) Then
        '                ' フォルダ作成処理
        '                If MDFile.DirCreate(mStrWorkDir) = False Then
        '                    Return blnRet
        '                End If
        '            End If

        '            ' 処理結果に正常を設定
        '            blnRet = True

        '        Catch ex As Exception
        '            ' ログ出力（致命的エラー）
        '            log.Fatal(ex.Message)

        '            ' エラーメッセージ表示
        '            Call CLMsg.ShowEtarnal(
        '                Err.Number,
        '                Err.Description,
        '                SCREEN_ID,
        '                SCREEN_NAME,
        '                System.Reflection.MethodBase.GetCurrentMethod.Name()
        '            )
        '        )

        '        End Try

        '        ' 戻り値設定
        '        Return blnRet

        '    End Function
#End Region


#Region " フォルダ削除処理 "


        '***************************************************************************************************
        '   ＩＤ　：DirDelete
        '   名称　：フォルダ削除処理
        '   概要　：フォルダの存在チェックを行い、フォルダの削除処理を行う。
        '   引数　：ByVal pStrDir As String = 削除対象フォルダ（フルパス）,
        '           Optional ByVal pBlnCompulsionDelFlg As Boolean = 強制削除フラグ
        '   戻り値：True = 正常, False = エラー
        '   作成日：2012/03/16(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/16(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>フォルダ削除処理</summary>
        ''' <param name="pStrDir">削除対象フォルダ（フルパス）</param>
        ''' <param name="pBlnCompulsionDelFlg">強制削除フラグ</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function DirDelete( _
            ByVal pStrDir As String, _
            Optional ByVal pBlnCompulsionDelFlg As Boolean = True _
        ) As Boolean

            Dim blnRet As Boolean = False           ' 処理結果

            Try
                ' フォルダ存在チェック
                If System.IO.Directory.Exists(pStrDir) = False Then
                    Call MessageBox.Show("DirDelete エラー : フォルダがありません！")
                    Return False
                End If

                ' 強制削除フラグチェック
                If pBlnCompulsionDelFlg Then

                    ' フォルダ情報取得
                    Dim dirInfo As New System.IO.DirectoryInfo(pStrDir)

                    ' 読み取り専用属性の場合、読み取り専用属性解除
                    If (dirInfo.Attributes _
                    And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                        dirInfo.Attributes = System.IO.FileAttributes.Normal
                    End If
                End If
                Try
                    ' フォルダ削除処理
                    System.IO.Directory.Delete(pStrDir, True)
                Catch ex As Exception
                    Return False
                End Try
                ' 処理結果に正常を設定
                blnRet = True

                Catch ex As Exception
                    ' ログ出力（致命的エラー）
                    log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " ファイル名取得処理 "
        '***************************************************************************************************
        '   ＩＤ　：FileGetFileName
        '   名称　：ファイル名取得処理
        '   概要　：フルパスファイル名からファイル名のみを取得する。
        '   引数　：ByVal iStrFileFullPath As String = ファイル名（フルパス）,
        '           ByRef ioStrFileName    As String = ファイル名
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/03/16(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/16(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>ファイル名取得処理</summary>
        ''' <param name="iStrFileFullPath">ファイル名（フルパス）</param>
        ''' <param name="ioStrFileName">ファイル名</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function FileGetFileName( _
            ByVal iStrFileFullPath As String, _
            ByRef ioStrFileName As String _
        ) As Boolean

            Dim blnRet As Boolean = False   ' 処理結果

            Try
                ' フルパスからファイル名のみを取得
                ioStrFileName = System.IO.Path.GetFileName(iStrFileFullPath)

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " フォルダ名取得処理 "
        '***************************************************************************************************
        '   ＩＤ　：FileGetDirName
        '   名称　：フォルダ名取得処理
        '   概要　：フルパスファイル名からフォルダ名のみを取得する。
        '   引数　：ByVal iStrFileFullPath As String = ファイル名（フルパス）,
        '           ByRef ioStrDirName     As String = フォルダ名
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/03/16(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/03/16(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>フォルダ名取得処理</summary>
        ''' <param name="iStrFileFullPath">ファイル名（フルパス）</param>
        ''' <param name="ioStrDirName">フォルダ名</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function FileGetDirName( _
            ByVal iStrFileFullPath As String, _
            ByRef ioStrDirName As String _
        ) As Boolean

            Dim blnRet As Boolean = False   ' 処理結果

            Try
                ' フルパスからファイル名のみを取得
                ioStrDirName = System.IO.Path.GetDirectoryName(iStrFileFullPath) & "\"

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " フォルダ配下ファイル取得処理 "
        '***************************************************************************************************
        '   ＩＤ　：GetDirUnderFiles
        '   名称　：フォルダ配下ファイル取得処理
        '   概要　：第1引数で指定されたフォルダ配下にある第2引数の検索文字列の対象ファイルを取得する。
        '           第3引数でデフォルトは、指定フォルダ配下ファイルのみ対象
        '           "*.txt" の場合、テキストファイル,
        '           "*.xls" の場合、Excelファイル,
        '           "*"     の場合、すべてのファイルが対象
        '   引数　：ByVal iDir                    As String                = ファイルを検索するフォルダ名（フルパス）,
        '           ByRef ioFiles                 As String()              = ファイル名,
        '           Optional ByVal iSearchPattern As String                = ファイル名検索文字列,
        '           Optional ByVal iSearchOption As System.IO.SearchOption = 検索オプション
        '               （IO.SearchOption.AllDirectories = 指定したフォルダにあるファイルが対象,
        '               IO.SearchOption.TopDirectoryOnly = 指定したフォルダのサブフォルダのファイルも対象）
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/11/20(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/20(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>フォルダ配下ファイル取得処理</summary>
        ''' <param name="iDir">検索するフォルダ名（フルパス）</param>
        ''' <param name="ioFiles">ファイル名</param>
        ''' <param name="iSearchPattern">ファイル名検索文字列</param>
        ''' <param name="iSearchOption">検索オプション</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function GetDirUnderFiles( _
            ByVal iDir As String, _
            ByRef ioFiles As String(), _
            Optional ByVal iSearchPattern As String = "*", _
            Optional ByVal iSearchOption As System.IO.SearchOption = IO.SearchOption.TopDirectoryOnly _
        ) As Boolean

            Dim blnRet As Boolean = False       ' 処理結果

            Try
                ' 指定フォルダ配下の指定されたファイル名検索文字列でファイルを取得
                ' ワイルドカード "*" は、すべてのファイルが対象
                ioFiles = System.IO.Directory.GetFiles( _
                    iDir, _
                    iSearchPattern, _
                    iSearchOption _
                )

                ' 処理結果に正常を設定
                blnRet = True

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            End Try

            ' 戻り値設定
            Return blnRet

        End Function
#End Region

#Region " CSVファイル出力処理 "
        '***************************************************************************************************
        '   ＩＤ　：CsvPut
        '   名称　：CSVファイル出力処理
        '   概要　：データテーブルからCSVファイルを作成する。
        '   引数　：ByVal pDtData             As DataTable = 出力するデータテーブル
        '           ByVal pStrCsvPath         As String    = CSVファイルパス
        '           ByVal pStrCsvName         As String    = CSVファイル名
        '           Optional ByVal pStrEncode As String    = エンコード
        '           Optional ByVal pHeaderFlg As Boolean   = ヘッダー出力フラグ（True：出力する, False：出力しない）
        '   戻り値：True = 正常, False = エラー
        '   作成日：2011/11/12(土)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/12(土)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>CSVファイル出力処理</summary>
        ''' <param name="pDtData">出力するデータテーブル</param>
        ''' <param name="pStrCsvPath">CSVファイルパス</param>
        ''' <param name="pStrCsvName">CSVファイル名</param>
        ''' <param name="pStrEncode">エンコード</param>
        ''' <returns>True = 正常, False = エラー</returns>
        ''' <remarks></remarks>
        Public Function CsvPut( _
            ByVal pDtData As DataTable, _
            ByVal pStrCsvPath As String, _
            Optional ByVal pStrCsvName As String = "", _
            Optional ByVal pStrEncode As String = "Shift_JIS", _
            Optional ByVal pHeaderFlg As Boolean = True _
        ) As Boolean

            Dim blnRet As Boolean = False                       ' 処理結果
            Dim encCsv As System.Text.Encoding = Nothing        ' CSV出力時のエンコード
            Dim sr As System.IO.StreamWriter = Nothing          ' ストリームライタオブジェクト
            Dim intColCnt As Integer = 0                        ' カラム数
            Dim intLastColIndex As Integer = 0                  ' 最後のカラムのインデックス
            Dim i As Integer = 0                                ' カウンタ
            Dim strField As String = ""                         ' 
            Dim row As DataRow = Nothing                        ' データロー

            Try
                ' エンコード設定
                encCsv = System.Text.Encoding.GetEncoding(pStrEncode)

                ' 上書きモードでCSVファイルオープン（ファイルが在れば上書き、なければ新規で作成）
                If String.IsNullOrEmpty(pStrCsvName) = True Then
                    sr = New System.IO.StreamWriter(pStrCsvPath, False, encCsv)
                Else
                    sr = New System.IO.StreamWriter(pStrCsvPath & pStrCsvName, False, encCsv)
                End If

                ' カラム数取得
                intColCnt = pDtData.Columns.Count

                ' 最後のカラムのインデックス取得
                intLastColIndex = intColCnt - 1

                '-----------------------------------------------------------------------------------
                '   ヘッダー
                '-----------------------------------------------------------------------------------
                ' ヘッダー出力フラグ判定
                If pHeaderFlg = True Then

                    ' カラム数分ループ
                    For i = 0 To intColCnt - 1

                        ' ヘッダの取得
                        strField = pDtData.Columns(i).Caption

                        ' "で囲む必要があるか調べる
                        If strField.IndexOf(ControlChars.Quote) > -1 OrElse _
                            strField.IndexOf(","c) > -1 OrElse _
                            strField.IndexOf(ControlChars.Cr) > -1 OrElse _
                            strField.IndexOf(ControlChars.Lf) > -1 OrElse _
                            strField.StartsWith(" ") OrElse _
                            strField.StartsWith(ControlChars.Tab) OrElse _
                            strField.EndsWith(" ") OrElse _
                            strField.EndsWith(ControlChars.Tab) Then
                            If strField.IndexOf(ControlChars.Quote) > -1 Then
                                '"を""とする
                                strField = strField.Replace("""", """""")
                            End If
                            strField = """" + strField + """"
                        End If

                        ' フィールドを書き込む
                        sr.Write(strField)

                        ' カンマを書き込む
                        If intLastColIndex > i Then
                            sr.Write(","c)
                        End If

                    Next i
                    ' 改行
                    sr.Write(ControlChars.Cr + ControlChars.Lf)
                End If

                '-----------------------------------------------------------------------------------
                '   データ
                '-----------------------------------------------------------------------------------
                ' データ数分ループ
                For Each row In pDtData.Rows
                    ' カラム数分ループ
                    For i = 0 To intColCnt - 1

                        ' フィールドの取得
                        strField = row(i).ToString()

                        '"で囲む必要があるか調べる
                        If strField.IndexOf(ControlChars.Quote) > -1 OrElse _
                            strField.IndexOf(","c) > -1 OrElse _
                            strField.IndexOf(ControlChars.Cr) > -1 OrElse _
                            strField.IndexOf(ControlChars.Lf) > -1 OrElse _
                            strField.StartsWith(" ") OrElse _
                            strField.StartsWith(ControlChars.Tab) OrElse _
                            strField.EndsWith(" ") OrElse _
                            strField.EndsWith(ControlChars.Tab) Then
                            If strField.IndexOf(ControlChars.Quote) > -1 Then
                                '"を""とする
                                strField = strField.Replace("""", """""")
                            End If
                            strField = """" + strField + """"
                        End If

                        ' フィールドを書き込む
                        sr.Write(strField)

                        ' カンマを書き込む
                        If intLastColIndex > i Then
                            sr.Write(","c)
                        End If

                    Next i

                    ' 改行
                    sr.Write(ControlChars.Cr + ControlChars.Lf)

                Next row

                ' クローズ
                sr.Close()

                ' 処理結果に正常を設定
                blnRet = True

            Catch ioex As System.IO.IOException
                ' ログ出力（IOException）
                log.Fatal(ioex.Message)
                MessageBox.Show(ioex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            Finally
                If Not sr Is Nothing Then
                    sr.Close()
                End If

            End Try

            ' 戻り値格納
            Return blnRet

        End Function
#End Region

#Region " CSVファイル読込処理 "
        '***************************************************************************************************
        '   ＩＤ　：CsvRead
        '   名称　：CSVファイル読込処理
        '   概要　：CSVファイルからデータテーブルに読み込む。
        '           1.レコードは、LFまたはCRLFで区切られる。
        '           2.フィールドは、カンマ（,）で区切られる。
        '           3.区切りのカンマの前後のスペース（タブを含む）は無視される。
        '           4.フィールドにカンマが含まれる場合、フィールドをダブルクォート（"）で囲まなければならない。
        '           5.フィールドにダブルクォートが含まれる場合、フィールドをダブルクォートで囲み、フィールド内のダブルクォートを2つの連続するダブルクォート（つまり、「""」）に置き換えなければならない。
        '           6.フィールドが改行文字を含む場合、フィールドをダブルクォートで囲まなければならない。
        '           7.フィールドの前後にスペースがある場合、フィールドをダブルクォートで囲まなければならない。
        '           8.すべてのフィールドがダブルクォートで囲まれているかもしれない。
        '           9.はじめのレコードは、ヘッダかもしれない。
        '   引数　：ByVal pStrCsvPath As String = CSVファイルパス
        '           ByVal pStrCsvName As String  = CSVファイル名
        '   戻り値：DataTable = CSVファイルから読み取ったデータ
        '   作成日：2011/11/12(土)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/12(土)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>CSVファイル読込処理</summary>
        ''' <param name="pStrCsvPath">CSVファイルパス</param>
        ''' <param name="pStrCsvName">CSVファイル名</param>
        ''' <returns>CSVファイルから読み取ったデータ</returns>
        ''' <remarks></remarks>
        Public Function CsvRead( _
            ByVal pStrCsvPath As String, _
            ByVal pStrCsvName As String _
        ) As DataTable

            Dim dtRet As DataTable = Nothing                        ' 処理結果
            Dim strConStr As String = ""                            ' 接続文字列
            Dim strSql As String = ""                               ' SQL文
            Dim Cn As System.Data.OleDb.OleDbConnection = Nothing   ' コネクションオブジェクト
            Dim da As System.Data.OleDb.OleDbDataAdapter = Nothing  ' データアダプタオブジェクト

            Try
                ' 接続文字列設定
                strConStr = "Provider=Microsoft.Jet.OLEDB.4.0"
                strConStr = strConStr & ";Data Source=" & pStrCsvPath
                strConStr = strConStr & ";Extended Properties=""text;HDR=No;FMT=Delimited"""

                ' コネクションオブジェクト生成
                Cn = New System.Data.OleDb.OleDbConnection(strConStr)

                ' SQL文作成
                strSql = " SELECT * FROM [" + pStrCsvName + "] " & vbCrLf

                ' データアダプタオブジェクト生成
                da = New System.Data.OleDb.OleDbDataAdapter(strSql, Cn)

                ' 処理結果をデータテーブルに設定
                da.Fill(dtRet)

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)

                ' エラーメッセージ表示
                Call CLMsg.ShowEtarnal( _
                    Err.Number, _
                    Err.Description, _
                    SCREEN_ID, _
                    SCREEN_NAME, _
                    System.Reflection.MethodBase.GetCurrentMethod.Name() _
                )

            End Try

            ' 戻り値格納
            Return dtRet

        End Function
#End Region
#End Region

    End Module
End Namespace
#End Region
