#Region "NSMDInfo"
'===========================================================================================================
'   ネームスペース：NSMDInfo
'   モジュールＩＤ：MDSystemInfo、MDLoginInfo
'   モジュール名称：システム情報モジュール、ログイン情報モジュール
'   備考  　　　　：
'===========================================================================================================

Namespace NSMDInfo

#Region "システム情報"
    '***************************************************************************************************
    '   ＩＤ　：MDLoginInfo
    '   名称　：システム情報
    '   概要　：システム各情報
    '   作成日：2011/11/08(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  m.suzuki  新規作成
    '       　：2012/08/14(火)  Fujisaku  マスタファイル情報追加
    '         ：2013/04/19(金)  Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    ''' <summary>システム情報</summary>
    ''' <remarks></remarks>
    Public Module MDSystemInfo
        ''' <summary>
        ''' OleDbのConnectionString
        ''' </summary>
        ''' <returns></returns>
        Public Property DbConnectionString As String = ""
        ''' <summary>
        ''' SQLType(0=MDB,1=SQL Server)
        ''' </summary>
        ''' <returns></returns>
        Public Property SQLType As Integer = 1
        'Public Property AccessProvider As String = ""                ' ACCESS MDB プロバイダ
        'Public Property AccessPath As String = ""                    ' ACCESS MDB ファイルパス
        'Public Property AccessName As String = ""                    ' ACCESS MDB ファイル名
        'Public Property AccessMstPath As String = ""                 ' ACCESS MDB マスタファイルパス
        'Public Property AccessMstName As String = ""                 ' ACCESS MDB マスタファイル名
        'Public Property AccessPersistSecurity As String = ""         ' ACCESS MDB セキュリティ警告
        'Public Property AccessPassword As String = ""                ' ACCESS MDB パスワード
        Public Property MessagePath As String = ""                   ' メッセージファイルパス
        Public Property MessageName As String = ""                   ' メッセージファイル名
        Public Property SequencePath As String = ""                  ' SEQUENCEファイルパス
        Public Property EncryptKey As String = ""                    ' 暗号化キー（公開キー）
        Public Property FlgChkDisplaySize As String = ""             ' 画面サイズチェックフラグ
        Public Property AppPath As String = ""                       ' アプリケーション実行パス

#If DEBUG Then
        Public DebugMode As Boolean = True                   'デバッグモード
#Else
        Public DebugMode As Boolean = False                  'デバッグモード
#End If


    End Module
#End Region

#Region "ログイン情報"
    '***************************************************************************************************
    '   ＩＤ　：MDLoginInfo
    '   名称　：ログイン情報
    '   概要　：ログイン各情報
    '   作成日：2011/11/08(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ログイン情報</summary>
    ''' <remarks></remarks>
    Public Module MDLoginInfo

        Public Property UserId As String = ""                        ' 個人認証ID
        Public Property PeriodId As String = ""                      ' 期ID
        Public Property PeriodName As String = ""                    ' 期名称
        Public Property Period As Integer = Nothing                  ' 期（数値のみ）
        Public Property PeriodNewFlg As Byte = 0                     ' 最新期フラグ（0：最新期以外, 1：最新期）
        Public Property Ksh As String = ""                           ' 会社コード
        Public Property KshName As String = ""                       ' 会社名称
        Public Property OperatorName As String = ""                  ' 操作者
        Public Property PeriodFrom As String = ""                    ' 期間From
        Public Property PeriodTo As String = ""                      ' 期間To
        Public Property PostId As String = ""                        ' 役職ID
        Public Property PostName As String = ""                      ' 役職名称
        Public Property CommitteeId As String = ""                   ' 所属委員会ID
        Public Property CommitteeName As String = ""                 ' 所属委員会名称
        Public Property CommitteeStatusFlg As Byte = 0               ' 委員会ステータスフラグ（0：専従, 1：委員会, 2：管理部）
        Public Property CommitteeIdList As List(Of String) = Nothing ' 管理部用委員会IDリスト

    End Module
#End Region

End Namespace
#End Region
