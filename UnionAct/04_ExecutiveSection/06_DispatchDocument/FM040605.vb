#Region "FM040605"
'===========================================================================================================
'   クラスＩＤ　　：FM040605
'   クラス名称　　：発信文書コピー編集期選択画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon

Public Class FM040605

#Region "定数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040605          ' FM040605
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040605      ' コピー編集期選択画面
#End Region

#Region "プロパティ"
    Public _strPeriodId As String = ""                              ' 期ID
    Public _strDocCode As String = ""                               ' 管理コード
    Public _strSubject As String = ""                               ' 標題

    ' 期ID
    Public Property strPeriodId() As String
        Get
            Return _strPeriodId
        End Get
        Set(ByVal value As String)
            _strPeriodId = value
        End Set
    End Property

    ' 管理コード
    Public Property strDocCode() As String
        Get
            Return _strDocCode
        End Get
        Set(ByVal value As String)
            _strDocCode = value
        End Set
    End Property

    ' 標題
    Public Property strSubject() As String
        Get
            Return _strSubject
        End Get
        Set(ByVal value As String)
            _strSubject = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM040605_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM040605_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If MDCommon.SetFormCenter(Me) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If Me.GetData() = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnEdit_Click
    '   名称　：編集ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Try
            ' 入力チェック処理
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            ' 各データ設定
            If Me.SetDate() = False Then

            End If

            Me.DialogResult = Windows.Forms.DialogResult.OK         ' ダイアログ結果（1：編集）

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' 各データ取得
            Me.DialogResult = Windows.Forms.DialogResult.Cancel ' ダイアログ結果（2：キャンセル）

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            ' Label（必須）
            Me.lblIndispensablePeriod.Visible = True        ' 期
            Me.lblIndispensablePeriod.Enabled = True
            Me.lblIndispensableDocCode.Visible = True       ' 管理コード
            Me.lblIndispensableDocCode.Enabled = True
            Me.lblIndispensableSubject.Visible = True       ' 標題
            Me.lblIndispensableSubject.Enabled = True

            ' Label
            Me.lblPeriod.Visible = True                     ' 期
            Me.lblPeriod.Enabled = True
            Me.lblDocCode.Visible = True                    ' 管理コード
            Me.lblDocCode.Enabled = True
            Me.lblSubject.Visible = True                    ' 標題
            Me.lblSubject.Enabled = True

            ' ComboBox
            Me.cboPeriod.Visible = True                     ' 期
            Me.cboPeriod.Enabled = True
            Me.cboPeriod.DataSource = Nothing
            Me.cboPeriod.Text = ""

            ' TextBox
            Me.txtDocCode.Visible = True                    ' 管理コード
            Me.txtDocCode.Enabled = True
            Me.txtDocCode.Text = ""
            Me.txtDocCode.BackColor = Color.LightYellow
            Me.txtSubject.Visible = True                    ' 標題
            Me.txtSubject.Enabled = True
            Me.txtSubject.Text = ""
            Me.txtSubject.BackColor = Color.LightYellow

            ' Button
            Me.btnEdit.Visible = True                       ' 編集
            Me.btnEdit.Enabled = True
            Me.btnCancel.Visible = True                     ' キャンセル
            Me.btnCancel.Enabled = True

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

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス
        Dim strSql As String = ""           ' SQL

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   期コンボボックス作成
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_name      AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.c_period_id AS ValueName" & vbCrLf
            strSql = strSql & "   FROM period AS a" & vbCrLf
            strSql = strSql & "  ORDER BY d_to DESC" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, _
                                          Me.cboPeriod, _
                                          strSql, _
                                          "DisplayName", _
                                          "ValueName", _
                                          False) = False Then
                Return False
            End If

            '-------------------------------------------------------------------------------
            '   管理コード・標題
            '-------------------------------------------------------------------------------
            Me.txtDocCode.Text = Me.strDocCode          ' 管理コード
            Me.txtSubject.Text = Me.strSubject          ' 標題

            ' 処理結果に正常を格納
            blnRet = True

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
            Call clsDb.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            ' 期
            If Me.cboPeriod.SelectedIndex < 0 Then
                Call CLMsg.Show("GE0006", "期")
                Return blnRet
            End If

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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetDate
    '   名称　：各データ設定処理
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetDate() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            '---------------------------------------
            '   各データ取得
            '---------------------------------------
            ' 期
            Me.strPeriodId = Me.cboPeriod.SelectedValue.ToString()
            ' 管理コード
            Me.strDocCode = Me.txtDocCode.Text.Trim()
            ' 標題
            Me.strSubject = Me.txtSubject.Text.Trim()

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

End Class

#End Region
