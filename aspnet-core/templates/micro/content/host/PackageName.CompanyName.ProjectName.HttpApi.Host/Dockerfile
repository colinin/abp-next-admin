FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

## 设置服务器时区.
## 建议在外部(如docker-compose.yml)中定义
## Set server time zone.
## Suggest defining it externally (such as Docker Compose. yml)
#ENV TZ=Asia/Shanghai
#RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone


## 解决连接SqlServer TLS版本过高问题.
## 如果数据提供者是SqlServer, 且兼容性版本在120及以下,需要手动取消注释.
## The version of connection SqlServer TLS is too high.
## If the data provider is SqlServer and the compatibility version is 120 and below, manual annotation needs to be cancelled.

#RUN sed -i 's/\[openssl_init\]/# \[openssl_init\]/g' /etc/ssl/openssl.cnf
#RUN sed -i '$a\[openssl_init]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\providers = provider_sect' /etc/ssl/openssl.cnf
#RUN sed -i '$a\ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
 #
#RUN sed -i '$a\[provider_sect]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\default = default_sect' /etc/ssl/openssl.cnf
#RUN sed -i '$a\legacy = legacy_sect' /etc/ssl/openssl.cnf
 #
#RUN sed -i '$a\[default_sect]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\activate = 1' /etc/ssl/openssl.cnf
 #
#RUN sed -i '$a\[legacy_sect]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\activate = 1' /etc/ssl/openssl.cnf
 #
#RUN sed -i '$a\[ssl_sect]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\system_default = system_default_sect' /etc/ssl/openssl.cnf
 #
#RUN sed -i '$a\[system_default_sect]' /etc/ssl/openssl.cnf
#RUN sed -i '$a\CipherString = DEFAULT:@SECLEVEL=0' /etc/ssl/openssl.cnf

## 阿里源
#RUN sed -i "s@http://deb.debian.org@http://mirrors.aliyun.com@g" /etc/apt/sources.list.d/debian.sources
#RUN apt-get clean
#RUN apt-get update

## .NET 8.0 默认使用8080端口,变更为80端口需要环境变量ASPNETCORE_HTTP_PORTS=80
## .NET 8.0 defaults to port 8080, changing to port 80 requires the environment variable ASPNETCORE.HTTP-PORTS=80
# EXPOSE 8080/tcp
EXPOSE 80/tcp

VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

RUN apt update
RUN apt install wget -y

ENTRYPOINT ["dotnet", "PackageName.CompanyName.ProjectName.HttpApi.Host.dll"]
