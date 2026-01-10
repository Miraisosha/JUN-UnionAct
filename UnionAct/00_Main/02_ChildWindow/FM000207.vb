#Region "FM000207"
'===========================================================================================================
'   クラスＩＤ　　：FM000207
'   クラス名称　　：データベースメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon

Public Class FM000207

#Region " 定数 "
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000207                                ' FM000207
    Private Const SCREEN_NAME = SCREEN_NAME_FM000207                            ' データベースメンテナンス画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000207_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub FM000207_Load( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles MyBase.Load

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面中央表示処理
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If

            ' デフォルトボタン無効
            Me.btnComp.Enabled = False          ' PC上修復・最適化ボタン無効
            Me.btnSubmit.Enabled = False        ' PC上調査提出ボタン無効
            Me.btnRenew.Enabled = False         ' PC上破棄・再取得ボタン無効
            Me.btnCompServer.Enabled = False    ' サーバー上修復・最適化ボタン無効

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSync_Click
    '   名称　：情報更新ボタンクリック処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnSync_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSync.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 同期確認メッセージボックス表示
            If CLMsg.Show("GQ0100") = vbYes Then
                ' 「はい」押下時、レプリカ同期処理
                syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True)
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnComp_Click
    '   名称　：修復・最適化ボタンクリック処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnComp_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnComp.Click

        Dim blnComp As Boolean
        Dim nameMdb As String
        Dim nameComp As String
        Dim nameBkup As String
        Dim pathOld As String
        Dim pathNew As String
        Dim pathDir As String

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 確認メッセージボックス表示
            If CLMsg.Show("GQ0101") = vbYes Then

                ' 「はい」押下時

                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' パスの確定
                pathDir = MDSystemInfo.AccessPath
                nameMdb = MDSystemInfo.AccessName
                nameComp = "Comp_" & MDSystemInfo.AccessName
                nameBkup = "Bkup_" & MDSystemInfo.AccessName
                pathOld = "Provider=" & MDSystemInfo.AccessProvider & ";Data Source=" & pathDir & nameComp
                pathNew = "Provider=" & MDSystemInfo.AccessProvider & ";Data Source=" & pathDir & nameMdb

                ' 元DBを退避
                System.IO.File.Delete(pathDir & nameComp)
                System.IO.File.Move(pathDir & nameMdb, pathDir & nameComp)

                Try
                    ' 退避DBを最適化
                    Dim jroJE As New JRO.JetEngine
                    jroJE.CompactDatabase(pathOld, pathNew)
                    blnComp = True
                Catch exCom As System.Runtime.InteropServices.COMException
                    blnComp = False
                End Try

                If blnComp Then
                    ' バックアップを削除
                    System.IO.File.Delete(pathDir & nameBkup)

                    ' 退避DBをバックアップ
                    System.IO.File.Move(pathDir & nameComp, pathDir & nameBkup)

                    ' 完了メッセージ表示
                    Call CLMsg.Show("BI0100")
                Else
                    ' 退避DBを元に戻す
                    System.IO.File.Move(pathDir & nameComp, pathDir & nameMdb)

                    ' エラーメッセージ表示
                    Call CLMsg.Show("BE0100")
                End If

                ' 終了時にメンテナンスチェックを解除して終わる
                Me.chkMaint1.CheckState = CheckState.Unchecked
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSubmit_Click
    '   名称　：調査提出ボタンクリック処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnSubmit_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnSubmit.Click

        Dim pathDir As String
        Dim nameMdb As String
        Dim nameBkup As String
        Dim servDir As String
        Dim blnDir As Boolean = True
        Dim intDir As Integer = 1

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 確認メッセージボックス表示
            If CLMsg.Show("GQ0102") = vbYes Then

                ' 「はい」押下時

                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' パスの確定
                pathDir = MDSystemInfo.AccessPath
                nameMdb = MDSystemInfo.AccessName
                nameBkup = "Bkup_" & MDSystemInfo.AccessName
                servDir = MDSystemInfo.AccessMstPath & "Replica\" & DateTime.Now.ToString("yyyyMMdd") & "_01\"

                ' 退避先ディレクトリ作成
                While blnDir
                    If Not System.IO.Directory.Exists(servDir) Then
                        System.IO.Directory.CreateDirectory(servDir)
                        blnDir = False
                    Else
                        intDir = intDir + 1
                        servDir = servDir.Substring(0, servDir.Length - 4) & "_" & intDir.ToString.PadLeft(2, "0"c) & "\"
                    End If
                End While

                ' ローカルMDBコピー
                System.IO.File.Copy(pathDir & nameMdb, servDir & nameMdb)

                ' 退避バックアップMDBあればコピー
                If System.IO.File.Exists(pathDir & nameBkup) Then
                    System.IO.File.Copy(pathDir & nameBkup, servDir & nameBkup)
                End If

                ' 完了メッセージ表示
                Call CLMsg.Show("BI0100")

                ' 終了時にメンテナンスチェックを解除して終わる
                Me.chkMaint1.CheckState = CheckState.Unchecked
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnRenew_Click
    '   名称　：破棄・再取得ボタンクリック処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnRenew_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnRenew.Click

        Dim pathDir As String
        Dim nameMdb As String
        Dim nameBkup As String
        Dim servDir As String

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 確認メッセージボックス表示
            If CLMsg.Show("GQ0103") = vbYes Then

                ' 「はい」押下時

                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' パスの確定
                pathDir = MDSystemInfo.AccessPath
                nameMdb = MDSystemInfo.AccessName
                nameBkup = "Bkup_" & MDSystemInfo.AccessName
                servDir = MDSystemInfo.AccessMstPath & "Replica\"

                ' 削除前をバックアップ
                System.IO.File.Delete(pathDir & nameBkup)
                System.IO.File.Move(pathDir & nameMdb, pathDir & nameBkup)

                ' MDBの破棄・再取得
                System.IO.File.Delete(pathDir & nameMdb)
                System.IO.File.Copy(servDir & nameMdb, pathDir & nameMdb)

                ' 同期して最新情報取得
                syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True)

                ' 完了メッセージ表示
                Call CLMsg.Show("BI0101")

                ' 終了時にメンテナンスチェックを解除して終わる
                Me.chkMaint1.CheckState = CheckState.Unchecked
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnTest_Click
    '   名称　：修復・最適化（サーバー上）ボタンクリック処理（コマンドプロンプト使用）
    '   概要　：
    '   作成日：2017/04/03(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/04/03(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnTest_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles btnCompServer.Click

        Dim strOfficeDir As String = ""         ' Officeインストールフォルダ
        Dim strCmdArg As String = ""            ' コマンドプロンプト文字列
        Dim intExitCode As Integer = 0          ' コマンドプロンプト結果コード
        Dim strDbServer As String = ""          ' サーバー上のMDBファイル（フルパス）
        Dim strDbBack As String = ""            ' サーバー上のバックアップMDBファイル（フルパス）
        Dim strDbWork As String = ""            ' ローカル上の作業MDBファイル（フルパス）
        Dim strDbTemp As String = ""            ' 最適化処理で作成される最適化前のファイル（フルパス）
        Dim result As Boolean = False           ' 処理結果

        ' 確認メッセージボックス表示
        If CLMsg.Show("GQ0105") = Windows.Forms.DialogResult.No Then
            ' 「いいえ」押下時、処理を抜ける
            Exit Sub
        End If

        Try
            ' ログ出力（処理開始）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------------------
            '   ファイル情報取得
            '-------------------------------------------------------------------------------------------
            ' サーバー上のMDBファイル（フルパス）取得
            strDbServer = MDSystemInfo.AccessMstPath & MDSystemInfo.AccessMstName

            ' サーバー上のバックMDBファイル（フルパス）取得
            strDbBack = MDSystemInfo.AccessMstPath & "Compact\Backup_" & Format(Now, "yyyyMMddHHmmss") & "_" & MDSystemInfo.AccessMstName

            ' ローカル上の作業MDBファイル（フルパス）取得
            strDbWork = MDSystemInfo.AccessPath & MDSystemInfo.AccessMstName

            ' 異常ファイルの最適化時に作成される最適化前のファイル（フルパス）取得
            strDbTemp = MDSystemInfo.AccessPath & "のバックアップ  (合計 acaoa 個).mdb"

            '   Compactフォルダ存在チェック（無ければ作成）
            If NSMDFile.DirExists(MDSystemInfo.AccessMstPath & "Compact\") = False Then
                If NSMDFile.DirCreate(MDSystemInfo.AccessMstPath & "Compact\") = False Then
                    Exit Sub
                End If
            End If

            '   コピー処理（サーバー ⇒ サーバー（バックアップ））
            ' サーバー上のMDBファイルをバックアップ
            If NSMDFile.FileCopy(strDbServer, strDbBack, True) = False Then
                Exit Sub
            End If

            '   コピー処理（サーバー ⇒ ローカル（作業用））
            ' サーバー上で最適化作業を行うと時間がかかる為
            If NSMDFile.FileCopy(strDbServer, strDbWork, True) = False Then
                Exit Sub
            End If

            '   Officeインストールフォルダ取得
            If Me.blnGetOfficePath(strOfficeDir) = False Then
                Exit Sub
            End If

            '   コマンドプロンプト文字列設定
            ' "/c" はコマンドプロンプト実行後、閉じるために必要
            ' パスに半角スペースがある為、ダブルクォーテーション区切りにする
            ' コマンドプロンプト文字列　例）/c ""C:\Program Files\Microsoft Office\Office14\MSACCESS.EXE" "C:\ACAOA\MDB\Server\acaoa.mdb" /compact"
            strCmdArg = "/c """"" & strOfficeDir & "MSACCESS.EXE"" """ & strDbWork & """ /compact"""

            '   コマンドプロンプト実行処理（ローカル上の作業用MDBファイルを最適化）
            If Me.blnExecuteCmd(strCmdArg, intExitCode) = False Then
                Exit Sub
            End If

            '   コピー処理（ローカル（最適化後） ⇒ サーバー
            If NSMDFile.FileCopy(strDbWork, strDbServer, True) = False Then
                Exit Sub
            End If

            '   ファイル削除処理
            ' ローカル上にコピーした作業用のMDBファイルを削除
            If NSMDFile.FileDelete(strDbWork) = False Then
                Exit Sub
            End If

            '   ファイル削除処理
            ' 異常ファイルを最適化した際に作成される最適化前のファイルを削除
            Call NSMDFile.FileDelete(strDbTemp)

            ' 処理結果に True を設定
            result = True

            ' 完了メッセージ表示
            Call CLMsg.Show("BI0100")

            ' 終了時にメンテナンスチェックを解除して終わる
            Me.chkMaint1.CheckState = CheckState.Unchecked
        Finally
            ' 処理失敗か判定
            If result = False Then
                '   ファイル削除処理
                ' サーバー上のバックアップMDBファイルを削除
                Call NSMDFile.FileDelete(strDbBack)

                ' ローカルにコピーした作業用のMDBファイルを削除
                Call NSMDFile.FileDelete(strDbWork)

                ' 最適化処理で作成される最適化前のファイルを削除
                Call NSMDFile.FileDelete(strDbTemp)

                ' エラーメッセージ表示
                Call CLMsg.Show("BE0101")
            End If

            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default

            ' ログ出力（処理終了）
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCompServer_Click
    '   名称　：修復・最適化（サーバー上）ボタンクリック処理（JRO API使用）
    '   概要　：
    '   作成日：2017/04/03(月)  m.suzuki
    '   更新日：2017/07/10(月)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/04/03(月)  m.suzuki  新規作成
    '   　　　：2017/07/10(月)  y.fujisaku  JRO側は使用しないよう変更
    '***************************************************************************************************
    Private Sub btnCompServer_Click( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    )

        Dim jroJe As JRO.JetEngine                  ' 最適化オブジェクト
        Dim blnCompactResult As Boolean = False     ' 最適化処理結果
        Dim strDbServer As String = ""              ' サーバーデータベースフルパス
        Dim strDbWork As String = ""                ' ローカルワークデータベースフルパス
        Dim strDbComOld As String = ""              ' 最適化前パラメータ
        Dim strDbComNew As String = ""              ' 最適化後パラメータ

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 確認メッセージボックス表示
            If CLMsg.Show("GQ0105") = vbYes Then

                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' サーバーデータベースフルパス取得
                strDbServer = MDSystemInfo.AccessMstPath & MDSystemInfo.AccessMstName

                ' ローカルデータベースフルパス取得
                strDbWork = MDSystemInfo.AccessPath & "Comp_" & MDSystemInfo.AccessMstName

                ' 最適化前パラメータ
                strDbComOld = "Provider=" & MDSystemInfo.AccessProvider & ";Data Source=" & strDbWork & ";"
                'strDbComOld = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" & strDbWork & ";"

                ' 最適化後パラメータ
                strDbComNew = "Provider=" & MDSystemInfo.AccessProvider & ";Data Source=" & MDSystemInfo.AccessPath & MDSystemInfo.AccessMstName & ";"
                'strDbComNew = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" & MDSystemInfo.AccessPath & MDSystemInfo.AccessMstName & ";"

                '   コピー
                ' サーバー上で作業を行うと時間がかかる為、サーバーデータベースをローカルにコピー
                NSMDFile.FileCopy(strDbServer, strDbWork, True)

                Try
                    ' 最適化処理
                    jroJe = New JRO.JetEngine
                    jroJe.CompactDatabase(strDbComOld, strDbComNew)
                    blnCompactResult = True
                Catch ex As Exception
                    blnCompactResult = False
                Finally
                    jroJe = Nothing
                End Try

                ' 最適化処理判定
                If blnCompactResult Then
                    '   処理成功
                    ' 最適化したローカルデータベースをサーバーデータベースへコピー
                    NSMDFile.FileCopy(MDSystemInfo.AccessPath & MDSystemInfo.AccessMstName, strDbServer, True)

                    ' ローカルの最適化前のファイルを削除
                    NSMDFile.FileDelete(strDbWork)

                    ' ローカルの最適化後のファイルを削除
                    NSMDFile.FileDelete(MDSystemInfo.AccessPath & MDSystemInfo.AccessMstName)

                    ' 完了メッセージ表示
                    Call CLMsg.Show("BI0100")
                Else
                    ' エラーメッセージ表示
                    Call CLMsg.Show("BE0101")
                End If

                ' 終了時にメンテナンスチェックを解除して終わる
                Me.chkMaint1.CheckState = CheckState.Unchecked
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：chkbox1_CheckedChanged
    '   名称　：メンテナンスチェックボックスチェンジ処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub chkbox1_CheckedChanged( _
        ByVal sender As System.Object, _
        ByVal e As System.EventArgs _
    ) Handles chkMaint1.CheckStateChanged

        Try
            ' メンテナンスチェックボックス状態判定
            If Me.chkMaint1.Checked Then
                ' チェック有
                Me.btnComp.Enabled = True           ' PC上修復・最適化ボタン有効
                Me.btnSubmit.Enabled = True         ' PC上調査提出ボタン有効
                Me.btnRenew.Enabled = True          ' PC上破棄・再取得ボタン有効
                Me.btnCompServer.Enabled = True     ' サーバー上修復・最適化ボタン有効
            Else
                ' チェック無
                Me.btnComp.Enabled = False          ' PC上修復・最適化ボタン無効
                Me.btnSubmit.Enabled = False        ' PC上調査提出ボタン無効
                Me.btnRenew.Enabled = False         ' PC上破棄・再取得ボタン無効
                Me.btnCompServer.Enabled = False    ' サーバー上修復・最適化ボタン無効
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：閉じるボタンクリック処理
    '   概要　：
    '   作成日：2013/06/17(月)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2013/06/17(月)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click( _
        ByVal sender As System.Object, _
         ByVal e As System.EventArgs _
    ) Handles btnCancel.Click

        ' 画面閉じる
        Me.Close()

    End Sub
