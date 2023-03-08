FROM mcr.microsoft.com/dotnet/aspnet:7.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

## 设置上海时区.(Set your own time zone.)
#ENV TZ=Asia/Shanghai
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone

## 解决连接SqlServer TLS版本过高问题.(The version of connection SqlServer TLS is too high.)
## 如果数据提供者是SqlServer,需要手动取消注释.(If the data provider is SqlServer, you need to manually uncomment it.)
##RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf
##RUN sed -i 's/TLSv1.2/TLSv1.0/g' /usr/lib/ssl/openssl.cnf

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

ENTRYPOINT ["dotnet", "PackageName.CompanyName.ProjectName.HttpApi.Host.dll"]
