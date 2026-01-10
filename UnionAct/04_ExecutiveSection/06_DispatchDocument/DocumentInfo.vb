Public Class DocumentInfo

    'Private _strDocCode As String = ""                  ' 01. 文書識別コード
    'Private _strDocNumber As String = ""                ' 02. 文書番号
    'Private _strManageCode As String = ""               ' 03. 管理コード
    'Private _strTemplateKind As String = ""             ' 04. 発信文書テンプレート区分
    ''Private _strTemplateFile = ""                       ' 05. 発信文書テンプレートファイル名
    'Private _strDbFileName As String = ""               ' 06. 発信文書ファイル名(表示用)
    'Private _strRealFileName As String = ""             ' 07. 発信文書エクセルファイル名(実ファイル名)
    'Private _strPeriodID As String = ""                 ' 08. 期ID
    'Private _strSubjectID As String = ""                ' 09. 標題枝番
    'Private _strSubject As String = ""                  ' 10. 標題
    'Private _strDateIssue As String = ""                ' 11. 発行日
    'Private _strLocalFileName As String = ""            ' 12. 一時ローカルファイル名（初期コピー時のファイル名）
    'Private _strApplyDate As String = ""                ' 13. 適用日付
    'Private _strDetails As String = ""                  ' 14. 詳細設定分類
    'Private _strPeriodName As String = ""               ' 15. 期名前

    'Private _arrCommitteeUpdate As String() = Nothing   ' . 選択された委員会変更ID

    'Private _bDetailsFlg As Integer = 1                 ' . 委員会への追加削除、委員長交代の場合のどちらが選択されているかのフラグ
    'Private _bCommitteeUpdateFlg As Boolean = False     ' . 委員会への追加削除もしくは委員長交代が選択されているかフラグ
    'Private _arrDocumentOut As String() = Nothing       ' . 

    '' 文書式別コード
    'Public Property strDocCode() As String
    '    Get
    '        Return _strDocCode
    '    End Get
    '    Set(ByVal value As String)
    '        _strDocCode = value
    '    End Set
    'End Property

    '' 文書番号
    'Public Property strDocNumber() As String
    '    Get
    '        Return _strDocNumber
    '    End Get
    '    Set(ByVal value As String)
    '        _strDocNumber = value
    '    End Set
    'End Property

    '' 管理コード
    'Public Property strManageCode() As String
    '    Get
    '        Return _strManageCode
    '    End Get
    '    Set(ByVal value As String)
    '        _strManageCode = value
    '    End Set
    'End Property

    '' テンプレート区分
    'Public Property strTemplateKind() As String
    '    Get
    '        Return _strTemplateKind
    '    End Get
    '    Set(ByVal value As String)
    '        _strTemplateKind = value
    '    End Set
    'End Property

    ' '' テンプレート名
    ''Public Property strTemplateFile() As String
    ''    Get
    ''        Return _strTemplateFile
    ''    End Get
    ''    Set(ByVal value As String)
    ''        _strTemplateFile = value
    ''    End Set
    ''End Property

    '' DB登録ファイル名
    'Public Property strDbFileName() As String
    '    Get
    '        Return _strDBFileName
    '    End Get
    '    Set(ByVal value As String)
    '        _strDBFileName = value
    '    End Set
    'End Property

    '' 一時ローカルファイル名
    'Public Property strLocalFileName() As String
    '    Get
    '        Return _strLocalFileName
    '    End Get
    '    Set(ByVal value As String)
    '        _strLocalFileName = value
    '    End Set
    'End Property

    '' 実ファイル名
    'Public Property strRealFileName() As String
    '    Get
    '        Return _strRealFileName
    '    End Get
    '    Set(ByVal value As String)
    '        _strRealFileName = value
    '    End Set
    'End Property

    '' 期ID
    'Public Property strPeriodId() As String
    '    Get
    '        Return _strPeriodId
    '    End Get
    '    Set(ByVal value As String)
    '        _strPeriodId = value
    '    End Set
    'End Property

    '' 標題
    'Public Property strSubject() As String
    '    Get
    '        Return _strSubject
    '    End Get
    '    Set(ByVal value As String)
    '        _strSubject = value
    '    End Set
    'End Property

    '' 標題ID
    'Public Property strSubjectId() As String
    '    Get
    '        Return _strSubjectID
    '    End Get
    '    Set(ByVal value As String)
    '        _strSubjectID = value
    '    End Set
    'End Property

    '' 発行日
    'Public Property strDateIssue() As String
    '    Get
    '        Return _strDateIssue
    '    End Get
    '    Set(ByVal value As String)
    '        _strDateIssue = value
    '    End Set
    'End Property

    '' 適用日付（要件改訂で不要っぽい）
    'Public Property strApplyDate() As String
    '    Get
    '        Return _strApplyDate
    '    End Get
    '    Set(ByVal value As String)
    '        _strApplyDate = value
    '    End Set
    'End Property

    '' 期名前
    'Public Property strPeriodName() As String
    '    Get
    '        Return _strPeriodName
    '    End Get
    '    Set(ByVal value As String)
    '        _strPeriodName = value
    '    End Set
    'End Property

    '' 委員会変更ID
    'Public Property ArrCommitteeUpdate() As String()
    '    Get
    '        Return _arrCommitteeUpdate
    '    End Get
    '    Set(ByVal value As String())
    '        _arrCommitteeUpdate = value
    '    End Set
    'End Property
End Class
