#Region "NSMDCommon"
'===========================================================================================================
'   ネームスペース：NSMDCommon
'   モジュールＩＤ：MDCommon
'   モジュール名称：共通モジュール
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDFile
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.Document

Namespace NSMDCommon
    Public Module MDCommon

#Region "定数"
        ' 画面関連
        Private Const SCREEN_ID As String = SCREEN_ID_MDCOMMON                          ' MDCommon
        Private Const SCREEN_NAME As String = SCREEN_NAME_MDCOMMON                      ' 共通モジュール
        ' log4net初期化
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "コンボボックス作成処理（定数マスタ詳細） 時間指定あり(新)"
        '***************************************************************************************************
        '   ＩＤ　：CreateCboConstantDtlDate
        '   名称　：コンボボックス作成処理（定数マスタ詳細）
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pClsDb                 As CLAccessMdb                   = アクセスMDBクラス
        '           ByVal pCbo                   As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           ByVal pStrConstantId         As String                        = 定数ID
        '           Bybal pDateRecord            As Date                          = 基準日
        '           Optional ByVal pBlnFirstItem As Boolean                       = True ：先頭空白データあり,
        '                                                                           False：先頭空白データなし
        '           Optional ByVal pBytComboBoxStyle As Byte                      = 0：テキスト編集可能,
        '                                                                           1：テキスト編集可能,
        '                                                                           2：テキスト編集不可
        '           Optional ByVal pIntDefaultSelect As Integer                   = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2013/08/31(土)  Fujisaku
        '   更新日：
        '   備考  ：MDConstにDropDownStyleの定数あります。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2013/08/31(土)  Fujisaku  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス作成処理（定数マスタ詳細）</summary>
        ''' <param name="pClsDb">アクセスMDBクラス</param>
        ''' <param name="pCboObj">コンボボックスオブジェクト</param>
        ''' <param name="pStrConstantId">定数ID</param>
        ''' <param name="pDateRecord">基準日</param>
        ''' <param name="pBlnFirstItem">先頭空白データ有無</param>
        ''' <param name="pBytComboBoxStyle">テキスト編集可否</param>
        ''' <param name="pIntDefaultSelect">初期選択</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateCboConstantDtlDate(ByVal pClsDb As CLAccessMdb, _
                                             ByVal pCboObj As System.Windows.Forms.ComboBox, _
                                             ByVal pStrConstantId As String, _
                                             ByVal pDateRecord As Date, _
                                             Optional ByVal pBlnFirstItem As Boolean = True, _
                                             Optional ByVal pBytComboBoxStyle As Byte = 2, _
                                             Optional ByVal pIntDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim strSql As String = ""                                       ' SQL文
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim dtBlank As DataRow = Nothing                                ' データロー
            Dim strRcdDate As String = pDateRecord.ToString("yyyyMMdd")     ' 基準日
            Try
                ' 初期処理
                pCboObj.BeginUpdate()                                       ' チラつき防止の為、最後まで描写しない
                pCboObj.DataSource = Nothing                                ' データソース初期化
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                ' SQL文
                strSql = "" & vbCrLf
                strSql = strSql & "   SELECT c_constant_seq" & vbCrLf
                strSql = strSql & "         ,l_name" & vbCrLf
                strSql = strSql & "     FROM constant_dtl" & vbCrLf
                strSql = strSql & "    WHERE c_constant = '" & pStrConstantId & "'" & vbCrLf
                strSql = strSql & "     AND d_from <= '" & strRcdDate & "'" & vbCrLf
                strSql = strSql & "     AND d_to >= '" & strRcdDate & "'" & vbCrLf
                strSql = strSql & " ORDER BY s_order" & vbCrLf  'chk
                dtRet = pClsDb.ExecuteSql(strSql)                           ' SQL実行
                ' 0件チェック
                If dtRet.Rows.Count = 0 Then
                    Return blnRet
                End If
                ' 先頭空白
                If pBlnFirstItem Then
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                pCboObj.DropDownStyle = pBytComboBoxStyle                   ' 外観機能（Simple, DropDown, DropDownList）
                pCboObj.DataSource = dtRet                                  ' データソース設定
                pCboObj.DisplayMember = "l_name"                            ' コンボボックス名称設定
                pCboObj.ValueMember = "c_constant_seq"                      ' コンボボックス値設定
                pCboObj.SelectedIndex = pIntDefaultSelect                   ' 指定した初期選択（デフォルトは 0番目）
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス作成処理（定数マスタ詳細） 時間指定なし(旧)"
        '***************************************************************************************************
        '   ＩＤ　：CreateCboConstantDtl
        '   名称　：コンボボックス作成処理（定数マスタ詳細）
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pClsDb                 As CLAccessMdb                   = アクセスMDBクラス
        '           ByVal pCbo                   As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           ByVal pStrConstantId         As String                        = 定数ID
        '           Optional ByVal pBlnFirstItem As Boolean                       = True ：先頭空白データあり,
        '                                                                           False：先頭空白データなし
        '           Optional ByVal DropDownStyle As Byte                          = 0：テキスト編集可能,
        '                                                                           1：テキスト編集可能,
        '                                                                           2：テキスト編集不可
        '           Optional ByVal pIntDefaultSelect As Integer                   = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/10/27(木) m.suzuki
        '   更新日：
        '   備考  ：MDConstにDropDownStyleの定数あります。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス作成処理（定数マスタ詳細）</summary>
        ''' <param name="pClsDb">アクセスMDBクラス</param>
        ''' <param name="pCboObj">コンボボックスオブジェクト</param>
        ''' <param name="pStrConstantId">定数ID</param>
        ''' <param name="pBlnFirstItem">先頭空白データ有無</param>
        ''' <param name="pBytComboBoxStyle">テキスト編集可否</param>
        ''' <param name="pIntDefaultSelect">初期選択</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateCboConstantDtl(ByVal pClsDb As CLAccessMdb, _
                                             ByVal pCboObj As System.Windows.Forms.ComboBox, _
                                             ByVal pStrConstantId As String, _
                                             Optional ByVal pBlnFirstItem As Boolean = True, _
                                             Optional ByVal pBytComboBoxStyle As Byte = 2, _
                                             Optional ByVal pIntDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim strSql As String = ""                                       ' SQL文
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim dtBlank As DataRow = Nothing                                ' データロー
            Try
                ' 初期処理
                pCboObj.BeginUpdate()                                       ' チラつき防止の為、最後まで描写しない
                pCboObj.DataSource = Nothing                                ' データソース初期化
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                ' SQL文
                strSql = "" & vbCrLf
                strSql = strSql & "   SELECT c_constant_seq" & vbCrLf
                strSql = strSql & "         ,l_name" & vbCrLf
                strSql = strSql & "     FROM constant_dtl" & vbCrLf
                strSql = strSql & "    WHERE c_constant = '" & pStrConstantId & "'" & vbCrLf
                strSql = strSql & " ORDER BY s_order" & vbCrLf  'chk
                dtRet = pClsDb.ExecuteSql(strSql)                           ' SQL実行
                ' 0件チェック
                If dtRet.Rows.Count = 0 Then
                    Return blnRet
                End If
                ' 先頭空白
                If pBlnFirstItem Then
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                pCboObj.DropDownStyle = pBytComboBoxStyle                   ' 外観機能（Simple, DropDown, DropDownList）
                pCboObj.DataSource = dtRet                                  ' データソース設定
                pCboObj.DisplayMember = "l_name"                            ' コンボボックス名称設定
                pCboObj.ValueMember = "c_constant_seq"                      ' コンボボックス値設定
                pCboObj.SelectedIndex = pIntDefaultSelect                   ' 指定した初期選択（デフォルトは 0番目）
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス作成処理（SQL）"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBox
        '   名称　：コンボボックス作成処理（SQL）
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pClsMdb                As CLAccessMdb                   = アクセスMDBクラス
        '           ByVal pCbo                   As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           ByVal pStrSql                As String                        = SQL文
        '           ByVal pStrDisplay            As String                        = 名称
        '           ByVal pStrValue              As String                        = 値
        '           Optional ByVal pBlnFirstItem As Boolean                       = True ：先頭空白データあり,
        '                                                                           False：先頭空白データなし
        '           Optional ByVal DropDownStyle As Byte                          = 0：テキスト編集可能,
        '                                                                           1：テキスト編集可能,
        '                                                                           2：テキスト編集不可
        '           Optional ByVal pIntDefaultSelect As Integer                   = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/10/27(木) m.suzuki
        '   更新日：
        '   備考  ：MDConstにDropDownStyleの定数あります。
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス作成処理（SQL）</summary>
        ''' <param name="pClsMdb">アクセスMDBクラス</param>
        ''' <param name="pCboObj">コンボボックスオブジェクト</param>
        ''' <param name="pStrSql">SQL文</param>
        ''' <param name="pStrDisplay">名称</param>
        ''' <param name="pStrValue">値</param>
        ''' <param name="pBlnFirstItem">先頭空白データ有無</param>
        ''' <param name="pBytComboBoxStyle">テキスト編集可否</param>
        ''' <param name="pIntDefaultSelect">初期選択</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateComboBoxNew(ByVal pClsMdb As CLAccessMdb, _
                                          ByVal pCboObj As System.Windows.Forms.ComboBox, _
                                          ByVal pStrSql As String, _
                                          ByVal pStrDisplay As String, _
                                          ByVal pStrValue As String, _
                                          Optional ByVal pBlnFirstItem As Boolean = True, _
                                          Optional ByVal pBytComboBoxStyle As Byte = 2, _
                                          Optional ByVal pIntDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim dtBlank As DataRow = Nothing                                ' データロー
            Try
                ' 初期処理
                pCboObj.Visible = False
                pCboObj.BeginUpdate()                                       ' チラつき防止の為、最後まで描写しない
                pCboObj.DataSource = Nothing                                ' データソース初期化
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                dtRet = pClsMdb.ExecuteSql(pStrSql)                         ' SQL実行
                ' 0件チェック
                If dtRet.Rows.Count = 0 Then
                    Return blnRet
                End If
                ' 先頭空白データ
                If pBlnFirstItem Then
                    ' 先頭空白データ有の場合、空白データを作成
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                pCboObj.DropDownStyle = pBytComboBoxStyle                   ' テキスト編集不可
                pCboObj.DataSource = dtRet                                  ' データソース設定
                pCboObj.DisplayMember = pStrDisplay                         ' コンボボックス名称設定
                pCboObj.ValueMember = pStrValue                             ' コンボボックス値設定
                ' 先頭アイテム表示
                If pCboObj.Items.Count = 0 Then
                    Call MessageBox.Show("データがありません")
                    Return blnRet
                Else
                    pCboObj.SelectedIndex = pIntDefaultSelect               ' 先頭データ表示
                End If
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
                pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
                pCboObj.Visible = True
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス作成処理"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBox
        '   名称　：コンボボックス作成処理
        '   概要　：引数の情報でコンボボックスのリストを作成する。
        '   引数　：ByVal pMdb        As CLAccessMdb                   = アクセスMDBクラス
        '           ByVal pCbo        As System.Windows.Forms.ComboBox = コンボボックス
        '           ByVal pStrTbl     As String                        = テーブル名
        '           ByVal pStrDisplay As String                        = 名称
        '           ByVal pStrValue   As String                        = 値
        '           ByVal pStrKey     As String                        = ソートキー
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/10/27(木)  m.suzuki
        '   更新日：
        '   備考  ：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/27(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス作成処理</summary>
        ''' <param name="pMdb">アクセスMDBクラス</param>
        ''' <param name="pCbo">コンボボックス名</param>
        ''' <param name="pStrTbl">テーブル名</param>
        ''' <param name="pStrDisplay">名称</param>
        ''' <param name="pStrValue">値</param>
        ''' <param name="pStrKey">ソートキー</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateComboBox(ByVal pMdb As CLAccessMdb, _
                                       ByVal pCbo As System.Windows.Forms.ComboBox, _
                                       ByVal pStrTbl As String, _
                                       ByVal pStrDisplay As String, _
                                       ByVal pStrValue As String, _
                                       Optional ByVal pStrKey As String = Nothing) As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Dim dt As DataTable = Nothing       ' 処理結果データテーブル
            Dim sql As String = Nothing
            Try
                ' 初期処理
                pCbo.BeginUpdate()                                          ' チラつき防止の為、最後まで描写しない
                pCbo.DataSource = Nothing                                   ' データソース初期化
                pCbo.Items.Clear()                                          ' コンボボックスリストクリア
                ' SQL作成
                sql = "" & vbCrLf
                sql = sql & " SELECT " & pStrValue & vbCrLf
                sql = sql & "       ," & pStrDisplay & vbCrLf
                sql = sql & "   FROM " & pStrTbl & vbCrLf
                sql = sql & "  ORDER BY " & pStrKey & vbCrLf    'chk
                sql = sql & ";" & vbCrLf
                dt = pMdb.ExecuteSql(sql)                                   ' SQL実行
                If Not dt Is Nothing Then
                    pCbo.DropDownStyle = ComboBoxStyle.DropDownList         ' テキスト編集不可
                    pCbo.DataSource = dt                                    ' データソース設定
                    pCbo.DisplayMember = pStrDisplay                        ' コンボボックス名称設定
                    pCbo.ValueMember = pStrValue                            ' コンボボックス値設定
                    ' 先頭アイテム表示
                    If pCbo.Items.Count = 0 Then
                        Call MessageBox.Show("データがありません")
                        Return False
                    Else
                        'pCbo.Text = cbo.GetItemText(cbo.Items(0))
                        pCbo.SelectedIndex = 0                              ' 先頭データ表示
                    End If
                    pCbo.EndUpdate()                                        ' チラつき防止の為、最後に描写する
                Else
                    Return False
                End If
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                pCbo.DataSource = Nothing                                   ' コンボボックスデータソース削除
                pCbo.Items.Clear()                                          ' コンボボックスリストクリア
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCbo.EndUpdate()                                            ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス（年）作成処理"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBoxYear
        '   名称　：コンボボックス（年）作成処理
        '   概要　：選択された期に属する年数をコンボボックスに格納する。
        '   引数　：ByVal pCbo                   As System.Windows.Forms.ComboBox,
        '           Optional ByVal pBlnFirstItem As Boolean = True
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/21(月)  a.onuma
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/21(月)  a.onuma  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス（年）作成処理</summary>
        ''' <param name="pCbo">コンボボックス</param>
        ''' <param name="pBlnFirstItem">先頭空白データ有無</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateComboBoxYear(ByVal pCbo As System.Windows.Forms.ComboBox, _
                                           Optional ByVal pBlnFirstItem As Boolean = True) As Boolean
            Dim blnRet As Boolean = False
            Dim yearList As List(Of String) = New List(Of String)()
            Try
                pCbo.BeginUpdate()                                          ' チラつき防止の為、最後まで描写しない
                pCbo.Items.Clear()
                If MDLoginInfo.PeriodFrom = Nothing OrElse MDLoginInfo.PeriodTo = Nothing Then
                    Call MessageBox.Show("開始年、または終了年が設定されていません。")
                    Return blnRet
                Else
                    If pBlnFirstItem = True Then
                        pCbo.Items.Add("")
                    End If
                    pCbo.Items.Add(MDLoginInfo.PeriodFrom.Substring(0, 4))  ' 期の開始年
                    pCbo.Items.Add(MDLoginInfo.PeriodTo.Substring(0, 4))    ' 期の終了年
                    pCbo.DropDownStyle = ComboBoxStyle.DropDownList         ' テキスト編集不可
                    pCbo.SelectedIndex = 0
                    blnRet = True
                End If
            Catch ex As Exception
                pCbo.Items.Clear()
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCbo.EndUpdate()                                            ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス（月）作成処理"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBoxMonth
        '   名称　：コンボボックス（月）作成処理
        '   概要　：選択された年に対応する月をコンボボックスに格納する。
        '   引数　：ByVal strSelectYear          As String                        = 年
        '           ByVal pCbo                   As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           Optional ByVal pBlnFirstItem As Boolean                       = True ：先頭空白データあり,
        '                                                                           False：先頭空白データなし
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/22(月)  a.onuma
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/22(月)  a.onuma  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス（月）作成処理</summary>
        ''' <param name="strSelectYear">年</param>
        ''' <param name="pCbo">コンボボックスオブジェクト</param>
        ''' <param name="pBlnFirstItem">先頭空白データ有無</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateComboBoxMonth(ByVal strSelectYear As String, _
                                            ByVal pCbo As System.Windows.Forms.ComboBox, _
                                            Optional ByVal pBlnFirstItem As Boolean = True) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim strStartMonth As String = String.Empty                      ' その期の開始月
            Dim monthList As List(Of String) = New List(Of String)
            Try
                pCbo.BeginUpdate()                                          ' チラつき防止の為、最後まで描写しない
                pCbo.Items.Clear()
                '空文字を先頭に格納
                If pBlnFirstItem = True Then
                    pCbo.Items.Add("")
                End If
                'テキスト編集不可
                pCbo.DropDownStyle = ComboBoxStyle.DropDownList
                If strSelectYear.Equals(MDLoginInfo.PeriodFrom.Substring(0, 4)) Then
                    strStartMonth = MDLoginInfo.PeriodFrom.Substring(4, 2)
                    monthList.Add(strStartMonth)
                    Dim strMonth = (CInt(strStartMonth) + 1).ToString().PadLeft(2, "0")
                    '年が変わるまで月を加算し格納する
                    While (CInt(strMonth) < 13)
                        monthList.Add(strMonth)
                        strMonth = (CInt(strMonth) + 1).ToString().PadLeft(2, "0")
                    End While
                    pCbo.Items.AddRange(monthList.ToArray())
                    blnRet = True
                ElseIf strSelectYear.Equals(MDLoginInfo.PeriodTo.Substring(0, 4)) Then
                    strStartMonth = MDLoginInfo.PeriodTo.Substring(4, 2)
                    monthList.Add(strStartMonth)
                    Dim strMonth = (CInt(strStartMonth) - 1).ToString().PadLeft(2, "0")
                    '年が変わるまで月を加算し格納する
                    While (CInt(strMonth) > 0)
                        monthList.Add(strMonth)
                        strMonth = (CInt(strMonth) - 1).ToString().PadLeft(2, "0")
                    End While
                    monthList.Sort()
                    pCbo.Items.AddRange(monthList.ToArray())
                    blnRet = True
                End If
            Catch ex As Exception
                pCbo.Items.Clear()
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                pCbo.EndUpdate()                                            ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス（年）作成処理（ログイン期のFromTo）"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBoxYYYY
        '   名称　：コンボボックス（年）作成処理
        '   概要　：年コンボボックスリストを作成する。
        '   引数　：ByVal iCbo                       As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           Optional ByVal iBlnFirstItem     As Boolean                       = True ：先頭空白データあり,
        '                                                                               False：先頭空白データなし
        '           Optional ByVal iBytComboBoxStyle As Byte                          = 0：テキスト編集可能,
        '                                                                               1：テキスト編集可能,
        '                                                                               2：テキスト編集不可
        '           Optional ByVal iIntDefaultSelect As Integer                       = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/02/07(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/07(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス（月）作成処理</summary>
        ''' <param name="iCbo">コンボボックスオブジェクト</param>
        ''' <param name="iBlnFirstItem">先頭空白データ有無</param>
        ''' <param name="iBytComboBoxStyle">コンボボックススタイル</param>
        ''' <param name="iIntDefaultSelect">初期選択</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function CreateComboBoxYYYY(ByVal iCbo As System.Windows.Forms.ComboBox,
                                           Optional ByVal iBlnFirstItem As Boolean = True,
                                           Optional ByVal iBytComboBoxStyle As Byte = 2,
                                           Optional ByVal iIntDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim intFristYear As Integer = 0                                 ' 年始月
            Dim intLastYear As Integer = 0                                  ' 年末月
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim drRet As DataRow = Nothing                                  ' データロー
            Dim dtBlank As DataRow = Nothing                                ' 空白データロー
            Try
                iCbo.BeginUpdate()                                          ' チラつき防止の為、最後まで描写しない
                iCbo.DataSource = Nothing                                   ' リストクリア
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("YearValue", GetType(Integer))
                dtRet.Columns.Add("YearDisplay", GetType(String))
                ' 先頭空白データ有の場合、空白データを作成
                If iBlnFirstItem Then
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                ' ログインした期のFromToを取得
                intFristYear = CInt(MDLoginInfo.PeriodFrom.Substring(0, 4)) ' ログイン期のFrom年
                intLastYear = CInt(MDLoginInfo.PeriodTo.Substring(0, 4))    ' ログイン期のToの年
                ' 年数分作成
                For i = intFristYear To intLastYear
                    drRet = dtRet.NewRow()                                  ' 新しいデータロー作成
                    drRet(0) = i                                            ' 値
                    drRet(1) = i.ToString().PadLeft(4, "0")                 ' 表示
                    dtRet.Rows.Add(drRet)                                   ' データ追加
                Next
                ' コンボボックス各設定
                iCbo.DropDownStyle = iBytComboBoxStyle                      ' ドロップダウンスタイル設定
                iCbo.DataSource = dtRet                                     ' データソース設定
                iCbo.ValueMember = "YearValue"                              ' コンボボックス値設定
                iCbo.DisplayMember = "YearDisplay"                          ' コンボボックス名称設定
                iCbo.SelectedIndex = iIntDefaultSelect                      ' 先頭データ選択
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                iCbo.Items.Clear()                                          ' コンボボックスクリア
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                iCbo.EndUpdate()                                            ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス（月）作成処理（1～12）"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBoxMM
        '   名称　：コンボボックス（月）作成処理
        '   概要　：月コンボボックスリストを作成する。
        '   引数　：ByVal objCbo                    As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           Optional ByVal blnFirstItem     As Boolean                       = True ：先頭空白データあり,
        '                                                                              False：先頭空白データなし
        '           Optional ByVal bytComboBoxStyle As Byte                          = 0：テキスト編集可能,
        '                                                                              1：テキスト編集可能,
        '                                                                              2：テキスト編集不可
        '           Optional ByVal intDefaultSelect As Integer                       = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/01/13(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/13(金)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>コンボボックス（月）作成処理</summary>
        ''' <param name="objCbo">コンボボックスオブジェクト</param>
        ''' <param name="blnFirstItem"></param>
        ''' <param name="bytComboBoxStyle"></param>
        ''' <param name="intDefaultSelect"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateComboBoxMM(ByVal objCbo As System.Windows.Forms.ComboBox, _
                                         Optional ByVal blnFirstItem As Boolean = True, _
                                         Optional ByVal bytComboBoxStyle As Byte = 2, _
                                         Optional ByVal intDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim intFristMonth As Integer = 1                                ' 年始月
            Dim intLastMonth As Integer = 12                                ' 年末月
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim drRet As DataRow = Nothing                                  ' データロー
            Dim dtBlank As DataRow = Nothing                                ' 空白データロー
            Try
                objCbo.BeginUpdate()                                        ' チラつき防止の為、最後まで描写しない
                objCbo.DataSource = Nothing                                 ' リストクリア
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("MonthValue", GetType(Integer))
                dtRet.Columns.Add("MonthDisplay", GetType(String))
                ' 先頭空白データ有の場合、空白データを作成
                If blnFirstItem Then
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                ' 月数分作成
                For i = intFristMonth To intLastMonth
                    drRet = dtRet.NewRow()                                  ' 新しいデータロー作成
                    drRet(0) = i                                            ' 値
                    drRet(1) = i.ToString().PadLeft(2, "0")                 ' 表示
                    dtRet.Rows.Add(drRet)                                   ' データ追加
                Next
                ' コンボボックス各設定
                objCbo.DropDownStyle = bytComboBoxStyle                     ' ドロップダウンスタイル設定
                objCbo.DataSource = dtRet                                   ' データソース設定
                objCbo.ValueMember = "MonthValue"                           ' コンボボックス値設定
                objCbo.DisplayMember = "MonthDisplay"                       ' コンボボックス名称設定
                objCbo.SelectedIndex = intDefaultSelect                     ' 先頭データ選択
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                objCbo.Items.Clear()                                        ' コンボボックスクリア
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                objCbo.EndUpdate()                                          ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "コンボボックス（日）作成処理（1～月末日）"
        '***************************************************************************************************
        '   ＩＤ　：CreateComboBoxDD
        '   名称　：コンボボックス（日）作成処理
        '   概要　：選択された年月に対応する日コンボボックスリストを作成する。
        '   引数　：ByVal objCbo                    As System.Windows.Forms.ComboBox = コンボボックスオブジェクト
        '           ByVal intYear                   As Integer                       = 年
        '           ByVal intMonth                  As Integer                       = 月
        '           Optional ByVal blnFirstItem     As Boolean                       = True ：先頭空白データあり,
        '                                                                              False：先頭空白データなし
        '           Optional ByVal bytComboBoxStyle As Byte                          = 0：テキスト編集可能,
        '                                                                              1：テキスト編集可能,
        '                                                                              2：テキスト編集不可
        '           Optional ByVal intDefaultSelect As Integer                       = 初期選択
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/01/13(金)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/13(金)  m.suzuki  新規作成
        '***************************************************************************************************
        Public Function CreateComboBoxDD(ByVal objCbo As System.Windows.Forms.ComboBox, _
                                         ByVal intYear As Integer, _
                                         ByVal intMonth As Integer, _
                                         Optional ByVal blnFirstItem As Boolean = True, _
                                         Optional ByVal bytComboBoxStyle As Byte = 2, _
                                         Optional ByVal intDefaultSelect As Integer = 0) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim intLastDay As Integer = 0                                   ' 月末日
            Dim dtRet As DataTable = Nothing                                ' データテーブル
            Dim drRet As DataRow = Nothing                                  ' データロー
            Dim dtBlank As DataRow = Nothing                                ' 空白データロー
            Try
                objCbo.BeginUpdate()                                        ' チラつき防止の為、最後まで描写しない
                objCbo.DataSource = Nothing                                 ' リストクリア
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("DayValue", GetType(Integer))
                dtRet.Columns.Add("DayDisplay", GetType(String))
                ' 先頭空白データ有の場合、空白データを作成
                If blnFirstItem Then
                    dtBlank = dtRet.NewRow()
                    dtRet.Rows.InsertAt(dtBlank, 0)
                End If
                ' 日数分作成
                intLastDay = Date.DaysInMonth(intYear, intMonth)            ' 月末日取得
                For i = 1 To intLastDay
                    drRet = dtRet.NewRow()                                  ' 新しいデータロー作成
                    drRet(0) = i                                            ' 値
                    drRet(1) = i.ToString().PadLeft(2, "0")                 ' 表示
                    dtRet.Rows.Add(drRet)                                   ' データ追加
                Next
                ' コンボボックス各設定
                objCbo.DropDownStyle = bytComboBoxStyle                     ' ドロップダウンスタイル設定
                objCbo.DataSource = dtRet                                   ' データソース設定
                objCbo.ValueMember = "DayValue"                             ' コンボボックス値設定
                objCbo.DisplayMember = "DayDisplay"                         ' コンボボックス名称設定
                objCbo.SelectedIndex = intDefaultSelect                     ' 先頭データ選択
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                objCbo.Items.Clear()                                        ' コンボボックスクリア
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                objCbo.EndUpdate()                                          ' チラつき防止の為、最後に描写する
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "Windowsフォーム上コントロール列挙処理"
        '***************************************************************************************************
        '   ＩＤ　：GetAllControls
        '   名称　：Windowsフォーム上コントロール列挙処理
        '   概要  ：Windowsフォーム上のすべてのコントロールを列挙する。
        '   引数　：ByVal pCon As Control = フォーム（Me）
        '   戻り値：コントロールリスト
        '   作成日：2011/11/01(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/01(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>Windowsフォーム上コントロール列挙処理</summary>
        ''' <param name="pCtl">フォーム（Me）</param>
        ''' <returns>コントロールリスト</returns>
        ''' <remarks></remarks>
        Public Function GetAllControls(ByVal pCtl As Control) As Control()
            Dim conRet As Control() = Nothing       ' 処理結果コントロール
            Dim ListCtrls As ArrayList = New ArrayList
            Try
                For Each ctl As Control In pCtl.Controls
                    ListCtrls.Add(ctl)
                    ListCtrls.AddRange(GetAllControls(ctl))
                Next
                ' 処理結果にコントロールを設定
                conRet = CType(ListCtrls.ToArray(GetType(Control)), Control())
            Catch ex As Exception
                log.Fatal(ex.Message)               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return conRet                           ' 戻り値設定
        End Function
#End Region

#Region "エラー箇所設定処理"
        '***************************************************************************************************
        '   ＩＤ　：SetErr
        '   名称　：エラー箇所設定処理
        '   概要　：Windowsフォーム上のエラー箇所のコントロールのバックカラーをピンク色にする。
        '   引数　：ByVal pCtl As Control = コントロール
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/22(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/22(火)  m.suzuki  新規作成
        '***************************************************************************************************
        Public Function SetErr(ByVal pCtl As Control) As Boolean
            Dim blnRet As Boolean = False       ' 処理結果
            Try
                pCtl.BackColor = Color.Pink     ' 指定コントロールのバックカラーをピンク色に設定
                blnRet = True                   ' 処理結果に正常を格納
            Catch ex As Exception
                log.Fatal(ex.Message)           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                       ' 戻り値格納
        End Function
#End Region

#Region "エラー箇所クリア処理"
        '***************************************************************************************************
        '   ＩＤ　：ClearErr
        '   名称　：エラー箇所クリア処理
        '   概要　：Windowsフォーム上のエラー箇所のコントロールのバックカラーを白色にする。
        '   引数　：ByVal pCtl As Control = フォーム（Me）
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/10/26(水)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/10/26(水)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>エラー箇所クリア処理</summary>
        ''' <param name="pCtl">フォーム（Me）</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function ClearErr(ByVal pCtl As Control) As Boolean
            Dim blnRet As Boolean = False                                           ' 処理結果
            Dim ctlAll As Control() = Nothing
            Try
                ctlAll = GetAllControls(pCtl)                                       ' フォーム上のすべてのコントロール名取得
                If Not ctlAll Is Nothing Then                                       ' コントロールがあるかチェック
                    For Each ctl As Control In ctlAll                               ' コントロール数分ループ
                        ' テキストボックスかコンボボックスかリストボックスか
                        ' デートタイムピッカーかマスクドテキストボックスかチェック
                        If (TypeOf ctl Is System.Windows.Forms.TextBox) _
                        Or (TypeOf ctl Is System.Windows.Forms.ComboBox) _
                        Or (TypeOf ctl Is System.Windows.Forms.ListBox) _
                        Or (TypeOf ctl Is System.Windows.Forms.DateTimePicker) _
                        Or (TypeOf ctl Is System.Windows.Forms.MaskedTextBox) Then
                            ctl.BackColor = Color.White                             ' バックカラーを白色にする
                        End If
                    Next
                    blnRet = True                                                   ' 処理結果に正常を格納
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                                                           ' 戻り値格納
        End Function
#End Region

#Region "画面中央表示処理"
        '***************************************************************************************************
        '   ＩＤ　：SetFormCenter
        '   名称　：画面中央表示処理
        '   概要　：フォームを画面中央に設定する。
        '   引数　：ByVal pFrm As Windows.Forms.Form = フォーム
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/01(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/01(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>画面中央表示処理</summary>
        ''' <param name="pFrm">フォーム</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function SetFormCenter(ByVal pFrm As Windows.Forms.Form) As Boolean
            Dim blnRet As Boolean = False                                   ' 処理結果
            Dim IntDisplayWidth As Integer = 0                              ' 画面幅
            Dim IntDisplayHeight As Integer = 0                             ' 画面高さ
            Dim IntFrmLeft As Integer = 0                                   ' 設定前フォーム左位置値
            Dim IntFrmTop As Integer = 0                                    ' 設定前フォーム上位置値
            Try
                IntFrmLeft = pFrm.Left                                      ' 設定前フォーム左位置値取得
                IntFrmTop = pFrm.Top                                        ' 設定前フォーム上位置値取得
                IntDisplayWidth = SystemInformation.WorkingArea.Width       ' 画面幅取得
                IntDisplayHeight = SystemInformation.WorkingArea.Height     ' 画面高さ取得
                pFrm.Left = (IntDisplayWidth - pFrm.Width) / 2              ' フォームを画面の水平方向にセンタリング
                pFrm.Top = (IntDisplayHeight - pFrm.Height) / 2             ' フォームを画面の縦方向にセンタリング
                blnRet = True                                               ' 処理結果に正常を格納
            Catch ex As Exception
                pFrm.Left = IntFrmLeft                                      ' 設定前フォーム左位置設定
                pFrm.Top = IntFrmTop                                        ' 設定前フォーム上位置設定
                log.Fatal(ex.Message)                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return blnRet                                                   ' 戻り値格納
        End Function
#End Region

#Region "NULL変換処理"
        '***************************************************************************************************
        '   ＩＤ　：NVL
        '   名称　：NULL変換処理
        '   概要　：指定文字列がNULLの場合、空文字を返す。
        '           指定文字列がNULLではない場合、そのまま文字列を返す。
        '   引数　：ByVal obj As Object = 対象オブジェクト
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/11/21(月)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/11/21(月)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>NULL変換処理</summary>
        ''' <param name="obj">対象文字列</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function NVL(ByVal obj As Object) As String
            Dim strRet As String = ""       ' 処理結果
            Try
                If IsDBNull(obj) Then       ' NULLかチェック
                    strRet = ""             ' NULLの場合、空文字を返す
                Else
                    strRet = obj            ' NULLではない場合、そのまま文字列を返す
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            End Try
            Return strRet                   ' 戻り値設定
        End Function
#End Region

#Region "対象日付のチェック"
        '***************************************************************************************************
        '   ＩＤ　：ChkTargetDate
        '   名称　：対象日付のチェック
        '   概要　：
        '   引数　：ByVal strPeriodId    As String = 期ID,
        '           ByVal strCommitteeId As String = 委員会ID,
        '           ByVal strTargetDate  As String = 対象日付
        '   戻り値：True = 正常, False = 異常
        '   作成日：2011/12/08(木)  a.onuma
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/12/08(木)  a.onuma  新規作成
        '***************************************************************************************************
        Public Function ChkTargetDate(ByVal strPeriodId As String, _
                                      ByVal strCommitteeId As String, _
                                      ByVal strTargetDate As String) As Boolean
            Dim blnRet As Boolean = False                                                   ' チェック結果
            Dim strSql As String = String.Empty                                             ' SQL
            Dim dtRet As DataTable = Nothing                                                ' 検索結果取得用
            Dim clsMdb As CLAccessMdb = New CLAccessMdb                                     ' データベースクラス
            Dim strFromMonth As String = String.Empty
            Dim strToMonth As String = String.Empty
            Dim strFromYear As String = String.Empty
            Dim strToYear As String = String.Empty
            Try
                If strTargetDate.Length < 7 Then
                    strTargetDate = strTargetDate & "01"
                End If
                '委員会が対象年月で有効であるかチェック
                strSql = "SELECT c_committee_id FROM committee WHERE c_committee_id = '" & strCommitteeId & _
                        "' AND d_from <= '" & strTargetDate & _
                        "' AND d_to >= '" & strTargetDate & "' "
                'DB接続開始
                clsMdb.Connect()
                dtRet = clsMdb.ExecuteSql(strSql)
                If dtRet.Rows.Count < 1 Then
                    MsgBox("選択された委員会は、指定の年月では有効ではありません。")
                    Return blnRet
                End If
                '対象委員会に所属する役職の最小開始月、最大終了月を取得
                strSql = "SELECT Min(cdtl.d_service_from) AS MinFrom, Max(cdtl.d_service_to) As MaxTo FROM committee_dtl cdtl ,committee com " & _
                        "WHERE com.c_committee_id = cdtl.c_committee_id AND com.d_from = cdtl.d_from" & _
                        " AND com.c_committee_id = '" & strCommitteeId & "' AND com.d_from <= '" & strTargetDate & _
                        "' AND com.d_to >= '" & strTargetDate & "' "
                ' SQL実行
                dtRet = clsMdb.ExecuteSql(strSql)
                If dtRet.Rows.Count > 0 Then
                    strFromMonth = dtRet.Rows(0).Item(0)
                    strToMonth = dtRet.Rows(0).Item(1)
                    '対象の期の開始年、終了年を取得
                    strSql = "SELECT Mid(d_from,1,4) AS FromYear ,Mid(d_to,1,4) AS ToYear FROM period WHERE c_period_id = '" & strPeriodId & "'"
                    dtRet = clsMdb.ExecuteSql(strSql)
                    If dtRet.Rows.Count > 0 Then
                        Dim fromDate As Date = CDate(dtRet.Rows(0).Item(0) & "/" & strFromMonth & "/01")
                        Dim toDate As Date = CDate(dtRet.Rows(0).Item(1) & "/" & strToMonth & "/01").AddMonths(1).AddDays(-1)
                        strTargetDate = strTargetDate.Insert(4, "/").Insert(7, "/")
                        If strTargetDate >= fromDate AndAlso strTargetDate <= toDate Then
                            '対象年月が範囲内であればTrueを返却
                            blnRet = True
                        Else
                            CLMsg.Show("GE0013", fromDate.Date, toDate.Date)
                            Return blnRet
                        End If
                    Else
                        MsgBox("指定された期の情報が取得できませんでした。")
                        Return blnRet
                    End If
                Else
                    MsgBox("選択された委員会は、指定の年月では有効ではありません。")
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_MDCOMMON, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                Call clsMdb.Disconnect()                                                        ' DB接続終了
            End Try
            Return blnRet
        End Function
#End Region

#Region "権限情報取得処理"
        '***************************************************************************************************
        '   ＩＤ　：getGrant
        '   名称　：権限情報取得処理
        '   概要　：委員会画面詳細テーブルとメニューコントロールマスタを参照して権限を取得する。
        '   引数　：ByVal iMenuId As String    = メニューID
        '   戻り値：getGrant      As DataTable = 取得したデータ
        '   作成日：2011/12/20(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2011/12/20(火)  m.suzuki  新規作成
        '       　：2011/12/21(水） 更新　取得する権限をログインしている期のもののみに変更
        '***************************************************************************************************
        Public Function getGrant(ByVal iMenuId As String) As DataTable
            Dim strSql As String = ""                   ' SQL文
            Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル
            Dim clsDb As CLAccessMdb = New CLAccessMdb  ' データベースクラス
            Dim strPSql As String = ""                  ' 期情報取得SQL文
            Dim dt As DataTable = Nothing               ' 期情報格納テーブル
            Dim loginPeriodfrom As String = ""          ' ログイン期の適用開始日
            Dim HitPeriodFrom As Integer                ' ログイン期結果(0=最新期/1=前期/それ以外=前々期以前)
            Dim nowPeriodFrom As String = String.Empty
            Try
                ' 期マスタから各期の適用開始日を取得する
                strPSql = "" & vbCrLf
                strPSql = strPSql & " SELECT d_from" & vbCrLf
                strPSql = strPSql & "   FROM period " & vbCrLf
                strPSql = strPSql & "  ORDER BY d_from DESC" & vbCrLf   'chk
                strPSql = strPSql & ";" & vbCrLf
                clsDb.Connect()
                ' 情報取得
                dt = clsDb.ExecuteSql(strPSql)
                'ログイン期の適用開始日を取得
                loginPeriodfrom = MDLoginInfo.PeriodFrom
                For i = 0 To dt.Rows.Count - 1
                    nowPeriodFrom = dt.Rows(i).Item(0).ToString
                    If loginPeriodfrom = nowPeriodFrom Then
                        HitPeriodFrom = i
                    End If
                Next
                clsDb.Disconnect()
                ' SQL文作成
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT dtl.c_committee_id" & vbCrLf                 ' 01. 委員会ID
                strSql = strSql & "       ,dtl.s_committee_seq" & vbCrLf                ' 02. 委員会ID枝番（役職ID）
                strSql = strSql & "       ,dtl.c_menu_id" & vbCrLf                      ' 03. メニューID
                If HitPeriodFrom = 0 Then
                    ' 現在期
                    strSql = strSql & "       ,dtl.c_now_reference" & vbCrLf            ' 04. 現在期参照権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_now_reg" & vbCrLf                  ' 05. 現在期登録権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_now_print" & vbCrLf                ' 06. 現在期印刷権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_now_output_file" & vbCrLf          ' 07. 現在期ファイル出力権限(可=1/不可=0)
                ElseIf HitPeriodFrom = 1 Then
                    ' 前期
                    strSql = strSql & "       ,dtl.c_before_reference" & vbCrLf         ' 04. 前期参照権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_before_reg" & vbCrLf               ' 05. 前期登録権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_before_print" & vbCrLf             ' 06. 前期印刷権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_before_output_file" & vbCrLf       ' 07. 前期ファイル出力権限(可=1/不可=0)
                Else
                    ' 前々期以前
                    strSql = strSql & "       ,dtl.c_two_before_reference" & vbCrLf     ' 04. 旧期(前々期以前)の参照権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_two_before_reg" & vbCrLf           ' 05. 旧期(前々期以前)の登録権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_two_before_print" & vbCrLf         ' 06. 旧期(前々期以前)の印刷権限(可=1/不可=0)
                    strSql = strSql & "       ,dtl.c_two_before_output_file" & vbCrLf   ' 07. 旧期(前々期以前)のファイル出力権限(可=1/不可=0)
                End If
                strSql = strSql & "   FROM committee_screen_dtl AS dtl" & vbCrLf        ' 委員会画面詳細テーブル
                ' 全メニューID取得（全メニュー）
                strSql = strSql & "       ,( SELECT DISTINCT a.c_menu_id" & vbCrLf                          ' メニューID
                strSql = strSql & "            FROM menucontrol As a" & vbCrLf                              ' メニューコントロールマスタ
                strSql = strSql & "           WHERE a.c_control = '" & iMenuId & "' ) AS cot" & vbCrLf      ' メニューIDと同じもの
                strSql = strSql & "  WHERE dtl.c_committee_id = '" & MDLoginInfo.CommitteeId & "'" & vbCrLf ' ログイン時の所属委員会IDと同じもの
                strSql = strSql & "    AND dtl.s_committee_seq = '" & MDLoginInfo.PostId & "'" & vbCrLf     ' ログイン時の委員会ID枝番（枝番ID）
                strSql = strSql & "    AND dtl.c_menu_id = cot.c_menu_id" & vbCrLf                          ' 委員会画面詳細とメニューコントロールマスタをメニューIDで結合
                strSql = strSql & ";" & vbCrLf
                clsDb.Connect()                     ' データベース接続
                log.Debug(strSql)                   ' ログ出力（SQL）
                dtRet = clsDb.ExecuteSql(strSql)    ' SQL実行
                If dtRet.Rows.Count < 1 Then        ' データ件数チェック
                    Call MessageBox.Show("権限データはありません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    Return Nothing
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                clsDb.Disconnect()                  ' データベース切断
            End Try
            getGrant = dtRet                        ' 処理結果に正常を設定
        End Function
#End Region

#Region "組合長名の取得"
        '***************************************************************************************************
        '   ＩＤ　：GetUnionLeader
        '   名称　：組合長名の取得
        '   概要　：組合長名を取得し返却します
        '   引数　：なし
        '   戻り値：GetUnionLeaderName As String = 組合長名
        '   作成日：2012/01/11(水)  a.onuma
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/11(水)  a.onuma  新規作成
        '***************************************************************************************************
        Public Function GetUnionLeaderName() As String
            Dim strLeaderName As String = String.Empty
            Dim clsDb As CLAccessMdb = New CLAccessMdb
            Dim strSql As String = String.Empty
            Dim dtRet As DataTable = Nothing
            '現在日付をスラッシュを除いた形で取得
            Dim strDateNow As String = Now.ToString("yyyyMMdd")
            Try
                '最新の中執委員会名簿から委員長名を取得
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT attr1.l_name " & vbCrLf
                strSql = strSql & "   FROM staf_attribute AS attr1, " & vbCrLf
                strSql = strSql & "        ( SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
                strSql = strSql & "            FROM staf_attribute " & vbCrLf
                strSql = strSql & "           WHERE d_from <= '" & strDateNow & "' " & vbCrLf '現在日以前の最新のユーザー情報
                strSql = strSql & "           GROUP BY c_user_id, c_ksh, c_staf_id ) AS attr2 " & vbCrLf
                strSql = strSql & "  WHERE attr1.c_user_id = ( SELECT com_list_dtl.c_user_id " & vbCrLf
                strSql = strSql & "                              FROM committee_list AS t7, " & vbCrLf
                strSql = strSql & "                                   committee_list_dtl AS com_list_dtl, " & vbCrLf
                strSql = strSql & "                                   ( SELECT c_committee_id,c_period_id, MAX(d_from) AS now_from " & vbCrLf
                strSql = strSql & "         	                          FROM committee_list " & vbCrLf
                strSql = strSql & "                                      WHERE d_from <= '" & strDateNow & "'  " & vbCrLf '最新の委員会名簿
                strSql = strSql & "         	                           AND c_committee_id = '001' " & vbCrLf '中央執行委員会
                strSql = strSql & "                                      GROUP BY c_committee_id,c_period_id ) AS t8 " & vbCrLf
                strSql = strSql & "                             WHERE t7.c_committee_id = t8.c_committee_id " & vbCrLf
                strSql = strSql & "                               AND t7.d_from = t8.now_from " & vbCrLf
                strSql = strSql & "                               AND t7.c_committee_list = com_list_dtl.c_committee_list " & vbCrLf
                strSql = strSql & "                               AND t7.c_period_id      = '" & MDLoginInfo.PeriodId & "' " & vbCrLf
                strSql = strSql & "                               AND t8.c_period_id      = '" & MDLoginInfo.PeriodId & "' " & vbCrLf
                strSql = strSql & "                               AND com_list_dtl.c_committee_id = '001' " & vbCrLf  '中央執行委員会
                strSql = strSql & "                               AND com_list_dtl.s_committee_seq = '1')  " & vbCrLf '委員長
                strSql = strSql & "    AND attr1.c_user_id = attr2.c_user_id  " & vbCrLf
                strSql = strSql & "    AND attr1.c_ksh = attr2.c_ksh " & vbCrLf
                strSql = strSql & "    AND attr1.d_from = attr2.now_from "
                strSql = strSql & ";"
                clsDb.Connect()                 ' DB接続開始
                dtRet = clsDb.ExecuteSql(strSql)
                If dtRet.Rows.Count > 0 Then
                    strLeaderName = dtRet.Rows(0).Item("l_name")
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodBase.GetCurrentMethod.Name())
            Finally
                clsDb.Disconnect()              ' DB接続終了
            End Try
            Return strLeaderName
        End Function
#End Region

#Region "更新回数をアルファベットで取得"
        '***************************************************************************************************
        '   ＩＤ　：GetRevision
        '   名称　：更新回数アルファベット取得処理
        '   概要　：更新回数をアルファベットに変換し返却します
        '   引数　：ByVal intUpdateCount As Integer = 更新回数
        '   戻り値：strUpdateCount       As String  = アルファベット変換後の更新回数
        '   作成日：2012/01/06(金)  a.onuma
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/06(金)  a.onuma  新規作成
        '***************************************************************************************************
        ''' <summary>更新回数アルファベット取得処理</summary>
        ''' <param name="intUpdateCount">更新回数</param>
        ''' <returns>アルファベット変換後の更新回数</returns>
        ''' <remarks></remarks>
        Public Function GetRevision(ByVal intUpdateCount As Integer) As String
            Dim intQuotient As Integer = 0
            Dim intRest As Integer = 0
            Dim strUpdateCount As String = String.Empty
            Try
                ' nを26で割った商を求める（小数点は切り捨てる）
                intQuotient = System.Math.Floor(intUpdateCount / 26)
                ' nの26での余りを求める
                intRest = (intUpdateCount Mod 26)
                If intUpdateCount < 26 Then '0～25の間はA～Zで表示
                    strUpdateCount = String.Format("{0:c}", Chr(&H41 + intRest))
                Else
                    '26以降は○A、○B…となるように表示
                    strUpdateCount = String.Format("{0:c}{1:c}", Chr(&H40 + intQuotient), Chr(&H41 + intRest))
                End If
                ' 表示加工（"・・" + アルファベット + "・・"）
                strUpdateCount = "・・" & strUpdateCount & "・・"
            Catch ex As Exception
                log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            Return strUpdateCount
        End Function
#End Region

#Region "管理部委員会IDリスト取得処理"
        '***************************************************************************************************
        '   ＩＤ　：getDepartmentCommitteeIdList
        '   名称　：管理部委員会IDリスト取処理
        '   概要　：
        '   引数　：ByVal clsMdb         As CLAccessMdb = データベースクラス
        '           ByVal strCommitteeId As String      = 委員会ID
        '   戻り値：True = 正常, False = 異常
        '   作成日：2012/01/12(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/12(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>管理部委員会IDリスト取得処理</summary>
        ''' <param name="clsMdb">データベースクラス</param>
        ''' <param name="strCommitteeId">委員会ID</param>
        ''' <returns>True = 正常, False = 異常</returns>
        ''' <remarks></remarks>
        Public Function getDepartmentCommitteeIdList(ByVal clsMdb As CLAccessMdb, _
                                                     ByVal strCommitteeId As String) As Boolean
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)          ' ログ出力（処理開始）
            Dim blnRet As Boolean = False                                                           ' 処理結果
            Dim strSql As String = ""                                                               ' SQL文
            Dim intCntRet As Integer = 0                                                            ' 処理結果件数
            Dim tbRet As DataTable = Nothing                                                        ' 処理結果格納データテーブル
            Dim strNow As String = Now.ToString("yyyyMMdd")                                         ' 現在日付
            Try
                ' SQL作成
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT a.c_committee_id" & vbCrLf
                strSql = strSql & "   FROM department_committee AS a" & vbCrLf
                strSql = strSql & "  WHERE c_department_id  = '" & strCommitteeId & "'" & vbCrLf
                strSql = strSql & "    AND d_from <= '" & strNow & "'" & vbCrLf
                strSql = strSql & "    AND d_to >= '" & strNow & "'" & vbCrLf
                strSql = strSql & ";" & vbCrLf
                ' SQL実行
                tbRet = clsMdb.ExecuteSql(strSql)                                                   ' SQL実行
                intCntRet = tbRet.Rows.Count                                                        ' 処理結果件数取得
                If intCntRet <> 0 Then                                                              ' 0件チェック
                    For i = 0 To intCntRet - 1
                        MDLoginInfo.CommitteeIdList.Add(tbRet.Rows(i).Item(0).ToString)             ' 委員会ID取得
                    Next
                End If
                blnRet = True
            Catch ex As Exception
                log.Fatal(ex.Message)                                                               ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)            ' ログ出力（処理終了）
            Return blnRet                                                                           ' 戻り値設定
        End Function
#End Region

#Region "管理部権限取得処理"
        '***************************************************************************************************
        '   ＩＤ　：getGrantDepartment
        '   名称　：管理部権限取得処理
        '   概要　：
        '   引数　：ByVal strCommitteeId As String = 委員会ID
        '   戻り値：True = 権限有り, False = 権限無し
        '   作成日：2012/01/12(木)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/12(木)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>管理部権限取得処理</summary>
        ''' <param name="strCommitteeId">委員会ID</param>
        ''' <returns>True = 権限有り, False = 権限無し</returns>
        ''' <remarks></remarks>
        Public Function getGrantDepartment(ByVal strCommitteeId As String) As Boolean
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
            Dim blnRet As Boolean = False                                                       ' 処理結果
            Try
                If MDLoginInfo.CommitteeId.Substring(0, 1).Equals("M") Then                     ' ログイン委員会IDが "M" から始まっているかチェック
                    If MDLoginInfo.CommitteeIdList.Count > 0 Then                               ' ログイン情報の管理部用委員会IDリストがある場合
                        If MDLoginInfo.CommitteeIdList.Contains(strCommitteeId) Then            ' ログイン情報の管理部用委員会IDリストにあるかチェック
                            blnRet = True
                        End If
                    End If
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
            Return blnRet                                                                       ' 戻り値設定
        End Function
#End Region

#Region "取締役社長名称取得処理"
        '***************************************************************************************************
        '   ＩＤ　：GetPresidentName
        '   名称　：取締役社長名取得処理
        '   概要  ：
        '   引数　：Optional ByVal iClsDb As CLAccessMdb = データベースクラス
        '   戻り値：取締役社長名
        '   作成日：2012/01/24(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/01/24(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>取締役社長名取得処理</summary>
        ''' <param name="iClsDb">データベースクラス</param>
        ''' <returns>取締役社長名</returns>
        ''' <remarks></remarks>
        Public Function GetPresidentName(Optional ByVal iClsDb As CLAccessMdb = Nothing) As String
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
            Dim strRet As String = ""                                                       ' 処理結果
            Dim intRet As Integer = 0                                                       ' 処理件数
            Dim strSql As String = ""                                                       ' SQL
            Dim dtRet As DataTable = Nothing                                                ' 処理結果データテーブル
            Dim clsDb As New CLAccessMdb                                                    ' データベースクラス
            Dim strNow As String = Now.ToString("yyyyMMdd")                                         ' 現在日付
            Try
                ' SQL作成
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT l_name AS PresidentName" & vbCrLf
                strSql = strSql & "   FROM constant_dtl" & vbCrLf
                strSql = strSql & "  WHERE c_constant = '" & CONSTANT_ID_OFFICER_NAME & "'" & vbCrLf
                strSql = strSql & "    AND d_from <= '" & strNow & "'" & vbCrLf
                strSql = strSql & "    AND d_to >= '" & strNow & "'" & vbCrLf
                strSql = strSql & "  ORDER BY d_from DESC" & vbCrLf 'chk
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
                If intRet > 0 Then
                    ' データ0件以上
                    strRet = dtRet.Rows(0).Item(0).ToString()
                Else
                    Call MessageBox.Show("取締役社長名が取得できません！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                End If
            Catch ex As Exception
                log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Finally
                If iClsDb Is Nothing Then
                    Call clsDb.Disconnect()                                                 ' データベース切断
                End If
            End Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' ログ出力（処理終了）
            Return strRet                                                                   ' 戻り値設定
        End Function
#End Region

#Region "日付文字列指定文字削除処理"
        '***************************************************************************************************
        '   ＩＤ　：ReplaceDate
        '   名称　：日付文字列指定文字削除処理
        '   概要  ：日付文字列（2012年02月28日等）から "年" "月" "日" を削除して日付文字列を返す。 
        '           空白削除フラグがTrueの場合、全角・半角スペースの空白を削除する。
        '   引数　：Optional ByVal iBlnTrimFlg As Boolean = False
        '   戻り値：指定文字列を削除した日付文字列
        '   作成日：2012/02/28(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/28(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>日付文字列指定文字削除処理</summary>
        ''' <param name="iBlnSpaceDelFlg">空白削除フラグ</param>
        ''' <returns>指定文字列を削除した日付文字列</returns>
        ''' <remarks></remarks>
        Public Function ReplaceDate(ByVal iStr As String, _
                                    Optional ByVal iBlnSpaceDelFlg As Boolean = True) As String
            Dim strRet As String = ""       ' 処理結果
            Try
                ' 文字列から "時" "分" "秒" の文字列削除
                strRet = iStr.Replace("年", "").Replace("月", "").Replace("日", "")
                ' 空白削除フラグが True の場合、両端（先頭・最後）の空白削除
                If iBlnSpaceDelFlg Then
                    strRet = strRet.Replace(" ", "").Replace("　", "")
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            ' 戻り値設定
            Return strRet
        End Function
#End Region

#Region "時間文字列指定文字削除処理"
        '***************************************************************************************************
        '   ＩＤ　：ReplaceTime
        '   名称　：時間文字列指定文字削除処理
        '   概要  ：時間文字列（13時30分40秒等）から "時" "分" "秒" を削除して時間文字列を返す。 
        '           空白削除フラグがTrueの場合、全角・半角スペースの空白を削除する。
        '   引数　：Optional ByVal iBlnTrimFlg As Boolean = True
        '   戻り値：指定文字列を削除した時間文字列
        '   作成日：2012/02/28(火)  m.suzuki
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/02/28(火)  m.suzuki  新規作成
        '***************************************************************************************************
        ''' <summary>時間文字列指定文字削除処理</summary>
        ''' <param name="iBlnSpaceDelFlg">空白削除フラグ</param>
        ''' <returns>指定文字列を削除した時間文字列</returns>
        ''' <remarks></remarks>
        Public Function ReplaceTime(ByVal iStr As String, _
                                    Optional ByVal iBlnSpaceDelFlg As Boolean = True) As String
            Dim strRet As String = ""       ' 処理結果
            Try
                ' 文字列から "時" "分" "秒" の文字列削除
                strRet = iStr.Replace("時", "").Replace("分", "").Replace("秒", "")
                ' 空白削除フラグが True の場合、両端（先頭・最後）の空白削除
                If iBlnSpaceDelFlg Then
                    strRet = strRet.Replace(" ", "").Replace("　", "")
                End If
            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, _
                                       SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
            End Try
            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            ' 戻り値設定
            Return strRet
        End Function
#End Region

#Region "レプリカ同期処理"
        '***************************************************************************************************
        '   ＩＤ　：syncMdb
        '   名称　：レプリカ同期処理
        '   概要  ：
        '   引数　：ByVal syncType As Integer 同期タイプ
        '            1:JRO.SyncTypeEnum.jrSyncTypeExport
        '            2:JRO.SyncTypeEnum.jrSyncTypeImport
        '            3:JRO.SyncTypeEnum.jrSyncTypeImpExp
        '         ：ByVal blnMsg AS Boolean　メッセージ表示有無
        '   戻り値：
        '   作成日：2012/08/14(火)  Fujisaku
        '   更新日：
        '---------------------------------------------------------------------------------------------------
        '   履歴　：2012/08/14(火)  Fujisaku  新規作成
        '***************************************************************************************************
        '        Public Function syncMdb(ByVal syncType As Integer, ByVal blnMsg As Boolean) As Boolean
        '            Dim blnRtn As Boolean = False
        '            Dim repDb As New JRO.Replica
        '            Dim con1 As New ADODB.Connection
        '            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        '            Try
        '                ' デザインマスタ情報が未設定の場合は同期処理をスキップ(開発用)
        '                If MDSystemInfo.AccessMstPath & MDSystemInfo.AccessMstName = "" Then
        '                    Return True
        '                End If

        '                ' カーソルを砂時計に設定して通信中ウィンドウを表示
        '                If blnMsg Then
        '                    Call FrmWaitSync.ShowWaitForm(Nothing)
        '                    Cursor.Current = Cursors.WaitCursor
        '                End If

        '                ' ADODBコネクションの作成
        '                Dim iCnt As Integer = 0
        '                con1.ConnectionString = "Provider=" & MDSystemInfo.AccessProvider & _
        '                                        ";Data Source=" & MDSystemInfo.AccessPath & MDSystemInfo.AccessName
        '                con1.Open()
        '                repDb.ActiveConnection = con1
        '                ' JROを使用して同期実施
        'replicate:
        '                Try
        '                    repDb.Synchronize(MDSystemInfo.AccessMstPath & MDSystemInfo.AccessMstName, _
        '                                      syncType, JRO.SyncModeEnum.jrSyncModeDirect)
        '                    blnRtn = True
        '                Catch exSync As Exception
        '                    ' 最大3回まで2秒のスリープ後再実施を繰り返す
        '                    iCnt += 1
        '                    If iCnt < 3 Then
        '                        System.Threading.Thread.Sleep(2 * 1000)
        '                        GoTo replicate
        '                    Else
        '                        ' メッセージ表示前に通信中ウィンドウ停止
        '                        If blnMsg Then
        '                            Call FrmWaitSync.CloseWaitForm()
        '                            Cursor.Current = Cursors.Default
        '                            CLMsg.Show("GI0100")
        '                            Return blnRtn
        '                        End If
        '                    End If
        '                End Try

        '            Catch ex As Exception
        '                log.Fatal(ex.Message)
        '                End
        '            Finally
        '                con1.Close()
        '                Call FrmWaitSync.CloseWaitForm()
        '                Cursor.Current = Cursors.Default
        '            End Try
        '            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        '            Return blnRtn
        '        End Function
#End Region

    End Module
End Namespace
#End Region
