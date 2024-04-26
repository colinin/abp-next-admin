FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

## 有些定时作业可能需要建立独立的数据库连接, 可能跨不同的数据库, 配置一下MSSQL
## 解决连接SqlServer TLS版本过高问题
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /usr/lib/ssl/openssl.cnf

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

ENTRYPOINT ["dotnet", "LY.MicroService.TaskManagement.HttpApi.Host.dll"]
