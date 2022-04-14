FROM mcr.microsoft.com/dotnet/aspnet:6.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

## Beijing time, self replacement
env tz=asia/shanghai
run ln -snf /usr/share/zoneinfo/$tz /etc/localtime && echo '$tz' > /etc/timezone

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

ENTRYPOINT ["dotnet", "LY.MicroService.WebhooksManagement.HttpApi.Host.dll"]