#End Region

#Region " 関数 "
    '***************************************************************************************************
    '   ＩＤ　：blnGetOfficePath
    '   名称　：Officeインストールフォルダ取得処理
    '   概要  ：Officeのインストールフォルダを取得する。
    '   引数　：ByRef oStrOfficeDir As String = Officeインストールフォルダ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2017/04/07(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/04/07(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function blnGetOfficePath( _
        ByRef oStrOfficeDir As String _
    ) As Boolean

        Dim objApp As Object = Nothing      ' Accessオブジェクト
        Dim result As Boolean = False       ' 処理結果

        Try
            ' Accessオブジェクト生成
            objApp = CreateObject("Access.Application")

            ' Officeインストール場所取得
            oStrOfficeDir = objApp.SysCmd(9)
            If Not "\".Equals(oStrOfficeDir.Last()) Then
                oStrOfficeDir = oStrOfficeDir & "\"
            End If

            ' 処理結果に True を設定
            result = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            If Not objApp Is Nothing Then
                ' Accessオブジェクト解放
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objApp)
                objApp = Nothing
            End If
            ' ガベージコレクションのメモリの解放
            System.GC.Collect()
        End Try

        ' 戻り値設定
        Return result

    End Function

    '***************************************************************************************************
    '   ＩＤ　：blnExecuteCmd
    '   名称　：コマンドプロンプト実行処理
    '   概要  ：指定した引数のコマンドプロンプトを実行する。
    '   引数　：ByVal iStrCmdProArg As String = コマンドプロンプト文字列
    '           ByRef oIntExitCode As Integer = コマンドプロンプト結果コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2017/04/07(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/04/07(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function blnExecuteCmd( _
        ByVal iStrCmdProArg As String, _
        ByRef oIntExitCode As Integer _
    ) As Boolean

        Dim result As Boolean = False                   ' 処理結果
        Dim strCmd As String = ""                       ' コマンドプロンプトパス
        Dim intExitCode As Integer = 0                  ' コマンドプロンプト処理結果
        Dim pro As New System.Diagnostics.Process       ' Processオブジェクト作成

        Try
            ' コマンドプロンプトパス取得（cmd.exe）
            strCmd = System.Environment.GetEnvironmentVariable("ComSpec")
            log.Info("FM000207Log：strCmd：" & strCmd)

            ' FileNameプロパティにコマンドプロンプトパスを設定
            pro.StartInfo.FileName = strCmd

            ' 出力を読み取れるようにする
            pro.StartInfo.UseShellExecute = False

            ' ウィンドウを表示しないようにする
            pro.StartInfo.CreateNoWindow = True

            ' コマンドライン設定
            pro.StartInfo.Arguments = iStrCmdProArg
            log.Info("FM000207Log：iStrCmdProArg：" & iStrCmdProArg)

            ' コマンドライン実行
            pro.Start()

            ' プロセス終了待機（親プロセス、子プロセスでブロック防止のため）
            pro.WaitForExit()

            ' 結果コード取得
            oIntExitCode = pro.ExitCode
            log.Info("FM000207Log：oIntExitCode：" & oIntExitCode)

            ' 処理結果に True を設定
            result = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' Processオブジェクト解放
            If Not pro Is Nothing Then
                pro.Close()
            End If
        End Try

        ' 戻り値設定
        Return result

    End Function
#End Region

End Class
#End Region
