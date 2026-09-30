[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

$repositoryRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionPath = Join-Path $repositoryRoot 'UnionAct.sln'
$vswherePath = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio\Installer\vswhere.exe'

if (-not (Test-Path -LiteralPath $solutionPath)) {
    throw "Solution not found: $solutionPath"
}

if (-not (Test-Path -LiteralPath $vswherePath)) {
    throw "Visual Studio Installer was not found: $vswherePath"
}

$devenvPath = & $vswherePath `
    -version '[17.0,18.0)' `
    -latest `
    -products '*' `
    -requires Microsoft.Component.MSBuild `
    -find 'Common7\IDE\devenv.com' |
    Select-Object -First 1

if ([string]::IsNullOrWhiteSpace($devenvPath) -or -not (Test-Path -LiteralPath $devenvPath)) {
    throw 'Visual Studio 2022 devenv.com was not found. Install Visual Studio 2022 with MSBuild and Visual Studio Installer Projects.'
}

function Invoke-SolutionBuild {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Configuration,

        [Parameter(Mandatory = $true)]
        [string]$Project
    )

    Write-Host ""
    Write-Host "=== Rebuilding $Project ($Configuration) ===" -ForegroundColor Cyan

    & $devenvPath $solutionPath /Rebuild $Configuration /Project $Project
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed: $Project ($Configuration), exit code $LASTEXITCODE"
    }
}

function Assert-DeploymentOutput {
    param(
        [Parameter(Mandatory = $true)]
        [string]$EnvironmentName,

        [Parameter(Mandatory = $true)]
        [string]$ConfigPath,

        [Parameter(Mandatory = $true)]
        [string]$MsiPath
    )

    if (-not (Test-Path -LiteralPath $ConfigPath)) {
        throw "$EnvironmentName config was not created: $ConfigPath"
    }

    [xml]$config = Get-Content -LiteralPath $ConfigPath -Raw
    $dbSetting = $config.configuration.connectionStrings.add |
        Where-Object { $_.name -eq 'UnionActDb' } |
        Select-Object -First 1
    $sequenceSetting = $config.configuration.appSettings.add |
        Where-Object { $_.key -eq 'SequencePath' } |
        Select-Object -First 1

    if ($null -eq $dbSetting -or [string]::IsNullOrWhiteSpace($dbSetting.connectionString)) {
        throw "$EnvironmentName config has no UnionActDb connectionString: $ConfigPath"
    }

    if ($null -eq $sequenceSetting -or [string]::IsNullOrWhiteSpace($sequenceSetting.value)) {
        throw "$EnvironmentName config has no SequencePath: $ConfigPath"
    }

    if (-not (Test-Path -LiteralPath $MsiPath)) {
        throw "$EnvironmentName installer was not created: $MsiPath"
    }

    $msi = Get-Item -LiteralPath $MsiPath
    Write-Host "$EnvironmentName OK: $($msi.FullName) ($($msi.LastWriteTime))" -ForegroundColor Green
}

try {
    Invoke-SolutionBuild -Configuration 'Release|x86' -Project 'Setup'
    Invoke-SolutionBuild -Configuration 'Staging|x86' -Project 'SetupStaging'

    Write-Host ""
    Write-Host '=== Validating outputs ===' -ForegroundColor Cyan

    Assert-DeploymentOutput `
        -EnvironmentName 'Release' `
        -ConfigPath (Join-Path $repositoryRoot 'UnionAct\bin\x86\Release\UnionAct.exe.config') `
        -MsiPath (Join-Path $repositoryRoot 'Setup\Release\Setup.msi')

    Assert-DeploymentOutput `
        -EnvironmentName 'Staging' `
        -ConfigPath (Join-Path $repositoryRoot 'UnionAct\bin\x86\Staging\UnionAct.exe.config') `
        -MsiPath (Join-Path $repositoryRoot 'Setup\Staging\Setup.msi')

    Write-Host ""
    Write-Host 'Release and Staging installers were created successfully.' -ForegroundColor Green
}
catch {
    Write-Host ""
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
