﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

ENTRYPOINT ["dotnet", "LINGYUN.MicroService.OpenApi.Gateway.dll"]
