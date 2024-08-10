FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]


RUN apt update
RUN apt install wget -y

ENTRYPOINT ["dotnet", "LY.MicroService.WechatManagement.HttpApi.Host.dll"]
