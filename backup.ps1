$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupDir = "C:\Backups\core_firstDB"
if (-not (Test-Path $backupDir)) { New-Item -ItemType Directory -Path $backupDir -Force }
$backupFile = "$backupDir\core_firstDB_$timestamp.sql"
$env:PGPASSWORD = "nasser@1426"
$pgDumpPath = "C:\Program Files\PostgreSQL\18\bin\pg_dump.exe"
& $pgDumpPath -U postgres -h localhost core_firstDB > $backupFile
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Backup created: $backupFile" -ForegroundColor Green
} else {
    Write-Host "❌ Backup failed! Check PostgreSQL connection or password." -ForegroundColor Red
}
