. "../build/build-aspnetcore-common.ps1"

Write-host "开始部署容器."

$rootFolder = (Get-Item -Path "../" -Verbose).FullName
$deployPath = $rootFolder + "/deploy";
$buildPath = $rootFolder + "/build";
$aspnetcorePath = $rootFolder + "/aspnet-core";
$vuePath = $rootFolder + "/apps/vben5";

Write-host "root: " + $rootFolder

## 部署中间件
Write-host "deploy middleware..."
Set-Location $rootFolder
docker-compose -f .\docker-compose.middleware.yml up -d --build

## 等待30秒, 数据库初始化完成
Write-host "initial database..."
Start-Sleep -Seconds 30
## 创建数据库
# Write-host "create database..."
# Set-Location $aspnetcorePath
# cmd.exe /c create-database.bat

## 执行数据库迁移
Start-Sleep -Seconds 5
Write-host "migrate database..."
Set-Location $buildPath
foreach ($solution in $migrationArray) {  
    Set-Location $solution.Path
    dotnet run --no-build
}

## 删除bin/obj目录
Write-host "delete bin/obj path..."
Set-Location $aspnetcorePath
cmd.exe /c delete-bin-obj.bat

## 构建并运行应用程序
Write-host "build&running backend application..."
Set-Location $rootFolder
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml -f .\docker-compose.override.configuration.yml up -d --build

## 构建前端项目
Write-host "build front project..."
Set-Location $vuePath
Start-Process pnpm -ArgumentList "dev:app" -NoNewWindow

Set-Location $deployPath
Write-host "application is running..."
