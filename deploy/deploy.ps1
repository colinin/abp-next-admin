. "../build/build-aspnetcore-common.ps1"

Write-host "开始部署容器."

$rootFolder = (Get-Item -Path "../" -Verbose).FullName
$deployPath = $rootFolder + "/deploy";
$buildPath = $rootFolder + "/build";
$aspnetcorePath = $rootFolder + "/aspnet-core";
$vuePath = $rootFolder + "/apps/vue";

Write-host "root: " + $rootFolder

## 部署中间件
Write-host "deploy middleware..."
Set-Location $rootFolder
docker-compose -f .\docker-compose.middleware.yml up -d --build

## 等待30秒, 数据库初始化完成
Write-host "initial database..."
Start-Sleep -Seconds 30
## 创建数据库
Write-host "create database..."
Set-Location $aspnetcorePath
cmd.exe /c create-database.bat

## 执行数据库迁移
Start-Sleep -Seconds 5
Write-host "migrate database..."
Set-Location $buildPath
foreach ($solution in $migrationArray) {  
    Set-Location $solution.Path
    dotnet run --no-build
}

## 发布程序包
Write-host "release .net project..."
Set-Location $buildPath
foreach ($solution in $serviceArray) {  
    $publishPath = $rootFolder + "/aspnet-core/services/Publish/" + $solution.Service + "/"
    dotnet publish -c Release -o $publishPath $solution.Path --no-cache
    $dockerFile = Join-Path $solution.Path "Dockerfile"
    if ((Test-Path $dockerFile)) {
        Copy-Item $dockerFile -Destination $publishPath
    }
}

## 构建前端项目
Write-host "build front project..."
Set-Location $vuePath
pnpm install
pnpm build

## 运行应用程序
Write-host "running application..."
Set-Location $rootFolder
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml -f .\docker-compose.override.configuration.yml up -d --build

Set-Location $deployPath
Write-host "application is running..."
