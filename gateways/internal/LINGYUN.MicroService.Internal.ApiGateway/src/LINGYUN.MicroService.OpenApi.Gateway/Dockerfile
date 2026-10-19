FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy props
COPY common.props ./
COPY common.secrets.props ./
COPY configureawait.props ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY NuGet.Config ./

# copy csproj
COPY gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.OpenApi.Gateway/ ./gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.OpenApi.Gateway/

# framework
COPY aspnet-core/framework/ ./aspnet-core/framework/

# modules
COPY aspnet-core/modules/ ./aspnet-core/modules/

# dotnet restore
RUN dotnet restore gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.OpenApi.Gateway/LINGYUN.MicroService.OpenApi.Gateway.csproj

# dotnet publish
WORKDIR /src/gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.OpenApi.Gateway
RUN dotnet publish -c Release -o /app/publish --no-restore

# dotnet run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LINGYUN.MicroService.OpenApi.Gateway.dll"]