# JUN-UnionAct

全日空乗員組合システム（ACAOA）です。

## 開発・ビルド環境

- Visual Studio 2022
- .NET Framework 4.8
- Microsoft Visual Studio Installer Projects
- Crystal Reportsランタイム
- ビルドプラットフォームは、Release・Staging・Setupのすべてで`x86`を使用します。

64bit版Windowsでもx86アプリケーションとして動作します。外部コンポーネントとの互換性を保つため、`Any CPU`や`x64`には変更しないでください。

## 設定ファイル

環境ごとの設定は、ビルド時に`app.config`へ変換適用されます。

| 環境 | 変換ファイル | 生成される設定ファイル |
| --- | --- | --- |
| Release | `UnionAct/app.Release.config` | `UnionAct/bin/x86/Release/UnionAct.exe.config` |
| Staging | `UnionAct/app.Staging.config` | `UnionAct/bin/x86/Staging/UnionAct.exe.config` |

アプリケーションが実行時に読み込むのは、実行ファイルと同じフォルダーにある`UnionAct.exe.config`です。`app.Release.config`や`app.Staging.config`を実行時に直接読み込むわけではありません。

### Setupプロジェクトへの収録設定

Visual Studioで各Setupプロジェクトの「ファイル システム」を開き、「Application Folder」に次のファイルを明示的に追加します。

- `Setup`：`UnionAct/bin/x86/Release/UnionAct.exe.config`
- `SetupStaging`：`UnionAct/bin/x86/Staging/UnionAct.exe.config`

「プライマリ出力 from UnionAct」の`ExcludeFilter`には`UnionAct.exe.config`を設定します。これにより、変換前の`app.config`が自動収録されることを防ぎます。

## Release・Stagingの一括ビルド

リポジトリ直下の`Build-All.cmd`をダブルクリックします。

スクリプトはVisual Studio 2022を自動検出し、次の順番でリビルドします。

1. `Setup`を`Release | x86`でリビルド
2. `SetupStaging`を`Staging | x86`でリビルド
3. 各環境の`connectionString`と`SequencePath`を検査
4. 各MSIが生成されたことを検査

PowerShellから直接実行する場合は、リポジトリ直下で次を実行します。

```powershell
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -File .\Build-All.ps1
```

生成物は次の場所に出力されます。

```text
Setup\Release\Setup.msi
Setup\Staging\Setup.msi
```

ビルドの最後に次のメッセージが表示されれば成功です。

```text
Release and Staging installers were created successfully.
```

## 配布バージョンの更新

既にインストールされている環境を、利用者による手動アンインストールなしで更新する場合は、ビルド前に`Setup`と`SetupStaging`の両方で次の操作を行います。

1. Setupプロジェクトを選択して、プロパティを表示する。
2. `Version`を上げる（例：`1.0.11`から`1.0.12`）。
3. ProductCodeを変更するか確認されたら「はい」を選択する。
4. `UpgradeCode`は変更しない。
5. `RemovePreviousVersions = True`になっていることを確認する。
6. `DetectNewerInstalledVersion = True`になっていることを確認する。
7. `Build-All.cmd`を実行する。

新しいSetupを実行すると、旧版が自動的に置き換えられます。同じバージョン番号のままMSIを再作成すると、Windows Installerが既存ファイルを残す場合があるため、通常の配布では必ずバージョンを上げてください。

## インストール後の確認

Release版では、次のファイルを確認します。

```text
C:\ACA\ACAOA\UnionAct.exe.config
```

以下が設定されていることを確認してください。

- `connectionStrings`内の`UnionActDb`に`connectionString`があること
- `UnionActDb`に`providerName`があること
- `appSettings`内の`SequencePath`が本番環境用になっていること

接続文字列などの具体的な値はREADMEやログへ記載しないでください。

## ビルド時の警告

既存コードのXMLコメント、Crystal Reports依存関係、古いAPIなどに関する警告が表示される場合があります。ビルド結果が「0失敗」で、最後の生成物検査が成功していることを確認してください。
