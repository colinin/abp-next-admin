# COMMON PATHS 

$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
$batchCommandPaths = @(
        "../aspnet-core/services/start-auth-server.bat",
		"../aspnet-core/services/start-identity-server.bat",
		"../aspnet-core/services/start-backend-admin.bat",
		"../aspnet-core/services/start-apigateway-host.bat",
		"../aspnet-core/services/start-apigateway-admin.bat",
		"../aspnet-core/services/start-messages.bat",
		"../aspnet-core/services/start-platform.bat"
	)

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 