readonly workdir=$(cd $(dirname $0); pwd)
echo "当前工作目录: $workdir"

echo "构建身份认证服务"
cd $workdir"/../aspnet-core/services/account/AuthServer.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/identityserver --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/identityserver/Dockerfile


echo "开始构建身份认证管理服务"
cd $workdir"/../aspnet-core/services/identity-server/LINGYUN.Abp.IdentityServer4.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/identityserver4-admin --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/identityserver4-admin/Dockerfile

echo "开始构建后台管理服务"

cd $workdir"/../aspnet-core/services/admin/LINGYUN.Abp.BackendAdmin.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/admin --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/admin/Dockerfile


echo "开始构建平台管理服务"

cd $workdir"/../aspnet-core/services/platform/LINGYUN.Platform.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/platform --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/platform/Dockerfile

echo "开始构建消息管理服务"
cd $workdir"/../aspnet-core/services/messages/LINGYUN.Abp.MessageService.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/messages --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/messages/Dockerfile

echo "开始构建本地化管理服务"
cd $workdir"/../aspnet-core/services/localization/LINGYUN.Abp.LocalizationManagement.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/localization --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/localization/Dockerfile

echo "开始构建网关管理服务"
cd $workdir"/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.HttpApi.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/apigateway-admin --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/apigateway-admin/Dockerfile

echo "开始构建网关服务"
cd $workdir"/../aspnet-core/services/apigateway/LINGYUN.ApiGateway.Host"

dotnet restore
dotnet publish -c Release -o ../../Publish/apigateway-host --no-cache --no-restore
cp -r -f Dockerfile ../../Publish/apigateway-host/Dockerfile
