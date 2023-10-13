Import-Module dw
Import-Module PSPolly

docker stop mssql
docker rm mssql
docker volume rm pdsi_mssql_data
docker-compose up -d

# Check if SQL Server is ready
Write-Host "Waiting for SQL Server to be ready..." -ForegroundColor Yellow

$Policy = New-PollyPolicy -Retry -RetryCount 15 -SleepDuration { New-TimeSpan -Seconds 2 }
Invoke-PollyCommand -Policy $Policy -ScriptBlock {
	if (Test-SqlConnection -ServerName 'localhost' -DatabaseName 'master' -UserName 'SA' -Password 'Strong@Passw0rd') {
		Write-Host "Restoring Databases..." -ForegroundColor Yellow
		SQLCMD.EXE -S localhost -U SA -P "Strong@Passw0rd" -i .\databases\RestoreDatabase.sql
	} else {
		throw "SQL Server is not ready"
	}
}

Write-Host "Ready to work..." -ForegroundColor Green
