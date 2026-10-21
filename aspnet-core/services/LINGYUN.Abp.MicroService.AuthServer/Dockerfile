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
COPY aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer/ ./aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer/

# framework
COPY aspnet-core/framework/ ./aspnet-core/framework/

# migrations
COPY aspnet-core/migrations/LINGYUN.Abp.MicroService.AuthServer.EntityFrameworkCore/ ./aspnet-core/migrations/LINGYUN.Abp.MicroService.AuthServer.EntityFrameworkCore/

# modules
COPY aspnet-core/modules/ ./aspnet-core/modules/

# dotnet restore
RUN dotnet restore aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer/LINGYUN.Abp.MicroService.AuthServer.csproj

# dotnet publish
WORKDIR /src/aspnet-core/services/LINGYUN.Abp.MicroService.AuthServer
RUN dotnet publish -c Release -o /app/publish --no-restore

# dotnet run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LINGYUN.Abp.MicroService.AuthServer.dll"]