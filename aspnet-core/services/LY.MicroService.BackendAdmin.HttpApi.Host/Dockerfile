FROM mcr.microsoft.com/dotnet/aspnet:6.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

ENTRYPOINT ["dotnet", "LY.MicroService.BackendAdmin.HttpApi.Host.dll"]
