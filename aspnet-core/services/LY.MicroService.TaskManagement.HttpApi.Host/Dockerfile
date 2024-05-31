FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL maintainer="colin.in@foxmail.com"
WORKDIR /app

COPY . /app

## 有些定时作业可能需要建立独立的数据库连接, 可能跨不同的数据库, 配置一下MSSQL
## 解决连接SqlServer TLS版本过高问题
RUN sed -i 's/\[openssl_init\]/# \[openssl_init\]/g' /etc/ssl/openssl.cnf
RUN sed -i '$a\[openssl_init]' /etc/ssl/openssl.cnf
RUN sed -i '$a\providers = provider_sect' /etc/ssl/openssl.cnf
RUN sed -i '$a\ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
 
RUN sed -i '$a\[provider_sect]' /etc/ssl/openssl.cnf
RUN sed -i '$a\default = default_sect' /etc/ssl/openssl.cnf
RUN sed -i '$a\legacy = legacy_sect' /etc/ssl/openssl.cnf
 
RUN sed -i '$a\[default_sect]' /etc/ssl/openssl.cnf
RUN sed -i '$a\activate = 1' /etc/ssl/openssl.cnf
 
RUN sed -i '$a\[legacy_sect]' /etc/ssl/openssl.cnf
RUN sed -i '$a\activate = 1' /etc/ssl/openssl.cnf
 
RUN sed -i '$a\[ssl_sect]' /etc/ssl/openssl.cnf
RUN sed -i '$a\system_default = system_default_sect' /etc/ssl/openssl.cnf
 
RUN sed -i '$a\[system_default_sect]' /etc/ssl/openssl.cnf
RUN sed -i '$a\CipherString = DEFAULT:@SECLEVEL=0' /etc/ssl/openssl.cnf

EXPOSE 80/tcp
VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

RUN apt update
RUN apt install wget -y

ENTRYPOINT ["dotnet", "LY.MicroService.TaskManagement.HttpApi.Host.dll"]
