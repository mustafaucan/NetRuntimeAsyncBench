# Windows / PowerShell sürümü.
# Kullanım:  .\run-all.ps1                    -> tüm benchmark'lar
#            .\run-all.ps1 '*ProductPage*'    -> sadece ürün sayfası
param([string]$Filter = '*')
$ErrorActionPreference = 'Stop'
Set-Location $PSScriptRoot
foreach ($p in 'Net10', 'Net11Off', 'Net11On') {
    Write-Host "================ $p ================"
    dotnet run -c Release --project $p -- --filter $Filter --artifacts "results/$p"
    if ($LASTEXITCODE -ne 0) { throw "$p başarısız oldu" }
}
Write-Host ""
Write-Host "Bitti. Tablolar: results/*/results/*-report-github.md"
