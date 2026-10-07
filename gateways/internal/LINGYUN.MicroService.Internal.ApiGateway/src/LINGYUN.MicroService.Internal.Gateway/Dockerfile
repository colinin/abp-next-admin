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
COPY gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/ ./gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/

# framework
COPY aspnet-core/framework/ ./aspnet-core/framework/

# modules
COPY aspnet-core/modules/ ./aspnet-core/modules/

# dotnet restore
RUN dotnet restore gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway/LINGYUN.MicroService.Internal.Gateway.csproj

# dotnet publish
WORKDIR /src/gateways/internal/LINGYUN.MicroService.Internal.ApiGateway/src/LINGYUN.MicroService.Internal.Gateway
RUN dotnet publish -c Release -o /app/publish --no-restore

# dotnet run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN rm -rf yarp.json
RUN mv yarp.docker.json yarp.json

ENTRYPOINT ["dotnet", "LINGYUN.MicroService.Internal.Gateway.dll"]