FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
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

RUN apt update && apt install wget -y

USER $APP_UID

## .NET 8.0 默认使用8080端口,变更为80端口需要环境变量ASPNETCORE_HTTP_PORTS=80
## .NET 8.0 defaults to port 8080, changing to port 80 requires the environment variable ASPNETCORE.HTTP-PORTS=80
EXPOSE 8080/tcp

VOLUME [ "./app/Logs" ]
VOLUME [ "./app/Modules" ]

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

## 复制构建项目所需配置
COPY ["common.props", "./"]
COPY ["configureawait.props", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["NuGet.Config", "./"]

## 复制项目结构
COPY ["src/", "build/src/"]
COPY ["migrations/", "build/migrations/"]
COPY ["host/", "build/host/"]
RUN dotnet restore "build/host/PackageName.CompanyName.ProjectName.HttpApi.Host/PackageName.CompanyName.ProjectName.HttpApi.Host.csproj"

## 构建
COPY . .
WORKDIR "/build/host/PackageName.CompanyName.ProjectName.HttpApi.Host"
RUN dotnet build "./PackageName.CompanyName.ProjectName.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

## 发布
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PackageName.CompanyName.ProjectName.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## 运行
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PackageName.CompanyName.ProjectName.HttpApi.Host.dll"]