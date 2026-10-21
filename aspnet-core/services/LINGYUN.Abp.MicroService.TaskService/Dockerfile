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
COPY aspnet-core/services/LINGYUN.Abp.MicroService.TaskService/ ./aspnet-core/services/LINGYUN.Abp.MicroService.TaskService/

# framework
COPY aspnet-core/framework/ ./aspnet-core/framework/

# migrations
COPY aspnet-core/migrations/LINGYUN.Abp.MicroService.TaskService.EntityFrameworkCore/ ./aspnet-core/migrations/LINGYUN.Abp.MicroService.TaskService.EntityFrameworkCore/

# modules
COPY aspnet-core/modules/ ./aspnet-core/modules/

# dotnet restore
RUN dotnet restore aspnet-core/services/LINGYUN.Abp.MicroService.TaskService/LINGYUN.Abp.MicroService.TaskService.csproj

# dotnet publish
WORKDIR /src/aspnet-core/services/LINGYUN.Abp.MicroService.TaskService
RUN dotnet publish -c Release -o /app/publish --no-restore

# dotnet run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LINGYUN.Abp.MicroService.TaskService.dll"]