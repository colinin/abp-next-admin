FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS runtime
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app
COPY ../../../configuration/account/AuthServer.Host/appsettings.Development.json /app/appsettings.Development.json

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "AuthServer.Host.dll"]